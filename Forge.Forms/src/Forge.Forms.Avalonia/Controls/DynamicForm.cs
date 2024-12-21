using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Forge.Forms.AvaloniaUI.Controls.Internal;
using Forge.Forms.AvaloniaUI.DynamicExpressions;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.Controls;

[TemplatePart(Name = "PART_ItemsGrid", Type = typeof(Grid))]
public sealed partial class DynamicForm : ContentControl, IDynamicForm
{
    public static readonly AvaloniaProperty ModelProperty =
        AvaloniaProperty.Register<DynamicForm, object>(
            "Model");

    internal static readonly AvaloniaProperty ValueProperty =
        AvaloniaProperty.Register<DynamicForm, object>(
            "Value");

    //TODO: Maybe Inherits?
    public static readonly AvaloniaProperty ContextProperty =
        AvaloniaProperty.Register<DynamicForm, object>(
            "Context");

    public static readonly AvaloniaProperty FormBuilderProperty =
        AvaloniaProperty.Register<DynamicForm, IFormBuilder>(
            "FormBuilder",
            FormBuilding.FormBuilder.Default);

    public static readonly AvaloniaProperty ModelInterceptorProperty =
        AvaloniaProperty.Register<DynamicForm, IModelInterceptor>(
            "ModelInterceptor");

    public static readonly AvaloniaProperty EnvironmentProperty =
        AvaloniaProperty.Register<DynamicForm, IEnvironment>(
            "Environment");

    public static HashSet<DynamicForm> ActiveForms = new();

    public static List<IModelInterceptor> InterceptorChain = new();

    private readonly List<Control> currentElements;
    internal readonly Dictionary<string, IDataBindingProvider> DataBindingProviders;
    internal readonly Dictionary<string, DataFormField> DataFields;

    internal readonly FormResourceContext ResourceContext;
    private double[] columns;
    private IFormDefinition currentDefinition;

    private Grid itemsGrid;
    private int rows;

    static DynamicForm()
    {
        EnvironmentProperty.Changed.AddClassHandler<DynamicForm>(EnvironmentChanged);
        ModelProperty.Changed.AddClassHandler<DynamicForm>(ModelChanged);
    }

    public DynamicForm()
    {
        IsTabStop = false;
        ResourceContext = new FormResourceContext(this);
        columns = new double[0];
        currentElements = new List<Control>();
        DataFields = new Dictionary<string, DataFormField>();
        DataBindingProviders = new Dictionary<string, IDataBindingProvider>();
        Environment = new FormEnvironment();

        // TODO: Need to find a way for BindingOperations.SetBindign
        var binding = new Binding
        {
            Source = this,
            Path = "DataContext",
            Mode = BindingMode.OneWay
        };
        Bind(ContextProperty, binding);
        //BindingOperations.Apply(this, ContextProperty, InstancedBinding.OneWay(context));

        Loaded += (s, e) => { ActiveForms.Add(this); };
        Unloaded += (s, e) => { ActiveForms.Remove(this); };
    }

    protected override Type StyleKeyOverride => typeof(DynamicForm);

    /// <summary>
    ///     Gets or sets the environment flags for this control.
    /// </summary>
    public IEnvironment Environment
    {
        get => (IEnvironment)GetValue(EnvironmentProperty);
        set => SetValue(EnvironmentProperty, value);
    }

    /// <summary>
    ///     Gets or sets the form builder that is responsible for building forms.
    /// </summary>
    public IFormBuilder FormBuilder
    {
        get => (IFormBuilder)GetValue(FormBuilderProperty);
        set => SetValue(FormBuilderProperty, value);
    }

    /// <summary>
    ///     Allows intercepting the model to make changes.
    ///     This is an instance level interceptor.
    ///     See <see cref="InterceptorChain" /> for global hooks.
    /// </summary>
    public IModelInterceptor ModelInterceptor
    {
        get => (IModelInterceptor)GetValue(ModelInterceptorProperty);
        set => SetValue(ModelInterceptorProperty, value);
    }

    /// <summary>
    ///     Raised when an action is performed within the generated form.
    /// </summary>
    public event EventHandler<ActionEventArgs> OnAction;

    /// <summary>
    ///     Gets or sets the model associated with this form.
    ///     If the value is a IFormDefinition, a form will be built based on that definition.
    ///     If the value is a Type, a form will be built and bound to a new instance of that type.
    ///     If the value is a simple object, a single field bound to this property will be displayed.
    ///     If the value is a complex object, a form will be built and bound to properties of that instance.
    /// </summary>
    /// <remarks>
    ///     Setting the value to a form definition that may eventually
    ///     create a direct model binding might cause unexpected behavior.
    ///     Because there is no way to know if a boxed value type is nullable,
    ///     binding this value to a boxed nullable primitive will treat it as non-nullable.
    /// </remarks>
    public object Model
    {
        get => GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    /// <summary>
    ///     Gets the value of the current model instance.
    /// </summary>
    public object Value => GetValue(ValueProperty);

    /// <summary>
    ///     Gets or sets the context associated with this form.
    ///     Models can utilize this property to get data from
    ///     outside of their instance scope.
    /// </summary>
    public object Context
    {
        get => GetValue(ContextProperty);
        set => SetValue(ContextProperty, value);
    }

    private static void EnvironmentChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
    {
        ((DynamicForm)d).ResourceContext.RaiseEnvironmentChanged();
    }

    public void ReloadElements()
    {
        DetachBindings();
        FillGrid();
    }

    public Dictionary<string, DataFormField> GetDataFields()
    {
        return new Dictionary<string, DataFormField>(DataFields);
    }

    private static void ModelChanged(object obj, AvaloniaPropertyChangedEventArgs e)
    {
        var form = (DynamicForm)obj;
        IModelContext context = new ModelContext(e.NewValue, form.ResourceContext);
        foreach (var globalInterceptor in InterceptorChain)
        {
            context = globalInterceptor.Intercept(context);
            if (context == null)
                throw new InvalidOperationException("ModelInterceptors are not allowed to return null.");
        }

        var localInterceptor = form.ModelInterceptor;
        if (localInterceptor != null)
        {
            context = localInterceptor.Intercept(context);
            if (context == null)
                throw new InvalidOperationException("ModelInterceptors are not allowed to return null.");
        }

        form.UpdateModel(e.OldValue, context.NewModel);
    }

    private void UpdateModel(object oldModel, object newModel)
    {
        if (Equals(oldModel, newModel) || Equals(Value, newModel)) return;

        var oldValue = Value;
        object newValue;
        if (newModel == null)
        {
            // null -> Clear Form
            ClearForm();
            SetValue(ValueProperty, null);
            currentDefinition = null;
            newValue = null;
        }
        else if (newModel is IFormDefinition formDefinition)
        {
            // IFormDefinition -> Build form
            var instance = formDefinition.CreateInstance(ResourceContext);
            SetValue(ValueProperty, instance);
            RebuildForm(formDefinition);
            currentDefinition = formDefinition;
            newValue = instance;
        }
        else if (oldModel != null && oldModel.GetType() == newModel.GetType())
        {
            // Same type -> update values only.
            SetValue(ValueProperty, newModel);
            newValue = newModel;
        }
        else if (newModel is Type type)
        {
            // Type -> Build form, Value = new Type
            formDefinition = FormBuilder.GetDefinition(type);
            if (formDefinition == null)
            {
                ClearForm();
                SetValue(ValueProperty, null);
                newValue = null;
            }
            else
            {
                var instance = formDefinition.CreateInstance(ResourceContext);
                SetValue(ValueProperty, instance);
                RebuildForm(formDefinition);
                newValue = instance;
            }

            currentDefinition = formDefinition;
        }
        else
        {
            // object -> Build form, Value = model
            formDefinition = FormBuilder.GetDefinition(newModel.GetType());
            if (formDefinition == null)
            {
                ClearForm();
                SetValue(ValueProperty, null);
                newValue = null;
            }
            else
            {
                SetValue(ValueProperty, newModel);
                RebuildForm(formDefinition);
                newValue = newModel;
            }

            currentDefinition = formDefinition;
        }

        if (!ReferenceEquals(newValue, oldValue))
        {
            if (oldValue is INotifyPropertyChanged oldNpc) oldNpc.PropertyChanged -= HandlePropertyChanged;

            if (newValue is INotifyPropertyChanged newNpc) newNpc.PropertyChanged += HandlePropertyChanged;
        }

        HandleModelChanged();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        DetachBindings();
        itemsGrid?.Children.Clear();
        itemsGrid = this.GetVisualChildren().OfType<Grid>().FirstOrDefault(x => x.Name == "PART_ItemsGrid");
        FillGrid();
    }

    private void RebuildForm(IFormDefinition formDefinition)
    {
        ClearForm();
        if (formDefinition == null) return;

        rows = formDefinition.FormRows.Count;
        columns = formDefinition.Grid;
        currentElements.Clear();
        DataBindingProviders.Clear();
        DataFields.Clear();

        Control ElementBuilder(FormElement e)
        {
            return CreateContentPresenter(e, formDefinition);
        }

        var rowPointer = 0;
        for (var i = 0; i < rows; i++)
        {
            var row = formDefinition.FormRows[i];
            foreach (var container in row.Elements)
            {
                // New API to allow custom layout logic
                if (container.Layout != null)
                {
                    var root = container.Layout.Build(ElementBuilder);
                    Grid.SetRow(root, rowPointer);
                    Grid.SetRowSpan(root, row.RowSpan);
                    Grid.SetColumn(root, container.Column);
                    Grid.SetColumnSpan(root, container.ColumnSpan);
                    currentElements.Add(root);
                    continue;
                }

                var elements = container.Elements;
                if (elements.Count == 0) continue;

                if (elements.Count > 1)
                {
                    var panel = new ActionPanel();
                    Grid.SetRow(panel, rowPointer);
                    Grid.SetRowSpan(panel, row.RowSpan);
                    Grid.SetColumn(panel, container.Column);
                    Grid.SetColumnSpan(panel, container.ColumnSpan);
                    currentElements.Add(panel);
                    foreach (var element in container.Elements)
                    {
                        var contentPresenter = CreateContentPresenter(element, formDefinition);
                        ActionPanel.SetPosition(contentPresenter, element.LinePosition);
                        panel.Children.Add(contentPresenter);
                    }
                }
                else
                {
                    var contentPresenter = CreateContentPresenter(elements[0], formDefinition);
                    Grid.SetRow(contentPresenter, rowPointer);
                    Grid.SetRowSpan(contentPresenter, row.RowSpan);
                    Grid.SetColumn(contentPresenter, container.Column);
                    Grid.SetColumnSpan(contentPresenter, container.ColumnSpan);
                    currentElements.Add(contentPresenter);
                }
            }

            if (row.StartsNewRow) rowPointer++;
        }

        FillGrid();
    }

    private Control CreateContentPresenter(FormElement element, IFormDefinition formDefinition)
    {
        var provider = element.CreateBindingProvider(ResourceContext, formDefinition.Resources);
        if (element is DataFormField field && field.Key != null && !field.IsDirectBinding
            && provider is IDataBindingProvider dataBindingProvider)
        {
            DataBindingProviders[field.Key] = dataBindingProvider;
            DataFields[field.Key] = field;
        }

        var contentPresenter = provider as ContentPresenter ?? new ContentPresenter
        {
            Content = provider,
            VerticalAlignment = VerticalAlignment.Center
        };

        var visibility = provider.ProvideValue("IsVisible");
        switch (visibility)
        {
            case bool b:
                contentPresenter.IsVisible = b;
                break;

            case BindingBase bindingBase:
                break;
        }

        return contentPresenter;
    }

    private void FillGrid()
    {
        if (itemsGrid == null) return;

        itemsGrid.Children.Clear();
        itemsGrid.RowDefinitions.Clear();
        itemsGrid.ColumnDefinitions.Clear();
        for (var i = 0; i < rows; i++) itemsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        foreach (var column in columns)
            itemsGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = column > 0d
                    ? new GridLength(column, GridUnitType.Star)
                    : new GridLength(-column, GridUnitType.Pixel)
            });

        foreach (var content in currentElements) itemsGrid.Children.Add(content);
    }

    private void ClearForm()
    {
        DetachBindings();
        itemsGrid?.Children.Clear();
        var resources = Resources;
        var keys = resources.Keys;
        foreach (var key in keys)
            if (key is DynamicResourceKey || key is BindingProxyKey)
            {
                var proxy = (BindingProxy)resources[key];
                proxy.Value = null;
                resources.Remove(key);
            }
    }

    private void DetachBindings()
    {
        foreach (var provider in DataBindingProviders.Values) provider.ClearBindings();
    }

    internal void RaiseOnAction(IActionContext actionContext)
    {
        OnAction?.Invoke(this, new ActionEventArgs(actionContext));
    }
}
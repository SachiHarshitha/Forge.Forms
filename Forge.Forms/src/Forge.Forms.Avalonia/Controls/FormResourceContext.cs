using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.DynamicExpressions;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.Controls;

internal class FormResourceContext : IFrameworkResourceContext, INotifyPropertyChanged
{
    public FormResourceContext(DynamicForm form)
        : this(form, null)
    {
    }

    public FormResourceContext(DynamicForm form, string basePath)
    {
        Form = form;
        BasePath = basePath;
    }

    public DynamicForm Form { get; }

    public string BasePath { get; }

    public IEnvironment Environment => Form.Environment;

    public object GetModelInstance()
    {
        return Form.Value;
    }

    public BindingExpressionBase[] GetBindings()
    {
        return ModelState.GetBindings(Form.Value);
    }

    public object GetContextInstance()
    {
        return Form.Context;
    }

    public Binding CreateDirectModelBinding()
    {
        return new Binding(nameof(Form.Model) + BasePath)
        {
            Source = Form
        };
    }

    public Binding CreateModelBinding(string path)
    {
        return new Binding(nameof(Form.Value) + BasePath + Resource.FormatPath(path))
        {
            Source = Form
        };
    }

    public Binding CreateContextBinding(string path)
    {
        return new Binding(nameof(Form.Context) + BasePath + Resource.FormatPath(path))
        {
            Source = Form
        };
    }

    public object? TryFindResource(object key)
    {
        return Form.FindResource(key);
    }

    public object? FindResource(object key)
    {
        return Form.FindResource(key);
    }

    public void AddResource(object key, object value)
    {
        Form.Resources.Add(key, value);
    }

    public Control GetOwningElement()
    {
        return Form;
    }

    public void OnAction(IActionContext actionContext)
    {
        Form.RaiseOnAction(actionContext);
    }

    // Although never invoked, it may provide a performance benefit to include INPC.
    public event PropertyChangedEventHandler PropertyChanged;

    internal void RaiseEnvironmentChanged()
    {
        OnPropertyChanged(nameof(Environment));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
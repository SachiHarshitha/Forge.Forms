using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Forge.Forms.AvaloniaUI.DynamicExpressions;
using MultiBinding = Avalonia.Data.MultiBinding;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

/// <summary>
///     Markup extension for creating deferred bindings.
/// </summary>
public class FormBindingExtension : MarkupExtension
{
    private bool _dataContextSet; // Flag to track if DataContext has been set
    private IDisposable _dataContextSubscription;

    private bool _isUpdating;
    private string _targetPropertyName; // Store the target property name

    public FormBindingExtension()
    {
    }

    public FormBindingExtension(string name)
    {
        Name = name;
    }

    [ConstructorArgument("name")] public string Name { get; set; }

    public string Converter { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
        if (pvt?.TargetObject == null) return null; // Return null if no target object is found.

        if (pvt.TargetObject is StyledElement styledElement)
        {
            // Store the target property name the first time it's accessed
            if (_targetPropertyName == null)
                _targetPropertyName = pvt.TargetProperty?.ToString(); // Store the name of the target property

            // Ensure TargetProperty is initialized before proceeding
            if (styledElement.DataContext != null)
            {
                _dataContextSet = true;
                return ApplyBinding(styledElement); // Apply the binding immediately if DataContext is available
            }

            // DataContext is null, subscribe to changes.
            ObserveDataContextChange(styledElement);
            var targetType = pvt.TargetProperty?.GetType();
            if (targetType?.Name == nameof(AvaloniaProperty) ||
                (targetType?.IsSubclassOf(typeof(AvaloniaProperty)) ?? false))
            {
                var targetProperty = pvt.TargetProperty as AvaloniaProperty;
                if (targetProperty != null)
                {
                    if (targetProperty?.PropertyType.IsEnum ?? false)
                        return Enum.GetValues(targetProperty?.PropertyType).GetValue(0);
                    if (targetProperty?.PropertyType == typeof(bool))
                        return false;
                    // if (targetProperty?.PropertyType == typeof(string))
                    //     return string.Empty;
                    // if (targetProperty?.PropertyType is IBinding)
                    //return new FormBindingExtension(Name);
                }
                //return AvaloniaProperty.UnsetValue;
            }

            return null;
        }

        if (pvt.TargetObject is SetterBase setter)
        {
            if ((pvt.TargetProperty is AvaloniaProperty dp2 && dp2.PropertyType == typeof(BindingBase))
                || (pvt.TargetProperty is PropertyInfo p2 && p2.PropertyType == typeof(BindingBase)))
                return new Binding($"[{Name}].Value")
                {
                    Converter = GetConverter()
                };

            return new Binding($"[{Name}].Value")
            {
                Converter = GetConverter()
            };
        }

        return this; // Fallback behavior for non-StyledElement
    }

    private object ApplyBinding(StyledElement styledElement)
    {
        if (_isUpdating)
            return null;

        _isUpdating = true;

        if (styledElement.DataContext is IBindingProvider field)
        {
            var value = field.ProvideValue(Name);
            Console.WriteLine($"Provided value for {Name}: {value}");

            // If the value is a BindingBase (e.g., Binding), apply it
            if (value is BindingBase binding)
            {
                var avaProp = GetDependencyProperty(styledElement.GetType(), _targetPropertyName);
                if (avaProp?.PropertyType == typeof(BindingBase)) return value;
                var prop = GetPropInfoProperty(styledElement.GetType(), _targetPropertyName);
                if (prop?.PropertyType == typeof(BindingBase)) return value;

                if (avaProp != null)
                {
                    var expression = styledElement.Bind(avaProp, binding);
                    if (expression != null) field.BindingCreated(expression, Name);
                    return expression;
                }

                return value;
            }

            if (value is MultiBinding multiBinding) return multiBinding;

            // Apply the literal value directly
            ApplyLiteralValueViaReflection(styledElement, value);
            _isUpdating = false;
        }
        else
        {
            Console.WriteLine($"DataContext does not implement IBindingProvider for {Name}.");
            _isUpdating = false;
        }

        return this;
    }


    private void ApplyLiteralValueViaReflection(StyledElement styledElement, object value)
    {
        // Use reflection to set the literal value
        var targetPropertyInfo = styledElement.GetType().GetProperty(_targetPropertyName);
        if (targetPropertyInfo != null)
            try
            {
                if (targetPropertyInfo.PropertyType.FullName.Contains(nameof(IBinding)) && value is string expr)
                    targetPropertyInfo.SetValue(styledElement, new DataBinding(expr, new BindingOptions()));
                else
                    targetPropertyInfo.SetValue(styledElement, value);
                Console.WriteLine($"Literal value set via reflection to {_targetPropertyName}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        else
            Console.WriteLine($"Target property '{_targetPropertyName}' not found on target.");
    }

    private void ObserveDataContextChange(StyledElement styledElement)
    {
        // If we've already subscribed, return early to prevent multiple subscriptions
        if (_dataContextSubscription != null) return;

        _dataContextSubscription = styledElement.GetObservable(StyledElement.DataContextProperty)
            .Where(dataContext => dataContext != null)
            .Subscribe(dataContext =>
            {
                // Once DataContext is set, apply the binding
                if (!_isUpdating)
                {
                    //_isUpdating = true;
                    Console.WriteLine("DataContext set, applying binding...");
                    ApplyBinding(styledElement);
                    _isUpdating = false;
                }
            });
    }

    private IValueConverter GetConverter()
    {
        if (string.IsNullOrEmpty(Converter)) return null;

        Console.WriteLine($"Getting converter: {Converter}");
        return Resource.GetValueConverter(null, Converter);
    }

    public void Dispose()
    {
        _dataContextSubscription?.Dispose();
        _dataContextSubscription = null;
    }


    private static AvaloniaProperty? GetDependencyProperty(Type type, string propertyName)
    {
        AvaloniaProperty avaloniaProperty = null;
        var avaloniaProps = type.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(p => p.FieldType.IsSubclassOf(typeof(AvaloniaProperty)));
        var property = avaloniaProps.FirstOrDefault(p => p.Name.Equals($"{propertyName}Property"));
        if (property != null) avaloniaProperty = property.GetValue(null) as AvaloniaProperty;
        return avaloniaProperty;
    }

    private static PropertyInfo? GetPropInfoProperty(Type type, string propertyName)
    {
        var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
        return props.FirstOrDefault(p => p.Name.Equals($"{propertyName}"));
    }
}
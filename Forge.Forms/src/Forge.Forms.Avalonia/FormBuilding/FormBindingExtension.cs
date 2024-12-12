using System;
using System.Reflection;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

/// <summary>
///     Markup extension for creating deferred bindings.
/// </summary>
public class FormBindingExtension : MarkupExtension
{
    public FormBindingExtension()
    {
    }

    public FormBindingExtension(string name)
    {
        Name = name;
    }

    [ConstructorArgument("name")]
    public string Name { get; set; }

    public string Converter { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        var pvt = serviceProvider as IProvideValueTarget;
        if (pvt?.TargetObject == null)
        {
            return null;
        }

        if (pvt.TargetObject is Control frameworkElement)
        {
            if (!(frameworkElement.DataContext is IBindingProvider field))
            {
                return pvt.TargetProperty is AvaloniaProperty
                    ? AvaloniaProperty.UnsetValue
                    : null;
            }

            var value = field.ProvideValue(Name);
            if (value is BindingBase binding)
            {
                if (pvt.TargetProperty is AvaloniaProperty dp && dp.PropertyType == typeof(BindingBase)
                    || pvt.TargetProperty is PropertyInfo p && p.PropertyType == typeof(BindingBase))
                {
                    return binding;
                }

                var instancedBinding = binding.Initiate(frameworkElement, pvt.TargetProperty as AvaloniaProperty);
                // If Observable is null, then it probably was OneWayToSource binding,
                // and it's not possible to read.
                if (instancedBinding?.Observable is { } observable)
                {
                    object providedValue = null;
                    observable.Subscribe(s =>
                    {
                        if (providedValue is BindingExpressionBase expression)
                        {
                            field.BindingCreated(expression, Name);
                        }
                        providedValue = s;
                    });
                    return providedValue;
                }
                /*
                var providedValue = binding.ProvideValue(serviceProvider);
                if (providedValue is BindingExpressionBase expression)
                {
                    field.BindingCreated(expression, Name);
                }

                return providedValue;*/
            }

            return value;
        }

        //TODO: Check for TriggerBase
        if (pvt.TargetObject is SetterBase)
        {
            if (pvt.TargetProperty is AvaloniaProperty dp2 && dp2.PropertyType == typeof(BindingBase)
                || pvt.TargetProperty is PropertyInfo p2 && p2.PropertyType == typeof(BindingBase))
            {
                return new Binding($"[{Name}].Value")
                {
                    Converter = GetConverter()
                };
            }

            return new Binding($"[{Name}].Value")
            {
                Converter = GetConverter()
            };
        }

        return this;
    }

    private IValueConverter GetConverter()
    {
        if (Converter == null) return null;

        return Resource.GetValueConverter(null, Converter);
    }
}
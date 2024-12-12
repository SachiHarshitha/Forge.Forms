using System;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class LiteralValue : Resource
{
    public static readonly LiteralValue Null = new(null);

    public static readonly LiteralValue True = new(true);

    public static readonly LiteralValue False = new(false);

    public LiteralValue(object value)
        : this(value, null)
    {
    }

    public LiteralValue(object value, string valueConverter)
        : base(valueConverter)
    {
        if (value is BindingBase)
            throw new ArgumentException("Value cannot be an instance of BindingBase.", nameof(value));

        Value = value;
    }

    public object Value { get; }

    public override bool IsDynamic => false;

    public override IBinding ProvideBinding(IResourceContext context)
    {
        return new Binding
        {
            Source = Value,
            Converter = GetValueConverter(context),
            Mode = BindingMode.OneTime
        };
    }

    public override object ProvideValue(IResourceContext context)
    {
        return ValueConverter != null
            ? ProvideBinding(context)
            : Value;
    }

    public override bool Equals(Resource other)
    {
        if (other is LiteralValue resource)
            return Equals(Value, resource.Value)
                   && ValueConverter == other.ValueConverter;

        return false;
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }
}
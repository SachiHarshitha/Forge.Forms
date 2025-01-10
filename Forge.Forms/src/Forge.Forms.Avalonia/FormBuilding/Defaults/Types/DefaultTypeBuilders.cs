using System;
using System.Globalization;
using System.Linq;
using AvaloniaUI.Controls.Banking;
using Forge.Forms.AvaloniaUI.Annotations;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults.Types;

internal class StringFieldBuilder : TypeBuilder<string>
{
    protected override FormElement Build(IFormProperty property, Func<string, object> deserializer)
    {
        return new StringField(property.Name)
        {
            IsPassword = property.GetCustomAttribute<PasswordAttribute>() != null,
            IsMultiline = property.GetCustomAttribute<MultiLineAttribute>() != null
        };
    }
}

internal class CreditCardFieldBuilder : TypeBuilder<SecureCreditCard>
{
    protected override FormElement Build(IFormProperty property, Func<string, object> deserializer)
    {
        return new CreditCardField(property.Name);
    }
}

internal class BooleanFieldBuilder : IFieldBuilder
{
    public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
    {
        var isSwitch = property.GetCustomAttribute<ToggleAttribute>() != null;
        return new BooleanField(property.Name)
        {
            IsSwitch = isSwitch
        };
    }
}

internal class DateTimeFieldBuilder : IFieldBuilder
{
    public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
    {
        var timeAttribute = property.GetCustomAttribute<TimeAttribute>();
        if (timeAttribute != null)
            return new TimeField(property.Name)
            {
                Is24Hours = Utilities.GetResource<bool>(timeAttribute.Is24Hours, false, Deserializers.Boolean)
            };

        return new DateField(property.Name);
    }
}

internal class ConvertedFieldBuilder : IFieldBuilder
{
    public ConvertedFieldBuilder(Func<string, CultureInfo, object> deserializer)
    {
        Deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
    }

    public Func<string, CultureInfo, object> Deserializer { get; }

    public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
    {
        var replacements = property
            .GetCustomAttributes<ReplaceAttribute>()
            .OrderBy(attr => attr.Position)
            .Select(attr => attr.GetReplacement());
        return new ConvertedField(
            property.Name,
            property.PropertyType,
            new ReplacementPipe(Deserializer, replacements));
    }
}

internal class NumericFieldBuilder : IFieldBuilder
{
    public FormElement TryBuild(IFormProperty property, Func<string, object> deserializer)
    {
        // Check if the property is numeric
        var propertyType = property.PropertyType;
        if (!IsNumericType(propertyType)) return null;

        // Determine min, max, and increment based on the property type
        object minValue, maxValue, increment;
        GetNumericLimits(propertyType, out minValue, out maxValue, out increment);

        return new NumericField(property.Name, propertyType)
        {
            Minimum = new LiteralValue(minValue),
            Maximum = new LiteralValue(maxValue),
            Increment = new LiteralValue(increment)
        };
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(byte) || type == typeof(sbyte) ||
               type == typeof(short) || type == typeof(ushort) ||
               type == typeof(int) || type == typeof(uint) ||
               type == typeof(long) || type == typeof(ulong) ||
               type == typeof(float) || type == typeof(double) ||
               type == typeof(decimal);
    }

    private static void GetNumericLimits(Type type, out object minValue, out object maxValue, out object increment)
    {
        if (type == typeof(byte))
        {
            minValue = byte.MinValue;
            maxValue = byte.MaxValue;
            increment = 1;
        }
        else if (type == typeof(sbyte))
        {
            minValue = sbyte.MinValue;
            maxValue = sbyte.MaxValue;
            increment = 1;
        }
        else if (type == typeof(short))
        {
            minValue = short.MinValue;
            maxValue = short.MaxValue;
            increment = 1;
        }
        else if (type == typeof(ushort))
        {
            minValue = ushort.MinValue;
            maxValue = ushort.MaxValue;
            increment = 1;
        }
        else if (type == typeof(int))
        {
            minValue = int.MinValue;
            maxValue = int.MaxValue;
            increment = 1;
        }
        else if (type == typeof(uint))
        {
            minValue = uint.MinValue;
            maxValue = uint.MaxValue;
            increment = 1;
        }
        else if (type == typeof(long))
        {
            minValue = long.MinValue;
            maxValue = long.MaxValue;
            increment = 1;
        }
        else if (type == typeof(ulong))
        {
            minValue = ulong.MinValue;
            maxValue = ulong.MaxValue;
            increment = 1;
        }
        else if (type == typeof(float))
        {
            minValue = float.MinValue;
            maxValue = float.MaxValue;
            increment = 0.1f;
        }
        else if (type == typeof(double))
        {
            minValue = double.MinValue;
            maxValue = double.MaxValue;
            increment = 0.1d;
        }
        else if (type == typeof(decimal))
        {
            minValue = decimal.MinValue;
            maxValue = decimal.MaxValue;
            increment = 0.1m;
        }
        else
        {
            throw new ArgumentException($"Unsupported numeric type: {type}");
        }
    }
}
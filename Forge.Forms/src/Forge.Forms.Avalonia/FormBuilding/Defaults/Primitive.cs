using System;
using System.Globalization;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

internal static class Primitive
{
    private static FormDefinition BuildWith(DataFormField field)
    {
        var definition = new FormDefinition(field.PropertyType);
        definition.FormRows.Add(new FormRow
        {
            Elements = { new FormElementContainer(0, 1, field) }
        });

        field.Freeze();
        return definition;
    }

    private static FormDefinition BuildConverted(Type type, Func<string, CultureInfo, object> deserializer)
    {
        var field = new ConvertedField(null, type, new ReplacementPipe(deserializer))
        {
            IsDirectBinding = true
        };

        return BuildWith(field);
    }

    public static FormDefinition String()
    {
        var field = new StringField(null)
        {
            IsDirectBinding = true
        };

        return BuildWith(field);
    }

    public static FormDefinition DateTime()
    {
        var field = new DateField(null)
        {
            IsDirectBinding = true
        };

        return BuildWith(field);
    }

    public static FormDefinition Boolean()
    {
        var field = new BooleanField(null)
        {
            IsDirectBinding = true
        };

        return BuildWith(field);
    }

    public static FormDefinition Numeric(Type numericType, object minValue, object maxValue, object incrementValue)
    {
        if (!IsNumericType(numericType))
            throw new ArgumentException("The specified type is not numeric.", nameof(numericType));

        var field = new NumericField(null, numericType)
        {
            IsDirectBinding = true,
            Minimum = new LiteralValue(minValue),
            Maximum = new LiteralValue(maxValue),
            Increment = new LiteralValue(incrementValue)
        };

        return BuildWith(field);
    }

    public static FormDefinition Int16()
    {
        return Numeric(typeof(short), short.MinValue, short.MaxValue, 1);
    }

    public static FormDefinition Int32()
    {
        return Numeric(typeof(int), int.MinValue, int.MaxValue, 1);
    }

    public static FormDefinition Int64()
    {
        return Numeric(typeof(long), long.MinValue, long.MaxValue, 1);
    }

    public static FormDefinition UInt16()
    {
        return Numeric(typeof(ushort), ushort.MinValue, ushort.MaxValue, 1);
    }

    public static FormDefinition UInt32()
    {
        return Numeric(typeof(uint), uint.MinValue, uint.MaxValue, 1);
    }

    public static FormDefinition UInt64()
    {
        return Numeric(typeof(ulong), ulong.MinValue, ulong.MaxValue, 1);
    }

    public static FormDefinition Single()
    {
        return Numeric(typeof(float), float.MinValue, float.MaxValue, 0.1f);
    }

    public static FormDefinition Double()
    {
        return Numeric(typeof(double), double.MinValue, double.MaxValue, 0.1);
    }

    public static FormDefinition Decimal()
    {
        return Numeric(typeof(decimal), decimal.MinValue, decimal.MaxValue, 0.1m);
    }

    public static FormDefinition Byte()
    {
        return Numeric(typeof(byte), byte.MinValue, byte.MaxValue, 1);
    }

    public static FormDefinition SByte()
    {
        return Numeric(typeof(sbyte), sbyte.MinValue, sbyte.MaxValue, 1);
    }

    public static FormDefinition Char()
    {
        return BuildConverted(typeof(char), Deserializers.Char);
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
}
using System;
using System.Collections.Generic;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public abstract class Resource : IEquatable<Resource>, IValueProvider
{
    /// <summary>
    ///     Global cache for value converters accessible from expressions.
    /// </summary>
    public static readonly Dictionary<string, IValueConverter> ValueConverters =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["IsNull"] = new IsNullConverter(),
            ["IsNotNull"] = new IsNotNullConverter(),
            ["AsBool"] = new AsBoolConverter(),
            ["Negate"] = new NegateConverter(),
            ["IsEmpty"] = new IsEmptyConverter(),
            ["IsNotEmpty"] = new IsNotEmptyConverter(),
            ["ToUpper"] = new ToUpperConverter(),
            ["ToLower"] = new ToLowerConverter(),
            ["Length"] = new LengthValueConverter(),
            ["ToString"] = new ToStringConverter(),
        };

    /// <summary>
    ///     Global cache for value converter factories that can take a parameter.
    /// </summary>
    public static readonly Dictionary<string, Func<object, IValueConverter>> ValueConverterFactories =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["IsEqual"] = parameter => new IsEqualConverter(parameter)
        };

    public static readonly Dictionary<string, IMultiValueConverter> MultiValueConverters =
        new(StringComparer.OrdinalIgnoreCase);

    public static readonly Dictionary<string, Func<object, IMultiValueConverter>> MultiValueConverterFactories =
        new(StringComparer.OrdinalIgnoreCase);

    protected Resource(string valueConverter)
    {
        ValueConverter = valueConverter;
    }

    public string ValueConverter { get; }

    public abstract bool IsDynamic { get; }

    public abstract bool Equals(Resource other);

    public abstract IBinding ProvideBinding(IResourceContext context);

    public virtual object ProvideValue(IResourceContext context)
    {
        return ProvideBinding(context);
    }

    protected IValueConverter GetValueConverter(IResourceContext context)
    {
        return GetValueConverter(context, ValueConverter);
    }

    protected internal static IValueConverter GetValueConverter(IResourceContext context, string valueConverter)
    {
        if (string.IsNullOrEmpty(valueConverter)) return null;

        object parameter = null;
        var index = valueConverter.IndexOf(':');
        if (index > 0)
        {
            var parameterPart = valueConverter.Substring(index + 1);
            valueConverter = valueConverter.Substring(0, index);
            if (parameterPart[0] == '\'')
                parameter = parameterPart.Substring(1);
            else
                switch (parameterPart)
                {
                    case "true":
                        parameter = true;
                        break;
                    case "false":
                        parameter = false;
                        break;
                    case "null":
                        parameter = null;
                        break;
                    default:
                        if (InvariantInt.TryParse(parameterPart, out var ival))
                            parameter = ival;
                        else if (InvariantDouble.TryParse(parameterPart, out var dval))
                            parameter = dval;
                        else
                            throw new FormatException("Invalid converter parameter.");

                        break;
                }
        }

        if (ValueConverterFactories.TryGetValue(valueConverter, out var f)) return f(parameter);

        if (ValueConverters.TryGetValue(valueConverter, out var c)) return c;

        if (context != null && context.TryFindResource(valueConverter) is IValueConverter converter) return converter;

        throw new InvalidOperationException($"Value converter '{valueConverter}' not found.");
    }

    protected IMultiValueConverter GetMultiValueConverter(IResourceContext context)
    {
        return GetMultiValueConverter(context, ValueConverter);
    }

    protected internal static IMultiValueConverter GetMultiValueConverter(IResourceContext context,
        string valueConverter)
    {
        if (string.IsNullOrEmpty(valueConverter)) return null;

        object parameter = null;
        var index = valueConverter.IndexOf(':');
        if (index > 0)
        {
            var parameterPart = valueConverter.Substring(index + 1);
            valueConverter = valueConverter.Substring(0, index);
            if (parameterPart[0] == '\'')
                parameter = parameterPart.Substring(1);
            else
                switch (parameterPart)
                {
                    case "true":
                        parameter = true;
                        break;
                    case "false":
                        parameter = false;
                        break;
                    case "null":
                        parameter = null;
                        break;
                    default:
                        if (InvariantInt.TryParse(parameterPart, out var ival))
                            parameter = ival;
                        else if (InvariantDouble.TryParse(parameterPart, out var dval))
                            parameter = dval;
                        else
                            throw new FormatException("Invalid converter parameter.");

                        break;
                }
        }

        if (MultiValueConverterFactories.TryGetValue(valueConverter, out var f)) return f(parameter);

        if (MultiValueConverters.TryGetValue(valueConverter, out var m)) return m;

        if (context != null && context.TryFindResource(valueConverter) is IMultiValueConverter converter)
            return converter;

        throw new InvalidOperationException($"MultiBinding converter '{valueConverter}' not found.");
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;

        if (ReferenceEquals(this, obj)) return true;

        if (obj.GetType() != GetType()) return false;

        return Equals((Resource)obj);
    }

    public abstract override int GetHashCode();

    public static string FormatPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return "";

        if (path[0] == '[') return path;

        return "." + path;
    }
}
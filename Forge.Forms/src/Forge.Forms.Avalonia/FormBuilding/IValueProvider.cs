﻿using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;
using Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public interface IValueProvider
{
    IBinding ProvideBinding(IResourceContext context);

    object ProvideValue(IResourceContext context);
}

public static class ValueProviderExtensions
{
    public static IProxy GetBestMatchingProxy(this IValueProvider valueProvider, IResourceContext context)
    {
        switch (valueProvider)
        {
            case LiteralValue v:
                return new PlainObject(v.Value);
            case BoundExpression b:
                return b.GetProxy(context);
            default:
                return null;
        }
    }
    
    public static IValueProvider Wrap(this IValueProvider valueProvider, string valueConverter)
    {
        return valueConverter == null
            ? valueProvider
            : new ValueProviderWrapper(valueProvider, valueConverter);
    }

    public static IValueProvider Wrap(this IValueProvider valueProvider, IValueConverter valueConverter)
    {
        return valueConverter == null
            ? valueProvider
            : new ValueProviderWrapperWithConverterInstance(valueProvider, valueConverter);
    }

    private class ValueProviderWrapperWithConverterInstance : IValueProvider
    {
        private readonly IValueProvider innerProvider;
        private readonly IValueConverter valueConverter;

        public ValueProviderWrapperWithConverterInstance(IValueProvider innerProvider, IValueConverter valueConverter)
        {
            this.innerProvider = innerProvider;
            this.valueConverter = valueConverter;
        }

        public IBinding ProvideBinding(IResourceContext context)
        {
            var bindingBase = innerProvider.ProvideBinding(context);
            if (bindingBase is Binding binding)
                binding.Converter = binding.Converter == null
                    ? valueConverter
                    : new ConverterWrapper(valueConverter, binding.Converter);

            return bindingBase;
        }

        public object ProvideValue(IResourceContext context)
        {
            var value = innerProvider.ProvideValue(context);
            if (value is Binding binding)
            {
                binding.Converter = binding.Converter == null
                    ? valueConverter
                    : new ConverterWrapper(valueConverter, binding.Converter);
                return binding;
            }

            if (value is BindingBase) return value;

            return valueConverter.Convert(value, typeof(object), null, CultureInfo.CurrentCulture);
        }
    }

    private class ValueProviderWrapper : IValueProvider
    {
        private readonly IValueProvider innerProvider;
        private readonly string valueConverter;

        public ValueProviderWrapper(IValueProvider innerProvider, string valueConverter)
        {
            this.innerProvider = innerProvider;
            this.valueConverter = valueConverter;
        }

        public IBinding ProvideBinding(IResourceContext context)
        {
            var bindingBase = innerProvider.ProvideBinding(context);
            if (bindingBase is Binding binding)
            {
                var converter = Resource.GetValueConverter(context, valueConverter);
                binding.Converter = binding.Converter == null
                    ? converter
                    : new ConverterWrapper(converter, binding.Converter);
            }

            return bindingBase;
        }

        public object ProvideValue(IResourceContext context)
        {
            var value = innerProvider.ProvideValue(context);
            var converter = Resource.GetValueConverter(context, valueConverter);
            if (value is Binding binding)
            {
                binding.Converter = binding.Converter == null
                    ? converter
                    : new ConverterWrapper(converter, binding.Converter);
                return binding;
            }

            if (value is BindingBase) return value;

            return converter.Convert(value, typeof(object), null, CultureInfo.CurrentCulture);
        }
    }
}
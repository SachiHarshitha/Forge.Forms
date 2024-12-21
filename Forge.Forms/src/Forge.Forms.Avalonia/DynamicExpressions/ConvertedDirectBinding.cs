using System;
using System.Collections.Generic;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;
using Forge.Forms.AvaloniaUI.Validation;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class ConvertedDirectBinding : IValueProvider
{
    public ConvertedDirectBinding(BindingOptions bindingOptions,
        List<IValidatorProvider> validationRules, ReplacementPipe replacementPipe,
        Func<IResourceContext, IErrorStringProvider> conversionErrorStringProvider)
    {
        BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
        ReplacementPipe = replacementPipe ?? throw new ArgumentNullException(nameof(replacementPipe));
        ValidationRules = validationRules ?? new List<IValidatorProvider>();
        ConversionErrorStringProvider = conversionErrorStringProvider;
    }

    public BindingOptions BindingOptions { get; }

    public List<IValidatorProvider> ValidationRules { get; }

    public ReplacementPipe ReplacementPipe { get; }

    public Func<IResourceContext, IErrorStringProvider> ConversionErrorStringProvider { get; }

    public IBinding ProvideBinding(IResourceContext context)
    {
        var binding = context.CreateDirectModelBinding();
        BindingOptions.Apply(binding);
        var deserializer = ReplacementPipe.CreateDeserializer(context);
        binding.Converter = new StringTypeConverter(deserializer);
        // binding.ValidationRules.Add(new ConversionValidator(deserializer, ConversionErrorStringProvider(context),
        //     binding.ConverterCulture));
        // var pipe = new ValidationPipe();
        // foreach (var validatorProvider in ValidationRules)
        //     binding.ValidationRules.Add(validatorProvider.GetValidator(context, pipe));
        //
        // binding.ValidationRules.Add(pipe);
        return binding;
    }

    public object ProvideValue(IResourceContext context)
    {
        return ProvideBinding(context);
    }
}
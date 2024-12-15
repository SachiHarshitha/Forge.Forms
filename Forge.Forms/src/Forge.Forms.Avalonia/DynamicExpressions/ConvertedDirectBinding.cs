using System;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class ConvertedDirectBinding : IValueProvider
{
    public ConvertedDirectBinding(BindingOptions bindingOptions,
        ReplacementPipe replacementPipe)
    {
        BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
        ReplacementPipe = replacementPipe ?? throw new ArgumentNullException(nameof(replacementPipe));
    }

    public BindingOptions BindingOptions { get; }

    public ReplacementPipe ReplacementPipe { get; }

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
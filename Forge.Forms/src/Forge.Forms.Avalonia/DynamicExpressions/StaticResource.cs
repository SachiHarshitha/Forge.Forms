using System;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class StaticResource : Resource
{
    public StaticResource(string resourceKey)
        : this(resourceKey, null)
    {
    }

    public StaticResource(string resourceKey, string valueConverter)
        : base(valueConverter)
    {
        ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
    }

    public string ResourceKey { get; }

    public override bool IsDynamic => false;

    public override IBinding ProvideBinding(IResourceContext context)
    {
        return new Binding
        {
            Source = context.FindResource(ResourceKey),
            Converter = GetValueConverter(context),
            Mode = BindingMode.OneTime
        };
    }

    public override bool Equals(Resource other)
    {
        if (other is StaticResource resource)
            return ResourceKey == resource.ResourceKey
                   && ValueConverter == resource.ValueConverter;

        return false;
    }

    public override int GetHashCode()
    {
        return ResourceKey.GetHashCode();
    }
}
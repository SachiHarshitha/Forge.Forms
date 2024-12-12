using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public class EnvironmentBinding : Resource
{
    public EnvironmentBinding(string key, bool oneTime, string valueConverter)
        : base(valueConverter)
    {
        Key = key;
        IsDynamic = !oneTime;
    }

    public string Key { get; }

    public override bool IsDynamic { get; }

    public override bool Equals(Resource other)
    {
        return other is EnvironmentBinding b && Key == b.Key;
    }

    public override IBinding ProvideBinding(IResourceContext context)
    {
        return new Binding($"Environment.Item[{Key}]")
        {
            Source = context,
            Converter = GetValueConverter(context),
            Mode = IsDynamic ? BindingMode.OneWay : BindingMode.OneTime,
            FallbackValue = false
        };
    }

    public override int GetHashCode()
    {
        return (Key + ValueConverter).GetHashCode() + (IsDynamic ? 1 : 0);
    }
}
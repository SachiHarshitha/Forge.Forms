using System;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class FileBinding : Resource
{
    public FileBinding(string filePath, bool watch, string valueConverter)
        : base(valueConverter)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        IsDynamic = watch;
    }

    public string FilePath { get; }

    public override bool IsDynamic { get; }

    public override IBinding ProvideBinding(IResourceContext context)
    {
        return new Binding(nameof(IProxy.Value))
        {
            Source = IsDynamic
                ? new FileWatcher(FilePath)
                : new PlainString(Utilities.TryReadFile(FilePath)),
            Converter = GetValueConverter(context),
            Mode = BindingMode.OneWay
        };
    }

    public override bool Equals(Resource other)
    {
        if (other is FileBinding resource)
            return FilePath == resource.FilePath
                   && ValueConverter == resource.ValueConverter;

        return false;
    }

    public override int GetHashCode()
    {
        return FilePath.GetHashCode();
    }
}
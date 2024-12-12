using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public class ReplacementPipe
{
    private readonly Func<string, CultureInfo, object> deserializer;
    private readonly List<RegexReplacement> replacements;

    public ReplacementPipe(Func<string, CultureInfo, object> deserializer)
        : this(deserializer, null)
    {
    }

    public ReplacementPipe(Func<string, CultureInfo, object> deserializer,
        IEnumerable<RegexReplacement> replacements)
    {
        this.deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        this.replacements = replacements?.ToList();
    }

    public Func<string, CultureInfo, object> CreateDeserializer(IResourceContext context)
    {
        if (replacements == null || replacements.Count == 0) return deserializer;

        return (value, culture) =>
        {
            //foreach (var replacement in compiledReplacements) value = replacement.Replace(value);

            return deserializer(value, culture);
        };
    }
}

public class RegexReplacement
{
    public RegexReplacement(IValueProvider pattern, IValueProvider replacement, IValueProvider regexOptions)
    {
        Pattern = pattern;
        Replacement = replacement;
        RegexOptions = regexOptions;
    }

    public IValueProvider Pattern { get; }

    public IValueProvider Replacement { get; }

    public IValueProvider RegexOptions { get; }
}
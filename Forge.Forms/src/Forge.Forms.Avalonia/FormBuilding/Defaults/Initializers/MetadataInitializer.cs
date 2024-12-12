﻿using System;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults.Initializers;

internal class MetadataInitializer : IFieldInitializer
{
    public void Initialize(FormElement element, IFormProperty property, Func<string, object> deserializer)
    {
        var attrs = property.GetCustomAttributes<MetaAttribute>();
        if (attrs == null) return;

        foreach (var attr in attrs)
            if (!string.IsNullOrEmpty(attr.Name))
                element.Metadata[attr.Name] = attr.Value;
    }
}
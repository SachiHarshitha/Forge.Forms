﻿using System;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

/// <summary>
///     Initializes built form fields.
/// </summary>
public interface IFieldInitializer
{
    void Initialize(FormElement element, IFormProperty property, Func<string, object> deserializer);
}
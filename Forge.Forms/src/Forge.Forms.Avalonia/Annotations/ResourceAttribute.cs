﻿using System;

namespace Forge.Forms.AvaloniaUI.Annotations;

/// <summary>
///     Allows attaching custom resources to fields or to the model.
///     These resources become available to generated controls.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class ResourceAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResourceAttribute" /> class.
    /// </summary>
    /// <param name="name">Resource key.</param>
    /// <param name="value">Resource value. Accepts an object or a dynamic expression.</param>
    public ResourceAttribute(string name, object value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    ///     Resource name. Accepts a string.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Resource value. Accepts an object or a dynamic expresion.
    /// </summary>
    public object Value { get; }
}
using System;

namespace Forge.Forms.AvaloniaUI.Annotations;

/// <summary>
///     Indicates that a string property should be rendered as a multi-line textbox.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MultiLineAttribute : Attribute
{
}
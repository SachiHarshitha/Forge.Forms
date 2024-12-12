using System;

namespace Forge.Forms.AvaloniaUI.Annotations;

/// <summary>
///     Properties marked with this attribute will never be generated.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class FieldIgnoreAttribute : Attribute
{
}
using System;

namespace Forge.Forms.AvaloniaUI.Annotations;

/// <summary>
///     Specifies that content rendering should be handled by WPF.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DirectContentAttribute : Attribute
{
}
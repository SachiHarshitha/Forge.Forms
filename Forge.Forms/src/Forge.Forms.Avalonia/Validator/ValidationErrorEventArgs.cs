using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.Validator;

public class ValidationErrorEventArgs : EventArgs
{
    public string PropertyName { get; }
    public List<string> Errors { get; }

    public ValidationErrorEventArgs(string propertyName, List<string> errors)
    {
        PropertyName = propertyName;
        Errors = errors;
    }
}
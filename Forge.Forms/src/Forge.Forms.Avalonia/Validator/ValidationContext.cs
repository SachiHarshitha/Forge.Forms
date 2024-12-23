namespace Forge.Forms.AvaloniaUI.Validator;

public class ValidationContext
{
    public object Value { get; }
    public string PropertyName { get; }

    public ValidationContext(object value, string propertyName)
    {
        Value = value;
        PropertyName = propertyName;
    }
}
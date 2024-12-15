using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Forge.Forms.AvaloniaUI;
using Forge.Forms.AvaloniaUI.Annotations;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Forms.Avalonia.Demo.Models;

public class CustomValidation : IActionHandler, INotifyPropertyChanged
{
    private string externalValidation;

    private string ignoreEmptyOrNulls;
    private bool invalidateTextbox;
    private string throughModelState;
    private string throughStaticMethod;

    [Value("Length", Must.BeGreaterThanOrEqualTo, 6, IgnoreNullOrEmpty = true,
        Message = "Type at least 6 characters or leave this field empty.")]
    [Field(Icon = PackIconKind.Text)]
    public string IgnoreEmptyOrNulls
    {
        get => ignoreEmptyOrNulls;
        set
        {
            ignoreEmptyOrNulls = value;
            OnPropertyChanged();
        }
    }

    [Text("Click the button to invalidate the text field")]
    [Action("invalidate", "INVALIDATE")]
    [Field(Icon = PackIconKind.Text)]
    public string ThroughModelState
    {
        get => throughModelState;
        set
        {
            throughModelState = value;
            OnPropertyChanged();
        }
    }

    [Text("Trigger validation from the popup menu or from the button below.")]
    [Action("validate", "VALIDATE")]
    [Value(Must.SatisfyMethod, nameof(Validate))]
    [Field(Icon = PackIconKind.Text)]
    public string ThroughStaticMethod
    {
        get => throughStaticMethod;
        set
        {
            throughStaticMethod = value;
            OnPropertyChanged();
        }
    }

    [Text("Invalidate this property by marking the checkbox below.")]
    [Value(Must.BeInvalid, When = "{Binding InvalidateTextbox}")]
    [Field(Icon = PackIconKind.Text)]
    public string ExternalValidation
    {
        get => externalValidation;
        set
        {
            externalValidation = value;
            OnPropertyChanged();
        }
    }

    public bool InvalidateTextbox
    {
        get => invalidateTextbox;
        set
        {
            invalidateTextbox = value;
            OnPropertyChanged();
        }
    }

    public void HandleAction(IActionContext actionContext)
    {
        switch (actionContext.Action)
        {
            case "invalidate":
                //TODO: ModelState.Invalidate Missing
                //ModelState.Invalidate(this, nameof(ThroughModelState), "Invalid value.");
                break;
            case "validate":
                ModelState.Validate(this, nameof(ThroughStaticMethod));
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public static bool Validate(ValidationContext validationContext)
    {
        return false;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
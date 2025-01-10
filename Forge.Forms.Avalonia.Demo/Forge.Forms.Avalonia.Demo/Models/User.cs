using System;
using AvaloniaUI.Controls.Banking;
using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.AvaloniaUI.Annotations;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Forms.Avalonia.Demo.Models;

[Title("Create account")]
[Action("cancel", "CANCEL", IsReset = true, IsCancel = true)]
[Action("register", "REGISTER", Validates = true, IsDefault = true)]
public class User : ObservableObject
{
    private SecureCreditCard _creditCard = new();
    private int age;
    private bool agreeToLicense;
    private string confirmPassword;
    private DateTimeOffset? dateOfBirth;
    private string firstName;
    private string lastName;
    private string password;
    private string username;

    [Heading("Personal details")]
    [property: Field(Name = "First Name",
        ToolTip = "Enter your first name here.",
        Icon = PackIconKind.Pencil)]
    [Value(Must.NotBeEmpty)]
    public string FirstName
    {
        get => firstName;
        set => SetProperty(ref firstName, value);
    }

    [Field(Name = "Last Name",
        ToolTip = "Enter your last name here.",
        Icon = "Empty")]
    [Value(Must.NotBeEmpty)]
    public string LastName
    {
        get => lastName;
        set => SetProperty(ref lastName, value);
    }

    [Field(Icon = PackIconKind.BirthdayCake)]
    [Value(Must.BeLessThan, 99,
        Message = "Things get complicated when you are above 99")]
    [Value(Must.BeGreaterThan, 16,
        Message = "You are underage and not allowed to participate.")]
    public int Age
    {
        get => age;
        set => SetProperty(ref age, value);
    }

    [Field(Icon = PackIconKind.Calendar)]
    [Value(Must.BeLessThan, "2020-01-01",
        Message = "You said you are born in the year {Value:yyyy}. Are you really from the future?")]
    public DateTimeOffset? DateOfBirth
    {
        get => dateOfBirth;
        set => SetProperty(ref dateOfBirth, value);
    }

    [Heading("Account details")]
    [Field(Name = "Username",
        Icon = PackIconKind.Account)]
    [Value(Must.MatchPattern, "^[a-zA-Z][a-zA-Z0-9]*$",
        Message = "{Value} is not a valid username, usernames must match pattern {Argument}.")]
    [Value(Must.NotExistIn, "{ContextBinding Users}",
        Message = "User {Value} is already taken.")]
    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    [Field(Icon = PackIconKind.Key)]
    [Value("Length", Must.BeGreaterThanOrEqualTo, 6,
        Message = "Your password has {Value|Length} characters, which is less than the required {Argument}.")]
    [Value("Length", Must.BeGreaterThan, 12,
        When = "{ContextBinding RequireLongPasswords}",
        Message = "The administrator decided that your password must be really long!")]
    [Password]
    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }

    [Field(Icon = "Empty")]
    [Value(Must.BeEqualTo, "{Binding Password}",
        Message = "The entered passwords do not match.",
        ArgumentUpdatedAction = ValidationAction.ClearErrors)]
    [Value(Must.NotBeEmpty)]
    [Password]
    public string ConfirmPassword
    {
        get => confirmPassword;
        set => SetProperty(ref confirmPassword, value);
    }

    [Break]
    [Heading("Credit card details")]
    [Field(Icon = PackIconKind.CreditCard)]
    public SecureCreditCard CardDetails
    {
        get => _creditCard;
        set => SetProperty(ref _creditCard, value);
    }

    [Break]
    [Break]
    [Heading("Review entered information")]
    [Text("Name: {Binding FirstName} {Binding LastName}")]
    [Text("Date of birth: {Binding DateOfBirth:yyyy-MM-dd}")]
    [Text("Username: {Binding Username}")]
    [Break]
    [Heading("License agreement")]
    [Text("By signing up, you agree to our terms of use, privacy policy, and cookie policy.")]
    [Value(Must.BeTrue, Message = "You must accept the license agreement.")]
    public bool AgreeToLicense
    {
        get => agreeToLicense;
        set => SetProperty(ref agreeToLicense, value);
    }
}
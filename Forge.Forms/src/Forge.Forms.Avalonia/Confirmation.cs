﻿using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.AvaloniaUI;

[Form(Mode = DefaultFields.None)]
[Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
[Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
[Action("negative", "{Binding NegativeAction}",
    IsCancel = true,
    ClosesDialog = true,
    IsVisible = "{Binding NegativeAction|IsNotEmpty}",
    Icon = "{Binding NegativeActionIcon}")]
[Action("positive", "{Binding PositiveAction}",
    IsDefault = true,
    ClosesDialog = true,
    IsVisible = "{Binding PositiveAction|IsNotEmpty}",
    Icon = "{Binding PositiveActionIcon}")]
public sealed class Confirmation : DialogBase
{
    public Confirmation()
    {
    }

    public Confirmation(string message)
    {
        Message = message;
    }

    public Confirmation(string message, string title)
    {
        Message = message;
        Title = title;
    }

    public Confirmation(string message, string title, string positiveAction)
    {
        Message = message;
        Title = title;
        PositiveAction = positiveAction;
    }

    public Confirmation(string message, string title, string positiveAction, string negativeAction)
    {
        Message = message;
        Title = title;
        PositiveAction = positiveAction;
        NegativeAction = negativeAction;
    }
}
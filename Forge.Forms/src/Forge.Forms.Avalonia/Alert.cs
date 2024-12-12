﻿using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.AvaloniaUI;

[Form(Mode = DefaultFields.None)]
[Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
[Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
[Action("positive", "{Binding PositiveAction}",
    IsDefault = true,
    IsCancel = true,
    ClosesDialog = true,
    IsVisible = "{Binding PositiveAction|IsNotEmpty}",
    Icon = "{Binding PositiveActionIcon}")]
public sealed class Alert : DialogBase
{
    public Alert()
    {
    }

    public Alert(string message)
    {
        Message = message;
    }

    public Alert(string message, string title)
    {
        Message = message;
        Title = title;
    }

    public Alert(string message, string title, string positiveAction)
    {
        Message = message;
        Title = title;
        PositiveAction = positiveAction;
    }
}
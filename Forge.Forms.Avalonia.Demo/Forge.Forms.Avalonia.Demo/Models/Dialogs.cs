﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.AvaloniaUI;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.Avalonia.Demo.Models;

[Title("Dialogs")]
[Action("alert", "ALERT", Placement = Placement.Before)]
[Action("confirm", "CONFIRM", Placement = Placement.Before)]
[Action("long_confirm", "LONG CONFIRM", Placement = Placement.Before)]
[Action("prompt", "PROMPT", Placement = Placement.Before)]
[Action("login", "LOGIN", Placement = Placement.Before)]
[Divider]
[Title("Windows")]
[Text("Windowing is not supported on Browser platform.", IsVisible = "{Binding IsBrowser}")]
[Action("alert", "ALERT", Parameter = "window", Placement = Placement.Before, IsEnabled = "{Binding IsNotBrowser}")]
[Action("confirm", "CONFIRM", Parameter = "window", Placement = Placement.Before, IsEnabled = "{Binding IsNotBrowser}")]
[Action("long_confirm", "LONG CONFIRM", Parameter = "window", Placement = Placement.Before,
    IsEnabled = "{Binding IsNotBrowser}")]
[Action("prompt", "PROMPT", Parameter = "window", Placement = Placement.Before, IsEnabled = "{Binding IsNotBrowser}")]
[Action("login", "LOGIN", Parameter = "window", Placement = Placement.Before, IsEnabled = "{Binding IsNotBrowser}")]
public class Dialogs : ObservableObject, IActionHandler
{
    public bool IsBrowser => OperatingSystem.IsBrowser();
    public bool IsNotBrowser => !OperatingSystem.IsBrowser();

    public async void HandleAction(IActionContext actionContext)
    {
        var parameter = actionContext.ActionParameter;
        var action = actionContext.Action as string;
        var longConfirm = new Confirmation(
            "Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.",
            "Use Google's location service?", "TURN ON SPEED BOOST", "NO THANKS");

        if (parameter is "window")
        {
            switch (action)
            {
                case "alert":
                    await Show.Window(275d).For(new Alert("Hello world!"));
                    break;
                case "confirm":
                    await Show.Window(275d).For(new Confirmation("Delete item?"));
                    break;
                case "long_confirm":
                    await Show.Window(250d).For(longConfirm);
                    break;
                case "prompt":
                    await Show.Window().For(new Prompt<string> { Title = "What's your name?", Value = "User" });
                    break;
                case "login":
                    var result = await Show.Window().For<Login>();
                    if (result.Action is "login")
                        await Show.Window(275d).For(new Alert($"Hello {result.Model.Username}!"));

                    break;
                default:
                    return;
            }
        }
        else
            switch (action)
            {
                case "alert":
                    await Show.Dialog(275d).For(new Alert("Hello world!"));
                    break;
                case "confirm":
                    await Show.Dialog(275d).For(new Confirmation("Delete item?"));
                    break;
                case "long_confirm":
                    await Show.Dialog(250d).For(longConfirm);
                    break;
                case "prompt":
                    await Show.Dialog().For(new Prompt<string> { Title = "What's your name?", Value = "User" });
                    break;
                case "login":
                    var result = await Show.Dialog().For<Login>();
                    if (result.Action is "login")
                        await Show.Dialog(275d).For(new Alert($"Hello {result.Model.Username}!"));

                    break;
                default:
                    return;
            }
    }
}
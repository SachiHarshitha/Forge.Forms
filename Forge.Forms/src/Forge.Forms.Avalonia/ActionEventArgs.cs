using System;

namespace Forge.Forms.AvaloniaUI;

public class ActionEventArgs : EventArgs
{
    public ActionEventArgs(IActionContext actionContext)
    {
        ActionContext = actionContext;
    }


    public IActionContext ActionContext { get; }
}
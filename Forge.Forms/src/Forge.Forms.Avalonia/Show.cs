using System.Threading.Tasks;
using DialogHostAvalonia;
using Forge.Forms.AvaloniaUI.Controls;
using Forge.Forms.AvaloniaUI.FormBuilding;
using Type = System.Type;

namespace Forge.Forms.AvaloniaUI;

public static class Show
{
    public static IModelHost Window()
    {
        return new WindowModelHost(null, WindowOptions.Default);
    }

    public static IModelHost Window(object context)
    {
        return new WindowModelHost(context, WindowOptions.Default);
    }

    public static IModelHost Window(WindowOptions options)
    {
        return new WindowModelHost(null, options);
    }

    public static IModelHost Window(double width)
    {
        return new WindowModelHost(null, new WindowOptions { Width = width });
    }

    public static IModelHost Window(object context, WindowOptions options)
    {
        return new WindowModelHost(context, options);
    }

    public static IModelHost Window(object context, double width)
    {
        return new WindowModelHost(context, new WindowOptions { Width = width });
    }

    public static IModelHost Dialog()
    {
        return new DialogModelHost(null, null, DialogOptions.Default);
    }

    public static IModelHost Dialog(DialogOptions options)
    {
        return new DialogModelHost(null, null, options);
    }

    public static IModelHost Dialog(double width)
    {
        return new DialogModelHost(null, null, new DialogOptions { Width = width });
    }

    public static IModelHost Dialog(string dialogIdentifier)
    {
        return new DialogModelHost(dialogIdentifier, null, DialogOptions.Default);
    }

    public static IModelHost Dialog(string dialogIdentifier, object context)
    {
        return new DialogModelHost(dialogIdentifier, context, DialogOptions.Default);
    }

    public static IModelHost Dialog(string dialogIdentifier, DialogOptions options)
    {
        return new DialogModelHost(dialogIdentifier, null, options);
    }

    public static IModelHost Dialog(string dialogIdentifier, double width)
    {
        return new DialogModelHost(dialogIdentifier, null, new DialogOptions { Width = width });
    }

    public static IModelHost Dialog(string dialogIdentifier, object context, DialogOptions options)
    {
        return new DialogModelHost(dialogIdentifier, context, options);
    }

    public static IModelHost Dialog(string dialogIdentifier, object context, double width)
    {
        return new DialogModelHost(dialogIdentifier, context, new DialogOptions { Width = width });
    }

    private class WindowModelHost : IModelHost
    {
        private readonly object context;
        private readonly WindowOptions options;

        public WindowModelHost(object context, WindowOptions options)
        {
            this.context = context;
            this.options = options;
        }

        public async Task<DialogResult<T>> For<T>(T model)
        {
            return (await ShowWindowAsync(model)).MakeGeneric<T>();
        }

        public async Task<DialogResult> For(Type type)
        {
            return await ShowWindowAsync(type);
        }

        public async Task<DialogResult> For(IFormDefinition formDefinition)
        {
            return await ShowWindowAsync(formDefinition);
        }


        private async Task<DialogResult> ShowWindowAsync(object model)
        {
            object lastAction = null;
            object lastActionParameter = null;
            var window = new DialogWindow(model, context, options);

            if (options.TopMost) window.Topmost = true;

            var tcs = new TaskCompletionSource<DialogResult>();

            window.Form.OnAction += (s, e) =>
            {
                lastAction = e.ActionContext.Action;
                lastActionParameter = e.ActionContext.ActionParameter;
            };

            window.Closed += (s, e) =>
            {
                var result = new DialogResult(window.Form.Value, lastAction, lastActionParameter);
                tcs.SetResult(result);
            };

            window.Show();
            return await tcs.Task;
        }
    }

    private class DialogModelHost : IModelHost
    {
        private readonly object context;
        private readonly string dialogIdentifier;
        private readonly DialogOptions options;

        public DialogModelHost(string dialogIdentifier, object context, DialogOptions options)
        {
            this.context = context;
            this.options = options;
            this.dialogIdentifier = dialogIdentifier ?? "MainDialogHost";
        }

        public async Task<DialogResult<T>> For<T>(T model)
        {
            return (await ShowDialog(model)).MakeGeneric<T>();
        }

        public Task<DialogResult> For(Type type)
        {
            return ShowDialog(type);
        }

        public Task<DialogResult> For(IFormDefinition formDefinition)
        {
            return ShowDialog(formDefinition);
        }

        private async Task<DialogResult> ShowDialog(object model)
        {
            object lastAction = null;
            object lastActionParameter = null;
            var wrapper = new DynamicFormWrap(model, context, options);
            wrapper.Form.OnAction += (s, e) =>
            {
                lastAction = e.ActionContext.Action;
                lastActionParameter = e.ActionContext.ActionParameter;
            };

            await DialogHost.Show(wrapper, dialogIdentifier);
            return new DialogResult(wrapper.Form.Value, lastAction, lastActionParameter);
        }
    }
}

public interface IModelHost
{
    Task<DialogResult<T>> For<T>(T model);

    Task<DialogResult> For(Type type);

    Task<DialogResult> For(IFormDefinition formDefinition);
}

public static class ModelHostExtensions
{
    public static async Task<DialogResult<T>> For<T>(this IModelHost modelHost)
    {
        var result = await modelHost.For(typeof(T));
        return new DialogResult<T>((T)result.Model, result.Action, result.ActionParameter);
    }
}

public class DialogResult
{
    public DialogResult(object model, object action, object actionParameter)
    {
        Model = model;
        Action = action;
        ActionParameter = actionParameter;
    }

    public object Model { get; }

    public object Action { get; }

    public object ActionParameter { get; }

    internal DialogResult<T> MakeGeneric<T>()
    {
        return new DialogResult<T>((T)Model, Action, ActionParameter);
    }
}

public class DialogResult<T>
{
    public DialogResult(T model, object action, object actionParameter)
    {
        Model = model;
        Action = action;
        ActionParameter = actionParameter;
    }

    public T Model { get; }

    public object Action { get; }

    public object ActionParameter { get; }
}
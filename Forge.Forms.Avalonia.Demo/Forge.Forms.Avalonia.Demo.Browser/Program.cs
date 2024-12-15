using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Forge.Forms.Avalonia.Demo;

internal sealed class Program
{
    //Trace.Listeners.Add(new ConsoleTraceListener());

    private static Task Main(string[] args)
    {
        return BuildAvaloniaApp()
            .LogToTrace()
            .WithInterFont()
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>();
    }
}
using Avalonia.Controls;

namespace Forge.Forms.AvaloniaUI;

public class WindowOptions : DialogOptions
{
    public new static WindowOptions Default = new();
    private bool canResize;
    private Window owner;
    private bool showCaptionButtons = true;

    private string title = "Dialog";
    private WindowStartupLocation windowStartupLocation;

    public WindowOptions()
        : this(Default)
    {
    }

    public WindowOptions(WindowOptions defaults)
        : base(defaults)
    {
        if (defaults == null) return;

        title = defaults.title;
        showCaptionButtons = defaults.showCaptionButtons;
        canResize = defaults.canResize;
        windowStartupLocation = defaults.windowStartupLocation;
        owner = defaults.owner;
    }

    public bool TopMost { get; set; }

    public bool BringToFront { get; set; }

    public string Title
    {
        get => title;
        set
        {
            if (value == title) return;

            title = value;
            OnPropertyChanged();
        }
    }

    public bool ShowCaptionButtons
    {
        get => showCaptionButtons;
        set
        {
            if (value == showCaptionButtons) return;

            showCaptionButtons = value;
            OnPropertyChanged();
        }
    }

    public bool CanResize
    {
        get => canResize;
        set
        {
            if (value == canResize) return;

            canResize = value;
            OnPropertyChanged();
        }
    }

    public WindowStartupLocation WindowStartupLocation
    {
        get => windowStartupLocation;
        set
        {
            if (value == windowStartupLocation) return;

            windowStartupLocation = value;
            OnPropertyChanged();
        }
    }

    public Window Owner
    {
        get => owner;
        set
        {
            if (value == owner) return;

            owner = value;
            OnPropertyChanged();
        }
    }
}
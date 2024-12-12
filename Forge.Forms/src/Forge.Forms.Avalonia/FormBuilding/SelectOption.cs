using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public class SelectOption : INotifyPropertyChanged
{
    private string name;
    private string value;

    public SelectOption(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    public string Name
    {
        get => name;
        set
        {
            if (name == value) return;
            name = value;
            OnPropertyChanged();
        }
    }

    public string Value
    {
        get => value;
        set
        {
            if (this.value == value) return;
            this.value = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public override string ToString()
    {
        return Name;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
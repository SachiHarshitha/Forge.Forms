using System.Collections.Generic;
using System.Collections.ObjectModel;
using Forge.Application.AvaloniaUI.Views;

namespace Forge.Application.AvaloniaUI.Routing.Default
{
    public class ListRoute : Route
    {
        private object selectedItem;
        private string displayMemberPath;

        public ListRoute(string title, IEnumerable<object> items)
        {
            this.RouteConfig.Title = title;
            this.RouteConfig.KeyBindings.Add(new KeyBinding(this.PopRouteCommand, Key.Escape, ModifierKeys.None));
            this.Items = new ObservableCollection<object>(items);
        }

        public ObservableCollection<object> Items { get; }

        public object SelectedItem
        {
            get => this.selectedItem;
            set
            {
                if (Equals(value, this.selectedItem)) return;
                this.selectedItem = value;
                this.NotifyPropertyChanged();
            }
        }

        public string DisplayMemberPath
        {
            get => this.displayMemberPath;
            set
            {
                if (value == this.displayMemberPath) return;
                this.displayMemberPath = value;
                this.NotifyPropertyChanged();
            }
        }

        protected internal override object CreateView(bool isReload)
        {
            return new CollectionView();
        }
    }
}

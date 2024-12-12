using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Forge.Application.AvaloniaUI.Infrastructure;
using Forge.Application.AvaloniaUI.Routing;

namespace Forge.Application.AvaloniaUI.Controls
{
    /// <summary>
    /// Interaction logic for MaterialRoutesWindow.xaml
    /// </summary>
    public partial class MaterialRoutesWindow : Window
    {
        private readonly AppController controller;

        public MaterialRoutesWindow(AppController controller)
        {
            this.DataContext = controller ?? throw new ArgumentNullException(nameof(controller));
            this.controller = controller;
            InitializeComponent();
        }

        //public object CurrentView => VisualChildren.GetChild(this.RouteContentPresenter, 0);

     

        private void MenuRoute_Click(object sender, RoutedEventArgs e)
        {
            if (!this.controller.IsMenuOpen)
            {
                return;
            }

            this.controller.IsMenuOpen = false;
            var route = ((Control)sender).DataContext as Route;
            this.controller.Routes.Change(route);
        }
    }
}

namespace Forge.Application.AvaloniaUI.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    internal partial class LoadingView : UserControl
    {
        public LoadingView(string message)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(message))
            {
                this.MessageTextBlock.Text = message;
            }
            else
            {
                this.StackPanel.Children.RemoveAt(1);
            }
        }
    }
}

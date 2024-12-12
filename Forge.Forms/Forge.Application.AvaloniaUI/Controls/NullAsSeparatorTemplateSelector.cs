using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;

namespace Forge.Application.AvaloniaUI.Controls
{
    internal class NullAsSeparatorTemplateSelector : IDataTemplate
    {
        public DataTemplate SelectTemplate(object item, AvaloniaObject container)
        {
            var element = container as Control;
            if (element == null)
            {
                return null;
            }

            if (item == null)
            {
                return element.FindResource("SeparatorDataTemplate") as DataTemplate;
            }

            return element.FindResource("ItemDataTemplate") as DataTemplate;
        }

        public Control? Build(object? param)
        {
            throw new System.NotImplementedException();
        }

        public bool Match(object? data)
        {
            throw new System.NotImplementedException();
        }
    }
}

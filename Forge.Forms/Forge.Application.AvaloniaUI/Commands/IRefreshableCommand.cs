using System.Windows.Input;

namespace Forge.Application.AvaloniaUI.Commands
{
    public interface IRefreshableCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}

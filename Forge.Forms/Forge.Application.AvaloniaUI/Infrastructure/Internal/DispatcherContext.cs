using System;

namespace Forge.Application.AvaloniaUI.Infrastructure.Internal
{
    internal class DispatcherContext : IContext
    {
        private readonly Dispatcher dispatcher;

        public DispatcherContext(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public bool IsSynchronized => this.dispatcher.CheckAccess();

        public void Invoke(Action action) => this.dispatcher.Invoke(action);
    }
}

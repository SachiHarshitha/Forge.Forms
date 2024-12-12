using System;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Forge.Application.AvaloniaUI.Controls
{
    internal class LockableToggleButton : ToggleButton
    {
        public static readonly AvaloniaProperty LockToggleProperty =
            AvaloniaProperty.Register<LockableToggleButton,bool>("LockToggle", false);

        protected override Type StyleKeyOverride => typeof(ToggleButton);

        public LockableToggleButton()
        {
        }

        public bool LockToggle
        {
            get { return (bool)this.GetValue(LockToggleProperty); }
            set { this.SetValue(LockToggleProperty, value); }
        }

        protected override void Toggle()
        {
            if (!this.LockToggle)
            {
                base.Toggle();
            }        
        }


    }
}

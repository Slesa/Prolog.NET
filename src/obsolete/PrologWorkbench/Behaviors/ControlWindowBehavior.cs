using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Prolog.Workbench.Behaviors
{
    public class ControlWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += OnLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= OnLeftButtonDown;
        }

        void OnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                AssociatedObject.WindowState = AssociatedObject.WindowState == WindowState.Maximized
                                                   ? WindowState.Normal
                                                   : WindowState.Maximized;
            }
            else
            {
                AssociatedObject.DragMove();
            }
        }
    }
}
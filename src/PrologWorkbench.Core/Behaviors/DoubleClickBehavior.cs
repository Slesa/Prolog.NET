using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrologWorkbench.Core.Behaviors
{
    public class DoubleClickBehavior
    {
        public static readonly DependencyProperty DoubleClickProperty =
            DependencyProperty.RegisterAttached("DoubleClick", 
                typeof (ICommand), 
                typeof (DoubleClickBehavior), 
                new FrameworkPropertyMetadata(null, DoubleClickBehavior.DoubleClickChanged));

        public static void SetDoubleClick(DependencyObject target, ICommand value)
        {
            target.SetValue(DoubleClickProperty, value);
        }

        public static ICommand GetDoubleClick(DependencyObject target)
        {
            return (ICommand) target.GetValue(DoubleClickProperty);
        }

        public static void ElementMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement) sender;
            var command = (ICommand) element.GetValue(DoubleClickProperty);
            command.Execute(null);
        }

        static void DoubleClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TreeViewItem;
            if(element==null ) return;
            if(e.NewValue!=null && e.OldValue==null)
            {
                element.MouseDoubleClick += ElementMouseDoubleClick;
            }
            if(e.NewValue==null && e.OldValue!=null)
            {
                element.MouseDoubleClick -= ElementMouseDoubleClick;
            }
        }
    }
}
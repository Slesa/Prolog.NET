using System.Windows;

namespace PrologWorkbench.Core.Behaviors
{
    public class ToolTipHotkey
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("TextProperty",
                typeof(string), typeof(ToolTipHotkey), new PropertyMetadata(default(string)));

        public static void SetText(FrameworkElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }

        public static string GetText(FrameworkElement element)
        {
            return (string) element.GetValue(TextProperty);
        }
    }
}
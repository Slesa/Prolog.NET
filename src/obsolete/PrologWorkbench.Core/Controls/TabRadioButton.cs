using System.Windows;
using System.Windows.Controls;

namespace PrologWorkbench.Core.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrologWorkbench.Core.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PrologWorkbench.Core.Controls;assembly=PrologWorkbench.Core.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TabRadioButton/>
    ///
    /// </summary>
    public class TabRadioButton : RadioButton
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof (string), typeof (TabRadioButton), new PropertyMetadata(default(Image)));

        public string Icon
        {
            get { return (string) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (TabRadioButton), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        static TabRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabRadioButton), new FrameworkPropertyMetadata(typeof(TabRadioButton)));
        }
    }
}

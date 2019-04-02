using System.Windows.Controls;

namespace PrologWorkbench.Core.Contracts
{
    public interface IWorkbenchModule
    {
        int Position { get; }
        string Icon { get; }
        string Title { get; }
        string ToolTip { get; }
        Control View { get; }
    }
}
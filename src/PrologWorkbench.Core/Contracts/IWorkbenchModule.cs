namespace PrologWorkbench.Core.Contracts
{
    public interface IWorkbenchModule
    {
        int Position { get; }
        string Icon { get; }
        string Title { get; }
    }
}
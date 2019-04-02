namespace PrologWorkbench.Core.Contracts
{
    public interface IProvideStatusUpdates
    {
        void Publish(string message);
    }
}
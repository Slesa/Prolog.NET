using Prolog;

namespace PrologWorkbench.Core.Contracts
{
    public interface IProvideMachine
    {
        PrologMachine Machine { get; }
        void Reset();
    }
}
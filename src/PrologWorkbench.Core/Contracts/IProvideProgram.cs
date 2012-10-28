using Prolog;

namespace PrologWorkbench.Core.Contracts
{
    public interface IProvideProgram
    {
        Program Program { get; }
        void Reset();
    }
}
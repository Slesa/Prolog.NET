using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Contracts
{
    public interface IProvideTranscript
    {
        Transcript Transcript { get; }
        void Reset();
    }

}
using Prolog;

namespace PrologWorkbench.Core
{
    public interface IProvideProgram
    {
        Program Program { get; }
        void Reset();
        bool Load(string fileName);
        bool Save(string fileName);
    }
}
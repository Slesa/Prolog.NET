using Prolog;

namespace PrologWorkbench.Core
{
    public interface IProvideProgram
    {
        Program Program { get; }
        bool Load(string fileName);
        bool Save(string fileName);
    }
}
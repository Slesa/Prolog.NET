using Prolog;

namespace PrologWorkbench.Core.Contracts
{
    public interface ILoadOrSaveProgram
    {
        Program Load(string fileName);
        bool Save(string fileName, Program program);
    }
}
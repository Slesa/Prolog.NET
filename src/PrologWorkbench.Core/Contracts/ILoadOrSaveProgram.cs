namespace PrologWorkbench.Core.Contracts
{
    public interface ILoadOrSaveProgram
    {
        bool Load(string fileName);
        bool Save(string fileName);
    }

}
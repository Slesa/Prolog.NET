using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.Models
{
    public class ProgramAccessor : ILoadOrSaveProgram
    {
        readonly IProvideProgram _programProvider;

        public ProgramAccessor(IProvideProgram programProvider)
        {
            _programProvider = programProvider;
        }

        public bool Load(string fileName)
        {
            return false;
        }

        public bool Save(string fileName)
        {
            return false;
        }
    }
}
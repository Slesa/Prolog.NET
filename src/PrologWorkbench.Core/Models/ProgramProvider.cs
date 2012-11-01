using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.Models
{
    public class ProgramProvider : IProvideProgram
    {
        public ProgramProvider()
        {
            Reset();
        }

        Program _program;
        public Program Program
        {
            get { return _program; }
            set
            {
                _program = value;
                OnProgramChanged();
            }
        }

        void OnProgramChanged()
        {
            if (ProgramChanged != null)
            {
                ProgramChanged(this, new ProgramChangedEventArgs(Program));
            }
        }

        public void Reset()
        {
            Program = new Program();
        }

        public event ProgramChangedEventHandler ProgramChanged;
    }
}

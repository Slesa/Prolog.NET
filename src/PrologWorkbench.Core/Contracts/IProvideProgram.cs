using System;
using Prolog;

namespace PrologWorkbench.Core.Contracts
{
    public class ProgramChangedEventArgs : EventArgs
    {
        public ProgramChangedEventArgs(Program program)
        {
            Program = program;
        }
        public Program Program { get; private set; }
    }

    public delegate void ProgramChangedEventHandler(object sender, ProgramChangedEventArgs e);

    public interface IProvideProgram
    {
        Program Program { get; set; }
        void Reset();
        event ProgramChangedEventHandler ProgramChanged;
    }
}
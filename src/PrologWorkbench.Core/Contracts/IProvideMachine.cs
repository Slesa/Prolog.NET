using System;
using Prolog;

namespace PrologWorkbench.Core.Contracts
{
    public class MachineChangedEventArgs : EventArgs
    {
        public MachineChangedEventArgs(PrologMachine machine)
        {
            Machine = machine;
        }
        public PrologMachine Machine { get; set; }
    }

    public delegate void MachineChangedEventHandler(object sender, MachineChangedEventArgs e);

    public interface IProvideMachine
    {
        PrologMachine Machine { get; }
        void Reset();
        event MachineChangedEventHandler MachineChanged;
    }
}
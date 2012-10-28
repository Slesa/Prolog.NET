using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.Models
{
    public class MachineProvider : IProvideMachine
    {
        PrologMachine _machine;
        public PrologMachine Machine
        {
            get { return _machine; }
            private set
            {
                _machine = value;
                OnMachineChanged();
            }
        }

        void OnMachineChanged()
        {
            if (MachineChanged != null)
            {
                MachineChanged(this, new MachineChangedEventArgs(Machine));
            }
        }

        public void Reset()
        {
        }

        public event MachineChangedEventHandler MachineChanged;
    }
}
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
            set
            {
                if (Machine == value) return;
                _machine = value;
                OnMachineChanged();
                if (_machine != null)
                {
                    _machine.ExecutionComplete += OnMachineExecutionComplete;
                }
            }
        }

        void OnMachineExecutionComplete(object sender, PrologQueryEventArgs e)
        {
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
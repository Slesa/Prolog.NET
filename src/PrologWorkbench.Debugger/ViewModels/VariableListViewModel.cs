using System;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Debugger.ViewModels
{
    public abstract class VariableListViewModel : NotificationObject
    {
        protected readonly IProvideMachine MachineProvider;
        PrologMachine _currentMachine;
        readonly string _title;

        protected VariableListViewModel(IProvideMachine machineProvider, string title)
        {
            _title = title;
            MachineProvider = machineProvider;
            MachineProvider.MachineChanged += OnMachineChanged;
        }

        public string Title { get { return _title; } }
        protected abstract void AdjustVariables();

        PrologVariableList _variables;
        public PrologVariableList Variables
        {
            get { return _variables; }
            set
            {
                _variables = value;
                RaisePropertyChanged(() => Variables);
            }
        }

        void OnMachineChanged(object sender, MachineChangedEventArgs e)
        {
            if (_currentMachine != null)
            {
                _currentMachine.ExecutionComplete -= OnExecutionComplete;
                _currentMachine.ExecutionSuspended -= OnExecutionSuspended;
            }
            _currentMachine = e.Machine;
            if (_currentMachine != null)
            {
                _currentMachine.ExecutionComplete += OnExecutionComplete;
                _currentMachine.ExecutionSuspended += OnExecutionSuspended;
            }
            AdjustVariables();
        }

        void OnExecutionComplete(object sender, PrologQueryEventArgs e)
        {
            AdjustVariables();
        }

        void OnExecutionSuspended(object sender, EventArgs e)
        {
            AdjustVariables();
        }
    }

    public class ArgumentsVariableListViewModel : VariableListViewModel
    {
        public ArgumentsVariableListViewModel(IProvideMachine machineProvider)
            : base(machineProvider, Resources.Strings.VariablesListViewModel_ArgumentsList)
        {
        }

        protected override void AdjustVariables()
        {
            Variables = MachineProvider.Machine.Arguments;
        }
    }

    public class TemporaryVariableListViewModel : VariableListViewModel
    {
        public TemporaryVariableListViewModel(IProvideMachine machineProvider)
            : base(machineProvider, Resources.Strings.VariablesListViewModel_TemporariesList)
        {
        }

        protected override void AdjustVariables()
        {
            Variables = MachineProvider.Machine.TemporaryVariables;
        }
    }

    public class PermanentVariablesListViewModel : VariableListViewModel
    {
        public PermanentVariablesListViewModel(IProvideMachine machineProvider, IEventAggregator eventAggregator)
            : base(machineProvider, Resources.Strings.VariablesListViewModel_PermanentsList)
        {
            eventAggregator.GetEvent<CurrentStackFrameChangedEvent>().Subscribe(OnStackFrameChanged);
        }

        protected override void AdjustVariables()
        {
        }

        void OnStackFrameChanged(PrologStackFrame stackFrame)
        {
            Variables = stackFrame.Variables;
        }
    }
}
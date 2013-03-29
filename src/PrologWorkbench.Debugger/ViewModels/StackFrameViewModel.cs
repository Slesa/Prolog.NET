using System;
using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class StackFrameViewModel : NotificationObject
    {
        readonly IProvideMachine _machineProvider;
        PrologMachine _currentMachine;
        public string Title { get { return Resources.Strings.StackFrameViewModel_Title; } }

        public StackFrameViewModel(IProvideMachine machineProvider)
        {
            _machineProvider = machineProvider;
            _machineProvider.MachineChanged += OnMachineChanged;
        }

        PrologStackFrameList _stackFrames;
        public PrologStackFrameList StackFrames
        {
            get { return _stackFrames; }
            set
            {
                _stackFrames = value;
                RaisePropertyChanged(() => StackFrames);
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
            SetCurrentStackFrame();
        }

        void SetCurrentStackFrame()
        {
            //if (_machineProvider.Machine.StackFrames.Count <= 0) return;
            //var stackFrame = _machineProvider.Machine.StackFrames[_machineProvider.Machine.StackFrames.Count - 1];
            StackFrames = _machineProvider.Machine.StackFrames;
        }

        void OnExecutionComplete(object sender, PrologQueryEventArgs e)
        {
            SetCurrentStackFrame();
        }

        void OnExecutionSuspended(object sender, EventArgs e)
        {
            SetCurrentStackFrame();
        }
    }
}
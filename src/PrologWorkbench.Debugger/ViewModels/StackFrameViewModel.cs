using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class CurrentStackFrameChangedEvent : CompositePresentationEvent<PrologStackFrame> { }

    public class StackFrameViewModel : NotificationObject
    {
        readonly IProvideMachine _machineProvider;
        readonly IEventAggregator _eventAggregator;
        PrologMachine _currentMachine;
        public string Title { get { return Resources.Strings.StackFrameViewModel_Title; } }

        public StackFrameViewModel(IProvideMachine machineProvider, IEventAggregator eventAggregator)
        {
            _machineProvider = machineProvider;
            _eventAggregator = eventAggregator;
            _machineProvider.MachineChanged += OnMachineChanged;

            RestartCommand = new DelegateCommand(OnRestart);
            RunToBacktrackCommand = new DelegateCommand(OnRunToBacktrack);
            RunToSuccessCommand = new DelegateCommand(OnRunToSuccess);
        }

        void OnRestart()
        {
            _machineProvider.Machine.Restart();
        }

        void OnRunToBacktrack()
        {
        }

        void OnRunToSuccess()
        {
        }

        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand RunToBacktrackCommand { get; private set; }
        public DelegateCommand RunToSuccessCommand { get; private set; }
        public DelegateCommand StepIntoCommand { get; private set; }
        public DelegateCommand StepOverCommand { get; private set; }
        public DelegateCommand ReturnToCallerCommand { get; private set; }

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

        PrologStackFrame _currentStackFrame;
        public PrologStackFrame CurrentStackFrame
        {
            get { return _currentStackFrame; }
            set
            {
                _currentStackFrame = value;
                _eventAggregator.GetEvent<CurrentStackFrameChangedEvent>().Publish(_currentStackFrame);
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
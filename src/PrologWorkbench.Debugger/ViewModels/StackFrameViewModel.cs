using System;
using System.ComponentModel;
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

            RestartCommand = new DelegateCommand(Restart, CanRestart);
            RunToBacktrackCommand = new DelegateCommand(RunToBacktrack, CanRunToBacktrack);
            RunToSuccessCommand = new DelegateCommand(RunToSuccess, CanRunToSuccess);
            StepIntoCommand = new DelegateCommand(StepInto, CanStepInto);
            StepOverCommand = new DelegateCommand(StepOver, CanStepOver);
            ReturnToCallerCommand = new DelegateCommand(ReturnToCaller, CanReturnToCaller);
        }

        #region Restart Command

        public DelegateCommand RestartCommand { get; private set; }

        bool CanRestart()
        {
            return _currentMachine != null && _currentMachine.CanRestart;
        }

        void Restart()
        {
            _currentMachine.Restart();
        }

        #endregion
        #region Run to backtrack command

        public DelegateCommand RunToBacktrackCommand { get; private set; }

        bool CanRunToBacktrack()
        {
            return _currentMachine != null && _currentMachine.CanRunToBacktrack;
        }

        void RunToBacktrack()
        {
            _currentMachine.RunToBacktrack();
        }

        #endregion
        #region Run to success command

        public DelegateCommand RunToSuccessCommand { get; private set; }

        bool CanRunToSuccess()
        {
            return _currentMachine != null && _currentMachine.CanRunToSuccess;
        }

        void RunToSuccess()
        {
            _currentMachine.RunToSuccess();
        }

        #endregion
        #region Step into command

        public DelegateCommand StepIntoCommand { get; private set; }

        bool CanStepInto()
        {
            return _currentMachine != null && _currentMachine.CanStepIn;
        }

        void StepInto()
        {
            _currentMachine.StepIn();
        }

        #endregion
        #region Step into command

        public DelegateCommand StepOverCommand { get; private set; }

        bool CanStepOver()
        {
            return _currentMachine != null && _currentMachine.CanStepOver;
        }

        void StepOver()
        {
            _currentMachine.StepOver();
        }

        #endregion
        #region Step into command

        public DelegateCommand ReturnToCallerCommand { get; private set; }

        bool CanReturnToCaller()
        {
            return _currentMachine != null && _currentMachine.CanStepOut;
        }

        void ReturnToCaller()
        {
            _currentMachine.StepOut();
        }

        #endregion

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
                AdjustCanExecutes();
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

        void AdjustCanExecutes()
        {
            RestartCommand.RaiseCanExecuteChanged();
            RunToBacktrackCommand.RaiseCanExecuteChanged();
            RunToSuccessCommand.RaiseCanExecuteChanged();
            StepIntoCommand.RaiseCanExecuteChanged();
            StepOverCommand.RaiseCanExecuteChanged();
            ReturnToCallerCommand.RaiseCanExecuteChanged();
        }

        void SetCurrentStackFrame()
        {
            AdjustCanExecutes();
            if (_machineProvider.Machine == null)
            {
                StackFrames = null;
                return;
            }
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
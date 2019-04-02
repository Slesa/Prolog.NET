using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Events;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class DebuggerViewModel
    {
        readonly IEventAggregator _eventAggregator;

        public DebuggerViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ActivateDebuggerEvent>().Subscribe(OnActivateDebugger);
        }

        void OnActivateDebugger(bool activate)
        {
            _eventAggregator.GetEvent<ActivateModuleEvent>().Publish(DebuggerModule.TagDebuggerModule);
        }
    }
}
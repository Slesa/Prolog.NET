using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using PrologWorkbench.Core.Events;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class DebuggerViewModel
    {
        readonly IRegionManager _regionManager;

        public DebuggerViewModel(IEventAggregator eventAggregator,  IRegionManager regionManager)
        {
            _regionManager = regionManager;
            eventAggregator.GetEvent<ActivateDebuggerEvent>().Subscribe(OnActivateDebugger);
        }

        void OnActivateDebugger(bool activate)
        {
            //_regionManager.Regions[ModulesViewModel.TagMainRegion].Activate();
        }
    }
}
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Debugger.ViewModels;
using PrologWorkbench.Debugger.Views;

namespace PrologWorkbench.Debugger
{
    public class DebuggerModule : IModule
    {
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public DebuggerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<VariableListViewModel>();

            _regionManager.RegisterViewWithRegion("VariablesRegion", typeof(VariableListView));
            _regionManager.RegisterViewWithRegion("DebuggerRegion", typeof(DebuggerView));
        }
    }
}

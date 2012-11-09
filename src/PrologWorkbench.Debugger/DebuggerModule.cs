using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Debugger.Resources;
using PrologWorkbench.Debugger.ViewModels;
using PrologWorkbench.Debugger.Views;

namespace PrologWorkbench.Debugger
{
    public class DebuggerModule : IModule, IWorkbenchModule
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
            _container.RegisterInstance<IWorkbenchModule>("DebuggerModule", this);

            _regionManager.RegisterViewWithRegion("VariablesRegion", typeof(VariableListView));
            _regionManager.RegisterViewWithRegion("DebuggerRegion", typeof(DebuggerView));
        }

        public int Position { get { return 30; } }
        public string Icon { get { return "/PrologWorkbench.Debugger;component/Resources/Debugger.png"; } }
        public string Title { get { return Strings.DebuggerModule_Title; } }
    }
}

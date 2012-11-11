using System.Windows.Controls;
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
            _container.RegisterType<ArgumentsVariableListViewModel>();
            _container.RegisterType<TemporaryVariableListViewModel>();
            _container.RegisterType<PermanentVariablesListViewModel>();

            _container.RegisterInstance<IWorkbenchModule>("DebuggerModule", this);

            _container.RegisterType<VariableListView>("VarArgumentsView", new InjectionConstructor(_container.Resolve<ArgumentsVariableListViewModel>()));
            _container.RegisterType<VariableListView>("VarTemporariesView", new InjectionConstructor(_container.Resolve<TemporaryVariableListViewModel>()));
            _container.RegisterType<VariableListView>("VarPermanentsView", new InjectionConstructor(_container.Resolve<PermanentVariablesListViewModel>()));

            _regionManager.RegisterViewWithRegion("VarArgumentsRegion", () => _container.Resolve<VariableListView>("VarArgumentsView"));
            _regionManager.RegisterViewWithRegion("VarTemporariesRegion", () => _container.Resolve<VariableListView>("VarTemporariesView"));
            _regionManager.RegisterViewWithRegion("VarPermanentsRegion", () => _container.Resolve<VariableListView>("VarPermanentsView"));
            
            _container.RegisterType<DebuggerView>(new ContainerControlledLifetimeManager());
        }

        public int Position { get { return 30; } }
        public string Icon { get { return "/PrologWorkbench.Debugger;component/Resources/Debugger.png"; } }
        public string Title { get { return Strings.DebuggerModule_Title; } }
        public Control View { get { return _container.Resolve<DebuggerView>(); } }
    }
}

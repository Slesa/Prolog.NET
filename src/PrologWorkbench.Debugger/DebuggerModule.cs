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
        public static readonly string TagDebuggerModule = "DebuggerModule";
        public static readonly string TagStackFrameView = "StackFrameView";
        public static readonly string TagInstructionsView = "InstructionsView";
        public static readonly string TagVarArgumentsView = "VarArgumentsView";
        public static readonly string TagVarTemporariesView = "VarTemporariesView";
        public static readonly string TagVarPermanentsView = "VarPermanentsView";
        public static readonly string TagStackFrameRegion = "StackFrameRegion";
        public static readonly string TagInstructionsRegion = "InstructionsRegion";
        public static readonly string TagVarArgumentsRegion = "VarArgumentsRegion";
        public static readonly string TagVarTemporariesRegion = "VarTemporariesRegion";
        public static readonly string TagVarPermanentsRegion = "VarPermanentsRegion";

        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public DebuggerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterInstance<IWorkbenchModule>(TagDebuggerModule, this);

            _container.RegisterType<StackFrameViewModel>();
            _container.RegisterType<InstructionsViewModel>();
            _container.RegisterType<ArgumentsVariableListViewModel>();
            _container.RegisterType<TemporaryVariableListViewModel>();
            _container.RegisterType<PermanentVariablesListViewModel>();
            
            _container.RegisterType<VariableListView>(TagVarArgumentsView, new InjectionConstructor(_container.Resolve<ArgumentsVariableListViewModel>()));
            _container.RegisterType<VariableListView>(TagVarTemporariesView, new InjectionConstructor(_container.Resolve<TemporaryVariableListViewModel>()));
            _container.RegisterType<VariableListView>(TagVarPermanentsView, new InjectionConstructor(_container.Resolve<PermanentVariablesListViewModel>()));

            _regionManager.RegisterViewWithRegion(TagStackFrameRegion, () => _container.Resolve<StackFrameView>(TagStackFrameView));
            _regionManager.RegisterViewWithRegion(TagInstructionsRegion, () => _container.Resolve<InstructionsView>(TagInstructionsView));
            _regionManager.RegisterViewWithRegion(TagVarArgumentsRegion, () => _container.Resolve<VariableListView>(TagVarArgumentsView));
            _regionManager.RegisterViewWithRegion(TagVarTemporariesRegion, () => _container.Resolve<VariableListView>(TagVarTemporariesView));
            _regionManager.RegisterViewWithRegion(TagVarPermanentsRegion, () => _container.Resolve<VariableListView>(TagVarPermanentsView));
            
            _container.RegisterType<DebuggerView>(new ContainerControlledLifetimeManager());
        }

        public int Position { get { return 30; } }
        public string Icon { get { return "/PrologWorkbench.Debugger;component/Resources/Debugger.png"; } }
        public string Title { get { return Strings.DebuggerModule_Title; } }
        public Control View { get { return _container.Resolve<DebuggerView>(); } }
    }
}

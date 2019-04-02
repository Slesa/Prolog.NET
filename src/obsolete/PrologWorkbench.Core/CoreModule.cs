using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;
using PrologWorkbench.Core.ViewModels;
using PrologWorkbench.Core.Views;

namespace PrologWorkbench.Core
{
    public class CoreModule : IModule
    {
        public static readonly string TagStatusBarRegion = "StatusBarRegion";
        public static readonly string TagModulesRegion = "ModulesRegion";

        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public CoreModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<IProvideStatusUpdates, StatusUpdateProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILoadOrSaveProgram, ProgramAccessor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProvideProgram, ProgramProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProvideMachine, MachineProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProvideTranscript, TranscriptProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProvideCurrentClause, CurrentClauseProvider>(new ContainerControlledLifetimeManager());

            _container.RegisterType<StatusBarViewModel>();

            _regionManager.RegisterViewWithRegion(TagStatusBarRegion, typeof(StatusBarView));
            _regionManager.RegisterViewWithRegion(TagModulesRegion, typeof(ModulesView));
        }
    }
}

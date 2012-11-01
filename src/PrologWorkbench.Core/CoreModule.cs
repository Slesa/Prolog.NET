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
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public CoreModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType(typeof(ILoadOrSaveProgram), typeof(ProgramAccessor));
            _container.RegisterType(typeof(IProvideProgram), typeof(ProgramProvider));
            _container.RegisterType(typeof(IProvideMachine), typeof(MachineProvider));
            _container.RegisterType(typeof(IProvideTranscript), typeof(TranscriptProvider));
            _container.RegisterType<StatusBarViewModel>();
            _regionManager.RegisterViewWithRegion("StatusBarRegion", typeof (StatusBarView));
        }
    }
}

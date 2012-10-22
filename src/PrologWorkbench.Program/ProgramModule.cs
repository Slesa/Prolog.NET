using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Program.ViewModels;
using PrologWorkbench.Program.Views;

namespace PrologWorkbench.Program
{
    public class ProgramModule : IModule
    {
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public ProgramModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType(typeof(IProvideFilename), typeof(FilenameProvider));
            _container.RegisterType<ProgramToolbarViewModel>();
            _container.RegisterType<ProgramTreeViewModel>();
            _regionManager.RegisterViewWithRegion("ProgramToolbarRegion", typeof (ProgramToolbarView));
            _regionManager.RegisterViewWithRegion("MainRegion", typeof (ProgramTreeViewModel));
        }
    }
}

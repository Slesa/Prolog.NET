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
            _container.RegisterType<TitleBarViewModel>();
            _container.RegisterType<ProgramViewModel>();
            _container.RegisterType<StatusBarViewModel>();
            _regionManager.RegisterViewWithRegion("TitleBarRegion", typeof(TitleBarView));
            _regionManager.RegisterViewWithRegion("StatusBarRegion", typeof (StatusBarView));
            _regionManager.RegisterViewWithRegion("ProgramRegion", typeof(ProgramView));
        }
    }
}

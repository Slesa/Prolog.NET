using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Program.Views;

namespace PrologWorkbench.Program
{
    public class ExplorerModule : IModule
    {
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public ExplorerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //_container.RegisterType<ProgramViewModel>();
            _regionManager.RegisterViewWithRegion("InstructionsRegion", typeof(InstructionsView));
            _regionManager.RegisterViewWithRegion("ListingsRegion", typeof(ListingsView));
            _regionManager.RegisterViewWithRegion("ExplorerRegion", typeof(ExplorerView));
        }
    }
}

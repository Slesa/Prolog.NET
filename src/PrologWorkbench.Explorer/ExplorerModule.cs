using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Views;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;

namespace PrologWorkbench.Explorer
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
            _container.RegisterType<InstructionsViewModel>();
            _container.RegisterType<ExplorerViewModel>();

            _regionManager.RegisterViewWithRegion("InstructionsRegion", typeof(InstructionsView));
            _regionManager.RegisterViewWithRegion("ListingsRegion", typeof(ProgramView));
            _regionManager.RegisterViewWithRegion("ExplorerRegion", typeof(ExplorerView));
        }
    }
}

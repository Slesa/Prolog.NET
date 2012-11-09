using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Views;
using PrologWorkbench.Explorer.Resources;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;

namespace PrologWorkbench.Explorer
{
    public class ExplorerModule : IModule, IWorkbenchModule
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
            _container.RegisterInstance<IWorkbenchModule>("ExplorerModule", this);

            _regionManager.RegisterViewWithRegion("InstructionsRegion", typeof(InstructionsView));
            _regionManager.RegisterViewWithRegion("ListingsRegion", typeof(ProgramView));
            _regionManager.RegisterViewWithRegion("ExplorerRegion", typeof(ExplorerView));
        }

        public int Position { get { return 20; } }
        public string Icon { get { return "/PrologWorkbench.Debugger;component/Resources/Explorer.png"; } }
        public string Title { get { return Strings.ExplorerModule_Title; } }
    }
}

using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Explorer.Resources;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;

namespace PrologWorkbench.Explorer
{
    public class ExplorerModule : IModule, IWorkbenchModule
    {
        public static readonly string TagExplorerModule = "ExplorerModule";
        public static readonly string TagInstructionsRegion = "InstructionsRegion";
        public static readonly string TagListingsRegion = "ListingsRegion";

        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public ExplorerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterInstance<IWorkbenchModule>(TagExplorerModule, this);

            _container.RegisterType<InstructionsViewModel>();
            _container.RegisterType<ExplorerViewModel>();
            _container.RegisterType<ProgramInfoViewModel>();

            _regionManager.RegisterViewWithRegion(TagInstructionsRegion, typeof(InstructionsView));
            _regionManager.RegisterViewWithRegion(TagListingsRegion, typeof(ProgramInfoView));
            
            _container.RegisterType<ExplorerView>(new ContainerControlledLifetimeManager());
        }

        public int Position { get { return 20; } }
        public string Icon { get { return "/PrologWorkbench.Explorer;component/Resources/Explorer.png"; } }
        public string Title { get { return Strings.ExplorerModule_Title; } }
        public Control View { get { return _container.Resolve<ExplorerView>(); } }
    }
}

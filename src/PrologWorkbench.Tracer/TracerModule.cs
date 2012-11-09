using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Tracer.Resources;
using PrologWorkbench.Tracer.ViewModels;
using PrologWorkbench.Tracer.Views;

namespace PrologWorkbench.Tracer
{
    public class TracerModule : IModule, IWorkbenchModule
    {
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public TracerModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<TraceViewModel>();
            _container.RegisterInstance<IWorkbenchModule>("TracerModule", this);

            _regionManager.RegisterViewWithRegion("TraceRegion", typeof(TraceView));
        }

        public int Position { get { return 40; } }
        public string Icon { get { return "/PrologWorkbench.Tracer;component/Resources/Tracer.png"; } }
        public string Title { get { return Strings.TracerModule_Title; } }
    }
}

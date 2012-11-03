using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Tracer.ViewModels;
using PrologWorkbench.Tracer.Views;

namespace PrologWorkbench.Tracer
{
    public class TracerModule : IModule
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
            _regionManager.RegisterViewWithRegion("TraceRegion", typeof(TraceView));
        }
    }
}

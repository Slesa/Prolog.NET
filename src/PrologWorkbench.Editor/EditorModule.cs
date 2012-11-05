using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.ViewModels;
using PrologWorkbench.Editor.Helpers;
using PrologWorkbench.Editor.ViewModels;
using PrologWorkbench.Editor.Views;

namespace PrologWorkbench.Editor
{
    public class EditorModule : IModule
    {
        readonly IUnityContainer _container;
        readonly IRegionManager _regionManager;

        public EditorModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType(typeof(IProvideFilename), typeof(FilenameProvider));

            _container.RegisterType<TitleBarViewModel>();
            _container.RegisterType<TranscriptViewModel>();

            _regionManager.RegisterViewWithRegion("TitleBarRegion", typeof(TitleBarView));
            _regionManager.RegisterViewWithRegion("CommandRegion", typeof(CommandView));
            _regionManager.RegisterViewWithRegion("TranscriptRegion", typeof(TranscriptView));

            _regionManager.RegisterViewWithRegion("EditorRegion", typeof(EditorView));
        }
    }
}

using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Views;
using PrologWorkbench.Editor.Helpers;
using PrologWorkbench.Editor.Resources;
using PrologWorkbench.Editor.ViewModels;
using PrologWorkbench.Editor.Views;

namespace PrologWorkbench.Editor
{
    public class EditorModule : IModule, IWorkbenchModule
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
            _container.RegisterInstance<IWorkbenchModule>("EditorModule", this);

            _container.RegisterType<TitleBarViewModel>();
            _container.RegisterType<TranscriptViewModel>();

            _regionManager.RegisterViewWithRegion("TitleBarRegion", typeof(TitleBarView));
            _regionManager.RegisterViewWithRegion("CommandRegion", typeof(CommandView));
            _regionManager.RegisterViewWithRegion("ProgramRegion", typeof(ProgramView));
            _regionManager.RegisterViewWithRegion("TranscriptRegion", typeof(TranscriptView));

            _container.RegisterType<EditorView>(new ContainerControlledLifetimeManager());
        }

        public int Position { get { return 10; } }
        public string Icon { get { return "/PrologWorkbench.Editor;component/Resources/Editor.png"; } }
        public string Title { get { return Strings.EditorModule_Title; } }
        public Control View { get { return _container.Resolve<EditorView>(); } }
    }
}

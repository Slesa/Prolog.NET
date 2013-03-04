using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Editor.Helpers;
using PrologWorkbench.Editor.Resources;
using PrologWorkbench.Editor.ViewModels;
using PrologWorkbench.Editor.Views;

namespace PrologWorkbench.Editor
{
    public class EditorModule : IModule, IWorkbenchModule
    {
        public static readonly string TagEditorModule = "EditorModule";
        public static readonly string TagTitleBarRegion = "TitleBarRegion";
        public static readonly string TagCommandRegion = "CommandRegion";
        public static readonly string TagProgramRegion = "ProgramRegion";
        public static readonly string TagTranscriptRegion = "TranscriptRegion";

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
            _container.RegisterInstance<IWorkbenchModule>(TagEditorModule, this);

            _container.RegisterType<ProgramEditViewModel>();
            _container.RegisterType<TitleBarViewModel>();
            _container.RegisterType<TranscriptViewModel>();

            _regionManager.RegisterViewWithRegion(TagTitleBarRegion, typeof(TitleBarView));
            _regionManager.RegisterViewWithRegion(TagCommandRegion, typeof(CommandView));
            _regionManager.RegisterViewWithRegion(TagProgramRegion, typeof(ProgramEditView));
            _regionManager.RegisterViewWithRegion(TagTranscriptRegion, typeof(TranscriptView));

            _container.RegisterType<EditorView>(new ContainerControlledLifetimeManager());
        }

        public int Position { get { return 10; } }
        public string Icon { get { return "/PrologWorkbench.Editor;component/Resources/Editor.png"; } }
        public string Title { get { return Strings.EditorModule_Title; } }
        public Control View { get { return _container.Resolve<EditorView>(); } }
    }
}

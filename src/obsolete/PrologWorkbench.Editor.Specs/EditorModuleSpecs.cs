using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Editor.Helpers;
using PrologWorkbench.Editor.Resources;
using PrologWorkbench.Editor.ViewModels;
using PrologWorkbench.Editor.Views;

namespace PrologWorkbench.Editor.Specs
{
    [Subject(typeof(EditorModule))]
    internal class When_creating_editor_module : WithFakes
    {
        Establish context = () =>
            {
                _container = new UnityContainer();
                _regionViewRegistry = An<IRegionViewRegistry>();
                _container.RegisterInstance(_regionViewRegistry);

                _regionManager = new RegionManager();

                _subject = new EditorModule(_container, _regionManager);
                var locator = new UnityServiceLocator(_container);
                ServiceLocator.SetLocatorProvider(() => locator);
            };

        Because of = () => _subject.Initialize();

        It should_register_filename_provider = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IProvideFilename) && x.MappedToType == typeof(FilenameProvider));

        It should_register_editor_module = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IWorkbenchModule) && x.Name.Equals(EditorModule.TagEditorModule));

        It should_register_program_edit_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(ProgramEditViewModel) && x.MappedToType == typeof(ProgramEditViewModel));

        It should_register_titlebar_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(TitleBarViewModel) && x.MappedToType == typeof(TitleBarViewModel));

        It should_register_transcript_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(TranscriptViewModel) && x.MappedToType == typeof(TranscriptViewModel));

        It should_register_editor_view = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(EditorView) && x.MappedToType == typeof(EditorView));

        It should_register_titlebar_region = () => _regionManager.Regions.ContainsRegionWithName(EditorModule.TagTitleBarRegion);
        It should_register_command_region = () => _regionManager.Regions.ContainsRegionWithName(EditorModule.TagCommandRegion);
        It should_register_program_region = () => _regionManager.Regions.ContainsRegionWithName(EditorModule.TagProgramRegion);
        It should_register_transcript_region = () => _regionManager.Regions.ContainsRegionWithName(EditorModule.TagTranscriptRegion);

        It should_have_an_icon = () => _subject.Icon.ShouldNotBeNull();
        It should_have_a_title = () => _subject.Title.ShouldEqual(Strings.EditorModule_Title);
        It should_have_a_view = () => _subject.View.ShouldNotBeNull();

        static IUnityContainer _container;
        static EditorModule _subject;
        static IRegionManager _regionManager;
        static IRegionViewRegistry _regionViewRegistry;
    }
}
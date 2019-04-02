using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Core.Specs
{
    [Subject(typeof(CoreModule))]
    internal class When_creating_core_module : WithFakes
    {
        Establish context = () =>
            {
                _container = new UnityContainer();
                _regionViewRegistry = An<IRegionViewRegistry>();
                _container.RegisterInstance(_regionViewRegistry);
                
                _regionManager = new RegionManager();
                
                _subject = new CoreModule(_container, _regionManager);
                 var locator = new UnityServiceLocator(_container);
                 ServiceLocator.SetLocatorProvider(() => locator);
            };

        Because of = () => _subject.Initialize();

        It should_register_status_update_provider = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof (IProvideStatusUpdates) && x.MappedToType == typeof (StatusUpdateProvider));

        It should_register_program_accessor = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(ILoadOrSaveProgram) && x.MappedToType == typeof(ProgramAccessor));

        It should_register_program_provider = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IProvideProgram) && x.MappedToType == typeof(ProgramProvider));

        It should_register_machine_provider = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IProvideMachine) && x.MappedToType == typeof(MachineProvider));

        It should_register_transcript_provider = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IProvideTranscript) && x.MappedToType == typeof(TranscriptProvider));

        It should_register_current_clause_provider = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IProvideCurrentClause) && x.MappedToType == typeof(CurrentClauseProvider));

        It should_register_status_bar_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(StatusBarViewModel) && x.MappedToType == typeof(StatusBarViewModel));

        It should_register_statusbar_region = () => _regionManager.Regions.ContainsRegionWithName(CoreModule.TagStatusBarRegion);
        It should_register_modules_view_region = () => _regionManager.Regions.ContainsRegionWithName(CoreModule.TagModulesRegion);

        static IUnityContainer _container;
        static CoreModule _subject;
        static IRegionManager _regionManager;
        static IRegionViewRegistry _regionViewRegistry;
    }
}
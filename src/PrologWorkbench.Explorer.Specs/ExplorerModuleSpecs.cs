using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Explorer.Resources;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;

namespace PrologWorkbench.Explorer.Specs
{
    [Subject(typeof(ExplorerModule))]
    internal class When_creating_explorer_module : WithFakes
    {
        Establish context = () =>
            {
                _container = new UnityContainer();
                _regionViewRegistry = An<IRegionViewRegistry>();
                _container.RegisterInstance(_regionViewRegistry);

                _regionManager = new RegionManager();

                _subject = new ExplorerModule(_container, _regionManager);
                var locator = new UnityServiceLocator(_container);
                ServiceLocator.SetLocatorProvider(() => locator);
            };

        Because of = () => _subject.Initialize();


        It should_register_explorer_module = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IWorkbenchModule) && x.Name.Equals(ExplorerModule.TagExplorerModule));

        It should_register_instructions_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(InstructionsViewModel) && x.MappedToType == typeof(InstructionsViewModel));

        It should_register_explorer_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(ExplorerViewModel) && x.MappedToType == typeof(ExplorerViewModel));

        It should_register_program_info_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(ProgramInfoViewModel) && x.MappedToType == typeof(ProgramInfoViewModel));

        It should_register_explorer_view = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(ExplorerView) && x.MappedToType == typeof(ExplorerView));

        It should_register_instructions_region = () => _regionManager.Regions.ContainsRegionWithName(ExplorerModule.TagInstructionsRegion);
        It should_register_listings_region = () => _regionManager.Regions.ContainsRegionWithName(ExplorerModule.TagListingsRegion);

        It should_have_an_icon = () => _subject.Icon.ShouldNotBeNull();
        It should_have_a_title = () => _subject.Title.ShouldEqual(Strings.ExplorerModule_Title);
        It should_have_a_view = () => _subject.View.ShouldNotBeNull();

        static IUnityContainer _container;
        static ExplorerModule _subject;
        static IRegionManager _regionManager;
        static IRegionViewRegistry _regionViewRegistry;
    }
}
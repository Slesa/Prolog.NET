using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Debugger.Resources;
using PrologWorkbench.Debugger.ViewModels;
using PrologWorkbench.Debugger.Views;

namespace PrologWorkbench.Debugger.Specs
{
    [Subject(typeof (DebuggerModule))]
    internal class When_creating_debugger_module : WithFakes
    {
        Establish context = () =>
            {
                _container = new UnityContainer();
                _regionViewRegistry = An<IRegionViewRegistry>();
                _container.RegisterInstance(_regionViewRegistry);
                _container.RegisterInstance(An<IProvideMachine>());

                _regionManager = new RegionManager();

                _subject = new DebuggerModule(_container, _regionManager);
                var locator = new UnityServiceLocator(_container);
                ServiceLocator.SetLocatorProvider(() => locator);
            };

        Because of = () => _subject.Initialize();

        It should_register_debugger_module = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IWorkbenchModule) && x.Name.Equals(DebuggerModule.TagDebuggerModule));

        It should_register_arguments_variable_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(ArgumentsVariableListViewModel) && x.MappedToType == typeof(ArgumentsVariableListViewModel));

        It should_register_temporary_variable_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(TemporaryVariableListViewModel) && x.MappedToType == typeof(TemporaryVariableListViewModel));

        It should_register_permanent_variable_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(PermanentVariablesListViewModel) && x.MappedToType == typeof(PermanentVariablesListViewModel));

        It should_register_arguments_variable_view = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(VariableListView) && x.MappedToType == typeof(VariableListView) && x.Name.Equals(DebuggerModule.TagVarArgumentsView));

        It should_register_temporary_variable_view = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(VariableListView) && x.MappedToType == typeof(VariableListView) && x.Name.Equals(DebuggerModule.TagVarTemporariesView));

        It should_register_permanent_variable_view = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(VariableListView) && x.MappedToType == typeof(VariableListView) && x.Name.Equals(DebuggerModule.TagVarPermanentsView));

        It should_register_var_arguments_region = () => _regionManager.Regions.ContainsRegionWithName(DebuggerModule.TagVarArgumentsRegion);
        It should_register_var_permanents_region = () => _regionManager.Regions.ContainsRegionWithName(DebuggerModule.TagVarPermanentsRegion);
        It should_register_var_temporaries_region = () => _regionManager.Regions.ContainsRegionWithName(DebuggerModule.TagVarTemporariesRegion);

        It should_have_an_icon = () => _subject.Icon.ShouldNotBeNull();
        It should_have_a_title = () => _subject.Title.ShouldEqual(Strings.DebuggerModule_Title);
        It should_have_a_view = () => _subject.View.ShouldNotBeNull();

        static IUnityContainer _container;
        static DebuggerModule _subject;
        static IRegionManager _regionManager;
        static IRegionViewRegistry _regionViewRegistry;
    }
}
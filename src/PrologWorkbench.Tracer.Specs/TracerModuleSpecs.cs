using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Tracer.Resources;
using PrologWorkbench.Tracer.ViewModels;
using PrologWorkbench.Tracer.Views;

namespace PrologWorkbench.Tracer.Specs
{
    [Subject(typeof(TracerModule))]
    public class When_creating_trace_module : WithFakes
    {
        Establish context = () =>
            {
                _container = new UnityContainer();
                _regionViewRegistry = An<IRegionViewRegistry>();
                _container.RegisterInstance(_regionViewRegistry);

                _regionManager = new RegionManager();

                _tracerModule = new TracerModule(_container, _regionManager);
                var locator = new UnityServiceLocator(_container);
                ServiceLocator.SetLocatorProvider(() => locator);
            };

        Because of = () => _tracerModule.Initialize();

        It should_register_tracer_viewmodel = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(TraceViewModel) && x.MappedToType == typeof(TraceViewModel));

        It should_register_tracer_view = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(TraceView) && x.MappedToType == typeof(TraceView));

        It should_register_tracer_module = () =>
            _container.Registrations.ShouldContain(
                x =>
                x.RegisteredType == typeof(IWorkbenchModule) && x.Name.Equals(TracerModule.TagTracerModule));

        It should_have_an_icon = () => _tracerModule.Icon.ShouldNotBeNull();
        It should_have_a_title = () => _tracerModule.Title.ShouldEqual(Strings.TracerModule_Title);
        It should_have_a_view = () => _tracerModule.View.ShouldNotBeNull();

        static IUnityContainer _container;
        static TracerModule _tracerModule;
        static IRegionManager _regionManager;
        static IRegionViewRegistry _regionViewRegistry;
    }
}
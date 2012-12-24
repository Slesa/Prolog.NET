using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.ViewModels;
using PrologWorkbench.Core.Views;

namespace PrologWorkbench.Core.Specs.Views
{
    [Subject(typeof(ModulesView))]
    public class When_creating_model_view : WithFakes
    {
        Establish context = () =>
                            {
                                _viewModel = new ModulesViewModel(An<IUnityContainer>(), An<IRegionManager>());
                            };

        Because of = () => _subject = new ModulesView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static ModulesView _subject;
        static ModulesViewModel _viewModel;
    }
}
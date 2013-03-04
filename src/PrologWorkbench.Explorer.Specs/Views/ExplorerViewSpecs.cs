using Machine.Fakes;
using Machine.Specifications;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;

namespace PrologWorkbench.Explorer.Specs.Views
{
    [Subject(typeof(ExplorerView))]
    public class When_creating_explorer_view : WithFakes
    {
        Establish context = () =>
            {
                _viewModel = new ExplorerViewModel();
            };

        Because of = () => _subject = new ExplorerView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static ExplorerView _subject;
        static ExplorerViewModel _viewModel;
    }
}
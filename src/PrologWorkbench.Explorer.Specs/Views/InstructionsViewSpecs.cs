using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;
using PrologWorkbench.Explorer.Events;

namespace PrologWorkbench.Explorer.Specs.Views
{
    [Subject(typeof(InstructionsView))]
    public class When_creating_instructions_view : WithFakes
    {
        Establish context = () =>
            {
                var e = An<IEventAggregator>();
                e.WhenToldTo(x => x.GetEvent<ExplorerClauseChangedEvent>()).Return(() => new ExplorerClauseChangedEvent());
                _viewModel = new InstructionsViewModel(e);
            };

        Because of = () => _subject = new InstructionsView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static InstructionsView _subject;
        static InstructionsViewModel _viewModel;
    }
}
using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.ViewModels;
using PrologWorkbench.Core.Views;

namespace PrologWorkbench.Core.Specs.Views
{
    [Subject(typeof(ProgramView))]
    public class When_creating_program_view : WithFakes
    {
        Establish context = () =>
                            {
                                _viewModel = new ProgramViewModel(An<IProvideProgram>(), An<IEventAggregator>());
                            };

        Because of = () => _subject = new ProgramView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static ProgramView _subject;
        static ProgramViewModel _viewModel;         
    }
}
using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Explorer.ViewModels;
using PrologWorkbench.Explorer.Views;

namespace PrologWorkbench.Program.Specs.Views
{
    [Subject(typeof(ProgramInfoView))]
    public class When_creating_program_view : WithFakes
    {
        Establish context = () =>
            {
                _viewModel = new ProgramInfoViewModel(An<IProvideProgram>(), An<IEventAggregator>());
            };

        Because of = () => _subject = new ProgramInfoView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static ProgramInfoView _subject;
        static ProgramInfoViewModel _viewModel;         
    }
}
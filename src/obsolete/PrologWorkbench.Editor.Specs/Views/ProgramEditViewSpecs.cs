using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Editor.ViewModels;
using PrologWorkbench.Editor.Views;

namespace PrologWorkbench.Editor.Specs.Views
{
    [Subject(typeof(ProgramEditView))]
    public class When_creating_program_view : WithFakes
    {
        Establish context = () =>
            {
                _viewModel = new ProgramEditViewModel(An<IProvideProgram>(), An<IEventAggregator>());
            };

        Because of = () => _subject = new ProgramEditView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static ProgramEditView _subject;
        static ProgramEditViewModel _viewModel;         
    }
}
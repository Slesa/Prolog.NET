using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Events;
using PrologWorkbench.Core.ViewModels;
using PrologWorkbench.Core.Views;

namespace PrologWorkbench.Core.Specs.Views
{
    [Subject(typeof(StatusBarView))]
    public class When_creating_status_bar_view : WithFakes
    {
        Establish context = () =>
                            {
                                var updateEvent = new UpdateStatusEvent();
                                var eventAggregator = An<IEventAggregator>();
                                eventAggregator.WhenToldTo(x=>x.GetEvent<UpdateStatusEvent>()).Return(updateEvent);
                                _viewModel = new StatusBarViewModel(eventAggregator);
                            };

        Because of = () => _subject = new StatusBarView(_viewModel);

        It should_have_set_datacontext = () => _subject.DataContext.ShouldNotBeNull();

        static StatusBarView _subject;
        static StatusBarViewModel _viewModel;
    }
}
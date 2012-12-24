using Machine.Fakes;
using Machine.Specifications;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Events;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs.Models
{
    [Subject(typeof(StatusUpdateProvider))]
    public class When_calling_publish_of_update_provider : WithFakes
    {
        Establish context = () =>
                            {
                                _updateEvent = An<UpdateStatusEvent>();
                                _eventAggregator = An<IEventAggregator>();
                                _eventAggregator.WhenToldTo(x=>x.GetEvent<UpdateStatusEvent>()).Return(_updateEvent);
                                _updateEvent.WhenToldTo(x=>x.Publish(Param<string>.IsNotNull)).Callback<string>(x=>_result = x);
                                _subject = new StatusUpdateProvider(_eventAggregator);
                            };

        Because of = () => _subject.Publish(Message);

        It should_create_update_event = () => _result.ShouldEqual(Message);

        static StatusUpdateProvider _subject;
        static IEventAggregator _eventAggregator;
        static UpdateStatusEvent _updateEvent;
        static string _result;
        const string Message = "Message";
    }
}
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Events;

namespace PrologWorkbench.Core.Models
{
    public class StatusUpdateProvider : IProvideStatusUpdates
    {
        readonly IEventAggregator _eventAggregator;

        public StatusUpdateProvider(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Publish(string message)
        {
            _eventAggregator.GetEvent<UpdateStatusEvent>().Publish(message);
        }
    }
}
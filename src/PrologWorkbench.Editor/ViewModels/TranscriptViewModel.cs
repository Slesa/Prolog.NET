using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Events;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Editor.ViewModels
{
    public class TranscriptViewModel
    {
        readonly IEventAggregator _eventAggregator;

        [Dependency]
        public IProvideTranscript TranscriptProvider { get; set; }

        public TranscriptViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CopyTranscriptCommand = new DelegateCommand(OnCopyTranscript, CanCopyTranscript);
        }

        public string Title { get { return Resources.Strings.TranscriptViewModel_Title; } }

        public ObservableCollection<TranscriptEntry> Transcript { get { return TranscriptProvider.Transcript; } }
        public TranscriptEntry CurrentTranscript { get; set; }

        public DelegateCommand CopyTranscriptCommand { get; private set; }

        bool CanCopyTranscript()
        {
            return CurrentTranscript != null;
        }

        void OnCopyTranscript()
        {
            if (!CanCopyTranscript()) return;
            _eventAggregator.GetEvent<SetCurrentInputEvent>().Publish(CurrentTranscript.Text);
        }

    }
}
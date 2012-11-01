using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Editor.ViewModels
{
    public class TranscriptViewModel
    {
        [Dependency]
        public IProvideTranscript TranscriptProvider { get; set; }

        public string Title { get { return Resources.Strings.TranscriptViewModel_Title; } }

        public ObservableCollection<TranscriptEntry> Transcript { get { return TranscriptProvider.Transcript; } } 
    }
}
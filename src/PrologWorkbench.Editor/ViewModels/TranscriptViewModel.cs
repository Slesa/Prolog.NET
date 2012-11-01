using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Editor.ViewModels
{
    public class TranscriptViewModel
    {
        [Dependency]
        public IProvideTranscript TranscriptProvider { get; set; }

        public Transcript Transcript { get { return TranscriptProvider.Transcript; } } 
    }
}
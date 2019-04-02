using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.Models
{
    public class TranscriptProvider : IProvideTranscript
    {
        public TranscriptProvider()
        {
            Transcript = new Transcript();
        }

        public Transcript Transcript { get; private set; }
        public void Reset()
        {
            Transcript.Clear();
        }
    }
}
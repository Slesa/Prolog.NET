using Machine.Fakes;
using Machine.Specifications;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs.Models
{
    [Subject(typeof(TranscriptProvider))]
    public class When_creating_new_transcript_provider : WithSubject<TranscriptProvider>
    {
        It should_have_transcipt = () => Subject.Transcript.ShouldNotBeNull();
    }

    [Subject(typeof(TranscriptProvider))]
    public class When_resetting_transcript_provider : WithSubject<TranscriptProvider>
    {
        Establish context = () => Subject.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Request, "Anything");

        Because of = () => Subject.Reset();

        It should_reset_transcipt = () => Subject.Transcript.Count.ShouldEqual(0);
    }
}
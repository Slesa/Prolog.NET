using Machine.Fakes;
using Machine.Specifications;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs.Models
{
    [Subject(typeof(Transcript))]
    public class When_adding_transcript_element : WithSubject<Transcript>
    {
        Establish context = () =>
                            {
                                Subject.CollectionChanged += (sender, args) => _called = true;
                            };

        Because of = () => Subject.AddTranscriptEntry(TranscriptEntryTypes.Request, "A text");

        It should_add_element_to_list = () => Subject.Count.ShouldEqual(1);
        It should_mark_list_as_changed = () => _called.ShouldBeTrue();

        static Transcript _subject;
        static bool _called;
    }
}
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using Prolog;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs.Models
{
    [Subject(typeof(ProgramProvider))]
    internal class When_initialy_using_program_provider : ProgramProviderSpecBase
    {
        Because of = () => _program = Subject.Program;
        It should_have_no_program = () => _program.ShouldNotBeNull();
        static Program _program;
    }


    [Subject(typeof(ProgramProvider))]
    internal class When_setting_program_file : ProgramProviderSpecBase
    {
        Establish context = () =>
            {
                _program = new Program();
                Subject.ProgramChanged += (s, e) =>
                    {
                        _handlerCalled = true;
                        _catchedProgram = e.Program;
                    };
            };
        Because of = () => Subject.Program = _program;
        It should_set_new_program = () => Subject.Program.ShouldBeTheSameAs(_program);
        It should_call_handler = () => _handlerCalled.ShouldBeTrue();
        It should_call_with_new_program = () => _catchedProgram.ShouldBeTheSameAs(_program);
        static Program _program;
        static bool _handlerCalled;
        static Program _catchedProgram;
    }


    [Subject(typeof(ProgramProvider))]
    internal class When_resetting_empty_program_file : ProgramProviderSpecBase
    {
        Establish context = () =>
            {
                Subject.ProgramChanged += (s,e) => _handlerCalled = true;
            };
        Because of = () => Subject.Reset();
        It should_reset_program = () => Subject.Program.ShouldNotBeNull();
        It should_call_handler = () => _handlerCalled.ShouldBeTrue();
        static Program _program;
        static bool _handlerCalled;
    }


    [Subject(typeof(ProgramProvider))]
    internal class When_resetting_program_file : ProgramProviderSpecBase
    {
        Establish context = () =>
            {
                _program = new Program();
                SetModified(_program);
                Subject.Program = _program;
                Subject.ProgramChanged += (s,e) => _handlerCalled = true;
            };
        Because of = () => Subject.Reset();
        It should_reset_program = () => Subject.Program.IsModified.ShouldBeFalse();
        It should_create_new_program = () => Subject.Program.ShouldNotEqual(_program);
        It should_call_handler = () => _handlerCalled.ShouldBeTrue();
        static Program _program;
        static bool _handlerCalled;
    }



    internal class ProgramProviderSpecBase : WithFakes
    {
        Establish context = () => Subject = new ProgramProvider();

        protected static void SetModified(Program program)
        {
            var t = program.GetType();
            //t.GetProperty("IsModified", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t.InvokeMember("IsModified",
                           BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty |
                           BindingFlags.Instance, null, program, new object[] { true });

        }

        protected const string SourceFilename = @"Resources\test.prolog";
        protected static ProgramProvider Subject;
    }
}

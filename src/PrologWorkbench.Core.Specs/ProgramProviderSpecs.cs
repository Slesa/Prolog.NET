using System;
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using Prolog;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs
{
    [Subject(typeof(ProgramProvider))]
    internal class When_initialy_using_program_provider : ProgramProviderSpecBase
    {
        Because of = () => _program = Subject.Program;
        It should_have_no_program = () => _program.ShouldBeNull();
        static Program _program;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_resetting_program_file : ProgramProviderSaveSpecBase
    {
        Because of = () => Subject.Reset();
        It should_reset_program = () => Subject.Program.IsModified.ShouldBeFalse();
    }


    internal class ProgramProviderSaveSpecBase : ProgramProviderSpecBase
    {
        Establish context = () =>
                                {
                                    //Subject.Load(SourceFilename);
                                    SetModified();
                                    //Subject.Program.IsModified = true;
                                    //Program.GetProperty("IsModified").SetValue(Subject, true, null);
                                };

        Cleanup shutdown = () =>
                               {
                                   if (System.IO.File.Exists(DestinationFilename))
                                       System.IO.File.Delete(DestinationFilename);
                               };
        static void SetModified()
        {
            Type t = Subject.Program.GetType();
            //t.GetProperty("IsModified", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t.InvokeMember("IsModified",
                           BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty |
                           BindingFlags.Instance, null, Subject.Program, new object[] {true});

        }
    }



    internal class ProgramProviderSpecBase : WithFakes
    {
        Establish context = () => Subject = new ProgramProvider();

        protected const string SourceFilename = @"Resources\test.prolog";
        protected const string DestinationFilename = @"Resources\tmp.prolog";
        protected static IProvideProgram Subject;
    }
}

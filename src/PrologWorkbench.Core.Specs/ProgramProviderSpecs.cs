using System;
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using Prolog;

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
    internal class When_loading_program_file_from_not_existing_file : ProgramProviderSpecBase
    {
        Because of = () => _result = Subject.Load("notexisting.pl");
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_loading_program_file_from_not_existing_directory : ProgramProviderSpecBase
    {
        Because of = () => _result = Subject.Load(@"notexisting\file.pl");
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_loading_program_file : ProgramProviderSpecBase
    {
        Because of = () => _result = Subject.Load(SourceFilename);
        It should_load_program = () => _result.ShouldBeTrue();
        It should_have_initialized_program = () => Subject.Program.ShouldNotBeNull();
        static bool _result;
    }


    [Subject(typeof(ProgramProvider))]
    internal class When_saving_program_file_without_program : ProgramProviderSpecBase
    {
        // Use standard SpecBase, as SaveSpecBase loads a program
        Because of = () => _result = Subject.Save(SourceFilename);
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_saving_program_file_without_filename : ProgramProviderSaveSpecBase
    {
        Because of = () => _result = Subject.Save("");
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_saving_program_file_to_not_existing_directory : ProgramProviderSaveSpecBase
    {
        Because of = () => _result = Subject.Save(@"notexisting\file.pl");
        It should_work = () => _result.ShouldBeFalse();
        static bool _result;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_saving_unmodified_program_file : ProgramProviderSpecBase
    {
        // Use standard SpecBase, as SaveSpecBase loads a program
        Establish context = () => Subject.Load(SourceFilename);
        Because of = () => _result = Subject.Save(DestinationFilename);
        It should_save_program = () => _result.ShouldBeTrue();
        It should_not_create_program_file = () => System.IO.File.Exists(DestinationFilename).ShouldBeFalse();
        static bool _result;
    }

    [Subject(typeof(ProgramProvider))]
    internal class When_saving_modified_program_file : ProgramProviderSaveSpecBase
    {
        Because of = () => _result = Subject.Save(DestinationFilename);
        It should_load_program = () => _result.ShouldBeTrue();
        It should_have_initialized_program = () => Subject.Program.ShouldNotBeNull();
        It should_rename_prolog_program = () => Subject.Program.FileName.ShouldEqual(DestinationFilename);
        static bool _result;
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
                                    Subject.Load(SourceFilename);
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

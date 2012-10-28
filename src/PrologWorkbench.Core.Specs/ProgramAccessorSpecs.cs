using System;
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using Prolog;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs
{
    [Subject(typeof(ProgramAccessor))]
    internal class When_loading_program_file_from_not_existing_file : ProgramAccessorSpecBase
    {
        Because of = () => _program = Subject.Load("notexisting.pl");
        It should_not_work = () => _program.ShouldBeNull();
        static Program _program;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_loading_program_file_from_not_existing_directory : ProgramAccessorSpecBase
    {
        Because of = () => _program = Subject.Load(@"notexisting\file.pl");
        It should_not_work = () => _program.ShouldBeNull();
        static Program _program;
    }


    [Subject(typeof (ProgramAccessor))]
    class When_loading_program_file : ProgramAccessorSpecBase
    {
        Because of = () => _program = Subject.Load(SourceFilename);
        It should_load_program = () => _program.ShouldNotBeNull();
        static Program _program;
    }


    [Subject(typeof (ProgramAccessor))]
    class When_saving_program_file_without_program : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Save(SourceFilename, null);
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof (ProgramAccessor))]
    class When_saving_program_file_without_filename : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Save("", null);
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof (ProgramAccessor))]
    class When_saving_program_file_to_not_existing_directory : ProgramAccessorSpecBase
    {
        Establish context = () =>
            {
                _program = new Program();
                SetModified(_program);
            };
        Because of = () => _result = Subject.Save(@"notexisting\file.pl", _program);
        It should_work = () => _result.ShouldBeFalse();
        static bool _result;
        static Program _program;
    }


    [Subject(typeof (ProgramAccessor))]
    class When_saving_unmodified_program_file : ProgramAccessorSpecBase
    {
        Establish context = () => _program = Subject.Load(SourceFilename);
        Because of = () => _result = Subject.Save(DestinationFilename, _program);
        It should_save_program = () => _result.ShouldBeTrue();
        It should_not_create_program_file = () => System.IO.File.Exists(DestinationFilename).ShouldBeFalse();
        static bool _result;
        static Program _program;
    }


    [Subject(typeof (ProgramAccessor))]
    class When_saving_modified_program_file : ProgramAccessorSpecBase
    {
        Establish context = () =>
            {
                _program = new Program();
                SetModified(_program);
            };
        Because of = () => _result = Subject.Save(DestinationFilename, _program);
        It should_load_program = () => _result.ShouldBeTrue();
        It should_rename_prolog_program = () => _program.FileName.ShouldEqual(DestinationFilename);
        static bool _result;
        static Program _program;
    }



    internal class ProgramAccessorSpecBase : WithSubject<ProgramAccessor>
    {
        Cleanup shutdown = () =>
            {
                if (System.IO.File.Exists(DestinationFilename))
                    System.IO.File.Delete(DestinationFilename);
            };

        protected static void SetModified(Program program)
        {
            var t = program.GetType();
            //t.GetProperty("IsModified", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t.InvokeMember("IsModified",
                           BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty |
                           BindingFlags.Instance, null, program, new object[] { true });

        }

        protected const string SourceFilename = @"Resources\test.prolog";
        protected const string DestinationFilename = @"Resources\tmp.prolog";
    }
}
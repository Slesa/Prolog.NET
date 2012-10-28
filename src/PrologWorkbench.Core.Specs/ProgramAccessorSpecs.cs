using Machine.Fakes;
using Machine.Specifications;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Core.Specs
{
    [Subject(typeof(ProgramAccessor))]
    internal class When_loading_program_file_from_not_existing_file : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Load("notexisting.pl");
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_loading_program_file_from_not_existing_directory : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Load(@"notexisting\file.pl");
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_loading_program_file : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Load(SourceFilename);
        It should_load_program = () => _result.ShouldBeTrue();
        It should_have_initialized_program = () => ProgramProvider.Program.ShouldNotBeNull();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_saving_program_file_without_program : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Save(SourceFilename);
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_saving_program_file_without_filename : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Save("");
        It should_not_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_saving_program_file_to_not_existing_directory : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Save(@"notexisting\file.pl");
        It should_work = () => _result.ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_saving_unmodified_program_file : ProgramAccessorSpecBase
    {
        Establish context = () => Subject.Load(SourceFilename);
        Because of = () => _result = Subject.Save(DestinationFilename);
        It should_save_program = () => _result.ShouldBeTrue();
        It should_not_create_program_file = () => System.IO.File.Exists(DestinationFilename).ShouldBeFalse();
        static bool _result;
    }


    [Subject(typeof(ProgramAccessor))]
    internal class When_saving_modified_program_file : ProgramAccessorSpecBase
    {
        Because of = () => _result = Subject.Save(DestinationFilename);
        It should_load_program = () => _result.ShouldBeTrue();
        It should_have_initialized_program = () => ProgramProvider.Program.ShouldNotBeNull();
        It should_rename_prolog_program = () => ProgramProvider.Program.FileName.ShouldEqual(DestinationFilename);
        static bool _result;
    }



    internal class ProgramAccessorSpecBase : WithFakes
    {
        Establish context = () =>
            {
                ProgramProvider = new ProgramProvider();
                Subject = new ProgramAccessor(ProgramProvider);
            };
        protected const string SourceFilename = @"Resources\test.prolog";
        protected const string DestinationFilename = @"Resources\tmp.prolog";
        protected static IProvideProgram ProgramProvider;
        protected static ProgramAccessor Subject;
    }
}
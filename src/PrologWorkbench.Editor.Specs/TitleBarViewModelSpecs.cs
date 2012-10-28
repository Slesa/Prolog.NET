using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using PrologWorkbench.Core;
using PrologWorkbench.Core.Models;
using PrologWorkbench.Editor.Helpers;
using PrologWorkbench.Editor.ViewModels;

namespace PrologWorkbench.Editor.Specs
{
    [Subject(typeof(TitleBarViewModel))]
    internal class When_nothing_in_titlebar_viewmodel_was_called : TitleBarViewModelSpecBase
    {
        It should_allow_new_program = () => Subject.NewCommand.CanExecute().ShouldBeTrue();
        It should_allow_load_program = () => Subject.LoadCommand.CanExecute().ShouldBeTrue();
        It should_allow_close_program = () => Subject.CloseCommand.CanExecute().ShouldBeFalse();
        It should_allow_save_program = () => Subject.SaveCommand.CanExecute().ShouldBeFalse();
        It should_allow_save_as_program = () => Subject.SaveAsCommand.CanExecute().ShouldBeFalse();
        It should_allow_quit_program = () => Subject.ExitCommand.CanExecute().ShouldBeTrue();
    }


    [Subject(typeof(TitleBarViewModel))]
    internal class When_calling_new_program : TitleBarViewModelSpecBase
    {
        Establish context = () =>
                                {
                                    Subject.CloseCommand.CanExecuteChanged += (s, a) => _closeChanged = true;
                                    Subject.SaveCommand.CanExecuteChanged += (s, a) => _saveChanged = true;
                                    Subject.SaveAsCommand.CanExecuteChanged += (s, a) => _saveAsChanged = true;
                                };

        Because of = () => Subject.NewCommand.Execute(); 

        It should_allow_new_program = () => Subject.NewCommand.CanExecute().ShouldBeTrue();
        It should_allow_load_program = () => Subject.LoadCommand.CanExecute().ShouldBeTrue();
        It should_allow_close_program = () => Subject.CloseCommand.CanExecute().ShouldBeTrue();
        It should_forbid_save_program = () => Subject.SaveCommand.CanExecute().ShouldBeFalse();
        It should_allow_save_as_program = () => Subject.SaveAsCommand.CanExecute().ShouldBeTrue();
        It should_allow_quit_program = () => Subject.ExitCommand.CanExecute().ShouldBeTrue();
        It should_change_can_close = () => _closeChanged.ShouldBeTrue();
        It should_not_allow_can_save = () => _saveChanged.ShouldBeFalse();
        It should_change_can_save_as = () => _saveAsChanged.ShouldBeTrue();

        static bool _closeChanged;
        static bool _saveChanged;
        static bool _saveAsChanged;
    }


    [Subject(typeof(TitleBarViewModel))]
    internal class When_calling_load_program : TitleBarViewModelSpecBase
    {
        Establish context = () =>
                                {
                                    Subject.CloseCommand.CanExecuteChanged += (s, a) => _closeChanged = true;
                                    Subject.SaveCommand.CanExecuteChanged += (s, a) => _saveChanged = true;
                                    Subject.SaveAsCommand.CanExecuteChanged += (s, a) => _saveAsChanged = true;
                                    Subject.FilenameProvider = LoadFilenameProvider;
                                };

        Because of = () => Subject.LoadCommand.Execute(); 

        It should_allow_new_program = () => Subject.NewCommand.CanExecute().ShouldBeTrue();
        It should_allow_load_program = () => Subject.LoadCommand.CanExecute().ShouldBeTrue();
        It should_allow_close_program = () => Subject.CloseCommand.CanExecute().ShouldBeTrue();
        It should_forbid_save_program = () => Subject.SaveCommand.CanExecute().ShouldBeFalse();
        It should_allow_save_as_program = () => Subject.SaveAsCommand.CanExecute().ShouldBeTrue();
        It should_allow_quit_program = () => Subject.ExitCommand.CanExecute().ShouldBeTrue();
        It should_change_can_close = () => _closeChanged.ShouldBeTrue();
        It should_not_allow_can_save = () => _saveChanged.ShouldBeFalse();
        It should_change_can_save_as = () => _saveAsChanged.ShouldBeTrue();

        static bool _closeChanged;
        static bool _saveChanged;
        static bool _saveAsChanged;
    }




    internal class TitleBarViewModelSpecBase : WithFakes
    {
        Establish context = () =>
                                {
                                    Subject = new TitleBarViewModel {ProgramProvider = new ProgramProvider()};
                                    CalledProperties = new List<string>();
                                };

        protected static IProvideFilename LoadFilenameProvider
        {
            get
            {
                var filenameProvider = An<IProvideFilename>();
                filenameProvider.WhenToldTo(x => x.GetLoadFileName()).Return(LoadFilename);
                return filenameProvider;
            }
        }

        protected static TitleBarViewModel Subject;
        protected static List<string> CalledProperties { get; set; }
        const string LoadFilename = @"Resources\test.prolog";
        protected const string SaveFilename = @"Resources\tmp.prolog";
    }
}

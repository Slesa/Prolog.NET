using System.IO;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Editor.Helpers;

namespace PrologWorkbench.Editor.ViewModels
{
    public class TitleBarViewModel 
    {
        [Dependency]
        public ILoadOrSaveProgram ProgramAccessor { get; set; }
        [Dependency]
        public IProvideProgram ProgramProvider { get; set; }
        [Dependency]
        public IProvideFilename FilenameProvider { get; set; }

        public TitleBarViewModel()
        {
            NewCommand = new DelegateCommand(OnNew);
            LoadCommand = new DelegateCommand(OnLoad);
            CloseCommand = new DelegateCommand(OnClose, CanClose);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            SaveAsCommand = new DelegateCommand(OnSaveAs, CanSaveAs);
            ExitCommand = new DelegateCommand(OnExit);
        }

        public string ApplicationName
        {
            get { return "Prolog.NET Workbench"; }
        }

        public string ApplicationIcon
        {
            get { return "/PrologWorkbench;component/Resources/ApplicationIcon.ico"; }
        }

        public DelegateCommand NewCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand SaveAsCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }


        void OnNew()
        {
            if (!EnsureSaved()) return;
            ProgramProvider.Reset();
            CloseCommand.RaiseCanExecuteChanged();
            SaveAsCommand.RaiseCanExecuteChanged();
        }

        void OnLoad()
        {
            if (!EnsureSaved()) return;
            if (!Load()) return;
            CloseCommand.RaiseCanExecuteChanged();
            SaveAsCommand.RaiseCanExecuteChanged();
        }

        void OnClose()
        {
            if (!EnsureSaved()) return;
            ProgramProvider.Reset();
            CloseCommand.RaiseCanExecuteChanged();
            SaveAsCommand.RaiseCanExecuteChanged();
        }

        bool CanClose()
        {
            return ProgramProvider.Program != null;
        } 

        void OnSave()
        {
            if (ProgramProvider.Program != null) Save();
        }

        bool CanSave()
        {
            return ProgramProvider.Program != null && !string.IsNullOrEmpty(ProgramProvider.Program.FileName) && ProgramProvider.Program.IsModified;
        }

        void OnSaveAs()
        {
            if (ProgramProvider.Program != null) SaveAs();
        }

        bool CanSaveAs()
        {
            return ProgramProvider.Program != null;
        }

        void OnExit()
        {
            Application.Current.Shutdown();
            //Close();
        }

        bool EnsureSaved()
        {
            if (ProgramProvider.Program == null || ProgramProvider.Program.IsModified == false)
            {
                return true;
            }

            var fileName = "Untitled";
            if (ProgramProvider.Program != null && !string.IsNullOrEmpty(ProgramProvider.Program.FileName))
            {
                fileName = Path.GetFileName(ProgramProvider.Program.FileName);
            }
            var title = string.Format("Do you want to save {0}?", fileName);
            var filename = FilenameProvider.GetSaveFileName(title, fileName);

            return !string.IsNullOrEmpty(filename) && Save();
        }

        bool Load()
        {
            var filename = FilenameProvider.GetLoadFileName();
            if (string.IsNullOrEmpty(filename)) return false;
            ProgramProvider.Program = ProgramAccessor.Load(filename);
            return true;
        }

        bool Save()
        {
            if (ProgramProvider.Program == null) return true;
            return string.IsNullOrEmpty(ProgramProvider.Program.FileName) 
                ? SaveAs()
                : ProgramAccessor.Save(ProgramProvider.Program.FileName, ProgramProvider.Program);
        }

        bool SaveAs()
        {
            if (ProgramProvider.Program == null) return true;
            var filename = FilenameProvider.GetSaveFileName("Save program as...");
            return !string.IsNullOrEmpty(filename) && ProgramAccessor.Save(filename, ProgramProvider.Program);
        }
    }
}
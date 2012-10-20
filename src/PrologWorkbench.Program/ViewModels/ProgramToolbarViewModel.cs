using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using PrologWorkbench.Core;

namespace PrologWorkbench.Program.ViewModels
{
    public class ProgramToolbarViewModel
    {
        const string FileExtesion = "prolog";
        const string FileFilter = "Prolog Source Files|*.prolog|All Files|*.*";

        readonly IProvideProgram _programProvider;

        public ProgramToolbarViewModel(IProvideProgram programProvider)
        {
            _programProvider = programProvider;

            NewCommand = new DelegateCommand(OnNew);
            LoadCommand = new DelegateCommand(OnLoad);
            CloseCommand = new DelegateCommand(OnClose, CanClose);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            SaveAsCommand = new DelegateCommand(OnSaveAs, CanSaveAs);
            ExitCommand = new DelegateCommand(OnExit);
        }

        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }


        void OnNew()
        {
            if (!EnsureSaved()) return;
            _programProvider.Reset();            
        }

        void OnLoad()
        {
            if (!EnsureSaved()) return;
            Load();
        }

        void OnClose()
        {
            if (!EnsureSaved()) return;
            _programProvider.Reset();
        }

        bool CanClose()
        {
            return _programProvider.Program != null;
        }

        void OnSave()
        {
            if (_programProvider.Program != null) Save();
        }

        bool CanSave()
        {
            return _programProvider.Program != null && !string.IsNullOrEmpty(_programProvider.Program.FileName);
        }

        void OnSaveAs()
        {
            if (_programProvider.Program != null) SaveAs();
        }

        bool CanSaveAs()
        {
            return _programProvider.Program != null;
        }

        void OnExit()
        {
            Application.Current.Shutdown();
            //Close();
        }

        bool EnsureSaved()
        {
            if (_programProvider.Program == null || _programProvider.Program.IsModified == false)
            {
                return true;
            }

            var fileName = "Untitled";
            if (_programProvider.Program != null && !string.IsNullOrEmpty(_programProvider.Program.FileName))
            {
                fileName = Path.GetFileName(_programProvider.Program.FileName);
            }

            var dialog = new SaveFileDialog
                             {
                                 Title = string.Format("Do you want to save {0}?", fileName),
                             };
            if (dialog.ShowDialog() == false) return false;
            return Save();
        }

        bool Load()
        {

            var dialog = new OpenFileDialog
                             {
                                 DefaultExt = FileExtesion,
                                 Filter = FileFilter,
                                 InitialDirectory = Directory.GetCurrentDirectory()
                             };
            if (dialog.ShowDialog() == false) return false;
            _programProvider.Load(dialog.FileName);
            return true;
        }

        bool Save()
        {
            if (_programProvider.Program == null) return true;
            return string.IsNullOrEmpty(_programProvider.Program.FileName) 
                ? SaveAs() 
                : _programProvider.Save(_programProvider.Program.FileName);
        }

        bool SaveAs()
        {
            if (_programProvider.Program == null) return true;

            var dialog = new SaveFileDialog
                             {
                                 DefaultExt = FileExtesion,
                                 Filter = FileFilter
                             };
            if (dialog.ShowDialog() == false) return false;
            return _programProvider.Save(dialog.FileName);
        }
    }
}
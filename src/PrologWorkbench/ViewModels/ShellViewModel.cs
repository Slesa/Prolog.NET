using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Prolog.Workbench.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        //readonly IRegionManager _regionManager;

        public ShellViewModel(/*IRegionManager regionManager*/)
        {
            //_regionManager = regionManager;
            //_regionManager.AddToRegion("ToolsBarRegion", )
        }
        
        



        bool EnsureSaved()
        {
            /*
            if (AppState.Program == null || AppState.Program.IsModified == false)
            {
                return true;
            }

            var fileName = "Untitled";
            if (AppState.Program != null && !string.IsNullOrEmpty(AppState.Program.FileName))
            {
                fileName = Path.GetFileName(AppState.Program.FileName);
            }

            var dialog = new SavePromptDialog
            {
                Message = string.Format("Do you want to save {0}?", fileName),
                //Owner = this
            };
            var result = dialog.ShowDialog();
            if (result == SavePromptDialogResults.Cancel)
            {
                return false;
            }
            if (result == SavePromptDialogResults.Save)
            {
                if (!Save(true))
                {
                    return false;
                }
                return true;
            }*/
            return true;
        }

        bool Open()
        {
            /*
            var dialog = new OpenFileDialog
            {
                DefaultExt = Properties.Resources.FileDefaultExt,
                Filter = Properties.Resources.FileFilter
            };
            if (dialog.ShowDialog() == false)
            //if (dialog.ShowDialog(this) == false)
            {
                return false;
            }
            Program program;
            try
            {
                program = Program.Load(dialog.FileName);
            }
            catch (FileNotFoundException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (IOException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }

            AppState.Program = program;
             * */
            return true;
        }

        bool Save(bool forceSave)
        {
            /*
            Debug.Assert(AppState.Program != null);
            if (AppState.Program == null)
            {
                return true;
            }

            if (!forceSave && !AppState.Program.IsModified)
            {
                return true;
            }

            if (string.IsNullOrEmpty(AppState.Program.FileName))
            {
                return SaveAs(forceSave);
            }

            try
            {
                AppState.Program.Save();
            }
            catch (FileNotFoundException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (IOException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
             * */
            return true;
        }

        bool SaveAs(bool forceSave)
        {
            /*
            Debug.Assert(AppState.Program != null);
            if (AppState.Program == null)
            {
                return true;
            }

            if (!forceSave && !AppState.Program.IsModified)
            {
                return true;
            }

            var dialog = new SaveFileDialog
            {
                DefaultExt = Properties.Resources.FileDefaultExt,
                Filter = Properties.Resources.FileFilter
            };
            //if (dialog.ShowDialog(this) == false)
            if (dialog.ShowDialog() == false)
            {
                return false;
            }

            try
            {
                AppState.Program.SaveAs(dialog.FileName);
            }
            catch (FileNotFoundException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (IOException ex)
            {
                //CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            */
            return true;
        }
    }



}
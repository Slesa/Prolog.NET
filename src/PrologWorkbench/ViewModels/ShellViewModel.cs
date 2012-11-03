using Microsoft.Practices.Prism.ViewModel;

namespace Prolog.Workbench.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        //readonly IRegionManager _regionManager;

        public ShellViewModel()
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

    }



}
using System.IO;
using Microsoft.Win32;

namespace PrologWorkbench.Program.ViewModels
{
    public class FilenameProvider : IProvideFilename
    {
        const string FileExtesion = "prolog";
        const string FileFilter = "Prolog Source Files|*.prolog|All Files|*.*";

        public string GetLoadFileName()
        {
            var dialog = new OpenFileDialog
                             {
                                 DefaultExt = FileExtesion,
                                 Filter = FileFilter,
                                 InitialDirectory = Directory.GetCurrentDirectory()
                             };
            return dialog.ShowDialog() == false ? string.Empty : dialog.FileName;
        }

        public string GetSaveFileName(string title, string fileName=null)
        {
            var dialog = new SaveFileDialog
                             {
                                 DefaultExt = FileExtesion,
                                 Filter = FileFilter,
                                 Title = title,
                                 FileName = fileName,
                             };
            return dialog.ShowDialog() == false ? string.Empty : dialog.FileName;
        }
    }
}
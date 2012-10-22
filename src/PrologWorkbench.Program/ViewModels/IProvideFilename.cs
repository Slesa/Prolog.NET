namespace PrologWorkbench.Program.ViewModels
{
    public interface IProvideFilename
    {
        string GetLoadFileName();
        string GetSaveFileName(string title, string fileName=null);
    }
}
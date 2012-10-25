namespace PrologWorkbench.Program.Helpers
{
    public interface IProvideFilename
    {
        string GetLoadFileName();
        string GetSaveFileName(string title, string fileName=null);
    }
}
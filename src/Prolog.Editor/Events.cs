namespace Prolog.Editor
{
    public class StatusMessageEvent
    {
        public string Text { get; }

        public StatusMessageEvent(string text)
        {
            Text = text;
        }
    }

    public class ResultEvent
    {
        public string Result { get; }

        public ResultEvent(string result)
        {
            Result = result;
        }
    }

    public class FileNameChangedEvent
    {
        public string FileName { get; }

        public FileNameChangedEvent(string fileName)
        {
            FileName = fileName;
        }
    }
}
namespace Transcript.Core.Model
{
    public interface ILogger
    {
        void Log(string message, bool append = false);
        void Clear();
    }
}

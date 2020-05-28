namespace Transcript.Core.Model
{
    public interface IProgressIndicator
    {
        void Busy();
        void Completed();
    }
}

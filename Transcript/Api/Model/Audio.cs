using Google.Cloud.Speech.V1;

namespace Transcript.Api.Model
{
    /// <summary>
    /// Link Google source file to desired output file
    /// </summary>
    internal class Audio
    {
        public Audio(RecognitionAudio recognitionAudio, string outputFile)
        {
            RecognitionAudio = recognitionAudio;
            OutputFile = outputFile;
        }

        public RecognitionAudio RecognitionAudio { get; set; }
        public string OutputFile { get; set; }
    }
}

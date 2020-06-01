using Google.Cloud.Speech.V1;

namespace Transcript.Api.Model
{
    /// <summary>
    /// Link Google source file to desired output file
    /// </summary>
    internal class Audio
    {
        public Audio(string inputFile, string outputFile, RecognitionAudio recognitionAudio)
        {
            InputFile = inputFile;
            OutputFile = outputFile;
            RecognitionAudio = recognitionAudio;
        }

        public string InputFile { get; set; }

        public string OutputFile { get; set; }

        public RecognitionAudio RecognitionAudio { get; set; }
    }
}

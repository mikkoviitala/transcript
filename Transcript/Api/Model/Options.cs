using System;
using Google.Cloud.Speech.V1;

namespace Transcript.Api.Model
{
    /// <summary>
    ///  SpeechToText Options
    /// </summary>
    public class Options
    {
        public string KeyPath { get; set; }

        public string Source { get; set; }

        public RecognitionConfig.Types.AudioEncoding Encoding { get; set; }

        public int? SampleRate { get; set; }

        public string LanguageCode { get; set; }

        public string Destination { get; set; }

        public override string ToString()
        {
            return $"Source:{Source}, Language:{LanguageCode}, Encoding:{Encoding}, SampleRate:{(SampleRate.HasValue ? SampleRate.Value.ToString() : "Omitted")}";
        }
    }
}

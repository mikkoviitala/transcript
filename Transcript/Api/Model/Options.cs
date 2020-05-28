using System;
using Google.Cloud.Speech.V1;
using Transcript.Core.Service;

namespace Transcript.Api.Model
{
    /// <summary>
    ///  SpeechToText Options
    /// </summary>
    public class Options
    {
        private static readonly ParameterMapper Mapper = new ParameterMapper();

        public string KeyPath { get; set; }

        public string Source { get; set; }

        public string Encoding { get; set; }

        public RecognitionConfig.Types.AudioEncoding GoogleEncoding
        {
            get
            {
                string encoding = Mapper.ToGoogleEncoding(Encoding);
                Enum.TryParse(encoding, out RecognitionConfig.Types.AudioEncoding parsed);
                return parsed;
            }
        }

        public int? SampleRate { get; set; }

        public string LanguageCode { get; set; }

        public string Destination { get; set; }

        public override string ToString()
        {
            return $"Source:{Source}, Language:{LanguageCode}, Encoding:{GoogleEncoding}, SampleRate:{(SampleRate.HasValue ? SampleRate.Value.ToString() : "Omitted")}";
        }
    }
}

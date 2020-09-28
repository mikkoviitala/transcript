using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Timers;
using Google.Cloud.Speech.V1;
using Transcript.Core.Service;

namespace Gui.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Language = Languages.First();
            Encoding = Encodings.First();
        }

        public string Version => VersionService.Version;

        public KeyValuePair<string, string> Language { get; set; }

        public KeyValuePair<RecognitionConfig.Types.AudioEncoding, string> Encoding { get; set; }

        public Dictionary<string, string> Languages { get; } = new Dictionary<string, string>
        {
            { "en-US", "English (en-US)" },
            { "fi-FI", "Finnish (fi-FI)" }
        };

        public Dictionary<RecognitionConfig.Types.AudioEncoding, string> Encodings { get; } = new Dictionary<RecognitionConfig.Types.AudioEncoding, string>
        {
            { RecognitionConfig.Types.AudioEncoding.EncodingUnspecified, "Unspecified" },
            { RecognitionConfig.Types.AudioEncoding.Amr, "Amr" },
            { RecognitionConfig.Types.AudioEncoding.AmrWb, "Amr-wb" },
            { RecognitionConfig.Types.AudioEncoding.Flac, "Flac" },
            { RecognitionConfig.Types.AudioEncoding.Linear16, "Linear16" },
            { RecognitionConfig.Types.AudioEncoding.Mulaw , "Mu-law" },
            { RecognitionConfig.Types.AudioEncoding.OggOpus, "Ogg Opus" },
            { RecognitionConfig.Types.AudioEncoding.SpeexWithHeaderByte, "Speex with header bytes" }
        };
    }
}

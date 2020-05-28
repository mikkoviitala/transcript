using System.IO;
using CommandLineParser.Arguments;
using CommandLineParser.Validation;
using Transcript.Core;

namespace Transcript.Client.Model
{
    /// <summary>
    /// Console application arguments
    /// </summary>
    [DistinctGroupsCertification("file", "url")]
    public class ApplicationArguments
    {
        private string _storageUrl;

        [SwitchArgument('k', "key", false, Description = "Create or update internal API Key from Google credentials JSON file, --password and source --file must be provided. See https://cloud.google.com/docs/authentication/production#creating_a_service_account on how to obtain the source file.")]
        public bool SetupKey { get; set; }

        [ValueArgument(typeof(string), 'p', "password", Description = "Your password for API Key encryption/decryption. Be aware that plain password is shown on screen.")]
        public string Password { get; set; }

        [ValueArgument(typeof(string), 'f', "file", Description = "Source file")]
        public string FilePath { get; set; }

        [ValueArgument(typeof(string), 'u', "url", Description = "Google Storage URL")]
        public string StorageUrl
        {
            get => _storageUrl.StartsWith("gs://") ? _storageUrl : $"gs://{_storageUrl}";
            set => _storageUrl = value;
        }

        [EnumeratedValueArgument(typeof(string), 'l', "language", AllowedValues = Constants.LanguageCodes, Description = "Audio file language code, if not provided \"en\" will be used")]
        public string LanguageCode { get; set; }

        [EnumeratedValueArgument(typeof(string), 's', "samplerate", AllowedValues = Constants.SampleRates, Description = "Audio file sample rate, if not provided it will be omitted")]
        public string SampleRate { get; set; }

        [EnumeratedValueArgument(typeof(string), 'e', "encoding", AllowedValues =  Constants.Encodings, Description = "Audio file encoding, if not provided \"unspecified\" will be used")]
        public string Encoding { get; set; }

        public bool IsValidPasswordProvided => !string.IsNullOrWhiteSpace(Password) && Password.Length >= 8;

        public string PasswordError => "--password must be provided and be at least eight (8) characters long";

        public string Key => "Google.key";

        public bool KeyExists => File.Exists(Key);

        public string KeyError => $"{Key} file not found, create or update one using --key switch";

        public bool FileProvided => !string.IsNullOrWhiteSpace(FilePath);

        public string FileError => $"--file must be provided";

        public bool StorageUrlProvided => !string.IsNullOrWhiteSpace(StorageUrl);

        public string StorageUrlError => $"--gs must be provided";
    }
}

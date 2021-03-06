﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Google.Cloud.Storage.V1;
using Transcript.Api.Model;
using Transcript.Core.Model;

namespace Transcript.Api
{
    /// <summary>
    /// Process 1...n audio files in parallel via Google Speech API and write results in separate text files.
    /// Sadly enough, Google Speech API supports only environment path based authentication...
    /// </summary>
    public class SpeechToText
    {
        private const string GoogleEnv = "GOOGLE_APPLICATION_CREDENTIALS";

        public List<Task<Result>> Transcribe(Options options)
        {
            Environment.SetEnvironmentVariable(GoogleEnv, options.KeyPath, EnvironmentVariableTarget.Process);

            var tasks = new List<Task<Result>>();

            var settings = new RecognitionConfig
            {
                EnableSeparateRecognitionPerChannel = true,
                EnableAutomaticPunctuation = true,
                LanguageCode = options.LanguageCode,
                Encoding = options.Encoding
            };

            if (options.SampleRate.HasValue)
                settings.SampleRateHertz = options.SampleRate.Value;

            var audios = FromGoogleStorage(options) ? GetStorageFiles(options) : GetLocalFiles(options);
            audios.ForEach(audio =>
            {
                var task = Task<Result>.Factory.StartNew(() =>
                {
                    try
                    {
                        File.WriteAllText(audio.OutputFile, string.Empty);

                        var speech = SpeechClient.Create();
                        var longOperation = speech.LongRunningRecognize(settings, audio.RecognitionAudio);
                        longOperation = longOperation.PollUntilCompleted();

                        var response = longOperation.Result;
                        foreach (var result in response.Results)
                        {
                            foreach (var alternative in result.Alternatives)
                                File.AppendAllText(audio.OutputFile, FormatLine(alternative));
                        }
                    }
                    catch (Exception e)
                    {
                        return new Result(audio.InputFile, false, e.Message);
                    }

                    return new Result(audio.InputFile, true);
                }, TaskCreationOptions.LongRunning);
                tasks.Add(task);
            });

            return tasks;
        }

        private List<Audio> GetLocalFiles(Options options)
        {
            string fullFilePath = Path.GetFullPath(options.Source);
            string fullPath = Path.GetDirectoryName(fullFilePath);
            string name = Path.GetFileName(fullFilePath);

            string[] fileNames = !name.Contains("*") ? new[] {fullFilePath} : Directory.GetFiles(fullPath, name).ToArray();

            return fileNames.Select(fileName => new Audio(fileName, GetOutputFile(options, fileName), RecognitionAudio.FromFile(fileName))).ToList();
        }

        private List<Audio> GetStorageFiles(Options options)
        {
            var parts = options.Source.Replace("gs://", "").Split('/');

            if (parts.Length < 2)
                return new List<Audio>();

            string bucket = $"{string.Join('/', parts.Take(parts.Length - 1))}";
            string name = parts[parts.Length - 1];

            if (!name.Contains("*"))
                return new List<Audio>
                {
                    new Audio(options.Source, GetOutputFile(options, name), RecognitionAudio.FromStorageUri(options.Source))
                };

            var storageClient = StorageClient.Create(GoogleCredential.FromFile(options.KeyPath));
            string[] fileNames = storageClient.ListObjects(bucket).Select(f => f.Name).ToArray();

            WildCard wildCard = new WildCard(name, RegexOptions.IgnoreCase);
            return fileNames
                .Where(fileName => !fileName.EndsWith("/") && wildCard.IsMatch(fileName))
                .Select(fileName =>
                {
                    string source = $"gs://{bucket}/{fileName}";
                    return new Audio(source, GetOutputFile(options, fileName), RecognitionAudio.FromStorageUri(source));
                }).ToList();
        }

        private bool FromGoogleStorage(Options options)
        {
            return options.Source.StartsWith("gs://", StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetOutputFile(Options options, string fileName)
        {
            return Path.Combine(options.Destination, $"{Path.GetFileNameWithoutExtension(fileName)}-transcript.txt");
        }

        private string FormatLine(SpeechRecognitionAlternative alternative)
        {
            return $"[{decimal.Round((decimal)alternative.Confidence * 100, 2)}%] {alternative.Transcript}\r\n";
        }
    }
}

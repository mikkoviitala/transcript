using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Transcript.Api;
using Transcript.Api.Model;
using Transcript.Client.Model;
using Transcript.Core;
using Transcript.Core.Model;
using Transcript.Core.Service;

namespace Transcript.Client.Service
{
    /// <summary>
    /// Process command line arguments
    /// </summary>
    public class ApplicationArgumentProcessor
    {
        public ApplicationArgumentProcessor()
            :this(new ConsoleLogger(), new ConsoleProgressIndicator())
        {}

        public ApplicationArgumentProcessor(ILogger logger, IProgressIndicator progressIndicator)
        {
            Logger = logger;
            ProgressIndicator = progressIndicator;
        }

        public ILogger Logger { get; set; }

        public IProgressIndicator ProgressIndicator { get; set; }

        public void Process(ApplicationArguments appArgs)
        {
            if (appArgs.IsValidPasswordProvided)
            {
                if (appArgs.SetupKey)
                {
                    SetupKey(appArgs);
                }
                else
                {
                    if (!appArgs.KeyExists)
                        Exit(appArgs.KeyError);

                    if (!appArgs.FileProvided && !appArgs.StorageUrlProvided)
                        Exit($"{appArgs.FileError}\r\nor\r\n{appArgs.StorageUrlError}");

                    SpeechToText(appArgs);
                }
            }
            else
            {
                Exit(appArgs.PasswordError);
            }
        }

        private void SetupKey(ApplicationArguments appArgs)
        {
            if (appArgs.FileProvided)
                EncryptKey(appArgs);
            else
                Exit(appArgs.FileError);
        }


        private void SpeechToText(ApplicationArguments appArgs)
        {
            var options = ApplicationArguments.ToOptions(appArgs);

            void Cleanup(object sender, UnhandledExceptionEventArgs e) => File.Delete(options.KeyPath);
            AppDomain.CurrentDomain.UnhandledException += Cleanup;

            string decrypted = DecryptKey(appArgs);
            File.WriteAllText(options.KeyPath, decrypted);

            if (!Directory.Exists(options.Destination))
                Directory.CreateDirectory(options.Destination);

            Logger.Log(string.Empty);
            Logger.Log($"Processing audio files {options}");

            ProgressIndicator.Busy();

            var api = new SpeechToText();
            var tasks = api.Transcribe(options);
            Task.WhenAll(tasks).Wait();

            ProgressIndicator.Completed();

            ReportResults(tasks);

            Logger.Log(string.Empty);
            Logger.Log($"Transcripts saved in \"{options.Destination}\"");
            Logger.Log(string.Empty);

            AppDomain.CurrentDomain.UnhandledException -= Cleanup;
            Cleanup(null, null);
        }

        private void EncryptKey(ApplicationArguments appArgs)
        {
            try
            {
                var crypto = new CryptoService();
                string encrypted = crypto.Encrypt(File.ReadAllText(appArgs.FilePath), appArgs.Password);
                File.WriteAllText(appArgs.Key, encrypted);

                bool loop;
                do
                {
                    Console.Write($"Google Credentials encrypted and stored in \"{appArgs.Key}\" file, delete source \"{appArgs.FilePath}\" ? [Y\\n] ");
                    string answer = Console.ReadLine();
                    answer = string.IsNullOrWhiteSpace(answer) ? "y" : answer.ToLowerInvariant();
                    loop = answer != "y" && answer != "n";

                    if (answer == "y")
                    {
                        File.Delete(appArgs.FilePath);
                        Console.WriteLine($"Source \"{appArgs.FilePath}\" deleted");
                    }
                    else
                    {
                        Console.WriteLine($"Source \"{appArgs.FilePath}\" not deleted, be aware that this file contains your Google API credentials as plain text");
                    }
                } while (loop);
            }
            catch (Exception e)
            {
                Exit(e.Message);
            }
        }

        private string DecryptKey(ApplicationArguments appArgs)
        {
            string decrypted = null;
            try
            {
                decrypted = (new CryptoService()).Decrypt(File.ReadAllText(appArgs.Key), appArgs.Password);
            }
            catch
            {
                Exit($"Unable to decrypt {appArgs.Key}, maybe you provided wrong password or file is corrupted?");
            }

            return decrypted;
        }

        private void ReportResults(List<Task<Result>> tasks)
        {
            var success = tasks.Where(t => t.Result.Success).ToList();
            var failed = tasks.Where(t => !t.Result.Success).ToList();

            if (success.Any())
                Logger.Log($"Success {success.Count}/{tasks.Count}");
            if (failed.Any())
            {
                Logger.Log($"Failed {failed.Count}/{tasks.Count}");
                failed.ForEach(t =>
                {
                    Logger.Log(string.Empty);
                    Logger.Log(t.Result.InputFile);
                    Logger.Log(t.Result.Message);
                });
            }
        }

        private void Exit(string message = null)
        {
            if (message != null)
                Logger.Log(message);

            Environment.Exit(message != null ? 666 : 0);
        }
    }
}

using System;
using System.Linq;
using Transcript.Client.Model;
using Transcript.Client.Service;
using Transcript.Core.Model;

namespace Transcript.Client
{
    public class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            ILogger logger = new ConsoleLogger();

            try
            {
                var parser = new CommandLineParser.CommandLineParser { ShowUsageOnEmptyCommandline = true };
                var appArgs = new ApplicationArguments();
                parser.ExtractArgumentAttributes(appArgs);
                parser.ParseCommandLine(args);
                parser.Dispose();

                if (args.Length > 0 && !HelpWanted(args))
                {
                    var processor = new ApplicationArgumentProcessor
                    {
                        Logger = logger,
                        ProgressIndicator = new ConsoleProgressIndicator()
                    };
                    processor.Process(appArgs);
                }
            }
            catch (Exception e)
            {
                logger.Log(e.Message);
            }
        }

        private static bool HelpWanted(string[] args)
        {
            return args.Contains("--help");
        }
    }
}

using System;
using Transcript.Core.Model;

namespace Transcript.Client.Service
{
    /// <summary>
    /// ILogger implementation
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, bool append = false)
        {
            if (!append)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}

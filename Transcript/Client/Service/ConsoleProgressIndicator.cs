using System;
using System.Threading;
using System.Threading.Tasks;
using Transcript.Core.Model;

namespace Transcript.Client.Service
{
    /// <summary>
    /// IProgressIndicator implementation
    /// </summary>
    public class ConsoleProgressIndicator : IProgressIndicator
    {
        private static readonly char[] Chars = { '-', '\\', '|', '/', '-', '\\', '|', '/' };
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly int _maxIndex = Chars.Length - 1;
        private int _currentIndex = -1;
        
        public void Busy()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_tokenSource.Token.IsCancellationRequested)
                {
                    if (_currentIndex == -1)
                        Console.WriteLine();

                    _currentIndex++;
                    if (_currentIndex > _maxIndex)
                        _currentIndex = 0;

                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine($"Please wait... {Chars[_currentIndex]}");
                    Thread.Sleep(150);
                }
            }, _tokenSource.Token);
        }

        public void Completed()
        {
            _tokenSource.Cancel();
        }
    }
}

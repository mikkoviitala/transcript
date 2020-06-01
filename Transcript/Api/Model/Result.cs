namespace Transcript.Api.Model
{
    public class Result
    {
        public Result(string inputFile, bool success, string message = null)
        {
            InputFile = inputFile;
            Success = success;
            Message = message;
        }

        public string InputFile { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

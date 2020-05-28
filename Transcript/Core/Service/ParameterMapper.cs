using System;
using System.Collections.Generic;

namespace Transcript.Core.Service
{
    /// <summary>
    /// Map parameters between console arguments and Google enums
    /// </summary>
    public class ParameterMapper
    {
        private static readonly Dictionary<string,string> EncodingDictionary = new Dictionary<string, string>
        {
            { "unspecified", "EncodingUnspecified" },
            { "linear", "Linear16" },
            { "flac", "Flac" },
            { "mulaw", "Mulaw" },
            { "amr", "Amr" },
            { "amrwb", "AmrWb" },
            { "speex", "SpeexWithHeaderByte" }
        };

        public string ToGoogleEncoding(string key)
        {
            if (EncodingDictionary.ContainsKey(key))
                return EncodingDictionary[key];

            throw new ArgumentException($"key {key} not found");
        }
    }
}

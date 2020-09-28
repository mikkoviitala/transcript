using System.Reflection;

namespace Transcript.Core.Service
{
    public static class VersionService
    {
        private const string Prefix = "Transcript v";
        private static string _version = null;

        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    var assembly = Assembly.GetEntryAssembly();
                    _version = $"{Prefix}{assembly.GetName().Version}";
                }
                return _version;
            }            
        }
    }
}

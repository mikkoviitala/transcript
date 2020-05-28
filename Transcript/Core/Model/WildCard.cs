//https://www.codeproject.com/Articles/11556/Converting-Wildcards-to-Regexes

using System.Text.RegularExpressions;

namespace Transcript.Core.Model
{
    public class WildCard : Regex
    {
        public WildCard(string pattern)
            : base(WildcardToRegex(pattern))
        {}

        public WildCard(string pattern, RegexOptions options)
            : base(WildcardToRegex(pattern), options)
        {}

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Escape(pattern)
                       .Replace("\\*", ".*")
                       .Replace("\\?", ".") + "$";
        }
    }
}

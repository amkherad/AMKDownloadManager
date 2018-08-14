using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AMKDownloadManager.Defaults.Messaging
{
    public static class MessageHelpers
    {
        public const string MessageSeparator = "\r\n";
        public const byte CrAsciiCode = 0x0D; //13
        public const byte LfAsciiCode = 0x0A; //10
        
        private static readonly Dictionary<string, string> EscapeMapping = new Dictionary<string, string>
        {
            //{"\"", @"\\\"""},
            {"\\\\", @"\\"},
            //{"\a", @"\a"},
            //{"\b", @"\b"},
            //{"\f", @"\f"},
            {"\n", @"\n"},
            {"\r", @"\r"},
            //{"\t", @"\t"},
            //{"\v", @"\v"},
            //{"\0", @"\0"},
        };

        private static readonly Regex EscapeRegex = new Regex(string.Join("|", EscapeMapping.Keys.ToArray()), RegexOptions.Compiled);


        public static string EscapeCrLf(string str)
        {
            return EscapeRegex.Replace(str, EscapeMatchEval);
        }

        public static string UnEscapeCrLf(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var result = new StringBuilder(str.Length + 10);
            for (var i = 0; i < str.Length;)
            {
                var backSlashIndex = str.IndexOf('\\', i);
                if (backSlashIndex < 0 || backSlashIndex == str.Length - 1)
                {
                    backSlashIndex = str.Length;
                }
                
                result.Append(str, i, backSlashIndex - i);

                if (backSlashIndex >= str.Length)
                {
                    break;
                }
                
                switch (str[backSlashIndex + 1])
                {
                    case 'n':
                        result.Append('\n');
                        break; // Line feed
                    case 'r':
                        result.Append('\r');
                        break; // Carriage return
                    //case 't':
                    //    result.Append('\t');
                    //    break; // Tab
                    case '\\':
                        result.Append('\\');
                        break; // Don't escape
                    default: // Unrecognized, copy as-is
                        result.Append('\\').Append(str[backSlashIndex + 1]);
                        break;
                }

                i = backSlashIndex + 2;
            }

            return result.ToString();
        }

        private static string EscapeMatchEval(Match match)
        {
            if (EscapeMapping.TryGetValue(match.Value, out var value))
            {
                return value;
            }

            return EscapeMapping[Regex.Escape(match.Value)];
        }
    }
}
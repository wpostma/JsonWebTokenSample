using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace JsonWebTokenSample.Common
{
    public static class ExtensionMethod
    {
        public static string ToSha256Hashing(this string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            SHA256 hashstring = SHA256.Create();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        public static string TrimInvalidCharacter(this string input)
        {
            //string pattern = "[\\~#%&*//{}/:?/|\"]";
            string pattern = "(?:[^a-z0-9 \".,\\^\\]\\[\\:\\}\\{\\+<>>=<=-]|(?<=['\"]))";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            string sanitized = r.Replace(input, string.Empty);
            return sanitized;
        }

        public static bool CheckNullStartWith(this string input, string compare, StringComparison strComparasion)
        {
            bool isStartWith = false;
            if (!string.IsNullOrEmpty(input))
            {
                if (!string.IsNullOrEmpty(compare))
                {
                    isStartWith = input.StartsWith(compare, strComparasion);
                }
            }
            return isStartWith;
        }
    }
}

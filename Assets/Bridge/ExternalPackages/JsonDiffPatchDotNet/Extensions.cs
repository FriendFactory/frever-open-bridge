using System;
using System.Linq;

namespace JsonDiffPatchDotNet
{
    public static class Extensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));
            return input.First().ToString().ToLower() + input.Substring(1);
        }

    }
}
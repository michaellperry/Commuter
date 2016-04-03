using System;

namespace Commuter.FeedJob
{
    public static class StringExtensions
    {
        public static string Truncate(this string str, int length)
        {
            if (str == null || str.Length <= length)
                return str;
            return str.Substring(0, length);
        }

        public static string NullIfWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }
    }
}

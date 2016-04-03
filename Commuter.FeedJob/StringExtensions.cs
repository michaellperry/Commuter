using System;

namespace Commuter.FeedJob
{
    public static class StringExtensions
    {
        public static string MaxLength(this string str, int length)
        {
            if (str == null || str.Length <= length)
                return str;
            return str.Substring(0, length);
        }
    }
}

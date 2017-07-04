using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemConsoleMonitor.Utils
{
    public static class ExtensionMethods
    {

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars - 3) + "...";
        }

        public static string ToMegabyte(this ulong input)
        {
            return $"{input / 1024 / 1024} MB";
        }

        public static string ToGigabyte(this ulong input)
        {
            return $"{input / 1024 / 1024 / 1024}GB";
        }


    }
}

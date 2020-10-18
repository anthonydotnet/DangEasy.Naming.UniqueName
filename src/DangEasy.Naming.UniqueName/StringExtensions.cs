using System;

namespace DangEasy.Naming.UniqueName
{
    public static class StringExtensions
    {
        public static bool InvariantEquals(this string compare, string compareTo)
        {
            return string.Equals(compare, compareTo, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool InvariantStartsWith(this string compare, string compareTo)
        {
            return compare.StartsWith(compareTo, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

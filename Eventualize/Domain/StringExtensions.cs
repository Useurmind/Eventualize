using System;
using System.Linq;

namespace Eventualize.Domain
{
    internal static class StringExtensions
    {
        public static Guid ToGuid(this string value)
        {
            Guid guid = Guid.Empty;
            Guid.TryParse(value, out guid);
            return guid;
        }
    }
}
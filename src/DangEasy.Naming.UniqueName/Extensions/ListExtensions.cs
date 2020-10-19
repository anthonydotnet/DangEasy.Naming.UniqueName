using DangEasy.Naming.UniqueName.Models;
using System.Collections.Generic;
using System.Linq;

namespace DangEasy.Naming.UniqueName.Extensions
{
    internal static class ListExtensions
    {
        internal static bool Contains(this IEnumerable<StructuredName> items, StructuredName model)
        {
            return items.Any(x => x.FullName.InvariantEquals(model.FullName));
        }

        internal static bool SimpleNameExists(this IEnumerable<StructuredName> items, string name)
        {
            return items.Any(x => x.FullName.InvariantEquals(name));
        }

        internal static bool SuffixedNameExists(this IEnumerable<StructuredName> items)
        {
            return items.Any(x => x.HasSuffix());
        }
    }
}

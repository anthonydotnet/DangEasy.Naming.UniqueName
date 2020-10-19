using DangEasy.Naming.UniqueName.Constants;
using DangEasy.Naming.UniqueName.Extensions;
using DangEasy.Naming.UniqueName.Models;
using System.Collections.Generic;
using System.Linq;

namespace DangEasy.Naming.UniqueName
{
    public class NameGenerator
    {
        public static string GetUniqueName(IEnumerable<string> names, string name)
        {
            var cleanName = string.IsNullOrWhiteSpace(name) ? StringConstants.SPACE_CHARACTER : name;

            var model = new StructuredName(cleanName);
            var items = names
                    .Where(x => x.InvariantStartsWith(model.Text)) // ignore non-matching names
                    .Select(x => new StructuredName(x));

            // name is empty, and there are no other names with suffixes, so just return " (1)"
            if (model.IsEmptyName() && !items.Any())
            {
                model.SetSuffix(1);

                return model.FullName;
            }

            // name is empty, and there are other names with suffixes
            if (model.IsEmptyName() && items.SuffixedNameExists()) 
            {
                var emptyNameSuffix = GetSuffixNumber(items);

                if (emptyNameSuffix > 0)
                {
                    model.SetSuffix(emptyNameSuffix);

                    return model.FullName;
                }
            }

            //  no suffix - name without suffix does NOT exist, AND name with suffix does NOT exist
            if (!model.HasSuffix() && !items.SimpleNameExists(model.Text) && !items.SuffixedNameExists())
            {
                model.SetNoSuffix();

                return model.FullName;
            }

            // no suffix - name without suffix exists, however name with suffix does NOT exist
            if (!model.HasSuffix() && items.SimpleNameExists(model.Text) && !items.SuffixedNameExists())
            {
                var firstSuffix = GetFirstSuffix(items);
                model.SetSuffix(firstSuffix);

                return model.FullName;
            }

            // no suffix - name without suffix exists, AND name with suffix does exist
            if (!model.HasSuffix() && items.SimpleNameExists(model.Text) && items.SuffixedNameExists())
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            // no suffix - name without suffix does NOT exist, however name with suffix exists
            if (!model.HasSuffix() && !items.SimpleNameExists(model.Text) && items.SuffixedNameExists())
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            // has suffix - name without suffix exists
            if (model.HasSuffix() && items.SimpleNameExists(model.Text))
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            // has suffix - name without suffix does NOT exist
            // a case where the user added the suffix, so add a secondary suffix
            if (model.HasSuffix() && !items.SimpleNameExists(model.Text))
            {
                model.SetText(model.FullName);
                model.SetNoSuffix();

                // filter items based on full name with suffix
                items = items.Where(x => x.Text.InvariantStartsWith(model.FullName));
                var secondarySuffix = GetFirstSuffix(items);
                model.SetSuffix(secondarySuffix);

                return model.FullName;
            }

            // has suffix - name without suffix also exists, therefore we simply increment
            if (model.HasSuffix() && items.SimpleNameExists(model.Text))
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            return name;
        }



        private static int GetFirstSuffix(IEnumerable<StructuredName> items)
        {
            const int suffixStart = 1;

            if (!items.Any(x => x.Number == suffixStart))
            {
                // none of the suffixes are the same as suffixStart, so we can use suffixStart!
                return suffixStart;
            }

            return GetSuffixNumber(items);
        }

        private static int GetSuffixNumber(IEnumerable<StructuredName> items)
        {
            int current = 1;
            foreach (var item in items.OrderBy(x => x.Number))
            {
                if (item.Number == current)
                {
                    current++;
                }
                else if (item.Number > current)
                {
                    // do nothing - we found our number!
                    // eg. when suffixes are 1 & 3, then this method is required to generate 2
                    break;
                }
            }

            return current;
        }
    }
}

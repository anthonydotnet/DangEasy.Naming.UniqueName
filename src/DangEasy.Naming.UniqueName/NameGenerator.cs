using DangEasy.Naming.UniqueName.Constants;
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
            if (model.NameIsEmpty() && !items.Any())
            {
                model.SetSuffix(1);

                return model.FullName;
            }

            // name is empty, and there are other names with suffixes
            if (model.NameIsEmpty() && SuffixedNameExists(items)) // items.Any())
            {
                var emptyNameSuffix = GetSuffixNumber(items);

                if (emptyNameSuffix > 0)
                {
                    model.SetSuffix(emptyNameSuffix);

                    return model.FullName;
                }
            }

            //  no suffix - name without suffix does NOT exist, AND name with suffix does NOT exist
            if (!model.HasSuffix() && !SimpleNameExists(items, model.Text) && !SuffixedNameExists(items))
            {
                model.SetNoSuffix();

                return model.FullName;
            }

            // no suffix - name without suffix exists, however name with suffix does NOT exist
            if (!model.HasSuffix() && SimpleNameExists(items, model.Text) && !SuffixedNameExists(items))
            {
                var firstSuffix = GetFirstSuffix(items);
                model.SetSuffix(firstSuffix);

                return model.FullName;
            }

            // no suffix - name without suffix exists, AND name with suffix does exist
            if (!model.HasSuffix() && SimpleNameExists(items, model.Text) && SuffixedNameExists(items))
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            // no suffix - name without suffix does NOT exist, however name with suffix exists
            if (!model.HasSuffix() && !SimpleNameExists(items, model.Text) && SuffixedNameExists(items))
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            // has suffix - name without suffix exists
            if (model.HasSuffix() && SimpleNameExists(items, model.Text))
            {
                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            // has suffix - name without suffix does NOT exist
            // this is a case where the user added the suffix, so we need to add a secondary suffix
            if (model.HasSuffix() && items.Count() == 1 && Contains(items, model)) // this is wrong!!!
            {
                model.SetText(model.FullName);
                model.SetNoSuffix();

                // filter items based on full name with suffix
                items = items.Where(x => x.Text.InvariantStartsWith(model.FullName));
                var secondarySuffix = GetFirstSuffix(items);
                model.SetSuffix(secondarySuffix);

                return model.FullName;
            }

            // TODO: we need to determine how to increment a multi-suffix, not just add a secondary one???

            if (model.HasSuffix() && !SimpleNameExists(items, model.Text))
            {
                // in this case, we have an increament... so we need to detect it in names, then use that item.Text instead of model.Text

                var nextSuffix = GetSuffixNumber(items);
                model.SetSuffix(nextSuffix);

                return model.FullName;
            }

            return name;
        }

        private static bool Contains(IEnumerable<StructuredName> items, StructuredName model)
        {
            return items.Any(x => x.FullName.InvariantEquals(model.FullName));
        }

        private static bool SimpleNameExists(IEnumerable<StructuredName> items, string name)
        {
            return items.Any(x => !x.HasSuffix());

            //   return items.Any(x => x.FullName.InvariantEquals(name));
        }

        private static bool SuffixedNameExists(IEnumerable<StructuredName> items)
        {
            return items.Any(x => x.HasSuffix());
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

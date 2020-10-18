using DangEasy.Naming.UniqueName.Constants;
using System.Text.RegularExpressions;

namespace DangEasy.Naming.UniqueName
{
        internal class StructuredName
        {
            private const string Suffixed_Pattern = @"(.*)\s\((\d+)\)$";
            internal string Text { get; private set; }
            internal int Number { get; private set; }
            public string FullName
            {
                get
                {
                    string text = string.IsNullOrWhiteSpace(Text) ? Text.Trim() : Text;

                    return Number > 0 ? $"{text} ({Number})" : text;
                }
            }

            internal StructuredName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    Text = StringConstants.SPACE_CHARACTER;
                    Number = 0;

                    return;
                }

                var rg = new Regex(Suffixed_Pattern);
                var matches = rg.Matches(name);
                if (matches.Count > 0)
                {
                    var match = matches[0];
                    Text = match.Groups[1].Value;
                    int number;
                    Number = int.TryParse(match.Groups[2].Value, out number) ? number : 0;
                    return;
                }
                else
                {
                    Text = name;
                    Number = 0;
                }
            }

            internal bool HasSuffix()
            {
                return Number > 0;
            }

            internal void SetSuffix(int number)
            {
                Number = number;
            }

            internal void SetNoSuffix()
            {
                Number = 0;
            }

            internal void SetText(string text)
            {
                Text = text;
            }

            internal bool NameIsEmpty()
            {
                return string.IsNullOrWhiteSpace(Text);
            }
    }
}

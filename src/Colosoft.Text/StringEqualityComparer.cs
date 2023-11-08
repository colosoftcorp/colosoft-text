using System;
using System.Collections.Generic;

namespace Colosoft.Text
{
    public class StringEqualityComparer : IEqualityComparer<string>
    {
        private static readonly Dictionary<StringComparison, StringEqualityComparer> ComparerMap
            = new Dictionary<StringComparison, StringEqualityComparer>();

        private static readonly object CompareInitSync = new object();

        private readonly StringComparison comparison;

        public StringComparison Comparison => this.comparison;

        public static StringEqualityComparer GetComparer(StringComparison comparison)
        {
            StringEqualityComparer comparer;

            if (!ComparerMap.TryGetValue(comparison, out comparer))
            {
                lock (CompareInitSync)
                {
                    if (!ComparerMap.TryGetValue(comparison, out comparer))
                    {
                        comparer = new StringEqualityComparer(comparison);
                        ComparerMap[comparison] = comparer;
                    }
                }
            }

            return comparer;
        }

        public StringEqualityComparer(StringComparison comparison)
        {
            this.comparison = comparison;
        }

        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, this.Comparison) == 0;
        }

        public int GetHashCode(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            switch (this.comparison)
            {
                case StringComparison.CurrentCultureIgnoreCase:
                    return obj.ToLower(System.Globalization.CultureInfo.CurrentCulture).GetHashCode();
                case StringComparison.InvariantCultureIgnoreCase:
                    return obj.ToUpperInvariant().GetHashCode();
                case StringComparison.OrdinalIgnoreCase:
                    return obj.ToLower(System.Globalization.CultureInfo.CurrentCulture).GetHashCode();
                case StringComparison.Ordinal:
                case StringComparison.InvariantCulture:
                case StringComparison.CurrentCulture:
                    return obj.GetHashCode();
            }

            return 0;
        }
    }
}

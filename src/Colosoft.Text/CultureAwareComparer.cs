using System;
using System.Globalization;

namespace Colosoft.Text
{
    [Serializable]
    internal sealed class CultureAwareComparer : MessageFormattableComparer
    {
        private readonly CultureInfo culture;
        private readonly CompareInfo compareInfo;
        private readonly bool ignoreCase;

        public CultureAwareComparer(CultureInfo culture, bool ignoreCase)
        {
            this.culture = culture;
            this.compareInfo = culture.CompareInfo;
            this.ignoreCase = ignoreCase;
        }

        public override int Compare(IMessageFormattable x, IMessageFormattable y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return this.compareInfo.Compare(x.Format(this.culture), y.Format(this.culture), this.ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public override bool Equals(object obj)
        {
            var comparer = obj as CultureAwareComparer;
            if (comparer == null)
            {
                return false;
            }

            return (this.ignoreCase == comparer.ignoreCase) && this.compareInfo.Equals(comparer.compareInfo);
        }

        public override bool Equals(IMessageFormattable x, IMessageFormattable y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if ((x == null) || (y == null))
            {
                return false;
            }

            return this.compareInfo.Compare(x.Format(this.culture), y.Format(this.culture), this.ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) == 0;
        }

        public override int GetHashCode()
        {
            int hashCode = this.compareInfo.GetHashCode();
            if (!this.ignoreCase)
            {
                return hashCode;
            }

            return ~hashCode;
        }

        public override int GetHashCode(IMessageFormattable obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (this.ignoreCase)
            {
                return StringComparer.InvariantCultureIgnoreCase.GetHashCode(obj.Format(this.culture));
            }

            return StringComparer.InvariantCulture.GetHashCode(obj.Format(this.culture));
        }
    }
}

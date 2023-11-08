using System;

namespace Colosoft.Text
{
    [Serializable]
    internal sealed class OrdinalComparer : MessageFormattableComparer
    {
        private readonly System.Globalization.CultureInfo culture;
        private readonly bool ignoreCase;

        public OrdinalComparer(System.Globalization.CultureInfo culture, bool ignoreCase)
        {
            this.culture = culture;
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

            var xStr = x.Format(this.culture);
            var yStr = y.Format(this.culture);

            if (this.ignoreCase)
            {
                return StringComparer.OrdinalIgnoreCase.Compare(xStr, yStr);
            }

            return string.CompareOrdinal(xStr, yStr);
        }

        public override bool Equals(object obj)
        {
            OrdinalComparer comparer = obj as OrdinalComparer;
            if (comparer == null)
            {
                return false;
            }

            return this.ignoreCase == comparer.ignoreCase;
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

            if (!this.ignoreCase)
            {
                return x.Equals(y);
            }

            var xStr = x.Format(this.culture);
            var yStr = y.Format(this.culture);

            if (xStr.Length != yStr.Length)
            {
                return false;
            }

            return StringComparer.OrdinalIgnoreCase.Compare(xStr, yStr) == 0;
        }

        public override int GetHashCode()
        {
            int hashCode = "OrdinalComparer".GetHashCode();
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
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Format(this.culture));
            }

            return obj.GetHashCode();
        }
    }
}

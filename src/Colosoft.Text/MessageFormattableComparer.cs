using System;
using System.Collections;
using System.Collections.Generic;

namespace Colosoft.Text
{
    public abstract class MessageFormattableComparer : IComparer, IEqualityComparer, IComparer<IMessageFormattable>, IEqualityComparer<IMessageFormattable>
    {
        private static readonly MessageFormattableComparer InvariantCultureValue;
        private static readonly MessageFormattableComparer InvariantCultureIgnoreCaseValue;
        private static readonly MessageFormattableComparer OrdinalValue;
        private static readonly MessageFormattableComparer OrdinalIgnoreCaseValue;

        public static MessageFormattableComparer CurrentCulture
        {
            get
            {
                return new CultureAwareComparer(System.Globalization.CultureInfo.CurrentCulture, false);
            }
        }

        public static MessageFormattableComparer CurrentCultureIgnoreCase
        {
            get
            {
                return new CultureAwareComparer(System.Globalization.CultureInfo.CurrentCulture, true);
            }
        }

        public static MessageFormattableComparer InvariantCulture
        {
            get
            {
                return InvariantCultureValue;
            }
        }

        public static MessageFormattableComparer InvariantCultureIgnoreCase
        {
            get
            {
                return InvariantCultureIgnoreCaseValue;
            }
        }

        public static MessageFormattableComparer Ordinal
        {
            get
            {
                return OrdinalValue;
            }
        }

        public static MessageFormattableComparer OrdinalIgnoreCase
        {
            get
            {
                return OrdinalIgnoreCaseValue;
            }
        }

#pragma warning disable S3963 // "static" fields should be initialized inline
#pragma warning disable CA1810 // Initialize reference type static fields inline
        static MessageFormattableComparer()
#pragma warning restore CA1810 // Initialize reference type static fields inline
        {
            InvariantCultureValue = new CultureAwareComparer(System.Globalization.CultureInfo.InvariantCulture, false);
            InvariantCultureIgnoreCaseValue = new CultureAwareComparer(System.Globalization.CultureInfo.InvariantCulture, true);
            OrdinalValue = new OrdinalComparer(System.Globalization.CultureInfo.InvariantCulture, false);
            OrdinalIgnoreCaseValue = new OrdinalComparer(System.Globalization.CultureInfo.InvariantCulture, true);
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public static MessageFormattableComparer Create(System.Globalization.CultureInfo culture, bool ignoreCase)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            return new CultureAwareComparer(culture, ignoreCase);
        }

        public new bool Equals(object x, object y)
        {
            if (x == y)
            {
                return true;
            }

            if ((x == null) || (y == null))
            {
                return false;
            }

            var str = x as IMessageFormattable;
            if (str != null)
            {
                var str2 = y as IMessageFormattable;
                if (str2 != null)
                {
                    return this.Equals(str, str2);
                }
            }

            return x.Equals(y);
        }

        public abstract bool Equals(IMessageFormattable x, IMessageFormattable y);

        public int Compare(object x, object y)
        {
            if (x == y)
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

            var str = x as IMessageFormattable;
            if (str != null)
            {
                var str2 = y as IMessageFormattable;
                if (str2 != null)
                {
                    return this.Compare(str, str2);
                }
            }

            IComparable comparable = x as IComparable;
            if (comparable == null)
            {
                throw new ArgumentException("Argument implement IComparable");
            }

            return comparable.CompareTo(y);
        }

        public abstract int Compare(IMessageFormattable x, IMessageFormattable y);

        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is IMessageFormattable str)
            {
                return this.GetHashCode(str);
            }

            return obj.GetHashCode();
        }

        public abstract int GetHashCode(IMessageFormattable obj);
    }
}

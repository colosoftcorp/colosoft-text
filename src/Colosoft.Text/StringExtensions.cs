using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Colosoft.Text
{
    public static class StringExtensions
    {
        private static readonly Dictionary<string, string> ForeignCharacters = new Dictionary<string, string>
        {
            { "äæǽ", "ae" },
            { "öœ", "oe" },
            { "ü", "ue" },
            { "Ä", "Ae" },
            { "Ü", "Ue" },
            { "Ö", "Oe" },
            { "ÀÁÂÃÄÅǺĀĂĄǍΑΆẢẠẦẪẨẬẰẮẴẲẶА", "A" },
            { "àáâãåǻāăąǎªαάảạầấẫẩậằắẵẳặа", "a" },
            { "Б", "B" },
            { "б", "b" },
            { "ÇĆĈĊČ", "C" },
            { "çćĉċč", "c" },
            { "Д", "D" },
            { "д", "d" },
            { "ÐĎĐΔ", "Dj" },
            { "ðďđδ", "dj" },
            { "ÈÉÊËĒĔĖĘĚΕΈẼẺẸỀẾỄỂỆЕЭ", "E" },
            { "èéêëēĕėęěέεẽẻẹềếễểệеэ", "e" },
            { "Ф", "F" },
            { "ф", "f" },
            { "ĜĞĠĢΓГҐ", "G" },
            { "ĝğġģγгґ", "g" },
            { "ĤĦ", "H" },
            { "ĥħ", "h" },
            { "ÌÍÎÏĨĪĬǏĮİΗΉΊΙΪỈỊИЫ", "I" },
            { "ìíîïĩīĭǐįıηήίιϊỉịиыї", "i" },
            { "Ĵ", "J" },
            { "ĵ", "j" },
            { "ĶΚК", "K" },
            { "ķκк", "k" },
            { "ĹĻĽĿŁΛЛ", "L" },
            { "ĺļľŀłλл", "l" },
            { "М", "M" },
            { "м", "m" },
            { "ÑŃŅŇΝН", "N" },
            { "ñńņňŉνн", "n" },
            { "ÒÓÔÕŌŎǑŐƠØǾΟΌΩΏỎỌỒỐỖỔỘỜỚỠỞỢО", "O" },
            { "òóôõōŏǒőơøǿºοόωώỏọồốỗổộờớỡởợо", "o" },
            { "П", "P" },
            { "п", "p" },
            { "ŔŖŘΡР", "R" },
            { "ŕŗřρр", "r" },
            { "ŚŜŞȘŠΣС", "S" },
            { "śŝşșšſσςс", "s" },
            { "ȚŢŤŦτТ", "T" },
            { "țţťŧт", "t" },
            { "ÙÚÛŨŪŬŮŰŲƯǓǕǗǙǛŨỦỤỪỨỮỬỰУ", "U" },
            { "ùúûũūŭůűųưǔǖǘǚǜυύϋủụừứữửựу", "u" },
            { "ÝŸŶΥΎΫỲỸỶỴЙ", "Y" },
            { "ýÿŷỳỹỷỵй", "y" },
            { "В", "V" },
            { "в", "v" },
            { "Ŵ", "W" },
            { "ŵ", "w" },
            { "ŹŻŽΖЗ", "Z" },
            { "źżžζз", "z" },
            { "ÆǼ", "AE" },
            { "ß", "ss" },
            { "Ĳ", "IJ" },
            { "ĳ", "ij" },
            { "Œ", "OE" },
            { "ƒ", "f" },
            { "ξ", "ks" },
            { "π", "p" },
            { "β", "v" },
            { "μ", "m" },
            { "ψ", "ps" },
            { "Ё", "Yo" },
            { "ё", "yo" },
            { "Є", "Ye" },
            { "є", "ye" },
            { "Ї", "Yi" },
            { "Ж", "Zh" },
            { "ж", "zh" },
            { "Х", "Kh" },
            { "х", "kh" },
            { "Ц", "Ts" },
            { "ц", "ts" },
            { "Ч", "Ch" },
            { "ч", "ch" },
            { "Ш", "Sh" },
            { "ш", "sh" },
            { "Щ", "Shch" },
            { "щ", "shch" },
            { "ЪъЬь", string.Empty },
            { "Ю", "Yu" },
            { "ю", "yu" },
            { "Я", "Ya" },
            { "я", "ya" },
        };

        public static char RemoveDiacritics(this char c)
        {
            foreach (var entry in ForeignCharacters)
            {
                if (entry.Key.IndexOf(c) != -1)
                {
                    return entry.Value[0];
                }
            }

            return c;
        }

        public static string RemoveDiacritics(this string s)
        {
            if (s == null)
            {
                return null;
            }

            var text = new StringBuilder();

            foreach (char c in s)
            {
                int len = text.Length;

                foreach (KeyValuePair<string, string> entry in ForeignCharacters)
                {
                    if (entry.Key.IndexOf(c) != -1)
                    {
                        text.Append(entry.Value);
                        break;
                    }
                }

                if (len == text.Length)
                {
                    text.Append(c);
                }
            }

            return text.ToString();
        }

#pragma warning disable CA1055 // URI-like return values should not be strings
        public static string NormalizeStringForUrl(this string name)
#pragma warning restore CA1055 // URI-like return values should not be strings
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            var normalizedString = name.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                switch (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        stringBuilder.Append(c);
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                        stringBuilder.Append('_');
                        break;
                }
            }

            string result = stringBuilder.ToString();
            return string.Join("_", result.Split(
                new char[] { '_' },
                StringSplitOptions.RemoveEmptyEntries));
        }

        public static string DelimitWith<T>(this IEnumerable<T> source, string separator)
        {
            return source.DelimitWith(separator, null);
        }

        public static string DelimitWith<T>(this IEnumerable<T> source, string separator, string format)
        {
            return source.DelimitWith(separator, format, string.Empty, string.Empty);
        }

        public static string DelimitWith<T>(this IEnumerable<T> source, string separator, string format, string prefix, string suffix)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var buffer = new StringBuilder();
                    if (prefix != null)
                    {
                        buffer.Append(prefix);
                    }

                    AppendItem(enumerator, buffer, format);

                    while (enumerator.MoveNext())
                    {
                        buffer.Append(separator);
                        AppendItem(enumerator, buffer, format);
                    }

                    if (suffix != null)
                    {
                        buffer.Append(suffix);
                    }

                    return buffer.ToString();
                }

                return string.Empty;
            }
        }

        private static void AppendItem<T>(IEnumerator<T> enumerator, StringBuilder buffer, string format)
        {
            if (format != null)
            {
#pragma warning disable CA1305 // Specify IFormatProvider
                buffer.AppendFormat(format, enumerator.Current);
#pragma warning restore CA1305 // Specify IFormatProvider
            }
            else
            {
                buffer.Append(enumerator.Current);
            }
        }
    }
}

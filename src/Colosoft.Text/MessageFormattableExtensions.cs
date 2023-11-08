using System;
using System.Collections.Generic;

namespace Colosoft
{
    public static class MessageFormattableExtensions
    {
        public static IMessageFormattable GetFormatter(this string text, params object[] parameters)
        {
            if (text == null)
            {
                return null;
            }

            return new Text.TextMessageFormattable(text, parameters);
        }

        public static IMessageFormattable Join(this IEnumerable<IMessageFormattable> messages, string separator)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            var enumerator = messages.GetEnumerator();
            IMessageFormattable current = null;

            if (!enumerator.MoveNext())
            {
                return MessageFormattable.Empty;
            }

            current = enumerator.Current;

            while (enumerator.MoveNext())
            {
                if (current == null)
                {
                    current = enumerator.Current;
                }
                else
                {
                    current = new Text.JoinMessageFormattable(current, separator, enumerator.Current);
                }
            }

            return current;
        }

        public static string FormatOrNull(this IMessageFormattable message)
        {
            return message?.Format(System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}

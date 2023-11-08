using System;
using System.Collections.Generic;
using System.Linq;

namespace Colosoft
{
    public sealed class MessageFormattable : IMessageFormattable
    {
        public static readonly IMessageFormattable Empty = string.Empty.GetFormatter();

        private readonly IEnumerable<MessageText> messages;
        private readonly object[] parameters;

        public MessageFormattable(MessageText[] messages, params object[] parameters)
        {
            this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
            this.parameters = parameters;
        }

        public IMessageFormattable Join(string separator, IMessageFormattable message)
        {
            return new Text.JoinMessageFormattable(this, separator, message);
        }

        string IMessageFormattable.Format()
        {
            return this.Format(System.Globalization.CultureInfo.CurrentCulture, null);
        }

        string IMessageFormattable.Format(System.Globalization.CultureInfo culture)
        {
            return this.Format(culture, this.parameters);
        }

        public string Format(System.Globalization.CultureInfo culture, params object[] parameters)
        {
            foreach (var i in this.messages)
            {
                if ((culture == null && i.CultureInfo == null) ||
                    (culture != null && i.CultureInfo == culture.Name))
                {
                    return i.Format(parameters);
                }
            }

            var message = this.messages.FirstOrDefault(f => string.IsNullOrEmpty(f.CultureInfo) || f.CultureInfo == "ivl");
            return message == null ? null : message.Format(parameters);
        }

        public bool Matches(System.Globalization.CultureInfo culture)
        {
            if (this.messages
                .Any(i =>
                    (culture == null && i.CultureInfo == null) ||
                    (culture != null && i.CultureInfo == culture.Name)))
            {
                return true;
            }

            return this.messages.Any(f => string.IsNullOrEmpty(f.CultureInfo) || f.CultureInfo == "ivl");
        }

        public bool Equals(IMessageFormattable other)
        {
            if (other == null)
            {
                return false;
            }

            return StringComparer.Ordinal.Equals(
                this.Format(System.Globalization.CultureInfo.InvariantCulture, this.parameters),
                other.Format(System.Globalization.CultureInfo.InvariantCulture, this.parameters));
        }
    }
}

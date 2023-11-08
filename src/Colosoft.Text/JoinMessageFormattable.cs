using System;

namespace Colosoft.Text
{
    public sealed class JoinMessageFormattable : IMessageFormattable, ICloneable
    {
        private readonly IMessageFormattable leftMessage;
        private readonly string separator;
        private readonly IMessageFormattable rightMessage;

        public JoinMessageFormattable(IMessageFormattable leftMessage, string separator, IMessageFormattable rightMessage)
        {
            this.leftMessage = leftMessage ?? throw new ArgumentNullException(nameof(leftMessage));
            this.separator = separator;
            this.rightMessage = rightMessage ?? throw new ArgumentNullException(nameof(rightMessage));
        }

        public string Format() => this.Format(System.Globalization.CultureInfo.CurrentCulture);

        public string Format(System.Globalization.CultureInfo culture)
        {
            return $"{this.leftMessage.Format(culture)}{this.separator}{this.rightMessage.Format(culture)}";
        }

        public string Format(System.Globalization.CultureInfo culture, params object[] parameters)
        {
            return $"{this.leftMessage.Format(culture, parameters)}{this.separator}{this.rightMessage.Format(culture, parameters)}";
        }

        public IMessageFormattable Join(string separator, IMessageFormattable message)
        {
            return new JoinMessageFormattable(this, separator, message);
        }

        public bool Matches(System.Globalization.CultureInfo culture)
        {
            return this.leftMessage.Matches(culture) && this.rightMessage.Matches(culture);
        }

        public bool Equals(IMessageFormattable other)
        {
            if (other == null)
            {
                return false;
            }

            var other2 = other as JoinMessageFormattable;
            if (other2 != null)
            {
                return ((this.leftMessage != null && other2.leftMessage != null && this.leftMessage.Equals(other2.leftMessage)) ||
                     (this.leftMessage == null && other2.leftMessage == null)) &&
                    ((this.rightMessage != null && other2.rightMessage != null && this.rightMessage.Equals(other2.rightMessage)) ||
                     (this.rightMessage == null && other2.rightMessage == null));
            }

            return false;
        }

        public object Clone()
        {
            var left = (this.leftMessage is ICloneable) ? (IMessageFormattable)((ICloneable)this.leftMessage).Clone() : this.leftMessage;
            var right = (this.rightMessage is ICloneable) ? (IMessageFormattable)((ICloneable)this.rightMessage).Clone() : this.rightMessage;

            return new JoinMessageFormattable(left, this.separator, right);
        }
    }
}

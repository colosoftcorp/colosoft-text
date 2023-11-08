using System;

namespace Colosoft.Text
{
    [Serializable]
    public sealed class TextMessageFormattable : IMessageFormattable, ICloneable
    {
        public string Text { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
        public object[] Parameters { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        public TextMessageFormattable(string text, params object[] parameters)
        {
            this.Text = text;
            this.Parameters = parameters;
        }

        public IMessageFormattable Join(string separator, IMessageFormattable message)
        {
            return new JoinMessageFormattable(this, separator, message);
        }

        string IMessageFormattable.Format()
        {
            return this.Format(System.Globalization.CultureInfo.CurrentCulture, null);
        }

        string IMessageFormattable.Format(System.Globalization.CultureInfo culture)
        {
            return this.Format(culture, this.Parameters);
        }

        public string Format(System.Globalization.CultureInfo culture, params object[] parameters)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                return null;
            }

            object[] values = null;

            if (parameters != null)
            {
                values = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var obj = parameters[i];
                    if (obj is IMessageFormattable formattable)
                    {
                        values[i] = formattable.Format(culture);
                    }
                    else
                    {
                        values[i] = obj;
                    }
                }
            }

            if (values != null && values.Length > 0)
            {
                return string.Format(culture, this.Text, values);
            }

            return this.Text;
        }

        public bool Matches(System.Globalization.CultureInfo culture)
        {
            return true;
        }

        public bool Equals(IMessageFormattable other)
        {
            var other2 = other as TextMessageFormattable;

            if (other2 != null && other2.Text == this.Text)
            {
                if (other2.Parameters == null && this.Parameters == null)
                {
                    return true;
                }

                if (other2.Parameters.Length != this.Parameters.Length)
                {
                    return false;
                }

                for (var i = 0; i < this.Parameters.Length; i++)
                {
                    if (this.Parameters[i] != other2.Parameters[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return this.Format(System.Globalization.CultureInfo.CurrentCulture, null);
        }

        public object Clone()
        {
            return new TextMessageFormattable(this.Text, this.Parameters);
        }
    }
}

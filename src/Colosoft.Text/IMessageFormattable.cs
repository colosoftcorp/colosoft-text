using System;

namespace Colosoft
{
    public interface IMessageFormattable : IEquatable<IMessageFormattable>
    {
        string Format();

        string Format(System.Globalization.CultureInfo culture);

        string Format(System.Globalization.CultureInfo culture, params object[] parameters);

        IMessageFormattable Join(string separator, IMessageFormattable message);

        bool Matches(System.Globalization.CultureInfo culture);
    }
}

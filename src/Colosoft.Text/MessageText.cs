namespace Colosoft
{
    public class MessageText
    {
        private readonly string text;
        private readonly string cultureInfo;

        public string Text => this.text;

        public string CultureInfo => this.cultureInfo;

        public MessageText(string text, string cultureInfo)
        {
            this.text = text;
            this.cultureInfo = cultureInfo;
        }

        public string Format(params object[] args)
        {
            if (string.IsNullOrEmpty(this.text))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(this.cultureInfo))
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, this.text, args);
            }

            return string.Format(System.Globalization.CultureInfo.GetCultureInfo(this.cultureInfo), this.text, args);
        }
    }
}

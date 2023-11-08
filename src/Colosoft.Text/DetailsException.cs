using System;

namespace Colosoft
{
    [Serializable]
    public class DetailsException : Exception, IDetailsException
    {
        public IMessageFormattable MessageFormattable { get; }

        public DetailsException()
        {
        }

        public DetailsException(string message)
            : base(message)
        {
        }

        public DetailsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DetailsException(IMessageFormattable message)
            : base(message != null ? message.Format(System.Globalization.CultureInfo.CurrentUICulture) : string.Empty)
        {
            this.MessageFormattable = message;
        }

        public DetailsException(IMessageFormattable message, Exception innerException)
            : base(message != null ? message.Format(System.Globalization.CultureInfo.CurrentUICulture) : string.Empty, innerException)
        {
            this.MessageFormattable = message;
        }

        protected DetailsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.MessageFormattable = info.GetValue("MessageFormattable", typeof(IMessageFormattable)) as IMessageFormattable;
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("MessageFormattable", this.MessageFormattable);
        }
    }
}

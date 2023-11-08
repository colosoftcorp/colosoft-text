using System;

namespace Colosoft
{
    [Serializable]
    public class DetailsInvalidOperationException : InvalidOperationException, IDetailsException
    {
        public IMessageFormattable MessageFormattable { get; }

        public DetailsInvalidOperationException()
        {
        }

        public DetailsInvalidOperationException(string message)
            : base(message)
        {
        }

        public DetailsInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DetailsInvalidOperationException(IMessageFormattable message)
            : base(message != null ? message.Format(System.Globalization.CultureInfo.CurrentUICulture) : string.Empty)
        {
        }

        public DetailsInvalidOperationException(IMessageFormattable message, Exception innerException)
            : base(message != null ? message.Format(System.Globalization.CultureInfo.CurrentUICulture) : string.Empty, innerException)
        {
            this.MessageFormattable = message;
        }

        protected DetailsInvalidOperationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Colosoft
{
    public interface IDetailsException
    {
        IMessageFormattable MessageFormattable { get; }
    }
}

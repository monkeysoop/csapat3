using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MekkdonaldsModel.Exception
{
    internal class LogFileDataException : System.Exception
    {
        public LogFileDataException()
        {
        }

        public LogFileDataException(string? message) : base(message)
        {
        }

        public LogFileDataException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}

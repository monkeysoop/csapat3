using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekkdonaldsModel.Persistence
{
    internal class BoardDataException : Exception
    {
        internal BoardDataException() { }
        internal BoardDataException(string message) : base(message) { }
    }
}

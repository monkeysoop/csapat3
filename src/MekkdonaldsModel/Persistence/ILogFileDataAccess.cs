using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mekkdonalds.Persistence
{
    internal interface ILogFileDataAccess
    {
        public Task<LogFile> Load(string path);

    }
}

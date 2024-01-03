using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core
{
    public abstract class MdbBaseException : Exception
    {
        public MdbBaseException(string message)
            : base(message)
        {
        }

        public MdbBaseException() 
        {
        }
    }
}

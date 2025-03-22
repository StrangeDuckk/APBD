using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    class OverfillException : Exception
    {
        public OverfillException(string message) : base(message) { }
    }
}

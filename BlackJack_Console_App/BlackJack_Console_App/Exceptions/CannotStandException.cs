using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Exceptions
{
    public class CannotStandException: Exception
    {
        public CannotStandException(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Exceptions
{
    public class WinGameException:Exception
    {
        public WinGameException(string message) : base(message) { }
    }
}

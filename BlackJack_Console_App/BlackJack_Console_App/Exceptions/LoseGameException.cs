using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Exceptions
{
    public class LoseGameException : Exception
    {
        public LoseGameException(string message) : base(message) { }
    }
}

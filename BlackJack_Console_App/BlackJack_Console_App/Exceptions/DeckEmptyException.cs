using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Exceptions
{
    public class DeckEmptyException:Exception
    {
        public DeckEmptyException(string message) : base(message) { }
    }
}

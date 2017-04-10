using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Exceptions
{
    public class CardNotValidException:Exception
    {
        public CardNotValidException(string message) : base(message) { }
    }
}

using BlackJack_Console_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.Start();
        }
    }
}

using BlackJack_Console_App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Models
{
    public class Card
    {
        public enum Suits /*{ SPADES = '\u2660', HEARTS = '\u2665'};*/{ SPADES = '\u2660', HEARTS = '\u2665', DIAMONDS = '\u2666', CLUBS = '\u2663' };
        public static string[] CardValues = /*{ "A", "8", "9", "10", "K" };*/{ "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        public string Value { get; private set; }
        public Suits Suit { get; private set; }

        public Card(Suits Suit, string Value){

            if (!CardValues.Contains(Value))
                throw new CardNotValidException("Not a valid card value!");

            this.Suit = Suit;
            this.Value = Value;
        }

        public override string ToString()
        {
            return Value + "-" + Suit;
        }
    }
}

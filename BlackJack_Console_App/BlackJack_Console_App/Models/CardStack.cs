using BlackJack_Console_App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlackJack_Console_App.Models.Card;

namespace BlackJack_Console_App.Models
{
    public abstract class CardStack
    {
        public List<Card> _cards = new List<Card>();


        public CardStack()//bool isMainDeck)
        {

            //if (isMainDeck)
            //    Init();
        }


        public int Size()
        {
            return _cards.Count;
        }

        public Card Pop(){

            int index = _cards.Count - 1;
            if (index == -1)
                throw new DeckEmptyException("No more card in the deck!");

            var toReturn = _cards[index];
            _cards.RemoveAt(index);
            return toReturn;
        }

        public void Push(params Card[] cards){
            foreach(var card in cards)
                _cards.Add(card);
        }


    }
}

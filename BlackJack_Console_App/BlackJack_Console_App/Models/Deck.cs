using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlackJack_Console_App.Models.Card;

namespace BlackJack_Console_App.Models
{
    public class Deck
    {
        public List<Card> _cards = new List<Card>();

        //to keep track if player has received AS
        //can be used later to decide if the AS can stay a 1 or increased to 11
        public  bool hasRcvAce { get; private set; }

        public bool hasStand { get; set; }

        void Init(){     
                 
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (var val in CardValues)
                {
                    _cards.Add(new Card(suit, val));
                }   
            }

            //initial Shuffle
            Shuffle();
        }

        public int Size()
        {
            return _cards.Count;
        }
        public Card Pop(){

            int index = _cards.Count - 1;
            if (index == -1)
                throw new Exception("No more card in the deck!");

            var toReturn = _cards[index];
            _cards.RemoveAt(index);
            return toReturn;
        }

        public void Push(params Card[] cards){
            foreach(var card in cards)
                _cards.Add(card);
        }

        public void Shuffle(){

            Random random = new Random();
            var max = _cards.Count;
            for (int i = 0; i < max; i++)
            {
                var temp = _cards[i];
                var index = random.Next(max);
                _cards[i] = _cards[index];
                _cards[index] = temp;
            }
        }

        public Deck(bool isMainDeck){
            hasRcvAce = false;
            hasStand = false;
            if(isMainDeck)
                Init();
        }

        public int GetScore() { 

            int score = 0;

            var figures = new string[4] { "10", "J", "Q", "K" };

            foreach (var card in _cards)
            {

                if (figures.Contains(card.Value))
                {
                    score += 10;
                }
                else if ("A".Equals(card.Value))
                {
                    if (score == 10)
                        score += 11;
                    else
                        score += 1;

                    hasRcvAce = true;
                }
                else
                {
                    score += int.Parse(card.Value);
                }

            }

            if (hasStand && hasRcvAce && score <= 11)
                score += 10;

            return score;
        }

        public override string ToString()
        {
            string str = "";

            foreach (var card in _cards)
            {
                str += card + ", ";
            }

            str += " Total: "+ GetScore();
            return str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Models
{
    public class Player
    {
        
        public Deck MainDeck, SideDeck;
        public Deck hand;

        public string Name { get; private set; }

        private int score;
    
        public Player(string name, Deck mainDeck, Deck sideDeck)
        {
            this.Name = name;
            this.MainDeck = mainDeck;
            this.SideDeck = sideDeck;

            hand = new Deck(isMainDeck:false);
            score = 0;
        }

        public void IncreaseScore()
        {
            ++score;
        }

        public int GetScore()
        {
            return score;
        }

        public void ResetScore()
        {
            score = 0;
        }

        public int Hit()
        {
            int _score = hand.GetScore();

            //hand.hasStand = false;

            if (_score < 21)
            {
                Card card = MainDeck.Pop();
                hand.Push(card);

                if(hand.GetScore()> 21)
                    throw new Exception(String.Format("Cards add up over 21. {0} loses!", Name));

                return hand.GetScore();
            }
            else if (score == 21)
                throw new Exception(string.Format("Cards add up to 21. {0} should stand!", Name));
            else
                throw new Exception(String.Format("Cards add up over 21. {0} loses!",Name));

        }

        public int Stand(int opponentScore)
        {
            if (hand.GetScore() < opponentScore)
                throw new Exception(String.Format("{0} cannot stand!", Name));
            else
            {
                hand.hasStand = true;
                //throw new Exception(String.Format("{0} stands!", Name));
                return hand.GetScore();
            }  
        }

        public void FlushHand() {

            SideDeck.Push(hand._cards.ToArray());
            hand = new Deck(isMainDeck: false);

        }

    }
}

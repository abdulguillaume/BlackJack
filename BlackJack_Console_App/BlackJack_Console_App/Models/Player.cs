using BlackJack_Console_App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Models
{
    public class Player
    {
        private static int id = 0;
       // public Deck MainDeck, SideDeck;
        public Hand hand;

        private int playerId;

        private Game game;

        public string Name { get; private set; }

        private int score;
    
        public Player(string name, Game game)//Deck mainDeck, Deck sideDeck)
        {
            this.Name = name;
            //this.MainDeck = mainDeck;
            //this.SideDeck = sideDeck;
            playerId = ++id;
            this.game = game;
            hand = new Hand();
            score = 0;
        }

        private Player getOpponent()
        {
            return this.game.dealer.Equals(this) ? this.game.player : this.game.dealer;
        }

        public int getId() {
            return playerId;
        }

        public int GetHandPts()
        {
            return this.hand.GetPts();
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

            int _score = hand.GetPts();

            Player opponent = getOpponent();

            if (_score < 21)
            {
                game.turn++;
                Card card = game.main_deck.Pop();
                hand.Push(card);

                if (hand.GetPts() > 21)
                {
                    throw new LoseGameException(String.Format("Cards add up over 21.\n{0}: {1}\n{2}: {3}\n{4} loses!", Name, hand, opponent.Name, opponent.hand, Name));
                }
                else if (hand.GetPts() == 21 && opponent.GetHandPts() == 21)
                {
                    throw new TieGameException(string.Format("\n{0}: {1}\n{2}: {3}\nIt's a tie!", Name, hand, opponent.Name, opponent.hand));
                }
                else if ( hand.GetPts() == 21 && opponent.GetHandPts() != 21 && game.turn % 2 == 0 )
                {
                    throw new WinGameException(string.Format("\n{0}: {1}\n{2}: {3}\n{4} wins!", Name, hand, opponent.Name, opponent.hand, Name));
                }
                else if ( hand.GetPts() != 21 && opponent.GetHandPts() == 21 && game.turn % 2 == 0 )
                {
                    throw new WinGameException(string.Format("\n{0}: {1}\n{2}: {3}\n{4} wins!",opponent.Name, opponent.hand, Name, hand, opponent.Name));
                }

            }

            return hand.GetPts();

        }

        public int Stand()
        {
            Player opponent = getOpponent();
            hand.hasStand = true;
            if (hand.GetPts() < opponent.GetHandPts())
            {
                hand.hasStand = false;
                throw new CannotStandException(String.Format("{0} cannot stand!", Name));
            }
                game.turn++;
                return hand.GetPts();
       
        }

        public void FlushHand() {

            game.side_deck.Push(hand._cards.ToArray());
            hand = new Hand();

        }

    }
}

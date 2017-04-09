using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Models
{
    public class Game
    {
        Deck main_deck, side_deck;

        Player player, dealer;

        public Game(){
            main_deck = new Deck(isMainDeck: true);
            side_deck = new Deck(isMainDeck: false);
            player = new Player("Player", main_deck, side_deck);
            dealer = new Player("Dealer", main_deck, side_deck);
        }

        public void Start() {

            int player_score, dealer_score;

            dealer.Hit();
            player_score = player.Hit();
            dealer_score = dealer.Hit();

            int turn = 0; //turn%2 = 1 => player's turn, turn%2 = 0 => dealer's turn

            while (true) {

                turn++;

                Console.WriteLine("\nType 'h' to hit, 's' to stand and 'r' to restart game.\n-------------\n");
                Console.WriteLine(string.Format("{0} score: +{1}, {2} score: +{3}", player.Name, player.GetScore(), dealer.Name, dealer.GetScore()));
                Console.WriteLine(String.Format("Main deck size: {0}, side deck size: {1}", main_deck.Size(), side_deck.Size()));
                Console.WriteLine("Dealer hand: " + dealer.hand);
                Console.WriteLine("Player hand: " + player.hand);
                Console.WriteLine("--Select option-- ");

                string str="";

                try
                {
                    
                    Flush(out player_score, out dealer_score);

                    str = Console.ReadLine();

                    switch (str)
                    {
                        case "h":Console.WriteLine("\nPlayer hits!\n");
                            player_score = player.Hit();
                            dealer_score = DealerLogic();
                            break;
                        case "s":Console.WriteLine("\nPlayer stands!\n");
                            player.Stand(dealer.GetScore());
                            dealer_score = DealerLogic();
                            break;
                        case "r":
                            break;
                        default:
                            Console.WriteLine("\nThis is not an option!");
                            break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
               

                if (str == "q") break;

                

            }

        }

        private void Flush(out int player_score, out int dealer_score)
        {
            dealer_score = 0;
            player_score = 0;
            
            if (dealer.hand.GetScore() > 21)
            {
                dealer.FlushHand();
                player.FlushHand();
                player.IncreaseScore();
                throw new Exception(String.Format("\nCards add up over 21. {0} loses!", dealer.Name));
            }
            else if (player.hand.GetScore() > 21)
            {
                dealer.FlushHand();
                player.FlushHand();
                dealer.IncreaseScore();
                throw new Exception(String.Format("\nCards add up over 21. {0} loses!", player.Name));
            } 
            else 
            {
                dealer_score = dealer.hand.GetScore();
                player_score = player.hand.GetScore();
            }

        }

        private int DealerLogic()
        {
            var player_score = player.hand.GetScore();
            if (player_score <= dealer.hand.GetScore())
            {
                Console.WriteLine("\nDealer stands!\n");
                dealer.Stand(player_score);
            }
            else if (player_score <= 21)
            {
                Console.WriteLine("\nDealer hits!\n");
                dealer.Hit(); 
            }

            return dealer.hand.GetScore();
        }

        public void ReStart() {
            //sideplayer.hand.Pop()
            dealer.FlushHand();
            player.FlushHand();
            main_deck.Push(side_deck._cards.ToArray());
            main_deck.Shuffle();

        }
    }
}

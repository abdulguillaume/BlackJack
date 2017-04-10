using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Models
{
    public class Game
    {
        public Deck main_deck, side_deck;

        public Player player, dealer;

        public int turn = 0; //whether you hit or stand it is considered an action
        //turn allow me to know if both players have played.
        //in some other cases there is no need for both players to play
        //for instance, if one player exceed 21, automatically the other one is the winner.
        //turn%2==0 => both players have played

        public Game(){

        }

        private void Init()
        {
            main_deck = new Deck(isMain: true);
            side_deck = new Deck(isMain: false);
            dealer = new Player("Dealer", this); //dealer is the first player in the game, playerId=1, will always play last
            player = new Player("Player", this);
        }

        public void Start() {

            Init();

            int player_score, dealer_score;

            while (true) {

                Console.WriteLine("\nType 'h' to hit, 's' to stand and 'r' to restart game.\n-------------\n");
                Console.WriteLine(string.Format("{0} score: +{1}, {2} score: +{3}", player.Name, player.GetScore(), dealer.Name, dealer.GetScore()));
                Console.WriteLine(String.Format("Main deck size: {0}, side deck size: {1}", main_deck.Size(), side_deck.Size()));
                Console.WriteLine("Dealer hand: " + dealer.hand);
                Console.WriteLine("Player hand: " + player.hand);
                Console.WriteLine("--Select option-- ");

                string str="";

                try
                {
                   
                    str = Console.ReadLine();

                    switch (str)
                    {
                        case "h":Console.WriteLine("\nPlayer hits!\n");
                            player_score = player.Hit();
                            dealer_score = DealerLogic(); 
                            break;
                        case "s":Console.WriteLine("\nPlayer stands!\n");
                            player.Stand();
                            dealer_score = DealerLogic(); 
                            break;
                        case "r":
                            ReStart();
                            break;
                        default:
                            Console.WriteLine("\nThis is not a valid option!");
                            break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Flush(out player_score, out dealer_score);
                }
               

                if (str == "q") break;

                

            }

        }

        private void Flush(out int player_score, out int dealer_score)
        {
            dealer_score = 0;
            player_score = 0;

            var dealerPts = dealer.hand.GetPts();
            var playerPts = player.hand.GetPts();

            if (dealerPts > 21 || (playerPts == 21 && dealerPts != 21))
            {
                dealer.FlushHand();
                player.FlushHand();
                player.IncreaseScore();
            }
            else if (playerPts > 21 || (dealerPts == 21 && playerPts != 21))
            {
                dealer.FlushHand();
                player.FlushHand();
                dealer.IncreaseScore();
            }
            else if (dealerPts == 21 && playerPts == 21)
            {
                dealer.FlushHand();
                player.FlushHand();
            }

        }


        private int DealerLogic()
        {
            var player_score = player.GetHandPts();
            if (player_score <= dealer.GetHandPts())
            {
                Console.WriteLine("\nDealer stands!\n");
                dealer.Stand();
            }
            else if (player_score <= 21)
            {
                Console.WriteLine("\nDealer hits!\n");
                dealer.Hit(); 
            }

            return dealer.GetHandPts();
        }

        public void ReStart() {

            dealer.FlushHand();
            player.FlushHand();
            dealer.ResetScore();
            player.ResetScore();
            main_deck.Push(side_deck._cards.ToArray());
            side_deck = new Deck(isMain: false);
            main_deck.Shuffle();
            turn = 0;

        }
    }
}

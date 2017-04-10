using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlackJack_Console_App.Models;
using Moq;
using System.Collections.Generic;
using BlackJack_Console_App.Exceptions;

namespace BlackJackTest
{

    [TestClass]
    public class BasicUnitTest
    {
        [TestMethod]
        public void GetPointsInHand() {

            List<Card> _cards1 = new List<Card>();
            _cards1.Add(new Card(Card.Suits.CLUBS, "A"));
            _cards1.Add(new Card(Card.Suits.CLUBS, "10"));
            Hand hand1 = new Hand();
            hand1._cards = _cards1;

            List<Card> _cards2 = new List<Card>();
            _cards2.Add(new Card(Card.Suits.CLUBS, "10"));
            _cards2.Add(new Card(Card.Suits.CLUBS, "A"));
            Hand hand2 = new Hand();
            hand2._cards = _cards2;

            List<Card> _cards3 = new List<Card>();
            _cards3.Add(new Card(Card.Suits.CLUBS, "A"));
            _cards3.Add(new Card(Card.Suits.CLUBS, "8"));
            Hand hand3 = new Hand();
            hand3._cards = _cards3;

            List<Card> _cards4 = new List<Card>();
            _cards4.Add(new Card(Card.Suits.CLUBS, "A"));
            _cards4.Add(new Card(Card.Suits.CLUBS, "A"));
            Hand hand4 = new Hand();
            hand4._cards = _cards4;

            List<Card> _cards5 = new List<Card>();
            _cards5.AddRange(_cards4); //A A
            _cards5.Add(new Card(Card.Suits.CLUBS, "9"));
            Hand hand5 = new Hand();
            hand5._cards = _cards5;

            List<Card> _cards6 = new List<Card>();
            _cards6.AddRange(_cards4); //A A
            _cards6.Add(new Card(Card.Suits.CLUBS, "8"));
            Hand hand6 = new Hand();
            hand6._cards = _cards6;

            //but with the same hand6 if the player decide to stand the opponent has a
            //greather hand points than his, Ace will be considered as 11.
            //Because the idea is that if you want to stand it is because you have
            //considered having a greather hand points
            Deck main_deck = new Deck(isMain: false);
            Deck side_deck = new Deck(isMain: false);
            Game game = new Game();
            game.main_deck = main_deck;
            game.side_deck = side_deck;
            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);
            game.dealer = dealer;
            game.player = player;
            player.hand._cards = _cards6; //total 10=8 A A, but if stand and opponent pts=14 => Ace will be 11 instead of 1=> 20 instead of 10.
            dealer.hand._cards = new List<Card>() {
                new Card(Card.Suits.CLUBS, "10"),
            new Card(Card.Suits.CLUBS, "4")
            };
            player.Stand();
            int test = player.GetHandPts();


            Assert.IsTrue(hand1.GetPts() == 21
                && hand2.GetPts() == 21
                && hand3.GetPts() == 9
                && hand4.GetPts() == 2
                && hand5.GetPts() == 21
                && hand6.GetPts() == 10
                && player.GetHandPts()==20);
            

        }


        [TestMethod]
        [ExpectedException(typeof(CardNotValidException))]
        public void CardNotValid() {
            new Card(Card.Suits.CLUBS, "14");
        }

        [TestMethod]
        public void MakeSureBothPlayerHasPlayedBeforeDecision()
        {
            List<Card> _cards = new List<Card>();
            _cards.Add(new Card(Card.Suits.CLUBS, "2"));//suit is not very important

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            _cards.Add(new Card(Card.Suits.CLUBS, "4"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //10
            dealer.Hit(); //4
            player.Stand();
            dealer.Hit(); //4 A
            player.Hit(); //10 A (win prospect, but waits until dealer to throw exception
            bool witness = false;
            try
            {
                dealer.Hit(); //4 A 2
            }
            catch (WinGameException e)
            {
                witness = true;
            }

            Assert.IsTrue(
                witness &&
                game.turn % 2 == 0
                );
        }

        [TestMethod]
        public void DealerCanStandPlayerHasMorePointsAceConsiderAs11()
        {
            List<Card> _cards = new List<Card>();
            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            _cards.Add(new Card(Card.Suits.CLUBS, "4"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //10
            dealer.Hit(); //4
            player.Stand();
            dealer.Hit(); //4 A (initially 5
            player.Stand();//10
            dealer.Stand();//can actually stand, now the Ace is worth 11. => 4+11=15

            Assert.IsTrue(dealer.GetHandPts() == 15);
        }

        [TestMethod]
        public void PlayerHitMainDeckDecrease()
        {
            List<Card> _cards = new List<Card>();
            _cards.Add(new Card(Card.Suits.CLUBS, "A"));//suit is not very important

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "4"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit();

            Assert.IsTrue(player.hand.Size()==1 
                && main_deck.Size()==2);
        }


        [TestMethod]
        [ExpectedException(typeof(DeckEmptyException))]
        public void PlayerHitMainDeckEmpty()
        {
            Deck main_deck = new Deck(isMain: false)
            {
                _cards = new List<Card>()
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit();
        }

        [TestMethod]
        public void PlayerFlushHandSideDeckIncrease()
        {
            List<Card> _cards = new List<Card>();
            _cards.Add(new Card(Card.Suits.CLUBS, "A"));//suit is not very important

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "4"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit();

            player.FlushHand();

            Assert.IsTrue(player.hand.Size() == 0
                    && side_deck.Size() == 1 &&
                    main_deck.Size() == 2);

        }

        [TestMethod]
        [ExpectedException(typeof(CannotStandException))]
        public void PlayerCannotStandOponnentHasMorePts()
        {
            List<Card> _cards = new List<Card>();
            _cards.Add(new Card(Card.Suits.CLUBS, "A"));//suit is not very important

            _cards.Add(new Card(Card.Suits.CLUBS, "5"));

            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "9"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //9
            dealer.Hit(); //10
            player.Hit(); //3
            dealer.Hit(); //5
            player.Stand(); //exception

        }

        [TestMethod]
        public void PlayerCanStandOponnentHasMorePtsButAceIsConsiderAs11Simple()
        {
            List<Card> _cards = new List<Card>();
            _cards.Add(new Card(Card.Suits.CLUBS, "A"));//suit is not very important

            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "5"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //A
            dealer.Hit(); //10
            player.Hit(); //5
            dealer.Hit(); //3
            player.Stand(); //exception //now player hand is 16

            Assert.IsTrue(player.hand.GetPts() == 16);

        }

        [TestMethod]
        public void PlayerCanStandOponnentHasMorePtsButAceIsConsiderAs11Complex()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "4"));

            _cards.Add(new Card(Card.Suits.CLUBS, "5"));

            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "5"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //A
            dealer.Hit(); //10
            player.Hit(); //5
            dealer.Hit(); //3  => 10+3
            player.Stand(); //exception //now player hand is 16
            dealer.Hit(); // 5 => 10+3+5 = 18
            player.Hit(); // 4 => 11+5+4 = 20

            Assert.IsTrue(player.hand.GetPts() == 20);

        }



        [TestMethod]
        [ExpectedException(typeof(WinGameException))]
        public void PlayerWinBlackJackSimple()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //A
            dealer.Hit(); //10
            player.Hit(); //10
            dealer.Hit(); //3  

        }


        [TestMethod]
        [ExpectedException(typeof(WinGameException))]
        public void PlayerWinBlackJackComplex()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "2"));

            _cards.Add(new Card(Card.Suits.CLUBS, "2"));

            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "8"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //A
            dealer.Hit(); //10
            player.Hit(); //8
            dealer.Hit(); //3  
            player.Hit(); //2
            dealer.Hit(); //2 

        }

        [TestMethod]
        [ExpectedException(typeof(WinGameException))]
        public void DealerWinBlackJackSimple()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //8
            dealer.Hit(); //A
            player.Hit(); //10
            dealer.Hit(); //10  

        }


        [TestMethod]
        [ExpectedException(typeof(WinGameException))]
        public void DealerWinBlackJackComplex()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "2"));
            _cards.Add(new Card(Card.Suits.CLUBS, "2"));            

            _cards.Add(new Card(Card.Suits.CLUBS, "8"));
            _cards.Add(new Card(Card.Suits.CLUBS, "3"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));
            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //10
            dealer.Hit(); //A
            player.Hit(); //3
            dealer.Hit(); //8  
            player.Hit(); //2
            dealer.Hit(); //2 

        }

        [TestMethod]
        [ExpectedException(typeof(LoseGameException))]
        public void PlayerGoesOver21()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "2"));
            _cards.Add(new Card(Card.Suits.CLUBS, "2"));

            _cards.Add(new Card(Card.Suits.CLUBS, "2"));
            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));
            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //10
            dealer.Hit(); //A
            player.Hit(); //10
            dealer.Hit(); //2  
            player.Hit(); //2
            dealer.Hit(); //2 

        }


        [TestMethod]
        [ExpectedException(typeof(TieGameException))]
        public void TieGame()
        {
            List<Card> _cards = new List<Card>();

            _cards.Add(new Card(Card.Suits.CLUBS, "10"));
            _cards.Add(new Card(Card.Suits.CLUBS, "10"));

            _cards.Add(new Card(Card.Suits.CLUBS, "A"));
            _cards.Add(new Card(Card.Suits.CLUBS, "A"));

            //_cards.Add(new Card(Card.Suits.CLUBS, "10"));
            //_cards.Add(new Card(Card.Suits.CLUBS, "10"));

            Deck main_deck = new Deck(isMain: false)
            {
                _cards = _cards
            };

            Deck side_deck = new Deck(isMain: false);

            Game game = new Game
            {
                main_deck = main_deck,
                side_deck = side_deck
            };

            Player dealer = new Player("Dealer", game);
            Player player = new Player("Player", game);

            game.dealer = dealer;
            game.player = player;

            player.Hit(); //10
            dealer.Hit(); //10
            player.Hit(); //A
            dealer.Hit(); //A  

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlackJack_Console_App.Models.Card;

namespace BlackJack_Console_App.Models
{
    public class Deck : CardStack
    {
        public Deck(bool isMain) : base()
        {
            if(isMain)
                Init();
        }

        void Init()
        {

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

        public void Shuffle()
        {

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


    }
}

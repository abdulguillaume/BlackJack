using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console_App.Models
{
    public class Hand: CardStack
    {
        //to keep track if player has received AS
        //can be used later to decide if the AS can stay a 1 or increased to 11
        public bool hasRcvAce { get; private set; }

        public bool hasStand { get; set; }

        public Hand() : base()
        {
            hasRcvAce = false;
            hasStand = false;
        }


        public int GetPts()
        {

            int pts = 0;

            var figures = new string[4] { "10", "J", "Q", "K" };

            foreach (var card in _cards)
            {

                if (figures.Contains(card.Value))
                {
                    pts += 10;
                }
                else if ("A".Equals(card.Value))
                {
                    if (pts == 10)
                        pts += 11;
                    else
                        pts += 1;

                    hasRcvAce = true;
                }
                else
                {
                    pts += int.Parse(card.Value);
                }

            }

            if ((hasStand && hasRcvAce && pts <= 11) || (hasRcvAce && pts == 11))
                pts += 10;

            return pts;
        }

        public override string ToString()
        {
            string str = "";

            foreach (var card in _cards)
            {
                str += card + "  ";
            }

            str += string.Format(" (min +{0}pts)", GetPts());
            return str;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFileGenerator
{
    public class Player
    {
        public int Number { get; set; }
        public int TotalOutsMade { get; set; }
        public int TotalWalks { get; set; }
        public int TotalHits { get; set; }

        public Player() { }

        public Player(int PlayerNumber, int PlayerOutsMade = 0, int PlayerWalks = 0, int PlayerHits = 0)
        {
            Number = PlayerNumber;
            TotalOutsMade = PlayerOutsMade;
            TotalWalks = PlayerWalks;
            TotalHits = PlayerHits;
        }

        public decimal GetBattingAverage()
        {
            var denominator = (TotalHits + TotalOutsMade);

            if (denominator > 0)
            {
                var value = (decimal)TotalHits / denominator;
                return value;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetOnBasePercentage()
        {
            var denominator = (TotalHits + TotalOutsMade);

            if (denominator > 0)
            {
                var onBasePercentage = ((decimal)(TotalHits + TotalWalks) / denominator) * 100;

                return onBasePercentage;
            }
            else
            {
                return 0;
            }
        }
    }
}

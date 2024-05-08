using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMemoryGame
{
    public class PlayingCard
    {
        public Suit Suit { get; }
        public Rank Rank { get; }

        public PlayingCard(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardMemoryGame
{
    internal class DeckOfCards
    {
        private List<PlayingCard> cards;
        public DeckOfCards(String pathToAtlas)
        {
            cards = new List<PlayingCard>();
          
        }
    }
}

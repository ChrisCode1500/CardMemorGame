using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace CardMemoryGame
{
    internal class PlayingCardPrinter
    {
        private static Dictionary<(Suit, Rank), Image> cardImages;
        private static Image backOfCard;
        static PlayingCardPrinter()
        {
            cardImages = new Dictionary<(Suit, Rank), Image>();
            string relativePath = @"card-deck.png";
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            Image atlas = Image.FromFile(fullPath);
            int cardWidth = (atlas.Width / 13);
            int cardHeight = atlas.Height / 5; 

            for (int suit = 0; suit < 4; suit++)
            {
                for (int rank = 1; rank <= 13; rank++)
                {
                    // rank enums start at 1, but list indices start at 0
                    int x = ((rank - 1) * cardWidth);
                    int y = suit * cardHeight;
                    Rectangle cropArea = new Rectangle(x, y, cardWidth, cardHeight);
                    Image cardImage = CropImage(atlas, cropArea);
                    // Assuming Enum.GetValues returns in the order declared
                    cardImages.Add(((Suit)suit, (Rank)rank), cardImage);
                }
            }
            backOfCard = CropImage(
                atlas,
                new Rectangle(2 * cardWidth, 4 * cardHeight, cardWidth, cardHeight)
            );
        }
        private static Image CropImage(Image image, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(image);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public static Image GetCardImage(Suit suit, Rank rank)
        {
            return cardImages[(suit, rank)];
        }

        public static Image GetCardBack()
        {
            return backOfCard;
        }

    }
}

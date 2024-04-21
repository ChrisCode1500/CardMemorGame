using System.Windows.Forms;

namespace CardMemoryGame
{
    public partial class Form1 : Form
    {
        private List<PictureBox> pictureBoxes;
        private List<PlayingCard> playingCards;
        // 12 cards used for the memory game
        private List<PlayingCard> pickedCards;
        
        private System.Windows.Forms.Timer timer;
        private const int displayDuration = 3000; // 3 second duration

        // Keep track of flipped cards to compare
        private PictureBox? firstFlippedPictureBox;
        private PlayingCard firstFlippedCard;
        // matched cards
        private List<PictureBox> matchedPictureBoxes = new List<PictureBox>();

        // to stop the game from bugging out when cards are clicked too fast
        private bool processingClick = false;

        public Form1()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = displayDuration;
            timer.Tick += Timer_Tick;

            this.pictureBoxes =
            [
                pictureBox1,
                pictureBox2,
                pictureBox3,
                pictureBox4,
                pictureBox5,
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9,
                pictureBox10,
                pictureBox11,
                pictureBox12
            ];
            InitializePlayingCards();
            RandomizePlayingCards();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach(var pictureBox in pictureBoxes)
            {
                pictureBox.Image = PlayingCardPrinter.GetCardBack();
            }
            timer.Stop();
        }

        private void InitializePlayingCards()
        {
            this.playingCards = new List<PlayingCard>();

            foreach(Rank rank in Enum.GetValues(typeof(Rank)))
            {
                this.playingCards.Add(new PlayingCard(Suit.Clubs, rank));
                this.playingCards.Add(new PlayingCard(Suit.Spades, rank));
            }                               
        }

        private void PictureBoxClick(object? sender, EventArgs e)
        {
            if (processingClick)
            {
                return;
            }

            if (sender is PictureBox pictureBox && pictureBox.Tag is PlayingCard playingCard)
            {
                // Check if the back of the card is showing and if it's not already matched
                if (pictureBox.Image == PlayingCardPrinter.GetCardBack() && !matchedPictureBoxes.Contains(pictureBox))
                {
                    processingClick = true;

                    // Flip the card to show the front
                    pictureBox.Image = PlayingCardPrinter.GetCardImage(playingCard.Suit, playingCard.Rank);

                    if (firstFlippedPictureBox == null)
                    {
                        // First card flipped
                        firstFlippedPictureBox = pictureBox;
                        firstFlippedCard = playingCard;

                        processingClick = false;
                    }
                    else
                    {
                        // Second card flipped, compare ranks
                        if (firstFlippedCard.Rank == playingCard.Rank)
                        {
                            // Ranks match, keep both cards facing up and add them to matched cards list
                            matchedPictureBoxes.Add(firstFlippedPictureBox);
                            matchedPictureBoxes.Add(pictureBox);
                            firstFlippedPictureBox = null; // Reset for the next pair

                            processingClick = false;

                            // Win condition
                            if (matchedPictureBoxes.Count == pictureBoxes.Count)
                            {
                                MessageBox.Show("YOU WIN!");

                                // Close game
                                Close();
                            }
                        }
                        else
                        {
                            // Ranks don't match, flip both cards back over
                            Task.Delay(1000).ContinueWith(_ =>
                            {
                                firstFlippedPictureBox.Image = PlayingCardPrinter.GetCardBack();
                                pictureBox.Image = PlayingCardPrinter.GetCardBack();
                                firstFlippedPictureBox = null; // Reset for the next pair

                                processingClick = false;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                    }
                }
            }
        }

        private void RandomizePlayingCards()
        {
            var randomize = new Random();
            pickedCards = new List<PlayingCard>();
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                pickedCards.Add(new PlayingCard(playingCards[i].Suit, playingCards[i].Rank));
            }
            pickedCards = pickedCards.OrderBy(card => randomize.Next()).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                PlayingCard card = pickedCards[i];
                this.pictureBoxes.ElementAt(i).Image = PlayingCardPrinter.GetCardImage(pickedCards[i].Suit, pickedCards[i].Rank);
                this.pictureBoxes.ElementAt(i).Tag = card;
            }

            // Subscribe to PictureBoxClick event handler outside of the loop
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.Click += PictureBoxClick;
            }

            timer.Start();
        }
    }
}

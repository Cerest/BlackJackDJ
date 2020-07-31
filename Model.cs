using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACNS_Blackjack
{
    class Model
    {
        public class Card
        {
            #region ctor
            public Card()
            {
                cardValue = 0;
                suit = 0;
                image = null;
            }
            #endregion
            #region properties
            

            Image image;
            CardValue cardValue;
            CardSuit suit;

            public Image Image
            {
                get
                {
                    return this.image;
                }
            }

            public CardValue Value
            {
                get
                {
                    return this.cardValue;
                }
                set
                {
                    this.cardValue = value;
                    GetImage();
                }
            }

            public CardSuit Suit
            {
                get
                {
                    return this.suit;
                }
                set
                {
                    this.suit = value;
                    GetImage();
                }
            }

            #endregion
            #region methods
            private void GetImage()
            {
                if (this.Suit != 0 && this.Value != 0)//so it must be a valid card (see the Enums)
                {
                    int x = 0;//starting point from the left
                    int y = 0;//starting point from the top. Can be 0, 98, 196 and 294
                    int height = 97;
                    int width = 73;

                    switch (this.Suit)
                    {
                        case CardSuit.Hearts:
                            y = 196;
                            break;
                        case CardSuit.Spades:
                            y = 98;
                            break;
                        case CardSuit.Clubs:
                            y = 0;
                            break;
                        case CardSuit.Diamonds:
                            y = 294;
                            break;
                    }

                    x = width * ((int)this.Value - 1); //the Ace has the value of 1 (see the Enum), so the X coordinate will be the starting (first one), that's why we have to subtract 1. The card 6 has the total width of the first 6 cards (6*73=438) minus the total width of the first 5 cards (5*73=365). Of course it is 73. The starting X coordinate is at the end of the 5th card (or the start of the left side of the 6th card). Hope you understand. :)

                    Bitmap source = new Bitmap(Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../..//cards.png")));//the original cards.png image
                    Bitmap img = new Bitmap(width, height);//this will be the created one for each card
                    Graphics g = Graphics.FromImage(img);
                    g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);//here we slice the original into pieces
                    g.Dispose();
                    this.image = img;  
                }
            }
            #endregion
        }
        public class Hand
        {
            private List<Card> cards;

            public List<Card> Cards
            {
                get { return cards; }
            }

            public Hand(int startingHand, Deck deck)
            {
                if (deck == null) throw new DeckException("No decks available to draw from!");
                else if (deck.Cards.Count == 0) throw new DeckException("No more cards to draw!");
                else
                {
                    cards = new List<Card>();
                    for (int i = 0; i < startingHand; i++)
                    {
                        deck.DrawCard(this);
                    }
                }
            }

            public void AddValue(Card drawn, ref int currentSum)
            {
                if (drawn.Value == CardValue.Ace)
                {
                    if (currentSum <= 10)
                    {
                        currentSum += 11;
                    }
                    else
                    {
                        currentSum += 1;
                    }
                }
                else if (drawn.Value == CardValue.Jack || drawn.Value == CardValue.Queen || drawn.Value == CardValue.King)
                {
                    currentSum += 10;
                }
                else
                {
                    currentSum += (int)drawn.Value;
                }
            }
        }
        public class Deck
        {
            private List<Card> cards;

            public List<Card> Cards
            {
                get { return this.cards; }
                set { this.cards = value; }
            }
            public Deck()
            {
                Cards = new List<Card>();
                ShuffleNewDeck();
            }

            public void ShuffleNewDeck()
            {
                cards.Clear();
                for (int i = 1; i < 5; i++)//CardSuits
                {
                    for (int j = 1; j < 14; j++)//CardValues: 2...10,J,Q,K,A = 13 different values 
                    { Card card = new Card(); 
                        card.Value = (CardValue)j; 
                        card.Suit = (CardSuit)i; 
                        cards.Add(card); 
                    } 
                } 
                Random r = new Random(); 
                cards = cards.OrderBy(x => r.Next()).ToList();
            }
            public Card DrawCard(Hand hand)
            {
                Card drawn = cards[cards.Count - 1];
                cards.Remove(drawn);
                hand.Cards.Add(drawn);
                return drawn;
            }
        }
        public enum CardValue
            {
                Ace = 1,
                Two = 2,
                Three = 3,
                Four = 4,
                Five = 5,
                Six = 6,
                Seven = 7,
                Eight = 8,
                Nine = 9,
                Ten = 10,
                Jack = 11,
                Queen = 12,
                King = 13
            }
        public enum CardSuit
            {
                Hearts = 1,
                Spades = 2,
                Clubs = 3,
                Diamonds = 4
            }
        #region custom exceptions
        [Serializable]
        internal class DeckException : Exception
        {
            public DeckException()
            {
            }

            public DeckException(string message) : base(message)
            {
            }

            public DeckException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DeckException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        #endregion
    }
}

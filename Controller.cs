using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static ACNS_Blackjack.Model;

namespace ACNS_Blackjack
{
    class Controller
    {
        int PlayerSum;
        int ComputerSum;
        bool playing;
        object PlayLock;
        object TurnLock;
        public int wallet = 100;
        public int pot = 0;

        Deck deck;
        public Hand dealerHand;
        public Hand playerHand;
        private string playerAction;
        StackPanel dealerField;
        StackPanel playerField;
        Border mask;
        Label wa;
        Label po;

        public bool Playing
        {
            get
            {
                lock (PlayLock)
                {
                    return playing;
                }
            }
            set
            {
                lock (PlayLock)
                {
                    playing = value;
                }
            }
        }

        public string PlayerAction
        {
            get
            {
                lock (TurnLock)
                {
                    return playerAction;
                }
            }
            set
            {
                lock (TurnLock)
                {
                    playerAction = value;
                }
            }
        }

        public Controller(StackPanel d, StackPanel s, Border m, Label w, Label p)
        {
            deck = new Deck();
            PlayLock = new object();
            TurnLock = new object();
            dealerField = d;
            playerField = s;
            mask = m;
            po = p;
            wa = w;
        }

        public void PlayGame()
        {
            Playing = true;
            dealerHand = new Hand(2, deck);
            playerHand = new Hand(2, deck);
            Application.Current.Dispatcher.Invoke(() =>
            {
                mask.Visibility = Visibility.Visible;
            });
            foreach (Model.Card c in dealerHand.Cards)
            {
                AddCardVisual(dealerField, c);
            }
            foreach (Model.Card c in playerHand.Cards)
            {
                AddCardVisual(playerField, c);
            }

            PlayerSum = GetHandValue(playerHand);
            ComputerSum = GetHandValue(playerHand);
            CheckWin();
            PlayerTurn();
            DealerTurn();
        }
        private void AddCardVisual(StackPanel field, Model.Card c)
        {
            string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../..//" + c.Value + "_of_" + c.Suit + ".bmp");
            Application.Current.Dispatcher.Invoke(() =>
            {

                Image cardImage = new Image();
                cardImage.Width = 73;
                cardImage.Height = 97;
                cardImage.Margin = new Thickness(2);
                cardImage.Source = new BitmapImage(new Uri(s));
                field.Children.Add(cardImage);
            });
        }

        public void PlayerTurn()
        {
            while (Playing)
            {
                if (PlayerAction == "")
                {
                    continue;
                }
                else
                {
                    switch (PlayerAction)
                    {
                        case "Hit":
                            PlayerAction = "";
                            Hit(playerField, playerHand);
                            PlayerSum = GetHandValue(playerHand);
                            CheckBust();
                            CheckWin(); 
                            break;
                        case "Stand":
                            PlayerAction = "";
                            PlayerSum = GetHandValue(playerHand); 
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                mask.Visibility = Visibility.Hidden;
                            });
                            return;
                    }
                }
            }
        }

        private void CheckWin()
        {
            if (!Playing)
            {
                if (PlayerSum == ComputerSum)
                {
                    MessageBox.Show("Tie game!");
                    Console.Beep(587, 333);
                    Console.Beep(261, 334);
                    Console.Beep(658, 333); 
                    CleanUp();
                }
                if (PlayerSum == 21 || PlayerSum > ComputerSum)
                {
                    MessageBox.Show("You win!"); 
                    Console.Beep(370, 500);
                    Console.Beep(392, 250);
                    Console.Beep(330, 250);
                    Console.Beep(440, 500);
                    pot += pot * 2;
                    CleanUp();
                }
                if (ComputerSum == 21 || ComputerSum > PlayerSum)
                {
                    MessageBox.Show("Dealer wins!");
                    Console.Beep(261, 400);
                    Console.Beep(174, 600);
                    pot = 0;
                    CleanUp();
                }
            }
        }

        public void CheckBust()
        {
            if (ComputerSum > 21) 
            { 
                MessageBox.Show("You win!");
                Console.Beep(370, 500);
                Console.Beep(392, 250);
                Console.Beep(330, 250);
                Console.Beep(440, 500);
                Console.Beep(600, 400);
                Console.Beep(261, 400);
                pot = pot * 2;
                CleanUp(); 
            }
            if (PlayerSum > 21) 
            { 
                MessageBox.Show("Dealer wins!");
                Console.Beep(261, 400);
                Console.Beep(174, 600);
                pot = 0;
                CleanUp(); 
            }
        }
        private void CleanUp()
        {
            Playing = false;
            wallet += pot;
            pot = 0;
            dealerHand.Cards.Clear();
            playerHand.Cards.Clear();
            Application.Current.Dispatcher.Invoke(() =>
            {
                wa.Content = "$" + wallet;
                po.Content = "$0";
            });
            Application.Current.Dispatcher.Invoke(() =>
            {
                dealerField.Children.Clear();
                playerField.Children.Clear();
            });
            if (wallet == 0)
            {
                MessageBox.Show("Broke! Better luck next time!");
                Console.Beep(300, 500);
                Console.Beep(300, 500);
                Console.Beep(174, 1000);
                System.Environment.Exit(0);
            }
            deck.ShuffleNewDeck();
            PlayGame();
        }
        private void Hit(StackPanel field, Model.Hand hand)
        {
            AddCardVisual(field, deck.DrawCard(hand));
        }

        private void DealerTurn()
        {
            ComputerSum = GetHandValue(dealerHand);
            Console.Beep(261, 400);
            if (ComputerSum == 21)
            {
                Playing = false;
                CheckWin();
            }
            if (ComputerSum >= 16)
            {
                if (ComputerSum >= PlayerSum)
                {
                    Playing = false;
                    CheckBust();
                    CheckWin();
                }
                else
                {
                    Hit(dealerField, dealerHand);
                    ComputerSum = GetHandValue(dealerHand);
                    CheckBust();
                    CheckWin();
                    DealerTurn();
                }
            }
            else
            {
                Hit(dealerField, dealerHand);
                ComputerSum = GetHandValue(dealerHand);
                CheckBust();
                CheckWin();
                DealerTurn();
            }
        }

        private int GetHandValue(Model.Hand hand)
        {
            int i = 0;
            foreach (Model.Card c in hand.Cards)
            {
                hand.AddValue(c, ref i);
            }
            return i;
        }
    }

}

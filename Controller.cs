using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACNS_Blackjack
{
    class Controller
    {
        int PlayerSum;
        int ComputerSum;
        decimal decBet;
        bool PlayerDone;
        bool ComputerDone;
        bool PlayerTurn;
        Deck deck = new Deck();
        Hand dealerHand = new Hand(2, deck);
        Hand playerHand = new Hand(2, deck);

        private void CheckWin()
        {
            if (PlayerSum == 21 || PlayerSum > ComputerSum)
            {
                MessageBox.Show(String.Format("You win {0:C}!", (decBet + (decBet/3)) ));
            }
            if (ComputerSum == 21 || ComputerSum > PlayerSum)
            {
                MessageBox.Show(string.Format("You Lose {0}!", decBet));
            }
        }

        private void Fold()
        {
            if (PlayerDone)
            {
                PlayerSum = 0;
                if (ComputerSum <= PlayerSum)
                {
                    ComputerSum = 1;
                }
                CheckWin();
            }
            else if (ComputerDone)
            {
                ComputerSum = 0;
                if (PlayerSum <= ComputerSum)
                {
                    PlayerSum = 1;
                }
                CheckWin();
            }
        }
        private void hit(Model.Hand hand)
        {
            deck.DrawCard(hand);
        }
        private void Bust()
        {
            if (ComputerSum > 21)
            {
                ComputerSum = 0;
            }
            else if (PlayerSum > 21)
            {
                PlayerSum = 0;
            }
            CheckWin();
        }

        private void stand()
        {
            if (PlayerTurn)
            {
                DealerTurn();
            }
            else
            {

            }
        }

        private void DealerTurn()
        {
            if (ComputerSum >= 16)
            {
                PlayerTurn = false;
                int Max = dealerHand.cards.Max;
                int Min = dealerHand.cards[1];
                int delta = Max - Min;
                int rnd = new Random(0, delta);
                
                if (rnd >= 10)
                {
                    stand();
                }
                else
                {
                    hit(dealerHand);
                }
            }
            else
            {
                hit(dealerHand);
            }
        }
    }
}

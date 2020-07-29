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

        private void CheckWin()
        {
            if (PlayerSum == 21 || PlayerSum > ComputerSum)
            {
                MessageBox.Show(String.Format("You win {0:C}!", (decBet + (decBet/3)) ));
            }
            if (ComputerSum == 21 || ComputerSum > PlayerSum)
            {
                MessageBox.Show("You Lose!");
            }
        }
        
        public int GetHandValue(Model.Hand hand)
        {
            int sum = 0;
            foreach(Model.Card c in hand.Cards)
            {
                hand.AddValue(c, ref sum);
            }
            return sum;
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
        private void hit()
        {
            Model.Deck deck = new Model.Deck();
            Model.Hand hand = new Model.Hand(1, deck);
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
    }
}

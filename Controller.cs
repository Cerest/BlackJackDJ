using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static ACNS_Blackjack.Model;

namespace ACNS_Blackjack
{
    class Controller
    {
        int PlayerSum;
        int ComputerSum;
        decimal decBet;
        bool playing;
        object Lockobj;
        
        Deck deck;
        Hand dealerHand;
        Hand playerHand;
        string PlayerAction;

        public bool Playing 
        {
            get
            {
                lock (Lockobj)
                {
                    return playing;
                }
            }
            set
            {
                lock (Lockobj)
                {
                    playing = value;
                }
            }
        }

        public Controller()
        {
            deck = new Deck();
        }

        public void PlayGame()
        {
            Playing = true;
            dealerHand = new Hand(2, deck);
            playerHand = new Hand(2, deck);
            PlayerTurn();
            DealerTurn();
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
                            Hit(playerHand);
                            PlayerSum = GetHandValue(playerHand);
                            CheckBust();
                            break;
                        case "Stand":
                            Playing = false;
                            break;
                    }
                }
            } 
        }


        private void CheckWin()
        {
            if (PlayerSum == 21 || PlayerSum > ComputerSum)
            {
                //player win
            }
            if (ComputerSum == 21 || ComputerSum > PlayerSum)
            {
                //dealer win
            }
        }

        public void CheckBust()
        {
            if (ComputerSum > 21) ;//Player win
            if (PlayerSum > 21) ;//CPU win
        }
        private void Hit(Model.Hand hand)
        {
            deck.DrawCard(hand);
        }

        private void DealerTurn()
        {
            while (Playing)
            {
                if (ComputerSum >= 16)
                {
                    int rnd = new Random().Next(0, 10);
                    if (rnd >= 4)
                    {
                        Playing = false;
                    }
                    else
                    {
                        Hit(dealerHand);
                        ComputerSum = GetHandValue(dealerHand);
                    }
                } 
                else
                {
                    Hit(dealerHand);
                    ComputerSum = GetHandValue(dealerHand); 
                }
                CheckBust();
            }
            CheckWin();
        }
        
        private int GetHandValue(Model.Hand hand)
        {
            int i = 0;
            foreach(Model.Card c in hand.Cards)
            {
                hand.AddValue(c, ref i);
            }
            return i;
        }
    }

}

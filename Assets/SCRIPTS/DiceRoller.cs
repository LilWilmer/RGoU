using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    //FIELDS------------------------------------------------------------------
    public Ur UrHandler { get; set; }
    public bool DoneRolling { get; set; }
    public bool CanRoll { get; set; }
    public int[] diceValues = new int[4];
    public int DiceTotal { get; set; }
    public Sprite[] diceImageOne = new Sprite[3];
    public Sprite[] diceImageZero = new Sprite[3];
    public HashSet<RollObserver> rollObservers = new HashSet<RollObserver>();
    private TObserver observer;

    //CONSTRUCTOR-------------------------------------------------------------
    public DiceRoller() { }

    public void RollDice()
    {
        //LOGIC CLASS
        if (CanRoll)
        {
            this.DiceTotal = 0;
            for (int ii = 0; ii < diceValues.Length; ii++)
            {
                diceValues[ii] = Random.Range(0, 2);
                DiceTotal += diceValues[ii];

                //THIS SHOULD NOT BE THE RESPONSIBILITY OF THIS METHOD --- --- ---
                if (diceValues[ii] == 0)
                {
                    this.transform.GetChild(ii).GetComponent<Image>().sprite =
                        diceImageZero[Random.Range(0, diceImageZero.Length)];
                }
                else
                {
                    this.transform.GetChild(ii).GetComponent<Image>().sprite =
                        diceImageOne[Random.Range(0, diceImageZero.Length)];
                }
                // --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
            }
            this.DoneRolling = true;
            CanRoll = false;

            //ROLL EVENT ALTER --- --- --- --- ---
            if (DiceTotal == 0)
            {
                UrHandler.SkipTurn();
            }
            else
            {
                foreach (RollObserver ob in rollObservers)
                {
                    ob.RollUpdate(this.DiceTotal);
                }
            }
        }
    }

    public void AddObserver(RollObserver ob){ rollObservers.Add(ob); }

    
    //TRASH METHODS-----------------------------------------------------------
    public void Setup()
    {
        this.observer = new TObserver(this);
    }

    private class TObserver : TurnObserver
    {
        private DiceRoller dr;
        public TObserver(DiceRoller dr)
        {
            this.dr = dr;
            this.dr.UrHandler.AddTurnObserver(this);
        }

        public void TurnUpdate()
        {
            this.dr.CanRoll = true;
        }
    }
}

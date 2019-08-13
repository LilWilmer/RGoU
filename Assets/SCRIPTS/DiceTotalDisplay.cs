using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceTotalDisplay : MonoBehaviour
{

    //FIELDS------------------------------------------------------------------
    private RollObserver observer;
    public DiceRoller diceRoller;
    public Ur UrHandler { get; set; }

    //CONSTRUCTOR-------------------------------------------------------------
    public DiceTotalDisplay(DiceRoller diceRoller)
    {
        this.diceRoller = diceRoller;
        this.observer = new Observer(this);
    }
    //SETUP:
    public void Setup()
    {
        this.observer = new Observer(this);
    }
    //INNER CLASS=============================================================
    private class Observer : RollObserver, TurnObserver
    {
        //FIELDS--------------------------------------------------------------
        private DiceTotalDisplay dtd;

        //CONSTRUCTOR---------------------------------------------------------
        public Observer(DiceTotalDisplay dtd)
        {
            this.dtd = dtd;
            dtd.diceRoller.AddObserver(this);
            dtd.UrHandler.AddTurnObserver(this);
        }

        //UPDATE EVENT--------------------------------------------------------
        public void RollUpdate(int DiceTotal)
        {
            dtd.GetComponent<Text>().text = "= " + DiceTotal;
        }

        public void TurnUpdate()
        {
            dtd.GetComponent<Text>().text = "";
        }
    }
}

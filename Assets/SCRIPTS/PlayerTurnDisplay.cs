using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnDisplay : MonoBehaviour
{
    //FIELDS------------------------------------------------------------------
    private TurnObserver observer;
    public Ur UrHandler { get; set; }

    //SETUP:
    public void Setup()
    {
        this.observer = new Observer(this);
    }

    //INNER CLASS=============================================================
    private class Observer : TurnObserver
    {
        //FIELDS--------------------------------------------------------------
        private PlayerTurnDisplay ptd;

        //CONSTRUCTOR---------------------------------------------------------
        public Observer(PlayerTurnDisplay ptd)
        {
            this.ptd = ptd;
            ptd.UrHandler.AddTurnObserver(this);
        }

        //UPDATE EVENT--------------------------------------------------------
        public void TurnUpdate()
        {
            ptd.GetComponent<Text>().text = "Current Player: "
                +ptd.UrHandler.Players[ptd.UrHandler.CurrentPlayer].Name;
        }
    }
}

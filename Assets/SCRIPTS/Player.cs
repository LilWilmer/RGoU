using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //FIELDS==================================================================
    private Observer observer;
    public DiceRoller dr { get; set; }
    public Ur UrHandler { get; set; }
    public PlayerStone[] Pieces { get; set; }
    public int ID { get; set; }
    public string playerName;
    public string Name
    {
        get { return playerName; }
        set { this.playerName = value; }
    }
    public int Score { get; set; }
    public bool IsMyTurn { get; set; }


    /* * * * *
     * SETUP
     */
    public void Setup()
    {
        this.Score = 0;
        this.observer = new Observer(this);
        foreach(PlayerStone stone in Pieces)
        {
            stone.Owner = this;
        }
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * *
     * Checks to see if any pieces have a valid move
     */
     public bool CanMove()
    {
        bool HasValidMove = false;

        int ii = 0;
        while (ii < Pieces.Length && Pieces[ii].CanMoveTo() == false)
        {
            ii++;
        }
        if(ii < Pieces.Length)
        {
            HasValidMove = true;
        }

        return HasValidMove;
    }

    //INNER OBSERVER CLASSES==================================================
    private class Observer : TurnObserver, PieceObserver, RollObserver
    {
        Player p;

        public Observer(Player p)
        {
            this.p = p;

            //Observing the Turn Handler
            this.p.UrHandler.AddTurnObserver(this);

            //Observing the dice roller
            this.p.dr.AddObserver(this);

            //Observing the player stones
            foreach (PlayerStone stone in this.p.Pieces)
            {
                stone.AddObserver(this);
            }
        }

        /* * * * * * * *
         * ON NEW TURN 
         */
        public void TurnUpdate()
        {
            this.p.IsMyTurn = !this.p.IsMyTurn;

            if (p.IsMyTurn == false)
            {
                foreach (PlayerStone stone in p.Pieces)
                {
                    stone.CanMove = false;
                }
            }
        }

        /* * * * * * * *
         * ON DICE ROLL
         */
        public void RollUpdate(int DiceTotal)
        {

            if (this.p.IsMyTurn)
            {
                foreach (PlayerStone stone in this.p.Pieces)
                {
                    stone.MoveDistance = DiceTotal;
                    stone.CanMove = true;
                }
                if(p.CanMove() == false)
                {
                    p.UrHandler.SkipTurn();
                }
            }
        }

        /* * * * * * * * *
         * ON STONE CLICK
         */
        public void PieceClicked(PlayerStone stone)
        {
            if (stone.CanMoveTo())
            {
                foreach(PlayerStone s in p.Pieces)
                {
                    s.CanMove = false;
                }
                stone.StartMove();
            }
        }

        /* * * * * * * * *
         * ON STONE MOVE
         */
        public void PieceMoved(PlayerStone stone)
        {
            if (!p.dr.CanRoll)
            {
                this.p.UrHandler.NewTurn();
            }
        }
    }
}

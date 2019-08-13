/*****************************************************************************
 * AUTH: William
 * FILE: Ur.cs
 * DATE: 17/01/19
 * PURP: This script is responsible for handling player turns, score and
 *       player stone positions
 * **************************************************************************/

//CORE MECHANICS:
//ROLLS{set piece movement active}
//MOVES{run end turn}
//END TURN{swap current player and active cam}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ur : MonoBehaviour {

    //GAME CONSTS=============================================================
    public const int NUM_PLAYERS = 2;//unused
    public const int NUM_PIECES = 6;

    //FIELDS==================================================================
    //PLAYERS:
    public Player[] Players = new Player[NUM_PLAYERS];
    public int CurrentPlayer { get; set; }
    public int Winner { get; set; }

    //GAME BOARD ITEMS:
    public PlayerStone[] p1Stones = new PlayerStone[NUM_PIECES];
    public PlayerStone[] p2Stones = new PlayerStone[NUM_PIECES];
    public DiceRoller theDiceRoller;//part of ui too but that needs to change
    public UrTile[] safeSpaces;
    public UrTile[] DoubleTurns;
    public UrTile[] GoalTiles;

    //ANIMATIONS:
    public PlayAnimation stone;

    //UI ELEMENTS:
    public DiceTotalDisplay totalDisplay;
    public PlayerTurnDisplay turnDisplay;
    public CameraController cc;

    //OBSERVERS:
    public HashSet<TurnObserver> turnObservers = new HashSet<TurnObserver>();
    public HashSet<EndObserver> endObservers = new HashSet<EndObserver>();


    //END CONDITION:
    public bool Isfinished { get; set; }
    public MC_ChangeScene[] GameOverScreen = new MC_ChangeScene[NUM_PLAYERS];

    //METHODS=================================================================
    //SETUP:
    private void Start()
    {
        //PLAYER SETUP:
        //P1
        this.Players[0].ID = 0;
        this.Players[0].UrHandler = this;
        this.Players[0].dr = theDiceRoller;
        this.Players[0].Pieces = p1Stones;
        this.Players[0].IsMyTurn = false;
        this.Players[0].Setup();
        //P2
        this.Players[1].ID = 1;
        this.Players[1].UrHandler = this;
        this.Players[1].dr = theDiceRoller;
        this.Players[1].Pieces = p2Stones;
        this.Players[1].IsMyTurn = true;
        this.Players[1].Setup();

        this.CurrentPlayer = 0;

        //BOARD SETUP:
        //Safe spaces
        foreach (UrTile tile in safeSpaces)
        {
            tile.SetSafeSpace();
        }
        //Double Turn Tiles
        foreach( UrTile tile in DoubleTurns)
        {
            tile.SetDoubleTurnTile();
        }
        //Goal Tiles
        foreach (UrTile tile in GoalTiles)
        {
            tile.SetGoalTile();
        }

        //DICE SETUP:
        this.theDiceRoller.UrHandler = this;
        this.theDiceRoller.Setup();

        //UI SETUP:
        this.totalDisplay.UrHandler = this;
        this.totalDisplay.Setup();
        this.turnDisplay.UrHandler = this;
        this.turnDisplay.Setup();
        this.cc.UrHandler = this;
        this.cc.Setup();

        NewTurn();

    }

    /* * * * * *
     * NEW TURN
     */
    public void NewTurn()
    {
        //CHECKS LAST PLAYERS TURN
        if (Players[(CurrentPlayer+1)%2].Score >= 6)
        {
            EndGame();
        }
        else
        {
            foreach (TurnObserver ob in turnObservers)
            {
                ob.TurnUpdate();
            }
        }
        this.CurrentPlayer = (this.CurrentPlayer + 1) % NUM_PLAYERS;

    }

    /* * * * * * *
     * SKIP TURN
     */
    public void SkipTurn()
    {
        //TODO: PLAY TURN SKIPPED ANIMATION

        NewTurn();
    }

    public void EndGame()
    {
        this.Winner = 1;
        if (this.Players[0].Score > this.Players[1].Score)
        {
            this.Winner = 0;
        }
        foreach(EndObserver ob in endObservers)
        {
            ob.GameOver();
        }
        GameOverScreen[Winner].RunMenuCommand();
    }


    //EVENT METHODS:
    public void AddTurnObserver(TurnObserver tob)
    {
        this.turnObservers.Add(tob);
    }
    public void AddEndObserver(EndObserver eob)
    {
        this.endObservers.Add(eob);
    }

}

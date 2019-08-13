using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrTile : MonoBehaviour {

    private void Start()
    {
        this.CanOccupy = true;
    }

    //FIELDS AND PROPERTIES---------------------------------------------------
    public UrTile[] nextTile;
    LinkedList<TileEffectStrat> TileEffects = new LinkedList<TileEffectStrat>();
    public PlayerStone CurrentPiece { get; set; }
    public bool CanOccupy { get; set; }

    public UrTile GetNext(int index)
    {
        return nextTile[index % nextTile.Length];
    }

    public void ActivateTile()
    {
        foreach (TileEffectStrat effect in TileEffects)
        {
            effect.ActivateTile();
        }
    }

    public void DeactivateTile()
    {
        foreach (TileEffectStrat effect in TileEffects)
        {
            effect.DeactivateTile();
        }
}

    public void ClearTile()
    {
        if(CurrentPiece != null)
        {
            CurrentPiece.SendBack();
        }
    }

    public void SetSafeSpace()
    {
        this.TileEffects.AddLast(new SafeSpace(this));
    }

    public void SetDoubleTurnTile()
    {
        this.TileEffects.AddLast(new DoubleTurn(this));
    }

    public void SetGoalTile()
    {
        this.TileEffects.AddLast(new GoalTile(this));
    }

    //TILE EFFECTS============================================================
    private class SafeSpace : TileEffectStrat
    {
        public PlayAnimation Anim;
        public AudioPlayer Sound;
        public UrTile Tile { get; set; }
        public SafeSpace(UrTile Tile)
        {
            this.Tile = Tile;
            this.Anim = Tile.GetComponentInChildren<PlayAnimation>();
            this.Sound = Tile.GetComponentInChildren<AudioPlayer>();
        }

        public void ActivateTile()
        {
            Tile.CanOccupy = false;
            Anim.PlayAnim("Activate");
            Sound.PlayClip(0);
        }

        public void DeactivateTile()
        {
            Tile.CanOccupy = true;
            Anim.PlayAnim("Deactivate");
            Sound.PlayClip(1);
        }
    }

    private class DoubleTurn : TileEffectStrat
    {
        public PlayAnimation Anim;
        public AudioPlayer Sound;
        public UrTile Tile { get; set; }
        public DoubleTurn(UrTile Tile)
        {
            this.Tile = Tile;
            this.Anim = Tile.GetComponentInChildren<PlayAnimation>();
            this.Sound = Tile.GetComponentInChildren<AudioPlayer>();
        }

        public void ActivateTile()
        {
            Tile.CurrentPiece.Owner.dr.CanRoll = true;
            Anim.PlayAnim("ActivateDoubleTurn");
            Sound.PlayClip(0);
        }

        public void DeactivateTile()
        {
            //Anim.PlayAnim("DeactivateDoubleTurn");
        }
    }

    private class GoalTile : TileEffectStrat
    {
        public PlayAnimation Anim;
        public UrTile Tile { get; set; }
        public GoalTile(UrTile Tile)
        {
            this.Anim = Tile.GetComponentInChildren<PlayAnimation>();
            this.Tile = Tile;
        }

        public void ActivateTile()
        {
            Anim.PlayAnim("Score");
            Tile.CurrentPiece.Score();
            Tile.CurrentPiece = null;
        }

        public void DeactivateTile()
        {
        }
    }
}

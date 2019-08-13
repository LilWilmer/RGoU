using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*TODO:
  -Add HashSet of MovementStrategies
  -
  */

public class PlayerStone : MonoBehaviour {

    //FIELDS==================================================================
    public PlayAnimation anim;
    public Vector3 startPosition;
    public Vector3 EndPosition;
    public UrTile startTile;
    public UrTile EndTile;
    public UrTile curTile;
    public UrTile CurTile
    {
        get { return curTile; }
        set { this.curTile = value; }
    }
    public UrTile TargetTile { get; set; }

    public Player Owner { get; set; }

    public bool CanMove { get; set; }
    public bool IsFloating { get; set; }
    public bool IsFloating2 { get; set; }

    public int MoveDistance { get; set; }

    public HashSet<PieceObserver> Observers = new HashSet<PieceObserver>();

    //SETUP===================================================================
    private void Start()
    {
        this.anim = GetComponentInChildren<PlayAnimation>();
        this.anim.AnimationFinished += AnimFin;

        this.startTile = this.curTile;
        this.startPosition = this.transform.position;

        this.EndPosition = this.EndTile.transform.position;

        CanMove = false;
        MoveDistance = 0;
    }

    //METHODS=================================================================
    /**
     * Adds an observer to the set
     */
    public void AddObserver(PieceObserver ob)
    {
        Observers.Add(ob);
    }

    /**
     * This is a MonoBehaviour callback method.. apon clicking the mouse this
     * method will be called.
     */
    private void OnMouseUp()
    {
        if (CanMove)
        {
            foreach (PieceObserver ob in Observers)
            {
                ob.PieceClicked(this);
            }
        }
    }

    /**
     * 
     */
    private void OnMouseOver()
    {
 
        if (CanMove && !IsFloating)
        {
            IsFloating2 = false;
            anim.PlayAnim("Float");
        }
        else if (CanMove && IsFloating2)
        {
            IsFloating2 = false;
            anim.PlayAnim("New Animation");
        }
    }

    /**
     * 
     */
    public bool CanMoveTo()
    {
        //Setting up variables------------
        int ii = 0;
        bool CanMove = false;
        this.TargetTile = this.curTile;

        //Finding the tile to land on by iterating thu the urTile "linkedList"
        while (this.TargetTile.nextTile.Length != 0 && ii < MoveDistance)
        {
            this.TargetTile = TargetTile.GetNext(Owner.ID);
            ii++;
        }
        //If ii does not equal the move distance then the piece does not move
        if (ii == MoveDistance)
        {
            if (this.TargetTile.CanOccupy)
            {
                if (TargetTile.CurrentPiece == null
                || TargetTile.CurrentPiece.Owner != this.Owner)
                {
                    CanMove = true;
                }
            }
        }
        return CanMove;
    }

    /**
     * Sets up certain things before calling MovePiece
     */
    public void StartMove()
    {
        IsFloating = false;
        //Leaving current tile
        this.curTile.DeactivateTile();
        this.curTile.CurrentPiece = null;

        //Moving piece
        if(curTile == startTile)
        {
            MoveFromStart();
        }
        else
        {
            MovePiece();
        }
    }

    /**
     * Moves the player piece to the target tile
     */
    public void MovePiece()
    {
        //this.transform.position = this.curTile.transform.position;

        if (this.curTile != this.TargetTile)
        {
            UrTile lastTile = this.curTile;
            this.curTile = this.curTile.GetNext(this.Owner.ID);
            transform.LookAt(this.curTile.transform);
            StartCoroutine(this.MovementCoroutine(this.transform.position,
                                      this.curTile.transform.position,
                                      0.5f));

            if (lastTile.CurrentPiece == null)
            {
                if (curTile.CurrentPiece == null)
                {
                    this.anim.PlayAnim("Hop");
                }
                else
                {
                    this.anim.PlayAnim("HopOver");
                }
            }
            else
            {
                if (curTile.CurrentPiece == null)
                {
                    this.anim.PlayAnim("HopOff");
                }
                else
                {
                    this.anim.PlayAnim("HopOffOver");
                }
            }
        }
        else
        {
            if(curTile.CurrentPiece!=null)
            {
                this.anim.PlayAnim("Take Piece");
            }
            this.curTile.ClearTile();
            //Setting up position on new tile.
            this.curTile = this.TargetTile;
            this.curTile.CurrentPiece = this;
            this.curTile.ActivateTile();

            //Letting listeners know the move has terminated.
            foreach (PieceObserver p_ob in Observers)
            {
                p_ob.PieceMoved(this);
            }

        }


    }

    /**
     * MOVEMENT COROUTINE -------------------------------------------------
     */
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.05f;
    public IEnumerator MovementCoroutine(Vector3 startPosition, 
                                         Vector3 endPosition,
                                         float duration)
    {
        float counter = 0f;
        while (this.transform.position != this.curTile.transform.position)
        {
            counter += Time.deltaTime;
            Debug.Log("MOVING");
            this.transform.position = Vector3.Lerp( startPosition, endPosition, counter / duration);

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("DONE MOVING");

        this.velocity = Vector3.zero;

    }


    public void MoveFromStart()
    {
        this.anim.PlayAnim("Sink");
        this.curTile = this.curTile.GetNext(this.Owner.ID);

    }

    public void RaisePiece()
    {
        this.anim.PlayAnim("Rise");
    }

    public void AnimFin(string AnimationName)
    {
        if (AnimationName == "Hop")
        {
            IsFloating = false;
            MovePiece();
        }
        else if (AnimationName == "Floating1")
        {
            IsFloating = true;
        }
        else if (AnimationName == "Floating2")
        {
            IsFloating2 = true;
        }
        else if (AnimationName == "Lower")
        {
            IsFloating = false;
        }
        else if (AnimationName == "Sink")
        {
            IsFloating = false;
            if (this.curTile == startTile) this.transform.position = startPosition;
            else this.transform.position = this.curTile.transform.position;
            RaisePiece();
        }
        else if(AnimationName == "Rise")
        {
            if(curTile != startTile) MovePiece();
        }
    }

    /**
     * Sends the stone back to its starting wrack
     */
    public void SendBack()
    {
    
        this.anim.PlayAnim("Sink");
        this.curTile = this.startTile;
        this.TargetTile = this.startTile;
    }

    /**
     * Increases the player score and moves the piece to the end tile
     */
    public void Score()
    {
        this.curTile = this.EndTile;
        this.transform.position = this.EndPosition;
        this.Owner.Score++;
    }
}

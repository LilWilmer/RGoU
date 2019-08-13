using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PieceObserver
{
    void PieceClicked(PlayerStone stone);
    void PieceMoved(PlayerStone stone);
}

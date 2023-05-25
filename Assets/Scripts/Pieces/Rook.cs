using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rook : BasePieces
{
    [HideInInspector]
    public Cell mCastleTriggerCell = null;
    private Cell mCastleCell = null;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mMovement = new Vector3Int(7, 7, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Rook");
    }

    public override void Place(Cell newCell)
    {
        base.Place(newCell);

        int triggerOffset = mCurrentCell.mBoardPosition.x < 4 ? 2 : -1;
        mCastleTriggerCell = SetCell(triggerOffset);

        int castleOffset = mCurrentCell.mBoardPosition.x < 4 ? 3 : -2;
        mCastleCell = SetCell(castleOffset);
    }
    public void Castle()
    {
        mTargetCell = mCastleCell;

        Move();
    }

    private Cell SetCell (int offset)
    {
        Vector2Int newPosition = mCurrentCell.mBoardPosition;
        newPosition.x += offset;

        return mCurrentCell.mBoard.mAllCells[newPosition.x, newPosition.y];
    }
}

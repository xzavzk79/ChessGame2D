using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn_4x4 : BasePieces_4x4
{
    private bool mIsFirstMove = true;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager_4x4 newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mIsFirstMove = true;

        mMovement = mColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Pawn");
    }

    protected override void Move()
    {
        base.Move();

        mIsFirstMove = false;

        CheckForPromotion();
    }

    private bool MatchesState(int targetX, int targetY, CellState_4x4 targetState)
    {
        CellState_4x4 cellState = CellState_4x4.None;
        cellState = mCurrentCell_4x4.mBoard_4x4.ValidateCell(targetX, targetY, this);

        if (cellState == targetState)
        {
            mHighlightedCells_4x4.Add(mCurrentCell_4x4.mBoard_4x4.mAllCells[targetX, targetY]);
            return true;
        }

        return false;
    }

    private void CheckForPromotion()
    {
        int currentX = mCurrentCell_4x4.mBoardPosition.x;
        int currentY = mCurrentCell_4x4.mBoardPosition.y;

        CellState_4x4 cellState = mCurrentCell_4x4.mBoard_4x4.ValidateCell(currentX, currentY + mMovement.y, this);

        if (cellState == CellState_4x4.OutOfBounds)
        {
            Color spriteColor = GetComponent<Image>().color;
            mPieceManager.PromotePiece(this, mCurrentCell_4x4, mColor, spriteColor);
        }
    }

    protected override void CheckPathing()
    {
        int currentX = mCurrentCell_4x4.mBoardPosition.x;
        int currentY = mCurrentCell_4x4.mBoardPosition.y;

        MatchesState(currentX - mMovement.z, currentY + mMovement.z, CellState_4x4.Enemy);

        if (MatchesState(currentX, currentY + mMovement.y, CellState_4x4.Free))
        {
            if (mIsFirstMove)
            {
                MatchesState(currentX, currentY + (mMovement.y * 2), CellState_4x4.Free);
            }
        }

        MatchesState(currentX + mMovement.z, currentY + mMovement.z, CellState_4x4.Enemy);
    }

    public override void Reset()
    {
        base.Reset();

        mIsFirstMove = true;
    }
}

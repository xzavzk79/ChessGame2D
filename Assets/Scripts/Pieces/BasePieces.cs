using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePieces : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;

    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();

    public virtual void Setup (Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    #region Движение
    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        // Выбранная позиция
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Проверка клеток
        for(int i = 1; i <= movement; i++) 
        { 
            currentX += xDirection;
            currentY += yDirection;

            //TODO: Get the state of the target cell

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }
    protected virtual void CheckPathing()
    {
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);

        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);
    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = true;
        }
    }

    protected void ClearCells()
    {
        foreach(Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = false;
        }
        mHighlightedCells.Clear();
    }
    #endregion

    #region События
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        CheckPathing();
        ShowCells();
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        transform.position += (Vector3)eventData.delta;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();
    }
    #endregion
}

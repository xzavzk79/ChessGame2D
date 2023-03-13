using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.IO.LowLevel.Unsafe;
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

    protected Cell mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();

    
    public virtual void Setup (Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Метод сообщающий нужную клетку для создания фигуры
    /// </summary>
    /// <param name="newCell"></param>
    public void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();

        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
        mCurrentCell.mCurrentPiece = null;

        gameObject.SetActive(false);
    }

    #region Движение
    /// <summary>
    /// Метод создающий путь для фигуры
    /// </summary>
    /// <param name="xDirection"></param>
    /// <param name="yDirection"></param>
    /// <param name="movement"></param>
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

            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            if (cellState == CellState.Enemy)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }

            if (cellState != CellState.Free)
                break;

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            Debug.Log(currentY);
        }
    }

    /// <summary>
    /// Метод проверяющий возможный путь фигуры
    /// </summary>
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

    /// <summary>
    /// Метод показывающий возможный путь фигуры
    /// </summary>
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

    protected virtual void Move()
    {
        mTargetCell.RemovePiece();

        mCurrentCell.mCurrentPiece = null;

        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;
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

        foreach (Cell cell in mHighlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                mTargetCell = cell;
                break;
            }

            mTargetCell = null;
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();

        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }

        Move();

        mPieceManager.SwitchSides(mColor);
    }
    #endregion
}

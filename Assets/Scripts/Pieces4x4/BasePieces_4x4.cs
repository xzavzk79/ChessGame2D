using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePieces_4x4 : EventTrigger //класс наследуемый от тригера события тк в нем легче работать с холстами, чтоюы не содзавать логичку двидений и тп
{
    public Sprite mExampleSprite;

    [HideInInspector]
    public Color mColor = Color.clear; //Переменная цветового типа и мы иницилизиуерм ее для отчистки 

    protected Cell_4x4 mOriginalCell_4x4 = null; //Исходная ячека
    protected Cell_4x4 mCurrentCell_4x4 = null;//Текущая ячейка в которой находится фрагмент

    protected RectTransform mRectTransform = null; //Переменная для повторного преобразования переменных
    protected PieceManager_4x4 mPieceManager;//Ссылка на менеждер

    protected Cell_4x4 mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell_4x4> mHighlightedCells_4x4 = new List<Cell_4x4>();
    /// <summary>
    /// Функция настройки, которвя принимает ряд параметор 
    /// </summary>
    /// <param name="newTeamColor">Цвет новой команды</param>
    /// <param name="newSpriteColor">Цвет спрайта</param>
    /// <param name="newPieceManager">Cсылка на менеджер по деталям</param>
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager_4x4 newPieceManager)
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
    public void Place(Cell_4x4 newCell)
    {

        mCurrentCell_4x4 = newCell;
        mOriginalCell_4x4 = newCell;
        mCurrentCell_4x4.mCurrentPiece_4x4 = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }
    public virtual void Reset()
    {
        Kill();

        Place(mOriginalCell_4x4);
    }

    public virtual void Kill()
    {
        mCurrentCell_4x4.mCurrentPiece_4x4 = null;

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
        int currentX = mCurrentCell_4x4.mBoardPosition.x;
        int currentY = mCurrentCell_4x4.mBoardPosition.y;

        //Проверка клеток
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState_4x4 cellState = CellState_4x4.None;
            cellState = mCurrentCell_4x4.mBoard_4x4.ValidateCell(currentX, currentY, this);

            if (cellState == CellState_4x4.Enemy)
            {
                mHighlightedCells_4x4.Add(mCurrentCell_4x4.mBoard_4x4.mAllCells[currentX, currentY]);
                break;
            }

            if (cellState != CellState_4x4.Free)
                break;

            mHighlightedCells_4x4.Add(mCurrentCell_4x4.mBoard_4x4.mAllCells[currentX, currentY]);
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
        foreach (Cell_4x4 cell in mHighlightedCells_4x4)
        {
            cell.mOutlineImage.enabled = true;
        }
    }

    protected void ClearCells()
    {
        foreach (Cell_4x4 cell in mHighlightedCells_4x4)
        {
            cell.mOutlineImage.enabled = false;
        }
        mHighlightedCells_4x4.Clear();
    }

    protected virtual void Move()
    {
        mTargetCell.RemovePiece();

        mCurrentCell_4x4.mCurrentPiece_4x4 = null;

        mCurrentCell_4x4 = mTargetCell;
        mCurrentCell_4x4.mCurrentPiece_4x4 = this;

        transform.position = mCurrentCell_4x4.transform.position;
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

        foreach (Cell_4x4 cell in mHighlightedCells_4x4)
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
            transform.position = mCurrentCell_4x4.gameObject.transform.position;
            return;
        }

        Move();

        mPieceManager.SwitchSides(mColor);
    }
    #endregion
}

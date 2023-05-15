using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell_4x4 : MonoBehaviour
{
    public Image mOutlineImage; //Ссылка на контур
    public Cell_4x4 newCell;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero; // Пространство для хранения значения того, где ячейка будет находится на доске
    [HideInInspector]
    public Board_4x4 mBoard_4x4 = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;//Ссылка на преобразрование ячейки

    [HideInInspector]
    public BasePieces_4x4 mCurrentPiece_4x4 = null;//за что отвечает

    /// <summary>
    /// Конструктор записывающий информацию клетки
    /// </summary>
    /// <param name="newBoardPosition"></param>
    /// <param name="newBoard"></param>
    public void Setup(Vector2Int newBoardPosition, Board_4x4 newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard_4x4 = newBoard;

        mRectTransform = GetComponent<RectTransform>(); // Преобразование Rec
    }



    public void RemovePiece()
    {
        if (mCurrentPiece_4x4 != null)
        {
            mCurrentPiece_4x4.Kill();
        }
    }
}

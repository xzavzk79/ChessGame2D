using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image mOutlineImage;
    public Cell newCell;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;

    /// <summary>
    /// Конструктор записывающий информацию клетки
    /// </summary>
    /// <param name="newBoardPosition"></param>
    /// <param name="newBoard"></param>
    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }
}

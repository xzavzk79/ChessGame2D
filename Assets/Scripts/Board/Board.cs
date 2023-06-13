using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds,
    Attack
}

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;

    public GameObject newCell;

    static int Width = 8;
    static int Height = 8;

    /// <summary>
    /// ������� ���� ������
    /// </summary>
    [HideInInspector]
    public Cell[,] mAllCells = new Cell[Width, Height];

    /// <summary>
    /// �������� ����
    /// </summary>
    public void Create()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // �������� ������
                newCell = Instantiate(mCellPrefab, transform);

                //�������� ����� ������
                newCell.name = $"Cell {x} {y}";

                // �������
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                // ������� ������
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                // �����
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);

            }
        }

        for (int x = 0; x < Width; x += 2)
        {
            for (int y = 0; y < Height; y++)
            {
                // �������� �� ���/�����
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                // ������ ����� ������
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public CellState ValidateCell(int targetX, int targetY, BasePieces checkingPiece)
    {
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;
        
        if(targetY < 0 || targetY> 7)
            return CellState.OutOfBounds;

        Cell targetCell = mAllCells[targetX, targetY];

        if (targetCell.mCurrentPiece != null)
        {
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friendly;

            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
                return CellState.Enemy;
        }

        if (checkingPiece.)
            return CellState.Attack;

        return CellState.Free;
    }
}           
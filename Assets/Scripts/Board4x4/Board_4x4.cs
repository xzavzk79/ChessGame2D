using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState_4x4
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board_4x4 : MonoBehaviour
{
    public GameObject mCellPrefab; 

    public GameObject newCell;

    static int Width = 14;
    static int Height = 14;

    /// <summary>
    /// ������� ���� ������
    /// </summary>
    [HideInInspector]
    public Cell_4x4[,] mAllCells = new Cell_4x4[Width, Height];

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
                // ������� ������ �������, ������ ������ �������� � Pr_Cell
                rectTransform.anchoredPosition = new Vector2((x * 71) + 50, (y * 71) + 50);

                // �����
                mAllCells[x, y] = newCell.GetComponent<Cell_4x4>();
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
        //����� ������������ ����� 
        //�� ��� �=0
        mAllCells[0, 0].GetComponent<Image>().color = new Color32(255, 255, 255,0);
        mAllCells[0, 1].GetComponent<Image>().color = new Color32(255, 255, 255,0);
        mAllCells[0, 2].GetComponent<Image>().color = new Color32(255, 255, 255,0);
        mAllCells[0, 11].GetComponent<Image>().color = new Color32(255, 255, 255,0);
        mAllCells[0, 12].GetComponent<Image>().color = new Color32(255, 255, 255,0);
        mAllCells[0, 13].GetComponent<Image>().color = new Color32(255, 255, 255,0);
        //�� ��� �=1
        mAllCells[1, 0].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[1, 1].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[1, 2].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[1, 11].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[1, 12].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[1, 13].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        //�� ��� �=2
        mAllCells[2, 0].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[2, 1].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[2, 2].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[2, 11].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[2, 12].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[2, 13].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        //�� ��� �=11
        mAllCells[11, 0].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[11, 1].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[11, 2].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[11, 11].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[11, 12].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[11, 13].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        //�� ��� �=12
        mAllCells[12, 0].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[12, 1].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[12, 2].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[12, 11].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[12, 12].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[12, 13].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        //�� ��� �=13
        mAllCells[13, 0].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[13, 1].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[13, 2].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[13, 11].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[13, 12].GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        mAllCells[13, 13].GetComponent<Image>().color = new Color32(255, 255, 255, 0);

    }

    public CellState_4x4 ValidateCell(int targetX, int targetY, BasePieces_4x4 checkingPiece)
    {
        if (targetX < 0 || targetX > 9)
            return CellState_4x4.OutOfBounds;

        if (targetY < 0 || targetY > 9)
            return CellState_4x4.OutOfBounds;

        Cell_4x4 targetCell = mAllCells[targetX, targetY];

        if (targetCell.mCurrentPiece_4x4 != null)
        {
            if (checkingPiece.mColor == targetCell.mCurrentPiece_4x4.mColor)
                return CellState_4x4.Friendly;

            if (checkingPiece.mColor != targetCell.mCurrentPiece_4x4.mColor)
                return CellState_4x4.Enemy;
        }

        return CellState_4x4.Free;
    }
}
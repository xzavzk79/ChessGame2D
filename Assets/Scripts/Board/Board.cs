using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;

    public GameObject newCell;

    static int Width = 10;
    static int Height = 10;

    /// <summary>
    /// Префабы всех клеток
    /// </summary>
    [HideInInspector]
    public Cell[,] mAllCells = new Cell[Width, Height];

    /// <summary>
    /// Создание поля
    /// </summary>
    public void Create()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // Создание клетки
                newCell = Instantiate(mCellPrefab, transform);

                //Создание имени клетки
                newCell.name = $"Cell {x} {y}";

                // Позиция
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                // размеры клетки
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                // Сетап
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);

            }
        }

        for (int x = 0; x < Width; x += 2)
        {
            for (int y = 0; y < Height; y++)
            {
                // Проверка на чет/нечет
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                // Красим нечет клетки
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }
}           
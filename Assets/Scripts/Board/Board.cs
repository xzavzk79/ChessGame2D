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

    /// <summary>
    /// префабы всех клеток
    /// </summary>
    [HideInInspector]
    public Cell[,] mAllCells = new Cell[8, 8];

    /// <summary>
    /// создание поля
    /// </summary>
    public void Create()
    {
        #region Создание клеток на поле
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                // Создание клетки
                newCell = Instantiate(mCellPrefab, transform);

                //Создание имени клетки
                newCell.name = $"Cell {x} {y}";

                // Позиция
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50); // размеры клетки

                // Сетап
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);

            }
        }
        #endregion

        #region Покраска
        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 8; y++)
            {
                // Проверка на чет/нечет
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                // Цвет
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(255, 255, 255, 255); // Красим нечет клетки в 
            }
        }
        #endregion
    }
}           
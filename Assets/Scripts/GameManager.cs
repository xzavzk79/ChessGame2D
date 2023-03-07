using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    public PieceManager mPieceManager;

    void Start()
    {
        #region Рисование поля
        // Создание поля
        mBoard.Create();
        #endregion

        #region Размещение фигур
        mPieceManager.Setup(mBoard);
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    public PieceManager mPieceManager;

    void Start()
    {
        // Создание поля
        mBoard.Create();

        //Размещение на полк фигур
        mPieceManager.Setup(mBoard);
    }
}

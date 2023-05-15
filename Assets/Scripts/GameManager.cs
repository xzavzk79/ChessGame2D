using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public Board_4x4 mBoard_4x4;
    public PieceManager mPieceManager;
    public PieceManager_4x4 mPieceManager_4x4;

    void Start()
    {
        // Создание поля
        //mBoard.Create();
        // Создание поля
        mBoard_4x4.Create();
        //Размещение на полк фигур
        //mPieceManager.Setup(mBoard);
        //Размещение на полк фигур
        mPieceManager_4x4.Setup(mBoard_4x4);
    }
}

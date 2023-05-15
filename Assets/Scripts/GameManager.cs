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
        // �������� ����
        //mBoard.Create();
        // �������� ����
        mBoard_4x4.Create();
        //���������� �� ���� �����
        //mPieceManager.Setup(mBoard);
        //���������� �� ���� �����
        mPieceManager_4x4.Setup(mBoard_4x4);
    }
}

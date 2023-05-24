using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    //public Board_4x4 mBoard_4x4;
    public PieceManager mPieceManager;
    //public PieceManager_4x4 mPieceManager_4x4;
    Rchboard RChBoard;

    void Start()
    {
        
        Scene curscene = SceneManager.GetActiveScene(); //�������� ������ ������� �����(��� ����� ���)

        //��������� ��� ������� ����� � ������� ������ ����� � ������� ��������
        if(curscene.name == "Chess")
        {
            // �������� ����
            mBoard.Create();
            //���������� �� ���� �����
            mPieceManager.Setup(mBoard);
        }
        else if (curscene.name == "shashki")
        {
            //�������� ���� ��� �����
            RChBoard = gameObject.GetComponent<Rchboard>();
            RChBoard.CreateChBoard();
            RChBoard.SetupPieces();
        }    
    }
}

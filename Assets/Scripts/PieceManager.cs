using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;
    
    public GameObject mPiecePrefab;
    public GameObject GameOver;
    public Text winText;

    private List<BasePieces> mWhitePieces = null;
    private List<BasePieces> mBlackPieces = null;
    private List<BasePieces> mPromotedPieces = new List<BasePieces>();

    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "K", "Q", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P", typeof(Pawn) },
        {"R", typeof(Rook) },
        {"KN", typeof(Knight) },
        {"B", typeof(Bishop) },
        {"K" , typeof(King) },
        {"Q", typeof(Queen) }
    };

    /// <summary>
    /// Метод распределяющий фигуры по сторонам
    /// </summary>
    /// <param name="board"></param>
    public void Setup(Board board)  
    {
        mWhitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);

        mBlackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);

        SwitchSides(Color.black);
    }

    /// <summary>
    /// Метод добавляющий фигуры на поле
    /// </summary>
    /// <param name="teamColor"></param>
    /// <param name="spriteColor"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private List<BasePieces> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePieces> newPieces = new List<BasePieces>();

        for (int i = 0; i < mPieceOrder.Length; i++)
        {
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            BasePieces newPiece = CreatePiece(pieceType, i);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
    }
    /// <summary>
    /// Метод создающий фигуру
    /// </summary>
    /// <param name="pieceType">тип фигуры</param>
    /// <returns></returns>
    private BasePieces CreatePiece(Type pieceType, int i)
    {
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);

        newPieceObject.name = $"Piece {i + 1}";

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        BasePieces newPiece = (BasePieces)newPieceObject.AddComponent(pieceType);

        return newPiece;
    }

    /// <summary>
    /// Метод расстанавливающий фигуры в нужной строке
    /// </summary>
    /// <param name="pawnRow"></param>
    /// <param name="royaltyRow"></param>
    /// <param name="pieces"></param>
    /// <param name="board"></param>
    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePieces> pieces, Board board)
    {
        for (int i = 0; i<8; i++) 
        {
            //Строка пешек
            pieces[i].Place(board.mAllCells[i, pawnRow]);


           //Строка короля
            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);
        }
    }

    private void SetInteractive(List<BasePieces> allPieces, bool value)
    {
        foreach (BasePieces piece in allPieces)
            piece.enabled = value;
    }

    public void SwitchSides(Color color)
    {
        if (!mIsKingAlive)
        {
            GameOver.SetActive(true);
            Time.timeScale = 0f;
            if (color == Color.black)
            {
                winText.text = "Black wins";
            }
            else
            {
                winText.text = "White wins";
            }

            ResetPieces();

            mIsKingAlive = true;

            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;

        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);

        foreach (BasePieces pieces in mPromotedPieces)
        {
            bool isBlackPiece = pieces.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;

            pieces.enabled = isPartOfTeam;
        }
    }
    public void HideGameOver()
    {
        GameOver.SetActive(false);
    }
    public void ResetPieces()
    {

        foreach (BasePieces pieces in mPromotedPieces)
        {
            pieces.Kill();

            Destroy(pieces.gameObject);
        }

        foreach (BasePieces piece in mWhitePieces)
            piece.Reset();
        
        foreach (BasePieces piece in mBlackPieces)
            piece.Reset();
    }
    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        // Анигилируем пешку
        pawn.Kill();

        //Создаем нового юнита
        BasePieces promotedPiece = CreatePiece(typeof(Queen), 0);
        promotedPiece.Setup(teamColor, spriteColor, this);

        // Размещаем нового юнита
        promotedPiece.Place(cell);

        // Добавляем в команду(список)

        mPromotedPieces.Add(promotedPiece);
    }
}

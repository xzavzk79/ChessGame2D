using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;
    
    public GameObject mPiecePrefab;

    private List<BasePieces> mWhitePieces = null;
    private List<BasePieces> mBlackPieces = null;

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

        PlacePieces(2, 1, mWhitePieces, board);
        PlacePieces(7, 8, mBlackPieces, board);

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
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.name = $"Piece {i+1}";

            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            BasePieces newPiece = (BasePieces)newPieceObject.AddComponent(pieceType);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
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
            pieces[i].Place(board.mAllCells[i+1, pawnRow]);


           //Строка короля
            pieces[i + 8].Place(board.mAllCells[i+1, royaltyRow]);
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
            ResetPieces();

            mIsKingAlive = true;

            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;

        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);
    }
    public void ResetPieces()
    {
        foreach (BasePieces piece in mWhitePieces)
            piece.Reset();
        
        foreach (BasePieces piece in mBlackPieces)
            piece.Reset();
    }
}

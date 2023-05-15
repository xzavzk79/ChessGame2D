using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceManager_4x4 : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab; //ССылка на объект 

    private List<BasePieces_4x4> mWhitePieces = null;
    private List<BasePieces_4x4> mBlackPieces = null;
    private List<BasePieces_4x4> mPromotedPieces = new List<BasePieces_4x4>();

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
    public void Setup(Board_4x4 board)
    {
        mWhitePieces = CreatePieces_4x4(Color.white, new Color32(80, 124, 159, 255), board);

        mBlackPieces = CreatePieces_4x4(Color.black, new Color32(210, 95, 64, 255), board);

        PlacePieces(2, 1, mWhitePieces, board);
        PlacePieces(7, 8, mBlackPieces, board);

        SwitchSides(Color.black);
    }

    /// <summary>
    /// Метод добавляющий фигуры на поле 4x4
    /// </summary>
    /// <param name="teamColor"></param>
    /// <param name="spriteColor"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private List<BasePieces_4x4> CreatePieces_4x4(Color teamColor, Color32 spriteColor, Board_4x4 board)
    {
        List<BasePieces_4x4> newPieces = new List<BasePieces_4x4>();

        for (int i = 0; i < mPieceOrder.Length; i++)
        {
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            BasePieces_4x4 newPiece = CreatePiece_4x4(pieceType, i);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
    }
    private BasePieces_4x4 CreatePiece_4x4(Type pieceType, int i)
    {
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);

        newPieceObject.name = $"Piece {i + 1}";

        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        BasePieces_4x4 newPiece = (BasePieces_4x4)newPieceObject.AddComponent(pieceType);

        return newPiece;
    }

    /// <summary>
    /// Метод расстанавливающий фигуры в нужной строке
    /// </summary>
    /// <param name="pawnRow"></param>
    /// <param name="royaltyRow"></param>
    /// <param name="pieces"></param>
    /// <param name="board"></param>
    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePieces_4x4> pieces, Board_4x4 board)
    {
        for (int i = 0; i <8; i++)
        {
            //Строка пешек
            pieces[i].Place(board.mAllCells[i + 1, pawnRow]);


            //Строка короля
            pieces[i + 8].Place(board.mAllCells[i + 1, royaltyRow]);
        }
    }

    private void SetInteractive(List<BasePieces_4x4> allPieces, bool value)
    {
        foreach (BasePieces_4x4 piece in allPieces)
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

        foreach (BasePieces_4x4 pieces in mPromotedPieces)
        {
            bool isBlackPiece = pieces.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;

            pieces.enabled = isPartOfTeam;
        }
    }
    public void ResetPieces()
    {
        foreach (BasePieces_4x4 pieces in mPromotedPieces)
        {
            pieces.Kill();

            Destroy(pieces.gameObject);
        }

        foreach (BasePieces_4x4 piece in mWhitePieces)
            piece.Reset();

        foreach (BasePieces_4x4 piece in mBlackPieces)
            piece.Reset();
    }
    public void PromotePiece(Pawn_4x4 pawn, Cell_4x4 cell, Color teamColor, Color spriteColor)
    {
        // Анигилируем пешку
        pawn.Kill();

        //Создаем нового юнита
        BasePieces_4x4 promotedPiece = CreatePiece_4x4(typeof(Queen), 0);
        promotedPiece.Setup(teamColor, spriteColor, this);

        // Размещаем нового юнита
        promotedPiece.Place(cell);

        // Добавляем в команду(список)

        mPromotedPieces.Add(promotedPiece);
    }
}

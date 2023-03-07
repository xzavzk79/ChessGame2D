using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceManager : MonoBehaviour
{
    public GameObject mPiecePrefab;

    private List<BasePieces> mWhitePieces = null;
    private List<BasePieces> mBlackPieces = null;

    #region Порядок расположениея фигур
    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "K", "Q", "B", "KN", "R"
    };
    #endregion

    #region Присвоение фигур
    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P", typeof(Pawn) },
        {"R", typeof(Rook) },
        {"KN", typeof(Knight) },
        {"B", typeof(Bishop) },
        {"K" , typeof(King) },
        {"Q", typeof(Queen) }
    };
    #endregion

    #region Присвоение сторон фигурам
    public void Setup(Board board)
    {
        mWhitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);

        mBlackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);
    }
    #endregion

    #region Создание фигур
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
    #endregion

    #region Расстановка фигур
    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePieces> pieces, Board board)
    {
        for (int i = 0; i<8; i++) 
        {
            pieces[i].Place(board.mAllCells[i, pawnRow]);

            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);
        }
    }
    #endregion
}

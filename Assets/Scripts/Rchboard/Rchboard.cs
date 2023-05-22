using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rchboard : MonoBehaviour
{
    [HideInInspector]
    public static string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };


    public GameObject tilePrefab;

    public GameObject checkerPrefab;

    public Material WhiteMat, BlackMat;
    public Material WhitePieceMat, BlackPieceMat;

    public GameObject[,] squares = new GameObject[8,8];

    GameObject[] pieceArrangement;

    public void CreateChBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squares[i, j] = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                if (i%2 != 0 && j%2 != 0 || i%2 == 0 && j%2==0)
                {
                    squares[i, j].GetComponent<Renderer>().material = BlackMat;
                }
                else
                {
                    squares[i, j].GetComponent<Renderer>().material = WhiteMat;
                }

                squares[i, j].transform.SetParent(gameObject.transform);
                squares[i, j].name = alphabet[i] + (j + 1);
            }
        }
    }
    public void SetupPieces()
    {
       
        for (int i = 0; i < 8; i++)
        {
            //1 линия
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("whitedef");
            GameObject newWhitePiece = Instantiate(checkerPrefab, squares[i, 1].transform);
            newWhitePiece.gameObject.GetComponent<PieceRCh>();
            //newWhitePiece.GetComponent<Renderer>().material = WhitePieceMat;
            //6 линия
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blackdef");
            GameObject newBlackPiece = Instantiate(checkerPrefab, squares[i, 7].transform);
            newBlackPiece.gameObject.GetComponent<PieceRCh>();
            //newBlackPiece.GetComponent<Renderer>().material = BlackPieceMat;
        }
    }
}

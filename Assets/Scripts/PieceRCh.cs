using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PieceRCh : MonoBehaviour
{
    #region глобальный стаф
    [HideInInspector]
    GameObject[] go;
    [HideInInspector]
    List<string> posmove = new List<string>();
    [HideInInspector]
    public static string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
    [HideInInspector]
    Rchboard rboard;

    public GameObject checkerPrefab;

    public bool Isempty = true;
    public bool Selected = false;
    public bool HaveChildObj = false;
    #endregion

    private void Start()
    {
        rboard = GameObject.Find("GameManager").GetComponent<Rchboard>();
    }

    private void OnMouseDown()
    {
        Selected = true;
        CheckPosMove();
        HaveChild();
        if (Isempty && Selected && !HaveChildObj)
        {

            GetObjwChild();
        }
    }
    public void CheckPosMove()
    {
        string[] curcell = this.name.Split(' ');
        int LeterrIndex = Array.IndexOf(alphabet, curcell[0]);

        int tmp = Convert.ToInt32(curcell[1]);

        if (rboard.WhiteTurn)
        {
            if (LeterrIndex == 0)
            {
                LeterrIndex += 1;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                Debug.Log(posmove[0]);
            }
            else if (LeterrIndex > 0 && LeterrIndex < 7)
            {
                LeterrIndex += 1;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                LeterrIndex -= 2;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                Debug.Log(posmove[0] + " " + posmove[1]);
            }
            else if (LeterrIndex == 7)
            {
                LeterrIndex -= 1;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                Debug.Log(posmove[0]);
            }
            posmove.Clear();
        }
        else
        {
            if (LeterrIndex == 0)
            {
                LeterrIndex += 1;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                Debug.Log(posmove[0]);
            }
            else if (LeterrIndex > 0 && LeterrIndex < 7)
            {
                LeterrIndex += 1;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                LeterrIndex -= 2;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                Debug.Log(posmove[0] + " " + posmove[1]);
            }
            else if (LeterrIndex == 7)
            {
                LeterrIndex -= 1;
                posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                Debug.Log(posmove[0]);
            }
            posmove.Clear();
        }
    }
    public void HaveChild()
    {
        if (gameObject.transform.childCount > 0)
        {
            HaveChildObj = true;
            Isempty = false;
        }
        else
        {
            HaveChildObj = false;
            Isempty = true;
        }
    }
    public void GetObjwChild()
    {
        FillArrayWithGo();
        restart:
        foreach (GameObject gameObj in go.Where(g => g.GetComponent<PieceRCh>().Selected == true))
        {
            if (gameObj.GetComponent<PieceRCh>().HaveChildObj == true && this.Selected == true && this.HaveChildObj == false)
            {
                MoveChild(gameObj);

                goto restart;
            }
        }
        Array.Clear(go, 0, go.Length);
    }
    public void MoveChild(GameObject gameObj)
    {
        if (!rboard.WhiteTurn && gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("blackdef") || gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("blackcool"))
        {
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blackdef");
            var child = Instantiate(checkerPrefab, this.gameObject.transform);
            DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
            gameObj.GetComponent<PieceRCh>().HaveChildObj = false;
            gameObj.GetComponent<PieceRCh>().Selected = false;
            gameObj.GetComponent<PieceRCh>().Isempty = true;
            this.Selected = false;
            rboard.WhiteTurn = true;
            HaveChild();
        }
        else if (rboard.WhiteTurn && gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("whitedef") || gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("whitecool"))
        {
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("whitedef");
            var child = Instantiate(checkerPrefab, this.gameObject.transform);
            DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
            gameObj.GetComponent<PieceRCh>().HaveChildObj = false;
            gameObj.GetComponent<PieceRCh>().Selected = false;
            gameObj.GetComponent<PieceRCh>().Isempty = true;
            this.Selected = false;
            rboard.WhiteTurn = false;
            HaveChild();
        }
        else
        {
            gameObj.GetComponent<PieceRCh>().Selected = false;
            this.Selected = false;
        }
    }
    public void FillArrayWithGo()
    {
        go = GameObject.FindGameObjectsWithTag("Tile");
    }
}

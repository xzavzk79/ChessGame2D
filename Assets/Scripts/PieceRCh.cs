using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceRCh : MonoBehaviour
{
    #region глобальный стаф
    [HideInInspector]
    GameObject[] go;
    [HideInInspector]
    Rchboard rboard;

    public GameObject checkerPrefab;

    public bool Isempty = true;
    public bool Selected = false;
    public bool HaveChildObj = false;
    #endregion

    #region Перемещение шашек

    private void Start()
    {
        rboard = GameObject.Find("GameManager").GetComponent<Rchboard>();
    }

    private void OnMouseDown()
    {
        Selected = true;
        HaveChild();
        if (Isempty && Selected && !HaveChildObj)
        {

            Debug.Log("ifwork");

            GetObjwChild();
        }
    }
    public void HaveChild()
    {
        if (gameObject.transform.childCount > 0)
        {

            Debug.Log("HchildTrueEmptFalse");

            HaveChildObj = true;
            Isempty = false;
        }
        else
        {

            Debug.Log("HchildFalseEmptTrue");

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

                Debug.Log(gameObj.name);

                MoveChild(gameObj);

                goto restart;
            }
        }
        Array.Clear(go, 0, go.Length);
    }
    public void MoveChild(GameObject gameObj)
    {

        Debug.Log("MovingChild");

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

        Debug.Log("FillArray");

        go = GameObject.FindGameObjectsWithTag("Tile");
    }
    #endregion
}

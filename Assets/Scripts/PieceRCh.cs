using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceRCh : MonoBehaviour
{
    [HideInInspector]
    GameObject[] go;

    public GameObject checkerPrefab;

    public bool Isempty = true;
    public bool Selected = false;
    public bool HaveChildObj = false;

    private void OnMouseDown()
    {
        Selected = true;
        HaveChild();
        if (Isempty && Selected && !HaveChildObj)
        {
            GetObjwChild();
        }
    }
    public void HaveChild()
    {
        if (gameObject.transform.childCount != 0)
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
        foreach (GameObject gameObj in go)
        {
            if (gameObj.GetComponent<PieceRCh>().Selected == true && gameObj.GetComponent<PieceRCh>().HaveChildObj == false && this.Selected ==true && this.HaveChildObj == false)
            {
                gameObj.GetComponent<PieceRCh>().Selected = false;
                gameObj.GetComponent<PieceRCh>().Isempty = true;
                this.Selected = false;
                continue;
            }
            if (gameObj.GetComponent<PieceRCh>().Selected == true && gameObj.GetComponent<PieceRCh>().HaveChildObj == true && this.Selected == true && this.HaveChildObj == true)
            {
                gameObj.GetComponent<PieceRCh>().Selected = false;
                this.Selected = false;
                continue;
            }
            if (gameObj.GetComponent<PieceRCh>().Selected == true && gameObj.GetComponent<PieceRCh>().HaveChildObj == true && this.Selected == true && this.HaveChildObj == false)
            {
                Debug.Log(gameObj.name);
                MoveChild(gameObj);
                continue;
            }
        }
        Array.Clear(go, 0, go.Length);
    }
    public void MoveChild(GameObject gameObj)
    {
        DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
        gameObj.GetComponent<PieceRCh>().HaveChildObj = false;
        gameObj.GetComponent<PieceRCh>().Selected = false;
        gameObj.GetComponent<PieceRCh>().Isempty = true;
        this.Selected = false;
        var child = Instantiate(checkerPrefab, this.gameObject.transform);
        HaveChild();
    }
    public void FillArrayWithGo()
    {
        go = GameObject.FindGameObjectsWithTag("Tile");
    }

    private void Start()
    {
        HaveChild();
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

    //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //        if (hit.collider != null)
    //        {
    //            Debug.Log(hit.collider.gameObject.name);
    //        }
    //    }
    //}
}

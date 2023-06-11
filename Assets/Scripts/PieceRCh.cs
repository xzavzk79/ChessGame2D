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


    public bool WhitTeam = false;
    public bool BlackTeam = false;

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
        HaveChild();
        if (HaveChildObj)
        {
            CheckTeam();
            CheckPosMove();
        }
        if (Isempty && Selected && !HaveChildObj)
        {
            GetObjwChild();
        }
    }
    public void CheckTeam()
    {
        if (this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("blackdef") || this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("blackcool"))
        {
            this.BlackTeam = true;
        }
        else
        {
            this.WhitTeam = true;
        }
    }
    public void CheckPosMove()
    {
        string[] curcell = this.name.Split(' ');
        int tmpletind = 0;
        int LeterrIndex = Array.IndexOf(alphabet, curcell[0]);

        int tmp = Convert.ToInt32(curcell[1]);

        if (rboard.WhiteTurn)
        {
            if (LeterrIndex == 0)
            {
                LeterrIndex += 1;
                GameObject move1 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp + 1));
                if (tmp + 1 >= 7)
                {
                    goto methodend;
                }
                if (move1.GetComponent<PieceRCh>().BlackTeam)
                {
                    tmpletind = LeterrIndex + 1;
                    if (GameObject.Find(alphabet[tmpletind] + " " + (tmp + 2)).GetComponent<PieceRCh>().BlackTeam)
                    {
                        this.Selected = false;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp + 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                }
            }
            else if (LeterrIndex > 0 && LeterrIndex < 7)
            {
                LeterrIndex += 1;
                GameObject move1 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp + 1));
                if (tmp + 1 >= 7)
                {
                    goto methodend;
                }
                if (move1.GetComponent<PieceRCh>().BlackTeam)
                {
                    tmpletind = LeterrIndex + 1;
                    if (tmpletind > 7)
                    {
                        goto WhiteLeftOutOfBoard;
                    }
                    else if (GameObject.Find(alphabet[tmpletind] + " " + (tmp + 2)).GetComponent<PieceRCh>().BlackTeam)
                    {
                        goto WhiteLeftOutOfBoard;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp + 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                }
                WhiteLeftOutOfBoard:
                LeterrIndex -= 2;
                GameObject move2 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp + 1));
                if (tmp + 1 >= 7)
                {
                    goto methodend;
                }
                if (move2.GetComponent<PieceRCh>().BlackTeam)
                {
                    tmpletind = LeterrIndex - 1;
                    if (tmpletind < 0)
                    {
                        goto methodend;
                    }
                    else if (GameObject.Find(alphabet[tmpletind] + " " + (tmp + 2)).GetComponent<PieceRCh>().BlackTeam)
                    {
                        goto methodend;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp + 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                }
            }
            else if (LeterrIndex == 7)
            {
                LeterrIndex -= 1;
                GameObject move1 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp + 1));
                if (tmp + 1 >= 7)
                {
                    goto methodend;
                }
                if (move1.GetComponent<PieceRCh>().BlackTeam)
                {
                    tmpletind  = LeterrIndex - 1;
                    if (GameObject.Find(alphabet[tmpletind] + " " + (tmp + 2)).GetComponent<PieceRCh>().BlackTeam)
                    {
                        this.Selected = false;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp + 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp + 1));
                }

            }

        }
        else
        {
            if (LeterrIndex == 0)
            {
                LeterrIndex += 1;
                GameObject move1 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp - 1));
                if (tmp - 1 <= 0)
                {
                    goto methodend;
                }
                if (move1.GetComponent<PieceRCh>().WhitTeam)
                {
                    tmpletind  = LeterrIndex + 1;
                    if (GameObject.Find(alphabet[tmpletind] + " " + (tmp - 2)).GetComponent<PieceRCh>().WhitTeam)
                    {
                        this.Selected = false;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp - 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                }
            }
            else if (LeterrIndex > 0 && LeterrIndex < 7)
            {
                LeterrIndex += 1;
                GameObject move1 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp - 1));
                if (tmp - 1 <= 0)
                {
                    goto methodend;
                }
                if (move1.GetComponent<PieceRCh>().WhitTeam)
                {
                    tmpletind = LeterrIndex + 1;
                    if (tmpletind > 7)
                    {
                        goto BlackLeftOutBoard;
                        
                    }
                    else if (GameObject.Find(alphabet[tmpletind] + " " + (tmp - 2)).GetComponent<PieceRCh>().WhitTeam)
                    {
                        goto BlackLeftOutBoard;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp - 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                }
                BlackLeftOutBoard:
                LeterrIndex -= 2;
                GameObject move2 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp - 1));
                if (move2.GetComponent<PieceRCh>().WhitTeam)
                {
                    tmpletind = LeterrIndex - 1;
                    if (tmpletind < 0)
                    {
                        goto methodend;
                    }
                    else if (GameObject.Find(alphabet[tmpletind] + " " + (tmp - 2)).GetComponent<PieceRCh>().WhitTeam)
                    {
                        goto methodend;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp - 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                }
            }
            else if (LeterrIndex == 7)
            {
                LeterrIndex -= 1;
                GameObject move1 = GameObject.Find(alphabet[LeterrIndex] + " " + (tmp - 1));
                if (tmp - 1 <= 0)
                {
                    goto methodend;
                }
                if (move1.GetComponent<PieceRCh>().WhitTeam)
                {
                    tmpletind = LeterrIndex - 1;
                    if (GameObject.Find(alphabet[tmpletind] + " " + (tmp - 2)).GetComponent<PieceRCh>().WhitTeam)
                    {
                        this.Selected = false;
                    }
                    else
                    {
                        posmove.Add(alphabet[tmpletind] + " " + (tmp - 2));
                    }
                    tmpletind = 0;
                }
                else
                {
                    posmove.Add(alphabet[LeterrIndex] + " " + (tmp - 1));
                }
            }
        }
        methodend:
            Debug.Log("methodends");
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
                if (gameObj.GetComponent<PieceRCh>().posmove.Contains(this.name))
                {
                    gameObj.GetComponent<PieceRCh>().posmove.Clear();
                    MoveChild(gameObj);
                }
                else
                {
                    gameObj.GetComponent<PieceRCh>().posmove.Clear();
                    gameObj.GetComponent<PieceRCh>().Selected = false;
                    this.Selected = false;
                }

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
            gameObj.GetComponent<PieceRCh>().BlackTeam = false;
            this.Selected = false;
            this.BlackTeam = true;
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
            gameObj.GetComponent<PieceRCh>().WhitTeam = false;
            this.Selected = false;
            this.WhitTeam = true;
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

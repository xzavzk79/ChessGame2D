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

    public bool WillKilled = false;
    public bool CanKill = false;
    public bool Isempty = true;
    public bool Selected = false;
    public bool HaveChildObj = false;
    #endregion

    #region Вызываются при нажатии на клетку
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
    #endregion

    #region Различные проверки и подготовления
    /// <summary>
    /// Заполняет список типа объект клетками
    /// </summary>
    public void FillArrayWithGo()
    {
        go = GameObject.FindGameObjectsWithTag("Tile");
    }
    /// <summary>
    /// Проверяет достигла ли шашка конца доски
    /// </summary>
    public void CheckForPromote()
    {
        string[] curcell = this.name.Split(' ');

        int tmp = Convert.ToInt32(curcell[1]);

        if (tmp >= 7 || tmp <= 1)
        {
            PromoteChecker(this.gameObject);
        }
    }
    /// <summary>
    /// выставляет флаг команды (белый/черный)
    /// </summary>
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
    /// <summary>
    /// записывает в список возможные ходы выбраной клетки
    /// </summary>
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
                        CanKill = true;
                        move1.GetComponent<PieceRCh>().WillKilled = true;
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
                        CanKill = true;
                        move1.GetComponent<PieceRCh>().WillKilled = true;
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
                        CanKill = true;
                        move2.GetComponent<PieceRCh>().WillKilled = true;
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
                    tmpletind = LeterrIndex - 1;
                    if (GameObject.Find(alphabet[tmpletind] + " " + (tmp + 2)).GetComponent<PieceRCh>().BlackTeam)
                    {
                        this.Selected = false;
                    }
                    else
                    {
                        CanKill = true;
                        move1.GetComponent<PieceRCh>().WillKilled = true;
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
                    tmpletind = LeterrIndex + 1;
                    if (GameObject.Find(alphabet[tmpletind] + " " + (tmp - 2)).GetComponent<PieceRCh>().WhitTeam)
                    {
                        this.Selected = false;
                    }
                    else
                    {
                        CanKill = true;
                        move1.GetComponent<PieceRCh>().WillKilled = true;
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
                        CanKill = true;
                        move1.GetComponent<PieceRCh>().WillKilled = true;
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
                        CanKill = true;
                        move2.GetComponent<PieceRCh>().WillKilled = true;
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
                        CanKill = true;
                        move1.GetComponent<PieceRCh>().WillKilled = true;
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
    /// <summary>
    /// выставляет флаг на наличие дочернего объекта
    /// </summary>
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
    #endregion

    #region Действия шашек
    /// <summary>
    /// Проверяет выбранные клетки на наличие/отсутсвие дочерних объектов
    /// </summary>
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
                    if (gameObj.GetComponent<PieceRCh>().CanKill)
                    {
                        KillCh(this.name);
                    }
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
    /// <summary>
    /// Уничтожает шашку противоположного цвета, если через нее совершили ход
    /// </summary>
    /// <param name="movetilename">Имя клетки в которую перемещают шашку</param>
    public void KillCh(string movetilename)
    {
        string[] killcell = movetilename.Split(' ');
        int LetterIndex = Array.IndexOf(alphabet, killcell[0]);

        int tmpnum = Convert.ToInt32(killcell[1]);
        string potkilledWhite = alphabet[LetterIndex - 1] + ' ' + Convert.ToString(tmpnum - 1);
        if (rboard.WhiteTurn && GameObject.Find(potkilledWhite).GetComponent<PieceRCh>().BlackTeam && GameObject.Find(potkilledWhite).GetComponent<PieceRCh>().WillKilled)
        {
            DestroyImmediate(GameObject.Find(potkilledWhite).GetComponent<PieceRCh>().transform.GetChild(0).gameObject);
            goto killmethodend;
        }
        potkilledWhite = alphabet[LetterIndex + 1] + ' ' + Convert.ToString(tmpnum - 1);
        if (rboard.WhiteTurn && GameObject.Find(potkilledWhite).GetComponent<PieceRCh>().BlackTeam && GameObject.Find(potkilledWhite).GetComponent<PieceRCh>().WillKilled)
        {
            DestroyImmediate(GameObject.Find(potkilledWhite).GetComponent<PieceRCh>().transform.GetChild(0).gameObject);
            goto killmethodend;
        }
        string potkilledBlack = alphabet[LetterIndex - 1] + ' ' + Convert.ToString(tmpnum + 1);
        if (!rboard.WhiteTurn && GameObject.Find(potkilledBlack).GetComponent<PieceRCh>().WhitTeam && GameObject.Find(potkilledBlack).GetComponent<PieceRCh>().WillKilled)
        {
            DestroyImmediate(GameObject.Find(potkilledBlack).GetComponent<PieceRCh>().transform.GetChild(0).gameObject);
            goto killmethodend;
        }
        potkilledBlack = alphabet[LetterIndex + 1] + ' ' + Convert.ToString(tmpnum + 1);
        if (!rboard.WhiteTurn && GameObject.Find(potkilledBlack).GetComponent<PieceRCh>().WhitTeam && GameObject.Find(potkilledBlack).GetComponent<PieceRCh>().WillKilled)
        {
            DestroyImmediate(GameObject.Find(potkilledBlack).GetComponent<PieceRCh>().transform.GetChild(0).gameObject);
            goto killmethodend;
        }
    killmethodend:
        Debug.Log("killmethodend");

    }
    /// <summary>
    /// Перемещает дочерний объект из одной клетки в другую
    /// </summary>
    /// <param name="gameObj">Объект в который перемещается дочерний</param>
    public void MoveChild(GameObject gameObj)
    {
        if (!rboard.WhiteTurn && gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("blackdef") || gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("blackcool"))
        {
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blackdef");
            var child = Instantiate(checkerPrefab, this.gameObject.transform);
            CheckForPromote();
            DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
            gameObj.GetComponent<PieceRCh>().HaveChildObj = false;
            gameObj.GetComponent<PieceRCh>().Selected = false;
            gameObj.GetComponent<PieceRCh>().Isempty = true;
            gameObj.GetComponent<PieceRCh>().BlackTeam = false;
            gameObj.GetComponent<PieceRCh>().CanKill = false;
            this.Selected = false;
            this.BlackTeam = true;
            rboard.WhiteTurn = true;
            HaveChild();
        }
        else if (rboard.WhiteTurn && gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("whitedef") || gameObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == Resources.Load<Sprite>("whitecool"))
        {
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("whitedef");
            var child = Instantiate(checkerPrefab, this.gameObject.transform);
            CheckForPromote();
            DestroyImmediate(gameObj.transform.GetChild(0).gameObject);
            gameObj.GetComponent<PieceRCh>().HaveChildObj = false;
            gameObj.GetComponent<PieceRCh>().Selected = false;
            gameObj.GetComponent<PieceRCh>().Isempty = true;
            gameObj.GetComponent<PieceRCh>().WhitTeam = false;
            gameObj.GetComponent<PieceRCh>().CanKill = false;
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
    /// <summary>
    /// Меняет шашку на королевскую
    /// </summary>
    /// <param name="promoteObject"></param>
    public void PromoteChecker(GameObject promoteObject)
    {
        if (promoteObject.GetComponent<PieceRCh>().WhitTeam)
        {
            DestroyImmediate(promoteObject.GetComponent<PieceRCh>().transform.GetChild(0).gameObject);
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("whitecool");
            Instantiate(checkerPrefab, promoteObject.transform);
        }
        else if (promoteObject.GetComponent<PieceRCh>().BlackTeam)
        {
            DestroyImmediate(promoteObject.GetComponent<PieceRCh>().transform.GetChild(0).gameObject);
            checkerPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blackcool");
            Instantiate(checkerPrefab, promoteObject.transform);
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectGame : MonoBehaviour
{
    public void LoadChess()
    {
        SceneManager.LoadScene("Chess");
    }
    public void LoadRCheckers()
    {
        SceneManager.LoadScene("shashki");
    }
}

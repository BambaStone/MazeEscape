using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OverPage : MonoBehaviour
{
    public void GoMainButton()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }
}

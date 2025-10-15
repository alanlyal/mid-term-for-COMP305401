using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void QuitGame()
    { 
    Application.Quit();
        Debug.Log("game was succesfully closed");
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Sources()
    {
        SceneManager.LoadScene("Sources");
    }
}

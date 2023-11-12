using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public GameObject EndScreen;

    // Restarts the game by loading the StartMenu scene
    public void Restart()
    {
        SceneManager.LoadScene("StartMenu");
    }

    // Exits the game
    public void ExitGame()
    {
        Application.Quit();
    }

    // Sets the EndScreen GameObject active
    public void CallEndScreen()
    {
        if (EndScreen != null)
        {
            EndScreen.SetActive(true);
        }
    }

    // Sets the EndScreen GameObject inactive
    public void HideEndScreen()
    {
        if (EndScreen != null)
        {
            EndScreen.SetActive(false);
        }
    }
}
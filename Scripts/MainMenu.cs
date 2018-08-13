using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("__Scene");
    }

    public void Quit()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}

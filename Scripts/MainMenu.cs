using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("__Scene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("__Tutorial");
    }

    public void Quit()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}
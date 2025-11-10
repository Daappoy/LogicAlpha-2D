using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(1, "GameScene"));
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

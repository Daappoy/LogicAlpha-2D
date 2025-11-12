using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuUI;
    public GameObject settingsUI;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        OpenMainMenu();
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highscore.ToString();
    }

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

    public void OpenMainMenu()
    {
        settingsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void UpdateHighScore()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highscore.ToString();
    }
}

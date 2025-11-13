using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameManager.GameState currentState;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (GameIsPaused && !(currentState == GameManager.GameState.GameOver))
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
        else if (!GameIsPaused && !(currentState == GameManager.GameState.GameOver))
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<ScoreManager>().SaveScore();
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(1, "MainMenu"));
        Debug.Log("Loading Main Menu");
        GameIsPaused = false;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.TransisionToScene(2, "GameScene"));
        Debug.Log("Retrying Game");
        GameIsPaused = false;
    }
}

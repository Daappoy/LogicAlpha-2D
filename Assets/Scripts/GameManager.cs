using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        DecidingChoice,
        Battle,
        GameOver
    }

    [Header("Game Stats")]
    public int EnemyDefeated = 0;
    public int PaperUsed = 0;
    public int RockUsed = 0;
    public int ScissorUsed = 0;
    [Header("Enemy Lists")]
    public List<Enemy> Enemies;
    [Header("Spawn Points")]
    public Transform EnemySpawnPoint;
    public Transform PlayerSpawnPoint;
    [Header("Prefabs and script references")]
    public ScoreManager ScoreManager;
    public ActionsManager ActionsManager;
    public GameObject EnemyPrefab;
    public EnemyDisplay currentEnemy;
    public GameObject PlayerPrefab;
    public PlayerDisplay currentPlayer;
    [Header("UI Health Bars")]
    public Slider PlayerHealthBar;
    public Slider EnemyHealthBar;
    [Header("Game UI")]
    public GameObject ActionsUI;
    public GameObject LostUI;
    public TextMeshProUGUI ScoreText;
    
    public GameState currentState;

    public void ResetGameStats()
    {
        EnemyDefeated = 0;
        ScoreManager.TotalScore = 0;
        PaperUsed = 0;
        RockUsed = 0;
        ScissorUsed = 0;
    }

    void Awake()
    {
        ScoreManager = FindObjectOfType<ScoreManager>();
        currentState = GameState.DecidingChoice;
        ResetGameStats();
        SpawnPlayer();
        SpawnRandomEnemy();
    }
    
    void Start()
    {
        ScoreManager = FindObjectOfType<ScoreManager>();
        LostUI.SetActive(false);
        UpdateUIBasedOnState();
        UpdateHealthBars();
    }


    public void UpdateUIBasedOnState()
    {
        if (currentState == GameState.DecidingChoice)
        {
            ActionsUI.SetActive(true);
        }
        else if (currentState == GameState.Battle)
        {
            ActionsUI.SetActive(false);
            LostUI.SetActive(false);
        }
        else if (currentState == GameState.GameOver)
        {
            ActionsUI.SetActive(false);
            LostUI.SetActive(true);
        }
    }

    [ContextMenu("Spawn Random Enemy")]
    public void SpawnRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, Enemies.Count);
        Enemy randomEnemy = Enemies[randomIndex];

        GameObject enemyInstance = Instantiate(EnemyPrefab, EnemySpawnPoint.position, Quaternion.identity);
        EnemyDisplay enemyDisplay = enemyInstance.GetComponent<EnemyDisplay>();
        if (enemyDisplay != null)
        {
            enemyDisplay.enemy = randomEnemy;
            enemyDisplay.currentHealth = randomEnemy.health;
            enemyDisplay.damage = randomEnemy.damage;
            currentEnemy = enemyDisplay;
        }
        
        currentState = GameState.DecidingChoice;
        UpdateUIBasedOnState();
        UpdateHealthBars();
        ActionsManager.ResetText();
    }
    public void SpawnPlayer()
    {
        Debug.Log("spawning player");
        GameObject playerInstance = Instantiate(PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
        PlayerDisplay playerDisplay = playerInstance.GetComponent<PlayerDisplay>();
        if (playerDisplay != null)
        {
            currentPlayer = playerDisplay;
        }
        
    }
    
    
    [ContextMenu("Destroy Spawned Enemy")]
    public void DestroySpawnedEnemy()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy.gameObject);
            currentEnemy = null;
        }

    }
    //proses menang round atau kalah round
    public void LostRound()
    {
        currentPlayer.currentHealth -= currentEnemy.damage;
        // currentPlayer.currentHealth = 0;
        if (currentPlayer.currentHealth <= 0)
        {
            currentState = GameState.GameOver;
            UpdateUIBasedOnState();

            currentPlayer.DestroySelf();
            currentPlayer = null;
            return;
        }
        UpdateHealthBars();
    }
    //kalo musuh mati, actionnya bakal nyala lagi as soon as musuhnya di spawn
    public void WonRound()
    {
        currentEnemy.currentHealth -= currentPlayer.damage;
        // currentEnemy.currentHealth = 0;
        if (currentEnemy.currentHealth <= 0)
        {
            Debug.Log("Enemy Defeated");
            EnemyDefeated += 1;
            ScoreManager.TotalScore += 2000;
            UpdateHealthBars();
            StartCoroutine(NewRound());
            // action button nyala as soon as enemynya di spawn
            ScoreUpdate();
            return;
        }
        //kalo musuhnya gak mati, button actionnya langsung nyalain lagi
        UpdateHealthBars();
    }
    public IEnumerator NewRound()
    {
        DestroySpawnedEnemy();
        currentEnemy = null;
        yield return new WaitForSeconds(1.5f);
        SpawnRandomEnemy();

        currentState = GameState.DecidingChoice;
        UpdateUIBasedOnState();
    }

    [ContextMenu("Update Health Bars")]
    void UpdateHealthBars()
    {
        if (currentPlayer != null)
        {
            PlayerHealthBar.value = currentPlayer.currentHealth;
        }
        else
        {
            PlayerHealthBar.value = 0;
        }

        if (currentEnemy != null)
        {
            EnemyHealthBar.value = currentEnemy.currentHealth;
        }
        else
        {
            EnemyHealthBar.value = 0;
        }
    }

    public void ScoreUpdate()
    {
        ScoreText.text = "Score: " + ScoreManager.TotalScore.ToString();
    }

    [ContextMenu("Lost Simulation")]
    public void LostSimulation()
    {
        currentState = GameState.GameOver;
        UpdateUIBasedOnState();
    }

}

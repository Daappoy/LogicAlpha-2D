using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;

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
    public AnimatorManager AnimatorManager;
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
    [Header("Animators")]
    public Animator PlayerAnimator;
    public Animator EnemyAnimator;

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
        //atur rotasi Y biar ngadep ke player
        enemyInstance.transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);

        EnemyAnimator = enemyInstance.GetComponent<Animator>();
        AnimatorManager.enemyAnimator = EnemyAnimator;
        currentState = GameState.DecidingChoice;
        UpdateUIBasedOnState();
        UpdateHealthBars();
        ActionsManager.ResetText();
        enemyDisplay.ApplyEnemyData();
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
        PlayerAnimator = playerInstance.GetComponent<Animator>();
        AnimatorManager.playerAnimator = PlayerAnimator;
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
        AudioManager.AudioInstance.PlaySFX(AudioManager.AudioInstance.AttackSound);
        currentPlayer.currentHealth -= currentEnemy.damage;
        // currentPlayer.currentHealth = 0;
        if (currentPlayer.currentHealth <= 0) // kalo player mati
        {
            AudioManager.AudioInstance.PlaySFX(AudioManager.AudioInstance.DeathSound);
            UpdateHealthBars();
            StartCoroutine(LostGame());
            return;
        }
        else
        {
            AnimatorManager.EnemyAttack();
            AnimatorManager.PlayerHurt();
            StartCoroutine(ReturnToNormalState());
        }
    }
    public void WonRound()
    {
        AudioManager.AudioInstance.PlaySFX(AudioManager.AudioInstance.AttackSound);
        currentEnemy.currentHealth -= currentPlayer.damage;
        // currentEnemy.currentHealth = 0;
        if (currentEnemy.currentHealth <= 0) // kalo musuh mati
        {
            AudioManager.AudioInstance.PlaySFX(AudioManager.AudioInstance.DeathSound);
            Debug.Log("Enemy Defeated");
            EnemyDefeated += 1;
            ScoreManager.TotalScore += 2000;
            UpdateHealthBars();
            StartCoroutine(NewRound());
            ScoreUpdate();
            return;
        }
        else
        {
            AnimatorManager.PlayerAttack();
            AnimatorManager.EnemyHurt();
            StartCoroutine(ReturnToNormalState());
        }
    }
    public IEnumerator NewRound()
    {
        AnimatorManager.PlayerAttack();
        AnimatorManager.EnemyDeath();
        yield return new WaitForSeconds(1f);
        DestroySpawnedEnemy();
        currentEnemy = null;
        yield return new WaitForSeconds(0.5f);
        SpawnRandomEnemy();

        currentState = GameState.DecidingChoice;
        UpdateUIBasedOnState();
        ActionsManager.ResetText();
        ActionsManager.ActionButtonOn();
        AnimatorManager.PlayerIdle();
    }
    public IEnumerator LostGame()
    {
        AnimatorManager.EnemyAttack();
        AnimatorManager.PlayerDeath();
        yield return new WaitForSeconds(1.5f);
        currentState = GameState.GameOver;
        UpdateUIBasedOnState();
    }

    public IEnumerator ReturnToNormalState()
    {
        UpdateHealthBars();
        yield return new WaitForSeconds(1.5f);
        currentState = GameState.DecidingChoice;
        UpdateUIBasedOnState();
        ActionsManager.ResetText();
        ActionsManager.ActionButtonOn();
        AnimatorManager.EnemyIdle();
        AnimatorManager.PlayerIdle();
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

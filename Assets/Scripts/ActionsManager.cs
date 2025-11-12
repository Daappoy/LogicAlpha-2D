using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionsManager : MonoBehaviour
{
    public GameManager gameManager;
    public Actions playerAction;
    public Actions enemyAction;
    public TextMeshProUGUI ActionResultText;
    public TextMeshProUGUI PlayerActionText;
    public TextMeshProUGUI EnemyActionText;

    public enum Actions
    {
        Paper,
        Rock,
        Scissor
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        PlayerActionText.text = "";
        EnemyActionText.text = "";
    }

    public void ChooseRock()
    {
        playerAction = Actions.Rock;
        PerformAction(playerAction);
        gameManager.RockUsed += 1;
    }
    public void ChoosePaper()
    {
        playerAction = Actions.Paper;
        PerformAction(playerAction);
        gameManager.PaperUsed += 1;
    }
    public void ChooseScissor()
    {
        playerAction = Actions.Scissor;
        PerformAction(playerAction);
        gameManager.ScissorUsed += 1;
    }

    public void PerformAction(Actions playerAction)
    {
        gameManager.currentState = GameManager.GameState.Battle;
        gameManager.UpdateUIBasedOnState();
        // Debug.Log("Player performed action: " + playerAction.ToString());
        enemyAction = (Actions)Random.Range(0, Actions.GetNames(typeof(Actions)).Length);
        // Debug.Log("Enemy performed action: " + enemyAction.ToString());
        StartCoroutine(ProcessBattleOutcome());
    }

    //ini menentukan kalo playernya menang atau gak
    private IEnumerator ProcessBattleOutcome()
    {
        PlayerActionText.text = playerAction.ToString() + "!";
        yield return new WaitForSeconds(1f);
        EnemyActionText.text = enemyAction.ToString() + "!";
        yield return new WaitForSeconds(1f);

        if (playerAction == enemyAction)
        {
            ActionResultText.text = "Tie, Try again.";
            // Debug.Log("It's a tie!");
            yield return new WaitForSeconds(1.5f);
            ResetText();
            ActionButtonOn();
        }
        else if
        ((playerAction == Actions.Rock && enemyAction == Actions.Scissor) ||
         (playerAction == Actions.Paper && enemyAction == Actions.Rock) ||
         (playerAction == Actions.Scissor && enemyAction == Actions.Paper))
        {
            // Player Menang
            ActionResultText.text = "Win!";
            gameManager.WonRound();
            // Debug.Log("Player wins!");
            yield return new WaitForSeconds(1.5f);
            ResetText();
            gameManager.currentState = GameManager.GameState.DecidingChoice;
            ActionButtonOn();
        }
        else
        {
            // Enemy Menang
            ActionResultText.text = "Lose!";
            gameManager.LostRound();
            // Debug.Log("Enemy wins!");
            yield return new WaitForSeconds(1.5f);
            ResetText();
            
        }
    }
    
    public void ActionButtonOn()
    {
        gameManager.currentState = GameManager.GameState.DecidingChoice;
        gameManager.UpdateUIBasedOnState();
    }

    public void ResetText()
    {
        PlayerActionText.text = "";
        EnemyActionText.text = "";
        ActionResultText.text = "";
    }
}

using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator enemyAnimator;

    //player animator functions
    public void PlayerHurt()
    {
        playerAnimator.SetTrigger("PlayerHurt");
    }
    public void PlayerAttack()
    {
        playerAnimator.SetTrigger("PlayerAttack");
    }
    public void PlayerDeath()
    {
        playerAnimator.SetTrigger("PlayerDeath");
    }
    public void PlayerIdle()
    {
        playerAnimator.SetTrigger("PlayerIdle");
    }
    //enemy animator functions
    public void EnemyHurt()
    {
        enemyAnimator.SetTrigger("Hurt");
    }
    public void EnemyAttack()
    {
        enemyAnimator.SetTrigger("Attack");
    }
    public void EnemyDeath()
    {
        enemyAnimator.SetTrigger("Death");
    }
    public void EnemyIdle()
    {
        enemyAnimator.SetTrigger("Idle");
    }
}

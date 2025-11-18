using UnityEngine;

public class EnemyDisplay : CharacterDisplay
{
    [Header("Assigned Scriptable Object")]
    public string enemyName;
    public Enemy enemy;

    [Header("Runtime Data")]
    public Animator enemyAnimator;

    [ContextMenu("Apply Enemy Data")]
    public void ApplyEnemyData()
    {
        enemyName = enemy.enemyName;
        currentHealth = enemy.health;
        damage = enemy.damage;

        if (enemy.animatorController != null)
        {
            enemyAnimator.runtimeAnimatorController = enemy.animatorController;
        }
        else
        {
            Debug.LogWarning($"Enemy {enemy.enemyName} belum memiliki Animator Controller!");
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

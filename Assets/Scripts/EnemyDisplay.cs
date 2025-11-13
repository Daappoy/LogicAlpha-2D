using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    [Header("Assigned Scriptable Object")]
    public Enemy enemy;

    [Header("Runtime Data")]
    public int currentHealth;
    public int damage;
    public Animator enemyAnimator;

    [ContextMenu("Apply Enemy Data")]
    public void ApplyEnemyData()
    {
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

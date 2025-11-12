using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;
    public int currentHealth;
    public int damage;
    void Awake()
    {
        if (enemy != null)
        {
            currentHealth = enemy.health;
            damage = enemy.damage;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

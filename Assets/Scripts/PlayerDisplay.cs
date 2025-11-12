using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour
{
    public PlayerStats playerStats;
    public int currentHealth;
    public int damage;
    void Awake()
    {
        if (playerStats != null)
        {
            currentHealth = playerStats.Health;
            damage = playerStats.Damage;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;

public class PlayerDisplay : CharacterDisplay
{
    public PlayerStats playerStats;
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

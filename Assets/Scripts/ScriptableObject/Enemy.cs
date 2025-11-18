using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public int health;
    public int damage;
    public RuntimeAnimatorController animatorController;
}

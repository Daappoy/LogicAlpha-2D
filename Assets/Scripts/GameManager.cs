using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int EnemyDefeated = 0;
    public int TotalScore = 0;
    public int PaperUsed = 0;
    public int RockUsed = 0;
    public int ScissorUsed = 0;

    public void ResetGameStats()
    {
        EnemyDefeated = 0;
        TotalScore = 0;
        PaperUsed = 0;
        RockUsed = 0;
        ScissorUsed = 0;
    }

    
}

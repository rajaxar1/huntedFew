using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    public BossStats bossStats = new BossStats();

    public void DamageBoss(int damage)
    {
        bossStats.health -= damage;
        if (bossStats.health <= 0f)
        {
            GameMaster.KillBoss(this);
        }
    }

    public class BossStats
    {
        public float health = 100f;

    }
}

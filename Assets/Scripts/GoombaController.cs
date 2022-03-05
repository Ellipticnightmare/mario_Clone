using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : EnemyController
{
    public override void RunHit()
    {
        base.RunHit();
        RunDeath();
    }
    public void RunDeath()
    {
        Destroy(this.gameObject);
        GameManager.GainPoints(100);
    }
}
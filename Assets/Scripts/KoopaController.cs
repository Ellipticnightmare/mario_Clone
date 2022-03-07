using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaController : EnemyController
{
    public GameObject shell;
    public override void RunHit()
    {
        base.RunHit();
        RunShell();
    }
    public void RunShell()
    {
        GameObject spawnShell = Instantiate(shell, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("shell"))
            RunDeath();
    }
    public void RunDeath()
    {
        Destroy(this.gameObject);
        GameManager.GainPoints(score);
    }
}
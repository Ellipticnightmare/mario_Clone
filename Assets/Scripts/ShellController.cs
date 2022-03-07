using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : EnemyController
{
    int dir = 0;
    public float respawnTimer;
    public GameObject Koopa;
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir, 0.0f), flipDistance, layerMask);
        if (hit.collider != null)
            dir *= -1;
        transform.Translate(new Vector2(dir * speed * Time.deltaTime, 0));
        if (dir == 0)
        {
            if (respawnTimer >= 0)
                respawnTimer -= Time.deltaTime;
            else
            {
                GameObject spawnKoop = Instantiate(Koopa, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
        else
            respawnTimer = 4;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("shell"))
            RunDeath();
        else if (other.gameObject.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
            GameManager.GainPoints(other.GetComponent<EnemyController>().score);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            StartKick();
        }
    }
    public void RunDeath()
    {
        Destroy(this.gameObject);
        GameManager.GainPoints(100);
    }
    public void StartKick()
    {
        Vector2 B = FindObjectOfType<marioController>().transform.position;
        if (AngleDir(this.transform.position, B) >= 0)
            dir = -1;
        else
            dir = 1;
    }
    float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }
    public override void RunHit()
    {
        base.RunHit();
        dir = 0;
    }
}
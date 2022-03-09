using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyToSpawn;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            Instantiate(enemyToSpawn, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}

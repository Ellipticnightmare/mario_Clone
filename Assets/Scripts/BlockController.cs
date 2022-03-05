using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public GameObject spawnObj, DeadBlock;
    public int spawnCount;
    public int spawnAboveDist;
    public void RunHit() //Gets called from marioController when block is hit from beneath
    {
        spawnCount--;
        GameObject newSpawn = Instantiate(spawnObj, new Vector3(this.transform.position.x, this.transform.position.y + spawnAboveDist, 0), Quaternion.identity);
        if(spawnCount <= 0)
        {
            GameObject deadBlock = Instantiate(DeadBlock, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
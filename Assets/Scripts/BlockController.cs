using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public blockType BlockType = blockType.baseBrick;
    public GameObject spawnObj, DeadBlock;
    public int spawnCount;
    public int spawnAboveDist;
    public void RunHit() //Gets called from marioController when block is hit from beneath
    {
        switch (BlockType)
        {
            case blockType.baseBrick:
                if (FindObjectOfType<marioController>().MarioState != marioController.marioState.small)
                    Destroy(this.gameObject);
                break;
            case blockType.hasItem:
                spawnCount--;
                GameObject newSpawn = Instantiate(spawnObj, new Vector3(this.transform.position.x, this.transform.position.y + spawnAboveDist, 0), Quaternion.identity);
                if (spawnCount <= 0)
                {
                    GameObject deadBlock = Instantiate(DeadBlock, transform.position, transform.rotation);
                    Destroy(this.gameObject);
                }
                break;
            case blockType.hasCoin:
                spawnCount--;
                FindObjectOfType<GameManager>().GainCoin();
                if (spawnCount <= 0)
                {
                    GameObject deadBlock = Instantiate(DeadBlock, transform.position, transform.rotation);
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    public enum blockType
    {
        baseBrick,
        hasItem,
        hasCoin
    };
}
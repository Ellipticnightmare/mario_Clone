using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityMover : EnemyController
{
    public ColliderBridge colBridge;
    public bool needsTag;
    public string neededTag;
    private void Start()
    {
        colBridge.thisControl = this;
    }
    public void Collided(Collider2D collision)
    {
        if (needsTag && collision.gameObject.CompareTag(neededTag))
            Destroy(this.gameObject);
        else if (!needsTag)
            Destroy(this.gameObject);
    }
}
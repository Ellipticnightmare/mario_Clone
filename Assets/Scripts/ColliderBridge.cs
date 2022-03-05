using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    public entityMover thisControl;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        thisControl.Collided(collision);
    }
}
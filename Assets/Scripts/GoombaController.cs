using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : MonoBehaviour
{
    public float speed; //the goombas speed
    public float flipDistance; //how far the raycast looking for walls goes, 0.32 should be fine
    private int dir; // direction goomba is going in
    private int layerMask; //bitmask to choose layer to ignore in raycasts

    private void Start()
    {
        dir = -1;   //goes to the left initially
        layerMask = 1 << 9; 
        layerMask = ~layerMask; //bitmask that tells the raycast to ignore layer 9 (enemy layer)
    }

    public void Update()
    {
        //checking for walls ahead with a raycast, flips direction if found
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir,0.0f), flipDistance, layerMask);
        if (hit.collider != null)
        {
            dir *= -1;
        }
        //movement!
        transform.Translate(new Vector2(dir * speed * Time.deltaTime, 0.0f));
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float flipDistance = .032f;
    int dir;
    public int score;
    public LayerMask layerMask;
    private void Start()
    {
        dir = -1;
    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir, 0.0f), flipDistance, layerMask);
        if(hit.collider != null)
            dir *= -1;
        transform.Translate(new Vector2(dir * speed * Time.deltaTime, 0.0f));
    }
    public virtual void RunHit() => dataLogger.throwMessage("hit " + this.gameObject.name);
}
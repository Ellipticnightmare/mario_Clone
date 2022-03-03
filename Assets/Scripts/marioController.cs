using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class marioController : MonoBehaviour
{
    public marioState MarioState = marioState.small;
    public UnityEvent onDeath, coinPickup, lifeUp;
    public void RunDeath() => onDeath.Invoke();
    public void RunCoin() => coinPickup.Invoke();
    public void RunLifeShroom() => lifeUp.Invoke();
    public void UpdateMarioAppearance()
    {

    }
    public void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit upHit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out upHit, 1.1f, layerMask))
        {
            if (upHit.collider.tag == "block")
                upHit.collider.gameObject.GetComponent<BlockController>().RunHit();
        }
        RaycastHit downHit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out downHit, 1.1f, layerMask))
        {
            if (downHit.collider.tag == "enemy")
                downHit.collider.gameObject.GetComponent<EnemyController>().RunHit();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag.ToString())
        {
            case "killzone":
                RunDeath();
                break;
            case "enemy":
                switch (MarioState)
                {
                    case marioState.small:
                        RunDeath();
                        break;
                    case marioState.big:
                        MarioState = marioState.small;
                        UpdateMarioAppearance();
                        break;
                    case marioState.star:
                        break;
                    case marioState.flower:
                        MarioState = marioState.big;
                        UpdateMarioAppearance();
                        break;
                }
                break;
            case "mushroom":
                MarioState = marioState.big;
                UpdateMarioAppearance();
                Destroy(collision.gameObject);
                break;
            case "flower":
                MarioState = marioState.flower;
                UpdateMarioAppearance();
                Destroy(collision.gameObject);
                break;
            case "star":
                MarioState = marioState.star;
                UpdateMarioAppearance();
                Destroy(collision.gameObject);
                break;
            case "coin":
                RunCoin();
                Destroy(collision.gameObject);
                break;
            case "lifeUp":
                RunLifeShroom();
                Destroy(collision.gameObject);
                break;
        }
    }
    public enum marioState
    {
        small,
        big,
        star,
        flower
    };
}
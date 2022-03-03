using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class marioController : MonoBehaviour
{
    public marioState MarioState = marioState.small;
    public UnityEvent onDeath, coinPickup, lifeUp;
    public keyBindingData[] keyBinds;
    public int baseSpeed;
    public GameObject visualHolder;
    public BoxCollider2D standCol, crouchCol;
    float hInput, vInput;
    float movSpeed;
    bool isGrounded;
    Rigidbody2D rigid;
    KeyCode right, left, up, down, jump, bButton;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        #region KeyBinding
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds[0].keyName);
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds[1].keyName);
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds[2].keyName);
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds[3].keyName);
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds[4].keyName);
        bButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds[5].keyName);
        #endregion

        float lNorm = (Input.GetKey(left)) ? 1 : 0;
        float rNorm = (Input.GetKey(right)) ? 1 : 0;
        float uNorm = (Input.GetKey(up)) ? 1 : 0;
        float dNorm = (Input.GetKey(down)) ? 1 : 0;
        float bButtonF = (Input.GetKey(bButton)) ? 1 : 0;

        hInput = rNorm - lNorm;
        vInput = uNorm - dNorm;

        if (Input.GetKey(down))
            bButtonF = 0;
        movSpeed = (bButtonF > 0) ? baseSpeed * 1.5f : Input.GetKey(down) ? baseSpeed * .75f : baseSpeed;

        rigid.velocity = new Vector2(hInput * movSpeed, rigid.velocity.y);

        if (hInput != 0)
            visualHolder.transform.localScale = new Vector3(hInput, 1, 1);

        standCol.enabled = (Input.GetKey(down)) ? false : true;
        crouchCol.enabled = (Input.GetKey(down)) ? true : false;
    }
    public void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit upHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out upHit, 1.1f, layerMask))
        {
            if (upHit.collider.tag == "block")
                upHit.collider.gameObject.GetComponent<BlockController>().RunHit();
        }
        RaycastHit downHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out downHit, 1.1f, layerMask))
        {
            isGrounded = true;
            if (downHit.collider.tag == "enemy")
                downHit.collider.gameObject.GetComponent<EnemyController>().RunHit();
        }
        else
            isGrounded = false;

        if (isGrounded && Input.GetKey(jump))
        {
            rigid.AddForce(Vector2.up * movSpeed * 10);
        }
    }
    public void UpdateMarioAppearance()
    {

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
    public void RunDeath() => onDeath.Invoke();
    public void RunCoin() => coinPickup.Invoke();
    public void RunLifeShroom() => lifeUp.Invoke();
    public enum marioState
    {
        small,
        big,
        star,
        flower
    };
}
[System.Serializable]
public struct keyBindingData
{
    public string commandName, keyName;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))] //Controls movement
[RequireComponent(typeof(BoxCollider2D))] //detects Crouching collisions
[RequireComponent(typeof(BoxCollider2D))] //detects Standing collisions
public class marioController : MonoBehaviour
{
    #region Variables
    #region Accessible
    public marioState MarioState = marioState.small; //manages health
    public UnityEvent onDeath, coinPickup, lifeUp; //runs events out to GameManager script, rather than needing references or Finds
    public keybinDatabase keyBinds; //lets us set custom keybindings, or even rebindable keys down the line.  Easier than changing script or Input Manager
    [Range(1,5)] //Sets a range for the speed
    public int baseSpeed; //Mario's base movement speed
    public GameObject visualHolder; //Holds the sprites/animations, etc in a dedicated object for ease of manipulation
    public BoxCollider2D standCol, crouchCol; //References the two colliders
    #endregion
    #region Protected
    float hInput, vInput; //Creates axes based on vertical and horizontal movement
    float movSpeed; //adjustable float based on basespeed and inputs
    bool isGrounded; //Detects if we're standing on something, regardless of type, lets us jump off of enemies and such as well as ground
    Rigidbody2D rigid; //Rigidbody reference
    KeyCode right, left, up, down, jump, bButton; //Stores keycodes so they can be accessed by other areas of the script
    marioState heldState = marioState.small; //holds the previous state for coming out of star
    #endregion
    #endregion

    #region Functions
    #region Base
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); //Assign Rigidbody automatically
    }
    public void Update()
    {
        #region KeyBinding //Read keybinding data
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds.keyBinds[0].keyName);
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds.keyBinds[1].keyName);
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds.keyBinds[2].keyName);
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds.keyBinds[3].keyName);
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds.keyBinds[4].keyName);
        bButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyBinds.keyBinds[5].keyName);
        #endregion

        float lNorm = (Input.GetKey(left)) ? 1 : 0; //Take in left input data
        float rNorm = (Input.GetKey(right)) ? 1 : 0; //Take in right input data
        float uNorm = (Input.GetKey(up)) ? 1 : 0; //Take in up input data
        float dNorm = (Input.GetKey(down)) ? 1 : 0; //Take in down input data
        float bButtonF = (Input.GetKey(bButton)) ? 1 : 0; //Take in Special input data

        hInput = rNorm - lNorm; //Build horizontal axis
        vInput = uNorm - dNorm; //Build vertical axis

        if (Input.GetKey(down)) //If crouching, prevent running, firing fireballs, etc
            bButtonF = 0;
        movSpeed = (bButtonF > 0) ? baseSpeed * 1.5f : Input.GetKey(down) ? baseSpeed * .75f : baseSpeed; //Adjust speed based on running, crouching, etc

        rigid.velocity = new Vector2(hInput * movSpeed, rigid.velocity.y); //Set velocity for movement

        if (hInput != 0)
            visualHolder.transform.localScale = new Vector3(hInput, 1, 1); //flip mario left or right depending on movement

        standCol.enabled = (Input.GetKey(down)) ? false : true; //If crouching, turn off standing collider
        crouchCol.enabled = (Input.GetKey(down)) ? true : false; //If standing, turn off crouching collider
    }
    public void FixedUpdate()
    {
        int layerMask = 1 << 8; //Read layers up to player layer
        layerMask = ~layerMask; //Read all layers except for player layer
        RaycastHit upHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out upHit, 1.1f, layerMask)) //Detect what Mario bonks his head into
        {
            if (upHit.collider.tag == "block")
                upHit.collider.gameObject.GetComponent<BlockController>().RunHit();
        }
        RaycastHit downHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out downHit, 1.1f, layerMask)) //Detect what Mario steps on, if stepping, make script know Mario is grounded
        {
            isGrounded = true;
            if (downHit.collider.tag == "enemy")
                downHit.collider.gameObject.GetComponent<EnemyController>().RunHit();
        }
        else
            isGrounded = false;

        if (isGrounded && Input.GetKey(jump)) //Allow for jumping if Mario is grounded
        {
            rigid.AddForce(Vector2.up * movSpeed * 10);
        }
    }
    #endregion
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag.ToString())
        {
            case "killzone": //Kill Mario upon falling through hole in ground
                RunDeath();
                break;
            case "enemy": //Update Mario's health based on size
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
            case "mushroom": //Increase Mario's health when small
                if (MarioState == marioState.small)
                    MarioState = marioState.big;
                UpdateMarioAppearance();
                Destroy(collision.gameObject);
                break;
            case "flower": //Move to Flower form
                MarioState = marioState.flower;
                UpdateMarioAppearance();
                Destroy(collision.gameObject);
                break;
            case "star": //Move to Star form
                heldState = MarioState;
                MarioState = marioState.star;
                UpdateMarioAppearance();
                Destroy(collision.gameObject);
                break;
            case "coin": //Pick up coin
                RunCoin();
                Destroy(collision.gameObject);
                break;
            case "lifeUp": //Gain extra life
                RunLifeShroom();
                Destroy(collision.gameObject);
                break;
        }
    }
    #region Custom
    public void UpdateMarioAppearance()
    {
        //Logic to change Mario's appearance will go here
    }
    public void RunDeath() => onDeath.Invoke(); //Call to GameManager, tell them we died
    public void RunCoin() => coinPickup.Invoke(); //Call to GameManager, tell them we picked up a coin
    public void RunLifeShroom() => lifeUp.Invoke(); //Call to GameManager, tell them we gained a life
    #endregion
    #endregion

    #region States
    public enum marioState //health state
    {
        small,
        big,
        star,
        flower
    };
    #endregion
}
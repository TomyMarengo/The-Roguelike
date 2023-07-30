using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [HideInInspector]
    public float lastHorizontal;
    [HideInInspector]
    public float lastVertical;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMoved;

    //References
    Rigidbody2D rb;
    PlayerStats player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMoved = new Vector2(1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
    }

    void FixedUpdate() 
    {
        Move();
    }

    void InputManagement()
    {
        if(GameManager.instance.currentState != GameManager.GameState.Gameplay)
            return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0) {
            lastHorizontal = moveDir.x;
            lastMoved = new Vector2(lastHorizontal, 0f);
        }
            
        if (moveDir.y != 0) {
            lastVertical = moveDir.y;
            lastMoved = new Vector2(0f, lastVertical);
        }

        if (moveDir.x != 0 && moveDir.y != 0) {
            lastMoved = new Vector2(lastHorizontal, lastVertical);
        }
            
    }

    void Move() 
    {
        if(GameManager.instance.currentState != GameManager.GameState.Gameplay)
            return;

        rb.velocity = new Vector2(moveDir.x, moveDir.y) * player.CurrentMoveSpeed;
    }
}

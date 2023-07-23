using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontal;
    [HideInInspector]
    public float lastVertical;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMoved;


    // Start is called before the first frame update
    void Start()
    {
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
        rb.velocity = new Vector2(moveDir.x, moveDir.y) * moveSpeed;
    }
}
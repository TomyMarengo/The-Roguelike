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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (moveDir.x != 0)
            lastHorizontal = moveDir.x;
        if (moveDir.y != 0)
            lastVertical = moveDir.y;
    }

    void Move() 
    {
        rb.velocity = new Vector2(moveDir.x, moveDir.y) * moveSpeed;
    }
}

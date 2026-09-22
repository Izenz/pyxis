using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float HorizontalInput;
    float MoveSpeed = 5f;
    bool isFacingRight = true;
    float JumpPower = 4f;
    bool isJumping = false;

    Rigidbody2D RB; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                HorizontalInput = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                HorizontalInput = 1f;
            }
            if ((Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) && !isJumping)
            {
                RB.linearVelocity = new Vector2(RB.linearVelocity.x, JumpPower);
                isJumping = true;
            }
        }

        FlipSprite();
    }

    private void FixedUpdate()
    {
        RB.linearVelocity = new Vector2(HorizontalInput * MoveSpeed, RB.linearVelocity.y);
    }

    void FlipSprite()
    {
        if (isFacingRight && HorizontalInput < 0f || !isFacingRight && HorizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 LocalScale = transform.localScale;
            LocalScale.x *= -1f;
            transform.localScale = LocalScale;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        isJumping = false;
    }
}

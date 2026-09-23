using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementState
    {
        Floor,
        Jumping
    }

    MovementState state = MovementState.Floor;

    float horizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;
    float jumpPower = 4f;

    Rigidbody2D rb;

    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        if (moveAction != null)
        {
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            horizontalInput = moveValue.x;
        }

        if (jumpAction != null && jumpAction.WasPressedThisFrame() && state != MovementState.Jumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            state = MovementState.Jumping;
        }

        CheckFlipSprite();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void CheckFlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        state = MovementState.Floor;
    }
}
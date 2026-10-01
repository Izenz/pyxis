using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementState
    {
        Floor,
        Jumping,
        Dashing
    }

    MovementState state = MovementState.Floor;

    float horizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true;
    float jumpPower = 4f;

    float dashPower = 15f;
    float dashDuration = 0.2f;
    float dashCooldown = 1f;
    bool canDash = true;
    bool isDashing = false;
    float originalGravity;

    Rigidbody2D rb;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction dashAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
    }

    void Update()
    {
        if (isDashing) return;

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

        if (dashAction != null && dashAction.WasPressedThisFrame() && canDash)
        {
            StartCoroutine(Dash());
        }

        CheckFlipSprite();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        MovementState previousState = state;
        state = MovementState.Dashing;

        float dashDirection = isFacingRight ? 1f : -1f;
        if (horizontalInput != 0f)
        {
            dashDirection = Mathf.Sign(horizontalInput);
        }

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(dashDirection * dashPower, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
        state = previousState;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
        if (!isDashing)
        {
            state = MovementState.Floor;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Detection Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Combat Settings")]
    [SerializeField] private float knockbackForce = 5f;

    private Vector2 moveInput;
    private bool isGrounded;

    protected override void Awake() => base.Awake();

    private void Update()
    {
        if (isDead) return;

        CheckEnvironment();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Move();
    }

    // --- Unity Event Receivers ---
    // Link these in the PlayerInput component under "Events"

    public void OnMove(InputAction.CallbackContext context)
    {
        // Reads the Vector2 value (WASD/Joystick)
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // context.started is equivalent to "GetButtonDown"
        if (context.started && isGrounded && !isDead)
        {
            Jump();
        }
    }

    // --- Core Logic ---

    public override void Move()
    {
        rBody.linearVelocity = new Vector2(moveInput.x * MoveSpeed, rBody.linearVelocity.y);
        FlipSprite(moveInput.x);
    }

    private void Jump()
    {
        rBody.linearVelocity = new Vector2(rBody.linearVelocity.x, jumpForce);
        anim.SetTrigger("Jump");
    }

    private void CheckEnvironment()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(rBody.linearVelocity.x));
        anim.SetFloat("VerticalVelocity", rBody.linearVelocity.y);
        anim.SetBool("IsGrounded", isGrounded);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if(!isDead)
        {
            anim.SetTrigger("Hurt");

            // Small knockback
            ApplyKnockback();
        }
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;

        anim.SetTrigger("Death");
        rBody.linearVelocity = Vector2.zero;
        rBody.simulated = false;

        moveInput = Vector2.zero;
    }

    private void ApplyKnockback()
    {
        // Determine direction: push away from where the player is currently facing

        float pushDirection = transform.localScale.x > 0 ? -1f : 1f;

        // Reset velocity first so the knockback is consistent
        rBody.linearVelocity = Vector2.zero;
        rBody.AddForce(new Vector2(pushDirection * knockbackForce, knockbackForce), ForceMode2D.Impulse);
    }
}

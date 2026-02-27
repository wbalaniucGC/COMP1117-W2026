using System.Collections;
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

    [Header("Combat & Health Settings")]
    [SerializeField] private float knockbackForce = 7f;
    [SerializeField] private float iframeDuration = 1.5f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private float hurtStunTime = 0.3f;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isInvulnerable = false;
    private bool isStunned = false;

    protected override void Awake() => base.Awake();

    private void Update()
    {
        if (isDead) return;

        CheckEnvironment();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (isDead || isStunned) return;
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
        if (isDead || isInvulnerable) return;

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            Die();            
        }
        else
        {
            StartCoroutine(HandleHurtSequence());
        }
    }

    private IEnumerator HandleHurtSequence()
    {
        isInvulnerable = true;
        isStunned = true; // Lock input
        anim.SetTrigger("Hurt");

        ApplyKnockback();

        // Wait for the stun to end before allowwing movement
        yield return new WaitForSeconds(hurtStunTime);
        isStunned = false;

        // iFrame Flashing Effect
        float timer = 0;
        while (timer < iframeDuration)
        {
            sRend.enabled = !sRend.enabled; // Flicker the sprite
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        sRend.enabled = true;
        isInvulnerable = false;
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;
        isStunned = false;

        if(sRend != null)
        {
            sRend.sortingLayerName = "Foreground";
            sRend.sortingOrder = 100;
        }

        anim.SetBool("IsDead", true);
        StopAllCoroutines();
        StartCoroutine(MarioDeathSequence());
    }

    private IEnumerator MarioDeathSequence()
    {
        // Phase 1: Freeze and Pose
        anim.SetTrigger("Hurt");
        rBody.linearVelocity = Vector2.zero;
        rBody.simulated = false; // Ignore physics temporarily

        yield return new WaitForSeconds(0.5f); // The "Oh no" moment

        // Phase 2: The Death Leap
        GetComponent<Collider2D>().enabled = false; // Fall through floors
        rBody.simulated = true;
        rBody.gravityScale = 3f; // Fast fall
        rBody.linearVelocity = new Vector2(0, 10f); // Upward pop

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
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

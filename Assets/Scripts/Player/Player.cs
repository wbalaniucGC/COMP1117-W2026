using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumps = 2;  // Totals jumps allowed.

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

    [Header("UI References")]
    [SerializeField] private GameOverUI gameOverUI;

    private Vector2 moveInput;
    private bool isGrounded;
    private int jumpsRemaining;
    private bool isInvulnerable = false;
    private bool isStunned = false;

    protected override void Awake()
    {
        base.Awake();
        jumpsRemaining = maxJumps;
    }

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
        if(context.started && !isDead && !isStunned)
        {
            if(isGrounded || jumpsRemaining > 0)
            {
                Jump();
            }
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

        jumpsRemaining--;
    }

    private void CheckEnvironment()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset jumps when the player lands
        if(isGrounded && !wasGrounded)
        {
            jumpsRemaining = maxJumps;
        }

        // Safety: if the player walks off a lege without jumping.
        // they should only have 1 jump left
        if(!isGrounded && wasGrounded && jumpsRemaining == maxJumps)
        {
            jumpsRemaining--;
        }
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

        // Wait for the play to fall out of view
        yield return new WaitForSeconds(1f);

        if(gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }

        // Destroy(gameObject);
        gameObject.SetActive(false);    // Instead of destroying and instantiating the player, we will deactivate, move and reactivate.
    }

    private void ApplyKnockback()
    {
        // Determine direction: push away from where the player is currently facing

        float pushDirection = transform.localScale.x > 0 ? -1f : 1f;

        // Reset velocity first so the knockback is consistent
        rBody.linearVelocity = Vector2.zero;
        rBody.AddForce(new Vector2(pushDirection * knockbackForce, knockbackForce), ForceMode2D.Impulse);
    }

    public void ResetState()
    {
        isDead = false;
        isStunned = false;
        currentHealth = 3;
        anim.SetBool("IsDead", false);
        sRend.enabled = true;
    }
}

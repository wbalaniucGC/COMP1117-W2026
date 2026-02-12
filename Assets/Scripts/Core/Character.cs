using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    // Private variables
    [Header("Base Stats")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int maxHealth = 3;

    [Header("Visuals")]
    [SerializeField] protected bool facesLeftByDefault = false;

    // Backing fields so Player and Enemies can check/set it
    protected bool isDead = false;
    protected int currentHealth;

    // Component references used by call characters.
    protected Animator anim;
    protected Rigidbody2D rBody;
    protected SpriteRenderer sRend;


    // Public Properties (Read-only)
    public float MoveSpeed => moveSpeed;
    public bool IsDead => isDead;
    

    protected virtual void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Find reference even if it's on a child object
        sRend = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;

        if (currentHealth < 0) Die();
    }

    protected void FlipSprite(float horizontalVelocity)
    {
        if (Mathf.Abs(horizontalVelocity) > 0.1f)
        {
            float direction = horizontalVelocity > 0 ? 1f : -1f;
            Vector3 newScale = transform.localScale;

            if (facesLeftByDefault)
            {
                // Invert the logic: Positive movement = Negative scale
                newScale.x = direction * -1f;
            }
            else
            {
                // Standard logic: Positive movement = Positive scale
                newScale.x = direction;
            }

            transform.localScale = newScale;
        }
    }

    // Abstract functions
    public abstract void Move();
    public abstract void Die();
}

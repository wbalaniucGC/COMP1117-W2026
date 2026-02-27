using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Character : MonoBehaviour, IDamageable
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
    

    protected virtual void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();

        // Logic stays on root, but these components are now children
        anim = GetComponentInChildren<Animator>();
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
            Vector3 newScale = anim.transform.localScale;

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

            anim.transform.localScale = newScale;
        }
    }

    // Abstract functions
    public abstract void Move();
    public abstract void Die();
}

using UnityEngine;

public abstract class FlyingEnemy : Enemy
{
    [Header("Flight Settings")]
    [SerializeField] protected Transform pointA;
    [SerializeField] protected Transform pointB;
    [SerializeField] protected float arrivalThreshold = 0.5f;

    protected Transform currentTarget;

    protected override void Awake()
    {
        base.Awake();

        // Ensure the enemy doesn't fall
        rBody.gravityScale = 0;

        // Start by heading forwards point B.
        currentTarget = pointB;
    }

    protected virtual void Update()
    {
        if(isDead) return;
        Move();
    }

    public override void Move()
    {
        // 1. Calculate direction to target
        Vector2 direction = (currentTarget.position - transform.position).normalized;

        // 2. Apply velocity
        rBody.linearVelocity = direction * MoveSpeed;

        // 3. Check if we arrived at the target
        if (Vector2.Distance(transform.position, currentTarget.position) < arrivalThreshold)
        {
            SwitchTarget();
        }

        // 4. Flip visuals based on X movement
        FlipSprite(rBody.linearVelocity.x);
    }

    protected virtual void SwitchTarget()
    {
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }
}

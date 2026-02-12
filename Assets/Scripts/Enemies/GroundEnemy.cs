using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;

    private bool isMovingLeft = true;

    protected override void Awake()
    {
        base.Awake();

        // Safety check: If boundaries are not assigned, the enemy stays still
        if(leftBoundary == null || rightBoundary == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing patrol points");
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        Move();
    }

    public override void Move()
    {
        // Figure out direction
        float direction = isMovingLeft ? -1f : 1f;
        rBody.linearVelocity = new Vector2(direction * MoveSpeed, rBody.linearVelocity.y);

        // Check boundaries and flip direction
        CheckBoundaries();

        // Use the FlipSprite method from Character.cs
        FlipSprite(rBody.linearVelocity.x);
    }

    private void CheckBoundaries()
    {
        // If moving left and reached the left marker
        if (isMovingLeft && transform.position.x <= leftBoundary.position.x)
        {
            isMovingLeft = false;
        }
        // If moving right and reached the right marker
        else if (!isMovingLeft && transform.position.x >= rightBoundary.position.x)
        {
            isMovingLeft = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (leftBoundary != null && rightBoundary != null)
        {
            Gizmos.color = Color.red;
            // Draw a line between the two points
            Gizmos.DrawLine(leftBoundary.position, rightBoundary.position);
            // Draw small spheres at the points
            Gizmos.DrawWireSphere(leftBoundary.position, 0.3f);
            Gizmos.DrawWireSphere(rightBoundary.position, 0.3f);
        }
    }
}

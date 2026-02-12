using UnityEngine;

public abstract class Enemy : Character
{
    [Header("Enemy Interaction")]
    [SerializeField] protected int contactDamage = 1;
    [SerializeField] protected float stompBounceForce = 12f;

    // Shared behaviour: ALL enemies need to detect the player hitting them.
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Try to get the Player script
        if(collision.gameObject.TryGetComponent(out Player player))
        {
            // 2. Check the collision normal (direction of the hit)
            // A normal.y of -1 means the hitcame from the very top.
            Vector2 contactNormal = collision.contacts[0].normal;

            if(contactNormal.y <= -0.5f)
            {
                // Successful "Bonk"
                OnStomped(player);
            }
            else
            {
                // Hit from the side or bottom
                player.TakeDamage(contactDamage);
            }
        }
    }

    protected virtual void OnStomped(Player player)
    {
        // Apply a "bounce" directly to the players rigidbody.
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);
        }

        Die();
    }

    public override void Die()
    {
        if(isDead) return;

        isDead = true;

        // 1. Play the "enemy-death" (explosion) animation
        anim.SetTrigger("Death");

        // 2. Disable the collider so the player doesn't hit the "corpse"
        rBody.simulated = false;
        rBody.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 3. Destroy the object
        Destroy(gameObject, 0.5f);
    }
}

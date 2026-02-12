using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("Loot settings")]
    [SerializeField] private GameObject gemPrefab;      // "____Prefab" is convention
    [SerializeField] private int gemCount = 3;          // How many gem get spawned from the chest
    [SerializeField] private float launchForce = 5f;    // Force behind launching gems

    [Header("Visuals")]
    [SerializeField] private Sprite openChestSprite;    // Sprite for an open chest

    private SpriteRenderer sRend;
    private bool isOpened = false;

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();         // Caching your reference
    }

    public void Interact()
    {
        // Safety check
        if(isOpened)
        {
            // If my chest is already opened, do nothing and leave.
            return;
        }

        // Chest is not opened
        isOpened = true;
        OpenChest();
    }

    private void OpenChest()
    {
        // 1. Change visual state to open
        // Safety check
        if (sRend != null && openChestSprite != null)
        {
            sRend.sprite = openChestSprite;
        }

        // 2. Spew gems
        for(int i = 0; i < gemCount; i++)
        {
            GameObject gem = Instantiate(gemPrefab, transform.position, Quaternion.identity);
            Rigidbody2D gemRB = gem.GetComponent<Rigidbody2D>();

            // Safety check
            if(gemRB != null)
            {
                // Launch it up in the air
                // Create a "fountain" effect
                Vector2 force = new Vector2(Random.Range(-1f, 1f), 1.5f).normalized * launchForce;
                gemRB.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}

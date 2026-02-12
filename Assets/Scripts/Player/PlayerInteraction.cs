using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 1.5f;        // How close I must be to the interactable objects
    [SerializeField] private LayerMask interactableLayer;       // Ensure that I'm interacting with interactable objects

    // Function that will be called when the "Interact" action is triggered
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Fire once when pressed
        if(context.started)
        {
            // Perform our interaction.
            PerformInteraction();
        }
    }

    private void PerformInteraction()
    {
        // Find everything in a circle around the fox on the interactable layer
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);

        // Check if I hit something
        if(hit != null)
        {
            // Hit something in the interactable layer
            // Safety first
            if(hit.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                // The object on the interactable layer DOES HAVE a script that implements IInteractable
                interactable.Interact();
                Debug.Log($"Interacted with {hit.name}");
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

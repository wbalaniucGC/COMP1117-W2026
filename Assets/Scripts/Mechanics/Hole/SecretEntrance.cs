using UnityEngine;
using UnityEngine.InputSystem;

public class SecretEntrance : MonoBehaviour
{
    [SerializeField] private GameObject undergroundPath;

    private bool isPlayerInRange = false;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.started && isPlayerInRange)
        {
            ToggleSecretPath();
        }
    }

    private void ToggleSecretPath()
    {
        if (undergroundPath == null) return;

        bool isNowActive = !undergroundPath.activeSelf;
        undergroundPath.SetActive(isNowActive);

        Debug.Log("Secret path active: " + isNowActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}

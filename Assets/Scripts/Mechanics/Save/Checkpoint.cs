using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;   // Accesses the Save Manager

    private SpriteRenderer sRend;

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check to see if the player entered into the checkpoint trigger
        if(other.CompareTag("Player"))
        {
            // Player triggered the checkpoint
            sRend.color = Color.green;

            // Save my progress!
            saveManager.SaveGame(other.transform.position);

            Debug.Log("Checkpoint Reached!");
        }
    }
}

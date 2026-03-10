using Unity.Cinemachine;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SaveManager saveManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            saveManager.SaveGame(collision.transform.position);

            Debug.Log("Checkpoint Reached!");
        }
    }
}

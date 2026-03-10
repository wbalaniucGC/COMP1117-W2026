using Unity.Cinemachine;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SaveManager saveManager;

    private SpriteRenderer sRend;

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sRend.color = Color.green;

            saveManager.SaveGame(collision.transform.position);

            Debug.Log("Checkpoint Reached!");
        }
    }
}

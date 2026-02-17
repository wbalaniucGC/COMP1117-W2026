using UnityEngine;

public class Cherry : MonoBehaviour
{
    public float healAmount = 5.0f;

    public void ApplyFloatAnimation()
    {
        Debug.Log("<color=red>CHERRY FLOATING!</color> Ready to heal the player.");

        // Simple movement logic to make it look different from the Gem
        transform.position += Vector3.up * Mathf.Sin(Time.time);
    }
}

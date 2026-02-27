using UnityEngine;

public class MarkerDetacher : MonoBehaviour
{
    private void Awake()
    {
        // Removes marker from hierarchy when the game starts.
        transform.SetParent(null);
    }
}

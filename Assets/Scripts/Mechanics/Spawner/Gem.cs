using UnityEngine;

public class Gem : MonoBehaviour
{
    public int gemValue = 10;

    public void PlayShineEffect()
    {
        // Simple visual feedback for the demo
        Debug.Log($"<color=cyan>GEM SPARKLE!</color> This gem is worth {gemValue} points.");

        // Example: Change color slightly to show access
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }
}

using UnityEngine;

public class WaterZone : Zone
{
    [SerializeField] private float speedModifier = 0.5f;

    // Reduce the players speed by half
    protected override void ApplyZoneEffect(Player player)
    {
        // Change my player's speed modifier value
        player.ApplySpeedModifier(speedModifier);
    }
}

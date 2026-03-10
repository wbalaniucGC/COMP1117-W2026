using UnityEngine;

public class KillZone : Zone
{
    protected override void ApplyZoneEffect(Player player)
    {
        player.Die();
        Debug.Log("Player entered KillZone");
    }
}

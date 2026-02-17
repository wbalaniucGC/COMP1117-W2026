using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    public EntitySpawner spawner;

    public Gem gemPrefab;
    public Cherry cherryPrefab;

    public void OnSpawnGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // The spawner returns a 'Gem' type, allowing immediate access to Gem methods
            Gem newGem = spawner.Spawn<Gem>(gemPrefab, GetRandomPosition());
            newGem.PlayShineEffect();
        }
    }

    public void OnSpawnCherry(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            // The spawner returns a 'Cherry' type
            Cherry newCherry = spawner.Spawn<Cherry>(cherryPrefab, GetRandomPosition());
            newCherry.ApplyFloatAnimation();
        }
    }  

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5, 5), Random.Range(-2, 2), 0);
    }
}

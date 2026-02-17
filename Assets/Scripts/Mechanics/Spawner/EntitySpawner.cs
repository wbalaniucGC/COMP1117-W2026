using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public T Spawn<T>(T prefab, Vector3 position) where T : Component
    {
        T newEntity = Instantiate(prefab, position, Quaternion.identity);

        Debug.Log($"Successfully spawn a generic {typeof(T).Name}");

        return newEntity;
    }
}

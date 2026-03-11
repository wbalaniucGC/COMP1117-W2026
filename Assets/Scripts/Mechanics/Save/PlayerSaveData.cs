using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    // When using JsonUtility, we must use SIMPLE data types
    // public Vector3 position;    // (float, float, float)
    public float[] position = new float[3];
}

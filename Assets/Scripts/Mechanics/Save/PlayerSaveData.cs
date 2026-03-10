using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    // When using JsonUtility, we must use simple data types
    public float[] position = new float[3];
}

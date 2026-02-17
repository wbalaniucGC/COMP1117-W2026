using UnityEngine;
using UnityEngine.InputSystem;

public class TimeRewinder : MonoBehaviour
{
    [Header("Settings")]
    public int maxFrames = 300;
    public bool isRewinding = false;

    private CircularBuffer<Vector3> positionHistory;
    private CircularBuffer<Quaternion> rotationHistory;

    private void Awake()
    {
        positionHistory = new CircularBuffer<Vector3>(maxFrames);
        rotationHistory = new CircularBuffer<Quaternion>(maxFrames);
    }

    public void OnRewindInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRewinding = true;
            Debug.Log("Rewind Performed (Button Pressed)");
        }
        else if (context.canceled)
        {
            isRewinding = false;
            Debug.Log("Rewind Canceled (Button Released)");
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    void Record()
    {
        positionHistory.Push(transform.position);
        rotationHistory.Push(transform.rotation);
    }

    void Rewind()
    {
        if (positionHistory.Count > 0)
        {
            transform.position = positionHistory.Pop();
            transform.rotation = rotationHistory.Pop();
        }
        else
        {
            isRewinding = false; // Stop if we run out of "tape"
        }
    }
}

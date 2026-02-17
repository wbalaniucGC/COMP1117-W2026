using System.Collections.Generic;

public class CircularBuffer<T>
{
    private List<T> buffer;
    private int capacity;

    public CircularBuffer(int capacity)
    {
        buffer = new List<T>(capacity);
        this.capacity = capacity;
    }

    public void Push(T item)
    {
        if(buffer.Count >= capacity)
        {
            buffer.RemoveAt(0); // Removes the oldest data.
        }
        buffer.Add(item);
    }

    public T Pop()
    {
        if(buffer.Count == 0) return default(T);

        int lastIndex = buffer.Count - 1;
        T item = buffer[lastIndex];
        buffer.RemoveAt(lastIndex);

        return item;
    }

    public int Count => buffer.Count;
}

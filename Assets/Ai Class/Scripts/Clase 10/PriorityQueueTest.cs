using UnityEngine;

namespace Clase10
{
    public class PriorityQueueTest : MonoBehaviour
    {
        private void OnEnable()
        {
            PriorityQueue<string> priority = new();
            priority.Enqueue("A", 50);
            priority.Enqueue("B", 6);
            priority.Enqueue("C", 8);
            priority.Enqueue("D", 0);
            priority.Enqueue("E", 9);

            while (!priority.IsEmpty)
                Debug.Log(priority.Dequeue()); // D, B, C, E, A
        }
    }
}

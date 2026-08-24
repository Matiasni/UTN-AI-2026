using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MovementVectorExample : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Header("Visualización")]
    [SerializeField] private float arrowHeadSize = 0.25f;

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Vector3 agentPosition = transform.position;
        Vector3 targetPosition = target.position;
        Vector3 movementVector = targetPosition - agentPosition;

        Vector3 start = transform.position + Vector3.right;
        Vector3 invertedAgent = -agentPosition;
        Vector3 result = invertedAgent + targetPosition;

        DrawArrow(Vector3.zero, agentPosition, Color.blue);
        DrawArrow(Vector3.zero, targetPosition, Color.yellow);
        DrawArrow(agentPosition, movementVector, Color.green);

#if UNITY_EDITOR
        Handles.color = Color.magenta;
        Handles.Label(start + invertedAgent * 0.5f, $"Agent {agentPosition}");
        Handles.color = Color.yellow;
        Handles.Label(start + invertedAgent + targetPosition * 0.5f, $"Target {targetPosition}");
        Handles.color = Color.green;
        Handles.Label(start + result * 0.5f, $"Movement {movementVector}");
#endif

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(agentPosition, 0.12f);
        Gizmos.DrawSphere(targetPosition, 0.12f);
    }

    private void DrawArrow(Vector3 start, Vector3 direction, Color color)
    {
        if (direction == Vector3.zero) return;

        Gizmos.color = color;
        Vector3 end = start + direction;
        Gizmos.DrawLine(start, end);

        Vector3 normalizedDirection = direction.normalized;
        Vector3 right = Quaternion.LookRotation(normalizedDirection) * Quaternion.Euler(0f, 150f, 0f) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(normalizedDirection) * Quaternion.Euler(0f, -150f, 0f) * Vector3.forward;

        Gizmos.DrawLine(end, end + right * arrowHeadSize);
        Gizmos.DrawLine(end, end + left * arrowHeadSize);
    }
}
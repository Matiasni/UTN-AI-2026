using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EscapeVectorExample : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Visualización")]
    [SerializeField] private bool showCalculation = false;
    [SerializeField] private bool correctOffset = true;
    [SerializeField] private float arrowHeadSize = 0.25f;

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Vector3 agentPosition = transform.position;
        Vector3 targetPosition = target.position;
        Vector3 escapeVector = agentPosition - targetPosition;

        escapeVector.Normalize();

        Vector3 invertedTarget = -targetPosition;

        if (!showCalculation)
        {
            DrawArrow(Vector3.zero, agentPosition, Color.blue);
            DrawArrow(Vector3.zero, targetPosition, Color.yellow);
            DrawArrow(agentPosition, escapeVector, Color.red);

#if UNITY_EDITOR
            Handles.color = Color.blue;
            Handles.Label(agentPosition * 0.5f, $"Agent\n{agentPosition}");

            Handles.color = Color.yellow;
            Handles.Label(targetPosition * 0.5f, $"Target\n{targetPosition}");

            Handles.color = Color.red;
            Handles.Label(agentPosition + escapeVector * 0.5f, $"Escape\n{escapeVector}");
#endif
        }
        else if (correctOffset)
        {
            Vector3 start = transform.position;
            Vector3 firstEnd = start + agentPosition;
            Vector3 resultEnd = firstEnd + invertedTarget;

            DrawArrow(start, agentPosition, Color.blue);
            DrawArrow(firstEnd, invertedTarget, Color.magenta);
            DrawArrow(start, escapeVector, Color.red);

            Gizmos.color = new Color(1f, 1f, 1f, 0.35f);
            Gizmos.DrawLine(firstEnd, resultEnd);
            Gizmos.DrawLine(start + invertedTarget, resultEnd);

#if UNITY_EDITOR
            Handles.color = Color.white;
            Handles.Label(start + Vector3.up * 1.5f, "Agent - Target = Agent + (-Target)");

            Handles.color = Color.blue;
            Handles.Label(start + agentPosition * 0.5f, $"Agent\n{agentPosition}");

            Handles.color = Color.magenta;
            Handles.Label(firstEnd + invertedTarget * 0.5f, $"-Target\n{-targetPosition}");

            Handles.color = Color.red;
            Handles.Label(start + escapeVector * 0.5f, $"Escape\n{escapeVector}");

            Handles.color = Color.yellow;
            Handles.Label(targetPosition * 0.5f, $"Target\n{targetPosition}");

            Handles.color = Color.white;
            Handles.Label(agentPosition * 0.5f, $"Agent Position\n{agentPosition}");
#endif
        }
        else
        {
            Vector3 calculationStart = Vector3.zero;
            Vector3 secondVectorStart = calculationStart + agentPosition;
            Vector3 resultEnd = secondVectorStart + invertedTarget;

            DrawArrow(calculationStart, agentPosition, Color.blue);
            DrawArrow(secondVectorStart, invertedTarget, Color.magenta);
            DrawArrow(calculationStart, escapeVector, Color.red);

            Gizmos.color = new Color(1f, 1f, 1f, 0.35f);
            Gizmos.DrawLine(calculationStart + agentPosition, resultEnd);
            Gizmos.DrawLine(calculationStart + invertedTarget, resultEnd);

            DrawPoints(agentPosition, targetPosition);

#if UNITY_EDITOR
            Handles.color = Color.blue;
            Handles.Label(calculationStart + agentPosition * 0.5f, $"Agent\n{agentPosition}");

            Handles.color = Color.magenta;
            Handles.Label(secondVectorStart + invertedTarget * 0.5f, $"-Target\n{-targetPosition}");

            Handles.color = Color.red;
            Handles.Label(calculationStart + escapeVector * 0.5f, $"Escape\n{escapeVector}");

            Handles.color = Color.white;
            Handles.Label(calculationStart + Vector3.up * 1.5f, "Agent - Target = Agent + (-Target)");
#endif
        }

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(agentPosition, 0.12f);
        Gizmos.DrawSphere(targetPosition, 0.12f);
    }

    private void DrawPoints(Vector3 agentPosition, Vector3 targetPosition)
    {
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
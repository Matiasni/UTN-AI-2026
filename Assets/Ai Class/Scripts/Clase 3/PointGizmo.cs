using UnityEngine;

public class PointGizmo : MonoBehaviour
{
    [SerializeField] private float radius = 0.3f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
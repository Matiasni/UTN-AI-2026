using UnityEngine;

public abstract class FieldOfViewSensor : MonoBehaviour
{
    [Header("Field Of View")]
    [SerializeField] protected float _viewRadius = 8f;
    [SerializeField] protected float _eyeHeight = 1f;
    [SerializeField, Range(0f, 360f)] protected float _viewAngle = 90f;

    [Header("Filtros por Layer")]
    [SerializeField] protected LayerMask _targetMask = ~0;
    [SerializeField] protected LayerMask _obstacleMask = ~0;

    protected Transform _target;

    public bool CanSeeTarget { get; protected set; }

    public Transform Target => _target;
    public float ViewRadius => _viewRadius;
    public float ViewAngle => _viewAngle;
    public float EyeHeight => _eyeHeight;

    protected static bool IsAValidTarget(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;

    protected bool InViewAngle()
    {
        Vector3 dirToTarget = (_target.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, dirToTarget) <= _viewAngle * 0.5f;
    }

    protected bool HasLineOfSight()
    {
        Vector3 eyePos = transform.position + Vector3.up * _eyeHeight;
        Vector3 targetPos = _target.position + Vector3.up * _eyeHeight;
        Vector3 direction = targetPos - eyePos;

        if (Physics.Raycast(eyePos, direction.normalized, out RaycastHit hit, direction.magnitude, _obstacleMask))
            return hit.transform == _target || hit.transform.IsChildOf(_target);

        return true;
    }
}

using UnityEngine;

public class FieldOfViewOverlap : FieldOfViewSensor
{
    [Header("Overlap")]
    [SerializeField] private float _checkInterval = 0.2f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _checkInterval)
        {
            _timer = 0f;
            CanSeeTarget = CheckVisibility();
        }
    }

    private bool CheckVisibility()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        _target = null;
        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(transform)) continue;

            _target = hit.transform.root;
            break;
        }

        if (_target == null) return false;

        return InViewAngle() && HasLineOfSight();
    }
}

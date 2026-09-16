using System.Collections;
using UnityEngine;

public class FieldOfViewTrigger : FieldOfViewSensor
{
    private bool _targetInRange;
    public float followTime;
    Coroutine _coroutine;

    private void Awake()
    {
        SphereCollider sensor = gameObject.AddComponent<SphereCollider>();
        sensor.isTrigger = true;
        sensor.radius = _viewRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAValidTarget(other.gameObject.layer, _targetMask)) return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _target = other.transform.root;
        _targetInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsAValidTarget(other.gameObject.layer, _targetMask)) return;

        _coroutine = StartCoroutine(KeepLooking());
    }

    private void Update()
    {
        CanSeeTarget = _targetInRange && _target != null && InViewAngle() && HasLineOfSight();
    }

    IEnumerator KeepLooking()
    {
        yield return new WaitForSeconds(followTime);
        _targetInRange = false;
    }
}

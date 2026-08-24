using UnityEngine;

public class BasicAgentSteering : Agent
{
    [Header("Stats")]
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _maxSteering = 3f;
    [SerializeField] private float _slowingDistance = 3f;
    [SerializeField] private float _minDistance = 0.1f;

    [Header("References")]
    [SerializeField] private Transform _target;

    private void Update()
    {
        Seek();

        transform.position += _velocity * Time.deltaTime;

        if (_velocity != Vector3.zero)
            transform.forward = _velocity;
    }

    private void Seek()
    {
        Vector3 desired = (_target.position - transform.position).normalized;
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxSteering * Time.deltaTime);

        _velocity += steering;
    }

    private void Flee()
    {
        Vector3 desired = (transform.position - _target.position).normalized;
        //Vector3 desired = (_target.position - transform.position).normalized * -1;
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxSteering * Time.deltaTime);

        _velocity += steering;
    }

    private void Arrive()
    {
        Vector3 direction = _target.position - transform.position;


        float distance = direction.magnitude;

        if (distance < _minDistance)
        {
            _velocity = Vector3.zero;
            return;
        }

        float targetSpeed = _maxSpeed * (distance / _slowingDistance);

        //targetSpeed = Mathf.Clamp(targetSpeed, 2, _maxSpeed);
        float desiredSpeed = Mathf.Min(targetSpeed, _maxSpeed);

        Vector3 desired = direction.normalized * desiredSpeed;
        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(
            steering,
            _maxSteering * Time.deltaTime
        );

        _velocity += steering;
    }
}
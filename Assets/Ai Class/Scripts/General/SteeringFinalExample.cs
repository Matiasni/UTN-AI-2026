using UnityEngine;

public class SteeringFinalExample : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _maxSteering = 3f;
    [SerializeField] private float _slowingDistance = 3f;
    [SerializeField] private float _minDistance = 0.1f;

    [Header("References")]
    [SerializeField] private Agent _target;

    private Vector3 _velocity;
    public enum SteeringModes { Seek, Flee, Arrive, Pursuit, Evade }
    public SteeringModes CurrentSteering;

    private void Update()
    {
        _velocity += SteeringVector();

        transform.position += _velocity * Time.deltaTime;

        if (_velocity != Vector3.zero)
            transform.forward = _velocity;
    }

    private Vector3 SteeringVector()
    {
        switch (CurrentSteering)
        {
            case SteeringModes.Seek:
                return CalculateSeek(_target.transform.position);
            case SteeringModes.Flee:
                return CalculateFlee(_target.transform.position);
            case SteeringModes.Arrive:
                return CalculateArrive(_target.transform.position);
            case SteeringModes.Pursuit:
                return Pursuit(_target);
            case SteeringModes.Evade:
                return Evade(_target);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 CalculateDesired(Vector3 target)
    {
        Vector3 desired = (target - transform.position).normalized;
        desired *= _maxSpeed;

        return desired;
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxSteering );

        return steering * Time.deltaTime;
    }

    private Vector3 CalculateSeek(Vector3 target)
    {
        var desired = CalculateDesired(target);

        return CalculateSteering(desired);
    }

    private Vector3 CalculateFlee(Vector3 target)
    {
        var desired = CalculateDesired(target);

        return CalculateSteering(-desired);
    }

    private Vector3 CalculateArrive(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        float distance = direction.magnitude;

        if (distance < _minDistance)
            return Vector3.zero;

        float targetSpeed = _maxSpeed * (distance / _slowingDistance);
        float desiredSpeed = Mathf.Min(targetSpeed, _maxSpeed);

        Vector3 desired = direction.normalized * desiredSpeed;

        return CalculateSteering(desired);
    }

    private Vector3 CalculateFuture(Agent target)
    {
        Vector3 direction = target.transform.position - transform.position;

        float distance = direction.magnitude;

        float prediction = distance / (_maxSpeed + target.Velocity.magnitude);

        return target.transform.position + target.Velocity * prediction;
    }

    private Vector3 Pursuit(Agent target)
    {
        Vector3 futurePosition = CalculateFuture(target);

        return CalculateSeek(futurePosition);
    }

    private Vector3 Evade(Agent target)
    {
        Vector3 futurePosition = CalculateFuture(target);

        return CalculateFlee(futurePosition);
    }
}
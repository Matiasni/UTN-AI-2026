using UnityEngine;

public class AvoidanceAgent : Agent
{
    [Header("Stats")]
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _maxSteering = 4f;
    [SerializeField] private float _slowingDistance = 3f;
    [SerializeField] private float _minDistance = 0.2f;

    [Header("Obstacle Avoidance")]
    [SerializeField] private float _avoidDistance = 3f;
    [SerializeField] private float _avoidAngle = 35f;
    [SerializeField] private float _avoidWeight = 6f;
    [SerializeField] private float _eyeHeight = 0.6f;
    [SerializeField] private LayerMask _obstacleMask = ~0;

    [Header("References")]
    [SerializeField] private FieldOfViewSensor _fieldOfView;

    public enum SteeringModes { Seek, Arrive, Pursuit }
    public SteeringModes currentSteering = SteeringModes.Pursuit;

    private void Update()
    {
        _velocity += SteeringVector() + AvoidObstacles();
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        transform.position += _velocity * Time.deltaTime;
        transform.position = Bounds.Instance.OutOfBounds(transform.position);

        if (_velocity != Vector3.zero)
            transform.forward = _velocity;
    }

    private Vector3 SteeringVector()
    {
        if (_fieldOfView == null || !_fieldOfView.CanSeeTarget)
            return Vector3.zero;

        Transform target = _fieldOfView.Target;
        if (target == null)
            return Vector3.zero;

        switch (currentSteering)
        {
            case SteeringModes.Seek:
                return Seek(target.position);
            case SteeringModes.Arrive:
                return Arrive(target.position);
            case SteeringModes.Pursuit:
                return Pursuit(target);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;
        return Vector3.ClampMagnitude(steering, _maxSteering * Time.deltaTime);
    }

    private Vector3 DesiredVector(Vector3 target) => (target - transform.position).normalized * _maxSpeed;

    private Vector3 Seek(Vector3 target) => CalculateSteering(DesiredVector(target));

    private Vector3 Arrive(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;

        if (distance < _minDistance)
            return Vector3.zero;

        float desiredSpeed = Mathf.Min(_maxSpeed * (distance / _slowingDistance), _maxSpeed);
        Vector3 desired = direction.normalized * desiredSpeed;

        return CalculateSteering(desired);
    }

    private Vector3 Pursuit(Transform target)
    {
        // Pursuit necesita la velocidad del target para predecir donde va a estar;
        // el resto de los modos solo usan su posicion, por eso este es el unico
        // lugar donde nos importa que ademas tenga un Agent.
        Agent targetAgent = target.GetComponent<Agent>();
        Vector3 targetVelocity = targetAgent != null ? targetAgent.Velocity : Vector3.zero;

        Vector3 direction = target.position - transform.position;
        float prediction = direction.magnitude / (_maxSpeed + targetVelocity.magnitude);
        Vector3 futurePosition = target.position + targetVelocity * prediction;

        return Seek(futurePosition);
    }

    private Vector3 AvoidObstacles()
    {
        Vector3 origin = transform.position + Vector3.up * _eyeHeight;

        Vector3 avoidance = Whisker(origin, transform.forward, 1f);
        avoidance += Whisker(origin, Quaternion.Euler(0, _avoidAngle, 0) * transform.forward, 0.5f);
        avoidance += Whisker(origin, Quaternion.Euler(0, -_avoidAngle, 0) * transform.forward, 0.5f);

        return Vector3.ClampMagnitude(avoidance, _avoidWeight * Time.deltaTime);
    }

    private Vector3 Whisker(Vector3 origin, Vector3 direction, float weight)
    {
        if (!Physics.Raycast(origin, direction, out RaycastHit hit, _avoidDistance, _obstacleMask))
            return Vector3.zero;

        if (hit.transform.IsChildOf(transform))
            return Vector3.zero;

        float proximity = 1f - (hit.distance / _avoidDistance);

        Vector3 away = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;
        Vector3 lateral = Vector3.ProjectOnPlane(away, transform.forward);

        if (lateral.sqrMagnitude < 0.0001f)
            lateral = transform.right;

        return lateral.normalized * proximity * weight;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * _eyeHeight;

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(origin, transform.forward * _avoidDistance);
        Gizmos.DrawRay(origin, (Quaternion.Euler(0, _avoidAngle, 0) * transform.forward) * _avoidDistance);
        Gizmos.DrawRay(origin, (Quaternion.Euler(0, -_avoidAngle, 0) * transform.forward) * _avoidDistance);
    }
}

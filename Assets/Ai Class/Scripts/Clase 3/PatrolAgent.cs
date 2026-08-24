using UnityEngine;

public class PatrolAgent : Agent
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float maxSpeed = 2f;

    private Transform target;

    private void Start()
    {
        target = pointB;
    }

    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        _velocity = direction * maxSpeed;

        transform.position += Velocity * Time.deltaTime;

        if (Velocity != Vector3.zero)
            transform.forward = Velocity.normalized;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }
}
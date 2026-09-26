using System;
using UnityEngine;

public class AgentWanderer : MonoBehaviour, IDestinationNotifier
{
    public event Action OnDestinationReached = delegate { };

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _areaRadius = 8f;
    [SerializeField] private float _arrivalDistance = 0.2f;

    private Vector3 _areaCenter;
    private Vector3 _destination;

    private void Start()
    {
        _areaCenter = transform.position;
        PickNewDestination();
    }

    private void Update()
    {
        Wander();
    }

    private void Wander()
    {
        Vector3 direction = _destination - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= _arrivalDistance)
        {
            OnDestinationReached?.Invoke();
            PickNewDestination();
            return;
        }

        transform.position += direction.normalized * _speed * Time.deltaTime;
        transform.forward = direction.normalized;
    }

    private void PickNewDestination()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _areaRadius;
        _destination = _areaCenter + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }
}
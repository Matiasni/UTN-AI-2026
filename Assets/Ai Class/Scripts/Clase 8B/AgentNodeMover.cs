using System;
using UnityEngine;

public class AgentNodeMover : MonoBehaviour, IDestinationNotifier
{
    public event Action OnDestinationReached = delegate { };

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _arrivalDistance = 0.2f;

    private Vector3 _destination;
    private bool _hasDestination;

    private void OnEnable()
    {
        MovementNode.OnNodeSelected += SetDestination;
    }

    private void OnDisable()
    {
        MovementNode.OnNodeSelected -= SetDestination;
    }

    private void Update()
    {
        if (!_hasDestination) return;

        Vector3 direction = _destination - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= _arrivalDistance)
        {
            _hasDestination = false;
            OnDestinationReached?.Invoke();
            return;
        }

        transform.position += direction.normalized * _speed * Time.deltaTime;
        transform.forward = direction.normalized;
    }

    private void SetDestination(MovementNode node)
    {
        _destination = node.transform.position;
        _hasDestination = true;
    }
}

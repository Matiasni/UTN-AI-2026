using System.Collections.Generic;
using UnityEngine;

public enum PoliceStates
{
    Idle,
    Patrol
}

public class FSMAgent : MonoBehaviour
{
    public float speed = 3f;
    [SerializeField] private PatrolData dataPatrol;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        IdleState idleState = new IdleState(_stateMachine);
        PatrolState patrolState = new PatrolState(this, dataPatrol, _stateMachine);

        _stateMachine.RegisterState(PoliceStates.Idle, idleState);
        _stateMachine.RegisterState(PoliceStates.Patrol, patrolState);

        _stateMachine.ChangeState(PoliceStates.Idle);
    }

    private void Update()
    {
        _stateMachine.Update();
    }
}
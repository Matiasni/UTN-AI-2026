using UnityEngine;

public class IdleState : State
{
    float _timeToChangePatrol = 3f;
    float _timer = 0f;

    public IdleState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _timer = 0;
        Debug.Log("Entre idle");
    }

    public override void Exit()
    {        
        Debug.Log("Sali Idle");
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        if (_timeToChangePatrol <= _timer)
        {
            StateMachine.ChangeState(PoliceStates.Patrol);
            Debug.Log("Me muevo");
        }

        Debug.Log("Estoy en idle");
    }
}
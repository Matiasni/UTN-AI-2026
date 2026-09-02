using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    private FSMAgent _agent;
    private PatrolData _data;
    private int currentNode;
    private int direction = 1;

    public PatrolState(FSMAgent agent, PatrolData data, StateMachine stateMachine) :base(stateMachine)
    {
        _agent = agent;
        _data = data;
    }

    public override void Enter()
    {
        Debug.Log("Entre Patrol");
    }

    public override void Exit()
    {
        Debug.Log("Sali Patrol");
    }

    public override void Update()
    {

        PatrolLoop();
        Debug.Log("Estoy en Patrol");
    }

    private void PatrolLoop()
    {
        var nextWaypoint = _data.wayPoints[currentNode];

        if (Vector3.Distance(nextWaypoint.position, _data.transform.position) <= _data.waypointCheckDistance)
        {
            currentNode = currentNode + 1 < _data.wayPoints.Count ? currentNode + 1 : 0;
        }

        var dir = nextWaypoint.position - _data.transform.position;
        _data.transform.position += dir.normalized * _agent.speed * Time.deltaTime;
    }

    private void PatrolPingPoing()
    {
        var nextWaypoint = _data.wayPoints[currentNode];

        if (Vector3.Distance(nextWaypoint.position, _data.transform.position) <= _data.waypointCheckDistance)
        {
            currentNode += direction;

            if (currentNode >= _data.wayPoints.Count)
            {
                currentNode = _data.wayPoints.Count - 1;
                direction = -1;
            }
            else if (currentNode < 0)
            {
                currentNode = 1;
                direction = 1;
            }
        }

        var dir = nextWaypoint.position - _data.transform.position;
        _data.transform.position += dir.normalized * _agent.speed * Time.deltaTime;
    }
}

[System.Serializable]
public class PatrolData
{
    public List<Transform> wayPoints;
    public Transform transform;
    public float waypointCheckDistance;
}
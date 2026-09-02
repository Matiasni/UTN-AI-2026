using System.Collections.Generic;
using System;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; }
    private Dictionary<Enum, State> states = new Dictionary<Enum, State>();

    public void RegisterState(Enum key, State state)
    {
        states[key] = state;
    }

    public void ChangeState(Enum key)
    {
        State newState = states[key];

        if (newState == CurrentState)
            return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
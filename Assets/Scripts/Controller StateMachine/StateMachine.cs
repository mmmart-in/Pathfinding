using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    private Dictionary<Type, State> instantiatedStates = new Dictionary<Type, State>();
    public State currentState { get; private set; }

    public StateMachine(object owner, State[] states)
    {
        Debug.Assert(states.Length > 0);
        foreach(State s in states)
        {
            Debug.Log("foreach state..");
            State instantiated = UnityEngine.Object.Instantiate(s);
            instantiated.Initialize(this, owner);
            instantiatedStates.Add(s.GetType(), instantiated);

            if (!currentState)
                currentState = instantiated;
        }
    }
    public void RunUpdate()
    {
        currentState.RunUpdate();
    }
    public void ChangeState<T>() where T : State
    {
        if (instantiatedStates.ContainsKey(typeof(T)))
        {
            State instance = instantiatedStates[typeof(T)];
            currentState?.Exit();
            currentState = instance;
            currentState.Enter();
        }
        else
            Debug.Log(typeof(T) + "not found");
    }
}

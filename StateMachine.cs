using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public T m_Owner;
    public State<T> m_CurrentState { get; private set; }

    public void InitStartState(State<T> startState)
    {
        m_CurrentState = startState;
        m_CurrentState.Enter(m_Owner);
    }

    public void Update()
    {
        m_CurrentState.Execute(m_Owner);
    }
}

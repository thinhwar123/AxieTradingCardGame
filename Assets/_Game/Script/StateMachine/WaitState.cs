using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State<MatchManager>
{
    private static WaitState m_Instance;
    public static WaitState Instance { get { return m_Instance ??= new WaitState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterWaitState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteWaitState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitWaitState();
    }
}

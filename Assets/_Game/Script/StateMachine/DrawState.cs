using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawState : State<MatchManager>
{
    private static DrawState m_Instance;
    public static DrawState Instance { get { return m_Instance ??= new DrawState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterDrawState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteDrawState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitDrawState();
    }
}

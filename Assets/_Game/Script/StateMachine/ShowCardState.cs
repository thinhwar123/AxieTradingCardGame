using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardState : State<MatchManager>
{
    private static ShowCardState m_Instance;
    public static ShowCardState Instance { get { return m_Instance ??= new ShowCardState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterShowCardState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteShowCardState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitShowCardState();
    }
}

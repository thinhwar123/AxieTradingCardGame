using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCardState : State<MatchManager>
{
    private static SetupCardState m_Instance;
    public static SetupCardState Instance { get { return m_Instance ??= new SetupCardState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterSetupCardState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteSetupCardState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitSetupCardState();
    }
}

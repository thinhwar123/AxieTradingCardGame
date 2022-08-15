using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnState : MonoBehaviour
{
    private static EndTurnState m_Instance;
    public static EndTurnState Instance { get { return m_Instance ??= new EndTurnState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterEndTurnState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteEndTurnState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitEndTurnState();
    }
}

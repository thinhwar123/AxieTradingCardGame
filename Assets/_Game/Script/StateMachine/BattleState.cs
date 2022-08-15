using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State<MatchManager>
{
    private static BattleState m_Instance;
    public static BattleState Instance { get { return m_Instance ??= new BattleState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterBattleState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteBattleState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitBattleState();
    }
}

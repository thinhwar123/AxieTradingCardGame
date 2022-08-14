using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupAbilityState : State<MatchManager>
{
    private static SetupAbilityState m_Instance;
    public static SetupAbilityState Instance { get { return m_Instance ??= new SetupAbilityState(); } }
    public void Enter(MatchManager go)
    {
        go.OnEnterSetupSkillState();
    }

    public void Execute(MatchManager go)
    {
        go.OnExecuteSetupSkillState();
    }

    public void Exit(MatchManager go)
    {
        go.OnExitSetupSkillState();
    }
}

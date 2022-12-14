using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempData : Singleton<TempData>
{
    public System.Random m_Random;
    [Header("Player Match Data")]
    public PlayerMatchData m_PlayerData;
    public PlayerMatchData m_OpponentData;
    private int m_ID;

    protected override void Awake()
    {
        base.Awake();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(m_Random.Next());
        }
    }
    public void InitNewData()
    {
        m_Random = new System.Random(123);
    }
    public void AddPlayerMathData(PlayerMatchData playerMatchData)
    {
        m_PlayerData = playerMatchData;
    }
    public PlayerMatchData GetPlayerData()
    {
        return m_PlayerData;
    }
    public PlayerMatchData GetOpponentData()
    {
        return m_OpponentData;
    }
    public List<bool> GetSkillActiveByTurn()
    {
        List<bool> attacker = new List<bool>();
        List<bool> defender = new List<bool>();
        if (GetPlayerData().m_BattleRole == BattleRole.ATTACKER)
        {
            attacker = GetPlayerData().m_SelectSkill;
            defender = GetOpponentData().m_SelectSkill;
        }
        else
        {
            attacker = GetOpponentData().m_SelectSkill;
            defender = GetPlayerData().m_SelectSkill;
        }
        Debug.Log(attacker.Count.ToString() + " " + defender.Count);

        List<bool> list = new List<bool>();
        for (int i = 0; i < attacker.Count; i++)
        {
            list.Add(attacker[i]);
            list.Add(defender[i]);
        }
        return list;
    }
}

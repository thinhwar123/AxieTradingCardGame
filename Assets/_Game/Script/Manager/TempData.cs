using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempData : Singleton<TempData>
{
    public System.Random m_Random;
    [Header("Player Match Data")]
    public List<PlayerMatchData> m_AllMatchData;
    public PlayerMatchData m_OpponentData;
    private int m_ID;

    protected override void Awake()
    {
        base.Awake();
        m_Random = new System.Random(123);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(m_Random.Next());
        }
    }
    public void AddPlayerMathData(PlayerMatchData playerMatchData)
    {
        m_ID = m_AllMatchData.Count;
        m_AllMatchData.Add(playerMatchData);
    }
    public PlayerMatchData GetPlayerData()
    {
        return m_AllMatchData[m_ID];
    }
    public PlayerMatchData GetOpponentData()
    {
        return m_OpponentData;
        for (int i = 0; i < m_AllMatchData.Count; i++)
        {
            if (i != m_ID)
            {
                return m_AllMatchData[i];
            }
        }
        return null;
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

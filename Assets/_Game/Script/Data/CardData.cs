using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardData
{
    public string m_ID;
    public string m_Name;
    public string m_Archetype;
    public Symbol m_Symbol;
    public AbilityType m_AbilityType;
    public string m_EffectDescription;
    public List<string> m_WinAnimation;
    public List<string> m_LoseAnimation;

    public CardData(string iD, string name, string archetype, Symbol symbol, AbilityType abilityType, string effectDescription, List<string> winAnimation, List<string> loseAnimation)
    {
        m_ID = iD;
        m_Name = name;
        m_Archetype = archetype;
        m_Symbol = symbol;
        m_AbilityType = abilityType;
        m_EffectDescription = effectDescription;
        m_WinAnimation = winAnimation;
        m_LoseAnimation = loseAnimation;
    }
}

public enum Symbol
{
    ROCK = 0,
    PAPPER = 1,
    SCISSORS = 2,
}
public enum AbilityType
{
    ACTIVE = 0,
    PASSIVE = 1,
}
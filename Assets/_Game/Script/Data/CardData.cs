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
    
    public CardData()
    {
        m_ID = "11410944";
        m_Name = "The Swap 1";
        m_Archetype = "Swap Back";
        m_Symbol = Symbol.ROCK;
        m_AbilityType = AbilityType.ACTIVE;
        m_EffectDescription = "Swap this card with your previous card";
        m_WinAnimation = new List<string>() { "attack/melee/horn-gore", "attack/melee/mouth-bite", "attack/melee/multi-attack", "attack/melee/normal-attack", "attack/melee/shrimp" };
        m_LoseAnimation = new List<string>() { "defense/hit-by-normal", "defense/hit-by-ranged-attack" };
    }

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
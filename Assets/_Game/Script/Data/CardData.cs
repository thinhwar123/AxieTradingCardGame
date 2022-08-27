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
    public string m_MeleeAnimation;
    public string m_RangeAnimation;
    public string m_DefenceAnimation;
    
    public CardData()
    {
        m_ID = "11410944";
        m_Name = "The Swap 1";
        m_Archetype = "Swap Back";
        m_Symbol = Symbol.ROCK;
        m_AbilityType = AbilityType.ACTIVE;
        m_EffectDescription = "Swap this card with your previous card";
    }

    public CardData(string iD, string name, string archetype, Symbol symbol, AbilityType abilityType, string effectDescription, string meleeAnimation, string rangeAnimation, string defenceAnimation)
    {
        m_ID = iD;
        m_Name = name;
        m_Archetype = archetype;
        m_Symbol = symbol;
        m_AbilityType = abilityType;
        m_EffectDescription = effectDescription;
        m_MeleeAnimation = meleeAnimation;
        m_RangeAnimation = rangeAnimation;
        m_DefenceAnimation = defenceAnimation;
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
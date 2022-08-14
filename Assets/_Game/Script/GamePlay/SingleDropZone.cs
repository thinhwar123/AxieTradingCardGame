using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDropZone : MonoBehaviour
{
    public DropZone m_DropZone;

    public StateButton m_ActiveButton;
    public GameObject m_PassiveButton;

    public bool m_IsActiveSkill;

    public BasicCard GetBasicCard()
    {
        if (IsEmpty()) return null;
        return m_DropZone.m_ListBasicCard[0];
    }
    public bool IsEmpty()
    {
        return m_DropZone.m_ListBasicCard.Count == 0;
    }
    public void ShowAbilityOptionButton(bool value)
    {
        bool isActiveSkill = GetBasicCard().m_CardData.m_AbilityType == AbilityType.ACTIVE;
        m_ActiveButton.gameObject.SetActive(isActiveSkill && value);
        m_PassiveButton.SetActive(!isActiveSkill && value);
        m_IsActiveSkill = !isActiveSkill;
                
        if (isActiveSkill)
        {            
            m_ActiveButton.SetState(false);
        }
    }
    public void OnClickStateButton(bool value)
    {
        m_IsActiveSkill = value;
    }
}

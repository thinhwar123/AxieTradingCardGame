using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UICShowCardInfor : UICanvas
{
    [SerializeField] private Transform m_CardPosition;
    private CardController m_CardController;

    public Transform GetCardPosition()
    {
        return m_CardPosition;
    }
}

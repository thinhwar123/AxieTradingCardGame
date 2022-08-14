using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public DropZone m_DropZone;

    public BasicCard GetRandomBasicCardInHand()
    {
        if (IsEmpty()) return null;
        return m_DropZone.m_ListBasicCard[Random.Range(0, m_DropZone.m_ListBasicCard.Count)];
    }
    public bool IsEmpty()
    {
        return m_DropZone.m_ListBasicCard.Count == 0;
    }
}

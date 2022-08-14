using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;
public class UICDeckBuild : UICanvas
{
    [SerializeField] private BasicCard m_BasicCard;
    [SerializeField] private Transform m_StartSpawnPosition;
    [SerializeField] private DropZone m_AllCard;

    private void Start()
    {
        InitAllCard();
    }
    private void InitAllCard()
    {
        BasicCard card;
        for (int i = 0; i < CardDataManager.Instance.m_CardDatas.Count; i++)
        {

            card = SimplePool.Spawn(m_BasicCard, m_StartSpawnPosition.position, Quaternion.identity);
            card.Transform.parent = m_AllCard.Transform;
            card.SetupCardConfig(CardDataManager.Instance.m_CardDatas[i]);
            card.Setup(true);
            card.InitCard(-1);
        }
    }
}

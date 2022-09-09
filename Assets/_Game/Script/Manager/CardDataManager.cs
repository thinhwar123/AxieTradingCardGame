using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataManager : Singleton<CardDataManager>
{
    [SerializeField] private TextAsset m_CardDataJson;

    public List<CardData> m_CardDatas;

    public string GetCardDataJsonString()
    {
        return m_CardDataJson.text;
    }
    public CardData GetCardData(string id)
    {
        for (int i = 0; i < m_CardDatas.Count; i++)
        {
            if (m_CardDatas[i].m_ID == id)
            {
                return m_CardDatas[i];
            }
        }
        return null;
    }
}

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
}

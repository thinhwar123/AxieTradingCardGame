using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public List<CardData> m_ListCardDatas;
    public bool isLoad = false;

    protected override void Awake()
    {
        base.Awake();
        m_ListCardDatas = new List<CardData>();
    }


}

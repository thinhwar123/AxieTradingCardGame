using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public List<BasicCard> m_ListCardDatas;

    protected override void Awake()
    {
        m_ListCardDatas = new List<BasicCard>();
    }
}

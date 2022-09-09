using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : Singleton<UIEffectManager>
{
    public List<UIEffect> m_Effects = new List<UIEffect>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayEffect("Swap", UI_Game.Instance.CanvasParentTF, Vector3.zero);
        }
    }
    public void PlayEffect(string effectName, Transform parent, Vector3 position)
    {
        Debug.Log(effectName);
        Thinh.SimplePool.Spawn(GetEffect(effectName)).OnSpawn(parent, position);
    }
    public UIEffect GetEffect(string effectName)
    {
        for (int i = 0; i < m_Effects.Count; i++)
        {
            if (m_Effects[i].m_EffectName == effectName)
            {
                return m_Effects[i];
            }
        }
        return m_Effects[0];
    }
}

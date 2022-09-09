using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;


public class UIProjectilesManager : Singleton<UIProjectilesManager>
{
    public List<UIProjectiles> m_UIProjectiles;
    
    public UIProjectiles GetUIProjectile(string name)
    {
        Debug.Log(name);
        for (int i = 0; i < m_UIProjectiles.Count; i++)
        {
            if (m_UIProjectiles[i].m_ProjectileName == name)
            {
                return m_UIProjectiles[i];
            }
        }
        return m_UIProjectiles[0];
    }
}

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;


public class UIProjectilesManager : Singleton<UIProjectilesManager>
{
    public Dictionary<string, UIProjectiles> projectiles;

    private void Start()
    {
        projectiles = new Dictionary<string, UIProjectiles>();
    }
    public void Spawn()
    {
        Spawn("aquatic-back-04-scale-dart");
    }
    public UIProjectiles Spawn(string key)
    {
        if (!projectiles.ContainsKey(key))
        {
            Debug.Log(key);
            UIProjectiles pt = SimplePool.Spawn(Resources.Load<UIProjectiles>("Projectiles/Spine GameObject (" + key + ")"));
            projectiles.Add(key, pt);
        }
        projectiles[key].gameObject.SetActive(true);
        return projectiles[key];
    }
    public void DeSpawn(string key)
    {
        if (projectiles.ContainsKey(key))
        {
            projectiles[key].gameObject.SetActive(false);
        }
    }
}

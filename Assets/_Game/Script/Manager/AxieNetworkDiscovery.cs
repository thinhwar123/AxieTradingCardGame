using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(NetworkDiscovery))]
public class AxieNetworkDiscovery : Singleton<AxieNetworkDiscovery>
{
    private NetworkDiscovery m_networkDiscovery;
    public NetworkDiscovery NetworkDiscovery { 
        get {
            if(m_networkDiscovery == null)
            {
                m_networkDiscovery = GetComponent<NetworkDiscovery>();
            }
            else if (m_networkDiscovery.transport == null)
            {
                m_networkDiscovery.transport = FindObjectOfType<Transport>();
            }
            return m_networkDiscovery; 
        } 
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (m_networkDiscovery == null)
        {
            m_networkDiscovery = GetComponent<NetworkDiscovery>();
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, );
            UnityEditor.Undo.RecordObjects(new Object[] { this, m_networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
}

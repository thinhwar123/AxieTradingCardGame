using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(NetworkDiscovery))]
public class AxieNetworkDiscovery : Singleton<AxieNetworkDiscovery>
{
    public NetworkDiscovery networkDiscovery;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, );
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
}

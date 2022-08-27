using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonJoin : MonoBehaviour
{
    public ServerResponse info;

    public void Connect()
    {
        if(info.uri != null)
        {
            AxieNetworkDiscovery.Instance.NetworkDiscovery.StopDiscovery();
            AxieNetworkManager.singleton.StartClient(info.uri);
        }
    }
}

using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonJoin : MonoBehaviour
{
    public ServerResponse info;
    public TextMeshProUGUI textPlayer;

    public void Connect()
    {
        if(info.uri != null)
        {
            AxieNetworkDiscovery.Instance.NetworkDiscovery.StopDiscovery();
            AxieNetworkManager.singleton.StartClient(info.uri);
        }
    }
}

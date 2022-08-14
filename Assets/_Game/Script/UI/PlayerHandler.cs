using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerHandler : NetworkBehaviour
{
    public List<DropZone> m_dropZone;
    [SyncVar]
    public string message;
    
    private PlayerHandler otherPlayerHandler;

    void Start()
    {
        if(!isLocalPlayer){
            gameObject.SetActive(false);
        }else{
            GameManager.Instance.m_Player.Add(this);
        }
        NetworkIdentity ni = NetworkClient.connection.identity;
        
    }

    void Update(){
        if(!isLocalPlayer){
            return;
        }
        if (Input.GetMouseButtonDown(0)){
            SendMessage();
        }
    }
    [Command(requiresAuthority = false)]
    void SendMessage(NetworkConnectionToClient sender = null){
        if(otherPlayerHandler == null){
            int num = AxieNetworkManager.Instance.playerSpawned.Count;
            for (int i = 0; i < num; i++){
                if(AxieNetworkManager.Instance.playerSpawned[i].GetComponent<PlayerHandler>() != this){
                    otherPlayerHandler = AxieNetworkManager.Instance.playerSpawned[i].GetComponent<PlayerHandler>();
                    break;
                }
            }
        }
        if(otherPlayerHandler == null){
            return;
        }
        otherPlayerHandler.message = "Send From Mana";
        Debug.Log("Send from Mana");
    }   
    
}

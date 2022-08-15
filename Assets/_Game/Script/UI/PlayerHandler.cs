using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerHandler : NetworkBehaviour
{
    public List<DropZone> m_dropZone;
    [SyncVar]
    public string message;
    [SyncVar]
    public Phase m_CurrentPhase;
    
    private PlayerHandler otherPlayerHandler;

    //void Start()
    //{
    //    if(!isLocalPlayer){
    //        gameObject.SetActive(false);
    //    }else{
    //        GameManager.Instance.m_Player.Add(this);
    //    }
    //    NetworkIdentity ni = NetworkClient.connection.identity;
    //}
    [ClientRpc]
    public void StartGame()
    {
        MatchManager.Instance.StartGame(this);
    }

    [Command(requiresAuthority = false)]
    public void NextPhase(Phase m_Phase, NetworkConnectionToClient sender = null)
    {
        this.m_CurrentPhase = m_Phase;
        if (otherPlayerHandler == null)
        {
            int num = AxieNetworkManager.Instance.playerSpawned.Count;
            for (int i = 0; i < num; i++)
            {
                if (AxieNetworkManager.Instance.playerSpawned[i].GetComponent<PlayerHandler>() != this)
                {
                    otherPlayerHandler = AxieNetworkManager.Instance.playerSpawned[i].GetComponent<PlayerHandler>();
                    break;
                }
            }
        }
        if (otherPlayerHandler == null)
        {
            return;
        }
        if (otherPlayerHandler.m_CurrentPhase == m_Phase)
        {
            otherPlayerHandler.StartCurrentPhase();
            StartCurrentPhase();
        }
    }

    public void StartCurrentPhase()
    {
        switch (m_CurrentPhase)
        {
            case Phase.SHOW_CARD:
                MatchManager.Instance.StartShowCardPhase();
                break;
            case Phase.BATTLE:
                MatchManager.Instance.StartBattlePhase();
                break;
        }
    }

    void Update(){
        if(!isLocalPlayer){
            return;
        }
        if (Input.GetKeyDown(KeyCode.C)){
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerHandler : NetworkBehaviour
{
    public List<DropZone> m_dropZone;
    private PlayerHandler otherPlayerHandler;
    [SyncVar]
    public PlayerMatchData m_MatchData;
    [SyncVar]
    public Phase m_CurrentPhase;

    [ClientRpc]
    public void StartGame()
    {
        if (isLocalPlayer)
        {
            MatchManager.Instance.StartGame(this);
        }
    }
    [Client]
    public void NextPhase(Phase m_Phase)
    {
        NextPhaseServer(m_Phase);
    }

    [Command(requiresAuthority = false) ]
    public void NextPhaseServer(Phase m_Phase, NetworkConnectionToClient sender = null)
    {
        PlayerHandler sender_handler = sender.identity.GetComponent<PlayerHandler>();
        if (sender_handler.otherPlayerHandler == null || sender_handler.otherPlayerHandler == sender_handler)
        {
            for (int i = 0; i < AxieNetworkManager.Instance.maxConnections; i++)
            {
                if (AxieNetworkManager.Instance.playerHandlers[i] != sender_handler)
                {
                    sender_handler.otherPlayerHandler = AxieNetworkManager.Instance.playerHandlers[i];
                    break;
                }
            }
        }
        if (sender_handler.otherPlayerHandler == null)
        {
            Debug.Log("Not Found Other Handler");
            return;
        }
        sender_handler.m_CurrentPhase = m_Phase;
        if (sender_handler.otherPlayerHandler.m_CurrentPhase == m_Phase)
        {
            for(int i = 0; i < AxieNetworkManager.Instance.maxConnections; i++)
            {
                AxieNetworkManager.Instance.playerHandlers[i].StartCurrentPhase(m_Phase);
            }
        }
    }
    [ClientRpc]
    public void StartCurrentPhase(Phase m_phase)
    {
        if (isLocalPlayer)
        {
            
            switch (m_phase)
            {
                case Phase.SHOW_CARD:
                    MatchManager.Instance.m_StartCountTime = false;
                    MatchManager.Instance.StartShowCardPhase();
                    break;
                case Phase.BATTLE:
                    MatchManager.Instance.m_StartCountTime = false;
                    MatchManager.Instance.StartBattlePhase();
                    break;
            }
        }
    }
    [Client]
    public void SetUpMatchData(PlayerMatchData data)
    {
        SetUpMatchDataServer(data);
    }
    [Command(requiresAuthority = false)]
    public void SetUpMatchDataServer(PlayerMatchData data, NetworkConnectionToClient sender = null)
    {
        PlayerHandler sender_handler = sender.identity.GetComponent<PlayerHandler>();
        if (sender_handler.otherPlayerHandler == null || sender_handler.otherPlayerHandler == sender_handler)
        {
            for (int i = 0; i < AxieNetworkManager.Instance.maxConnections; i++)
            {
                if (AxieNetworkManager.Instance.playerHandlers[i] != sender_handler)
                {
                    AxieNetworkManager.Instance.playerHandlers[i].MatchData(data);
                    return;
                }
            }
        }
        else
        {
            sender_handler.otherPlayerHandler.MatchData(data);
        }
    }
    [ClientRpc]
    public void MatchData(PlayerMatchData data)
    {
        if (isLocalPlayer)
        {
            TempData.Instance.m_OpponentData = data;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AxieNetworkManager : NetworkManager
{
    private static AxieNetworkManager m_Instance;
    public static AxieNetworkManager Instance{
        get {
            m_Instance = FindObjectOfType<AxieNetworkManager>();

            // Create new instance if one doesn't already exist.
            if (m_Instance == null)
            {
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                m_Instance = singletonObject.AddComponent<AxieNetworkManager>();
                singletonObject.name = typeof(AxieNetworkManager).ToString() + " (Singleton)";

                // Make instance persistent.
                //DontDestroyOnLoad(singletonObject);
            }
            return m_Instance;
        }
    }
    private List<GameObject> playerSpawned;
    public List<PlayerHandler> playerHandlers;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if(playerSpawned == null){
                playerSpawned = new List<GameObject>();
            }
            Transform startPos = GetStartPosition();
            GameObject player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
            playerSpawned.Add(player);
            if(playerSpawned.Count == maxConnections)
        {
            playerHandlers = new List<PlayerHandler>();
            for (int i = 0; i < maxConnections; i++)
            {
                playerHandlers.Add(playerSpawned[i].GetComponent<PlayerHandler>());
                playerHandlers[i].StartGame();
            }
        }
        }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        UI_Game.Instance.CloseUI(UIID.UICIngame);
    }
}

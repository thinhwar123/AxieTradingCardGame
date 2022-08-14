using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AxieNetworkManager : NetworkManager
{
    private static AxieNetworkManager instance;
    public static AxieNetworkManager Instance{
        get {
            if (instance == null){
                instance = FindObjectOfType<AxieNetworkManager>();
            }
            return instance;
        }
    }
    public List<GameObject> playerSpawned;
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
        }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            // destroy ball
            if (playerSpawned != null)
            foreach(GameObject player in playerSpawned){
                NetworkServer.Destroy(player);
            }

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
}

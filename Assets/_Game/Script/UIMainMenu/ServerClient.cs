using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ServerClient : NetworkBehaviour
{
    // Start is called before the first frame update
    public bool goGame;

    //public int index;

    public bool isFinishLoadingMatch = false;

    public UIManager uiManager;

    //private void Start()
    //{
    //    goGame = false;
    //}

    [ClientRpc]
    public void GoGame()
    {
        if (isServer) return;
        Debug.Log("Go!");
        goGame = true;
    }

    [ClientRpc]
    public void AvatarEnemyClient(int i)
    {
        if (isServer) return;
        //uiManager.avatarEnemy.sprite = uiManager.listAvatars[i];
        PlayerPrefs.SetInt("enemy", i);
    }

    [ClientRpc]
    public void FinishLoadingClient()
    {
        if (isServer) return;
        Debug.Log("Loading from client");
        isFinishLoadingMatch = true;
    }

    [Command(requiresAuthority = false)]
    public void FinishLoadingServer()
    {
        Debug.Log("Loading from server");
        isFinishLoadingMatch = true;
    }

    [Command(requiresAuthority = false)]
    public void AvatarEnemyServer(int i)
    {
        //uiManager.avatarEnemy.sprite = uiManager.listAvatars[i];
        PlayerPrefs.SetInt("enemy", i);
    }
}

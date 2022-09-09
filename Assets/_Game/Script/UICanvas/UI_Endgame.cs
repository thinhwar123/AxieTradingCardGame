using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Endgame : UICanvas
{
    //public Image player;
    //public Image enemy;
    public GameObject playerWin;
    public GameObject playerLose;
    public GameObject playerDraw;
    public Image loadIcon;
    public bool isLoading;
    public float count;
    //public GameObject enemyLose;
    //public AvatarData list;
    public override void Setup()
    {
        base.Setup();
        count = 0f;
    }

    private void Update()
    {
        if (isLoading)
        {
            loadIcon.transform.Rotate(Vector3.forward * 50f * Time.deltaTime);
            count += Time.deltaTime;
            if(count >= 5f)
            {
                BackToMainMenu();
                isLoading = false;
            }
        }
    }

    public void Setup(int state)
    {
        //player.sprite = list.listAvatar[PlayerPrefs.GetInt("player")].avatar;
        //enemy.sprite = list.listAvatar[PlayerPrefs.GetInt("enemy")].avatar;
        if (state == -1)
        {
            playerWin.SetActive(false);
            playerLose.SetActive(true);
            playerDraw.SetActive(false);
        }
        else if(state == 0)
        {
            playerWin.SetActive(false);
            playerLose.SetActive(false);
            playerDraw.SetActive(true);
        }
        else if(state == 1)
        {
            playerWin.SetActive(true);
            playerLose.SetActive(false);
            playerDraw.SetActive(false);
        }
        isLoading = true;
    }

    public void BackToMainMenu()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
            AxieNetworkDiscovery.Instance.NetworkDiscovery.StopDiscovery();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
            AxieNetworkDiscovery.Instance.NetworkDiscovery.StopDiscovery();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
            AxieNetworkDiscovery.Instance.NetworkDiscovery.StopDiscovery();
        }
        UI_Game.Instance.CloseUI(UIID.UICIngame);
        UI_Game.Instance.CloseUI(UIID.UICEndGame);
    }
}

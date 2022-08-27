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
        //code
        Debug.Log("Back");
        UI_Game.Instance.CloseUI(UIID.UICIngame);
        UI_Game.Instance.CloseUI(UIID.UICEndGame);
        //Stophost
        if (MatchManager.Instance.GetPlayerHandler().isServerOnly)
        {
            AxieNetworkManager.Instance.StopHost();
        }
    }

}

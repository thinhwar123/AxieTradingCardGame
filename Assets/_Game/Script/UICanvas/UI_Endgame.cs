using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Endgame : UICanvas
{
    public Image player;
    public Image enemy;
    public GameObject playerWin;
    public GameObject playerLose;
    public GameObject enemyWin;
    public GameObject enemyLose;
    public AvatarData list;

    public void Setup(int state)
    {
        player.sprite = list.listAvatar[PlayerPrefs.GetInt("player")].avatar;
        enemy.sprite = list.listAvatar[PlayerPrefs.GetInt("enemy")].avatar;
        if (state == -1)
        {
            playerWin.SetActive(false);
            playerLose.SetActive(true);
            enemyWin.SetActive(true);
            enemyLose.SetActive(false);
        }
        else if(state == 0)
        {
            playerWin.SetActive(true);
            playerLose.SetActive(false);
            enemyWin.SetActive(true);
            enemyLose.SetActive(false);
        }
        else if(state == 1)
        {
            playerWin.SetActive(true);
            playerLose.SetActive(false);
            enemyWin.SetActive(false);
            enemyLose.SetActive(true);
        }
    }

}

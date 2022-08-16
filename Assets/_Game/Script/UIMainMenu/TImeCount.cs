using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TImeCount : MonoBehaviour
{
    // Start is called before the first frame update
    private int second;
    private int minute;
    private float countTime = 0f;
    public bool isWaiting;
    public TextMeshProUGUI timeWait;
    public GameObject waitPanel;
    
    void Start()
    {
        second = 0;
        minute = 0;
        countTime = 0f;
        isWaiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting)
        {
            TimeCount();
        }
    }

    public void TimeCount()
    {
        countTime += Time.deltaTime;
        if (countTime > second + 1 && countTime < second + 2)
        {
            second++;
            if (second >= 60)
            {
                minute++;
                second = 0;
                countTime = 0f;
            }
        }
        string min = "";
        string sec = "";

        if (minute < 10)
        {
            min = "0" + minute.ToString();
        }
        else
        {
            min = minute.ToString();
        }

        if (second < 10)
        {
            sec = "0" + second.ToString();
        }
        else
        {
            sec = second.ToString();
        }

        timeWait.text = min + " : " + sec;
    }

    public void StopWaiting()
    {
        isWaiting = false;
        waitPanel.SetActive(false);
        //isWaiting = false;
    }
}

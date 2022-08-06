using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    #region Inspector Variables
    public float timeLoad = 5f;
    public Text tvPercent;
    public float timeSplash = 1.5f;
    #endregion

    #region Member Variables
    bool loading = false;
    float _time = 0;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Invoke("EnableLoading", timeSplash);
        _time = 0;
    }

    private void Update()
    {
        if (loading)
        {
            if (_time < timeLoad)
            {
                _time += Time.deltaTime;
                if (tvPercent != null)
                {
                    tvPercent.text = string.Format("{0:0}%", (_time / timeLoad) * 100);
                }
            }
            else
            {
                loading = false;
                GotoGame();
            }
        }
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void EnableLoading()
    {
        loading = true;
    }
    private void GotoGame()
    {
        AsyncOperation asyn = SceneManager.LoadSceneAsync(1);
    }
    #endregion
}

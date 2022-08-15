using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StateButton : MonoBehaviour
{
    public UnityAction<bool> OnClick;

    public bool isOn;

    [Header("----- UI -----")]
    public GameObject isOffGO;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            SetState(!isOn);
            OnClick?.Invoke(isOn);
        });
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;
        isOffGO.SetActive(!isOn);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Numerics;


[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserData", order = 1)]
public class UserData : ScriptableObject
{
    /// <summary>
    ///  0 = lock , 1 = unlock , 2 = selected
    /// </summary>
    /// <param name="data"></param>
    /// <param name="ID"></param>
    /// <param name="state"></param>
    public void SetDataState(string data, int ID, int state)
    {
        PlayerPrefs.SetInt(data + ID, state);
    }

    /// <summary>
    ///  0 = lock , 1 = unlock , 2 = selected
    /// </summary>
    /// <param name="data"></param>
    /// <param name="ID"></param>
    /// <param name="state"></param>
    public int GetDataState(string data, int ID, int defaultID = 0)
    {
        return PlayerPrefs.GetInt(data + ID, defaultID);
    }

    /// <summary>
    ///  0 = lock , 1 = unlock , 2 = selected
    /// </summary>
    /// <param name="data"></param>
    /// <param name="ID"></param>
    /// <param name="state"></param>
    public void SetDataState(string data, int ID, ref List<int> variable, int state)
    {
        variable[ID] = state;
        PlayerPrefs.SetInt(data + ID, state);
    }

    /// <summary>
    /// Key_Name
    /// if(bool) true == 1
    /// </summary>
    /// <param name="data"></param>
    /// <param name="value"></param>
    public void SetIntData(string data, ref int variable, int value)
    {
        variable = value;
        PlayerPrefs.SetInt(data, value);
    } 
    
    public void SetBoolData(string data, ref bool variable, bool value)
    {
        variable = value;
        PlayerPrefs.SetInt(data, value ? 1 : 0);
    }

    public void SetFloatData(string data, ref float variable, float value)
    {
        variable = value;
        PlayerPrefs.GetFloat(data, value);
    }

    public void SetStringData(string data, ref string variable, string value)
    {
        variable = value;
        PlayerPrefs.SetString(data, value);
    }

    public void OnInitData()
    {

    }

    public void OnResetData()
    {
        PlayerPrefs.DeleteAll();
        OnInitData();
    }

    public const string Key_Level = "Level";
    public const string Key_Cash = "Cash";
    public const string Key_FxIsOn = "SoundIsOn";
    public const string Key_MusicIsOn = "MusicIsOn";
    public const string Key_VibrationIsOn = "VibrationIsOn";
    public const string Key_RemoveAds = "RemoveAds";
    public const string Key_Tutorial = "Tutorial";

    public const string Key_Slot_Type_ = "KeySlotType_";
    public const string Key_Slot_Level_ = "KeySlotLevel_";

    public const string Key_Max_Level_Melee_Unlock = "Key_Max_Level_Melee_Unlock";
    public const string Key_Max_Level_Range_Unlock = "Key_Max_Level_Range_Unlock";
}



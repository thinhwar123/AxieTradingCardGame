using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TempData))]
public class TempDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Test Data"))
        {
            TempData tempDataEditor = (TempData)target;
            EditorUtility.SetDirty(tempDataEditor);

            tempDataEditor.m_AllMatchData = new List<PlayerMatchData>();

            PlayerMatchData playerMatchData = new PlayerMatchData();
            List<CardData> cardDatas = CardDataManager.Instance.m_CardDatas;
            for (int i = 0; i < 3; i++)
            {
                CardData cardData = cardDatas[Random.Range(0, cardDatas.Count)];
                playerMatchData.m_SelectCard.Add(cardData);
                playerMatchData.m_SelectSkill.Add(true);
            }

            tempDataEditor.m_AllMatchData.Add(playerMatchData);
        }
    }
}

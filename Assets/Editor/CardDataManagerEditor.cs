using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
[CustomEditor(typeof(CardDataManager))]
public class CardDataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Card Datas"))
        {
            CardDataManager cardDataManager = (CardDataManager)target;
            EditorUtility.SetDirty(cardDataManager);
            CardData cardData;

            cardDataManager.m_CardDatas = new List<CardData>();

            JSONNode root = JSONNode.Parse(cardDataManager.GetCardDataJsonString());

            for (int i = 0; i < root.Count; i++)
            {
                JSONNode node = root[i];

                cardData = new CardData(node["id"], node["name"], node["archetype"], GetSymbol(node["symbol"]), GetAbilityType(node["active"]), node["effect_description"], node["melee_animation"], node["ranged_animation"], node["defense_animation"]);
                cardDataManager.m_CardDatas.Add(cardData);
            }

        }

    }
    public Symbol GetSymbol(string symbol)
    {
        switch (symbol)
        {
            case "rock":
                return Symbol.ROCK;
            case "paper":
                return Symbol.PAPPER;
            case "sciccors":
                return Symbol.SCISSORS;
        }
        return Symbol.ROCK;
    }
    public AbilityType GetAbilityType(string abilityType)
    {
        switch (abilityType)
        {
            case "true":
                return AbilityType.ACTIVE;
            case "false":
                return AbilityType.PASSIVE;
        }
        return AbilityType.ACTIVE;
    }
    public List<string> GetListAnimation(JSONNode jsonNode)
    {
        List<string> listAnimation = new List<string>();
        JSONArray array = jsonNode.AsArray;
        for (int i = 0; i < array.Count; i++)
        {
            listAnimation.Add(array[i]);
        }


        return listAnimation;
    }
}

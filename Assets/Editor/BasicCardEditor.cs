using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BasicCard)), CanEditMultipleObjects]
public class BasicCardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update Card"))
        {
            foreach (var card in targets)
            {
                BasicCard basicCard = (BasicCard) card;
                basicCard.ResetCard();
                basicCard.InitCard();
            }
        }
        if (GUILayout.Button("Random Card"))
        {
            foreach (var card in targets)
            {
                BasicCard basicCard = (BasicCard)card;
                basicCard.RandomCardConfig();
                basicCard.ResetCard();
                basicCard.InitCard();
            }
        }
        if (GUILayout.Button("Flip Card False"))
        {
            foreach (var card in targets)
            {
                BasicCard basicCard = (BasicCard)card;
                basicCard.FlipCard(false);
            }
        }
        if (GUILayout.Button("Flip Card True"))
        {
            foreach (var card in targets)
            {
                BasicCard basicCard = (BasicCard)card;
                basicCard.FlipCard(true);
            }
        }

    }
}

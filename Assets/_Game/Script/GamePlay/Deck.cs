using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;
public class Deck : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [SerializeField] private BasicCard m_BasicCard;

    public void DrawACard(DropZone dropZone, int cardLookDirection)
    {
        BasicCard card = SimplePool.Spawn(m_BasicCard, Transform.position, Quaternion.identity);
        card.Transform.SetParent(Transform);
        card.transform.localScale = Vector3.one;
        card.RandomCardConfig();
        card.Setup(false);
        card.SetCanDrag(false);
        card.InitCard(cardLookDirection);

        MatchManager.Instance.m_BasicCards.Add(card);
        StartCoroutine(card.m_CardController.MoveCardToDropZone(dropZone, () => card.FlipCard(true)));
    }
}

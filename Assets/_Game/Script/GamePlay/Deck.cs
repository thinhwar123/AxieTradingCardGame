using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;
public class Deck : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [SerializeField] private BasicCard m_BasicCard;
    [SerializeField] private List<CardData> m_ListCardDataInDeck;

    public void InitDeck()
    {
        m_ListCardDataInDeck = new List<CardData>();
        for (int i = 0; i < 24; i++)
        {
            m_ListCardDataInDeck.Add(CardDataManager.Instance.m_CardDatas[PlayerPrefs.GetInt(i.ToString(), i)]);
        }
    }

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
    public void DrawACard(CardData cardData, DropZone dropZone, int cardLookDirection)
    {
        BasicCard card = SimplePool.Spawn(m_BasicCard, Transform.position, Quaternion.identity);
        card.Transform.SetParent(Transform);
        card.transform.localScale = Vector3.one;
        card.SetupCardConfig(cardData);
        card.Setup(false);
        card.SetCanDrag(false);
        card.InitCard(cardLookDirection);

        MatchManager.Instance.m_BasicCards.Add(card);
        StartCoroutine(card.m_CardController.MoveCardToDropZone(dropZone, () => card.FlipCard(true)));
    }
    public void DrawACardFormDeck(DropZone dropZone, int cardLookDirection)
    {
        CardData cardData = m_ListCardDataInDeck[Random.Range(0, m_ListCardDataInDeck.Count)];
        m_ListCardDataInDeck.Remove(cardData);

        BasicCard card = SimplePool.Spawn(m_BasicCard, Transform.position, Quaternion.identity);
        card.Transform.SetParent(Transform);
        card.transform.localScale = Vector3.one;
        card.SetupCardConfig(cardData);
        card.Setup(false);
        card.SetCanDrag(false);
        card.InitCard(cardLookDirection);

        MatchManager.Instance.m_BasicCards.Add(card);
        StartCoroutine(card.m_CardController.MoveCardToDropZone(dropZone, () => card.FlipCard(true)));
    }
}

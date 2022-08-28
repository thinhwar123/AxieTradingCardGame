using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;
using TMPro;
public class Deck : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [SerializeField] private BasicCard m_BasicCard;
    [SerializeField] private List<CardData> m_ListCardDataInDeck;
    public TextMeshProUGUI m_CardInDeck;

    public void InitDeck()
    {
        m_ListCardDataInDeck = new List<CardData>();
        for (int i = 0; i < 24; i++)
        {
            m_ListCardDataInDeck.Add(CardDataManager.Instance.GetCardData(PlayerPrefs.GetString(i.ToString())));
        }
        if (m_CardInDeck != null)
        {
            m_CardInDeck.text = "24/24";
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
        if (m_CardInDeck !=null)
        {
            m_CardInDeck.text = string.Format("{0}/24", m_ListCardDataInDeck.Count);
        }

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
        if (m_CardInDeck != null)
        {
            m_CardInDeck.text = string.Format("{0}/24", m_ListCardDataInDeck.Count);
        }
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
        if (m_CardInDeck != null)
        {
            m_CardInDeck.text = string.Format("{0}/24", m_ListCardDataInDeck.Count);
        }
    }
}

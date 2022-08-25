using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private RectTransform m_RectTransform;
	public RectTransform RectTransform { get { return m_RectTransform ??= GetComponent<RectTransform>(); } }

	private Transform m_Transform;
	public Transform Transform { get { return m_Transform ??= transform; } }

	[SerializeField] private bool m_AutoFixSpace;
	[SerializeField] private bool m_CanDropCard;
	[SerializeField] private HorizontalLayoutGroup m_HorizontalLayoutGroup;
	[SerializeField] private int m_MaxCard;

	public List<BasicCard> m_ListBasicCard = new List<BasicCard>();
	public CardController m_Card;

	public void OnPointerEnter(PointerEventData eventData)
	{
        if (eventData.pointerDrag == null || IsMaxCard() || !m_CanDropCard)
			return;

		CardController card = eventData.pointerDrag.GetComponent<CardController>();
		if (card != null)
		{
			if (!card.m_CanDrag) return;
			card.ChangeDropZone(this);
			card.GetCardSlot().SetParentTransform(Transform);
			m_Card = card;
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{

	}
	public bool IsMaxCard()
    {
		return m_ListBasicCard.Count >= m_MaxCard;
    }
	public void AddBasicCard(BasicCard basicCard)
    {
        if (!m_ListBasicCard.Contains(basicCard))
			m_ListBasicCard.Add(basicCard);
		if (m_AutoFixSpace)
			FixSpace();

	}
	public void RemoveBasicCard(BasicCard basicCard)
    {
		if (m_ListBasicCard.Contains(basicCard))
			m_ListBasicCard.Remove(basicCard);
		if (m_AutoFixSpace)
			FixSpace();
	}
	public void FixSpace()
    {
		float space = 0 ;
        if (RectTransform.rect.width - 40 < 225 * m_ListBasicCard.Count)
        {
			space = ((RectTransform.rect.width - 225 - 40) / (m_ListBasicCard.Count - 1)) - 225;

		}
		m_HorizontalLayoutGroup.spacing = space;
	}
}

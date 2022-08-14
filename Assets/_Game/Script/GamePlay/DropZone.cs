using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Transform m_Transform;
	public Transform Transform { get { return m_Transform ??= transform; } }
	[SerializeField] private int m_MaxCard;

	public CardController m_Card;


	public void OnPointerEnter(PointerEventData eventData)
	{
        Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null || IsMaxCard())
			return;

		CardController card = eventData.pointerDrag.GetComponent<CardController>();
		if (card != null)
		{
			card.GetCardSlot().SetParentTransform(Transform);
			m_Card = card;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("OnPointerExit");
		//if (eventData.pointerDrag == null)
		//	return;

		//CardController card = eventData.pointerDrag.GetComponent<CardController>();
		//if (card != null)
		//{
		//	card.GetCardSlot().SetToLastParentTransform();
		//}
	}
	public bool IsMaxCard()
    {
		return Transform.childCount >= m_MaxCard;
    }
	
}

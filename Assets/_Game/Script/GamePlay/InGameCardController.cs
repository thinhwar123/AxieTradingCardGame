using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameCardController : CardController , IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_IsScale || !m_CanDrag) return;
        KillAllTween();
        m_OnDrag = true;
        m_CurrentCardSlot.SaveLastParentTransform();
        m_CurrentCardSlot.SetBorder(true);
        m_Offset = (Vector2)Transform.position - eventData.position;
        m_CanvasGroup.blocksRaycasts = false;
        Transform.parent = UI_Game.Instance.CanvasParentTF;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_OnDrag && !m_CanDrag)
        {
            KillAllTween();
            m_CurrentCardSlot.SetBorder(false);
            m_Tweens.Add(Transform.DOMove(m_CurrentCardSlot.Transform.position, 0.2f).OnComplete(() => Transform.parent = m_CurrentCardSlot.Transform));
            m_CanvasGroup.blocksRaycasts = true;
            m_OnDrag = false;
        }

        if (m_IsScale || !m_CanDrag) return;
        Transform.position = eventData.position + m_Offset;

        int newSiblingIndex = m_CurrentCardSlot.Transform.parent.childCount;

        for (int i = 0; i < m_CurrentCardSlot.Transform.parent.childCount; i++)
        {
            if (this.Transform.position.x < m_CurrentCardSlot.Transform.parent.GetChild(i).position.x)
            {

                newSiblingIndex = i;

                if (m_CurrentCardSlot.Transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }

        m_CurrentCardSlot.Transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_IsScale || !m_CanDrag) return;
        KillAllTween();
        m_CurrentCardSlot.SetBorder(false);
        m_Tweens.Add(Transform.DOMove(m_CurrentCardSlot.Transform.position, 0.2f).OnComplete(() => Transform.parent = m_CurrentCardSlot.Transform));
        m_CanvasGroup.blocksRaycasts = true;
        m_OnDrag = false;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!m_BasicCard.m_IsFlipped) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (m_IsScale)
            {
                m_IsScale = false;
                KillAllTween();

                Transform.parent = UI_Game.Instance.CanvasParentTF;
                m_Tweens.Add(RectTransform.DOScale(1f, 0.5f));
                m_Tweens.Add(RectTransform.DOMove(m_CurrentCardSlot.Transform.position, 0.5f).OnComplete(() =>
                {
                    Transform.parent = m_CurrentCardSlot.Transform;
                    m_CanDrag = m_LastCanDrag;
                }));

                UI_Game.Instance.CloseUI(UIID.UICShowCardInfor);
                m_BasicCard.ShowAbilityInfor(false);
            }
            else
            {
                m_IsScale = true;
                m_LastCanDrag = m_CanDrag;
                m_CanDrag = false;
                KillAllTween();

                Transform cardPosition = UI_Game.Instance.OpenUI<UICShowCardInfor>(UIID.UICShowCardInfor).GetCardPosition();
                Transform.parent = cardPosition;
                m_Tweens.Add(RectTransform.DOScale(2.5f, 0.5f));
                m_Tweens.Add(RectTransform.DOLocalMove(Vector3.zero, 0.5f));
                m_BasicCard.ShowAbilityInfor(true);
            }

        }
        //else if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    //Debug.Log("Left click");
        //    m_UIManager.ResetPopupFullCard();
        //    if (numberOfCopy < 3)
        //    {
        //        if (!isChooseToDeck)
        //        {
        //            if (m_UIManager.listDeckCards.Count < 24)
        //            {
        //                numberOfCopy++;
        //                m_UIManager.CloneCard(m_BasicCard.GetID());
        //            }
        //            else
        //            {
        //                m_UIManager.PopupFullCard();
        //            }
        //            //m_UIManager.AddBasicCard(m_BasicCard);
        //            //if (m_UIManager.listDeckCards.Count >= 24) numberOfCopy--;
        //        }
        //        else if (isChooseToDeck)
        //        {
        //            isChooseToDeck = false;
        //            m_UIManager.OutDeck(m_BasicCard);
        //            m_UIManager.ChangeNumberOfClone(m_BasicCard.GetID());
        //            m_UIManager.DeleteBasicCard(m_BasicCard);
        //        }
        //    }
        //    else
        //    {
        //        m_UIManager.PopupFullCopyCard();
        //    }
        //}
    }
}

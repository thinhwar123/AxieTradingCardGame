using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckBuildCardController : CardController, IPointerClickHandler
{
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
        else if (eventData.button == PointerEventData.InputButton.Left && !m_IsScale)
        {
            //Debug.Log("Left click");
            m_UIManager.ResetPopupFullCard();
            if (numberOfCopy < 3)
            {
                if (!isChooseToDeck)
                {
                    if (m_UIManager.listDeckCards.Count < 24)
                    {
                        numberOfCopy++;
                        m_UIManager.CloneCard(m_BasicCard.GetID());
                    }
                    else
                    {
                        m_UIManager.PopupFullCard();
                    }
                    //m_UIManager.AddBasicCard(m_BasicCard);
                    //if (m_UIManager.listDeckCards.Count >= 24) numberOfCopy--;
                }
                else if (isChooseToDeck)
                {
                    isChooseToDeck = false;
                    m_UIManager.OutDeck(m_BasicCard);
                    m_UIManager.ChangeNumberOfClone(m_BasicCard.GetID());
                    m_UIManager.DeleteBasicCard(m_BasicCard);
                }
            }
            else
            {
                m_UIManager.PopupFullCopyCard();
            }
        }
    }
}

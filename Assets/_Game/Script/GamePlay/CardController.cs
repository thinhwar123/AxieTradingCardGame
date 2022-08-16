using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Thinh;
using DG.Tweening;
using UnityEngine.Events;
using Mirror;

public class CardController : NetworkBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }
    private RectTransform m_RectTransform;
    public RectTransform RectTransform { get { return m_RectTransform ??= GetComponent<RectTransform>(); } }
    [SerializeField] private DropZone m_CurrentDropZone;
    [SerializeField] private BasicCard m_BasicCard;
    [SerializeField] private CardSlot m_CardSlot;
    [SerializeField] private CanvasGroup m_CanvasGroup;

    public bool m_CanDrag { get; private set; }
    private bool m_IsScale;
    private bool m_OnDrag;
    private Vector2 m_Offset;
    private CardSlot m_CurrentCardSlot;

    public int numberOfCopy = 0;
    public bool isChooseToDeck = false;
    public UIManager m_UIManager;

    private List<Tween> m_Tweens;
    private bool m_LastCanDrag;
    public void InitCardController()
    {
        m_Tweens = new List<Tween>();
        m_IsScale = false;

        CreateCardSlot();
    }
    public void CreateCardSlot()
    {
        m_CurrentCardSlot = SimplePool.Spawn(m_CardSlot, Transform.position, Quaternion.identity);
        m_CurrentCardSlot.SetParentTransform(Transform.parent);
        m_CurrentCardSlot.Transform.localScale = Vector3.one;
        m_CurrentCardSlot.SaveLastParentTransform();
        Transform.parent = m_CurrentCardSlot.Transform;
        Transform.localScale = Vector3.one;

    }
    public CardSlot GetCardSlot()
    {
        return m_CurrentCardSlot;
    }
    public void SetCanDrag(bool value)
    {
        m_CanDrag = value;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_IsScale || !m_CanDrag) return;
        KillAllTween();
        m_OnDrag = true;
        m_CurrentCardSlot.SaveLastParentTransform();
        m_CurrentCardSlot.SetBorder(true);
        m_Offset = (Vector2)Transform.position - eventData.position;
        m_CanvasGroup.blocksRaycasts = false;
        m_Transform.parent = UI_Game.Instance.CanvasParentTF;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_OnDrag && !m_CanDrag)
        {
            KillAllTween();
            m_CurrentCardSlot.SetBorder(false);
            m_Tweens.Add(Transform.DOMove(m_CurrentCardSlot.Transform.position, 0.2f).OnComplete(() => m_Transform.parent = m_CurrentCardSlot.Transform));
            m_CanvasGroup.blocksRaycasts = true;
            m_OnDrag = false;
        }

        if (m_IsScale || !m_CanDrag) return;
        Transform.position = eventData.position + m_Offset;

        int newSiblingIndex = m_CurrentCardSlot.Transform.parent.childCount;

        for (int i = 0; i < m_CurrentCardSlot.Transform.parent.childCount; i++)
        {
            if (this.transform.position.x < m_CurrentCardSlot.Transform.parent.GetChild(i).position.x)
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
        m_Tweens.Add(Transform.DOMove(m_CurrentCardSlot.Transform.position, 0.2f).OnComplete(() => m_Transform.parent = m_CurrentCardSlot.Transform));
        m_CanvasGroup.blocksRaycasts = true;
        m_OnDrag = false;
    }


    public void OnPointerClick(PointerEventData eventData)
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
        else if(eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log("Left click");
            m_UIManager.ResetPopupFullCard();            
            if(numberOfCopy < 3)
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
    public void SetCurrentDropZone(DropZone dropZone)
    {
        m_CurrentDropZone = dropZone;
        m_CurrentDropZone.AddBasicCard(m_BasicCard);
    }
    public void ChangeDropZone(DropZone dropZone)
    {
        if (m_CurrentDropZone != null)
        {
            m_CurrentDropZone.RemoveBasicCard(m_BasicCard);
        }
        m_CurrentDropZone = dropZone;
        m_CurrentDropZone.AddBasicCard(m_BasicCard);
    }
    public IEnumerator MoveCardToDropZone(DropZone dropZone, UnityAction onMoveComplete)
    {
        Debug.Log(dropZone.gameObject.name);
        ChangeDropZone(dropZone);

        Transform.parent = UI_Game.Instance.CanvasParentTF;
        m_CurrentCardSlot.Transform.parent = dropZone.Transform;
        m_CurrentCardSlot.Transform.localScale = Vector3.one;
        yield return new WaitForSeconds(0.5f);
        m_Tweens.Add(Transform.DOMove(m_CurrentCardSlot.Transform.position, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => m_Transform.SetParent(m_CurrentCardSlot.Transform)));
        yield return new WaitForSeconds(0.5f);
        onMoveComplete?.Invoke();
    }
    public void KillAllTween()
    {
        for (int i = 0; i < m_Tweens.Count; i++)
        {
            m_Tweens[i]?.Kill();
        }
        m_Tweens.Clear();
    }
    public void DespawnCardSlot()
    {
        Thinh.SimplePool.Despawn(m_CurrentCardSlot.gameObject);
        m_CurrentCardSlot = null;
    }
}

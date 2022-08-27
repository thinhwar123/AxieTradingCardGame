using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Thinh;
using DG.Tweening;
using UnityEngine.Events;
using Mirror;

public class CardController : NetworkBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }
    private RectTransform m_RectTransform;
    public RectTransform RectTransform { get { return m_RectTransform ??= GetComponent<RectTransform>(); } }
    [SerializeField] protected DropZone m_CurrentDropZone;
    [SerializeField] protected BasicCard m_BasicCard;
    [SerializeField] protected CardSlot m_CardSlot;
    [SerializeField] protected CanvasGroup m_CanvasGroup;

    public bool m_CanDrag { get; protected set; }
    protected bool m_IsScale;
    protected bool m_OnDrag;
    protected Vector2 m_Offset;
    protected CardSlot m_CurrentCardSlot;

    public int numberOfCopy = 0;
    public bool isChooseToDeck = false;
    public UIManager m_UIManager;

    protected List<Tween> m_Tweens;
    protected bool m_LastCanDrag;
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
        Transform.SetParent(m_CurrentCardSlot.Transform);
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

        Transform.SetParent(UI_Game.Instance.CanvasParentTF);
        m_CurrentCardSlot.Transform.SetParent(dropZone.Transform);
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
    public virtual void ScaleToNormal()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UICIngame : UICanvas
{
    [SerializeField] private Deck m_PlayerDeck;
    [SerializeField] private Hand m_PlayerHand;
    [SerializeField] private List<SingleDropZone> m_PlayerSingleDropZones;

    [SerializeField] private Deck m_OpponentDeck;
    [SerializeField] private List<SingleDropZone> m_OpponentSingleDropZones;

    [SerializeField] private Image m_ImageSliderFill;
    [SerializeField] private TextMeshProUGUI m_TextTimeCount;
    [SerializeField] private CanvasGroup m_TimeCount;

    [SerializeField] private Phase m_CurrentPhase;
    [SerializeField] private List<Transform> m_PhaseList;
    [SerializeField] private Transform m_Border;

    [SerializeField] private CanvasGroup m_ButtonEndPhase;

    private Tween m_FadeTimeCountTween;
    private Tween m_MoveBoderTween;
    public override void Setup()
    {
        base.Setup();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangePhase(m_CurrentPhase);
        }
    }
    public void PlayerDrawCard(int count)
    {
        StartCoroutine(PlayerStartDraw(count));
    }
    IEnumerator PlayerStartDraw(int count)
    {
        for (int i = 0; i < count; i++)
        {
            m_PlayerDeck.DrawACard(m_PlayerHand.m_DropZone, -1);
            yield return new WaitForEndOfFrame();
        }
    }
    public void ShowOpponentCard()
    {
        for (int i = 0; i < m_OpponentSingleDropZones.Count; i++)
        {
            m_OpponentDeck.DrawACard(m_OpponentSingleDropZones[i].m_DropZone, 1);
        }
    }
    public void ShowAbilityOptionButton(bool value)
    {
        for (int i = 0; i < m_PlayerSingleDropZones.Count; i++)
        {
            m_PlayerSingleDropZones[i].ShowAbilityOptionButton(value);
        }
    }
    public void SetShowTimeCount(bool value, bool usingTween = true)
    {
    
        m_FadeTimeCountTween?.Kill();
        if (usingTween)
        {
            m_FadeTimeCountTween = m_TimeCount.DOFade(value ? 1 : 0, 0.5f);
        }
        else
        {
            m_TimeCount.alpha = value ? 1 : 0;  
        }
        
    }
    public void StartBattle()
    {

    }
    public void UpdateTime(float time, float fillBar)
    {
        m_TextTimeCount.text = time.ToString();
        m_ImageSliderFill.fillAmount = fillBar;
    }
    public void ChangePhase(Phase phase)
    {
        m_CurrentPhase = phase;
        m_MoveBoderTween?.Kill();
        m_MoveBoderTween = m_Border.DOMove(m_PhaseList[(int)m_CurrentPhase].position, 0.5f);
    }
    public void SetButtonEndPhase(bool value)
    {
        m_ButtonEndPhase.DOFade(value ? 1 : 0, 0.2f).OnComplete(() => m_ButtonEndPhase.interactable = value);
        m_ButtonEndPhase.blocksRaycasts = value;
    }
    public void OnClickButtonEndPhasePhase()
    {
        switch (m_CurrentPhase)
        {
            case Phase.SETUP_CARD:
                MatchManager.Instance.StartShowCardPhase();
                break;
            case Phase.SETUP_ABILITY:
                MatchManager.Instance.StartBattlePhase();
                break;
        }
        
    }
    public void AutoFillSingleDropZone()
    {
        for (int i = 0; i < m_PlayerSingleDropZones.Count; i++)
        {
            if (m_PlayerSingleDropZones[i].IsEmpty())
            {
                StartCoroutine(m_PlayerHand.GetRandomBasicCardInHand().m_CardController.MoveCardToDropZone(m_PlayerSingleDropZones[i].m_DropZone, null));
            }
        }
    }
}

public enum Phase
{
    DRAW = 0,
    SETUP_CARD = 1,
    SHOW_CARD = 2,
    SETUP_ABILITY = 3,
    BATTLE = 4,
    END_TURN = 5,
}
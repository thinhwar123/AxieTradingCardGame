using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MatchManager : Singleton<MatchManager>
{
    [Header("Match Config")]
    public float m_TimeThinking;
    private float m_CurrentTimeThinking;

    [Header("Player Match Data")]
    public List<PlayerMatchData> m_PlayerMatchData;

    [Header("BasicCard")]
    public List<BasicCard> m_BasicCards;

    private StateMachine<MatchManager> m_StateMachine;
    public StateMachine<MatchManager> StateMachine { get { return m_StateMachine; } }
    private UICIngame m_UICIngame;
    private bool m_StartCountTime;

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        m_StartCountTime = false;
        m_BasicCards = new List<BasicCard>();
        m_StateMachine = new StateMachine<MatchManager>(this);
        m_StateMachine.InitStartState(WaitState.Instance);
    }
    private void Start()
    {
        m_UICIngame = UI_Game.Instance.OpenUI<UICIngame>(UIID.UICIngame);
        DelayAction(StartDrawPhase, 1);
    }
    private void Update()
    {
        StateMachine.Update();
    }
    #endregion


    #region Phase Functions
    /// <summary>
    /// Wait State
    /// </summary>
    public void OnEnterWaitState()
    {

    }
    public void OnExecuteWaitState()
    {

    }
    public void OnExitWaitState()
    {

    }
    /// <summary>
    /// Draw State
    /// </summary>
    public void OnEnterDrawState()
    {
        m_UICIngame.PlayerDrawCard(8);        
        m_UICIngame.ChangePhase(Phase.DRAW);

        
        DelayAction(StartSetupCardPhase, 3);
    }
    public void OnExecuteDrawState()
    {


    }
    public void OnExitDrawState()
    {

    }
    /// <summary>
    /// Setup Card State
    /// </summary>
    public void OnEnterSetupCardState()
    {
        m_UICIngame.ChangePhase(Phase.SETUP_CARD);

        m_UICIngame.SetShowTimeCount(true);
        m_UICIngame.SetButtonEndPhase(true);


        m_StartCountTime = true;
        m_CurrentTimeThinking = m_TimeThinking;
        SetCanDragCard(true);
    }
    public void OnExecuteSetupCardState()
    {
        if (!m_StartCountTime) return;
        if (m_CurrentTimeThinking > 0)
        {
            m_CurrentTimeThinking = Mathf.Clamp(m_CurrentTimeThinking - Time.deltaTime, 0, m_TimeThinking);
            UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame).UpdateTime((int)m_CurrentTimeThinking, m_CurrentTimeThinking / m_TimeThinking);
        }
        else
        {
            m_StartCountTime = false;
            StartShowCardPhase();
        }
    }
    public void OnExitSetupCardState()
    {


    }
    /// <summary>
    /// Show Card State
    /// </summary>
    public void OnEnterShowCardState()
    {
        m_UICIngame.ChangePhase(Phase.SHOW_CARD);
        m_UICIngame.ShowOpponentCard();

        DelayAction(StartSetupAbilityPhase, 2f);
    }
    public void OnExecuteShowCardState()
    {

    }
    public void OnExitShowCardState()
    {

    }
    /// <summary>
    /// Setup Skill State
    /// </summary>
    public void OnEnterSetupSkillState()
    {
        m_UICIngame.ChangePhase(Phase.SETUP_ABILITY);
        m_UICIngame.SetShowTimeCount(true);
        m_UICIngame.SetButtonEndPhase(true);
        m_StartCountTime = true;
        m_CurrentTimeThinking = m_TimeThinking;
        m_UICIngame.ShowAbilityOptionButton(true);
    }
    public void OnExecuteSetupSkillState()
    {
        if (!m_StartCountTime) return;
        if (m_CurrentTimeThinking > 0)
        {
            m_CurrentTimeThinking = Mathf.Clamp(m_CurrentTimeThinking - Time.deltaTime, 0, m_TimeThinking);
            UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame).UpdateTime((int)m_CurrentTimeThinking, m_CurrentTimeThinking / m_TimeThinking);
        }
        else
        {
            m_StartCountTime = false;
            StartBattlePhase();
        }
    }
    public void OnExitSetupSkillState()
    {
        m_UICIngame.SetShowTimeCount(false);
        m_UICIngame.SetButtonEndPhase(false);
        m_UICIngame.ShowAbilityOptionButton(false);
    }
    /// <summary>
    /// Battle State
    /// </summary>
    public void OnEnterBattleState()
    {
        m_UICIngame.ChangePhase(Phase.BATTLE);
    }
    public void OnExecuteBattleState()
    {

    }
    public void OnExitBattleState()
    {

    }
    /// <summary>
    /// End Turn 
    /// </summary>
    public void OnEnterEndTurnState()
    {

    }
    public void OnExecuteEndTurnState()
    {

    }
    public void OnExitEndTurnState()
    {

    }
    #endregion

    #region Match Function
    public void StartDrawPhase()
    {
        StateMachine.ChangeState(DrawState.Instance);
    }
    public void StartSetupCardPhase()
    {
        StateMachine.ChangeState(SetupCardState.Instance);
    }
    public void StartShowCardPhase()
    {
        SetCanDragCard(false);

        m_UICIngame.SetShowTimeCount(false);
        m_UICIngame.SetButtonEndPhase(false);

        m_UICIngame.AutoFillSingleDropZone();
        DelayAction(() => StateMachine.ChangeState(ShowCardState.Instance), 0.5f);
    }
    public void StartSetupAbilityPhase()
    {
        StateMachine.ChangeState(SetupAbilityState.Instance);
    }
    public void StartBattlePhase()
    {
        StateMachine.ChangeState(BattleState.Instance);
    }
    public void StartCountTime()
    {
        m_StartCountTime = true;
    }
    public void SetCanDragCard(bool value)
    {
        for (int i = 0; i < m_BasicCards.Count; i++)
        {
            m_BasicCards[i].SetCanDrag(value);
        }
    }
    public void DelayAction(UnityAction action, float time)
    {
        StartCoroutine(CoDelayAction(action, time));
    }
    IEnumerator CoDelayAction(UnityAction action, float time)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
    #endregion
}
[System.Serializable]
public class PlayerMatchData
{
    public List<CardData> m_Deck;
    public List<CardData> m_SelectCard;
    public List<bool> m_SelectSkill;
}
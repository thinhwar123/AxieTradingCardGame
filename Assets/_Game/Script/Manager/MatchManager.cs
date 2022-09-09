using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MatchManager : Singleton<MatchManager>
{
    private PlayerHandler m_PlayerHandler;
    [Header("Match Config")]
    public float m_TimeThinking;
    private float m_CurrentTimeThinking;

    [Header("BasicCard")]
    public List<BasicCard> m_BasicCards;

    private StateMachine<MatchManager> m_StateMachine;
    public StateMachine<MatchManager> StateMachine { get { return m_StateMachine; } }
    public UICIngame m_UICIngame;
    public bool m_StartCountTime;
    public List<CardData> m_SavedDataDeskCard;

    #region Unity Functions
    protected override void Awake()
    {
        transform.SetParent(null);
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        m_SavedDataDeskCard = new List<CardData>();
        for (int i = 0; i < 24; i++)
        {
            m_SavedDataDeskCard.Add(CardDataManager.Instance.m_CardDatas[PlayerPrefs.GetInt(i.ToString(), i)]);
        }
        m_StartCountTime = false;
        m_BasicCards = new List<BasicCard>();
        m_StateMachine = new StateMachine<MatchManager>(this);
        m_StateMachine.InitStartState(WaitState.Instance);
    }
    //private void Start()
    //{
        //m_UICIngame = UI_Game.Instance.OpenUI<UICIngame>(UIID.UICIngame);
        //DelayAction(StartDrawPhase, 2);
   // }
    public void StartGame(PlayerHandler handler)
    {
        if (UI_Game.Instance.IsOpenedUI(UIID.UICTimeCount))
        {
            UI_Game.Instance.GetUI<UICTimeCount>(UIID.UICTimeCount).StopWaiting();
        }
        m_UICIngame = UI_Game.Instance.OpenUI<UICIngame>(UIID.UICIngame);
        m_UICIngame.m_PlayerDeck.InitDeck();
        TempData.Instance.InitNewData();
        this.m_PlayerHandler = handler;
        CreatePlayerMathData();
        DelayAction(StartDrawPhase, 2);
    }

    public void EndPhase(Phase curPhase)
    {
        switch (curPhase)
        {
            case Phase.SETUP_CARD:
                //m_StartCountTime = false;
                SetCanDragCard(false);
                m_PlayerHandler.NextPhase(Phase.SHOW_CARD);
                break;
            case Phase.SETUP_ABILITY:
                //m_StartCountTime = false;
                m_UICIngame.ShowAbilityOptionButton(false);
                m_PlayerHandler.NextPhase(Phase.BATTLE);
                break;
        }
    }

    private void Update()
    {
        StateMachine.Update();
    }
    #endregion

    public PlayerHandler GetPlayerHandler()
    {
        return this.m_PlayerHandler;
    }

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
        m_UICIngame.SetupRole(TempData.Instance.GetPlayerData().m_BattleRole);
        m_UICIngame.SetFadeDropZone(false);
        int cardDraw = (TempData.Instance.GetPlayerData().m_MaxCardInHand - m_UICIngame.m_PlayerHand.GetCardInHandCount());
        TempData.Instance.GetPlayerData().m_MaxCardInHand = 6;
        m_UICIngame.PlayerDrawCard(cardDraw < 0 ? 0 : cardDraw);        
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

        if (m_CurrentTimeThinking > 0)
        {
            m_CurrentTimeThinking = Mathf.Clamp(m_CurrentTimeThinking - Time.deltaTime, 0, m_TimeThinking);
            UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame).UpdateTime((int)m_CurrentTimeThinking, m_CurrentTimeThinking / m_TimeThinking);
        }
        else if(m_StartCountTime)
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
        if (m_CurrentTimeThinking > 0)
        {
            m_CurrentTimeThinking = Mathf.Clamp(m_CurrentTimeThinking - Time.deltaTime, 0, m_TimeThinking);
            UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame).UpdateTime((int)m_CurrentTimeThinking, m_CurrentTimeThinking / m_TimeThinking);
        }
        else if(m_StartCountTime)
        {
            m_StartCountTime = false;
            StartBattlePhase();
            //SetButtonEndPhase(false);
            //MatchManager.Instance.EndPhase(m_CurrentPhase);
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
        m_UICIngame.SetFadeDropZone(true);
        m_UICIngame.EndSetupRole();
        m_UICIngame.StartBattle();
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
        m_UICIngame.ChangePhase(Phase.END_TURN);
        m_UICIngame.ClearBattle();
        TempData.Instance.GetPlayerData().SwapRole();
        TempData.Instance.GetPlayerData().m_Round++;
        if (TempData.Instance.GetPlayerData().m_Round < 6)
        {
            DelayAction(StartDrawPhase, 2);
        }
        else
        {
            if (m_UICIngame.m_Score1 > m_UICIngame.m_Score2)
            {
                Debug.Log("Win Game");
                UI_Game.Instance.OpenUI<UI_Endgame>(UIID.UICEndGame).Setup(1);
            }
            else if (m_UICIngame.m_Score1 < m_UICIngame.m_Score2)
            {
                Debug.Log("Lose Game");
                UI_Game.Instance.OpenUI<UI_Endgame>(UIID.UICEndGame).Setup(-1);
            }
            else
            {
                Debug.Log("Draw Game");
                UI_Game.Instance.OpenUI<UI_Endgame>(UIID.UICEndGame).Setup(0);
            }

        }

    }
    public void OnExecuteEndTurnState()
    {

    }
    public void OnExitEndTurnState()
    {

    }
    #endregion

    #region Match Function
    public void CreatePlayerMathData()
    {
        PlayerMatchData playerMatchData = new PlayerMatchData();
        if (m_PlayerHandler.isClientOnly)
        {
            playerMatchData.m_BattleRole = BattleRole.DEFENDER;
        }
        
        TempData.Instance.AddPlayerMathData(playerMatchData);
        

    }
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
        StartCoroutine(CoStartShowCardPhase());
    }
    IEnumerator CoStartShowCardPhase()
    {
        SetCanDragCard(false);
        m_UICIngame.SetShowTimeCount(false);
        m_UICIngame.SetButtonEndPhase(false);
        m_UICIngame.AutoScaleCardToNormal();
        yield return new WaitForSeconds(0.5f);

        if (m_UICIngame.HasEmptyDropZone())
        {
            m_UICIngame.AutoFillSingleDropZone();
        }
        yield return new WaitForSeconds(1);
        TempData.Instance.GetPlayerData().m_SelectCard = m_UICIngame.GetSelectCardData();
        m_PlayerHandler.SetUpMatchData(TempData.Instance.GetPlayerData());
        DelayAction(() => StateMachine.ChangeState(ShowCardState.Instance), 0.5f);
    }
    public void StartSetupAbilityPhase()
    {
        StateMachine.ChangeState(SetupAbilityState.Instance);
    }
    public void StartBattlePhase()
    {

        m_UICIngame.AutoScaleCardToNormal();
        TempData.Instance.GetPlayerData().m_SelectSkill = m_UICIngame.GetSelectSkill();
        m_PlayerHandler.SetUpMatchData(TempData.Instance.GetPlayerData());
        DelayAction(() => StateMachine.ChangeState(BattleState.Instance), 0.5f);
    }
    public void StartEndPhase()
    {
        StateMachine.ChangeState(EndTurnState.Instance);
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
    public bool IsPlayerCard(BasicCard basicCard)
    {
        List<CardData> cardDatas = m_UICIngame.GetSelectCardData();
        for (int i = 0; i < cardDatas.Count; i+=2)
        {
            if (cardDatas[i].m_ID == basicCard.GetID())
            {
                return true;
            }
        } 
        return false;
    }
    #endregion
}
[System.Serializable]
public class PlayerMatchData
{
    public int m_MaxCardInHand;
    public int m_Round;
    public BattleRole m_BattleRole;
    public List<CardData> m_Deck;
    public List<CardData> m_SelectCard;
    public List<bool> m_SelectSkill;

    public PlayerMatchData()
    {
        m_Round = 0;
        m_MaxCardInHand = 6;
        m_BattleRole = BattleRole.ATTACKER;
        m_Deck = new List<CardData>();
        m_SelectCard = new List<CardData>();
        m_SelectSkill = new List<bool>();
    }
    public void SwapRole()
    {
        switch (m_BattleRole)
        {
            case BattleRole.ATTACKER:
                m_BattleRole = BattleRole.DEFENDER;
                break;
            case BattleRole.DEFENDER:
                m_BattleRole = BattleRole.ATTACKER;
                break;

        }
    }
}
public enum BattleRole
{
    ATTACKER,
    DEFENDER,
}

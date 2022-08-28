using AxieMixer.Unity;
using Newtonsoft.Json.Linq;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class BasicCard : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [Header("Reference")]
    public CardController m_CardController;
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private RectTransform m_CardGraphic;
    [SerializeField] private TextMeshProUGUI m_TextCardName;
    [SerializeField] private Image m_ImageAbilityIcon1;
    [SerializeField] private TextMeshProUGUI m_TextAbilityName1;

    [SerializeField] private RectTransform m_RectAbilityDecription;
    [SerializeField] private CanvasGroup m_CanvasGroupAbilityDecription;
    [SerializeField] private Image m_ImageAbilityPopupBG;
    [SerializeField] private Image m_ImageAbilityIcon2;
    [SerializeField] private TextMeshProUGUI m_TextAbilityName2;
    [SerializeField] private TextMeshProUGUI m_TextAbilityDescription;

    [SerializeField] private GameObject m_CardShirt;

    [SerializeField] private List<CanvasGroup> m_ListImageCardBackground;
    [SerializeField] private List<Sprite> m_ListImageCardAbilityIcon;
    [SerializeField] private List<Color> m_ListColorAbilityPopupBG;



    [Header("Config")]

    [SerializeField] private string m_AxieID;
    [SerializeField] private string m_AxieName;
    [SerializeField] private float m_Scale;
    [SerializeField] private Symbol m_Symbol;

    [SerializeField] private AbilityType m_AbilityType;
    [SerializeField] private string m_AbilityName;
    [SerializeField] private string m_AbilityDescription;

    [SerializeField] private string m_MoveAnimation;
    [SerializeField] private string m_MeleeAnimation;
    [SerializeField] private string m_RangeAnimation;
    [SerializeField] private string m_DefenceAnimation;

    public CardData m_CardData { get; private set; }

    public bool m_IsSilent;
    public bool m_IsFlipped { get; private set; }
    private const bool USE_GRAPHIC = true;
    public UnityAction m_OnWinBattleCallback;
    public UnityAction m_OnLoseBattleCallback;
    private int m_CardLookDirection;
    private SkeletonGraphic m_SkeletonGraphic;
    private List<Tween> m_Tweens;
    private static string m_ActiveDescription = "- Active: The player can choose to use this ability or not after flipping the cards.";
    private static string m_PassiveDescription = "- Passive: This ability activates automatically after the card is flipped.";

    #region Unity & Init Functions
    private void Awake()
    {
        m_Tweens = new List<Tween>();
    }
    private void Start()
    {
        // TODO: Test
        //RandomCardConfig();
        //Setup(false);
        //InitCard();
    }
    public void Setup(bool isFlipped)
    {
        m_IsFlipped = isFlipped;
    }
    public void InitCard(int cardLookDirection)
    {
        m_CardController.InitCardController();

        for (int i = 0; i < m_ListImageCardBackground.Count; i++)
        {
            m_ListImageCardBackground[i].alpha = (i == (int)m_Symbol && m_IsFlipped) ? 1 : 0;
        }

        m_CardShirt.SetActive(!m_IsFlipped);
        m_TextCardName.text = string.Format(m_AxieName);

        Sprite icon = m_ListImageCardAbilityIcon[((int)m_Symbol * 2 + (int)m_AbilityType)];
        m_ImageAbilityIcon1.sprite = icon;
        m_ImageAbilityIcon2.sprite = icon;

        m_TextAbilityName1.text = m_AbilityName;
        m_TextAbilityName2.text = string.Format("{0} - {1}",m_AbilityType == AbilityType.ACTIVE ? "Active" : "Passive",m_AbilityName);
        m_TextAbilityDescription.text = string.Format("{0}\n\n{1}: {2}", m_AbilityType == AbilityType.ACTIVE ? m_ActiveDescription : m_PassiveDescription, m_AbilityName, m_AbilityDescription);
        m_ImageAbilityPopupBG.color = m_ListColorAbilityPopupBG[(int)m_Symbol];
        m_CardGraphic.localScale = new Vector3(cardLookDirection, 1, 1);
        GenerateCardGraphic();


        m_IsSilent = false;
        m_CanvasGroup.alpha = 1;
        m_CardLookDirection = cardLookDirection;
        m_OnWinBattleCallback = null;
        m_OnLoseBattleCallback = null;
    }

    #endregion

    #region Card Logic Functions
    public void ShowAbilityInfor(bool value)
    {

        KillAllTween();
        if (value)
        {
            m_Tweens.Add(m_CanvasGroupAbilityDecription.DOFade(1, 0.5f));
            m_Tweens.Add(m_RectAbilityDecription.DOLocalMoveX(275, 0.5f));
        }
        else
        {
            m_Tweens.Add(m_CanvasGroupAbilityDecription.DOFade(0, 0.5f));
            m_Tweens.Add(m_RectAbilityDecription.DOLocalMoveX(0, 0.5f));
        }
    }
    public void SetupCardConfig(CardData cardData)
    {
        m_CardData = cardData;
        m_AxieID = cardData.m_ID;
        m_AxieName = cardData.m_Name;
        m_Symbol = cardData.m_Symbol;
        m_AbilityType = cardData.m_AbilityType;
        m_AbilityName = cardData.m_Archetype;
        m_AbilityDescription = cardData.m_EffectDescription;
        m_MoveAnimation = "action/run";
        m_MeleeAnimation = cardData.m_MeleeAnimation;
        m_RangeAnimation = cardData.m_RangeAnimation;
        m_DefenceAnimation = cardData.m_DefenceAnimation;
    }

    public string GetID()
    {
        return m_AxieID;
    }
    
    public void RandomCardConfig()
    {
        CardData randomData = CardDataManager.Instance.m_CardDatas[Random.Range(0, CardDataManager.Instance.m_CardDatas.Count)];
        SetupCardConfig(randomData);
    }
    public void ResetCard()
    {
        if (m_SkeletonGraphic != null)
        {
            Destroy(m_SkeletonGraphic.gameObject);
        }
        m_SkeletonGraphic = null;
    }
    public void FlipCard(bool isFlipped)
    {
        if (m_IsFlipped == isFlipped) return;
        m_IsFlipped = isFlipped;
        Transform.DOScaleX(0, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
        {
            for (int i = 0; i < m_ListImageCardBackground.Count; i++)
            {
                m_ListImageCardBackground[i].alpha = (i == (int)m_Symbol && m_IsFlipped) ? 1 : 0;
            }
            m_CardShirt.SetActive(!m_IsFlipped);
        });
        Transform.DOScaleX(1, 0.3f).SetEase(Ease.OutSine).SetDelay(0.3f);
    }
    public void SetCanDrag(bool value)
    {
        m_CardController.SetCanDrag(value);
    }
    public void KillAllTween()
    {
        for (int i = 0; i < m_Tweens.Count; i++)
        {
            m_Tweens[i]?.Kill();
        }
        m_Tweens.Clear();
    }
    public void ActiveAbility()
    {
        StartCoroutine(CoActiveSkill());
    }
    IEnumerator CoActiveSkill()
    {
        if (m_IsSilent)
        {
            m_SkeletonGraphic.AnimationState.SetAnimation(0, "battle/get-debuff", false);
        }
        else
        {
            m_SkeletonGraphic.AnimationState.SetAnimation(0, "battle/get-buff", false);
            UIEffectManager.Instance.PlayEffect(m_AbilityName, Transform, Transform.position);
            switch (m_AbilityName)
            {
                case "Swap Back":                    
                    SwapBack();
                    break;
                case "Twin Power":
                    TwinPower();
                    break;
                case "Breaker":
                    Breaker();
                    break;
                case "Card Maker":
                    CardMaker();
                    break;
                case "Card Eater":
                    CardEater();
                    break;
                case "Silencer":
                    Silencer();
                    break;
                case "Surprise":
                    Surprise();
                    break;
                case "Crusader":
                    Crusader();
                    break;
                case "Two Face":
                    TwoFace();
                    break;                    
                case "Slime Power":
                    SlimePower();
                    break;
            }
        }
        yield return new WaitForSeconds(2);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);

    }

   
    #endregion

    #region Axie Graphic Generate Functions
    public void GenerateCardGraphic()
    {
        if (AxieMixerManager.Instance.HasAxie2dBuilderResult(m_AxieID))
        {
            SpawnSkeletonGraphic(AxieMixerManager.Instance.GetAxie2DBuilderResult(m_AxieID));
        }
        else
        {
            StartCoroutine(GetAxiesGenes(m_AxieID));
        }
    }
    public IEnumerator GetAxiesGenes(string axieId)
    {
        string searchString = "{ axie (axieId: \"" + axieId + "\") { id, genes, newGenes}}";
        JObject jPayload = new JObject();
        jPayload.Add(new JProperty("query", searchString));

        var wr = new UnityWebRequest("https://graphql-gateway.axieinfinity.com/graphql", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jPayload.ToString().ToCharArray());
        wr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        wr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        wr.SetRequestHeader("Content-Type", "application/json");
        wr.timeout = 10;
        yield return wr.SendWebRequest();
        if (wr.error == null)
        {
            var result = wr.downloadHandler != null ? wr.downloadHandler.text : null;
            if (!string.IsNullOrEmpty(result))
            {
                JObject jResult = JObject.Parse(result);
                string genesStr = (string)jResult["data"]["axie"]["newGenes"];
                ProcessMixer(axieId, genesStr, m_Scale, USE_GRAPHIC);
            }
        }
    }
    private void ProcessMixer(string axieId, string genesStr, float scale, bool isGraphic)
    {
        if (string.IsNullOrEmpty(genesStr))
        {
            Debug.LogError($"[{axieId}] genes not found!!!");
            return;
        }
        Axie2dBuilderResult builderResult = Mixer.Builder.BuildSpineFromGene(axieId, genesStr, scale, isGraphic);
        AxieMixerManager.Instance.AddAxie2dBuilderResult(axieId, builderResult);
        SpawnSkeletonGraphic(builderResult);
    }
    private void SpawnSkeletonGraphic(Axie2dBuilderResult builderResult)
    {

        m_SkeletonGraphic = SkeletonGraphic.NewSkeletonGraphicGameObject(builderResult.skeletonDataAsset, m_CardGraphic, builderResult.sharedGraphicMaterial);
        m_SkeletonGraphic.rectTransform.sizeDelta = new Vector2(1, 1);
        m_SkeletonGraphic.rectTransform.localScale = Vector3.one;
        m_SkeletonGraphic.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_SkeletonGraphic.Initialize(true);
        m_SkeletonGraphic.Skeleton.SetSkin("default");
        m_SkeletonGraphic.Skeleton.SetSlotsToSetupPose();
        
        m_SkeletonGraphic.gameObject.AddComponent<AutoBlendAnimGraphicController>();
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);

        if (builderResult.adultCombo.ContainsKey("body") &&
         builderResult.adultCombo["body"].Contains("mystic") &&
         builderResult.adultCombo.TryGetValue("body-class", out var bodyClass) &&
         builderResult.adultCombo.TryGetValue("body-id", out var bodyId))
        {
            m_SkeletonGraphic.gameObject.AddComponent<MysticIdGraphicController>().Init(bodyClass, bodyId);
        }

    }

    #endregion

    #region Ability Functions
    public void SwapBack()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<SingleDropZone> singleDropZones = uicIngame.GetSelectSingleDropZoneByTurn();
        for (int i = 0; i < singleDropZones.Count - 2; i++)
        {
            if (singleDropZones[i].GetBasicCard() == this)
            {
                BasicCard tempCard = singleDropZones[i + 2].GetBasicCard();
                StartCoroutine(m_CardController.MoveCardToDropZone(singleDropZones[i + 2].m_DropZone, null));
                StartCoroutine(tempCard.m_CardController.MoveCardToDropZone(singleDropZones[i].m_DropZone, null));
                break;
            }
        }
    }
    public void TwinPower()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        for (int i = index%2; i < basicCards.Count; i+=2)
        {
            if (basicCards[i] != this && basicCards[i].m_AxieID == m_AxieID)
            {
                ChangeSymbol(GetWinSymbol(m_Symbol));
            }
        }
    }
    public void Breaker()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        int maxRate = 0;
        for (int i = (index + 1) % 2; i < basicCards.Count; i += 2)
        {
            if (basicCards[i].m_Symbol == m_Symbol)
            {
                maxRate++;
            }
        }
        if (TempData.Instance.m_Random.Next() % 3 + 1 < maxRate)
        {
            ChangeSymbol(GetWinSymbol(m_Symbol));
        }
    }
    public void CardMaker()
    {
        if (MatchManager.Instance.IsPlayerCard(this))
        {
            m_OnWinBattleCallback = () =>
            {
                TempData.Instance.GetPlayerData().m_MaxCardInHand = 8;
            };
        }
    }
    public void CardEater()
    {
        if (!MatchManager.Instance.IsPlayerCard(this))
        {
            m_OnWinBattleCallback = () =>
            {
                TempData.Instance.GetPlayerData().m_MaxCardInHand = 4;
            };
        }
    }
    public void Silencer()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        BasicCard opponentCard = basicCards[index + (index % 2 == 0 ? 1 : -1)];
        opponentCard.m_IsSilent = true;
    }
    public void Surprise()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        BasicCard opponentCard = basicCards[index + (index % 2 == 0 ? 1 : -1)];
        if (opponentCard.m_Symbol == GetWinSymbol(m_Symbol))
        {
            opponentCard.ChangeSymbol(GetRandomSymbol());
        }

    }
    public void Crusader()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        BasicCard opponentCard = basicCards[index + (index % 2 == 0 ? 1 : -1)];
        if (opponentCard.m_Symbol == GetWinSymbol(m_Symbol))
        {
            ChangeSymbol(GetRandomSymbol());
        }
    }
    public void TwoFace()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        BasicCard opponentCard = basicCards[index + (index % 2 == 0 ? 1 : -1)];
        if (TempData.Instance.m_Random.Next() % 2 == 1)
        {
            if (opponentCard.m_Symbol == GetWinSymbol(m_Symbol))
            {
                ChangeSymbol(GetWinSymbol(opponentCard.m_Symbol));
            }
            else if (opponentCard.m_Symbol == GetLoseSymbol(m_Symbol))
            {
                ChangeSymbol(GetLoseSymbol(opponentCard.m_Symbol));
            }
        }
    }
    public void SlimePower()
    {
        UICIngame uicIngame = UI_Game.Instance.GetUI<UICIngame>(UIID.UICIngame);
        List<BasicCard> basicCards = uicIngame.GetSelectBasicCardByTurn();
        int index = basicCards.IndexOf(this);
        BasicCard opponentCard = basicCards[index + (index % 2 == 0 ? 1 : -1)];

        int playerScore = MatchManager.Instance.IsPlayerCard(this) ? uicIngame.m_Score1 : uicIngame.m_Score2;
        int opponentScore = MatchManager.Instance.IsPlayerCard(this) ? uicIngame.m_Score2 : uicIngame.m_Score1;
        //if (playerScore + 2 < opponentScore)
        //{
        //    ChangeSymbol(GetWinSymbol(opponentCard.m_Symbol));
        //}
        m_SkeletonGraphic.skeletonDataAsset = AxieMixerManager.Instance.m_SlimeSkeletonDataAsset;
        m_SkeletonGraphic.rectTransform.sizeDelta = new Vector2(1, 1);
        m_SkeletonGraphic.rectTransform.localScale = Vector3.one;
        m_SkeletonGraphic.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_SkeletonGraphic.material = null;
        m_SkeletonGraphic.Initialize(true);
        m_SkeletonGraphic.Skeleton.SetSkin("default");
        m_SkeletonGraphic.Skeleton.SetSlotsToSetupPose();
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);
        m_MoveAnimation = AxieMixerManager.Instance.slimeMoveAnimation;
        m_MeleeAnimation = AxieMixerManager.Instance.slimeMeleeAnimation;
        m_RangeAnimation = AxieMixerManager.Instance.slimeRangeAnimation;
        m_DefenceAnimation = AxieMixerManager.Instance.slimeDefenceAnimation;
        ChangeSymbol(GetWinSymbol(opponentCard.m_Symbol));
    }
    public void ChangeSymbol(Symbol symbol)
    {
        Debug.Log("ChangeSymbol");
        m_Symbol = symbol;

        Sprite icon = m_ListImageCardAbilityIcon[((int)m_Symbol * 2 + (int)m_AbilityType)];
        m_ImageAbilityIcon1.sprite = icon;
        m_ImageAbilityIcon2.sprite = icon;

        for (int i = 0; i < m_ListImageCardBackground.Count; i++)
        {
            m_ListImageCardBackground[i].DOFade((i == (int)m_Symbol && m_IsFlipped) ? 1 : 0, 2);
        }
    }
    public Symbol GetRandomSymbol()
    {
         return (Symbol) (TempData.Instance.m_Random.Next()%3);
    }
    public Symbol GetWinSymbol(Symbol symbol)
    {
        switch (symbol)
        {
            case Symbol.ROCK:
                return Symbol.PAPPER;
            case Symbol.PAPPER:
                return Symbol.SCISSORS;
            case Symbol.SCISSORS:
                return Symbol.ROCK;
        }
        return Symbol.ROCK;
    }
    public Symbol GetLoseSymbol(Symbol symbol)
    {
        switch (symbol)
        {
            case Symbol.ROCK:
                return Symbol.SCISSORS;
            case Symbol.PAPPER:
                return Symbol.ROCK;
            case Symbol.SCISSORS:
                return Symbol.PAPPER;
        }
        return Symbol.ROCK;
    }
    #endregion

    #region Combat Functions
    public void MeleeBattle(BasicCard basicCard)
    {
        Vector3 pos = (Transform.position + basicCard.transform.position) /2;
        m_SkeletonGraphic.rectTransform.DOMoveX((pos + m_CardLookDirection * Vector3.right).x, 1);

        if (GetLoseSymbol(m_Symbol) == basicCard.m_Symbol)
        {
            StartCoroutine(OnMeleeAttack());
            m_OnWinBattleCallback?.Invoke();
        }
        else if (GetWinSymbol(m_Symbol) == basicCard.m_Symbol)
        {
            StartCoroutine(OnMeleeHit());
        }
        else
        {
            StartCoroutine(OnMeleeAttack());
        }
    }
    IEnumerator OnMeleeAttack()
    {
        m_SkeletonGraphic.transform.SetParent( UI_Game.Instance.CanvasParentTF);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, m_MoveAnimation, true);
        m_CanvasGroup.DOFade(0, 0.5f);
        yield return new WaitForSeconds(1f);
        
        m_SkeletonGraphic.AnimationState.SetAnimation(0, m_MeleeAnimation, false);
        yield return new WaitForSeconds(1.5f);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);
        yield return new WaitForSeconds(0.5f);

        DestroyCard();
    }
    IEnumerator OnMeleeHit()
    {
        m_SkeletonGraphic.transform.SetParent(UI_Game.Instance.CanvasParentTF);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, m_MoveAnimation, true);
        m_CanvasGroup.DOFade(0, 0.5f);
        yield return new WaitForSeconds(1f);
        
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);
        yield return new WaitForSeconds(0.5f);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, m_DefenceAnimation, false);
        yield return new WaitForSeconds(1.5f);
        DestroyCard();
    }
    public void RangeBattle(BasicCard basicCard)
    {

        if (GetLoseSymbol(m_Symbol) == basicCard.m_Symbol)
        {
            StartCoroutine(OnRangeAttack(basicCard));
            m_OnWinBattleCallback?.Invoke();
        }
        else if (GetWinSymbol(m_Symbol) == basicCard.m_Symbol)
        {
            StartCoroutine(OnRangeHit());
        }
        else
        {
            StartCoroutine(OnRangeAttack(basicCard));
        }
    }
    IEnumerator OnRangeAttack(BasicCard basicCard)
    {
        m_SkeletonGraphic.transform.SetParent(UI_Game.Instance.CanvasParentTF);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, m_RangeAnimation, true);
        m_CanvasGroup.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Thinh.SimplePool.Spawn(UIProjectilesManager.Instance.GetUIProjectile(m_AbilityName)).Setup(Transform.position, basicCard.Transform.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);
        yield return new WaitForSeconds(2f);

        DestroyCard();
    }
    IEnumerator OnRangeHit()
    {
        m_SkeletonGraphic.transform.SetParent(UI_Game.Instance.CanvasParentTF);
        m_CanvasGroup.DOFade(0, 0.5f);
        yield return new WaitForSeconds(1f);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, m_DefenceAnimation, false);
        yield return new WaitForSeconds(0.5f);
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);
        yield return new WaitForSeconds(0.5f);
        DestroyCard();
    }
    public void DestroyCard()
    {
        Destroy(m_SkeletonGraphic.gameObject);
        m_CardController.DespawnCardSlot();
        Thinh.SimplePool.Despawn(gameObject);
    }
    #endregion
}


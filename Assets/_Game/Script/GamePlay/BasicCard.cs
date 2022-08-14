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

public class BasicCard : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [Header("Reference")]
    public CardController m_CardController;
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

    [SerializeField] private List<GameObject> m_ListImageCardBackground;
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

    [SerializeField] private List<string> m_WinAnimation;
    [SerializeField] private List<string> m_LoseAnimation;

    public CardData m_CardData { get; private set; }

    public bool m_IsFlipped { get; private set; }
    private const bool USE_GRAPHIC = true;
    private SkeletonGraphic skeletonGraphic;
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
            m_ListImageCardBackground[i].SetActive(i == (int)m_Symbol && m_IsFlipped);
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
        m_WinAnimation = cardData.m_WinAnimation;
        m_LoseAnimation = cardData.m_LoseAnimation;
    }
    public void RandomCardConfig()
    {
        CardData randomData = CardDataManager.Instance.m_CardDatas[Random.Range(0, CardDataManager.Instance.m_CardDatas.Count)];
        SetupCardConfig(randomData);
    }
    public void ResetCard()
    {
        if (skeletonGraphic != null)
        {
            Destroy(skeletonGraphic.gameObject);
        }
        skeletonGraphic = null;
    }
    public void FlipCard(bool isFlipped)
    {
        if (m_IsFlipped == isFlipped) return;
        m_IsFlipped = isFlipped;
        Transform.DOScaleX(0, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
        {
            for (int i = 0; i < m_ListImageCardBackground.Count; i++)
            {
                m_ListImageCardBackground[i].SetActive(i == (int)m_Symbol && m_IsFlipped);
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

        skeletonGraphic = SkeletonGraphic.NewSkeletonGraphicGameObject(builderResult.skeletonDataAsset, m_CardGraphic, builderResult.sharedGraphicMaterial);
        skeletonGraphic.rectTransform.sizeDelta = new Vector2(1, 1);
        skeletonGraphic.rectTransform.localScale = Vector3.one;
        skeletonGraphic.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        skeletonGraphic.Initialize(true);
        skeletonGraphic.Skeleton.SetSkin("default");
        skeletonGraphic.Skeleton.SetSlotsToSetupPose();

        skeletonGraphic.gameObject.AddComponent<AutoBlendAnimGraphicController>();
        skeletonGraphic.AnimationState.SetAnimation(0, "action/idle/normal", true);

        if (builderResult.adultCombo.ContainsKey("body") &&
         builderResult.adultCombo["body"].Contains("mystic") &&
         builderResult.adultCombo.TryGetValue("body-class", out var bodyClass) &&
         builderResult.adultCombo.TryGetValue("body-id", out var bodyId))
        {
            skeletonGraphic.gameObject.AddComponent<MysticIdGraphicController>().Init(bodyClass, bodyId);
        }
    }

    #endregion


}


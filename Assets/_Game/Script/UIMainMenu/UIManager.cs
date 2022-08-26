using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Thinh;
using System.Linq;
using Mirror;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider loadingBar;
    public float speedLoad;
    public float fill;
    public bool isLoading;    
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingMatchText;
    public GameObject loadingPanel;
    public GameObject mainMenuPanel;
    public GameObject WaitJoin;
    //public Button soundSetting;
    public Button soundOnOff;
    public Sprite soundOn;
    public Sprite soundOff;
    public List<Sprite> listAvatars = new List<Sprite>();
    public int indexAvatar = 0;
    public Image avatarMain;
    public Image avatarEnemy;
    public Image avatar;
    public bool isSoundOn;
    public GameObject buildScreen;
    public GameObject matchPanel;
    public BasicCard card;
    public RectTransform allCards;
    public RectTransform pool;
    public List<BasicCard> listCards = new List<BasicCard>();
    public List<BasicCard> listCopyCards = new List<BasicCard>();
    public List<BasicCard> listDeckCards { get => PlayerData.Instance.m_ListCardDatas; }
    public List<CardData> listCardDatas = new List<CardData>();
    public static bool mainMenu = true;

    [SerializeField] private BasicCard m_BasicCard;
    [SerializeField] private Transform m_StartSpawnPosition;
    [SerializeField] private DropZone m_AllCard;
    [SerializeField] private DropZone m_AllCopyCard;
    public DropZone buildSpace;
    public DropZone deckSpace;

    public TextMeshProUGUI popupFullCardText;
    public TextMeshProUGUI popupCanHostOrJoin;
    public TextMeshProUGUI numberOfCards;
    public TextMeshProUGUI timeWait;

    [Scene]
    public string onlineScene;

    //public bool goGame = false;
    public bool isFirstTime = true;
    public bool isCanStart = true;
    public bool isWaiting = false;
    private int second;
    private int minute;
    private float countTime = 0f;
    public bool isServer = false;    

    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;

    public ServerClient serverClient;

    //network
    public AxieNetworkManager networkManager;
    void Start()
    {
        isLoading = true;
        InitAllCard();
        InitAllCopyCard();
        //goGame = false;
        isCanStart = true;
        isWaiting = false;
        second = 0;
        minute = 0;
        countTime = 0f;
        isServer = false;
        serverClient.goGame = false;
        LoadCardData();
        //ResetDeck();
        //LoadDeck();
        //GenAllCard();
        //isLoadingMatch = true;
        //ShowDeckInfo();
    }

    //public void ShowDeckInfo()
    //{
    //    if(listCardDatas.Count == 0)
    //    {
    //        Debug.Log("No Data");
    //        return;
    //    }

    //    for (int i = 0; i < listCardDatas.Count; i++)
    //    {
    //        Debug.Log("number " + i.ToString() + ":" + listCardDatas[i].m_ID);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SceneManager.LoadScene(2);
        //}

        if (isLoading)
        {
            LoadingGame();
        }

        //if (isWaiting)
        //{
        //    TimeCount();
        //}

        //if ((networkManager.playerHandlers.Count == networkManager.maxConnections || serverClient.goGame) && isCanStart)
        //{
        //    //CmdGoGame();
        //    Debug.Log("GoGoGo");
        //    if (isServer)
        //    {
        //        serverClient.GoGame();
        //        serverClient.AvatarEnemyClient(indexAvatar);
        //        //networkManager.ServerChangeScene(onlineScene);
        //    }
        //    else
        //    {
        //        serverClient.AvatarEnemyServer(indexAvatar);
        //    }
        //    avatarMain.sprite = avatar.sprite;
        //    isWaiting = false;
        //    isCanStart = false;
        //    mainMenu = false;
        //    matchPanel.SetActive(true);
        //    StartCoroutine(LoadingMatch());
        //    //networkManager.ServerChangeScene(onlineScene);
        //}
    }

    //[ClientRpc]
    //public void CmdGoGame()
    //{
    //    goGame = true;
    //}

    public void TimeCount()
    {
        countTime += Time.deltaTime;
        if (countTime > second + 1 && countTime < second + 2)
        {
            second++;
            if (second >= 60)
            {
                minute++;
                second = 0;
                countTime = 0f;
            }
        }
        string min = "";
        string sec = "";

        if (minute < 10)
        {
            min = "0" + minute.ToString();
        }
        else
        {
            min = minute.ToString();
        }

        if (second < 10)
        {
            sec = "0" + second.ToString();
        }
        else
        {
            sec = second.ToString();
        }

        timeWait.text = min + " : " + sec;
    }
    //public void Host()
    //{
    //    networkManager.StartHostClient();
    //}

    public void LoadingGame()
    {
        //isLoading = false;

        fill += speedLoad;        

        if(fill < 1f)
        {
            loadingBar.value = fill;
            loadingText.text = ((int)(100 * fill)).ToString() + "%";
        }
        else
        {
            isLoading = false;
            Invoke("EndLoading", 1);
            SwitchToBuildSpace();
            LoadDeck();
        }
    }

    public void EndLoading()
    {
        //yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        //SwitchToBuildSpace();
    }

    public void SoundSetting()
    {
        //soundOnOff.gameObject.SetActive(!soundOnOff.gameObject.activeInHierarchy);
        popupCanHostOrJoin.text = "";
        StartCoroutine(Effect01(!soundOnOff.gameObject.activeInHierarchy, soundOnOff.gameObject));
    }

    IEnumerator Effect01(bool active, GameObject obj)
    {
        if (active)
        {
            obj.transform.localScale = Vector3.zero;
            obj.SetActive(true);
            obj.transform.DOScale(1f, 0.2f);
        }
        else
        {
            //btn.transform.localScale = Vector3.zero;            
            obj.transform.DOScale(0f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            obj.SetActive(false);
        }
    }

    public void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if(fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = canvasGroup.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    public IEnumerator Effect02(bool active, GameObject obj)
    {
        if (active)
        {
            //FadeOut(0.1f);
            //yield return new WaitForSeconds(0.1f);
            obj.SetActive(true);
            FadeIn(0.2f);
        }
        else
        {
            FadeOut(0.2f);
            yield return new WaitForSeconds(0.2f);
            obj.SetActive(false);
        }
    }

    public void SoundOnOff()
    {
        popupCanHostOrJoin.text = "";
        if (isSoundOn)
        {
            isSoundOn = false;
            soundOnOff.image.sprite = soundOff;
            // Turn Off //
        }
        else
        {
            isSoundOn = true;
            soundOnOff.image.sprite = soundOn;
            // Turn On //
        }
    }

    public void Join()
    {
        // Code //

        //mainMenu = false;

        //matchPanel.SetActive(true);
        popupCanHostOrJoin.text = "";
        if (IsDeckBuilded())
        {
            mainMenu = false;
            networkManager.StartClient();
            //serverClient.AvatarEnemyServer(indexAvatar);
        }
        else
        {
            popupCanHostOrJoin.text = "You must build deck first";
        }

        //networkManager.StartClient();
        //isWaiting = true;
        //minute = 0;
        //second = 0;
        //countTime = 0f;
        //WaitJoin.SetActive(true);

        //StartCoroutine(LoadingMatch());

        //networkManager.StartClient();

        //SceneManager.LoadScene(0);

    }

    public void Host()
    {
        //networkManager.StartServer();
        //networkManager.StartHost();
        popupCanHostOrJoin.text = "";
        if (IsDeckBuilded())
        {
            isServer = true;
            networkManager.StartHost();
            isWaiting = true;
            minute = 0;
            second = 0;
            countTime = 0f;
            //serverClient.goGame = true;
            WaitJoin.SetActive(true);
        }
        else
        {
            popupCanHostOrJoin.text = "You must build deck first";
        }
    }

    public bool IsDeckBuilded()
    {
        if (listCardDatas.Count == 24) return true;
        return false;
    }
    //public IEnumerator LoadingMatch()
    //{
    //    //for (int i = 0; i <= 100; i++)
    //    //{
    //    //    loadingMatchText.text = i.ToString() + "%";
    //    //    yield return new WaitForSeconds(0.2f);
    //    //}
    //    //SceneManager.LoadScene(0);

    //    AsyncOperation op = SceneManager.LoadSceneAsync(1);

    //    op.allowSceneActivation = false;

    //    int i = 0;

    //    while (!op.isDone)
    //    {
    //        if (i < 100) 
    //        {
    //            loadingMatchText.text = i.ToString() + "%";
    //            i++;
    //            yield return new WaitForSeconds(0.05f);
    //            if(i == 100)
    //            {
    //                if (isServer)
    //                {
    //                    serverClient.FinishLoadingClient();
    //                }
    //                else
    //                {
    //                    serverClient.FinishLoadingServer();
    //                }
    //            }
    //        }
    //        //else if(isServer)
    //        //{
    //        //    serverClient.FinishLoadingClient();
    //        //}
    //        //else
    //        //{
    //        //    serverClient.FinishLoadingServer();
    //        //}

    //        if(op.progress >= 0.9f && !op.allowSceneActivation && i == 100 && serverClient.isFinishLoadingMatch)
    //        {
    //            if (isServer)
    //            {
    //                for(int k = 0; k < networkManager.maxConnections; k++)
    //                {
    //                    networkManager.playerHandlers[i].StartGame();
    //                }
    //            }
    //            op.allowSceneActivation = true;
    //        }

    //        yield return null;
    //    }

    //    //if (isServer)
    //    //{
    //    //    networkManager.ServerChangeScene(onlineScene);
    //    //}
    //    //else yield return null;
    //}

    public void BuildDeck()
    {
        popupCanHostOrJoin.text = "";
        if (!buildScreen.activeInHierarchy)
        {
            //buildScreen.transform.localScale = new Vector3(1f, 1f, 1f);
            //buildScreen.SetActive(true);
            //StartCoroutine(Effect01(true, buildScreen));
            //buildScreen.SetActive(true);
            StartCoroutine(Effect02(true, buildScreen));
            CardsActive(true);
            //InitAllCard();
            //ShowAllCard();
            //GenAllCard();
            //ShowAllCard();
        }
    }

    public void CardsActive(bool active)
    {
        foreach(var card in listCards)
        {
            card.gameObject.SetActive(active);
        }
        foreach(var card in listDeckCards)
        {
            card.gameObject.SetActive(active);
        }
    }

    //public void CardsActiveTrue()
    //{
    //    foreach (var card in listCards)
    //    {
    //        card.gameObject.SetActive(true);
    //    }
    //}

    public void CloseBuildScreen()
    {
        Debug.Log("Close");
        //ResetBuildSpace();
        //StartCoroutine(Effect01(false, buildScreen));
        //buildScreen.SetActive(false);
        CardsActive(false);
        StartCoroutine(Effect02(false, buildScreen));
    }

    public void Save()
    {
        Debug.Log("Save");
        //foreach(var card in listCards)
        //{
        //    card.transform.SetParent(pool, false);
        //}
        if (listDeckCards.Count == 24)
        {
            SaveCardData();
            CardsActive(false);
            //ResetBuildSpace();
            //StartCoroutine(Effect01(false, buildScreen));
            //buildScreen.SetActive(false);
            StartCoroutine(Effect02(false, buildScreen));
        }
        else
        {
            PopupNotEnoughCard();
        }
    }

    public void InitAllCard()
    {
        BasicCard card;
        for (int i = 0; i < CardDataManager.Instance.m_CardDatas.Count; i++)
        {
            card = SimplePool.Spawn(m_BasicCard, m_StartSpawnPosition.position, Quaternion.identity);
            card.Transform.SetParent(m_AllCard.Transform, false);
            card.SetupCardConfig(CardDataManager.Instance.m_CardDatas[i]);
            card.Setup(true);
            card.InitCard(-1);
            card.m_CardController.m_UIManager = this;
            //card.gameObject.SetActive(false);
            listCards.Add(card);
            //listCardSlots.Add(card.m_CardController.GetCardSlot());
        }
    }

    public void InitAllCopyCard()
    {
        BasicCard card;
        for (int i = 0; i < CardDataManager.Instance.m_CardDatas.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                card = SimplePool.Spawn(m_BasicCard, m_StartSpawnPosition.position, Quaternion.identity);
                card.Transform.SetParent(m_AllCopyCard.Transform, false);
                card.SetupCardConfig(CardDataManager.Instance.m_CardDatas[i]);
                card.Setup(true);
                card.InitCard(-1);
                card.m_CardController.m_UIManager = this;
                //card.gameObject.SetActive(false);
                listCopyCards.Add(card);
            }
            //listCardSlots.Add(card.m_CardController.GetCardSlot());
        }
    }

    //public void GenAllCard()
    //{
    //    for(int i = 0; i < 10; i++)
    //    {
    //        for(int j = 0; j < 3; j++)
    //        {
    //            for(int k = 0; k < 2; k++)
    //            {
    //                var id = Random.Range(100000, 999999);
    //                BasicCard cd = Instantiate(card, Vector3.zero, Quaternion.identity);
    //                cd.transform.SetParent(allCards, false);
    //                cd.SetCardConfig(id.ToString(), (CardType)j, (AbilityType)k);
    //                cd.Setup(true);
    //                cd.InitCard();
    //                //cd.gameObject.SetActive(false);
    //                listCards.Add(cd);
    //                //cd.Setup(true);
    //            }
    //        }
    //    }
    //}

    public void SwitchToBuildSpace()
    {
        foreach (var card in listCards)
        {
            //card.gameObject.SetActive(true);
            //card.transform.parent = buildSpace.transform;

            //card.Transform.SetParent(buildSpace.transform, false);
            //Destroy(card.m_CardController.GetCardSlot().gameObject);

            card.m_CardController.GetCardSlot().Transform.SetParent(buildSpace.transform, false);
            //card.gameObject.SetActive(true);
            //card.InitCard(-1);

            //card.m_CardController = null;
            //Destroy(card.m_CardController.GetCardSlot().gameObject);
            //card.gameObject.SetActive(true);
            //card.InitCard(-1);
            //card.m_CardController.GetCardSlot().gameObject.SetActive(true);
        }
        //isFirstTime = false;
    }    

    public void AddBasicCard(BasicCard bsCard)
    {
        if (listDeckCards.Count < 24)
        {
            listDeckCards.Add(bsCard);
            numberOfCards.text = listDeckCards.Count.ToString() + "/24";
        }
        else
        {
            PopupFullCard();
        }
    }

    public void DeleteBasicCard(BasicCard bsCard)
    {
        if (listDeckCards.Count > 0)
        {
            listDeckCards.Remove(bsCard);
            numberOfCards.text = listDeckCards.Count.ToString() + "/24";
        }
    }

    public void SaveCardData()
    {
        listCardDatas.Clear();
        for(int i = 0; i < listDeckCards.Count; i++)
        {
            listCardDatas.Add(listDeckCards[i].m_CardData);
            PlayerPrefs.SetString(i.ToString(), listDeckCards[i].m_CardData.m_ID);
        }

        //for (int i = 0; i < listCardDatas.Count; i++)
        //{
        //    Debug.Log("number " + i.ToString() + ":" + listCardDatas[i].m_ID);
        //}
    }

    public void ResetDeck()
    {
        for(int i = 0; i < 24; i++)
        {
            PlayerPrefs.SetString(i.ToString(), "");
        }
    }

    public void LoadCardData()
    {
        //if (PlayerPrefs == null) return;
        listCardDatas.Clear();
        for(int i = 0; i < 24; i++)
        {
            string id = PlayerPrefs.GetString(i.ToString(), CardDataManager.Instance.m_CardDatas[i/3].m_ID);
            //Debug.Log(id);
            //CardData crData;
            //if (id == "") continue;
            for(int j = 0; j < CardDataManager.Instance.m_CardDatas.Count; j++)
            {
                if(CardDataManager.Instance.m_CardDatas[j].m_ID == id)
                {
                    //CardData crData = new CardData(CardDataManager.Instance.m_CardDatas[j].m_ID,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_Name,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_Archetype,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_Symbol,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_AbilityType,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_EffectDescription,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_WinAnimation,
                    //                               CardDataManager.Instance.m_CardDatas[j].m_LoseAnimation);
                    CardData crData = CardDataManager.Instance.m_CardDatas[j];
                    //crData.m_ID = CardDataManager.Instance.m_CardDatas[i].m_ID;
                    //crData.m_Archetype = CardDataManager.Instance.m_CardDatas[i].m_Archetype;
                    //crData.m_Symbol = CardDataManager.Instance.m_CardDatas[i].m_Symbol;
                    //crData.m_AbilityType = CardDataManager.Instance.m_CardDatas[i].m_AbilityType;
                    //crData.m_EffectDescription = CardDataManager.Instance.m_CardDatas[i].m_EffectDescription;
                    //crData.m_Name = CardDataManager.Instance.m_CardDatas[i].m_Name;
                    //crData.m_WinAnimation = CardDataManager.Instance.m_CardDatas[i].m_WinAnimation;
                    //crData.m_LoseAnimation = CardDataManager.Instance.m_CardDatas[i].m_LoseAnimation;
                    
                    listCardDatas.Add(crData);                    
                }
            }
        }
    }

    public void LoadDeck()
    {
        listDeckCards.Clear();
        numberOfCards.text = listCardDatas.Count.ToString() + "/24";
        popupFullCardText.text = "";        
        foreach(var cardData in listCardDatas)
        {
            CloneCard(cardData.m_ID);
        }
    }

    public void PopupFullCard()
    {
        popupFullCardText.text = "Deck is full";
    }

    public void PopupFullCopyCard()
    {
        popupFullCardText.text = "You have 3 copy of this card";
    }

    public void PopupNotEnoughCard()
    {
        popupFullCardText.text = "You have less then 24 cards";
    }

    public void ResetPopupFullCard()
    {
        popupFullCardText.text = "";
    }

    public void CloneCard(string id)
    {
        //Debug.Log("have");
        foreach (var card in listCopyCards.Where(b => !b.m_CardController.isChooseToDeck))
        {
            //Debug.Log("have");
            if(card.GetID() == id)
            {
                if (listDeckCards.Count < 24)
                {
                    card.m_CardController.isChooseToDeck = true;
                    card.m_CardController.GetCardSlot().transform.SetParent(deckSpace.transform, false);
                    AddBasicCard(card);                    
                    return;
                }
                else
                {
                    PopupFullCard();
                }
            }
        }
    }

    public void ChangeNumberOfClone(string id)
    {
        foreach(var card in listCards)
        {
            if (card.GetID() == id)
            {
                card.m_CardController.numberOfCopy--;
                return;
            }
        }
    }

    public void OutDeck(BasicCard bsCard)
    {
        bsCard.m_CardController.GetCardSlot().transform.SetParent(m_AllCopyCard.transform, false);
    }

    public void ButtonLeft()
    {
        if(indexAvatar == 0)
        {
            indexAvatar = listAvatars.Count - 1;
            avatar.sprite = listAvatars[indexAvatar];
        }
        else
        {
            indexAvatar--;
            avatar.sprite = listAvatars[indexAvatar];
        }
    }

    public void ButtonRight()
    {
        if (indexAvatar == listAvatars.Count-1)
        {
            indexAvatar = 0;
            avatar.sprite = listAvatars[indexAvatar];
        }
        else
        {
            indexAvatar++;
            avatar.sprite = listAvatars[indexAvatar];
        }
    }
    //public IEnumerator FullCard()
    //{

    //}
}



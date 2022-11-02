using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtraQuestTreasurePanel : MonoBehaviour {

    private GameObject canvas;

    private Button button1;
    private Button button2;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private Girl1_status girl1_status;
    private SoundController sc;

    private Text stagenum_text;
    private Text ExtraQuestText;
    private Text ExtraQuest_Score;
    private Text ExtraQuest_GetItemText;
    private Image ExtraQuest_GetItemImage;

    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;

    private GameObject Panel2;
    private GameObject Panel3;

    private GameObject quest_panel;

    private int ItemRank;

    private Dictionary<int, string> ItemGetDict;

    private Tween coinTween;
    private int currentDispCoin;
    private int preDispCoin;

    private float countTime;

    private Animator treasure_animator;
    private Image treasureImage;
    private GameObject treasure_effect1;
    private GameObject treasure_effect2;

    private Sprite treasure_sprite1;
    private Sprite treasure_sprite2;
    private Sprite treasure_sprite3;
    private Sprite treasure_sprite_empty;

    private Text gohoubi_star_text;

    private Transform resulttransform;

    private GameObject Magic_effect_Prefab1;
    private GameObject Magic_effect_Prefab2;
    private List<GameObject> _listEffect = new List<GameObject>();
    private List<GameObject> _listEffect2 = new List<GameObject>();

    private int _poncount;
    private int star_count;
    private List<GameObject> _liststar = new List<GameObject>();

    private GameObject starPrefab;
    private GameObject Manzoku_star_content;

    private int i;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        canvas = GameObject.FindWithTag("Canvas");

        Panel2 = canvas.transform.Find("ScoreHyoujiPanel/ExtraQuestTreasurePanel/Panel2").gameObject;
        Panel2.SetActive(false);
        Panel3 = canvas.transform.Find("ScoreHyoujiPanel/ExtraQuestTreasurePanel/Panel3").gameObject;
        Panel3.SetActive(false);

        //パネル2
        ExtraQuest_Score = Panel2.transform.Find("QuestClearScore/ScoreText").GetComponent<Text>();
        //ExtraQuest_Score.text = GameMgr.Okashi_spquest_MaxScore.ToString();

        treasure_animator = Panel2.GetComponent<Animator>();
        treasure_animator.SetInteger("trans_motion", 0);

        gohoubi_star_text = Panel2.transform.Find("ItemImgPanel/gohoubi_text/Text").GetComponent<Text>();

        treasureImage = Panel2.transform.Find("ItemImgPanel/Button/ItemImg").GetComponent<Image>();
        treasure_effect1 = Panel2.transform.Find("ItemImgPanel/Particle_KiraExplode").gameObject;
        treasure_effect1.SetActive(false);
        treasure_effect2 = Panel2.transform.Find("ItemImgPanel/Particle_KiraStar").gameObject;
        treasure_effect2.SetActive(false);

        button2 = Panel2.transform.Find("ItemImgPanel/Button").GetComponent<Button>();
        button2.interactable = false;

        //宝箱イメージ
        treasure_sprite1 = Resources.Load<Sprite>("Sprites/Icon/" + "treasure_extra1");
        treasure_sprite2 = Resources.Load<Sprite>("Sprites/Icon/" + "treasure_extra2");
        treasure_sprite3 = Resources.Load<Sprite>("Sprites/Icon/" + "treasure_extra3");
        treasure_sprite_empty = Resources.Load<Sprite>("Sprites/Icon/" + "treasure_extra_empty");

        //パネル3
        ExtraQuest_GetItemText = Panel3.transform.Find("ItemGetPanel/ItemText").GetComponent<Text>();
        ExtraQuest_GetItemImage = Panel3.transform.Find("ItemGetPanel/ItemImagePanel/ItemImage").GetComponent<Image>();

        //エフェクトプレファブの取得
        Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_2");
        Magic_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_ResultKamiHubuki");

        Manzoku_star_content = this.transform.Find("Panel2/Manzoku_Score_star/Viewport/Content").gameObject;
        starPrefab = (GameObject)Resources.Load("Prefabs/StarScore");        

        //スター消す
        foreach (Transform child in Manzoku_star_content.transform)
        {
            Destroy(child.gameObject);
        }
        _liststar.Clear();

        //StartCoroutine("WaitButton");
    }

    /*IEnumerator WaitButton()
    {

        yield return new WaitForSeconds(1.0f); //1~2秒まったら、ボタンがおせるようになる。連打防止。
        button1.interactable = true;
    }*/

    public void TreasurePanel_Start()
    {
        Panel2.SetActive(true);

        //アイテム取得処理　先に。
        GetItemMethod();

        star_count = GameMgr.ExtraClear_QuestItemRank-1;
        if(star_count < 1) { star_count = 1; } //例外処理

        switch(GameMgr.ExtraClear_QuestItemRank)
        {
            case 1:

                gohoubi_star_text.text = "がんばった賞";
                break;

            case 2:

                gohoubi_star_text.text = "1つ星ごほうび";
                break;

            case 3:

                gohoubi_star_text.text = "2つ星ごほうび";
                break;

            case 4:

                gohoubi_star_text.text = "3つ星ごほうび";
                break;

            case 5:

                gohoubi_star_text.text = "3つ星ごほうび";
                break;
        }
        

        //数字の演出
        //カウントアップのための秒数を割り出す。
        countTime = GameMgr.Okashi_spquest_MaxScore * 0.03f; //1ごとに0.03fで表示する

        currentDispCoin = 0;
        coinTween = null;

        UpdateCoin(GameMgr.Okashi_spquest_MaxScore);
    }

    public void MainQuestOKButtonON2()
    {
        Panel3.SetActive(true);

        //アイテムがピョンとはじけるアニメ
        sc.PlaySe(86); //76　取得音
        //sc.PlaySe(87); //宝箱開ける音
        Panel3_GetItem_animOn();
    }

    public void MainQuestOKButtonON3()
    {
        girlEat_judge.PanelResultOFF();
    }

    //①数字演出
    void UpdateCoin(int num)
    {
        DOTween.Kill(coinTween);
        coinTween = DOTween.To(() => currentDispCoin, (val) =>
        {
            //Debug.Log("bang");
            currentDispCoin = val;

            if (currentDispCoin < 100) //文字色をかえる。
            {
                ExtraQuest_Score.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                //白(255f / 255f, 255f / 255f, 255f / 255f)　(129f / 255f, 87f / 255f, 60f / 255f) 青文字(105f / 255f, 168f / 255f, 255f / 255f)      
            }
            else if (currentDispCoin >= 100 && currentDispCoin < 200) //GameMgr.high_score
            {
                ExtraQuest_Score.color = new Color(255f / 255f, 252f / 255f, 158f / 255f); //うす黄色

            }
            else if (currentDispCoin >= 200 && currentDispCoin < 300)
            {
                ExtraQuest_Score.color = new Color(119f / 255f, 255f / 255f, 94f / 255f); //うす翠色　ピンク(255f / 255f, 105f / 255f, 139f / 255f)
            }
            else
            {
                ExtraQuest_Score.color = new Color(255f / 255f, 152f / 255f, 197f / 255f); //うすぴんく色　ピンク(255f / 255f, 105f / 255f, 139f / 255f)
            }

            ExtraQuest_Score.text = string.Format("{0:#,0}", val);

            if (currentDispCoin != preDispCoin)
            {
                sc.PlaySe(37); //トゥルルルルという文字送り音
            }
            preDispCoin = currentDispCoin; //前回の値も保存
        }, num, countTime).SetEase(Ease.OutQuart)
        .OnComplete(EndCountUpAnim); //②エンドアニメ　再生終了時;
    }

    void EndCountUpAnim()
    {
        _poncount = 0;

        //③宝箱登場の演出
        //StartCoroutine("StarPon"); //☆登場演出アリは、こっちをONにして、下の行をコメントアウトすればOK
        StartCoroutine("TreasureAnim");

    }

    IEnumerator StarPon()
    {
        if (_poncount >= star_count)
        {
            StartCoroutine("TreasureAnim");
            yield break;
        }

        _liststar.Add(Instantiate(starPrefab, Manzoku_star_content.transform));
        sc.PlaySe(30);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_liststar[_poncount].transform.DOScale(new Vector3(-0.5f, -0.5f, -0.5f), 0.0f)
        .SetRelative());
        sequence.Append(_liststar[_poncount].transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1.0f)
        .SetRelative()
        .SetEase(Ease.OutElastic)); //30px上から、元の位置に戻る。

        if (_poncount >= star_count - 1) //最後の星が出るタイミング
        {

            _listEffect2.Clear();
            for (i = 0; i < _liststar.Count; i++)
            {
                _listEffect2.Add(Instantiate(Magic_effect_Prefab2, _liststar[i].transform));
                _listEffect2.Add(Instantiate(Magic_effect_Prefab1, _liststar[i].transform));
            }

        }

        yield return new WaitForSeconds(0.2f);

        _poncount++;
        StartCoroutine("StarPon");

    }

    IEnumerator TreasureAnim()
    {
        yield return new WaitForSeconds(1.0f);

        sc.PlaySe(19); //タンタカターン
        sc.PlaySe(27); //ジャキーン
        //sc.PlaySe(4);
        //sc.PlaySe(84); //宝箱ガチャン音

        treasure_effect1.SetActive(true);

        //宝箱上から降ってくる
        switch (GameMgr.ExtraClear_QuestItemRank)
        {
            case 1:

                treasureImage.sprite = treasure_sprite_empty;
                break;

            case 2:

                treasureImage.sprite = treasure_sprite1;
                break;

            case 3:

                treasureImage.sprite = treasure_sprite2;
                break;

            case 4:

                treasureImage.sprite = treasure_sprite3;
                break;

            case 5:

                treasureImage.sprite = treasure_sprite3;
                break;
        }
        treasure_animator.SetInteger("trans_motion", 100);


        yield return new WaitForSeconds(1.0f);

        //ボタン押せるようになる。
        button2.interactable = true;

        //エフェクトも出現        
        treasure_effect2.SetActive(true);
    }

    void GetItemMethod()
    {        

        //例外処理。100未満はランク１に。
        if (GameMgr.Okashi_spquest_MaxScore < 100)
        {
            GameMgr.ExtraClear_QuestItemRank = 1;
        }

        //ご褒美アイテムのランク決めは、GirlEat_Judge.csで決定
        ItemRank = GameMgr.ExtraClear_QuestItemRank;       

        ItemGetDict = new Dictionary<int, string>();
        switch (GameMgr.ExtraClear_QuestNum) //GirlEat_JudgeでExtraClear_QuestNumを決定
        {
            //設定してはいるが、現状Rank1・5は未使用。234のみ設定しておけばOK
            case 0: //ハートを100

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "saboten_1");
                ItemGetDict.Add(3, "saboten_2");
                ItemGetDict.Add(4, "saboten_3");
                ItemGetDict.Add(5, "saboten_3");
                break;

            case 1: //300

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "dryflowerpot_1");
                ItemGetDict.Add(3, "dryflowerpot_2");
                ItemGetDict.Add(4, "dryflowerpot_3");
                ItemGetDict.Add(5, "dryflowerpot_3");
                break;

            case 2: //650

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "aroma_candle1");
                ItemGetDict.Add(3, "aroma_candle2");
                ItemGetDict.Add(4, "mini_house");
                ItemGetDict.Add(5, "dryflowerpot_3");
                break;

            case 3: //1000

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "shokukan_powerup1");
                ItemGetDict.Add(3, "shokukan_powerup2");
                ItemGetDict.Add(4, "shokukan_powerup3");
                ItemGetDict.Add(5, "shokukan_powerup3");
                break;

            case 4: //230点以上のスーパークレープ　条件分岐

                break;

            case 10: //茶色いクッキー

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "cookie_powerup2");
                ItemGetDict.Add(3, "cookie_powerup3");
                ItemGetDict.Add(4, "cookie_powerup4");
                ItemGetDict.Add(5, "cookie_powerup5");
                break;

            case 11: //お茶会用のお茶

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "tea_powerup2");
                ItemGetDict.Add(3, "tea_powerup3");
                ItemGetDict.Add(4, "tea_powerup4");
                ItemGetDict.Add(5, "tea_powerup5");
                break;

            case 12: //ヒカリが3種類のお菓子を作れるようにする。

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "hikari_powerup1");
                ItemGetDict.Add(3, "hikari_powerup2");
                ItemGetDict.Add(4, "hikari_powerup3");
                ItemGetDict.Add(5, "hikari_powerup3");
                break;

            case 13: //カミナリのようにすっぱいクレープ 酸味が100以上か、絶妙にすっぱいときのクレープ　すっぱすぎてもクリアできる

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "crepe_powerup2");
                ItemGetDict.Add(3, "crepe_powerup3");
                ItemGetDict.Add(4, "crepe_powerup4");
                ItemGetDict.Add(5, "crepe_powerup5");
                break;

            case 14: //300点以上のいちごのクレープ

                break;

            case 20: //ハート3000以上

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "memory_feather1");
                ItemGetDict.Add(3, "memory_feather2");
                ItemGetDict.Add(4, "memory_feather3");
                ItemGetDict.Add(5, "memory_feather3");
                break;

            case 21: //ムーディーな大人のおかし　カンノーリ　ティラミス　コーヒー　カフェオレシュー　ココアクッキー　ビスコッティ

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "otona_powerup1");
                ItemGetDict.Add(3, "candy_powerup1");
                ItemGetDict.Add(4, "candy_powerup1");
                ItemGetDict.Add(5, "crepe_powerup5");

                if (ItemRank == 4) //レシピの獲得
                {

                }
                break;

            case 22: //ヒカリが10種類のお菓子を作れる

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "aroma_potion1");
                ItemGetDict.Add(3, "aroma_potion2");
                ItemGetDict.Add(4, "aroma_potion3");
                ItemGetDict.Add(5, "crepe_powerup5");
                break;

            case 23: //200をこえる夢のようなパンケーキ

                ItemGetDict.Add(1, "neko_badge1");
                ItemGetDict.Add(2, "magic_crystal1");
                ItemGetDict.Add(3, "magic_crystal2");
                ItemGetDict.Add(4, "magic_crystal3");
                ItemGetDict.Add(5, "magic_crystal3");
                break;

            case 24: //300点超えのプリンセストータ

                break;

            default: //

                break;
        }
        if (ItemGetDict.Count == 0)
        {
            ItemGetDict.Add(1, "neko_badge1");
            ItemGetDict.Add(2, "neko_badge2");
            ItemGetDict.Add(3, "neko_badge3");
            ItemGetDict.Add(4, "neko_badge4");
            ItemGetDict.Add(5, "neko_badge5");
        }


        //描画更新とアイテム取得処理
        ExtraQuest_GetItemText.text = database.items[database.SearchItemIDString(ItemGetDict[ItemRank])].itemNameHyouji;
        ExtraQuest_GetItemImage.sprite = database.items[database.SearchItemIDString(ItemGetDict[ItemRank])].itemIcon_sprite;
        pitemlist.addPlayerItem(ItemGetDict[ItemRank], 1);

    }

    //ボインとはじくようなアニメ
    void Panel3_GetItem_animOn()
    {
        resulttransform = Panel3.transform.Find("ItemGetPanel").gameObject.transform;
        //resultPos = resulttransform.localPosition;
        //resultScale = resulttransform.localScale;

        {
            Sequence sequence = DOTween.Sequence();

            //まず、初期値。
            Panel3.transform.Find("ItemGetPanel").GetComponent<CanvasGroup>().alpha = 0;
            sequence.Append(resulttransform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));
            //sequence.Join(resulttransform.DOLocalMove(new Vector3(0, 0, 0), 0.0f)
            //); //
            //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

            //移動のアニメ
            sequence.Append(resulttransform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.75f)
                .SetEase(Ease.OutElastic));
            /*sequence.Join(resulttransform.DOLocalMove(new Vector3(0f, 80f, 0), 0.75f)
                .SetRelative()
                .SetEase(Ease.OutExpo)); //元の位置に戻る。*/
            sequence.Join(Panel3.transform.Find("ItemGetPanel").GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        }

    }

}

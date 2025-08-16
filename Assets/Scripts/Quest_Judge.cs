using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class Quest_Judge : MonoBehaviour {

    private GameObject canvas;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private BGM sceneBGM;
    private bool mute_on;

    private GameObject barMain_obj;
    private Bar_Main_Controller barMain;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject NinkiStatus_Panel_obj;
    private NinkiStatus_Controller ninkiStatus_Controller;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;

    private Toggle questListToggle;
    private Toggle nouhinToggle;

    private Girl1_status girl1_status;
    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private Exp_Controller exp_Controller;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private QuestSetDataBase quest_database;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    private GameObject black_effect;

    private Text debug_taste_resultText;

    private ItemCardEffectDataBase itemCardEffect_database;

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの点数
    List<int> itemslot_NouhinScore = new List<int>(); //こっちが所持数
    List<int> itemslot_NouhinAddPoint = new List<int>(); //該当トッピングの固有追加点数
    List<int> itemslot_PitemScore = new List<int>();
    private int check_slot_nouhinscore;

    //お菓子の点数
    List<int> result_OkashiScore = new List<int>();

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private GameObject yes_no_panel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private SoundController sc;

    private Dictionary<int, int> deleteOriginalList = new Dictionary<int, int>(); //オリジナルアイテムリストの削除用のリスト。ID, 個数のセット
    private Dictionary<int, int> deleteExtremeList = new Dictionary<int, int>(); //お菓子パネルアイテムリストの削除用のリスト。ID, 個数のセット

    private int i, count, list_count;
    public bool nouhinOK_flag;
    private int nouhinOK_status;
    private bool okashicheck_OK;

    private int _getMoney;
    private int _getNinki;
    private int _getHeart;
    private string _kanso;
    private int _baseMoney;
    private int _MSMoney;

    private int _id;
    private int _Listid;
    private int _Qid;
    private int _questID;
    private int _qitemID;
    private int _clientnum;
    private string _clientname;
    private int _girlset_id;
    private int _girlset_listid;
    private int _GirlJudgeUse;

    private int del_itemid;
    private int del_itemkosu;

    private int set_kaisu;
    private int okashi_totalscore;
    private int okashi_totalkosu;
    private int okashi_score;
    private int shokukan_score;
    private int topping_score;

    private string _filename;
    private string _itemname;
    private string _itemname2;
    private string _itemname3;
    private string _itemname4;
    private string _itemname5;
    private string _itemname6;
    private string _itemname7;
    private string _itemname8;
    private string _itemsubtype;

    private int _kosu_default;
    private int _kosu_total;
    private int _kosu_min;
    private int _kosu_max;
    private int _buy_price;

    private int _rich;
    private int _sweat;
    private int _bitter;
    private int _sour;

    private int _crispy;
    private int _fluffy;
    private int _smooth;
    private int _hardness;
    private int _jiggly;
    private int _chewy;

    private int _juice;
    private int _beauty;
    private int _tea_flavor;

    private int sp1_wind;
    private int sp_score2;
    private int sp_score3;
    private int sp_score4;
    private int sp_score5;
    private int sp_score6;
    private int sp_score7;
    private int sp_score8;
    private int sp_score9;
    private int sp_score10;

    private string[] _tp;
    private int[] _tp_score;

    private string _a;
    private string _b;
    private int _temp_shokukan;
    private int _temp_kyori;
    private float _temp_ratio;

    private int itemType;
    private string _basename;
    private int _basehp;
    private int _baseday;
    private int _basequality;
    private int _baseexp;
    private float _baseprobability;
    private int _baserich;
    private int _basesweat;
    private int _basebitter;
    private int _basesour;
    private int _basecrispy;
    private int _basefluffy;
    private int _basesmooth;
    private int _basehardness;
    private int _basejiggly;
    private int _basechewy;
    private int _basejuice;
    private int _basepowdery;
    private int _baseoily;
    private int _basewatery;
    private int _basebeauty;
    private int _basetea_flavor;

    private int _base_sp_wind;
    private int _base_sp_score2;
    private int _base_sp_score3;
    private int _base_sp_score4;
    private int _base_sp_score5;
    private int _base_sp_score6;
    private int _base_sp_score7;
    private int _base_sp_score8;
    private int _base_sp_score9;
    private int _base_sp_score10;

    private int _basescore;
    private float _basegirl1_like;
    private int _basecost;
    private int _basesell;
    private string[] _basetp;
    private string[] _koyutp;
    private string[] _baseMS;
    private int[] _baseMSvalue;
    private string _base_itemType;
    private string _base_itemType_sub;
    private string _base_itemType_subB;
    private int _base_extreme_kaisu;
    private int _base_item_hyouji;

    private bool judge_anim_on;
    private int judge_anim_status;
    private bool judge_end;

    private int rich_score;
    private int sweat_score;
    private int bitter_score;
    private int sour_score;

    private int crispy_score;
    private int fluffy_score;
    private int smooth_score;
    private int hardness_score;
    private int jiggly_score;
    private int chewy_score;
    private int juice_score;
    private int beauty_score;
    private int tea_flavor_score;
    private int Hosei_score;

    private int rich_result;
    private int sweat_result;
    private int bitter_result;
    private int sour_result;

    private int sweat_level;
    private int bitter_level;
    private int sour_level;

    private string _sweat_kansou;
    private string _bitter_kansou;
    private string _sour_kansou;

    private string debug_money_text;

    private int spscore_total;
    private int _spscore_difference;
    private int spscore_deg;
    private int spscore_deg_base;


    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject WhiteFadeCanvas;

    private GameObject quest_Judge_CanvasPanel;

    //時間
    private float timeOut;

    private GameObject eat_hukidashiPrefab;
    private GameObject eat_hukidashiitem;
    private Text eat_hukidashitext;

    private GameObject character;
    private GameObject character_01, character_03;

    private GameObject questResultPanel;
    private GameObject questResultPanel2;
    
    private Transform questResultPanel_tsukatext_pos;
    private Vector3 questResultPanel_tsukatext_defpos;

    private Transform questResultPanel_tsukatext_pos2;
    private Vector3 questResultPanel_tsukatext_defpos2;
    private Text HintText; //お客さんからの感想テキスト表示

    private bool endresultbutton;

    private int keta;
    private bool slot_ok;
    private int _slotmoney;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");
       
        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //人気コントローラー取得
        ninkiStatus_Controller = NinkiStatus_Controller.Instance.GetComponent<NinkiStatus_Controller>();


        //**クエストパネル関係取得　酒場のみのオブジェクトなので気を付ける
        barMain_obj = GameObject.FindWithTag("Bar_Main");
        barMain = barMain_obj.transform.GetComponent<Bar_Main_Controller>();
        quest_Judge_CanvasPanel = canvas.transform.Find("Quest_Judge_CanvasPanel").gameObject;
        shopquestlistController_obj = quest_Judge_CanvasPanel.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();

        questListToggle = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_QuestList").GetComponent<Toggle>();
        nouhinToggle = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_Nouhin").GetComponent<Toggle>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes = yes_no_panel.transform.Find("Yes").gameObject;
        no = yes_no_panel.transform.Find("No").gameObject;

        //クエストリザルトパネル
        questResultPanel = quest_Judge_CanvasPanel.transform.Find("QuestResultPanel").gameObject;
        questResultPanel.SetActive(false);

        questResultPanel_tsukatext_pos = questResultPanel.transform.Find("QuestResultImage/MoneyTsukaText").transform;
        questResultPanel_tsukatext_defpos = questResultPanel_tsukatext_pos.localPosition;

        //名声パネルの取得
        NinkiStatus_Panel_obj = canvas.transform.Find("NinkiStatus_panel").gameObject;       

        WhiteFadeCanvas = quest_Judge_CanvasPanel.transform.Find("WhiteFadeCanvas").gameObject;

        questResultPanel2 = quest_Judge_CanvasPanel.transform.Find("QuestResultPanel2").gameObject;
        questResultPanel2.SetActive(false);
        HintText = questResultPanel2.transform.Find("QuestResultImage/HintText").GetComponent<Text>();
        questResultPanel_tsukatext_pos2 = questResultPanel2.transform.Find("QuestResultImage/MoneyTsukaText").transform;
        questResultPanel_tsukatext_defpos2 = questResultPanel_tsukatext_pos2.localPosition;

        //ここまで



        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子 

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //魔法エフェクトの計算データベース
        itemCardEffect_database = ItemCardEffectDataBase.Instance.GetComponent<ItemCardEffectDataBase>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; ; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        //キャラクタ取得
        character = GameObject.FindWithTag("Character");

        //Prefab内の、コンテンツ要素を取得
        eat_hukidashiPrefab = (GameObject)Resources.Load("Prefabs/QuestJudge_hukidashi");

        //初期化
        _basetp = new string[database.items[0].toppingtype.Length];
        _koyutp = new string[database.items[0].koyu_toppingtype.Length];
        _baseMS = new string[database.items[0].item_MagicSlot.Length];
        _baseMSvalue = new int[database.items[0].item_MagicSlotValue.Length];
        _tp = new string[quest_database.questset[0].Quest_topping.Length];
        _tp_score = new int[quest_database.questset[0].Quest_tp_score.Length];
        

        InitializeItemSlotDicts();

        judge_anim_on = false;
        judge_anim_status = 0;
        judge_end = false;

        
        endresultbutton = false;
        mute_on = false;
    }

    void InitSetting()
    {
        character_01 = character.transform.Find("CharacterImage/CharacterImage01").gameObject;
        character_03 = character.transform.Find("CharacterImage/CharacterImage03").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (judge_anim_on == true)
        {
            switch (judge_anim_status)
            {
                case 0: //初期化 状態１

                    MoneyStatus_Panel_obj.SetActive(false);
                    NinkiStatus_Panel_obj.SetActive(false);
                    //text_area.SetActive(false);
                    shopquestlistController_obj.SetActive(false);
                    black_effect.SetActive(false);

                    timeOut = 1.5f;
                    judge_anim_status = 1;


                    //カメラ寄る。
                    trans = 2; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //吹き出しの作成
                    eat_hukidashiitem = Instantiate(eat_hukidashiPrefab, character.transform);
                    eat_hukidashitext = eat_hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                    eat_hukidashitext.text = ".";
                    sc.PlaySe(7);

                    //表情をむ～っとにする
                    switch (GameMgr.Scene_Name)
                    {
                        case "Or_Bar_A1":

                            character_01.transform.Find("Jitome").gameObject.SetActive(true);
                            break;

                        case "Or_Bar_C1":

                            character_03.transform.Find("Jitome").gameObject.SetActive(true);
                            break;

                    }

                    _text.text = "納品中.";

                    break;

                case 1: // 状態2

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.5f;
                        judge_anim_status = 2;

                        eat_hukidashitext.text = ". .";

                        _text.text = "納品中. .";

                    }
                    break;

                case 2:

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.5f;
                        if (nouhinOK_status == 0)
                        {
                            judge_anim_status = 4;

                            //白でフェード
                            /*WhiteFadeCanvas.SetActive(true);
                            WhiteFadeCanvas.GetComponent<CanvasGroup>().alpha = 0;
                            WhiteFadeCanvas.GetComponent<CanvasGroup>().DOFade(1, 1.0f);*/

                        }
                        else
                        {
                            judge_anim_status = 4;
                        }

                        //eat_hukidashitext.text = ". .";

                    }
                    break;

                case 3:
                   
                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.5f;
                        judge_anim_status = 4;

                    }
                    break;

                case 4: //アニメ終了。判定する

                    MoneyStatus_Panel_obj.SetActive(true);
                    NinkiStatus_Panel_obj.SetActive(true);

                    //text_area.SetActive(true);

                    //表情を戻す
                    switch (GameMgr.Scene_Name)
                    {
                        case "Or_Bar_A1":

                            character_01.transform.Find("Jitome").gameObject.SetActive(false);
                            break;

                        case "Or_Bar_C1":

                            character_03.transform.Find("Jitome").gameObject.SetActive(false);
                            break;

                    }

                    //食べ中吹き出しの削除
                    if (eat_hukidashiitem != null)
                    {
                        Destroy(eat_hukidashiitem);
                    }

                    judge_anim_on = false;
                    judge_end = true;
                    judge_anim_status = 0;

                    //カメラ寄る。
                    trans = 0; //transが0以下のときに、ズームアウトするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    break;

                default:
                    break;
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }
    }




    //
    //指定のアイテムを、必要個数だけ納品する場合の処理。味の判定などはしない。
    //
    public void Quest_result(int _ID, bool _status)
    {
        InitSetting();

        _qitemID = _ID;

        SetInitQItem(_qitemID);

        nouhinOK_flag = false;
        deleteOriginalList.Clear();
        deleteExtremeList.Clear();

        _getNinki = 0;
        _getMoney = 0;
        _slotmoney = 0;

        _kosu_total = _kosu_default; //トータルで〇個いる。デフォルトアイテムから１個、プレイヤーアイテムリストから、１個＋１個のような感じで、減っていく。

        //プレイヤーのアイテムリストを検索
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            if (pitemlist.playeritemlist[database.items[i].itemName] > 0) //持っている個数が1以上のアイテムのみ、探索。
            {                

                //まず該当アイテムがあるかどうか調べる。
                if( _itemname == database.items[i].itemName)
                {

                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.playeritemlist[database.items[i].itemName] >= _kosu_total)
                    {
                        nouhinOK_flag = true;

                        if (_status) //削除処理もいれる場合
                        {
                            //所持アイテムを削除
                            pitemlist.deletePlayerItem(database.items[i].itemName, _kosu_total);
                        }
                    }
                    else
                    {
                        nouhinOK_flag = false;

                        _kosu_total -= pitemlist.playeritemlist[database.items[i].itemName];

                        if (_status) //削除処理もいれる場合
                        {
                            //さらにデリートリストに追加しておく。
                            del_itemid = i;
                            del_itemkosu = pitemlist.playeritemlist[database.items[i].itemName];
                        }
                    }
                }
            }
        }
        
        if (!nouhinOK_flag) //上の探索で納品OKがtrueなら、オリジナルアイテムリストは検索しない
        {
            //次にプレイヤーのオリジナルアイテムリストを検索。player_originalitemlistは個数が1以上のものしかセットされていない。
            i = 0;
            while (i < pitemlist.player_originalitemlist.Count)
            {

                //まず該当アイテムがあるかどうか調べる。
                if (_itemname == pitemlist.player_originalitemlist[i].itemName)
                {
                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.player_originalitemlist[i].ItemKosu >= _kosu_total)
                    {
                        nouhinOK_flag = true;

                        if (_status) //削除処理もいれる場合
                        {
                            //さらにデリートリストに追加しておく。あとで降順に削除
                            deleteOriginalList.Add(i, _kosu_total);
                        }

                        break;
                    }
                    else
                    {
                        nouhinOK_flag = false;

                        _kosu_total -= pitemlist.player_originalitemlist[i].ItemKosu;

                        if (_status) //削除処理もいれる場合
                        {
                            //さらにデリートリストに追加しておく。あとで降順に削除
                            deleteOriginalList.Add(i, pitemlist.player_originalitemlist[i].ItemKosu);
                        }
                        
                    }
                }
                i++;
            }
        }

        if (!nouhinOK_flag) //上の探索で納品OKがtrueなら、お菓子パネルアイテムリストは検索しない
        {
            //
            i = 0;
            while (i < pitemlist.player_extremepanel_itemlist.Count)
            {

                //まず該当アイテムがあるかどうか調べる。
                if (_itemname == pitemlist.player_extremepanel_itemlist[i].itemName)
                {
                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.player_extremepanel_itemlist[i].ItemKosu >= _kosu_total)
                    {
                        nouhinOK_flag = true;

                        if (_status) //削除処理もいれる場合
                        {
                            //さらにデリートリストに追加しておく。あとで降順に削除
                            deleteExtremeList.Add(i, _kosu_total);
                        }

                        break;
                    }
                    else
                    {
                        nouhinOK_flag = false;

                        _kosu_total -= pitemlist.player_extremepanel_itemlist[i].ItemKosu;

                        if (_status) //削除処理もいれる場合
                        {
                            //さらにデリートリストに追加しておく。あとで降順に削除
                            deleteExtremeList.Add(i, pitemlist.player_extremepanel_itemlist[i].ItemKosu);
                        }

                    }
                }
                i++;
            }
        }

        if (nouhinOK_flag)
        {
            if (_status) //決定した場合。削除処理や演出アニメははいらない
            {
                Result_Okashi_Judge1();
                //StartCoroutine("Okashi_Judge_Anim1");
            }
        }
        else //納品、数が足りてない場合
        {
            if (_status) //削除処理もいれる場合
            {
                sc.PlaySe(6);
                _text.text = "まだ数が足りてないようね..。";

                //リスト更新
                shopquestlistController.NouhinList_DrawView();
                shopquestlistController.nouhin_select_on = 0;

                yes_no_panel.SetActive(false);

                questListToggle.interactable = true;
                nouhinToggle.interactable = true;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
            }
        }

        GameMgr.System_shop_defaulttext_koushin = false;
    }

    /*
    IEnumerator Okashi_Judge_Anim1()
    {
        judge_anim_on = true;

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        judge_end = false;

        if (nouhinOK_flag)
        {
            Result_Okashi_Judge1();            
        }
        else
        {
        }
    }*/

    void Result_Okashi_Judge1()
    {
        //アイテム削除
        if (deleteOriginalList.Count > 0)
        {
            pitemlist.deletePlayerItem(database.items[del_itemid].itemName, del_itemkosu); //デフォルトアイテムから先に削除
            DeleteOriginalItem(); //オリジナルからも削除
            DeleteExtremeItem(); //エクストリームパネルからも選んでいれば削除
        }

        _baseMoney = basemoney_keisan();
        _getMoney = _baseMoney;
        _getNinki = 0; //納品のみのクエストは、人気度は上がらない
        //BarNPC_MeidoFriendPointUP(1); //店主の友好度は上がる

        //ルーティのマッサージポイント
        switch (GameMgr.Scene_Name)
        {
            case "Or_Bar_A1":

                GameMgr.NPC_pahupahu_point += Random.Range(1, 3);
                break;
        }

        //足りてるので、納品完了の処理
        _text.text = "報酬 " + GameMgr.ColorYellow + _getMoney + "</color>" + GameMgr.MoneyCurrency + " を受け取った！" + "\n" + "ありがとう！お客さんもとても喜んでいるわ！";

        //該当のクエストを削除
        quest_database.questTakeset.RemoveAt(_qitemID);

        Debug.Log("納品完了！");

        //ジャキーンみたいな音を鳴らす。
        //sc.PlaySe(4);
        sc.PlaySe(76);
        sc.PlaySe(31);

        //キャラクタ表情変更
        switch (GameMgr.Scene_Name)
        {
            case "Or_Bar_A1":

                character_01.transform.Find("Smile").gameObject.SetActive(true);
                break;

            case "Or_Bar_C1":

                character_03.transform.Find("Smile").gameObject.SetActive(true);
                break;

        }

        //クエストリザルト画面をだす。
        questResultPanel.SetActive(true);
        questResultPanel.transform.Find("QuestResultImage/GetMoneyParam").GetComponent<Text>().text = _getMoney.ToString();
        keta = Digit(_getMoney);
        questResultPanel_tsukatext_pos.DOLocalMove(new Vector3(20f * (keta - 1), 0f, 0), 0.0f).SetRelative();

        StartCoroutine("EndQuestResultButton");
    }

    void DeleteOriginalItem()
    {

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteOriginalList.Count > 0)
        {
            //Debug.Log("オリジナルアイテムを納品");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteOriginalList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {

                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);

                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }
    }

    void DeleteExtremeItem()
    {

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteExtremeList.Count > 0)
        {
            //Debug.Log("オリジナルアイテムを納品");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteExtremeList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                pitemlist.deleteExtremePanelItem(deletePair.Key, deletePair.Value);
                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }
    }





    //
    //クッキーなどの判定するお菓子を納品した場合の処理　特にお菓子タイプでないといけない縛りはない
    //
    public void Okashi_Judge(int _ID)
    {
        InitSetting();
        _qitemID = _ID;

        Okashi_Judge_Method();
    }
   

    void Okashi_Judge_Method()
    {
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();
       
        deleteOriginalList.Clear();
        deleteExtremeList.Clear();
        result_OkashiScore.Clear();
        okashi_totalscore = 0;
        okashi_totalkosu = 0;
        _getNinki = 0;
        _getMoney = 0;
        _getHeart = 0;
        _slotmoney = 0;

        set_kaisu = pitemlistController._listcount.Count;

        //listcount分 提出するアイテムのパラメータを判定する。
        for (list_count = 0; list_count < set_kaisu; list_count++)
        {
            //選択したアイテムのデータをセット
            SetInitNouhinItem(pitemlistController._listcount[list_count]);

            //酒場の判定をセット
            _GirlJudgeUse = quest_database.questTakeset[_qitemID].GirlJudgeUse;
            if (_GirlJudgeUse == 0)
            {
                SetInitQItem(_qitemID); //依頼アイテムのパラメータを代入 番号指定で、女の子の判定を使用できる
            }
            else
            {
                Debug.Log("酒場　女の子の好み判定に使用");
                SetInitQItemGirlJudge(_qitemID, _basename); //1 = 女の子の好みを使用
            }

            //
            //お菓子の正解判定。①タイプ　②味　③スロットを見る。
            //A. 一つでも違うのが入っていると、失格
            //B. タイプはOKで、味が足りない場合は、やはり失格 nouhinOK_status = 2;
            //C. それをこえたら、各アイテムごとの平均値*納品個数をみて、最終的なスコアをだす。
            //

            okashi_score = 0;            
            check_slot_nouhinscore = 0;

            shokukan_score = 0;
            crispy_score = 0;
            fluffy_score = 0;
            smooth_score = 0;
            hardness_score = 0;
            juice_score = 0;
            beauty_score = 0;
            topping_score = 0;

            //未使用。
            rich_score = 0;
            jiggly_score = 0;
            chewy_score = 0;

            _a = "";
            _b = "";
            HintText.text = "";

            //①指定のトッピングがあるかをチェック。一つでも指定のものがあれば、OK　現在はチェックしない。

            nouhinOK_status = 0; //先にOKでリセット
            okashicheck_OK = false;
            slot_ok = true;

            //納品用スコアがすべて０の場合、トッピングを計算しないので、無視する。
            /*for (i=0; i < itemslot_NouhinScore.Count; i++)
            {
                //Debug.Log("納品スコア" + itemslotInfo[i] + " " + itemslot_NouhinScore[i]);
                //0はNonなので、無視
                if (i != 0)
                {
                    check_slot_nouhinscore += itemslot_NouhinScore[i];
                }
            }
            //Debug.Log("check_slot_nouhinscore: " + check_slot_nouhinscore);

            if (check_slot_nouhinscore == 0)
            {
                nouhinOK_status = 0;
            }
            else
            {
                i = 0;
                while (i < itemslot_NouhinScore.Count)
                {
                    //0はNonなので、無視
                    if (i != 0)
                    {
                        //納品スコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                        if (itemslot_PitemScore[i] >= itemslot_NouhinScore[i])
                        {
                            if (itemslot_NouhinScore[i] != 0)
                            {
                                nouhinOK_status = 0;
                                slot_ok = true;

                                //クエストのトッピングスコアをここで追加
                                break;
                            }
                        }
                        //のっていなかった場合は、マイナス補正
                        else
                        {
                            nouhinOK_status = 0;
                            
                        }
                    }
                    i++;
                }
            }

            if(slot_ok)
            {
                //補正なし
            }
            else
            {
                //ほしいトッピングのっていないので、マイナス
                okashi_score -= 50;
            }*/


            //②味パラメータの計算。GirlEat_Judgeのをそのまま流用。

            rich_result = _baserich - _rich;
            sweat_result = _basesweat - _sweat;
            bitter_result = _basebitter - _bitter;
            sour_result = _basesour - _sour;

            //rich_score = girlEat_judge.TasteKeisanBase(_rich, rich_result, "味のコク: "); //クエストの値, お菓子の値-クエストの値, デバッグ表示用。返り値は、点数。

            sweat_score = girlEat_judge.TasteKeisanBase(_sweat, sweat_result, _base_itemType_sub, _basegirl1_like, "甘味: "); //クエストの値, お菓子の値-クエストの値, デバッグ表示用。返り値は、点数。
            sweat_level = girlEat_judge.taste_level;

            bitter_score = girlEat_judge.TasteKeisanBase(_bitter, bitter_result, _base_itemType_sub, _basegirl1_like, "苦み: ");
            bitter_level = girlEat_judge.taste_level;

            sour_score = girlEat_judge.TasteKeisanBase(_sour, sour_result, _base_itemType_sub, _basegirl1_like, "酸味: ");
            sour_level = girlEat_judge.taste_level;

            //書き方が少し違うけど、GirlEat_Judgeでやってることとほぼ一緒
            if (_crispy > 0)
            {
                _temp_kyori = _basecrispy - _crispy;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    crispy_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (crispy_score < GameMgr.low_score)
                    {
                        _a = "さくさく感がちょっと足りない。";
                    }
                    else if (crispy_score >= GameMgr.low_score && crispy_score < GameMgr.high_score)
                    {
                        _a = "まあまあのさくさく感";
                    }
                    else
                    {
                        _a = "さくさく感がいい感じだわ";
                    }                   
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    crispy_score = (int)(_basescore * _temp_ratio);
                    _a = "さくさく感がちょっと足りない。";
                }
            }

            if (_fluffy > 0)
            {
                _temp_kyori = _basefluffy - _fluffy;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    fluffy_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (fluffy_score < GameMgr.low_score)
                    {
                        _a = "ふんわりがちょっと足りない。";
                    }
                    else if (fluffy_score >= GameMgr.low_score && fluffy_score < GameMgr.high_score)
                    {
                        _a = "まあまあのふんわり具合";
                    }
                    else
                    {
                        _a = "ふんわり感がいい感じだわ";
                    }                    
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    fluffy_score = (int)(_basescore * _temp_ratio);
                    _a = "ふんわり感がちょっと足りない。";
                }
            }

            if (_smooth > 0)
            {
                _temp_kyori = _basesmooth - _smooth;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    smooth_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (smooth_score < GameMgr.low_score)
                    {
                        _a = "なめらかさがちょっと足りない。";
                    }
                    else if (smooth_score >= GameMgr.low_score && smooth_score < GameMgr.high_score)
                    {
                        _a = "なめらかさ　まあまあ";
                    }
                    else
                    {
                        _a = "なめらかさはいい感じだわ";
                    }                    
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    smooth_score = (int)(_basescore * _temp_ratio);
                    _a = "なめらかな感じがちょっと足りない。";
                }

            }

            if (_hardness > 0)
            {
                _temp_kyori = _basehardness - _hardness;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    hardness_score = (int)(_basescore * _temp_ratio) + _temp_kyori; //basescoreは、現在一律３０点

                    if(hardness_score < GameMgr.low_score)
                    {
                        _a = "歯ごたえがちょっと足りない。";
                    }
                    else if (hardness_score >= GameMgr.low_score && hardness_score < GameMgr.high_score)
                    {
                        _a = "歯ごたえ　まあまあ";
                    }
                    else
                    {
                        _a = "歯ごたえがいい感じだわ";
                    }                        
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    hardness_score = (int)(_basescore * _temp_ratio);
                    _a = "歯ごたえがちょっと足りない。";
                }
            }

            if (_jiggly > 0)
            {
                _temp_kyori = _basejiggly - _jiggly;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    jiggly_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (jiggly_score < GameMgr.low_score)
                    {
                        _a = "ぷにぷに感がちょっと足りない。";
                    }
                    else if (jiggly_score >= GameMgr.low_score && jiggly_score < GameMgr.high_score)
                    {
                        _a = "ぷにぷに感　まあまあ";
                    }
                    else
                    {
                        _a = "ぷにぷに感がいい感じだわ";
                    }                  
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    jiggly_score = (int)(_basescore * _temp_ratio);
                    _a = "ぷにぷに感がちょっと足りない。";
                }
            }

            if (_chewy > 0)
            {
                _temp_kyori = _basechewy - _chewy;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    chewy_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (chewy_score < GameMgr.low_score)
                    {
                        _a = "噛みごたえがちょっと足りない。";
                    }
                    else if (chewy_score >= GameMgr.low_score && chewy_score < GameMgr.high_score)
                    {
                        _a = "噛みごたえ　まあまあ";
                    }
                    else
                    {
                        _a = "噛みごたえがいい感じだわ";
                    }                   
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    chewy_score = (int)(_basescore * _temp_ratio);
                    _a = "噛みごたえがちょっと足りない。";
                }
            }

            if (_juice > 0)
            {
                _temp_kyori = _basejuice - _juice;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    juice_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (juice_score < GameMgr.low_score)
                    {
                        _a = "のどごしがちょっと足りない。";
                    }
                    else if (juice_score >= GameMgr.low_score && juice_score < GameMgr.high_score)
                    {
                        _a = "のどごしまあまあ";
                    }
                    else
                    {
                        _a = "のどごしがいいね。";
                    }
                    
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    juice_score = (int)(_basescore * _temp_ratio);
                    _a = "のどごしがちょっと足りない。";
                }
            }

            if (_tea_flavor > 0)
            {
                _temp_kyori = _basetea_flavor - _tea_flavor;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    tea_flavor_score = (int)(_basescore * _temp_ratio) + _temp_kyori;

                    if (tea_flavor_score < GameMgr.low_score)
                    {
                        _a = "香りがちょっと足りない。";
                    }
                    else if (tea_flavor_score >= GameMgr.low_score && tea_flavor_score < GameMgr.high_score)
                    {
                        _a = "香り　まあまあ";
                    }
                    else
                    {
                        _a = "香りがいい感じだわ";
                    }
                    
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    tea_flavor_score = (int)(_basescore * _temp_ratio);
                    _a = "香りがちょっと足りない。";
                }
            }

            //特殊点の計算　女の子の好み使用した場合のみ
            spscore_total = 0;
            if (_GirlJudgeUse == 1)
            {
                Debug.Log("酒場クエスト特殊点の計算");
                SpScoreKeisanQuest(_base_sp_wind, sp1_wind, "風らしさ");
                SpScoreKeisanQuest(_base_sp_score2, sp_score2, "海らしさ");
                SpScoreKeisanQuest(_base_sp_score3, sp_score3, "愛らしさ");
                SpScoreKeisanQuest(_base_sp_score4, sp_score4, "夏らしさ");
                SpScoreKeisanQuest(_base_sp_score5, sp_score5, "大人");
                SpScoreKeisanQuest(_base_sp_score6, sp_score6, "子供");
                SpScoreKeisanQuest(_base_sp_score7, sp_score7, "メルヘン");
                SpScoreKeisanQuest(_base_sp_score8, sp_score8, "芸術性");
                SpScoreKeisanQuest(_base_sp_score9, sp_score9, "光・キラキラ");
                SpScoreKeisanQuest(_base_sp_score10, sp_score10, "和風感");
            }

            //特定のお菓子の判定。一致しているかチェック。itemNameには、固有名、サブタイプ、サブタイプBどれを入れてもOK。Nonが入ってると無視する。
            OkashiTypeJudge(_itemname);
            OkashiTypeJudge(_itemname2);
            OkashiTypeJudge(_itemname3);
            OkashiTypeJudge(_itemname4);
            OkashiTypeJudge(_itemname5);
            OkashiTypeJudge(_itemname6);
            OkashiTypeJudge(_itemname7);
            OkashiTypeJudge(_itemname8);
            OkashiTypeJudge2(_itemsubtype);

            //④トッピングスロットをみて、スコアを加算する。アイテムについているスロットの点数を加算する。
            for (i = 0; i < itemslot_PitemScore.Count; i++)
            {
                //0はNonなので、無視
                if (i != 0)
                {
                    //トッピングごとに、得点を加算する。妹の採点のtotal_scoreの加算値と共有。
                    if (itemslot_PitemScore[i] > 0)
                    {
                        topping_score += slotnamedatabase.slotname_lists[i].slot_totalScore * itemslot_PitemScore[i];
                        //_basebeauty += slotnamedatabase.slotname_lists[i].slot_Beauty * itemslot_PitemScore[i]; //見た目に対するボーナス得点　ややこしいので廃止
                        _slotmoney += slotnamedatabase.slotname_lists[i].slot_Money * itemslot_PitemScore[i];
                    }
                }
            }
            Debug.Log("_slotmoney(最初の計算): " + _slotmoney);

            //クエストによっては、トッピングによって、さらに追加得点。
            for (i = 0; i < itemslot_NouhinScore.Count;  i++)
            {
                //0はNonなので、無視
                if (i != 0)
                {
                    //納品スコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                    if (itemslot_PitemScore[i] >= itemslot_NouhinScore[i])
                    {
                        topping_score += itemslot_NouhinAddPoint[i] * itemslot_PitemScore[i];
                    }
                }
            }

            //⑤油っこいなどのマイナスの値がついてた場合、マイナス補正
            if (_basepowdery > 50)
            {
                okashi_score -= 30;
            }
            if (_baseoily > 50)
            {
                okashi_score -= 30;
            }
            if (_basewatery > 50)
            {
                okashi_score -= 30;
            }

            //
            //見た目点数の計算
            //
            //先に演出がかかっているかをチェック
            itemCardEffect_database.MagicEffect_SlotKeisan(_baseMS, _baseMSvalue, _id, 1);
            _basebeauty += itemCardEffect_database._add_magicbeauty;

            if (_beauty > 0)
            {
                beauty_score = girlEat_judge.BeautyKeisanBase(_basebeauty, _beauty);

                if (beauty_score < 50)
                {
                    _b = "";
                }
                else if (beauty_score >= 50 && beauty_score < 100)
                {
                    _b = "見た目がいい";
                }
                else
                {
                    _b = "素晴らしい見た目";
                }
            }
            //
            //
            //

            //最終補正　妹の基準より、やや厳しめにするために、点数を下げる。
            Hosei_score = -15;

            //総合点数を計算
            okashi_score += sweat_score + bitter_score + sour_score +
                crispy_score + fluffy_score + smooth_score + hardness_score + jiggly_score + chewy_score +
                juice_score + beauty_score + tea_flavor_score + topping_score + spscore_total + Hosei_score;

            //採点はここまで


            //スコアを保持
            result_OkashiScore.Add(okashi_score * pitemlistController._listkosu[list_count]);
            okashi_totalkosu += pitemlistController._listkosu[list_count];

            //アイテムを削除
            switch (itemType)
            {
                case 0:

                    //所持アイテムを削除
                    pitemlist.deletePlayerItem(database.items[_id].itemName, _kosu_default);
                    break;

                case 1:

                    //所持アイテムをリストに追加し、あとで降順に削除
                    deleteOriginalList.Add(_id, _kosu_default);
                    break;

                case 2:

                    //所持アイテムをリストに追加し、あとで降順に削除
                    deleteExtremeList.Add(_id, _kosu_default);
                    break;

            }
        }

        //各スコアを加算し、平均をとり、最終スコアを算出
        for (i = 0; i < result_OkashiScore.Count; i++)
        {
            okashi_totalscore += result_OkashiScore[i];
        }
        if (okashi_totalkosu == 0) { okashi_totalkosu = 1; }

        //最終スコア
        okashi_totalscore /= okashi_totalkosu;       
        
        if(okashi_totalscore <= 0) //0点以下でも、無条件でダメ
        {
            okashi_totalscore = 0;
            //nouhinOK_status = 2;
        }
        Debug.Log("okashi_totalscore: " + okashi_totalscore);
        GameMgr.bar_quest_okashiScore = okashi_totalscore;

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        DeleteOriginalItem();
        DeleteExtremeItem();

        StartCoroutine("Okashi_Judge_Anim2");
        
    }

    void SpScoreKeisanQuest(int _basespscore, int _girlspscore, string _spname)
    {
        spscore_deg = 5;
        spscore_deg_base = -30;

        //女の子の判定値があった場合、追加加点

        //風らしさ
        if (_girlspscore > 0)
        {
            Debug.Log("酒場クエ　特殊計算: " + _spname + "ON");

            _spscore_difference = _basespscore - _girlspscore;

            if (_spscore_difference > 0) //判定値があり超えていた場合　加点される
            {
                spscore_total += SpScore_HoseiA(_spscore_difference);
            }

            if (_spscore_difference < 0) //合格点に達してない場合は、減点
            {
                //GameMgr.Contest_Clear_Failed = true;  //Onにすると、足りなかったときに強制的にコンテスト失格になる。
                spscore_total += _spscore_difference * spscore_deg + spscore_deg_base; //マイナスの場合、減点大きくなる
            }
        }
        else
        {
            spscore_total += 0;
        }
    }

    int SpScore_HoseiA(float _score)
    {
        Debug.Log("SPScore補正前: " + _score);

        if (_score > 0f && _score <= 100f)
        {
            _score = _score * 1.0f;
        }
        else if (_score > 100f && _score <= 200f)
        {
            _score = _score * 1.2f;
        }
        else if (_score > 200f)
        {
            _score = _score * 1.3f;
        }

        Debug.Log("SPScore補正後点: " + _score);
        return (int)_score;
    }

    void OkashiTypeJudge(string _name)
    {
        if (okashicheck_OK) //お菓子OKがでてたら、チェックはせずそのままOKで。
        {

        }
        else
        {
            if (_name == "Non") //特に指定なしなら次をみる
            {
                okashicheck_OK = false;
            }
            else if (_name == _basename || _name == _base_itemType_sub || _name == _base_itemType_subB) //お菓子の名前かサブタイプ系が一致している。
            {
                //サブは計算せず、特定のお菓子自体が正解なら、正解　ここで抜けてOK
                nouhinOK_status = 0;
                okashicheck_OK = true;
            }
            else
            {
                //不正解。次をみる。
                okashicheck_OK = false;
                //nouhinOK_status = 1;
            }
        }
    }

    //最後にサブタイプを一回チェック　これでダメなら、違うお菓子なので不正解に。
    void OkashiTypeJudge2(string _nameSub)
    {
        if (okashicheck_OK) //お菓子OKがでてたら、チェックはせずそのままOKで。
        {

        }
        else
        {
            //③お菓子の種別の計算
            if (_nameSub == "Non") //特に指定なし
            {
                //不正解。そもそも違うお菓子を納品している。
                nouhinOK_status = 1;
                okashicheck_OK = false;
            }
            else if (_nameSub == _base_itemType_sub || _nameSub == _base_itemType_subB) //お菓子の種別が一致している。
            {
                //正解
                nouhinOK_status = 0;
                okashicheck_OK = true;
            }
            else
            {
                //不正解。そもそも違うお菓子を納品している。
                nouhinOK_status = 1;
                okashicheck_OK = false;
            }
        }
    }

    IEnumerator Okashi_Judge_Anim2()
    {
        judge_anim_on = true;

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        judge_end = false;

        switch (nouhinOK_status)
        {
            case 0: //正解の場合

                //味によって、取得のお金が増減する。おいしいと、お金もちょっとプラス。

                _baseMoney = basemoney_keisan();
                _getNinki = 0;
                Debug.Log("_baseMoney: " + _baseMoney);

                if (okashi_totalscore < 30) //粗悪なお菓子だと、マイナス評価
                {
                    _getMoney = (int)(_baseMoney * 0.2f);
                    debug_money_text = "(基準値 * 0.2f)";                    
                    _kanso = "う～ん..。お客さん不満だったみたい。" + "\n" + "次からは気をつけてね。報酬額が少し減った！";
                    
                }
                else if (okashi_totalscore >= 30 && okashi_totalscore < 45) //30~45
                {
                    _getMoney = (int)(_baseMoney * 0.4f);
                    debug_money_text = "(基準値 * 0.4f)";
                    _kanso = "ありがとう。　..少しお客さん不満だったみたい。" + "\n" + "次はもっと期待してるわね！";
                }
                else if (okashi_totalscore >= 45 && okashi_totalscore < GameMgr.low_score) //45~60
                {
                    _getMoney = (int)(_baseMoney * 0.8f);
                    debug_money_text = "(基準値 * 0.8f)";
                    _kanso = "ありがとう！　お客さん喜んでたわ！";
                }
                else if (okashi_totalscore >= GameMgr.low_score && okashi_totalscore < 80) //60~80
                {
                    _getMoney = (int)(_baseMoney * 1.1f);
                    debug_money_text = "(基準値 * 1.1f)";
                    _kanso = "ありがとう！　お客さん、気に入ってたみたい！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 80 && okashi_totalscore < GameMgr.high_score) //80~100
                {
                    _getMoney = (int)(_baseMoney * 1.35f);
                    debug_money_text = "(基準値 * 1.35)";
                    _kanso = "ありがとう！お客さん、大喜びだったわ！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";                    
                }
                else if (okashi_totalscore >= GameMgr.high_score && okashi_totalscore < 120) //100~120
                {
                    _getMoney = (int)(_baseMoney * 1.5f);
                    debug_money_text = "(基準値 * 1.5f)";
                    _kanso = "ありがとう！とても良い出来みたい！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 120 && okashi_totalscore < 150) //100~120
                {
                    _getMoney = (int)(_baseMoney * 1.75f);
                    debug_money_text = "(基準値 * 1.75f)";
                    _kanso = "グレイトだわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 150 && okashi_totalscore < 200) //120~150
                {
                    _getMoney = (int)(_baseMoney * 1.85f);
                    debug_money_text = "(基準値 * 1.85f)";
                    _kanso = "ほっぺたがとろけちゃうぐらい最高だって！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                    BarNPC_FriendPointUP(1);
                }
                else if(okashi_totalscore >= 200)
                {
                    //お菓子ごとに、上がりにくかったり補正がかかる
                    switch(_base_itemType_sub)
                    {
                        case "Cookie":

                            CostHosei_1();
                            break;

                        case "Cookie_Hard":

                            CostHosei_1();
                            break;

                        case "Cookie_Mat":

                            CostHosei_1();
                            break;

                        case "Rusk":

                            CostHosei_1();
                            break;

                        default:

                            CostHosei_default();                            
                            break;
                    }                   
                }

                //マジックスロットついてたらさらに報酬が上乗せ
                _MSMoney = 0;
                for (i = 0; i < _baseMS.Length; i++)
                {
                    _MSMoney = _baseMSvalue[i] * 200;
                    _getMoney += _MSMoney; //種類によらず一個ついてたら+300 MSValueはUseLVが入ってるので、LVが高いと報酬上がる
                }

                //そのクエストで一回だけスターもらえる _questID 人気度の計算はCostHoseiで済み
                if (_getNinki >= 1)
                {
                    if (quest_database.questTakeset[_qitemID].Quest_GetNinkiFlag == 0)
                    {
                        //まだスターをとったことないので、そのままスターゲット _getninkiは、上で事前に計算済 テイクのほうでなく元のクエストデータを上書きする。
                        _Listid = quest_database.SearchQuestID(quest_database.questTakeset[_qitemID].Quest_ID);
                        quest_database.questset[_Listid].Quest_GetNinkiFlag += 1;
                        Debug.Log("高得点なおかしだったので、クライアントからスターもらえる。QuestID: " + quest_database.questTakeset[_qitemID].Quest_ID);
                    }
                    else //スター何個かとったことあるので、次はスターはもらえない ただし、フラグがあると、家に直接きてくれる予定
                    {
                        _getNinki = 0;
                    }
                }

                //60点以上なら、店主のマッサージポイントあがる。
                if (okashi_totalscore >= GameMgr.low_score) //60~80
                {
                    //BarNPC_MeidoFriendPointUP(1);

                    //ルーティのマッサージポイント
                    switch (GameMgr.Scene_Name)
                    {
                        case "Or_Bar_A1":

                            if (okashi_totalscore >= GameMgr.low_score && okashi_totalscore < 200) //
                            {
                                GameMgr.NPC_pahupahu_point += Random.Range(1, 3);
                            }
                            else if (okashi_totalscore >= 200) //
                            {
                                GameMgr.NPC_pahupahu_point += Random.Range(2, 5);
                            }
                            break;
                    }
                }                             

                _getHeart = (int)(okashi_totalscore * 0.1f);
                _text.text = "評価: " + GameMgr.ColorYellow + okashi_totalscore + "</color>" + "点" + 
                    "　報酬 " + GameMgr.ColorYellow + _getMoney + GameMgr.MoneyCurrency + "　</color>" + "を受け取った！" + "\n" + _kanso;

                Debug.Log("納品完了！" + " 採点：" + okashi_totalscore + "点！");

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                

                //60点以下は通常音
                if (okashi_totalscore < GameMgr.low_score)
                {
                    //ジャキーンみたいな音を鳴らす。                
                    //sc.PlaySe(4);
                    sc.PlaySe(76);
                    sc.PlaySe(31);
                }
                else if (okashi_totalscore >= GameMgr.low_score && okashi_totalscore < GameMgr.high_score) //80点以上
                {
                    sc.PlaySe(76);
                    sc.PlaySe(31);

                    sc.PlaySe(78);
                    sc.PlaySe(88);

                }
                else if (okashi_totalscore >= GameMgr.high_score && okashi_totalscore < 250) //ハイスコア
                {
                    sc.PlaySe(76);
                    sc.PlaySe(31);

                    sc.PlaySe(78);
                    sc.PlaySe(88);
                    //sc.PlaySe(43); 同時に5つ以上のSEはならないので一旦オフ
                }
                else if (okashi_totalscore >= 250) //250点以上のときは、ファンファーレ
                {
                    sc.PlaySe(76);
                    sc.PlaySe(31);

                    sc.PlaySe(78);
                    sc.PlaySe(88);
                    //sc.PlaySe(43);
                    sceneBGM.PlayFanfare1();
                    mute_on = true;
                }
                else
                {
                    
                }

                //キャラクタ表情変更
                if (okashi_totalscore < GameMgr.low_score)
                { }
                else
                {
                    switch (GameMgr.Scene_Name)
                    {
                        case "Or_Bar_A1":

                            character_01.transform.Find("Smile").gameObject.SetActive(true);
                            break;

                        case "Or_Bar_C1":

                            character_03.transform.Find("Smile").gameObject.SetActive(true);
                            break;

                    }
                }

                //クエストリザルト画面をだす。
                questResultPanel2.SetActive(true);
                questResultPanel2.transform.Find("QuestResultImage/GetMoneyParam").GetComponent<Text>().text = _getMoney.ToString();
                keta = Digit(_getMoney);
                questResultPanel_tsukatext_pos2.DOLocalMove(new Vector3(20f* (keta-1), 0f, 0), 0.0f).SetRelative();

                //感想もいれる。
                SetHintText();

                StartCoroutine("EndQuestResultButton");

                //デバッグ用味採点テキスト
                DebugTasteText();
                break;

            case 1: //そもそも違うお菓子を納品

                sc.PlaySe(6);

                _baseMoney = basemoney_keisan();
                _getMoney = (int)(_baseMoney * 0.03f);
                _text.text = "ごめんなさい。ちょっとお菓子が違ってたみたい。" + "\n" + "次はちゃんと正しいものを持ってきてね。" + "\n" +
                    "お駄賃 " + GameMgr.ColorYellow + _getMoney + GameMgr.MoneyCurrency + "　</color>" + "を受け取った！";

                Debug.Log("納品失敗..");

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                //所持金をプラス
                moneyStatus_Controller.GetMoney(_getMoney); //アニメつき

                //名声値は減る
                //ninkiStatus_Controller.DegNinki(3); //アニメつき


                WhiteFadeCanvas.SetActive(false);

                ResetQuestStatus();
                break;

            /*case 2: //0点以下の場合

                sc.PlaySe(6);

                if (_a != "")
                {
                    _text.text = "評価: " + GameMgr.ColorYellow + okashi_totalscore + "</color>" + "点" + "\n" + 
                        "う～ん。ちょっと味がイマイチだったかも..。"  + "次は頑張ってね。";
                }
                else
                {
                    _text.text = "評価: " + GameMgr.ColorYellow + okashi_totalscore + "</color>" + "点" + "\n" + 
                        "う～ん。ちょっと味がイマイチだったかも..。" + "次は頑張ってね。";
                }

                Debug.Log("納品失敗..");

                //デバッグ用味採点テキスト
                DebugTasteText();

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                WhiteFadeCanvas.SetActive(false);
                sceneBGM.FadeInBGM();

                ResetQuestStatus();
                break;*/
        }

        GameMgr.System_shop_defaulttext_koushin = false;
    }

    void CostHosei_1()
    {
        if (okashi_totalscore >= 200 && okashi_totalscore < 250) //200~
        {
            _getMoney = (int)(_baseMoney * 1.2f + okashi_totalscore);
            debug_money_text = "(基準値 * 1.2f + okashi_totalscore)";
            _getNinki = 0;
            _kanso = "まるで宝石のようにすばらしい味らしいわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(1);
        }
        else if (okashi_totalscore >= 250 && okashi_totalscore < 300) //250~ ここから下ファンファーレ
        {
            _getMoney = (int)(_baseMoney * 1.3f + okashi_totalscore);
            debug_money_text = "(基準値 * 1.3f + okashi_totalscore)";
            _getNinki = 0;
            _kanso = "天使のような素晴らしい味らしいわ！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(1);
        }
        else if (okashi_totalscore >= 300 && okashi_totalscore < 500) //300~
        {
            _getMoney = (int)(_baseMoney * 1.5f + (okashi_totalscore * 1.1f));
            debug_money_text = "(基準値 * 1.4f + (okashi_totalscore * 1.1f))";
            _getNinki = 1;
            _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(3);
        }
        else if (okashi_totalscore >= 500 && okashi_totalscore < 1000) //500~
        {
            _getMoney = (int)(_baseMoney * 1.75f + (okashi_totalscore * 1.2f));
            debug_money_text = "(基準値 * 1.5f + (okashi_totalscore * 1.2f))";
            _getNinki = 1;
            _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(3);
        }
        else if (okashi_totalscore >= 1000) //1000~
        {
            _getMoney = (int)(_baseMoney * 2.00f + (okashi_totalscore * 1.3f));
            debug_money_text = "(基準値  * 2.00f + (okashi_totalscore * 1.3f))";
            _getNinki = 2;
            _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(5);
        }
    }

    void CostHosei_default()
    {
        if (okashi_totalscore >= 200 && okashi_totalscore < 250) //200~
        {
            _getMoney = (int)(_baseMoney * (okashi_totalscore / 200) * 1.3f);
            debug_money_text = "(基準値 * (okashi_totalscore / 200) * 1.3f)";
            _getNinki = 0;
            _kanso = "まるで宝石のようにすばらしい味らしいわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(1);
        }
        else if (okashi_totalscore >= 250 && okashi_totalscore < 300) //250~ ここから下ファンファーレ
        {
            _getMoney = (int)(_baseMoney * (okashi_totalscore / 200) * 1.5f);
            debug_money_text = "(基準値 * (okashi_totalscore / 200) * 1.5f)";
            _getNinki = 1;
            _kanso = "天使のような素晴らしい味らしいわ！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(1);
        }
        else if (okashi_totalscore >= 300 && okashi_totalscore < 500) //300~
        {
            _getMoney = (int)(_baseMoney * (okashi_totalscore / 200) * 1.75f);
            debug_money_text = "(基準値 * (okashi_totalscore / 200) * 1.75f)";
            _getNinki = 1;
            _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(3);
        }
        else if (okashi_totalscore >= 500 && okashi_totalscore < 1000) //500~
        {
            _getMoney = (int)(_baseMoney * (okashi_totalscore / 200) * 2.25f);
            debug_money_text = "(基準値 * (okashi_totalscore / 200) * 2.25f)";
            _getNinki = 1;
            _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(5);
        }
        else if (okashi_totalscore >= 1000) //1000~
        {
            _getMoney = (int)(_baseMoney * (okashi_totalscore / 200) * 3.0f);
            debug_money_text = "(基準値 * (okashi_totalscore / 200) * 3.0f)";
            _getNinki = 2;
            _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
            BarNPC_FriendPointUP(5);
        }

        if(_getMoney >= 30000) //30000超えた場合、上がりにくくなるよう補正
        {
            _getMoney = (int)(_getMoney * 0.7f);
        }

        if(_getMoney >= 999999) //ないとは思うけど、上限999999
        {
            _getMoney = 999999;
        }
    }

    int basemoney_keisan()
    {
        return _buy_price * _kosu_default + _slotmoney;
    }
    

    public void OnEndResultButton() //クエストリザルトボタンおすと、フラグがONに。各QuestResultPanelから呼び出しされる。
    {
        InitSetting();

        sc.PlaySe(2);

        endresultbutton = true;
        questResultPanel.SetActive(false);
        questResultPanel2.SetActive(false);

        //通貨のテキスト位置を元に戻しておく
        questResultPanel_tsukatext_pos.localPosition = questResultPanel_tsukatext_defpos;
        questResultPanel_tsukatext_pos2.localPosition = questResultPanel_tsukatext_defpos2;
    }

    IEnumerator EndQuestResultButton()
    {
        while (!endresultbutton)
        {
            yield return null;
        }

        endresultbutton = false;

        //所持金をプラス
        moneyStatus_Controller.GetMoney(_getMoney); //アニメつき  

        //ハートも少しプラス
        PlayerStatus.girl1_Love_exp += _getHeart;

        if (GameMgr.System_QuestStarGet_ON)
        {
            GameMgr.System_BarGetNinki = 0;

            //名声をプラスかマイナス。0は変化なし
            ninkiStatus_Controller.GetNinki(_getNinki);
            GameMgr.System_BarGetNinki = _getNinki;

            if (_getNinki > 0)
            {
                //もしスターをゲットしてた場合は、スターゲットの会話を表示
                barMain.StarGetEvent();
            }
        }

        //表情をもどす
        switch (GameMgr.Scene_Name)
        {
            case "Or_Bar_A1":

                character_01.transform.Find("Smile").gameObject.SetActive(false);
                break;

            case "Or_Bar_C1":

                character_03.transform.Find("Smile").gameObject.SetActive(false);
                break;

        }

        ResetQuestStatus();
    }

    void ResetQuestStatus()
    {
        //リスト更新
        shopquestlistController.NouhinList_DrawView();
        shopquestlistController.nouhin_select_on = 0;

        yes_no_panel.SetActive(false);

        questListToggle.interactable = true;
        nouhinToggle.interactable = true;

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        if(mute_on)
        {
            mute_on = false;
            sceneBGM.StopFanfare();
            sceneBGM.PlaySub();
        }

        switch (GameMgr.Scene_Category_Num)
        {
            case 30:

                GameMgr.Reset_SceneStatus = true;
                break;
           
        }
        
    }

    void BarNPC_FriendPointUP(int _point)
    {
        switch(_clientnum)
        {
            case 100: //100ルーティさん

                GameMgr.NPC_FriendPoint[40] += _point;
                break;

            case 101: //101アプリコットさん

                GameMgr.NPC_FriendPoint[41] += _point;
                break;

            default:

                GameMgr.NPC_BarFriendPoint[_clientnum] += _point;
                break;
        }
        
        Debug.Log("友好度アップ: " + _clientname + " " + _point + "上昇");
        Debug.Log("酒場NPC友好度は150点～から上がる");
    }

    void BarNPC_MeidoFriendPointUP(int _point) //酒場の店主の友好度上昇 現在は、マッサージかその人自身からのご依頼こなさないと上がらない
    {
        //ルーティのマッサージポイント
        switch (GameMgr.Scene_Name)
        {
            case "Or_Bar_A1":

                //100ルーティさん
                GameMgr.NPC_FriendPoint[40] += _point;
                break;

            case "Or_Bar_C1":

                //101アプリコットさん
                GameMgr.NPC_FriendPoint[41] += _point;
                break;

        }
    }

    void DebugTasteText()
    {
        debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();

        debug_taste_resultText.text =
            "###  好みの比較　結果　###"
            + "\n" + "\n" + "判定用お菓子セットの番号: " + _questID
            + "\n" + "\n" + "酒場の判定（固有=0, 女の子の好み=1）: " + _GirlJudgeUse
            + "\n" + "そのときの女の子判定番号: " + _girlset_id
            + "\n" + "\n" + "判定アイテム名: " + _itemname
            + "\n" + "判定アイテム名2: " + _itemname2
            + "\n" + "判定アイテム名3: " + _itemname3
            + "\n" + "判定アイテム名4: " + _itemname4
            + "\n" + "判定アイテム名5: " + _itemname5
            + "\n" + "判定アイテム名6: " + _itemname6
            + "\n" + "判定アイテム名7: " + _itemname7
            + "\n" + "判定アイテム名8: " + _itemname8
            + "\n" + "判定サブタイプ: " + _itemsubtype
            + "\n" + "\n" + "あまさ: " + _basesweat
            + "\n" + " お客さんの好みの甘さ: " + _sweat
            + "\n" + "お菓子のあまさ: " + _basesweat
            + "\n" + " 点数: " + sweat_score
            + "\n" + "\n" + "苦さ: " + _basebitter
            + "\n" + " お客さんの好みの苦さ: " + _bitter
            + "\n" + "お菓子のにがさ: " + _basebitter
            + "\n" + " 点数: " + bitter_score
            + "\n" + "\n" + "酸味: " + _basesour
            + "\n" + " お客さんの好みの酸味: " + _sour
            + "\n" + "お菓子の酸味: " + _basesour
            + "\n" + " 点数: " + sour_score
            + "\n" + "\n" + "さくさく度: " + _basecrispy + "\n" + "さくさく閾値: " + _crispy + "\n" + " 点数: " + crispy_score
            + "\n" + "\n" + "ふわふわ度: " + _basefluffy + "\n" + "ふわふわ閾値: " + _fluffy + "\n" + " 点数: " + fluffy_score
            + "\n" + "\n" + "なめらか度: " + _basesmooth + "\n" + "なめらか閾値: " + _smooth + "\n" + " 点数: " + smooth_score
            + "\n" + "\n" + "歯ごたえ度: " + _basehardness + "\n" + "歯ごたえ閾値: " + _hardness + "\n" + " 点数: " + hardness_score
            + "\n" + "\n" + "のどごし度: " + _basejuice + "\n" + "のどごし閾値: " + _juice + "\n" + " 点数: " + juice_score
            + "\n" + "\n" + "香り: " + _basetea_flavor + "\n" + "香り閾値: " + _tea_flavor + "\n" + " 点数: " + tea_flavor_score
            + "\n" + "\n" + "ぷるぷる度: " + "-"
            + "\n" + "\n" + "噛み応え度: " + "-"
            + "\n" + "\n" + "トッピングスコア: " + topping_score
            + "\n" + "\n" + "特殊値の加算（女の子の好み=1の時のみ）: " + spscore_total
            + "\n" + "\n" + "指定のトッピングあったかどうか falseで-50点: " + slot_ok
            + "\n" + "\n" + "お菓子の見た目: " + _basebeauty + "\n" + "見た目閾値: " + _beauty + "\n" + "見た目スコア: " + beauty_score
            + "\n" + "\n" + "補正値　無条件で点数を下げる: " + Hosei_score
            + "\n" + "\n" + "総合得点: " + okashi_score
            + "\n" + "\n" + "### ###"
            + "\n" + "\n" + "お金の取得式: " + "\n" + debug_money_text
            + "\n" + "\n" + "基準値(_buy_price * _kosu_default + _slotmoney): " + _baseMoney
            + "\n" + "\n" + "_slotmoney: " + _slotmoney
            + "\n" + "\n" + "マジックスロットでお金追加(_MSMoney): " + _MSMoney
            //+ "\n" + "\n" + "okashi_totalscore / GameMgr.high_score 計算: "
            + "\n" + "\n" + "お金の取得合計: " + _getMoney
            + "\n" + "\n" + "ハートの取得合計: " + _getHeart;
    }

    //
    //パラメータのセットアップ
    //
    void SetInitQItem(int _count)
    {
        // 判定用に依頼のお菓子のパラメータを代入
        _Qid = quest_database.questTakeset[_count]._ID;              //基本判定のときは、使わない
        _questID = quest_database.questTakeset[_count].Quest_ID;    //基本判定のときは、使わない

        _itemname = quest_database.questTakeset[_count].Quest_itemName;
        _itemname2 = quest_database.questTakeset[_count].Quest_itemName2;
        _itemname3 = quest_database.questTakeset[_count].Quest_itemName3;
        _itemname4 = quest_database.questTakeset[_count].Quest_itemName4;
        _itemname5 = quest_database.questTakeset[_count].Quest_itemName5;
        _itemname6 = quest_database.questTakeset[_count].Quest_itemName6;
        _itemname7 = quest_database.questTakeset[_count].Quest_itemName7;
        _itemname8 = quest_database.questTakeset[_count].Quest_itemName8;
        _itemsubtype = quest_database.questTakeset[_count].Quest_itemSubtype;

        _kosu_min = quest_database.questTakeset[_count].Quest_kosu_min;
        _kosu_max = quest_database.questTakeset[_count].Quest_kosu_max;

        _kosu_default = quest_database.questTakeset[_count].Quest_kosu_default;
        _buy_price = quest_database.questTakeset[_count].Quest_buy_price;

        _rich = quest_database.questTakeset[_count].Quest_rich;
        _sweat = quest_database.questTakeset[_count].Quest_sweat;
        _bitter = quest_database.questTakeset[_count].Quest_bitter;
        _sour = quest_database.questTakeset[_count].Quest_sour;

        _crispy = quest_database.questTakeset[_count].Quest_crispy;
        _fluffy = quest_database.questTakeset[_count].Quest_fluffy;
        _smooth = quest_database.questTakeset[_count].Quest_smooth;
        _hardness = quest_database.questTakeset[_count].Quest_hardness;
        _jiggly = quest_database.questTakeset[_count].Quest_jiggly;
        _chewy = quest_database.questTakeset[_count].Quest_chewy;

        _juice = quest_database.questTakeset[_count].Quest_juice;
        _beauty = quest_database.questTakeset[_count].Quest_beauty;
        _tea_flavor = quest_database.questTakeset[_count].Quest_tea_flavor;

        _clientname = quest_database.questTakeset[_count].Quest_ClientName;
        _clientnum = quest_database.questTakeset[_count].Quest_ClientNumber;

        for (i = 0; i < _tp.Length; i++)
        {
            _tp[i] = quest_database.questTakeset[_count].Quest_topping[i];
            _tp_score[i] = quest_database.questTakeset[_count].Quest_tp_score[i];
        }


        //一回まず各スコアを初期化。
        for (i = 0; i < itemslot_NouhinScore.Count; i++)
        {
            itemslot_NouhinScore[i] = 0;
            itemslot_NouhinAddPoint[i] = 0;
        }

        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _tp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_tp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslot_NouhinScore[count]++;
                    itemslot_NouhinAddPoint[count] = _tp_score[i];
                }
                count++;
            }
        }
    }

    void SetInitQItemGirlJudge(int _count, string _itemName)
    {
        // 判定用に依頼のお菓子のパラメータを代入
        _Qid = quest_database.questTakeset[_count]._ID;              //基本判定のときは、使わない
        _questID = quest_database.questTakeset[_count].Quest_ID;    //基本判定のときは、使わない

        _itemname = quest_database.questTakeset[_count].Quest_itemName;
        _itemname2 = quest_database.questTakeset[_count].Quest_itemName2;
        _itemname3 = quest_database.questTakeset[_count].Quest_itemName3;
        _itemname4 = quest_database.questTakeset[_count].Quest_itemName4;
        _itemname5 = quest_database.questTakeset[_count].Quest_itemName5;
        _itemname6 = quest_database.questTakeset[_count].Quest_itemName6;
        _itemname7 = quest_database.questTakeset[_count].Quest_itemName7;
        _itemname8 = quest_database.questTakeset[_count].Quest_itemName8;
        _itemsubtype = quest_database.questTakeset[_count].Quest_itemSubtype;

        _kosu_min = quest_database.questTakeset[_count].Quest_kosu_min;
        _kosu_max = quest_database.questTakeset[_count].Quest_kosu_max;

        _kosu_default = quest_database.questTakeset[_count].Quest_kosu_default;
        _buy_price = quest_database.questTakeset[_count].Quest_buy_price;


        //女の子の好み判定を使用 名前をもとにItemDBから判定用番号を取得し、それをgirlsetDBから探して入れる
        _girlset_id = database.items[database.SearchItemIDString(_itemName)].SetJudge_Num;

        //お菓子の判定値をセッティング
        girl1_status.InitializeStageGirlHungrySet(_girlset_id, 0, 0); //compNum, セットする配列番号　の順　
        //_girlset_listid = girlLikeSet_database.SearchSetID(_girlset_id);

        _rich = girl1_status.girl1_Rich[0];
        _sweat = girl1_status.girl1_Sweat[0];
        _bitter = girl1_status.girl1_Bitter[0];
        _sour = girl1_status.girl1_Sour[0];

        _crispy = girl1_status.girl1_Crispy[0];
        _fluffy = girl1_status.girl1_Fluffy[0];
        _smooth = girl1_status.girl1_Smooth[0];
        _hardness = girl1_status.girl1_Hardness[0];
        _jiggly = girl1_status.girl1_Jiggly[0];
        _chewy = girl1_status.girl1_Chewy[0];

        _juice = girl1_status.girl1_Juice[0];
        _beauty = girl1_status.girl1_Beauty[0];
        _tea_flavor = girl1_status.girl1_Tea_Flavor[0];

        sp1_wind = girl1_status.girl1_SP1_Wind[0];
        sp_score2 = girl1_status.girl1_SP_Score2[0];
        sp_score3 = girl1_status.girl1_SP_Score3[0];
        sp_score4 = girl1_status.girl1_SP_Score4[0];
        sp_score5 = girl1_status.girl1_SP_Score5[0];
        sp_score6 = girl1_status.girl1_SP_Score6[0];
        sp_score7 = girl1_status.girl1_SP_Score7[0];
        sp_score8 = girl1_status.girl1_SP_Score8[0];
        sp_score9 = girl1_status.girl1_SP_Score9[0];
        sp_score10 = girl1_status.girl1_SP_Score10[0];

        _clientname = quest_database.questTakeset[_count].Quest_ClientName;
        _clientnum = quest_database.questTakeset[_count].Quest_ClientNumber;

        for (i = 0; i < _tp.Length; i++)
        {
            _tp[i] = girlLikeSet_database.girllikeset[_girlset_listid].girlLike_topping[i];
            _tp_score[i] = girlLikeSet_database.girllikeset[_girlset_listid].girlLike_topping_score[i];
        }


        //一回まず各スコアを初期化。
        for (i = 0; i < itemslot_NouhinScore.Count; i++)
        {
            itemslot_NouhinScore[i] = 0;
            itemslot_NouhinAddPoint[i] = 0;
        }

        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _tp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_tp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslot_NouhinScore[count]++;
                    itemslot_NouhinAddPoint[count] = _tp_score[i];
                }
                count++;
            }
        }
    }

    void SetInitNouhinItem(int _count_n)
    {
        // 判定用に依頼のお菓子のパラメータを代入
        itemType = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggleitem_type;

        switch (itemType)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                _id = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggle_originplist_ID;

                //各パラメータを取得
                _basename = database.items[_id].itemName;
                _basehp = database.items[_id].itemHP;
                _baseday = database.items[_id].item_day;
                _basequality = database.items[_id].Quality;
                _baseexp = 0; //元アイテムの経験値は影響なし。材料のみの経験値を加算する。
                _baseprobability = database.items[_id].Ex_Probability;
                _baserich = database.items[_id].Rich;
                _basesweat = database.items[_id].Sweat;
                _basebitter = database.items[_id].Bitter;
                _basesour = database.items[_id].Sour;
                _basecrispy = database.items[_id].Crispy;
                _basefluffy = database.items[_id].Fluffy;
                _basesmooth = database.items[_id].Smooth;
                _basehardness = database.items[_id].Hardness;
                _basejuice = database.items[_id].Juice;
                _basejiggly = database.items[_id].Jiggly;
                _basechewy = database.items[_id].Chewy;
                _basepowdery = database.items[_id].Powdery;
                _baseoily = database.items[_id].Oily;
                _basewatery = database.items[_id].Watery;
                _basebeauty = database.items[_id].Beauty;
                _basetea_flavor = database.items[_id].Tea_Flavor;
                _base_sp_wind = database.items[_id].SP_wind;
                _base_sp_score2 = database.items[_id].SP_Score2;
                _base_sp_score3 = database.items[_id].SP_Score3;
                _base_sp_score4 = database.items[_id].SP_Score4;
                _base_sp_score5 = database.items[_id].SP_Score5;
                _base_sp_score6 = database.items[_id].SP_Score6;
                _base_sp_score7 = database.items[_id].SP_Score7;
                _base_sp_score8 = database.items[_id].SP_Score8;
                _base_sp_score9 = database.items[_id].SP_Score9;
                _base_sp_score10 = database.items[_id].SP_Score10;
                _basescore = database.items[_id].Base_Score;
                _basegirl1_like = database.items[_id].girl1_itemLike;
                _basecost = database.items[_id].cost_price;
                _basesell = database.items[_id].sell_price;
                _base_itemType = database.items[_id].itemType.ToString();
                _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                _base_itemType_subB = database.items[_id].itemType_subB.ToString();
                _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                _base_item_hyouji = database.items[_id].item_Hyouji;

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = database.items[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = database.items[_id].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
                {
                    _baseMS[i] = database.items[_id].item_MagicSlot[i].ToString();
                    _baseMSvalue[i] = database.items[_id].item_MagicSlotValue[i];
                }

                break;

            case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                _id = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggle_originplist_ID;

                //各パラメータを取得
                _basename = pitemlist.player_originalitemlist[_id].itemName;
                _basehp = pitemlist.player_originalitemlist[_id].itemHP;
                _baseday = pitemlist.player_originalitemlist[_id].item_day;
                _basequality = pitemlist.player_originalitemlist[_id].Quality;
                _baseexp = pitemlist.player_originalitemlist[_id].Exp;
                _baseprobability = pitemlist.player_originalitemlist[_id].Ex_Probability;
                _baserich = pitemlist.player_originalitemlist[_id].Rich;
                _basesweat = pitemlist.player_originalitemlist[_id].Sweat;
                _basebitter = pitemlist.player_originalitemlist[_id].Bitter;
                _basesour = pitemlist.player_originalitemlist[_id].Sour;
                _basecrispy = pitemlist.player_originalitemlist[_id].Crispy;
                _basefluffy = pitemlist.player_originalitemlist[_id].Fluffy;
                _basesmooth = pitemlist.player_originalitemlist[_id].Smooth;
                _basehardness = pitemlist.player_originalitemlist[_id].Hardness;
                _basejuice = pitemlist.player_originalitemlist[_id].Juice;
                _basejiggly = pitemlist.player_originalitemlist[_id].Jiggly;
                _basechewy = pitemlist.player_originalitemlist[_id].Chewy;
                _basepowdery = pitemlist.player_originalitemlist[_id].Powdery;
                _baseoily = pitemlist.player_originalitemlist[_id].Oily;
                _basewatery = pitemlist.player_originalitemlist[_id].Watery;
                _basebeauty = pitemlist.player_originalitemlist[_id].Beauty;
                _basetea_flavor = pitemlist.player_originalitemlist[_id].Tea_Flavor;
                _base_sp_wind = pitemlist.player_originalitemlist[_id].SP_wind;
                _base_sp_score2 = pitemlist.player_originalitemlist[_id].SP_Score2;
                _base_sp_score3 = pitemlist.player_originalitemlist[_id].SP_Score3;
                _base_sp_score4 = pitemlist.player_originalitemlist[_id].SP_Score4;
                _base_sp_score5 = pitemlist.player_originalitemlist[_id].SP_Score5;
                _base_sp_score6 = pitemlist.player_originalitemlist[_id].SP_Score6;
                _base_sp_score7 = pitemlist.player_originalitemlist[_id].SP_Score7;
                _base_sp_score8 = pitemlist.player_originalitemlist[_id].SP_Score8;
                _base_sp_score9 = pitemlist.player_originalitemlist[_id].SP_Score9;
                _base_sp_score10 = pitemlist.player_originalitemlist[_id].SP_Score10;
                _basescore = pitemlist.player_originalitemlist[_id].Base_Score;
                _basegirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
                _basecost = pitemlist.player_originalitemlist[_id].cost_price;
                _basesell = pitemlist.player_originalitemlist[_id].sell_price;
                _base_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
                _base_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();
                _base_itemType_subB = pitemlist.player_originalitemlist[_id].itemType_subB.ToString();
                _base_extreme_kaisu = pitemlist.player_originalitemlist[_id].ExtremeKaisu;
                _base_item_hyouji = pitemlist.player_originalitemlist[_id].item_Hyouji;

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_originalitemlist[_id].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
                {
                    _baseMS[i] = pitemlist.player_originalitemlist[_id].item_MagicSlot[i].ToString();
                    _baseMSvalue[i] = pitemlist.player_originalitemlist[_id].item_MagicSlotValue[i];
                }
                break;

            case 2: //お菓子パネル設定アイテムリストから選択している場合

                _id = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggle_originplist_ID;

                //各パラメータを取得
                _basename = pitemlist.player_extremepanel_itemlist[_id].itemName;
                _basehp = pitemlist.player_extremepanel_itemlist[_id].itemHP;
                _baseday = pitemlist.player_extremepanel_itemlist[_id].item_day;
                _basequality = pitemlist.player_extremepanel_itemlist[_id].Quality;
                _baseexp = pitemlist.player_extremepanel_itemlist[_id].Exp;
                _baseprobability = pitemlist.player_extremepanel_itemlist[_id].Ex_Probability;
                _baserich = pitemlist.player_extremepanel_itemlist[_id].Rich;
                _basesweat = pitemlist.player_extremepanel_itemlist[_id].Sweat;
                _basebitter = pitemlist.player_extremepanel_itemlist[_id].Bitter;
                _basesour = pitemlist.player_extremepanel_itemlist[_id].Sour;
                _basecrispy = pitemlist.player_extremepanel_itemlist[_id].Crispy;
                _basefluffy = pitemlist.player_extremepanel_itemlist[_id].Fluffy;
                _basesmooth = pitemlist.player_extremepanel_itemlist[_id].Smooth;
                _basehardness = pitemlist.player_extremepanel_itemlist[_id].Hardness;
                _basejuice = pitemlist.player_extremepanel_itemlist[_id].Juice;
                _basejiggly = pitemlist.player_extremepanel_itemlist[_id].Jiggly;
                _basechewy = pitemlist.player_extremepanel_itemlist[_id].Chewy;
                _basepowdery = pitemlist.player_extremepanel_itemlist[_id].Powdery;
                _baseoily = pitemlist.player_extremepanel_itemlist[_id].Oily;
                _basewatery = pitemlist.player_extremepanel_itemlist[_id].Watery;
                _basebeauty = pitemlist.player_extremepanel_itemlist[_id].Beauty;
                _basetea_flavor = pitemlist.player_extremepanel_itemlist[_id].Tea_Flavor;
                _base_sp_wind = pitemlist.player_extremepanel_itemlist[_id].SP_wind;
                _base_sp_score2 = pitemlist.player_extremepanel_itemlist[_id].SP_Score2;
                _base_sp_score3 = pitemlist.player_extremepanel_itemlist[_id].SP_Score3;
                _base_sp_score4 = pitemlist.player_extremepanel_itemlist[_id].SP_Score4;
                _base_sp_score5 = pitemlist.player_extremepanel_itemlist[_id].SP_Score5;
                _base_sp_score6 = pitemlist.player_extremepanel_itemlist[_id].SP_Score6;
                _base_sp_score7 = pitemlist.player_extremepanel_itemlist[_id].SP_Score7;
                _base_sp_score8 = pitemlist.player_extremepanel_itemlist[_id].SP_Score8;
                _base_sp_score9 = pitemlist.player_extremepanel_itemlist[_id].SP_Score9;
                _base_sp_score10 = pitemlist.player_extremepanel_itemlist[_id].SP_Score10;
                _basescore = pitemlist.player_extremepanel_itemlist[_id].Base_Score;
                _basegirl1_like = pitemlist.player_extremepanel_itemlist[_id].girl1_itemLike;
                _basecost = pitemlist.player_extremepanel_itemlist[_id].cost_price;
                _basesell = pitemlist.player_extremepanel_itemlist[_id].sell_price;
                _base_itemType = pitemlist.player_extremepanel_itemlist[_id].itemType.ToString();
                _base_itemType_sub = pitemlist.player_extremepanel_itemlist[_id].itemType_sub.ToString();
                _base_itemType_subB = pitemlist.player_extremepanel_itemlist[_id].itemType_subB.ToString();
                _base_extreme_kaisu = pitemlist.player_extremepanel_itemlist[_id].ExtremeKaisu;
                _base_item_hyouji = pitemlist.player_extremepanel_itemlist[_id].item_Hyouji;

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_extremepanel_itemlist[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_extremepanel_itemlist[_id].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].item_MagicSlot.Length; i++)
                {
                    _baseMS[i] = pitemlist.player_extremepanel_itemlist[_id].item_MagicSlot[i].ToString();
                    _baseMSvalue[i] = pitemlist.player_extremepanel_itemlist[_id].item_MagicSlotValue[i];
                }
                break;
        }

        //一回まず各スコアを初期化。とっぴんぐ・固有トッピングで、共通のリスト
        for (i = 0; i < itemslot_PitemScore.Count; i++)
        {
            itemslot_PitemScore[i] = 0;
        }


        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslot_PitemScore[count]++;
                }
                count++;
            }
        }

        //固有トッピングスロットも見る。一致する効果があれば、所持数+1。現在未使用。
        for (i = 0; i < _koyutp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_koyutp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log("_koyutp: " + _koyutp[i]);
                    itemslot_PitemScore[count]++;
                }
                count++;
            }
        }
    }

    void InitializeItemSlotDicts()
    {
        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslot_NouhinScore.Add(0); //納品アイテムの必要スロット所持数
            itemslot_NouhinAddPoint.Add(0); //スロット追加点
            itemslot_PitemScore.Add(0); //選択したアイテムのスロット所持数
        }
    }

    void SetHintText()
    {

        //ヒントを表示する。０のものは、判定なしなので、表示もしない。
        _sweat_kansou = "";
        _bitter_kansou = "";
        _sour_kansou = "";

        if (sweat_level != 0)
        {
            SweatHintHyouji();
        }
        if (bitter_level != 0)
        {
            BitterHintHyouji();
        }
        if (sour_level != 0)
        {
            SourHintHyouji();
        }

        HintText.text = _a + _sweat_kansou + _bitter_kansou + _sour_kansou + _b;
    }

    void SweatHintHyouji()
    {
        //甘さがどの程度好みにあっていたかを、感想でいう。８はピッタリパーフェクト。
        if (sweat_level == 8)
        {
            _sweat_kansou = "甘さ S: 神の甘さ！ パーフェクト！！";
        }
        else if (sweat_level == 7)
        {
            _sweat_kansou = "甘さ A+: 絶妙な甘さ！";
        }
        else if (sweat_level == 6)
        {
            _sweat_kansou = "甘さ A: 甘さ、素晴らしい具合！";
        }
        else if (sweat_level == 5)
        {
            _sweat_kansou = "甘さ B: いい感じの甘さ";
        }
        else if (sweat_level == 4)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = "甘さ C: 甘さがちょっと足りない";
            }
            else
            {
                _sweat_kansou = "甘さ C: 少し甘いかも？";
            }
        }
        else if (sweat_level == 3)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = "甘さ D: 甘さが足りない";
            }
            else
            {
                _sweat_kansou = "甘さ D: 甘さがちょっと強すぎ";
            }
        }
        else if (sweat_level >= 1 && sweat_level <= 2)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = GameMgr.ColorRedDeep + "甘さ F: 甘さが全然足りない" + "</color>";
            }
            else
            {
                _sweat_kansou = GameMgr.ColorRedDeep + "甘さ F: 甘すぎ" + "</color>";
            }
        }
        else
        {
            _sweat_kansou = "";
        }

        if (sweat_level != 0)
        {
            _sweat_kansou = "\n" + _sweat_kansou;
        }
        else
        {

        }
    }

    void BitterHintHyouji()
    {
        //苦さがどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (bitter_level == 8)
        {
            _bitter_kansou = "苦さ S: 神の苦さ！ パーフェクト！！";
        }
        else if (bitter_level == 7)
        {
            _bitter_kansou = "苦さ A+: 絶妙な苦さ！";
        }
        else if (bitter_level == 6)
        {
            _bitter_kansou = "苦さ A: 苦さ、すばらしい！";
        }
        else if (bitter_level == 5)
        {
            _bitter_kansou = "苦さ B: いい感じの苦さ";
        }
        else if (bitter_level == 4)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = "苦さ C: 苦さがちょっと足りない";
            }
            else
            {
                _bitter_kansou = "苦さ C: 少し苦いかも？";
            }

        }
        else if (bitter_level == 3)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = "苦さ D:苦さが足りない";
            }
            else
            {
                _bitter_kansou = "苦さ D: 苦みが少し強すぎかも。";
            }

        }
        else if (bitter_level >= 1 && bitter_level <= 2)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = GameMgr.ColorRedDeep + "苦さ F: 苦さが全然足りない" + "</color>";
            }
            else
            {
                _bitter_kansou = GameMgr.ColorRedDeep + "苦さ F: 苦すぎ..。" + "</color>";
            }

        }
        else
        {
            _bitter_kansou = "";
        }

        if (bitter_level != 0)
        {
            _bitter_kansou = "\n" + _bitter_kansou;
        }
        else
        {

        }
    }

    void SourHintHyouji()
    {
        //酸味がどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (sour_level == 8)
        {
            _sour_kansou = "酸味 S: 神のすっぱさ！ パーフェクト！！";
        }
        else if (sour_level == 7)
        {
            _sour_kansou = "酸味 A+: 絶妙なすっぱさ！";
        }
        else if (sour_level == 6)
        {
            _sour_kansou = "酸味 A: すっぱさ、すばらしい！";
        }
        else if (sour_level == 5)
        {
            _sour_kansou = "酸味 B: いい感じのすっぱさ";
        }
        else if (sour_level == 4)
        {
            if (sour_result < 0)
            {
                _sour_kansou = "酸味 C: すっぱさちょっと足りない";
            }
            else
            {
                _sour_kansou = "酸味 C: 少しすっぱいかも？";
            }

        }
        else if (sour_level == 3)
        {
            if (sour_result < 0)
            {
                _sour_kansou = "酸味 D: すっぱさが足りない";
            }
            else
            {
                _sour_kansou = "酸味 D: 少しすっぱ過ぎる？";
            }

        }
        else if (sour_level >= 1 && sour_level <= 2)
        {
            if (sour_result < 0)
            {
                _sour_kansou = GameMgr.ColorRedDeep + "酸味 F: 全然すっぱさがない" + "</color>";
            }
            else
            {
                _sour_kansou = GameMgr.ColorRedDeep + "酸味 F: すっぺぇ..。" + "</color>";
            }

        }
        else
        {
            _sour_kansou = "";
        }

        if (sour_level != 0)
        {
            _sour_kansou = "\n" + _sour_kansou;
        }
        else
        {

        }
    }

    //入れた数字の桁数を取得する
    public int Digit(int num)
    {
        int digit = 1;
        for (int i = num; i >= 10; i /= 10)
        {
            digit++;
        }
        return digit;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}

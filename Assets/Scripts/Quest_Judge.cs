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

    private GameObject shopMain_obj;
    private Shop_Main shopMain;
    private GameObject barMain_obj;
    private Bar_Main barMain;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject NinkiStatus_Panel_obj;
    private NinkiStatus_Controller ninkiStatus_Controller;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;
    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private Toggle questListToggle;
    private Toggle nouhinToggle;

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

    private GameObject black_effect;

    private Text debug_taste_resultText;

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

    private int _getMoney;
    private int _getNinki;
    private string _kanso;

    private int _id;
    private int _Qid;
    private int _questID;
    private int _qitemID;

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

    private string[] _tp;
    private int[] _tp_score;

    private string _a;
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
    private int _basescore;
    private float _basegirl1_like;
    private int _basecost;
    private int _basesell;
    private string[] _basetp;
    private string[] _koyutp;
    private string _base_itemType;
    private string _base_itemType_sub;
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
    private float _score_deg;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject WhiteFadeCanvas;

    //時間
    private float timeOut;

    private GameObject eat_hukidashiPrefab;
    private GameObject eat_hukidashiitem;
    private Text eat_hukidashitext;

    private GameObject character;

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

        switch (SceneManager.GetActiveScene().name)
        {
            case "Bar":

                barMain_obj = GameObject.FindWithTag("Bar_Main");
                barMain = barMain_obj.GetComponent<Bar_Main>();

                //名声パネルの取得
                NinkiStatus_Panel_obj = canvas.transform.Find("NinkiStatus_panel").gameObject;
                ninkiStatus_Controller = NinkiStatus_Panel_obj.GetComponent<NinkiStatus_Controller>();

                WhiteFadeCanvas = canvas.transform.Find("WhiteFadeCanvas").gameObject;

                questResultPanel2 = canvas.transform.Find("QuestResultPanel2").gameObject;
                questResultPanel2.SetActive(false);
                HintText = questResultPanel2.transform.Find("QuestResultImage/HintText").GetComponent<Text>();
                questResultPanel_tsukatext_pos2 = questResultPanel2.transform.Find("QuestResultImage/MoneyTsukaText").transform;
                questResultPanel_tsukatext_defpos2 = questResultPanel_tsukatext_pos2.localPosition;
                break;

            case "Shop":

                shopMain_obj = GameObject.FindWithTag("Shop_Main");
                shopMain = shopMain_obj.GetComponent<Shop_Main>();
                break;
        }
                       

        shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();
        back_ShopFirst_obj = shopquestlistController_obj.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        questListToggle = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_QuestList").GetComponent<Toggle>();
        nouhinToggle = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_Nouhin").GetComponent<Toggle>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes = yes_no_panel.transform.Find("Yes").gameObject;
        no = yes_no_panel.transform.Find("No").gameObject;
        

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        text_area = canvas.transform.Find("MessageWindow").gameObject; ; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

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
        _tp = new string[quest_database.questset[0].Quest_topping.Length];
        _tp_score = new int[quest_database.questset[0].Quest_tp_score.Length];

        InitializeItemSlotDicts();

        judge_anim_on = false;
        judge_anim_status = 0;
        judge_end = false;

        //クエストリザルトパネル
        questResultPanel = canvas.transform.Find("QuestResultPanel").gameObject;
        questResultPanel.SetActive(false);
        

        questResultPanel_tsukatext_pos = questResultPanel.transform.Find("QuestResultImage/MoneyTsukaText").transform;
        questResultPanel_tsukatext_defpos = questResultPanel_tsukatext_pos.localPosition;

        
        endresultbutton = false;
        mute_on = false;
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
                    back_ShopFirst_obj.SetActive(false);

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

                    _text.text = "鑑定中.";

                    break;

                case 1: // 状態2

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.5f;
                        judge_anim_status = 2;

                        eat_hukidashitext.text = ". .";

                        _text.text = "鑑定中. .";

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

                            sceneBGM.FadeOutBGM();
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

                    if (GameMgr.Story_Mode != 0)
                    {
                        NinkiStatus_Panel_obj.SetActive(true);
                    }
                    //text_area.SetActive(true);


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
        _qitemID = _ID;

        SetInitQItem(_qitemID);

        nouhinOK_flag = false;
        deleteOriginalList.Clear();
        deleteExtremeList.Clear();

        _getNinki = 0;
        _getMoney = 0;

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

                //back_ShopFirst_btn.interactable = true;
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
            }
        }
         
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

        _getMoney = _buy_price * _kosu_default;
        _getNinki = 1;

        //足りてるので、納品完了の処理
        _text.text = "報酬 " + GameMgr.ColorYellow + _getMoney + "</color>" + GameMgr.MoneyCurrency + " を受け取った！" + "\n" + "ありがとう！お客さんもとても喜んでいるわ！";

        //該当のクエストを削除
        quest_database.questTakeset.RemoveAt(_qitemID);

        Debug.Log("納品完了！");

        //ジャキーンみたいな音を鳴らす。
        //sc.PlaySe(4);
        sc.PlaySe(76);
        sc.PlaySe(31);

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
    //クッキーなどの判定するお菓子を納品した場合の処理
    //
    public void Okashi_Judge(int _ID)
    {
        _qitemID = _ID;

        Okashi_Judge_Method();
    }
   

    void Okashi_Judge_Method()
    {
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        SetInitQItem(_qitemID); //依頼アイテムのパラメータを代入

        deleteOriginalList.Clear();
        deleteExtremeList.Clear();
        result_OkashiScore.Clear();
        okashi_totalscore = 0;
        okashi_totalkosu = 0;
        _getNinki = 0;
        _getMoney = 0;

        set_kaisu = pitemlistController._listcount.Count;

        //listcount分 提出するアイテムのパラメータを判定する。
        for (list_count = 0; list_count < set_kaisu; list_count++)
        {
            //選択したアイテムのデータをセット
            SetInitNouhinItem(pitemlistController._listcount[list_count]);

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
            HintText.text = "";

            //①指定のトッピングがあるかをチェック。一つでも指定のものがあれば、OK

            nouhinOK_status = 2; //先にNGはたてておく。
            slot_ok = true;

            //納品用スコアがすべて０の場合、トッピングを計算しないので、無視する。
            for (i=0; i < itemslot_NouhinScore.Count; i++)
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
            }


            //②味パラメータの計算。GirlEat_Judgeのをそのまま流用。

            rich_result = _baserich - _rich;
            sweat_result = _basesweat - _sweat;
            bitter_result = _basebitter - _bitter;
            sour_result = _basesour - _sour;

            //rich_score = girlEat_judge.TasteKeisanBase(_rich, rich_result, "味のコク: "); //クエストの値, お菓子の値-クエストの値, デバッグ表示用。返り値は、点数。

            sweat_score = girlEat_judge.TasteKeisanBase(_sweat, sweat_result, "甘味: "); //クエストの値, お菓子の値-クエストの値, デバッグ表示用。返り値は、点数。
            sweat_level = girlEat_judge.TasteLevel_Keisan(_sweat, sweat_score);

            bitter_score = girlEat_judge.TasteKeisanBase(_bitter, bitter_result, "苦み: ");
            bitter_level = girlEat_judge.TasteLevel_Keisan(_bitter, bitter_score);

            sour_score = girlEat_judge.TasteKeisanBase(_sour, sour_result, "酸味: ");
            sour_level = girlEat_judge.TasteLevel_Keisan(_sour, sour_score);

            //書き方が少し違うけど、GirlEat_Judgeでやってることとほぼ一緒
            if (_crispy > 0)
            {
                _temp_kyori = _basecrispy - _crispy;

                if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                {
                    _temp_ratio = 1.0f;
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    crispy_score = (int)(_basescore * _temp_ratio) + _temp_kyori;
                    _a = "さくさく感がいい感じだわ";
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
                    _a = "ふんわり感がいい感じだわ";
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
                    _a = "なめらかさはいい感じだわ";
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

                    hardness_score = (int)(_basescore * _temp_ratio) + _temp_kyori;
                    _a = "歯ごたえがいい感じだわ";
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
                    _a = "ぷにぷに感がいい感じだわ";
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
                    _a = "噛みごたえがいい感じだわ";
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
                    _a = "のどごしがいいね。";
                }
                else
                {
                    _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                    Debug.Log("_temp_ratio: " + _temp_ratio);

                    juice_score = (int)(_basescore * _temp_ratio);
                    _a = "のどごしがちょっと足りない。";
                }
            }

            if(_base_itemType_sub == "Tea" || _base_itemType_sub == "Tea_Potion" || _base_itemType_sub == "Coffee_Mat")
            {
                if (_crispy > 0)
                {
                    _temp_kyori = _basecrispy - _crispy;

                    if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
                    {
                        _temp_ratio = 1.0f;
                        Debug.Log("_temp_ratio: " + _temp_ratio);

                        crispy_score = (int)(_basescore * _temp_ratio) + _temp_kyori;
                        _a = "香りがいい感じだわ";
                    }
                    else
                    {
                        _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                        Debug.Log("_temp_ratio: " + _temp_ratio);

                        crispy_score = (int)(_basescore * _temp_ratio);
                        _a = "香りがちょっと足りない。";
                    }
                }
            }

            //特定のお菓子の判定。一致していない場合は、③は計算するまでもなく不正解となる。
            if (_itemname == "Non") //特に指定なし
            {
                //③お菓子の種別の計算
                if (_itemsubtype == "Non") //特に指定なし
                {

                }
                else if (_itemsubtype == _base_itemType_sub) //お菓子の種別が一致している。
                {

                }
                else
                {
                    //不正解。そもそも違うお菓子を納品している。
                    nouhinOK_status = 1;
                }
            }
            else if (_itemname == _basename) //お菓子の名前が一致している。
            {
                //サブは計算せず、特定のお菓子自体が正解なら、正解

            }
            else
            {
                //不正解。そもそも違うお菓子を納品している。
                nouhinOK_status = 1;
            }

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
                        _basebeauty += slotnamedatabase.slotname_lists[i].slot_Beauty * itemslot_PitemScore[i]; //見た目に対するボーナス得点
                    }
                }
            }

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

            //見た目点数の計算
            if (_beauty > 0)
            {
                beauty_score = (_basebeauty - _beauty);
            }

            //最終補正　妹の基準より、やや厳しめにするために、点数を下げる。
            Hosei_score = -15;

            //総合点数を計算
            okashi_score += sweat_score + bitter_score + sour_score +
                crispy_score + fluffy_score + smooth_score + hardness_score + jiggly_score + chewy_score +
                juice_score + beauty_score + topping_score + Hosei_score;

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

                _score_deg = 1.0f * okashi_totalscore / GameMgr.high_score;                

                if (okashi_totalscore < 30) //粗悪なお菓子だと、マイナス評価
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 0.2f);
                    debug_money_text = "(基準値 * 0.2f)";
                    _getNinki = -5;
                    _kanso = "う～ん..。お客さん不満だったみたい。次からは気をつけてね。" + "\n" + "報酬額を少し減らされてしまった！";
                    
                }
                else if (okashi_totalscore >= 30 && okashi_totalscore < 45) //30~45
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 0.4f);
                    debug_money_text = "(基準値 * 0.4f)";
                    _getNinki = -1;
                    _kanso = "ありがとう。　..少しお客さん不満だったみたい。" + "\n" + "次はもっと期待してるわね！";
                }
                else if (okashi_totalscore >= 45 && okashi_totalscore < GameMgr.low_score) //45~60
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 0.8f);
                    debug_money_text = "(基準値 * 0.8f)";
                    _getNinki = 1;
                    _kanso = "ありがとう！　お客さん喜んでたわ！";
                }
                else if (okashi_totalscore >= GameMgr.low_score && okashi_totalscore < 80) //60~80
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 1.1f);
                    debug_money_text = "(基準値 * 1.1f)";
                    _getNinki = 2;
                    _kanso = "ありがとう！　お客さん、気に入ってたみたい！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 80 && okashi_totalscore < GameMgr.high_score) //80~100
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 1.5f);
                    debug_money_text = "(基準値 * 1.5f)";
                    _getNinki = 2;
                    _kanso = "ありがとう！お客さん、大喜びだったわ！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";                    
                }
                else if (okashi_totalscore >= GameMgr.high_score && okashi_totalscore < 120) //100~120
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 2.0f);
                    debug_money_text = "(基準値 * 2.0f)";
                    _getNinki = 3;
                    _kanso = "ありがとう！とても良い出来みたい！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 120 && okashi_totalscore < 140) //100~120
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 2.5f);
                    debug_money_text = "(基準値 * 2.5f)";
                    _getNinki = 3;
                    _kanso = "グレイトだわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 140 && okashi_totalscore < 150) //120~150
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 2.75f);
                    debug_money_text = "(基準値 * 2.75f)";
                    _getNinki = 5;
                    _kanso = "ほっぺたがとろけちゃうぐらい最高だって！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 150 && okashi_totalscore < 175) //150~175
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 3.0f);
                    debug_money_text = "(基準値 * 3.0f)";
                    _getNinki = 6;
                    _kanso = "まるで宝石のようにすばらしい味らしいわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 175 && okashi_totalscore < 200) //175~200
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 4.0f);
                    debug_money_text = "(基準値 * 4.0f)";
                    _getNinki = 6;
                    _kanso = "天使のような素晴らしい味らしいわ！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 200) //200~
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (_score_deg) * 5.0f);
                    debug_money_text = "(基準値 * (okashi_totalscore / GameMgr.high_score) * 5.0f)";
                    _getNinki = 10;
                    _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }

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
                    sceneBGM.FadeInBGM();
                }
                else if (okashi_totalscore >= GameMgr.low_score && okashi_totalscore < GameMgr.high_score) //80点以上
                {
                    sc.PlaySe(76);
                    sc.PlaySe(31);

                    sc.PlaySe(78);
                    sc.PlaySe(88);
                    sceneBGM.FadeInBGM();
                }
                else if (okashi_totalscore >= GameMgr.high_score && okashi_totalscore < 200) //ハイスコア
                {
                    sc.PlaySe(76);
                    sc.PlaySe(31);

                    sc.PlaySe(78);
                    sc.PlaySe(88);
                    sc.PlaySe(43);
                    sceneBGM.FadeInBGM();
                }
                else if (okashi_totalscore >= 200) //200点以上のときは、ファンファーレ
                {
                    sc.PlaySe(76);
                    sc.PlaySe(31);

                    sc.PlaySe(78);
                    sc.PlaySe(88);
                    sc.PlaySe(43);
                    sceneBGM.PlayFanfare1();
                    //sceneBGM.NowFadeVolumeONBGM();
                    mute_on = true;
                }
                else
                {
                    sceneBGM.FadeInBGM();
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

                _getMoney = (int)(_buy_price * _kosu_default * 0.03f);
                _text.text = "ごめんなさい。ちょっとお菓子が違ってたみたい。" + "\n" + "次はちゃんと正しいものを持ってきてね。" + "\n" +
                    "お駄賃 " + GameMgr.ColorYellow + _getMoney + GameMgr.MoneyCurrency + "　</color>" + "を受け取った！";

                Debug.Log("納品失敗..");

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                //所持金をプラス
                moneyStatus_Controller.GetMoney(_getMoney); //アニメつき

                //名声値は減る
                if (GameMgr.Story_Mode == 1)
                {
                    //ninkiStatus_Controller.DegNinki(3); //アニメつき
                }

                WhiteFadeCanvas.SetActive(false);
                //sceneBGM.FadeInBGM();

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
        
    }

    

    public void OnEndResultButton() //クエストリザルトボタンおすと、フラグがONに。
    {
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

        //名声をプラスかマイナス。0は変化なし
        if (GameMgr.Story_Mode == 1)
        {
            if (_getNinki < 0)
            {
                //名声値は減る
                PlayerStatus.player_ninki_param -= (Mathf.Abs(_getNinki));
                //ninkiStatus_Controller.DegNinki(Mathf.Abs(_getNinki)); //アニメつき
            }
            else if (_getNinki > 0)
            {
                //名声値は増える
                PlayerStatus.player_ninki_param += (Mathf.Abs(_getNinki));
                //ninkiStatus_Controller.GetNinki(_getNinki); //アニメつき
            }
            ninkiStatus_Controller.money_Draw();
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

        back_ShopFirst_btn.interactable = true;
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        if(mute_on)
        {
            mute_on = false;
            sceneBGM.StopFanfare();
            sceneBGM.PlaySub();
            sceneBGM.FadeInBGM();
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Bar":

                barMain.shop_status = 0;
                break;

            case "Shop":

                shopMain.shop_status = 0;
                break;
        }
        
    }

    void DebugTasteText()
    {
        debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();

        debug_taste_resultText.text =
            "###  好みの比較　結果　###"
            + "\n" + "\n" + "判定用お菓子セットの番号: " + _questID
            + "\n" + "\n" + "判定アイテム名: " + _itemname
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
            + "\n" + "\n" + "ぷるぷる度: " + "-"
            + "\n" + "\n" + "噛み応え度: " + "-"
            + "\n" + "\n" + "トッピングスコア: " + topping_score
            + "\n" + "\n" + "指定のトッピングあったかどうか falseで-50点: " + slot_ok
            + "\n" + "\n" + "お菓子の見た目: " + _basebeauty + "\n" + "見た目閾値: " + _beauty + "\n" + "見た目スコア: " + beauty_score
            + "\n" + "\n" + "補正値　無条件で点数を下げる: " + Hosei_score
            + "\n" + "\n" + "総合得点: " + okashi_score
            + "\n" + "\n" + "### ###"
            + "\n" + "\n" + "お金の取得式: " + "\n" + debug_money_text
            + "\n" + "\n" + "基準値: " + _buy_price * _kosu_default
            + "\n" + "\n" + "okashi_totalscore / GameMgr.high_score 計算: " + _score_deg.ToString()
            + "\n" + "\n" + "お金の取得合計: " + _getMoney;
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
                _basescore = database.items[_id].Base_Score;
                _basegirl1_like = database.items[_id].girl1_itemLike;
                _basecost = database.items[_id].cost_price;
                _basesell = database.items[_id].sell_price;
                _base_itemType = database.items[_id].itemType.ToString();
                _base_itemType_sub = database.items[_id].itemType_sub.ToString();
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
                _basescore = pitemlist.player_originalitemlist[_id].Base_Score;
                _basegirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
                _basecost = pitemlist.player_originalitemlist[_id].cost_price;
                _basesell = pitemlist.player_originalitemlist[_id].sell_price;
                _base_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
                _base_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();
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
                _basescore = pitemlist.player_extremepanel_itemlist[_id].Base_Score;
                _basegirl1_like = pitemlist.player_extremepanel_itemlist[_id].girl1_itemLike;
                _basecost = pitemlist.player_extremepanel_itemlist[_id].cost_price;
                _basesell = pitemlist.player_extremepanel_itemlist[_id].sell_price;
                _base_itemType = pitemlist.player_extremepanel_itemlist[_id].itemType.ToString();
                _base_itemType_sub = pitemlist.player_extremepanel_itemlist[_id].itemType_sub.ToString();
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

        HintText.text = _a + _sweat_kansou + _bitter_kansou + _sour_kansou;
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
            _sweat_kansou = "甘さ A: 甘さ、ほどよくよい具合！";
        }
        else if (sweat_level == 5)
        {
            _sweat_kansou = "甘さ B: まあまあの甘さ";
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
            _bitter_kansou = "苦さ A: 苦さ、ほどよくいい具合！";
        }
        else if (bitter_level == 5)
        {
            _bitter_kansou = "苦さ B: まあまあの苦さ";
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
            _sour_kansou = "酸味 A: すっぱさ、ほどよくいい具合！";
        }
        else if (sour_level == 5)
        {
            _sour_kansou = "酸味 B: まあまあのすっぱさ";
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

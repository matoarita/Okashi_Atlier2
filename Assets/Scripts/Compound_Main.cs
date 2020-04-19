using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Compound_Main : MonoBehaviour
{

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    private SoundController sc;

    private Exp_Controller exp_Controller;

    private BGM sceneBGM;
    public bool bgm_change_flag;
    public bool bgm_change_flag2;

    private Girl1_status girl1_status;
    private Special_Quest special_quest;
    private Touch_Controller touch_controller;

    private TimeController time_controller;

    private Debug_Panel_Init debug_panel_init;

    private GameObject selectPanel_1;
    private GameObject select_original_button_obj;
    private GameObject select_recipi_button_obj;
    private GameObject select_extreme_button_obj;
    private GameObject select_sister_shop_button_obj;
    private Button select_original_button;
    private Button select_recipi_button;
    private Button select_extreme_button;
    private Button select_sister_shop_button;
    private Button select_no_button;

    private GameObject girl_love_exp_bar;
    private GameObject moneystatus_panel;

    private GameObject TimePanel_obj1;
    private GameObject TimePanel_obj2;

    private GameObject getmatplace_panel;
    private GetMatPlace_Panel getmatplace;
    private ItemMatPlaceDataBase matplace_database;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;
    private GameObject recipilist_scrollview_init_obj;

    private GameObject recipimemoController_obj;
    private GameObject recipiMemoButton;
    private GameObject memoResult_obj;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;
    public bool girlEat_ON;

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

    private GameObject black_panel_A;
    private GameObject compoBG_A;
    private GameObject ResultBGimage;

    private GameObject SelectCompo_panel_1;

    private CombinationMain Combinationmain;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject compoundselect_onoff_obj;

    private GameObject original_toggle;
    private GameObject recipi_toggle;
    private GameObject topping_toggle;
    private GameObject extreme_toggle;
    private GameObject roast_toggle;
    private GameObject blend_toggle;

    private GameObject menu_toggle;
    private GameObject girleat_toggle;
    private GameObject shop_toggle;
    private GameObject getmaterial_toggle;
    private GameObject stageclear_toggle;

    private Button extreme_Button;
    private Button recipi_Button;
    private GameObject sell_Button;
    private GameObject present_Button;
    private GameObject stageclear_Button;

    private bool Recipi_loading;
    private bool GirlLove_loading;
    public bool check_recipi_flag;
    public bool check_GirlLoveEvent_flag;
    private int not_read_total;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject yes_no_panel; //通常時のYes, noボタン
    private GameObject yes_no_clear_panel; //クリア時のYes, noボタン

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private int i, j, _id;
    private int recipi_num;
    private int comp_ID;
    private int clear_love;
    private int recipi_id;

    public int compound_status;
    public int compound_select;

    public int event_itemID; //イベントレシピ使用時のイベントのID

    public string originai_text;
    public string extreme_text;
    public string recipi_text;

    // Use this for initialization
    void Start()
    {
       
        //Debug.Log("Compound scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子        

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");        

        //時間表示パネルの取得
        TimePanel_obj1 = canvas.transform.Find("TimePanel/TimeHyouji_1").gameObject;
        TimePanel_obj2 = canvas.transform.Find("TimePanel/TimeHyouji_2").gameObject;
        TimePanel_obj2.SetActive(false);

        //好感度バーの取得
        girl_love_exp_bar = canvas.transform.Find("Girl_love_exp_bar").gameObject;

        //お金ステータスパネルの取得
        moneystatus_panel = canvas.transform.Find("MoneyStatus_panel").gameObject;

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false;
        bgm_change_flag2 = false;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //プレイヤー所持アイテムリストパネルの取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();
        playeritemlist_onoff.SetActive(false);

        //レシピリストパネルの取得
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().RecipiList_ScrollView_Init();
        recipilist_onoff = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();
        recipilist_onoff.SetActive(false);

        //レシピメモボタンを取得
        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
        recipiMemoButton = canvas.transform.Find("Compound_BGPanel_A/RecipiMemoButton").gameObject;
        memoResult_obj = canvas.transform.Find("Compound_BGPanel_A/Memo_Result").gameObject;

        //調合セレクトパネルを取得
        selectPanel_1 = canvas.transform.Find("Compound_BGPanel_A/SelectPanel_1").gameObject;
        select_original_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/OriginalButton").gameObject;
        select_original_button = select_original_button_obj.GetComponent<Button>();
        select_recipi_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/RecipiButton").gameObject;
        select_recipi_button = select_recipi_button_obj.GetComponent<Button>();
        select_extreme_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/ExButton").gameObject;
        select_extreme_button = select_extreme_button_obj.GetComponent<Button>();
        select_sister_shop_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/SellButton").gameObject;
        select_sister_shop_button = select_sister_shop_button_obj.GetComponent<Button>();
        select_no_button = selectPanel_1.transform.Find("No").GetComponent<Button>();

        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes = playeritemlist_onoff.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = playeritemlist_onoff.transform.Find("No").gameObject;
        no_text = no.GetComponentInChildren<Text>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_clear_panel = canvas.transform.Find("StageClear_Yes_no_Panel").gameObject;

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //材料ランダムで３つ手に入るオブジェクトの取得
        get_material_obj = GameObject.FindWithTag("GetMaterial");
        get_material = get_material_obj.GetComponent<GetMaterial>();

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //エクストリームパネルの取得
        Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");
        extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();

        //ボタンの取得
        extreme_Button = Extremepanel_obj.transform.Find("ExtremeButton").gameObject.GetComponent<Button>(); //エクストリームボタン
        recipi_Button = Extremepanel_obj.transform.Find("RecipiButton").gameObject.GetComponent<Button>(); //レシピボタン
        sell_Button = Extremepanel_obj.transform.Find("SellButton").gameObject; //売るボタン
        present_Button = Extremepanel_obj.transform.Find("PresentButton").gameObject; //売るボタン 

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //黒半透明パネルの取得
        black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;
        black_panel_A.SetActive(false);

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;
        compoBG_A.SetActive(false);
        ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
        ResultBGimage.SetActive(false);

        //調合選択画面の取得
        SelectCompo_panel_1 = canvas.transform.Find("Compound_BGPanel_A/SelectPanel_1").gameObject;
        SelectCompo_panel_1.SetActive(false);

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel/Comp").gameObject;
        getmatplace = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();
        getmatplace_panel.SetActive(false);

        //タッチ判定オブジェクトの取得
        touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

        //時間管理オブジェクトの取得
        time_controller = canvas.transform.Find("TimePanel").GetComponent<TimeController>();

        compoundselect_onoff_obj = canvas.transform.Find("CompoundSelect_ScrollView").gameObject;

        original_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Original_Toggle").gameObject;
        recipi_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Recipi_Toggle").gameObject;
        extreme_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Extreme_Toggle").gameObject;
        roast_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Roast_Toggle").gameObject;
        //blend_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Blend_Toggle").gameObject;

        menu_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/ItemMenu_Toggle").gameObject;
        girleat_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GirlEat_Toggle").gameObject;
        shop_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Shop_Toggle").gameObject;
        getmaterial_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GetMaterial_Toggle").gameObject;
        stageclear_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;

        stageclear_Button = canvas.transform.Find("StageClear_Button").gameObject;
        stageclear_Button.SetActive(false);
        
        compound_status = 0;
        compound_select = 0;

        girlEat_ON = false;
        Recipi_loading = false;
        GirlLove_loading = false;
        check_recipi_flag = false;
        check_GirlLoveEvent_flag = false;

        //女の子　お菓子ハングリー状態のリセット
        girl1_status.Girl1_Status_Init();

        //ステージクリア用の好感度数値
        switch (GameMgr.stage_number)
        {
            case 1:

                if (GameMgr.stage1_load_ok != true)
                {
                    GameMgr.stage1_load_ok = true;
                    GameMgr.scenario_flag = 110;                   
                }
                clear_love = GameMgr.stage1_clear_love;
                break;

            case 2:

                clear_love = GameMgr.stage2_clear_love;
                break;

            case 3:

                clear_love = GameMgr.stage3_clear_love;
                break;
        }

        //初期メッセージ
        _text.text = "どうしようかなぁ？";
        text_area.SetActive(true);

        //各調合時のシステムメッセージ集
        originai_text = "新しくお菓子を作るよ！" + "\n" + "好きな材料を" + GameMgr.ColorYellow + "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";
        extreme_text = "エクストリーム調合をするよ！ 一個目の材料を選んでね。";
        recipi_text = "レシピから作るよ。何を作る？";

    }

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.scenario_ON != true)
        {
            switch (GameMgr.scenario_flag)
            {

                case 110: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。チュートリアルするかどうか。
                    
                    StartScenario();
                    break;

                case 130: //ショップから帰ってきた。

                    StartScenario();

                    break;

                case 165: //パンの作り方をきいてきた。

                    matplace_database.matPlaceKaikin("Ido"); //井戸解禁
                    StartScenario();

                    break;

                default:
                    break;
            }
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。チュートリアルなどの強制イベントのチェック。
        if (GameMgr.scenario_ON == true)
        {

            //チュートリアルモードがONになったら、この中の処理が始まる。
            if (GameMgr.tutorial_ON == true)
            {

                switch (GameMgr.tutorial_Num)
                {
                    case 0: //最初にシナリオを読み始める。

                        Extremepanel_obj.SetActive(true);

                        //一時的に腹減りを止める。+腹減りステータスをリセット
                        girl1_status.GirlEat_Judge_on = false;
                        girl1_status.Girl_Full();
                        girl1_status.Girl1_Status_Init();
                        girl1_status.OkashiNew_Status = 1;
                        GameMgr.tutorial_Num = 1; //退避
                        break;

                    case 10: //宴ポーズ。エクストリームパネルを押そう！で、待機。

                        MainCompoundMethod();
                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(true);
                        _text.text = "左のエクストリームパネルを押してみよう！";
                        break;

                    case 20: //エクストリームパネルを押して、オリジナル調合画面を開いた

                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(false);

                        compoBG_A.transform.Find("Image").GetComponent<Image>().raycastTarget = false;
                        pitemlistController.Offinteract();
                        kakuritsuPanel_obj.SetActive(false);
                        no.SetActive(false);

                        Extremepanel_obj.SetActive(false);

                        break;

                    case 30: //宴がポーズ状態。右のレシピメモを押そう。

                        text_area.SetActive(true);
                        _text.text = "右の「レシピメモ」ボタンを押してみよう！";
                        break;

                    case 40: //メモ画面を開いた。

                        text_area.SetActive(false);
                        break;

                    case 50: //宴ポーズ。オリジナル調合をしてみるところ。

                        pitemlistController.Oninteract();
                        text_area.SetActive(true);
                        _text.text = originai_text;

                        GameMgr.tutorial_Num = 55;
                        break;

                    case 55: //調合中
                        break;

                    case 60: //調合完了！

                        text_area.SetActive(false);

                        break;

                    case 70: //宴ポーズ。やったね！クッキーができた～から、レシピを閃き、ボタンを押し待ち。

                        card_view.SetinteractiveOn();
                        text_area.SetActive(true);
                        GameMgr.tutorial_Num = 75; //退避
                        break;

                    case 75:

                        text_area.SetActive(true);
                        _text.text = "カードを押してみよう！";
                        break;

                    case 80: //ボタンを押し、元の画面に戻る。

                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        //compoundselect_onoff_obj.SetActive(true);
                        text_area.SetActive(false);
                        break;

                    case 90: //「あげる」ボタンを押すところ。「あげる」のみをON、他のボタンはオフ。

                        //Debug.Log("GameMgr.チュートリアルNo: " + GameMgr.tutorial_Num);
                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(false);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = false;
                        text_area.SetActive(false);

                        girl1_status.SetOneQuest(0);
                        girl1_status.Girl_Hungry();
                        girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                        GameMgr.tutorial_Num = 95; //退避

                        break;

                    case 100:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(true);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = true;
                        text_area.SetActive(true);
                        _text.text = "お菓子をあげてみよう！";

                        GameMgr.tutorial_Num = 105; //退避
                        break;

                    case 105:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = true;
                        
                        break;

                    case 110:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        compoundselect_onoff_obj.SetActive(false);

                        text_area.SetActive(false);
                        break;

                    case 120:

                        MainCompoundMethod();

                        girl1_status.timeGirl_hungry_status = 2; //一回、画像を元に戻す。

                        girl1_status.SetOneQuest(11);
                        girl1_status.Girl_Hungry();
                        girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                        //text_area.SetActive(true);

                        GameMgr.tutorial_Num = 130;

                        GameMgr.tutorial_Progress = true;
                        break;

                    case 130:

                        text_area.SetActive(false);
                        break;

                    case 140:

                        extreme_Button.interactable = true;
                        text_area.SetActive(true);

                        _text.text = "ねこクッキーを作ってみよう！";

                        break;

                    case 150: //レシピボタンでも～を説明中。ボタンは押せないようにしておく。

                        MainCompoundMethod();

                        select_original_button.interactable = false;
                        select_recipi_button.interactable = false;
                        select_no_button.interactable = false;

                        text_area.SetActive(false);
                        break;

                    case 160:

                        MainCompoundMethod();

                        select_recipi_button.interactable = true;

                        text_area.SetActive(true);

                        GameMgr.tutorial_Num = 165;
                        break;

                    case 165: //レシピ調合中

                        MainCompoundMethod();
                        break;

                    case 170: //れしぴ調合完了！

                        text_area.SetActive(false);
                        break;

                    case 180:

                        card_view.SetinteractiveOn();

                        text_area.SetActive(true);
                        _text.text = "カードを押してみよう！";
                        break;

                    case 190: //元の画面に戻る

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);
                        compoundselect_onoff_obj.SetActive(false);

                        text_area.SetActive(false);

                        break;

                    case 200:

                        MainCompoundMethod();

                        extreme_Button.interactable = true;
                        //sell_Button.SetActive(false);
                        text_area.SetActive(true);

                        _text.text = "もう一度パネルを押してみよう！";

                        break;

                    case 210: //エクストリーム調合　他のボタンは触れない

                        MainCompoundMethod();

                        select_original_button.interactable = false;
                        select_recipi_button.interactable = false;
                        select_no_button.interactable = false;

                        text_area.SetActive(false);
                        break;

                    case 220:

                        //MainCompoundMethod();
                        text_area.SetActive(true);

                        _text.text = "「仕上げる」を押してみよう！";

                        break;

                    case 230:

                        MainCompoundMethod();

                        kakuritsuPanel_obj.SetActive(false);
                        text_area.SetActive(false);

                        break;

                    case 240:

                        MainCompoundMethod();

                        //kakuritsuPanel_obj.SetActive(true);
                        text_area.SetActive(true);

                        GameMgr.tutorial_Num = 245; //退避

                        break;

                    case 245:

                        text_area.SetActive(true);
                        break;


                    case 250:

                        kakuritsuPanel_obj.SetActive(false);
                        text_area.SetActive(false);
                        break;

                    case 260:

                        card_view.SetinteractiveOn();

                        text_area.SetActive(true);
                        _text.text = "カードを押してみよう！";

                        GameMgr.tutorial_Num = 265; //退避
                        break;

                    case 270: //再び、元画面に戻る。

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(false);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = false;

                        text_area.SetActive(false);
                        break;

                    case 280:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(true);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = true;
                        girl1_status.timeGirl_hungry_status = 1;

                        text_area.SetActive(true);

                        GameMgr.tutorial_Num = 285; //退避
                        break;

                    case 285:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = true;
                        
                        break;

                    case 290:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        //sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(false);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        girleat_toggle.GetComponent<Toggle>().interactable = false;

                        text_area.SetActive(false);
                        break;

                    default:


                        break;
                }

            }
            else //チュートリアル以外、デフォルトで、宴を読んでいるときの処理
            {
                compoundselect_onoff_obj.SetActive(false);
                Extremepanel_obj.SetActive(false);
                text_area.SetActive(false);
                check_recipi_flag = false;
                TimePanel_obj1.SetActive(false);
                girl_love_exp_bar.SetActive(false);
                moneystatus_panel.SetActive(false);

                //腹減りカウント一時停止
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.Girl_Full();
                girl1_status.Girl1_Status_Init();

                touch_controller.Touch_OnAllOFF();

            }

        }
        else //以下が、通常の処理
        {

            if (girlEat_ON) //お菓子判定中の間は、無条件で、メインの処理は無視する。
            {

            }
            else
            {
                //好感度チェック。好感度に応じて、イベントが発生。
                if (check_GirlLoveEvent_flag == false)
                {
                    //腹減りカウント一時停止
                    girl1_status.GirlEat_Judge_on = false;

                    Check_GirlLoveEvent();
                }
                else
                {
                    //読んでいないレシピがあれば、読む処理。優先順位二番目。
                    if (check_recipi_flag != true)
                    {
                        //腹減りカウント一時停止
                        girl1_status.GirlEat_Judge_on = false;

                        Check_RecipiFlag();
                    }
                    else
                    {
                        //Debug.Log("compound_status: " + compound_status);
                        //メインの調合処理　各ボタンを押すと、中の処理が動き始める。
                        MainCompoundMethod();

                    }
                }
            }
        }
    }

    void StartScenario()
    {
        GameMgr.scenario_ON = true; //これがONのときは、調合シーンの、調合ボタンなどはオフになり、シナリオを優先する。「Utage_scenario.cs」のUpdateが同時に走っている。
    }


    //メインの調合シーンの処理
    void MainCompoundMethod()
    {
        switch (compound_status)
        {
            case 0:

                //Debug.Log("メインの調合シーン　スタート");

                //ボタンなどの状態の初期設定
                if (GameMgr.tutorial_ON != true)
                {
                    compoundselect_onoff_obj.SetActive(true);

                    //腹減りカウント開始
                    girl1_status.GirlEat_Judge_on = true;   
                    
                    touch_controller.Touch_OnAllON();
                    compoBG_A.transform.Find("Image").GetComponent<Image>().raycastTarget = true;
                    GameMgr.scenario_read_endflag = false;
                }
                
                recipilist_onoff.SetActive(false);
                playeritemlist_onoff.SetActive(false);
                yes_no_panel.SetActive(false);
                getmatplace_panel.SetActive(false);               
                kakuritsuPanel_obj.SetActive(false);
                black_panel_A.SetActive(false);
                ResultBGimage.SetActive(false);
                compoBG_A.SetActive(false);
                

                TimePanel_obj1.SetActive(true);
                TimePanel_obj2.SetActive(false);                

                girl_love_exp_bar.SetActive(true);
                moneystatus_panel.SetActive(true);
                select_original_button.interactable = true;
                select_recipi_button.interactable = true;
                select_no_button.interactable = true;
                menu_toggle.GetComponent<Toggle>().interactable = true;
                getmaterial_toggle.GetComponent<Toggle>().interactable = true;
                shop_toggle.GetComponent<Toggle>().interactable = true;
                girleat_toggle.GetComponent<Toggle>().interactable = true;

                recipiMemoButton.SetActive(false);

                //音関係
                if (bgm_change_flag == true)
                {
                    bgm_change_flag = false;
                    sceneBGM.OnMainBGM();
                }
                if (bgm_change_flag2 == true)
                {
                    bgm_change_flag2 = false;
                    sceneBGM.OnMainBGMFade();
                }
                sceneBGM.MuteOFFBGM();


                //一度でも調合成功していれば、エクストリーム調合が出現になる。
                if ( PlayerStatus.First_recipi_on == true )
                {
                    select_extreme_button_obj.SetActive(true);
                   
                }
                else
                {
                    select_extreme_button_obj.SetActive(false);
                }



                //好感度がステージの、一定の数値を超えたら、クリアボタンがでる。
                if (girl1_status.girl1_Love_exp >= clear_love)
                {
                    stageclear_toggle.SetActive(true);
                    //stageclear_Button.SetActive(true);
                }
                else
                {
                    if (stageclear_toggle.activeSelf == true)
                    {
                        stageclear_toggle.SetActive(false);
                        //stageclear_Button.SetActive(false);
                    }
                }

                Extremepanel_obj.SetActive(true);
                extreme_panel.extremeButtonInteractOn();
                extreme_panel.LifeAnimeOnTrue();

                text_area.SetActive(true);

                //時間のチェック
                time_controller.TimeCheck_flag = true;
                time_controller.TimeKoushin(); //時間の更新

                compound_status = 110; //退避
                break;

            case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択

                recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                kakuritsuPanel_obj.SetActive(true);
                compoBG_A.SetActive(true);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                text_area.SetActive(true);
                time_controller.TimeCheck_flag = false;

                //BGMを変更
                if (bgm_change_flag2 != true)
                {
                    sceneBGM.OnCompoundBGM();
                    bgm_change_flag2 = true;
                }

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                break;

            case 2: //エクストリーム調合の処理を開始。クリック後に処理が始まる。

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 2; //トッピング調合を選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                kakuritsuPanel_obj.SetActive(false);
                compoBG_A.SetActive(true);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                time_controller.TimeCheck_flag = false;
                text_area.SetActive(true);

                //BGMを変更
                if (bgm_change_flag2 != true)
                {
                    sceneBGM.OnCompoundBGM();
                    bgm_change_flag2 = true;
                }

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。

                extreme_panel.extreme_Compo_Setup();

                break;

            case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 3; //オリジナル調合を選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                kakuritsuPanel_obj.SetActive(true);

                compoBG_A.SetActive(true);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                recipiMemoButton.SetActive(true);
                recipimemoController_obj.SetActive(false);
                time_controller.TimeCheck_flag = false;
                memoResult_obj.SetActive(false);
                text_area.SetActive(true);

                //BGMを変更
                if (bgm_change_flag2 != true)
                {
                    sceneBGM.OnCompoundBGM();
                    bgm_change_flag2 = true;
                }

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 

                break;

            case 4: //調合シーンに入ってますよ、というフラグ。各ケース処理後、必ずこの中の処理に移行する。yes, noボタンを押されるまでは、待つ状態に入る。

                break;

            case 5: //「焼く」を選択

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 5; //焼くを選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                //yes.SetActive(false);
                //no.SetActive(true);

                break;

            case 6: //オリジナル調合かレシピ調合を選択できるパネルを表示

                //BGMを変更
                if (bgm_change_flag2 != true)
                {
                    sceneBGM.OnCompoundBGM();
                    bgm_change_flag2 = true;
                }

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ

                playeritemlist_onoff.SetActive(false);
                recipilist_onoff.SetActive(false);

                SelectCompo_panel_1.SetActive(true);
                compoBG_A.SetActive(true);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                time_controller.TimeCheck_flag = false;
                text_area.SetActive(false);

                recipiMemoButton.SetActive(false);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);

                

                if (extreme_panel.extreme_itemID != 9999 && extreme_panel.extreme_kaisu > 0)
                {
                    select_extreme_button.interactable = true;
                } else
                {
                    select_extreme_button.interactable = false;
                }


                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                break;

            case 10: //「あげる」を選択

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 13; //あげるシーンに入っています、というフラグ
                compound_select = 10; //あげるを選択

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                card_view.PresentGirl(extreme_panel.extreme_itemtype, extreme_panel.extreme_itemID);
                StartCoroutine("Girl_present_Final_select");


                break;

            case 11: //お菓子をあげたあとの処理。女の子が、お菓子を判定

                compound_status = 12;
                girlEat_ON = true; //お菓子判定中フラグ

                //お菓子の判定処理を起動。引数は、決定したアイテムのアイテムIDと、店売りかオリジナルで制作したアイテムかの、判定用ナンバー 0or1
                girlEat_judge.Girleat_Judge_method(extreme_panel.extreme_itemID, extreme_panel.extreme_itemtype);

                break;

            case 12: //お菓子を判定中

                break;

            case 13: //あげるかあげないかを選択中

                break;

            case 20: //材料採取地を選択中

                compoundselect_onoff_obj.SetActive(false);

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                Extremepanel_obj.SetActive(false);
                text_area.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                break;

            case 21: //材料採取地に到着。探索中

                break;

            case 30: //「売る」を選択

                compoundselect_onoff_obj.SetActive(false);
                //stageclear_Button.SetActive(false);

                compound_status = 31; //売るシーンに入っています、というフラグ
                compound_select = 30; //売るを選択

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);
                StartCoroutine("Sell_Final_select");


                break;

            case 31: //売るかどうか、選択中

                break;

            case 32: //売る処理の実行

                compound_status = 32;

                extreme_panel.Sell_Okashi();
                break;

            case 40: //ステージクリアを選択

                compoundselect_onoff_obj.SetActive(false);
                //stageclear_Button.SetActive(false);

                compound_status = 41; //売るシーンに入っています、というフラグ
                compound_select = 40; //売るを選択

                yes_no_clear_panel.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);
                StartCoroutine("StageClear_Final_select");
                break;

            case 41: //クリアするかどうか、選択中
                break;

            case 99: //アイテム画面を開いたとき

                compoundselect_onoff_obj.SetActive(false);
                black_panel_A.SetActive(true);
                compound_status = 99;
                compound_select = 99;
                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。


                break;

            case 100: //退避用

                break;


            /*case 5: //ブレンド調合の処理（未使用）

                compoundselect_onoff_obj.SetActive(false);
                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 5; //ブレンド調合を選択
                recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                no.SetActive(true);

                break;*/



            default:
                break;
        }
    }


    public void OnCheck_1() //レシピ調合をON
    {
        if (recipi_toggle.GetComponent<Toggle>().isOn == true)
        {
            recipi_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            _text.text = recipi_text;
            compound_status = 1;
        }
    }

    public void OnCheck_1_button()
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = recipi_text;
        compound_status = 1;
    }

    public void OnCheck_2() //トッピング調合をON
    {
        if (extreme_toggle.GetComponent<Toggle>().isOn == true)
        {
            extreme_toggle.GetComponent<Toggle>().isOn = false;

            pitemlistController.extremepanel_on = false;
            card_view.DeleteCard_DrawView();

            _text.text = extreme_text;
            compound_status = 2;
        }
    }

    public void OnCheck_2_button()
    {
        if (GameMgr.tutorial_ON == true)
        {
            if (GameMgr.tutorial_Num == 210) //エクストリーム説明中は、押しても反応なし
            {

            }
            else
            {
                pitemlistController.extremepanel_on = false;

                card_view.DeleteCard_DrawView();
                SelectCompo_panel_1.SetActive(false);

                _text.text = extreme_text;
                compound_status = 2;

                if (GameMgr.tutorial_ON == true)
                {
                    if (GameMgr.tutorial_Num == 220)
                    {
                        GameMgr.tutorial_Progress = true;
                        GameMgr.tutorial_Num = 230;
                    }
                }
            }
        }
        else
        {

            pitemlistController.extremepanel_on = false;

            card_view.DeleteCard_DrawView();
            SelectCompo_panel_1.SetActive(false);

            _text.text = extreme_text;
            compound_status = 2;

        }
        
    }

    public void OnCheck_3() //オリジナル調合をON
    {
        if (original_toggle.GetComponent<Toggle>().isOn == true)
        {
            original_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            _text.text = originai_text;
            compound_status = 3;
        }
    }

    public void OnCheck_3_button() //調合選択画面からボタンを選択して、オリジナル調合をON
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = originai_text;
        compound_status = 3;
    }

    /*public void OnCheck_4() //ブレンド調合をON
    {
        if (blend_toggle.GetComponent<Toggle>().isOn == true)
        {

            //Debug.Log("check4");
            _text.text = "ブレンド調合をします。まずはレシピを選ぶ。";
            compound_status = 5;
        }
    }*/

    public void OnCheck_5() //"焼き"をON
    {
        if (roast_toggle.GetComponent<Toggle>().isOn == true)
        {
            roast_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            _text.text = "作った生地を焼きます。焼きたい生地を選んでください。";
            compound_status = 5;
        }
    }

    public void OnCancel_Select()
    {
        compound_status = 0;
    }

    public void OnMenu_toggle() //メニューをON
    {
        if (menu_toggle.GetComponent<Toggle>().isOn == true)
        {
            menu_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            compound_status = 99;
        }
    }

    public void OnShop_toggle() //ショップへ移動
    {
        if (shop_toggle.GetComponent<Toggle>().isOn == true)
        {
            card_view.DeleteCard_DrawView();

            shop_toggle.GetComponent<Toggle>().isOn = false;
            FadeManager.Instance.LoadScene("Shop", 0.3f);
        }
    }

    public void OnGetMaterial_toggle() //材料採取地選択
    {
        if (getmaterial_toggle.GetComponent<Toggle>().isOn == true)
        {
            getmaterial_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();
            _text.text = "妹と一緒に材料を取りにいくよ！行き先を選んでね。";
            compound_status = 20;

            //BGMを変更
            //sceneBGM.OnGetMatStartBGM();
            //bgm_change_flag = true;

            //音ならす
            sc.PlaySe(36);

            getmatplace.SetInit();
            getmatplace_panel.SetActive(true);
            yes_no_panel.SetActive(true);
            yes_no_panel.transform.Find("Yes").gameObject.SetActive(false);

            TimePanel_obj1.SetActive(false);
            TimePanel_obj2.SetActive(true);
        }
    }

    public void OnGirlEat() //女の子にお菓子をあげる
    {
        if (girleat_toggle.GetComponent<Toggle>().isOn == true)
        {
            girleat_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            card_view.DeleteCard_DrawView();

            if ( extreme_panel.extreme_itemID != 9999 )
            {
                _text.text = "今、作ったお菓子をあげますか？";
                compound_status = 10;
            }
            else //まだ作ってないときは
            {
                _text.text = "まだお菓子を作っていない。";
            }
            

        }
    }

    public void OnStageClear() //ステージクリアボタン
    {
        if (stageclear_toggle.GetComponent<Toggle>().isOn == true)
        {
            stageclear_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            card_view.DeleteCard_DrawView();

            _text.text = "コンテストに出場しますか？" + "\n" + "（次のお話へ進みます。）";
            compound_status = 40;

        }
    }


    //レシピを見たときの処理。recipiitemselecttoggleから呼ばれる。
    public void eventRecipi_ON()
    {
        recipilist_onoff.SetActive(false);
        Debug.Log("イベントレシピID: " + event_itemID + "　レシピ名: " + pitemlist.eventitemlist[event_itemID].event_itemNameHyouji);

        compoundselect_onoff_obj.SetActive(false);
        kakuritsuPanel_obj.SetActive(false);
        text_area.SetActive(false);
        black_panel_A.SetActive(false);

        //一時的に腹減りを止める。
        girl1_status.GirlEat_Judge_on = false;

        compoBG_A.transform.Find("Image").GetComponent<Image>().raycastTarget = false; //このときだけ、背景画像のタッチ判定をオフにする。そうしないと、宴がクリックに反応しなくなる。
        Extremepanel_obj.SetActive(false);


        GameMgr.recipi_read_ID = event_itemID;
        GameMgr.itemuse_recipi_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        StartCoroutine("eventRecipi_end");

    }

    IEnumerator eventRecipi_end()
    {
        //Debug.Log("eventRecipi_end() on");
        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        Recipi_loading = false;

        compoBG_A.transform.Find("Image").GetComponent<Image>().raycastTarget = true;
        Extremepanel_obj.SetActive(true);
        text_area.SetActive(true);
        compound_status = 1;
    }


    void text_scenario()
    {

    }



    void Check_RecipiFlag()
    {
        if (Recipi_loading == true)
        {
            //レシピを読み込み中のときは、所持チェックはしない。
        }
        else //レシピを読み込み中でない。
        {
            //所持しているが、まだ読んでいないレシピがないか、チェックする。

            i = 0;
            not_read_total = 0;
            Recipi_loading = false;

            while (i < pitemlist.eventitemlist.Count)
            {
                //所持はしているのに、リードフラグは０のまま（＝読んでいないもの）のレシピの個数をカウントする。
                if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ReadFlag == 0)
                {
                    not_read_total++;
                }
                i++;
            }

            //Debug.Log("not_read_total: " + not_read_total);

            if (not_read_total <= 0) //全て読み終えた
            {
                //最後にチェック。全てのリードフラグが1になったら、全て読み終了。その場合は、レシピチェックをオフにする。
                check_recipi_flag = true;
                compound_status = 0;

                //Debug.Log("レシピ全て読み完了");
            }
            else //1冊でも読んでないものがある。
            {
                i = 0;
                while (i < pitemlist.eventitemlist.Count)
                {

                    //もし、所持はしているのに、リードフラグは０のまま（＝読んでいないもの）がある場合、レシピを読む処理に入る。
                    if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ReadFlag == 0)
                    {                   

                        recipi_num = i; //読んでない本のIDを取得

                        break;
                    }

                    ++i;
                }

                Recipi_loading = true; //レシピを読み込み中ですよ～のフラグ

                StartCoroutine("Recipi_Read_Method");

            }
        }
    }

    IEnumerator Recipi_Read_Method()
    {
        touch_controller.Touch_OnAllOFF();
        compoundselect_onoff_obj.SetActive(false);
        Extremepanel_obj.SetActive(false);
        text_area.SetActive(false);       

        GameMgr.recipi_read_ID = pitemlist.eventitemlist[recipi_num].ev_ItemID;
        GameMgr.recipi_read_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                         //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        Recipi_loading = false;

        /* レシピを読む処理 */
        pitemlist.eventitemlist[recipi_num].ev_ReadFlag = 1; //該当のイベントアイテムのレシピのフラグをONにしておく（読んだ、という意味）
        Recipi_FlagON_Method();
        Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji + "を読んだ");
    }


    IEnumerator Girl_present_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);
        card_view.DeleteCard_DrawView();
        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //女の子にアイテムをあげる処理
                compound_status = 11; //status=11で処理。

                yes_no_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;
                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "";
                compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;

                //ボタンなどの状態の初期設定
                if (GameMgr.tutorial_ON == true)
                {
                    compoundselect_onoff_obj.SetActive(true);
                }
                break;

        }
    }

    IEnumerator Sell_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //売る処理
                compound_status = 32; //status=32で処理。

                yes_no_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;
                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "";
                compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    IEnumerator StageClear_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //ステージクリア処理

                yes_no_clear_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;

                switch(GameMgr.stage_number)
                {
                    case 1:

                        GameMgr.stage1_girl1_loveexp = girl1_status.girl1_Love_exp;
                        FadeManager.Instance.LoadScene("002_Stage2_eyecatch", 0.3f);
                        break;

                    case 2:

                        GameMgr.stage2_girl1_loveexp = girl1_status.girl1_Love_exp;
                        FadeManager.Instance.LoadScene("003_Stage3_eyecatch", 0.3f);
                        break;

                    case 3:

                        GameMgr.stage3_girl1_loveexp = girl1_status.girl1_Love_exp;
                        FadeManager.Instance.LoadScene("100_Ending", 0.3f);
                        break;

                }
                

                break;

            case false:

                //Debug.Log("cancel");

                yes_no_clear_panel.SetActive(false);

                _text.text = "";
                compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    //好感度によって発生する、サブイベント達
    public void Check_GirlLoveEvent()
    {
        if (GirlLove_loading == true)
        {

        }
        else
        {
            switch (GameMgr.stage_number)
            {
                //ステージ１のサブイベント
                case 1:

                    if (girl1_status.girl1_Love_exp >= 50)
                    {

                        if (GameMgr.GirlLoveEvent_01 != true) //ステージ１　好感度イベント１
                        {
                            GameMgr.GirlLoveEvent_num = 1;
                            GameMgr.GirlLoveEvent_01 = true;

                            //レシピの追加
                            recipi_id = Find_eventitemdatabase("rusk_recipi");
                            pitemlist.add_eventPlayerItem(recipi_id, 1); //ラスクのレシピを追加                            

                            //ラスク作りのクエスト発生
                            if (GameMgr.OkashiQuest_flag[1] != true)
                            {
                                Debug.Log("スペシャルクエスト: ラスクが食べたい　開始");

                                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                                special_quest.SetSpecialOkashi(1);
                                                                
                            }

                            Debug.Log("好感度イベント１をON: お兄ちゃん。誰かお客さんがきたよ。");

                            //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                            StartCoroutine("ReadGirlLoveEvent");
                        }
                    }

                    break;

                //ステージ２のサブイベント
                case 2:

                    break;

                //ステージ３のサブイベント
                case 3:

                    break;

                default:
                    break;

            }

            if(!GirlLove_loading)
            {
                check_GirlLoveEvent_flag = true;
            }
            compound_status = 0;
        }
    }

    IEnumerator ReadGirlLoveEvent()
    {
        touch_controller.Touch_OnAllOFF();
        compoundselect_onoff_obj.SetActive(false);
        extreme_Button.interactable = false;
        GirlLove_loading = true;

        while (girlEat_judge.heart_count > 0)
        {
            yield return null;
        }

        text_area.SetActive(false);
        Extremepanel_obj.SetActive(false);
        sceneBGM.MuteBGM();
        
        GameMgr.girlloveevent_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.girlloveevent_endflag)
        {
            yield return null;
        }

        sceneBGM.MuteOFFBGM();

        girl1_status.Girl1_Status_Init2();
        GirlLove_loading = false;

        _text.text = "";

        check_GirlLoveEvent_flag = true;
        check_recipi_flag = false;
    }

    //レシピの番号チェック。コンポ調合アイテムを解禁し、レシピリストに表示されるようにする。
    void Recipi_FlagON_Method()
    {
        
        switch (pitemlist.eventitemlist[recipi_num].event_itemName)
        {
            case "ev02_orangeneko_cookie_memo": //オレンジネコクッキー閃きのメモ

                //オレンジジャムの作り方を解禁
                CompoON_compoitemdatabase("orange_jam");

                break;

            case "najya_start_recipi": //ナジャのお菓子作りの基本                

                CompoON_compoitemdatabase("neko_cookie");
                CompoON_compoitemdatabase("appaleil");

                break;

            case "cookie_base_recipi": //クッキー生地作り方のレシピ＜初級＞  

                CompoON_compoitemdatabase("cookie_nonsuger");
                CompoON_compoitemdatabase("emerald_neko_cookie");
                break;

            case "ice_cream_recipi": //アイスクリームの書

                CompoON_compoitemdatabase("ice_cream");

                break;

            case "financier_recipi": //フィナンシェ

                CompoON_compoitemdatabase("kogashi_butter");

                break;

            case "crepe_recipi": //クレープ

                CompoON_compoitemdatabase("appaleil_milk");
                CompoON_compoitemdatabase("crepe");

                break;

            case "maffin_recipi": //マフィン

                CompoON_compoitemdatabase("maffin");

                break;

            case "bisucouti_recipi": //ビスコッティ

                CompoON_compoitemdatabase("baking_mix"); 
                CompoON_compoitemdatabase("biscotti");

                break;

            case "princesstota_recipi": //プリンセストータ

                CompoON_compoitemdatabase("princess_tota");

                break;

            default:
                break;
        }
    }


    //アイテム名を入力すると、該当するeventitem_IDを返す処理
    public int Find_eventitemdatabase(string compo_itemname)
    {
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (compo_itemname == pitemlist.eventitemlist[j].event_itemName)
            {
                return j;
            }
            j++;
        }

        return 9999; //該当するIDがない場合
    }
   
    //アイテム名を入力すると、該当するcompoIDをOnにする
    void CompoON_compoitemdatabase(string compo_itemname)
    {
        j = 0;
        while (j < databaseCompo.compoitems.Count)
        {
            if (compo_itemname == databaseCompo.compoitems[j].cmpitem_Name)
            {
                comp_ID = j;
                break;
            }
            j++;
        }

        databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;
    }
}
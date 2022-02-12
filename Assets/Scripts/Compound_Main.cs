using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using DG.Tweening;

public class Compound_Main : MonoBehaviour
{
    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject text_area;
    private Text _text;

    private GameObject text_area_Main;
    private Text _textmain;

    private Text picnic_itemText;

    private GameObject mainUI_panel_obj;
    private GameObject UIOpenButton_obj;
    private bool SceneStart_flag;

    private SaveController save_controller;
    private keyManager keymanager;

    private GameObject canvas;

    private SoundController sc;

    private Exp_Controller exp_Controller;
    private MoneyStatus_Controller moneyStatus_Controller;

    private BGM sceneBGM;
    private Map_Ambience map_ambience;
    public bool bgm_change_flag;
    public bool bgm_change_flag2;
    private bool bgm_changeuse_ON = true; //お菓子手帳のシーンで、BGMを切り替えるかどうか。

    private Girl1_status girl1_status;
    private Special_Quest special_quest;
    private Touch_Controller touch_controller;

    private TimeController time_controller;

    private Debug_Panel_Init debug_panel_init;
    private Debug_Panel debug_panel;

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
    private Text girl_param;
    private GameObject moneystatus_panel;
    private GameObject kaerucoin_panel;
    private GameObject GetMatStatusButton_obj;
    private GameObject mainUIFrame_panel;

    private GameObject manpuku_bar;
    private Slider manpuku_slider;

    private GameObject GirlHeartEffect_obj;

    private GameObject TimePanel_obj1;
    private GameObject TimePanel_obj2;

    private Text questname;

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

    private GameObject recipimemoController_obj;
    private GameObject recipiMemoButton;
    private GameObject memoResult_obj;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;
    public bool girlEat_ON; //食べ中のフラグ
    public bool compo_ON; //調合を開始したフラグ

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

    private GameObject Stagepanel_obj;

    private GameObject black_panel_A;
    private GameObject compoBG_A;
    private GameObject ResultBGimage;
    private GameObject compoBG_A_effect;

    private GameObject SelectCompo_panel_1;
    private GameObject system_panel;
    private GameObject status_panel;
    private GameObject okashihint_panel;

    private CombinationMain Combinationmain;
    private BGAcceTrigger BGAccetrigger;

    private PlayerItemList pitemlist;

    private Buf_Power_Keisan bufpower_keisan;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject BG_Imagepanel;
    private List<GameObject> bgwall_sprite = new List<GameObject>();

    private GameObject BG_effectpanel;
    private List<GameObject> bgeffect_obj = new List<GameObject>();

    private GameObject bgweather_image_panel;
    private List<GameObject> bg_weather_image = new List<GameObject>();

    //Live2Dモデルの取得
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private int default_live2d_draworder;
    private Vector3 Live2d_default_pos;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;
    private int trans_position;
    public int ResultComplete_flag;
    private bool live2d_posmove_flag; //位置を変更したフラグ
    private GameObject character_root;
    private GameObject character_move;
    private GameObject Anchor_Pos;

    private GameObject compoundselect_onoff_obj;    

    private GameObject compoBGA_image;
    private GameObject compoBGA_imageOri;
    private GameObject compoBGA_imageRecipi;
    private GameObject compoBGA_imageExtreme;

    private GameObject original_toggle;
    private GameObject recipi_toggle;
    private GameObject topping_toggle;
    private GameObject extreme_toggle;
    private GameObject roast_toggle;
    private GameObject blend_toggle;

    private GameObject menu_toggle;
    private GameObject girleat_toggle;
    private GameObject shop_toggle;
    private GameObject bar_toggle;
    private GameObject getmaterial_toggle;
    private GameObject stageclear_toggle;
    private GameObject sleep_toggle;
    private GameObject system_toggle;
    private GameObject status_toggle;
    private GameObject hinttaste_toggle;

    private GameObject MainUICloseButton;
    //private GameObject HintTasteButton;
    private Button extreme_Button;
    private Button recipi_Button;
    private GameObject sell_Button;
    private GameObject present_Button;
    private GameObject stageclear_panel;
    private GameObject stageclear_Button;
    private Toggle stageclear_button_toggle;
    private Text stageclear_button_text;
    private string stageclear_default_text;

    private bool status_zero_readOK;
    private bool Recipi_loading;
    private bool GirlLove_loading;
    public bool check_recipi_flag;
    public bool check_GirlLoveEvent_flag;
    public bool check_GirlLoveSubEvent_flag;
    public bool check_OkashiAfter_flag;
    public bool check_CompoAfter_flag; //Exp_Controllerから読み出し
    public bool check_GetMat_flag; //採取地から帰ってきたあとのサブイベントのチェック用
    private int not_read_total;
    private int _checkexp;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject yes_no_panel; //通常時のYes, noボタン
    private GameObject yes_no_clear_panel; //クリア時のYes, noボタン
    private GameObject yes_no_clear_okashi_panel; //クリア時のYes, noボタン
    private GameObject yes_no_sleep_panel; //寝るかどうかのYes, noボタン

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject autosave_panel;

    private int i, j, _id, ev_id;
    private int random;
    private int nokori_kaisu;
    private int event_num;
    private int recipi_num;
    private int comp_ID;
    private int clear_love;
    private int recipi_id;
    private bool read_girlevent;
    public bool SubEvAfterHeartGet; //Utage_scenarioからも読まれる
    private int SubEvAfterHeartGet_num;
    private bool subevent_after_end;
    private bool GetFirstCollectionItem;
    private bool GetEmeraldItem;
    private string GetEmeraldItemName;
    private int get_heart;

    private string _todayfood;
    private List<string> _todayfood_lib = new List<string>();
    private int _todayfoodexpence;
    private List<int> _todayfoodexpence_lib = new List<int>();

    //デバッグ確認用　ゲーム中では、GameMgr.compound_status（select）を使う。
    public int compound_status;
    public int compound_select;

    public int event_itemID; //イベントレシピ使用時のイベントのID

    public bool Load_eventflag; //ロード直後のイベント発生フラグ

    public string originai_text;
    public string extreme_text;
    public string recipi_text;

    private bool gameover_loading;
    private bool Sleep_on;
    private bool mute_on;

    private int picnic_exprob;

    private GameObject HintObjectGroup;
    private GameObject ClickPanel_1;
    private GameObject ClickPanel_2;

    private int motion_layer_num = 1;

    // Use this for initialization
    void Start()
    {
       
        //Debug.Log("Compound scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //セーブコントローラーの取得
        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();

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
        debug_panel = GameObject.FindWithTag("Debug_Panel").GetComponent<Debug_Panel>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();
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
        yes = canvas.transform.Find("Yes_no_Panel/Yes").gameObject;
        no = canvas.transform.Find("Yes_no_Panel/No").gameObject;

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_clear_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel1").gameObject;
        yes_no_sleep_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel2").gameObject;
        yes_no_clear_okashi_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel3").gameObject;

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("Compound_BGPanel_A/FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        //システムパネルの取得
        system_panel = canvas.transform.Find("SystemPanel").gameObject;

        //ステータスパネルの取得
        status_panel = canvas.transform.Find("StatusPanel").gameObject;

        //お菓子ヒントパネルの取得
        okashihint_panel = canvas.transform.Find("TasteHintPanel").gameObject;

        //オートセーブ表示パネルの取得
        autosave_panel = canvas.transform.Find("AutoSaveCompletePanel").gameObject;

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //材料ランダムで３つ手に入るオブジェクトの取得
        get_material_obj = GameObject.FindWithTag("GetMaterial");
        get_material = get_material_obj.GetComponent<GetMaterial>();

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        //女の子の反映用ハートエフェクト取得
        GirlHeartEffect_obj = GameObject.FindWithTag("Particle_Heart_Character");

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();
        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.transform.Find("Text").GetComponent<Text>();

        //エクストリームパネルの取得+初期化
        Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");
        extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();       

        //ボタンの取得
        extreme_Button = Extremepanel_obj.transform.Find("Comp/ExtremeButton").gameObject.GetComponent<Button>(); //エクストリームボタン

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();        

        //黒半透明パネルの取得
        black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;
        black_panel_A.SetActive(false);

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;
        compoBG_A.SetActive(false);
        compoBGA_image = canvas.transform.Find("Compound_BGPanel_A/BG").gameObject;
        compoBGA_imageOri = canvas.transform.Find("Compound_BGPanel_A/OriCompoImage").gameObject;
        compoBGA_imageRecipi = canvas.transform.Find("Compound_BGPanel_A/RecipiCompoImage").gameObject;
        compoBGA_imageExtreme = canvas.transform.Find("Compound_BGPanel_A/ExtremeImage").gameObject;
        ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
        ResultBGimage.SetActive(false);
        compoBG_A_effect = GameObject.FindWithTag("Comp_Effect").gameObject;
        compoBG_A_effect.SetActive(false);

        //調合選択画面の取得
        SelectCompo_panel_1 = canvas.transform.Find("Compound_BGPanel_A/SelectPanel_1").gameObject;
        SelectCompo_panel_1.SetActive(false);

        //ピクニック用のテキスト取得
        picnic_itemText = canvas.transform.Find("Compound_BGPanel_A/SelectPanel_1/Picnic_yesno/ItemText/Text").GetComponent<Text>();

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel/Comp").gameObject;
        getmatplace = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();
        getmatplace_panel.SetActive(false);
        GetMatStatusButton_obj = canvas.transform.Find("MainUIPanel/Comp/GetMatStatusPanel").gameObject;

        //タッチ判定オブジェクトの取得
        touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

        //時間管理オブジェクトの取得
        time_controller = canvas.transform.Find("MainUIPanel/Comp/TimePanel").GetComponent<TimeController>();

        //お金パネル
        moneyStatus_Controller = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();

        //キー入力受付コントローラーの取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();

        //Live2Dモデルの取得
        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
        cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
        default_live2d_draworder = cubism_rendercontroller.SortingOrder;
        Live2d_default_pos = _model_obj.transform.localPosition;
        live2d_animator = _model_obj.GetComponent<Animator>();
        live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。
        ResultComplete_flag = 0; //調合が完了したよフラグ
        live2d_posmove_flag = false;
        character_root = GameObject.FindWithTag("CharacterRoot").gameObject;
        character_move = character_root.transform.Find("CharacterMove").gameObject;
        Anchor_Pos = character_move.transform.Find("Anchor_1").gameObject;

        compoundselect_onoff_obj = canvas.transform.Find("MainUIPanel/Comp/CompoundSelect_ScrollView").gameObject;

        //ヒントボタンの取得
        HintObjectGroup = canvas.transform.Find("MainUIPanel/HintObject/").gameObject;
        ClickPanel_1 = canvas.transform.Find("MainUIPanel/HintObject/ClickPanel1").gameObject;
        ClickPanel_1.SetActive(false);
        ClickPanel_2 = canvas.transform.Find("MainUIPanel/HintObject/ClickPanel2").gameObject;
        ClickPanel_2.SetActive(false);

        //
        //トグル・UI関係
        //

        //original_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Original_Toggle").gameObject;
        //extreme_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Extreme_Toggle").gameObject;
        //roast_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Roast_Toggle").gameObject;
        //blend_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Blend_Toggle").gameObject;

        recipi_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Recipi_Toggle").gameObject;
        //recipi_toggle = canvas.transform.Find("MainUIPanel/Comp/Recipi_Toggle").gameObject;
        recipi_toggle.SetActive(false);

        girleat_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GirlEat_Toggle").gameObject;
        //girleat_toggle = canvas.transform.Find("MainUIPanel/Comp/GirlEat_Toggle").gameObject;
        //girleat_toggle.SetActive(false);

        //hinttaste_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/HintTaste_Toggle").gameObject;
        hinttaste_toggle = canvas.transform.Find("MainUIPanel/Comp/HintTaste_Toggle").gameObject;
        hinttaste_toggle.SetActive(false);

        menu_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/ItemMenu_Toggle").gameObject;       
        shop_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Shop_Toggle").gameObject;
        bar_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Bar_Toggle").gameObject;
        getmaterial_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GetMaterial_Toggle").gameObject;
        stageclear_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;
        sleep_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Sleep_Toggle").gameObject;
        system_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/System_Toggle").gameObject;
        status_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Status_Toggle").gameObject;

        //メインUIパネルの取得
        mainUI_panel_obj = canvas.transform.Find("MainUIPanel").gameObject;
        UIOpenButton_obj = canvas.transform.Find("MainUIOpenButton").gameObject;
        MainUICloseButton = canvas.transform.Find("MainUIPanel/Comp/MainUICloseButton").gameObject;
        mainUIFrame_panel = canvas.transform.Find("MainUIPanel/Comp/MainUIPanelTopFrame").gameObject;
        SceneStart_flag = false;

        //ステージパネルの取得
        Stagepanel_obj = canvas.transform.Find("MainUIPanel/Comp/StagePanel").gameObject;

        //時間表示パネルの取得
        TimePanel_obj1 = canvas.transform.Find("MainUIPanel/Comp/TimePanel/TimeHyouji_1").gameObject;
        TimePanel_obj2 = canvas.transform.Find("MainUIPanel/Comp/TimePanel/TimeHyouji_2").gameObject;
        TimePanel_obj2.SetActive(false);

        //好感度バーの取得
        girl_love_exp_bar = canvas.transform.Find("MainUIPanel/Girl_love_exp_bar").gameObject;
        girl_param = girl_love_exp_bar.transform.Find("Girllove_param").GetComponent<Text>();

        //満腹ゲージの取得
        manpuku_bar = canvas.transform.Find("MainUIPanel/ManpukuBar").gameObject;
        manpuku_slider = manpuku_bar.GetComponent<Slider>();

        //お金ステータスパネルの取得
        moneystatus_panel = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").gameObject;

        
        //えめらるどんぐりパネルの取得
        kaerucoin_panel = canvas.transform.Find("KaeruCoin_Panel").gameObject;

        stageclear_panel = canvas.transform.Find("MainUIPanel/StageClearButton_Panel").gameObject;
        stageclear_Button = canvas.transform.Find("MainUIPanel/StageClearButton_Panel/StageClear_Button").gameObject;
        stageclear_button_toggle = stageclear_Button.GetComponent<Toggle>();
        stageclear_button_text = stageclear_Button.transform.Find("TextPlate/Text").GetComponent<Text>();
        stageclear_default_text = stageclear_button_text.text;
        stageclear_button_toggle.isOn = false;
        stageclear_Button.SetActive(false);
        

        //背景天気オブジェクトの取得
        bgweather_image_panel = GameObject.FindWithTag("BGImageWindowOutPanel");
        BG_Imagepanel = GameObject.FindWithTag("BG");
        BG_effectpanel = GameObject.FindWithTag("BG_Effect");

        bg_weather_image.Clear();
        foreach (Transform child in bgweather_image_panel.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            bg_weather_image.Add(child.gameObject);
        }

        bgwall_sprite.Clear();
        foreach (Transform child in BG_Imagepanel.transform)
        {
            bgwall_sprite.Add(child.gameObject);
        }

        bgeffect_obj.Clear();
        foreach (Transform child in BG_effectpanel.transform)
        {
            bgeffect_obj.Add(child.gameObject);
        }

        Change_BGimage();
        //メモボタン
        //HintTasteButton = canvas.transform.Find("MainUIPanel/HintTasteButton").gameObject;
        //HintTasteButton.SetActive(false);

        /* --- */

        Sleep_on = false;

        status_zero_readOK = false;
        girlEat_ON = false;
        compo_ON = false;
        Recipi_loading = false;
        GirlLove_loading = false;
        check_recipi_flag = false;
        check_GirlLoveEvent_flag = false;
        check_GirlLoveSubEvent_flag = false;
        check_OkashiAfter_flag = false;
        check_CompoAfter_flag = false;
        check_GetMat_flag = false;
        read_girlevent = false;
        gameover_loading = false;
        mute_on = false;
        SubEvAfterHeartGet = false;
        subevent_after_end = false;
        GetFirstCollectionItem = false;
        GetEmeraldItem = false;
        Load_eventflag = false;

        //女の子　お菓子ハングリー状態のリセット
        girl1_status.Girl1_Status_Init();
        girl1_status.DefFaceChange();

        //ステージクリア用の好感度数値
        switch (GameMgr.stage_number)
        {
            case 1:

                if (GameMgr.stage1_load_ok != true)
                {
                    GameMgr.stage1_load_ok = true;
                    GameMgr.scenario_flag = 110;                   
                }
                //clear_love = GameMgr.stage1_clear_love;
                break;

            case 2:

                //clear_love = GameMgr.stage2_clear_love;
                break;

            case 3:

                //clear_love = GameMgr.stage3_clear_love;
                break;
        }

        //メイン画面に表示する、現在のクエスト
        questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

        //初期メッセージ
        StartMessage();        
        text_area_Main.SetActive(false);

        //初期アイテムの取得。一度きり。
        DefaultStartPitem();

        //各調合時のシステムメッセージ集
        originai_text = "新しくお菓子を作ろう！" + "\n" + "好きな材料を" + GameMgr.ColorYellow + 
            "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";
        extreme_text = "仕上げをしよう！にいちゃん！ 一個目の材料を選んでね。";
        recipi_text = "ヒカリのお菓子手帳だよ！" + "\n" + "にいちゃんのレシピが増えたら、ここに書いてくね！";

        

        //飾りアイテムのセット
        BGAccetrigger = GameObject.FindWithTag("BGAccessory").GetComponent<BGAcceTrigger>();
        BGAccetrigger.DrawBGAcce();

        ReturnBackHome();

        //セーブがあるかどうかをチェック
        save_controller.SystemloadCheck_SaveOnly();

        //二週目以降はエメラルショップはじめからでてる。
        if (GameMgr.ending_count >= 1)
        {
            matplace_database.matPlaceKaikin("Emerald_Shop");
        }

        //ロード画面から読み込んだ際の処理
        if (GameMgr.GameLoadOn)
        {
            Debug.Log("ロード画面から読み込んだ");

            GameMgr.GameLoadOn = false;
            save_controller.DrawGameScreen();
            keymanager.InitCompoundMainScene();
            GameMgr.MesaggeKoushinON = true;
            StartMessage();           

            character_move.transform.position = new Vector3(0f, 0, 0); //念のため、ゼロにリセット

            Debug.Log("エンディング回数: " + GameMgr.ending_count);

            //ロード直後のサブイベントを発生させる
            //Load_eventflag = true; //ロード直後に、おかえりなさい～のようなサブイベントを発生
            //compound_Main.check_GirlLoveEvent_flag = false; //compound_Mainからロードしても、おかえりなさい～が発生
        }

        Debug.Log("ストーリーモード: " + GameMgr.Story_Mode);

        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。
    }

    //家に帰ってきたときの処理
    void ReturnBackHome()
    {
        if (GameMgr.Scene_back_home)
        {
            GameMgr.Scene_back_home = false;

            //入店の音
            sc.PlaySe(38); //ドア
            sc.PlaySe(50); //ベル

            //パネルを閉じる
            mainUI_panel_obj.GetComponent<MainUIPanel>().OnCloseButton(); //メニューは最初閉じ

            //オートセーブ
            if (GameMgr.AUTOSAVE_ON)
            {
                save_controller.OnSaveMethod();
                Debug.Log("オートセーブ完了");

                AutoSaveCompleteText();
            }
        }
    }

    //オートセーブ完了できたら、ちっちゃくテキストを表示する
    public void AutoSaveCompleteText()
    {
        autosave_panel.SetActive(true);

        autosave_panel.GetComponent<CanvasGroup>().DOFade(0, 1.0f).SetDelay(2)
            .OnComplete(() => autosave_panel.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグでの確認用
        compound_status = GameMgr.compound_status;
        compound_select = GameMgr.compound_select;

        //お金が0を下回ったらゲームオーバー
        /*if(PlayerStatus.player_money <= 0)
        {
            if (!gameover_loading)
            {
                gameover_loading = true; //アップデートを更新しないようにしている。
                //お金が0になったので、ゲーム終了　ぐええ
                Debug.Log("ゲームオーバー画面表示");

                FadeManager.Instance.LoadScene("999_Gameover", 0.3f);
            }
        }*/

        if (GameMgr.scenario_ON != true)
        {
            switch (GameMgr.scenario_flag)
            {

                case 110: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。チュートリアルするかどうか。

                    GameMgr.scenario_ON = true; //これがONのときは、調合シーンの、調合ボタンなどはオフになり、シナリオを優先する。「Utage_scenario.cs」のUpdateが同時に走っている。
                    //GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
                    //GameMgr.compound_status = 1000;

                    break;

                default:
                    break;
            }            
        }

        //上のイベントが先に発生した場合、以下の処理は無視される。以下は、別シーンから戻ってきたときに、何かイベントが発生するかどうか。
        if (GameMgr.scenario_ON != true)
        {
            if (GameMgr.CompoundEvent_flag)
            {
                GameMgr.CompoundEvent_flag = false;

                GameMgr.CompoundEvent_storynum = GameMgr.CompoundEvent_num;
                GameMgr.CompoundEvent_storyflag = true;
                GameMgr.scenario_ON = true; //これがONのときは、調合シーンの、調合ボタンなどはオフになり、シナリオを優先する。「Utage_scenario.cs」のUpdateが同時に走っている。

                GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
                GameMgr.compound_status = 1000;
            }
        }

        //スペシャルアニメスタート時まではオフ
        if (GameMgr.tutorial_ON != true)
        {
            if (!girl1_status.special_animatFirst)
            {
                WindowOff();
            }
        }

        //ピクニックイベント中、お菓子制作中は更新する。
        if(GameMgr.picnic_event_reading_now)
        {
            MainCompoundMethod();            
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。チュートリアルなどの強制イベントのチェック。
        if (GameMgr.scenario_ON == true)
        {
            //Debug.Log("ゲームマネジャー　シナリオON");           

            //チュートリアルモードがONになったら、この中の処理が始まる。
            if (GameMgr.tutorial_ON == true)
            {
                touch_controller.Touch_OnAllOFF();
                girl1_status.HukidashiFlag = false;

                switch (GameMgr.tutorial_Num)
                {
                    case 0: //最初にシナリオを読み始める。

                        canvas.SetActive(false);

                        //一時的に腹減りを止める。+腹減りステータスをリセット
                        girl1_status.GirlEat_Judge_on = false;
                        girl1_status.DeleteHukidashiOnly();
                        //girl1_status.Girl_Full();
                        girl1_status.Girl1_Status_Init();
                        girl1_status.OkashiNew_Status = 1;
                        GameMgr.tutorial_Num = 1; //退避
                        break;

                    case 10: //宴ポーズ。エクストリームパネルを押そう！で、待機。

                        canvas.SetActive(true);
                        MainCompoundMethod();
                        compoundselect_onoff_obj.SetActive(false);                        
                        OffCompoundSelectnoExtreme();
                        //extreme_Button.interactable = true;

                        _textmain.text = "左の「お菓子パネル」を押してみよう！";
                        break;

                    case 15: //はじめてパネルを開いた。オリジナル調合を押そう！

                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(false);

                        compoBGA_image.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = false;

                        Extremepanel_obj.SetActive(false);

                        select_original_button.interactable = false;
                        select_recipi_button.interactable = false;
                        select_extreme_button.interactable = false;
                        select_no_button.interactable = false;

                        break;

                    case 16: //メッセージおわり

                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(false);

                        compoBGA_image.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = false;

                        Extremepanel_obj.SetActive(false);

                        select_original_button.interactable = true;
                        select_recipi_button.interactable = false;
                        select_extreme_button.interactable = false;
                        select_no_button.interactable = false;

                        break;

                    case 20: //エクストリームパネルを押して、オリジナル調合画面を開いた

                        MainCompoundMethod();

                        select_recipi_button.interactable = true;
                        select_extreme_button.interactable = true;
                        select_no_button.interactable = true;

                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(false);

                        compoBGA_image.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
                        compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = false;
                        pitemlistController.Offinteract();
                        kakuritsuPanel_obj.SetActive(false);

                        recipiMemoButton.GetComponent<Button>().interactable = false;

                        break;

                    case 30: //宴がポーズ状態。右のレシピメモを押そう。

                        recipiMemoButton.GetComponent<Button>().interactable = true;

                        text_area.SetActive(true);
                        _text.text = "右上の「レシピをみる」ボタンを押して" + "\n" + "クッキーを作ってみよう！";
                        break;

                    case 40: //メモ画面を開いた。

                        text_area.SetActive(false);
                        break;

                    case 50: //宴ポーズ。オリジナル調合をしてみるところ。

                        pitemlistController.Oninteract();
                        text_area.SetActive(true);
                        _text.text = "左のリストから、" + "\n" + "好きな材料を" + GameMgr.ColorYellow + "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。"; ;

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

                        canvas.SetActive(false);

                        break;

                    case 90: //「あげる」ボタンを押すところ。「あげる」のみをON、他のボタンはオフ。

                        //Debug.Log("GameMgr.チュートリアルNo: " + GameMgr.tutorial_Num);
                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);
                        OffCompoundSelect();
                        text_area.SetActive(false);

                        //girl1_status.SetOneQuest(1);　//comp_Numを直接指定
                        girl1_status.Girl_Hungry();
                        girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                        GameMgr.tutorial_Num = 95; //退避

                        break;

                    case 100:

                        MainCompoundMethod();
                        canvas.SetActive(true);
                        compoundselect_onoff_obj.SetActive(true);

                        _textmain.text = "お菓子をあげてみよう！";

                        //このタイミングで、アイテムのどれかが0になっていたら、また、全てのアイテムを5ずつにリセットしなおす。
                        if (pitemlist.KosuCount("komugiko") <= 1 || pitemlist.KosuCount("butter") <= 1 || pitemlist.KosuCount("suger") <= 1)
                        {
                            pitemlist.addPlayerItemString("komugiko", 5 - pitemlist.KosuCount("komugiko"));
                            pitemlist.addPlayerItemString("butter", 5 - pitemlist.KosuCount("butter"));
                            pitemlist.addPlayerItemString("suger", 5 - pitemlist.KosuCount("suger"));
                        }

                        GameMgr.tutorial_Num = 105; //退避
                        break;

                    case 105:

                        MainCompoundMethod();

                        OffCompoundSelect();
                        girleat_toggle.GetComponent<Toggle>().interactable = true;

                        break;

                    case 110:

                        MainCompoundMethod();
                        girl1_status.DeleteHukidashiOnly();
                        canvas.SetActive(false);

                        break;

                    case 120:

                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);

                        girl1_status.timeGirl_hungry_status = 2; //一回、画像を元に戻す。

                        //girl1_status.SetOneQuest(11);
                        girl1_status.Girl_Hungry();
                        girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                        GameMgr.tutorial_Num = 130;

                        GameMgr.tutorial_Progress = true;
                        break;

                    case 130:

                        text_area.SetActive(false);
                        break;

                    case 140:

                        extreme_Button.interactable = true;
                        canvas.SetActive(true);

                        _textmain.text = "ねこクッキーを作ってみよう！";

                        break;

                    case 150: //レシピボタンでも～を説明中。ボタンは押せないようにしておく。

                        MainCompoundMethod();

                        select_original_button.interactable = false;
                        select_extreme_button.interactable = false;
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
                        canvas.SetActive(false);

                        break;

                    case 200:

                        MainCompoundMethod();

                        canvas.SetActive(true);
                        OffCompoundSelectnoExtreme();
                        //extreme_Button.interactable = true;

                        _textmain.text = "もう一度パネルを押してみよう！";

                        break;

                    case 210: //エクストリーム調合　他のボタンは触れない

                        MainCompoundMethod();

                        select_original_button.interactable = false;
                        select_extreme_button.interactable = true;
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
                        canvas.SetActive(false);


                        break;

                    case 280:

                        MainCompoundMethod();
                        canvas.SetActive(true);

                        compoundselect_onoff_obj.SetActive(true);
                        OffCompoundSelect();
                        girleat_toggle.GetComponent<Toggle>().interactable = true;
                        girl1_status.timeGirl_hungry_status = 1;

                        _textmain.text = "お菓子をあげてみよう！";

                        GameMgr.tutorial_Num = 285; //退避
                        break;

                    case 285:

                        MainCompoundMethod();

                        break;

                    case 290:

                        MainCompoundMethod();
                        girl1_status.DeleteHukidashiOnly();
                        canvas.SetActive(false);

                        break;

                    default:

                        break;
                }

            }
            else //チュートリアル以外、デフォルトで、宴を読んでいるときの処理
            {
                mainUI_panel_obj.GetComponent<MainUIPanel>().OnCloseButton(); //メニューは最初閉じ
                WindowOff();
                
                check_recipi_flag = false;

                //腹減りカウント一時停止
                girl1_status.GirlEat_Judge_on = false;
                //girl1_status.Girl_Full();
                girl1_status.DeleteHukidashiOnly();
                girl1_status.Girl1_Status_Init();

                touch_controller.Touch_OnAllOFF();
                SceneStart_flag = false;

                //テキストエリアの表示
                if (GameMgr.picnic_event_reading_now)
                {
                    if (GameMgr.compound_select == 1 || GameMgr.compound_select == 2 || GameMgr.compound_select == 3 || GameMgr.compound_select == 120)
                    {

                    }
                    else
                    {
                        text_area.SetActive(false);
                    }
                }
                else
                {
                    text_area.SetActive(false);
                }
            }

        }
        else //以下が、通常の処理
        {

            if (girlEat_ON) //お菓子判定中の間は、無条件で、メインの処理は無視する。
            {

            }
            else
            {
                if (compo_ON) //お菓子調合中もメインの処理は無視。おわったら、サブイベントチェックしてから、メインへ。
                {

                }
                else
                {
                    //クエストクリア時、次のお菓子イベントが発生するかどうかのチェック。
                    if (check_GirlLoveEvent_flag == false)
                    {
                        Debug.Log("イベントチェックON");
                        //腹減りカウント一時停止
                        girl1_status.GirlEat_Judge_on = false;

                        Check_GirlLoveEvent();
                    }
                    else
                    {
                        //サブイベントの発生をチェック。
                        if (check_GirlLoveSubEvent_flag == false)
                        {
                            Debug.Log("サブイベントチェックON");
                            //腹減りカウント一時停止
                            girl1_status.GirlEat_Judge_on = false;

                            //好感度に応じて発生するサブイベント
                            GirlLove_EventMethod();
                        }
                        else
                        {
                            //読んでいないレシピがあれば、読む処理。優先順位二番目。
                            if (check_recipi_flag != true)
                            {
                                //腹減りカウント一時停止
                                girl1_status.GirlEat_Judge_on = false;

                                //Debug.Log("チェックレシピ中");
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
        }
    }


    //メインの調合シーンの処理  Utageからも読まれる。
    public void MainCompoundMethod()
    {
        switch (GameMgr.compound_status)
        {
            case 0:

                Debug.Log("メインの調合シーン　GameMgr.compound_status:0 スタート");               

                //ボタンなどの状態の初期設定
                if (GameMgr.tutorial_ON != true)
                {
                    canvas.SetActive(true);
                        
                    //腹減りカウント開始
                    girl1_status.GirlEat_Judge_on = true;
                    girl1_status.WaitHint_on = true;

                    compoBGA_image.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageOri.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = true;
                    GameMgr.scenario_read_endflag = false;
                    
                    keymanager.InitCompoundMainScene();
                }

                mainUI_panel_obj.SetActive(true);
                GetMatStatusButton_obj.SetActive(false);
                recipilist_onoff.SetActive(false);
                playeritemlist_onoff.SetActive(false);
                yes_no_panel.SetActive(false);
                getmatplace_panel.SetActive(false);               
                kakuritsuPanel_obj.SetActive(false);
                black_panel_A.SetActive(false);
                ResultBGimage.SetActive(false);
                compoBG_A.SetActive(false);
                compoBG_A_effect.SetActive(false);
                system_panel.SetActive(false);
                status_panel.SetActive(false);
                okashihint_panel.SetActive(false);
                recipiMemoButton.SetActive(false);
                extreme_panel.SetInitParamExtreme(); //compo=0のタイミングで、毎回エクストリームパネルのアイテム削除を判定する。

                WindowOn();                
                select_original_button.interactable = true;
                select_recipi_button.interactable = true;
                select_no_button.interactable = true;
                               
                OnCompoundSelect();

                //ステージ更新
                mainUI_panel_obj.GetComponent<MainUIPanel>().StageNumKoushin();

                //装備品アイテムの効果計算
                bufpower_keisan.CheckEquip_Keisan();

                //
                //アニメーション、キャラの表情関係
                //
                //Live2Dデフォルト
                cubism_rendercontroller.SortingOrder = default_live2d_draworder;
                Anchor_Pos.transform.localPosition = new Vector3(0f, 0.134f, -5f);
                girl1_status.HukidashiFlag = true;
                girl1_status.tween_start = false;
                live2d_animator.Play("Idle", motion_layer_num, 0.0f);

                //時間のチェック。採取地から帰ってきたときのみ、リザルトパネルを押してから、更新
                if (getmatplace.slot_view_status == 0)
                {
                    Debug.Log("時間更新＆チェック");
                    time_controller.TimeCheck_flag = true;
                    time_controller.TimeKoushin(); //時間の更新
                }

                //音関係
                if (!GameMgr.tutorial_ON)
                {
                    //メインBGMを変更　ハートレベルに応じてBGMも切り替わる。
                    bgm_change_story();

                    if (bgm_change_flag == true)
                    {
                        bgm_change_flag = false;
                       
                        sceneBGM.OnMainBGM();
                    }
                    if (bgm_changeuse_ON)
                    {
                        if (bgm_change_flag2 == true)
                        {
                            bgm_change_flag2 = false;
                            sceneBGM.OnMainBGMFade();         
                            //sceneBGM.OnMainBGM(); //即座に切り替え
                        }
                    }
                }
                sceneBGM.MuteOFFBGM();
                map_ambience.MuteOFF();

                //イベントに応じてコマンドを増やす関係
                FlagEvent();

                
                if(!exp_Controller._temp_extremeSetting) //もしfalseだったら、このタイミングでも、パネルのアイテムを削除する。
                {
                    extreme_panel.deleteExtreme_Item();
                }                                                                        

                if (!subevent_after_end)
                {
                    if (GameMgr.tutorial_ON != true)
                    {
                        if (girl1_status.special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート。それまでは他のボタン入力できない。
                        {
                            mainUI_panel_obj.SetActive(false);
                            touch_controller.Touch_OnAllOFF();
                        }
                    }

                    /*if (read_girlevent) 
                    {
                        read_girlevent = false;
                        mainUI_panel_obj.GetComponent<MainUIPanel>().OnCloseButton(); //メニューは最初閉じ
                    }*/
                }

                if (!SceneStart_flag)
                {
                    mainUI_panel_obj.GetComponent<MainUIPanel>().OnCloseButton(); //メニューは最初閉じ
                    SceneStart_flag = true; //シーンの最初のみこの処理をいれる。
                }

                //調合成功後に、サブイベントチェック。ちなみに、このcompoundstatus=0の最後にいれないと、作った後のサブイベント発生はバグるので注意。
                if (check_CompoAfter_flag)
                {
                    Debug.Log("調合後に、サブイベントチェック入る");
                    check_CompoAfter_flag = false;
                    check_GirlLoveSubEvent_flag = false; //イベントチェック
                }

                status_zero_readOK = true;

                break;

            case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                GameMgr.compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択

                recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                kakuritsuPanel_obj.SetActive(true);
                compoBG_A.SetActive(true);
                compoBG_A_effect.SetActive(false);
                //compoBGA_image.SetActive(false);
                compoBGA_imageOri.SetActive(false);
                compoBGA_imageRecipi.SetActive(true);
                compoBGA_imageExtreme.SetActive(false);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();               
                time_controller.TimeCheck_flag = false;
                yes_no_panel.SetActive(true);
                yes.SetActive(false);
                stageclear_panel.SetActive(false);

                if (GameMgr.tutorial_ON != true)
                {  }
                else
                {
                    no.SetActive(false);
                }

                text_area.SetActive(true);
                WindowOff();
                StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

                //ヒカリちゃんを表示する
                ReDrawLive2DPos_Compound();

                //BGMを変更
                if (!GameMgr.tutorial_ON)
                {
                    if (bgm_changeuse_ON)
                    {
                        if (bgm_change_flag2 != true)
                        {
                            sceneBGM.OnCompoundBGM();
                            bgm_change_flag2 = true;
                        }
                    }
                }
                map_ambience.Mute();

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.WaitHint_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                keymanager.SelectOff();

                break;

            case 2: //エクストリーム調合の処理を開始。クリック後に処理が始まる。

                GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                GameMgr.compound_select = 2; //トッピング調合を選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                kakuritsuPanel_obj.SetActive(false);
                compoBG_A.SetActive(true);
                compoBG_A_effect.SetActive(false);
                //compoBGA_image.SetActive(false);
                compoBGA_imageOri.SetActive(false);
                compoBGA_imageRecipi.SetActive(false);
                compoBGA_imageExtreme.SetActive(true);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                time_controller.TimeCheck_flag = false;
                stageclear_panel.SetActive(false);

                text_area.SetActive(true);
                WindowOff();
                StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

                //ヒカリちゃんを表示する
                ReDrawLive2DPos_Compound();

                //BGMを変更
                /*if (!GameMgr.tutorial_ON)
                {                                     
                    if (bgm_changeuse_ON) //調合時にBGMを切り替えるかどうか。
                    {
                        if (bgm_change_flag2 != true)
                        {
                            sceneBGM.OnCompoundBGM();
                            bgm_change_flag2 = true;
                        }
                    }
                }*/
                map_ambience.Mute();

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.WaitHint_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。

                extreme_panel.extreme_Compo_Setup();

                keymanager.SelectOff();

                break;

            case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                GameMgr.compound_select = 3; //オリジナル調合を選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                kakuritsuPanel_obj.SetActive(true);

                compoBG_A.SetActive(true);
                compoBG_A_effect.SetActive(false);
                //compoBGA_image.SetActive(false);
                compoBGA_imageOri.SetActive(true);
                compoBGA_imageRecipi.SetActive(false);
                compoBGA_imageExtreme.SetActive(false);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                recipiMemoButton.SetActive(true);
                recipimemoController_obj.SetActive(false);
                time_controller.TimeCheck_flag = false;
                memoResult_obj.SetActive(false);
                stageclear_panel.SetActive(false);

                text_area.SetActive(true);
                WindowOff();
                StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

                //ヒカリちゃんを表示する
                ReDrawLive2DPos_Compound();

                //BGMを変更
                /*if (!GameMgr.tutorial_ON)
                {
                    if (bgm_changeuse_ON)
                    {
                        if (bgm_change_flag2 != true)
                        {
                            sceneBGM.OnCompoundBGM();
                            bgm_change_flag2 = true;
                        }
                    }
                }*/
                map_ambience.Mute();

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.WaitHint_on = false;
                girl1_status.Girl1_touchhair_start = false; //gaze状態もリセット

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 

                keymanager.SelectOff();

                if (GameMgr.tutorial_ON == true)
                {
                    if (GameMgr.tutorial_Num == 16)
                    {
                        GameMgr.tutorial_Progress = true;
                        GameMgr.tutorial_Num = 20;
                    }
                }

                break;

            case 4: //調合シーンに入ってますよ、というフラグ。各ケース処理後、必ずこの中の処理に移行する。yes, noボタンを押されるまでは、待つ状態に入る。

                break;

            case 5: //「焼く」を選択

                compoundselect_onoff_obj.SetActive(false);

                GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                GameMgr.compound_select = 5; //焼くを選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。

                break;

            /*case 5: //ブレンド調合の処理（未使用）

            compoundselect_onoff_obj.SetActive(false);
            compound_status = 4; //調合シーンに入っています、というフラグ
            compound_select = 5; //ブレンド調合を選択
            recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
            no.SetActive(true);

            break;*/

            case 6: //オリジナル調合かレシピ調合を選択できるパネルを表示

                //BGMを変更
                if (!GameMgr.tutorial_ON)
                {
                    if (bgm_changeuse_ON)
                    {
                        if (bgm_change_flag2 != true)
                        {
                            sceneBGM.OnCompoundBGM();
                            bgm_change_flag2 = true;
                        }
                    }
                }

                /*if (!GameMgr.tutorial_ON)
                {
                    if (bgm_changeuse_ON)
                    {
                        if (bgm_change_flag2 == true)
                        {
                            bgm_change_flag2 = false;
                            sceneBGM.OnMainBGMFade();
                        }
                    }
                }*/

                StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

                GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                GameMgr.compound_select = 6;

                playeritemlist_onoff.SetActive(false);
                recipilist_onoff.SetActive(false);
                kakuritsuPanel_obj.SetActive(false);
                stageclear_panel.SetActive(false);

                SelectCompo_panel_1.SetActive(true);
                compoBG_A.SetActive(true);
                compoBG_A_effect.SetActive(false);
                //compoBGA_image.SetActive(true);
                compoBGA_imageOri.SetActive(false);
                compoBGA_imageRecipi.SetActive(false);
                compoBGA_imageExtreme.SetActive(false);
                touch_controller.Touch_OnAllOFF();
                extreme_panel.extremeButtonInteractOFF();
                time_controller.TimeCheck_flag = false;
                yes_no_panel.SetActive(false);

                text_area.SetActive(false);
                WindowOff();

                map_ambience.Mute();

                //カメラリセット
                //アイドルに戻るときに0に戻す。
                trans = 0;

                //intパラメーターの値を設定する.
                maincam_animator.SetInteger("trans", trans);

                //ヒカリちゃんを表示しない。デフォルト描画順
                SetLive2DPos_Compound();
                cubism_rendercontroller.SortingOrder = default_live2d_draworder;  //ヒカリちゃんを表示しない。デフォルト描画順
                GameMgr.QuestManzokuFace = false; //おいしかった表情は、調合シーンに入るとリセットされる。

                recipiMemoButton.SetActive(false);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);
                
                if (extreme_panel.extreme_itemID != 9999 && PlayerStatus.player_extreme_kaisu > 0)　//extreme_panel.extreme_kaisu
                {
                    select_extreme_button.interactable = true;
                } else
                {
                    select_extreme_button.interactable = false;
                }

                //ピクニックイベント中は、ピクニックテキストのアイテムテキスト更新
                if (GameMgr.picnic_event_reading_now)
                {
                    if (extreme_panel.extreme_itemID != 9999)
                    {
                        picnic_itemText.text = GameMgr.ColorYellow + pitemlist.player_originalitemlist[extreme_panel.extreme_itemID].item_SlotName + "</color>" +
                        pitemlist.player_originalitemlist[extreme_panel.extreme_itemID].itemNameHyouji;
                    }
                    else
                    {
                        picnic_itemText.text = "なし";
                    }
                }

                //おいしそ～状態は、移動すると元に戻る。
                if (girl1_status.GirlOishiso_Status == 1)
                {
                    girl1_status.GirlOishiso_Status = 0;
                }


                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.WaitHint_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                keymanager.InitCompoundMainScene();

                break;

            case 10: //「あげる」を選択

                GameMgr.compound_status = 13; //あげるシーンに入っています、というフラグ
                GameMgr.compound_select = 10; //あげるを選択

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);                

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.Walk_Start = false;

                text_area.SetActive(true);
                WindowOff();

                card_view.PresentGirl(extreme_panel.extreme_itemtype, extreme_panel.extreme_itemID);
                StartCoroutine("Girl_present_Final_select");


                break;

            case 11: //お菓子をあげたあとの処理。女の子が、お菓子を判定

                GameMgr.compound_status = 12;
                text_area.SetActive(false);
                girlEat_ON = true; //お菓子判定中フラグ
                character_move.transform.position = new Vector3(0, 0, 0);

                //お菓子の判定処理を起動。引数は、決定したアイテムのアイテムIDと、店売りかオリジナルで制作したアイテムかの、判定用ナンバー 0or1 1=コンテストのとき
                girlEat_judge.Girleat_Judge_method(extreme_panel.extreme_itemID, extreme_panel.extreme_itemtype, 0);

                break;

            case 12: //お菓子を判定中

                break;

            case 13: //あげるかあげないかを選択中

                break;

            case 20: //材料採取地を選択中

                GameMgr.compound_status = 21; //
                GameMgr.compound_select = 20; //

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                touch_controller.Touch_OnAllOFF();
                time_controller.TimeCheck_flag = false;

                //おいしそ～状態は、元に戻る。
                if (girl1_status.GirlOishiso_Status == 1)
                {
                    girl1_status.GirlOishiso_Status = 0;
                }

                //UI OFF
                WindowOff();

                text_area.SetActive(true);
                moneystatus_panel.SetActive(true);
                GetMatStatusButton_obj.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;
                girl1_status.WaitHint_on = false;

                //吹き出しも消す
                girl1_status.DeleteHukidashiOnly();

                break;

            case 21: //採取地選択中

                break;

            case 22: //材料採取地に到着。探索中

                break;

            case 30: //「売る」を選択（未実装）

                compoundselect_onoff_obj.SetActive(false);

                GameMgr.compound_status = 31; //売るシーンに入っています、というフラグ
                GameMgr.compound_select = 30; //売るを選択

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

                GameMgr.compound_status = 32;

                extreme_panel.Sell_Okashi();
                break;

            case 40: //コンテストを選択

                GameMgr.compound_status = 41; //コンテスト進むかシーンに入っています、というフラグ
                GameMgr.compound_select = 40;             

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                text_area.SetActive(true);
                WindowOff();
                black_panel_A.SetActive(true);
                StartCoroutine("Contest_Final_select");
                break;

            case 41: //クリアするかどうか、選択中
                break;

            case 42: //次のお菓子へ進むかを選択

                GameMgr.compound_status = 43; //次のお菓子へ進むかシーンに入っています、というフラグ
                GameMgr.compound_select = 40;

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                text_area.SetActive(true);
                WindowOff();
                black_panel_A.SetActive(true);
                StartCoroutine("OkashiClear_Final_select");
                break;

            case 43: //次のお菓子へ進むかどうか、選択中
                break;

            case 50: //寝るを選択

                GameMgr.compound_status = 51; //売るシーンに入っています、というフラグ
                GameMgr.compound_select = 50; //売るを選択

                yes_no_sleep_panel.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                text_area.SetActive(true);
                WindowOff();
                black_panel_A.SetActive(true);
                StartCoroutine("Sleep_Final_select");
                break;

            case 51: //寝るかどうか選択中
                break;

            case 60: //レシピ本の選択画面を開いたとき

                black_panel_A.SetActive(true);
                GameMgr.compound_status = 61;
                GameMgr.compound_select = 60;
                recipilist_onoff.SetActive(true);

                WindowOff();
                break;

            case 61: //レシピ本選択中

                break;

            case 99: //アイテム画面を開いたとき
                
                black_panel_A.SetActive(true);
                GameMgr.compound_status = 99;
                GameMgr.compound_select = 99;
                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。

                WindowOff();

                break;

            case 100: //退避用 トグル選択中のときなどに使用

                break;            

            case 110: //調合、最後これでよいか選択中のステータス

                break;

            case 120: //なんらかの選択で、最後これでいいかどうかの確認中

                break;

            case 200: //システム画面を開いたとき

                GameMgr.compound_status = 201;
                GameMgr.compound_select = 200;

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                system_panel.SetActive(true); //システムパネルを表示。

                WindowOff();

                break;

            case 201: //システム画面選択中

                break;

            case 250: //お菓子ヒント画面を開いたとき

                GameMgr.compound_status = 251;
                GameMgr.compound_select = 250;

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                okashihint_panel.SetActive(true); //お菓子ヒントパネルを表示。

                WindowOff();

                break;

            case 251: //お菓子ヒント画面選択中

                break;

            case 300: //ステータス画面を開いたとき

                GameMgr.compound_status = 301;
                GameMgr.compound_select = 300;

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                status_panel.SetActive(true); //システムパネルを表示。

                WindowOff();

                break;

            case 301: //ステータス画面選択中

                break;

            case 999: //その他、なんらかの選択確認などで一時退避として使う。

                break;

            case 1000: //サブイベントなどを読み中

                break;

            case 1001: //サブイベント中、アイテムを渡すのをキャンセルするかどうか、確認する。                

                /*pitemlistController.Offinteract();

                text_area.SetActive(true);
                _text.text = "やっぱりやめる？";

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

                StartCoroutine("EventPresent_Final_select");*/

                break;

            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if (status_zero_readOK)
        {
            status_zero_readOK = false;

            switch (GameMgr.compound_status)
            {
                case 0:

                    if (!Sleep_on)
                    {
                        if (ResultComplete_flag != 0) //厨房から帰ってくるときの動き
                        {
                            Debug.Log("厨房から戻ってくる動き。");

                            //腹減りカウント一時停止
                            girl1_status.GirlEat_Judge_on = false;
                            girl1_status.ResetHukidashi();

                            ResultComplete_flag = 0;
                            //intパラメーターの値を設定する.  

                            //戻るアニメに遷移
                            trans_motion = 100; 
                            live2d_animator.SetInteger("trans_motion", trans_motion);
                            trans_expression = 2;
                            live2d_animator.SetInteger("trans_expression", trans_expression);

                            character_move.transform.DOMove(new Vector3(0f, 0, 0), 0.1f);
                            live2d_posmove_flag = false;
                            //
                        }
                        else
                        {
                                ResetLive2DPos_Face(); //表情をデフォルトに戻す。                            
                        }
                    }
                    else
                    {
                        ResultComplete_flag = 0;

                        trans_motion = 11; //位置をもとに戻す。
                        live2d_animator.SetInteger("trans_motion", trans_motion);
                        girl1_status.DefFaceChange();
                    }

                    GameMgr.compound_select = 0;
                    GameMgr.compound_status = 110; //退避
                    break;

                case 110: //退避用

                    break;
            }
        }
    }

    //調合シーンに入った時の、キャラクタ位置や状態など更新
    void SetLive2DPos_Compound()
    {
        cubism_rendercontroller.SortingOrder = 100;

        //位置変更
        character_move.transform.position = new Vector3(2.8f, 0, 0);
        live2d_posmove_flag = true; //位置を変更したフラグ

        //もし、リターンホーム中にすぐにシーン切り替えた場合用に、Live2D自体の位置もリセット。そして、すぐOriCompoMotionに遷移
        trans_motion = 11;
        live2d_animator.SetInteger("trans_motion", trans_motion);
        //live2d_animator.Play("OriCompoMotion", motion_layer_num, 0.0f);

        live2d_animator.SetInteger("trans_nade", 0);
        Anchor_Pos.transform.localPosition = new Vector3(-0.5f, 0.05f, -5f);
        

        girl1_status.face_girl_Normal();
        girl1_status.AddMotionAnimReset();
        girl1_status.IdleMotionReset();
        girl1_status.DoTSequence_Kill();
        
        girl1_status.Walk_Start = false;
    }

    //さらに、表示するときのコマンド
    void ReDrawLive2DPos_Compound()
    {
        cubism_rendercontroller.SortingOrder = 100;
             
    }

    //さらに調合位置に戻すコマンド　SetImage, NewRecipiButton.csから呼び出し
    public void ReSetLive2DPos_Compound()
    {
        character_move.transform.position = new Vector3(2.8f, 0, 0);
        live2d_posmove_flag = true; //位置を変更したフラグ   
    }

    void ResetLive2DPos_Face()
    {
        Debug.Log("Live2D位置のリセット");

        if (live2d_posmove_flag) //調合シーンに入った時に、位置を変更するので、変更したという合図
        {
            character_move.transform.position = new Vector3(0f, 0, 0);
            live2d_posmove_flag = false;
        }

        girl1_status.DefFaceChange();

    }

    public void WindowOff() //Girl1_Statusなどからもよむ
    {
        //mainUI_panel_obj.SetActive(false); //MainUI自体をオフにすると、材料選択や採取地でUIが消えてしまうので、あえて個別でON/OFFしている。

        compoundselect_onoff_obj.SetActive(false);       
        Extremepanel_obj.SetActive(false);
        text_area_Main.SetActive(false);
        girl_love_exp_bar.SetActive(false);
        manpuku_bar.SetActive(false);
        TimePanel_obj1.SetActive(false);
        moneystatus_panel.SetActive(false);
        mainUIFrame_panel.SetActive(false);
        Stagepanel_obj.SetActive(false);

        //MainUICloseButton.SetActive(false);
        //UIOpenButton_obj.SetActive(false);

        stageclear_panel.SetActive(false);        
        hinttaste_toggle.SetActive(false);
        //girleat_toggle.SetActive(false);
        recipi_toggle.SetActive(false);
        HintObjectGroup.SetActive(false);
    }

    public void WindowOn()
    {
        mainUI_panel_obj.SetActive(true);
        compoundselect_onoff_obj.SetActive(true);
        Extremepanel_obj.SetActive(true);
        text_area.SetActive(false);
        text_area_Main.SetActive(true); //テキストエリアメインは、MainUIPanel.csのほうも、trueとfalseを設定する。 
        girl_love_exp_bar.SetActive(true);        
        TimePanel_obj1.SetActive(true);
        TimePanel_obj2.SetActive(false);
        moneystatus_panel.SetActive(true);
        mainUIFrame_panel.SetActive(true);
        Stagepanel_obj.SetActive(true);

        if (GameMgr.Story_Mode == 1)
        {
            manpuku_bar.SetActive(true);
        }

        //MainUICloseButton.SetActive(true);
        //UIOpenButton_obj.SetActive(true);
        //girleat_toggle.SetActive(true);
        recipi_toggle.SetActive(true);
        HintObjectGroup.SetActive(true);

        //パネルを閉じる
        mainUI_panel_obj.GetComponent<MainUIPanel>().OnCloseButton(); //メニューは最初閉じ

        //ただし、ハートを取得しまだ残ってる場合は、ゲージはONのままにしておく。
        if (girlEat_judge.heart_count > 0)
        {
            StartCoroutine("HeartZeroWait");
            girl_love_exp_bar.SetActive(true);
        }
        else
        {
            //girl_love_exp_bar.SetActive(false);
        }

        //クエストをクリアしたら、クリアボタンがでる。
        QuestClearCheck();

        //ゲーム進行度に応じて、ヒントボタンなどは表示する。
        CheckButtonFlag();

        if (extreme_panel.extreme_itemID != 9999)
        {
            girleat_toggle.GetComponent<Toggle>().interactable = true;
            //girleat_toggle.SetActive(true);
        }
        else
        {
            girleat_toggle.GetComponent<Toggle>().interactable = false;
            //girleat_toggle.SetActive(false);
        }

        extreme_panel.extremeButtonInteractOn();
        extreme_panel.LifeAnimeOnTrue();
    }

    IEnumerator HeartZeroWait()
    {
        while (girlEat_judge.heart_count > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        if(GameMgr.MenuOpenFlag)
        {
            
        } else
        {
            //girl_love_exp_bar.SetActive(false);
        }
    }

    public void QuestClearCheck() //SaveControllerからも読み込んでいる。
    {
        stageclear_panel.SetActive(false);

        //5個クエストをクリアしたら、クリアボタンがでる。
        if (GameMgr.OkashiQuest_flag_stage1[4])
        {
            //stageclear_toggle.SetActive(true);
            stageclear_panel.SetActive(true);
            stageclear_Button.SetActive(true);
            stageclear_button_text.text = "コンテストへ";
        }
        else
        {
            if (stageclear_toggle.activeSelf == true || stageclear_panel.activeSelf)
            {
                //stageclear_toggle.SetActive(false);
                stageclear_panel.SetActive(false);
                //stageclear_Button.SetActive(false);
            }

            if (GameMgr.QuestClearflag)
            {
                stageclear_panel.SetActive(true);
                stageclear_Button.SetActive(true);
                stageclear_button_text.text = stageclear_default_text;
                //stageclear_toggle.SetActive(true);
            }
        }
    }


    public void OnCheck_1() //レシピ調合をON
    {
        if (recipi_toggle.GetComponent<Toggle>().isOn == true)
        {
            recipi_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            _text.text = recipi_text;
            GameMgr.compound_status = 60;
        }
    }

    public void OnCheck_1_button()
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = recipi_text;
        GameMgr.compound_status = 1;
    }

    public void OnCheck_2() //トッピング調合をON
    {
        if (extreme_toggle.GetComponent<Toggle>().isOn == true)
        {
            extreme_toggle.GetComponent<Toggle>().isOn = false;

            pitemlistController.extremepanel_on = false;
            card_view.DeleteCard_DrawView();

            _text.text = extreme_text;
            GameMgr.compound_status = 2;
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
                GameMgr.compound_status = 2;

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
            GameMgr.compound_status = 2;

        }
        
    }

    public void OnCheck_3() //オリジナル調合をON
    {
        if (original_toggle.GetComponent<Toggle>().isOn == true)
        {
            original_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            _text.text = originai_text;
            GameMgr.compound_status = 3;
        }
    }

    public void OnCheck_3_button() //調合選択画面からボタンを選択して、オリジナル調合をON
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = originai_text;
        GameMgr.compound_status = 3;
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
            GameMgr.compound_status = 5;
        }
    }

    public void OnCancel_Select()
    {
        GameMgr.compound_status = 0;
    }

    public void OnCancelCompound_Select() //調合画面から戻るとき
    {

        //カメラをメニューオープンの状態で戻す。メイン画面でカメラ位置を指定してたときの名残。
        GameMgr.compound_status = 0;

    }

    public void OnOKPicnic_Select()
    {
        //ピクニックイベント中、お菓子制作中は更新する。
        black_panel_A.SetActive(true);
        text_area.SetActive(true);
        yes_no_panel.SetActive(true);
        yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

        GameMgr.compound_select = 120;
        if (exp_Controller._temp_extremeSetting)
        {
            _text.text = "このお菓子でいく？";

            extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

            //一時的に腹減りを止める。
            //girl1_status.GirlEat_Judge_on = false;
            //girl1_status.Walk_Start = false;

            //WindowOff();

            Debug.Log("このお菓子でいく？");
            card_view.PresentGirl(extreme_panel.extreme_itemtype, extreme_panel.extreme_itemID);
            StartCoroutine("PicnicItem_Final_select");
        }
        else
        {
            //_text.text = "持っていくお菓子が決まってないよ～";
            _text.text = "今は、何のお菓子も選択されていないよ。" + "\n" + "やっぱりやめる？";
            StartCoroutine("PicnicItem_Cancel_Final_select");
        }
    }

    public void OnCancelPicnic_Select()
    {
        //ピクニックイベント中、お菓子制作中は更新する。
        black_panel_A.SetActive(true);
        text_area.SetActive(true);
        yes_no_panel.SetActive(true);
        yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);
        GameMgr.compound_select = 120;

        _text.text = "やっぱりやめる？";
        StartCoroutine("PicnicItem_Cancel_Final_select");
    }

    public void OnMenu_toggle() //メニューをON
    {
        if (menu_toggle.GetComponent<Toggle>().isOn == true)
        {          
            menu_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();
            HintButtonOFF();

            GameMgr.compound_status = 99;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
        }
    }

    public void OnMenu_button() //メニュー　ボタンで押した場合
    {
        card_view.DeleteCard_DrawView();

        GameMgr.compound_status = 99;

        StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
    }


    public void OnSystem_toggle() //システムボタンを押した
    {
        if (system_toggle.GetComponent<Toggle>().isOn == true)
        {
            system_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            GameMgr.compound_status = 200;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
        }
    }

    public void OnTasteHint_Toggle() //お菓子ヒントボタンを押した
    {
        if (hinttaste_toggle.GetComponent<Toggle>().isOn == true)
        {
            hinttaste_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            GameMgr.compound_status = 250;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
        }
    }

    public void OnStatus_toggle() //ステータスボタンを押した
    {
        if (status_toggle.GetComponent<Toggle>().isOn == true)
        {
            status_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            GameMgr.compound_status = 300;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
        }
    }


    public void OnShop_toggle() //ショップへ移動
    {
        if (shop_toggle.GetComponent<Toggle>().isOn == true)
        {
            shop_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            //連続で押せないようにする。
            OffCompoundSelect();

            FadeManager.Instance.LoadScene("Shop", 0.3f);
        }
    }

    public void OnBar_toggle() //ショップへ移動
    {
        if (bar_toggle.GetComponent<Toggle>().isOn == true)
        {
            bar_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            //連続で押せないようにする。
            OffCompoundSelect();

            FadeManager.Instance.LoadScene("Bar", 0.3f);
        }
    }


    public void OnGetMaterial_toggle() //材料採取地選択
    {
        if (getmaterial_toggle.GetComponent<Toggle>().isOn == true)
        {
            getmaterial_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();
            _text.text = "妹と一緒に材料を取りにいくよ！行き先を選んでね。";
            GameMgr.compound_status = 20;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

            //BGMを変更
            sceneBGM.OnGetMatStartBGM();
            map_ambience.Mute();
            bgm_change_flag = true;

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
                _text.text = "今、作ったお菓子をあげますか？"; // + "\n" + "あと " + GameMgr.ColorLemon + nokori_kaisu + "</color>" + "回　あげられるよ。"
                HintButtonOFF();
                GameMgr.compound_status = 10;

            }
            else //まだ作ってないときは
            {
                _textmain.text = "まだお菓子を作っていない。";
                //GameMgr.compound_status = 0;
            }
            

        }
    }

    public void OnSleep() //寝る or セーブ
    {
        if (sleep_toggle.GetComponent<Toggle>().isOn == true)
        {
            sleep_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            card_view.DeleteCard_DrawView();

            text_area.SetActive(true);
            _text.text = "今日はもう寝る？"; //
            HintButtonOFF();

            GameMgr.compound_status = 50;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

        }
    }

    public void OnStageClear() //ステージクリアボタン
    {
        if (stageclear_toggle.GetComponent<Toggle>().isOn == true || stageclear_button_toggle.isOn == true)
        {
            stageclear_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。
            stageclear_button_toggle.isOn = false;

            StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。

            card_view.DeleteCard_DrawView();

            if (GameMgr.OkashiQuest_flag_stage1[4])
            {
                if (extreme_panel.extreme_itemID == 9999)
                {
                    //お菓子を作ってないと、コンテストへ進めない。
                    _text.text = "お兄ちゃん..。まだお菓子を作ってないよ～。";
                    GameMgr.compound_status = 40;
                    yes_no_clear_panel.SetActive(true);
                    yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Button>().interactable = false;
                    yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Sound_Trigger>().enabled = false;
                }
                else
                {
                    _text.text = "コンテストに出るの？";
                    GameMgr.compound_status = 40;
                    yes_no_clear_panel.SetActive(true);
                    yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Button>().interactable = true;
                    yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Sound_Trigger>().enabled = true;
                }
            }
            else
            {
                if (GameMgr.QuestClearflag)
                {
                    _text.text = "次のお話にすすむ？　おにいちゃん。";

                    GameMgr.compound_status = 42;
                    yes_no_clear_okashi_panel.SetActive(true);

                }
            }

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
        text_area_Main.SetActive(false);
        black_panel_A.GetComponent<Image>().raycastTarget = false;
        //yes_no_panel.SetActive(false);

        //一時的に腹減りを止める。
        girl1_status.GirlEat_Judge_on = false;

        compoBGA_image.GetComponent<Image>().raycastTarget = false; //このときだけ、背景画像のタッチ判定をオフにする。そうしないと、宴がクリックに反応しなくなる。
        compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
        compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
        //Extremepanel_obj.SetActive(false);
        OffCompoundSelect();


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

        compoBGA_image.GetComponent<Image>().raycastTarget = true;
        compoBGA_imageOri.GetComponent<Image>().raycastTarget = true;
        compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = true;
        //Extremepanel_obj.SetActive(true);
        text_area.SetActive(false);
        text_area_Main.SetActive(true);
        black_panel_A.GetComponent<Image>().raycastTarget = true;
        OnCompoundSelect();
        //yes_no_panel.SetActive(true);
        GameMgr.compound_status = 60;
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

            //レシピチェック中の状態
            GameMgr.compound_select = 1100;
            GameMgr.compound_status = 1100;

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
                GameMgr.compound_status = 0; //ここまでで、チェックの処理が全て完了したので、status=0にする。

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
        text_area_Main.SetActive(false);
        UIOpenButton_obj.SetActive(false);

        GameMgr.scenario_ON = true;
        GameMgr.recipi_read_ID = pitemlist.eventitemlist[recipi_num].ev_ItemID;
        GameMgr.recipi_read_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                         //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        GameMgr.scenario_ON = false;
        Recipi_loading = false;

        /* レシピを読む処理 */
        pitemlist.eventitemlist[recipi_num].ev_ReadFlag = 1; //該当のイベントアイテムのレシピのフラグをONにしておく（読んだ、という意味）
        Recipi_FlagON_Method();
        Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji + "を読んだ");

        //レシピを読むと、知識がつきアイテム発見力が上がる。
        if (pitemlist.eventitemlist[recipi_num].event_itemName != "najya_start_recipi")
        {
            PlayerStatus.player_girl_findpower += 5;
        }
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
                GameMgr.compound_status = 11; //status=11で処理。

                yes_no_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;
                map_ambience.Mute();

                //時間の項目リセット
                time_controller.ResetTimeFlag();

                //お菓子をあげた回数をカウント
                PlayerStatus.player_girl_eatCount++;

                ClickPanel_1.SetActive(false);
                ClickPanel_2.SetActive(false);
                break;

            case false:

                //Debug.Log("cancel");

                //_textmain.text = "";
                StartMessage();
                GameMgr.compound_status = 0;

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

    IEnumerator PicnicItem_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);
        card_view.DeleteCard_DrawView();
        yes_no_panel.SetActive(false);
        GameMgr.compound_select = 6;

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                GameMgr.compound_select = 1000; //シナリオイベント読み中の状態にもどす。
                GameMgr.compound_status = 1000;

                //ピクニックアイテム決定
                compoBG_A.SetActive(false);
                
                yes_selectitem_kettei.onclick = false;                

                //時間の項目リセット
                time_controller.ResetTimeFlag();

                GameMgr.event_pitem_use_OK = true;

                //決定したアイテムの番号と個数
                GameMgr.event_kettei_itemID = extreme_panel.extreme_itemID;
                GameMgr.event_kettei_item_Type = extreme_panel.extreme_itemtype;
                //GameMgr.event_kettei_item_Kosu = updown_counter.updown_kosu; //最終個数を入れる。
                GameMgr.event_kettei_item_Kosu = 1;
                break;

            case false:

                //Debug.Log("cancel+ブラックパネルOFF");

                //GameMgr.compound_status = 6;

                yes_selectitem_kettei.onclick = false;

                break;

        }
    }

    //ピクニックキャンセルした
    IEnumerator PicnicItem_Cancel_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_no_panel.SetActive(false);
        black_panel_A.SetActive(false);
        GameMgr.compound_select = 6;

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                compoBG_A.SetActive(false);

                GameMgr.compound_select = 1000; //シナリオイベント読み中の状態にもどす。
                GameMgr.compound_status = 1000;

                //kettei_on_waiting = false;
                GameMgr.event_pitem_cancel = true; //やめたフラグON
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                break;

            case false:

                Debug.Log("cancel");

                //GameMgr.compound_status = 6;

                yes_selectitem_kettei.onclick = false;

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
                GameMgr.compound_status = 32; //status=32で処理。

                yes_no_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;
                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "";
                GameMgr.compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    IEnumerator OkashiClear_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //次お菓子へ進む処理

                yes_no_clear_okashi_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;

                //キャラクタ位置を0にもどす。
                girl1_status.ResetCharacterPosition();

                girlEat_judge.QuestClearMethod();

                break;

            case false:

                yes_no_clear_okashi_panel.SetActive(false);

                //_textmain.text = "";
                StartMessage();
                GameMgr.compound_status = 0;

                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    IEnumerator Contest_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //コンテストへ進む処理

                yes_no_clear_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;

                //キャラクタ位置を0にもどす。
                girl1_status.ResetCharacterPosition();

                switch (GameMgr.stage_number)
                {
                    case 1:

                        GameMgr.stage1_girl1_loveexp = PlayerStatus.girl1_Love_exp; //クリア時の好感度を保存
                        FadeManager.Instance.LoadScene("Contest", 0.3f);
                        break;

                    case 2:

                        GameMgr.stage2_girl1_loveexp = PlayerStatus.girl1_Love_exp; //クリア時の好感度を保存
                        FadeManager.Instance.LoadScene("003_Stage3_eyecatch", 0.3f);
                        break;

                    case 3:

                        GameMgr.stage3_girl1_loveexp = PlayerStatus.girl1_Love_exp; //クリア時の好感度を保存
                        FadeManager.Instance.LoadScene("100_Ending", 0.3f);
                        break;

                }
                

                break;

            case false:

                yes_no_clear_panel.SetActive(false);

                //_textmain.text = "";
                StartMessage();
                GameMgr.compound_status = 0;

                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    IEnumerator Sleep_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //寝る処理

                yes_no_sleep_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;

                time_controller.OnSleepMethod();

                break;

            case false:

                yes_no_sleep_panel.SetActive(false);

                //_textmain.text = "";
                StartMessage();
                GameMgr.compound_status = 0;

                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    //好感度によって発生する、メインイベント。基本、クエストクリアボタンを押さないと発動しない。
    public void Check_GirlLoveEvent()
    {
        if (GirlLove_loading == true)
        {

        }
        else
        {
            GameMgr.girlloveevent_bunki = 0; //サブイベントが発生しない限り、メインの好感度イベントを発生するようにする。

            switch (GameMgr.stage_number)
            {
                //ステージ１のメインイベント
                case 1:
                  
                    if (!GameMgr.OkashiQuest_flag_stage1[0]) //レベル１のときのイベント。一番最初で起こるイベント。
                    {
                        event_num = 0;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント０
                        {
                            GameMgr.GirlLoveEvent_num = 0;
                            GameMgr.GirlLoveEvent_stage1[event_num] = true;　//0番がtrueになってたら、現在は、ステージ１－１のクエストが発生中という意味。

                            //クッキー作りのクエスト発生
                            Debug.Log("好感度イベント１をON: クッキーが食べたい　開始");

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            special_quest.SetSpecialOkashi(0, 0);
                        }
                    }

                    if (GameMgr.OkashiQuest_flag_stage1[0] && GameMgr.questclear_After) //レベル２のときのイベント
                    {

                        event_num = 10;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント１
                        {
                            GameMgr.GirlLoveEvent_num = 10;
                            GameMgr.GirlLoveEvent_stage1[event_num] = true;　//1番がtrueになってたら、現在は、ステージ１－２のクエストが発生中という意味。

                            GameMgr.questclear_After = false;

                            //レシピの追加
                            pitemlist.add_eventPlayerItemString("rusk_recipi", 1);//ラスクのレシピを追加                            

                            //クエスト発生
                            Debug.Log("好感度イベント２をON: ラスクが食べたい　開始");

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            special_quest.SetSpecialOkashi(10, 0);

                            //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                            StartCoroutine("ReadGirlLoveEvent");
                        }
                    }

                    if (GameMgr.OkashiQuest_flag_stage1[1] && GameMgr.questclear_After) //レベル３のときのイベント。
                    {
                        event_num = 20;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント２
                        {
                            GameMgr.GirlLoveEvent_num = 20;
                            GameMgr.GirlLoveEvent_stage1[event_num] = true;

                            GameMgr.questclear_After = false;

                            //レシピの追加
                            pitemlist.add_eventPlayerItemString("crepe_recipi", 1); //クレープのレシピを追加                                

                            //クエスト発生
                            Debug.Log("好感度イベント３をON: クレープが食べたい　開始");

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            special_quest.SetSpecialOkashi(20, 0);

                            //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                            StartCoroutine("ReadGirlLoveEvent");
                        }
                    }

                    if (GameMgr.OkashiQuest_flag_stage1[2] && GameMgr.questclear_After) //レベル４のときのイベント。
                    {
                        event_num = 30;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント３
                        {
                            GameMgr.GirlLoveEvent_num = 30;
                            GameMgr.GirlLoveEvent_stage1[event_num] = true;

                            GameMgr.questclear_After = false;

                            //クエスト発生
                            Debug.Log("好感度イベント４をON: シュークリームが食べたい　開始");

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            special_quest.SetSpecialOkashi(30, 0);

                            //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                            StartCoroutine("ReadGirlLoveEvent");
                        }
                    }

                    if (GameMgr.OkashiQuest_flag_stage1[3] && GameMgr.questclear_After) //レベル５のときのイベント。
                    {
                        event_num = 40;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　好感度イベント４
                        {
                            GameMgr.GirlLoveEvent_num = 40;
                            GameMgr.GirlLoveEvent_stage1[event_num] = true;

                            GameMgr.questclear_After = false;

                            //クエスト発生
                            Debug.Log("好感度イベント５をON: ドーナツが食べたい　開始");

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            special_quest.SetSpecialOkashi(40, 0);

                            //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                            StartCoroutine("ReadGirlLoveEvent");
                        }
                    }

                    if (GameMgr.OkashiQuest_flag_stage1[4] && GameMgr.questclear_After) //ステージ１　５つクリアしたので、コンテストイベント
                    {
                        event_num = 50;

                        if (GameMgr.GirlLoveEvent_stage1[event_num] != true) //ステージ１　ラストイベント
                        {
                            GameMgr.GirlLoveEvent_num = 50;
                            GameMgr.GirlLoveEvent_stage1[event_num] = true;

                            GameMgr.questclear_After = false;

                            //コンテストの締め切り日を設定
                            GameMgr.stage1_limit_day = PlayerStatus.player_day + 7;
                            time_controller.DeadLine_Setting();

                            //クエスト発生
                            Debug.Log("ステージ１ラストイベントをON: コンテスト　開始");

                            //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                            special_quest.SetSpecialOkashi(50, 0);

                            //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                            StartCoroutine("ReadGirlLoveEvent");

                            //イベントCG解禁
                            GameMgr.SetEventCollectionFlag("event10", true);

                            //広場は必ずでる。
                            matplace_database.matPlaceKaikin("Hiroba"); //広場解禁
                            //matplace_database.matPlaceKaikin("HimawariHill"); //ひまわり解禁
                        }
                    }

                    break;

                //ステージ２のイベント
                case 2:

                    break;

                //ステージ３のイベント
                case 3:

                    break;

                default:
                    break;

            }

            if(!GirlLove_loading)
            {
                check_GirlLoveEvent_flag = true;
                check_GirlLoveSubEvent_flag = false; //好感度イベントのチェック後に、サブイベントの発生チェック
            }
            
        }
    }

    //好感度イベント発生用の閾値を計算
    int CheckLoveExp(int kaisu)
    {
        _checkexp = 0;

        for (i=0; i < kaisu; i++)
        {
            _checkexp += girl1_status.stage1_lvTable[i];
        }
        return _checkexp;
    }

    IEnumerator ReadGirlLoveEvent()
    {
        OffCompoundSelect();
        compoundselect_onoff_obj.SetActive(false);
        Extremepanel_obj.SetActive(false);
        compoBG_A.SetActive(false);
        girl_love_exp_bar.SetActive(true);
        mainUIFrame_panel.SetActive(true);
        GirlLove_loading = true;

        //腹減りカウント一時停止
        girl1_status.GirlEat_Judge_on = false;
        //girl1_status.Girl_Full();
        
        girl1_status.Girl1_Status_Init();

        GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
        GameMgr.compound_status = 1000;

        while (girlEat_ON)
        {
            yield return null;
        }

        while (girlEat_judge.heart_count > 0)
        {
            yield return null;
        }

        //吹き出しオフはこのタイミングで。
        girl1_status.hukidasiOff();

        mainUI_panel_obj.SetActive(false);
        GirlHeartEffect_obj.SetActive(false);       

        if (mute_on)
        {
            sceneBGM.MuteBGM();
            map_ambience.Mute();
        }

        //サブイベントを読み始めたら、元のキャラクタの位置は元に戻しておく。
        ResetLive2DPos_Face();

        GameMgr.scenario_ON = true;
        GameMgr.girlloveevent_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.girlloveevent_endflag)
        {
            yield return null;
        }

        GameMgr.girlloveevent_endflag = false;
        GameMgr.scenario_ON = false;

        sceneBGM.MuteOFFBGM();
        //メインBGMを変更　ハートレベルに応じてBGMも切り替わる。
        bgm_change_story();

        map_ambience.MuteOFF();
        mute_on = false;

        mainUI_panel_obj.SetActive(true);
        //OnCompoundSelect();
        Extremepanel_obj.SetActive(true);
        GirlHeartEffect_obj.SetActive(true);
        girlEat_judge.EffectClear();

        //イベント後に、ハートを獲得するなどの演出がある場合
        if (SubEvAfterHeartGet)
        {
            SubEvAfterHeartGet = false;

            switch(SubEvAfterHeartGet_num)
            {
                case 60:

                    _textmain.text = "ぽんぽんの力で、より元気になってきた。";
                    //girlEat_judge.loveGetPlusAnimeON(30, false);    
                    GirlExpressionKoushin(50);

                    break;

                case 61:

                    //入店の音
                    sc.PlaySe(38); //ドア
                    sc.PlaySe(50); //ベル

                    switch (GameMgr.event_judge_status)
                    {
                        case 0:

                            _textmain.text = "あまりピクニックは喜ばなかったようだ..。";
                            girlEat_judge.DegHeart((int)(SujiMap(GameMgr.event_okashi_score, 0, 30, 60, 0)), true);
                            GirlExpressionKoushin(-20);
                            break;

                        case 1:

                            get_heart = GameMgr.event_okashi_score; //GameMgr.event_okashi_score / 5
                            _textmain.text = "ピクニックを喜んだようだ。" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";
                            girlEat_judge.loveGetPlusAnimeON(get_heart, false); //trueにしておくと、ハートゲット後に、クエストクリアをチェック
                            GirlExpressionKoushin(10);
                            break;

                        case 2:

                            get_heart = GameMgr.event_okashi_score;
                            _textmain.text = "ピクニックをとても喜んだようだ！" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";
                            girlEat_judge.loveGetPlusAnimeON(get_heart, false);
                            GirlExpressionKoushin(30);
                            GameMgr.picnic_after = true;
                            GameMgr.picnic_after_time = 60;
                            break;

                        case 3:

                            get_heart = GameMgr.event_okashi_score;
                            _textmain.text = "ピクニックが最高だったようだ！" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";
                            girlEat_judge.loveGetPlusAnimeON(get_heart, false);
                            GirlExpressionKoushin(50);
                            GameMgr.picnic_after = true;
                            GameMgr.picnic_after_time = 60;
                            break;

                        case 4:

                            get_heart = GameMgr.event_okashi_score;
                            _textmain.text = "思い出に残るピクニックだった！" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";
                            girlEat_judge.loveGetPlusAnimeON(GameMgr.event_okashi_score, false);
                            GirlExpressionKoushin(100);
                            GameMgr.picnic_after = true;
                            GameMgr.picnic_after_time = 60;
                            break;
                    }
                                       

                    break;

                    /*
                case 70:

                    _textmain.text = "メガネによろこんだ！";
                    girlEat_judge.loveGetPlusAnimeON(10, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 71:

                    _textmain.text = "スク水をよろこんだようだ！";
                    girlEat_judge.loveGetPlusAnimeON(30, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 72:

                    _textmain.text = "黒メイド服をよろこんだようだ！";
                    girlEat_judge.loveGetPlusAnimeON(30, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 73:

                    _textmain.text = "天使のワンピースをよろこんだようだ！";
                    girlEat_judge.loveGetPlusAnimeON(70, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 74:

                    _textmain.text = "深紅のハートドレスをよろこんだようだ！";
                    girlEat_judge.loveGetPlusAnimeON(70, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 75:

                    _textmain.text = "バルーンハットをたいそうよろこんだようだ！";
                    girlEat_judge.loveGetPlusAnimeON(50, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 76:

                    _textmain.text = "天使の羽根にこころを浄化された！";
                    girlEat_judge.loveGetPlusAnimeON(70, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 77:

                    _textmain.text = "ねこみみに興味をひいたようだ！";
                    girlEat_judge.loveGetPlusAnimeON(30, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 78:

                    _textmain.text = "お花のヘアピンを気に入ったようだ！";
                    girlEat_judge.loveGetPlusAnimeON(10, false);
                    GameMgr.girl_express_param += 50;
                    break;

                case 79:

                    _textmain.text = "ティンクルスターダストを気に入ったようだ！";
                    girlEat_judge.loveGetPlusAnimeON(100, false);
                    GameMgr.girl_express_param += 50;
                    break;*/

                case 100:

                    _textmain.text = "ぬいぐるみに喜んだようだ！";
                    girlEat_judge.loveGetPlusAnimeON(30, false);
                    GirlExpressionKoushin(50);
                    break;
            }

            StartCoroutine("ReadGirlLoveEventAfter");
            read_girlevent = false;
            subevent_after_end = true; //サブイベントアフター演出を読み中            
        }
        else
        {
            //_textmain.text = "";
            StartMessage(); //テキスト更新

            mainUI_panel_obj.GetComponent<MainUIPanel>().OnCloseButton(); //メニューは最初閉じ
            read_girlevent = true; //ONなら、メニューを閉じた状態からスタート。

            check_GirlLoveEvent_flag = true;
            girl1_status.Girl1_Status_Init2();

        }       

        //腹減りカウント開始
        girl1_status.GirlEat_Judge_on = true;
        girl1_status.WaitHint_on = true;
        
        GirlLove_loading = false;
       
        check_recipi_flag = false;
       
    }   

    IEnumerator ReadGirlLoveEventAfter()
    {
        while (girlEat_judge.heart_count > 0)
        {
            yield return null;
          
        }

        check_GirlLoveEvent_flag = true;
        girl1_status.Girl1_Status_Init2();
        subevent_after_end = false;
    }

    //レシピの番号チェック。コンポ調合アイテムを解禁し、レシピリストに表示されるようにする。
    void Recipi_FlagON_Method()
    {
        
        switch (pitemlist.eventitemlist[recipi_num].event_itemName)
        {
            case "ev02_orangeneko_cookie_memo": //オレンジネコクッキー閃きのメモ

                //オレンジジャムの作り方を解禁
                databaseCompo.CompoON_compoitemdatabase("orange_jam");

                break;

            case "najya_start_recipi": //ナジャのお菓子作りの基本                

                //databaseCompo.CompoON_compoitemdatabase("neko_cookie");
                //databaseCompo.CompoON_compoitemdatabase("appaleil");

                break;

            case "financier_recipi": //フィナンシェ

                databaseCompo.CompoON_compoitemdatabase("kogashi_butter");

                break;

            case "bisucouti_recipi": //ビスコッティ

                //databaseCompo.CompoON_compoitemdatabase("baking_mix");
                //databaseCompo.CompoON_compoitemdatabase("biscotti");

                break;

            case "recipibook_4": //アイスの実の森　ゲットすると、アイスクリームレシピも自動で追加される。

                ev_id = pitemlist.Find_eventitemdatabase("ice_cream_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ナジャの基本のレシピを追加

                break;

            case "recipibook_6": //お茶のすすめ

                //いける場所を追加
                matplace_database.matPlaceKaikin("Lavender_field"); //アメジストの湖畔解禁
                break;

            default:
                break;
        }
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

        }
    }

    void DefaultStartPitem()
    {
        //ゲーム「はじめから」で始まった場合の、最初の一回だけする処理       
        if (GameMgr.gamestart_recipi_get != true)
        {

            extreme_panel.deleteExtreme_Item();

            GameMgr.gamestart_recipi_get = true; //フラグをONに。  

            //ゲームの一番最初に絶対手に入れるレシピ
            ev_id = pitemlist.Find_eventitemdatabase("najya_start_recipi");
            pitemlist.add_eventPlayerItem(ev_id, 1); //ナジャの基本のレシピを追加

            ev_id = pitemlist.Find_eventitemdatabase("ev01_neko_cookie_recipi");
            pitemlist.add_eventPlayerItem(ev_id, 1); //クッキーのレシピを追加

            //すでにレシピ100%フラグなど達成してた場合は、引き継がれる要素
            if (GameMgr.GirlLoveSubEvent_stage1[101])
            {
                ev_id = pitemlist.Find_eventitemdatabase("silver_neko_cookie_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //銀のねこクッキーのレシピ
                pitemlist.EventReadOn("silver_neko_cookie_recipi");
            }
            if (GameMgr.GirlLoveSubEvent_stage1[102])
            {
                ev_id = pitemlist.Find_eventitemdatabase("gold_neko_cookie_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //金のねこクッキーのレシピ
                pitemlist.EventReadOn("gold_neko_cookie_recipi");
            }

            //Debug.Log("プレイヤーステータス　アイテム初期化　実行");
            //初期に所持するアイテム

            pitemlist.addPlayerItemString("komugiko", 10);
            pitemlist.addPlayerItemString("butter", 5);
            pitemlist.addPlayerItemString("suger", 5);
            pitemlist.addPlayerItemString("orange", 3);
            //pitemlist.addPlayerItemString("grape", 2);
            //pitemlist.addPlayerItemString("stone_oven", 1);

        }
    }

    //外へ出る、などのコマンドを増やす系のイベント
    void FlagEvent()
    {
        if (GameMgr.KeyInputOff_flag)
        {

        }
    }

    //メッセージを更新・表示する. QuestTitlePanel.csからも読んでいる。クエストタイトルパネルを閉じたタイミングで更新。
    public void StartMessage()
    {
        if (!GameMgr.MesaggeKoushinON)
        {
            _textmain.text = "";
        }
        else
        {
            _textmain.text = "どうしようかなぁ？"; //デフォルトメッセージ

            switch (GameMgr.OkashiQuest_Num)
            {
                case 0: //クッキー

                    if (!PlayerStatus.First_recipi_on) //最初お菓子をつくってないときは、これがでる。
                    {
                        _textmain.text = GameMgr.ColorLemon + "左の「おかしパネル」" + "</color>" + "から、" + "\n" + "クッキーを作ってみようね！　にいちゃん！";
                    }
                    else
                    {
                        if (!GameMgr.Beginner_flag[0]) //クッキーをまだあげていない
                        {
                            _textmain.text = GameMgr.ColorLemon + "「あげる」" + "</color>" + "ボタンを押して、クッキーをちょうだい！";
                        }
                        else
                        {
                            _textmain.text = "どうしようかなぁ？";
                        }
                    }
                    break;

                case 1: //ぶどうクッキー

                    if (GameMgr.Story_Mode == 0)
                    {
                        if (!GameMgr.MapEvent_01[0]) //まだ森にいったことがない場合
                        {
                            _textmain.text = "どうしようかなぁ？" + "\n" + "（むらさきのくだものは、「近くの森」で採れたっけ。）";
                        }
                        else
                        {

                        }
                    }
                    break;

                case 10: //ラスクのとき

                    if (!GameMgr.Beginner_flag[1]) //ラスクのレシピをまだ読んだことが無い
                    {
                        _textmain.text = "ラスクのレシピを読もう！　にいちゃん！";
                    }
                    break;
            }
        }

        //メイン画面に表示する、現在のクエスト
        questname.text = girl1_status.OkashiQuest_Name; //現在のクエストネーム更新
    }

    //SPお菓子とは別で、パティシエレベルor好感度が一定に達すると発生するサブイベント
    void GirlLove_EventMethod()
    {
        GameMgr.girlloveevent_bunki = 1; //サブイベントの発生のチェック。宴用に分岐。

        if (GirlLove_loading)
        {

        }
        else
        {
            check_GirlLoveSubEvent_flag = true;           

            //クエストで発生するサブイベント
            switch (GameMgr.OkashiQuest_Num)
            {
                case 0: //クッキー

                    //はじめてのお菓子。食べた直後に発生する。
                    if (GameMgr.GirlLoveSubEvent_stage1[0] == false)
                    {
                        if (check_OkashiAfter_flag) //お菓子をあげたあとのフラグ
                        {

                            GameMgr.GirlLoveSubEvent_stage1[0] = true;

                            if (GameMgr.Okashi_dislike_status == 2) //そもそもクッキー以外のものをあげたとき
                            {
                                if (GameMgr.Okashi_totalscore < GameMgr.low_score) //クリアできないときのヒントをだす。＋クッキーを食べたいなぁ～。
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[3] = true;
                                    GameMgr.GirlLoveSubEvent_num = 3;
                                    GameMgr.Okashi_OnepointHint_num = 0;

                                    check_GirlLoveSubEvent_flag = false;
                                }
                                else //クリアできたら、そのままOK!　＋　でもクッキーが食べたいから、にいちゃん、クッキーを作って！！
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[4] = true;
                                    GameMgr.GirlLoveSubEvent_num = 4;

                                    check_GirlLoveSubEvent_flag = false;
                                }
                            }
                            else
                            {
                                if (GameMgr.Okashi_totalscore < GameMgr.low_score) //クリアできなかった場合、ヒントをだす。
                                {
                                    GameMgr.GirlLoveSubEvent_num = 0;
                                    GameMgr.Okashi_OnepointHint_num = 0;

                                    check_GirlLoveSubEvent_flag = false;
                                }
                                else if (GameMgr.Okashi_totalscore < GameMgr.high_score)//クリアできた。60~85。現在未使用。
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[1] = true;
                                    GameMgr.GirlLoveSubEvent_num = 1;

                                    check_GirlLoveSubEvent_flag = true; //trueにすると、そのイベントを無視できる。
                                }
                                else //クリアできた。85~
                                {
                                    GameMgr.GirlLoveSubEvent_stage1[2] = true;
                                    GameMgr.GirlLoveSubEvent_num = 2;

                                    check_GirlLoveSubEvent_flag = true;
                                }
                            }                            
                        }
                    }

                    //一度お菓子を作って失敗し、次に作って成功した。または、クッキー以外のお菓子を作り、その後、クッキーを作って成功した。
                    if (GameMgr.GirlLoveSubEvent_stage1[0] == true && GameMgr.GirlLoveSubEvent_stage1[1] == false && GameMgr.GirlLoveSubEvent_stage1[2] == false)
                    {
                        if (check_OkashiAfter_flag)
                        {
                            if (!GameMgr.GirlLoveSubEvent_stage1[5] || !GameMgr.GirlLoveSubEvent_stage1[6])
                            {
                                if (GameMgr.Okashi_dislike_status == 2) //そもそもクッキー以外のものをあげたとき
                                {

                                }
                                else
                                {
                                    if (GameMgr.Okashi_totalscore < GameMgr.low_score) //クリアできなかった場合。フラグはたたず、やり直し
                                    {

                                    }
                                    else if (GameMgr.Okashi_totalscore < GameMgr.high_score)//クリアできた。60~85
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[5] = true;
                                        GameMgr.GirlLoveSubEvent_num = 5;
                                        GameMgr.Okashi_OnepointHint_num = 9999;

                                        check_GirlLoveSubEvent_flag = false;
                                    }
                                    else //クリアできた。85~
                                    {
                                        GameMgr.GirlLoveSubEvent_stage1[6] = true;
                                        GameMgr.GirlLoveSubEvent_num = 6;
                                        GameMgr.Okashi_OnepointHint_num = 9999;

                                        check_GirlLoveSubEvent_flag = false;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case 1: //ぶどうクッキー

                    if (GameMgr.GirlLoveSubEvent_stage1[7] == false) //はじめてぶどうをとってきた
                    {
                        if (check_GetMat_flag)
                        {
                            if (pitemlist.KosuCount("grape") >= 1)
                            {
                                GameMgr.GirlLoveSubEvent_stage1[7] = true;
                                GameMgr.GirlLoveSubEvent_num = 7;

                                check_GirlLoveSubEvent_flag = false;
                            }
                        }
                    }
                    break;

                case 2: //かわいいクッキー

                    if (GameMgr.GirlLoveSubEvent_stage1[8] == false && girl1_status.special_animatFirst == true)
                    {
                        GameMgr.GirlLoveSubEvent_stage1[8] = true;
                        GameMgr.GirlLoveSubEvent_num = 8;
                        check_GirlLoveSubEvent_flag = false;

                        mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                    }
                    break;

                case 11: //ラスク2

                    if (girl1_status.special_animatFirst) //ステージ2-2 はじまってから、ベリーファーム開始
                    {
                        if (!GameMgr.GirlLoveSubEvent_stage1[10])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[10] = true;
                            GameMgr.GirlLoveSubEvent_num = 10;

                            check_GirlLoveSubEvent_flag = false;
                            mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。

                            //ベリーファームへ行けるようになる。
                            matplace_database.matPlaceKaikin("BerryFarm"); //ベリーファーム解禁

                        }
                    }

                    break;

                case 13: //キラキララスク 10から分岐１

                    if (girl1_status.special_animatFirst) //ステージ2-2 はじまってから、ベリーファーム開始
                    {
                        if (!GameMgr.GirlLoveSubEvent_stage1[10])
                        {
                            GameMgr.GirlLoveSubEvent_stage1[10] = true;
                            GameMgr.GirlLoveSubEvent_num = 10;

                            check_GirlLoveSubEvent_flag = false;
                            mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。

                            //ベリーファームへ行けるようになる。
                            matplace_database.matPlaceKaikin("BerryFarm"); //ベリーファーム解禁

                        }
                    }

                    break;

                case 20: //クレープ1

                    if (check_CompoAfter_flag) //お菓子を作ったあとのフラグ. Exp_Controllerから読み出し。
                    {
                        if (GameMgr.GirlLoveSubEvent_stage1[20] == false && database.items[GameMgr.Okashi_makeID].itemType_sub.ToString() == "Crepe")
                        {
                            GameMgr.GirlLoveSubEvent_stage1[20] = true;
                            GameMgr.GirlLoveSubEvent_num = 20;
                            check_GirlLoveSubEvent_flag = false;

                            mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                        }
                    }

                    if (GameMgr.GirlLoveSubEvent_stage1[21] == false)
                    {
                        GameMgr.GirlLoveSubEvent_stage1[21] = true;
                        GameMgr.GirlLoveSubEvent_num = 21;
                        check_GirlLoveSubEvent_flag = false;

                        mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                    }
                    break;

                case 40: //ドーナツ　ひまわりのたね

                    if (GameMgr.GirlLoveSubEvent_stage1[40] == false) //ひまわりのたねをとってきた
                    {
                        if (check_GetMat_flag)
                        {
                            if (pitemlist.KosuCount("himawari_seed") >= 1)
                            {
                                GameMgr.GirlLoveSubEvent_stage1[40] = true;
                                GameMgr.GirlLoveSubEvent_num = 40;

                                check_GirlLoveSubEvent_flag = false;
                            }
                        }
                    }

                    //ひまわり油
                    if (check_CompoAfter_flag) //お菓子を作ったあとのフラグ. Exp_Controllerから読み出し。
                    {
                        if (GameMgr.GirlLoveSubEvent_stage1[41] == false && database.items[GameMgr.Okashi_makeID].itemName == "himawari_Oil")
                        {
                            {
                                GameMgr.GirlLoveSubEvent_stage1[41] = true;
                                GameMgr.GirlLoveSubEvent_num = 41;
                                check_GirlLoveSubEvent_flag = false;

                                mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                            }
                        }
                    }

                    if (check_CompoAfter_flag) //ドーナツをはじめて作った。
                    {
                        if (GameMgr.GirlLoveSubEvent_stage1[42] == false && database.items[GameMgr.Okashi_makeID].itemType_sub.ToString() == "Donuts")
                        {
                            GameMgr.GirlLoveSubEvent_stage1[42] = true;
                            GameMgr.GirlLoveSubEvent_num = 42;
                            check_GirlLoveSubEvent_flag = false;

                            mute_on = true; //ゲームの音をOFFにし、宴のBGMを鳴らす。
                        }
                    }
                    break;

            }
            

            //
            //メインクエストに関係しないサブイベント関係は、60番台～
            //

            //キラキラポンポン 発生すると、さらに親睦を深めて、BGMが変わる。
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (PlayerStatus.girl1_Love_lv >= 15 && GameMgr.GirlLoveSubEvent_stage1[60] == false) //4になったときのサブイベントを使う。
                {
                    GameMgr.GirlLoveSubEvent_num = 60;
                    GameMgr.GirlLoveSubEvent_stage1[60] = true;

                    check_GirlLoveSubEvent_flag = false;

                    mute_on = true;

                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                    SubEvAfterHeartGet_num = 60;

                    //イベントCG解禁
                    GameMgr.SetEventCollectionFlag("event1", true);
                    GameMgr.SetEventCollectionFlag("event2", true);
                }
            }

            //ピクニック
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {

                //クレープ以降　一回目は必ず発生               
                if (PlayerStatus.player_cullent_hour >= 12 && PlayerStatus.player_cullent_hour <= 14
                    && GameMgr.GirlLoveEvent_num >= 20) //12時から15時の間に、サイコロふる
                {

                    if (GameMgr.picnic_End && GameMgr.picnic_count <= 0)
                    {

                        GameMgr.picnic_End = false;
                        GameMgr.picnic_event_ON = true;
                    }

                    if (GameMgr.picnic_event_ON)
                    {
                        random = Random.Range(0, 100);
                        Debug.Log("ピクニックイベント　抽選スタート　60以下で成功: " + random);

                        if (GameMgr.GirlLoveSubEvent_stage1[61])
                        {
                            picnic_exprob = 60; //60%の確率で発生。
                        }
                        else
                        {
                            picnic_exprob = 100; //初回は100%
                        }

                        if (random <= picnic_exprob)
                        {
                            GameMgr.GirlLoveSubEvent_num = 61;
                            GameMgr.GirlLoveSubEvent_stage1[61] = true; //イベント初発生の分をフラグっておく。
                            GameMgr.picnic_End = true;//さらにカウンターを置く。カウンターが０になったら、またランダムで発生するようになる。
                            GameMgr.picnic_event_ON = false;
                            GameMgr.picnic_event_reading_now = true;
                            GameMgr.picnic_count = 5; //次のピクニックイベントまでの日数カウンタ

                            check_GirlLoveSubEvent_flag = false;

                            mute_on = true;
                            GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。

                            SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                            SubEvAfterHeartGet_num = 61;
                        }
                    }
                }
            }            

            //
            //ビギナー系のサブイベント関係は、80番台～
            //

            //はじめてお菓子を作ったら発生
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            {}
            else
            {
                if (PlayerStatus.First_recipi_on)
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[80] == false)
                    {
                        GameMgr.GirlLoveSubEvent_stage1[80] = true;
                        GameMgr.GirlLoveSubEvent_num = 80;
                        check_GirlLoveSubEvent_flag = false;
                    }
                }
            }

            //はじめてコレクションアイテムを手に入れたら発生
            /*if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            {}
            else
            {
                if (!GameMgr.Beginner_flag[2]) //はじめてコレクションアイテム手に入れた
                {
                    //所持数チェック
                    GetFirstCollectionItem = false;
                    for (i=0; i< GameMgr.CollectionItemsName.Count; i++)
                    {
                        if(pitemlist.KosuCount(GameMgr.CollectionItemsName[i]) >= 1)
                        {
                            GetFirstCollectionItem = true;
                        }
                    }

                    if (GetFirstCollectionItem)
                    {
                        GameMgr.Beginner_flag[2] = true;
                        GameMgr.GirlLoveSubEvent_stage1[81] = true;
                        GameMgr.GirlLoveSubEvent_num = 81;
                        check_GirlLoveSubEvent_flag = false;
                    }
                }
            }*/

            //はじめて体力が0
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (!GameMgr.Beginner_flag[4]) 
                {

                    if (PlayerStatus.player_girl_lifepoint <= 0)
                    {
                        GameMgr.Beginner_flag[4] = true;
                        GameMgr.GirlLoveSubEvent_stage1[82] = true;
                        GameMgr.GirlLoveSubEvent_num = 82;

                        mute_on = true;
                        check_GirlLoveSubEvent_flag = false;
                    }
                }
            }

            //はじめてお金が半分を下回った
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //酒場でていなければイベント発生
                if (matplace_database.matplace_lists[matplace_database.SearchMapString("Bar")].placeFlag == 1)
                {

                }
                else
                {
                    if (!GameMgr.Beginner_flag[5])
                    {

                        if (PlayerStatus.player_money <= 1000)
                        {
                            GameMgr.Beginner_flag[5] = true;
                            GameMgr.GirlLoveSubEvent_stage1[83] = true;
                            GameMgr.GirlLoveSubEvent_num = 83;

                            mute_on = true;
                            check_GirlLoveSubEvent_flag = false;
                        }
                    }
                }
            }

            //はじめてエメラルどんぐりをとったら発生　衣装交換アイテムの説明がある。
            /*if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (pitemlist.KosuCount("emeralDongri") >= 1 || pitemlist.KosuCount("sapphireDongri") >= 1)
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[84] == false)
                    {
                        GameMgr.GirlLoveSubEvent_stage1[84] = true;
                        GameMgr.GirlLoveSubEvent_num = 84;

                        mute_on = true;
                        check_GirlLoveSubEvent_flag = false;
                    }
                }
            }*/

            //はじめて水っぽいなどのマイナス効果がつくお菓子を作った
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {

                if (!GameMgr.Beginner_flag[6])
                {
                    if (exp_Controller._temp_extremeSetting)
                    {
                        if (pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].Oily > GameMgr.Watery_Line ||
                            pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].Powdery > GameMgr.Watery_Line)
                        {
                            GameMgr.Beginner_flag[6] = true;
                            GameMgr.GirlLoveSubEvent_stage1[85] = true;
                            GameMgr.GirlLoveSubEvent_num = 85;

                            mute_on = true;
                            check_GirlLoveSubEvent_flag = false;                                                    
                        }
                        else
                        {
                            if (pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].itemType_sub.ToString() == "Juice" ||
                                pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].itemType_sub.ToString() == "Tea" ||
                                pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].itemType_sub.ToString() == "Tea_Potion" ||
                                pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].itemType_sub.ToString() == "Coffee_Mat")
                            { }
                            else
                            {
                                if (pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].Watery > GameMgr.Watery_Line)
                                {
                                    GameMgr.Beginner_flag[6] = true;
                                    GameMgr.GirlLoveSubEvent_stage1[85] = true;
                                    GameMgr.GirlLoveSubEvent_num = 85;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                }
                            }
                        }
                    }
                }

            }

            //はじめて衣装装備を買った 70番台～　周回しても、フラグは引継ぎ。二度目以上の発生はない。
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                //所持数チェック
                GetEmeraldItem = false;
                i = 0;
                while(i < pitemlist.emeralditemlist.Count)
                {
                    if (pitemlist.KosuCountEmerald(pitemlist.emeralditemlist[i].event_itemName) >= 1)
                    {
                        GetEmeraldItemName = pitemlist.emeralditemlist[i].event_itemName;
                       
                        switch (GetEmeraldItemName)
                        {
                            case "Glass_Acce":

                                if (!GameMgr.GirlLoveSubEvent_stage1[70])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 70;
                                    GameMgr.GirlLoveSubEvent_stage1[70] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 70;
                                }
                                break;

                            case "Sukumizu_Costume":

                                if (!GameMgr.GirlLoveSubEvent_stage1[71])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 71;
                                    GameMgr.GirlLoveSubEvent_stage1[71] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 71;
                                }
                                break;

                            case "Meid_Black_Costume":

                                if (!GameMgr.GirlLoveSubEvent_stage1[72])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 72;
                                    GameMgr.GirlLoveSubEvent_stage1[72] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 72;
                                }
                                break;

                            case "PinkGoth_Costume":

                                if (!GameMgr.GirlLoveSubEvent_stage1[73])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 73;
                                    GameMgr.GirlLoveSubEvent_stage1[73] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 73;
                                }
                                break;

                            case "RedDress_Costume":

                                if (!GameMgr.GirlLoveSubEvent_stage1[74])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 74;
                                    GameMgr.GirlLoveSubEvent_stage1[74] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 74;
                                }
                                break;

                            case "BalloonHat_Acce":

                                if (!GameMgr.GirlLoveSubEvent_stage1[75])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 75;
                                    GameMgr.GirlLoveSubEvent_stage1[75] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 75;
                                }
                                break;

                            case "AngelWing_Acce":

                                if (!GameMgr.GirlLoveSubEvent_stage1[76])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 76;
                                    GameMgr.GirlLoveSubEvent_stage1[76] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 76;
                                }
                                break;

                            case "Nekomimi_Acce":

                                if (!GameMgr.GirlLoveSubEvent_stage1[77])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 77;
                                    GameMgr.GirlLoveSubEvent_stage1[77] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 77;
                                }
                                break;

                            case "FlowerHairpin_Acce":

                                if (!GameMgr.GirlLoveSubEvent_stage1[78])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 78;
                                    GameMgr.GirlLoveSubEvent_stage1[78] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 78;
                                }
                                break;

                            case "TwincleStarDust_Acce":

                                if (!GameMgr.GirlLoveSubEvent_stage1[79])
                                {
                                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                                    GameMgr.GirlLoveSubEvent_num = 79;
                                    GameMgr.GirlLoveSubEvent_stage1[79] = true;

                                    mute_on = true;
                                    check_GirlLoveSubEvent_flag = false;
                                    GetEmeraldItem = true;

                                    SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                                    SubEvAfterHeartGet_num = 79;
                                }
                                break;

                            default:

                                break;
                        }

                        if (GetEmeraldItem)
                        {
                            break;
                        }                       
                    }
                    i++;
                }    
            }

            //置物や土産を買った 100番台～ 周回しても、フラグは引継ぎ。二度目以上の発生はない。
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (!GameMgr.GirlLoveSubEvent_stage1[100])
                {
                    if (pitemlist.KosuCount("kuma_nuigurumi") >= 1)
                    {
                        //メイン画面にもどったときに、イベントを発生させるフラグをON
                        GameMgr.GirlLoveSubEvent_num = 100;
                        GameMgr.GirlLoveSubEvent_stage1[100] = true;

                        mute_on = true;
                        check_GirlLoveSubEvent_flag = false;

                        SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
                        SubEvAfterHeartGet_num = 100;
                    }
                }
            }

            //レシピ100%達成
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (GameMgr.game_Recipi_archivement_rate >= 100.0f && GameMgr.GirlLoveSubEvent_stage1[101] == false) //4になったときのサブイベントを使う。
                {
                    GameMgr.GirlLoveSubEvent_num = 101;
                    GameMgr.GirlLoveSubEvent_stage1[101] = true;

                    check_GirlLoveSubEvent_flag = false;

                    mute_on = true;

                    ev_id = pitemlist.Find_eventitemdatabase("silver_neko_cookie_recipi");
                    pitemlist.add_eventPlayerItem(ev_id, 1); //銀のねこクッキーのレシピを追加
                }
            }

            //お金10万ルピア達成
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (PlayerStatus.player_money >= GameMgr.GoldMasterMoneyLine && GameMgr.GirlLoveSubEvent_stage1[102] == false) //4になったときのサブイベントを使う。
                {
                    GameMgr.GirlLoveSubEvent_num = 102;
                    GameMgr.GirlLoveSubEvent_stage1[102] = true;

                    check_GirlLoveSubEvent_flag = false;

                    mute_on = true;

                    ev_id = pitemlist.Find_eventitemdatabase("gold_neko_cookie_recipi");
                    pitemlist.add_eventPlayerItem(ev_id, 1); //金のねこクッキーのレシピを追加
                }
            }

            //その他イベント、ロード後イベントなど。90番台～
            if (!check_GirlLoveSubEvent_flag) //上で先に発生していたら、ひとまずチェックを回避
            { }
            else
            {
                if (Load_eventflag)
                {
                    if (GameMgr.GirlLoveSubEvent_stage1[90] == false)
                    {
                        Load_eventflag = false;
                        if (GameMgr.GirlLoveEvent_num == 50) //コンテストのとき
                        {
                            
                            GameMgr.GirlLoveSubEvent_num = 91;
                            check_GirlLoveSubEvent_flag = false;
                        }
                        else if (GameMgr.GirlLoveEvent_num >= 40 && GameMgr.GirlLoveEvent_num < 50) //ステージ４　コンテストが近い
                        {

                            GameMgr.GirlLoveSubEvent_num = 92;
                            check_GirlLoveSubEvent_flag = false;
                        }
                        else
                        {
                            GameMgr.GirlLoveSubEvent_num = 90;
                            check_GirlLoveSubEvent_flag = false;
                        }
                    }
                }
            }
           
            //フラグは必ずリセット           
            check_OkashiAfter_flag = false;
            check_GetMat_flag = false;

            //最後のタイミングで、決定したサブイベントの宴を再生
            if (!check_GirlLoveSubEvent_flag) //サブイベント発生した
            {
                girl1_status.HukidashiFlag = false;
                ResultComplete_flag = 0; //イベント読み始めたら、調合終了の合図をたてておく。

                //クエスト発生
                Debug.Log("サブ好感度イベントの発生");

                //イベント発動時は、ひとまず好感度ハートがバーに吸収されるか、感想を言い終えるまで待つ。
                StartCoroutine("ReadGirlLoveEvent");

            }
        }
    }

    //タイムコントローラーから、眠りの宴シナリオを呼び出す際に使用。
    public void OnSleepReceive()
    {
        Sleep_on = true;
        StartCoroutine("SleepDayEnd");
    }

    IEnumerator SleepDayEnd()
    {
        //今日の食事がランダムで決まる
        InitTodayFoodLibrary();
        if(_todayfood_lib.Count <= 0)
        {
            _todayfood = "じゃがバター";
            _todayfoodexpence = 30;
        } else
        {
            random = Random.Range(0, _todayfood_lib.Count);
            _todayfood = _todayfood_lib[random];
            _todayfoodexpence = _todayfoodexpence_lib[random];
        }
        

        random = Random.Range(0, 10);
        _todayfoodexpence += (random-5);
        GameMgr.MgrTodayFood = _todayfood;
        GameMgr.Foodexpenses = _todayfoodexpence;

        //** ここまで **

        GameMgr.scenario_ON = true;
        GameMgr.sleep_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                   //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        //リセット。
        PlayerStatus.player_girl_lifepoint = PlayerStatus.player_girl_maxlifepoint; //体力は全回復
        PlayerStatus.player_day++;
        GameMgr.Sale_ON = false;
        //PlayerStatus.player_time = 0;
        PlayerStatus.player_cullent_hour = GameMgr.StartDay_hour;
        PlayerStatus.player_cullent_minute = 0;

        //寝るイベント発生時に、ピクニックイベントのカウンタが+1進む。
        Debug.Log("ピクニックカウント: " + GameMgr.picnic_count);
        GameMgr.picnic_count--;

        //一日経つと、食費を消費
        //所持金をへらす
        moneyStatus_Controller.UseMoney(GameMgr.Foodexpenses);

        //寝たらスリープフラグもOFFに。
        Sleep_on = false;
    }

    public void OffCompoundSelectnoExtreme()
    {
        OffInteract();
    }

    public void OffCompoundSelect() //GirlEatJudgeからも読み出し
    {
        OffInteract();
        extreme_Button.interactable = false;
    }

    void OffInteract()
    {
        touch_controller.Touch_OnAllOFF();
        menu_toggle.GetComponent<Toggle>().interactable = false;
        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
        shop_toggle.GetComponent<Toggle>().interactable = false;
        bar_toggle.GetComponent<Toggle>().interactable = false;
        girleat_toggle.GetComponent<Toggle>().interactable = false;
        recipi_toggle.GetComponent<Toggle>().interactable = false;
        sleep_toggle.GetComponent<Toggle>().interactable = false;
        system_toggle.GetComponent<Toggle>().interactable = false;
        status_toggle.GetComponent<Toggle>().interactable = false;
        MainUICloseButton.GetComponent<Button>().interactable = false;
        hinttaste_toggle.GetComponent<Toggle>().interactable = false;
    }

    void OnCompoundSelect()
    {
        touch_controller.Touch_OnAllON();
        menu_toggle.GetComponent<Toggle>().interactable = true;
        getmaterial_toggle.GetComponent<Toggle>().interactable = true;
        shop_toggle.GetComponent<Toggle>().interactable = true;
        bar_toggle.GetComponent<Toggle>().interactable = true;
        girleat_toggle.GetComponent<Toggle>().interactable = true;
        recipi_toggle.GetComponent<Toggle>().interactable = true;
        sleep_toggle.GetComponent<Toggle>().interactable = true;
        system_toggle.GetComponent<Toggle>().interactable = true;
        status_toggle.GetComponent<Toggle>().interactable = true;
        MainUICloseButton.GetComponent<Button>().interactable = true;
        hinttaste_toggle.GetComponent<Toggle>().interactable = true;
        extreme_Button.interactable = true;
    }

    public void OnCompoundSelectObj() //GirlEatJudgeから読み出し
    {
        compoundselect_onoff_obj.SetActive(true);
    }

    //ゲームの進行度合いなどに応じて、表示ボタンなどを追加する。
    public void CheckButtonFlag()
    {
        if (GameMgr.GirlLoveSubEvent_stage1[0] || GameMgr.OkashiQuest_Num >= 1)
        {
            hinttaste_toggle.SetActive(true);
        }
        else
        {
            hinttaste_toggle.SetActive(false);
        }

        //まだお菓子を作ったことがなかったら、生地をクリックボタンが登場
        if (!PlayerStatus.First_recipi_on)
        {
            ClickPanel_1.SetActive(true);
        }
        else
        {
            ClickPanel_1.SetActive(false);
        }

        //お菓子が完成したが、一度もまだあげたことがない時。
        if(PlayerStatus.player_girl_eatCount <= 0 && extreme_panel.extreme_itemID != 9999)
        {
            ClickPanel_2.SetActive(true);
        }
        else
        {
            ClickPanel_2.SetActive(false);
        }
    }

    void HintButtonOFF() //ヒントボタンを非表示
    {
        ClickPanel_1.SetActive(false);
        ClickPanel_2.SetActive(false);
    }

    //クエスト進行・ハートレベルに応じてBGMが変わる。デバッグパネルからもアクセス
    public void bgm_change_story()
    {
        //BGM.csのほうが強い

        map_ambience.Stop();

        if (GameMgr.Story_Mode == 0)
        {
            if (GameMgr.GirlLoveEvent_num == 50) //コンテスト　のどかなはれ
            {
                GameMgr.mainBGM_Num = 4; //コンテスト　鳥の鳴き声が外でなく
                map_ambience.OnSunnyDayBird();
            }
            else
            {
                //最初のBGM
                if (PlayerStatus.girl1_Love_lv >= 1) //デフォルト　雨
                {
                    GameMgr.mainBGM_Num = 0; //雨はじまり                
                }
                if (GameMgr.GirlLoveEvent_num == 0) //最初は雨
                {
                    map_ambience.OnRainyDay(); //背景のSEを鳴らす。
                }
                if (GameMgr.GirlLoveEvent_num >= 1) //雨やむ
                {
                    map_ambience.Stop();
                }
                if (GameMgr.GirlLoveEvent_num >= 10) //ラスクでBGM変化
                {
                    GameMgr.mainBGM_Num = 2; //少し明るい　ラスクのBGM
                    map_ambience.Stop();
                }

            }
        }
        else
        {

        }
    }

    //ストーリー進行に応じて、背景の天気+エフェクトも変わる。
    public void Change_BGimage()
    {
        if (GameMgr.Story_Mode == 0)
        {
            if (GameMgr.GirlLoveEvent_num >= 0) //デフォルト　雨
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_white").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_morning").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Rain").gameObject.SetActive(true);
            }
            if (GameMgr.GirlLoveEvent_num >= 1) //くもり　どんより HLv2~
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_white").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_morning").gameObject.SetActive(true);
            }
            if (GameMgr.GirlLoveEvent_num >= 10) //うすぐもり HLv4~
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_morning").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
            }
            if (GameMgr.GirlLoveEvent_num >= 20) //やや霧がかったはれ　風が強い HLv6~
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_sunny").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);               
            }
            if (GameMgr.GirlLoveEvent_num >= 30) //はれ HLv8~
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_noon").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);
            }
            if (GameMgr.GirlLoveEvent_num >= 50) //のどかなはれ HLv10~
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_noon").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);
            }
            if (PlayerStatus.girl1_Love_lv >= 15) //快晴　夕方
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_noon").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);
            }
            if (PlayerStatus.girl1_Love_lv >= 20) //快晴　夕方　ぽんぽ日和
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_noon").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);
            }
        }
        else
        {
            //フリーモード
            DrawALLOFFBG();
            RealTimeBGSetInit();

            BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
            BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);
            BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);

            BG_RealtimeChange();
        }
    }

    //TimeControllerから読む。背景をリアルタイムに変更する処理。
    public void BG_RealtimeChange()
    {
        switch(GameMgr.BG_cullent_weather) //TimeControllerで変更
        {
            case 1:

                break;

            case 2: //深夜→朝

                bgweather_image_panel.transform.Find("BG_windowout_morning").GetComponent<SpriteRenderer>().DOFade(1, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_morning").GetComponent<SpriteRenderer>().DOFade(1, 5.0f)
                    .OnComplete(RealTimeBGSetInit);
                break;

            case 3:

                bgweather_image_panel.transform.Find("BG_windowout_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                break;

            case 4:

                bgweather_image_panel.transform.Find("BG_windowout_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);

                bgweather_image_panel.transform.Find("BG_windowout_sunny").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);                
                break;

            case 5:

                bgweather_image_panel.transform.Find("BG_windowout_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                bgweather_image_panel.transform.Find("BG_windowout_sunny").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);

                bgweather_image_panel.transform.Find("BG_windowout_noon").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                break;

            case 6:

                bgweather_image_panel.transform.Find("BG_windowout_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_morning").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                bgweather_image_panel.transform.Find("BG_windowout_sunny").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                bgweather_image_panel.transform.Find("BG_windowout_noon").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);

                bgweather_image_panel.transform.Find("BG_windowout_evening").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                BG_Imagepanel.transform.Find("BG_sprite_evening").GetComponent<SpriteRenderer>().DOFade(0, 5.0f);
                break;
        }
    }

    //満腹ゲージの処理
    public void ManpukuBarKoushin(int _param)
    {
        if(_param >= 0)
        {
            PlayerStatus.player_girl_manpuku += _param;
        }
        else
        {
            PlayerStatus.player_girl_manpuku += _param;
        }

        if(PlayerStatus.player_girl_manpuku <= 0)
        {
            PlayerStatus.player_girl_manpuku = 0;
        }
        if (PlayerStatus.player_girl_manpuku >= 100)
        {
            PlayerStatus.player_girl_manpuku = 100;
        }

        manpuku_slider.value = PlayerStatus.player_girl_manpuku;
    }

    //機嫌状態の処理
    public void GirlExpressionKoushin(int _param)
    {
        if (_param >= 0)
        {
            PlayerStatus.player_girl_manpuku += _param;
        }
        else
        {
            PlayerStatus.player_girl_manpuku += _param;
        }

        if (PlayerStatus.player_girl_express_param <= 0)
        {
            PlayerStatus.player_girl_express_param = 0;
        }
        else if (PlayerStatus.player_girl_express_param >= 100)
        {
            PlayerStatus.player_girl_express_param = 100;
        }

        if (PlayerStatus.player_girl_express_param < 20)
        {
            PlayerStatus.player_girl_expression = 1;
        }
        else if (PlayerStatus.player_girl_express_param >= 20 && PlayerStatus.player_girl_express_param < 40)
        {
            PlayerStatus.player_girl_expression = 2;
        }
        else if (PlayerStatus.player_girl_express_param >= 40 && PlayerStatus.player_girl_express_param < 60)
        {
            PlayerStatus.player_girl_expression = 3;
        }
        else if (PlayerStatus.player_girl_express_param >= 60 && PlayerStatus.player_girl_express_param < 80)
        {
            PlayerStatus.player_girl_expression = 4;
        }
        else if (PlayerStatus.player_girl_express_param >= 80)
        {
            PlayerStatus.player_girl_expression = 5;
        }
    }

    public void ChangeBGM() //デバッグパネルからアクセス用
    {
        sceneBGM.OnMainBGM();
    }

    void DrawALLOFFBG()
    {
        for (i = 0; i < bg_weather_image.Count; i++)
        {
            bg_weather_image[i].SetActive(false);
            bg_weather_image[i].GetComponent<SpriteRenderer>().DOFade(1, 0.0f);
        }
        for (i = 0; i < bgwall_sprite.Count; i++)
        {
            bgwall_sprite[i].SetActive(false);
            bgwall_sprite[i].GetComponent<SpriteRenderer>().DOFade(1, 0.0f);
        }
        for (i = 0; i < bgeffect_obj.Count; i++)
        {
            bgeffect_obj[i].SetActive(false);
        }
        
    }

    void RealTimeBGSetInit()
    {
        for (i = 0; i < bg_weather_image.Count; i++)
        {
            bg_weather_image[i].SetActive(true);
            bg_weather_image[i].GetComponent<SpriteRenderer>().DOFade(1, 0.0f);
        }
        for (i = 0; i < bgwall_sprite.Count; i++)
        {
            bgwall_sprite[i].SetActive(true);
            bgwall_sprite[i].GetComponent<SpriteRenderer>().DOFade(1, 0.0f);
        }
        bgweather_image_panel.transform.Find("BG_windowout_white").gameObject.SetActive(false);
    }

    //即座に朝背景に変更。Utageの寝るイベントから呼び出し
    public void OnMorningBG()
    {
        RealTimeBGSetInit();

    }

    public void HeartGuageTextKoushin()
    {
        //好感度ゲージを更新               
        debug_panel.GirlLove_Koushin(PlayerStatus.girl1_Love_exp);
    }

    public void MoneyTextKoushin()
    {
        moneystatus_panel.GetComponent<MoneyStatus_Controller>().money_Draw();
    }

    void InitTodayFoodLibrary()
    {
        _todayfood_lib.Clear();
        _todayfoodexpence_lib.Clear();

        for (i = 1; i <= PlayerStatus.girl1_Love_lv; i++)
        {
            switch (i)
            {
                case 1:
                    
                    _todayfood_lib.Add("じゃがバター");
                    _todayfoodexpence_lib.Add(30);                    
                    _todayfood_lib.Add("じゃがいもとお豆のスープ");
                    _todayfoodexpence_lib.Add(60);
                    _todayfood_lib.Add("パン");
                    _todayfoodexpence_lib.Add(50);
                    _todayfood_lib.Add("ゆでじゃが");
                    _todayfoodexpence_lib.Add(70);
                    _todayfood_lib.Add("野菜の端っこゆで");
                    _todayfoodexpence_lib.Add(50);
                    _todayfood_lib.Add("ほしにくのせパン");
                    _todayfoodexpence_lib.Add(100);
                    _todayfood_lib.Add("ねぎピザ");
                    _todayfoodexpence_lib.Add(30);
                    _todayfood_lib.Add("きのこピザ");
                    _todayfoodexpence_lib.Add(30);
                    break;

                case 3:

                    _todayfood_lib.Add("じゃがいもとベーコンの炒め");
                    _todayfoodexpence_lib.Add(75);
                    _todayfood_lib.Add("じゃがいもスープ");
                    _todayfoodexpence_lib.Add(60);                                     
                    _todayfood_lib.Add("ポテト・ガレット");
                    _todayfoodexpence_lib.Add(50);                  
                    _todayfood_lib.Add("ハンバーグもどき");
                    _todayfoodexpence_lib.Add(70);
                    
                    break;

                case 5:

                    _todayfood_lib.Add("特製ポテトペペロンチーノ");
                    _todayfoodexpence_lib.Add(100);
                    _todayfood_lib.Add("バリバリ貝のブイヤ・ベース");
                    _todayfoodexpence_lib.Add(110);
                    _todayfood_lib.Add("じゃがいものトマト煮込み");
                    _todayfoodexpence_lib.Add(90);
                    _todayfood_lib.Add("ステーキ");
                    _todayfoodexpence_lib.Add(90);
                    _todayfood_lib.Add("おさかな地中海蒸し焼き");
                    _todayfoodexpence_lib.Add(120);
                    _todayfood_lib.Add("手ごねバーグ");
                    _todayfoodexpence_lib.Add(120);
                    _todayfood_lib.Add("ほっくりじゃがピザ");
                    _todayfoodexpence_lib.Add(150);
                    _todayfood_lib.Add("ビールとえだまめのたきこみご飯");
                    _todayfoodexpence_lib.Add(120);
                    break;

                case 7:

                    _todayfood_lib.Add("ゴールデンカレーライス");
                    _todayfoodexpence_lib.Add(160);
                    _todayfood_lib.Add("アツアツリゾット");
                    _todayfoodexpence_lib.Add(120);
                    _todayfood_lib.Add("イカスミスパゲティ");
                    _todayfoodexpence_lib.Add(200);
                    
                    break;

                case 9:

                    _todayfood_lib.Add("落雷じゃがいもスープ");
                    _todayfoodexpence_lib.Add(200);
                    _todayfood_lib.Add("ブルゴーニュ風ステーキ");
                    _todayfoodexpence_lib.Add(300);
                    _todayfood_lib.Add("じゃがいもりだくさんのペスカトーレ");
                    _todayfoodexpence_lib.Add(200);
                    _todayfood_lib.Add("うまうまステーキ");
                    _todayfoodexpence_lib.Add(250);
                    break;

                default:

                    break;
            }
        }             
    }

    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
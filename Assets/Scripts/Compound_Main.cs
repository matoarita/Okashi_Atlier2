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
    private MessageWindow msg_window;

    private GameObject text_area_Main;
    private Text _textmain;

    private GameObject text_area_compound;
    private Text _textcomp;

    private GameObject text_hikari_makecaption;

    private Text picnic_itemText;

    private GameObject mainUI_panel_obj;
    private bool SceneStart_flag;

    private SaveController save_controller;
    private keyManager keymanager;
    private SceneInitSetting sceneinit_setting;
    private ExpTable exp_table;

    private GameObject canvas;

    private SoundController sc;

    private Exp_Controller exp_Controller;
    private MoneyStatus_Controller moneyStatus_Controller;
    private CompoundMainController compoundmain_Controller;
    private ContestStartListDataBase conteststartList_database;
    private CatDataBase catDataBase;
    private QuestSetDataBase quest_database;

    private BGM sceneBGM;
    private Map_Ambience map_ambience;
    public bool bgm_change_flag2;

    private Girl1_status girl1_status;
    private Special_Quest special_quest;
    private Touch_Controll character_touch_controll;

    private TimeController time_controller;

    private Debug_Panel_Init debug_panel_init;
    private Debug_Panel debug_panel;

    private GameObject selectPanel_1;
    private GameObject select_original_button_obj;
    private GameObject select_recipi_button_obj;
    private GameObject select_extreme_button_obj;
    private GameObject select_hikarimake_button_obj;
    private GameObject select_sister_shop_button_obj;
    private Button select_original_button;
    private Button select_recipi_button;
    private Button select_extreme_button;
    private Button select_hikarimake_button;
    private Button select_sister_shop_button;
    private Button select_no_button;

    private GameObject starStampPanel;
    private GameObject newAreaRelease_Panel;
    private string newarea_gohoubitext;
    private string newarea_titletext;
    private Sprite newarea_gohoubiicon;

    private GameObject girl_love_exp_bar;
    private Text girl_param;
    private GameObject moneystatus_panel;
    //private GameObject kaerucoin_panel;

    private GameObject quest_kakuninButton_obj;
    private GameObject quest_CheckPanel_obj;
    //private GameObject GetMatStatusButton_obj;

    private GameObject contest_kakuninButton_obj;
    private GameObject contest_kakuninButton_iconobj;
    private GameObject contest_CheckPanel_obj;

    private GameObject starPanel_kakuninButton_obj;
    private GameObject starPanel_obj;


    private GameObject gameQuestPanel;
    private GameObject gameQuestPanel_Panel;
    private GameObject yachinPanel;

    private GameObject manpuku_bar;
    private Slider manpuku_slider;
    private Text manpuku_text;

    private Text kigen_text;

    private GameObject GirlHeartEffect_obj;

    private GameObject TimePanel_obj1;
    private GameObject TimePanel_obj2;

    private Text questname;

    private GameObject getmatplace_panel;
    private GetMatPlace_Panel getmatplace;
    private ItemMatPlaceDataBase matplace_database;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject recipimemoController_obj;
    private GameObject recipiMemoButton;
    private GameObject memoResult_obj;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

    private GameObject Stagepanel_obj;

    private GameObject black_panel_A;
    private GameObject scene_black_effect;
    private GameObject compoBG_A;
    private GameObject ResultBGimage;
    private GameObject fadeout_panel_obj;

    private GameObject Hikarimake_StartPanel;
    private GameObject SelectCompo_panel_1;
    private GameObject system_panel;
    private GameObject status_panel;
    private GameObject okashihint_panel;

    private GameObject bg_accessory_panel;

    private CombinationMain Combinationmain;
    private BGAcceTrigger BGAccetrigger;

    private PlayerItemList pitemlist;

    private Buf_Power_Keisan bufpower_keisan;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private EventDataBase eventdatabase;
    private MagicSkillListDataBase magicskill_database;

    private PlayerDefaultStartItemGet playerDefaultStart_ItemGet;

    private GameObject bgpanelmatome;
    private GameObject bgpanelmatome_debug;
    private Touch_Controll_Item bg_touch_controll;
    private GameObject BG_Imagepanel;
    private Animator BGImg_dayAnim;
    private GameObject BGImageTemaePanel;
    private List<GameObject> bgwall_sprite = new List<GameObject>();

    private GameObject BG_effectpanel;
    private List<GameObject> bgeffect_obj = new List<GameObject>();

    private BG_Effect_Particle BG_effectpanel_Effect;

    private GameObject bgweather_image_panel;
    private List<GameObject> bg_weather_image = new List<GameObject>();

    private GameObject CatComingGatPanel_obj;

    //Live2Dモデルの取得
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private int default_live2d_draworder;
    private Vector3 Live2d_default_pos;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;
    private int trans_position;
    //public int ResultComplete_flag;
    private GameObject character_root;
    private GameObject character_move;
    private GameObject Anchor_Pos;

    private GameObject compoundselect_onoff_obj;    

    private GameObject compoBGA_image;
    private GameObject compoBGA_imageOri;
    private GameObject compoBGA_imageRecipi;
    private GameObject compoBGA_imageExtreme;
    private GameObject compoBGA_imageHikariMake;

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

    private GameObject mainlist_scrollview_obj;
    private GameObject outhome_toggle;

    private Button extreme_Button;
    private Button recipi_Button;
    private GameObject sell_Button;
    private GameObject present_Button;
    private GameObject stageclear_panel;
    private GameObject stageclear_Button;
    private Toggle stageclear_button_toggle;
    private Text stageclear_button_text;
    private string stageclear_default_text;

    private bool Recipi_loading;
    private bool NewAreaCheck_loading;
    public bool check_recipi_flag;
    private int not_read_total;
    private int _checkexp;
    private bool starrank_kaikin_ON;

    private GameObject yes_no_panel; //通常時のYes, noボタン
    private GameObject yes_no_clear_panel; //クリア時のYes, noボタン
    private GameObject yes_no_clear_okashi_panel; //クリア時のYes, noボタン
    private GameObject yes_no_sleep_panel; //寝るかどうかのYes, noボタン

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject autosave_panel;



    private int i, j, _id, ev_id;
    private int random, random2;
    private int lot_count;
    private int nokori_kaisu;
    private int event_num;
    private int recipi_num;
    private int newarea_num;
    private int newarea_star;
    private int comp_ID;
    private int clear_love;
    private int recipi_id;
    //public bool SubEvAfterHeartGet; //Utage_scenarioからも読まれる
    //private int SubEvAfterHeartGet_num;
    private bool subevent_after_end;
    private bool GetFirstCollectionItem;
    private bool GetEmeraldItem;
    private string GetEmeraldItemName;
    private int get_heart;
    private int get_star;
    private bool map_move;
    private string _bg_str1, _bg_str2, _bg_str3;
    private int cat_cost;
    private bool closebutton;

    private string _todayfood;
    private List<string> _todayfood_lib = new List<string>();
    private int _todayfoodexpence;
    private List<int> _todayfoodexpence_lib = new List<int>();
    private float _todayfood_buf;

    //デバッグ確認用　ゲーム中では、GameMgr.compound_status（select）を使う。
    public int compound_status;
    public int compound_select;

    public int event_itemID; //イベントレシピ使用時のイベントのID

    private bool gameover_loading;
    private bool mute_on;

    private bool heartget_ON;

    private int picnic_exprob;

    private GameObject HintObjectGroup;
    private GameObject ClickPanel_1;
    private GameObject ClickPanel_2;

    private List<GameObject> _effect_list = new List<GameObject>(); //
    private GameObject effparticle_Prefab1;

    private int motion_layer_num = 1;

    private int daynum;

    private Color color_set;
    private bool StartRead;
    private int _baseID;
    private bool _teishutu_on;

    // Use this for initialization
    void Start()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 10;
        //Debug.Log("(GameMgr.Scene_Category_Num): " + GameMgr.Scene_Category_Num);

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

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子        

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化
        debug_panel = GameObject.FindWithTag("Debug_Panel").GetComponent<Debug_Panel>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //イベントデータベースの取得
        eventdatabase = EventDataBase.Instance.GetComponent<EventDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //ゲーム最初に所持するアイテムを決定するスクリプト
        playerDefaultStart_ItemGet = PlayerDefaultStartItemGet.Instance.GetComponent<PlayerDefaultStartItemGet>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //レベルアップチェック用オブジェクトの取得
        exp_table = ExpTable.Instance.GetComponent<ExpTable>();

        //キー入力受付コントローラーの取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();
        GameMgr.matbgm_change_flag = false;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        //レシピリストパネルの取得
        recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A").gameObject;
        compoBG_A.SetActive(false);

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;
        //scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f); //黒い画面からスタート

        //調合コントローラーの取得
        compoundmain_Controller = canvas.transform.Find("CompoundMainController").GetComponent<CompoundMainController>();

        //レシピメモボタンを取得
        recipimemoController_obj = compoBG_A.transform.Find("RecipiMemo_ScrollView").gameObject;
        recipiMemoButton = compoBG_A.transform.Find("RecipiMemoButton").gameObject;
        memoResult_obj = compoBG_A.transform.Find("Memo_Result").gameObject;

        //調合セレクトパネルを取得
        selectPanel_1 = compoBG_A.transform.Find("SelectPanel_1").gameObject;
        select_original_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/OriginalButton").gameObject;
        select_original_button = select_original_button_obj.GetComponent<Button>();
        select_recipi_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/RecipiButton").gameObject;
        select_recipi_button = select_recipi_button_obj.GetComponent<Button>();
        select_extreme_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/ExButton").gameObject;
        select_extreme_button = select_extreme_button_obj.GetComponent<Button>();
        select_hikarimake_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/HikariMakeButton").gameObject;
        select_hikarimake_button = select_hikarimake_button_obj.GetComponent<Button>();
        select_sister_shop_button_obj = selectPanel_1.transform.Find("Scroll View/Viewport/Content/SellButton").gameObject;
        select_sister_shop_button = select_sister_shop_button_obj.GetComponent<Button>();
        select_no_button = selectPanel_1.transform.Find("No").GetComponent<Button>();

        starStampPanel = canvas.transform.Find("StarStampPanel").gameObject;
        starStampPanel.SetActive(false);

        newAreaRelease_Panel = canvas.transform.Find("NewAreaReleasePanel").gameObject;
        newAreaRelease_Panel.SetActive(false);

        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_clear_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel1").gameObject;
        yes_no_sleep_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel2").gameObject;
        yes_no_clear_okashi_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel3").gameObject;

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

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();
        msg_window = text_area.GetComponentInChildren<MessageWindow>();
        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.transform.Find("Text").GetComponent<Text>();
        text_area_compound = compoBG_A.transform.Find("MessageWindowComp").gameObject;
        _textcomp = text_area_compound.GetComponentInChildren<Text>();

        text_hikari_makecaption = text_area_compound.transform.Find("HikariMakeOkashiCaptionText").gameObject;
        text_hikari_makecaption.SetActive(false);

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

        fadeout_panel_obj = canvas.transform.Find("FadeOutPanel").gameObject;
        fadeout_panel_obj.GetComponent<CanvasGroup>().DOFade(0, 0.0f); //白い画面はオフ

        compoBGA_image = compoBG_A.transform.Find("BG").gameObject;
        compoBGA_imageOri = compoBG_A.transform.Find("OriCompoImage").gameObject;
        compoBGA_imageRecipi = compoBG_A.transform.Find("RecipiCompoImage").gameObject;
        compoBGA_imageExtreme = compoBG_A.transform.Find("ExtremeImage").gameObject;
        compoBGA_imageHikariMake = compoBG_A.transform.Find("HikariMakeImage").gameObject;
        ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
        ResultBGimage.SetActive(false);

        Hikarimake_StartPanel = compoBG_A.transform.Find("HikariMakeStartPanel").gameObject;
        Hikarimake_StartPanel.SetActive(false);

        //調合選択画面の取得
        SelectCompo_panel_1 = compoBG_A.transform.Find("SelectPanel_1").gameObject;
        SelectCompo_panel_1.SetActive(false);

        //ピクニック用のテキスト取得
        picnic_itemText = compoBG_A.transform.Find("SelectPanel_1/Picnic_yesno/ItemText/Text").GetComponent<Text>();

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel/Comp").gameObject;
        getmatplace = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();
        getmatplace_panel.SetActive(false);
        //GetMatStatusButton_obj = canvas.transform.Find("MainUIPanel/Comp/GetMatStatusPanel").gameObject;

        //ねこゲットパネルの取得
        CatComingGatPanel_obj = canvas.transform.Find("CatComeGetPanel").gameObject;
        CatComingGatPanel_obj.SetActive(false);
        CatComingGatPanel_obj.transform.Find("Yes_no_Pane_catget").gameObject.SetActive(false);
        closebutton = false;

        effparticle_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_3");

        //お金パネル
        moneystatus_panel = canvas.transform.Find("MainUIPanel/MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Controller.Instance.GetComponent<MoneyStatus_Controller>();


        //Live2Dモデルの取得
        character_root = GameObject.FindWithTag("CharacterRoot").gameObject;
        character_move = character_root.transform.Find("CharacterMove").gameObject;
        _model_obj = character_root.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;

        cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
        default_live2d_draworder = cubism_rendercontroller.SortingOrder;
        Live2d_default_pos = _model_obj.transform.localPosition;
        live2d_animator = _model_obj.GetComponent<Animator>();
        live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。
        GameMgr.ResultComplete_flag = 0; //調合が完了したよフラグ

        Anchor_Pos = character_move.transform.Find("Anchor_1").gameObject;
        character_touch_controll = character_root.transform.Find("CharacterMove/Character").GetComponent<Touch_Controll>();
        live2d_animator.Play("Idle", motion_layer_num, 0.0f); //デフォルトアイドルモーションセット
        live2d_animator.Update(0f);

        //メイン調合のコマンドオブジェクト取得
        compoundselect_onoff_obj = canvas.transform.Find("MainUIPanel/Comp/CompoundSelect_ScrollView").gameObject;

        //女の子の反映用ハートエフェクト取得
        GirlHeartEffect_obj = character_root.transform.Find("CharacterMove/Particle_Heart_Character").gameObject;

        //ヒントボタンの取得
        HintObjectGroup = canvas.transform.Find("MainUIPanel/Comp/HintObject/").gameObject;
        ClickPanel_1 = HintObjectGroup.transform.Find("ClickPanel1").gameObject;
        ClickPanel_1.SetActive(false);
        ClickPanel_2 = HintObjectGroup.transform.Find("ClickPanel2").gameObject;
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

        mainlist_scrollview_obj = canvas.transform.Find("MainUIPanel/MainList_ScrollView_01").gameObject;
        outhome_toggle = mainlist_scrollview_obj.transform.Find("Viewport/Content_Main/NPC1_SelectToggle").gameObject;

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
        SceneStart_flag = false;

        //ステージパネルの取得
        Stagepanel_obj = canvas.transform.Find("MainUIPanel/StagePanel").gameObject;

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
        manpuku_text = manpuku_bar.transform.Find("ManpukuText").GetComponent<Text>();

        //クエスト確認ボタンの取得
        quest_kakuninButton_obj = canvas.transform.Find("MainUIPanel/QuestKakuninButtonPanel").gameObject;
        quest_CheckPanel_obj = canvas.transform.Find("QuestKakuninHyoujiPanel").gameObject;
        quest_CheckPanel_obj.SetActive(false);

        //コンテスト確認ボタンの取得
        contest_kakuninButton_obj = canvas.transform.Find("MainUIPanel/ContestKakuninButtonPanel").gameObject;
        contest_kakuninButton_iconobj = contest_kakuninButton_obj.transform.Find("ContestKakuninButton").gameObject;
        contest_CheckPanel_obj = canvas.transform.Find("ContestKakuninHyoujiPanel").gameObject;
        contest_CheckPanel_obj.SetActive(false);

        starPanel_kakuninButton_obj = canvas.transform.Find("MainUIPanel/StarPanelKakuninButtonPanel").gameObject;
        starPanel_obj = canvas.transform.Find("StarStampPanel").gameObject;
        starPanel_obj.SetActive(false);

        if(GameMgr.System_ContestIcon_OnFlag) //スターパネルをもらうまではパネルをオフ
        {
            starPanel_kakuninButton_obj.transform.Find("StarKakuninButton").gameObject.SetActive(true);
        }
        else
        {
            starPanel_kakuninButton_obj.transform.Find("StarKakuninButton").gameObject.SetActive(false);
        }

        if (GameMgr.System_ContestIcon_OnFlag) //スターパネルをもらったあとはコンテストアイコンをON
        {
            contest_kakuninButton_iconobj.SetActive(true);
        }
        else
        {
            contest_kakuninButton_iconobj.SetActive(false);
        }

        /*if (!GameMgr.System_Contest_StartNow) //falseの場合、コンテストすぐはじまらず何日後スタートバージョンのとき
        {
            if (GameMgr.System_ContestIcon_OnFlag) //スターパネルをもらったあとはコンテストアイコンをON
            {
                contest_kakuninButton_iconobj.SetActive(true);
            }
            else
            {
                contest_kakuninButton_iconobj.SetActive(false);
            }
        }
        else
        {
            contest_kakuninButton_iconobj.SetActive(false);
        }*/

        //メインクエ表示パネルの取得
        gameQuestPanel = canvas.transform.Find("MainUIPanel/Comp/GameQuestPanel").gameObject;
        gameQuestPanel_Panel = canvas.transform.Find("MainUIPanel/Comp/GameQuestPanel/Panel").gameObject;

        //家賃パネルの取得
        yachinPanel = canvas.transform.Find("MainUIPanel/Comp/YachinPanel").gameObject;

        kigen_text = manpuku_bar.transform.Find("KigenText").GetComponent<Text>();

        //えめらるどんぐりパネルの取得
        //kaerucoin_panel = canvas.transform.Find("MainUIPanel/KaeruCoin_Panel").gameObject;

        stageclear_panel = canvas.transform.Find("MainUIPanel/Comp/StageClearButton_Panel").gameObject;
        stageclear_Button = stageclear_panel.transform.Find("StageClear_Button").gameObject;
        stageclear_button_toggle = stageclear_Button.GetComponent<Toggle>();
        stageclear_button_text = stageclear_Button.transform.Find("TextPlate/Text").GetComponent<Text>();
        stageclear_default_text = stageclear_button_text.text;
        stageclear_button_toggle.isOn = false;
        stageclear_Button.SetActive(false);


        //背景天気オブジェクトの取得


        //デバッグシーンとゲーム用シーンで分ける
        switch (SceneManager.GetActiveScene().name)
        {
            case "Hikari_CompMain": //こっちはデバッグ用

                bgpanelmatome_debug = GameObject.FindWithTag("BG_Debug");
                bg_touch_controll = bgpanelmatome_debug.transform.Find("BGAccessory").GetComponent<Touch_Controll_Item>();

                bgweather_image_panel = bgpanelmatome_debug.transform.Find("BGImageWindowOutPanel/BGOutimg_sc01").gameObject;
                BG_Imagepanel = bgpanelmatome_debug.transform.Find("BGImagePanel/BGimg_sc02").gameObject;
                BGImg_dayAnim = BG_Imagepanel.GetComponent<Animator>();
                BGImageTemaePanel = GameObject.FindWithTag("BGImageTemaePanel");
                BG_effectpanel = bgpanelmatome_debug.transform.Find("BG_Effect/effect_sc01").gameObject;
                BG_effectpanel_Effect = bgpanelmatome_debug.transform.Find("BG_Effect").GetComponent<BG_Effect_Particle>();
                BG_effectpanel_Effect.SetEffectName("effect_sc01"); //使うエフェクトネームをパーティクル自身にも保存
                bg_accessory_panel = bgpanelmatome_debug.transform.Find("BGAccessory").gameObject;
                bg_accessory_panel.SetActive(false);
                break;

            default: //ゲームシーン全般

                bgpanelmatome_debug = GameObject.FindWithTag("BG_Debug");
                bgpanelmatome_debug.SetActive(false);

                bgpanelmatome = GameObject.FindWithTag("BG");

                CompoundRoomSetting(); //調合部屋を設定                

                break;
        }

        bg_weather_image.Clear();
        foreach (Transform child in bgweather_image_panel.transform) //
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


        //飾りアイテムのセット        
        BGAccetrigger = bg_accessory_panel.GetComponent<BGAcceTrigger>();
        BGAccetrigger.DrawBGAcce();

        /* --- */


        //パラメータ関係リセット
        Recipi_loading = false;
        NewAreaCheck_loading = false;
        check_recipi_flag = false;
        heartget_ON = false;
        map_move = false;

        GameMgr.GirlLoveEvent_bunki_status = 0;
        GameMgr.girlEat_ON = false;
        GameMgr.GirlLove_loading = false;
        GameMgr.check_GirlLoveEvent_flag = false;
        GameMgr.check_GirlLoveSubEvent_flag = false;
        GameMgr.check_GirlLoveTimeEvent_flag = false;
        GameMgr.check_CompoAfter_flag = false;
        GameMgr.check_GetMat_flag = false;
        GameMgr.check_OkashiAfter_flag = false;
        GameMgr.Sleep_CheckEnd = false;
        GameMgr.Status_zero_readOK = false;
        GameMgr.Utage_MapMoveON = false;
        GameMgr.utage_charaHyouji_flag = false;

        GameMgr.check_StarPanel_Endflag = true; //スターパネル　チェック前にすでに開始フラグはたてる。サブイベントの発生を回避する。チェック終わりでfalseになる。

        gameover_loading = false;
        subevent_after_end = false;
        GetFirstCollectionItem = false;
        GetEmeraldItem = false;

        GameMgr.AmusePlayCount = 0;

        //女の子　お菓子ハングリー状態のリセット
        girl1_status.Girl1_Status_Init();
        girl1_status.DefFaceChange();

        //ステージごとのロード処理
        switch (GameMgr.stage_number)
        {
            case 1:

                if (GameMgr.stage1_load_ok != true)
                {
                    GameMgr.stage1_load_ok = true;
                    GameMgr.scenario_flag = 110;
                }

                break;

            case 2:


                break;

            case 3:

                break;
        }

        //メイン画面に表示する、現在のクエスト
        questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();

        //初期メッセージ
        StartMessage();
        text_area_Main.SetActive(false);


        //初期アイテムの取得。一度きり。
        playerDefaultStart_ItemGet.DefaultStartPitem();

        //ウィンドウキャラ名設定
        GameMgr.Window_CharaName = GameMgr.mainGirl_Name;

        ReturnBackHome();

        //セーブがあるかどうかをチェック
        save_controller.SystemloadCheck_SaveOnly();

        //二週目以降はエメラルショップはじめからでてる。
        if (GameMgr.ending_count >= 1)
        {
            matplace_database.matPlaceKaikin("Or_EmeraldShop_A1");
        }

        //ロード画面から読み込んだ際の処理
        if (GameMgr.GameLoadOn)
        {
            Debug.Log("ロード画面から読み込んだ");
            GameMgr.GameLoadOn = false;

            save_controller.DrawGameScreen();
            //keymanager.InitCompoundMainScene();
            GameMgr.MesaggeKoushinON = true;
            StartRead = false;
            StartMessage();
            
            character_move.transform.position = new Vector3(0f, 0, 0); //念のため、ゼロにリセット            

            Debug.Log("エンディング回数: " + GameMgr.ending_count);

            if(GameMgr.GameOverLoadFlag) //さらに、ゲームオーバー画面から読んだ場合
            {
                GameMgr.GameOverLoadFlag = false;

                //コンテストの提出データは更新する
                GameMgr.contest_TotalScore = GameMgr.GmO_contest_TotalScore;
                GameMgr.contest_okashiName = GameMgr.GmO_contest_okashiName;
                GameMgr.contest_okashiNameHyouji = GameMgr.GmO_contest_okashiNameHyouji;
                GameMgr.contest_okashiSlotName = GameMgr.GmO_contest_okashiSlotName;
                GameMgr.contest_okashiID = GameMgr.GmO_contest_okashiID;
                GameMgr.contest_lasthint_text = GameMgr.GmO_contest_lasthint_text; //
                GameMgr.contest_last_Disqualification = GameMgr.GmO_contest_last_Disqualification; //課題のおかしでないため失格した場合　保存用
                GameMgr.contest_shokukan_param = GameMgr.GmO_contest_shokukan_param; //
                GameMgr.contest_shokukan_mes = GameMgr.GmO_contest_shokukan_mes; //
                GameMgr.contest_sweat_param = GameMgr.GmO_contest_sweat_param; //
                GameMgr.contest_sour_param = GameMgr.GmO_contest_sour_param; //
                GameMgr.contest_bitter_param = GameMgr.GmO_contest_bitter_param;
            }
        }
        

        //Debug.Log("ストーリーモード: " + GameMgr.Story_Mode);

        //固有IDがないオリジナルアイテムを自動で削除する　バージョン更新用
        pitemlist.DeleteETCOriginalKoyuIDItem();

        //ランダムシードの初期化　現在の秒をもとに値を決める
        Random.InitState(System.DateTime.Now.Second); 

        Touch_ALLOFF(); //Compound_Status=0になるまでは、触れない
        if (!GameMgr.outgirl_Nowprogress) //外出中はLive2Dをオフに。
        {
            CharacterLive2DImageON();
            //Touch_ALLON();
        }
        else
        {
            CharacterLive2DImageOFF();
            Touch_ALLOFF();
        }

        GameMgr.CompoAfter_BackGirl = false;
        GameMgr.System_PoolEnd = false; //ソーダアイランドプールフラグをリセット
        GameMgr.System_HotelEnd = false; //他、場所フラグをリセット
        GameMgr.System_HotSpringEnd = false;
        GameMgr.NPC_NoScoreCheck = false; //NPCにアイテムあげるとき、店売りアイテムあげるかどうかのチェック用　trueだと店売り扱いで処理する。各シーンだと抜けがある可能性あるため、念のためここで処理。
        StartRead = false;


        //現在の時間計算（寝るチェックなしで時間のみ更新）
        time_controller.TimeKoushin(0, false);

        //家賃日までの日数計算
        yachinPanel.GetComponent<YachinPanel>().Setting_CullentYachin();
        yachinPanel.GetComponent<YachinPanel>().YachinHyouji();

        //時間をチェックし、背景を自動で変更
        Change_BGimage();

        //レベルアップパネル系　入室時に削除
        girlEat_judge.ListLVUPClear();

        //プレイヤーステータスのリセット
        PlayerStatus.ResetPlayerMagicStatus();


        //デバッグ用 本番ではオフにする。コンテスト終了後、寝るが終わったあとに始まるイベントのこと　寝るを押せばすぐに発動するようにしてる。
        //GameMgr.Contest_afterHomeEventFlag = true;
        //

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    void SetBGObj(string _bgoutimg, string _bgimg, string _effimg)
    {
        //まず外、背景の家中、エフェクトの３つを背景オブジェクト全てオフ
        foreach (Transform child in bgpanelmatome.transform.Find("BGImageWindowOutPanel").transform) //
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in bgpanelmatome.transform.Find("BGImagePanel").transform) //
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in bgpanelmatome.transform.Find("BG_Effect").transform) //
        {
            child.gameObject.SetActive(false);
        }

        //その後、該当シーンのものを表示
        BGImageTemaePanel = GameObject.FindWithTag("BGImageTemaePanel");

        _bg_str1 = "BGImageWindowOutPanel/" + _bgoutimg;
        bgweather_image_panel = bgpanelmatome.transform.Find(_bg_str1).gameObject;
        bgweather_image_panel.SetActive(true);

        _bg_str2 = "BGImagePanel/" + _bgimg;
        BG_Imagepanel = bgpanelmatome.transform.Find(_bg_str2).gameObject;
        BGImg_dayAnim = BG_Imagepanel.GetComponent<Animator>();
        BG_Imagepanel.SetActive(true);

        _bg_str3 = "BG_Effect/" + _effimg;
        BG_effectpanel = bgpanelmatome.transform.Find(_bg_str3).gameObject;
        BG_effectpanel.SetActive(true);
        BG_effectpanel_Effect = bgpanelmatome.transform.Find("BG_Effect").GetComponent<BG_Effect_Particle>();
        BG_effectpanel_Effect.SetEffectName(_effimg); //使うエフェクトネームをパーティクル自身にも保存

        bg_accessory_panel = bgpanelmatome.transform.Find("BGAccessory").gameObject;
        bg_touch_controll = bg_accessory_panel.GetComponent<Touch_Controll_Item>();
    }

    //家に帰ってきたときの処理
    void ReturnBackHome()
    {
        if (GameMgr.Scene_back_home)
        {
            GameMgr.Scene_back_home = false;

            //玄関音
            sc.EnterSound_01();

            //時間をチェックし、背景を自動で変更 　フリーモードのときのみチェックしてる
            Weather_Change();

            //オートセーブ
            if (GameMgr.AUTOSAVE_ON)
            {
                save_controller.OnSaveMethod(GameMgr.System_save_nowslot);
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
        if (GameMgr.Scene_LoadedOn_End) //シーンのロード（セーブデータのロードも）全て完了してからUpdateの処理に入る
        {

            if (!StartRead) //シーン最初だけ読み込む
            {
                StartRead = true;
                
                sceneBGM.OnMainBGM(); //メインBGMを変更　ハートレベルに応じてBGMも切り替わる場合。
                if (!GameMgr.Contest_afterHomeEventFlag) //コンテストから帰ってきた直後はBGMはオフのまま
                {
                    sceneBGM.NowFadeVolumeONBGM();
                }

                PlayerStatus.SetPatissierRank(PlayerStatus.player_ninki_param); //パティシエランクのチェックとセット　現在の状態に更新

                OuthomePanelONOFF();               
            }

            //デバッグでの確認用
            compound_status = GameMgr.compound_status;
            compound_select = GameMgr.compound_select;

            //宴途中でホワイトをONにする　フェードアウト演出用
            if (GameMgr.Utage_FadeOutWhiteON)
            {
                //白からフェードイン        
                fadeout_panel_obj.GetComponent<CanvasGroup>().alpha = 1;
            }

            //宴途中でブラックをオフにする 他シーンへ移動する演出用
            if (GameMgr.Utage_SceneEnd_BlackON)
            {
                Debug.Log("シーン全体　ブラックアウト");
                GameMgr.Utage_SceneEnd_BlackON = false;
                scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f);
                canvas.SetActive(true);
                map_move = true;
            }

            if (map_move) //シーン移動中は、デフォルト処理にはそもそも入らないようにする。
            {
                //Debug.Log("シーン移動中");
            }
            else
            {
                //エクストラモード　お金が0を下回ったらゲームオーバー
                if (GameMgr.System_GameOver_ON)
                {
                    if (GameMgr.Story_Mode != 0)
                    {
                        if (PlayerStatus.player_money <= 0)
                        {
                            if (!gameover_loading)
                            {
                                gameover_loading = true; //アップデートを更新しないようにしている。
                                                         //お金が0になったので、ゲーム終了　ぐええ
                                Debug.Log("ゲームオーバー画面表示");

                                FadeManager.Instance.LoadScene("999_Gameover", 0.3f);
                            }
                        }
                    }
                }

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
                        //GameMgr.CompoundEvent_flag = false;

                        eventdatabase.ReturnHomeCompoundEvent();
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
                if (GameMgr.picnic_event_reading_now)
                {
                    MainCompoundMethod();
                }

                

                //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。チュートリアルなどの強制イベントのチェック。
                if (GameMgr.scenario_ON == true)
                {
                    //Debug.Log("ゲームマネジャー　シナリオON");           

                    //チュートリアル
                    //チュートリアルモードがONになったら、この中の処理が始まる。
                    if (GameMgr.tutorial_ON)
                    {
                        TutorialEvent();
                    }
                    else //チュートリアル以外、デフォルトで、宴を読んでいるときの処理
                    {
                        WindowOff();

                        check_recipi_flag = false;

                        //腹減りカウント一時停止
                        girl1_status.GirlEatJudgecounter_OFF();

                        girl1_status.DeleteHukidashiOnly();
                        girl1_status.Girl1_Status_Init();

                        Touch_ALLOFF();
                        SceneStart_flag = false;

                        //テキストエリアの表示
                        if (GameMgr.catcoming_event_ON)
                        { }
                        else
                        {
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

                }
                else //以下が、通常の処理
                {

                    if (GameMgr.girlEat_ON) //お菓子判定中の間は、無条件で、メインの処理は無視する。
                    { }
                    else
                    {
                        if (GameMgr.CompoundSceneStartON) //お菓子調合中もメインの処理は無視。おわったら、サブイベントチェックしてから、メインへ。
                        { }
                        else
                        {
                            if (GameMgr.ResultOFF) //リザルト画面開き中のときは、イベントチェックしない
                            { }
                            else
                            {
                                if (GameMgr.Sleep_CheckEnd) //眠り中はイベントチェックしない
                                { }
                                else
                                {
                                    //まずイベントチェック前に、採取から帰ってきてたら、採取パネル表示して閉じるまでを優先
                                    if (GameMgr.Getmat_return_home)
                                    {
                                        GameMgr.Getmat_return_home = false;

                                        GetMatReturnCheck(); //ここを通過したあと、ResultOFF=trueにする。なので、一回しかここは通らない。
                                    }
                                    else
                                    {
                                        //クエストクリア時、次のお菓子イベントが発生するかどうかのチェック。
                                        if (!GameMgr.check_GirlLoveEvent_flag)
                                        {
                                            Debug.Log("メインイベントチェックON");

                                            //メインイベント
                                            eventdatabase.GirlLoveMainEvent();
                                        }
                                        else
                                        {
                                            //サブイベントの発生をチェック。
                                            if (!GameMgr.check_GirlLoveSubEvent_flag)
                                            {
                                                Debug.Log("サブイベントチェックON");

                                                //好感度に応じて発生するサブイベント
                                                eventdatabase.GirlLove_SubEventMethod();
                                            }
                                            else
                                            {
                                                //時間イベントの発生をチェック。
                                                if (!GameMgr.check_GirlLoveTimeEvent_flag)
                                                {
                                                    Debug.Log("時間イベントチェックON");

                                                    //時間イベント　ヒカリが採取から帰ってくるのチェックもここ。
                                                    eventdatabase.GirlLove_SubTimeEventMethod();
                                                }
                                                else
                                                {
                                                    //スターに応じて、エリア解禁をするチェック　寝るの最後にチェックをONにする
                                                    if (!GameMgr.NewAreaRelease_flag)
                                                    {
                                                        //Debug.Log("スターイベント＆解禁チェック");
                                                        Check_NewAreaFlag();
                                                        //GameMgr.NewAreaRelease_flag = true;
                                                    }
                                                    else
                                                    {
                                                        //読んでいないレシピがあれば、読む処理。優先順位二番目。
                                                        if (!check_recipi_flag)
                                                        {

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
                            }
                        }
                    }
                }
            }
        }
    }

    void TutorialEvent()
    {

        Touch_ALLOFF();
        //girl1_status.HukidashiFlag = false;

        switch (GameMgr.tutorial_Num)
        {
            case 0: //最初にシナリオを読み始める。

                canvas.SetActive(false);

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();

                girl1_status.DeleteHukidashiOnly();
                //girl1_status.Girl_Full();
                girl1_status.Girl1_Status_Init();
                //girl1_status.OkashiNew_Status = 1;
                GameMgr.tutorial_Num = 1; //退避
                break;

            case 10: //宴ポーズ。エクストリームパネルを押そう！で、待機。

                canvas.SetActive(true);
                special_quest.RedrawQuestName();

                MainCompoundMethod();
                compoundselect_onoff_obj.SetActive(false);
                OffCompoundSelectnoExtreme();
                //extreme_Button.interactable = true;

                _textmain.text = "左の「お菓子パネル」を押してみよう！";
                break;

            case 15: //はじめてパネルを開いた。オリジナル調合を押そう！

                MainCompoundMethod();

                mainUI_panel_obj.SetActive(false);
                //compoundselect_onoff_obj.SetActive(false);
                text_area_compound.SetActive(false);

                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                compoBGA_image.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageHikariMake.GetComponent<Image>().raycastTarget = false;

                Extremepanel_obj.SetActive(false);

                select_original_button.interactable = false;
                select_recipi_button.interactable = false;
                select_extreme_button.interactable = false;
                select_no_button.interactable = false;

                break;

            case 16: //メッセージおわり

                MainCompoundMethod();

                mainUI_panel_obj.SetActive(false);
                //compoundselect_onoff_obj.SetActive(false);
                text_area_compound.SetActive(false);

                compoBG_A.GetComponent<GraphicRaycaster>().enabled = true;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = true;
                compoBGA_image.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageHikariMake.GetComponent<Image>().raycastTarget = false;

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

                mainUI_panel_obj.SetActive(false);
                //compoundselect_onoff_obj.SetActive(false);
                text_area_compound.SetActive(false);

                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                compoBGA_image.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageOri.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = false;
                compoBGA_imageHikariMake.GetComponent<Image>().raycastTarget = false;
                pitemlistController.Offinteract();

                recipiMemoButton.GetComponent<Button>().interactable = false;

                break;

            case 30: //宴がポーズ状態。右上のレシピメモを押そう。

                recipiMemoButton.GetComponent<Button>().interactable = true;

                text_area_compound.SetActive(true);
                _textcomp.text = "右上の「レシピをみる」ボタンを押して" + "\n" + "クッキーを作ってみよう！";
                break;

            case 40: //メモ画面を開いた。

                text_area_compound.SetActive(false);
                break;

            case 50: //宴ポーズ。オリジナル調合をしてみるところ。

                pitemlistController.Oninteract();
                text_area_compound.SetActive(true);
                _textcomp.text = "左のリストから、" + "\n" + "好きな材料を" + GameMgr.ColorYellow + "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。"; ;

                GameMgr.tutorial_Num = 55;
                break;

            case 55: //調合中
                break;

            case 60: //調合完了！

                text_area_compound.SetActive(false);

                break;

            case 70: //宴ポーズ。やったね！クッキーができた～から、レシピを閃き、ボタンを押し待ち。

                card_view.SetinteractiveOn();
                text_area_compound.SetActive(true);
                GameMgr.tutorial_Num = 75; //退避
                break;

            case 75:

                text_area_compound.SetActive(true);
                _textcomp.text = "カードを押してみよう！";
                break;

            case 80: //ボタンを押し、元の画面に戻る。

                MainCompoundMethod();

                canvas.SetActive(false);

                break;

            case 90: //「あげる」ボタンを押すところ。「あげる」のみをON、他のボタンはオフ。

                //Debug.Log("GameMgr.チュートリアルNo: " + GameMgr.tutorial_Num);
                MainCompoundMethod();

                //compoundselect_onoff_obj.SetActive(false);
                OffCompoundSelect();
                text_area_compound.SetActive(false);

                //girl1_status.Girl_EatDecide();
                girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                GameMgr.tutorial_Num = 95; //退避

                break;

            case 100:

                MainCompoundMethod();
                canvas.SetActive(true);
                compoundselect_onoff_obj.SetActive(true);
                //special_quest.RedrawQuestName();

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

                //girl1_status.Girl_EatDecide();
                girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                GameMgr.tutorial_Num = 130;

                GameMgr.tutorial_Progress = true;
                break;

            case 130:

                text_area_compound.SetActive(false);
                break;

            case 140:

                //一回機嫌はリセットする
                girl1_status.GirlExpressionKoushin(50);
                MainCompoundMethod();
                girl1_status.DefFaceChange();

                extreme_Button.interactable = true;
                canvas.SetActive(true);

                _textmain.text = "ねこクッキーを作ってみよう！";

                break;

            case 150: //レシピボタンでも～を説明中。ボタンは押せないようにしておく。

                MainCompoundMethod();

                mainUI_panel_obj.SetActive(false);

                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                select_original_button.interactable = false;
                select_extreme_button.interactable = false;
                select_recipi_button.interactable = false;
                select_no_button.interactable = false;

                text_area_compound.SetActive(false);
                break;

            case 160:

                MainCompoundMethod();

                mainUI_panel_obj.SetActive(false);
                compoBG_A.GetComponent<GraphicRaycaster>().enabled = true;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = true;
                select_recipi_button.interactable = true;
                text_area_compound.SetActive(true);

                GameMgr.tutorial_Num = 165;
                break;

            case 165: //レシピ調合中

                MainCompoundMethod();
                mainUI_panel_obj.SetActive(false);
                break;

            case 170: //れしぴ調合完了！

                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                text_area_compound.SetActive(false);
                break;

            case 180:

                card_view.SetinteractiveOn();

                text_area_compound.SetActive(true);
                _textcomp.text = "カードを押してみよう！";
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

                mainUI_panel_obj.SetActive(false);
                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                select_original_button.interactable = false;
                select_extreme_button.interactable = true;
                select_recipi_button.interactable = false;
                select_no_button.interactable = false;

                text_area_compound.SetActive(false);
                break;

            case 220:

                compoBG_A.GetComponent<GraphicRaycaster>().enabled = true;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = true;
                //MainCompoundMethod();
                text_area_compound.SetActive(true);

                _textcomp.text = "「仕上げ」ボタンを押してみよう！";

                break;

            case 230:

                MainCompoundMethod();

                mainUI_panel_obj.SetActive(false);
                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                text_area_compound.SetActive(false);
                pitemlistController.Offinteract();

                break;

            case 240:

                MainCompoundMethod();

                mainUI_panel_obj.SetActive(false);
                compoBG_A.GetComponent<GraphicRaycaster>().enabled = false;
                selectPanel_1.GetComponent<GraphicRaycaster>().enabled = false;
                text_area_compound.SetActive(true);
                pitemlistController.Oninteract();

                GameMgr.tutorial_Num = 245; //退避

                break;

            case 245:

                text_area_compound.SetActive(true);
                break;


            case 250:

                text_area_compound.SetActive(false);
                break;

            case 260:

                card_view.SetinteractiveOn();

                text_area_compound.SetActive(true);
                _textcomp.text = "カードを押してみよう！";

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
                //OffCompoundSelect();
                //girleat_toggle.GetComponent<Toggle>().interactable = true;
                girl1_status.timeGirl_hungry_status = 1;

                _textmain.text = "お菓子をあげてみよう！";

                GameMgr.tutorial_Num = 285; //退避
                break;

            case 285:

                MainCompoundMethod();

                OffCompoundSelect();
                girleat_toggle.GetComponent<Toggle>().interactable = true;

                break;

            case 290:

                MainCompoundMethod();
                girl1_status.DeleteHukidashiOnly();
                canvas.SetActive(false);

                girl1_status.Girl1_Status_Init2(); //腹減り状態=0にして、Timeoutが0.5s　すぐに電球出る状態
                girl1_status.GirlExpressionKoushin(50); //機嫌は元に戻す

                break;

            default:

                break;
        }

    }

    void GetMatReturnCheck()
    {
        //リザルトパネルを表示する
        getmatplace.ResultPanelOn();

        //お外いきたかったら、このタイミングで、ハートボーナスがもらえる。
        if (GameMgr.OsotoIkitaiFlag)
        {
            if (!GameMgr.outgirl_Nowprogress) 
            {
                GameMgr.OsotoIkitaiFlag = false;
                girlEat_judge.loveGetPlusAnimeON(5, false);
                _textmain.text = "お外にいって、喜んだようだ。";
                girl1_status.GirlExpressionKoushin(20);
            }
            else //ヒカリがいないときは発生しない
            {
                GameMgr.OsotoIkitaiFlag = false;
                _textmain.text = "家に戻ってきた。どうしようかなぁ？";
            }
        }
        else
        {
            _textmain.text = "家に戻ってきた。どうしようかなぁ？";
        }
        //ハートゲージを更新。
        HeartGuageTextKoushin();

        //オートセーブ
        if (GameMgr.AUTOSAVE_ON)
        {
            save_controller.OnSaveMethod(GameMgr.System_save_nowslot);
            Debug.Log("オートセーブ完了");

            AutoSaveCompleteText();
        }
    }

    void OsotoIttazoCheck()
    {
        if (!GameMgr.outgirl_Nowprogress)
        {
            girlEat_judge.loveGetPlusAnimeON(5, false);
            girl1_status.GirlExpressionKoushin(20);

            if (GameMgr.OsotoIttazoPlace == "SodaIsland")
            {
                _textmain.text = "遊園地で遊んで、満足しているようだ。";
            }
            else if (GameMgr.OsotoIttazoPlace == "RotenStreet")
            {
                _textmain.text = "屋台で遊んで、喜んだようだ。";
            }
            else
            {
                _textmain.text = "お外にいって、喜んだようだ。";
            }
        }

        //ハートゲージを更新。
        HeartGuageTextKoushin();
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

                    compoBG_A.GetComponent<GraphicRaycaster>().enabled = true;
                    selectPanel_1.GetComponent<GraphicRaycaster>().enabled = true;
                    compoBGA_image.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageOri.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageRecipi.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageExtreme.GetComponent<Image>().raycastTarget = true;
                    compoBGA_imageHikariMake.GetComponent<Image>().raycastTarget = true;
                    GameMgr.scenario_read_endflag = false;
                    
                    //keymanager.InitCompoundMainScene();
                }

                mainUI_panel_obj.SetActive(true);
                BGImageTemaePanel.SetActive(true);
                //GetMatStatusButton_obj.SetActive(false);
                recipilist_onoff.SetActive(false);
                playeritemlist_onoff.SetActive(false);
                yes_no_panel.SetActive(false);
                getmatplace_panel.SetActive(false);               
                black_panel_A.SetActive(false);
                ResultBGimage.SetActive(false);
                compoBG_A.SetActive(false);
                system_panel.SetActive(false);
                status_panel.SetActive(false);
                okashihint_panel.SetActive(false);
                recipiMemoButton.SetActive(false);
                GameMgr.OnCatGetMaterial_modeON = false; //ねこ採取画面のモードはここでリセット
                GameMgr.check_StarPanel_Endflag = false; //ここまでくると、必ずスターパネルはチェックしたことになる。（スター発生していなくてもチェックは完了）

                WindowOn();                
                select_original_button.interactable = true;
                select_recipi_button.interactable = true;
                select_no_button.interactable = true;
                               
                OnCompoundSelect();

                if (GameMgr.System_CatAutoMaterial_ON)
                {
                    //ねこチェック　ねこの採取フラグが必要か否か
                    GameMgr.catGetMat_PlayFlag = catDataBase.Check_CatGotoFlag();
                }

                //エクストリームパネル表示更新
                GameMgr.extremepanel_Koushin = true;

                //ステージ更新
                mainUI_panel_obj.GetComponent<MainUIPanel>().StageNumKoushin();

                //満腹度更新
                ManpukuKoushin();

                //機嫌表示更新
                GirlExpressionKoushinHyouji();

                //装備品アイテムの効果計算
                bufpower_keisan.CheckEquip_Keisan();

                //家賃日までの日数計算と金額設定
                yachinPanel.GetComponent<YachinPanel>().Setting_CullentYachin();
                yachinPanel.GetComponent<YachinPanel>().YachinHyouji();

                //覚えたスキルやステータスを毎回チェックし、ぬけがないか更新。
                exp_table.SkillCheckHeartLV(PlayerStatus.girl1_Love_maxlv, 0); //2番目が0で、実際のスキルの更新

                //スターによって解放されるスキルがないかチェック
                exp_table.StarLVCheck();

                //魔法一番使ってるものをここでチェック
                GameMgr.MagicSkill_TopUseName = magicskill_database.Count_TopUseMagicSkill();

                //メインクエのメッセージ更新
                gameQuestPanel_Panel.SetActive(true);
                gameQuestPanel.GetComponent<GameQuestPanel>().TextKoushin();


                //
                //アニメーション、キャラの表情関係
                //
                //Live2Dデフォルト
                //ReSetLive2DOrder_Default();
                //Anchor_Pos.transform.localPosition = new Vector3(0f, 0.134f, -5f);
                girl1_status.HukidashiFlag = true;
                girl1_status.tween_start = false;
                girl1_status.IdleMotionReset(0); // 0は即時切り替え

                //コンテスト・クエストの締め切りチェック（ビックリマークの表示）
                mainUI_panel_obj.transform.Find("QuestKakuninButtonPanel").GetComponent<QuestKakuninButtonPanel>().Check_LimitMarkDraw();
                mainUI_panel_obj.transform.Find("ContestKakuninButtonPanel").GetComponent<ContestKakuninButtonPanel>().Check_LimitMarkDraw();

                //お天気チェック
                Weather_Change();

                //外出時のキャラクタ表示処理
                if (!GameMgr.outgirl_Nowprogress)
                {
                    CharacterLive2DImageON();

                    if (GameMgr.CompoAfter_BackGirl) //戻り中の間はタッチはできない　girl1_status内でもUpdateでオフにしている。効力強い。
                    {
                        Touch_ALLOFF();
                    }
                    else
                    {
                        Touch_ALLON();
                    }

                    //sleep_toggle.GetComponent<Toggle>().interactable = true;
                    if (GameMgr.QuestClearflag)
                    {
                        stageclear_Button.GetComponent<Toggle>().interactable = true;
                    }

                }
                else //外出中はLive2Dをオフに。
                {
                    CharacterLive2DImageOFF();
                    Touch_ALLOFF();
                    girleat_toggle.GetComponent<Toggle>().interactable = false;
                    //sleep_toggle.GetComponent<Toggle>().interactable = false;
                    if (GameMgr.QuestClearflag)
                    {
                        stageclear_Button.GetComponent<Toggle>().interactable = false;
                    }
                }

                //音関係
                if (!GameMgr.tutorial_ON)
                {
                    if (GameMgr.matbgm_change_flag == true)
                    {
                        GameMgr.matbgm_change_flag = false;

                        sceneBGM.OnMainBGM(); //メインBGMを変更　ハートレベルに応じてBGMも切り替わる。
                    }
                }

                if (!GameMgr.Contest_afterHomeEventFlag) //コンテストから帰ってきた直後はBGMはオフのまま
                {
                    sceneBGM.NowFadeVolumeONBGM();
                    sceneBGM.MuteOFFBGM();
                    map_ambience.MuteOFF();
                    
                }

                //イベントに応じてコマンドを増やす関係
                FlagEvent();


                //時間更新＆寝るイベントチェック
                if (GameMgr.ResultOFF) //リザルト画面開き中のときは、タイムチェックしない
                { }
                else
                {
                    if (!GameMgr.ReadGirlLoveTimeEvent_reading_now) //ヒカリが外出から帰ってきて、採取パネルやほめるイベントを読み中　全て終わったらfalseになる。
                    {
                        Debug.Log("時間更新チェック＆寝るチェック");
                        time_controller.TimeKoushin(0, true); //時間の更新&寝るイベントのチェック　寝るチェック後に、エリア解禁チェックフラグを入れる 
                    }
                }


                //最初の一回だけ、吹き出しアニメスタート。
                if (!subevent_after_end)
                {
                    if (GameMgr.tutorial_ON != true)
                    {
                        if (girl1_status.special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート。それまでは他のボタン入力できない。
                        {
                            mainUI_panel_obj.SetActive(false);
                            Touch_ALLOFF();
                        }
                    }

                }   
                
                //お外いきたい言ったときに、屋台やソーダアイランドに行くと、帰ってきたときにハートが上がる処理のチェック
                if(GameMgr.OsotoIttazoFlag)
                {
                    GameMgr.OsotoIttazoFlag = false;

                    OsotoIttazoCheck();
                }

                //家に帰ってきたときにイベントが発生するフラグ系で、何度も発生するやつはフラグをリセット
                GameMgr.CompoundEvent_readend[130] = false; //何度でも発生するように、ここでまた読みフラグをリセットする


                //調合成功後に、サブイベントチェック。ちなみに、このcompoundstatus=0の最後にいれないと、作った後のサブイベント発生はバグるので注意。
                if (!GameMgr.tutorial_ON)
                {
                    if (GameMgr.check_CompoAfter_flag)
                    {
                        GameMgr.check_CompoAfter_flag = false;

                        Debug.Log("調合後に、サブイベントチェック入る");
                        GameMgr.check_CompoAfter_SubEventflag = true;
                        GameMgr.check_GirlLoveSubEvent_flag = false; //イベントチェック
                        GameMgr.check_GirlLoveTimeEvent_flag = false; //時間イベントもチェック
                    }
                }

                //お菓子以外で、条件を満たしていないかクエストクリアチェック
                if (girl1_status.special_animatFirst) //SPアニメ終わったあとにチェック
                {
                    //全てのイベントが終わったあとに、ここをチェックする。
                    if (!GameMgr.Sleep_CheckEnd) //寝るチェックが終了
                    {
                        if (GameMgr.check_GirlLoveSubEvent_flag && GameMgr.check_GirlLoveTimeEvent_flag)
                        {
                            if (GameMgr.ResultOFF) //リザルト画面開き中のときは、タイムチェックしない
                            {
                            }
                            else
                            {
                                //お菓子以外で、条件を満たしていないかクエストクリアチェック
                                girlEat_judge.ExtraSPQuestClearCheck();
                            }
                        }
                    }

                }

                GameMgr.Status_zero_readOK = true;

                break;

            case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。                

                break;

            case 2: //エクストリーム調合の処理を開始。クリック後に処理が始まる。
               
                break;

            case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。
               
                break;

            case 4: //調合シーンに入ってますよ、というフラグ。各ケース処理後、必ずこの中の処理に移行する。yes, noボタンを押されるまでは、待つ状態に入る。

                break;

            case 5: //「焼く」を選択

                break;

            case 6: //各種調合の選択画面エントランス

                //GameMgr.CompoundSceneStartON = true; //調合シーンに入っています、というフラグ開始。処理をCompoundMainControllerオブジェに移す。

                //ヒカリちゃんを表示しない。デフォルト描画順
                //SetLive2DPos_Compound();
                //ReSetLive2DOrder_Default();
                map_ambience.Mute();

                girlEat_judge.ListLVUPClear();
                StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
                
                break;


            case 7: //ヒカリが作るを開始
                
                break;

            case 8: //ヒカリお菓子作りのスタートパネルを開く               
               
                break;

            case 10: //「あげる」を選択

                GameMgr.compound_status = 13; //あげるシーンに入っています、というフラグ
                GameMgr.compound_select = 13; //あげるを選択

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);                

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

                text_area.SetActive(true);
                WindowOff();

                card_view.PresentGirl(2, 0);
                StartCoroutine("Girl_present_Final_select");


                break;

            case 11: //お菓子をあげたあとの処理。女の子が、お菓子を判定

                GameMgr.compound_status = 12;
                text_area.SetActive(false);
                GameMgr.girlEat_ON = true; //お菓子判定中フラグ
                character_move.transform.position = new Vector3(0, 0, 0);

                //お菓子の判定処理を起動。引数は、決定したアイテムのアイテムIDと、店売りかオリジナルで制作したアイテムかの、判定用ナンバー 0or1 1=コンテストのとき
                girlEat_judge.Girleat_Judge_method(0, 2, 0);
                GameMgr.extremepanel_Koushin = true; //食べたので、パネルのお菓子は消える。

                break;

            case 12: //お菓子を判定中

                break;

            case 13: //あげるかあげないかを選択中

                break;

            case 20: //材料採取地を選択中

                GameMgr.compound_status = 21; //
                GameMgr.compound_select = 20; //

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                Touch_ALLOFF();

                //おいしそ～状態は、元に戻る。
                if (girl1_status.GirlOishiso_Status == 1)
                {
                    girl1_status.GirlOishiso_Status = 0;
                }

                //UI OFF
                WindowOff();

                text_area.SetActive(true);
                //text_area.GetComponent<MessageWindow>().DrawIcon();
                //moneystatus_panel.SetActive(true);
                //GetMatStatusButton_obj.SetActive(true);

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

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

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();

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

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                //text_area.SetActive(true);
                WindowOff();
                //black_panel_A.SetActive(true);
                StartCoroutine("Contest_Final_select");
                break;

            case 41: //クリアするかどうか、選択中
                break;

            case 42: //次のお菓子へ進むかを選択

                GameMgr.compound_status = 43; //次のお菓子へ進むかシーンに入っています、というフラグ
                GameMgr.compound_select = 40;

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                text_area.SetActive(true);
                WindowOff();
                black_panel_A.SetActive(true);
                StartCoroutine("OkashiClear_Final_select");
                break;

            case 43: //次のお菓子へ進むかどうか、選択中
                break;

            case 50: //寝るを選択

                GameMgr.compound_status = 51; //寝るシーンに入っています、というフラグ
                GameMgr.compound_select = 50; //寝るを選択

                yes_no_sleep_panel.SetActive(true);

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止                

                WindowOff();

                text_area.SetActive(true);

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

                girlEat_judge.ListLVUPClear();

                WindowOff();
                break;

            case 61: //レシピ本選択中

                break;

            case 99: //アイテム画面を開いたとき
                
                black_panel_A.SetActive(true);
                GameMgr.compound_status = 99;
                GameMgr.compound_select = 99;
                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。

                girlEat_judge.ListLVUPClear();

                WindowOff();

                break;

            case 100: //退避用 トグル選択中のときなどに使用

                break;            

            case 110: //調合、最後これでよいか選択中のステータス　status=0が全て読み終わった後のデフォルトの待機状態もここ。

                break;

            case 120: //なんらかの選択で、最後これでいいかどうかの確認中

                break;

            case 130: //クエスト確認パネル開いてる最中

                girlEat_judge.ListLVUPClear();
                break;

            case 140: //コンテスト確認パネル開いてる最中

                girlEat_judge.ListLVUPClear();
                break;

            case 150: //スターパネル開いてる最中

                girlEat_judge.ListLVUPClear();
                break;

            case 200: //システム画面を開いたとき

                GameMgr.compound_status = 201;
                GameMgr.compound_select = 200;

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                system_panel.SetActive(true); //システムパネルを表示。

                WindowOff();

                break;

            case 201: //システム画面選択中

                break;

            case 250: //お菓子ヒント画面を開いたとき

                GameMgr.compound_status = 251;
                GameMgr.compound_select = 250;

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                okashihint_panel.SetActive(true); //お菓子ヒントパネルを表示。

                WindowOff();

                break;

            case 251: //お菓子ヒント画面選択中

                break;

            case 300: //ステータス画面を開いたとき

                GameMgr.compound_status = 301;
                GameMgr.compound_select = 300;

                //腹減りカウント一時停止
                girl1_status.GirlEatJudgecounter_OFF();
                girlEat_judge.ListLVUPClear();

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

                break;

            default:
                break;
        }
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
        //Stagepanel_obj.SetActive(false);
        quest_kakuninButton_obj.SetActive(false);
        contest_kakuninButton_obj.SetActive(false);
        starPanel_kakuninButton_obj.SetActive(false);
        gameQuestPanel_Panel.SetActive(false);
        yachinPanel.SetActive(false);

        stageclear_panel.SetActive(false);        
        hinttaste_toggle.SetActive(false);
        //girleat_toggle.SetActive(false);
        recipi_toggle.SetActive(false);
        HintObjectGroup.SetActive(false);

        mainlist_scrollview_obj.SetActive(false);
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
        quest_kakuninButton_obj.SetActive(true);
        contest_kakuninButton_obj.SetActive(true);
        starPanel_kakuninButton_obj.SetActive(true);
        gameQuestPanel_Panel.SetActive(true);
        yachinPanel.SetActive(true);

        //Stagepanel_obj.SetActive(true);

        OuthomePanelONOFF();

        if (GameMgr.Story_Mode == 1)
        {
            if (GameMgr.System_Manpuku_ON)
            {
                manpuku_bar.SetActive(true);
            }
            else
            {
                manpuku_bar.SetActive(false);
            }
        }

        //girleat_toggle.SetActive(true);
        recipi_toggle.SetActive(true);
        HintObjectGroup.SetActive(true);

        //ただし、ハートを取得しまだ残ってる場合は、ゲージはONのままにしておく。
        if (girlEat_judge.heart_count > 0)
        {
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

        if (pitemlist.player_extremepanel_itemlist.Count > 0)
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

    void OuthomePanelONOFF()
    {
        if (GameMgr.OutEntrance_ON)
        {
            switch (GameMgr.Scene_Name)
            {
                case "Compound":

                    mainlist_scrollview_obj.SetActive(false);
                    break;

                case "Or_Compound":

                    mainlist_scrollview_obj.SetActive(true);
                    //mainlist_scrollview_obj.SetActive(false);
                    break;

                default:

                    mainlist_scrollview_obj.SetActive(false);
                    break;
            }
        }
        else
        {
            mainlist_scrollview_obj.SetActive(false);
        }
    }

    public void QuestClearCheck() //SaveControllerからも読み込んでいる。
    {
        stageclear_panel.SetActive(false);

        //小クエストをクリアしたら、ステージ最後にクリアボタンがでる。
        if (GameMgr.Contest_PanelON)
        {
            //stageclear_toggle.SetActive(true);
            stageclear_panel.SetActive(true);
            stageclear_Button.SetActive(true);
            if (!GameMgr.QuestClearflag)
            {
                stageclear_button_text.text = "コンテストへ";
            }
            else
            {
                stageclear_button_text.text = "お話をすすめる";
            }

            if (GameMgr.outgirl_Nowprogress)
            {
                stageclear_button_toggle.interactable = false;
            }
            else
            {
                stageclear_button_toggle.interactable = true;
            }
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

    public void OnCheck_Recipi() //レシピをON
    {
        if (recipi_toggle.GetComponent<Toggle>().isOn == true)
        {
            recipi_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            GameMgr.compound_status = 60;
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
        text_area_compound.SetActive(true);
        yes_no_panel.SetActive(true);
        yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

        GameMgr.compound_select = 120;
        if (pitemlist.player_extremepanel_itemlist.Count > 0)
        {
            _textcomp.text = "このお菓子でいく？";

            extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

            Debug.Log("このお菓子でいく？");
            card_view.PresentGirl(2, 0);
            StartCoroutine("PicnicItem_Final_select");
        }
        else
        {
            _textcomp.text = "今は、何のお菓子も選択されていないよ。" + "\n" + "やっぱりやめる？";
            StartCoroutine("PicnicItem_Cancel_Final_select");
        }
    }

    public void OnCancelPicnic_Select()
    {
        //ピクニックイベント中、お菓子制作中は更新する。
        black_panel_A.SetActive(true);
        text_area_compound.SetActive(true);
        yes_no_panel.SetActive(true);
        yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);
        GameMgr.compound_select = 120;

        _textcomp.text = "やっぱりやめる？";
        StartCoroutine("PicnicItem_Cancel_Final_select");
    }

    IEnumerator PicnicItem_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        text_area_compound.SetActive(false);
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
                GameMgr.event_kettei_itemID = 0;
                GameMgr.event_kettei_item_Type = 2;
                //GameMgr.event_kettei_item_Kosu = updown_counter.updown_kosu; //最終個数を入れる。
                GameMgr.event_kettei_item_Kosu = 1;

                GameMgr.CompoundSceneStartON = false;　//ピクニックの調合シーンは終了
                Debug.Log("ピクニックの調合シーン終了");
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

        text_area_compound.SetActive(false);
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

                GameMgr.CompoundSceneStartON = false;　//ピクニックの調合シーンは終了
                Debug.Log("ピクニックの調合シーン終了");
                break;

            case false:

                Debug.Log("cancel");

                //GameMgr.compound_status = 6;

                yes_selectitem_kettei.onclick = false;

                break;

        }
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

            switch(GameMgr.Scene_Name)
            {
                case "Compound":

                    OnGetMatPanel(); //採取地画面を直接開く
                    break;

                case "Or_Compound":

                    OnGetMatPanel(); //採取地画面を直接開く

                    //玄関音　外に一度出るパターン
                    //sc.EnterSound_01();
                    //GameMgr.SceneSelectNum = 0;
                    //FadeManager.Instance.LoadScene("Or_Compound_Enterance", GameMgr.SceneFadeTime);
                    break;

                default:

                    OnGetMatPanel(); //採取地画面を直接開く
                    break;
            }       
        }
    }


    void OnGetMatPanel()
    {
        card_view.DeleteCard_DrawView();

        //ウィンドウキャラ名設定
        if (PlayerStatus.player_cullent_hour >= GameMgr.NightDay_hour) //19時こえたとき　もう外にでれない
        {
            if (!GameMgr.outgirl_Nowprogress)
            {
                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                _text.text = "にいちゃん！　もう遅いからやめとこ～。";
            }
            else
            {
                GameMgr.Window_CharaName = GameMgr.player_Name_First;
                _text.text = "さすがに夜だし、やめとこうかな。";
            }
        }
        else
        {
            if (!GameMgr.outgirl_Nowprogress)
            {
                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                _text.text = "にいちゃん！　お外、どこに行く～？";
            }
            else
            {
                GameMgr.Window_CharaName = GameMgr.player_Name_First;
                _text.text = "どこに行こうかな？";
            }
        }
        
        GameMgr.compound_status = 20;

        StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。        

        //BGMを変更
        if (GameMgr.GetMatBGMCHANGE_ON)
        {
            sceneBGM.OnGetMatStartBGM();
            map_ambience.Mute();
            GameMgr.matbgm_change_flag = true;
        }

        //音ならす
        //sc.PlaySe(36);

        getmatplace.SetInit();
        getmatplace_panel.SetActive(true);
        //yes_no_panel.SetActive(true);
        //yes_no_panel.transform.Find("Yes").gameObject.SetActive(false);

        moneystatus_panel.SetActive(false);
        TimePanel_obj1.SetActive(false);
        BGImageTemaePanel.SetActive(false);
        //TimePanel_obj2.SetActive(true);

        //お金のアニメはすぐ止めて表記更新
        moneyStatus_Controller.DrawMoneyHyouji();
    }

    public void OnOutHome_toggle() //玄関からアトリエの外へでる
    {


        switch (GameMgr.Scene_Name)
        {
            case "Compound":

                OnGetMatPanel(); //採取地画面を直接開く
                break;

            case "Or_Compound":

                //OnGetMatPanel(); //採取地画面を直接開く

                //玄関音　外に一度出るパターン
                sc.EnterSound_01();
                GameMgr.SceneSelectNum = 0;
                FadeManager.Instance.LoadScene("Or_Compound_Enterance", GameMgr.SceneFadeTime);
                break;

            default:

                OnGetMatPanel(); //採取地画面を直接開く
                break;
        }

    }

    public void OnGirlEat() //女の子にお菓子をあげる
    {
        if (girleat_toggle.GetComponent<Toggle>().isOn == true)
        {
            girleat_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            card_view.DeleteCard_DrawView();

            if (pitemlist.player_extremepanel_itemlist.Count > 0)
            {
                GameMgr.Window_CharaName = "";
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

            //受注クエストの個人依頼をみて、当日かどうかをチェックする。
            _teishutu_on = quest_database.CheckKojinQuest_ToDay();

            if (_teishutu_on) //個人依頼の当日
            {
                if (!GameMgr.outgirl_Nowprogress)
                {
                    GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                    _text.text = "にいちゃん！　きょうは、大事なお仕事のしめきり！" + "\n" + "お客さんがくるまで寝る？"; //
                }
                else
                {
                    GameMgr.Window_CharaName = GameMgr.player_Name_First;
                    _text.text = "きょうは、大事なお仕事の締め切りだったっけ。" + "\n" + "お客さんがくるまで寝る？"; //
                }
            }
            else
            {
                if (!GameMgr.outgirl_Nowprogress)
                {
                    GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                    _text.text = "今日はもう寝る？"; //
                }
                else
                {
                    GameMgr.Window_CharaName = GameMgr.player_Name_First;
                    _text.text = "ヒカリが戻ってくるまで寝る？"; //
                }
            }

            text_area.SetActive(true);
            text_area.GetComponent<MessageWindow>().CharaNameON();

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

            if (GameMgr.Contest_PanelON)
            {
                if (!GameMgr.QuestClearflag)
                {
                    if (PlayerStatus.player_cullent_hour >= GameMgr.NightDay_hour) //20時をこえるかどうか。
                    {
                        text_area.SetActive(true);
                        black_panel_A.SetActive(true);

                        if (GameMgr.outgirl_Nowprogress)
                        {
                            GameMgr.Window_CharaName = GameMgr.player_Name_First;
                            _text.text = "時間が遅くなりそうだ..。今日はやめておこう。";
                        }
                        else
                        {
                            GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                            _text.text = "今日はもう遅いからやめとこ～..。";
                        }
                        GameMgr.compound_status = 40;
                        yes_no_clear_panel.SetActive(true);
                        yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Button>().interactable = false;
                        yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Sound_Trigger>().enabled = false;
                    }
                    else
                    {
                        GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                        _text.text = "コンテストに出るの？";
                        GameMgr.compound_status = 40;
                        contest_CheckPanel_obj.SetActive(true);

                        /*yes_no_clear_panel.SetActive(true);
                        yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Button>().interactable = true;
                        yes_no_clear_panel.transform.Find("Yes_Clear").GetComponent<Sound_Trigger>().enabled = true;*/
                    }
                }
                else
                {
                    text_area.SetActive(true);
                    black_panel_A.SetActive(true);

                    GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                    _text.text = "次のお話にすすむ？　おにいちゃん。";

                    GameMgr.compound_status = 42;
                    yes_no_clear_okashi_panel.SetActive(true);
                }
            }
            else
            {
                text_area.SetActive(true);
                black_panel_A.SetActive(true);

                if (GameMgr.QuestClearflag)
                {
                    _text.text = "次のお話にすすむ？　おにいちゃん。";

                    GameMgr.compound_status = 42;
                    yes_no_clear_okashi_panel.SetActive(true);

                }
            }

        }
    }

    public void OnQuestCheck_button() //クエスト確認　ボタンで押した場合
    {
        GameMgr.compound_status = 130;

        quest_CheckPanel_obj.SetActive(true);
        quest_CheckPanel_obj.transform.Find("PanelB").gameObject.SetActive(false);

        StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
    }

    public void OnContestGoCheck_button() //コンテストの受注確認　ボタンで押した場合
    {
        GameMgr.compound_status = 140;

        contest_CheckPanel_obj.SetActive(true);

        StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
    }

    public void OnStarPanelOpen_button() //スターパネルを開いた　ボタンで押した場合
    {
        GameMgr.compound_status = 150;

        starPanel_obj.SetActive(true);

        StartMessage(); //メインのほうも、デフォルトメッセージに戻しておく。
    }








    //レシピを見たときの処理。recipiitemselecttoggleから呼ばれる。
    public void eventRecipi_ON()
    {
        recipilist_onoff.SetActive(false);
        Debug.Log("イベントレシピID: " + event_itemID + "　レシピ名: " + pitemlist.eventitemlist[event_itemID].event_itemNameHyouji);

        compoundselect_onoff_obj.SetActive(false);
        text_area.SetActive(false);
        text_area_Main.SetActive(false);
        black_panel_A.GetComponent<Image>().raycastTarget = false;
        //yes_no_panel.SetActive(false);

        //腹減りカウント一時停止
        girl1_status.GirlEatJudgecounter_OFF();
        girl1_status.hukidasiOff(); //ふきだしオフ

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

        girl1_status.GirlEat_Judge_on = true;
        girl1_status.hukidasiOn(); //ふきだしオン

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
        Touch_ALLOFF();
        compoundselect_onoff_obj.SetActive(false);
        Extremepanel_obj.SetActive(false);
        text_area.SetActive(false);
        text_area_Main.SetActive(false);

        //腹減りカウント一時停止
        girl1_status.GirlEatJudgecounter_OFF();

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

        //終わったら再開
        girl1_status.GirlEat_Judge_on = true;

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

    //レシピの番号チェック。ゲットしたアイテム以外に、コンポ調合アイテムを解禁し、レシピリストに表示されるようにする。
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

            case "recipibook_7": //クレープの秘訣　ゲットすると、基本クレープレシピも自動で追加される。

                ev_id = pitemlist.Find_eventitemdatabase("crepe_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ナジャの基本のレシピを追加

                break;

            case "recipibook_11": //クレープのレシピ大全　ゲットすると、基本クレープレシピも自動で追加される。

                ev_id = pitemlist.Find_eventitemdatabase("crepe_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ナジャの基本のレシピを追加

                break;

            case "roll_cookie_recipi": //ロールクッキー　ゲットすると、魔法「ウィンドロール」も自動で追加される。

                magicskill_database.skillHyoujiKaikin("Wind_Roll");
                magicskill_database.skillLearnLv_Name("Wind_Roll", 1);
                break;


            //魔法の本
            case "mg_firstmagic_book": //初心者向けおかし魔法の本

                //magicskill_database.skillHyoujiKaikin("Beautiful_Power");
                magicskill_database.skillHyoujiKaikin("Luminous_Suger");
                magicskill_database.skillHyoujiKaikin("Luminous_Fruits");
                //magicskill_database.skillHyoujiKaikin("Buttelfy_illumination");

                magicskill_database.skillHyoujiKaikin("Cookie_Study");
                magicskill_database.skillHyoujiKaikin("Caramelized");
                magicskill_database.skillHyoujiKaikin("Fire_Flowers");

                //magicskill_database.skillHyoujiKaikin("Heart_of_Icecream");
                //magicskill_database.skillHyoujiKaikin("Freezing_Spell");

                magicskill_database.skillHyoujiKaikin("Appaleil_Study");
                magicskill_database.skillHyoujiKaikin("Wind_Ark");

                break;

            case "mg_chocolatemagic_book":

                magicskill_database.skillHyoujiKaikin("Chocolate_Philosophy");
                magicskill_database.skillHyoujiKaikin("Bake_Beans");
                magicskill_database.skillHyoujiKaikin("Chocolate_Tempering");
                //magicskill_database.skillHyoujiKaikin("Night_Barron");
                break;

            case "mg_windmagic_book": //風の魔術書

                magicskill_database.skillHyoujiKaikin("Wind_Twister");
                magicskill_database.skillHyoujiKaikin("Wind_Heart");
                magicskill_database.skillHyoujiKaikin("Wind_FlatBar");
                magicskill_database.skillHyoujiKaikin("Wind_Crown");
                magicskill_database.skillHyoujiKaikin("Wind_Roll");
                magicskill_database.skillHyoujiKaikin("Wind_Pen");
                magicskill_database.skillHyoujiKaikin("Bubble_Mist");

                break;

            case "mg_starmagic_book": //星の魔術書

                //magicskill_database.skillHyoujiKaikin("Star_Gazer");
                magicskill_database.skillHyoujiKaikin("Soda_Study");
                magicskill_database.skillHyoujiKaikin("Tea_Study");
                magicskill_database.skillHyoujiKaikin("Star_Blessing");
                magicskill_database.skillHyoujiKaikin("Latte_Art");
                magicskill_database.skillHyoujiKaikin("Magic_Soda");

                break;

            case "mg_beautifulpower_book":
                magicskill_database.skillHyoujiKaikin("Beautiful_Power");
                break;

            case "mg_parfect_princess_book":
                magicskill_database.skillHyoujiKaikin("Parfect_Princess");
                break;

            case "mg_buttelfy_illumination_book":

                magicskill_database.skillHyoujiKaikin("Buttelfy_illumination");
                break;

            case "mg_glitter_book":

                magicskill_database.skillHyoujiKaikin("Glitter");
                break;

            case "mg_mp_regenaration_book":

                magicskill_database.skillHyoujiKaikin("MP_Regenaration");
                magicskill_database.skillLearnLv_Name("MP_Regenaration", 1);
                break;

            case "mg_rare_findUP_book":

                magicskill_database.skillHyoujiKaikin("Rare_FindUP");
                magicskill_database.skillLearnLv_Name("Rare_FindUP", 1);
                break;

            case "mg_summon_mirabo_book":

                magicskill_database.skillHyoujiKaikin("Summon_Mirabo");
                break;

            case "mg_secondbake_book":

                magicskill_database.skillHyoujiKaikin("Cookie_SecondBake");
                break;
            
            case "mg_controltempature_book":
                magicskill_database.skillHyoujiKaikin("Temperature_of_Control");
                //magicskill_database.skillLearnLv_Name("Temperature_of_Control", 1); //入手時点でLV1習得すみの状態
                break;

            case "mg_fire_flowers_book":
                magicskill_database.skillHyoujiKaikin("Fire_Flowers");
                break;
                
            case "mg_bake_beans_book":
                magicskill_database.skillHyoujiKaikin("Bake_Beans");
                break;

            case "mg_chocolatetempering_book":
                magicskill_database.skillHyoujiKaikin("Chocolate_Tempering");
                break;

            case "mg_caramelized_book":
                magicskill_database.skillHyoujiKaikin("Caramelized");
                break;
            
            case "mg_heart_of_icecream_book":
                magicskill_database.skillHyoujiKaikin("Heart_of_Icecream");
                break;

            case "mg_freezespell_book":
                magicskill_database.skillHyoujiKaikin("Heart_of_Icecream");
                magicskill_database.skillHyoujiKaikin("Freezing_Spell");
                break;

            case "mg_freezing_overrun_book":
                magicskill_database.skillHyoujiKaikin("Freezing_OverRun");
                break;

            case "mg_sugerpot_book":
                magicskill_database.skillHyoujiKaikin("SugerPot");
                break;

            case "mg_mnemonic_book":
                magicskill_database.skillHyoujiKaikin("Mnemonic");
                break;

            case "mg_nappe_book":
                magicskill_database.skillHyoujiKaikin("Nappe");
                break;

            case "mg_appaleil_study_book":
                magicskill_database.skillHyoujiKaikin("Appaleil_Study");
                break;

            case "mg_windarc_book":
                magicskill_database.skillHyoujiKaikin("Wind_Ark");
                magicskill_database.skillLearnLv_Name("Wind_Ark", 1); //入手時点でLV1習得すみの状態
                break;

            case "mg_windtwister_book":
                magicskill_database.skillHyoujiKaikin("Wind_Twister");
                magicskill_database.skillLearnLv_Name("Wind_Twister", 1); //入手時点でLV1習得すみの状態
                break;

            case "mg_windroll_book":
                magicskill_database.skillHyoujiKaikin("Wind_Roll");
                break;

            case "mg_windpencil_book":
                magicskill_database.skillHyoujiKaikin("Wind_Pen");
                break;

            case "mg_float_material_book":
                magicskill_database.skillHyoujiKaikin("Float_Material");
                break;

            case "mg_bubblemist_book":
                magicskill_database.skillHyoujiKaikin("Bubble_Mist");
                break;

            case "mg_statue_of_material_book":
                magicskill_database.skillHyoujiKaikin("Statue_of_Penguin");
                magicskill_database.skillHyoujiKaikin("Statue_of_Bear");
                magicskill_database.skillHyoujiKaikin("Statue_of_Cat");
                magicskill_database.skillHyoujiKaikin("Statue_of_Rabitts");
                //magicskill_database.skillHyoujiKaikin("Statue_of_AngelWing");
                break;

            case "mg_star_gazer_book":
                magicskill_database.skillHyoujiKaikin("Star_Gazer");
                break;

            case "mg_star_blessing_book":
                magicskill_database.skillHyoujiKaikin("Star_Gazer");
                magicskill_database.skillHyoujiKaikin("Star_Blessing");
                break;

            case "mg_latte_art_book":
                magicskill_database.skillHyoujiKaikin("Latte_Art");
                break;

            case "mg_moonlight_banana_book":
                magicskill_database.skillHyoujiKaikin("Moonlight_Banana");
                break;

            case "mg_magic_soda_book":
                magicskill_database.skillHyoujiKaikin("Magic_Soda");
                break;

            case "mg_rainbowrain_book":
                magicskill_database.skillHyoujiKaikin("Rainbow_Rain");
                break;
            
            case "mg_aromapotion_book":
                magicskill_database.skillHyoujiKaikin("Aroma_Potion");
                magicskill_database.skillLearnLv_Name("Aroma_Potion", 1); //アロマポーションはLV1習得すみの状態
                break;

            case "mg_plant_growth_book":
                magicskill_database.skillHyoujiKaikin("Plant_Growth");
                break;

            case "mg_spring_pharmacy_book":
                magicskill_database.skillHyoujiKaikin("Spring_Pharmacy");
                break;

            case "mg_saint_fleur_book":
                magicskill_database.skillHyoujiKaikin("Saint_Fleur");
                break;

            case "mg_santiman_book":
                magicskill_database.skillHyoujiKaikin("Santiman");
                break;

            case "mg_epiclesis_book":
                magicskill_database.skillHyoujiKaikin("Epiclesis");
                break;

            case "mg_latria_book":
                magicskill_database.skillHyoujiKaikin("Latria");
                break;

            case "mg_three_stars_book":
                magicskill_database.skillHyoujiKaikin("Three_Stars");
                break;

            case "mg_night_barron_book":
                magicskill_database.skillHyoujiKaikin("Night_Barron");
                break;

            case "mg_crescent_moon_book":
                magicskill_database.skillHyoujiKaikin("Crescent_Moon");
                break;

            case "mg_lightning_grape_book":
                magicskill_database.skillHyoujiKaikin("Lightning_Grape");
                magicskill_database.skillLearnLv_Name("Lightning_Grape", 1); //習得済
                break;

            case "mg_dreamy_sapphire_book":
                magicskill_database.skillHyoujiKaikin("Dreamy_Sapphire");
                magicskill_database.skillLearnLv_Name("Dreamy_Sapphire", 1); //習得済
                break;

            case "mg_time_illusion_book":
                magicskill_database.skillHyoujiKaikin("Time_illusion");
                break;

            case "mg_time_return_book":
                magicskill_database.skillHyoujiKaikin("Time_Return");
                break;

            case "mg_warming_handmade_book":
                magicskill_database.skillHyoujiKaikin("Warming_Handmade");
                break;

            case "mg_life_stream_book":
                magicskill_database.skillHyoujiKaikin("Life_Stream");
                break;

            case "mg_abracadabra_book":
                magicskill_database.skillHyoujiKaikin("AbraCadabra");
                break;

            case "mg_TrueofMyheart_book":
                magicskill_database.skillHyoujiKaikin("True_of_Myheart");
                magicskill_database.skillLearnLv_Name("True_of_Myheart", 1); //真実のハートはLV1習得すみの状態
                break;

            default:
                break;
        }
    }


    void Check_NewAreaFlag()
    {
        if (NewAreaCheck_loading == true)
        {
            //レシピを読み込み中のときは、所持チェックはしない。
        }
        else //レシピを読み込み中でない。
        {
            //所持しているが、まだ読んでいないレシピがないか、チェックする。
            NewAreaCheck_loading = false;
            starrank_kaikin_ON = false;

            if (GameMgr.Before_Player_ninkiparam < PlayerStatus.player_ninki_param) //コンテスト前の人気と獲得後の人気を比較し、取得したスターをチェックする。
            {
                WindowOff();

                //スターランク＋新エリアチェック中の状態
                GameMgr.compound_select = 1100;
                GameMgr.compound_status = 1100;

                //現在のスターランクをチェックする。スターランクは現在未使用。
                PlayerStatus.SetPatissierRank(PlayerStatus.player_ninki_param); //パティシエランクのチェックとセット　現在の状態に更新　念のため

                //もし、獲得したスターがあれば、このタイミングでスタンプラリーを開き、アニメーション
                //フラグの解放なども、スターパネルのほうでチェックしている。
                StartCoroutine(StartWait());
                //starrank_kaikin_ON = true;
                NewAreaCheck_loading = true; //スタースタンプパネル表示中のフラグ                
            }
            else
            {
                GameMgr.NewAreaRelease_flag = true;
                GameMgr.check_StarPanel_Endflag = false;
            }
        }
    }

    //少し間をおく
    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(0.5f); //1秒待つ

        starStampPanel.SetActive(true);
    }

    //StarStampPanelから読み出し パネルを閉じたあとに呼び出される
    public void EndStarReleaseCheck()
    {
        NewAreaCheck_loading = false;
        check_recipi_flag = false;
        GameMgr.check_StarPanel_Endflag = false; //スターパネル終了後のフラグ
        GameMgr.check_StarPanel_Eventflag = true;
        GameMgr.check_GirlLoveSubEvent_flag = false;
        GameMgr.compound_status = 0; //ここまでで、チェックの処理が全て完了したので、status=0にする。
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

                if(PlayerStatus.player_girl_eatCount >= 999)
                {
                    PlayerStatus.player_girl_eatCount = 999; //999でカンスト
                }

                ClickPanel_1.SetActive(false);
                ClickPanel_2.SetActive(false);

                //あげたお菓子がエデンだった場合　最終イベントが発生しEDへ　ただし、まずかったり油っこいなどがあった場合はふつうに失敗
                _baseID = pitemlist.player_extremepanel_itemlist[0].itemID;
                if(database.items[database.SearchItemID(_baseID)].itemName == "Eden") //Eden neko_cookie
                {
                    GameMgr.ending_on = true;
                    GameMgr.Fullmoon_judge_on = true;
                    //GameMgr.ending_number = 1;

                    //一回目　食べると何も起こらない　二回目、くじらさんと話してから5日後の19時以降に食べると、EDが発生
                    /*if(!GameMgr.GirlLoveSubEvent_stage1[600])
                    {
                        GameMgr.ending_on = true;
                        //GameMgr.ending_number = 1;
                        //GameMgr.Ending_counterenshutu_on = true;
                    }
                    else
                    {
                        //くじらさんから満月の夜を聞いていた
                        if(GameMgr.NPCHiroba_eventList[271])
                        {
                            //5日後の19時以降
                            if (PlayerStatus.player_cullent_month == GameMgr.System_Fullmoon_month &&
                               PlayerStatus.player_cullent_day == GameMgr.System_Fullmoon_day &&
                               PlayerStatus.player_cullent_hour >= GameMgr.NightDay_hour)
                            {
                                GameMgr.ending_on = true;
                                GameMgr.Fullmoon_judge_on = true;
                                GameMgr.ending_number = 1;
                                //GameMgr.Ending_counterenshutu_on = true;
                            }
                            else
                            {
                                GameMgr.ending_on = true;
                                GameMgr.Fullmoon_judge_on = false;
                                //GameMgr.ending_number = 1;
                                //GameMgr.Ending_counterenshutu_on = true;
                            }

                        }
                        else
                        {
                            GameMgr.ending_on = true;
                            GameMgr.Fullmoon_judge_on = false; //満月の夜を知らない場合　再度、なにもおこらない
                        }
                    }*/

                }
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

                        GameMgr.SceneSelectNum = GameMgr.Contest_MainStoryPlaceNum;
                        FadeManager.Instance.LoadScene("Or_Contest_Reception", 0.3f);
                        break;

                    /*case 2:

                        GameMgr.stage2_clear_girl1_loveexp = PlayerStatus.girl1_Love_exp; //クリア時の好感度を保存
                        GameMgr.stage2_clear_girl1_lovelv = PlayerStatus.girl1_Love_lv;
                        FadeManager.Instance.LoadScene("003_Stage3_eyecatch", 0.3f);
                        break;

                    case 3:

                        GameMgr.stage3_clear_girl1_loveexp = PlayerStatus.girl1_Love_exp; //クリア時の好感度を保存
                        GameMgr.stage3_clear_girl1_lovelv = PlayerStatus.girl1_Love_lv;
                        FadeManager.Instance.LoadScene("100_Ending", 0.3f);
                        break;*/

                }
                

                break;

            case false:

                yes_no_clear_panel.SetActive(false);

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
        yes_selectitem_kettei.onclick = false;

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //寝る処理
                yes_no_sleep_panel.SetActive(false);

                //受注クエストの個人依頼をみて、当日かどうかをチェックする。
                _teishutu_on = quest_database.CheckKojinQuest_ToDay();

                if (_teishutu_on) //個人依頼の当日
                {
                    OnSleep_KojinQuestComing(); //朝10時まで時間を進める処理
                }
                else
                {
                    if (!GameMgr.outgirl_Nowprogress)
                    {
                        GameMgr.sleep_status = 1; //宴で使用
                        OnSleepReceive();
                    }
                    else //外出中は、ヒカリが戻ってくるまで寝る処理に。
                    {
                        OnSleep_HikariReturnBack();
                    }
                }

                break;

            case false:

                yes_no_sleep_panel.SetActive(false);

                StartMessage();
                GameMgr.compound_status = 0;
                
                break;

        }
    }

    IEnumerator CatComing_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        //black_panel_A.SetActive(false);
        CatComingGatPanel_obj.transform.Find("Yes_no_Pane_catget").gameObject.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                sc.PlaySe(25);
                sc.PlaySe(catDataBase.SetVoice(0, 1)); //鳴き声 checkのほうを参照

                CatIconAnim_Hyouji(CatComingGatPanel_obj);
                CatComingGatPanel_obj.transform.Find("CatDataPanel/CatMemo").gameObject.SetActive(false);
                _text.text = catDataBase.catdata_checklist[0].catnameHyouji + "がお家にきたよ～～！！";

                //表示されたねこを実際にねこリストに追加する
                catDataBase.CatCopyCheckToOrigin();

                _effect_list.Clear();
                _effect_list.Add(Instantiate(effparticle_Prefab1, CatComingGatPanel_obj.transform));
                AnimPoyon(CatComingGatPanel_obj.transform.Find("CatDataPanel/CatIconBaseImg").gameObject); //ぽよんアニメ

                yes_selectitem_kettei.onclick = false;
                break;

            case false:

                sc.PlaySe(240); //共通　悲しい鳴き声

                //なにもせず終了
                _text.text = "ねこちゃん。かなしそうな顔で去っていったよ～..。";

                yes_selectitem_kettei.onclick = false;
                break;

        }

        StartCoroutine("WaitInteract");
        StartCoroutine("WaitCloseButtonON");
    }

    IEnumerator WaitCloseButtonON()
    {
        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (closebutton != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        closebutton = false; //オンクリックのフラグはオフにしておく。

        //エフェクトも削除
        if (_effect_list.Count > 0)
        {
            Destroy(_effect_list[0].gameObject);
            _effect_list.Clear();
        }

        GameMgr.catcoming_event_endflag = true;
        CatComingGatPanel_obj.SetActive(false);
    }

    IEnumerator WaitInteract()
    {
        yield return new WaitForSeconds(0.5f); //1秒待つ

        CatComingGatPanel_obj.transform.Find("CloseButton").gameObject.SetActive(true);
    }

    public void OnCloseButton()
    {
        closebutton = true;
    }

    void FinalCheck_CatCheckDataKoushin(GameObject _obj, int _catid)
    {
        CatIconImage_Hyouji(_obj);

        //ねこの画像
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").GetComponent<Image>().sprite = catDataBase.SetSprite(_catid, 1);

        //ねこの画像アニメ
        foreach (Transform child in _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim/" + catDataBase.SetCatAnimObj(_catid, 1)).gameObject.SetActive(true);

        //ねこの名前
        _obj.transform.Find("CatDataPanel/NameText").GetComponent<Text>().text = catDataBase.catdata_checklist[_catid].catnameHyouji;

        //ねこのステータス
        _obj.transform.Find("CatDataPanel/CatMemo/LV_text").GetComponent<Text>().text = catDataBase.catdata_checklist[_catid].catLv.ToString();
        _obj.transform.Find("CatDataPanel/CatMemo/TansakuSP_text").GetComponent<Text>().text = catDataBase.CatTansakuTextLibrary(catDataBase.catdata_checklist[_catid].catTansaku_Speed);
        _obj.transform.Find("CatDataPanel/CatMemo/TansakuKaisu_text").GetComponent<Text>().text = catDataBase.catdata_checklist[_catid].catTansaku_Kaisu.ToString();
    }

    void CatIconAnim_Hyouji(GameObject _obj) //FinalCheckのときにアニメアイコンを表示　画像は非表示
    {
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").gameObject.SetActive(false);
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.SetActive(true);
    }

    void CatIconImage_Hyouji(GameObject _obj) //FinalCheckのときに画像を表示
    {
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").gameObject.SetActive(true);
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.SetActive(false);
    }



    public void ReadGirlLoveEvent_Fire() //EventDataBaseやGetMatPlace_Panelから読み出し
    {
        Debug.Log("ReadGirlLoveEvent発生");
        StartCoroutine("ReadGirlLoveEvent");
    }

    public void ReadGirlLoveTimeEvent_Fire() //EventDataBaseから読み出し 時間イベントからの読み出し
    {
        Debug.Log("ReadGirlLoveTimeEvent発生");
        GameMgr.compound_status = 0; //採取地選択画面など開いてる場合、被る可能性があるので、一度画面をリセット
        MainCompoundMethod(); //ただし、こっちを通す場合は、Muteが解除されるバグがあったので、使用に気を付ける。        
        StartCoroutine("ReadGirlLoveEvent");
    }

    //宴イベント読む
    IEnumerator ReadGirlLoveEvent()
    {
        OffCompoundSelect();
        compoundselect_onoff_obj.SetActive(false);
        Extremepanel_obj.SetActive(false);
        compoBG_A.SetActive(false);
        girl_love_exp_bar.SetActive(true);
        GameMgr.GirlLove_loading = true;

        girl1_status.HukidashiFlag = false;
        GameMgr.ResultComplete_flag = 0; //イベント読み始めたら、調合終了の合図をたてておく。

        //腹減りカウント一時停止
        girl1_status.GirlEatJudgecounter_OFF();
        girl1_status.Girl1_Status_Init();
        girlEat_judge.ListLVUPClear();

        GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
        GameMgr.compound_status = 1000;

        while (GameMgr.girlEat_ON) //女の子　食べ中のフラグ
        {
            yield return null;
        }

        /*while (girlEat_judge.heart_count > 0)
        {
            yield return null;
        }*/

        //吹き出しオフはこのタイミングで。
        girl1_status.hukidasiOff();

        mainUI_panel_obj.SetActive(false);
        GirlHeartEffect_obj.SetActive(false);       

        if (GameMgr.Mute_on)
        {
            sceneBGM.MuteBGM();
            map_ambience.Mute();
        }

        //サブイベントを読み始めたら、元のキャラクタの位置は元に戻しておく。
        ResetLive2DPos_Face();

        GameMgr.scenario_ON = true;

        if (GameMgr.GirlLoveEvent_bunki_status == 0) //通常は0で、ここで処理してOK
        {
            GameMgr.girlloveevent_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
        }
        else if (GameMgr.GirlLoveEvent_bunki_status == 1) //家から帰ってきたときのイベントのみ、フラグの変数が変わる。
        {
            GameMgr.CompoundEvent_storyflag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
        }

        while (!GameMgr.girlloveevent_endflag)
        {
            yield return null;
        }

        //ねこくるイベント発生中の場合は、そっちの処理が終わるのを待つ
        if(GameMgr.catcoming_event_ON)
        {
            GameMgr.compound_status = 1100;

            GameMgr.Mute_on = false;
            sceneBGM.MuteOFFBGM();
            map_ambience.MuteOFF();

            //確認画面をここで開く
            text_area.SetActive(true);
            CatComingGatPanel_obj.transform.Find("Yes_no_Pane_catget").gameObject.SetActive(true);
            CatComingGatPanel_obj.transform.Find("CatDataPanel/CatMemo").gameObject.SetActive(true);
            _text.text = "おにいちゃん。この子をお家で飼う？";

            //ランダムキャットの抽選
            catDataBase.RandomCatSelect();

            sc.PlaySe(catDataBase.SetVoice(0, 1)); //鳴き声 checkのほうを参照

            CatComingGatPanel_obj.SetActive(true);
            CatComingGatPanel_obj.transform.Find("CloseButton").gameObject.SetActive(false);

            FinalCheck_CatCheckDataKoushin(CatComingGatPanel_obj, 0);

            StartCoroutine("CatComing_Final_select");

            while (!GameMgr.catcoming_event_endflag)
            {
                yield return null;
            }
            GameMgr.catcoming_event_endflag = false;

            GameMgr.catcoming_event_ON = false;
        }

        Debug.Log("サブイベント読み終了");

        GameMgr.GirlLoveEvent_bunki_status = 0;
        GameMgr.GirlLove_loading = false;
        GameMgr.girlloveevent_endflag = false;
        //GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;
        GameMgr.girl_returnhome_endflag = false;
        GameMgr.CompoAfter_BackGirl = false; //戻り中に発生した場合は、戻ったことにしてfalseに。
        GameMgr.Mute_on = false;

        if (GameMgr.Utage_FadeOutWhiteON)
        {
            GameMgr.Utage_FadeOutWhiteON = false;

            //キラキラ音もなる
            sc.PlaySe(78);

            //白からフェードイン        
            fadeout_panel_obj.GetComponent<CanvasGroup>().alpha = 1;
            fadeout_panel_obj.GetComponent<CanvasGroup>().DOFade(0, 1.5f);
        }

        if (GameMgr.Utage_MapMoveON)
        {
            Debug.Log("Utage_MapMoveON");
            GameMgr.Utage_MapMoveON = false;
            
            GameMgr.Contest_afterHomeEventFlag = false;　//シーン移動するときは、コンテスト終了後発生イベントのフラグも一度消しておく。

            //読むシナリオによっては、シーン移動
            switch (GameMgr.GirlLoveSubEvent_num)
            {
                case 1110: //家賃払えなかったのでゲームオーバー　ゲームオーバーへ飛ぶ

                    GameMgr.SceneSelectNum = 10;
                    FadeManager.Instance.LoadScene("999_Gameover", GameMgr.SceneFadeTime);
                    break;

                case 2000: //光先生にはじめてあい、魔法教えてもらう。

                    GameMgr.SceneSelectNum = 30;
                    FadeManager.Instance.LoadScene("Or_NPC_MagicHouse", GameMgr.SceneFadeTime);
                    break;
            }

            if(GameMgr.ending_on)
            {
                Debug.Log("Ending シーン移動");
                if (GameMgr.ending_number == 1)
                {
                    FadeManager.Instance.LoadScene("110_TotalResult", GameMgr.SceneFadeTime); //100_Ending　にすると、エンディングのシーンへ一度飛ぶ
                }
                else
                {
                    FadeManager.Instance.LoadScene("110_TotalResult", GameMgr.SceneFadeTime);
                }
            }
        }
        else //シーン移動などしない場合は、以下デフォルトの処理
        {
            sceneBGM.MuteOFFBGM();
            map_ambience.MuteOFF();
            
            //sceneBGM.OnMainBGM(); //メインBGMを変更　ハートレベルに応じてBGMも切り替わる。

            mainUI_panel_obj.SetActive(true);
            //OnCompoundSelect();
            Extremepanel_obj.SetActive(true);
            GirlHeartEffect_obj.SetActive(true);
            girlEat_judge.EffectClear();

            GameMgr.ending_on = false;

            //イベント後に、ハートを獲得するなどの演出がある場合
            if (GameMgr.SubEvAfterHeartGet)
            {
                GameMgr.SubEvAfterHeartGet = false;

                heartget_ON = false;
                switch (GameMgr.SubEvAfterHeartGet_num)
                {
                    case 60:

                        _textmain.text = "ぽんぽんの力で、より元気になってきた。";
                        //girlEat_judge.loveGetPlusAnimeON(30, false);    
                        girl1_status.GirlExpressionKoushin(50);

                        break;

                    case 61:

                        //玄関音
                        sc.EnterSound_01();

                        switch (GameMgr.event_judge_status)
                        {
                            case 0:

                                _textmain.text = "あまりピクニックは喜ばなかったようだ..。";
                                girlEat_judge.UpDegHeart(-1 * (int)SujiMap(GameMgr.event_okashi_score, 0, 30, 60, 0), true);
                                girl1_status.GirlExpressionKoushin(-20);
                                break;

                            case 1:

                                //get_heart = GameMgr.event_okashi_score; //GameMgr.event_okashi_score / 5
                                get_heart = 10;
                                _textmain.text = "ピクニックを喜んだようだ。" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";
                                girl1_status.GirlExpressionKoushin(10);

                                heartget_ON = true;
                                break;

                            case 2:

                                //get_heart = GameMgr.event_okashi_score;
                                get_heart = 20;
                                _textmain.text = "ピクニックをとても喜んだようだ！" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";

                                girl1_status.GirlExpressionKoushin(30);
                                GameMgr.picnic_after = true;
                                GameMgr.picnic_after_time = 60;

                                heartget_ON = true;
                                break;

                            case 3:

                                //get_heart = GameMgr.event_okashi_score;
                                get_heart = 30;
                                _textmain.text = "ピクニックが最高だったようだ！" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";

                                girl1_status.GirlExpressionKoushin(50);
                                GameMgr.picnic_after = true;
                                GameMgr.picnic_after_time = 60;

                                heartget_ON = true;
                                break;

                            case 4:

                                //get_heart = GameMgr.event_okashi_score;
                                get_heart = 50;
                                _textmain.text = "思い出に残るピクニックだった！" + "\n" + "ハート " + GameMgr.ColorPink + get_heart + "</color>" + "上がった！";

                                girl1_status.GirlExpressionKoushin(100);
                                GameMgr.picnic_after = true;
                                GameMgr.picnic_after_time = 60;

                                heartget_ON = true;
                                break;
                        }


                        break;


                    case 70:

                        _textmain.text = "メガネによろこんだ！";
                        get_heart = 20;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 71:

                        _textmain.text = "スク水をよろこんだようだ！";
                        get_heart = 50;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 72:

                        _textmain.text = "黒メイド服をよろこんだようだ！";
                        get_heart = 50;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 73:

                        _textmain.text = "天使のワンピースをよろこんだようだ！";
                        get_heart = 50;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 74:

                        _textmain.text = "深紅のハートドレスをよろこんだようだ！";
                        get_heart = 50;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 75:

                        _textmain.text = "バルーンハットをたいそうよろこんだようだ！";
                        get_heart = 30;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 76:

                        _textmain.text = "天使の羽根にこころを浄化された！";
                        get_heart = 30;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 77:

                        _textmain.text = "ねこみみに興味をひいたようだ！";
                        get_heart = 10;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 78:

                        _textmain.text = "お花のヘアピンを気に入ったようだ！";
                        get_heart = 10;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 79:

                        _textmain.text = "ティンクルスターダストを気に入ったようだ！";
                        get_heart = 30;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 80:

                        _textmain.text = "ラベンダードレスをよろこんだようだ！";
                        get_heart = 50;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 81:

                        _textmain.text = "どんぐりポシェットを気に入っている！";
                        get_heart = 20;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 82:

                        _textmain.text = "パティシエハットに興奮している！";
                        get_heart = 20;
                        girl1_status.GirlExpressionKoushin(50);
                        break;

                    case 100:

                        _textmain.text = "ぬいぐるみに喜んだようだ！";
                        get_heart = 30;
                        girl1_status.GirlExpressionKoushin(50);

                        heartget_ON = true;
                        break;

                    case 110:

                        _textmain.text = "ほんの少し、ヒカリの勇気がわいてきた！";
                        get_heart = 15;
                        girl1_status.GirlExpressionKoushin(50);

                        heartget_ON = true;
                        break;

                    case 130:

                        _textmain.text = "いっぱい遊んで、喜んでいるようだ！";
                        get_heart = 10;
                        girl1_status.GirlExpressionKoushin(50);

                        heartget_ON = true;
                        break;

                    case 200: //コンテスト終了後に、順位に応じてハートが上がるイベント


                        if (GameMgr.Contest_Cate_Ranking == 0) //コンテストがトーナメント形式=0
                        {
                            switch (GameMgr.contest_Rank_Count) //順位
                            {
                                case 1:

                                    _textmain.text = "ヒカリの勇気が少しわいてきた！";
                                    get_heart = 50;
                                    break;

                                case 0:

                                    _textmain.text = "ヒカリは励ましている！";
                                    get_heart = 2;
                                    break;
                            }
                        }
                        else
                        {
                            switch (GameMgr.contest_Rank_Count) //順位
                            {
                                case 1:

                                    _textmain.text = "ヒカリは喜びのダンスを踊っている！";
                                    get_heart = 50;
                                    break;

                                case 2:

                                    _textmain.text = "ヒカリは応援している！";
                                    get_heart = 30;
                                    break;

                                case 3:

                                    _textmain.text = "ヒカリは応援している！";
                                    get_heart = 10;
                                    break;

                                case 4:

                                    _textmain.text = "ヒカリは励ましている！";
                                    get_heart = 3;
                                    break;

                                case 5:

                                    _textmain.text = "ヒカリは励ましている！";
                                    get_heart = 1;
                                    break;
                               
                                default:

                                    _textmain.text = "ヒカリは応援している！";
                                    get_heart = 1;
                                    break;
                            }
                        }

                        girl1_status.GirlExpressionKoushin(50);

                        heartget_ON = true;
                        break;

                    case 210: //コンテスト終了　提出おかしが違って失格だった場合


                        _textmain.text = "ヒカリは励ましている！";
                        get_heart = 5;

                        girl1_status.GirlExpressionKoushin(10);

                        heartget_ON = true;
                        break;

                    case 211: //コンテスト終了　時間がすぎて失格だった場合


                        _textmain.text = "ヒカリは、ぼくの肩をもみもみしている！";
                        get_heart = 0;

                        girl1_status.GirlExpressionKoushin(10);

                        heartget_ON = false;
                        break;
                }

                if (heartget_ON)
                {
                    if (pitemlist.KosuCount("aroma_potion1") >= 1)
                    {
                        get_heart = (int)(get_heart * 1.2f);
                        girlEat_judge.loveGetPlusAnimeON(get_heart, false); //trueにしておくと、ハートゲット後に、クエストクリアをチェック
                    }
                    else
                    {
                        girlEat_judge.loveGetPlusAnimeON(get_heart, false); //trueにしておくと、ハートゲット後に、クエストクリアをチェック
                    }
                }


                StartCoroutine("ReadGirlLoveEventAfter");

            }
            else
            {
                //_textmain.text = "";
                StartMessage(); //テキスト更新

                GameMgr.check_GirlLoveEvent_flag = true;
                girl1_status.Girl1_Status_Init2();
                GameMgr.compound_status = 0;
                //Debug.Log("compound_statusを0にする");
            }

            //腹減りカウント開始
            girl1_status.GirlEat_Judge_on = true;

            check_recipi_flag = false;
        }      
       
    }   

    IEnumerator ReadGirlLoveEventAfter()
    {
        subevent_after_end = true; //サブイベントアフター演出を読み中   
        while (girlEat_judge.heart_count > 0)
        {
            yield return null;
          
        }

        subevent_after_end = false;

        girl1_status.Girl1_Status_Init2();
        GameMgr.compound_status = 0;       
        //Debug.Log("compound_statusを0にする");

        //ハートがすべてなくなってから、再度サブイベントチェック
        GameMgr.check_GirlLoveEvent_flag = true;
        GameMgr.check_GirlLoveSubEvent_flag = false;
        
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

            if (GameMgr.Story_Mode == 0)
            {
                switch (GameMgr.GirlLoveEvent_num)
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

                    case 1: //さくらクッキー


                        if (!GameMgr.MapEvent_Or[0]) //まだ春風の森にいったことがない場合
                        {
                            _textmain.text = "どうしようかなぁ？" + "\n" + "（さくらの花びらは、「春風の森」で採れそうだけど..。）";
                        }
                        else
                        {

                        }

                        break;

                }
            }
        }

        //メイン画面に表示する、現在のクエスト
        special_quest.RedrawQuestName();
    }

    

    //眠りの宴シナリオを呼び出す際に使用。TimeControllerからも読み出し
    public void OnSleepReceive()
    {
        GameMgr.Sleep_CheckEnd = true;
        StartCoroutine("SleepDayEnd");
    }

    IEnumerator SleepDayEnd()
    {

        //今日の食事がランダムで決まる
        InitTodayFoodLibrary();
        if (_todayfood_lib.Count <= 0)
        {
            _todayfood = "じゃがバター";
            _todayfoodexpence = 30;

            _todayfoodexpence += (Random.Range(0, 10) - 5);
        }
        else
        {
            RandomFoodLottery();

            lot_count = 0;
            while (lot_count < 5)
            {
                if (_todayfoodexpence > PlayerStatus.player_money) //所持金が足りないときは、もう一度抽選
                {
                    RandomFoodLottery();
                }
                else
                {
                    break;
                }
                lot_count++;
            }

            if (lot_count >= 5) //全部抽選して、全て所持金が足りなかった場合　０ルピアのパンを食べる。
            {
                _todayfood = "パンのきれはじ";
                _todayfoodexpence = 0;
            }
        }


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

        //リセットステータス
        PlayerStatus.player_girl_lifepoint = PlayerStatus.player_girl_maxlifepoint; //体力は全回復
        PlayerStatus.player_mp = PlayerStatus.player_maxmp; //MPも全回復

        PlayerStatus.player_day++; //一日を加算
        GameMgr.Sale_ON = false;
        //PlayerStatus.player_time = 0;
        PlayerStatus.player_cullent_hour = GameMgr.StartDay_hour;
        PlayerStatus.player_cullent_minute = 0;
        GameMgr.BarQuest_NewReset = false; //酒場クエストの更新フラグ　リセット
        GameMgr.BarQuest_NewReset2 = false;
        GameMgr.GirlLoveSubEvent_NPC_OkashiPresentON = false; //固有NPCが直接家にきて依頼するフラグ　リセット

        //寝るタイミングで、いくつかのフラグもリセット
        SleepAfter_FlagReset();

        //月日も更新しておく カレンダー更新
        time_controller.TimeKoushin(0, false);

        //寝るイベント発生時に、ピクニックイベントのカウンタが+1進む。        
        GameMgr.picnic_count--;
        GameMgr.outgirl_count--; //外出カウンタも進む
        Debug.Log("ピクニックカウント: " + GameMgr.picnic_count);
        Debug.Log("外出カウント: " + GameMgr.outgirl_count);

        //ねこ家くるカウントもすすむ？
        GameMgr.catcoming_count--;
        Debug.Log("ねこ家くるカウント: " + GameMgr.catcoming_count);

        //各NPCのイベント日数カウンタも進む
        for (i = 0; i < GameMgr.NPCHiroba_eventDayCounter.Length; i++)
        {
            switch (i) //各条件
            {
                case 0: //アマクサ　初優勝後お祝いにくる

                    if (conteststartList_database.ReturnVictoryCount(1) >= 1) //初優勝後から
                    {
                        GameMgr.NPCHiroba_eventDayCounter[i]--;
                    }
                    break;

                case 1: //ねこみみ少女　次の会話発生までのかうんた 連続でイベント発生するのを防止

                    if (GameMgr.NPCHiroba_eventList[1050])
                    {
                        GameMgr.NPCHiroba_eventDayCounter[i]--;
                    }
                    break;
            }

            if (GameMgr.NPCHiroba_eventDayCounter[i] <= 0)
            {
                GameMgr.NPCHiroba_eventDayCounter[i] = 0;
            }
        }

        for (i = 0; i < GameMgr.NPC_FriendTimeCounter.Length; i++)
        {
            GameMgr.NPC_FriendTimeCounter[i]--;
            GameMgr.NPC_BarFriendTimeCounter[i]--;

            if (GameMgr.NPC_FriendTimeCounter[i] < 0)
            {
                GameMgr.NPC_FriendTimeCounter[i] = 0;

            }
            if (GameMgr.NPC_BarFriendTimeCounter[i] < 0)
            {
                GameMgr.NPC_BarFriendTimeCounter[i] = 0;
            }
        }

        for (i = 0; i < GameMgr.GirlLoveSubEvent_stage1_Counter.Length; i++)
        {
            GameMgr.GirlLoveSubEvent_stage1_Counter[i]--;

            if (GameMgr.GirlLoveSubEvent_stage1_Counter[i] < 0)
            {
                GameMgr.GirlLoveSubEvent_stage1_Counter[i] = 0;

            }
        }


        //変更したセリフ類は、必ず元に戻す。
        time_controller.TimeReturnHomeSleep_Status = false;

        cat_cost = 0;
        if (GameMgr.System_CatAutoMaterial_ON)
        {
            //ねこがいる場合、ねこのエサ費用を減らす　一匹ずつチェック
            for (i = 0; i < catDataBase.catdata_list.Count; i++)
            {
                if (catDataBase.catdata_list[i].catStatus == 100)
                { }
                else //外にでておらず家にいる場合　好感度がほんのわずかにあがっていく。
                {
                    catDataBase.catdata_list[i].catHP += 1;
                }

                //外に出てるに関わらず、エサ代は減っていく。
                cat_cost = catDataBase.catdata_list[i].catCost;

                if (PlayerStatus.player_money < cat_cost)
                {
                    //そのねこにエサをあげれない。ので好感度が下がる。好感度が0になると逃亡する。エサをもらってないので、不機嫌な状態に変化。
                    catDataBase.catdata_list[i].catHP -= 10;
                    catDataBase.catdata_list[i].catEsaNoGive = true;
                }
                else
                {
                    moneyStatus_Controller.Getmoney_noAnim(-cat_cost);
                    catDataBase.catdata_list[i].catEsaNoGive = false;
                }
            }
        }

        //ねこチェック　好感度が0を下回っていたら、ここで逃亡が決定する
        catDataBase.GetOutCatCheck();

        //一日経つと、食費を消費
        moneyStatus_Controller.UseMoney(GameMgr.Foodexpenses);


        //腹が回復する。
        if (GameMgr.System_Manpuku_ON)
        {
            if (PlayerStatus.player_girl_manpuku <= 20)
            {
                PlayerStatus.player_girl_manpuku = 10;
            }
        }
        else
        {
            PlayerStatus.player_girl_manpuku = 50; //満腹機能使ってないときは、常に50を保つ。
        }

        //
        //アイテム関係
        //
        DayStartItemPassive();




        //寝たらスリープフラグもOFFに。
        GameMgr.Sleep_CheckEnd = false;

        //寝た後のサブイベントの発生チェック　コンテストの発生もないかここでチェックする
        GameMgr.check_GirlLoveSubEvent_flag = false;
        for (i = 0; i < GameMgr.check_SleepEnd_Eventflag.Length; i++) //寝ておきたあとにイベント発生するものがないか全てチェック
        {
            GameMgr.check_SleepEnd_Eventflag[i] = true;
        }

    }

    void DayStartItemPassive()
    {
        //ムゲンニワトリがたまごを産んでくれる
        if (pitemlist.KosuCount("mugen_niwatori") >= 1)
        {
            //まれにプレミアム卵うんでくれる
            random2 = Random.Range(0, 100);
            if (random2 <= 10) //10%
            {
                random = Random.Range(1, 2);
                pitemlist.addPlayerItemString("egg_premiaum", random);
            }
            else
            {
                random = Random.Range(1, 3);
                pitemlist.addPlayerItemString("egg", random);
            }
        }

        //湧き出る泉から砂糖水が湧き出る
        if (pitemlist.KosuCount("infinity_fountain") >= 1)
        {
            random = Random.Range(1, 3);
            pitemlist.addPlayerItemString("sugerwater", random);
        }

        //湧き出る泉から炭酸水が湧き出る
        if (pitemlist.KosuCount("infinity_fountain_tansan") >= 1)
        {
            random = Random.Range(1, 3);
            pitemlist.addPlayerItemString("water_soda", random);
        }
    }

    void RandomFoodLottery()
    {
        random = Random.Range(0, _todayfood_lib.Count);
        _todayfood = _todayfood_lib[random];
        _todayfoodexpence = _todayfoodexpence_lib[random];

        _todayfoodexpence += (Random.Range(0, 10) - 5);
    }

    void OnSleep_HikariReturnBack()
    {
        //GameMgr.EventAfter_MoveEnd = true;
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;
        
        StartCoroutine("WaitForSleepAtHikari");
    }

    IEnumerator WaitForSleepAtHikari()
    {
        yield return new WaitForSeconds(2.0f); //1秒待つ

        //時間を17時まで進める。
        time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day, 17, 0);

        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;

        eventdatabase.OutGirlReturnHome();
        ReadGirlLoveTimeEvent_Fire();
    }

    void OnSleep_KojinQuestComing()
    {
        //GameMgr.EventAfter_MoveEnd = true;
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

        StartCoroutine("WaitForComingClient");
    }

    IEnumerator WaitForComingClient()
    {
        yield return new WaitForSeconds(2.0f); //1秒待つ

        //時間を10時まで進める。
        time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day, 10, 0);

        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f);
        
        yield return new WaitForSeconds(1.0f); //1秒待つ

        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;

        StartMessage();
        GameMgr.compound_status = 0;
    }

    void SleepAfter_FlagReset()
    {
        GameMgr.NPCHiroba_eventList[101] = false; //ぬねがまた登場し始める
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
        Touch_ALLOFF();       
        menu_toggle.GetComponent<Toggle>().interactable = false;
        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
        shop_toggle.GetComponent<Toggle>().interactable = false;
        bar_toggle.GetComponent<Toggle>().interactable = false;
        girleat_toggle.GetComponent<Toggle>().interactable = false;
        recipi_toggle.GetComponent<Toggle>().interactable = false;
        sleep_toggle.GetComponent<Toggle>().interactable = false;
        system_toggle.GetComponent<Toggle>().interactable = false;
        status_toggle.GetComponent<Toggle>().interactable = false;
        hinttaste_toggle.GetComponent<Toggle>().interactable = false;
        quest_kakuninButton_obj.transform.Find("QuestKakuninButton").GetComponent<Button>().interactable = false;
        contest_kakuninButton_obj.transform.Find("ContestKakuninButton").GetComponent<Button>().interactable = false;
        starPanel_kakuninButton_obj.transform.Find("StarKakuninButton").GetComponent<Button>().interactable = false;
        mainlist_scrollview_obj.SetActive(false);
    }

    void OnCompoundSelect()
    {
        Touch_ALLON();
        menu_toggle.GetComponent<Toggle>().interactable = true;
        getmaterial_toggle.GetComponent<Toggle>().interactable = true;
        shop_toggle.GetComponent<Toggle>().interactable = true;
        bar_toggle.GetComponent<Toggle>().interactable = true;
        girleat_toggle.GetComponent<Toggle>().interactable = true;
        recipi_toggle.GetComponent<Toggle>().interactable = true;
        sleep_toggle.GetComponent<Toggle>().interactable = true;
        system_toggle.GetComponent<Toggle>().interactable = true;
        status_toggle.GetComponent<Toggle>().interactable = true;
        hinttaste_toggle.GetComponent<Toggle>().interactable = true;
        extreme_Button.interactable = true;
        quest_kakuninButton_obj.transform.Find("QuestKakuninButton").GetComponent<Button>().interactable = true;
        contest_kakuninButton_obj.transform.Find("ContestKakuninButton").GetComponent<Button>().interactable = true;
        starPanel_kakuninButton_obj.transform.Find("StarKakuninButton").GetComponent<Button>().interactable = true;
        OuthomePanelONOFF();
    }

    //メインシーン内のタッチオブジェクトのONOFF　GirlEat_Judgeやgirl1_statusからも読み出し
    void Touch_ALLON()
    {
        GameMgr.CharacterTouch_ALLON = true;
        GameMgr.BGTouch_ALLON = true;
    }

    void Touch_ALLOFF()
    {
        GameMgr.CharacterTouch_ALLOFF = true;
        GameMgr.BGTouch_ALLOFF = true;
    }


    public void OnCompoundSelectObj() //GirlEatJudgeから読み出し
    {
        compoundselect_onoff_obj.SetActive(true);
    }

    //ゲームの進行度合いなどに応じて、表示ボタンなどを追加する。
    public void CheckButtonFlag()
    {
        if (GameMgr.Story_Mode == 0)
        {
            if (GameMgr.GirlLoveSubEvent_stage1[0] || GameMgr.GirlLoveEvent_num >= 1)
            {
                hinttaste_toggle.SetActive(true);
            }
            else
            {
                hinttaste_toggle.SetActive(false);
            }
        }
        else
        {
            hinttaste_toggle.SetActive(true);
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
        if(PlayerStatus.player_girl_eatCount <= 0 && pitemlist.player_extremepanel_itemlist.Count > 0)
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

    



    //ストーリー進行に応じて、背景の天気+エフェクトも変わる。Save_Controllerからも読まれる。
    public void Change_BGimage()
    {
        //天気モードONのときのみ　変更
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {
            //DrawALLOFFBG();
            RealTimeBGSetInit();

            //時間をチェックし、背景を自動で変更。
            Weather_ChangeNow();
        }

        /*if (GameMgr.Story_Mode == 0)
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
                bgweather_image_panel.transform.Find("BG_windowout_morning").gameObject.SetActive(true);
                //BG_Imagepanel.transform.Find("BG_sprite_morning").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
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
                //BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);
            }
            if (GameMgr.GirlLoveEvent_num >= 50) //のどかなはれ HLv10~
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_noon").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_Ball").gameObject.SetActive(true);
                //BG_effectpanel.transform.Find("BG_Particle_Light_Kira").gameObject.SetActive(true);
            }
            if (PlayerStatus.girl1_Love_lv >= 15) //快晴　夕方
            {
                DrawALLOFFBG();
                bgweather_image_panel.transform.Find("BG_windowout_noon").gameObject.SetActive(true);
                BG_Imagepanel.transform.Find("BG_sprite_sunny").gameObject.SetActive(true);
                //BG_effectpanel.transform.Find("BG_Particle_Light").gameObject.SetActive(true);
                BG_effectpanel.transform.Find("BG_Particle_Light_twilight").gameObject.SetActive(true);
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
            BG_effectpanel.transform.Find("BG_Particle_Light_Morning").gameObject.SetActive(true);
            BG_effectpanel.transform.Find("BG_Particle_Light_Night").gameObject.SetActive(true);
            BG_effectpanel.transform.Find("BG_Particle_Light_twilight").gameObject.SetActive(true);
            BG_effectpanel.transform.Find("BG_Particle_Light_moon").gameObject.SetActive(true);

            //時間をチェックし、背景を自動で変更。
            Weather_ChangeNow(0.0f);
        }*/
    }

    public void Weather_Change() //TimeControllerからも読み出し
    {
        //天気モードONのときのみ　変更
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {

            if (GameMgr.BG_cullent_weather != 2) //朝起きたては、強制的にCompound_MainのOnMorningBG()で変更するので、朝の判定のみ削除。お昼～夜まではチェック。
            {
                if (GameMgr.BG_cullent_weather != GameMgr.BG_before_weather)
                {
                    GameMgr.BG_before_weather = GameMgr.BG_cullent_weather;

                    //天気アニメ変更をトリガー
                    BG_RealtimeChange(0); //背景更新
                    Debug.Log("天気を変更");
                }
            }
        }
    }

    //現在時刻に合わせて、即背景を変更。前時間と現在時間の比較計算をしない。ロード直後はこっちを使用。（うまくbeforeとcullentの値の切り替えが出来なかったため。）
    public void Weather_ChangeNow()
    {
        //天気モードONのときのみ　変更
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {
            //Debug.Log("GameMgr.BG_cullent_weather: " + GameMgr.BG_cullent_weather);
            //Debug.Log("GameMgr.BG_before_weather: " + GameMgr.BG_before_weather);
            GameMgr.BG_before_weather = GameMgr.BG_cullent_weather;

            //天気アニメ変更をトリガー
            BG_RealtimeChange(1); //背景更新
            Debug.Log("天気を変更　即時");
        }
    }

    //天気の表示処理。背景をリアルタイムに変更する処理。
    void BG_RealtimeChange(int _status)
    {
        switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
        {
            case 1:

                BG_effectpanel_Effect.WeatherEffect_Koushin();
                break;

            case 2: //深夜→朝

                if (_status == 1) //即時切替
                {
                    BGImg_dayAnim.Play("BGimg_dayanim");
                }
                else
                { }
                daynum = 0;
                BGImg_dayAnim.SetInteger("daystatus", daynum);
                BG_effectpanel_Effect.WeatherEffect_Koushin();

                bg_accessory_panel.GetComponent<BGAcceTrigger>().WeatherChangeMorning();
                break;

            case 3: //朝

                if (_status == 1) //即時切替
                {
                    BGImg_dayAnim.Play("BGimg_dayanim");
                }
                else
                { }
                daynum = 0;
                BGImg_dayAnim.SetInteger("daystatus", daynum);
                BG_effectpanel_Effect.WeatherEffect_Koushin();

                bg_accessory_panel.GetComponent<BGAcceTrigger>().WeatherChangeMorning();
                break;

            case 4: //昼

                if (_status == 1) //即時切替
                {
                    BGImg_dayAnim.Play("BGimg_dayanim");
                }
                else
                { }
                daynum = 0;
                BGImg_dayAnim.SetInteger("daystatus", daynum);
                BG_effectpanel_Effect.WeatherEffect_Koushin();

                bg_accessory_panel.GetComponent<BGAcceTrigger>().WeatherChangeMorning();
                break;

            case 5: //夕方

                if (_status == 1) //即時切替
                {
                    BGImg_dayAnim.Play("BGimg_dayanim");
                }
                else
                { }
                daynum = 0;
                BGImg_dayAnim.SetInteger("daystatus", daynum);
                BG_effectpanel_Effect.WeatherEffect_Koushin();

                bg_accessory_panel.GetComponent<BGAcceTrigger>().WeatherChangeMorning();
                break;

            case 6: //夜

                if (_status == 1) //即時切替
                {
                    BGImg_dayAnim.Play("BGimg_dayanim4");
                }
                else
                { }
                daynum = 3;
                BGImg_dayAnim.SetInteger("daystatus", daynum);
                BG_effectpanel_Effect.WeatherEffect_Koushin();

                bg_accessory_panel.GetComponent<BGAcceTrigger>().WeatherChangeNight();
                break;
        }
    }

    
    
    void ManpukuKoushin()
    {
        if (GameMgr.System_Manpuku_ON)
        { }
        else
        {
            PlayerStatus.player_girl_manpuku = 50; //満腹機能使ってないときは、常に50を保つ。
        }

        if (PlayerStatus.player_girl_manpuku >= 0 && PlayerStatus.player_girl_manpuku < 1)
        {
            manpuku_text.text = "空腹";
        }
        else if (PlayerStatus.player_girl_manpuku >= 1 && PlayerStatus.player_girl_manpuku < 30)
        {
            manpuku_text.text = "はらへり";
        }
        else if (PlayerStatus.player_girl_manpuku >= 30 && PlayerStatus.player_girl_manpuku < 60)
        {
            manpuku_text.text = "ふつう";
        }
        else if (PlayerStatus.player_girl_manpuku >= 60 && PlayerStatus.player_girl_manpuku < 80)
        {
            manpuku_text.text = "みたされ";
        }
        else if (PlayerStatus.player_girl_manpuku >= 80)
        {
            manpuku_text.text = "まんぷく";
        }

        manpuku_slider.value = PlayerStatus.player_girl_manpuku;
    }  

    void GirlExpressionKoushinHyouji()
    {
        //機嫌決定
        if (PlayerStatus.player_girl_express_param < 20)
        {
            kigen_text.text = "機嫌: 怒り";
        }
        else if (PlayerStatus.player_girl_express_param >= 20 && PlayerStatus.player_girl_express_param < 40)
        {
            kigen_text.text = "機嫌: 不機嫌";
        }
        else if (PlayerStatus.player_girl_express_param >= 40 && PlayerStatus.player_girl_express_param < 60)
        {
            kigen_text.text = "機嫌: まあまあ";
        }
        else if (PlayerStatus.player_girl_express_param >= 60 && PlayerStatus.player_girl_express_param < 80)
        {
            kigen_text.text = "機嫌: 良";
        }
        else if (PlayerStatus.player_girl_express_param >= 80)
        {
            kigen_text.text = "機嫌: 最高";
        }
    }

    
    /*
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
    }*/

    void RealTimeBGSetInit()
    {
        //bgweather_image_panel.transform.Find("BG_windowout_white").gameObject.SetActive(false);
    }

    //即座に朝背景に変更。Utageの寝るイベントから呼び出し
    public void OnMorningBG()
    {
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {
            RealTimeBGSetInit();

            //背景を朝に変更
            GameMgr.BG_cullent_weather = 2;
            Weather_ChangeNow();
        }
    }

    public void HeartGuageTextKoushin()
    {
        //好感度ゲージを更新               
        debug_panel.GirlLove_Koushin(PlayerStatus.girl1_Love_exp);
    }

    void InitTodayFoodLibrary()
    {
        _todayfood_lib.Clear();
        _todayfoodexpence_lib.Clear();

        _todayfood_buf = 1.0f;
        if(PlayerStatus.girl1_Love_lv >= 12)
        {
            _todayfood_buf = 1.3f;
        }
        else if (PlayerStatus.girl1_Love_lv >= 18)
        {
            _todayfood_buf = 1.5f;
        }
        else if (PlayerStatus.girl1_Love_lv >= 25)
        {
            _todayfood_buf = 2.0f;
        }
        else if (PlayerStatus.girl1_Love_lv >= 35)
        {
            _todayfood_buf = 2.2f;
        }
        else if (PlayerStatus.girl1_Love_lv >= 50)
        {
            _todayfood_buf = 2.5f;
        }
        else if (PlayerStatus.girl1_Love_lv >= 75)
        {
            _todayfood_buf = 3.0f;
        }

        for (i = 1; i <= PlayerStatus.girl1_Love_lv; i++)
        {
            switch (i)
            {
                case 1:
                    
                                      
                    _todayfood_lib.Add("じゃがいもとお豆のスープ");
                    _todayfoodexpence_lib.Add((int)(60f * _todayfood_buf));
                    _todayfood_lib.Add("パン");
                    _todayfoodexpence_lib.Add((int)(50f * _todayfood_buf));
                    _todayfood_lib.Add("みそパン");
                    _todayfoodexpence_lib.Add((int)(80f * _todayfood_buf));
                    _todayfood_lib.Add("ゆでじゃが");
                    _todayfoodexpence_lib.Add((int)(70f * _todayfood_buf));
                    _todayfood_lib.Add("野菜の端っこゆで");
                    _todayfoodexpence_lib.Add((int)(50f * _todayfood_buf));
                    _todayfood_lib.Add("ほしにくのせパン");
                    _todayfoodexpence_lib.Add((int)(100f * _todayfood_buf));
                    _todayfood_lib.Add("ねぎピザ");
                    _todayfoodexpence_lib.Add((int)(30f * _todayfood_buf));
                    _todayfood_lib.Add("きのこピザ");
                    _todayfoodexpence_lib.Add((int)(30f * _todayfood_buf));
                    if (pitemlist.KosuCountEvent("bugget_recipi") >= 1)
                    {
                        _todayfood_lib.Add("じゃりパン");
                        _todayfoodexpence_lib.Add((int)(30f * _todayfood_buf));
                    }
                    if (pitemlist.KosuCountEvent("potatebutter_recipi") >= 1)
                    {
                        _todayfood_lib.Add("じゃがバター");
                        _todayfoodexpence_lib.Add((int)(30f * _todayfood_buf));
                    }
                    break;

                case 3:

                    _todayfood_lib.Add("じゃがいもとアスパラガスの炒め");
                    _todayfoodexpence_lib.Add((int)(75f * _todayfood_buf));
                    _todayfood_lib.Add("じゃがいもシチュー");
                    _todayfoodexpence_lib.Add((int)(60f * _todayfood_buf));                                     
                    _todayfood_lib.Add("ポテト・ガレット");
                    _todayfoodexpence_lib.Add((int)(50f * _todayfood_buf));                  
                    _todayfood_lib.Add("ハンバーグもどき");
                    _todayfoodexpence_lib.Add((int)(70f * _todayfood_buf));
                    
                    break;

                case 5:

                    _todayfood_lib.Add("特製ポテトペペロンチーノ");
                    _todayfoodexpence_lib.Add((int)(100f * _todayfood_buf));
                    _todayfood_lib.Add("バリバリ貝のブイヤ・ベース");
                    _todayfoodexpence_lib.Add((int)(110f * _todayfood_buf));
                    _todayfood_lib.Add("じゃがいものトマト煮込み");
                    _todayfoodexpence_lib.Add((int)(90f * _todayfood_buf));
                    _todayfood_lib.Add("回転網焼きステーキ");
                    _todayfoodexpence_lib.Add((int)(400f * _todayfood_buf));
                    _todayfood_lib.Add("おさかな地中海蒸し焼き");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    _todayfood_lib.Add("手ごねバーグ");
                    _todayfoodexpence_lib.Add((int)(120f * _todayfood_buf));
                    _todayfood_lib.Add("ほっくりじゃがピザ");
                    _todayfoodexpence_lib.Add((int)(150f * _todayfood_buf));
                    _todayfood_lib.Add("ビールとえだまめのたきこみご飯");
                    _todayfoodexpence_lib.Add((int)(120f * _todayfood_buf));
                    break;

                case 7:

                    _todayfood_lib.Add("欧風ゴールデンカレーライス");
                    _todayfoodexpence_lib.Add((int)(160f * _todayfood_buf));
                    _todayfood_lib.Add("アツアツリゾット");
                    _todayfoodexpence_lib.Add((int)(120f * _todayfood_buf));
                    _todayfood_lib.Add("イカスミスパゲティ");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    
                    break;

                case 10:

                    
                    _todayfood_lib.Add("黄金じゃがバター焼き");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    _todayfood_lib.Add("エビグラタン");
                    _todayfoodexpence_lib.Add((int)(220f * _todayfood_buf));
                    _todayfood_lib.Add("マッシュルームパスタ");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    break;

                case 15:

                    _todayfood_lib.Add("あったかじゃがいもポタージュ");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    _todayfood_lib.Add("ブルゴーニュの羊ステーキ");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    _todayfood_lib.Add("じゃがいもりだくさんのペスカトーレ");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    _todayfood_lib.Add("うまうまステーキ");
                    _todayfoodexpence_lib.Add((int)(350f * _todayfood_buf));
                    break;

                case 18:

                    _todayfood_lib.Add("ポテトコロッケ");
                    _todayfoodexpence_lib.Add((int)(100f * _todayfood_buf));
                    _todayfood_lib.Add("へんな草風味のジェノベーゼ");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    _todayfood_lib.Add("たこペペロンチーノ");
                    _todayfoodexpence_lib.Add((int)(400f * _todayfood_buf));
                    _todayfood_lib.Add("巨大イカの逆襲パスタ・オリーブ仕上げ");
                    _todayfoodexpence_lib.Add((int)(400f * _todayfood_buf));
                    _todayfood_lib.Add("ジャンボ・えびふら～い");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    break;

                case 30:

                    _todayfood_lib.Add("牛肉のビールやわらか煮込み");
                    _todayfoodexpence_lib.Add((int)(400f * _todayfood_buf));
                    _todayfood_lib.Add("チキンのレモン煮");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    _todayfood_lib.Add("巨大なエビ・ドリア");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    _todayfood_lib.Add("なすとひき肉のチーズペンネ");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    _todayfood_lib.Add("厚切りアルプス牛ロースステーキ");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    break;

                case 40:

                    _todayfood_lib.Add("マッシュルーム・スープ");
                    _todayfoodexpence_lib.Add((int)(100f * _todayfood_buf));
                    _todayfood_lib.Add("地中海風サラダパスタ");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    _todayfood_lib.Add("こくまろビーフシチュー");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    _todayfood_lib.Add("ムール貝のアダルティ酒蒸し");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    _todayfood_lib.Add("じゃがいものクリームパスタ");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    break;

                case 60:

                    _todayfood_lib.Add("じゃがいものパンケーキ");
                    _todayfoodexpence_lib.Add((int)(200f * _todayfood_buf));
                    _todayfood_lib.Add("マグロサーモンこってりドリア");
                    _todayfoodexpence_lib.Add((int)(600f * _todayfood_buf));
                    _todayfood_lib.Add("カジキとあらびきチョリソーピザ");
                    _todayfoodexpence_lib.Add((int)(800f * _todayfood_buf));
                    _todayfood_lib.Add("スクランブルエッグ");
                    _todayfoodexpence_lib.Add((int)(100f * _todayfood_buf));
                    _todayfood_lib.Add("ムラサきのこの京風オリーブパスタ");
                    _todayfoodexpence_lib.Add((int)(800f * _todayfood_buf));
                    _todayfood_lib.Add("きのこもりだくさんサラダ");
                    _todayfoodexpence_lib.Add((int)(500f * _todayfood_buf));
                    break;

                case 80:

                    _todayfood_lib.Add("ポテト・ボンボン");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    _todayfood_lib.Add("イタリアンロイヤルトマトピザ");
                    _todayfoodexpence_lib.Add((int)(2000f * _todayfood_buf));
                    _todayfood_lib.Add("ジャーマンポテトガーリックピザ");
                    _todayfoodexpence_lib.Add((int)(1400f * _todayfood_buf));
                    _todayfood_lib.Add("まんぷくラザニア");
                    _todayfoodexpence_lib.Add((int)(800f * _todayfood_buf));
                    _todayfood_lib.Add("にいちゃんハンバーグ");
                    _todayfoodexpence_lib.Add((int)(300f * _todayfood_buf));
                    _todayfood_lib.Add("新・黄金じゃがバター");
                    _todayfoodexpence_lib.Add((int)(1000f * _todayfood_buf));
                    
                    break;

                case 90:

                    _todayfood_lib.Add("ロイヤルポテト焼き");
                    _todayfoodexpence_lib.Add((int)(2000f * _todayfood_buf));
                    _todayfood_lib.Add("スタースパゲティ");
                    _todayfoodexpence_lib.Add((int)(3000f * _todayfood_buf));
                    _todayfood_lib.Add("ステーキ・シャトーブリアン");
                    _todayfoodexpence_lib.Add((int)(5000f * _todayfood_buf));
                    _todayfood_lib.Add("うなぎのパイ皮包み焼き");
                    _todayfoodexpence_lib.Add((int)(4000f * _todayfood_buf));
                    break;

                default:

                    break;
            }
        }             
    }

    void CompoundRoomSetting()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Compound": //前の村ではON

                //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                SetBGObj("BGOutimg_sc01", "BGimg_sc01", "effect_sc01");
                bg_accessory_panel.SetActive(true);
                break;

            case "Or_Compound": //飾りアイテムはオランジーナではひとまず使わない

                switch (GameMgr.OrCompound_RoomNum)
                {
                    case 0: //デフォルトの家

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc02", "effect_sc02");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 1: //花と森

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc03", "effect_sc03");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 2: //春２

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc04", "effect_sc04");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 3: //夏１　うみのへや

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc05", "effect_sc05");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 4: //夏２　すずらん

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc06", "effect_sc06");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 5: //夏３　ほしのへや

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc07", "effect_sc07");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 6: //秋１　ヨーロッパ

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc08", "effect_sc08");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 7: //ねこのへや

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc09", "effect_sc09");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 8: //ゲージュツの部屋

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc10", "effect_sc10");
                        bg_accessory_panel.SetActive(false);
                        break;

                    case 9: //なし

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc11", "effect_sc11");
                        bg_accessory_panel.SetActive(false);
                        break;

                    default:

                        //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                        SetBGObj("BGOutimg_sc03", "BGimg_sc02", "effect_sc02");
                        bg_accessory_panel.SetActive(false);
                        break;
                }

                break;

            default:

                //そのあと、シーンそれぞれのオブジェクトを取得し、表示
                SetBGObj("BGOutimg_sc03", "BGimg_sc02", "effect_sc02");
                bg_accessory_panel.SetActive(false);
                break;
        }
    }

   



    //Live2D関連コマンド

    //さらに、表示するときのコマンド
    void ReDrawLive2DPos_Compound()
    {
        cubism_rendercontroller.SortingOrder = 1500; //描画順指定
    }

    void ReSetLive2DOrder_Default()
    {
        cubism_rendercontroller.SortingOrder = default_live2d_draworder;  //ヒカリちゃんを表示しない。デフォルト描画順 //描画順指定
    }

    void ResetLive2DPos_Face() //CompoundMainControllerに移行　位置をセンターに戻す処理
    {
        compoundmain_Controller.ResetLive2D_ModelPos();
    }

    //ゲームメイン中のLive2Dキャラクタの表示をONにする。
    void CharacterLive2DImageON()
    {
        cubism_rendercontroller.Opacity = 1.0f;
        GirlHeartEffect_obj.SetActive(true);
        GirlHeartEffect_obj.GetComponent<Particle_Heart_Character>().LoveRateChange();
    }

    //ゲームメイン中のLive2Dキャラクタの表示をOFFにする。
    void CharacterLive2DImageOFF()
    {
        cubism_rendercontroller.Opacity = 0.0f;
        GirlHeartEffect_obj.SetActive(false);
    }

    void AnimPoyon(GameObject _obj)
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        _obj.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(_obj.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));


        //移動のアニメ
        sequence.Append(_obj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.75f)
            .SetEase(Ease.OutElastic));

        sequence.Join(_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }


    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    public void ChangeBGM() //デバッグパネルからアクセス用
    {
        sceneBGM.OnMainBGM();
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneloaded: " + scene);
        GameMgr.Scene_LoadedOn_End = true;
    }

    //シーンがアンロードされたタイミングで呼び出しされる
    void OnSceneUnloaded(Scene current)
    {
        //プレイヤーステータスのリセット
        PlayerStatus.ResetPlayerMagicStatus();

        Debug.Log("OnSceneUnloaded: " + current);
        GameMgr.Scene_LoadedOn_End = false;
    }
}
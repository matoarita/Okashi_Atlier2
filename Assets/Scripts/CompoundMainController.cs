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

public class CompoundMainController : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject canvas;

    private GameObject text_area_compound;
    private Text _textcomp;

    private Exp_Controller exp_Controller;
    private ItemDataBase database;

    private Text picnic_itemText;

    public string originai_text;
    public string extreme_text;
    public string recipi_text;
    public string magic_text;
    public string magiclearn_text;
    public string hikarimake_text;

    private GameObject text_hikari_makecaption;

    private BGM sceneBGM;
    private Map_Ambience map_ambience;

    private SceneInitSetting sceneinit_setting;

    private Girl1_status girl1_status;
    private Special_Quest special_quest;
    private TimeController time_controller;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject magicskilllistController_Learn;
    private GameObject magicskilllistController_Use;
    private MagicSkillListController magicskilllistController;
    private MagicSkillListController magicskilllistController_2;

    private PlayerItemList pitemlist;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject recipimemoController_obj;
    private GameObject recipiMemoButton;
    private GameObject memoResult_obj;

    private GameObject compoundselect_onoff_obj;

    private GameObject compoBG_A;
    private GameObject compoBGA_image;
    private GameObject compoBGA_imageOri;
    private GameObject compoBGA_imageRecipi;
    private GameObject compoBGA_imageExtreme;
    private GameObject compoBGA_imageHikariMake;

    private GameObject ResultBGimage;

    private GameObject selectPanel_1;
    private GameObject select_original_button_obj;
    private GameObject select_recipi_button_obj;
    private GameObject select_extreme_button_obj;
    private GameObject select_hikarimake_button_obj;
    private Button select_original_button;
    private Button select_recipi_button;
    private Button select_extreme_button;
    private Button select_hikarimake_button;

    private GameObject Hikarimake_StartPanel;
    private GameObject SelectCompo_panel_1;

    private GameObject MagicStartPanel;
    private GameObject magic_compo1;
    private GameObject magic_compo2;
    private GameObject magic_compo3;
    private GameObject magic_compo4;
    private GameObject player_mp_panel;

    private GameObject MagicLearnPanel;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject yes_no_panel; //通常時のYes, noボタン

    //Live2Dモデルの取得
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private int default_live2d_draworder;
    private bool character_On; //そのシーンにヒカリちゃんが存在するかどうかを検出
    private GameObject character_root;
    private GameObject character_move;
    private GameObject Anchor_Pos;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;
    private int trans_position;

    private int i;
    private int _id;

    private GameObject Debug_CompoIcon;

    private bool WaitForCompEnd; //調合シーン終了時に一回だけ行う処理のフラグ

    // Use this for initialization
    void Start()
    {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        Debug_CompoIcon = this.transform.Find("Debug_CompIcon").gameObject;
        Debug_CompoIcon.SetActive(false);

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        //map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();

        

        //コンポBGパネルの取得
        compoBG_A = this.transform.Find("Compound_BGPanel_A").gameObject;
        compoBG_A.SetActive(false);

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

        //確率パネルの取得
        kakuritsuPanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        compoBGA_image = compoBG_A.transform.Find("BG").gameObject;
        compoBGA_imageOri = compoBG_A.transform.Find("OriCompoImage").gameObject;
        compoBGA_imageRecipi = compoBG_A.transform.Find("RecipiCompoImage").gameObject;
        compoBGA_imageExtreme = compoBG_A.transform.Find("ExtremeImage").gameObject;
        compoBGA_imageHikariMake = compoBG_A.transform.Find("HikariMakeImage").gameObject;
        ResultBGimage = compoBG_A.transform.Find("ResultBG").gameObject;
        ResultBGimage.SetActive(false);

        Hikarimake_StartPanel = compoBG_A.transform.Find("HikariMakeStartPanel").gameObject;
        Hikarimake_StartPanel.SetActive(false);

        MagicStartPanel = compoBG_A.transform.Find("MagicStartPanel").gameObject;
        MagicStartPanel.SetActive(false);

        magic_compo1 = MagicStartPanel.transform.Find("magicComp1").gameObject;
        magic_compo1.SetActive(true);
        magic_compo2 = MagicStartPanel.transform.Find("magicComp2").gameObject;
        magic_compo2.SetActive(false);
        magic_compo3 = MagicStartPanel.transform.Find("magicComp3").gameObject;
        magic_compo3.SetActive(false);
        magic_compo4 = MagicStartPanel.transform.Find("magicComp4").gameObject;
        magic_compo4.SetActive(false);
        player_mp_panel = MagicStartPanel.transform.Find("PlayerMPPanel").gameObject;
        player_mp_panel.SetActive(true);

        MagicLearnPanel = compoBG_A.transform.Find("MagicLearnPanel").gameObject;
        MagicLearnPanel.SetActive(false);

        //windowテキストエリアの取得
        text_area_compound = compoBG_A.transform.Find("MessageWindowComp").gameObject;
        _textcomp = text_area_compound.GetComponentInChildren<Text>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        text_hikari_makecaption = text_area_compound.transform.Find("HikariMakeOkashiCaptionText").gameObject;
        text_hikari_makecaption.SetActive(false);

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //調合選択画面の取得
        SelectCompo_panel_1 = compoBG_A.transform.Find("SelectPanel_1").gameObject;
        SelectCompo_panel_1.SetActive(false);

        //ピクニック用のテキスト取得
        picnic_itemText = compoBG_A.transform.Find("SelectPanel_1/Picnic_yesno/ItemText/Text").GetComponent<Text>();


        //Live2Dモデルの取得 
        //調合専用処理では、Live2Dの描画順のみ操作し、位置やアクションなどには触らないようにする。LateUpdateの関係など、ややこしいので、元シーンで処理する。
        //Scene scene = SceneManager.GetActiveScene();
        //Debug.Log("Active scene is '" + scene.name + "'.");
        character_On = false;
        for (i = 0; i < SceneManager.sceneCount; i++)
        {
            //読み込まれているシーンを取得し、その名前をログに表示
            string sceneName = SceneManager.GetSceneAt(i).name;
            Debug.Log(sceneName);

            GameObject[] rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();
            
            foreach (var obj in rootObjects)
            {
                //Debug.LogFormat("RootObject = {0}", obj.name);
                if (obj.name == "CharacterRoot")
                {
                    Debug.Log("character_On: ヒカリちゃん　シーン内に存在する");
                    character_On = true;
                    _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
                    cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
                    default_live2d_draworder = cubism_rendercontroller.SortingOrder;
                    live2d_animator = _model_obj.GetComponent<Animator>();
                    character_root = GameObject.FindWithTag("CharacterRoot").gameObject;
                    character_move = character_root.transform.Find("CharacterMove").gameObject;
                    Anchor_Pos = character_move.transform.Find("Anchor_1").gameObject;

                }
                else
                {

                }
            }

        }



        //各調合時のシステムメッセージ集
        magic_text = "にいちゃん！　ふしぎな魔法をヒカリがかけてあげる！" + "\n" + "使いたい魔法を選んでね！";
        magiclearn_text = "どの魔法をおぼえる？　にいちゃん！";
        hikarimake_text = "にいちゃん！　ヒカリお菓子作りの手伝いしたいな！" + "\n" +
        "好きな材料を" + GameMgr.ColorYellow +
        "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";

        WaitForCompEnd = false;
    }

    void InitSetting()
    {
        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes = yes_no_panel.transform.Find("Yes").gameObject;
        no = yes_no_panel.transform.Find("No").gameObject;

        if (playeritemlist_onoff == null)
        {
            //プレイヤー所持アイテムリストパネルの取得
            playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

            //レシピリストパネルの取得
            recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilist_onoff.GetComponent<RecipiListController>();

            //スキルリストの取得
            magicskilllistController_Learn = canvas.transform.Find("MagicSkillList_Panel/MagicSkillList_ScrollView").gameObject;
            magicskilllistController_Use = canvas.transform.Find("MagicSkillList_Panel/MagicSkillList_ScrollView2").gameObject;
            magicskilllistController = magicskilllistController_Learn.GetComponent<MagicSkillListController>();
            magicskilllistController_2 = magicskilllistController_Use.GetComponent<MagicSkillListController>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        

        //デバッグ用　使用中アイコン
        if (GameMgr.CompoundSceneStartON)
        {
            Debug_CompoIcon.SetActive(true);
        }
        else
        {
            Debug_CompoIcon.SetActive(false);
        }


        if (GameMgr.CompoundSceneStartON)
        {
            if (!WaitForCompEnd) //
            {
                WaitForCompEnd = true;
            }

            switch (GameMgr.compound_status)
            {
                case 0:

                    break;

                case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    compoBGA_imageRecipi.SetActive(true);

                    recipilist_onoff.SetActive(true); //レシピリスト画面を表示。

                    yes_no_panel.SetActive(true);
                    yes.SetActive(false);
                   
                    text_area_compound.SetActive(true);

                    //ヒカリちゃんを表示する
                    ReDrawLive2DOrder_Compound();
                    SetLive2DPos_Compound();

                    if (GameMgr.tutorial_ON != true)
                    { }
                    else
                    {
                        no.SetActive(false);
                    }

                    break;

                case 2: //エクストリーム調合の処理を開始。クリック後に処理が始まる。

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 2; //トッピング調合を選択

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    compoBGA_imageExtreme.SetActive(true);

                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                    GameMgr.Comp_kettei_bunki = 0;

                    text_area_compound.SetActive(true);

                    //ヒカリちゃんを表示する
                    ReDrawLive2DOrder_Compound();
                    SetLive2DPos_Compound();

                    extreme_Compo_Setup(); //ベースアイテムを事前に設定しておく処理

                    break;

                case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 3; //オリジナル調合を選択
                   
                    //各調合画面を一度オフ
                    CompoScreenReset();

                    compoBGA_imageOri.SetActive(true);

                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 
                    GameMgr.Comp_kettei_bunki = 0;

                    recipiMemoButton.SetActive(true);

                    text_area_compound.SetActive(true);

                    //ヒカリちゃんを表示する
                    ReDrawLive2DOrder_Compound();
                    SetLive2DPos_Compound();

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

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 6;

                    GameMgr.updown_kosu = 1; //調合シーン最初に、数値をリセット

                    //最初に読むこむ設定
                    InitSetting();
                    
                    //Live2Dキャラの位置初期化
                    SetLive2DPos_Compound();

                    //BGMを変更
                    if (!GameMgr.Contest_ON) //大会はじまったときは調合BGM変わらないようにする
                    {
                        if (!GameMgr.tutorial_ON)
                        {
                            if (GameMgr.CompoBGMCHANGE_ON)
                            {
                                if (GameMgr.compobgm_change_flag != true)
                                {
                                    sceneBGM.OnCompoundBGM();
                                    GameMgr.compobgm_change_flag = true;
                                }
                            }
                        }
                    }

                    //念のため、調合シーンに入ったら、寝るフラグがたたないように強制オフにしてる。
                    time_controller.TimeCheck_flag = false;

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    SelectCompo_panel_1.SetActive(true);
                    compoBG_A.SetActive(true);
                                       
                    yes_no_panel.SetActive(false); //yes no は状態リセット
                    yes.SetActive(true);
                    no.SetActive(true);

                    text_area_compound.SetActive(false);
                    text_hikari_makecaption.SetActive(false);

                    //カメラリセット
                    //アイドルに戻るときに0に戻す。
                    trans = 0;

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //ヒカリちゃんを表示しない。デフォルト描画順
                    ReSetLive2DOrder_Default();
                    GameMgr.QuestManzokuFace = false; //おいしかった表情は、調合シーンに入るとリセットされる。

                    

                    if (pitemlist.player_extremepanel_itemlist.Count > 0 && PlayerStatus.player_extreme_kaisu > 0) //extreme_panel.extreme_kaisu
                    {
                        select_extreme_button.interactable = true;
                    }
                    else
                    {
                        select_extreme_button.interactable = false;
                    }

                    if (GameMgr.outgirl_Nowprogress)
                    {
                        select_hikarimake_button.interactable = false;
                    }
                    else
                    {
                        select_hikarimake_button.interactable = true;
                    }

                    //ピクニックイベント中は、ピクニックテキストのアイテムテキスト更新
                    if (GameMgr.picnic_event_reading_now)
                    {
                        select_hikarimake_button.interactable = false; //ピクニックイベント中はヒカリお菓子作るボタンオフ

                        if (pitemlist.player_extremepanel_itemlist.Count > 0)
                        {
                            picnic_itemText.text = GameMgr.ColorYellow + pitemlist.player_extremepanel_itemlist[0].item_SlotName + "</color>" +
                            pitemlist.player_extremepanel_itemlist[0].itemNameHyouji;
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


                    //腹減りカウント一時停止
                    girl1_status.GirlEatJudgecounter_OFF();
                    girl1_status.Girl1_touchhair_start = false; //gaze状態もリセット

                    //吹き出しも消す
                    girl1_status.DeleteHukidashiOnly();

                    //魔法環境音を止める。
                    sceneBGM.StopMagicAmbient();

                    break;


                case 7: //ヒカリが作るを開始

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 7; //ヒカリに作らせるを選択

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    compoBGA_imageHikariMake.SetActive(true);
                    recipiMemoButton.SetActive(true);

                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 
                    GameMgr.Comp_kettei_bunki = 0;

                    text_area_compound.SetActive(true);
                    _textcomp.text = hikarimake_text;
                    text_hikari_makecaption.SetActive(true);

                    //エフェクトはオフ
                    compoBGA_imageHikariMake.transform.Find("Particle_KiraExplode").gameObject.SetActive(false);

                    //ヒカリちゃんを表示する
                    ReDrawLive2DOrder_Compound();                  

                    break;

                case 8: //ヒカリお菓子作りのスタートパネルを開く               

                    
                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 8;

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    ReSetLive2DOrder_Default();

                    playeritemlist_onoff.SetActive(false);
                    recipilist_onoff.SetActive(false);
                    SelectCompo_panel_1.SetActive(false);
                    yes_no_panel.SetActive(false);
                    text_area_compound.SetActive(false);

                    Hikarimake_StartPanel.SetActive(true);

                    break;


                case 20: //魔法の選択画面を開く              


                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 20;
                    GameMgr.MagicSkillSelectStatus = 0; //魔法を使うを選択
                    GameMgr.Comp_kettei_bunki = 0;

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    //ヒカリちゃんを表示する
                    ReDrawLive2DOrder_Compound();
                    SetLive2DPos_Compound();

                    //ヒカリちゃん表示をオフ
                    //ReSetLive2DOrder_Default();                   

                    playeritemlist_onoff.SetActive(false);
                    recipilist_onoff.SetActive(false);
                    SelectCompo_panel_1.SetActive(false);
                    yes_no_panel.SetActive(false);

                    text_area_compound.SetActive(true);
                    _textcomp.text = magic_text;

                    magicskilllistController_Use.SetActive(true);
                    magicskilllistController_2.OnDefaultText(0);

                    MagicStartPanel.SetActive(true);
                    magic_compo1.SetActive(true);
                    magic_compo2.SetActive(false);
                    magic_compo3.SetActive(false);
                    magic_compo4.SetActive(false);
                    player_mp_panel.SetActive(true);
                    player_mp_panel.transform.Find("player_mp").GetComponent<Text>().text = PlayerStatus.player_mp.ToString();
                    player_mp_panel.transform.Find("player_maxmp").GetComponent<Text>().text = PlayerStatus.player_maxmp.ToString();

                    //環境音鳴らす
                    //sceneBGM.PlayMagicAmbient1();

                    break;

                case 21: //魔法選択後、アイテム選択中のステータス

                    GameMgr.compound_status = 100; //トグルや魔法選択中の状況からは変わらないので100のまま
                    GameMgr.compound_select = 21;

                    //魔法選択時の調合画面を開く
                    magic_compo1.SetActive(false);
                    magic_compo2.SetActive(true);
                    player_mp_panel.SetActive(false);

                    magicskilllistController_Use.SetActive(false);
                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 

                    recipiMemoButton.SetActive(true);
                    text_area_compound.SetActive(true);

                    //ヒカリちゃん表示をオフ
                    ReSetLive2DOrder_Default();

                    //ヒカリちゃんを表示する
                    //ReDrawLive2DOrder_Compound();

                    //環境音とめる
                    //sceneBGM.StopMagicAmbient();
                    break;

                case 22: //魔法演出画面

                    GameMgr.compound_status = 4; //演出中
                    GameMgr.compound_select = 22;

                    //魔法演出画面を開く
                    magic_compo2.SetActive(false);
                    magic_compo3.SetActive(true);

                    //スキル名表示
                    magic_compo3.transform.Find("SkillTextTemplate/Text").GetComponent<Text>().text = GameMgr.UseMagicSkill_nameHyouji + " Lv." + GameMgr.UseMagicSkillLv;

                    playeritemlist_onoff.SetActive(false);                 
                    recipiMemoButton.SetActive(false);
                    text_area_compound.SetActive(false);

                    //ヒカリちゃん表示をオフ
                    //ReSetLive2DOrder_Default();

                    //ヒカリちゃんを表示する
                    ReDrawLive2DOrder_Compound();
                    Live2DPos_CenterNow(); //このタイミングで位置はセンターにする

                    
                    break;

                case 23: //魔法調合後の完成画面

                    GameMgr.compound_status = 4; //演出中
                    GameMgr.compound_select = 23;

                    //魔法演出画面を開く
                    magic_compo3.SetActive(false);
                    magic_compo4.SetActive(true);
                  
                    text_area_compound.SetActive(false); //専用ウィンドウを表示させてるのでオフ

                    //できるアイテムを表示
                    if (GameMgr.Result_compound_success)
                    {
                        magic_compo4.transform.Find("ItemTextTemplate/Text").GetComponent<Text>().text = GameMgr.ResultItem_nameHyouji + "　が " + GameMgr.Result_Kosu + "個 できたよ！";
                    }
                    else
                    {
                        magic_compo4.transform.Find("ItemTextTemplate/Text").GetComponent<Text>().text = "失敗しちゃった・・。";
                    }

                    //ヒカリちゃん表示をオフ
                    ReSetLive2DOrder_Default();

                    //位置変更 右に。
                    //ReSetLive2DPos_Compound();
                    break;

                case 30: //魔法・スキルの習得画面を開く              


                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 30;
                    GameMgr.MagicSkillSelectStatus = 1; //魔法を習得を選択

                    //各調合画面を一度オフ
                    CompoScreenReset();

                    //ヒカリちゃん表示をオフ
                    ReSetLive2DOrder_Default();

                    playeritemlist_onoff.SetActive(false);
                    recipilist_onoff.SetActive(false);
                    SelectCompo_panel_1.SetActive(false);
                    yes_no_panel.SetActive(false);

                    text_area_compound.SetActive(true);
                    _textcomp.text = magiclearn_text;

                    magicskilllistController_Learn.SetActive(true); //魔法をおぼえるがONになった状態のリストを表示
                    magicskilllistController.OnDefaultText(1);

                    MagicLearnPanel.SetActive(true);

                    break;

                default://compound=110　最後調合するかどうかの確認中など、待機状態
                    break;
            }
        }
        else
        {
            if(WaitForCompEnd) //調合終了を聞いたタイミングで一回だけ行う処理
            {
                WaitForCompEnd = false;

                OnCancelCompound_Select();
            }
        }
    }

    private void LateUpdate()
    {
        if (GameMgr.Status_zero_readOK)
        {
            GameMgr.Status_zero_readOK = false;

            switch (GameMgr.compound_status)
            {
                case 0:

                    if (!GameMgr.EventAfter_MoveEnd) //寝るイベント発生などのフラグ
                    {
                        if (GameMgr.ResultComplete_flag != 0) //厨房から帰ってくるときの動き
                        {
                            Debug.Log("厨房から戻ってくる動き。");

                            //腹減りカウント一時停止
                            girl1_status.GirlEatJudgecounter_OFF();
                            girl1_status.ResetHukidashi();

                            //戻るアニメに遷移 これより前に、Hikari_Live2DのほうのPositionを事前に右にしておく必要がある。_character_moveではなく。
                            trans_motion = 100;
                            live2d_animator.SetInteger("trans_motion", trans_motion);
                            trans_expression = 2;
                            live2d_animator.SetInteger("trans_expression", trans_expression);

                            //
                        }
                        else
                        {
                            girl1_status.DefFaceChange(); //表情をデフォルトに戻す。                            
                        }
                    }
                    else
                    {

                        trans_motion = 11; //位置をもとに戻す。
                        live2d_animator.SetInteger("trans_motion", trans_motion);
                        girl1_status.DefFaceChange();
                    }

                    //調合後、元の状態にリセット
                    Anchor_Pos.transform.localPosition = new Vector3(0f, 0.134f, -5f); //アンカーを元位置にリセット
                    ReSetLive2DOrder_Default();
                    GameMgr.live2d_posmove_flag = false;
                    GameMgr.ResultComplete_flag = 0;

                    GameMgr.compound_select = 0;
                    GameMgr.compound_status = 110; //退避
                    break;

                case 110: //退避用

                    break;
            }
        }
    }

    void CompoScreenReset()
    {
        compoBGA_imageOri.SetActive(false);
        compoBGA_imageRecipi.SetActive(false);
        compoBGA_imageExtreme.SetActive(false);
        compoBGA_imageHikariMake.SetActive(false);
        MagicStartPanel.SetActive(false);
       
        playeritemlist_onoff.SetActive(false);
        recipilist_onoff.SetActive(false);
        magicskilllistController_Learn.SetActive(false);
        magicskilllistController_Use.SetActive(false);

        recipiMemoButton.SetActive(false);
        recipimemoController_obj.SetActive(false);
        memoResult_obj.SetActive(false);
    }

    public void OnCancelCompound_Select() //調合画面から元シーンに戻るとき
    {
        if (!GameMgr.tutorial_ON)
        {
            if (GameMgr.CompoBGMCHANGE_ON)
            {
                if (GameMgr.compobgm_change_flag == true)
                {
                    GameMgr.compobgm_change_flag = false;
                    sceneBGM.OnChangeCompoBGMFade();
                    //sceneBGM.OnMainBGM(); //即座に切り替え
                }
            }
        }

        GameMgr.CompoundSceneStartON = false;　//調合シーン終了
        GameMgr.compound_status = 0;

        ResetLive2D_DefaultStatus(); //Live2Dキャラクタの状態を元状態にもどす

        compoBG_A.SetActive(false);
    }

    //エクストリーム調合開始　お菓子パネルに現在セットされてるお菓子をベースにする処理
    void extreme_Compo_Setup()
    {
        //以下、エクストリーム用に再度パラメータを設定

        _id = database.SearchItemID(pitemlist.player_extremepanel_itemlist[0].itemID);

        Debug.Log("ベースアイテムID: " + pitemlist.player_extremepanel_itemlist[0].itemID + " " + database.items[_id].itemName);
        GameMgr.Final_list_baseitemID = 0; //エクストリームパネル上のリスト配列番号
        GameMgr.Final_toggle_baseType = 2;
        GameMgr.temp_baseitemID = _id; //アイテムDB上のリスト配列番号のこと

        GameMgr.Final_kettei_kosu1 = 1;

        GameMgr.Comp_kettei_bunki = 10; //トッピング材料から選び始める。
        pitemlistController.reset_and_DrawView_Topping();

        card_view.SelectCard_DrawView(GameMgr.Final_toggle_baseType, GameMgr.Final_list_baseitemID);
        card_view.OKCard_DrawView(GameMgr.Final_kettei_kosu1);

        itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
        itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理
    }

    //Live2D関連コマンド

    //調合シーンに入った時の、Live2D処理。

    //調合シーンに入った時の、キャラクタ位置や状態など更新
    void SetLive2DPos_Compound()
    {
        if (character_On)
        {
            //位置変更 右に。 すでに右位置にいる場合は、この処理は省略
            if (!GameMgr.live2d_posmove_flag)
            {
                Live2DPos_Compound();

                //女の子アニメーション初期化
                girl1_status.face_girl_Normal();
                girl1_status.AddMotionAnimReset();
                girl1_status.IdleMotionReset();

                //もし、リターンホーム中にすぐにシーン切り替えた場合用に、Live2D自体の位置もリセット。
                trans_motion = 11;
                live2d_animator.SetInteger("trans_motion", trans_motion);
                //live2d_animator.Play("OriCompoMotion", motion_layer_num, 0.0f); //OriCompoMotion
                live2d_animator.SetInteger("trans_nade", 0);
            }           
            
            if (GameMgr.ResultComplete_flag != 0) //なんらかの調合をしたあとに、再度ここの処理に戻った場合　生地とか作って戻ってくるとき
            {
                GameMgr.ResultComplete_flag = 0; //調合完了フラグ　画面に戻るたびリセットされる

                trans_motion = 11;
                live2d_animator.SetInteger("trans_motion", trans_motion);
                //live2d_animator.Play("OriCompoMotion", motion_layer_num, 0.0f); //OriCompoMotion
                live2d_animator.SetInteger("trans_nade", 0);

                Live2DPos_Compound(); //また元の右位置に戻す
            }
        }
    }

    //さらに、表示するときのコマンド
    void ReDrawLive2DOrder_Compound()
    {
        if (character_On)
        {
            GameMgr.CharacterTouch_ALLOFF = true;
            cubism_rendercontroller.SortingOrder = 1500; //描画順指定
        }
    }

    void ReSetLive2DOrder_Default()
    {
        if (character_On)
        {
            cubism_rendercontroller.SortingOrder = default_live2d_draworder;  //ヒカリちゃんを表示しない。デフォルト描画順 //描画順指定
        }
    }

    //_moveを使って、一時的に位置をセンターにする
    void Live2DPos_CenterNow()
    {
        if (character_On)
        {
            character_move.transform.DOMove(new Vector3(0.0f, 0, 0), 0.0f); //
            Anchor_Pos.transform.localPosition = new Vector3(0.0f, 0.134f, -5f); //これが目線デフォルト位置
        }
    }

    //さらに調合位置（右）に戻すコマンド　SetImage, NewRecipiButton.csから呼び出し
    public void Live2DPos_Compound()
    {
        if (character_On)
        {
            //character_move.transform.position = new Vector3(2.8f, 0, 0); //画面右あたり
            character_move.transform.DOMove(new Vector3(2.8f, 0, 0), 0.2f); //画面右あたり 少し間を置くことで、キャラが一瞬真ん中に映るのを防ぐ
            Anchor_Pos.transform.localPosition = new Vector3(-0.5f, 0.05f, -5f);
            GameMgr.live2d_posmove_flag = true; //位置を変更したフラグ 
        }
    }

    //_moveを真ん中位置にリセット＋live2d_posmove_flagもリセット
    /*void ResetLive2DPos_Init()
    {
        if (character_On)
        {
            Debug.Log("Live2D位置のリセット");

            if (GameMgr.ResultComplete_flag != 0) //厨房から帰ってくるときの動き
            {
                //character_move.transform.position = new Vector3(0f, 0, 0);
                character_move.transform.DOMove(new Vector3(0f, 0, 0), 0.2f); //少し間を置くことで、キャラが一瞬真ん中に映るのを防ぐ
                
                //girl1_status.DefFaceChange();
            }
            else
            {
                character_move.transform.position = new Vector3(0f, 0, 0);
            }

            ResetLive2D_DefaultStatus();
        }       
    }*/

    void ResetLive2D_DefaultStatus()
    {
        character_move.transform.position = new Vector3(0f, 0, 0);
        //character_move.transform.DOMove(new Vector3(0f, 0, 0), 0.2f); //少し間を置くことで、キャラが一瞬真ん中に映るのを防ぐ

        GameMgr.live2d_posmove_flag = false;
        Anchor_Pos.transform.localPosition = new Vector3(0.0f, 0.134f, -5f);
        girl1_status.HukidashiFlag = true;
        girl1_status.tween_start = false;
        girl1_status.IdleMotionReset();
        girl1_status.DefFaceChange();

        GameMgr.CharacterTouch_ALLON = true; //タッチもできるように。
    }

    //Live2Dのモデルと視点アンカーの位置情報も含めて、0に戻す　compound_Mainから読み出し
    public void ResetLive2D_ModelPos()
    {        
        //Live2D自体の位置もリセット。
        trans_motion = 11;
        live2d_animator.SetInteger("trans_motion", trans_motion);
        live2d_animator.SetInteger("trans_nade", 0);

        ResetLive2D_DefaultStatus();
    }

    
}

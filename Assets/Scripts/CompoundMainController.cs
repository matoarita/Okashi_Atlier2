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
    public string hikarimake_text;

    private GameObject text_hikari_makecaption;

    private BGM sceneBGM;
    private Map_Ambience map_ambience;

    private Girl1_status girl1_status;
    private Special_Quest special_quest;
    private Touch_Controller touch_controller;
    private TimeController time_controller;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

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

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    //Live2Dモデルの取得
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private int default_live2d_draworder;
    private bool character_On; //そのシーンにヒカリちゃんが存在するかどうかを検出

    private GameObject Debug_CompoIcon;

    // Use this for initialization
    void Start () {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        Debug_CompoIcon = this.transform.Find("Debug_CompIcon").gameObject;
        Debug_CompoIcon.SetActive(false);

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

        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes = canvas.transform.Find("Yes_no_Panel/Yes").gameObject;
        no = canvas.transform.Find("Yes_no_Panel/No").gameObject;

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

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

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //時間管理オブジェクトの取得
                time_controller = canvas.transform.Find("MainUIPanel/Comp/TimePanel").GetComponent<TimeController>();
                break;

        }


        //Live2Dモデルの取得 
        //調合専用処理では、Live2Dの描画順のみ操作し、位置やアクションなどには触らないようにする。LateUpdateの関係など、ややこしいので、元シーンで処理する。

        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = scene.GetRootGameObjects();

        character_On = false;
        foreach (var obj in rootObjects)
        {
            //Debug.LogFormat("RootObject = {0}", obj.name);
            if(obj.name == "CharacterRoot")
            {
                Debug.Log("character_On: ヒカリちゃん　シーン内に存在する");
                character_On = true;
                _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
                cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
                default_live2d_draworder = cubism_rendercontroller.SortingOrder;

                //タッチ判定オブジェクトの取得
                touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();
            }
            else
            {
                
            }
        }

        //各調合時のシステムメッセージ集
        originai_text = "新しくお菓子を作ろう！" + "\n" + "好きな材料を" + GameMgr.ColorYellow +
            "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";
        extreme_text = "仕上げをしよう！にいちゃん！ 一個目の材料を選んでね。";
        recipi_text = "ヒカリのお菓子手帳だよ！" + "\n" + "にいちゃんのレシピが増えたら、ここに書いてくね！";
        hikarimake_text = "にいちゃん！　ヒカリお菓子作りの手伝いしたいな！" + "\n" +
            "好きな材料を" + GameMgr.ColorYellow +
            "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(playeritemlist_onoff == null)
        {
            //プレイヤー所持アイテムリストパネルの取得
            playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

            //レシピリストパネルの取得
            recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilist_onoff.GetComponent<RecipiListController>();
        }

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

            switch (GameMgr.compound_status)
            {
                case 0:

                    break;

                case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択

                    recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                    kakuritsuPanel_obj.SetActive(true);
                    compoBG_A.SetActive(true);
                    //compoBGA_image.SetActive(false);
                    compoBGA_imageOri.SetActive(false);
                    compoBGA_imageRecipi.SetActive(true);
                    compoBGA_imageExtreme.SetActive(false);
                    compoBGA_imageHikariMake.SetActive(false);                   
                    yes_no_panel.SetActive(true);
                    yes.SetActive(false);

                    if (GameMgr.tutorial_ON != true)
                    { }
                    else
                    {
                        no.SetActive(false);
                    }

                    text_area_compound.SetActive(true);
                    //WindowOff();

                    //ヒカリちゃんを表示する
                    ReDrawLive2DPos_Compound();

                    break;

                case 2: //エクストリーム調合の処理を開始。クリック後に処理が始まる。

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 2; //トッピング調合を選択

                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    kakuritsuPanel_obj.SetActive(false);
                    compoBG_A.SetActive(true);
                    //compoBGA_image.SetActive(false);
                    compoBGA_imageOri.SetActive(false);
                    compoBGA_imageRecipi.SetActive(false);
                    compoBGA_imageExtreme.SetActive(true);
                    compoBGA_imageHikariMake.SetActive(false);

                    text_area_compound.SetActive(true);
                    //WindowOff();

                    //ヒカリちゃんを表示する
                    ReDrawLive2DPos_Compound();

                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。

                    extreme_Compo_Setup();

                    break;

                case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 3; //オリジナル調合を選択

                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    kakuritsuPanel_obj.SetActive(true);

                    compoBG_A.SetActive(true);
                    //compoBGA_image.SetActive(false);
                    compoBGA_imageOri.SetActive(true);
                    compoBGA_imageRecipi.SetActive(false);
                    compoBGA_imageExtreme.SetActive(false);
                    compoBGA_imageHikariMake.SetActive(false);
                    recipiMemoButton.SetActive(true);
                    recipimemoController_obj.SetActive(false);
                    memoResult_obj.SetActive(false);

                    text_area_compound.SetActive(true);
                    //WindowOff();

                    //ヒカリちゃんを表示する
                    ReDrawLive2DPos_Compound();

                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 

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
                        if (GameMgr.CompoBGMCHANGE_ON)
                        {
                            if (GameMgr.compobgm_change_flag != true)
                            {
                                sceneBGM.OnCompoundBGM();
                                GameMgr.compobgm_change_flag = true;
                            }
                        }
                    }

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 6;

                    switch (SceneManager.GetActiveScene().name)
                    {
                        case "Compound":

                            time_controller.TimeCheck_flag = false;
                            break;
                    }

                    playeritemlist_onoff.SetActive(false);
                    recipilist_onoff.SetActive(false);
                    kakuritsuPanel_obj.SetActive(false);

                    SelectCompo_panel_1.SetActive(true);
                    compoBG_A.SetActive(true);
                    //compoBGA_image.SetActive(true);
                    compoBGA_imageOri.SetActive(false);
                    compoBGA_imageRecipi.SetActive(false);
                    compoBGA_imageExtreme.SetActive(false);
                    compoBGA_imageHikariMake.SetActive(false);
                    
                    yes_no_panel.SetActive(false);

                    text_area_compound.SetActive(false);
                    text_hikari_makecaption.SetActive(false);
                    //WindowOff();

                    //カメラリセット
                    //アイドルに戻るときに0に戻す。
                    trans = 0;

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //ヒカリちゃんを表示しない。デフォルト描画順
                    //ReSetLive2DOrder_Default();
                    GameMgr.QuestManzokuFace = false; //おいしかった表情は、調合シーンに入るとリセットされる。

                    recipiMemoButton.SetActive(false);
                    recipimemoController_obj.SetActive(false);
                    memoResult_obj.SetActive(false);

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

                    break;


                case 7: //ヒカリが作るを開始

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 7; //ヒカリに作らせるを選択

                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    kakuritsuPanel_obj.SetActive(true);

                    compoBG_A.SetActive(true);
                    //compoBGA_image.SetActive(false);
                    compoBGA_imageOri.SetActive(false);
                    compoBGA_imageRecipi.SetActive(false);
                    compoBGA_imageExtreme.SetActive(false);
                    compoBGA_imageHikariMake.SetActive(true);
                    recipiMemoButton.SetActive(true);
                    recipimemoController_obj.SetActive(false);
                    memoResult_obj.SetActive(false);

                    text_area_compound.SetActive(true);
                    //WindowOff();
                    _textcomp.text = hikarimake_text;
                    text_hikari_makecaption.SetActive(true);

                    //エフェクトはオフ
                    compoBGA_imageHikariMake.transform.Find("Particle_KiraExplode").gameObject.SetActive(false);

                    //ヒカリちゃんを表示する
                    ReDrawLive2DPos_Compound();

                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 

                    break;

                case 8: //ヒカリお菓子作りのスタートパネルを開く               

                    Hikarimake_StartPanel.SetActive(true);

                    GameMgr.compound_status = 4; //調合シーンに入っています、というフラグ
                    GameMgr.compound_select = 8;

                    ReSetLive2DOrder_Default();

                    playeritemlist_onoff.SetActive(false);
                    recipilist_onoff.SetActive(false);
                    kakuritsuPanel_obj.SetActive(false);

                    SelectCompo_panel_1.SetActive(false);
                    compoBG_A.SetActive(true);
                    //compoBGA_image.SetActive(true);
                    compoBGA_imageOri.SetActive(false);
                    compoBGA_imageRecipi.SetActive(false);
                    compoBGA_imageExtreme.SetActive(false);
                    compoBGA_imageHikariMake.SetActive(false);
                    yes_no_panel.SetActive(false);

                    text_area_compound.SetActive(false);
                    //WindowOff();
                    text_hikari_makecaption.SetActive(false);

                    recipiMemoButton.SetActive(false);
                    recipimemoController_obj.SetActive(false);
                    memoResult_obj.SetActive(false);

                    /*if (pitemlist.player_extremepanel_itemlist.Count > 0 && PlayerStatus.player_extreme_kaisu > 0) //extreme_panel.extreme_kaisu
                    {
                        select_extreme_button.interactable = true;
                    }
                    else
                    {
                        select_extreme_button.interactable = false;
                    }*/

                    break;
            }
        }
    }

    public void OnCancelCompound_Select() //調合画面から戻るとき
    {
        GameMgr.CompoundSceneStartON = false;　//調合シーン終了
        GameMgr.compound_status = 0;
    }

    //エクストリーム調合開始　お菓子パネルに現在セットされてるお菓子をベースにする処理
    void extreme_Compo_Setup()
    {
        //以下、エクストリーム用に再度パラメータを設定
        GameMgr.extremepanel_on = true;

        if (exp_Controller._temp_extreme_itemtype == 0) //デフォルトアイテムの場合
        {
            pitemlistController.final_base_kettei_item = database.items[exp_Controller._temp_extreme_id].itemID;
        }
        else if (exp_Controller._temp_extreme_itemtype == 1) //オリジナルアイテムの場合
        {
            pitemlistController.final_base_kettei_item = pitemlist.player_originalitemlist[exp_Controller._temp_extreme_id].itemID;
        }
        else if (exp_Controller._temp_extreme_itemtype == 2) //エクストリームパネルに設定したアイテムの場合　通常これのみ使用
        {
            pitemlistController.final_base_kettei_item = pitemlist.player_extremepanel_itemlist[exp_Controller._temp_extreme_id].itemID;
        }

        pitemlistController.base_kettei_item = exp_Controller._temp_extreme_id;
        pitemlistController._base_toggle_type = exp_Controller._temp_extreme_itemtype;

        pitemlistController.final_base_kettei_kosu = 1;

        pitemlistController.kettei1_bunki = 10; //トッピング材料から選び始める。
        pitemlistController.reset_and_DrawView_Topping();

        card_view.SelectCard_DrawView(pitemlistController._base_toggle_type, pitemlistController.base_kettei_item);
        card_view.OKCard_DrawView(pitemlistController.final_base_kettei_kosu);

        itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
        itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理
    }

    //Live2D関連コマンド

    //調合シーンに入った時の、Live2D描画順の処理。位置動きは触らない。

    //さらに、表示するときのコマンド
    void ReDrawLive2DPos_Compound()
    {
        if (character_On)
        {
            touch_controller.Touch_OnAllOFF();
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
}

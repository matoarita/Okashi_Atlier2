using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Hiroba1_Main_Controller : MonoBehaviour {

    private GameObject text_area;
    private Text _text;
    private bool text_area_hyouji_on;

    private SceneInitSetting sceneinit_setting;

    private SoundController sc;

    private GameObject npc1_toggle_obj;
    private GameObject npc2_toggle_obj;
    private GameObject npc3_toggle_obj;
    private GameObject npc4_toggle_obj;
    private GameObject npc5_toggle_obj;
    private GameObject npc6_toggle_obj;
    private GameObject npc7_toggle_obj;
    private GameObject npc8_toggle_obj;

    private Toggle npc1_toggle;
    private Toggle npc2_toggle;
    private Toggle npc3_toggle;
    private Toggle npc4_toggle;
    private Toggle npc5_toggle;
    private Toggle npc6_toggle;
    private Toggle npc7_toggle;
    private Toggle npc8_toggle;

    private Text npc1_toggle_text;
    private Text npc2_toggle_text;
    private Text npc3_toggle_text;
    private Text npc4_toggle_text;
    private Text npc5_toggle_text;
    private Text npc6_toggle_text;
    private Text npc7_toggle_text;
    private Text npc8_toggle_text;

    private GameObject npc_subview_obj;

    private ItemMatPlaceDataBase matplace_database;

    private PlayerItemList pitemlist;

    private TimeController time_controller;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject mainlist_controller_obj;

    private GameObject sceneplace_namepanel_obj;
    private ScenePlaceNamePanel sceneplace_namepanel;

    private GameObject back_atlier_obj;
    private GameObject scene_black_effect;
    private GameObject fadeout_panel_obj;

    private GameObject Character_panel;
    private List<GameObject> Character_list = new List<GameObject>();

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private BGM sceneBGM;
    private bool bgm_change_flag;

    private int ev_id;

    private int rndnum;
    private string default_scenetext;

    private bool StartRead;
    private bool check_event;

    //private bool map_move;
    private int map_move_num;

    private int _place_num;
    private int talkrot;

    // Use this for initialization
    void Start () {

        //InitSetup();
        
    }

    public void InitSetup()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 60;

        //Debug.Log("Hiroba scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();
        text_area_hyouji_on = false;

        sceneplace_namepanel_obj = canvas.transform.Find("MainListPanel/ScenePlaceNamePanel").gameObject;
        sceneplace_namepanel = sceneplace_namepanel_obj.GetComponent<ScenePlaceNamePanel>();
        sceneplace_namepanel_obj.SetActive(false);

        Character_panel = canvas.transform.Find("Character_Panel").gameObject; //広場シーンのみ、ゲームキャラと宴の表示の切り替えをこのパネルでやっている。
        //ので、GameMgr.utage_charaHyouji_flagは使ってないので、注意。

        back_atlier_obj = canvas.transform.Find("BackHomeButtonPanel").gameObject;
        back_atlier_obj.SetActive(false);

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f); //黒い画面は最初ON　イベントチェックしてからオフ

        fadeout_panel_obj = canvas.transform.Find("FadeOutPanel").gameObject;
        fadeout_panel_obj.GetComponent<CanvasGroup>().DOFade(0, 0.0f); //白い画面はオフ

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //移動用リストオブジェクトの初期化
        foreach (Transform child in canvas.transform.Find("MainListPanel").transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
        }

        //キャラクタ表示の初期化
        Character_list.Clear();
        foreach (Transform child in Character_panel.transform.Find("CharacterImage").transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);        
            Character_list.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false; //BGMをmainListControllerの宴のほうで変えたかどうかのフラグ。変えてた場合、trueで、宴終了後に元のBGMに切り替える。       

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        GameMgr.Utage_MapMoveON = false;
        GameMgr.utage_charaHyouji_flag = false;

        GameMgr.hiroba_event_startblack = false;

        StartRead = false;
        check_event = false;
        talkrot = 0;

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    

    void InitSetting()
    {
        if (playeritemlist_onoff == null)
        {
            //プレイヤー所持アイテムリストパネルの取得
            playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

            //レシピリストパネルの取得
            recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilist_onoff.GetComponent<RecipiListController>();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateHiroba1MainScene()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
            sceneBGM.NowFadeVolumeONBGM();
        }

        //強制的に発生するイベントをチェック。はじめてショップへきた時など
        EventCheck();

        //宴途中でホワイトをONにする　フェードアウト演出用
        if (GameMgr.Utage_FadeOutWhiteON)
        {
            //白からフェードイン        
            fadeout_panel_obj.GetComponent<CanvasGroup>().alpha = 1;
        }

        //宴途中でブラックをオフにする 宴のBGを最初に表示したい場合などに使用
        if (GameMgr.Utage_SceneStart_BlackON)
        {
            GameMgr.Utage_SceneStart_BlackON = false;
            scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //こっちはフェードで。
        }

        //宴途中でブラックをオンにする ドアをあけて会場へ移動する演出用
        if (GameMgr.Utage_SceneEnd_BlackON)
        {
            GameMgr.Utage_SceneEnd_BlackON = false;
            scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f);
        }

        if (GameMgr.Utage_MapMoveON) //マップ移動中は、ウィンドウオフのまま
        {
            //Debug.Log("マップ移動中　ウィンドウオフのまま");
            WindowOff();
        }
        else
        {
            //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
            if (GameMgr.scenario_ON == true)
            {
                WindowOff();
            }
            else
            {
                switch (GameMgr.Scene_Status)
                {
                    case 0:

                        if (!text_area_hyouji_on)
                        {
                            text_area.SetActive(false);
                        }
                        else
                        {
                            text_area.SetActive(true);
                        }

                        //黒をオフ
                        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 0.0f);

                        //placename_panel.SetActive(true);
                        mainlist_controller_obj.SetActive(true);
                        sceneplace_namepanel_obj.SetActive(true);
                        back_atlier_obj.SetActive(true);

                        sceneBGM.MuteOFFBGM();

                        GameMgr.Scene_Status = 100;
                        GameMgr.Scene_Select = 0;

                        break;

                    case 100: //退避

                        break;

                    default:

                        break;
                }
            }
        }
    }

    void WindowOff()
    {
        text_area.SetActive(false);
        //placename_panel.SetActive(false);
        mainlist_controller_obj.SetActive(false);
        sceneplace_namepanel_obj.SetActive(false);
        back_atlier_obj.SetActive(false);
    }

    void EventCheck()
    {
        //強制的に発生するイベントをチェック。はじめてショップへきた時など
        if (!check_event)
        {
            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark":

                    if (!GameMgr.NPCHiroba_HikarieventList[0]) //はじめて中央噴水へきた。
                    {
                        GameMgr.NPCHiroba_HikarieventList[0] = true;

                        GameMgr.hiroba_event_placeNum = 2000; //ヒカリの広場でのイベント
                        GameMgr.hiroba_event_ID = 220000;

                        //BGMかえる
                        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                        sceneBGM.StopAmbient();
                        bgm_change_flag = true;

                        //GameMgr.Utage_MapMoveON = true;
                        //map_move_num = 3000;
                        //GameMgr.Utage_MapMoveBlackON = true;

                        check_event = true;

                        matplace_database.matPlaceKaikin("Or_Hiroba1"); //解禁

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Spring_Entrance":

                    if (!GameMgr.NPCHiroba_eventList[1000]) //イリスさんと再開
                    {                       
                        GameMgr.NPCHiroba_eventList[1000] = true;

                        GameMgr.hiroba_event_placeNum = 1500; //お花屋さんイベント
                        GameMgr.hiroba_event_ID = 10000;

                        //BGMかえる
                        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                        bgm_change_flag = true;

                        check_event = true;

                        matplace_database.matPlaceKaikin("Or_HirobaEnter_A1"); //解禁

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Spring_Shoping_Moll":

                    if (!GameMgr.NPCHiroba_eventList[1030]) //アマクサと再開
                    {
                        GameMgr.NPCHiroba_eventList[1030] = true;

                        GameMgr.hiroba_event_placeNum = 1610; //アマクサイベント
                        GameMgr.hiroba_event_ID = 10000;

                        //メイン画面にもどったときに、イベントを発生させるフラグをON
                        GameMgr.CompoundEvent_num[100] = true;
                        GameMgr.CompoundEvent_flag = true;

                        //BGMかえる
                        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                        bgm_change_flag = true;

                        check_event = true;

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Spring_Oku": //花園

                    if (!GameMgr.NPCHiroba_HikarieventList[120]) //はじめて秘密の花園へきた。
                    {
                        GameMgr.NPCHiroba_HikarieventList[120] = true;

                        GameMgr.hiroba_event_placeNum = 2000; //ヒカリの広場でのイベント
                        GameMgr.hiroba_event_ID = 230100;

                        check_event = true;

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Spring_RotenStreet": //春エリア　露店通り

                    if (GameMgr.NPCMagic_eventList[0] && !GameMgr.NPCMagic_eventList[1]) //コンテスト後くやしいぜ～が発生してるが、まだミラボ先生には会ってない
                    {
                        GameMgr.NPCMagic_eventList[1] = true;

                        GameMgr.hiroba_event_placeNum = 5000; //魔法先生のイベント　開始は広場から
                        GameMgr.hiroba_event_ID = 1;

                        GameMgr.Utage_MapMoveON = true;
                        map_move_num = 2000;
                        GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ

                        //BGMかえる
                        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                        sceneBGM.StopAmbient();
                        bgm_change_flag = true;


                        check_event = true;

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Spring_Out_alter": //春のさいだんマップ

                    if (!GameMgr.NPCHiroba_HikarieventList[150]) //
                    {
                        GameMgr.NPCHiroba_HikarieventList[150] = true;

                        GameMgr.hiroba_event_placeNum = 2000; //ヒカリの広場でのイベント
                        GameMgr.hiroba_event_ID = 200030;
                        GameMgr.hiroba_event_startblack = true; //シーン背景最初は黒のままで、宴のBG表示されてからオフにする。

                        check_event = true;

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Summer_ThemePark_Map": //遊園地入口マップ

                    if (!GameMgr.NPCHiroba_HikarieventList[250]) //はじめてソーダアイランドきた
                    {
                        GameMgr.NPCHiroba_HikarieventList[250] = true;

                        matplace_database.ReSetMapFlagString("Or_Hiroba_Summer_SodaIsland", 1);

                        GameMgr.hiroba_event_placeNum = 2000; //ヒカリの広場でのイベント
                        GameMgr.hiroba_event_ID = 290000;

                        check_event = true;

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Summer_ThemePark_AquariumEntrance": //水族館入口

                    if (!GameMgr.NPCHiroba_HikarieventList[300]) //はじめて水族館へきた。
                    {
                        GameMgr.NPCHiroba_HikarieventList[300] = true;

                        GameMgr.hiroba_event_placeNum = 2000; //ヒカリの広場でのイベント
                        GameMgr.hiroba_event_ID = 300000;

                        check_event = true;

                        EventReadingStart();
                    }
                    break;

                case "Or_Hiroba_Winter_altar": //冬のさいだんマップ　禁忌の図書館

                    if (!GameMgr.NPCHiroba_HikarieventList[151]) //
                    {
                        GameMgr.NPCHiroba_HikarieventList[151] = true;

                        GameMgr.hiroba_event_placeNum = 2000; //ヒカリの広場でのイベント
                        GameMgr.hiroba_event_ID = 200040;
                        //GameMgr.hiroba_event_startblack = true; //シーン背景最初は黒のままで、宴のBG表示されてからオフにする。

                        check_event = true;

                        EventReadingStart();
                    }
                    break;
            }           
        }
    }

    public void text_scenario()
    {
        _text.text = default_scenetext;
    }


    void EventText()
    {
        switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
        {
            case 40: //ドーナツイベント時
                if (GameMgr.hiroba_event_end[8])
                {
                    _text.text = "ピンクのドーナツを作ってみよう！";
                }
                else
                {
                    if (GameMgr.hiroba_event_end[1])
                    {
                        _text.text = "さて、村長さんにも話を聞いたし。行くところは..。";
                    }
                    else
                    {
                        if (GameMgr.hiroba_event_end[0])
                        {
                            _text.text = "色んな人に話を聞いてみよう！";
                        }
                        else
                        {
                            _text.text = "ここは、村の中央広場のようだ。いろんな人がいるみたいだ。";
                        }
                    }
                }
                break;

            default:

                _text.text = "ここは、オランジーナの街の中央広場だ。" + "\n" + "大きい噴水がある。";
                break;
        }
    }

    //
    void EventReadingStart()
    {
        StartCoroutine("EventReading");
    }

    IEnumerator EventReading()
    {
        GameMgr.hiroba_event_flag = true;
        GameMgr.scenario_ON = true;

        GameMgr.Scene_Select = 1000; //シナリオイベント読み中の状態
        GameMgr.Scene_Status = 1000;

        //キャラ表示パネルも一時的にオフ
        Character_panel.GetComponent<CanvasGroup>().DOFade(0, 0.0f);

        //通常シーンブラックはオフだが、マップの背景を見せたくない場合（宴のBGを使う場合）に最初黒を残す処理
        if (GameMgr.hiroba_event_startblack)
        { }
        else
        {
            //ここのタイミングを黒をOFF
            scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 0.0f); //ブラックをオフ
        }

        //Debug.Log("広場イベント　読み中");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;
        GameMgr.hiroba_event_startblack = false;

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

            GameMgr.Scene_Select = 0; //何もしていない状態
            GameMgr.Scene_Status = 0;

            MapMove(0);           
        }
        else
        {

            GameMgr.Scene_Select = 0; //何もしていない状態
            GameMgr.Scene_Status = 0;

            check_event = false;

            //読み終わったら、またウィンドウなどを元に戻す。
            if (!text_area_hyouji_on)
            {
                text_area.SetActive(false);
            }
            else
            {
                text_area.SetActive(true);
            }
            mainlist_controller_obj.SetActive(true);

            //もし遊園地にのってた場合、メイン画面にもどって喜んだイベント発生
            if (GameMgr.AmusePlayCount >= 1)
            {
                AmusePlaying_AfterEvent(); //メイン画面にもどったときに、イベントを発生させるフラグをON 
            }

            //キャラ表示パネルを戻す
            Character_panel.GetComponent<CanvasGroup>().DOFade(1, 0.0f);

            //音を戻す。
            if (bgm_change_flag)
            {
                bgm_change_flag = false;
                sceneBGM.FadeInBGM(GameMgr.System_default_sceneFadeBGMTime);
                sceneBGM.PlayAmbient(9999); //指定なしで、マップデフォルトのアンビエントをまた鳴らす
            }

            ToggleFlagCheck();

            //テキストあらたに変わってたら更新
            SceneToggleDefaultSetup();
            text_scenario(); //テキストの更新

            
        }
    }

    void AmusePlaying_AfterEvent()
    {
        //メイン画面にもどったときに、イベントを発生させるフラグをON
        GameMgr.CompoundEvent_num[130] = true; //イベント番号のこと
        GameMgr.CompoundEvent_flag = true;

        GameMgr.SubEvAfterHeartGet = true; //イベント終了後に、ハートを獲得する演出などがある場合はON。
        GameMgr.SubEvAfterHeartGet_num = 130;
    }

    void MapMove(int _status)
    {
        switch (map_move_num)
        {
            case 0: //ブルートパーズの花畑へ移動

                _place_num = matplace_database.SearchMapString("Bluetopaz_Garden");
                GameMgr.Select_place_num = _place_num;
                GameMgr.Select_place_name = matplace_database.matplace_lists[_place_num].placeName;
                GameMgr.Select_place_day = matplace_database.matplace_lists[_place_num].placeDay;

                //GameMgr.SceneSelectNum = 13;

                //音量フェードアウト
                sceneBGM.FadeOutBGM(0.5f);

                if (_status == 0)
                {
                    StartCoroutine("WaitForGotoMap");
                }
                else
                {
                    //_status == 1だと即時移動 宴からの移動なら、0でOK
                    GoAreaMove("GetMaterial");
                    //FadeManager.Instance.LoadScene("GetMaterial", GameMgr.SceneFadeTime);
                }
                break;

            case 1410: //女王の間へ移動

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(1));
                break;

            case 1510: //ソーダアイランドへ移動

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(2));
                break;

            case 1520: //ゴンドラ乗り場へ移動

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(3));
                break;

            case 1530: //水族館へ移動

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(4));
                break;

            case 1540: //水族館から外へ移動

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(5));
                break;

            case 2000: //光先生にはじめてあい、魔法教えてもらう。

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(6));                
                break;

            case 3000: //噴水後、すぐにコンテストへ移動

                //音量フェードアウト
                sceneBGM.FadeOutBGM(1.0f);

                StartCoroutine(WaitForGotoMap2(10));
                break;
        }
    }

    IEnumerator WaitForGotoMap()
    {
        yield return new WaitForSeconds(0.5f); //1秒待つ

        GoAreaMove("GetMaterial");
        //FadeManager.Instance.LoadScene("GetMaterial", GameMgr.SceneFadeTime);
    }

    IEnumerator WaitForGotoMap2(int _status)
    {
        yield return new WaitForSeconds(1.0f); //1秒待つ

        switch (_status)
        {
            case 1:

                On_QueenEnterActive();
                break;

            case 2:

                On_Active70();
                break;

            case 3:

                //On_Active54();
                On_BackHomeActive02();
                break;

            case 4:

                On_Active76();
                break;

            case 5:

                On_Active75();
                break;

            case 6:

                GameMgr.SceneSelectNum = 30;
                FadeManager.Instance.LoadScene("Or_NPC_MagicHouse", GameMgr.SceneFadeTime);
                break;

            case 10:

                On_ContestOutActive01(); //春コンテスト会場外へ移動
                //GameMgr.SceneSelectNum = 0;
                //FadeManager.Instance.LoadScene("Or_NPC_MagicHouse", GameMgr.SceneFadeTime);
                break;

            default:

                On_Active75();
                break;
        }
        
    }


    //
    //広場 各NPCのイベント実行・フラグ管理
    //

    //Npc1
    public void OnNPC1_toggle()
    {
        /*if (npc1_toggle.isOn == true)
        {
            npc1_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                //On_Active31();
                On_ContestOutActive01(); //会場前へ
                break;

            case "Or_Hiroba_CentralPark2":

                On_Active01();
                break;

            case "Or_Hiroba_CentralPark_Left": //中央噴水　左

                On_Active01();
                break;

            case "Or_Hiroba_CentralPark_Right": //中央噴水　右

                if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_51) //PlayerStatus.player_ninki_param < GameMgr.System_StarBlockLv_02
                {
                    On_Active1701(); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[2])
                    {
                        On_Active03();
                    }
                    else
                    {
                        //GameMgr.NPCHiroba_blockReleaseList[2] = true; //
                        GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                        GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
                        On_BlockReleaseActive1(110);
                    }                   
                }

                break;

            case "Or_Hiroba_CentralPark_Castle_Street": //中央噴水　お城前

                On_Active31();
                break;

            case "Or_Hiroba_Spring_Entrance":

                On_Active07();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll":

                On_ShopActive01();
                break;

            case "Or_Hiroba_Spring_UraStreet":

                On_NPC_MagicActive04();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1600_Roten_Ringo();
                break;

            case "Or_Hiroba_Spring_Out_MagicHouseLake":

                On_NPC_MagicActive04();
                break;

            case "Or_Hiroba_Summer_Entrance":

                On_BarActive02();
                break;

            case "Or_Hiroba_Summer_MainStreet":

                On_ShopActive02();
                break;

            case "Or_Hiroba_Summer_MainStreet_Shop":

                On_ShopActive02();
                break;

            case "Or_Hiroba_Summer_MainStreet_Gondora":

                On_Active1510_soda_guide();
                break;

            case "Or_Hiroba_Summer_ThemePark_Map":

                On_Active71();
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter":

                On_ContestActive02();
                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA":

                On_NPC_MagicActive02();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active1550_Amupark_biking();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaMae":

                On_Active1560_Amupark_kanransha();
                break;

            case "Or_Hiroba_Summer_ThemePark_Pool":

                On_Active1570_Amupark_pool();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMae":

                On_Active1530_aquarium_reception();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumEntrance":

                On_Active77();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMainHall":

                On_Active78();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMain2F":

                On_Active80();
                break;

            case "Or_Hiroba_Autumn_Entrance":

                On_Active106();
                break;

            case "Or_Hiroba_Autumn_Entrance_bridge":

                On_Active101();
                break;

            case "Or_Hiroba_Autumn_MainStreet":

                //On_Active102();
                On_ContestOutActive03();
                break;

            case "Or_Hiroba_Autumn_DepartMae":

                On_ContestOutActive03();
                break;

            case "Or_Hiroba_Autumn_BarStreet":

                On_BarActive03();
                break;

            case "Or_Hiroba_Autumn_UraStreet2":

                On_NPC_MagicActive03();
                break;

            case "Or_Hiroba_Autumn_Riverside":

                On_Active100();
                break;

            case "Or_Hiroba_Winter_Entrance":

                On_Active150();
                break;

            case "Or_Hiroba_Winter_EntranceHiroba":

                //On_Active151(); //ランプ街道削除
                On_Active152();
                break;

            case "Or_Hiroba_Winter_Street1":

                On_Active152();
                break;

            case "Or_Hiroba_Winter_MainStreet":

                On_Active153();
                break;

            case "Or_Hiroba_Winter_MainHiroba":

                On_Active154();
                break;

            case "Or_Hiroba_Winter_Street2":

                if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_11) //エデンレシピが隠されている祭壇へ
                {
                    On_Active2000(200010, false); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[11])
                    {
                        On_Active155();
                    }
                    else
                    {
                        GameMgr.NPCHiroba_blockReleaseList[11] = true; //
                        On_BlockReleaseActive1(0); //hiroba_numの指定
                    }
                }              
                break;

            case "Or_Hiroba_Winter_ContestBridge": //冬の橋

                //On_ContestActive04();

                //エデンレシピ月の祭壇へ
                On_Active156();                
                break;

            case "Or_Hiroba_Winter_Street3":

                On_Active161();
                break;

            case "Or_Hiroba_MainGate_Street":

                On_Active30();
                break;

            case "Or_Hiroba_MainGate_Street2_hiroba":

                On_Active200();
                break;

            case "Or_Hiroba_MainGate_Entrance":

                On_Active203();
                break;

            case "Or_Hiroba_Catsle_Garden":

                On_Active301();
                break;

            default:

                On_Active100();
                break;
        }
        //}
    }

    //Npc2
    public void OnNPC2_toggle()
    {
        /*if (npc2_toggle.isOn == true)
        {
            npc2_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水でToggle2を押した

                //On_Active32();
                On_Active01();
                break;

            case "Or_Hiroba_CentralPark2":

                On_Active04();
                break;

            case "Or_Hiroba_CentralPark_Castle_Street": //中央噴水　お城前

                On_Active32();
                break;

            case "Or_Hiroba_Spring_Entrance":

                On_Active05();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll":

                On_BarActive01();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1601_Roten_PotatoButter();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                On_Active1604_Roten_JoukenKyobai();
                break;

            case "Or_Hiroba_Summer_MainStreet":

                On_Active52();
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter":

                On_Active73();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active75();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMainHall":

                On_Active79();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMain2F":

                On_BarActive02();
                break;

            case "Or_Hiroba_Autumn_Entrance":

                On_Active1001_Nuno();
                break;

            case "Or_Hiroba_Autumn_MainStreet":

                On_Active1002_Kinoko();
                break;

            case "Or_Hiroba_Winter_EntranceHiroba":

                On_Active1003_Basan();
                break;

            case "Or_Hiroba_Winter_Street1":

                On_Active1005_Niji_girl();
                break;

            case "Or_Hiroba_Winter_MainHiroba":

                On_Active160();
                break;

            case "Or_Hiroba_MainGate_Big_hiroba":

                On_Active201();
                break;

            default:

                On_Active1001_Nuno();
                break;
        }
        //}
    }

    //Npc3
    public void OnNPC3_toggle()
    {
        /*if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水でToggle3を押した

                //On_Active03();
                if (GameMgr.outgirl_Nowprogress)
                {
                    GameMgr.hiroba_event_placeNum = 2000; //
                    GameMgr.hiroba_event_ID = 200100;

                    EventReadingStart();

                    /*if (text_area_hyouji_on)
                    {
                        _text.text = "ヒカリがいないから、行ってもしょうがないな・・。";
                    }*/
                }
                else
                {
                    On_Active10();
                }
                break;

            case "Or_Hiroba_CentralPark2":

                //On_Active31();
                On_Active30();
                //On_BackHomeActive02();
                break;

            case "Or_Hiroba_Spring_Entrance":

                //On_Active31();
                On_Active30();
                //On_BackHomeActive02();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll":

                On_Active01();
                break;

            case "Or_Hiroba_Spring_Oku":

                On_Active07();
                break;

            case "Or_Hiroba_Spring_UraStreet":

                On_Active07();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active30();
                //On_BackHomeActive02();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                On_Active10();
                break;

            case "Or_Hiroba_Spring_BarStreet":

                On_Active07();
                break;

            case "Or_Hiroba_Spring_Flower_Campo":

                On_Active08();
                break;

            case "Or_Hiroba_Spring_Oku_Garden":

                On_Active09();
                break;            

            case "Or_Hiroba_Spring_Out_Plain":

                On_Active15();
                break;

            case "Or_Hiroba_Spring_Out_MagicHouseLake":

                On_Active16();
                break;

            case "Or_Hiroba_Spring_Out_alter":

                On_Active11();
                break;

            case "Or_Hiroba_Summer_Entrance":

                //On_Active32();
                On_BackHomeActive02();
                break;

            case "Or_Hiroba_Summer_Street":

                On_Active02();
                break;

            case "Or_Hiroba_Summer_MainStreet":

                //On_Active50();
                On_Active02();
                break;

            case "Or_Hiroba_Summer_MainStreet_Shop":

                On_Active51();
                break;

            case "Or_Hiroba_Summer_MainStreet_Oku":

                On_Active51();
                break;

            case "Or_Hiroba_Summer_MainStreet_Gondora":

                On_Active53();
                break;

            case "Or_Hiroba_Summer_ThemePark_Map":

                On_Active1520_soda_guide_return();
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter":

                On_Active70();
                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA":

                On_Active71();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active71();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaMae":

                On_Active73();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMae":

                On_Active73();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumEntrance":

                //On_Active75();
                On_Active1540_aquarium_return();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMainHall":

                On_Active76();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMain2F":

                On_Active77();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMiniHall":

                On_Active77();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumBigWhale":

                On_Active78();
                break;

            case "Or_Hiroba_Summer_ThemePark_Pool":

                On_Active74();
                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA_2":

                On_Active72();
                break;

            case "Or_Hiroba_Summer_ThemePark_beachMae":

                On_Active90();
                break;

            case "Or_Hiroba_Autumn_Entrance":

                //On_Active32();
                On_BackHomeActive02();
                break;

            case "Or_Hiroba_Autumn_Entrance_bridge":

                On_Active106();
                break;

            case "Or_Hiroba_Autumn_MainStreet":

                On_Active100();
                break;

            case "Or_Hiroba_Autumn_DepartMae":

                On_Active101();
                break;

            case "Or_Hiroba_Autumn_BarStreet":

                On_Active102();
                break;

            case "Or_Hiroba_Autumn_UraStreet":

                //On_Active101();
                On_Active102();
                break;

            case "Or_Hiroba_Autumn_UraStreet2":

                On_Active104();
                break;

            case "Or_Hiroba_Autumn_Riverside":

                On_Active03();
                break;

            case "Or_Hiroba_Winter_Entrance":

                //On_Active31();
                On_BackHomeActive02();
                break;

            case "Or_Hiroba_Winter_EntranceHiroba":

                On_Active04();
                break;

            case "Or_Hiroba_Winter_Street1":

                On_Active150();
                break;

            case "Or_Hiroba_Winter_MainStreet":

                //On_Active151(); //ランプ街道削除
                On_Active150();
                break;

            case "Or_Hiroba_Winter_MainHiroba":

                On_Active152();
                break;

            case "Or_Hiroba_Winter_Street2":

                On_Active153();
                break;

            case "Or_Hiroba_Winter_ContestBridge":

                On_Active154();
                break;

            case "Or_Hiroba_Winter_altar":

                On_Active155();
                break;

            case "Or_Hiroba_Winter_Street3":

                On_Active153();
                break;

            case "Or_Hiroba_Winter_PatissierHouseMae":

                On_Active160();
                break;           

            case "Or_Hiroba_MainGate_Street":

                On_Active201();
                break;

            case "Or_Hiroba_MainGate_Street2_hiroba":

                On_Active203();                
                break;

            case "Or_Hiroba_MainGate_Entrance":

                On_StationActive01();
                break;

            case "Or_Hiroba_MainGate_Big_hiroba":
                
                On_Active202();
                break;

            case "Or_Hiroba_Catsle_Garden":

                On_Active33();
                break;

            case "Or_Hiroba_Catsle_MainStreet":

                On_Active300();
                break;

            case "Or_Hiroba_Catsle_MainEntrance":

                On_BackHomeActive02();
                //On_Active301();
                break;

            default:

                On_Active1002_Kinoko();
                break;
        }
        //}
    }

    //Npc4
    public void OnNPC4_toggle()
    {
        /*if (npc4_toggle.isOn == true)
        {
            npc4_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                //On_Active05();
                On_ShopActive01();
                break;

            case "Or_Hiroba_CentralPark2": //散歩道

                On_Active2001();
                break;

            case "Or_Hiroba_CentralPark_Left": //中央噴水　左

                On_Active05();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //ハートレベルがいくつか必要

                if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_01)
                {
                    On_Active2000(200000, false); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[0])
                    {
                        On_Active08();
                    }
                    else
                    {
                        GameMgr.NPCHiroba_blockReleaseList[0] = true; //秘密の花園が解放されるイベント
                        On_BlockReleaseActive1(10); //hiroba_numの指定
                    }                   
                }

                break;

            case "Or_Hiroba_Spring_Oku":

                On_ContestOutActive01();
                break;

            case "Or_Hiroba_Spring_BarStreet":

                On_Active10();
                break;

            case "Or_Hiroba_Spring_UraStreet":

                On_Active15(); //On_Active07
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active11();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_10) //エデンレシピが隠されている祭壇へ
                {
                    On_Active2000(200005, false); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[10])
                    {
                        //エデンレシピ星の祭壇へ
                        On_Active18();
                        
                    }
                    else
                    {
                        GameMgr.NPCHiroba_blockReleaseList[10] = true; //
                        On_BlockReleaseActive1(0); //hiroba_numの指定
                    }
                }
                break;

            case "Or_Hiroba_Spring_Flower_Campo":

                On_FarmActive01();
                break;

            case "Or_Hiroba_Spring_Oku_Garden":

                On_Active16();
                break;

            case "Or_Hiroba_Spring_Out_Plain":

                On_Active17();
                break;

            case "Or_Hiroba_Summer_Entrance":

                //On_Active50();
                On_Active51();
                break;

            case "Or_Hiroba_Summer_Street":

                On_Active51();
                break;

            case "Or_Hiroba_Summer_MainStreet":

                On_Active53();
                break;

            case "Or_Hiroba_Summer_MainStreet_Oku":

                On_Active54();
                break;

            case "Or_Hiroba_Summer_MainStreet_Gondora":

                On_Active1620_Summer_GentleMan();
                break;

            case "Or_Hiroba_Summer_ThemePark_Map":

                //On_Active1520_soda_guide_return();
                On_Active86();
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter":

                On_Active72();
                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA":

                On_Active90();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active74();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaMae":

                On_Active85();
                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA_2":

                On_Active91();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumBigWhale":

                On_Active1009_WhiteWhale();
                break;

            case "Or_Hiroba_Summer_ThemePark_beachMae":

                On_NPC_HirobaActive01();
                break;

            case "Or_Hiroba_Autumn_MainStreet":

                On_ShopActive03();
                break;

            case "Or_Hiroba_Autumn_DepartMae":

                //On_Active103();
                On_Active104();
                break;

            case "Or_Hiroba_Autumn_UraStreet":

                On_Active105();
                break;

            case "Or_Hiroba_Winter_EntranceHiroba":

                On_NPC_MagicActive04();
                break;

            case "Or_Hiroba_Winter_Street1":

                On_ShopActive04();
                break;

            case "Or_Hiroba_Winter_MainStreet":

                On_ShopActive04();
                break;

            case "Or_Hiroba_Winter_MainHiroba":

                On_BarActive04();
                break;

            case "Or_Hiroba_Winter_PatissierHouseMae":

                On_NPC_MagicActive05();
                break;

            case "Or_Hiroba_Catsle_MainStreet":

                On_Active302();
                break;

            case "Or_Hiroba_Catsle_MainEntrance":

                On_NPC_CatsleActive01();
                break;

            default:

                On_Active1003_Basan();
                break;
        }
        //}
    }

    //Npc5
    public void OnNPC5_toggle()
    {
        /*if (npc5_toggle.isOn == true)
        {
            npc5_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_BarActive01();
                //On_Active04();
                break;

            case "Or_Hiroba_CentralPark_Left": //中央噴水　左

                //冬エリア入口
                if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_50) //PlayerStatus.player_ninki_param < GameMgr.System_StarBlockLv_03
                {
                    On_Active1702(); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[1])
                    {
                        On_Active04();
                    }
                    else
                    {
                        //GameMgr.NPCHiroba_blockReleaseList[1] = true; //
                        GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                        GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
                        On_BlockReleaseActive1(120);
                    }
                    
                }
                break;

            case "Or_Hiroba_CentralPark_Right": //中央噴水　右

                //夏エリア入口
                if (PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_52) //PlayerStatus.player_ninki_param < GameMgr.System_StarBlockLv_01
                {
                    On_Active1700(); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[3])
                    {
                        On_Active02();
                    }
                    else
                    {
                        //GameMgr.NPCHiroba_blockReleaseList[3] = true; //ねこにおかしをあげて、クリアするまでは通れない。
                        GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                        GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
                        On_BlockReleaseActive1(100);
                    }
                    
                }

                break;

            case "Or_Hiroba_Spring_Entrance":

                On_Active1500_flower();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll":

                On_Active09();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1602_Roten_Crape();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                On_Active1605_Roten_Cafelatte();
                break;

            case "Or_Hiroba_Spring_BarStreet":

                On_BarActive01();
                break;

            case "Or_Hiroba_Spring_Oku":

                On_MapActive01();
                break;

            case "Or_Hiroba_Spring_Oku_Garden":

                On_EmeralShopActive01();
                break;

            case "Or_Hiroba_Spring_Out_alter":

                if (!GameMgr.NPCHiroba_HikarieventList[160])
                {
                    GameMgr.NPCHiroba_HikarieventList[160] = true;

                    On_Active2000(200031, true); //春のさいだん
                    ev_id = pitemlist.Find_eventitemdatabase("eden_recipi_03");
                    pitemlist.add_eventPlayerItem(ev_id, 1);
                }
                else
                {
                    On_Active2000(200032, false); //春のさいだん
                }
                break;

            case "Or_Hiroba_Summer_Entrance":

                On_Active1621_Summer_Ariachan();
                break;

            case "Or_Hiroba_Summer_MainStreet":

                On_Active70();
                break;

            case "Or_Hiroba_Summer_MainStreet_Oku":

                On_Active1008_SummerCat();
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter":

                On_Active1007_Saboten();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                //On_Active70();
                On_Active1006_Piero();
                break;            

            case "Or_Hiroba_Autumn_MainStreet":

                On_Active104();
                break;

            case "Or_Hiroba_Winter_altar":

                if (!GameMgr.NPCHiroba_HikarieventList[161])
                {
                    GameMgr.NPCHiroba_HikarieventList[161] = true;

                    On_Active2000(200041, true); //禁忌の図書館
                    ev_id = pitemlist.Find_eventitemdatabase("eden_recipi_04");
                    pitemlist.add_eventPlayerItem(ev_id, 1);
                }
                else
                {
                    On_Active2000(200042, false); //禁忌の図書館
                }
                break;

            default:

                On_Active1500_flower();
                break;
        }
        //}
    }

    //Npc6
    public void OnNPC6_toggle()
    {
        /*if (npc6_toggle.isOn == true)
        {
            npc6_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                //On_Active200();
                On_FarmActive01();
                break;

            case "Or_Hiroba_CentralPark_Left": //中央噴水　左

                On_Active30();
                break;

            case "Or_Hiroba_CentralPark_Right": //中央噴水　右

                On_Active30();
                break;

            case "Or_Hiroba_CentralPark_Castle_Street": //中央噴水　お城前

                if (PlayerStatus.player_ninki_param < GameMgr.System_StarBlockLv_04) 
                    //PlayerStatus.player_ninki_param < GameMgr.System_StarBlockLv_04 //PlayerStatus.girl1_Love_lv < GameMgr.System_HeartBlockLv_53
                {
                    On_Active1703(); //まだ通れない
                }
                else
                {
                    if (GameMgr.NPCHiroba_blockReleaseList[4])
                    {
                        On_Active300();
                    }
                    else
                    {
                        GameMgr.NPCHiroba_blockReleaseList[4] = true; //
                        On_BlockReleaseActive1(0);
                    }
                    
                }               
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //

                //On_Active10();
                On_Active12();
                break;

            case "Or_Hiroba_Spring_Oku": //

                //On_FarmActive01();
                On_Active13();
                break;

            case "Or_Hiroba_Summer_MainStreet": //

                On_Active1004_Alice();
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter":

                On_ContestActive02();
                break;

            case "Or_Hiroba_Autumn_MainStreet": //

                On_BarActive03();
                break;

            default:

                On_Active1005_Niji_girl();
                break;
        }
        //}
    }

    //Npc7
    public void OnNPC7_toggle()
    {
        /*if (npc7_toggle.isOn == true)
        {
            npc7_toggle.isOn = false;*/

        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_Active300();
                break;

            case "Or_Hiroba_CentralPark_Left": //中央噴水　左

                On_Active33();
                break;

            case "Or_Hiroba_CentralPark_Right": //中央噴水　右

                On_Active33();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll":

                On_Active1610_Amakusa();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1603_Roten_Gelato();
                break;

            case "Or_Hiroba_Autumn_MainStreet": //

                On_ContestActive03();
                break;

            default:

                On_Active1006_Piero();
                break;
        }
        //}
    }

    //Npc8
    public void OnNPC8_toggle()
    {
        /*if (npc8_toggle.isOn == true)
        {
            npc8_toggle.isOn = false;*/

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_BackHomeActive01();
                    break;

                default:

                    On_BackHomeActive01();
                    break;
            }
        //}
    }

    //SubView1
    public void OnSubNPC1_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_ShopActive01();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //中央噴水

                On_ShopActive01();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1600_Roten_Ringo();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                On_Active1605_Roten_Cafelatte();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active75();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMae":

                On_Active1530_aquarium_reception();
                break;

            case "Or_Hiroba_Summer_ThemePark_Pool":

                On_Active1570_Amupark_pool();
                break;

            case "Or_Hiroba_Summer_ThemePark_Hotel":

                On_Active1575_Amupark_hotel();
                break;

            case "Or_Hiroba_HotSpring":

                On_Active1580_HotSpring();
                break;

            case "Or_Hiroba_Catsle_MainEntrance":

                On_NPC_CatsleActive01();
                break;            
                
            default:

                On_ShopActive01();
                break;
        }
    }

    //SubView2
    public void OnSubNPC2_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_BarActive01();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //

                On_BarActive01();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1601_Roten_PotatoButter();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                On_Active1604_Roten_JoukenKyobai();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active1550_Amupark_biking();
                break;

            default:

                On_BarActive01();
                break;
        }
    }

    //SubView3
    public void OnSubNPC3_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_ContestOutActive01(); //会場前へ
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //

                On_Active04();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1602_Roten_Crape();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active1560_Amupark_kanransha();
                break;

            default:

                On_Active1006_Piero();
                break;
        }
    }

    //SubView4
    public void OnSubNPC4_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_Active01();
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //

                On_Active04();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active1603_Roten_Gelato();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active85();
                break;

            default:

                On_Active1006_Piero();
                break;
        }
    }

    //SubView5
    public void OnSubNPC5_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                if (GameMgr.outgirl_Nowprogress)
                {
                    GameMgr.hiroba_event_placeNum = 2000; //
                    GameMgr.hiroba_event_ID = 200100;

                    EventReadingStart();

                    /*if (text_area_hyouji_on)
                    {
                        _text.text = "ヒカリがいないから、行ってもしょうがないな・・。";
                    }*/
                }
                else
                {
                    On_Active10();
                }
                
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active11();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active86();
                break;

            default:

                On_Active1006_Piero();
                break;
        }
    }

    //SubView6
    public void OnSubNPC6_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                //On_Active04();
                On_FarmActive01();
                break;

            case "Or_Hiroba_Spring_RotenStreet":

                On_Active30();
                //On_BackHomeActive02();
                break;

            case "Or_Hiroba_Spring_RotenStreet2":

                On_Active10();
                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba":

                On_Active71();
                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMae":

                On_Active73();
                break;

            case "Or_Hiroba_Summer_ThemePark_Pool":

                On_Active73();
                break;

            case "Or_Hiroba_Summer_ThemePark_Hotel":

                //On_Active73();
                On_Active70();
                break;

            case "Or_Hiroba_HotSpring":

                On_BackHomeActive02();
                break;

            case "Or_Hiroba_Catsle_MainEntrance":

                On_BackHomeActive02();
                //On_Active301();
                break;           

            default:

                On_Active1006_Piero();
                break;
        }
    }


    //
    //以下、イベントアクションの処理一覧
    //

    //各エリアの移動
    void On_Active01()
    {
        //_text.text = "春エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み シーン自体は自分を読む
        GameMgr.SceneSelectNum = 10;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active02()
    {

        //_text.text = "夏エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 100;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active03()
    {
        //_text.text = "秋エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 200;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active04()
    {
        //_text.text = "冬エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 300;
        GoAreaMove("Or_Hiroba1");
    }

    //春エリア
    void On_Active05()
    {
        //_text.text = "散歩道へ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み シーン自体は自分を読む
        GameMgr.SceneSelectNum = 1;
        GoAreaMove("Or_Hiroba1");
    }
  

    void On_Active07()
    {
        //_text.text = "春エリア商店街へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 11;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active08()
    {
        //_text.text = "春エリア　奥側へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 12;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active09()
    {
        //_text.text = "春エリア　裏通りへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 13;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active10()
    {
        //_text.text = "春エリア　露店通りへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 14;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active11()
    {
        //_text.text = "春エリア　露店通り奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 15;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active12()
    {
        //_text.text = "春エリア商店街へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 16;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active13()
    {
        //_text.text = "春エリア商店街へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 17;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active15()
    {
        //_text.text = "春エリア　裏通り奥　庭へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 20;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active16()
    {
        //_text.text = "春エリア　離れの草原へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 21;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active17()
    {
        //_text.text = "春エリア　静けさの湖　光先生の家前へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 22;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active18()
    {
        //_text.text = "春エリア　祭壇へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 23;
        GoAreaMove("Or_Hiroba1");
    }

    //中央噴水
    void On_Active30()
    {
        //_text.text = "噴水エリア　手前へ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active31()
    {
        //_text.text = "噴水エリア　左へ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 2;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active32()
    {
        //_text.text = "噴水エリア　右へ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 3;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active33()
    {
        //_text.text = "噴水エリア　お城通りへ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 4;
        GoAreaMove("Or_Hiroba1");
    }

    //夏
    void On_Active50()
    {
        //_text.text = "夏エリア入口　奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 101;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active51()
    {
        //_text.text = "夏エリア　メインストリートへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 102;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active52()
    {
        //_text.text = "夏エリア　ショップ前へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 103;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active53()
    {
        //_text.text = "夏エリア　メインストリート奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 104;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active54()
    {
        //_text.text = "夏エリア　メインストリート奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 105;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active70()
    {
        //_text.text = "夏エリア遊園地　全体マップへ　移動";

        //GameMgr.Scene_back_home = true;       

        //シーン読み込み
        GameMgr.SceneSelectNum = 150;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active71()
    {
        //_text.text = "夏エリア遊園地　入口へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 151;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active72()
    {
        //_text.text = "夏エリア遊園地　右の通りへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 152;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active73()
    {
        //_text.text = "夏エリア遊園地　観覧車広場へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 153;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active74()
    {
        //_text.text = "夏エリア遊園地　観覧車乗り場へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 154;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active75()
    {
        //_text.text = "夏エリア遊園地　水族館前へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 155;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active76()
    {
        //_text.text = "夏エリア遊園地　水族館入口へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 156;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active77()
    {
        //_text.text = "夏エリア遊園地　水族館メイン広場へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 157;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active78()
    {
        //_text.text = "夏エリア遊園地　水族館メイン２Fへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 158;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active79()
    {
        //_text.text = "夏エリア遊園地　水族館ミニホールへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 159;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active80()
    {
        //_text.text = "夏エリア遊園地　水族館大水槽へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 160;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active85()
    {
        //_text.text = "夏エリア遊園地　プールへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 170;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active86()
    {
        //_text.text = "夏エリア遊園地　プールへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 171;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active90()
    {
        //_text.text = "夏エリア遊園地　13番街　奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 175;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active91()
    {
        //_text.text = "夏エリア遊園地　13番街　奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 176;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active100()
    {
        //_text.text = "秋エリア　メイプル大橋へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 201;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active101()
    {
        //_text.text = "秋エリア　メインストリートへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 202;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active102()
    {
        //_text.text = "秋エリア　百貨店前へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 203;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active103()
    {
        //_text.text = "秋エリア　酒場前へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 204;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active104()
    {
        //_text.text = "秋エリア　コンサートホール前へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 205;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active105()
    {
        //_text.text = "秋エリア　裏通りへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 206;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active106()
    {
        //_text.text = "秋エリア　メイプル大橋前　川のほとりへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 207;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active150()
    {
        //_text.text = "冬エリア　入口前広場　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 301;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active151()
    {
        //_text.text = "冬エリア　広場通り　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 302;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active152()
    {
        //_text.text = "冬エリア　メインストリート　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 303;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active153()
    {
        //_text.text = "冬エリア　大広場　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 304;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active154()
    {
        //_text.text = "冬エリア　大広場右　細い通り　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 305;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active155()
    {
        //_text.text = "冬エリア　コンテスト前の橋　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 306;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active156()
    {
        //_text.text = "冬エリア　祭壇　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 307;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active160()
    {
        //_text.text = "冬エリア　大広場左　通り　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 320;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active161()
    {
        //_text.text = "冬エリア　パティシエ家前1　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 321;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active200()
    {
        //_text.text = "正門前ストリート　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 400;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active201()
    {
        //_text.text = "正門前ストリート　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 401;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active202()
    {
        //_text.text = "正門前ストリート　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 402;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active203()
    {
        //_text.text = "オランジーナ大広場　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 403;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active300()
    {
        //_text.text = "城エリア　手前　庭へ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 500;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active301()
    {
        //_text.text = "城エリア 大通りへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 501;
        GoAreaMove("Or_Hiroba1");
    }

    void On_Active302()
    {
        //_text.text = "城エリア 大通りへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 502;
        GoAreaMove("Or_Hiroba1");
    }

    void On_MapActive01()
    {
        //_text.text = "ブルートパーズのお花畑へ　移動";

        GameMgr.hiroba_event_placeNum = 2000; //

        if (!GameMgr.NPCHiroba_eventList[2510]) //ブルートパーズの花畑を初めていく
        {
            GameMgr.NPCHiroba_eventList[2510] = true;

            //sceneBGM.FadeOutBGM();
            //bgm_change_flag = true;
            GameMgr.hiroba_event_ID = 230000; //そのときに呼び出すイベント番号 placeNumとセットで使う。        

            GameMgr.Utage_MapMoveON = true; //シナリオ読み終わり後、マップを移動する
            map_move_num = 0;
            GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ

            matplace_database.matPlaceKaikin("Bluetopaz_Garden"); //ブルートパーズ解禁

            check_event = true;

            EventReadingStart();

            //GameMgr.Scene_back_home = true;
            //シーン読み込み
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[2510]) //すでにブルートパーズにいったことがある
            {
                matplace_database.matPlaceKaikin("Bluetopaz_Garden"); //ブルートパーズ解禁

                map_move_num = 0;
                MapMove(1);
            }
        }       
    }

    void On_ShopActive01()
    {
        //_text.text = "春エリアのお店へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Shop");
    }

    void On_ShopActive02()
    {
        //_text.text = "夏エリアのお店へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 10;
        GoAreaMove("Or_Shop");
    }

    void On_ShopActive03()
    {
        //_text.text = "秋エリアのお店へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 20;
        GoAreaMove("Or_Shop");
    }

    void On_ShopActive04()
    {
        //_text.text = "冬エリアのお店へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 30;
        GoAreaMove("Or_Shop");
        //FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
    }

    void On_EmeralShopActive01()
    {
        //_text.text = "春エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Emerald_Shop");
    }

    void On_BarActive01()
    {
        //_text.text = "春エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Bar");
    }

    void On_BarActive02()
    {
        //_text.text = "夏エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 10;
        GoAreaMove("Or_Bar");
    }

    void On_BarActive03()
    {
        //_text.text = "秋エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 20;
        GoAreaMove("Or_Bar");
    }

    void On_BarActive04()
    {
        //_text.text = "秋エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();
        GameMgr.ShopEnter_ButtonON = true;

        //シーン読み込み
        GameMgr.SceneSelectNum = 30;
        GoAreaMove("Or_Bar");
    }

    void On_FarmActive01()
    {
        //_text.text = "冬エリアのお店へ入る";

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Farm");
    }

    void On_ContestOutActive01()
    {
        //_text.text = "春エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Outside_the_Contest");
    }

    void On_ContestOutActive02()
    {
        //_text.text = "夏エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 10;
        GoAreaMove("Or_Outside_the_Contest");
    }

    void On_ContestOutActive03()
    {
        //_text.text = "秋エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 20;
        GoAreaMove("Or_Outside_the_Contest");
    }

    void On_ContestOutActive04()
    {
        //_text.text = "冬エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 30;
        GoAreaMove("Or_Outside_the_Contest");
    }

    void On_ContestActive01()
    {
        //_text.text = "春エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_Contest_Reception");
    }

    void On_ContestActive02()
    {
        //_text.text = "夏エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 10;
        GoAreaMove("Or_Contest_Reception");
    }

    void On_ContestActive03()
    {
        //_text.text = "秋エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 20;
        GoAreaMove("Or_Contest_Reception");
    }

    void On_ContestActive04()
    {
        //_text.text = "冬エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 30;
        GoAreaMove("Or_Contest_Reception");
    }

    void On_NPC_MagicActive01()
    {
        //_text.text = "火の魔法の先生の家へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_NPC_MagicHouse");
    }

    void On_NPC_MagicActive02()
    {
        //_text.text = "氷の魔法の先生の家へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 10;
        GoAreaMove("Or_NPC_MagicHouse");
    }

    void On_NPC_MagicActive03()
    {
        //_text.text = "風の魔法の先生の家へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 20;
        GoAreaMove("Or_NPC_MagicHouse");
    }

    void On_NPC_MagicActive04()
    {
        //_text.text = "光の魔法の先生の家へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 30;
        GoAreaMove("Or_NPC_MagicHouse");
       
    }

    void On_NPC_MagicActive05()
    {
        //_text.text = "星の魔法の先生の家へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 40;
        GoAreaMove("Or_NPC_MagicHouse");
    }

    void On_NPC_HirobaActive01()
    {
        //_text.text = "夏エリアのケーキショップへ入る";

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_NPC_Hiroba");
    }

    void On_NPC_CatsleActive01()
    {
        //宴の処理へ
        GameMgr.hiroba_event_placeNum = 1410; //

        if (!GameMgr.NPCHiroba_eventList[1600]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[1600] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;            

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[1600]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                //
                GameMgr.hiroba_event_ID = 10;

                GameMgr.Utage_MapMoveON = true; //シナリオ読み終わり後、マップを移動する
                map_move_num = 1410;
                GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ

                
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }
        EventReadingStart();
             
    }

    void On_QueenEnterActive()
    {
        //_text.text = "城へ入る";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        GoAreaMove("Or_NPC_Catsle"); 
    }

    void On_StationActive01()
    {
        //_text.text = "城へ入る";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 100;
        GoAreaMove("Station");        
    }

    void On_BackHomeActive01()
    {
        //_text.text = "春エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //アトリエに戻る
        GoAreaMove("Or_Compound_Enterance");
        //FadeManager.Instance.LoadScene("Or_Compound_Enterance", GameMgr.SceneFadeTime);
    }

    void On_BackHomeActive02()
    {
        //玄関音
        sc.EnterSound_01();
        //sc.EnterSound_03();

        GameMgr.Scene_back_home = true;

        GoAreaMove("Or_Compound");
    }

    void On_Active1000()
    {
        //いちご少女押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 0; //いちご少女を押した　という指定番号

        if (GameMgr.Story_Mode == 0)
        {
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[2])
                    {
                        GameMgr.hiroba_event_ID = 40; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 41; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }

                    break;

                case 50: //コンテストイベント時

                    if (!GameMgr.hiroba_event_end[10])
                    {
                        GameMgr.hiroba_event_ID = 50; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        if (!GameMgr.hiroba_ichigo_first)
                        {
                            GameMgr.hiroba_event_ID = 51; //いちごお菓子もってきた。初回
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 52; //いちごお菓子もってきた。二回目以降
                        }

                        GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                        GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
                    }

                    break;

                default:

                    GameMgr.hiroba_event_ID = 0;
                    break;
            }
        }
        else
        {
            if (!GameMgr.hiroba_ichigo_first)
            {
                GameMgr.hiroba_event_ID = 10050; //いちごお菓子もってきた。初回
            }
            else
            {
                GameMgr.hiroba_event_ID = 52; //いちごお菓子もってきた。二回目以降
            }

            GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
            GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
        }

        EventReadingStart();

    }

    void GoAreaMove(string _scenename) //マップ移動　移動に10分かかる
    {
        //日数の経過。場所ごとに、移動までの日数が変わる。
        time_controller.SetMinuteToHour(GameMgr.System_HirobaMove_Time, 1, 0, false);
        time_controller.TimeKoushin(0, false);
        FadeManager.Instance.LoadScene(_scenename, GameMgr.SceneFadeTime);
    }

    void On_Active1001_Nuno()
    {
        //NPC白い布　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1200; //

        if (!GameMgr.NPCHiroba_eventList[100]) //はじめて
        {           

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[100]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();

    }

    void On_Active1002_Kinoko()
    {
        //NPCきのこ　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1210; //

        if (!GameMgr.NPCHiroba_eventList[120]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[120] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[120]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10 + talkrot;
                TalkRotation(1, 1); //2つめが1の時は、パターンがローテーションせずに止まる

                check_event = true;
            }
        }

        EventReadingStart();
    }

    void On_Active1003_Basan()
    {
        //NPC魔女ばあさん　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1220; //

        if (!GameMgr.NPCHiroba_eventList[140]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[140] = true;

            GameMgr.hiroba_event_ID = 0;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[140]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }


    void On_Active1004_Alice()
    {
        //NPCアリス　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1230; //

        if (!GameMgr.NPCHiroba_eventList[160]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[160] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[160]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }

    void On_Active1005_Niji_girl()
    {
        //NPC虹の女の子　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1240; //

        if (!GameMgr.NPCHiroba_eventList[180]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[180] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[180]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }


    void On_Active1006_Piero()
    {
        //NPCピエロ　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1250; //

        if (!GameMgr.NPCHiroba_eventList[200]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[200] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[200]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }


    void On_Active1007_Saboten()
    {
        //NPCおどるサボテン　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1260; //

        if (!GameMgr.NPCHiroba_eventList[220]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[220] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[220]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10 + talkrot;
                TalkRotation(1, 0); //1つめは、会話のパターン数　1だと2個ある。　2つめが0の時は、パターンがローテーションする

                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }


    void On_Active1008_SummerCat()
    {
        //NPCおどるサボテン　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1270; //

        if (!GameMgr.NPCHiroba_eventList[240]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[240] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[240]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }

    void On_Active1009_WhiteWhale()
    {
        //NPC白クジラ　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1280; //

        if (!GameMgr.NPCHiroba_eventList[260]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[260] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }
        else
        {
            //一回あった状態で、３つのレシピを所持してる。夢喰い沼を教えてくれる。
            if(pitemlist.KosuCountEvent("eden_recipi_02") >= 1 && 
                pitemlist.KosuCountEvent("eden_recipi_03") >= 1 && 
                pitemlist.KosuCountEvent("eden_recipi_04") >= 1)
            {

                if (!GameMgr.NPCHiroba_eventList[270]) //ほかに発生するイベントがなく、すでに友達になった。
                {
                    if (PlayerStatus.girl1_Love_exp >= GameMgr.System_trueheart_cost) //ハートが3000以上なら場所を教えてくれる。
                    {
                        GameMgr.NPCHiroba_eventList[270] = true;

                        GameMgr.hiroba_event_ID = 100;

                        //BGMかえる
                        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                        bgm_change_flag = true;

                        //matplace_database.ReSetMapFlagString("DreamEater_Swamp", 1); //ゆめくいぬま発見

                        check_event = true;

                        GameMgr.OsotoIttazoFlag = false;　//イベントあったあとは、お外フラグをオフにしとく。
                    }
                    else
                    {
                        //まだハートが足りてないとき　こころが強くないので教えられないと断られる
                        GameMgr.hiroba_event_ID = 102;

                        check_event = true;
                    }
                }
                else
                { 
                    //すでに夢喰い沼の場所を教えてくれてる

                    if (!GameMgr.NPCHiroba_eventList[272])
                    {
                        //まだブラックロータスをゲットしてないなら、再度ゆめくいぬまの場所を教えてくれる。
                        if (!GameMgr.GirlLoveSubEvent_stage1[405])
                        {
                            GameMgr.hiroba_event_ID = 101;

                            check_event = true;
                        }
                        else
                        {
                            //ゲット済なら、セリフが変わる　このタイミングで、ハートレシピと真実のハートを教えてくれる
                            GameMgr.hiroba_event_ID = 103;
                            GameMgr.NPCHiroba_eventList[272] = true;
                            check_event = true;

                            //BGMかえる
                            sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                            bgm_change_flag = true;

                            ev_id = pitemlist.Find_eventitemdatabase("mg_TrueofMyheart_book");
                            pitemlist.add_eventPlayerItem(ev_id, 1); //真実のハート魔法を追加

                            //最後のエデンレシピ「ハートのレシピ」をゲット
                            ev_id = pitemlist.Find_eventitemdatabase("eden_recipi_05");
                            pitemlist.add_eventPlayerItem(ev_id, 1); //最後のエデンレシピを追加

                            GameMgr.OsotoIttazoFlag = false;　//イベントあったあとは、お外フラグをオフにしとく。
                        }
                    }
                    else
                    {
                        //最後のレシピと真実のハートを教えてくれたあと

                        GameMgr.hiroba_event_ID = 120;
                        check_event = true;
                    }


                    //エデンを一回食べたことがある　満月の夜に食べるとよいと、教えてくれる。
                    //110~番台　現在は未使用
                    /*if (GameMgr.GirlLoveSubEvent_stage1[600])
                    {
                        if (!GameMgr.NPCHiroba_eventList[271])
                        {
                            GameMgr.NPCHiroba_eventList[271] = true;
                            GameMgr.hiroba_event_ID = 110;

                            check_event = true;

                            //満月の夜の日を設定　カレントデイから5日後
                            time_controller.CullenderKeisan(PlayerStatus.player_day + 5);
                            GameMgr.System_Fullmoon_month = GameMgr.Cullender_Month;
                            GameMgr.System_Fullmoon_day = GameMgr.Cullender_Day;
                        }
                        else
                        {
                            //満月の夜を過ぎてた場合、次の満月を教えてくれる　再設定
                            if(PlayerStatus.player_cullent_month > GameMgr.System_Fullmoon_month) //超えたので再設定
                            {
                                GameMgr.hiroba_event_ID = 112; //

                                check_event = true;

                                //満月の夜の日を設定　カレントデイから5日後
                                time_controller.CullenderKeisan(PlayerStatus.player_day + 5);
                                GameMgr.System_Fullmoon_month = GameMgr.Cullender_Month;
                                GameMgr.System_Fullmoon_day = GameMgr.Cullender_Day;
                            }
                            else if (PlayerStatus.player_cullent_month < GameMgr.System_Fullmoon_month)
                            {
                                GameMgr.hiroba_event_ID = 111; //満月の夜にエデンを食べろを繰り返す

                                check_event = true;
                            }
                            else //月は一緒の場合、日をみる
                            {
                                if (PlayerStatus.player_cullent_day > GameMgr.System_Fullmoon_day) //超えた場合は再設定
                                {
                                    GameMgr.hiroba_event_ID = 112; //満月の夜にエデンを食べろを繰り返す

                                    check_event = true;

                                    //満月の夜の日を設定　カレントデイから5日後
                                    time_controller.CullenderKeisan(PlayerStatus.player_day + 5);
                                    GameMgr.System_Fullmoon_month = GameMgr.Cullender_Month;
                                    GameMgr.System_Fullmoon_day = GameMgr.Cullender_Day;
                                }
                                else if (PlayerStatus.player_cullent_day < GameMgr.System_Fullmoon_day) //未満の場合は、繰り返す
                                {
                                    GameMgr.hiroba_event_ID = 111; //満月の夜にエデンを食べろを繰り返す

                                    check_event = true;
                                }
                                else //今日の場合　再設定はせず、セリフのみ変わる
                                {
                                    GameMgr.hiroba_event_ID = 113; //満月の夜にエデンを食べろを繰り返す

                                    check_event = true;
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 101;

                        check_event = true;
                    }*/
                }
            }            
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[260]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 10;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }

        EventReadingStart();
    }


    //人間NPC関連のマップイベントは1500～
    //
    void On_Active1500_flower()
    {
        //お花屋さん押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1500; //
        GameMgr.hiroba_event_ID = 0;

        //BGMかえる
        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        bgm_change_flag = true;

        EventReadingStart();
    }

    void On_Active1510_soda_guide()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1510; //       

        GameMgr.Utage_MapMoveON = true; //シナリオ読み終わり後、マップを移動する
        map_move_num = 1510;
        GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ
        

        if(pitemlist.ReturnItemKosu("gondra_freepassport") >= 1)
        {
            GameMgr.hiroba_event_ID = 10;
        }
        else
        {
            GameMgr.hiroba_event_ID = 0;
        }

        EventReadingStart();
    }

    void On_Active1520_soda_guide_return()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1520; //       

        GameMgr.Utage_MapMoveON = true; //シナリオ読み終わり後、マップを移動する
        map_move_num = 1520;
        GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1530_aquarium_reception()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1530; //       

        GameMgr.Utage_MapMoveON = true; //シナリオ読み終わり後、マップを移動する
        map_move_num = 1530;
        GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1540_aquarium_return()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1540; //       

        GameMgr.Utage_MapMoveON = true; //シナリオ読み終わり後、マップを移動する
        map_move_num = 1540;
        GameMgr.Utage_MapMoveBlackON = true; //ワンセット　シーンを黒くするための宴の分岐用フラグ

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1550_Amupark_biking()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1550; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1560_Amupark_kanransha()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1560; //       

        GameMgr.hiroba_event_ID = 0;   

        EventReadingStart();
    }

    void On_Active1570_Amupark_pool()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1570; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1575_Amupark_hotel()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1575; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }    

    void On_Active1580_HotSpring()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1580; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1600_Roten_Ringo()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1600; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1601_Roten_PotatoButter()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1601; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1602_Roten_Crape()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1602; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1603_Roten_Gelato()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1603; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1604_Roten_JoukenKyobai()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1604; //       

        GameMgr.hiroba_event_ID = 0;

        GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
        GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ

        //下は、使うときだけtrueにすればOK
        GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
        GameMgr.KoyuJudge_num = GameMgr.NPC_OkashiJudge_num[50];//GirlLikeSetの番号を直接指定
        GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする

        EventReadingStart();
    }

    void On_Active1605_Roten_Cafelatte()
    {
        //NPC宴の処理へ
        GameMgr.hiroba_event_placeNum = 1605; //       

        GameMgr.hiroba_event_ID = 0;

        EventReadingStart();
    }

    void On_Active1610_Amakusa()
    {
        //アマクサ
        GameMgr.hiroba_event_placeNum = 1610; //
        GameMgr.hiroba_event_ID = 0;

        //BGMかえる
        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        bgm_change_flag = true;

        /*if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[1030]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                GameMgr.hiroba_event_ID = 0;
                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }*/
        EventReadingStart();
    }

    void On_Active1620_Summer_GentleMan()
    {
        //宴の処理へ
        GameMgr.hiroba_event_placeNum = 1620; //

        if (!GameMgr.NPCHiroba_eventList[1200]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[1200] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[1200]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                //頭から順番に会話をまわしていく。
                GameMgr.hiroba_event_ID = 10 + talkrot;
                TalkRotation(2, 1); //2つめが1の時は、パターンがローテーションせずに止まる

                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }
        EventReadingStart();
    }

    //アリアちゃん
    void On_Active1621_Summer_Ariachan()
    {
        //宴の処理へ
        GameMgr.hiroba_event_placeNum = 1621; //

        GameMgr.NPCHiroba_eventList[1220] = true; //はじめてイベントは無くした。

        if (!GameMgr.NPCHiroba_eventList[1220]) //はじめて
        {
            GameMgr.NPCHiroba_eventList[1220] = true;

            GameMgr.hiroba_event_ID = 0;
            //BGMかえる
            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;

            check_event = true;
        }

        if (check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {
            if (GameMgr.NPCHiroba_eventList[1220]) //ほかに発生するイベントがなく、すでに友達になった。
            {
                //頭から順番に会話をまわしていく。
                GameMgr.hiroba_event_ID = 10 + talkrot;               
                //TalkRotation(2, 1); //2つめが1の時は、パターンがローテーションせずに止まる


                //BGMかえる
                //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
                //bgm_change_flag = true;

                check_event = true;
            }
        }
        EventReadingStart();
    }

    //広場ブロック
    void On_Active1700() //夏エリア入口
    {
        GameMgr.hiroba_event_placeNum = 1700; //

        //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        //bgm_change_flag = true;
        GameMgr.hiroba_event_ID = 170000; //そのときに呼び出すイベント番号 placeNumとセットで使う。        

        EventReadingStart();
    }

    void On_Active1701() //秋エリア入口
    {
        GameMgr.hiroba_event_placeNum = 1700; //

        //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        //bgm_change_flag = true;
        GameMgr.hiroba_event_ID = 170001; //そのときに呼び出すイベント番号 placeNumとセットで使う。        

        EventReadingStart();
    }

    void On_Active1702() //冬エリア入口
    {
        GameMgr.hiroba_event_placeNum = 1700; //

        //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        //bgm_change_flag = true;
        GameMgr.hiroba_event_ID = 170002; //そのときに呼び出すイベント番号 placeNumとセットで使う。        

        EventReadingStart();
    }

    void On_Active1703() //城エリア入口
    {
        GameMgr.hiroba_event_placeNum = 1700; //

        //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        //bgm_change_flag = true;
        GameMgr.hiroba_event_ID = 170003; //そのときに呼び出すイベント番号 placeNumとセットで使う。        

        EventReadingStart();
    }

    //ヒカリ関連のマップイベントはActive2000～
    //
    void On_Active2000(int _num, bool bgmchange)
    {
        GameMgr.hiroba_event_placeNum = 2000; //

        if (bgmchange)
        {
            sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            bgm_change_flag = true;
        }
        GameMgr.hiroba_event_ID = _num; //そのときに呼び出すイベント番号 placeNumとセットで使う。        

        EventReadingStart();
    }

    void On_Active2001() //散歩道のイベント
    {
        GameMgr.hiroba_event_placeNum = 2000; //

        if(!GameMgr.NPCHiroba_HikarieventList[100])
        {
            GameMgr.NPCHiroba_HikarieventList[100] = true;

            //sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
            //bgm_change_flag = true;
            GameMgr.hiroba_event_ID = 210000; //そのときに呼び出すイベント番号 placeNumとセットで使う。    
            check_event = true;
        }

        if(check_event) { } //上で先にイベント発生したら、以下は読まない。
        else
        {

        }         

        EventReadingStart();
    }


    //広場ブロック解放イベント
    void On_BlockReleaseActive1(int _id)
    {
        GameMgr.hiroba_event_placeNum = 2100; //

        //BGMかえる
        sceneBGM.FadeOutBGM(GameMgr.System_default_sceneFadeBGMTime);
        bgm_change_flag = true;

        GameMgr.hiroba_event_ID = _id; //そのときに呼び出すイベント番号 placeNumとセットで使う。        


        EventReadingStart();
    }


    //
    //その他処理　publicは、同じオブジェクトにつけたHiroba1_Main_Orのcsから読み出し
    //
    public void ToggleAllOff()
    {
        /*npc1_toggle.interactable = false;
        npc2_toggle.interactable = false;
        npc3_toggle.interactable = false;
        npc4_toggle.interactable = false;
        npc5_toggle.interactable = false;
        npc6_toggle.interactable = false;
        npc7_toggle.interactable = false;*/
    }

    public void SceneToggleDefaultSetup()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_50").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                
                if (GameMgr.outgirl_Nowprogress)
                {
                    GameMgr.Window_CharaName = GameMgr.player_Name_First;
                    default_scenetext = "オランジーナの噴水だ。でっかいな～。";
                }
                else
                {
                    GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                    default_scenetext = "にいちゃん！　おっきい噴水があるよ～！";
                }

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                //text_area_hyouji_on = true;
                break;

            case "Or_Hiroba_CentralPark2": //中央噴水のお散歩小道

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_02").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ちいさな散歩道だ。";

                break;

            case "Or_Hiroba_CentralPark_Left": //中央噴水　左

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_51").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オランジーナの街の中央噴水左だ。" + "\n" + "春と冬エリアが見えている";

                break;

            case "Or_Hiroba_CentralPark_Right": //中央噴水　右

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_52").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オランジーナの街の中央噴水右だ。" + "\n" + "夏と秋エリアが見えている";

                break;

            case "Or_Hiroba_CentralPark_Castle_Street": //中央噴水　お城前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_53").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オランジーナの街のお城前通りだ。";

                break;

            case "Or_Hiroba_Spring_Entrance": //春のエリア入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_03").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スプリングガーデンの入口だ。" + "\n" + "春の商店街へ続く道やコンテスト会場がある。";

                matplace_database.matPlaceKaikin("Or_HirobaEnter_A1"); //春エリア入口解禁
                break;

            case "Or_Hiroba_Spring_Shoping_Moll": //春のエリア商店街

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_04").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();               

                default_scenetext = "スプリングガーデンの商店街だ。" + "\n" + "人でにぎわっている。";
                              
                break;

            case "Or_Hiroba_Spring_Oku": //春のエリア商店街　奥側

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_05").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "";

                break;

            case "Or_Hiroba_Spring_UraStreet": //春のエリア商店街　裏通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_06").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは裏通りのようだ。" + "\n" + "少し陰になっている。";

                break;

            case "Or_Hiroba_Spring_RotenStreet": //春のエリア商店街　露店通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_07").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                default_scenetext = "にいちゃん！　なんかいっぱいお店がある～！";

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                //text_area_hyouji_on = true;

                if (GameMgr.OsotoIkitaiFlag) //お外いきたいフラグがたってた場合、来た時点でよろこび
                {
                    GameMgr.OsotoIkitaiFlag = false;

                    GameMgr.OsotoIttazoFlag = true;
                    GameMgr.OsotoIttazoPlace = "RotenStreet";
                }
                break;

            case "Or_Hiroba_Spring_RotenStreet2": //春のエリア商店街　露店通り2

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_08").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                default_scenetext = "わぁ～☆　あったかいばしょ～！";

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                //text_area_hyouji_on = true;

                break;

            case "Or_Hiroba_Spring_RotenStreet3": //春のエリア商店街　露店通り右の広場　使ってない

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_16").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                default_scenetext = "らーめんイベントとか。広場でスタートする場合はここ使いたい";

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                //text_area_hyouji_on = true;

                break;

            case "Or_Hiroba_Spring_BarStreet": //春のエリア商店街　酒場前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_14").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは酒場前のようだ。";

                break;

            case "Or_Hiroba_Spring_Flower_Campo": //春のエリア商店街　奥の右の小道

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_13").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは秘密の花園の小道だ。";

                break;

            case "Or_Hiroba_Spring_Oku_Garden": //春のエリア 裏通り奥の庭

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_10").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは裏通り奥の庭だ。";

                break;

            case "Or_Hiroba_Spring_Out_Plain": //春エリア　離れの草原

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_11").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは春エリアから少し離れたところにある草原だ。";

                break;

            case "Or_Hiroba_Spring_Out_MagicHouseLake": //春エリア　静けさの湖　ミラボー先生の家前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_12").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは静けさの湖。光先生の家がある。";

                break;

            case "Or_Hiroba_Spring_Out_alter": //春エリア　祭壇

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_15").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは春の祭壇だ。";

                break;

            case "Or_Hiroba_Summer_Entrance": //夏エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_100").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスの入口だ。" + "\n" + "青っぽい家が多い。";

                matplace_database.matPlaceKaikin("Or_HirobaEnter_B1"); //夏エリア入口解禁

                break;

            case "Or_Hiroba_Summer_Street": //夏エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_101").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスの通りのようだ。";

                break;

            case "Or_Hiroba_Summer_MainStreet": //夏エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_102").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスのメインストリートのようだ。";

                break;

            case "Or_Hiroba_Summer_MainStreet_Shop": //夏エリア　ショップ通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_103").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスのショップ前の通りだ。";

                break;

            case "Or_Hiroba_Summer_MainStreet_Oku": //夏エリア　メインストリート奥

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_104").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスの奥の通りだ。";

                break;

            case "Or_Hiroba_Summer_MainStreet_Gondora": //夏エリア　ゴンドラ乗り場

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_105").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスのゴンドラ乗り場だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_Map": //夏エリア　遊園地　全体マップ

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_150").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地だ。島全体が遊園地になっている。";

                if(GameMgr.OsotoIkitaiFlag) //お外いきたいフラグがたってた場合、来た時点でよろこび
                {
                    GameMgr.OsotoIkitaiFlag = false;

                    GameMgr.OsotoIttazoFlag = true;
                    GameMgr.OsotoIttazoPlace = "SodaIsland";
                }

                //スウィートホテル解禁
                if (GameMgr.NPCHiroba_HikarieventList[320])
                {
                    npc4_toggle_obj.SetActive(true);
                }
                else
                {
                    npc4_toggle_obj.SetActive(false);
                }
                break;

            case "Or_Hiroba_Summer_ThemePark_Enter": //夏エリア　遊園地入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_151").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地の入口だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA": //夏エリア　遊園地　右の通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_152").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　右の通りだ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba": //夏エリア　遊園地　観覧車広場

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_153").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                default_scenetext = "にいちゃん！" + "\n" + "のりもの、いっぱいあるよ～！　あちぃ～～・・。";

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                //text_area_hyouji_on = true;

                //スウィートホテル解禁
                /*if (GameMgr.NPCHiroba_HikarieventList[320])
                {
                    npc_subview_obj.transform.Find("SubView5_SelectToggle").gameObject.SetActive(true);
                }
                else
                {
                    npc_subview_obj.transform.Find("SubView5_SelectToggle").gameObject.SetActive(false);
                }*/

                break;

            case "Or_Hiroba_Summer_ThemePark_KanranShaMae": //夏エリア　遊園地　観覧車乗り場

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_154").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　観覧車乗り場だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMae": //夏エリア　遊園地　水族館前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_155").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                default_scenetext = "にいちゃん！" + "\n" + "これが水族館？　おっっきい～～！！";

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                text_area_hyouji_on = true;

                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumEntrance": //夏エリア　遊園地　水族館入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_156").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　水族館入口だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMainHall": //夏エリア　遊園地　水族館メイン広場

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_157").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　水族館メイン広場のようだ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMain2F": //夏エリア　遊園地　水族館メイン２F

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_158").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　水族館２階だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumMiniHall": //夏エリア　遊園地　水族館ミニホール

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_159").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　水族館ミニホールだ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_AquariumBigWhale": //夏エリア　遊園地　水族館　大水槽

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_160").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　水族館大水槽だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_Pool": //夏エリア　遊園地　プール

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_170").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                if (!GameMgr.System_PoolEnd)
                {
                    default_scenetext = "にいちゃん！　ここプ～ル？" + "\n" + "早く泳ぎたいなぁ～！";
                }
                else
                {
                    default_scenetext = "プール最高だった！" + "\n" + "にいちゃん。楽しかったね～♪";
                }

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                text_area_hyouji_on = true;

                break;

            case "Or_Hiroba_Summer_ThemePark_Hotel": //夏エリア　遊園地　ホテル

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_171").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                if (!GameMgr.System_HotelEnd)
                {
                    default_scenetext = "にいちゃん！" + "\n" + "ここがホテル～～？　おっきいね～～♪";
                }
                else
                {
                    default_scenetext = "ホテルたのしかった～！" + "\n" + "またいこ～ね♪" + "\n" + "体力とMPが全回復した！";
                }

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                text_area_hyouji_on = true;

                break;

            case "Or_Hiroba_HotSpring": //温泉エリア

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_172").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                if (!GameMgr.System_HotSpringEnd)
                {
                    default_scenetext = "にいちゃん！" + "\n" + "ここが温泉～？　あったかいよ～！";
                }
                else
                {
                    default_scenetext = "温泉きもちよかったね～！" + "\n" + "コーヒー牛乳うまい。にいちゃん♪" + "\n" + "体力とMPが全回復した！";
                }

                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                text_area_hyouji_on = true;

                break;

            case "Or_Hiroba_Summer_ThemePark_StreetA_2": //夏エリア　遊園地　13番街　奥

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_175").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　13番街　奥の細道だ。";

                break;

            case "Or_Hiroba_Summer_ThemePark_beachMae": //夏エリア　遊園地　13番街　奥

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_176").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス遊園地　13番街　浜辺だ。";

                break;

            case "Or_Hiroba_Summer_CakeShop_Mae": //夏エリア　遊園地　13番街　奥

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_177").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームス　ケーキショップ前だ。";

                break;

            case "Or_Hiroba_Autumn_Entrance": //秋エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_200").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスの入口だ。" + "\n" + "紅葉の赤やカラフルな色の建物に包まれている。";

                matplace_database.matPlaceKaikin("Or_HirobaEnter_C1"); //秋エリア入口解禁

                //ヒカリが一緒にいないと、ぬねは登場しない
                if (GameMgr.outgirl_Nowprogress) //trueだとヒカリがいない
                {
                    mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject.SetActive(false);
                }else
                {
                    mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject.SetActive(true);
                }
                break;

            case "Or_Hiroba_Autumn_Entrance_bridge": //秋エリア　入口大橋

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_201").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスの橋.." + "\n" + "メイプル大橋と呼ばれている。";

                break;

            case "Or_Hiroba_Autumn_MainStreet": //秋エリア　メインストリート

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_202").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスのメインの大通りだ。";

                break;

            case "Or_Hiroba_Autumn_DepartMae": //秋エリア　デパート前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_203").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスのデパート前だ。";

                break;

            case "Or_Hiroba_Autumn_BarStreet": //秋エリア　酒場前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_204").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスの酒場前の通りだ。";

                break;

            case "Or_Hiroba_Autumn_UraStreet": //秋エリア　裏通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_205").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスの裏通りだ。";

                break;

            case "Or_Hiroba_Autumn_UraStreet2": //秋エリア　裏通り2

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_206").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブス裏通りのさらに奥だ。";

                break;

            case "Or_Hiroba_Autumn_Riverside": //秋エリア　川のほとり

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_207").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスの川のようだ。";

                break;

            case "Or_Hiroba_Winter_Entrance": //冬エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_300").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの入口だ。";

                break;


            case "Or_Hiroba_Winter_EntranceHiroba": //冬エリア　前広場

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_301").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの入口広場だ。" + "\n" + "夜の真っ暗で幻想的な雰囲気に包まれている。";

                matplace_database.matPlaceKaikin("Or_HirobaEnter_D1"); //冬エリア入口解禁
                break;

            case "Or_Hiroba_Winter_Street1": //冬エリア　広場通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_302").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの広場通りだ。";

                break;

            case "Or_Hiroba_Winter_MainStreet": //冬エリア　広場通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_303").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストのメインストリートだ。";

                break;

            case "Or_Hiroba_Winter_MainHiroba": //冬エリア　広場通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_304").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの大広場だ。";

                break;

            case "Or_Hiroba_Winter_Street2": //冬エリア　右奥の細道

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_305").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、大広場からの細い通りのようだ。";

                break;

            case "Or_Hiroba_Winter_ContestBridge": //冬エリア　コンテスト会場前の橋

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_306").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの橋だ。";

                break;

            case "Or_Hiroba_Winter_altar": //冬エリア　祭壇

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_307").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの祭壇だ。";

                break;

            case "Or_Hiroba_Winter_Street3": //冬エリア　左奥の通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_320").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、大広場からの通りだ。";

                break;

            case "Or_Hiroba_Winter_PatissierHouseMae": //冬エリア　パティシエの家前

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_321").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、星パティシエの家の前のようだ。";

                break;

            case "Or_Hiroba_MainGate_Street": //正門前ストリート

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_400").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、正門前のストリートのようだ。";

                break;

            case "Or_Hiroba_MainGate_Street2_hiroba": //正門前露店通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_401").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、正門前の露店通りのようだ。";

                break;

            case "Or_Hiroba_MainGate_Entrance": //正門前ゲート

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_402").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、正門前のゲートだ。";

                break;

            case "Or_Hiroba_MainGate_Big_hiroba": //オランジーナ大広場

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_403").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、正門前の大広場だ。";

                break;

            case "Or_Hiroba_Catsle_Garden": //城エリア　大通り前庭

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_500").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、噴水広場北にあるなぞの庭のようだ。";

                break;

            case "Or_Hiroba_Catsle_MainStreet": //城エリア　大通り

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_501").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オランジーナ城へ繋がるメインストリートだ。";

                break;

            case "Or_Hiroba_Catsle_MainEntrance": //城エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_502").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
                default_scenetext = "にいちゃん！" + "\n" + "・・なんか怖そうなおにいちゃんがいる。";

                matplace_database.matPlaceKaikin("Or_HirobaEnter_Catsle"); //城エリア入口解禁
                
                //場所によって、テキストエリア＋横長のサブビュー表示の場合もあり
                text_area_hyouji_on = true;
                foreach (GameObject child in Character_list)
                {
                    if(child.name == "CharacterImage01")
                    {
                        child.SetActive(true);
                        break;
                    }
                }
                break;

            default:

                break;
        }
    }

    void ToggleSetup()
    {
        //トグル初期状態
        npc1_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC1_SelectToggle").gameObject;
        npc2_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject;
        npc3_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC3_SelectToggle").gameObject;
        npc4_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC4_SelectToggle").gameObject;
        npc5_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC5_SelectToggle").gameObject;
        npc6_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC6_SelectToggle").gameObject;
        npc7_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC7_SelectToggle").gameObject;
        npc8_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC8_SelectToggle").gameObject;

        npc1_toggle_text = npc1_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc2_toggle_text = npc2_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc3_toggle_text = npc3_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc4_toggle_text = npc4_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc5_toggle_text = npc5_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc6_toggle_text = npc6_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc7_toggle_text = npc7_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();
        npc8_toggle_text = npc8_toggle_obj.transform.Find("Background/Text").GetComponent<Text>();

        npc_subview_obj = mainlist_controller_obj.transform.Find("SubView/Viewport/Content_Main").gameObject;

        /*npc1_toggle = npc1_toggle_obj.GetComponent<Toggle>();
        npc2_toggle = npc2_toggle_obj.GetComponent<Toggle>();
        npc3_toggle = npc3_toggle_obj.GetComponent<Toggle>();
        npc4_toggle = npc4_toggle_obj.GetComponent<Toggle>();
        npc5_toggle = npc5_toggle_obj.GetComponent<Toggle>();
        npc6_toggle = npc6_toggle_obj.GetComponent<Toggle>();
        npc7_toggle = npc7_toggle_obj.GetComponent<Toggle>();
        npc8_toggle = npc8_toggle_obj.GetComponent<Toggle>();

        npc1_toggle.interactable = true;
        npc2_toggle.interactable = true;
        npc3_toggle.interactable = true;
        npc4_toggle.interactable = true;
        npc5_toggle.interactable = true;
        npc6_toggle.interactable = true;
        npc7_toggle.interactable = true;
        npc8_toggle.interactable = true;*/

        //一度すべてオフ
        /*foreach (Transform child in mainlist_controller_obj.transform.Find("Viewport/Content_Main").transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
        }*/
    }

    public void ToggleFlagCheck()
    {

    }

    void TalkRotation(int _talkmax_rot, int _stopstatus)
    {
        if(_stopstatus == 0) //0の場合、止まらずにローテーションする
        {
            if (talkrot <= _talkmax_rot) //
            {
                talkrot++;
            }
        }
        else if (_stopstatus == 1) //1の場合、パターン終わりで止まる
        {
            if (talkrot < _talkmax_rot) //
            {
                talkrot++;
            }
        }        

        if (talkrot > _talkmax_rot)
        {
            talkrot = 0;
        }
    }

    //ネームプレートの設定とアニメーションON
    public void SceneNamePlateSetting()
    {
        sceneplace_namepanel.OnSceneNamePlate();
        //sceneplace_namepanel_obj.SetActive(true);
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMgr.Scene_LoadedOn_End = true;
    }

    //シーンがアンロードされたタイミングで呼び出しされる
    void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        GameMgr.Scene_LoadedOn_End = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Contest_Main_Reception : MonoBehaviour
{
    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject text_area;
    private Text _text;

    private GameObject yes_no_panel;

    private SceneInitSetting sceneinit_setting;
    private GameObject scene_black_effect;

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

    private GameObject npc2sub_toggle_obj;

    private ContestStartListDataBase conteststartList_database;
    private ItemMatPlaceDataBase matplace_database;

    private PlayerItemList pitemlist;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private TimeController time_controller;

    private GameObject mainlist_controller_obj;
    private GameObject contestList_ScrollView_obj;
    private GameObject contest_detailedPanel;
    private GameObject time_panel;
    private GameObject money_panel;

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private BGM sceneBGM;
    public bool bgm_change_flag;

    private GameObject newAreaReleasePanel_obj;
    private GameObject backshopfirst_obj;

    private int ev_id;

    private int i, rndnum;
    private int backnum;
    private int contest_list, _id;

    private bool StartRead;
    private bool flag_chk;
    private bool check_event;

    private string default_scenetext;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private List<GameObject> BGImg_List_mago = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 120;

        //Debug.Log("Hiroba scene loaded");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 0.0f); //黒い画面はオフ

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel_ContestSelect").gameObject;

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //リストオブジェクトの取得
        mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_01").gameObject;
        

        //トグル初期状態
        npc1_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC1_SelectToggle").gameObject;
        npc2_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject;
        npc3_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC3_SelectToggle").gameObject;
        npc4_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC4_SelectToggle").gameObject;
        npc5_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC5_SelectToggle").gameObject;
        npc6_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC6_SelectToggle").gameObject;
        npc7_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC7_SelectToggle").gameObject;
        npc8_toggle_obj = mainlist_controller_obj.transform.Find("Viewport/Content_Main/NPC8_SelectToggle").gameObject;

        npc1_toggle = npc1_toggle_obj.GetComponent<Toggle>();
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
        npc8_toggle.interactable = true;

        npc2sub_toggle_obj = mainlist_controller_obj.transform.Find("SubView/Viewport/Content_Main/SubView2_SelectToggle").gameObject;
        npc2sub_toggle_obj.SetActive(false);

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false; //BGMをmainListControllerの宴のほうで変えたかどうかのフラグ。変えてた場合、trueで、宴終了後に元のBGMに切り替える。

        
        newAreaReleasePanel_obj = canvas.transform.Find("NewAreaReleasePanel").gameObject;
        newAreaReleasePanel_obj.SetActive(false);

        contestList_ScrollView_obj = canvas.transform.Find("ContestListPanel/ContestList_ScrollView").gameObject;
        contestList_ScrollView_obj.SetActive(false);
        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);

        contest_detailedPanel = canvas.transform.Find("ContestListPanel/Contest_DetailedPanel").gameObject;
        contest_detailedPanel.SetActive(false);

        time_panel = canvas.transform.Find("TimePanel").gameObject;
        money_panel = canvas.transform.Find("MoneyStatus_panel").gameObject;

        //
        //背景と場所名の設定 最初にこれを行う
        //
        BGImagePanel = GameObject.FindWithTag("BG");
        BGImg_List.Clear();
        BGImg_List_mago.Clear();
        i = 0;
        foreach (Transform child in BGImagePanel.transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            BGImg_List.Add(child.gameObject);
            BGImg_List[i].SetActive(false);
            i++;
        }

        //受付シーンで、10個ぐらいコンテストのリスト一覧をだし、そこで指定すると、以下のコンテスト番号を決定
        switch (GameMgr.SceneSelectNum)
        {
            case 0: //春のコンテスト　会場受付

                
                //GameMgr.ContestSelectNum = 10000; //どのコンテストかを指定する Contest_Main_Orでコンテストの設定を決めてる　コンテスト名はその中で決めてる
                GameMgr.Scene_Name = "Or_Contest_Reception_Spring";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 0; //バックボタン押したときの戻り先
                GameMgr.Window_CharaName = "ガトー";

                default_scenetext = "いらっシャ～イ！" + "\n" + "ここは、春コンテストの受付デ～スよ～！";
                break;

            case 10: //夏のコンテスト　会場受付

                //GameMgr.ContestSelectNum = 10000; //どのコンテストかを指定する Contest_Main_Orでコンテストの設定を決めてる　コンテスト名はその中で決めてる
                GameMgr.Scene_Name = "Or_Contest_Reception_Summer";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 10; //バックボタン押したときの戻り先
                GameMgr.Window_CharaName = "ガトー";

                default_scenetext = "ハロー！！" + "\n" + "ここは、夏コンテストの受付デース！！";
                break;

            case 20: //秋のコンテスト　会場受付

                //GameMgr.ContestSelectNum = 10000; //どのコンテストかを指定する Contest_Main_Orでコンテストの設定を決めてる　コンテスト名はその中で決めてる
                GameMgr.Scene_Name = "Or_Contest_Reception_Autumn";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 20; //バックボタン押したときの戻り先
                GameMgr.Window_CharaName = "ガトー";

                default_scenetext = "ようこそ紳士淑女。" + "\n" + "ここは、秋コンテストの受付でございます。";
                break;

            case 30: //冬のコンテスト　会場受付

                //GameMgr.ContestSelectNum = 10000; //どのコンテストかを指定する Contest_Main_Orでコンテストの設定を決めてる　コンテスト名はその中で決めてる
                GameMgr.Scene_Name = "Or_Contest_Reception_Winter";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 30; //バックボタン押したときの戻り先
                GameMgr.Window_CharaName = "ガトー";

                default_scenetext = "こんばんは..。" + "\n" + "ここは.. 冬コンテストの受付です..。";
                break;

        }

        //天気対応
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {
            if (GameMgr.Story_Mode != 0)
            {

                switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
                {
                    case 1:

                        break;

                    case 2: //深夜→朝

                        break;

                    case 3: //朝

                        break;

                    case 4: //昼

                        break;

                    case 5: //夕方

                        BGImg_List_mago[1].gameObject.SetActive(true);
                        break;

                    case 6: //夜

                        BGImg_List_mago[2].gameObject.SetActive(true);

                        npc1_toggle.interactable = false;
                        npc2_toggle.interactable = false;
                        npc3_toggle.interactable = false;
                        npc4_toggle.interactable = false;
                        npc5_toggle.interactable = false;
                        npc6_toggle.interactable = false;
                        npc7_toggle.interactable = false;
                        break;
                }
            }
        }
        //** 場所名設定ここまで **//

        //移動時に調合シーンステータスを0に。
        GameMgr.compound_status = 0;
        GameMgr.compound_select = 0;

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        StartRead = false;
        check_event = false;

        text_scenario();

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    void SettingBGPanel(int _num)
    {
        BGImg_List[_num].gameObject.SetActive(true);

        i = 0;
        foreach (Transform child in BGImg_List[_num].transform) //子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            BGImg_List_mago.Add(child.gameObject);
            BGImg_List_mago[i].SetActive(false);
            i++;
        }
        BGImg_List_mago[0].gameObject.SetActive(true); //朝の画像一番上オブジェクトをON
    }

    void Update()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
            sceneBGM.NowFadeVolumeONBGM();
        }

        //コンテスト失格になった場合の、後処理
        if(GameMgr.contest_LimitTimeOver_After_flag)
        {
            GameMgr.contest_LimitTimeOver_After_flag = false;

            ContestTimeOverAfter();
        }

        //コンテスト終了後　フラグ解放などのイベント発生
        if (GameMgr.NewAreaRelease_flag)
        {
            GameMgr.NewAreaRelease_flag = false;

            //フラグをチェックし、必要ならONにする。
            NewAreaFlagCheck();

            GameMgr.scenario_ON = true;
            newAreaReleasePanel_obj.SetActive(true);
            GameMgr.Scene_Status = 0;
            //sceneBGM.MuteBGM();
        }

        //コンテストリスト選択後、出場するをおすと、宴開始してコンテストシーンへ
        if(GameMgr.Contest_ReadyToStart2)
        {
            GameMgr.Contest_ReadyToStart2 = false;

            if (trans == 1) //カメラが寄っていたら、デフォに戻す。
            {
                //カメラ寄る。
                trans--; //transが1を超えたときに、ズームするように設定されている。

                //intパラメーターの値を設定する.
                maincam_animator.SetInteger("trans", trans);
            }
            else if (trans == 10) //カメラが寄っていたら、デフォに戻す。
            {
                //カメラ寄る。
                trans = 0; //transが1を超えたときに、ズームするように設定されている。

                //intパラメーターの値を設定する.
                maincam_animator.SetInteger("trans", trans);
            }
            On_ActiveContestStart2();
        }
        

        if (GameMgr.Reset_SceneStatus)
        {
            GameMgr.Reset_SceneStatus = false;
            GameMgr.Scene_Status = 0;
            text_scenario();
        }

        //宴途中でブラックをオフにする ドアをあけて会場へ移動する演出用
        if (GameMgr.Utage_SceneEnd_BlackON)
        {
            GameMgr.Utage_SceneEnd_BlackON = false;
            sceneBGM.FadeOutBGM(2.0f);
            scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f);
        }

        //シーンイベントのチェック
        EventCheck();

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            //placename_panel.SetActive(false);
            mainlist_controller_obj.SetActive(false);
            backshopfirst_obj.SetActive(false);
            contestList_ScrollView_obj.SetActive(false);
            time_panel.SetActive(false);
            money_panel.SetActive(false);
        }
        else
        {
            switch (GameMgr.Scene_Status)
            {
                case 0:

                    text_area.SetActive(true);
                    //placename_panel.SetActive(true);
                    mainlist_controller_obj.SetActive(true);
                    backshopfirst_obj.SetActive(false);
                    backshopfirst_obj.GetComponent<Button>().interactable = true;
                    contestList_ScrollView_obj.SetActive(false);
                    time_panel.SetActive(true);
                    money_panel.SetActive(true);

                    sceneBGM.MuteOFFBGM();

                    GameMgr.Scene_Status = 100;
                    GameMgr.Scene_Select = 0;

                    ToggleFlagCheck();

                    if (trans == 1) //カメラが寄っていたら、デフォに戻す。
                    {
                        //カメラ寄る。
                        trans--; //transが1を超えたときに、ズームするように設定されている。

                        //intパラメーターの値を設定する.
                        maincam_animator.SetInteger("trans", trans);
                    }
                    else if (trans == 10) //カメラが寄っていたら、デフォに戻す。
                    {
                        //カメラ寄る。
                        trans = 0; //transが1を超えたときに、ズームするように設定されている。

                        //intパラメーターの値を設定する.
                        maincam_animator.SetInteger("trans", trans);
                    }

                    break;

                case 50: //賞品リストを開き中

                    yes_no_panel.SetActive(false);
                    text_area.SetActive(false);
                    time_panel.SetActive(false);
                    money_panel.SetActive(false);
                    break;

                case 51:　//賞品リスト閉じるボタンおした

                    yes_no_panel.SetActive(true);
                    text_area.SetActive(true);
                    time_panel.SetActive(true);
                    money_panel.SetActive(true);
                    GameMgr.Scene_Status = 100;
                    break;

                case 100: //退避

                    break;

                default:

                    break;
            }
        }
    }

    void EventCheck()
    {
        //強制的に発生するイベントをチェック。はじめてショップへきた時など
        if (!check_event)
        {
            switch (GameMgr.Scene_Name)
            {
                case "Or_Contest_Reception_Spring":

                    EventCheck_OrA1();
                    break;

                case "Or_Contest_Reception_Summer":

                    EventCheck_OrB1();
                    break;

                case "Or_Contest_Reception_Autumn":

                    EventCheck_OrC1();
                    break;

                case "Or_Contest_Reception_Winter":

                    EventCheck_OrD1();
                    break;

            }           
        }
    }

    void EventCheck_OrA1()
    {
        if (!GameMgr.NPCHiroba_eventList[0]) //はじめてきた
        {
            GameMgr.NPCHiroba_eventList[0] = true;

            //宴の処理用に番号を先に渡す　宴切り替えはeventReadingの中でOnにしてる
            GameMgr.hiroba_event_placeNum = 1001; //レセプションの、主にはじめてきたときなどのイベント番号
            GameMgr.hiroba_event_ID = 1000;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            //GameMgr.CompoundEvent_num = 0;
            //GameMgr.CompoundEvent_flag = true;

            check_event = true;

            matplace_database.matPlaceKaikin("Or_Contest_A1"); //解禁

            EventReadingStart();
        }

        if (check_event) //上でイベント発生してたら、被らないように一回チェックを外す
        { }
        else
        {
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                default:

                    break;
            }
        }
    }

    void EventCheck_OrB1()
    {
        if (!GameMgr.NPCHiroba_eventList[0]) //はじめてきた
        {
            GameMgr.NPCHiroba_eventList[0] = true;

            //宴の処理用に番号を先に渡す　宴切り替えはeventReadingの中でOnにしてる
            GameMgr.hiroba_event_placeNum = 1001; //レセプションの、主にはじめてきたときなどのイベント番号
            GameMgr.hiroba_event_ID = 1000;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            //GameMgr.CompoundEvent_num = 0;
            //GameMgr.CompoundEvent_flag = true;

            check_event = true;

            matplace_database.matPlaceKaikin("Or_Contest_A1"); //解禁

            EventReadingStart();
        }

        if (check_event) //上でイベント発生してたら、被らないように一回チェックを外す
        { }
        else
        {
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                default:

                    break;
            }
        }

    }

    void EventCheck_OrC1()
    {
        if (!GameMgr.NPCHiroba_eventList[0]) //はじめてきた
        {
            GameMgr.NPCHiroba_eventList[0] = true;

            //宴の処理用に番号を先に渡す　宴切り替えはeventReadingの中でOnにしてる
            GameMgr.hiroba_event_placeNum = 1001; //レセプションの、主にはじめてきたときなどのイベント番号
            GameMgr.hiroba_event_ID = 1000;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            //GameMgr.CompoundEvent_num = 0;
            //GameMgr.CompoundEvent_flag = true;

            check_event = true;

            matplace_database.matPlaceKaikin("Or_Contest_A1"); //解禁

            EventReadingStart();
        }

        if (check_event) //上でイベント発生してたら、被らないように一回チェックを外す
        { }
        else
        {
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                default:

                    break;
            }
        }

    }

    void EventCheck_OrD1()
    {
        if (!GameMgr.NPCHiroba_eventList[0]) //はじめてきた
        {
            GameMgr.NPCHiroba_eventList[0] = true;

            //宴の処理用に番号を先に渡す　宴切り替えはeventReadingの中でOnにしてる
            GameMgr.hiroba_event_placeNum = 1001; //レセプションの、主にはじめてきたときなどのイベント番号
            GameMgr.hiroba_event_ID = 1000;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            //GameMgr.CompoundEvent_num = 0;
            //GameMgr.CompoundEvent_flag = true;

            check_event = true;

            matplace_database.matPlaceKaikin("Or_Contest_A1"); //解禁

            EventReadingStart();
        }

        if (check_event) //上でイベント発生してたら、被らないように一回チェックを外す
        { }
        else
        {
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                default:

                    break;
            }
        }

    }

    void ToggleFlagCheck()
    {
        Debug.Log("チェック　本日がコンテスト開催日かどうか");

        i = 0;
        flag_chk = false;
        while (i < GameMgr.contest_accepted_list.Count)
        {
            if (GameMgr.contest_accepted_list[i].Month == PlayerStatus.player_cullent_month &&
                GameMgr.contest_accepted_list[i].Day == PlayerStatus.player_cullent_day)
            {
                Debug.Log("本日コンテスト開催日 " + GameMgr.contest_accepted_list[i].Month + "/" + GameMgr.contest_accepted_list[i].Day + " " +
                    GameMgr.contest_accepted_list[i].contestName);
                contest_list = i;
                flag_chk = true;
                
                break;
            }
            i++;
        }

        if(flag_chk)
        {
            flag_chk = false;

            npc2sub_toggle_obj.SetActive(true);
        }
        else
        {
            npc2sub_toggle_obj.SetActive(false);
        }
    }

    void NewAreaFlagCheck()
    {

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

    void text_scenario()
    {
        _text.text = default_scenetext;
    }

    void ContestTimeOverAfter()
    {
        _text.text = "コンテスト失格になってしまった。" + "\n" + "名声が下がった・・。";
    }

    //MainListController2から読み出し
    public void EventReadingStart()
    {
        StartCoroutine("EventReading");
    }

    IEnumerator EventReading()
    {
        GameMgr.scenario_ON = true;
        GameMgr.hiroba_event_flag = true;
        GameMgr.Scene_Select = 1000; //シナリオイベント読み中の状態
        GameMgr.Scene_Status = 1000;

        //Debug.Log("広場イベント　読み中");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        if (GameMgr.Contest_ReadyToStart) //コンテスト開始のイベントだった場合は、このタイミングでコンテスト本番スタート
        {
            GameMgr.Contest_ReadyToStart = false;

            //もしエクストリームパネルにすでにお菓子があった場合は、オリジナルリストへ移動しておく。
            if(pitemlist.player_extremepanel_itemlist.Count > 0)
            {
                pitemlist.ExtremeToCopyOriginalItem(99);
                pitemlist.deleteAllExtremePanelItem();
            }

            _id = conteststartList_database.SearchContestString(GameMgr.contest_accepted_list[contest_list].contestName);

            GameMgr.contest_accepted_list.RemoveAt(contest_list); //受付していたコンテストは削除
            conteststartList_database.conteststart_lists[_id].Contest_Accepted = 0; //DBのフラグもオフに。           

            GameMgr.ContestSelectNum = conteststartList_database.conteststart_lists[_id].Contest_placeNumID;
            GameMgr.Contest_Cate_Ranking = conteststartList_database.conteststart_lists[_id].Contest_RankingType;

            Debug.Log("コンテスト本会場へ移動");
            StartCoroutine("WaitForGotoContest");
            
        }
        else
        {

            GameMgr.Scene_Select = 0; //何もしていない状態
            GameMgr.Scene_Status = 0;

            //読み終わったら、またウィンドウなどを元に戻す。
            text_area.SetActive(true);
            mainlist_controller_obj.SetActive(true);

            //音を戻す。
            if (bgm_change_flag)
            {
                bgm_change_flag = false;
                sceneBGM.FadeInBGM(GameMgr.System_default_sceneFadeBGMTime);
            }

            //読み終わったフラグをたてる
            EventReadEnd_Flagcheck();
            

            ToggleFlagCheck();

            text_scenario(); //テキストの更新
        }
        
    }

    IEnumerator WaitForGotoContest()
    {
        yield return new WaitForSeconds(1.0f); //1秒待つ

        FadeManager.Instance.LoadScene("Or_Contest_A1", GameMgr.SceneFadeTime);
    }

    void EventReadEnd_Flagcheck()
    {
        switch (GameMgr.hiroba_event_ID)
        {
            //クエスト４　「ドーナツ作り」～　0番台
            case 40:

                GameMgr.hiroba_event_end[2] = true;
                break;

            case 1040:

                GameMgr.hiroba_event_end[0] = true;
                break;

            case 2045:

                GameMgr.hiroba_event_end[1] = true;
                break;

            case 3040:

                GameMgr.hiroba_event_end[6] = true;
                break;

            case 3042:

                ev_id = pitemlist.Find_eventitemdatabase("donuts_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ドーナツのレシピを追加

                GameMgr.hiroba_event_end[8] = true;
                break;

            case 4040:

                GameMgr.hiroba_event_end[3] = true;
                break;

            case 4042:

                GameMgr.hiroba_event_end[7] = true;
                break;

            case 5041:

                GameMgr.hiroba_event_end[4] = true;
                break;

            case 5042:

                GameMgr.hiroba_event_end[5] = true;
                break;

            //クエスト５　コンテスト～  10番台
            case 50:

                GameMgr.hiroba_event_end[10] = true;
                break;

            case 3050:

                GameMgr.hiroba_event_end[11] = true;
                break;

            case 4050:

                GameMgr.hiroba_event_end[12] = true;
                break;

            case 5050:

                GameMgr.hiroba_event_end[13] = true;
                break;

            default:

                break;
        }
    }


    //
    //広場 各NPCのイベント実行・フラグ管理
    //

    //NPC1
    public void OnNPC1_toggle()
    {
        if (npc1_toggle.isOn == true)
        {
            npc1_toggle.isOn = false;
            
        }
    }

    //NPC2
    public void OnNPC2_toggle()
    {
        if (npc2_toggle.isOn == true)
        {
            npc2_toggle.isOn = false;

            
        }
    }

    //NPC3
    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;

            
        }
    }

    //NPC4
    public void OnNPC4_toggle()
    {
        if (npc4_toggle.isOn == true)
        {
            npc4_toggle.isOn = false;

            
        }
    }

    //NPC5
    public void OnNPC5_toggle()
    {
        if (npc5_toggle.isOn == true)
        {
            npc5_toggle.isOn = false;

            
        }
    }

    //NPC6
    public void OnNPC6_toggle()
    {
        if (npc6_toggle.isOn == true)
        {
            npc6_toggle.isOn = false;

            
        }
    }

    //NPC7
    public void OnNPC7_toggle()
    {
        if (npc7_toggle.isOn == true)
        {
            npc7_toggle.isOn = false;
            
        }
    }

    //NPC8
    public void OnNPC8_toggle()
    {
        if (npc8_toggle.isOn == true)
        {
            npc8_toggle.isOn = false;

           
        }
    }

    //SubView1
    public void OnSubNPC1_toggle()
    {
        //コンテストリストメニュー開く
        contestList_ScrollView_obj.SetActive(true);
        backshopfirst_obj.SetActive(true);
        mainlist_controller_obj.SetActive(false);

        _text.text = "今開催しているコンテストデ～ス！";

        //カメラ寄る。
        trans++; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);
    }

    //SubView2
    public void OnSubNPC2_toggle()
    {
        On_ActiveContestStart();
        
    }

    //SubView3
    public void OnSubNPC3_toggle()
    {
        On_ActiveContest_explanation();
    }

    //SubView4
    public void OnSubNPC4_toggle()
    {

    }

    //SubView5
    public void OnSubNPC5_toggle()
    {

    }

    //SubView6　立ち去る
    public void OnSubNPC6_toggle()
    {
        //戻る
        GameMgr.SceneSelectNum = backnum;
        FadeManager.Instance.LoadScene("Or_Outside_the_Contest", GameMgr.SceneFadeTime);
    }

    void On_ActiveContestStart()
    {
        //宴の処理へ
        GameMgr.hiroba_event_placeNum = 1000; //

        //イベント発生フラグをチェック
        GameMgr.hiroba_event_ID = 0;

        GameMgr.utage_charaHyouji_flag = true; //宴のキャラ表示する　キャラの切り替えはUtage_Scenario.csでやる
        GameMgr.Contest_ReadyToStart = true; //宴読み終わり後、即コンテストを開始する　trueにしなければ、そこでイベント終了

        EventReadingStart();

        CanvasOff();
    }

    void On_ActiveContestStart2()
    {
        //宴の処理へ
        GameMgr.hiroba_event_placeNum = 1000; //

        //イベント発生フラグをチェック
        GameMgr.hiroba_event_ID = 10;

        GameMgr.utage_charaHyouji_flag = true; //宴のキャラ表示する　キャラの切り替えはUtage_Scenario.csでやる
        GameMgr.Contest_ReadyToStart = true;　//宴読み終わり後、即コンテストを開始する　trueにしなければ、そこでイベント終了

        EventReadingStart();

        CanvasOff();
    }

    void On_ActiveContest_explanation() //コンテストの説明をきく
    {
        //宴の処理へ
        GameMgr.hiroba_event_placeNum = 1002; //
        GameMgr.hiroba_event_ID = 0;
        GameMgr.utage_charaHyouji_flag = true;

        EventReadingStart();

        CanvasOff();
    }

    void CanvasOff()
    {
        text_area.SetActive(false);
        mainlist_controller_obj.gameObject.SetActive(false);
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

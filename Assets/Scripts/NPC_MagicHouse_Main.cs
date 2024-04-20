using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPC_MagicHouse_Main : MonoBehaviour
{
    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject text_area;
    private Text _text;

    private SceneInitSetting sceneinit_setting;

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

    private string default_scenetext;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private List<GameObject> BGImg_List_mago = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 150;

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

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

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

        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);

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

        switch (GameMgr.SceneSelectNum)
        {
            case 0: //火のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Fire";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 13; //バックボタン押したときの戻り先

                default_scenetext = "こんにちは。" + "\n" + "火の魔法を教えてあげますよ。ふふ。";
                break;

            case 10: //氷のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Ice";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 152; //バックボタン押したときの戻り先

                default_scenetext = "ヘイヘイヘーーーーイ！！ボウヤ！" + "\n" + "氷の魔法を習いにきましたか～！？";
                break;

            case 20: //風のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Wind";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 152; //バックボタン押したときの戻り先

                default_scenetext = "はぁあ～・・。だる～い・・。" + "\n" + "風魔法～？　えー・・。";
                break;

            case 30: //光のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Luminous";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 301; //バックボタン押したときの戻り先

                default_scenetext = "..。" + "\n" + "..お前たち。光の魔法を習いにきたのか..？　フン..。";
                break;

            case 40: //星のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Star";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 321; //バックボタン押したときの戻り先

                default_scenetext = "ムッシュ～！" + "\n" + "ここは星の魔法を覚えれますヨ～。";
                break;

            case 50: //森のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Forest";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 321; //バックボタン押したときの戻り先

                default_scenetext = "あんれまあ！" + "\n" + "森の魔法を覚えにきたんかいね？";
                break;

            case 60: //時のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Time";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 321; //バックボタン押したときの戻り先

                default_scenetext = "あら～！" + "\n" + "時の魔法を覚えたいの？";
                break;

            case 70: //音のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Sound";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 321; //バックボタン押したときの戻り先

                default_scenetext = "ハッハー！" + "\n" + "音の魔法を覚えたいのですね～？";
                break;

            case 80: //心のパティシエ魔法の先生

                GameMgr.Scene_Name = "Or_NPC_MagicHouse_Heart";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定
                backnum = 321; //バックボタン押したときの戻り先

                default_scenetext = "おや、きみたちは。" + "\n" + "心の魔法を習いにきたのかい？";
                break;

        }

        //天気対応
        /*if (GameMgr.WEATHER_TIMEMODE_ON)
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
        }*/
        //** 場所名設定ここまで **//

        GameMgr.Scene_Status = 0;
        StartRead = false;

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
        }
       

        if (GameMgr.Reset_SceneStatus)
        {
            GameMgr.Reset_SceneStatus = false;
            GameMgr.Scene_Status = 0;
            text_scenario();
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            //placename_panel.SetActive(false);
            mainlist_controller_obj.SetActive(false);
            backshopfirst_obj.SetActive(false);
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

                    sceneBGM.MuteOFFBGM();

                    GameMgr.Scene_Status = 100;
                    GameMgr.Scene_Select = 0;

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

                case 100: //退避

                    break;

                default:

                    break;
            }
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


    //MainListController2から読み出し
    public void EventReadingStart()
    {
        StartCoroutine("EventReading");
    }

    IEnumerator EventReading()
    {
        GameMgr.hiroba_event_flag = true;
        GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
        GameMgr.compound_status = 1000;

        //Debug.Log("広場イベント　読み中");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        if(GameMgr.Contest_ReadyToStart) //コンテスト開始のイベントだった場合は、このタイミングでコンテスト本番スタート
        {
            GameMgr.Contest_ReadyToStart = false;

            _id = conteststartList_database.SearchContestString(GameMgr.contest_accepted_list[contest_list].contestName);

            GameMgr.contest_accepted_list.RemoveAt(contest_list); //受付していたコンテストは削除
            conteststartList_database.conteststart_lists[_id].Contest_Accepted = 0; //DBのフラグもオフに。

            GameMgr.ContestSelectNum = conteststartList_database.conteststart_lists[_id].Contest_placeNumID;
            GameMgr.Contest_Cate_Ranking = conteststartList_database.conteststart_lists[_id].Contest_RankingType;
            FadeManager.Instance.LoadScene("Or_Contest_A1", GameMgr.SceneFadeTime);
        }
        else
        {
            GameMgr.scenario_read_endflag = false;
            GameMgr.compound_select = 0; //何もしていない状態
            GameMgr.compound_status = 0;

            //読み終わったら、またウィンドウなどを元に戻す。
            text_area.SetActive(true);
            mainlist_controller_obj.SetActive(true);

            //音を戻す。
            if (bgm_change_flag)
            {
                bgm_change_flag = false;
                sceneBGM.FadeInBGM();
            }

            //読み終わったフラグをたてる
            EventReadEnd_Flagcheck();
           

            text_scenario(); //テキストの更新
        }
        
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
        //backshopfirst_obj.SetActive(true);
        //mainlist_controller_obj.SetActive(false);

        _text.text = "いらっしゃ～い・・。" + "\n" + "なにかよう？";

        //カメラ寄る。
        //trans++; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        //maincam_animator.SetInteger("trans", trans);
    }

    //SubView2
    public void OnSubNPC2_toggle()
    {
        On_ActiveContestStart();
        
    }

    //SubView3
    public void OnSubNPC3_toggle()
    {

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
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_ActiveContestStart()
    {
        //噴水押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1000; //

        //イベント発生フラグをチェック
        GameMgr.hiroba_event_ID = 0;

        GameMgr.utage_charaHyouji_flag = true; //宴のキャラ表示する　キャラの切り替えはUtage_Scenario.csでやる
        GameMgr.Contest_ReadyToStart = true;

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

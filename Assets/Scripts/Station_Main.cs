using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Station_Main : MonoBehaviour
{

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


    private ItemMatPlaceDataBase matplace_database;

    private PlayerItemList pitemlist;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject mainlist_controller_obj;
    private SoundController sc;

    private Debug_Panel_Init debug_panel_init;
    private GameObject scene_black_effect;

    private GameObject canvas;

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private BGM sceneBGM;
    private Map_Ambience map_ambience;

    private bool bgm_change_flag;

    private int ev_id;

    private int i, rndnum;

    private bool StartRead;

    private string default_scenetext;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private List<GameObject> BGImg_List_mago = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 140;

        //Debug.Log("Hiroba scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();
        
        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;

        //移動用リストオブジェクトの初期化
        foreach (Transform child in canvas.transform.Find("MainListPanel").transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
        }

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
            case 0: //デフォルト　ガレット村　駅

                GameMgr.Scene_Name = "Sta_Grt";
                SettingBGPanel(0); //Map〇〇のリスト番号を指定                                   
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_01").gameObject; //リストオブジェクトの取得
                mainlist_controller_obj.SetActive(true);

                default_scenetext = "ガレット村の駅についた。";
                break;

            case 100: //オランジーナ　駅

                GameMgr.Scene_Name = "Sta_Or";
                SettingBGPanel(1); //Map〇〇のリスト番号を指定
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_02").gameObject; //リストオブジェクトの取得
                mainlist_controller_obj.SetActive(true);

                default_scenetext = "オランジーナ駅についた。";
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
                        break;
                }
            }
        }
        //** 場所名設定ここまで **//


        //トグル初期状態
        ToggleSetup();

        GameMgr.Scene_Status = 0;
        StartRead = false;

        bgm_change_flag = false;

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

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            //placename_panel.SetActive(false);
            mainlist_controller_obj.SetActive(false);

        }
        else
        {
            switch (GameMgr.Scene_Status)
            {
                case 0:

                    text_area.SetActive(true);
                    //placename_panel.SetActive(true);
                    mainlist_controller_obj.SetActive(true);
                    yes_no_panel.SetActive(false);

                    text_scenario();

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
        GameMgr.Scene_Select = 1000; //シナリオイベント読み中の状態
        GameMgr.Scene_Status = 1000;

        //Debug.Log("広場イベント　読み中");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        

        
        //電車にのる
        if (GameMgr.Station_TrainGoFlag)
        {
            GameMgr.Station_TrainGoFlag = false;

            TrainGo();
            
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
                sceneBGM.FadeInBGM();
            }

            text_scenario(); //テキストの更新
        }
    }

    //電車移動中　暗くなる演出
    void TrainGo()
    {
        sceneBGM.MuteBGM();
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

        StartCoroutine("WaitForTrainGo");
    }

    IEnumerator WaitForTrainGo()
    {
        yield return new WaitForSeconds(2.0f); //1秒待つ

        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.Scene_Select = 0; //何もしていない状態
        GameMgr.Scene_Status = 0;

        switch (GameMgr.Scene_Name)
        {
            case "Sta_Grt":

                GameMgr.SceneSelectNum = 100;
                FadeManager.Instance.LoadScene("Station", GameMgr.SceneFadeTime);
                break;

            case "Sta_Or":

                GameMgr.SceneSelectNum = 0;
                FadeManager.Instance.LoadScene("Station", GameMgr.SceneFadeTime);
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

    //SubView1 アトリエ中へ入る
    public void OnSubNPC1_toggle()
    {
        On_Active01();
        
    }

    //SubView2
    public void OnSubNPC2_toggle()
    {
        mainlist_controller_obj.SetActive(false);
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

    //SubView6
    public void OnSubNPC6_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Sta_Grt":

                //GameMgr.SceneSelectNum = 100;
                GameMgr.Scene_back_home = true;
                FadeManager.Instance.LoadScene("Compound", GameMgr.SceneFadeTime);
                break;

            case "Sta_Or":

                GameMgr.SceneSelectNum = 0;
                FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                break;
        }
    }

    void On_Active01()
    {
        //ステーション　宴の処理へ
        GameMgr.hiroba_event_placeNum = 3000; //

        //sceneBGM.FadeOutBGM();
        //bgm_change_flag = true;

        switch (GameMgr.Scene_Name)
        {
            case "Sta_Grt":

                GameMgr.hiroba_event_ID = 1000;
                break;

            case "Sta_Or":

                GameMgr.hiroba_event_ID = 2000;
                break;
        }


        EventReadingStart();

        CanvasOff();
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

        //一度すべてオフ
        /*foreach (Transform child in mainlist_controller_obj.transform.Find("Viewport/Content_Main").transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
        }*/
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

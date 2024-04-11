using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hiroba1_Main_Controller : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

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

    private ItemMatPlaceDataBase matplace_database;

    private PlayerItemList pitemlist;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject mainlist_controller_obj;

    private GameObject sceneplace_namepanel_obj;
    private ScenePlaceNamePanel sceneplace_namepanel;

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private BGM sceneBGM;
    public bool bgm_change_flag;

    private int ev_id;

    private int rndnum;
    private string default_scenetext;

    private int scene_status, scene_num;
    private bool StartRead;

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

        sceneplace_namepanel_obj = canvas.transform.Find("MainListPanel/ScenePlaceNamePanel").gameObject;
        sceneplace_namepanel = sceneplace_namepanel_obj.GetComponent<ScenePlaceNamePanel>();
        sceneplace_namepanel_obj.SetActive(false);

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //移動用リストオブジェクトの初期化
        foreach (Transform child in canvas.transform.Find("MainListPanel").transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
        }

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false; //BGMをmainListControllerの宴のほうで変えたかどうかのフラグ。変えてた場合、trueで、宴終了後に元のBGMに切り替える。       

        scene_status = 0;
        StartRead = false;

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
            switch (scene_status)
            {
                case 0:

                    text_area.SetActive(false);
                    //placename_panel.SetActive(true);
                    mainlist_controller_obj.SetActive(true);

                    sceneBGM.MuteOFFBGM();

                    scene_status = 100;
                    scene_num = 0;

                    break;

                case 100: //退避

                    break;

                default:

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

        GameMgr.scenario_read_endflag = false;
        GameMgr.compound_select = 0; //何もしていない状態
        GameMgr.compound_status = 0;

        //読み終わったら、またウィンドウなどを元に戻す。
        text_area.SetActive(false);
        mainlist_controller_obj.SetActive(true);

        //音を戻す。
        if (bgm_change_flag)
        {
            bgm_change_flag = false;
            sceneBGM.FadeInBGM();
        }

        //読み終わったフラグをたてる
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

        ToggleFlagCheck();

        text_scenario(); //テキストの更新
    }


    //
    //広場 各NPCのイベント実行・フラグ管理
    //

    //Npc1
    public void OnNPC1_toggle()
    {
        if (npc1_toggle.isOn == true)
        {
            npc1_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_Active01();
                    break;

                case "Or_Hiroba_CentralPark2":

                    On_Active01();
                    break;

                case "Or_Hiroba_Spring_Entrance":

                    On_Active07();
                    break;

                case "Or_Hiroba_Spring_Shoping_Moll":

                    On_ShopActive01();
                    break;

                case "Or_Hiroba_Spring_UraStreet":

                    On_NPC_MagicActive01();
                    break;

                case "Or_Hiroba_Summer_Entrance":

                    On_BarActive02();
                    break;

                case "Or_Hiroba_Summer_MainStreet":

                    On_ShopActive02();
                    break;

                default:

                    On_Active100();
                    break;
            }
        }
    }

    //Npc2
    public void OnNPC2_toggle()
    {
        if (npc2_toggle.isOn == true)
        {
            npc2_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水でToggle2を押した

                    On_Active02();
                    break;

                case "Or_Hiroba_CentralPark2":

                    On_Active04();
                    break;

                case "Or_Hiroba_Spring_Entrance":

                    On_Active05();
                    break;

                case "Or_Hiroba_Spring_Shoping_Moll":

                    On_BarActive01();
                    break;

                default:

                    On_Active101();
                    break;
            }
        }
    }

    //Npc3
    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水でToggle3を押した

                    On_Active03();
                    break;

                case "Or_Hiroba_CentralPark2":

                    On_Active06();
                    break;

                case "Or_Hiroba_Spring_Entrance":

                    On_Active06();
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

                case "Or_Hiroba_Summer_Entrance":

                    On_Active06();
                    break;

                case "Or_Hiroba_Summer_Street":

                    On_Active02();
                    break;

                case "Or_Hiroba_Summer_MainStreet":

                    On_Active10();
                    break;

                case "Or_Hiroba_Autumn_Entrance":

                    On_Active06();
                    break;

                case "Or_Hiroba_Winter_Entrance":

                    On_Active06();
                    break;

                default:

                    On_Active102();
                    break;
            }
        }
    }

    //Npc4
    public void OnNPC4_toggle()
    {
        if (npc4_toggle.isOn == true)
        {
            npc4_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_Active05();
                    break;

                case "Or_Hiroba_Spring_Shoping_Moll":

                    On_Active08();
                    break;

                case "Or_Hiroba_Spring_Oku":

                    On_ContestActive01();
                    break;

                case "Or_Hiroba_Spring_UraStreet":

                    On_Active07();
                    break;

                case "Or_Hiroba_Summer_Entrance":

                    On_Active10();
                    break;

                case "Or_Hiroba_Summer_Street":

                    On_Active11();
                    break;

                default:

                    On_Active103();
                    break;
            }
        }
    }

    //Npc5
    public void OnNPC5_toggle()
    {
        if (npc5_toggle.isOn == true)
        {
            npc5_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_Active04();
                    break;

                case "Or_Hiroba_Spring_Shoping_Moll":

                    On_Active09();
                    break;

                default:

                    On_Active104();
                    break;
            }
        }
    }

    //Npc6
    public void OnNPC6_toggle()
    {
        if (npc6_toggle.isOn == true)
        {
            npc6_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_Active04();
                    break;

                default:

                    On_Active105();
                    break;
            }
        }
    }

    //Npc7
    public void OnNPC7_toggle()
    {
        if (npc7_toggle.isOn == true)
        {
            npc7_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_Active04();
                    break;

                default:

                    On_Active106();
                    break;
            }
        }
    }

    //Npc8
    public void OnNPC8_toggle()
    {
        if (npc8_toggle.isOn == true)
        {
            npc8_toggle.isOn = false;

            switch (GameMgr.Scene_Name)
            {
                case "Or_Hiroba_CentralPark": //中央噴水

                    On_BackHomeActive01();
                    break;

                default:

                    On_BackHomeActive01();
                    break;
            }

        }
    }

    //SubView1
    public void OnSubNPC1_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_Spring_Shoping_Moll": //中央噴水

                On_ShopActive01();
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
            case "Or_Hiroba_Spring_Shoping_Moll": //中央噴水

                On_BarActive01();
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
            case "Or_Hiroba_Spring_Shoping_Moll": //中央噴水

                On_Active04();
                break;

            default:

                On_Active106();
                break;
        }
    }

    //SubView4
    public void OnSubNPC4_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_Spring_Shoping_Moll": //中央噴水

                On_Active04();
                break;

            default:

                On_Active106();
                break;
        }
    }

    //SubView5
    public void OnSubNPC5_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_Active04();
                break;

            default:

                On_Active106();
                break;
        }
    }

    //SubView6
    public void OnSubNPC6_toggle()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                On_Active04();
                break;

            default:

                On_Active106();
                break;
        }
    }


    //
    //以下、イベントアクションの処理一覧
    //
    void On_Active01()
    {
        //_text.text = "春エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み シーン自体は自分を読む
        GameMgr.SceneSelectNum = 10;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active02()
    {
        //_text.text = "夏エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 100;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active03()
    {
        //_text.text = "秋エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 200;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active04()
    {
        //_text.text = "冬エリアへ移動";

        //GameMgr.Scene_back_home = true;
        //メインシーン読み込み
        GameMgr.SceneSelectNum = 300;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active05()
    {
        //_text.text = "噴水エリア　奥側へ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み シーン自体は自分を読む
        GameMgr.SceneSelectNum = 1;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }


    void On_Active06()
    {
        //_text.text = "噴水エリア　手前へ移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active07()
    {
        //_text.text = "春エリア商店街へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 11;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active08()
    {
        //_text.text = "春エリア　奥側へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 12;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active09()
    {
        //_text.text = "春エリア　裏通りへ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 13;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active10()
    {
        //_text.text = "夏エリア入口　奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 101;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_Active11()
    {
        //_text.text = "夏エリア入口　奥へ　移動";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 102;
        FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
    }

    void On_ShopActive01()
    {
        //_text.text = "春エリアのお店へ入る";

        //入店の音
        sc.ShopEnterSound01();
        
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
    }

    void On_ShopActive02()
    {
        //_text.text = "夏エリアのお店へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Shop", GameMgr.SceneFadeTime);
    }

    void On_BarActive01()
    {
        //_text.text = "春エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
    }

    void On_BarActive02()
    {
        //_text.text = "夏エリアの酒場へ入る";

        //入店の音
        sc.ShopEnterSound01();

        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
    }

    void On_ContestActive01()
    {
        //_text.text = "春エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Outside_the_Contest", GameMgr.SceneFadeTime);
    }

    void On_NPC_MagicActive01()
    {
        //_text.text = "春エリアの魔法の先生の家へ入る";

        //GameMgr.Scene_back_home = true;
        //シーン読み込み
        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_NPC_MagicHouse", GameMgr.SceneFadeTime);
    }

    void On_BackHomeActive01()
    {
        //_text.text = "春エリアのコンテスト01";

        //GameMgr.Scene_back_home = true;
        //アトリエに戻る
        FadeManager.Instance.LoadScene("Or_Compound_Enterance", GameMgr.SceneFadeTime);
    }

    void On_Active100()
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

        CanvasOff();

    }

    void On_Active101()
    {
        //噴水押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 1; //

        //イベント発生フラグをチェック
        switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
        {
            case 40: //ドーナツイベント時

                if (!GameMgr.hiroba_event_end[0])
                {
                    sceneBGM.FadeOutBGM();
                    bgm_change_flag = true;
                    GameMgr.hiroba_event_ID = 1040;

                    //MenuWindowExpand();
                }
                else
                {
                    GameMgr.hiroba_event_ID = 1041;
                }
                break;

            case 50:

                GameMgr.hiroba_event_ID = 1050;
                break;

            default:

                GameMgr.hiroba_event_ID = 1000;
                break;
        }

        EventReadingStart();

        CanvasOff();
    }

    void On_Active102()
    {
        //村長の家押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 2; //

        if (GameMgr.Story_Mode == 0)
        {
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    /*if (!GameMgr.hiroba_event_end[0] || !GameMgr.hiroba_event_end[3] || !GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 2040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else //最初アマクサにあったら、すぐイベントが進む。
                    {*/
                    if (!GameMgr.hiroba_event_end[1])
                    {
                        sceneBGM.FadeOutBGM();
                        bgm_change_flag = true;
                        GameMgr.hiroba_event_ID = 2045;
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 2046;
                    }

                    //}
                    break;

                case 50:

                    GameMgr.hiroba_event_ID = 2050;
                    break;

                default:

                    GameMgr.hiroba_event_ID = 2000;
                    break;
            }
        }
        else
        {
            sceneBGM.FadeOutBGM();
            bgm_change_flag = true;
            GameMgr.hiroba_event_ID = 12000;
        }

        EventReadingStart();

        CanvasOff();
    }

    void On_Active103()
    {
        //パン工房押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 3; //

        //イベント発生フラグをチェック
        switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
        {
            case 40: //ドーナツイベント時

                if (!GameMgr.hiroba_event_end[8])
                {
                    if (!GameMgr.hiroba_event_end[6])
                    {
                        sceneBGM.FadeOutBGM();
                        bgm_change_flag = true;
                        GameMgr.hiroba_event_ID = 3040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        //ひまわり油をもっていたら、イベントが進む。ひまわり油は削除する。
                        if (pitemlist.ReturnItemKosu("himawari_Oil") >= 1)
                        {
                            pitemlist.SearchDeleteItem("himawari_Oil");
                            pitemlist.addPlayerItemString("flyer", 1);

                            sceneBGM.FadeOutBGM();
                            bgm_change_flag = true;
                            GameMgr.hiroba_event_ID = 3042;
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 3041;
                        }
                    }
                }
                else //ドーナツレシピを教わった。
                {
                    GameMgr.hiroba_event_ID = 3043;

                }
                break;

            case 50:

                if (!GameMgr.hiroba_event_end[11])
                {
                    sceneBGM.FadeOutBGM();
                    bgm_change_flag = true;
                    GameMgr.hiroba_event_ID = 3050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                }
                else
                {
                    sceneBGM.FadeOutBGM();
                    bgm_change_flag = true;
                    GameMgr.hiroba_event_ID = 3051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                }

                break;

            default:

                GameMgr.hiroba_event_ID = 3000;
                break;
        }

        EventReadingStart();

        CanvasOff();
    }

    void On_Active104()
    {
        //お花屋さん押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 4; //

        if (GameMgr.Story_Mode == 0)
        {
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[6])
                    {
                        if (!GameMgr.hiroba_event_end[3])
                        {
                            GameMgr.hiroba_event_ID = 4040;
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 4041;
                        }
                    }
                    else //油の話をききにくる。
                    {
                        if (!GameMgr.hiroba_event_end[7])
                        {
                            GameMgr.hiroba_event_ID = 4042;
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 4043;
                        }
                    }
                    break;

                case 50:

                    if (!GameMgr.hiroba_event_end[12])
                    {
                        GameMgr.hiroba_event_ID = 4050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 4051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }

                    break;

                default:

                    GameMgr.hiroba_event_ID = 4000;
                    break;
            }
        }
        else
        {
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 50:

                    GameMgr.hiroba_event_ID = 14050; //そのときに呼び出すイベント番号 placeNumとセットで使う。

                    break;

                default:

                    GameMgr.hiroba_event_ID = 100000;
                    break;
            }
        }

        EventReadingStart();

        CanvasOff();
    }

    void On_Active105()
    {
        //図書館押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 5; //

        //図書室はBGMかえる
        sceneBGM.FadeOutBGM();
        bgm_change_flag = true;

        if (GameMgr.Story_Mode == 0)
        {
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[4] && !GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 5040;
                    }
                    else if (GameMgr.hiroba_event_end[4] && !GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 5041;
                    }
                    else if (GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 5042;
                    }
                    break;

                case 50:

                    if (!GameMgr.hiroba_event_end[13])
                    {
                        GameMgr.hiroba_event_ID = 5050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 5051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }

                    break;

                default:

                    GameMgr.hiroba_event_ID = 5000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }
        }
        else
        {
            if (GameMgr.GirlLoveEvent_num == 50) //コンテスト時
            {
                if (!GameMgr.hiroba_event_end[13])
                {
                    GameMgr.hiroba_event_ID = 5050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                }
                else
                {
                    GameMgr.hiroba_event_ID = 5051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                }
            }
            else
            {
                GameMgr.hiroba_event_ID = 15000;
            }
        }

        EventReadingStart();

        CanvasOff();
    }

    void On_Active106()
    {
        //井戸端の奥さん押した　宴の処理へ
        GameMgr.hiroba_event_placeNum = 6; //

        if (GameMgr.Story_Mode == 0)
        {
            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。ベニエのこともあるし、お菓子のレシピや場所のヒント、だったりもする。
                    rndnum = Random.Range(0, 5);
                    GameMgr.hiroba_event_ID = 6040 + rndnum;
                    break;

                case 50: //

                    //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。
                    rndnum = Random.Range(0, 5);
                    GameMgr.hiroba_event_ID = 6050;
                    break;

                default:

                    GameMgr.hiroba_event_ID = 6000;
                    break;
            }
        }
        else
        {
            //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。
            rndnum = Random.Range(0, 7);
            GameMgr.hiroba_event_ID = 16000 + rndnum;
        }

        EventReadingStart();

        CanvasOff();
    }

    
    //
    //その他処理　publicは、同じオブジェクトにつけたHiroba1_Main_Orのcsから読み出し
    //

    void CanvasOff()
    {
        text_area.SetActive(false);
        mainlist_controller_obj.gameObject.SetActive(false);
    }

    public void ToggleAllOff()
    {
        npc1_toggle.interactable = false;
        npc2_toggle.interactable = false;
        npc3_toggle.interactable = false;
        npc4_toggle.interactable = false;
        npc5_toggle.interactable = false;
        npc6_toggle.interactable = false;
        npc7_toggle.interactable = false;
    }

    public void SceneToggleDefaultSetup()
    {
        switch (GameMgr.Scene_Name)
        {
            case "Or_Hiroba_CentralPark": //中央噴水

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_01").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オランジーナの街の中央噴水だ。" + "\n" + "大きい噴水がある。";

                break;

            case "Or_Hiroba_CentralPark2": //中央噴水のお散歩小道

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_02").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ちいさな散歩道だ。";

                break;

            case "Or_Hiroba_Spring_Entrance": //春のエリア入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_03").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スプリングガーデンの入口だ。" + "\n" + "春の商店街へ続く道やコンテスト会場がある。";

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

            case "Or_Hiroba_Summer_Entrance": //夏エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_100").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、サマー・ドリームスの入口だ。" + "\n" + "青っぽい家が多い。";

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

            case "Or_Hiroba_Autumn_Entrance": //秋エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_200").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、オータム・リーブスの入口だ。" + "\n" + "紅葉の赤やカラフルな色に包まれている。";

                break;

            case "Or_Hiroba_Winter_Entrance": //秋エリア　入口

                //移動用リストオブジェクトの取得
                mainlist_controller_obj = canvas.transform.Find("MainListPanel/MainList_ScrollView_300").gameObject;
                mainlist_controller_obj.SetActive(true);
                ToggleSetup();

                default_scenetext = "ここは、スノーマンズ・レストの入口だ。" + "\n" + "夜の真っ暗で幻想的な雰囲気に包まれている。";

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

    public void ToggleFlagCheck()
    {
        /*
        if (GameMgr.Story_Mode == 0)
        {
            //新しく3人のNPCのとこへいける
            if (GameMgr.hiroba_event_end[0])
            {
                npc5_toggle_obj.SetActive(true);
                npc6_toggle_obj.SetActive(true);
                npc7_toggle_obj.SetActive(true);
            }

            //パン工房へいける
            if (GameMgr.hiroba_event_end[0] && GameMgr.hiroba_event_end[1])
            {
                npc4_toggle_obj.SetActive(true);
            }

            //ストロベリーガーデンへいける
            if (GameMgr.hiroba_event_end[2])
            {
                matplace_database.matPlaceKaikin("StrawberryGarden"); //ストロベリーガーデン解禁　いちごがとれるようになる。
            }

            //ひまわり畑へいける
            if (GameMgr.hiroba_event_end[7])
            {
                matplace_database.matPlaceKaikin("HimawariHill"); //ひまわり畑解禁　ひまわりの種がとれるようになる。
            }

            if (GameMgr.hiroba_event_end[0])
            {
                MenuWindowExpand();
            }
        }
        else
        {
            MenuWindowExpand();
            npc4_toggle_obj.SetActive(true);
            npc5_toggle_obj.SetActive(true);
            npc6_toggle_obj.SetActive(true);
            npc7_toggle_obj.SetActive(true);

            //3番街へいける
            if (GameMgr.GirlLoveSubEvent_stage1[160])
            {
                npc8_toggle_obj.SetActive(true);
            }
        }*/

    }

    //ネームプレートの設定とアニメーションON
    public void SceneNamePlateSetting()
    {
        sceneplace_namepanel.OnSceneNamePlate();
        sceneplace_namepanel_obj.SetActive(true);
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Outside_theContest_Main : MonoBehaviour
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

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private GameObject BG_Imagepanel;

    private BGM sceneBGM;
    public bool bgm_change_flag;

    private GridLayoutGroup gridlayout;
    private GameObject list_BG;
    private Vector3 defaultPos;
    private Vector3 MovedPos;

    private GameObject newAreaReleasePanel_obj;

    private int ev_id;

    private int rndnum;

    private int scene_status, scene_num;
    private bool StartRead;

    // Use this for initialization
    void Start()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 110;

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

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //リストオブジェクトの取得
        mainlist_controller_obj = canvas.transform.Find("MainList_ScrollView").gameObject;

        //リストオブジェクトのレイアウトグループ情報の取得
        gridlayout = mainlist_controller_obj.transform.Find("Viewport/Content_Main").GetComponent<GridLayoutGroup>();
        list_BG = mainlist_controller_obj.transform.Find("ListBGimage").gameObject;
        defaultPos = mainlist_controller_obj.transform.localPosition;
        

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
       

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false; //BGMをmainListControllerの宴のほうで変えたかどうかのフラグ。変えてた場合、trueで、宴終了後に元のBGMに切り替える。

        text_scenario();

        newAreaReleasePanel_obj = canvas.transform.Find("NewAreaReleasePanel").gameObject;
        newAreaReleasePanel_obj.SetActive(false);

        BG_Imagepanel = GameObject.FindWithTag("BG");

        if (GameMgr.Story_Mode != 0)
        {
            //まずリセット
            BG_Imagepanel.transform.Find("BGPlace_01/BG_sprite_morning").gameObject.SetActive(true);
            BG_Imagepanel.transform.Find("BGPlace_01/BG_sprite_evening").gameObject.SetActive(false);
            BG_Imagepanel.transform.Find("BGPlace_01/BG_sprite_night").gameObject.SetActive(false);

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

                    BG_Imagepanel.transform.Find("BG_sprite_evening").gameObject.SetActive(true);
                    break;

                case 6: //夜

                    BG_Imagepanel.transform.Find("BG_sprite_night").gameObject.SetActive(true);

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

        scene_status = 0;
        StartRead = false;

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    void Update()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
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
            scene_status = 0;
            //sceneBGM.MuteBGM();
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

                    text_area.SetActive(true);
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

    void ToggleFlagCheck()
    {

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

                _text.text = "クープデュモンドのコンテスト会場前だ。";
                break;
        }
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
        text_area.SetActive(true);
        mainlist_controller_obj.SetActive(true);

        //音を戻す。
        if (bgm_change_flag)
        {
            bgm_change_flag = false;
            sceneBGM.FadeInBGM();
        }

        //読み終わったフラグをたてる
        switch(GameMgr.hiroba_event_ID)
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

    //コンテスト　中に入る
    public void OnNPC1_toggle()
    {
        if (npc1_toggle.isOn == true)
        {
            npc1_toggle.isOn = false;

            FadeManager.Instance.LoadScene("Or_Contest_A1", 0.3f);
            //EventReadingStart();

            //CanvasOff();
        }
    }

    //噴水
    public void OnNPC2_toggle()
    {
        if (npc2_toggle.isOn == true)
        {
            npc2_toggle.isOn = false;

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
    }

    //村長の家
    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;

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
    }

    //パン工房
    public void OnNPC4_toggle()
    {
        if (npc4_toggle.isOn == true)
        {
            npc4_toggle.isOn = false;

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
    }

    //お花屋さん
    public void OnNPC5_toggle()
    {
        if (npc5_toggle.isOn == true)
        {
            npc5_toggle.isOn = false;

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

                        GameMgr.hiroba_event_ID = 14000;
                        break;
                }
            }

            EventReadingStart();

            CanvasOff();
        }
    }

    //図書館
    public void OnNPC6_toggle()
    {
        if (npc6_toggle.isOn == true)
        {
            npc6_toggle.isOn = false;

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
    }

    //井戸端の奥さん
    public void OnNPC7_toggle()
    {
        if (npc7_toggle.isOn == true)
        {
            npc7_toggle.isOn = false;

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
    }

    //広場に戻る
    public void OnNPC8_toggle()
    {
        if (npc8_toggle.isOn == true)
        {
            npc8_toggle.isOn = false;

            //3番街へ（主にNPCイベント関係）
            FadeManager.Instance.LoadScene("Or_Hiroba1", 0.3f);
        }
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
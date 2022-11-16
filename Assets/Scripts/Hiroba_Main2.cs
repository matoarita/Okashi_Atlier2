using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hiroba_Main2 : MonoBehaviour
{
    //
    //** 広場全部で共通スクリプト **
    //

    private GameObject text_area;
    private Text _text;

    private PlayerItemList pitemlist;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject mainlist_controller2_obj;
    private MainListController2 mainlist_controller2;

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private GameObject timepanel;
    private TimeController time_controller;

    private GameObject BG_Imagepanel;

    private BGM sceneBGM;
    public bool bgm_change_flag;

    private int ev_id;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Hiroba scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false; //BGMをmainListControllerの宴のほうで変えたかどうかのフラグ。変えてた場合、trueで、宴終了後に元のBGMに切り替える。

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //所持アイテム画面を開く。初期設定で最初はOFF。
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        mainlist_controller2_obj = canvas.transform.Find("MainList_ScrollView").gameObject;
        mainlist_controller2 = mainlist_controller2_obj.GetComponent<MainListController2>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        text_scenario();

        //text_area.SetActive(false);
        playeritemlist_onoff.SetActive(false);

        //時間のチェック
        //時間管理オブジェクトの取得
        timepanel = canvas.transform.Find("TimePanel").gameObject;
        time_controller = canvas.transform.Find("TimePanel").GetComponent<TimeController>();

        //time_controller.TimeCheck_flag = true;
        //time_controller.TimeKoushin(); //時間の更新

        BG_Imagepanel = GameObject.FindWithTag("BG");

        if (GameMgr.Story_Mode != 0)
        {
            //まずリセット
            BG_Imagepanel.transform.Find("BG_sprite_morning").gameObject.SetActive(true);
            BG_Imagepanel.transform.Find("BG_sprite_evening").gameObject.SetActive(false);
            BG_Imagepanel.transform.Find("BG_sprite_night").gameObject.SetActive(false);

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

                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

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

                _text.text = "ここは、村の中央広場のようだ。いろんな人がいるみたいだ。";
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
        text_area.SetActive(true);
        mainlist_controller2_obj.SetActive(true);

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
        mainlist_controller2.ToggleFlagCheck();

        text_scenario(); //テキストの更新
    }
}

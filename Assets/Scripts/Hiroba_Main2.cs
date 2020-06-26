using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hiroba_Main2 : MonoBehaviour
{

    private GameObject text_area;
    private Text _text;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject mainlist_controller2_obj;
    private MainListController2 mainlist_controller2;

    private Debug_Panel_Init debug_panel_init;

    private GameObject canvas;

    private GameObject timepanel;
    private TimeController time_controller;

    private BGM sceneBGM;
    public bool bgm_change_flag;

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

        //所持アイテム画面を開く。初期設定で最初はOFF。
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

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

        time_controller.TimeCheck_flag = true;
        time_controller.TimeKoushin(); //時間の更新

    }

    // Update is called once per frame
    void Update()
    {

    }

    void text_scenario()
    {
        switch (GameMgr.scenario_flag)
        {

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
        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;

        //読み終わったら、またウィンドウなどを元に戻す。
        text_area.SetActive(true);
        timepanel.SetActive(true);
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
            case 40:

                GameMgr.hiroba_event_end[2] = true;                
                break;

            case 1040:

                GameMgr.hiroba_event_end[0] = true;
                break;

            case 2045:

                GameMgr.hiroba_event_end[1] = true;
                break;

            default:

                break;
        }
        mainlist_controller2.ToggleFlagCheck();

    }
}

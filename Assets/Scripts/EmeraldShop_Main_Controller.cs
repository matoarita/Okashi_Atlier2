using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EmeraldShop_Main_Controller : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private ItemShopDataBase shop_database;
    private ItemMatPlaceDataBase matplace_database;

    private SceneInitSetting sceneinit_setting;

    private SoundController sc;
    private Girl1_status girl1_status;

    private BGM sceneBGM;

    private GameObject text_area;
    private Text _text;
    private string shopdefault_text;

    private Debug_Panel_Init debug_panel_init;

    private GameObject placename_panel;

    private GameObject shopitemlist_onoff;
    private GameObject shopquestlist_obj;

    private GameObject money_status_obj;

    private GameObject character;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject backshopfirst_obj;

    private GameObject black_effect;

    private GameObject canvas;
    private GameObject shop_select;
    private GameObject shopon_toggle_buy;
    private GameObject shopon_toggle_sell;
    private GameObject shopon_toggle_talk;
    private GameObject shopon_toggle_quest;
    private GameObject shopon_toggle_uwasa;
    private GameObject shopon_toggle_back;

    private bool StartRead;
    private bool check_event;

    //public int shop_status;
    //public int shop_scene; //どのシーンを選択しているかを判別

    private int i;

    // Use this for initialization
    void Start () {
		
	}

    public void InitSetup()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 50;

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //黒パネルの取得
        black_effect = canvas.transform.Find("BlackBG").gameObject;
        if (!GameMgr.emeraldShopEvent_stage[0]) { black_effect.SetActive(true); }
        else { black_effect.SetActive(false); }

        character = GameObject.FindWithTag("Character");
        character.GetComponent<FadeCharacter>().SetOff();

        shop_select = canvas.transform.Find("Shop_Select").gameObject;
        shopon_toggle_buy = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Buy").gameObject;
        //shopon_toggle_sell = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Sell").gameObject;
        shopon_toggle_talk = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Talk").gameObject;
        shopon_toggle_back = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Back").gameObject;
        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);

        //自分の持ってるお金などのステータス
        money_status_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;
        money_status_obj.SetActive(false);

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        playeritemlist_onoff.SetActive(false); //

        //ショップリスト画面。初期設定で最初はOFF。
        shopitemlist_onoff = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shopitemlist_onoff.SetActive(false);

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();
        text_area.GetComponent<MessageWindow>().DrawIcon(); //顔アイコンの有無　再設定

        //初期メッセージ
        shopdefault_text = "ニャニャ。よ～見つけなすったね。こんなところを・・。" + "\n" + "それで何がほしいニャ？";
        _text.text = shopdefault_text;
        text_area.SetActive(false);

        //移動時に調合シーンステータスを0に。
        GameMgr.compound_status = 0;
        GameMgr.compound_select = 0;

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        StartRead = false;
        check_event = false;

        //シーン読み込みのたびに、ショップの在庫をMaxにしておく。イベントアイテムは補充しない。
        for (i = 0; i < shop_database.shopitems.Count; i++)
        {
            if (shop_database.shopitems[i].shop_itemType == 0 || shop_database.shopitems[i].shop_itemType == 3)
            {
                shop_database.shopitems[i].shop_itemzaiko = shop_database.shopitems[i].shop_itemzaiko_max;
            }
            else
            {

            }
        }

        //入店の音
        sc.PlaySe(51);

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateEmeraldScene()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
            sceneBGM.NowFadeVolumeONBGM();
        }

        //強制的に発生するイベントをチェック。はじめてショップへきた時など
        EventCheck();

        if (GameMgr.Reset_SceneStatus)
        {
            GameMgr.Reset_SceneStatus = false;
            GameMgr.Scene_Status = 0;
        }

        //宴途中でブラックをオフにする
        if (GameMgr.Scene_Black_Off)
        {
            GameMgr.Scene_Black_Off = false;
            black_effect.SetActive(false);
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            playeritemlist_onoff.SetActive(false);
            shopitemlist_onoff.SetActive(false);
            backshopfirst_obj.SetActive(false);
            shop_select.SetActive(false);
            text_area.SetActive(false);
            placename_panel.SetActive(false);

        }
        else
        {
            //Debug.Log("shop_status" + shop_status);
            switch (GameMgr.Scene_Status)
            {
                case 0:

                    character.GetComponent<FadeCharacter>().SetOn();
                    shopitemlist_onoff.SetActive(false);
                    playeritemlist_onoff.SetActive(false);
                    backshopfirst_obj.SetActive(false);
                    shop_select.SetActive(true);
                    text_area.SetActive(true);
                    placename_panel.SetActive(true);
                    black_effect.SetActive(false);

                    _text.text = shopdefault_text;

                    GameMgr.Scene_Select = 0;
                    GameMgr.Scene_Status = 100;

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

                case 1: //ショップのアイテム選択中
                    break;

                case 2: //話す
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

            if (!GameMgr.emeraldShopEvent_stage[0]) //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。チュートリアルするかどうか。
            {
                GameMgr.emeraldShopEvent_stage[0] = true;
                GameMgr.scenario_ON = true;

                GameMgr.emeraldshop_event_num = 0;
                GameMgr.emeraldshop_event_flag = true;

                //メイン画面にもどったときに、イベントを発生させるフラグをON
                //GameMgr.CompoundEvent_num = 0;
                //GameMgr.CompoundEvent_flag = true;

                check_event = true;

                StartCoroutine("Scenario_loading");
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

            if (check_event) //上でイベント発生してたら、被らないように一回チェックを外す
            { }
            else
            {

            }
        }
    }

    public void OnCheck_1() //ショップ　アイテムを買う
    {
        if (shopon_toggle_buy.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_buy.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shopitemlist_onoff.SetActive(true); //ショップリスト画面を表示。
            backshopfirst_obj.SetActive(true);
            shop_select.SetActive(false);
            placename_panel.SetActive(false);

            GameMgr.Scene_Status = 1; //ショップのシーンに入っている、というフラグ
            GameMgr.Scene_Select = 1;

            _text.text = "何がほしいのかえ？";

        }
    }

    public void OnCheck_2() //話す
    {
        if (shopon_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 2; //眺めるを押したときのフラグ
            GameMgr.Scene_Select = 2;

            _text.text = "..（今はしゃべる気がないようだ。）";

            /*
            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100;

            StartCoroutine("UtageEndWait");*/
        }
    }

    public void OnCheck_Back() //ショップからでて、広場に戻る
    {
        if (shopon_toggle_back.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_back.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            //店のドア音
            sc.PlaySe(38);
            sc.PlaySe(51);

            switch (GameMgr.Scene_Name)
            {
                case "Or_EmeraldShop_A1": //春エリア

                    GameMgr.SceneSelectNum = 20;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_EmeraldShop_B1": //夏エリア

                    GameMgr.SceneSelectNum = 100;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                default:

                    break;
            }
        }
    }

    //アトリエに戻る
    public void OnCheck_BackHome()
    {
        //店のドア音
        sc.PlaySe(38);
        sc.PlaySe(51);

        GameMgr.Scene_back_home = true;

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Or_Compound", GameMgr.SceneFadeTime);
    }

    IEnumerator UtageEndWait()
    {
        while (GameMgr.scenario_ON)
        {
            yield return null;
        }

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;
    }

    IEnumerator Scenario_loading()
    {
        //Debug.Log("シナリオ開始");

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        //Debug.Log("シナリオ終了");
        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        check_event = false;
        GameMgr.Scene_Status = 0;

    }

    public void SceneNamePlateSetting()
    {
        placename_panel.GetComponent<PlaceNamePanel>().OnSceneNamePlate();
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

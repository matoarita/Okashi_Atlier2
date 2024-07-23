using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//ショップシーン間で共通の処理

public class Shop_Main_Controller : MonoBehaviour {

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
    private ShopItemListController shoplistController;

    private GameObject money_status_obj;

    public GameObject hukidasi_sub;
    private GameObject hukidasi_sub_Prefab;

    private GameObject character;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject backshopfirst_obj;

    private GameObject black_effect;

    private GameObject canvas;
    private GameObject shop_select;
    private GameObject shopon_toggle_buy;
    private GameObject shopon_toggle_sell;
    private GameObject shopon_toggle_talk;
    private GameObject shopon_toggle_quest;
    private GameObject shopon_toggle_uwasa;
    private GameObject shopon_toggle_present;
    private GameObject shopon_toggle_compound;
    private GameObject shopon_toggle_back;

    private bool check_event;
    private bool check_lvevent;
    private bool lvevent_loading;

    private int shop_hyouji_flag;

    //public int shop_status;
    //public int shop_scene; //どのシーンを選択しているかを判別

    private bool hukidasi_oneshot; //吹き出しの作成は一つのみ

    private int i;

    private List<bool> shopuwasa_List = new List<bool>();
    private List<int> random_uwasa_select = new List<int>();
    private int uwasalist_count;
    private int rnd;
    private bool StartRead;

    // Use this for initialization
    void Start () {

        
    }

    public void InitSetup()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 20;

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

        //吹き出しプレファブの取得
        hukidasi_sub_Prefab = (GameObject)Resources.Load("Prefabs/Emo_Hukidashi_Anim");

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //プレイヤー所持アイテムリストパネルの取得
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        //レシピリストパネルの取得
        recipilist_onoff = canvas.transform.Find("RecipiList_ScrollView").gameObject;
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();

        character = GameObject.FindWithTag("Character");
        character.GetComponent<FadeCharacter>().SetOff();

        hukidasi_oneshot = false;

        shop_select = canvas.transform.Find("Shop_Select").gameObject;
        shopon_toggle_buy = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Buy").gameObject;
        shopon_toggle_sell = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Sell").gameObject;
        shopon_toggle_talk = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Talk").gameObject;
        shopon_toggle_quest = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Quest").gameObject;
        shopon_toggle_uwasa = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Uwasa").gameObject;
        shopon_toggle_present = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Present").gameObject;
        shopon_toggle_present.SetActive(false);
        shopon_toggle_compound = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Compound").gameObject;
        shopon_toggle_back = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Back").gameObject;
        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);
        //shopon_toggle_quest.SetActive(false);

        //自分の持ってるお金などのステータス
        money_status_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;
        money_status_obj.SetActive(false);

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //ショップリスト画面。初期設定で最初はOFF。
        shopitemlist_onoff = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shoplistController = shopitemlist_onoff.GetComponent<ShopItemListController>();
        shopitemlist_onoff.SetActive(false);

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();
        text_area.GetComponent<MessageWindow>().DrawIcon(); //顔アイコンの有無　再設定

        //初期メッセージ
        shopdefault_text = "いらっしゃい～。";
        _text.text = shopdefault_text;
        text_area.SetActive(false);

        //移動時に調合シーンステータスを0に。
        GameMgr.compound_status = 0;
        GameMgr.compound_select = 0;

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

        check_event = false; //強制で発生するイベントのフラグ
        check_lvevent = false; //レベルに応じて発生するイベントのフラグ
        lvevent_loading = false;

        //シーンごとのフラグチェック
        SceneFlagcheck();

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
        if (!GameMgr.ShopEnter_ButtonON) //重複防止 trueのときは音ならさない
        {
            sc.PlaySe(51);

        }
        GameMgr.ShopEnter_ButtonON = false;       
        

        StartRead = false;
        GameMgr.Scene_LoadedOn_End = true; //シーン読み込み完了

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    void SceneFlagcheck()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                if (GameMgr.Story_Mode == 1)
                {
                    //あるクエスト以降、プリンさんにお菓子渡せる。
                    if (GameMgr.GirlLoveEvent_num >= 11)
                    {
                        shopon_toggle_present.SetActive(true);
                    }
                }
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void UpdateShopScene()
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



        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            //playeritemlist_onoff.SetActive(false);
            shopitemlist_onoff.SetActive(false);
            backshopfirst_obj.SetActive(false);
            shop_select.SetActive(false);
            text_area.SetActive(false);
            money_status_obj.SetActive(false);
            placename_panel.SetActive(false);
            black_effect.SetActive(false);

        }
        else
        {
            if (!check_lvevent) //ショップの品数が増えるなど、パティシエレベルや好感度に応じたイベントの発生フラグをチェック
            {
                Debug.Log("チェック　パティシエレベルor好感度レベルイベント");
                CheckShopLvEvent();

                if (lvevent_loading) { }
                else
                {
                    //すべてのイベントをチェックし終わって、何もなければfalseになっており、lveventのチェックをtrueにして終了する。はず。
                    check_lvevent = true;
                }
            }
            else
            {
                //Debug.Log("shop_status" + shop_status);
                switch (GameMgr.Scene_Status)
                {
                    case 0:

                        character.GetComponent<FadeCharacter>().SetOn();
                        shopitemlist_onoff.SetActive(false);

                        backshopfirst_obj.SetActive(false);
                        backshopfirst_obj.GetComponent<Button>().interactable = true;
                        shop_select.SetActive(true);
                        text_area.SetActive(true);
                        money_status_obj.SetActive(true);
                        placename_panel.SetActive(true);
                        black_effect.SetActive(false);

                        if (playeritemlist_onoff != null && playeritemlist_onoff.activeInHierarchy)
                        {
                            playeritemlist_onoff.SetActive(false);
                        }

                        //_text.text = shopdefault_text;

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

                    case 2:
                        break;

                    case 3: //クエスト選択中
                        break;

                    case 4: //うわさ話聞き中

                        break;

                    case 5: //ショップの売る選択中

                        break;

                    case 100: //退避
                        break;

                    case 500: //調合用

                        //調合終了まち
                        if (GameMgr.CompoundSceneStartON == false)
                        {
                            GameMgr.compound_select = 0; //何もしていない状態
                            GameMgr.compound_status = 0;

                            GameMgr.Scene_Status = 0;
                            GameMgr.Scene_Select = 0;
                        }
                        break;

                    default:
                        break;


                }
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
                case "Shop_Grt":

                    EventCheck_Grt();
                    break;

                case "Or_Shop_A1":

                    EventCheck_OrA1();
                    break;

                case "Or_Shop_B1":

                    EventCheck_OrB1();
                    break;

                case "Or_Shop_C1":

                    EventCheck_OrC1();
                    break;

                case "Or_Shop_D1":

                    EventCheck_OrD1();
                    break;
            }                        
        }
    }

    void EventCheck_Grt()
    {
        if (!GameMgr.ShopEvent_stage[0]) //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。チュートリアルするかどうか。
        {
            GameMgr.ShopEvent_stage[0] = true;
            GameMgr.scenario_ON = true;

            GameMgr.shop_event_num = 0;
            GameMgr.shop_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 0;
            GameMgr.CompoundEvent_flag = true;

            check_event = true;

            StartCoroutine("Scenario_loading");
        }


        if (check_event)
        { }
        else
        {
            if (GameMgr.Story_Mode == 0)
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {

                    case 2: //かわいい材料を探しに来た。

                        if (!GameMgr.ShopEvent_stage[6])
                        {
                            GameMgr.ShopEvent_stage[6] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 2;
                            GameMgr.shop_event_flag = true;

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    case 10: //ショップ二度目。ラスク作りの材料を買いにきた。

                        if (!GameMgr.ShopEvent_stage[1])
                        {
                            GameMgr.ShopEvent_stage[1] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 10;
                            GameMgr.shop_event_flag = true;

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    case 20: //クレープイベント

                        if (!GameMgr.ShopEvent_stage[2])
                        {
                            GameMgr.ShopEvent_stage[2] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 20;
                            GameMgr.shop_event_flag = true;

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    case 22: //アイスイベント

                        if (!GameMgr.ShopEvent_stage[7])
                        {
                            GameMgr.ShopEvent_stage[7] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 22;
                            GameMgr.shop_event_flag = true;

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    case 30: //シュークリームイベント

                        if (!GameMgr.ShopEvent_stage[3])
                        {
                            GameMgr.ShopEvent_stage[3] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 30;
                            GameMgr.shop_event_flag = true;

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    case 40: //ドーナツイベント開始。まずはプリンさんに聞きにくる。

                        if (!GameMgr.ShopEvent_stage[4])
                        {
                            GameMgr.ShopEvent_stage[4] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 40;
                            GameMgr.shop_event_flag = true;

                            //メイン画面にもどったときに、イベントを発生させるフラグをON
                            GameMgr.CompoundEvent_num = 20;
                            GameMgr.CompoundEvent_flag = true;

                            //村の広場にいけるようになる。
                            matplace_database.matPlaceKaikin("Hiroba");

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    case 50: //コンテストイベント

                        if (!GameMgr.ShopEvent_stage[5])
                        {
                            GameMgr.ShopEvent_stage[5] = true;
                            GameMgr.scenario_ON = true;

                            GameMgr.shop_event_num = 50;
                            GameMgr.shop_event_flag = true;

                            GameMgr.CompoundEvent_flag = false; //もし一度もショップへきたことなかった場合は、帰ってきてもヒカリが「なに買ってきたの？」と聞くイベントは発生しない。

                            check_event = true;

                            StartCoroutine("Scenario_loading");
                        }

                        break;

                    default:
                        break;
                }
            }
        }
    }

    void EventCheck_OrA1()
    {
        matplace_database.matPlaceKaikin("Or_Shop_A1"); //ショップ解禁

        if (!GameMgr.Or_ShopEvent_stage[0]) //はじめてお店へきた。
        {
            GameMgr.Or_ShopEvent_stage[0] = true;

            GameMgr.scenario_ON = true;

            GameMgr.shop_event_num = 1000;
            GameMgr.shop_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 0;
            GameMgr.CompoundEvent_flag = true;

            check_event = true;

            StartCoroutine("Scenario_loading");
            
        }

        if (check_event)
        { }
        else
        {

        }
    }

    void EventCheck_OrB1()
    {
        matplace_database.matPlaceKaikin("Or_Shop_B1"); //ショップ解禁

        if (!GameMgr.Or_ShopEvent_stage[0]) //はじめてお店へきた。
        {
            GameMgr.Or_ShopEvent_stage[0] = true;

            GameMgr.scenario_ON = true;

            GameMgr.shop_event_num = 0;
            GameMgr.shop_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 0;
            GameMgr.CompoundEvent_flag = true;

            check_event = true;

            StartCoroutine("Scenario_loading");

            
        }
    }

    void EventCheck_OrC1()
    {
        matplace_database.matPlaceKaikin("Or_Shop_C1"); //ショップ解禁

        if (!GameMgr.Or_ShopEvent_stage[0]) //はじめてお店へきた。
        {
            GameMgr.Or_ShopEvent_stage[0] = true;

            GameMgr.scenario_ON = true;

            GameMgr.shop_event_num = 0;
            GameMgr.shop_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 0;
            GameMgr.CompoundEvent_flag = true;

            check_event = true;

            StartCoroutine("Scenario_loading");

            
        }
    }

    void EventCheck_OrD1()
    {
        matplace_database.matPlaceKaikin("Or_Shop_D1"); //ショップ解禁

        if (!GameMgr.Or_ShopEvent_stage[0]) //はじめてお店へきた。
        {
            GameMgr.Or_ShopEvent_stage[0] = true;

            GameMgr.scenario_ON = true;

            GameMgr.shop_event_num = 0;
            GameMgr.shop_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 0;
            GameMgr.CompoundEvent_flag = true;

            check_event = true;

            StartCoroutine("Scenario_loading");

            
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

            _text.text = "何を買うの？";

        }
    }

    public void OnCheck_2() //話す
    {
        if (shopon_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 2; //眺めるを押したときのフラグ
            GameMgr.Scene_Select = 2;

            //_text.text = "なぁに？お話する？";

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100;
            GameMgr.utage_charaHyouji_flag = true;

            StartCoroutine("UtageEndWait");
        }
    }

    public void OnCheck_3() //依頼
    {
        if (shopon_toggle_quest.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_quest.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            backshopfirst_obj.SetActive(true);
            shop_select.SetActive(false);
            placename_panel.SetActive(false);
            //money_status_obj.SetActive(false);

            GameMgr.Scene_Status = 3; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 3;

            _text.text = "いまは、こんな依頼があるわよ。どれをうける？";

            //カメラ寄る。
            trans++; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

        }
    }

    /*
    public void OnCheck_4() //うわさ話　一回100Gとかで、ランダムで有用な情報をきける。
    {
        if (shopon_toggle_uwasa.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_uwasa.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 4; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 4;

            //_text.text = "これはうわさ話なんだけど..聞く？　一回100Gいただくわ。";

            //カメラ寄る。
            trans = 10; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.uwasa_flag = true;

            //一度読んだうわさ話は出ない。
            InitUwasaList();

            StartCoroutine("UtageEndWait");
        }
    }*/

    public void OnCheck_5() //ショップ　アイテムを売る
    {
        if (shopon_toggle_sell.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_sell.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            //backshopfirst_obj.SetActive(true);
            shop_select.SetActive(false);
            placename_panel.SetActive(false);

            GameMgr.Scene_Status = 5; //ショップのシーンに入っている、というフラグ
            GameMgr.Scene_Select = 5;

            playeritemlist_onoff.SetActive(true); //プレイヤーアイテムリスト画面を表示。

            _text.text = "フルーツや材料は、買い取りをしてるわよ。" + "\n" + "何を売るの？";

        }
    }

    public void OnCheck_6() //ショップ　アイテムをあげる
    {
        if (shopon_toggle_present.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_present.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            if (GameMgr.GirlLoveEvent_num == 11)
            {
                GameMgr.talk_number = 500;
            }
            else //11以降　いつでもお茶を渡せる。高得点でレコード取得。
            {
                GameMgr.talk_number = 501;
            }
            GameMgr.Scene_Status = 6; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 6;

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;

            GameMgr.utage_charaHyouji_flag = true;

            //アイテムを使用するときのフラグ
            GameMgr.event_pitem_use_select = true;
            GameMgr.shop_event_ON = true;

            //下は、使うときだけtrueにすればOK
            GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
            GameMgr.KoyuJudge_num = GameMgr.Shop_Okashi_num01;//GirlLikeSetの番号を直接指定
            GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする

            StartCoroutine("UtageEndWait");

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
                case "Or_Shop_A1": //春エリア

                    GameMgr.SceneSelectNum = 11;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_B1": //夏エリア

                    GameMgr.SceneSelectNum = 103;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_C1": //秋エリア

                    GameMgr.SceneSelectNum = 202;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Shop_D1": //冬エリア

                    GameMgr.SceneSelectNum = 303;
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

    public void OnCheck_Compound() //ショップで調合できるかお試し
    {
        if (shopon_toggle_compound.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_compound.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 500; //
            GameMgr.Scene_Select = 500;

            GameMgr.compound_status = 6;

            GameMgr.CompoundSceneStartON = true; //調合シーンに入っています、というフラグ開始。処理をCompoundMainControllerオブジェに移す。
        }
    }


    //ショップの品数が増えるなど、パティシエレベルや好感度に応じたイベントの発生フラグをチェック
    void CheckShopLvEvent()
    {
        //品物追加　いくつかの器具解禁 ShopItemListController.csで、品の追加処理をかいている。

        /*
        //品物追加　かわいいトッピング追加
        if (GameMgr.GirlLoveEvent_num >= 2) //かわいいクッキーイベント開始
        {
            if (!GameMgr.ShopLVEvent_stage[3])
            {
                //Debug.Log("ショップレベルイベント１　開始");
                GameMgr.ShopLVEvent_stage[3] = true;
                GameMgr.scenario_ON = true;

                GameMgr.shop_lvevent_num = 1;
                GameMgr.shop_lvevent_flag = true;

                lvevent_loading = true;
                StartCoroutine("Scenario_loading");
            }
        }

        
        //品物追加　ラスク　パンナイフ追加
        if (GameMgr.GirlLoveEvent_num >= 10) //ラスクイベント開始
        {
            if (!GameMgr.ShopLVEvent_stage[0])
            {
                //Debug.Log("ショップレベルイベント１　開始");
                GameMgr.ShopLVEvent_stage[0] = true;
                GameMgr.scenario_ON = true;

                GameMgr.shop_lvevent_num = 1;
                GameMgr.shop_lvevent_flag = true;

                lvevent_loading = true;
                StartCoroutine("Scenario_loading");
            }
        }

        //品物追加　シュークリームイベント以降
        if (GameMgr.GirlLoveEvent_num >= 30 && !GameMgr.ShopLVEvent_stage[1])
        {
            GameMgr.ShopLVEvent_stage[1] = true;
            GameMgr.scenario_ON = true;

            GameMgr.shop_lvevent_num = 1;
            GameMgr.shop_lvevent_flag = true;

            lvevent_loading = true;
            StartCoroutine("Scenario_loading");
        }

        //品物追加　ドーナツイベント以降
        if (GameMgr.GirlLoveEvent_num >= 40 && !GameMgr.ShopLVEvent_stage[2])
        {
            GameMgr.ShopLVEvent_stage[2] = true;
            GameMgr.scenario_ON = true;

            GameMgr.shop_lvevent_num = 1;
            GameMgr.shop_lvevent_flag = true;

            lvevent_loading = true;
            StartCoroutine("Scenario_loading");
        }
        */
    }

    IEnumerator UtageEndWait()
    {
        GameMgr.Scene_Select = 1000; //シナリオイベント読み中の状態
        GameMgr.Scene_Status = 1000;

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
        check_lvevent = true;

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        //Debug.Log("シナリオ終了");
        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        check_event = false;
        check_lvevent = false;
        lvevent_loading = false;
        GameMgr.Scene_Status = 0;

    }

    //
    //ショップうわさ関係
    //
    /*
    void InitUwasaList()
    {
        shopuwasa_List.Clear();
        random_uwasa_select.Clear();
        uwasalist_count = 5;

        //クッキー作り開始　初期値
        for (i = 0; i < uwasalist_count; i++) //頭から５個ずつ
        {
            shopuwasa_List.Add(GameMgr.ShopUwasa_stage1[i]);
        }

        //

        //ランダムで噂を選ぶメソッド
        uwasa_randomselect();

        if (random_uwasa_select.Count > 0)
        {
        }
        else //もしきける話を全て聞いていた場合
        {
            //きける話を全てリセット
            for (i = 0; i < shopuwasa_List.Count; i++)
            {
                shopuwasa_List[i] = false; //すべてのうわさの聞いたフラグをリセット

            }

            random_uwasa_select.Clear();
            uwasa_randomselect();
            //GameMgr.uwasa_number = 9999; //うわさ話はすべて聞いたというフラグ
        }

        rnd = Random.Range(0, random_uwasa_select.Count);
        GameMgr.uwasa_number = random_uwasa_select[rnd];
    }

    void uwasa_randomselect()
    {
        for (i = 0; i < shopuwasa_List.Count; i++)
        {
            if (!shopuwasa_List[i]) //まだうわさをきいてないやつだけをランダムで選ばれるようにする。
            {
                random_uwasa_select.Add(i);
            }
        }
    }
    */

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

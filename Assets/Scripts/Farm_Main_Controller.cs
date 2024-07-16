using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Farm_Main_Controller : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private BGM sceneBGM;

    private ItemShopDataBase shop_database;
    private ItemMatPlaceDataBase matplace_database;

    private SoundController sc;
    private SceneInitSetting sceneinit_setting;

    private PlayerItemList pitemlist;

    private GameObject text_area;
    private Text _text;

    private GameObject shopitemlist_onoff;

    private Debug_Panel_Init debug_panel_init;
    private GameObject money_status_obj;

    private GameObject canvas;
    private GameObject farm_select;
    private GameObject placename_panel;
    private GameObject farm_toggle_buy;
    private GameObject farm_toggle_talk;
    private GameObject farm_toggle_present;
    private GameObject farm_toggle_back;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject backshopfirst_obj;

    private bool StartRead;
    private bool check_event;

    private int i;

    //public int farm_status;
    //public int farm_scene; //どのシーンを選択しているかを判別

    // Use this for initialization
    void Start () {
		
	}

    public void InitSetup()
    {
        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 40;

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        farm_select = canvas.transform.Find("Farm_Select").gameObject;
        farm_toggle_buy = farm_select.transform.Find("Viewport/Content/FarmOn_Toggle_Buy").gameObject;
        farm_toggle_talk = farm_select.transform.Find("Viewport/Content/FarmOn_Toggle_Talk").gameObject;
        farm_toggle_present = farm_select.transform.Find("Viewport/Content/FarmOn_Toggle_Present").gameObject;
        farm_toggle_present.SetActive(false);
        farm_toggle_back = farm_select.transform.Find("Viewport/Content/FarmOn_Toggle_Back").gameObject;
        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);

        //ファームでのショップリスト画面。初期設定で最初はOFF。
        shopitemlist_onoff = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shopitemlist_onoff.SetActive(false);

        //シーン最初にプレイヤーアイテムリストの生成
        sceneinit_setting = SceneInitSetting.Instance.GetComponent<SceneInitSetting>();
        sceneinit_setting.PlayerItemListController_Init();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        playeritemlist_onoff.SetActive(false); //

        //自分の持ってるお金などのステータス
        money_status_obj = GameObject.FindWithTag("MoneyStatus_panel");
        money_status_obj.SetActive(false);

        //場所の名前プレートの取得
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //初期メッセージ
        _text.text = "ここは牧場だ。";
        text_area.SetActive(false);

        StartRead = false;
        check_event = false;

        //移動時に調合シーンステータスを0に。
        GameMgr.compound_status = 0;
        GameMgr.compound_select = 0;

        GameMgr.Scene_Status = 0;
        GameMgr.Scene_Select = 0;

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

        if (GameMgr.Story_Mode == 1)
        {
            //あるクエスト以降、モタリケにお菓子わたせる。
            if (GameMgr.GirlLoveEvent_num >= 11)
            {
                farm_toggle_present.SetActive(true);
            }
        }

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateFarmScene()
    {
        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
            sceneBGM.NowFadeVolumeONBGM();
        }

        //イベント発生フラグをチェック
        EventCheck();



        if (GameMgr.Reset_SceneStatus)
        {
            GameMgr.Reset_SceneStatus = false;
            GameMgr.Scene_Status = 0;
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            shopitemlist_onoff.SetActive(false);
            backshopfirst_obj.SetActive(false);
            farm_select.SetActive(false);
            text_area.SetActive(false);
            money_status_obj.SetActive(false);
            placename_panel.SetActive(false);

            GameMgr.Scene_Status = 0;
            GameMgr.Scene_Select = 0;
        }
        else
        {
            //Debug.Log("shop_status" + shop_status);
            switch (GameMgr.Scene_Status)
            {
                case 0:

                    shopitemlist_onoff.SetActive(false);
                    backshopfirst_obj.SetActive(false);
                    farm_select.SetActive(true);
                    text_area.SetActive(true);
                    money_status_obj.SetActive(true);
                    placename_panel.SetActive(true);

                    GameMgr.Scene_Select = 0;
                    GameMgr.Scene_Status = 100;

                    if (trans == 1) //カメラが寄っていたら、デフォに戻す。
                    {
                        //カメラ寄る。
                        trans--; //transが1を超えたときに、ズームするように設定されている。

                        //intパラメーターの値を設定する.
                        maincam_animator.SetInteger("trans", trans);
                    }

                    _text.text = "ここは牧場だ。";

                    break;

                case 1: //ショップのアイテム選択中
                    break;

                case 2:
                    break;

                case 3:
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
        // 強制的に発生するイベントをチェック。はじめてショップへきた時など
        if (!check_event)
        {
            switch (GameMgr.Scene_Name)
            {
                case "Farm_Grt":

                    EventCheck_Grt();
                    break;

                case "Or_Farm":

                    EventCheck_OrA1();
                    break;
            }
        }
            
    }

    void EventCheck_Grt()
    {

    }

    void EventCheck_OrA1()
    {
        matplace_database.matPlaceKaikin("Or_Farm"); //牧場解禁

        if (!GameMgr.FarmEvent_stage[0]) //はじめて牧場をおとずれる。プリンさんからたまごの話をきいてから、フラグがたつ。
        {
            GameMgr.FarmEvent_stage[0] = true;
            GameMgr.scenario_ON = true;

            GameMgr.farm_event_num = 0;
            GameMgr.farm_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 10;
            GameMgr.CompoundEvent_flag = true;

            check_event = true;

            //たまご・牛乳を各５個ずつもらえる。
            pitemlist.addPlayerItemString("egg", 5);
            pitemlist.addPlayerItemString("milk", 5);
            pitemlist.add_eventPlayerItemString("whippedcream_recipi", 1);

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

    }

    public void OnCheck_1() //牧場のショップ　アイテムを買う
    {
        if (farm_toggle_buy.GetComponent<Toggle>().isOn == true)
        {
            farm_toggle_buy.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shopitemlist_onoff.SetActive(true); //ショップリスト画面を表示。
            backshopfirst_obj.SetActive(true);
            farm_select.SetActive(false);
            placename_panel.SetActive(false);

            GameMgr.Scene_Status = 1; //ショップのシーンに入っている、というフラグ
            GameMgr.Scene_Select = 1;

            _text.text = "ぎょ～さん買っていきぃ！";

        }
    }

    public void OnCheck_2() //話す
    {
        if (farm_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            farm_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 2; //話すを押したときのフラグ
            GameMgr.Scene_Select = 2;

            //_text.text = "（..今はしゃべる気がないようだ。）";


            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100;

            StartCoroutine("UtageEndWait");
        }
    }

    public void OnCheck_3() //アイテムをあげる
    {
        if (farm_toggle_present.GetComponent<Toggle>().isOn == true)
        {
            farm_toggle_present.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 3; //クエストを押したときのフラグ
            GameMgr.Scene_Select = 3;

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 500;
            GameMgr.utage_charaHyouji_flag = true;

            //アイテムを使用するときのフラグ
            GameMgr.event_pitem_use_select = true;
            GameMgr.farm_event_ON = true;

            //下は、使うときだけtrueにすればOK
            GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
            GameMgr.KoyuJudge_num = GameMgr.Farm_Okashi_num01;//GirlLikeSetの番号を直接指定
            GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする

            StartCoroutine("UtageEndWait");

        }
    }

    public void OnCheck_Back() //ショップからでて、広場に戻る
    {
        if (farm_toggle_back.GetComponent<Toggle>().isOn == true)
        {
            farm_toggle_back.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。


            switch (GameMgr.Scene_Name)
            {
                case "Or_Farm_A1": //春エリア

                    GameMgr.SceneSelectNum = 11;
                    FadeManager.Instance.LoadScene("Or_Hiroba1", GameMgr.SceneFadeTime);
                    break;

                case "Or_Farm_B1": //夏エリア

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

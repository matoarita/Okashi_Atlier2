using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop_Main : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private ItemShopDataBase shop_database;
    private ItemMatPlaceDataBase matplace_database;

    private SoundController sc;
    private Girl1_status girl1_status;

    private GameObject text_area;
    private Text _text;
    private string shopdefault_text;

    private Debug_Panel_Init debug_panel_init;

    private GameObject placename_panel;

    private GameObject shopitemlist_onoff;
    private GameObject shopquestlist_obj;

    private GameObject money_status_obj;

    public GameObject hukidasi_sub;
    private GameObject hukidasi_sub_Prefab;

    private GameObject character;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject backbutton_obj;

    private GameObject black_effect;

    private GameObject canvas;
    private GameObject shop_select;
    private GameObject shopon_toggle_buy;
    private GameObject shopon_toggle_talk;
    private GameObject shopon_toggle_quest;

    private bool check_lvevent;
    private bool lvevent_loading;

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    public int shop_status;
    public int shop_scene; //どのシーンを選択しているかを判別

    private bool hukidasi_oneshot; //吹き出しの作成は一つのみ

    private int i;

    // Use this for initialization
    void Start () {

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

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

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

        character = GameObject.FindWithTag("Character");
        character.GetComponent<FadeCharacter>().SetOff();

        hukidasi_oneshot = false;

        //シーン読み込みのたびに、ショップの在庫をMaxにしておく。イベントアイテムは補充しない。
        for ( i= 0; i < shop_database.shopitems.Count; i++)
        {
            if (shop_database.shopitems[i].shop_itemType == 0)
            {
                shop_database.shopitems[i].shop_itemzaiko = 50;
            }
            else
            {

            }
        }

        shop_select = canvas.transform.Find("Shop_Select").gameObject;
        shopon_toggle_buy = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Buy").gameObject;
        shopon_toggle_talk = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Talk").gameObject;
        shopon_toggle_quest = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Quest").gameObject;
        //backbutton_obj = shop_select.transform.Find("Viewport/Content/Button_modoru").gameObject;

        //自分の持ってるお金などのステータス
        money_status_obj = GameObject.FindWithTag("MoneyStatus_panel");
        money_status_obj.SetActive(false);

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //プレイヤー所持アイテムリストパネルの初期化・取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        playeritemlist_onoff.SetActive(false); //ショップ画面では使わないのでOFF

        //ショップリスト画面。初期設定で最初はOFF。
        shopitemlist_onoff = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shopitemlist_onoff.SetActive(false);

        //クエストリスト画面。初期設定で最初はOFF。
        shopquestlist_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlist_obj.SetActive(false);

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //初期メッセージ
        shopdefault_text = "いらっしゃい～。";
        _text.text = shopdefault_text;
        text_area.SetActive(false);

        shop_status = 0;
        shop_scene = 0;

        check_lvevent = false;
        lvevent_loading = false;

        //入店の音
        sc.PlaySe(51);
    }
	
	// Update is called once per frame
	void Update () {

        //イベント発生フラグをチェック
        switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
        {

            case 0: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。チュートリアルするかどうか。

                if (!GameMgr.ShopEvent_stage[0])
                {
                    GameMgr.ShopEvent_stage[0] = true;
                    GameMgr.scenario_ON = true;

                    GameMgr.shop_event_num = 0;
                    GameMgr.shop_event_flag = true;

                    //メイン画面にもどったときに、イベントを発生させるフラグをON
                    GameMgr.CompoundEvent_num = 0;
                    GameMgr.CompoundEvent_flag = true;

                    StartCoroutine("Scenario_loading");
                }

                break;

            case 1: //ショップ二度目。ラスク作りの材料を買いにきた。

                if (!GameMgr.ShopEvent_stage[1])
                {
                    GameMgr.ShopEvent_stage[1] = true;
                    GameMgr.scenario_ON = true;

                    GameMgr.shop_event_num = 10;
                    GameMgr.shop_event_flag = true;

                    StartCoroutine("Scenario_loading");
                }

                break;

            case 2: //クレープイベント

                if (!GameMgr.ShopEvent_stage[2])
                {
                    GameMgr.ShopEvent_stage[2] = true;
                    GameMgr.scenario_ON = true;

                    GameMgr.shop_event_num = 20;
                    GameMgr.shop_event_flag = true;

                    StartCoroutine("Scenario_loading");
                }

                break;

            case 3: //シュークリームイベント

                if (!GameMgr.ShopEvent_stage[3])
                {
                    GameMgr.ShopEvent_stage[3] = true;
                    GameMgr.scenario_ON = true;

                    GameMgr.shop_event_num = 30;
                    GameMgr.shop_event_flag = true;

                    StartCoroutine("Scenario_loading");
                }

                break;

            case 4: //ドーナツイベント開始。まずはプリンさんに聞きにくる。

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

                    StartCoroutine("Scenario_loading");
                }

                break;


            default:
                break;
        }
              

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            shopitemlist_onoff.SetActive(false);
            shopquestlist_obj.SetActive(false);
            shop_select.SetActive(false);
            text_area.SetActive(false);
            money_status_obj.SetActive(false);
            placename_panel.SetActive(false);
            black_effect.SetActive(false);

            shop_status = 0;
            shop_scene = 0;
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
                    check_lvevent = true;
                }
            }
            else
            {
                //Debug.Log("shop_status" + shop_status);
                switch (shop_status)
                {
                    case 0:

                        character.GetComponent<FadeCharacter>().SetOn();
                        shopitemlist_onoff.SetActive(false);
                        shopquestlist_obj.SetActive(false);
                        shop_select.SetActive(true);
                        text_area.SetActive(true);
                        money_status_obj.SetActive(true);
                        placename_panel.SetActive(true);
                        black_effect.SetActive(false);

                        //_text.text = shopdefault_text;

                        shop_scene = 0;
                        shop_status = 100;

                        if (trans == 1) //カメラが寄っていたら、デフォに戻す。
                        {
                            //カメラ寄る。
                            trans--; //transが1を超えたときに、ズームするように設定されている。

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

                    case 100: //退避
                        break;

                    default:
                        break;


                }
            }
        }

	}

    public void OnCheck_1() //ショップ　アイテムを買う
    {
        if (shopon_toggle_buy.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_buy.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shopitemlist_onoff.SetActive(true); //ショップリスト画面を表示。
            shop_select.SetActive(false);
            placename_panel.SetActive(false);

            shop_status = 1; //ショップのシーンに入っている、というフラグ
            shop_scene = 1;

            _text.text = "何を買うの？";
            
        }
    }

    public void OnCheck_2() //話す
    {
        if (shopon_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shop_status = 2; //眺めるを押したときのフラグ
            shop_scene = 2;

            //_text.text = "なぁに？お話する？";

            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100;

        }
    }

    public void OnCheck_3() //依頼
    {
        if (shopon_toggle_quest.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_quest.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shopquestlist_obj.SetActive(true); //ショップリスト画面を表示。
            shop_select.SetActive(false);
            placename_panel.SetActive(false);
            //money_status_obj.SetActive(false);

            shop_status = 3; //クエストを押したときのフラグ
            shop_scene = 3;

            _text.text = "ありがとう！お菓子をぜひ買い取らせていただくわ。";

            //カメラ寄る。
            trans++; //transが1を超えたときに、ズームするように設定されている。

            //intパラメーターの値を設定する.
            maincam_animator.SetInteger("trans", trans);

        }
    }

    //ショップの品数が増えるなど、パティシエレベルや好感度に応じたイベントの発生フラグをチェック
    void CheckShopLvEvent()
    {
        if (girl1_status.girl1_Love_lv >= 2 || GameMgr.GirlLoveEvent_num >= 1) //好感度レベル２以上 or ラスクイベント開始
        {
            if (!GameMgr.ShopLVEvent_stage[0])
            {
                GameMgr.ShopLVEvent_stage[0] = true;
                GameMgr.scenario_ON = true;

                GameMgr.shop_lvevent_num = 0;
                GameMgr.shop_lvevent_flag = true;

                lvevent_loading = true;
                StartCoroutine("Scenario_loading");
            }
        }

        if (girl1_status.girl1_Love_lv >= 4 && !GameMgr.ShopLVEvent_stage[1]) //好感度レベル４以上
        {
            GameMgr.ShopLVEvent_stage[1] = true;
            GameMgr.scenario_ON = true;

            GameMgr.shop_lvevent_num = 1;
            GameMgr.shop_lvevent_flag = true;

            lvevent_loading = true;
            StartCoroutine("Scenario_loading");
        }
    }

    IEnumerator Scenario_loading()
    {

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;

        if (lvevent_loading)
        {
            check_lvevent = true;
            lvevent_loading = false;
        }
    }
}

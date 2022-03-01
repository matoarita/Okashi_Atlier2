using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Farm_Main : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private ItemShopDataBase shop_database;

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

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject backshopfirst_obj;

    public int farm_status;
    public int farm_scene; //どのシーンを選択しているかを判別

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

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        farm_select = canvas.transform.Find("Farm_Select").gameObject;
        farm_toggle_buy = farm_select.transform.Find("Viewport/Content/FarmOn_Toggle_Buy").gameObject;
        farm_toggle_talk = farm_select.transform.Find("Viewport/Content/FarmOn_Toggle_Talk").gameObject;
        backshopfirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        backshopfirst_obj.SetActive(false);

        //ファームでのショップリスト画面。初期設定で最初はOFF。
        shopitemlist_onoff = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
        shopitemlist_onoff.SetActive(false);

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

        farm_status = 0;
        farm_scene = 0;

        //シーン読み込みのたびに、ショップの在庫をMaxにしておく。イベントアイテムは補充しない。
        for (i = 0; i < shop_database.farmitems.Count; i++)
        {
            if (shop_database.farmitems[i].shop_itemType == 0 || shop_database.farmitems[i].shop_itemType == 3)
            {
                shop_database.farmitems[i].shop_itemzaiko = shop_database.farmitems[i].shop_itemzaiko_max;
            }
            else
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //イベント発生フラグをチェック

        if (!GameMgr.FarmEvent_stage[0]) //はじめて牧場をおとずれる。プリンさんからたまごの話をきいてから、フラグがたつ。
        {
            GameMgr.FarmEvent_stage[0] = true;
            GameMgr.scenario_ON = true;

            GameMgr.farm_event_num = 0;
            GameMgr.farm_event_flag = true;

            //メイン画面にもどったときに、イベントを発生させるフラグをON
            GameMgr.CompoundEvent_num = 10;
            GameMgr.CompoundEvent_flag = true;

            //たまご・牛乳を各５個ずつもらえる。
            pitemlist.addPlayerItemString("egg", 5);
            pitemlist.addPlayerItemString("milk", 5);
            pitemlist.add_eventPlayerItemString("whippedcream_recipi", 1);
        }

        switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
        {
            default:

                break;
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

            farm_status = 0;
            farm_scene = 0;
        }
        else
        {
            //Debug.Log("shop_status" + shop_status);
            switch (farm_status)
            {
                case 0:

                    shopitemlist_onoff.SetActive(false);
                    backshopfirst_obj.SetActive(false);
                    farm_select.SetActive(true);
                    text_area.SetActive(true);
                    money_status_obj.SetActive(true);
                    placename_panel.SetActive(true);

                    farm_scene = 0;
                    farm_status = 100;

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

                case 100: //退避
                    break;

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

            farm_status = 1; //ショップのシーンに入っている、というフラグ
            farm_scene = 1;

            _text.text = "ぎょ～さん買っていきぃ！";

        }
    }

    public void OnCheck_2() //話す
    {
        if (farm_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            farm_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            //farm_status = 2; //話すを押したときのフラグ
            //farm_scene = 2;

            _text.text = "（..今はしゃべる気がないようだ。）";

            /*
            GameMgr.scenario_ON = true; //これがONのときは、シナリオを優先する。
            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100;*/

        }
    }
}

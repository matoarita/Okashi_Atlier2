﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject shopitemlist_onoff;

    private GameObject money_status_obj;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject backbutton_obj;

    private GameObject canvas;
    private GameObject shop_select;
    private GameObject shopon_toggle_buy;
    private GameObject shopon_toggle_talk;
    private GameObject shopon_toggle_watch;

    public int shop_status;

    // Use this for initialization
    void Start () {

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        shop_select = canvas.transform.Find("Shop_Select").gameObject;
        shopon_toggle_buy = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Buy").gameObject;
        shopon_toggle_talk = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Talk").gameObject;
        shopon_toggle_watch = shop_select.transform.Find("Viewport/Content/ShopOn_Toggle_Watch").gameObject;

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;
        backbutton_obj.SetActive(false);

        //自分の持ってるお金などのステータス
        money_status_obj = GameObject.FindWithTag("MoneyStatus_panel");
        money_status_obj.SetActive(false);

        //プレイヤー所持アイテムリストパネルの初期化・取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        playeritemlist_onoff.SetActive(false); //ショップ画面では使わないのでOFF

        //ショップリスト画面を開く。初期設定で最初はOFF。
        shopitemlist_onoff = GameObject.FindWithTag("ShopitemList_ScrollView");
        shopitemlist_onoff.SetActive(false);

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //初期メッセージ
        _text.text = "いらっしゃい～。";
        text_area.SetActive(false);

        shop_status = 0;
    }
	
	// Update is called once per frame
	void Update () {

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            shopitemlist_onoff.SetActive(false);
            shop_select.SetActive(false);
            backbutton_obj.SetActive(false);
            text_area.SetActive(false);
            money_status_obj.SetActive(false);

            shop_status = 0;
        }
        else
        {
            //Debug.Log("shop_status" + shop_status);
            switch (shop_status)
            {
                case 0:

                    shopitemlist_onoff.SetActive(false);
                    shop_select.SetActive(true);
                    backbutton_obj.SetActive(true);
                    text_area.SetActive(true);
                    money_status_obj.SetActive(true);

                    _text.text = "いらっしゃい～。";

                    break;

                case 1:
                    break;

                case 2:
                    break;

                default:
                    break;

                
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

            shop_status = 1; //ショップのシーンに入っている、というフラグ

            _text.text = "何を買うの？";
            
        }
    }

    public void OnCheck_2() //話す
    {
        if (shopon_toggle_talk.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_talk.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            shop_status = 2; //眺めるを押したときのフラグ

            //_text.text = "なぁに？お話する？";

            GameMgr.talk_flag = true;
            GameMgr.talk_number = 100; //ショップ関係は、100番台

        }
    }

    public void OnCheck_3() //眺める
    {
        if (shopon_toggle_watch.GetComponent<Toggle>().isOn == true)
        {
            shopon_toggle_watch.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            //shop_select.SetActive(false);

            shop_status = 3; //眺めるを押したときのフラグ

            _text.text = "なぁに？お話する？";

        }
    }
}

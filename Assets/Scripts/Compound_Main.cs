using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Compound_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject card_view_obj;
    private CardView card_view;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject compoundselect_onoff_obj;
    private GameObject recipi_toggle;
    private GameObject topping_toggle;
    private GameObject original_toggle;
    private GameObject roast_toggle;
    private GameObject blend_toggle;

    private GameObject backbutton_obj;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private int i;
    private int recipi01_ID;

    public int compound_status;
    public int compound_select;

    public int event_itemID; //イベントレシピ使用時のイベントのID


    // Use this for initialization
    void Start () {

        //Debug.Log("Compound scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;
        backbutton_obj.SetActive(false);

        //プレイヤー所持アイテムリストパネルの取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        //レシピ画面の初期設定
        recipilist_onoff = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();

        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes = playeritemlist_onoff.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = playeritemlist_onoff.transform.Find("No").gameObject;
        no_text = no.GetComponentInChildren<Text>();
               

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        compoundselect_onoff_obj = GameObject.FindWithTag("CompoundSelect");

        recipi_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Recipi_Toggle").gameObject;
        topping_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Topping_Toggle").gameObject;
        original_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Original_Toggle").gameObject;
        roast_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Roast_Toggle").gameObject;
        //blend_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Blend_Toggle").gameObject;

        //初期メッセージ
        _text.text = "何の調合をする？";
        text_area.SetActive(false);

        compound_status = 0;
        compound_select = 0;


        //レシピ調合用の、フラグ管理部分
        //Check_RecipiFlag();
        
    }
	
	// Update is called once per frame
	void Update () {

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            compoundselect_onoff_obj.SetActive(false);
            backbutton_obj.SetActive(false);
            text_area.SetActive(false);
        }
        else
        {

            //メインの調合処理　各ボタンを押すと、中の処理が動き始める。
            switch (compound_status)
            {
                case 0:

                    recipilist_onoff.SetActive(false);
                    playeritemlist_onoff.SetActive(false);
                    compoundselect_onoff_obj.SetActive(true);
                    backbutton_obj.SetActive(true);
                    text_area.SetActive(true);

                    text_scenario();
                    break;

                case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                    compoundselect_onoff_obj.SetActive(false);
                    compound_status = 4; //調合シーンに入っています、というフラグ
                    compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択
                    recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                    no.SetActive(true);

                    break;

                case 2: //トッピング調合の処理を開始。クリック後に処理が始まる。

                    compoundselect_onoff_obj.SetActive(false);
                    compound_status = 4; //調合シーンに入っています、というフラグ
                    compound_select = 2; //トッピング調合を選択
                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                    no.SetActive(true);

                    break;

                case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                    compoundselect_onoff_obj.SetActive(false);
                    compound_status = 4; //調合シーンに入っています、というフラグ
                    compound_select = 3; //オリジナル調合を選択
                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 
                    no.SetActive(true);

                    break;

                case 4: //調合シーンに入ってますよ、というフラグ。各ケース処理後、必ずこの中の処理に移行する。yes, noボタンを押されるまでは、待つ状態に入る。

                    recipi_toggle.GetComponent<Toggle>().isOn = false;
                    original_toggle.GetComponent<Toggle>().isOn = false;
                    topping_toggle.GetComponent<Toggle>().isOn = false;
                    roast_toggle.GetComponent<Toggle>().isOn = false;
                    //blend_toggle.GetComponent<Toggle>().isOn = false;

                    break;

                case 5: //「焼く」を選択

                    compoundselect_onoff_obj.SetActive(false);
                    compound_status = 4; //調合シーンに入っています、というフラグ
                    compound_select = 5; //焼くを選択
                    playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                    pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                    no.SetActive(true);

                    break;


                case 100: //調合中　退避用
                    break;


                /*case 5: //ブレンド調合の処理（未使用）

                    compoundselect_onoff_obj.SetActive(false);
                    compound_status = 4; //調合シーンに入っています、というフラグ
                    compound_select = 5; //ブレンド調合を選択
                    recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                    no.SetActive(true);

                    break;*/



                default:
                    break;
            }

            if(GameMgr.event_recipi_endflag == true)
            {
                compoundselect_onoff_obj.SetActive(true);
                backbutton_obj.SetActive(true);
                text_area.SetActive(true);

                text_scenario();

                GameMgr.event_recipi_endflag = false;
            }
        }

    }

    public void OnCheck_1() //レシピ調合をON
    {
        if (recipi_toggle.GetComponent<Toggle>().isOn == true)
        {
            yes_no_load();

            //Debug.Log("check1");
            _text.text = "レシピから、お菓子を作るよ。調合したいアイテムを選択してください。";
            compound_status = 1;
        }
    }

    public void OnCheck_2() //トッピング調合をON
    {
        if (topping_toggle.GetComponent<Toggle>().isOn == true)
        {
            yes_no_load();

            //Debug.Log("check2");
            _text.text = "すでに作ったアイテムにトッピングをして、味の調節をしよう。";
            compound_status = 2;
        }
    }

    public void OnCheck_3() //オリジナル調合をON
    {
        if (original_toggle.GetComponent<Toggle>().isOn == true)
        {
            yes_no_load();

            //Debug.Log("check3");
            _text.text = "オリジナル調合をするよ！ 一つ目のアイテムを選択してください。";
            compound_status = 3;
        }
    }

    /*public void OnCheck_4() //ブレンド調合をON
    {
        if (blend_toggle.GetComponent<Toggle>().isOn == true)
        {

            //Debug.Log("check4");
            _text.text = "ブレンド調合をします。まずはレシピを選ぶ。";
            compound_status = 5;
        }
    }*/

    public void OnCheck_5() //"焼き"をON
    {
        if (roast_toggle.GetComponent<Toggle>().isOn == true)
        {
            yes_no_load();

            //Debug.Log("check3");
            _text.text = "作った生地を焼きます。焼きたい生地を選んでください。";
            compound_status = 5;
        }
    }


    //イベント用レシピを見たときの処理。
    public void eventRecipi_ON()
    {
        recipilist_onoff.SetActive(false);
        Debug.Log("イベントレシピID: " + event_itemID + "　レシピ名: " + pitemlist.eventitemlist[event_itemID].event_itemNameHyouji);

        compoundselect_onoff_obj.SetActive(false);
        backbutton_obj.SetActive(false);
        text_area.SetActive(false);

        GameMgr.event_recipiID = event_itemID;
        GameMgr.event_recipi_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
    }


    void yes_no_load()
    {
        
        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF

        if (recipi_toggle.GetComponent<Toggle>().isOn == true) //レシピ調合の場合、名前が少し違う
        {
            yes = recipilist_onoff.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = recipilist_onoff.transform.Find("No").gameObject;
            no_text = no.GetComponentInChildren<Text>();
        }
        else
        {
            yes = playeritemlist_onoff.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = playeritemlist_onoff.transform.Find("No").gameObject;
            no_text = no.GetComponentInChildren<Text>();
        }

        backbutton_obj.SetActive(false);
    }


    void text_scenario()
    {
        switch (GameMgr.scenario_flag)
        {
            case 115:
                _text.text = "何の調合をする？" + "\n" + "(さっきのレシピを見てみるか。)";

                /*original_toggle.GetComponent<Toggle>().interactable = false;
                topping_toggle.GetComponent<Toggle>().interactable = false;
                roast_toggle.GetComponent<Toggle>().interactable = false;*/
                break;

            case 120:
                _text.text = "何の調合をする？" + "\n" + "(まずは、お菓子の材料を買わないとな..。)";

                break;

            default:
                _text.text = "何の調合をする？";
                break;
        }
    }



    void Check_RecipiFlag()
    {
        i = 0;
        //レシピを名前で検索し、アイテムIDを入れる。
        while (i < database.items.Count)
        {
            if (database.items[i].itemName == "recipi_01")
            {
                recipi01_ID = i;
                //Debug.Log("recipi01_ID: " + recipi01_ID);
                break;
            }
            ++i;
        }

        if (pitemlist.playeritemlist[recipi01_ID] >= 1) //レシピ01の本を持っているとき
        {
            //レシピ調合可能なアイテムを、調合アイテムデータベースのフラグをONにする。
            databaseCompo.compoitems[5].cmpitem_flag = 1; //コンポDBのItemID　5番をONにする。
            Debug.Log("recipi01の本を持っている。");
        }
    }
}

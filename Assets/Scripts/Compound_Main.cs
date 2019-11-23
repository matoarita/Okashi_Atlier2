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
    private GameObject saveload_panel;
    private GameObject recipi_toggle;
    private GameObject topping_toggle;
    private GameObject original_toggle;
    private GameObject roast_toggle;
    private GameObject blend_toggle;

    private GameObject menu_toggle;

    private GameObject backbutton_obj;

    private bool ReadRecipi_ALLOK;
    private bool Recipi_loading;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private int i, j;
    private int comp_ID;

    public int compound_status;
    public int compound_select;

    public int event_itemID; //イベントレシピ使用時のイベントのID


    // Use this for initialization
    void Start() {

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
        playeritemlist_onoff.SetActive(false);

        //レシピ画面の初期設定
        recipilist_onoff = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();
        recipilist_onoff.SetActive(false);


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
        saveload_panel = canvas.transform.Find("SaveLoadPanel").gameObject;

        recipi_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Recipi_Toggle").gameObject;
        topping_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Topping_Toggle").gameObject;
        original_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Original_Toggle").gameObject;
        roast_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Roast_Toggle").gameObject;
        //blend_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Blend_Toggle").gameObject;

        menu_toggle = saveload_panel.transform.Find("Viewport/Content_compound/Menu_Toggle").gameObject;

        //初期メッセージ
        _text.text = "何の調合をする？";
        text_area.SetActive(false);

        compound_status = 0;
        compound_select = 0;

        ReadRecipi_ALLOK = false;
        Recipi_loading = false;

    }

    // Update is called once per frame
    void Update() {

        //読んでいないレシピがあれば、読む処理。優先順位が一番先。
        if (ReadRecipi_ALLOK == false)
        {
            Check_RecipiFlag();
        }

        else //以下が、通常の処理
        {
            //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
            if (GameMgr.scenario_ON == true)
            {
                compoundselect_onoff_obj.SetActive(false);
                saveload_panel.SetActive(false);
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
                        saveload_panel.SetActive(true);
                        backbutton_obj.SetActive(true);
                        text_area.SetActive(true);

                        text_scenario();
                        break;

                    case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                        compoundselect_onoff_obj.SetActive(false);
                        saveload_panel.SetActive(false);
                        compound_status = 4; //調合シーンに入っています、というフラグ
                        compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択
                        recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                        no.SetActive(true);

                        break;

                    case 2: //トッピング調合の処理を開始。クリック後に処理が始まる。

                        compoundselect_onoff_obj.SetActive(false);
                        saveload_panel.SetActive(false);
                        compound_status = 4; //調合シーンに入っています、というフラグ
                        compound_select = 2; //トッピング調合を選択
                        playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                        pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                        no.SetActive(true);

                        break;

                    case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                        compoundselect_onoff_obj.SetActive(false);
                        saveload_panel.SetActive(false);
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
                        saveload_panel.SetActive(false);
                        compound_status = 4; //調合シーンに入っています、というフラグ
                        compound_select = 5; //焼くを選択
                        playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                        pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                        no.SetActive(true);

                        break;

                    case 99:

                        compoundselect_onoff_obj.SetActive(false);
                        saveload_panel.SetActive(false);
                        compound_status = 100;
                        compound_select = 99;
                        playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
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

                if (GameMgr.recipi_read_endflag == true)
                {
                    compoundselect_onoff_obj.SetActive(true);
                    saveload_panel.SetActive(true);
                    backbutton_obj.SetActive(true);
                    text_area.SetActive(true);

                    text_scenario();

                    GameMgr.recipi_read_endflag = false;
                }

                if (GameMgr.event_recipi_endflag == true)
                {
                    compoundselect_onoff_obj.SetActive(true);
                    saveload_panel.SetActive(true);
                    backbutton_obj.SetActive(true);
                    text_area.SetActive(true);

                    text_scenario();

                    GameMgr.event_recipi_endflag = false;
                }


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

    public void OnMenu_toggle() //メニューをON
    {
        if (menu_toggle.GetComponent<Toggle>().isOn == true)
        {
            menu_toggle.GetComponent<Toggle>().isOn = false;
            yes_no_load();

            //Debug.Log("check1");
            compound_status = 99;
        }
    }

    //イベント用レシピを見たときの処理。
    public void eventRecipi_ON()
    {
        recipilist_onoff.SetActive(false);
        Debug.Log("イベントレシピID: " + event_itemID + "　レシピ名: " + pitemlist.eventitemlist[event_itemID].event_itemNameHyouji);

        compoundselect_onoff_obj.SetActive(false);
        saveload_panel.SetActive(false);
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
        if (Recipi_loading == true)
        {
            //レシピを読み込み中のときは、所持チェックはしない。
        }
        else //レシピを読み込み中でない。
        {
            //所持しているが、まだ読んでいないレシピがないか、チェックする。

            i = 0;

            while (i < pitemlist.eventitemlist.Count)
            {
                ReadRecipi_ALLOK = true;

                //もし、所持はしているのに、リードフラグは０のまま（＝読んでいないもの）がある場合、レシピを読む処理に入る。
                if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ReadFlag == 0)
                {

                    ReadRecipi_ALLOK = false;

                    /* レシピを読む処理 */
                    Recipi_loading = true; //レシピを読み込み中ですよ～のフラグ
                    pitemlist.eventitemlist[i].ev_ReadFlag = 1; //該当のイベントアイテムのレシピのフラグをONにしておく（読んだ、という意味）
                    Recipi_FlagON_Method();
                    Debug.Log("レシピ: " + pitemlist.eventitemlist[i].event_itemNameHyouji + "を読んだ");

                    break;
                }
                ++i;
            }

            if (Recipi_loading == true)
            {
                StartCoroutine(Recipi_Read_Method());
            }
        }
    }

    IEnumerator Recipi_Read_Method() {

        compoundselect_onoff_obj.SetActive(false);
        saveload_panel.SetActive(false);
        backbutton_obj.SetActive(false);
        text_area.SetActive(false);
        GameMgr.recipi_read_ID = pitemlist.eventitemlist[i].ev_ItemID;
        GameMgr.recipi_read_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        Recipi_loading = false;
    }

    void Recipi_FlagON_Method()
    {
        //レシピの番号チェック　
        switch(pitemlist.eventitemlist[i].ev_ItemID)
        {
            case 2:

                Find_compoitemdatabase("cookie_base");
                databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;

                Find_compoitemdatabase("financier_base");
                databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;

                Find_compoitemdatabase("appaleil");
                databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;
                break;

            default:
                break;
        }
    }

    //アイテム名を入力すると、該当するcompoIDを返す処理
    void Find_compoitemdatabase(string compo_itemname)
    {
        j = 0;
        while ( j < databaseCompo.compoitems.Count )
        {
            if (compo_itemname == databaseCompo.compoitems[j].cmpitem_Name)
            {
                comp_ID = j;
                break;
            }
            j++;
        }
    }
}

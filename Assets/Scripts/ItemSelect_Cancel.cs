using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemSelect_Cancel : SingletonMonoBehaviour<ItemSelect_Cancel>
{

    private GameObject text_area; //Sceneテキスト表示エリアのこと。
    private Text _text; //

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject quest_Judge_CanvasPanel;
    private GameObject NouhinKetteiPanel_obj;

    private GameObject canvas;

    private GameObject shopitemlistController_obj;
    private ShopItemListController shopitemlistController;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;

    private SceneInitSetting sceneinit_setting;

    private GameObject yes_no_panel;
    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject item_tsuika; //PlayeritemList_ScrollViewの子オブジェクト「item_tsuika」ボタン

    private ItemDataBase database;

    private GameObject black_effect;

    public int update_ListSelect_Flag;

    public bool kettei_on_waiting;

    private bool playerlist_check_on;

    private int i;

    // Use this for initialization
    void Start() {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        update_ListSelect_Flag = 0;
        kettei_on_waiting = false;

    }

    void InitSetting()
    {
        yes_selectitem_kettei = SelectItem_kettei.Instance.GetComponent<SelectItem_kettei>();
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        canvas = GameObject.FindWithTag("Canvas");

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //アイテムリストがすでに生成されているかをチェック
        playerlist_check_on = false;
        foreach (Transform child in canvas.transform)
        {
            if (child.name == "PlayeritemList_ScrollView")
            {
                playerlist_check_on = true;
            }
        }

        if (playerlist_check_on)
        {
            //プレイヤーアイテムリストオブジェクトの初期化
            if (pitemlistController_obj == null)
            {
                pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();
            }

            //レシピ調合のときは、参照するオブジェクトが変わるので、それ対策
            if (GameMgr.CompoundSceneStartON)
            {
                kakuritsuPanel_obj = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                text_area = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject;
                _text = text_area.GetComponentInChildren<Text>();

                //まずは、レシピ・それ以外の調合用にオブジェクト取得
                if (GameMgr.compound_select == 1) //レシピ調合のときは、参照するオブジェクトが変わる。
                {
                    yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
                    yes = yes_no_panel.transform.Find("Yes").gameObject;
                    yes_text = yes.GetComponentInChildren<Text>();
                    no = yes_no_panel.transform.Find("No").gameObject;

                }
                else
                {
                    //レシピ以外では、アイテムリスト備え付けのyes,noを使う。
                    yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                    yes_text = yes.GetComponentInChildren<Text>();
                    no = pitemlistController_obj.transform.Find("No").gameObject;
                }
            }
            else
            {
                text_area = canvas.transform.Find("MessageWindow").gameObject;
                _text = text_area.GetComponentInChildren<Text>();

                //アイテムリスト備え付けのyes,noを使う。
                yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                yes_text = yes.GetComponentInChildren<Text>();
                no = pitemlistController_obj.transform.Find("No").gameObject;
            }
        }
    }

    //メソッドを読み込むタイミングでイニシャライズ
    void OnEnableInit()
    {
        if (updown_counter_obj == null)
        {
            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
        }
    }

    // Update is called once per frame
    void Update() {

        if (GameMgr.Scene_LoadedOn_End) //シーン読み込み完了してから動き出す
        {
            //初期化
            switch (GameMgr.Scene_Category_Num)
            {
                case 10:

                    //シーン読み込みのたびに、一度リセットされてしまうので、アップデートで一度初期化

                    if (canvas == null)
                    {
                        InitSetting();

                        kettei_on_waiting = false;
                    }

                    //レシピリストオブジェクトの初期化
                    if (recipilistController_obj == null)
                    {
                        recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
                        recipilistController = recipilistController_obj.GetComponent<RecipiListController>();
                    }

                    break;

                case 20:

                    if (canvas == null)
                    {
                        InitSetting();

                        shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                        shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                        yes = shopitemlistController_obj.transform.Find("Yes").gameObject;
                        yes_text = yes.GetComponentInChildren<Text>();
                        no = shopitemlistController_obj.transform.Find("No").gameObject;
                        no_text = no.GetComponentInChildren<Text>();

                        kettei_on_waiting = false;
                    }

                    break;

                case 30:

                    if (canvas == null)
                    {
                        InitSetting();

                        quest_Judge_CanvasPanel = canvas.transform.Find("Quest_Judge_CanvasPanel").gameObject;
                        NouhinKetteiPanel_obj = quest_Judge_CanvasPanel.transform.Find("NouhinKetteiPanel").gameObject;

                        shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                        shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                        shopquestlistController_obj = quest_Judge_CanvasPanel.transform.Find("ShopQuestList_ScrollView").gameObject;
                        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();

                        yes = shopitemlistController_obj.transform.Find("Yes").gameObject;
                        yes_text = yes.GetComponentInChildren<Text>();
                        no = shopitemlistController_obj.transform.Find("No").gameObject;
                        no_text = no.GetComponentInChildren<Text>();

                        kettei_on_waiting = false;
                    }

                    break;

                case 9999: //シーン読み込み途中などの回避用

                    break;



                default: //上記シーン以外

                    InitSetting();

                    break;
            }
        }



        //各シーンごとの、待機処理
        if (kettei_on_waiting == false) //トグルが押されていない時の処理。
                                        //トグル押されたら、トグルのほうのyes,noやキャンセルが優先され、この中のスクリプトは無視する。
        {

            //シーンに関係なく調合処理をつかうとき
            if (GameMgr.CompoundSceneStartON)
            {
                //調合中か、あげる処理に入っているか、もしくはアイテムリストを開いているとき
                switch (GameMgr.compound_status)
                {
                    case 4:

                        if (GameMgr.compound_select == 1) //レシピ調合のときは、参照するオブジェクトが変わる。
                        {
                            yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
                            yes = yes_no_panel.transform.Find("Yes").gameObject;
                            yes_text = yes.GetComponentInChildren<Text>();
                            no = yes_no_panel.transform.Find("No").gameObject;
                        }
                        else if (GameMgr.compound_select == 6) //ピクニックイベントなどでは、調合のセレクト画面でyes,noを押すので回避用。
                        {
                            //yes = canvas.transform.Find("Yes_no_Panel/Yes").gameObject;
                            //yes_text = yes.GetComponentInChildren<Text>();
                            //no = canvas.transform.Find("Yes_no_Panel/No").gameObject;
                        }
                        else
                        {
                            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                            yes_text = yes.GetComponentInChildren<Text>();
                            no = pitemlistController_obj.transform.Find("No").gameObject;
                        }

                        if (GameMgr.compound_select == 8 || GameMgr.compound_select == 120)
                        {
                            //ピクニックイベントなどでは、調合のセレクト画面でyes,noを押すので回避用。
                        }
                        else if (GameMgr.compound_select == 7)
                        {
                            if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                            {
                                if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                {
                                    //Debug.Log("調合シーンキャンセル");

                                    card_view.DeleteCard_DrawView();

                                    GameMgr.compound_status = 8; //何も選択していない状態にもどる。
                                    GameMgr.compound_select = 0;

                                    yes_selectitem_kettei.onclick = false;

                                }
                            }
                        }
                        else
                        {
                            if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                            {
                                if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                {
                                    //Debug.Log("調合シーンキャンセル");

                                    card_view.DeleteCard_DrawView();

                                    GameMgr.compound_status = 6; //何も選択していない状態にもどる。
                                    GameMgr.compound_select = 0;

                                    yes_selectitem_kettei.onclick = false;

                                }
                            }
                        }
                        break;

                    case 100:　//compound_status = 100のとき。一度トグルをおし、カードなどを選択し始めた場合、status=100になる。

                        //調合選択中のとき、キャンセル待ち処理
                        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7) //オリジナル調合のときの処理
                        {
                            if (GameMgr.final_select_flag == false) //最後、これで調合するかどうかを待つフラグ
                            {

                                //オリジナル調合時の、待機中の処理
                                {
                                    if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                                    {

                                        if (GameMgr.Comp_kettei_bunki == 1) //現在一個目を選択している状態
                                        {
                                            if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                            {
                                                //Debug.Log("一個目はcancel");

                                                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                                All_cancel();

                                            }
                                        }

                                        if (GameMgr.Comp_kettei_bunki == 2) //現在二個目を選択している状態
                                        {
                                            if (yes_selectitem_kettei.kettei1 == true) //調合二個で決定した状態
                                            {
                                                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                                                GameMgr.final_select_flag = true;

                                            }
                                            else if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                            {
                                                //Debug.Log("二個目はcancel");

                                                Two_cancel();

                                            }
                                        }
                                    }
                                }
                            }
                        }


                        if (GameMgr.compound_select == 2) //トッピング調合のときの処理
                        {

                            if (GameMgr.final_select_flag == false) //最後、これで調合するかどうかを待つフラグ
                            {

                                //トッピング調合時の、待機中の処理

                                if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                                {

                                    if (GameMgr.Comp_kettei_bunki == 10) //現在ベースアイテムを選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            pitemlistController.topping_DrawView_1(); //リストビューを更新し、トッピング材料だけ表示する。

                                            All_cancel();

                                        }
                                    }

                                    if (GameMgr.Comp_kettei_bunki == 11) //現在一個目を選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == true) //ベースアイテム＋調合１個で決定した状態
                                        {

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                                            GameMgr.final_select_flag = true;

                                        }

                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {

                                            exp_Controller._success_rate = 100;
                                            kakuritsuPanel.KakuritsuYosoku_Reset();
                                            Two_cancel();
                                        }
                                    }

                                    if (GameMgr.Comp_kettei_bunki == 12) //現在二個目を選択している状態
                                    {
                                        if (yes_selectitem_kettei.kettei1 == true) //ベースアイテム＋調合二個で決定した状態
                                        {

                                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                                            GameMgr.final_select_flag = true;

                                        }

                                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                        {
                                            //Debug.Log("二個目はcancel");

                                            exp_Controller._success_rate = exp_Controller._temp_srate_1;
                                            kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_1);
                                            Three_cancel();
                                        }
                                    }
                                }
                            }
                        }


                        if (GameMgr.compound_select == 21) //魔法の処理　魔法決定後、アイテムを選択中の画面
                        {

                            if (GameMgr.final_select_flag == false) //最後、これで調合するかどうかを待つフラグがオフの間だけ反応する
                            {

                                if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                                {

                                    if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                    {
                                        //魔法選択画面へ戻る
                                        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                                        GameMgr.compound_status = 20;//魔法選択スタートのステータスに戻す。

                                    }

                                }
                            }
                        }
                        break;

                    default://compound=110　最後調合するかどうかの確認中など、待機状態
                        break;
                }
            }

            if (GameMgr.Scene_Category_Num == 10) // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
            {

                switch (GameMgr.compound_status)
                {
                    case 21: //status=21。材料採取地選択中

                        if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                        {
                            if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                            {

                                GameMgr.compound_status = 0; //何も選択していない状態にもどる。
                                GameMgr.compound_select = 0;

                                yes_selectitem_kettei.onclick = false;

                            }
                        }

                        break;


                    case 61: //compound_status = 61。レシピ本を選択中。

                        if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                        {
                            if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                            {
                                All_cancel();

                                GameMgr.compound_status = 0; //何も選択していない状態にもどる。
                            }
                        }
                        break;

                    case 99: //compound_status = 99。アイテム画面開き中。

                        if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                        {
                            if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                            {
                                All_cancel();

                                GameMgr.compound_status = 0; //何も選択していない状態にもどる。
                            }
                        }
                        break;


                    case 100: //compound_status = 100のとき。一度トグルをおし、カードなどを選択し始めた場合、status=100になる。

                        
                        /*if (GameMgr.compound_select == 99) //カード一度開いた状態で、メニュー開いたのときの処理
                        {
                            if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                            {
                                if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                {
                                    All_cancel();

                                    pitemlistController._count1 = 9999;

                                    GameMgr.compound_status = 99; //何も選択していない状態にもどる。
                                }
                            }
                        }*/

                        if (GameMgr.compound_select == 200) //システム画面開いたのときの処理
                        {
                            if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                            {
                                if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                                {
                                    All_cancel();

                                    GameMgr.compound_status = 0; //何も選択していない状態にもどる。
                                }
                            }
                        }
                        break;



                    default://compound=110　最後調合するかどうかの確認中など、待機状態
                        break;
                }
            }


            if (GameMgr.Scene_Category_Num == 30)
            {
                if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                {
                    if (GameMgr.Scene_Select == 3 && shopquestlistController.nouhin_select_on == 1) //ショップ納品時の選択
                    {
                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                        {
                            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                            no = pitemlistController_obj.transform.Find("No").gameObject;

                            //Debug.Log("キャンセル");
                            if (pitemlistController._listcount.Count > 0)
                            {
                                _text.text = "次のお菓子を選んでね。";
                            }
                            else
                            {
                                _text.text = "渡したいお菓子を選んでね。";
                            }


                            //Debug.Log("pitemlistController._listcount[i]を削除: " + pitemlistController._listcount[pitemlistController._listcount.Count - 1]);
                            pitemlistController._listcount.RemoveAt(pitemlistController._listcount.Count - 1); //一番最後に挿入されたやつを、そのまま削除
                            pitemlistController._listkosu.RemoveAt(pitemlistController._listkosu.Count - 1); //一番最後に挿入されたやつを、そのまま削除

                            for (i = 0; i < pitemlistController._listitem.Count; i++)
                            {
                                //まずは、一度全て表示を初期化
                                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                                pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;

                            }


                            //選択済みのやつだけONにしておく。
                            for (i = 0; i < pitemlistController._listcount.Count; i++)
                            {
                                Debug.Log("pitemlistController._listcount[i]: " + i + ": " + pitemlistController._listcount[i]);
                                pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().interactable = false;
                                //pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().isOn = true;
                            }

                            card_view.DeleteCard_DrawView();

                            yes.SetActive(false);
                            //no.SetActive(false);
                            NouhinKetteiPanel_obj.SetActive(true);

                            yes_selectitem_kettei.onclick = false;

                            //Debug.Log("リストカウント: " + pitemlistController._listcount.Count);
                            if (pitemlistController._listcount.Count <= 0) //すべて選択してないときは、noはOFF
                            {
                                Debug.Log("リストカウント: " + pitemlistController._listcount.Count);
                                no.SetActive(false);
                            }

                        }
                    }
                }
            }

            //全シーンで共通の処理
            switch (GameMgr.compound_status)
            {
                case 1000: //サブイベントで、アイテムを何も選択していない状態

                    if (yes_selectitem_kettei.onclick) //Yes, No ボタンが押された
                    {
                        if (yes_selectitem_kettei.kettei1 == false) //キャンセルボタンをおした。
                        {
                            //kettei_on_waiting = false;
                            GameMgr.event_pitem_cancel = true; //やめたフラグON
                            yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
                        }
                    }
                    break;
            }
        }         
    }



    //一個目の選択をキャンセルする処理　トグルは何も押されていない状態に戻る
    public void All_cancel()
    {
        InitSetting();
        OnEnableInit();

        kettei_on_waiting = false;

        //シーンに関係なく調合処理をつかうとき
        if (GameMgr.CompoundSceneStartON)
        {
            GameMgr.compound_status = 4;

            //オリジナル調合の処理
            if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
            {
                if (GameMgr.Comp_kettei_bunki == 1)
                {
                    _text.text = "一つ目の材料を選択してね。";
                }

                GameMgr.Comp_kettei_bunki = 0;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }
            //エクストリーム調合のときの処理
            else if (GameMgr.compound_select == 2)
            {
                if (GameMgr.Comp_kettei_bunki == 10)
                {
                    //Debug.Log("調合シーンキャンセル");

                    _text.text = "";

                    card_view.DeleteCard_DrawView();
                    card_view.DeleteCard_DrawView();

                    GameMgr.Comp_kettei_bunki = 0;

                    GameMgr.compound_status = 6; //何も選択していない状態にもどる。
                    GameMgr.compound_select = 6;
                    //GameMgr.CompoundSceneStartON = false;　//調合シーン終了

                    /*
                    if (GameMgr.extremepanel_on != true) //通常のエクストリーム調合。ベースアイテム選択に戻る
                    {
                        _text.text = "ベースのお菓子を選択してね。";

                        pitemlistController.kettei1_bunki = 0;

                        update_ListSelect_Flag = 0; //オールリセットするのみ。
                        update_ListSelect(); //アイテム選択時の、リストの表示処理
                    }
                    else //エクストリームパネルから選んでいる。すぐに、プレイヤーアイテムリストをオフにする。
                    {
                        //Debug.Log("調合シーンキャンセル");

                        _text.text = "";

                        card_view.DeleteCard_DrawView();
                        card_view.DeleteCard_DrawView();

                        pitemlistController.kettei1_bunki = 0;

                        GameMgr.compound_status = 6; //何も選択していない状態にもどる。
                        GameMgr.compound_select = 0;
                        //GameMgr.CompoundSceneStartON = false;　//調合シーン終了

                    }*/
                }
            }
            else if (GameMgr.compound_select == 21) //魔法調合の処理
            {

                _text.text = GameMgr.UseMagicSkill_nameHyouji + "→ ";

                //GameMgr.Comp_kettei_bunki = 20;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }           
            else if (GameMgr.compound_select == 1) //レシピ調合のときの処理
            {
                _text.text = "レシピを選択してね。";

                //レシピのキャンセル(recipiitemlistController)は、recipiitemSelectToggleの中で処理。
                //なので、recipiitemlistControllerは、このスクリプト内では記述していない。
            }            
            else if (GameMgr.compound_select == 5) //焼くのときの処理
            {
                _text.text = "一つ目の材料を選択してね。";


                GameMgr.Comp_kettei_bunki = 0;

                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理
            }
        }
        else
        {
            //調合以外での処理
            if (GameMgr.Scene_Category_Num == 10) // 調合シーンでやりたい処理
            {          
                //お菓子をあげるときの処理
                if (GameMgr.compound_select == 10)
                {
                    _text.text = "あげるお菓子を選択してね。";


                    GameMgr.Comp_kettei_bunki = 0;

                    update_ListSelect_Flag = 0; //オールリセットするのみ。
                    update_ListSelect(); //アイテム選択時の、リストの表示処理
                }

                else if (GameMgr.compound_select == 99)
                {

                    update_ListSelect_Flag = 0; //オールリセットするのみ。
                    update_ListSelect(); //アイテム選択時の、リストの表示処理
                }

            }
            else if (GameMgr.Scene_Category_Num == 200) //
            {
                update_ListSelect_Flag = 0; //オールリセットするのみ。
                                            //update_ListSelect(); //アイテム選択時の、リストの表示処理
            }
            else
            {
                update_ListSelect_Flag = 0; //オールリセットするのみ。
                update_ListSelect(); //アイテム選択時の、リストの表示処理                
            }
        }

        updown_counter_obj.SetActive(false);
        card_view.DeleteCard_DrawView();

        yes.SetActive(false);

        if (GameMgr.tutorial_ON == true)
        {
            no.SetActive(false);
        }
        else
        {
            no.SetActive(true);
        }
     

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

    }



    //以下の処理は、調合シーンのみで使う。

    //二個目の選択をキャンセルする処理（一個目は選択中）

    public void Two_cancel()
    {
        InitSetting();
        OnEnableInit();

        kettei_on_waiting = false;

        if (GameMgr.Comp_kettei_bunki == 2)
        {
            update_ListSelect_Flag = 1; //二個目まで、選択できないようにする。
            update_ListSelect(); //アイテム選択時の、リストの表示処理

            pitemlistController._listitem[GameMgr.List_count2].GetComponent<Toggle>().isOn = false; //選択していたものをキャンセル。

            GameMgr.Comp_kettei_bunki = 1;

            _text.text = "一個目: " + database.items[GameMgr.Final_list_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" 
                + "二個目を選択してください。";
        }

        if (GameMgr.Comp_kettei_bunki == 11)
        {
            update_ListSelect_Flag = 10; //ベースアイテム選択のみの状態
            update_ListSelect(); //アイテム選択時の、リストの表示処理


            pitemlistController._listitem[GameMgr.List_count1].GetComponent<Toggle>().isOn = false;

            GameMgr.Comp_kettei_bunki = 10;

            _text.text = "ベースアイテム: " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "\n" + "一つ目のトッピングアイテムを選択してください。";

            if (GameMgr.tutorial_ON == true)
            {
                no.SetActive(false);
            }
            else
            {
                no.SetActive(true);
            }
        }


        card_view.DeleteCard_DrawView02();
        card_view.OKCard_DrawView(GameMgr.Final_kettei_kosu1);

        yes.SetActive(false);
        //no.SetActive(false);
        updown_counter_obj.SetActive(false);

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }



    //三個目の選択をキャンセルする処理
    public void Three_cancel()
    {
        InitSetting();
        OnEnableInit();

        kettei_on_waiting = false;

        if (GameMgr.Comp_kettei_bunki == 3)
        {
            update_ListSelect_Flag = 2; //二個目まで、選択できないようにする。
            update_ListSelect(); //アイテム選択時の、リストの表示処理

            pitemlistController._listitem[GameMgr.List_count3].GetComponent<Toggle>().isOn = false; //三個目の選択はキャンセル

            GameMgr.Comp_kettei_bunki = 2;
            GameMgr.Final_list_itemID3 = 9999;

            _text.text = "一個目: " + database.items[GameMgr.Final_list_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" 
                + "二個目: " + database.items[GameMgr.Final_list_itemID2].itemNameHyouji + " " + GameMgr.Final_kettei_kosu2 + "個" + "\n" 
                + "最後に一つ追加できます。";
        }

        if (GameMgr.Comp_kettei_bunki == 12)
        {
            update_ListSelect_Flag = 11; //ベース・一個目の選択の状態に戻る。
            update_ListSelect(); //アイテム選択時の、リストの表示処理

            pitemlistController._listitem[GameMgr.List_count2].GetComponent<Toggle>().isOn = false;

            GameMgr.Comp_kettei_bunki = 11;
            GameMgr.Final_list_itemID2 = 9999;

            _text.text = "ベースアイテム: " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "\n" + 
                "一個目: " + database.items[GameMgr.Final_list_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" 
                + "二個目を選択してください。";
        }

        card_view.DeleteCard_DrawView03();
        card_view.OKCard_DrawView02(GameMgr.Final_kettei_kosu2);

        yes_text.text = "決定";
        yes.SetActive(false);
        //no.SetActive(false);
        updown_counter_obj.SetActive(false);

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }


    //四個目の選択をキャンセルする処理
    public void Four_cancel()
    {
        InitSetting();
        OnEnableInit();

        //トッピング調合のときのみ、使う。

        kettei_on_waiting = false;

        update_ListSelect_Flag = 12; //ベースアイテムと一個目・二個目を選択できないようにする。
        update_ListSelect();

        pitemlistController._listitem[GameMgr.List_count3].GetComponent<Toggle>().isOn = false;

        GameMgr.Comp_kettei_bunki = 12;

        _text.text = "ベースアイテム: " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "\n" 
            + "一個目: " + database.items[GameMgr.Final_list_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" 
            + "二個目: " + database.items[GameMgr.Final_list_itemID2].itemNameHyouji + " " + GameMgr.Final_kettei_kosu2 + "個" + "\n" 
            + "最後に一つ追加できます。";

        card_view.DeleteCard_DrawView04();
        card_view.OKCard_DrawView03(1);


        yes_text.text = "決定";
        yes.SetActive(false);
        //no.SetActive(false);
        updown_counter_obj.SetActive(false);

        GameMgr.final_select_flag = false;

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }



    //リストからアイテム選択時に、選択したアイテムを再度入力できなくする処理
    public void update_ListSelect()
        {

            for (i = 0; i < pitemlistController._listitem.Count; i++)
            {
                //まずは、一度全て表示を初期化
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
            }

            if (update_ListSelect_Flag == 0)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }
            }
            else if (update_ListSelect_Flag == 1)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_1();
                }
            }
            else if (update_ListSelect_Flag == 2)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_1();
                    update_ListSelect_2();
                }
            }
            else if (update_ListSelect_Flag == 3)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_1();
                    update_ListSelect_2();
                    update_ListSelect_3();
                }
            }
            else if (update_ListSelect_Flag == 10)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                }
            }
            else if (update_ListSelect_Flag == 11)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                    update_ListSelect_1();
                }
            }
            else if (update_ListSelect_Flag == 12)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                    update_ListSelect_1();
                    update_ListSelect_2();
                }
            }
            else if (update_ListSelect_Flag == 13)
            {
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    update_ListSelect_base();
                    update_ListSelect_1();
                    update_ListSelect_2();
                    update_ListSelect_3();
                }
            }
    }


    void update_ListSelect_base()
    {
            //トッピング調合時、ベースアイテムを選択できないようにする。

            //とりあえず、表示されてるリストを上から順番に見ていく。店売りかオリジナルの判定＋その時のプレイヤーリスト番号が一致するものが、一個目に選択したもの。

            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == GameMgr.Final_toggle_baseType && 
            pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == GameMgr.Final_list_baseitemID)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //一個目も選択できないようにする
                //Debug.Log("一個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type1 + " リストID: " + pitemlistController.kettei_item1);
            }

    }

    void update_ListSelect_1()
    {
            //一個目選択したものを選択できないようにする。

            //とりあえず、表示されてるリストを上から順番に見ていく。店売りかオリジナルの判定＋その時のプレイヤーリスト番号が一致するものが、一個目に選択したもの。

            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == GameMgr.Final_toggle_Type1 && 
            pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == GameMgr.Final_list_itemID1)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //一個目も選択できないようにする
                //Debug.Log("一個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type1 + " リストID: " + pitemlistController.kettei_item1);
            }

    }

    void update_ListSelect_2()
        {
            //二個目選択したものを選択できないようにする。一個目と同様の処理。
            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == GameMgr.Final_toggle_Type2 && 
            pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == GameMgr.Final_list_itemID2)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //二個目も選択できないようにする
                //Debug.Log("二個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type2 + " リストID: " + pitemlistController.kettei_item2);
            }

    }

    void update_ListSelect_3()
        {
            //三個目選択したものを選択できないようにする。
            if (pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggleitem_type == GameMgr.Final_toggle_Type3 && 
            pitemlistController._listitem[i].GetComponent<itemSelectToggle>().toggle_originplist_ID == GameMgr.Final_list_itemID3)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false; //三個目も選択できないようにする
                //Debug.Log("三個目選択したもの: アイテムトグルタイプ" + pitemlistController._toggle_type3 + " リストID: " + pitemlistController.kettei_item3);
            }

    }
}

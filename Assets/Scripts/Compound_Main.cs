﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Compound_Main : MonoBehaviour
{

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    private BGM sceneBGM;
    private bool bgm_change_flag;

    private Girl1_status girl1_status;

    private Debug_Panel_Init debug_panel_init;

    private GameObject getmatplace_panel;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;
    private GameObject recipilist_scrollview_init_obj;

    private GameObject recipimemoController_obj;
    private GameObject recipiMemoButton;
    private GameObject memoResult_obj;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

    private GameObject black_panel_A;
    private GameObject compoBG_A;

    private GameObject SelectCompo_panel_1;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject compoundselect_onoff_obj;
    private GameObject saveload_panel;

    private GameObject original_toggle;
    private GameObject recipi_toggle;
    private GameObject topping_toggle;
    private GameObject extreme_toggle;
    private GameObject roast_toggle;
    private GameObject blend_toggle;

    private GameObject menu_toggle;
    private GameObject girleat_toggle;
    private GameObject shop_toggle;
    private GameObject getmaterial_toggle;
    private GameObject stageclear_toggle;

    private GameObject backbutton_obj;

    private Button extreme_Button;
    private Button recipi_Button;
    private GameObject sell_Button;
    private GameObject present_Button;
    private GameObject stageclear_Button;

    private bool Recipi_loading;
    private bool GirlLove_loading;
    public bool check_recipi_flag;
    public bool check_GirlLoveEvent_flag;
    private int not_read_total;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject yes_no_panel; //通常時のYes, noボタン
    private GameObject yes_no_clear_panel; //クリア時のYes, noボタン

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private int i, j, _id;
    private int recipi_num;
    private int comp_ID;
    private int clear_love;

    public int compound_status;
    public int compound_select;

    public int event_itemID; //イベントレシピ使用時のイベントのID



    // Use this for initialization
    void Start()
    {

        //Debug.Log("Compound scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
        bgm_change_flag = false;

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;
        backbutton_obj.SetActive(false);

        //プレイヤー所持アイテムリストパネルの取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();
        playeritemlist_onoff.SetActive(false);

        //レシピリストパネルの取得
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().RecipiList_ScrollView_Init();
        recipilist_onoff = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();
        recipilist_onoff.SetActive(false);

        //レシピメモボタンを取得
        recipimemoController_obj = canvas.transform.Find("Compound_BGPanel_A/RecipiMemo_ScrollView").gameObject;
        recipiMemoButton = canvas.transform.Find("Compound_BGPanel_A/RecipiMemoButton").gameObject;
        memoResult_obj = canvas.transform.Find("Compound_BGPanel_A/Memo_Result").gameObject;


        //事前にyes, noオブジェクトなどを読み込んでから、リストをOFF
        yes = playeritemlist_onoff.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = playeritemlist_onoff.transform.Find("No").gameObject;
        no_text = no.GetComponentInChildren<Text>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_clear_panel = canvas.transform.Find("StageClear_Yes_no_Panel").gameObject;

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //材料ランダムで３つ手に入るオブジェクトの取得
        get_material_obj = GameObject.FindWithTag("GetMaterial");
        get_material = get_material_obj.GetComponent<GetMaterial>();

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //エクストリームパネルの取得
        Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");
        extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();

        //ボタンの取得
        extreme_Button = Extremepanel_obj.transform.Find("ExtremeButton").gameObject.GetComponent<Button>(); //エクストリームボタン
        recipi_Button = Extremepanel_obj.transform.Find("RecipiButton").gameObject.GetComponent<Button>(); //レシピボタン
        sell_Button = Extremepanel_obj.transform.Find("SellButton").gameObject; //売るボタン
        present_Button = Extremepanel_obj.transform.Find("PresentButton").gameObject; //売るボタン 

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //黒半透明パネルの取得
        black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;
        black_panel_A.SetActive(false);

        //コンポBGパネルの取得
        compoBG_A = canvas.transform.Find("Compound_BGPanel_A").gameObject;
        compoBG_A.SetActive(false);

        //調合選択画面の取得
        SelectCompo_panel_1 = canvas.transform.Find("Compound_BGPanel_A/SelectPanel_1").gameObject;
        SelectCompo_panel_1.SetActive(false);

        //材料採取地パネルの取得
        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel").gameObject;
        getmatplace_panel.SetActive(false);

        compoundselect_onoff_obj = GameObject.FindWithTag("CompoundSelect");
        saveload_panel = canvas.transform.Find("SaveLoadPanel").gameObject;

        original_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Original_Toggle").gameObject;
        recipi_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Recipi_Toggle").gameObject;
        extreme_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Extreme_Toggle").gameObject;
        roast_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Roast_Toggle").gameObject;
        //blend_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Blend_Toggle").gameObject;

        menu_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/ItemMenu_Toggle").gameObject;
        girleat_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GirlEat_Toggle").gameObject;
        shop_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/Shop_Toggle").gameObject;
        getmaterial_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/GetMaterial_Toggle").gameObject;
        stageclear_toggle = compoundselect_onoff_obj.transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;

        stageclear_Button = canvas.transform.Find("StageClear_Button").gameObject;
        stageclear_Button.SetActive(false);

        //初期メッセージ
        _text.text = "何の調合をする？";
        text_area.SetActive(true);

        compound_status = 0;
        compound_select = 0;

        Recipi_loading = false;
        GirlLove_loading = false;
        check_recipi_flag = false;
        check_GirlLoveEvent_flag = false;

        //女の子　お菓子ハングリー状態のリセット
        girl1_status.Girl1_Status_Init();

        //ステージクリア用の好感度数値
        switch (GameMgr.stage_number)
        {
            case 1:

                clear_love = GameMgr.stage1_clear_love;
                break;

            case 2:

                clear_love = GameMgr.stage2_clear_love;
                break;

            case 3:

                clear_love = GameMgr.stage3_clear_love;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

        switch (GameMgr.scenario_flag)
        {

            case 110: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。また、何を作ればよいかを指示してくれる。

                GameMgr.scenario_ON = true; //これがONのときは、調合シーンの、調合ボタンなどはオフになり、シナリオを優先する。「Utage_scenario.cs」のUpdateが同時に走っている。
                break;

            case 130: //ショップから帰ってきた。

                GameMgr.scenario_ON = true; 
                break;

            default:
                break;
        }

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。チュートリアルなどの強制イベントのチェック。
        if (GameMgr.scenario_ON == true)
        {

            //チュートリアルモードがONになったら、この中の処理が始まる。
            if (GameMgr.tutorial_ON == true) 
            {
                                       
                switch (GameMgr.tutorial_Num)
                {
                    case 0: //最初にシナリオを読み始める。

                        Extremepanel_obj.SetActive(true);

                        //一時的に腹減りを止める。+腹減りステータスをリセット
                        girl1_status.GirlEat_Judge_on = false;
                        girl1_status.Girl_Full();
                        girl1_status.Girl1_Status_Init();
                        GameMgr.tutorial_Num = 1; //退避
                        break;

                    case 10: //宴ポーズ。エクストリームパネルを押そう！で、待機。

                        MainCompoundMethod();
                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(true);
                        _text.text = "左のエクストリームパネルを押してみよう！";
                        break;

                    case 20: //エクストリームパネルを押して、オリジナル調合画面を開いた
                       
                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);
                        text_area.SetActive(false);

                        compoBG_A.GetComponent<Image>().raycastTarget = false;
                        pitemlistController.Offinteract();
                        kakuritsuPanel_obj.SetActive(false);
                        no.SetActive(false);

                        Extremepanel_obj.SetActive(false);

                        break;

                    case 30: //宴がポーズ状態。右のレシピメモを押そう。

                        text_area.SetActive(true);
                        _text.text = "右の「レシピメモ」ボタンを押してみよう！";
                        break;

                    case 40: //メモ画面を開いた。

                        text_area.SetActive(false);
                        break;

                    case 50: //宴ポーズ。オリジナル調合をしてみるところ。

                        pitemlistController.Oninteract();
                        text_area.SetActive(true);
                        _text.text = "新しくお菓子を作るよ！" + "\n" + "好きな材料を" + "<color=#0000FF>" + "２つ" + "</color>" + "か" + "<color=#0000FF>" + "３つ" + "</color>" + "選んでね。";

                        GameMgr.tutorial_Num = 55;
                        break;

                    case 55: //調合中
                        break;

                    case 60: //調合完了！

                        text_area.SetActive(false);
                        
                        break;

                    case 70: //宴ポーズ。やったね！クッキーができた～から、レシピを閃き、ボタンを押し待ち。

                        card_view.SetinteractiveOn();
                        text_area.SetActive(true);
                        GameMgr.tutorial_Num = 75; //退避
                        break;

                    case 80: //ボタンを押し、元の画面に戻る。

                        MainCompoundMethod();

                        compoundselect_onoff_obj.SetActive(false);

                        extreme_Button.interactable = false;
                        sell_Button.SetActive(false);

                        //compoundselect_onoff_obj.SetActive(true);
                        text_area.SetActive(false);
                        break;

                    case 90: //「あげる」ボタンを押すところ。「あげる」のみをON、他のボタンはオフ。

                        //Debug.Log("GameMgr.チュートリアルNo: " + GameMgr.tutorial_Num);
                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(true);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        text_area.SetActive(false);

                        girl1_status.InitializeStageGirlHungrySet(0, 0);
                        girl1_status.Girl_Hungry();
                        girl1_status.timeGirl_hungry_status = 1; //腹減り状態に切り替え

                        GameMgr.tutorial_Num = 95; //退避

                        break;

                    case 100:

                        MainCompoundMethod();

                        extreme_Button.interactable = false;
                        sell_Button.SetActive(false);

                        compoundselect_onoff_obj.SetActive(true);
                        menu_toggle.GetComponent<Toggle>().interactable = false;
                        getmaterial_toggle.GetComponent<Toggle>().interactable = false;
                        shop_toggle.GetComponent<Toggle>().interactable = false;
                        text_area.SetActive(true);
                        break;

                    case 110:

                        extreme_Button.interactable = false;
                        compoundselect_onoff_obj.SetActive(false);

                        text_area.SetActive(false);
                        break;

                    default:
                        break;
                }
                
            }

            else //チュートリアル以外、デフォルトで、宴を読んでいるときの処理
            {
                compoundselect_onoff_obj.SetActive(false);
                Extremepanel_obj.SetActive(false);
                text_area.SetActive(false);
                check_recipi_flag = false;
            }

        }
        else //以下が、通常の処理
        {

            //好感度チェック。好感度に応じて、イベントが発生。
            if (check_GirlLoveEvent_flag == false)
            {
                Check_GirlLoveEvent();

            }
            else
            {

                //読んでいないレシピがあれば、読む処理。優先順位二番目。
                if (check_recipi_flag == false)
                {
                    Check_RecipiFlag();
                }
                else
                {

                    //メインの調合処理　各ボタンを押すと、中の処理が動き始める。
                    MainCompoundMethod();
                    
                }
            }
        }
    }

    //メインの調合シーンの処理
    void MainCompoundMethod()
    {
        switch (compound_status)
        {
            case 0:

                if (GameMgr.tutorial_ON != true)
                {
                    compoundselect_onoff_obj.SetActive(true);

                    //腹減りカウント開始
                    girl1_status.GirlEat_Judge_on = true;
                }

                //Debug.Log("メインの調合シーン　スタート");
                recipilist_onoff.SetActive(false);
                playeritemlist_onoff.SetActive(false);
                yes_no_panel.SetActive(false);
                getmatplace_panel.SetActive(false);               
                kakuritsuPanel_obj.SetActive(false);
                black_panel_A.SetActive(false);
                compoBG_A.SetActive(false);
                sceneBGM.MuteOFFBGM();
                
                recipiMemoButton.SetActive(false);

                if (bgm_change_flag == true)
                {
                    bgm_change_flag = false;
                    sceneBGM.OnMainBGM();
                }



                //好感度がステージの、一定の数値を超えたら、クリアボタンがでる。
                if (girl1_status.girl1_Love_exp >= clear_love)
                {
                    //stageclear_toggle.SetActive(true);
                    stageclear_Button.SetActive(true);
                }
                else
                {
                    if (stageclear_toggle.activeSelf == true)
                    {
                        //stageclear_toggle.SetActive(false);
                        stageclear_Button.SetActive(false);
                    }
                }

                Extremepanel_obj.SetActive(true);
                extreme_panel.extremeButtonInteractOn();
                extreme_panel.LifeAnimeOnTrue();

                text_area.SetActive(true);

                text_scenario();

                compound_status = 100; //退避
                break;

            case 1: //レシピ調合の処理を開始。クリック後に処理が始まる。

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 1; //今、どの調合をしているかを番号で知らせる。レシピ調合を選択

                recipilist_onoff.SetActive(true); //レシピリスト画面を表示。
                kakuritsuPanel_obj.SetActive(true);
                black_panel_A.SetActive(true);
                compoBG_A.SetActive(true);
                extreme_panel.extremeButtonInteractOFF();
                text_area.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                yes.SetActive(false);
                no.SetActive(true);

                break;

            case 2: //エクストリーム調合の処理を開始。クリック後に処理が始まる。

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 2; //トッピング調合を選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                kakuritsuPanel_obj.SetActive(true);
                black_panel_A.SetActive(true);
                compoBG_A.SetActive(true);
                extreme_panel.extremeButtonInteractOFF();

                //BGMを変更
                //sceneBGM.OnCompoundBGM();

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。

                yes.SetActive(false);
                no.SetActive(true);

                break;

            case 3: //オリジナル調合の処理を開始。クリック後に処理が始まる。

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 3; //オリジナル調合を選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                kakuritsuPanel_obj.SetActive(true);

                //black_panel_A.SetActive(true);
                compoBG_A.SetActive(true);
                extreme_panel.extremeButtonInteractOFF();
                recipiMemoButton.SetActive(true);
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);

                //BGMを変更
                //sceneBGM.OnCompoundBGM();

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。 
                yes.SetActive(false);
                no.SetActive(true);

                break;

            case 4: //調合シーンに入ってますよ、というフラグ。各ケース処理後、必ずこの中の処理に移行する。yes, noボタンを押されるまでは、待つ状態に入る。

                break;

            case 5: //「焼く」を選択

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ
                compound_select = 5; //焼くを選択

                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                pitemlistController.ResetKettei_item(); //プレイヤーアイテムリスト、選択したアイテムIDとリスト番号をリセット。
                yes.SetActive(false);
                no.SetActive(true);

                break;

            case 6: //オリジナル調合かレシピ調合を選択できるパネルを表示

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 4; //調合シーンに入っています、というフラグ

                SelectCompo_panel_1.SetActive(true);
                compoBG_A.SetActive(true);
                extreme_panel.extremeButtonInteractOFF();
                recipimemoController_obj.SetActive(false);
                memoResult_obj.SetActive(false);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                break;

            case 10: //「あげる」を選択

                compoundselect_onoff_obj.SetActive(false);

                compound_status = 13; //あげるシーンに入っています、というフラグ
                compound_select = 10; //あげるを選択

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                card_view.PresentGirl(extreme_panel.extreme_itemtype, extreme_panel.extreme_itemID);
                StartCoroutine("Girl_present_Final_select");


                break;

            case 11: //お菓子をあげたあとの処理。女の子が、お菓子を判定

                compound_status = 12;

                //お菓子の判定処理を起動。引数は、決定したアイテムのアイテムIDと、店売りかオリジナルで制作したアイテムかの、判定用ナンバー 0or1
                girlEat_judge.Girleat_Judge_method(extreme_panel.extreme_itemID, extreme_panel.extreme_itemtype);

                break;

            case 12: //お菓子を判定中

                break;

            case 13: //あげるかあげないかを選択中

                break;

            case 20: //材料採取地を選択中

                compoundselect_onoff_obj.SetActive(false);

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                Extremepanel_obj.SetActive(false);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                break;

            case 21: //材料採取地に到着。探索中

                break;

            case 30: //「売る」を選択

                compoundselect_onoff_obj.SetActive(false);
                stageclear_Button.SetActive(false);

                compound_status = 31; //売るシーンに入っています、というフラグ
                compound_select = 30; //売るを選択

                yes_no_panel.SetActive(true);
                yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);
                StartCoroutine("Sell_Final_select");


                break;

            case 31: //売るかどうか、選択中

                break;

            case 32: //売る処理の実行

                compound_status = 32;

                extreme_panel.Sell_Okashi();
                break;

            case 40: //ステージクリアを選択

                compoundselect_onoff_obj.SetActive(false);
                stageclear_Button.SetActive(false);

                compound_status = 41; //売るシーンに入っています、というフラグ
                compound_select = 40; //売るを選択

                yes_no_clear_panel.SetActive(true);

                //一時的に腹減りを止める。
                girl1_status.GirlEat_Judge_on = false;

                extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止
                black_panel_A.SetActive(true);
                StartCoroutine("StageClear_Final_select");
                break;

            case 41: //クリアするかどうか、選択中
                break;

            case 99: //アイテム画面を開いたとき

                compoundselect_onoff_obj.SetActive(false);
                saveload_panel.SetActive(false);
                black_panel_A.SetActive(true);
                compound_status = 4;
                compound_select = 99;
                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。
                yes.SetActive(false);
                no.SetActive(true);

                break;

            case 100: //退避用

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
    }


    public void OnCheck_1() //レシピ調合をON
    {
        if (recipi_toggle.GetComponent<Toggle>().isOn == true)
        {
            recipi_toggle.GetComponent<Toggle>().isOn = false;
            yes_no_load();

            card_view.DeleteCard_DrawView();

            _text.text = "レシピから作るよ。何を作る？";
            compound_status = 1;
        }
    }

    public void OnCheck_1_button()
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = "レシピから作るよ。何を作る？";
        compound_status = 1;
    }

    public void OnCheck_2() //トッピング調合をON
    {
        if (extreme_toggle.GetComponent<Toggle>().isOn == true)
        {
            extreme_toggle.GetComponent<Toggle>().isOn = false;
            yes_no_load();

            pitemlistController.extremepanel_on = false;
            card_view.DeleteCard_DrawView();

            _text.text = "エクストリーム調合をするよ！ まずは、お菓子を選んでね。";
            compound_status = 2;
        }
    }

    public void OnCheck_3() //オリジナル調合をON
    {
        if (original_toggle.GetComponent<Toggle>().isOn == true)
        {
            original_toggle.GetComponent<Toggle>().isOn = false;
            yes_no_load();

            card_view.DeleteCard_DrawView();

            _text.text = "新しくお菓子を作るよ！" + "\n" + "好きな材料を" + "<color=#0000FF>" + "２つ" + "</color>" + "か" + "<color=#0000FF>" + "３つ" + "</color>" + "選んでね。";
            compound_status = 3;
        }
    }

    public void OnCheck_3_button() //調合選択画面からボタンを選択して、オリジナル調合をON
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = "新しくお菓子を作るよ！" + "\n" + "好きな材料を" + "<color=#0000FF>" + "２つ" + "</color>" + "か" + "<color=#0000FF>" + "３つ" + "</color>" + "選んでね。";
        compound_status = 3;
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
            roast_toggle.GetComponent<Toggle>().isOn = false;
            yes_no_load();

            card_view.DeleteCard_DrawView();

            _text.text = "作った生地を焼きます。焼きたい生地を選んでください。";
            compound_status = 5;
        }
    }

    public void OnCancel_Select()
    {
        compound_status = 0;
    }

    public void OnMenu_toggle() //メニューをON
    {
        if (menu_toggle.GetComponent<Toggle>().isOn == true)
        {
            menu_toggle.GetComponent<Toggle>().isOn = false;
            yes_no_load();

            card_view.DeleteCard_DrawView();

            compound_status = 99;
        }
    }

    public void OnShop_toggle() //ショップへ移動
    {
        if (shop_toggle.GetComponent<Toggle>().isOn == true)
        {
            card_view.DeleteCard_DrawView();

            shop_toggle.GetComponent<Toggle>().isOn = false;
            FadeManager.Instance.LoadScene("Shop", 0.3f);
        }
    }

    public void OnGetMaterial_toggle() //材料をランダムで入手する処理
    {
        if (getmaterial_toggle.GetComponent<Toggle>().isOn == true)
        {
            getmaterial_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();
            _text.text = "妹と一緒に材料を取りにいくよ！行き先を選んでね。";
            compound_status = 20;

            //BGMを変更
            sceneBGM.OnGetMatStartBGM();
            bgm_change_flag = true;

            //compoundselect_onoff_obj.SetActive(false);
            getmatplace_panel.SetActive(true);
            yes_no_panel.SetActive(true);
            yes_no_panel.transform.Find("Yes").gameObject.SetActive(false);

        }
    }

    public void OnGirlEat() //女の子にお菓子をあげる
    {
        if (girleat_toggle.GetComponent<Toggle>().isOn == true)
        {
            girleat_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            yes_no_load();

            card_view.DeleteCard_DrawView();

            if ( extreme_panel.extreme_itemID != 9999 )
            {
                _text.text = "今、作ったお菓子をあげますか？";
                compound_status = 10;
            }
            else //まだ作ってないときは
            {
                _text.text = "まだお菓子を作っていない。";
            }
            

        }
    }

    public void OnStageClear() //ステージクリアボタン
    {
        //if (stageclear_toggle.GetComponent<Toggle>().isOn == true)
        //{
            //stageclear_toggle.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            yes_no_load();

            card_view.DeleteCard_DrawView();

            _text.text = "次のお話に進みますか？";
            compound_status = 40;

        //}
    }


    //レシピを見たときの処理。recipiitemselecttoggleから呼ばれる。
    public void eventRecipi_ON()
    {
        recipilist_onoff.SetActive(false);
        Debug.Log("イベントレシピID: " + event_itemID + "　レシピ名: " + pitemlist.eventitemlist[event_itemID].event_itemNameHyouji);

        compoundselect_onoff_obj.SetActive(false);
        saveload_panel.SetActive(false);
        kakuritsuPanel_obj.SetActive(false);
        backbutton_obj.SetActive(false);
        text_area.SetActive(false);
        black_panel_A.SetActive(false);

        //一時的に腹減りを止める。
        girl1_status.GirlEat_Judge_on = false;

        compoBG_A.GetComponent<Image>().raycastTarget = false; //このときだけ、背景画像のタッチ判定をオフにする。そうしないと、宴がクリックに反応しなくなる。
        Extremepanel_obj.SetActive(false);


        GameMgr.recipi_read_ID = event_itemID;
        GameMgr.itemuse_recipi_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        StartCoroutine("eventRecipi_end");

    }

    IEnumerator eventRecipi_end()
    {
        //Debug.Log("eventRecipi_end() on");
        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        Recipi_loading = false;

        compoBG_A.GetComponent<Image>().raycastTarget = true;
        Extremepanel_obj.SetActive(true);
        compound_status = 1;
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
        /*
        switch (GameMgr.scenario_flag)
        {
            case 115:
                _text.text = "何の調合をする？" + "\n" + "(さっきのレシピを見てみるか。)";

                break;

            case 120:
                _text.text = "何の調合をする？" + "\n" + "(まずは、お菓子の材料を買わないとな..。)";

                break;

            default:
                break;
        }*/
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
            not_read_total = 0;
            Recipi_loading = false;

            while (i < pitemlist.eventitemlist.Count)
            {
                //所持はしているのに、リードフラグは０のまま（＝読んでいないもの）のレシピの個数をカウントする。
                if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ReadFlag == 0)
                {
                    not_read_total++;
                }
                i++;
            }

            i = 0;

            while (i < pitemlist.eventitemlist.Count)
            {

                //もし、所持はしているのに、リードフラグは０のまま（＝読んでいないもの）がある場合、レシピを読む処理に入る。
                if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ReadFlag == 0)
                {
                    /*
                    //一度もレシピを読み込んでいなければ、レシピトグルをONに。
                    if (PlayerStatus.First_recipi_on != true)
                    {
                        PlayerStatus.First_recipi_on = true;
                    }*/

                    Recipi_loading = true; //レシピを読み込み中ですよ～のフラグ

                    recipi_num = i;

                    break;
                }

                ++i;
            }

            if (Recipi_loading != true)
            {
            }
            else
            {
                StartCoroutine("Recipi_Read_Method");
            }

            if (not_read_total <= 0)
            {
                //最後にチェック。全てのリードフラグが1になったら、全て読み終了。その場合は、レシピチェックをオフにする。
                check_recipi_flag = true;
                //Debug.Log("レシピ全て読み完了");
            }
        }
    }

    IEnumerator Recipi_Read_Method()
    {

        compoundselect_onoff_obj.SetActive(false);
        Extremepanel_obj.SetActive(false);
        text_area.SetActive(false);

        //一時的に腹減りを止める。
        girl1_status.GirlEat_Judge_on = false;

        GameMgr.recipi_read_ID = pitemlist.eventitemlist[recipi_num].ev_ItemID;
        GameMgr.recipi_read_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                         //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        not_read_total--;
        GameMgr.recipi_read_endflag = false;
        Recipi_loading = false;

        //Debug.Log("not_read_total: " + not_read_total);

        /* レシピを読む処理 */
        pitemlist.eventitemlist[recipi_num].ev_ReadFlag = 1; //該当のイベントアイテムのレシピのフラグをONにしておく（読んだ、という意味）
        Recipi_FlagON_Method();
        Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji + "を読んだ");
    }

    void Recipi_FlagON_Method()
    {

        //レシピの番号チェック
        switch(pitemlist.eventitemlist[recipi_num].event_itemName)
        {
            case "ev02_orangeneko_cookie_memo": //オレンジネコクッキー閃きのメモ

                //オレンジジャムの作り方を解禁
                Find_compoitemdatabase("orange_jam");
                databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;
                break;

            case "najya_start_recipi": //ナジャのお菓子作りの基本                

                break;

            case "cookie_base_recipi": //クッキー生地作り方のレシピ＜初級＞  

                //Find_compoitemdatabase("financier");
                //databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;

                Find_compoitemdatabase("appaleil");
                databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;
                break;

            case "ice_cream_recipi": //アイスクリームの書

                Find_compoitemdatabase("ice_cream");
                databaseCompo.compoitems[comp_ID].cmpitem_flag = 1;
                break;

            case "financier_recipi": //フィナンシェ

                Find_compoitemdatabase("kogashi_butter");
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
        while (j < databaseCompo.compoitems.Count)
        {
            if (compo_itemname == databaseCompo.compoitems[j].cmpitem_Name)
            {
                comp_ID = j;
                break;
            }
            j++;
        }
    }


    IEnumerator Girl_present_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);
        card_view.DeleteCard_DrawView();
        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //女の子にアイテムをあげる処理
                compound_status = 11; //status=11で処理。

                yes_no_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;
                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "";
                compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    IEnumerator Sell_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //売る処理
                compound_status = 32; //status=32で処理。

                yes_no_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;
                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "";
                compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    IEnumerator StageClear_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //ステージクリア処理

                yes_no_clear_panel.SetActive(false);
                yes_selectitem_kettei.onclick = false;

                switch(GameMgr.stage_number)
                {
                    case 1:

                        GameMgr.stage1_girl1_loveexp = girl1_status.girl1_Love_exp;
                        FadeManager.Instance.LoadScene("002_Stage2_eyecatch", 0.3f);
                        break;

                    case 2:

                        GameMgr.stage2_girl1_loveexp = girl1_status.girl1_Love_exp;
                        FadeManager.Instance.LoadScene("003_Stage3_eyecatch", 0.3f);
                        break;

                    case 3:

                        GameMgr.stage3_girl1_loveexp = girl1_status.girl1_Love_exp;
                        FadeManager.Instance.LoadScene("100_Ending", 0.3f);
                        break;

                }
                

                break;

            case false:

                //Debug.Log("cancel");

                yes_no_clear_panel.SetActive(false);

                _text.text = "";
                compound_status = 0;

                //extreme_panel.LifeAnimeOnTrue();
                yes_selectitem_kettei.onclick = false;
                break;

        }
    }

    //好感度によって発生する、サブイベント達
    public void Check_GirlLoveEvent()
    {
        if (GirlLove_loading == true)
        {

        }
        else
        {
            switch (GameMgr.stage_number)
            {
                //ステージ１のサブイベント
                case 1:

                    if (girl1_status.girl1_Love_exp >= 0 && girl1_status.girl1_Love_exp < 25)
                    {
                        check_GirlLoveEvent_flag = true;
                    }
                    else if (girl1_status.girl1_Love_exp >= 25)
                    {

                        if (GameMgr.GirlLoveEvent_01 != true)
                        {
                            GameMgr.GirlLoveEvent_num = 1;
                            GameMgr.GirlLoveEvent_01 = true;

                            //_text.text = "イベント１をON。" + "\n" + "お兄ちゃん。誰かお客さんがきたよ。";

                            StartCoroutine("ReadGirlLoveEvent");
                        }
                        else
                        {
                            check_GirlLoveEvent_flag = true;
                        }
                    }
                    else
                    {
                        check_GirlLoveEvent_flag = true;
                    }
                    break;

                //ステージ２のサブイベント
                case 2:

                    check_GirlLoveEvent_flag = true;
                    break;

                //ステージ３のサブイベント
                case 3:

                    check_GirlLoveEvent_flag = true;
                    break;

                default:
                    break;

            }
        }
    }

    IEnumerator ReadGirlLoveEvent()
    {
        compoundselect_onoff_obj.SetActive(false);
        text_area.SetActive(false);
        Extremepanel_obj.SetActive(false);
        sceneBGM.MuteBGM();
        GirlLove_loading = true;

        GameMgr.girlloveevent_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.girlloveevent_endflag)
        {
            yield return null;
        }

        compoundselect_onoff_obj.SetActive(true);
        text_area.SetActive(true);
        Extremepanel_obj.SetActive(true);
        sceneBGM.MuteOFFBGM();

        GirlLove_loading = false;

        _text.text = "";

        check_GirlLoveEvent_flag = true;
    }
}
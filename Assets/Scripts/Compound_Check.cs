using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Compound_Check : MonoBehaviour {

    private GameObject canvas;

    private CombinationMain Combinationmain;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject text_hikari_makecaption;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private Exp_Controller exp_Controller;

    private Buf_Power_Keisan bufpower_keisan;
    private int _buf_kakuritsu;

    private GameObject card_view_obj;
    private CardView card_view;

    private Girl1_status girl1_status;
    private GirlEat_Judge girleat_judge;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private MagicSkillListDataBase magicskill_database;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;
    private GameObject yes_no_panel_magic;

    private Sprite yes_sprite1;
    private Sprite yes_sprite2;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private GameObject updown_counter_oricompofinalcheck_obj;
    private Updown_counter updown_counter_oricompofinalcheck;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject costTimePanel_obj;
    private Text _cost_hourtext;
    private Text _cost_minutestext;
    private Text _cost_player_mptext;
    private Text _cost_mptext;
    private int costMP;

    private GameObject resultitemName_obj;

    private GameObject FinalCheckPanel;
    private Text FinalCheck_Text;
    private string final_itemmes;

    private int _costTime, _hour, _minutes;

    private GameObject compoBG_A;

    private GameObject BlackImage;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<string> _itemSubtypeB_temp_result = new List<string>(); //調合DBのサブタイプBの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。
    private int inputcount;

    private bool compoDB_select_judge;
    private bool hikari_nomake;
    private string resultitemID;
    private int result_compoID;
    private bool resultDB_Failed;
    private List<int> result_kosuset = new List<int>();

    private string success_text;
    private float _success_rate;
    private float _ex_probabilty_temp;
    private int dice;
    private bool _debug_sw;

    private int baseitemID;
    private int itemID_1;
    private int itemID_2;
    private int itemID_3;

    private GameObject memo_result_obj;
    private GameObject recipiMemoButton_obj;
    private GameObject recipiMemoScrollView_obj;
    private GameObject MagicSelectLv_Panel;

    private int i;
    private int _rate, _debug_beforerate;
    private int _releaseID;
    private bool newrecipi_flag;

    private GameObject finalcheck_Prefab; //調合最終チェック用のアイテムプレファブ
    private GameObject finalcheck_Prefab2; //間の掛け算表記「×」
    private GameObject resultitem_Hyouji;
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    private List<GameObject> _listitem = new List<GameObject>();
    private int list_count;
    private Sprite texture2d;

    private string magicName;
    private int magicLearnLv;
    private int _magic_rate;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //コンポBGパネルの取得
        compoBG_A = this.transform.parent.gameObject;

        //確率パネルの取得
        kakuritsuPanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        costTimePanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/CostTimePanel").gameObject;
        _cost_hourtext = costTimePanel_obj.transform.Find("Image/TimeHour_param").GetComponent<Text>();
        _cost_minutestext = costTimePanel_obj.transform.Find("Image/TimeMinutes_param").GetComponent<Text>();

        resultitemName_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/TextPanel/Image/Result_item/NameText").gameObject;
        MagicSelectLv_Panel = compoBG_A.transform.Find("MagicStartPanel/magicComp2/MagicSelectLv_Panel").gameObject;
        MagicSelectLv_Panel.SetActive(false);

        FinalCheckPanel = compoBG_A.transform.Find("FinalCheckPanel").gameObject;
        FinalCheck_Text = FinalCheckPanel.transform.Find("Comp/KakuritsuMessage/Image/Text").GetComponent<Text>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子
        girleat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //調合用メソッドの取得
        Combinationmain = CombinationMain.Instance.GetComponent<CombinationMain>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //テキストウィンドウの取得
        text_area = compoBG_A.transform.Find("MessageWindowComp").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        text_hikari_makecaption = text_area.transform.Find("HikariMakeOkashiCaptionText").gameObject;       

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = FinalCheckPanel.transform.Find("Comp/TextPanel/Image/Scroll View/Viewport/Content").gameObject;
        finalcheck_Prefab = (GameObject)Resources.Load("Prefabs/finalcheck_item");
        finalcheck_Prefab2 = (GameObject)Resources.Load("Prefabs/finalcheck_kakeru");
        resultitem_Hyouji = FinalCheckPanel.transform.Find("Comp/TextPanel/Image/Result_item").gameObject;

        memo_result_obj = compoBG_A.transform.Find("Memo_Result").gameObject;
        recipiMemoButton_obj = compoBG_A.transform.Find("RecipiMemoButton").gameObject;
        recipiMemoScrollView_obj = compoBG_A.transform.Find("RecipiMemo_ScrollView").gameObject;

        yes_no_panel_magic = compoBG_A.transform.Find("MagicStartPanel/Yes_no_Panel_Finalcheck").gameObject;
        yes_no_panel_magic.SetActive(false);

        _debug_sw = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(pitemlistController_obj == null)
        {
            //プレイヤーアイテムリストの取得。以下のオブジェクトは、「CompoundMain」オブジェクトで初期化しているので、Start()でやると処理の順序の関係でバグる。
            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
            updown_counter_oricompofinalcheck_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/updown_counter").gameObject; //オリジナル調合の場合、参照先が異なる
            updown_counter_oricompofinalcheck = updown_counter_oricompofinalcheck_obj.GetComponent<Updown_counter>();

            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;

            yes_sprite1 = Resources.Load<Sprite>("Sprites/Window/miniwindowB");
            yes_sprite2 = Resources.Load<Sprite>("Sprites/Window/sabwindowA_pink_66");

        }

        if (GameMgr.final_select_flag == true) //最後、これで調合するかどうかを待つフラグ
        {
            if (GameMgr.compound_select == 1) //レシピ調合のときの処理
            {
                GameMgr.compound_status = 110;

                SelectPaused();

                GameMgr.final_select_flag = false;
                resultitemName_obj.SetActive(true);
                GameMgr.Extreme_On = false;

                //確率パネルの取得・参照先を指定
                kakuritsuPanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                costTimePanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/CostTimePanel").gameObject;
                _cost_hourtext = costTimePanel_obj.transform.Find("Image/TimeHour_param").GetComponent<Text>();
                _cost_minutestext = costTimePanel_obj.transform.Find("Image/TimeMinutes_param").GetComponent<Text>();

                StartCoroutine("recipiFinal_select");

            }

            if (GameMgr.compound_select == 2) //トッピング調合のときの処理
            {

                GameMgr.compound_status = 110;

                SelectPaused();

                GameMgr.final_select_flag = false;
                resultitemName_obj.SetActive(true);
                GameMgr.Extreme_On = false;

                //確率パネルの取得・参照先を指定
                kakuritsuPanel_obj = compoBG_A.transform.Find("ExtremeImage/KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();
                kakuritsuPanel_obj.SetActive(true);

                costTimePanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/CostTimePanel").gameObject;
                _cost_hourtext = costTimePanel_obj.transform.Find("Image/TimeHour_param").GetComponent<Text>();
                _cost_minutestext = costTimePanel_obj.transform.Find("Image/TimeMinutes_param").GetComponent<Text>();

                StartCoroutine("topping_Final_select");

            }

            if (GameMgr.compound_select == 3) //オリジナル調合のときの処理
            {

                GameMgr.compound_status = 110;

                SelectPaused();

                GameMgr.final_select_flag = false;
                resultitemName_obj.SetActive(true);
                GameMgr.Extreme_On = false;

                FinalCheckPanel.SetActive(true);
                yes.GetComponent<Button>().interactable = false;
                no.GetComponent<Button>().interactable = false;

                //確率パネルの取得・参照先を指定
                kakuritsuPanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                costTimePanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/CostTimePanel").gameObject;
                _cost_hourtext = costTimePanel_obj.transform.Find("Image/TimeHour_param").GetComponent<Text>();
                _cost_minutestext = costTimePanel_obj.transform.Find("Image/TimeMinutes_param").GetComponent<Text>();

                //一度contentの中身を削除
                foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
                {
                    Destroy(child.gameObject);
                }
                list_count = 0;
                _listitem.Clear();

                StartCoroutine("Final_select");

            }

            if (GameMgr.compound_select == 7) //ヒカリに作らせるのときの処理
            {

                GameMgr.compound_status = 110;

                SelectPaused();

                GameMgr.final_select_flag = false;
                resultitemName_obj.SetActive(true);
                GameMgr.Extreme_On = false;

                FinalCheckPanel.SetActive(true);
                yes.GetComponent<Button>().interactable = false;
                no.GetComponent<Button>().interactable = false;
                text_hikari_makecaption.SetActive(false);

                //確率パネルの取得・参照先を指定
                kakuritsuPanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                costTimePanel_obj = compoBG_A.transform.Find("FinalCheckPanel/Comp/CostTimePanel").gameObject;
                _cost_hourtext = costTimePanel_obj.transform.Find("Image/TimeHour_param").GetComponent<Text>();
                _cost_minutestext = costTimePanel_obj.transform.Find("Image/TimeMinutes_param").GetComponent<Text>();

                //一度contentの中身を削除
                foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
                {
                    Destroy(child.gameObject);
                }
                list_count = 0;
                _listitem.Clear();

                StartCoroutine("Final_select");

            }

            if (GameMgr.compound_select == 21) //魔法調合のときの処理
            {

                GameMgr.compound_status = 110;

                SelectPaused();

                GameMgr.final_select_flag = false;
                GameMgr.Extreme_On = false;

                //確率パネルの取得・参照先を指定
                kakuritsuPanel_obj = compoBG_A.transform.Find("MagicStartPanel/magicComp2/MagicSelectLv_Panel/KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                costTimePanel_obj = compoBG_A.transform.Find("MagicStartPanel/magicComp2/MagicSelectLv_Panel/CostTimePanel").gameObject;
                _cost_hourtext = costTimePanel_obj.transform.Find("Image/TimeHour_param").GetComponent<Text>();
                _cost_minutestext = costTimePanel_obj.transform.Find("Image/TimeMinutes_param").GetComponent<Text>();
                _cost_player_mptext = costTimePanel_obj.transform.Find("Image/PlayerMP_param").GetComponent<Text>();
                _cost_mptext = costTimePanel_obj.transform.Find("Image/CostMP_param").GetComponent<Text>();

                StartCoroutine("MagicFinal_select"); //最終確認 スキルレベルを選択する。
                //MagicFinal_select();
            }
        }
        
        
    }

    IEnumerator Final_select()
    {
        //*** 2個or3個選んだ状態で、最後、これでOKかどうか聞くメソッド　***//

        switch (GameMgr.Comp_kettei_bunki)
        {
            case 2: //2個選択しているとき

                itemID_1 = GameMgr.temp_itemID1;
                itemID_2 = GameMgr.temp_itemID2;

                GameMgr.temp_itemID3 = 9999; //9999は空を表す数字
                itemID_3 = GameMgr.temp_itemID3;

                card_view.OKCard_DrawView02(GameMgr.Final_kettei_kosu2);

                CompoundJudge(itemID_1, itemID_2, itemID_3, 0); //調合の判定・確率処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。

                recipiMemoScrollView_obj.SetActive(false);
                memo_result_obj.SetActive(false);

                //確率に応じて、テキストが変わる。
                FinalCheck_Text.text = success_text;

                //選んだアイテムを表示する。リザルトアイテムも表示する。
                FinalCheck_ItemIconHyouji(0); //2個表示のとき

                if (GameMgr.compound_select == 3)
                {
                    _text.text = final_itemmes + "\n" + "作る？";
                }
                else if (GameMgr.compound_select == 7)
                {
                    _text.text = final_itemmes + "\n" + "このお菓子を作ってもらう？";
                }

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                FinalCheckPanel.SetActive(false);
                yes.GetComponent<Button>().interactable = true;
                no.GetComponent<Button>().interactable = true;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        if (GameMgr.compound_select == 3)
                        {
                            //選んだ二つをもとに、一つのアイテムを生成する。そして、調合完了！

                            //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.result_ok = true; //調合完了のフラグをたてておく。

                            if (updown_counter_oricompofinalcheck_obj.activeInHierarchy)
                            {
                                exp_Controller.set_kaisu = GameMgr.updown_kosu; //何セット作るかの個数もいれる。
                            }
                            else
                            {
                                exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                            }

                            exp_Controller.result_kosuset.Clear();
                            for (i = 0; i < result_kosuset.Count; i++)
                            {
                                exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                            }

                            GameMgr.compound_status = 4;

                            card_view.CardCompo_Anim();
                            Off_Flag_Setting();

                            exp_Controller.ResultOK();

                        }
                        else if (GameMgr.compound_select == 7)
                        {
                            //ヒカリに作ってもらう。材料の決定

                            exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                            /*if (updown_counter_oricompofinalcheck_obj.activeInHierarchy)
                            {
                                exp_Controller.set_kaisu = updown_counter_oricompofinalcheck.updown_kosu; //何セット作るかの個数もいれる。
                            }
                            else
                            {
                                exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                            }*/

                            exp_Controller.result_kosuset.Clear();
                            for (i = 0; i < result_kosuset.Count; i++)
                            {
                                exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                            }

                            GameMgr.compound_status = 4;

                            //card_view.CardCompo_Anim();
                            Off_Flag_Setting();

                            exp_Controller.HikariMakeOK();
                        }

                        //温度管理ONにしてたら、画面もオフにする。
                        if (GameMgr.tempature_control_ON)
                        {
                            GameMgr.tempature_control_Offflag = true;
                        }

                        break;

                    case false:

                        if (magicskill_database.skillName_SearchLearnLevel("Temperature_of_Control") >= 1)
                        {
                            if (GameMgr.tempature_control_ON)
                            {
                                Debug.Log("温度管理画面を表示する");

                                GameMgr.tempature_control_select_flag = true;
                            }
                            else
                            {
                                cancel_Method1();
                            }
                        }
                        else
                        {
                            cancel_Method1();
                        }

                        break;
                }
                break;

            case 3: //3個選択しているとき

                itemID_1 = GameMgr.temp_itemID1;
                itemID_2 = GameMgr.temp_itemID2;
                itemID_3 = GameMgr.temp_itemID3;

                card_view.OKCard_DrawView03(GameMgr.Final_kettei_kosu3);

                CompoundJudge(itemID_1, itemID_2, itemID_3, 0); //調合の処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。

                recipiMemoScrollView_obj.SetActive(false);
                memo_result_obj.SetActive(false);

                //確率に応じて、テキストが変わる。
                FinalCheck_Text.text = success_text;

                //選んだアイテムを表示する。リザルトアイテムも表示する。
                FinalCheck_ItemIconHyouji(1); //3個表示のとき

                if (GameMgr.compound_select == 3)
                {
                    _text.text = final_itemmes + "\n" + "作る？";
                }
                else if (GameMgr.compound_select == 7)
                {
                    _text.text = final_itemmes + "\n" + "このお菓子を作ってもらう？";
                }

                //Debug.Log(database.items[itemID_1].itemNameHyouji + "と" + database.items[itemID_2].itemNameHyouji + "と" + database.items[itemID_3].itemNameHyouji + "でいいですか？");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                FinalCheckPanel.SetActive(false);
                yes.GetComponent<Button>().interactable = true;
                no.GetComponent<Button>().interactable = true;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        if (GameMgr.compound_select == 3)
                        {
                            //選んだ三つをもとに、一つのアイテムを生成する。

                            //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.result_ok = true; //オリジナル調合完了のフラグをたてておく。

                            exp_Controller.set_kaisu = GameMgr.updown_kosu; //何セット作るかの個数もいれる。

                            exp_Controller.result_kosuset.Clear();
                            for (i = 0; i < result_kosuset.Count; i++)
                            {
                                exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                            }

                            GameMgr.compound_status = 4;

                            //card_view.DeleteCard_DrawView();
                            card_view.CardCompo_Anim();
                            Off_Flag_Setting();

                            exp_Controller.ResultOK();

                        }
                        else if (GameMgr.compound_select == 7)
                        {
                            //ヒカリに作ってもらう。材料の決定                            

                            exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                            /*if (updown_counter_oricompofinalcheck_obj.activeInHierarchy)
                            {
                                exp_Controller.set_kaisu = updown_counter_oricompofinalcheck.updown_kosu; //何セット作るかの個数もいれる。
                            }
                            else
                            {
                                exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                            }*/

                            exp_Controller.result_kosuset.Clear();
                            for (i = 0; i < result_kosuset.Count; i++)
                            {
                                exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                            }

                            GameMgr.compound_status = 4;

                            //card_view.CardCompo_Anim();
                            Off_Flag_Setting();

                            exp_Controller.HikariMakeOK();
                        }

                        //温度管理ONにしてたら、画面もオフにする。
                        if (GameMgr.tempature_control_ON)
                        {
                            GameMgr.tempature_control_Offflag = true;
                        }

                        break;

                    case false:

                        if (magicskill_database.skillName_SearchLearnLevel("Temperature_of_Control") >= 1)
                        {
                            if (GameMgr.tempature_control_ON)
                            {
                                Debug.Log("温度管理画面を表示する");

                                GameMgr.tempature_control_select_flag = true;
                            }
                            else
                            {
                                cancel_Method2();
                            }
                        }
                        else
                        {
                            cancel_Method2();
                            
                        }

                        break;
                }
                break;
        }
    }

    void cancel_Method1()
    {
        //Debug.Log("1個目を選択した状態に戻る");

        recipiMemoButton_obj.SetActive(true);
        GameMgr.compound_status = 100;
        itemselect_cancel.Two_cancel();

        if (GameMgr.compound_select == 7)
        {
            text_hikari_makecaption.SetActive(true);
        }
    }

    void cancel_Method2()
    {
        //Debug.Log("三個目はcancel");

        recipiMemoButton_obj.SetActive(true);

        GameMgr.compound_status = 100;
        itemselect_cancel.Three_cancel();

        yes.SetActive(true);
        yes_text.text = "作る";
        YesSetDesign2();

        if (GameMgr.compound_select == 7)
        {
            text_hikari_makecaption.SetActive(true);
        }
    }
    

    IEnumerator topping_Final_select()
    {
        //*** 1個or2個or3個選んだ状態で、最後、これでOKかどうか聞くメソッド　***//

        switch (GameMgr.Comp_kettei_bunki)
        {
            case 11: //べーすあいてむ + 1個選択しているとき

                itemID_1 = GameMgr.temp_itemID1;
                baseitemID = GameMgr.temp_baseitemID;

                GameMgr.temp_itemID2 = 9999; //9999は空を表す数字                
                GameMgr.temp_itemID3 = 9999; //9999は空を表す数字
                itemID_2 = GameMgr.temp_itemID2;
                itemID_3 = GameMgr.temp_itemID3;

                card_view.OKCard_DrawView02(1);

                CompoundJudge(itemID_1, itemID_2, itemID_3, baseitemID); //エクストリーム調合で、新規作成されるアイテムがないかをチェック。ない場合は、通常通りトッピング。ある場合は、新規作成する。

                _text.text = "ベースアイテム: " + database.items[baseitemID].itemNameHyouji + "に" + "\n" + "一個目: " + 
                    database.items[itemID_1].itemNameHyouji + " " +
                    GameMgr.Final_kettei_kosu1 + "個" + "をトッピングします。" + "\n" + "　トッピングしますか？";

                Debug.Log("ベースアイテム＋一個トッピング　調合確認中");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                kakuritsuPanel_obj.SetActive(false);
                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        GameMgr.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();
                        
                        //エクストリーム調合で、コンポDBに合致する新しいアイテムが生成される場合は、新規調合に変える。それ以外は、通常通りトッピング
                        if (compoDB_select_judge == true)
                        {
                            GameMgr.Extreme_On = true;

                            //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.ResultOK();
                            Debug.Log("トッピング時だが、新しいお菓子が生成されるパターン");
                        }
                        else
                        {
                            GameMgr.Extreme_On = false;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.Topping_Result_OK();
                        }

                        break;

                    case false:

                        //Debug.Log("ベースアイテムを選択した状態に戻る");
                        GameMgr.compound_status = 100;

                        exp_Controller._success_rate = 100;
                        kakuritsuPanel.KakuritsuYosoku_Reset();
                        itemselect_cancel.Two_cancel();

                        break;
                }
                break;

            case 12: //べーすあいてむ + 2個選択しているとき

                itemID_1 = GameMgr.temp_itemID1;
                itemID_2 = GameMgr.temp_itemID2;
                baseitemID = GameMgr.temp_baseitemID;

                GameMgr.temp_itemID3 = 9999; //9999は空を表す数字
                itemID_3 = GameMgr.temp_itemID3;

                card_view.OKCard_DrawView03(1);

                CompoundJudge(itemID_1, itemID_2, itemID_3, baseitemID); //エクストリーム調合で、新規作成されるアイテムがないかをチェック。ある場合は、そのレシピを閃く。

                _text.text = "ベースアイテム: " + database.items[baseitemID].itemNameHyouji + "に" + "\n" + 
                    "一個目: " + database.items[itemID_1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" + 
                    "二個目：" + database.items[itemID_2].itemNameHyouji + " " + GameMgr.Final_kettei_kosu2 + "個" + "\n" + 
                    "　トッピングしますか？";

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                kakuritsuPanel_obj.SetActive(false);
                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        GameMgr.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();


                        //エクストリーム調合で、コンポDBに合致する新しいアイテムが生成される場合は、新規調合に変える。それ以外は、通常通りトッピング
                        if (compoDB_select_judge == true)
                        {
                            GameMgr.Extreme_On = true;

                            //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.ResultOK();
                            Debug.Log("トッピング時だが、新しいお菓子が生成されるパターン");
                        }
                        else
                        {
                            GameMgr.Extreme_On = false;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.Topping_Result_OK();
                        }

                        break;

                    case false:

                        //Debug.Log("1個目を選択した状態に戻る");
                        GameMgr.compound_status = 100;

                        exp_Controller._success_rate = exp_Controller._temp_srate_1;
                        kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_1);
                        itemselect_cancel.Three_cancel();

                        break;
                }
                break;

            case 13: //べーすあいてむ + 3個選択しているとき

                itemID_1 = GameMgr.temp_itemID1;
                itemID_2 = GameMgr.temp_itemID2;
                itemID_3 = GameMgr.temp_itemID3;
                baseitemID = GameMgr.temp_baseitemID;

                card_view.OKCard_DrawView04();

                _text.text = "ベースアイテム: " + database.items[baseitemID].itemNameHyouji + "に" + "\n" + 
                    "一個目: " + database.items[itemID_1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" + 
                    "二個目：" + database.items[itemID_2].itemNameHyouji + " " + GameMgr.Final_kettei_kosu2 + "個" + "\n" + 
                    "三個目：" + database.items[itemID_3].itemNameHyouji + " " + GameMgr.Final_kettei_kosu3 + "個" + 
                    "　トッピングしますか？";

                //Debug.Log(database.items[itemID_1].itemNameHyouji + "と" + database.items[itemID_2].itemNameHyouji + "と" + database.items[itemID_3].itemNameHyouji + "でいいですか？");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                kakuritsuPanel_obj.SetActive(false);
                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //選んだ三つをもとに、一つのアイテムを生成する。

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                        GameMgr.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        exp_Controller.Topping_Result_OK();

                        break;

                    case false:

                        //Debug.Log("2個目を選択した状態に戻る");
                        GameMgr.compound_status = 100;

                        exp_Controller._success_rate = exp_Controller._temp_srate_2;
                        kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_2);
                        itemselect_cancel.Four_cancel();

                        break;
                }
                break;
        }
    }

    IEnumerator recipiFinal_select()
    {
        itemID_1 = GameMgr.temp_itemID1;
        itemID_2 = GameMgr.temp_itemID2;
        itemID_3 = GameMgr.temp_itemID3;

        CompoundJudge(itemID_1, itemID_2, itemID_3, 0); //食材の距離計算も行う。

        _text.text = database.items[GameMgr.Final_result_itemID1].itemNameHyouji + "が" +
            databaseCompo.compoitems[GameMgr.Final_result_compID].cmpitem_result_kosu * GameMgr.Final_setCount + 
            "個　出来ます。" + "\n" + "作る？" + "\n" + "成功確率: " + _success_rate + "％";
       

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除
                for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
                {
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = true;
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().isOn = false;
                }

                recipilistController.final_recipiselect_flag = false;

                exp_Controller.recipiresult_ok = true; //レシピ調合完了のフラグ。これがONになったら、アイテムリストを更新する。

                itemselect_cancel.kettei_on_waiting = false;

                yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                //card_view.DeleteCard_DrawView();
                card_view.CardCompo_Anim();

                GameMgr.compound_status = 4;

                exp_Controller.Recipi_ResultOK();

                Debug.Log("選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                recipilistController.final_recipiselect_flag = false;

                for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
                {
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = true;
                    recipilistController._recipi_listitem[i].GetComponent<Toggle>().isOn = false;
                }

                recipilistController.Redraw_OkashiBook();
                recipilistController.BlackImageOFF();

                GameMgr.compound_status = 100;
                itemselect_cancel.All_cancel();

                break;
        }
    }


    /* 魔法調合時の調合決定処理 */

    IEnumerator MagicFinal_select()
    {
        //*** 魔法調合時これでOKかどうか聞くメソッド　***//
        yes.GetComponent<Button>().interactable = false;
        no.GetComponent<Button>().interactable = false;

        switch (GameMgr.Comp_kettei_bunki)
        {
            case 20: //1個選択しているとき

                itemID_1 = GameMgr.temp_itemID1;
                itemID_2 = magicskill_database.SearchSkillString(GameMgr.UseMagicSkill);

                GameMgr.temp_itemID3 = 9999; //9999は空を表す数字
                itemID_3 = GameMgr.temp_itemID3;

                if (magicskill_database.magicskill_lists[itemID_2].skill_LvSelect == "Non" ||
                    magicskill_database.magicskill_lists[itemID_2].skill_LvSelect == "CompNo")
                {
                    //常に習得レベルで固定する扱いになるので、判定では使用しない。
                    GameMgr.UseMagicSkillLv = magicskill_database.magicskill_lists[itemID_2].skillLv;
                }
                else if (magicskill_database.magicskill_lists[itemID_2].skill_LvSelect == "Use")//[USE]が入っている時
                {
                    magicskill_database.magicskill_lists[itemID_2].skillUseLv = 1; //
                    GameMgr.UseMagicSkillLv = 1;
                }

                if (magicskill_database.magicskill_lists[itemID_2].skill_LvSelect == "CompNo")
                {
                    //調合DBの判定が必要ない魔法の場合　元アイテムをresultItemにして、新たに生成しなおす。
                    CompoundJudge(itemID_1, itemID_2, itemID_3, 0); //調合の判定・確率処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。GameMgr.Comp_kettei_bunki=20で判定
                    GameMgr.Comp_kettei_bunki = 22;
                }
                else
                {
                    CompoundJudge(itemID_1, itemID_2, itemID_3, 0); //調合の判定・確率処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。GameMgr.Comp_kettei_bunki=20で判定
                    GameMgr.Comp_kettei_bunki = 21;
                }

                MagicSelectLv_Panel.SetActive(true);
                MagicSelectLv_Panel.transform.Find("MagicSkillNameImg/Text").GetComponent<Text>().text = GameMgr.UseMagicSkill_nameHyouji;
                //魔法のときは、対象アイテムと魔法のエフェクトなどを表示する

                recipiMemoScrollView_obj.SetActive(false);
                memo_result_obj.SetActive(false);
                compoBG_A.GetComponent<Compound_BGPanel_A>().BlackImageON();

                //確率に応じて、テキストが変わる。
                //FinalCheck_Text.text = success_text;

                
                _text.text = "魔法のレベルを選択してね。";
                //_text.text = "魔法のレベルを選択してね。" + "\n" + "（魔法によっては、固定されているものもあります。）";
                updown_counter_obj.SetActive(true);
                yes_no_panel_magic.SetActive(true);

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {
                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                yes_no_panel_magic.SetActive(false);
                MagicSelectLv_Panel.SetActive(false);
                compoBG_A.GetComponent<Compound_BGPanel_A>().BlackImageOFF();
                yes.GetComponent<Button>().interactable = true;
                no.GetComponent<Button>().interactable = true;

                //選んだ二つをもとに、一つのアイテムを生成する。そして、調合完了！
                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        magicskill_database.magicskill_lists[itemID_2].skillUseLv = GameMgr.UseMagicSkillLv; //使ったスキルレベルで魔法DBのUSELVも更新

                        CompoundJudge(itemID_1, itemID_2, itemID_3, 0); //魔法レベルが決定したあと、再び調合判定。

                        //MPを消費
                        PlayerStatus.player_mp -= costMP;

                        //魔法によって、仕上げ回数も消費する。
                        if (magicskill_database.magicskill_lists[itemID_2].skill_LvSelect == "CompNo")
                        {
                            GameMgr.Extreme_On = true;
                            //CompNoのお菓子は、仕上げ回数が減る
                        }
                        else
                        {
                            GameMgr.Extreme_On = false;
                        }

                        //魔法によって、ハートも消費する。さらに、演出時間もここで決定
                        switch (GameMgr.UseMagicSkill)
                        {
                            case "Cookie_SecondBake":

                                GameMgr.System_magic_playtime = GameMgr.System_magic_playtime_def01;
                                break;

                            case "Warming_Handmade":

                                girleat_judge.UpDegHeart(-(GameMgr.UseMagicSkillLv * 30), false); //ハートを消費するパターン;
                                                                                                  //PlayerStatus.girl1_Love_exp -= GameMgr.UseMagicSkillLv * 30;
                                break;

                            default:

                                GameMgr.System_magic_playtime = GameMgr.System_magic_playtime_default;
                                break;
                        }

                        //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.magic_result_ok = true; //調合完了のフラグをたてておく。
                      

                        exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                        /*if (updown_counter_oricompofinalcheck_obj.activeInHierarchy)
                        {
                            exp_Controller.set_kaisu = GameMgr.updown_kosu; //何セット作るかの個数もいれる。
                        }
                        else
                        {
                            exp_Controller.set_kaisu = 1; //updownカウンター使っていない仕様のときは1でリセット
                        }*/

                        exp_Controller.result_kosuset.Clear();
                        for (i = 0; i < result_kosuset.Count; i++)
                        {
                            exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                        }

                        GameMgr.compound_status = 22;

                        //card_view.CardCompo_Anim();
                        card_view.DeleteCard_DrawView();
                        Off_Flag_Setting();

                        exp_Controller.MagicResultOK();

                        break;

                    case false:

                        //Debug.Log("1個目を選択した状態に戻る");

                        GameMgr.compound_status = 100;
                        itemselect_cancel.All_cancel();

                        break;
                }
                              
                break;
        }
    }

    public void YesSetDesignDefault()
    {
        yes_text.color = new Color(56f / 255f, 56f / 255f, 36f / 255f); //焦げ茶文字
        yes.GetComponent<Image>().sprite = yes_sprite1;
    }

    public void YesSetDesign2()
    {
        yes_text.color = new Color(255f / 255f, 255f / 255f, 255f / 255f); //白文字
        yes.GetComponent<Image>().sprite = yes_sprite2;
    }

    //調合判定メソッド　itemSelectToggleからも読まれる。
    public void CompoundJudge(int tempID_1, int tempID_2, int tempID_3, int basetemp_ID)
    {
        _itemIDtemp_result.Clear();
        _itemKosutemp_result.Clear();
        _itemSubtype_temp_result.Clear();
        _itemSubtypeB_temp_result.Clear();
        _ex_probabilty_temp = 1.0f;

        //オリジナル調合の場合はこっち
        if (GameMgr.Comp_kettei_bunki == 2 || GameMgr.Comp_kettei_bunki == 3)
        {
            _itemIDtemp_result.Add(database.items[tempID_1].itemName);
            _itemIDtemp_result.Add(database.items[tempID_2].itemName);

            _itemSubtype_temp_result.Add(database.items[tempID_1].itemType_sub.ToString());
            _itemSubtype_temp_result.Add(database.items[tempID_2].itemType_sub.ToString());

            _itemSubtypeB_temp_result.Add(database.items[tempID_1].itemType_subB.ToString());
            _itemSubtypeB_temp_result.Add(database.items[tempID_2].itemType_subB.ToString());

            _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu1);
            _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu2);

            if (tempID_3 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                _itemSubtypeB_temp_result.Add("empty");
                GameMgr.Final_kettei_kosu3 = 9999; //個数にも9999=emptyを入れる。
                _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu3);

                //アイテムごとの確率補正値を、先にここで計算
                _ex_probabilty_temp = database.items[tempID_1].Ex_Probability *
                database.items[tempID_2].Ex_Probability;

                inputcount = 2;
            }
            else
            {
                _itemIDtemp_result.Add(database.items[tempID_3].itemName);
                _itemSubtype_temp_result.Add(database.items[tempID_3].itemType_sub.ToString());
                _itemSubtypeB_temp_result.Add(database.items[tempID_3].itemType_subB.ToString());
                _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu3);

                //アイテムごとの確率補正値を、先にここで計算
                _ex_probabilty_temp = database.items[tempID_1].Ex_Probability *
                database.items[tempID_2].Ex_Probability *
                database.items[tempID_3].Ex_Probability;

                inputcount = 3;
            }

        }

        //エクストリーム調合の場合は、こっち。ベース決定アイテムを、temp_resultに入れる。
        else if (GameMgr.Comp_kettei_bunki == 11 || GameMgr.Comp_kettei_bunki == 12)
        {
            _itemIDtemp_result.Add(database.items[basetemp_ID].itemName);
            _itemIDtemp_result.Add(database.items[tempID_1].itemName);

            _itemSubtype_temp_result.Add(database.items[basetemp_ID].itemType_sub.ToString());
            _itemSubtype_temp_result.Add(database.items[tempID_1].itemType_sub.ToString());

            _itemSubtypeB_temp_result.Add(database.items[basetemp_ID].itemType_subB.ToString());
            _itemSubtypeB_temp_result.Add(database.items[tempID_1].itemType_subB.ToString());

            _itemKosutemp_result.Add(1);
            //Debug.Log("pitemlistController.final_kettei_kosu1: " + pitemlistController.final_kettei_kosu1);

            _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu1);

            if (tempID_2 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                _itemSubtypeB_temp_result.Add("empty");
                GameMgr.Final_kettei_kosu2 = 9999; //個数にも9999=emptyを入れる。
                _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu2);

                //アイテムごとの確率補正値を、先にここで計算
                _ex_probabilty_temp = database.items[basetemp_ID].Ex_Probability *
                database.items[tempID_1].Ex_Probability;

                inputcount = 2;
            }
            else
            {
                _itemIDtemp_result.Add(database.items[tempID_2].itemName);
                _itemSubtype_temp_result.Add(database.items[tempID_2].itemType_sub.ToString());
                _itemSubtypeB_temp_result.Add(database.items[tempID_2].itemType_subB.ToString());
                _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu2);

                //アイテムごとの確率補正値を、先にここで計算
                _ex_probabilty_temp = database.items[basetemp_ID].Ex_Probability *
                database.items[tempID_1].Ex_Probability *
                database.items[tempID_2].Ex_Probability;

                inputcount = 3;
            }
        }

        //魔法調合の場合はこっち
        if (GameMgr.Comp_kettei_bunki == 20 || GameMgr.Comp_kettei_bunki == 21 || GameMgr.Comp_kettei_bunki == 22)
        {
            _itemIDtemp_result.Add(database.items[tempID_1].itemName);

            magicName = magicskill_database.magicskill_lists[tempID_2].skillName;
            magicLearnLv = magicskill_database.magicskill_lists[tempID_2].skillLv;
            costMP = magicskill_database.magicskill_lists[tempID_2].skillCost;

            //消費MPも表示
            _cost_player_mptext.text = PlayerStatus.player_mp.ToString() + " / " + PlayerStatus.player_maxmp.ToString();
            _cost_mptext.text = costMP.ToString();

            if (magicskill_database.magicskill_lists[tempID_2].skill_LvSelect == "Non" ||
                magicskill_database.magicskill_lists[tempID_2].skill_LvSelect == "CompNo")
            {
                _itemIDtemp_result.Add(magicskill_database.magicskill_lists[tempID_2].skillName);
                Debug.Log("魔法名とLV: " + magicskill_database.magicskill_lists[tempID_2].skillName);
            }
            //USEは、使用時に魔法のレベルを選択できるモード　パラメータ数が膨大になるので、今回は見送り
            /*else if (magicskill_database.magicskill_lists[tempID_2].skill_LvSelect == "Use")//[USE]が入っている時
            {
                _itemIDtemp_result.Add(magicskill_database.magicskill_lists[tempID_2].skillName + GameMgr.UseMagicSkillLv);
                Debug.Log("魔法名とLV: " + magicskill_database.magicskill_lists[tempID_2].skillName + GameMgr.UseMagicSkillLv);
            }*/


            _itemSubtype_temp_result.Add(database.items[tempID_1].itemType_sub.ToString());
            _itemSubtype_temp_result.Add("empty");

            _itemSubtypeB_temp_result.Add(database.items[tempID_1].itemType_subB.ToString());
            _itemSubtypeB_temp_result.Add("empty");

            _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu1);
            _itemKosutemp_result.Add(1);


            if (tempID_3 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                _itemSubtypeB_temp_result.Add("empty");
                GameMgr.Final_kettei_kosu3 = 9999; //個数にも9999=emptyを入れる。
                _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu3);

                //アイテムごとの確率補正値を、先にここで計算
                _ex_probabilty_temp = database.items[tempID_1].Ex_Probability *
                (float)(magicskill_database.magicskill_lists[tempID_2].success_rate * 0.01);

                //魔法の場合、一度に作る個数が増えるほど、確率が10%ほど下がる
                _ex_probabilty_temp -= (float)(GameMgr.Final_kettei_kosu1 * 0.1);

                //Debug.Log("_ex_probabilty_temp: " + _ex_probabilty_temp);

                inputcount = 2;
            }
            else
            {
                _itemIDtemp_result.Add(database.items[tempID_3].itemName);
                _itemSubtype_temp_result.Add(database.items[tempID_3].itemType_sub.ToString());
                _itemSubtypeB_temp_result.Add(database.items[tempID_3].itemType_subB.ToString());
                _itemKosutemp_result.Add(GameMgr.Final_kettei_kosu3);

                //アイテムごとの確率補正値を、先にここで計算
                _ex_probabilty_temp = database.items[tempID_1].Ex_Probability *
                (float)(magicskill_database.magicskill_lists[tempID_2].success_rate * 0.01) *
                database.items[tempID_3].Ex_Probability;


                //魔法の場合、一度に作る個数が増えるほど、確率が10%ほど下がる
                _ex_probabilty_temp -= (float)(GameMgr.Final_kettei_kosu1 * 0.1);

                inputcount = 3;
            }
        }


        i = 0;

        resultitemID = "gomi_1"; //どの調合組み合わせのパターンにも合致しなかった場合は、ゴミのIDが入っている。調合DBのゴミのitemNameを入れると、後で数値に変換してくれる。現在は、500に変換される。
        compoDB_select_judge = false;
        resultDB_Failed = false;

        //判定処理//

        //新規作成のため、以下の判定処理を行う。個数は、判定に関係しない。

        Combinationmain.CombinationMain_Method(_itemIDtemp_result.ToArray(), _itemSubtype_temp_result.ToArray(), _itemSubtypeB_temp_result.ToArray(), _itemKosutemp_result.ToArray(), inputcount, 0);
        compoDB_select_judge = Combinationmain.compFlag;
        if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
        {
            if (Combinationmain.resultitemName == "Failed")
            {
                //CompoDBの組み合わせで失敗が指定されるものを引いた場合　その調合は失敗になる。

                resultitemID = "gomi_1";
                result_compoID = Combinationmain.result_compID;
                compoDB_select_judge = false;
                resultDB_Failed = true;
            }
            else
            {
                if (GameMgr.Comp_kettei_bunki == 22) //魔法で、調合DBのリザルトアイテムでなく、元アイテムを強化する場合
                {
                    resultitemID = database.items[tempID_1].itemName; //元アイテムを指定
                }
                else
                {
                    resultitemID = Combinationmain.resultitemName;
                }
                
                result_compoID = Combinationmain.result_compID;
                resultDB_Failed = false;

                result_kosuset.Clear();
                for (i = 0; i < Combinationmain.result_kosuset.Count; i++)
                {
                    result_kosuset.Add(Combinationmain.result_kosuset[i]); //そのときの個数の組み合わせ（CompoDBの左から順番になっている。）も記録。
                }
            }
        }


        if (compoDB_select_judge) //一致する場合
        {
            exp_Controller.DoubleItemCreated = 0; //2個以上のアイテムが同時に作られない場合、デフォルトは0。

            //特定のアイテム（卵白と卵黄など生成アイテムが2種類の場合）のcompIDを判定。2個以上生成アイテム。
            if (databaseCompo.compoitems[result_compoID].cmpitemID_result2 != "Non")
            {
                exp_Controller.DoubleItemCreated = 1;
            }
        }

        //stringのリザルドアイテムを、アイテムIDに変換。
        GameMgr.Final_result_itemID1 = database.SearchItemIDString(resultitemID);
        GameMgr.Final_result_compID = result_compoID;


        //制作時間の予想を表示
        _hour = 0;
        _minutes = 0;
        _costTime = databaseCompo.compoitems[GameMgr.Final_result_compID].cost_Time;
        while (_costTime >= 60)
        {
            _costTime = _costTime - 60;
            _hour++;
        }
        _minutes = _costTime * GameMgr.TimeStep; //1分刻み

        _cost_hourtext.text = _hour.ToString();
        _cost_minutestext.text = _minutes.ToString();



        //
        //

        if (compoDB_select_judge) //一致する場合
        {
            newrecipi_flag = false;
            hikari_nomake = false;

            //新しいレシピかどうか。           
            _releaseID = databaseCompo.SearchCompoIDString(databaseCompo.compoitems[result_compoID].release_recipi);
            if (databaseCompo.compoitems[_releaseID].cmpitem_flag == 0) //0なら新しいレシピ
            {
                if (GameMgr.compound_select == 7) //ヒカリに作らせる場合　兄がおぼえていないレシピは作れない。
                {
                    success_text = "にいちゃん～・・。このレシピ、見たことないよ～・・。";
                    exp_Controller._success_judge_flag = 2; //必ず失敗する
                    kakuritsuPanel.KakuritsuYosoku_Img(0);
                    exp_Controller.NewRecipiFlag = false;
                    newrecipi_flag = false;
                    compoDB_select_judge = false;
                    hikari_nomake = true;

                    resultitemID = "gomi_1";
                    //stringのリザルドアイテムを、アイテムIDに変換。
                    GameMgr.Final_result_itemID1 = database.SearchItemIDString(resultitemID);
                }
                else
                {
                    success_text = "新しいお菓子を思いつきそう..？";
                    newrecipi_flag = true;
                    exp_Controller.NewRecipiFlag = true;
                    //kakuritsuPanel.KakuritsuYosoku_NewImg(); //??にする。
                }
            }
            else
            {
                exp_Controller.NewRecipiFlag = false;
                newrecipi_flag = false;
            }

            //例外処理　二個以上同時にできる場合は、新しいレシピとしては登録されない
            if (exp_Controller.DoubleItemCreated == 1)
            {
                exp_Controller.NewRecipiFlag = false;
            }

            //調合判定
            //成功率の計算。コンポDBの、基本確率　＋　プレイヤーのレベル
            if (GameMgr.compound_select != 7)
            {
                if (GameMgr.compound_select != 2)
                {
                    _success_rate = Kakuritsu_Keisan(GameMgr.Final_result_compID);
                }
                else //トッピング調合の場合　新規レシピは確率計算するが、一度作ったことがあるか、特にレシピがない場合は100%
                {
                    if (newrecipi_flag)
                    {
                        _success_rate = Kakuritsu_Keisan(GameMgr.Final_result_compID);
                    }
                    else
                    {
                        _success_rate = 100f;
                    }
                }
            }
            else
            {
                //ヒカリが作る場合、成功率を事前に計算
                bufpower_keisan.hikariBuf_okashilv(database.items[GameMgr.Final_result_itemID1].itemType_sub.ToString()); //GameMgr.hikari_make_okashiTime_successrate_bufを事前計算
                _success_rate = Kakuritsu_Keisan(GameMgr.Final_result_compID);
            }
        }


        if (compoDB_select_judge == true)
        {
            if (!newrecipi_flag)
            {
                if (_success_rate >= 0.0 && _success_rate < 20.0)
                {
                    //成功率超低い
                    success_text = "絶望的。奇跡が起こるかも..？";
                }
                else if (_success_rate >= 20.0 && _success_rate < 40.0)
                {
                    //成功率低め
                    success_text = "たぶん、失敗しそう..。";
                }
                else if (_success_rate >= 40.0 && _success_rate < 60.0)
                {
                    //普通
                    success_text = "頑張れば、いける・・！？";
                }
                else if (_success_rate >= 60.0 && _success_rate < 80.0)
                {
                    //成功率高め
                    success_text = "ちょっと難しいかも..。";
                }
                else if (_success_rate >= 80.0 && _success_rate < 99.9)
                {
                    //成功率かなり高い
                    success_text = "これなら楽勝だね！";
                }
                else //100%~
                {
                    //１００％成功
                    success_text = "１００％成功する！！";
                }
            }


            //調合判定を行うかどうか+成功確率の表示更新


            //新規調合の場合　もしくは、　エクストリーム調合の場合。
            if (GameMgr.Comp_kettei_bunki == 2 || GameMgr.Comp_kettei_bunki == 3)
            {
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                exp_Controller._success_rate = _success_rate;
                kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
            }
            else if (GameMgr.Comp_kettei_bunki == 11 || GameMgr.Comp_kettei_bunki == 12)
            {
                //
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                exp_Controller._success_rate = _success_rate;
                kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
            }
            else if (GameMgr.Comp_kettei_bunki == 20 || GameMgr.Comp_kettei_bunki == 21 || GameMgr.Comp_kettei_bunki == 22)
            {
                //
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                if (!_debug_sw) //魔法のみ、決定時もう一度判定されるので、デバッグONのときは、100%成功にする。
                {
                    exp_Controller._success_rate = _success_rate;
                    kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
                }
                else
                {
                    exp_Controller._success_rate = 100;
                    kakuritsuPanel.KakuritsuYosoku_Img(100);
                    Debug.Log("最終成功率デバッグにより: " + "100%");
                }
            }
            

        }
        //どの調合リストにも当てはまらなかった場合
        else
        {
            //Debug.Log("どの調合リストにも当てはまらなかった。");            

            //エクストリーム調合の場合は、通常通りトッピングの処理を行う。
            if (GameMgr.Comp_kettei_bunki == 11 || GameMgr.Comp_kettei_bunki == 12)
            {
                if (resultDB_Failed) //ただし、DB上で、その組み合わせは失敗が指定されてる場合は、トッピングもできず、失敗になる。
                {
                    //失敗
                    exp_Controller._success_judge_flag = 2; //必ず失敗する
                    success_text = "これは.. 失敗かも？";
                    kakuritsuPanel.KakuritsuYosoku_Img(0);
                }
                else
                {
                    //トッピングは100％成功なので、exp_Controller._success_judge_flag や exp_Controller._success_rateの設定は不要　exp_Controllerで直接指定してる
                    _success_rate = 100f;                    
                    kakuritsuPanel.KakuritsuYosoku_Img(_success_rate); //ふつうにトッピングするときは、100%成功

                }
            }
            else
            {
                //失敗
                exp_Controller._success_judge_flag = 2; //必ず失敗する
                success_text = "これは.. 失敗かも？";
                kakuritsuPanel.KakuritsuYosoku_Img(0);
            }

        }

        //判定予測処理　ここまで//
    }



    void SelectPaused()
    {
        if (GameMgr.compound_select == 1) //レシピ調合のときの処理
        {
            for (i = 0; i < recipilistController._recipi_listitem.Count; i++)
            {
                recipilistController._recipi_listitem[i].GetComponent<Toggle>().interactable = false;
            }
        }
        else //それ以外の調合
        {
            //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
            for (i = 0; i < pitemlistController._listitem.Count; i++)
            {
                pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false;
            }
        }

        yes.SetActive(true);
        no.SetActive(true);

        yes_text.text = "決定";

        if (GameMgr.final_select_flag == true)
        {
            yes_text.text = "制作開始！";
            YesSetDesign2();
        }

    }

    void Off_Flag_Setting()
    {
        //解除
        for (i = 0; i < pitemlistController._listitem.Count; i++)
        {
            pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
            pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false;
        }

        //カード全て削除
        //card_view.DeleteCard_DrawView();

        GameMgr.Comp_kettei_bunki = 0;

        itemselect_cancel.kettei_on_waiting = false;

        updown_counter_obj.SetActive(false);        

        //yes_selectitem_kettei.kettei1 = false;
        yes.SetActive(false);

        //yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        //Debug.Log("選択完了！");
    }

    //最後、アイテムアイコンを表示
    void FinalCheck_ItemIconHyouji(int _status)
    {
        //一個目
        _listitem.Add(Instantiate(finalcheck_Prefab, content.transform));
        _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_1].itemNameHyouji; //アイテム名
        texture2d = database.items[itemID_1].itemIcon_sprite;
        _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
        _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = GameMgr.Final_kettei_kosu1.ToString(); //個数

        //×をいれる
        //_listitem.Add(Instantiate(finalcheck_Prefab2, content.transform));
        //list_count += 2; //一個飛ばし

        list_count++;

        //二個目
        _listitem.Add(Instantiate(finalcheck_Prefab, content.transform));
        _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_2].itemNameHyouji; //アイテム名
        texture2d = database.items[itemID_2].itemIcon_sprite;
        _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
        _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = GameMgr.Final_kettei_kosu2.ToString(); //個数

        if(_status == 1) //3個表示のとき
        {
            //×をいれる
            //_listitem.Add(Instantiate(finalcheck_Prefab2, content.transform));
            //list_count += 2;

            list_count++;

            //三個目
            _listitem.Add(Instantiate(finalcheck_Prefab, content.transform));
            _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_3].itemNameHyouji; //アイテム名
            texture2d = database.items[itemID_3].itemIcon_sprite;
            _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = GameMgr.Final_kettei_kosu3.ToString(); //個数

        }

        //リザルトアイテムの表示
        if (!newrecipi_flag)
        {
            resultitemName_obj.GetComponent<Text>().text = database.items[GameMgr.Final_result_itemID1].itemNameHyouji; //アイテム名
            if (compoDB_select_judge == true)
            {
                final_itemmes = database.items[GameMgr.Final_result_itemID1].itemNameHyouji + "が出来そう！";
            }
            else
            {
                if (hikari_nomake)
                {
                    final_itemmes = "にいちゃんが作れないレシピは、ヒカリも作れないよ～・・。";
                }
                else
                {
                    final_itemmes = "これは.. ダメかもしれぬ。";
                }
            }
            texture2d = database.items[GameMgr.Final_result_itemID1].itemIcon_sprite;
            resultitem_Hyouji.transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            resultitem_Hyouji.transform.Find("KosuText").gameObject.SetActive(true);
            resultitem_Hyouji.transform.Find("newrecipi_BG").gameObject.SetActive(false);
            resultitem_Hyouji.transform.Find("DefaultBG").gameObject.SetActive(true);

            if (GameMgr.compound_select == 7)
            {
                resultitem_Hyouji.transform.Find("KosuText").GetComponent<Text>().text = "1";
            }
            else
            {
                resultitem_Hyouji.transform.Find("KosuText").GetComponent<Text>().text =
                databaseCompo.compoitems[GameMgr.Final_result_compID].cmpitem_result_kosu.ToString(); //個数
            }
        }
        else //新しいお菓子を思いつきそうな場合。アイコンは「？」とかになる。
        {
            resultitemName_obj.GetComponent<Text>().text = "???"; //アイテム名
            final_itemmes = "今までに作ったことのないお菓子が出来そう！";
            texture2d = Resources.Load<Sprite>("Sprites/Icon/question");
            resultitem_Hyouji.transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            resultitem_Hyouji.transform.Find("KosuText").gameObject.SetActive(true);            
            //resultitem_Hyouji.transform.Find("itemImage").GetComponent<Image>().color = new Color(256,256,256);
            resultitem_Hyouji.transform.Find("newrecipi_BG").gameObject.SetActive(true);
            resultitem_Hyouji.transform.Find("DefaultBG").gameObject.SetActive(false);

            if (GameMgr.compound_select == 7)
            {
                resultitem_Hyouji.transform.Find("KosuText").GetComponent<Text>().text = "1";
            }
            else
            {
                resultitem_Hyouji.transform.Find("KosuText").GetComponent<Text>().text =
                databaseCompo.compoitems[GameMgr.Final_result_compID].cmpitem_result_kosu.ToString(); //個数
            }
        }
    }

    //確率計算式 ここの計算の値が、そのまま実際の計算時のサイコロを振るときにも反映される。
    public int Kakuritsu_Keisan(int _compID)
    {
        _buf_kakuritsu = 0;
        _buf_kakuritsu = bufpower_keisan.Buf_CompKakuritsu_Keisan(databaseCompo.compoitems[_compID].cmpitemID_result); //にいちゃん・ヒカリが作るとき共通でバフかかる
        databaseCompo.RecipiCount_database();
       
        
        if (GameMgr.compound_select == 7) //ヒカリが作るときの成功率計算
        {
            _rate = (int)(databaseCompo.compoitems[_compID].success_Rate * _ex_probabilty_temp * GameMgr.hikari_make_okashiTime_successrate_buf);
        }
        else
        {
            //レシピ達成率に応じて調合成功率あがる + 装備品による確率上昇
            _rate = (int)(databaseCompo.compoitems[_compID].success_Rate * _ex_probabilty_temp) + GameMgr.game_Exup_rate + _buf_kakuritsu;
        }
        _debug_beforerate = _rate;

        //魔法調合の場合は、各スキルのレベルで成功率が上がる
        //パッシブによるバフ効果で確率が上がる場合は、上記のbufpower_keisan.Buf_CompKakuritsu_Keisan内で計算する
        _magic_rate = 0;
        if (GameMgr.Comp_kettei_bunki == 20 || GameMgr.Comp_kettei_bunki == 21 || GameMgr.Comp_kettei_bunki == 22)
        {
            switch(magicName)
            {
                case "Freezing_Spell":

                    _magic_rate = magicLearnLv * 3;
                    break;

                case "SugerPot":

                    _magic_rate = magicLearnLv * 5;
                    break;

                case "Luminous_Suger":

                    _magic_rate = magicLearnLv * 10;
                    break;

                case "Luminous_Fruits":

                    _magic_rate = magicLearnLv * 10;
                    break;
            }

            _rate += _magic_rate;
        }

        Debug.Log("成功基本確率(調合DBの値): " + databaseCompo.compoitems[_compID].success_Rate);
        Debug.Log("手帳成功率(GameMgr.game_Exup_rate): +" + GameMgr.game_Exup_rate);
        Debug.Log("アイテム系・魔法パッシブのバフ合計: +" + _buf_kakuritsu);
        Debug.Log("_ex_probabilty_temp: *" + _ex_probabilty_temp);
        Debug.Log("成功率（魔法バフ前）: " + _debug_beforerate);
        Debug.Log("魔法使用の習得LVによる成功率バフ: +" + _magic_rate);
        Debug.Log("最終成功率(ヒカリの場合、ヒカリ成功率）: " + _rate);
        
        Debug.Log("制作時間目安(1分単位): " + databaseCompo.compoitems[_compID].cost_Time);

        if (databaseCompo.compoitems[_compID].success_Rate >= 100) //生地系などは、基本的に失敗しない
        {
            if (GameMgr.compound_select == 7) //ヒカリが作るときは、失敗する可能性あり。
            {
                RateJougenCheck();
            }
            else
            {
                _rate = 100;
            }
  
        }
        else
        {
            RateJougenCheck();        
        }
        

        return _rate;
    }
 
    void RateJougenCheck()
    {
        if (_rate >= 98) //99~は、全て98で上限
        {
            _rate = 98; //上限は98％　ミスする可能性は０ではない
        }

        if (_rate < 0)
        {
            _rate = 0;
        }
    }

    //デバッグ用　確率を100%に。
    public void DebugKakuritsuALLOK()
    {
        _debug_sw = !_debug_sw;
        if (!_debug_sw)
        {
            exp_Controller._success_rate = _success_rate;
            kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
        }
        else
        {
            exp_Controller._success_rate = 100;
            kakuritsuPanel.KakuritsuYosoku_Img(100);
        }
    }
}

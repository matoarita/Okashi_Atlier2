using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Compound_Check : MonoBehaviour {

    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private CombinationMain Combinationmain;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private Sprite yes_sprite1;
    private Sprite yes_sprite2;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private GameObject updown_counter_setpanel;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject resultitemName_obj;

    private GameObject FinalCheckPanel;
    private Text FinalCheck_Text;
    private Text FinalCheck_itemText;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。

    private bool compoDB_select_judge;
    private string resultitemID;
    private int result_compoID;
    private List<int> result_kosuset = new List<int>();

    private string success_text;
    private float _success_rate;
    private int dice;

    private int itemID_1;
    private int itemID_2;
    private int itemID_3;

    public bool final_select_flag;

    private int i;
    private int _rate;
    private int _releaseID;
    private bool newrecipi_flag;

    private GameObject finalcheck_Prefab; //調合最終チェック用のアイテムプレファブ
    private GameObject finalcheck_Prefab2; //間の掛け算表記「×」
    private GameObject resultitem_Hyouji;
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    private List<GameObject> _listitem = new List<GameObject>();
    private int list_count;
    private Sprite texture2d;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("Compound_BGPanel_A/FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

        resultitemName_obj = canvas.transform.Find("Compound_BGPanel_A/FinalCheckPanel/Comp/TextPanel/Image/Result_item/NameText").gameObject;

        FinalCheckPanel = canvas.transform.Find("Compound_BGPanel_A/FinalCheckPanel").gameObject;
        FinalCheck_Text = FinalCheckPanel.transform.Find("Comp/KakuritsuMessage/Text").GetComponent<Text>();
        FinalCheck_itemText = FinalCheckPanel.transform.Find("Comp/TextPanel/Image/ItemText").GetComponent<Text>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //調合用メソッドの取得
        Combinationmain = CombinationMain.Instance.GetComponent<CombinationMain>();        

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //テキストウィンドウの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = FinalCheckPanel.transform.Find("Comp/TextPanel/Image/Scroll View/Viewport/Content").gameObject;
        finalcheck_Prefab = (GameObject)Resources.Load("Prefabs/finalcheck_item");
        finalcheck_Prefab2 = (GameObject)Resources.Load("Prefabs/finalcheck_kakeru");
        resultitem_Hyouji = FinalCheckPanel.transform.Find("Comp/TextPanel/Image/Result_item").gameObject;

        final_select_flag = false;
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
            updown_counter_setpanel = updown_counter_obj.transform.Find("SetPanel").gameObject;

            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;

            yes_sprite1 = Resources.Load<Sprite>("Sprites/Window/miniwindowB");
            yes_sprite2 = Resources.Load<Sprite>("Sprites/Window/sabwindowA_pink_66");

        }

        if (final_select_flag == true) //最後、これで調合するかどうかを待つフラグ
        {
            if (compound_Main.compound_select == 1) //レシピ調合のときの処理
            {

                compound_Main.compound_status = 110;

                SelectPaused();

                final_select_flag = false;
                resultitemName_obj.SetActive(true);

                StartCoroutine("recipiFinal_select");

            }

            if (compound_Main.compound_select == 2) //トッピング調合のときの処理
            {

                compound_Main.compound_status = 110;

                SelectPaused();

                final_select_flag = false;
                resultitemName_obj.SetActive(true);

                StartCoroutine("topping_Final_select");

            }

            if (compound_Main.compound_select == 3) //オリジナル調合のときの処理
            {

                compound_Main.compound_status = 110;

                SelectPaused();

                final_select_flag = false;
                resultitemName_obj.SetActive(true);

                FinalCheckPanel.SetActive(true);
                yes.GetComponent<Button>().interactable = false;
                no.GetComponent<Button>().interactable = false;

                //一度contentの中身を削除
                foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
                {
                    Destroy(child.gameObject);
                }
                list_count = 0;
                _listitem.Clear();

                StartCoroutine("Final_select");

            }
        }
        
        
    }

    IEnumerator Final_select()
    {
        //*** 2個or3個選んだ状態で、最後、これでOKかどうか聞くメソッド　***//

        switch (pitemlistController.kettei1_bunki)
        {
            case 2: //2個選択しているとき

                itemID_1 = pitemlistController.final_kettei_item1;
                itemID_2 = pitemlistController.final_kettei_item2;

                pitemlistController.kettei_item3 = 9999;
                pitemlistController.final_kettei_item3 = 9999; //9999は空を表す数字

                card_view.OKCard_DrawView02(pitemlistController.final_kettei_kosu2);

                CompoundJudge(); //調合の判定・確率処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。

                updown_counter_obj.SetActive(true);
                updown_counter_setpanel.SetActive(true);

                //確率に応じて、テキストが変わる。
                FinalCheck_Text.text = success_text;

                //選んだアイテムを表示する。リザルトアイテムも表示する。
                FinalCheck_ItemIconHyouji(0); //2個表示のとき
                /*FinalCheck_itemText.text = database.items[itemID_1].itemNameHyouji + " × " + pitemlistController.final_kettei_kosu1 + "\n"
                    + database.items[itemID_2].itemNameHyouji + " × " + pitemlistController.final_kettei_kosu2;*/
               

                _text.text = "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" 
                    + "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個";

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                FinalCheckPanel.SetActive(false);
                yes.GetComponent<Button>().interactable = true;
                no.GetComponent<Button>().interactable = true;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //選んだ二つをもとに、一つのアイテムを生成する。そして、調合完了！

                        //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.result_ok = true; //調合完了のフラグをたてておく。

                        exp_Controller.extreme_on = false;

                        exp_Controller.set_kaisu = updown_counter.updown_kosu; //何セット作るかの個数もいれる。

                        exp_Controller.result_kosuset.Clear();
                        for (i = 0; i < result_kosuset.Count; i++)
                        {
                            exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                        }

                        compound_Main.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        exp_Controller.ResultOK();

                        break;

                    case false:

                        //Debug.Log("1個目を選択した状態に戻る");

                        compound_Main.compound_status = 100;
                        itemselect_cancel.Two_cancel();

                        break;
                }
                break;

            case 3: //3個選択しているとき

                itemID_1 = pitemlistController.final_kettei_item1;
                itemID_2 = pitemlistController.final_kettei_item2;
                itemID_3 = pitemlistController.final_kettei_item3;

                card_view.OKCard_DrawView03(pitemlistController.final_kettei_kosu3);

                CompoundJudge(); //調合の処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。

                updown_counter_obj.SetActive(true);
                updown_counter_setpanel.SetActive(true);

                //確率に応じて、テキストが変わる。
                
                FinalCheck_Text.text = success_text;

                //選んだアイテムを表示する。リザルトアイテムも表示する。
                /*FinalCheck_itemText.text = database.items[itemID_1].itemNameHyouji + " × " + pitemlistController.final_kettei_kosu1 + "\n"
                    + database.items[itemID_2].itemNameHyouji + " × " + pitemlistController.final_kettei_kosu2 + "\n"
                    + database.items[itemID_3].itemNameHyouji + " × " + pitemlistController.final_kettei_kosu3;*/
                FinalCheck_ItemIconHyouji(1); //3個表示のとき

                _text.text = "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" 
                    + "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" 
                    + "三個目：" + database.items[itemID_3].itemNameHyouji + " " + pitemlistController.final_kettei_kosu3 + "個";

                //Debug.Log(database.items[itemID_1].itemNameHyouji + "と" + database.items[itemID_2].itemNameHyouji + "と" + database.items[itemID_3].itemNameHyouji + "でいいですか？");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                FinalCheckPanel.SetActive(false);
                yes.GetComponent<Button>().interactable = true;
                no.GetComponent<Button>().interactable = true;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //選んだ三つをもとに、一つのアイテムを生成する。

                        //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.result_ok = true; //オリジナル調合完了のフラグをたてておく。

                        exp_Controller.extreme_on = false;

                        exp_Controller.set_kaisu = updown_counter.updown_kosu; //何セット作るかの個数もいれる。

                        exp_Controller.result_kosuset.Clear();
                        for (i = 0; i < result_kosuset.Count; i++)
                        {
                            exp_Controller.result_kosuset.Add(result_kosuset[i]); //exp_Controllerにオリジナル個数組み合わせセットもここで登録。
                        }

                        compound_Main.compound_status = 4;

                        //card_view.DeleteCard_DrawView();
                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        exp_Controller.ResultOK();


                        break;

                    case false:

                        //Debug.Log("三個目はcancel");

                        compound_Main.compound_status = 100;
                        itemselect_cancel.Three_cancel();

                        yes.SetActive(true);
                        yes_text.text = "作る";
                        YesSetDesign2();                       
                        

                        break;
                }
                break;
        }
    }

    IEnumerator topping_Final_select()
    {
        //*** 1個or2個or3個選んだ状態で、最後、これでOKかどうか聞くメソッド　***//

        switch (pitemlistController.kettei1_bunki)
        {
            case 11: //べーすあいてむ + 1個選択しているとき

                itemID_1 = pitemlistController.final_kettei_item1;
                pitemlistController.kettei_item2 = 9999;
                pitemlistController.kettei_item3 = 9999;
                pitemlistController.final_kettei_item2 = 9999; //9999は空を表す数字                
                pitemlistController.final_kettei_item3 = 9999; //9999は空を表す数字

                card_view.OKCard_DrawView02(1);

                CompoundJudge(); //エクストリーム調合で、新規作成されるアイテムがないかをチェック。ない場合は、通常通りトッピング。ある場合は、新規作成する。

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "に" + "\n" + "一個目: " + 
                    database.items[itemID_1].itemNameHyouji + " " + 
                    pitemlistController.final_kettei_kosu1 + "個" + "をトッピングします。" + "\n" + "　トッピングしますか？";

                Debug.Log("ベースアイテム＋一個トッピング　調合確認中");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        compound_Main.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        /*
                        //新しいアイテムを閃く
                        if (compoDB_select_judge == true)
                        {                           
                            exp_Controller.NewRecipiflag_check = true;
                        }

                        //コンポDBに該当していなければ、通常通りトッピングの処理
                        else
                        {
                            exp_Controller.NewRecipiflag_check = false;
                        }
                       

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。
                        exp_Controller.Topping_Result_OK();*/

                        
                        //エクストリーム調合で、コンポDBに合致する新しいアイテムが生成される場合は、新規調合に変える。それ以外は、通常通りトッピング
                        if (compoDB_select_judge == true)
                        {
                            exp_Controller.extreme_on = true;

                            //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.ResultOK();
                        }
                        else
                        {
                            exp_Controller.extreme_on = false;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.Topping_Result_OK();
                        }

                        break;

                    case false:

                        //Debug.Log("ベースアイテムを選択した状態に戻る");
                        compound_Main.compound_status = 100;

                        exp_Controller._success_rate = 100;
                        kakuritsuPanel.KakuritsuYosoku_Reset();
                        itemselect_cancel.Two_cancel();

                        break;
                }
                break;

            case 12: //べーすあいてむ + 2個選択しているとき

                itemID_1 = pitemlistController.final_kettei_item1;
                itemID_2 = pitemlistController.final_kettei_item2;

                pitemlistController.kettei_item3 = 9999;
                pitemlistController.final_kettei_item3 = 9999; //9999は空を表す数字

                card_view.OKCard_DrawView03(1);

                CompoundJudge(); //エクストリーム調合で、新規作成されるアイテムがないかをチェック。ある場合は、そのレシピを閃く。

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "に" + "\n" + 
                    "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + 
                    "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + 
                    "　トッピングしますか？";

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        compound_Main.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        /*
                        //新しいアイテムを閃く
                        if (compoDB_select_judge == true)
                        {                           
                            exp_Controller.NewRecipiflag_check = true;
                        }

                        //コンポDBに該当していなければ、通常通りトッピングの処理
                        else
                        {
                            exp_Controller.NewRecipiflag_check = false;
                        }                       

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。
                        exp_Controller.Topping_Result_OK();*/

                        
                        //エクストリーム調合で、コンポDBに合致する新しいアイテムが生成される場合は、新規調合に変える。それ以外は、通常通りトッピング
                        if (compoDB_select_judge == true)
                        {
                            exp_Controller.extreme_on = true;

                            //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.ResultOK();
                        }
                        else
                        {
                            exp_Controller.extreme_on = false;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。
                            exp_Controller.Topping_Result_OK();
                        }

                        break;

                    case false:

                        //Debug.Log("1個目を選択した状態に戻る");
                        compound_Main.compound_status = 100;

                        exp_Controller._success_rate = exp_Controller._temp_srate_1;
                        kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_1);
                        itemselect_cancel.Three_cancel();

                        break;
                }
                break;

            case 13: //べーすあいてむ + 3個選択しているとき

                itemID_1 = pitemlistController.final_kettei_item1;
                itemID_2 = pitemlistController.final_kettei_item2;
                itemID_3 = pitemlistController.final_kettei_item3;

                card_view.OKCard_DrawView04();

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "に" + "\n" + 
                    "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + 
                    "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + 
                    "三個目：" + database.items[itemID_3].itemNameHyouji + " " + pitemlistController.final_kettei_kosu3 + "個" + 
                    "　トッピングしますか？";

                //Debug.Log(database.items[itemID_1].itemNameHyouji + "と" + database.items[itemID_2].itemNameHyouji + "と" + database.items[itemID_3].itemNameHyouji + "でいいですか？");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:
                        //選んだ三つをもとに、一つのアイテムを生成する。

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                        compound_Main.compound_status = 4;

                        //card_view.DeleteCard_DrawView();
                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        exp_Controller.Topping_Result_OK();

                        break;

                    case false:

                        //Debug.Log("2個目を選択した状態に戻る");
                        compound_Main.compound_status = 100;

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
        _text.text = database.items[recipilistController.result_recipiitem].itemNameHyouji + "が" +
            databaseCompo.compoitems[recipilistController.result_recipicompID].cmpitem_result_kosu * recipilistController.final_select_kosu + "個　出来ます。" + "\n" + "作る？";

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

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

                compound_Main.compound_status = 4;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

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

                compound_Main.compound_status = 100;
                itemselect_cancel.All_cancel();

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


    void CompoundJudge()
    {
        _itemIDtemp_result.Clear();
        _itemKosutemp_result.Clear();
        _itemSubtype_temp_result.Clear();

        //オリジナル調合の場合はこっち
        if (pitemlistController.kettei1_bunki == 2 || pitemlistController.kettei1_bunki == 3)
        {
            _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item1].itemName);
            _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item2].itemName);

            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item1].itemType_sub.ToString());
            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item2].itemType_sub.ToString());

            _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu1);
            _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu2);

            if (pitemlistController.final_kettei_item3 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                pitemlistController.final_kettei_kosu3 = 9999; //個数にも9999=emptyを入れる。
                _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu3);
            }
            else
            {
                _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item3].itemName);
                _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item3].itemType_sub.ToString());
                _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu3);
            }
        }

        //エクストリーム調合の場合は、こっち。ベース決定アイテムを、temp_resultに入れる。
        else if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
        {
            _itemIDtemp_result.Add(database.items[pitemlistController.final_base_kettei_item].itemName);
            _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item1].itemName);

            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_base_kettei_item].itemType_sub.ToString());
            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item1].itemType_sub.ToString());

            _itemKosutemp_result.Add(1);
            _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu1);

            if (pitemlistController.final_kettei_item2 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                pitemlistController.final_kettei_kosu2 = 9999; //個数にも9999=emptyを入れる。
                _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu2);
            }
            else
            {
                _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item2].itemName);
                _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item2].itemType_sub.ToString());
                _itemKosutemp_result.Add(pitemlistController.final_kettei_kosu2);
            }
        }

        i = 0;

        resultitemID = "gomi_1"; //どの調合組み合わせのパターンにも合致しなかった場合は、ゴミのIDが入っている。調合DBのゴミのitemNameを入れると、後で数値に変換してくれる。現在は、500に変換される。

        compoDB_select_judge = false;


        //判定処理//

        //新規作成のため、以下の判定処理を行う。個数は、判定に関係しない。


        //①固有の名称同士の組み合わせか、②固有＋サブの組み合わせか、③サブ同士のジャンルで組み合わせが一致していれば、制作する。

        //①３つの入力をもとに、組み合わせ計算するメソッド＜固有名称の組み合わせ確認＞     
        Combinationmain.Combination(_itemIDtemp_result.ToArray(), _itemKosutemp_result.ToArray(), 0); //決めた３つのアイテム＋それぞれの個数、の配列

        compoDB_select_judge = Combinationmain.compFlag;
        if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
        {
            resultitemID = Combinationmain.resultitemName;
            result_compoID = Combinationmain.result_compID;

            result_kosuset.Clear();
            for (i = 0; i < Combinationmain.result_kosuset.Count; i++)
            {
                result_kosuset.Add(Combinationmain.result_kosuset[i]); //そのときの個数の組み合わせ（CompoDBの左から順番になっている。）も記録。
            }
        }


        //②　①の組み合わせにない場合は、2通りが考えられる。　アイテム名＋サブ＋サブ　か　アイテム名＋アイテム名＋サブの組み合わせ
        if (compoDB_select_judge == false)
        {
            //個数計算していないので、バグあり
            Combinationmain.Combination2(_itemIDtemp_result.ToArray(), _itemSubtype_temp_result.ToArray(), _itemKosutemp_result.ToArray(), 0);

            compoDB_select_judge = Combinationmain.compFlag;
            if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
            {
                resultitemID = Combinationmain.resultitemName;
                result_compoID = Combinationmain.result_compID;

                result_kosuset.Clear();
                for (i = 0; i < Combinationmain.result_kosuset.Count; i++)
                {
                    result_kosuset.Add(Combinationmain.result_kosuset[i]); //そのときの個数の組み合わせ（CompoDBの左から順番になっている。）も記録。
                }
            }
        }


        //③固有の組み合わせがなかった場合のみ、サブジャンル同士の組み合わせがないかも見る。サブ＋サブ＋サブ

        if (compoDB_select_judge == false)
        {
            Combinationmain.Combination(_itemSubtype_temp_result.ToArray(), _itemKosutemp_result.ToArray(), 0);

            compoDB_select_judge = Combinationmain.compFlag;
            if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
            {
                resultitemID = Combinationmain.resultitemName;
                result_compoID = Combinationmain.result_compID;

                result_kosuset.Clear();
                for (i = 0; i < Combinationmain.result_kosuset.Count; i++)
                {
                    result_kosuset.Add(Combinationmain.result_kosuset[i]); //そのときの個数の組み合わせ（CompoDBの左から順番になっている。）も記録。
                }
            }
            
        }


        //stringのリザルドアイテムを、アイテムIDに変換。
        i = 0;

        while (i < database.items.Count)
        {

            if (database.items[i].itemName == resultitemID)
            {
                pitemlistController.result_item = i; //プレイヤーコントローラーの変数に、アイテムIDを代入
                break;
            }
            ++i;
        }

        pitemlistController.result_compID = result_compoID;



        //調合判定

        if (compoDB_select_judge == true)
        {

            //成功率の計算。コンポDBの、基本確率　＋　プレイヤーのレベル
            _success_rate = Kakuritsu_Keisan(pitemlistController.result_compID);


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


            //調合判定を行うかどうか+成功確率の表示更新

            //新規調合の場合　もしくは、　エクストリーム調合の場合。
            if (pitemlistController.kettei1_bunki == 2 || pitemlistController.kettei1_bunki == 3)
            {
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                exp_Controller._success_rate = _success_rate;
                kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
            }
            else if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
            {
                //
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                exp_Controller._success_rate = _success_rate;
                kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
            }

            //新しいレシピかどうか。
            _releaseID = databaseCompo.SearchCompoIDString(databaseCompo.compoitems[result_compoID].release_recipi);
            if(databaseCompo.compoitems[_releaseID].cmpitem_flag == 0) //0なら新しいレシピ
            {                
                success_text = "新しいお菓子を思いつきそう..？";
                newrecipi_flag = true;
                exp_Controller.NewRecipiFlag = true;
                kakuritsuPanel.KakuritsuYosoku_NewImg();
                resultitemName_obj.SetActive(false);
            }
            else
            {
                exp_Controller.NewRecipiFlag = false;
                newrecipi_flag = false;
            }
        }

        //どの調合リストにも当てはまらなかった場合
        else
        {
            //Debug.Log("どの調合リストにも当てはまらなかった。");            

            //エクストリーム調合の場合は、通常通りトッピングの処理を行う。
            if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
            {

            }
            else
            {
                //失敗
                exp_Controller._success_judge_flag = 2; //必ず失敗する
                success_text = "これは.. ダメかもしれぬ。";
                kakuritsuPanel.KakuritsuYosoku_Img(0);
            }
                            
        }

        //判定予測処理　ここまで//
    }

    void SelectPaused()
    {
        if (compound_Main.compound_select == 1) //レシピ調合のときの処理
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

        if (SceneManager.GetActiveScene().name == "Compound")
        {
            if (final_select_flag == true)
            {
                yes_text.text = "制作開始！";
                YesSetDesign2();
            }

            if (compound_Main.compound_select == 5)
            {
                yes_text.text = "生地を焼く！";
            }

            if (compound_Main.compound_select == 10)
            {
                yes_text.text = "あげる";
            }
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

        pitemlistController.kettei1_bunki = 0;

        itemselect_cancel.kettei_on_waiting = false;

        updown_counter_obj.SetActive(false);        

        yes_selectitem_kettei.kettei1 = false;
        yes.SetActive(false);

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

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
        _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = "× " + pitemlistController.final_kettei_kosu1.ToString(); //個数

        //×をいれる
        _listitem.Add(Instantiate(finalcheck_Prefab2, content.transform));
        list_count += 2; //一個飛ばし

        //二個目
        _listitem.Add(Instantiate(finalcheck_Prefab, content.transform));
        _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_2].itemNameHyouji; //アイテム名
        texture2d = database.items[itemID_2].itemIcon_sprite;
        _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
        _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = "× " + pitemlistController.final_kettei_kosu2.ToString(); //個数

        if(_status == 1) //3個表示のとき
        {
            //×をいれる
            _listitem.Add(Instantiate(finalcheck_Prefab2, content.transform));
            list_count += 2;

            //三個目
            _listitem.Add(Instantiate(finalcheck_Prefab, content.transform));
            _listitem[list_count].transform.Find("NameText").GetComponent<Text>().text = database.items[itemID_3].itemNameHyouji; //アイテム名
            texture2d = database.items[itemID_3].itemIcon_sprite;
            _listitem[list_count].transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            _listitem[list_count].transform.Find("KosuText").GetComponent<Text>().text = "× " + pitemlistController.final_kettei_kosu3.ToString(); //個数

        }

        //リザルトアイテムの表示
        if (!newrecipi_flag)
        {
            resultitem_Hyouji.transform.Find("NameText").GetComponent<Text>().text = database.items[pitemlistController.result_item].itemNameHyouji; //アイテム名
            texture2d = database.items[pitemlistController.result_item].itemIcon_sprite;
            resultitem_Hyouji.transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            resultitem_Hyouji.transform.Find("KosuText").gameObject.SetActive(true);
            resultitem_Hyouji.transform.Find("KosuText").GetComponent<Text>().text =
                "× " + databaseCompo.compoitems[pitemlistController.result_compID].cmpitem_result_kosu.ToString(); //個数
            resultitem_Hyouji.transform.Find("newrecipi_BG").gameObject.SetActive(false);
        }
        else //新しいお菓子を思いつきそうな場合。アイコンは「？」とかになる。
        {
            resultitem_Hyouji.transform.Find("NameText").GetComponent<Text>().text = "???"; //アイテム名
            texture2d = Resources.Load<Sprite>("Sprites/Icon/question");
            resultitem_Hyouji.transform.Find("itemImage").GetComponent<Image>().sprite = texture2d; //画像データ
            //resultitem_Hyouji.transform.Find("itemImage").GetComponent<Image>().color = new Color(256,256,256);
            resultitem_Hyouji.transform.Find("KosuText").gameObject.SetActive(false); //個数
            resultitem_Hyouji.transform.Find("newrecipi_BG").gameObject.SetActive(true);
        }
    }

    //確率計算式
    public int Kakuritsu_Keisan(int _compID)
    {
        _rate = databaseCompo.compoitems[_compID].success_Rate + (PlayerStatus.player_renkin_lv);

        if (_rate >= 100)
        {
            _rate = 100;
        }

        if (_rate < 0)
        {
            _rate = 0;
        }

        return _rate;
    }
 
    
}

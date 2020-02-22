﻿using System.Collections;
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

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。

    private bool compoDB_select_judge;
    private string resultitemID;
    private int result_compoID;

    private string success_text;
    private float _success_rate;
    private int dice;

    private int itemID_1;
    private int itemID_2;
    private int itemID_3;

    private int i;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //確率パネルの取得
        kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
        kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

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

        
    }
	
	// Update is called once per frame
	void Update () {

        if(pitemlistController_obj == null)
        {
            //プレイヤーアイテムリストの取得。以下のオブジェクトは、「CompoundMain」オブジェクトで初期化しているので、Start()でやると処理の順序の関係でバグる。
            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();

            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;
        }

        if (compound_Main.compound_select == 3) //オリジナル調合のときの処理
        {
            if (pitemlistController.final_select_flag == true) //最後、これで調合するかどうかを待つフラグ
            {

                SelectPaused();

                pitemlistController.final_select_flag = false;

                StartCoroutine("Final_select");
            }
        }
        
        else if (compound_Main.compound_select == 2) //トッピング調合のときの処理
        {
            if (pitemlistController.final_select_flag == true) //最後、これで調合するかどうかを待つフラグ
            {

                SelectPaused();

                //pitemlistController.final_select_flag = false;

                StartCoroutine("topping_Final_select");
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

                card_view.OKCard_DrawView02();

                CompoundMethod(); //調合の処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。

                _text.text = "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "　調合しますか？" + "\n" + success_text;

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //選んだ二つをもとに、一つのアイテムを生成する。そして、調合完了！

                        //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.result_ok = true; //調合完了のフラグをたてておく。

                        exp_Controller.extreme_on = false;

                        compound_Main.compound_status = 4;

                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        exp_Controller.ResultOK();

                        break;

                    case false:

                        //Debug.Log("1個目を選択した状態に戻る");

                        itemselect_cancel.Two_cancel();

                        break;
                }
                break;

            case 3: //3個選択しているとき

                itemID_1 = pitemlistController.final_kettei_item1;
                itemID_2 = pitemlistController.final_kettei_item2;
                itemID_3 = pitemlistController.final_kettei_item3;

                card_view.OKCard_DrawView03();

                CompoundMethod(); //調合の処理にうつる。結果、resultIDに、生成されるアイテム番号が代入されている。

                _text.text = "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "三個目：" + database.items[itemID_3].itemNameHyouji + " " + pitemlistController.final_kettei_kosu3 + "個" + "\n" + "　調合しますか？" + success_text;

                //Debug.Log(database.items[itemID_1].itemNameHyouji + "と" + database.items[itemID_2].itemNameHyouji + "と" + database.items[itemID_3].itemNameHyouji + "でいいですか？");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //選んだ三つをもとに、一つのアイテムを生成する。

                        //調合成功確率計算、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.result_ok = true; //オリジナル調合完了のフラグをたてておく。

                        exp_Controller.extreme_on = false;

                        compound_Main.compound_status = 4;

                        //card_view.DeleteCard_DrawView();
                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

                        exp_Controller.ResultOK();


                        break;

                    case false:

                        //Debug.Log("三個目はcancel");

                        itemselect_cancel.Three_cancel();

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

                card_view.OKCard_DrawView02();

                CompoundMethod(); //エクストリーム調合で、新規作成されるアイテムがないかをチェック。ない場合は、通常通りトッピング。ある場合は、新規作成する。

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "に" + "\n" + "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "をトッピングします。" + "\n" + "　調合しますか？";

                Debug.Log("ベースアイテム＋一個トッピング　調合確認中");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                pitemlistController.final_select_flag = false;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //新しいアイテムを閃く
                        /*if (compoDB_select_judge == true)
                        {                           
                            exp_Controller.extreme_on = true;
                        }

                        //コンポDBに該当していなければ、通常通りトッピングの処理
                        else
                        {
                            exp_Controller.extreme_on = false;
                        }*/

                        exp_Controller.compound_success = true;

                        compound_Main.compound_status = 4;


                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

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

                card_view.OKCard_DrawView03();

                CompoundMethod(); //エクストリーム調合で、新規作成されるアイテムがないかをチェック。ある場合は、そのレシピを閃く。

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "に" + "\n" + "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "　調合しますか？";

                //Debug.Log("成功確率は、" + databaseCompo.compoitems[resultitemID].success_Rate);

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                pitemlistController.final_select_flag = false;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //新しいアイテムを閃く
                        /*if (compoDB_select_judge == true)
                        {                           
                            exp_Controller.extreme_on = true;
                        }

                        //コンポDBに該当していなければ、通常通りトッピングの処理
                        else
                        {
                            exp_Controller.extreme_on = false;
                        }*/

                        exp_Controller.compound_success = true;

                        compound_Main.compound_status = 4;

                        //card_view.DeleteCard_DrawView();
                        card_view.CardCompo_Anim();
                        Off_Flag_Setting();

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

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "に" + "\n" + "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目：" + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "三個目：" + database.items[itemID_3].itemNameHyouji + " " + pitemlistController.final_kettei_kosu3 + "個" + "　調合しますか？";

                //Debug.Log(database.items[itemID_1].itemNameHyouji + "と" + database.items[itemID_2].itemNameHyouji + "と" + database.items[itemID_3].itemNameHyouji + "でいいですか？");

                while (yes_selectitem_kettei.onclick != true)
                {

                    yield return null; // オンクリックがtrueになるまでは、とりあえず待機
                }

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:
                        //選んだ三つをもとに、一つのアイテムを生成する。

                        exp_Controller.compound_success = true;

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

                        exp_Controller._success_rate = exp_Controller._temp_srate_2;
                        kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_2);
                        itemselect_cancel.Four_cancel();

                        break;
                }
                break;
        }
    }

    public void CompoundMethod()
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

        //一個目に選んだアイテムが生地タイプでもなく、フルーツ同士の合成でもない場合、
        //新規作成のため、以下の判定処理を行う。個数は、判定に関係しない。


        //①固有の名称同士の組み合わせか、②固有＋サブの組み合わせか、③サブ同士のジャンルで組み合わせが一致していれば、制作する。

        //①３つの入力をもとに、組み合わせ計算するメソッド＜固有名称の組み合わせ確認＞     
        Combinationmain.Combination(_itemIDtemp_result.ToArray(), _itemKosutemp_result.ToArray()); //決めた３つのアイテム＋それぞれの個数、の配列

        compoDB_select_judge = Combinationmain.compFlag;
        if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
        {
            resultitemID = Combinationmain.resultitemName;
            result_compoID = Combinationmain.result_compID;

        }


        //②　①の組み合わせにない場合は、2通りが考えられる。　アイテム名＋サブ＋サブ　か　アイテム名＋アイテム名＋サブの組み合わせ
        if (compoDB_select_judge == false)
        {
            //個数計算していないので、バグあり
            Combinationmain.Combination2(_itemIDtemp_result.ToArray(), _itemSubtype_temp_result.ToArray(), _itemKosutemp_result.ToArray());

            compoDB_select_judge = Combinationmain.compFlag;
            if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
            {
                resultitemID = Combinationmain.resultitemName;
                result_compoID = Combinationmain.result_compID;

            }
        }


        //③固有の組み合わせがなかった場合のみ、サブジャンル同士の組み合わせがないかも見る。サブ＋サブ＋サブ

        if (compoDB_select_judge == false)
        {
            Combinationmain.Combination(_itemSubtype_temp_result.ToArray(), _itemKosutemp_result.ToArray());

            compoDB_select_judge = Combinationmain.compFlag;
            if (compoDB_select_judge) //一致するものがあれば、resultitemの名前を入れる。
            {
                resultitemID = Combinationmain.resultitemName;
                result_compoID = Combinationmain.result_compID;

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



        //新規調合で新しいアイテムが作成される場合の処理。
        //さらに生地への合成か、全く新しいアイテムが作成されるかで変わる。
        //コンポDBに一致するものがあった場合は、以下の処理を行う。

        if (compoDB_select_judge == true)
        {

            //成功率の計算。コンポDBの、基本確率　＋　プレイヤーのレベル
            _success_rate = databaseCompo.compoitems[pitemlistController.result_compID].success_Rate + (PlayerStatus.player_renkin_lv);

            //一個目が生地ではなく、小麦粉も使われていない。全く新しいアイテムが生成される。
            {
                exp_Controller.comp_judge_flag = 0; //新規調合の場合は0にする。

                if (_success_rate >= 0.0 && _success_rate < 20.0)
                {
                    //成功率超低い
                    success_text = "これは.. 奇跡が起こればあるいは・・。";
                }
                else if (_success_rate >= 20.0 && _success_rate < 40.0)
                {
                    //成功率低め
                    success_text = "かなりきつい・・かも。";
                }
                else if (_success_rate >= 40.0 && _success_rate < 60.0)
                {
                    //普通
                    success_text = "頑張れば、いける・・！";
                }
                else if (_success_rate >= 60.0 && _success_rate < 80.0)
                {
                    //成功率高め
                    success_text = "問題なくいけそうだね。";
                }
                else if (_success_rate >= 80.0 && _success_rate < 99.9)
                {
                    //成功率かなり高い
                    success_text = "これなら楽勝！！";
                }
                else //100%~
                {
                    //１００％成功
                    success_text = "100%パーフェクト！";
                }
            }

            //調合判定を行うかどうか

            //新規調合の場合　もしくは、　エクストリーム調合の場合で、新しいレシピをひらめきそうな場合。下がエクストリーム調合
            if (pitemlistController.kettei1_bunki == 2 || pitemlistController.kettei1_bunki == 3)
            {
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                exp_Controller._success_rate = _success_rate;
                kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
            }
            else if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
            {
                //?? 新しいお菓子を思いつきそうだ
                exp_Controller._success_judge_flag = 1; //判定処理を行う。
                exp_Controller._success_rate = _success_rate;
                kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
                //kakuritsuPanel.KakuritsuYosoku_NewImg();
                //success_text = "新しいお菓子を思いつきそうだ。";
            }
        }

        //どの調合リストにも当てはまらなかった場合（result_item=500）、
        //生地にアイテムを合成するのか、どの組み合わせにも当てはまらず単純に失敗するのか、の判定をさらに行う
        else
        {
            //Debug.Log("どの調合リストにも当てはまらなかった。");

            //オリジナル調合の場合は、生地合成、もしくは単純に失敗かの判定
            if (pitemlistController.kettei1_bunki == 2 || pitemlistController.kettei1_bunki == 3)
            {
                //一個目に選んだアイテムが、生地タイプのアイテムの場合で、2個目のアイテムが合成用のアイテムであれば、
                //成功失敗の判定処理はせず、生地にアイテムを合成する処理になる。

                if (database.items[pitemlistController.final_kettei_item1].itemType_sub == Item.ItemType_sub.Pate ||
                database.items[pitemlistController.final_kettei_item1].itemType_sub == Item.ItemType_sub.Cookie_base ||
                database.items[pitemlistController.final_kettei_item1].itemType_sub == Item.ItemType_sub.Pie_base ||
                database.items[pitemlistController.final_kettei_item1].itemType_sub == Item.ItemType_sub.Chocolate_base ||
                database.items[pitemlistController.final_kettei_item1].itemType_sub == Item.ItemType_sub.Cake_base)
                {

                    switch (database.items[pitemlistController.final_kettei_item2].itemType_sub)
                    {
                        case Item.ItemType_sub.Fruits:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1; //1の場合、生地にアイテムを合成する処理のフラグ
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        case Item.ItemType_sub.Nuts:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        case Item.ItemType_sub.Suger:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        case Item.ItemType_sub.Komugiko:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        case Item.ItemType_sub.Butter:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        case Item.ItemType_sub.Source:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        case Item.ItemType_sub.Potion:

                            success_text = "生地にアイテムを合成します。";
                            exp_Controller._success_judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            kakuritsuPanel.KakuritsuYosoku_Img(100);
                            break;

                        default:

                            //2個目のアイテムが、上記のパターンにあてはまらない場合は、失敗する。

                            exp_Controller._success_judge_flag = 2; //必ず失敗する
                            success_text = "これは.. ダメかもしれぬ。";
                            kakuritsuPanel.KakuritsuYosoku_Img(0);
                            break;
                    }

                }

                //一個目が生地でない場合、
                //DBにも登録されておらず、生地への合成でもないので、失敗する。

                else
                {
                    //失敗
                    exp_Controller._success_judge_flag = 2; //必ず失敗する
                    success_text = "これは.. ダメかもしれぬ。";
                    kakuritsuPanel.KakuritsuYosoku_Img(0);
                }
            }

            //エクストリーム調合の場合は、通常通りトッピングの処理を行う。
            else if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
            {

            }
        }

        //判定予測処理　ここまで//
    }

    void SelectPaused()
    {
        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < pitemlistController._listitem.Count; i++)
        {
            pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false;
        }

        yes.SetActive(true);
        no.SetActive(true);

        yes_text.text = "決定";

        if (SceneManager.GetActiveScene().name == "Compound")
        {
            if (pitemlistController.final_select_flag == true)
            {
                yes_text.text = "制作開始！";
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
}
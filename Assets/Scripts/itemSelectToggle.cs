//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

//***  アイテムの調合処理、プレイヤーのアイテム所持リストの処理はここでやっています。
//***  プレファブにとりつけているスクリプト、なので、privateの値は、インスタンスごとに変わってくるため、バグに注意。

public class itemSelectToggle : MonoBehaviour
{
    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;


    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject item_tsuika; //PlayeritemList_ScrollViewの子オブジェクト「item_tsuika」ボタン

    private GameObject black_effect;

    public int toggleitem_ID; //リストの要素自体に、アイテムIDを保持する。
    public int toggleitem_type; //リストの要素に、プレイヤーアイテムリストか、オリジナルかを識別するための番号を割り振る。
    public int toggle_originplist_ID; //ややこしいが、オリジナルアイテムリストの最初から順番に、IDを割り振っておく。toggleorigin_ID=0だと、オリジナルアイテムプレイヤーリストの0番を参照する。

    private int i;

    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private int kettei_item1; //このスクリプトは、プレファブのインスタンスに取り付けているので、各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。
    private int kettei_item2;
    private int kettei_item3;

    private int itemID_1;
    private int itemID_2;
    private int itemID_3;
    private int baseitemID;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string>  _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。

    private bool compoDB_select_judge;
    private string resultitemID;
    private int result_compoID;

    private int judge_flag; //調合判定を行うか否か
    private bool compoundsuccess_flag; //成功か失敗か
    private float _success_rate;
    private float _final_success_rate;
    private int dice;
    private string success_text;

    void Start()
    {
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();


        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();

        //Initialise the Text to say the first state of the Toggle デバッグ用テキスト
        //m_Text = m_Toggle.GetComponentInChildren<Text>();
        //m_Text.text = "First Value : " + m_Toggle.isOn;

        //Add listener for when the state of the Toggle changes, to take action アドリスナー　トグルの値が変化したときに、｛｝内のメソッドを呼び出す
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });


        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。
        {
            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();
        }


        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        updown_counter_obj = pitemlistController_obj.transform.Find("updown_counter").gameObject;
        updown_counter = updown_counter_obj.GetComponent<Updown_counter>();

        yes = pitemlistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = pitemlistController_obj.transform.Find("No").gameObject;

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();


        item_tsuika = pitemlistController_obj.transform.Find("ItemADDbutton_Debug").gameObject;

        //テキストウィンドウの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();


        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //黒半透明パネルの取得
        black_effect = GameObject.FindWithTag("Black_Effect");

        i = 0;

        count = 0;
        judge_flag = 0;

        itemID_1 = 0;
        itemID_2 = 0;

        yes.SetActive(false);
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {

            if (compound_Main.compound_select == 3) //オリジナル調合のときの処理
            {
                if (pitemlistController.final_select_flag == true) //最後、これで調合するかどうかを待つフラグ
                {

                    SelectPaused();

                    StartCoroutine("Final_select");
                }
            }

            if (compound_Main.compound_select == 2) //トッピング調合のときの処理
            {
                if (pitemlistController.final_select_flag == true) //最後、これで調合するかどうかを待つフラグ
                {

                    SelectPaused();

                    StartCoroutine("topping_Final_select");
                }
            }

        }
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            

            if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理
            {

                itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                compound_Main.compound_status = 100; //トグルを押して、調合中の状態。All_cancelで、status=4に戻る。status=4でキャンセルすると、最初の調合選択シーンに戻る。

                // オリジナル調合を選択した場合の処理
                if ( compound_Main.compound_select == 3 )
                {
                    yes.SetActive(true);

                    compound_active();
                }

                // トッピング調合を選択した場合の処理
                if (compound_Main.compound_select == 2)
                {
                    yes.SetActive(true);

                    compound_topping_active();
                }

                // 「焼く」を選択した場合の処理
                if (compound_Main.compound_select == 5)
                {
                    yes.SetActive(true);

                    compound_roast_active();
                }

                // お菓子を「あげる」を選択した場合の処理
                if (compound_Main.compound_select == 10)
                {
                    yes.SetActive(true);

                    Girl_present();
                }

                // 単にメニューを開いたとき
                if (compound_Main.compound_select == 99)
                {
                    Player_ItemList_Open();
                }
            }

            else if (SceneManager.GetActiveScene().name == "GirlEat") // 女の子にアイテムあげるシーンでやりたい処理
            {

                itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                girleat_active();
            }

            else if (SceneManager.GetActiveScene().name == "QuestBox") // クエスト納品でやりたい処理
            {

                itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                qbox_active();
            }

            else // その他シーンでやりたい処理
            {
                //アイテム画面を表示した時などで使用

                Player_ItemList_Open();

            }
        }

    }
    
    //アイテム画面を開いた時の処理。アイテムを選択すると、カードを表示する。
    void Player_ItemList_Open()
    {
        no.SetActive(true);

        count = 0;

        while (count < pitemlistController._listitem.Count)
        {
            if (count != pitemlistController._count1)
            {
                selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                if (selectToggle == true) break;
            }
            ++count;
        }

        pitemlistController._listitem[count].GetComponent<Toggle>().interactable = false;

        //リスト中の選択された番号を格納。
        pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;


        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        pitemlistController._count1 = count;

        itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID; //itemID_1という変数に、プレイヤーが一個目に選択したアイテムIDを格納する。
        pitemlistController.final_kettei_item1 = itemID_1;

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1);

        //あらためて新しく押されたやつ以外の表示をリセットする。
        count = 0;

        while (count < pitemlistController._listitem.Count)
        {
            if (count != pitemlistController._count1)
            {
                pitemlistController._listitem[count].GetComponent<Toggle>().isOn = false;
                pitemlistController._listitem[count].GetComponent<Toggle>().interactable = true;
            }
            ++count;
        }
    }



    /* ### 調合シーンの処理 ### */

    public void compound_active()
    {
        switch (pitemlistController.kettei1_bunki)
        {
            case 0: //何も選択されていない時

                //一個目のアイテムを選択したときの処理（トグルの処理）

                count = 0;

                while (count < pitemlistController._listitem.Count)
                {
                    selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                    if (selectToggle == true) break;
                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._count1 = count;

                itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID; //itemID_1という変数に、プレイヤーが一個目に選択したアイテムIDを格納する。
                pitemlistController.final_kettei_item1 = itemID_1;

                //押したタイミングで、分岐＝１に。
                pitemlistController.kettei1_bunki = 1;

                //もし生地アイテムを一個目に選んだ場合、生地にアイテムを混ぜ込む処理になる。
                if (database.items[itemID_1].itemType_sub == Item.ItemType_sub.Pate)
                {
                    _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";
                }
                else
                {
                    _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";
                }


                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + itemID_1 + " " + database.items[itemID_1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示。トグルタイプとリスト番号を入れると、表示してくれる。
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("itemselect_kakunin_one");
                break;

            case 1: //一個目が選択されている時

                //二個目のアイテムを選択したときの処理を書く（トグルの処理）

                count = 0;

                selectToggle = false;

                while (count < pitemlistController._listitem.Count)
                {
                    if (count != pitemlistController._count1)
                    {
                        selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                        if (selectToggle == true) break;
                    }

                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.kettei_item2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._toggle_type2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._count2 = count;

                itemID_2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
                pitemlistController.final_kettei_item2 = itemID_2;

                //押したタイミングで、分岐＝２に。
                pitemlistController.kettei1_bunki = 2;

                _text.text = database.items[itemID_2].itemNameHyouji + "が選択されました。個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("2個目　アイテムID:" + itemID_2 + " " + database.items[itemID_2].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView02(pitemlistController._toggle_type2, pitemlistController.kettei_item2); //選択したアイテム2枚目をカードで表示
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("itemselect_kakunin_two");
                break;

            case 2: //二個目が選択されている時

                //三個目のアイテムを選択したときの処理を書く（トグルの処理）

                count = 0;

                selectToggle = false;

                while (count < pitemlistController._listitem.Count)
                {
                    if (count != pitemlistController._count1)
                    {
                        if (count != pitemlistController._count2)
                        {
                            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                            if (selectToggle == true) break;
                        }
                    }

                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.kettei_item3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._toggle_type3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._count3 = count;

                itemID_3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
                pitemlistController.final_kettei_item3 = itemID_3;

                //押したタイミングで、分岐＝３に。
                pitemlistController.kettei1_bunki = 3;

                _text.text = database.items[itemID_3].itemNameHyouji + "が選択されました。個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("3個目　アイテムID:" + itemID_3 + " " + database.items[itemID_3].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView03(pitemlistController._toggle_type3, pitemlistController.kettei_item3); //選択したアイテム2枚目をカードで表示
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("itemselect_kakunin_three");
                break;
        }
    }



    IEnumerator itemselect_kakunin_one()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除

                itemselect_cancel.update_ListSelect_Flag = 1; //一個目を選択したものを選択できないようにするときの番号。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                card_view.OKCard_DrawView();

                yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                if (database.items[itemID_1].itemType_sub == Item.ItemType_sub.Pate)
                {
                    _text.text = "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "　生地にアイテムを混ぜます" + "\n" + "二個目を選択してください。";
                }
                else
                {
                    _text.text = "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目を選択してください。";
                }
                //Debug.Log("一個目選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                itemselect_cancel.All_cancel();

                break;
        }

    }

    

    IEnumerator itemselect_kakunin_two()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");
                //解除

                itemselect_cancel.update_ListSelect_Flag = 2; //二個目まで、選択できないようにする。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                card_view.OKCard_DrawView02();

                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                pitemlistController.final_kettei_kosu2 = updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                yes_text.text = "調合する";

                _text.text = "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目: " + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";
                //Debug.Log("二個目選択完了！");
                break;

            case false:

                //Debug.Log("二個目はcancel"); 

                itemselect_cancel.Two_cancel();

                break;
        }


    }

    IEnumerator itemselect_kakunin_three()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        pitemlistController.final_select_flag = false;

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");

                //Debug.Log("三個目選択完了！");

                itemselect_cancel.update_ListSelect_Flag = 3; //二個目まで、選択できないようにする。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                updown_counter_obj.SetActive(false);

                card_view.OKCard_DrawView03();

                pitemlistController.final_kettei_kosu3 = updown_counter.updown_kosu;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                pitemlistController.final_select_flag = true; //最後調合するかどうかのフラグをオンに。

                break;

            case false:

                //Debug.Log("三個目はcancel");

                itemselect_cancel.Three_cancel();
                break;
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

                pitemlistController.final_select_flag = false;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:

                        //選んだ二つをもとに、一つのアイテムを生成する。そして、調合完了！

                        if (judge_flag == 0)
                        {
                            exp_Controller.compound_success = true;
                        }
                        else if (judge_flag == 1)
                        {
                            //調合成功の判定
                            CompoundSuccess_judge();

                            if (compoundsuccess_flag == true)
                            {
                                exp_Controller.compound_success = true;

                            }
                            else if (compoundsuccess_flag == false)
                            {
                                exp_Controller.compound_success = false;
                                pitemlistController.result_item = database.trash_ID_1; //失敗したので、ゴミが入る。

                            }
                        }

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.result_ok = true; //調合完了のフラグをたてておく。

                        exp_Controller.extreme_on = false;

                        compound_Main.compound_status = 4;

                        Off_Flag_Setting();

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

                pitemlistController.final_select_flag = false;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:
                        //選んだ三つをもとに、一つのアイテムを生成する。

                        if (judge_flag == 0)
                        {
                            exp_Controller.compound_success = true;
                        }
                        else if (judge_flag == 1)
                        {

                            //調合成功の判定
                            CompoundSuccess_judge();

                            if (compoundsuccess_flag == true)
                            {
                                exp_Controller.compound_success = true;

                            }
                            else if (compoundsuccess_flag == false)
                            {
                                exp_Controller.compound_success = false;
                                pitemlistController.result_item = database.trash_ID_1; //失敗したので、ゴミが入る。

                            }
                        }

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.result_ok = true; //調合完了のフラグをたてておく。

                        exp_Controller.extreme_on = false;

                        compound_Main.compound_status = 4;

                        Off_Flag_Setting();


                        break;

                    case false:

                        //Debug.Log("三個目はcancel");

                        itemselect_cancel.Three_cancel();

                        break;
                }
                break;
        }
    }

    

    void CompoundMethod()
    {
        _itemIDtemp_result.Clear();
        _itemSubtype_temp_result.Clear();

        //オリジナル調合の場合はこっち
        if (pitemlistController.kettei1_bunki == 2 || pitemlistController.kettei1_bunki == 3)
        {
            _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item1].itemName);
            _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item2].itemName);

            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item1].itemType_sub.ToString());
            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item2].itemType_sub.ToString());

            if (pitemlistController.final_kettei_item3 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                pitemlistController.final_kettei_kosu3 = 9999; //個数にも9999=emptyを入れる。
            }
            else
            {
                _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item3].itemName);
                _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item3].itemType_sub.ToString());
            }
        }

        //エクストリーム調合の場合は、こっち。ベース決定アイテムを、temp_resultに入れる。
        else if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
        {
            _itemIDtemp_result.Add(database.items[pitemlistController.final_base_kettei_item].itemName);
            _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item1].itemName);

            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_base_kettei_item].itemType_sub.ToString());
            _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item1].itemType_sub.ToString());

            if (pitemlistController.final_kettei_item2 == 9999) //二個しか選択していないときは、9999が入っている。
            {
                _itemIDtemp_result.Add("empty");
                _itemSubtype_temp_result.Add("empty");
                pitemlistController.final_kettei_kosu2 = 9999; //個数にも9999=emptyを入れる。
            }
            else
            {
                _itemIDtemp_result.Add(database.items[pitemlistController.final_kettei_item2].itemName);
                _itemSubtype_temp_result.Add(database.items[pitemlistController.final_kettei_item2].itemType_sub.ToString());
            }
        }


        i = 0;

        resultitemID = "gomi_1"; //どの調合組み合わせのパターンにも合致しなかった場合は、ゴミのIDが入っている。調合DBのゴミのitemNameを入れると、後で数値に変換してくれる。現在は、500に変換される。

        compoDB_select_judge = false;

        //判定処理//

        //一個目に選んだアイテムが生地タイプでもなく、フルーツ同士の合成でもない場合、
        //新規作成のため、以下の判定処理を行う。個数は、判定に関係しない。


        //①固有の名称同士の組み合わせか、②固有＋サブの組み合わせか、③サブ同士のジャンルで組み合わせが一致していれば、制作する。

            while (i < databaseCompo.compoitems.Count)
            {

                if (databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[0] && databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[1] && databaseCompo.compoitems[i].cmpitemID_3 == _itemIDtemp_result[2])
                {
                //if (databaseCompo.compoitems[i].cmpitem_kosu1 == pitemlistController.final_kettei_kosu1 && databaseCompo.compoitems[i].cmpitem_kosu2 == pitemlistController.final_kettei_kosu2 && databaseCompo.compoitems[i].cmpitem_kosu3 == pitemlistController.final_kettei_kosu3)
                //{
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[0][1][2]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                           //Debug.Log("[0][1][2]と合致");
                        break;
                    //}
                }
                else if (databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[0] && databaseCompo.compoitems[i].cmpitemID_3 == _itemIDtemp_result[1] && databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[2])
                {
                //if (databaseCompo.compoitems[i].cmpitem_kosu1 == pitemlistController.final_kettei_kosu1 && databaseCompo.compoitems[i].cmpitem_kosu3 == pitemlistController.final_kettei_kosu2 && databaseCompo.compoitems[i].cmpitem_kosu2 == pitemlistController.final_kettei_kosu3)
                //{
                        compoDB_select_judge = true;
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[0][2][1]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                           //Debug.Log("[0][2][1]と合致");
                        break;
                    //}
                }
                else if (databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[0] && databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[1] && databaseCompo.compoitems[i].cmpitemID_3 == _itemIDtemp_result[2])
                {
                //if (databaseCompo.compoitems[i].cmpitem_kosu2 == pitemlistController.final_kettei_kosu1 && databaseCompo.compoitems[i].cmpitem_kosu1 == pitemlistController.final_kettei_kosu2 && databaseCompo.compoitems[i].cmpitem_kosu3 == pitemlistController.final_kettei_kosu3)
                //{
                        compoDB_select_judge = true;
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[1][0][2]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                           //Debug.Log("[1][0][2]と合致");
                        break;
                    //}
                }
                else if (databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[0] && databaseCompo.compoitems[i].cmpitemID_3 == _itemIDtemp_result[1] && databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[2])
                {
                //if (databaseCompo.compoitems[i].cmpitem_kosu2 == pitemlistController.final_kettei_kosu1 && databaseCompo.compoitems[i].cmpitem_kosu3 == pitemlistController.final_kettei_kosu2 && databaseCompo.compoitems[i].cmpitem_kosu1 == pitemlistController.final_kettei_kosu3)
                //{
                        compoDB_select_judge = true;
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[1][2][0]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                           //Debug.Log("[1][2][0]と合致");
                        break;
                    //}
                }
                else if (databaseCompo.compoitems[i].cmpitemID_3 == _itemIDtemp_result[0] && databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[1] && databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[2])
                {
                //if (databaseCompo.compoitems[i].cmpitem_kosu3 == pitemlistController.final_kettei_kosu1 && databaseCompo.compoitems[i].cmpitem_kosu1 == pitemlistController.final_kettei_kosu2 && databaseCompo.compoitems[i].cmpitem_kosu2 == pitemlistController.final_kettei_kosu3)
                //{
                        compoDB_select_judge = true;
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[2][0][1]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                           //Debug.Log("[2][0][1]と合致");
                        break;
                    //}
                }
                else if (databaseCompo.compoitems[i].cmpitemID_3 == _itemIDtemp_result[0] && databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[1] && databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[2])
                {
                //if (databaseCompo.compoitems[i].cmpitem_kosu3 == pitemlistController.final_kettei_kosu1 && databaseCompo.compoitems[i].cmpitem_kosu2 == pitemlistController.final_kettei_kosu2 && databaseCompo.compoitems[i].cmpitem_kosu1 == pitemlistController.final_kettei_kosu3)
                //{
                        compoDB_select_judge = true;
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[2][1][0]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                           //Debug.Log("[2][1][0]と合致");
                        break;
                    //}
                }

                ++i;
            }

        //②　①の組み合わせにない場合は、2通りが考えられる。　アイテム名＋サブ＋サブ　か　アイテム名＋アイテム名＋サブの組み合わせ
        if (compoDB_select_judge == false)
        {
            i = 0;

            while (i < databaseCompo.compoitems.Count)
            {
                //コンポDBの一個目のアイテム名と選んだ3アイテムの名前が一致するかどうかを見る。

                if (databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[0]) //一個目に選択したのが一致してた
                {
                    //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                    if (databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[1])
                    {
                        //アイテム名＋アイテム名＋サブ(サブが空の場合もある。）
                        //3個目がサブ
                        if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[2]) 
                        {
                            compoDB_select_judge = true; //一致するものがあった場合は、true
                            resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                            result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                            break;
                        }
                    }
                    else if (_itemIDtemp_result[2] != "empty" && databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[2])
                    {
                        //アイテム名＋サブ＋アイテム名
                        //2個目がサブ
                        if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1]) 
                        {
                            compoDB_select_judge = true; //一致するものがあった場合は、true
                            resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                            result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                            break;
                        }
                    }
                    //2・3個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                    else if (databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[2])
                    {
                        //アイテム名＋サブ＋サブ。
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                        break;
                    }
                    else if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[2])
                    {
                        //アイテム名＋サブ＋サブ。
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                        break;
                    }
                }

                else if (databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[1]) //二個目に選択したのが一致してた
                {
                    //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                    if (databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[0])
                    {
                        //アイテム名＋アイテム名＋サブ
                        //3個目がサブ
                        if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[2]) //3個目のサブも一致していた
                        {
                            compoDB_select_judge = true; //一致するものがあった場合は、true
                            resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                            result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                            break;
                        }
                    }
                    else if (_itemIDtemp_result[2] != "empty" && databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[2])
                    {
                        //アイテム名＋サブ＋アイテム名
                        //2個目がサブ
                        if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[0]) //2個目のサブも一致していた
                        {
                            compoDB_select_judge = true; //一致するものがあった場合は、true
                            resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                            result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                            break;
                        }
                    }
                    //1・3個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                    else if (databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[2])
                    {
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                        break;
                    }
                    else if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[2])
                    {
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                        break;
                    }
                }

                else if (databaseCompo.compoitems[i].cmpitemID_1 == _itemIDtemp_result[2]) //三個目に選択したのが一致してた
                {
                    //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                    if (databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[0])
                    {
                        //アイテム名＋アイテム名＋サブ
                        //3個目がサブ
                        if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1])
                        {
                            compoDB_select_judge = true; //一致するものがあった場合は、true
                            resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                            result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                            break;
                        }
                    }
                    else if (databaseCompo.compoitems[i].cmpitemID_2 == _itemIDtemp_result[1])
                    {
                        //アイテム名＋サブ＋アイテム名
                        //2個目がサブ
                        if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[0])
                        {
                            compoDB_select_judge = true; //一致するものがあった場合は、true
                            resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                            result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                            break;
                        }
                    }
                    //1・2個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                    if (databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1])
                    {
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                        break;
                    }
                    //一致してた場合、残りの2個のサブタイプを見る
                    if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[0])
                    {
                        compoDB_select_judge = true; //一致するものがあった場合は、true
                        resultitemID = databaseCompo.compoitems[i].cmpitemID_result;
                        result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。

                        break;
                    }
                }
                ++i;
            }
        }

        //③固有の組み合わせがなかった場合のみ、ジャンル同士の組み合わせがないかも見る。

        if (compoDB_select_judge == false)
        {
            subtype_Kensaku();
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

            //Debug.Log("調合DBに該当する。");

            
            //一個目が生地ではなく、小麦粉も使われていない。全く新しいアイテムが生成される。
            {
                exp_Controller.comp_judge_flag = 0; //新規調合の場合は0にする。

                if (databaseCompo.compoitems[pitemlistController.result_compID].success_Rate >= 0 && databaseCompo.compoitems[pitemlistController.result_compID].success_Rate < 20)
                {
                    //成功率超低い
                    success_text = "これは.. 奇跡が起こればあるいは・・。";
                }
                else if (databaseCompo.compoitems[pitemlistController.result_compID].success_Rate >= 20 && databaseCompo.compoitems[pitemlistController.result_compID].success_Rate < 40)
                {
                    //成功率低め
                    success_text = "かなりきつい・・かも。";
                }
                else if (databaseCompo.compoitems[pitemlistController.result_compID].success_Rate >= 40 && databaseCompo.compoitems[pitemlistController.result_compID].success_Rate < 60)
                {
                    //普通
                    success_text = "頑張れば、いける・・！";
                }
                else if (databaseCompo.compoitems[pitemlistController.result_compID].success_Rate >= 60 && databaseCompo.compoitems[pitemlistController.result_compID].success_Rate < 80)
                {
                    //成功率高め
                    success_text = "問題なくいけそうだね。";
                }
                else if (databaseCompo.compoitems[pitemlistController.result_compID].success_Rate >= 80 && databaseCompo.compoitems[pitemlistController.result_compID].success_Rate < 100)
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
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1; //1の場合、生地にアイテムを合成する処理のフラグ
                            break;

                        case Item.ItemType_sub.Nuts:

                            success_text = "生地にアイテムを合成します。";
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            break;

                        case Item.ItemType_sub.Suger:

                            success_text = "生地にアイテムを合成します。";
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            break;

                        case Item.ItemType_sub.Komugiko:

                            success_text = "生地にアイテムを合成します。";
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            break;

                        case Item.ItemType_sub.Butter:

                            success_text = "生地にアイテムを合成します。";
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            break;

                        case Item.ItemType_sub.Source:

                            success_text = "生地にアイテムを合成します。";
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            break;

                        case Item.ItemType_sub.Potion:

                            success_text = "生地にアイテムを合成します。";
                            judge_flag = 0; //必ず成功する
                            exp_Controller.comp_judge_flag = 1;
                            break;

                        default:

                            //2個目のアイテムが、上記のパターンにあてはまらない場合は、失敗する。

                            judge_flag = 1; //成功判定の処理をON
                            compoundsuccess_flag = false;
                            success_text = "これは.. ダメかもしれぬ。";
                            break;
                    }

                }

                //一個目が生地でない場合、
                //DBにも登録されておらず、生地への合成でもないので、失敗する。

                else
                {
                    //失敗
                    judge_flag = 1; //成功判定の処理をON
                    compoundsuccess_flag = false;
                    success_text = "これは.. ダメかもしれぬ。";
                }
            }

            //エクストリーム調合の場合は、通常通りトッピングの処理を行う。
            else if (pitemlistController.kettei1_bunki == 11 || pitemlistController.kettei1_bunki == 12)
            {

            }
        }
   
        //判定予測処理　ここまで//
    }

    void subtype_Kensaku()
    {
        i = 0;

        while (i < databaseCompo.compoitems.Count)
        {
            //Debug.Log("調合用アイテムサブタイプDB: 1: " + databaseCompo.compoitems[i].cmp_subtype_1 + " 2: " + databaseCompo.compoitems[i].cmp_subtype_2 + " 3: " + databaseCompo.compoitems[i].cmp_subtype_3);
            //Debug.Log(i);


            if (databaseCompo.compoitems[i].cmp_subtype_1 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[2])
            {

                compoDB_select_judge = true; //一致するものがあった場合は、true
                resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[0][1][2]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                   //Debug.Log("[0][1][2]と合致");
                break;

            }
            else if (databaseCompo.compoitems[i].cmp_subtype_1 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[2])
            {

                compoDB_select_judge = true;
                resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[0][2][1]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                   //Debug.Log("[0][2][1]と合致");
                break;

            }
            else if (databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_1 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[2])
            {

                compoDB_select_judge = true;
                resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[1][0][2]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                   //Debug.Log("[1][0][2]と合致");
                break;

            }
            else if (databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_1 == _itemSubtype_temp_result[2])
            {

                compoDB_select_judge = true;
                resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[1][2][0]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                   //Debug.Log("[1][2][0]と合致");
                break;

            }
            else if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_1 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[2])
            {

                compoDB_select_judge = true;
                resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[2][0][1]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                   //Debug.Log("[2][0][1]と合致");
                break;

            }
            else if (databaseCompo.compoitems[i].cmp_subtype_3 == _itemSubtype_temp_result[0] && databaseCompo.compoitems[i].cmp_subtype_2 == _itemSubtype_temp_result[1] && databaseCompo.compoitems[i].cmp_subtype_1 == _itemSubtype_temp_result[2])
            {

                compoDB_select_judge = true;
                resultitemID = databaseCompo.compoitems[i].cmpitemID_result; //[2][1][0]の組み合わせ。3つの値が一致したときの、リザルトアイテムIDを決定。
                result_compoID = i;//そのときのコンポデータベースの配列も、一緒に記録。
                                   //Debug.Log("[2][1][0]と合致");
                break;
            }

            ++i;
        }
    }

    void CompoundSuccess_judge()
    {
        if (pitemlistController.result_item == 500) //調合リストに含まれていないため、失敗が分かっているときの処理
        {
            _final_success_rate = 0;
            compoundsuccess_flag = false;
        }
        else //調合リストに合致し、最後に調合成功か失敗かを判定
        {
            _success_rate = databaseCompo.compoitems[pitemlistController.result_compID].success_Rate;

            _final_success_rate = _success_rate + (PlayerStatus.player_renkin_lv);

            dice = Random.Range(1, 100); //1~100までのサイコロをふる。

            Debug.Log("最終成功確率: " + _final_success_rate + " " + "ダイスの目: " + dice);

            if (dice <= (int)_final_success_rate) //出た目が、成功率より下なら成功
            {
                compoundsuccess_flag = true;
            }
            else //失敗
            {
                compoundsuccess_flag = false;
            }
        }
    }

    //オリジナル調合はここまで




    /* ### トッピング調合シーンの処理 ### */

    public void compound_topping_active()
    {
        //最初に、ベースとなるアイテムを一つ選択する。その後、トッピングアイテムを選ぶ（３つまで。好きな数でよい）

        switch (pitemlistController.kettei1_bunki)
        {
            case 0: //何も選択されていない時

                //ベースアイテムを選択する処理。
                count = 0;

                while (count < pitemlistController._listitem.Count)
                {
                    selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                    if (selectToggle == true) break;
                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.base_kettei_item = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._base_toggle_type = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._base_count = count;

                baseitemID = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID; //baseitemIDに、プレイヤーが一個目に選択したアイテムIDを格納する。
                pitemlistController.final_base_kettei_item = baseitemID; //最終的にできあがるベースアイテムの、アイテムID。

                pitemlistController.kettei1_bunki = 10;

                _text.text = database.items[baseitemID].itemNameHyouji + "をベースにします。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + baseitemID + " " + database.items[baseitemID].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView(pitemlistController._base_toggle_type, pitemlistController.base_kettei_item); //選択したアイテムをカードで表示

                SelectPaused();

                StartCoroutine("itemselect_kakunin_baseitem");
                break;

            case 10:

                //トッピング一個目のアイテムを選択したときの処理（トグルの処理）

                count = 0;

                while (count < pitemlistController._listitem.Count)
                {
                    //if (count != pitemlistController._base_count)
                    //{
                        selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                        if (selectToggle == true) break;
                    //}
                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._count1 = count;

                itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID; //itemID_1という変数に、プレイヤーが一個目に選択したアイテムIDを格納する。
                pitemlistController.final_kettei_item1 = itemID_1;

                pitemlistController.kettei1_bunki = 11;

                _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + itemID_1 + " " + database.items[itemID_1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView02(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("topping_itemselect_kakunin_one");
                break;

            case 11:

                //トッピング二個目のアイテムを選択したときの処理を書く（トグルの処理）

                count = 0;

                selectToggle = false;

                while (count < pitemlistController._listitem.Count)
                {
                    //if (count != pitemlistController._base_count)
                    //{
                        if (count != pitemlistController._count1)
                        {
                            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                            if (selectToggle == true) break;
                        }
                    //}

                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.kettei_item2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._toggle_type2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._count2 = count;

                itemID_2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
                pitemlistController.final_kettei_item2 = itemID_2;

                pitemlistController.kettei1_bunki = 12;

                _text.text = database.items[itemID_2].itemNameHyouji + "が選択されました。個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("2個目　アイテムID:" + itemID_2 + " " + database.items[itemID_2].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView03(pitemlistController._toggle_type2, pitemlistController.kettei_item2); //選択したアイテム2枚目をカードで表示
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("topping_itemselect_kakunin_two");
                break;

            case 12:

                //トッピング三個目のアイテムを選択したときの処理を書く（トグルの処理）

                count = 0;

                selectToggle = false;

                while (count < pitemlistController._listitem.Count)
                {
                    //if (count != pitemlistController._base_count)
                    //{
                        if (count != pitemlistController._count1)
                        {
                            if (count != pitemlistController._count2)
                            {
                                selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                                if (selectToggle == true) break;
                            }
                        }
                    //}

                    ++count;
                }

                //リスト中の選択された番号を格納。
                pitemlistController.kettei_item3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                pitemlistController._toggle_type3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;


                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                pitemlistController._count3 = count;

                itemID_3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
                pitemlistController.final_kettei_item3 = itemID_3;

                pitemlistController.kettei1_bunki = 13;

                _text.text = database.items[itemID_3].itemNameHyouji + "が選択されました。個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("3個目　アイテムID:" + itemID_3 + " " + database.items[itemID_3].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView04(pitemlistController._toggle_type3, pitemlistController.kettei_item3); //選択したアイテム2枚目をカードで表示
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("topping_itemselect_kakunin_three");
                break;

            default:
                break;
        }
    }



    //トッピング調合のときのみ、使うメソッド。ベースアイテムこれでいいですか？

    IEnumerator itemselect_kakunin_baseitem()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                pitemlistController.topping_DrawView_2(); //リストビューを更新し、トッピング材料だけ表示する。

                //Debug.Log("ok");

                itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                card_view.OKCard_DrawView();

                yes.SetActive(false);
                //no.SetActive(false);
                //updown_counter_obj.SetActive(false);

                pitemlistController.final_base_kettei_kosu = 1; //updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                _text.text = "ベースアイテム: " + database.items[baseitemID].itemNameHyouji + " " + "1個" + "\n" + "トッピングアイテム一個目を選択してください。";
                //Debug.Log("ベースアイテム選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                itemselect_cancel.All_cancel();
                break;
        }

    }


    IEnumerator topping_itemselect_kakunin_one()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除
                itemselect_cancel.update_ListSelect_Flag = 11; //ベースアイテムと一個目を選択できないようにする。
                itemselect_cancel.update_ListSelect();

                card_view.OKCard_DrawView02();
                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                //pitemlistController.final_kettei_item1 = itemID_1;
                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                yes_text.text = "調合する";

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目を選択してください。";
                //Debug.Log("二個目選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                itemselect_cancel.Two_cancel();

                break;
        }

    }


    IEnumerator topping_itemselect_kakunin_two()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");
                //解除
                itemselect_cancel.update_ListSelect_Flag = 12; //ベースアイテムと一個目・二個目を選択できないようにする。
                itemselect_cancel.update_ListSelect();

                card_view.OKCard_DrawView03();
                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                pitemlistController.final_kettei_kosu2 = updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;


                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                yes_text.text = "調合する";

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目: " + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";
                //Debug.Log("三個目選択完了！");
                break;

            case false:

                //Debug.Log("二個目はcancel");

                itemselect_cancel.Three_cancel();

                break;
        }


    }

    IEnumerator topping_itemselect_kakunin_three()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        pitemlistController.final_select_flag = false;

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");

                //Debug.Log("三個目選択完了！");

                itemselect_cancel.update_ListSelect_Flag = 13; //ベースアイテムと一個目・二個目・三個目を選択できないようにする。
                itemselect_cancel.update_ListSelect();

                updown_counter_obj.SetActive(false);

                card_view.OKCard_DrawView04();

                pitemlistController.final_kettei_kosu3 = updown_counter.updown_kosu;

                pitemlistController.final_select_flag = true; //最後調合するかどうかのフラグをオンに。


                yes_text.text = "トッピング開始！";
                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                break;

            case false:

                //Debug.Log("三個目はcancel");

                itemselect_cancel.Four_cancel();

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
                        if (compoDB_select_judge == true)
                        {
                            exp_Controller.compound_success = true;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                            exp_Controller.extreme_on = true;
                        }

                        //コンポDBに該当していなければ、通常通りトッピングの処理
                        else
                        {
                            exp_Controller.compound_success = true;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                            exp_Controller.extreme_on = false;
                        }

                        
                        compound_Main.compound_status = 4;

                        Off_Flag_Setting();

                        break;

                    case false:

                        //Debug.Log("ベースアイテムを選択した状態に戻る");

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
                        if (compoDB_select_judge == true)
                        {
                            exp_Controller.compound_success = true;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                            exp_Controller.extreme_on = true;
                        }

                        //コンポDBに該当していなければ、通常通りトッピングの処理
                        else
                        {
                            exp_Controller.compound_success = true;

                            //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                            exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                            exp_Controller.extreme_on = false;
                        }

                        compound_Main.compound_status = 4;

                        Off_Flag_Setting();

                        break;

                    case false:

                        //Debug.Log("1個目を選択した状態に戻る");

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

                pitemlistController.final_select_flag = false;

                switch (yes_selectitem_kettei.kettei1)
                {
                    case true:
                        //選んだ三つをもとに、一つのアイテムを生成する。

                        /*//調合成功の判定
                        CompoundSuccess_judge();

                        if (compoundsuccess_flag == true)
                        {
                            exp_Controller.compound_success = true;

                        }
                        else if (compoundsuccess_flag == false)
                        {
                            exp_Controller.compound_success = false;
                            pitemlistController.result_item = database.trash_ID_1; //失敗したので、ゴミが入る。

                        }*/

                        exp_Controller.compound_success = true;

                        //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                        exp_Controller.topping_result_ok = true; //調合完了のフラグをたてておく。

                        compound_Main.compound_status = 4;

                        Off_Flag_Setting();

                        break;

                    case false:

                        //Debug.Log("2個目を選択した状態に戻る");

                        itemselect_cancel.Four_cancel();

                        break;
                }
                break;
        }
    }



    /* ### 「焼く」処理 ### */

    public void compound_roast_active()
    {
        count = 0;

        while (count < pitemlistController._listitem.Count)
        {
            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        //リスト中の選択された番号を格納。
        pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;


        //表示中リストの、リスト番号を保存。
        pitemlistController._count1 = count;

        itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID; //itemID_1という変数に、プレイヤーが一個目に選択したアイテムIDを格納する。
        pitemlistController.final_kettei_item1 = itemID_1;

        pitemlistController.kettei1_bunki = 9999; //分岐なし。テキストの更新を避けるため、とりあえず適当な数字を入れて回避。

        _text.text = database.items[itemID_1].itemNameHyouji + "を焼きますか？○○時間かかります。";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("1個目　アイテムID:" + itemID_1 + " " + database.items[itemID_1].itemNameHyouji + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示
        updown_counter_obj.SetActive(true);

        SelectPaused();

        StartCoroutine("roast_Final_select");
    }

    IEnumerator roast_Final_select()
    {    

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //選んだ生地を焼き、お菓子を作成。
                exp_Controller.compound_success = true;

                //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                exp_Controller.roast_result_ok = true; //調合完了のフラグをたてておく。

                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu;

                compound_Main.compound_status = 4;

                Off_Flag_Setting();

                break;

            case false:

                //Debug.Log("一個目はcancel");

                _text.text = "焼きたい生地を選択してください。";

                itemselect_cancel.All_cancel();
                break;
     
        }
    }


    /* ### 調合シーンで、女の子にお菓子をあげる処理 ＜エクストリーム調合に方針変え＞ ### */

    public void Girl_present()
    {
        count = 0;

        while (count < pitemlistController._listitem.Count)
        {
            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        //リスト中の選択された番号を格納。
        pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;


        //表示中リストの、リスト番号を保存。
        pitemlistController._count1 = count;

        itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID; //itemID_1という変数に、プレイヤーが一個目に選択したアイテムIDを格納する。
        pitemlistController.final_kettei_item1 = itemID_1;

        pitemlistController.kettei1_bunki = 9999; //分岐なし。テキストの更新を避けるため、とりあえず適当な数字を入れて回避。

        _text.text = database.items[itemID_1].itemNameHyouji + "をあげますか？";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("1個目　アイテムID:" + itemID_1 + " " + database.items[itemID_1].itemNameHyouji + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示

        SelectPaused();

        StartCoroutine("Girl_present_Final_select");
    }

    IEnumerator Girl_present_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //女の子にアイテムをあげる処理
                compound_Main.compound_status = 11; //status=11で処理。

                Off_Flag_Setting();

                break;

            case false:

                //Debug.Log("一個目はcancel");

                //_text.text = "焼きたい生地を選択してください。";

                itemselect_cancel.All_cancel();
                break;

        }
    }





    /* ### 女の子にあげるときのシーン ### */

    public void girleat_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;
        //max = pitemlist.playeritemlist.Count; //現在のプレイヤーアイテムリストの最大数を更新

        while (count < pitemlistController._listitem.Count)
        {
            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        //リスト中の選択された番号を格納。
        pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;


        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        pitemlistController._count1 = count;

        itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
        pitemlistController.final_kettei_item1 = itemID_1;//選択したアイテムの、アイテムIDを格納しておく。

        //pitemlistController.cardImage_onoff_pcontrol.SetActive(true);
        _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。これでいいですか？";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + itemID_1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示

        SelectPaused();

        StartCoroutine("girlselect_kakunin");

    }

    IEnumerator girlselect_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された　＝　あげる処理へ。プレイヤーリストコントローラー側で処理してる。

                //Debug.Log("ok");

                text_area.SetActive(false);

                exp_Controller.girleat_ok = true; //ガールにアイテムあげた完了のフラグをたてておく。

                Off_Flag_Setting();

                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                _text.text = "女の子にあげるアイテムを選択してください。";

                itemselect_cancel.All_cancel();
                break;
        }
    }





    /* ### クエストボックスのシーン ### */

    public void qbox_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < pitemlistController._listitem.Count)
        {
            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        //リスト中の選択された番号を格納。
        pitemlistController.kettei_item1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        pitemlistController._toggle_type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;


        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        pitemlistController._count1 = count;

        itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
        pitemlistController.final_kettei_item1 = itemID_1;//選択したアイテムの、アイテムIDを格納しておく。

        //pitemlistController.cardImage_onoff_pcontrol.SetActive(true);
        _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。これでいいですか？";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + itemID_1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示
        updown_counter_obj.SetActive(true);

        SelectPaused();

        StartCoroutine("qbox_select_kakunin");

    }

    IEnumerator qbox_select_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された　＝　あげる処理へ。プレイヤーリストコントローラー側で処理してる。

                //Debug.Log("ok");

                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu;

                exp_Controller.qbox_ok = true;

                Off_Flag_Setting();
                
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                _text.text = "お菓子を選択してください。";

                itemselect_cancel.All_cancel();
                break;
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

        card_view.DeleteCard_DrawView();

        pitemlistController.kettei1_bunki = 0;
        //pitemlistController.kettei1_on = false;
        itemselect_cancel.kettei_on_waiting = false;

        updown_counter_obj.SetActive(false);

        yes_selectitem_kettei.kettei1 = false;
        yes.SetActive(false);
        //no.SetActive(false);
        //item_tsuika.SetActive(true);

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        //Debug.Log("選択完了！");
    }


    void SelectPaused()
    {
        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < pitemlistController._listitem.Count; i++)
        {
            pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false;
            //pitemlistController._listitem[i].GetComponentsInChildren<Text>()[0].color = new Color(132f / 255f, 68f / 255f, 205f / 255f);
            //pitemlistController._listitem[i].GetComponentsInChildren<Text>()[1].color = new Color(132f / 255f, 68f / 255f, 205f / 255f);
        }

        yes.SetActive(true);
        no.SetActive(true);
        //item_tsuika.SetActive(false);

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
}

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

    private GameObject canvas;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private CombinationMain Combinationmain;

    private GameObject compound_Check_obj;
    private Compound_Check compound_Check;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Shop_Main shop_Main;

    private GameObject GirlEat_scene_obj;
    private GirlEat_Main girlEat_scene;

    private GameObject questjudge_obj;
    private Quest_Judge questjudge;

    private GameObject kakuritsuPanel_obj;
    private KakuritsuPanel kakuritsuPanel;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private QuestSetDataBase quest_database;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private Sprite yes_sprite1;
    private Sprite yes_sprite2;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject item_tsuika; //PlayeritemList_ScrollViewの子オブジェクト「item_tsuika」ボタン

    private GameObject black_effect;

    private GameObject NouhinKetteiPanel_obj;

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

    private int kosusum;

    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string>  _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。

    private bool compoDB_select_judge;
    private string resultitemID;
    private int result_compoID;

    private int judge_flag; //調合判定を行うか否か
    private bool compoundsuccess_flag; //成功か失敗か
    private float _success_rate;
    private int dice;
    private string success_text;

    void Start()
    {
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //調合用メソッドの取得
        Combinationmain = CombinationMain.Instance.GetComponent<CombinationMain>();

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

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        switch (SceneManager.GetActiveScene().name) // 調合シーンでやりたい処理。
        {
            case "Compound":
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                kakuritsuPanel_obj = canvas.transform.Find("KakuritsuPanel").gameObject;
                kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();

                compound_Check_obj = GameObject.FindWithTag("Compound_Check");
                compound_Check = compound_Check_obj.GetComponent<Compound_Check>();

                break;

            case "Shop":

                shop_Main = GameObject.FindWithTag("Shop_Main").GetComponent<Shop_Main>();

                back_ShopFirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
                back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();
                break;
        }


        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();        

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
        updown_counter = updown_counter_obj.GetComponent<Updown_counter>();

        yes = pitemlistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = pitemlistController_obj.transform.Find("No").gameObject;
        yes.SetActive(false);

        yes_sprite1 = Resources.Load<Sprite>("Sprites/Window/miniwindowB");
        yes_sprite2 = Resources.Load<Sprite>("Sprites/Window/sabwindowA_pink_66");

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();
        
        item_tsuika = pitemlistController_obj.transform.Find("ItemADDbutton_Debug").gameObject;

        //テキストウィンドウの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();


        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();        

        i = 0;

        count = 0;
        judge_flag = 0;

        itemID_1 = 0;
        itemID_2 = 0;
       
    }


    void Update()
    {
        if (pitemlistController.shopsell_final_select_flag == true) //最後、これを売るかどうかを待つフラグ
        {
            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
            updown_button = updown_counter_obj.GetComponentsInChildren<Button>();

            pitemlistController.shopsell_final_select_flag = false;
            StartCoroutine("shop_sell_Final_select");
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
                if (compound_Main.compound_select == 3)
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

                }

                // 単にメニューを開いたとき
                if (compound_Main.compound_select == 99)
                {
                    Player_ItemList_Open();
                }
            }

            else if (SceneManager.GetActiveScene().name == "Shop") // ショップでやりたい処理
            {
                itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                back_ShopFirst_btn.interactable = false;

                switch (shop_Main.shop_scene)
                {

                    case 3: //納品時の画面開いた時
                        
                        NouhinKetteiPanel_obj = canvas.transform.Find("NouhinKetteiPanel").gameObject;

                        shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
                        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();

                        questjudge_obj = GameObject.FindWithTag("Quest_Judge");
                        questjudge = questjudge_obj.GetComponent<Quest_Judge>();

                        //黒半透明パネルの取得
                        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

                        nouhin_active(); //納品したいアイテムを、納品個数に達するまで、選択できる。か、一種類のみで、必要個数
                        break;

                    case 5: //売るとき

                        itemsell_active(); //アイテムを売る
                        break;
                }
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

        card_view.ItemListCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1);

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

                //もし生地アイテムを一個目に選んだ場合、生地にアイテムを混ぜ込む処理になる。現在は未使用。
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

                _text.text = database.items[itemID_2].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";

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

                _text.text = database.items[itemID_3].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";

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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除

                itemselect_cancel.update_ListSelect_Flag = 1; //一個目を選択したものを選択できないようにするときの番号。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu;
                card_view.OKCard_DrawView(pitemlistController.final_kettei_kosu1);

                yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);                

                itemselect_cancel.kettei_on_waiting = false;
                

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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");
                //解除

                itemselect_cancel.update_ListSelect_Flag = 2; //二個目まで、選択できないようにする。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                pitemlistController.final_kettei_kosu2 = updown_counter.updown_kosu;
                card_view.OKCard_DrawView02(pitemlistController.final_kettei_kosu2);

                yes.SetActive(true);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);               

                itemselect_cancel.kettei_on_waiting = false;
               
                yes_text.text = "作る";
                compound_Check.YesSetDesign2();

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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");

                //Debug.Log("三個目選択完了！");

                itemselect_cancel.update_ListSelect_Flag = 3; //二個目まで、選択できないようにする。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                updown_counter_obj.SetActive(false);

                pitemlistController.final_kettei_kosu3 = updown_counter.updown_kosu;
                card_view.OKCard_DrawView03(pitemlistController.final_kettei_kosu3);

                yes.SetActive(true);
                
                compound_Check.final_select_flag = true;


                break;

            case false:

                //Debug.Log("三個目はcancel");

                itemselect_cancel.Three_cancel();
                break;
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

                //トッピングの場合、このタイミングで確率も計算。一個目
                Compo_KakuritsuKeisan_1();

                _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。" + "\n" + "これでいいですか？";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + itemID_1 + " " + database.items[itemID_1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView02(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示
                updown_counter_obj.SetActive(false);

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

                //トッピングの場合、このタイミングで確率も計算。二個目
                Compo_KakuritsuKeisan_2();

                _text.text = database.items[itemID_2].itemNameHyouji + "が選択されました。" + "\n" + "これでいいですか？";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("2個目　アイテムID:" + itemID_2 + " " + database.items[itemID_2].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView03(pitemlistController._toggle_type2, pitemlistController.kettei_item2); //選択したアイテム2枚目をカードで表示
                updown_counter_obj.SetActive(false);

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

                //トッピングの場合、このタイミングで確率も計算。三個目
                Compo_KakuritsuKeisan_3();
                

                _text.text = database.items[itemID_3].itemNameHyouji + "が選択されました。" + "\n" + "これでいいですか？";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("3個目　アイテムID:" + itemID_3 + " " + database.items[itemID_3].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView04(pitemlistController._toggle_type3, pitemlistController.kettei_item3); //選択したアイテム2枚目をカードで表示
                updown_counter_obj.SetActive(false);

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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                pitemlistController.topping_DrawView_2(); //リストビューを更新し、トッピング材料だけ表示する。

                //Debug.Log("ok");

                itemselect_cancel.update_ListSelect_Flag = 10; //ベースアイテムを選択できないようにする。
                itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                card_view.OKCard_DrawView(1);

                yes.SetActive(false);
                //no.SetActive(false);
                //updown_counter_obj.SetActive(false);

                pitemlistController.final_base_kettei_kosu = 1; //updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;                

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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除
                itemselect_cancel.update_ListSelect_Flag = 11; //ベースアイテムと一個目を選択できないようにする。
                itemselect_cancel.update_ListSelect();

                card_view.OKCard_DrawView02(1);
                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                //pitemlistController.final_kettei_item1 = itemID_1;
                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu;

                compound_Check.final_select_flag = true; //ここにfinalをいれることで、一個だけしかトッピングできないようにする。
                //itemselect_cancel.kettei_on_waiting = false; //finalをいれたときは、こっちはオフで大丈夫。
                
                //yes_text.text = "調合する";

                //_text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一個目: " + database.items[itemID_1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目を選択してください。";
                //Debug.Log("二個目選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                exp_Controller._success_rate = 100;
                kakuritsuPanel.KakuritsuYosoku_Reset();
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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true:

                //Debug.Log("ok");
                //解除
                itemselect_cancel.update_ListSelect_Flag = 12; //ベースアイテムと一個目・二個目を選択できないようにする。
                itemselect_cancel.update_ListSelect();

                card_view.OKCard_DrawView03(1);
                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                pitemlistController.final_kettei_kosu2 = updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;                

                yes_text.text = "調合する";

                _text.text = "ベースアイテム: " + database.items[pitemlistController.final_base_kettei_item].itemNameHyouji + "\n" + "一個目: " + database.items[pitemlistController.final_kettei_item1].itemNameHyouji + " " + pitemlistController.final_kettei_kosu1 + "個" + "\n" + "二個目: " + database.items[itemID_2].itemNameHyouji + " " + pitemlistController.final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";
                //Debug.Log("三個目選択完了！");
                break;

            case false:

                //Debug.Log("二個目はcancel");

                exp_Controller._success_rate = exp_Controller._temp_srate_1;
                kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_1);
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

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

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

                compound_Check.final_select_flag = true;

                yes_text.text = "トッピング開始！";
                

                break;

            case false:

                //Debug.Log("三個目はcancel");

                exp_Controller._success_rate = exp_Controller._temp_srate_2;
                kakuritsuPanel.KakuritsuYosoku_Img(exp_Controller._temp_srate_2);
                itemselect_cancel.Four_cancel();

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

                //exp_Controller.Roast_ResultOK();

                card_view.DeleteCard_DrawView();

                break;

            case false:

                //Debug.Log("一個目はcancel");

                _text.text = "焼きたい生地を選択してください。";

                itemselect_cancel.All_cancel();
                break;
     
        }
    }


    /* ### アイテム売るときのシーン ### */

    public void itemsell_active()
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

        _text.text = database.items[itemID_1].itemNameHyouji + "が選択されました。　" + 
            GameMgr.ColorYellow + database.items[itemID_1].sell_price + "G</color>"
            + "\n" + "個数を選択してください";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + itemID_1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示

        SelectPaused();

        yes.SetActive(true);
        no.SetActive(true);
        updown_counter_obj.SetActive(true);
        back_ShopFirst_btn.interactable = false;

        StartCoroutine("itemsellselect_kakunin");

    }

    IEnumerator itemsellselect_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された　＝　あげる処理へ。プレイヤーリストコントローラー側で処理してる。

                //Debug.Log("ok");

                pitemlistController.final_kettei_kosu1 = updown_counter.updown_kosu; //最終個数を入れる。

                pitemlistController.shopsell_final_select_flag = true; //確認のフラグ

                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                _text.text = "売るアイテムを選択してね。";

                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }

                yes.SetActive(false);
                no.SetActive(false);
                updown_counter_obj.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                card_view.DeleteCard_DrawView();

                break;
        }
    }

    IEnumerator shop_sell_Final_select()
    {

        _text.text = database.items[pitemlistController.final_kettei_item1].itemNameHyouji + "を　" + pitemlistController.final_kettei_kosu1 + "個 売りますか？" + "\n" +
            "全部で　" + GameMgr.ColorYellow + database.items[pitemlistController.final_kettei_item1].sell_price * pitemlistController.final_kettei_kosu1 + "G</color>" + "で買い取ります。";

        updown_button[0].interactable = false;
        updown_button[1].interactable = false;
        updown_button[2].interactable = false;
        updown_button[3].interactable = false;

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。売り決定。
               

                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }

                yes.SetActive(false);
                no.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                updown_button[0].interactable = true;
                updown_button[1].interactable = true;
                updown_button[2].interactable = true;
                updown_button[3].interactable = true;
                updown_counter_obj.SetActive(false);

                card_view.DeleteCard_DrawView();
               
                exp_Controller.Shop_SellOK();

                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "何を売りますか？";

                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }

                yes.SetActive(false);
                no.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                updown_button[0].interactable = true;
                updown_button[1].interactable = true;
                updown_button[2].interactable = true;
                updown_button[3].interactable = true;
                updown_counter_obj.SetActive(false);

                card_view.DeleteCard_DrawView();

                break;
        }

    }


    /* ### 納品時のシーン ### */

    public void nouhin_active()
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
        pitemlistController._listcount.Add(count);

        itemID_1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID;
        pitemlistController.final_kettei_item1 = itemID_1;//選択したアイテムの、アイテムIDを格納しておく。

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + itemID_1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(pitemlistController._toggle_type1, pitemlistController.kettei_item1); //選択したアイテムをカードで表示
        updown_counter_obj.SetActive(true);
        NouhinKetteiPanel_obj.SetActive(false);

        SelectPaused();

        StartCoroutine("nouhin_select_kakunin");

    }

    IEnumerator nouhin_select_kakunin()
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
                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    //まずは、一度全て表示を初期化
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;

                }

                //Debug.Log("pitemlistController._listcount[i]: " + pitemlistController._listcount[pitemlistController._listcount.Count - 1]);

                //選択済みのやつだけONにしておく。
                for (i = 0; i < pitemlistController._listcount.Count; i++)
                {
                    Debug.Log("pitemlistController._listcount[i]: " + pitemlistController._listcount[i]);
                    pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().interactable = false;
                }

                pitemlistController._listkosu.Add(updown_counter.updown_kosu);

                kosusum = 0;

                //個数を判定し、必要個数に達しているかどうかを判定
                for (i = 0; i < pitemlistController._listkosu.Count; i++)
                {
                    kosusum += pitemlistController._listkosu[i];
                }

                //個数が達した場合は、次の処理
                if (kosusum >= quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                {
                    //一旦オフにし、選択できなくする。
                    for (i = 0; i < pitemlistController._listitem.Count; i++)
                    {
                        //Debug.Log("pitemlistController._listcount[i]: " + pitemlistController._listcount[i]);
                        pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false;
                    }

                    _text.text = "これで納品する？";

                    NouhinKetteiPanel_obj.SetActive(true);
                    NouhinKetteiPanel_obj.transform.Find("NouhinButton").gameObject.SetActive(true);

                    shopquestlistController.final_select_flag = true;

                    black_effect.SetActive(true);
                    yes.SetActive(false);
                    no.SetActive(false);

                    //最終確認へ
                   
                }
                else
                {
                    _text.text = "次のお菓子を選んでね。";

                    //リスト更新
                    //shopquestlistController.NouhinList_DrawView();

                    NouhinKetteiPanel_obj.SetActive(false);
                    itemselect_cancel.kettei_on_waiting = false;
                }

                
                card_view.DeleteCard_DrawView();

                updown_counter.OpenFlag = false;
                updown_counter_obj.SetActive(false);
                yes.SetActive(false);
                //no.SetActive(false);
                
                yes_selectitem_kettei.onclick = false;
                

                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                _text.text = "渡したいお菓子を選んでね。";

                //Debug.Log("pitemlistController._listcount[i]を削除: " + pitemlistController._listcount[pitemlistController._listcount.Count - 1]);
                pitemlistController._listcount.RemoveAt(pitemlistController._listcount.Count - 1); //一番最後に挿入されたやつを、そのまま削除
                //pitemlistController._listkosu.RemoveAt(pitemlistController._listkosu.Count - 1); //個数は決定後に追加されるので、ここでは削除しない

                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    //まずは、一度全て表示を初期化
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;

                }

                
                //選択済みのやつだけONにしておく。
                for (i = 0; i < pitemlistController._listcount.Count; i++)
                {
                    //Debug.Log("pitemlistController._listcount[i]: " + i + ": " + pitemlistController._listcount[i]);
                    pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().interactable = false;
                    //pitemlistController._listitem[pitemlistController._listcount[i]].GetComponent<Toggle>().isOn = true;
                }               

                card_view.DeleteCard_DrawView();

                updown_counter.OpenFlag = false;
                updown_counter_obj.SetActive(false);
                yes.SetActive(false);
                NouhinKetteiPanel_obj.SetActive(true);

                yes_selectitem_kettei.onclick = false;
                itemselect_cancel.kettei_on_waiting = false;

                if (pitemlistController._listcount.Count <= 0) //すべて選択してないときは、noはOFF
                {
                    no.SetActive(false);
                }

                break;
        }
    }


    void SelectPaused()
    {
        //一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < pitemlistController._listitem.Count; i++)
        {
            pitemlistController._listitem[i].GetComponent<Toggle>().interactable = false;
        }
        
        no.SetActive(true);
        
        yes.SetActive(true);
        yes_text.text = "決定";

        //yesは元デザインに戻す
        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。
        {
            compound_Check.YesSetDesignDefault();
        }

    }


    //
    //調合確率の計算
    //
    void Compo_KakuritsuKeisan_1()
    {
        _success_rate = (100 * database.items[itemID_1].Ex_Probability) + PlayerStatus.player_renkin_lv;

        if(_success_rate >= 100 )
        {
            _success_rate = 100;
        }

        if (_success_rate < 0)
        {
            _success_rate = 0;
        }

        exp_Controller._temp_srate_1 = _success_rate; //キャンセル時などに、すぐ表示できるよう一時保存
        exp_Controller._success_rate = _success_rate;
        kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
    }

    void Compo_KakuritsuKeisan_2()
    {
        _success_rate = (exp_Controller._temp_srate_1 * database.items[itemID_2].Ex_Probability * 0.75f) + PlayerStatus.player_renkin_lv;

        if (_success_rate >= 100)
        {
            _success_rate = 100;
        }

        if (_success_rate < 0)
        {
            _success_rate = 0;
        }

        exp_Controller._temp_srate_2 = _success_rate; //キャンセル時などに、すぐ表示できるよう一時保存
        exp_Controller._success_rate = _success_rate;
        kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
    }

    void Compo_KakuritsuKeisan_3()
    {
        _success_rate = (exp_Controller._temp_srate_2 * database.items[itemID_3].Ex_Probability * 0.5f) + PlayerStatus.player_renkin_lv;

        if (_success_rate >= 100)
        {
            _success_rate = 100;
        }

        if (_success_rate < 0)
        {
            _success_rate = 0;
        }

        exp_Controller._temp_srate_3 = _success_rate; //キャンセル時などに、すぐ表示できるよう一時保存
        exp_Controller._success_rate = _success_rate;
        kakuritsuPanel.KakuritsuYosoku_Img(_success_rate);
    }
}

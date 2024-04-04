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

    private GameObject black_effect;

    private GameObject quest_Judge_CanvasPanel;
    private GameObject NouhinKetteiPanel_obj;

    public int toggleitem_ID; //リストの要素自体に、アイテムIDを保持する。
    public int toggleitem_type; //リストの要素に、プレイヤーアイテムリストか、オリジナルかを識別するための番号を割り振る。
    public int toggle_originplist_ID; //ややこしいが、オリジナルアイテムリストの最初から順番に、IDを割り振っておく。toggleorigin_ID=0だと、オリジナルアイテムプレイヤーリストの0番を参照する。

    private int i;

    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private int _itemID1;

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

        // 調合シーンでやりたい処理。
        if (GameMgr.CompoundSceneStartON)
        {
            //テキストウィンドウの取得
            text_area = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject;
            _text = text_area.GetComponentInChildren<Text>();

            kakuritsuPanel_obj = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/FinalCheckPanel/Comp/KakuritsuPanel").gameObject;
            kakuritsuPanel = kakuritsuPanel_obj.GetComponent<KakuritsuPanel>();
        }
        else
        {
            //テキストウィンドウの取得
            text_area = canvas.transform.Find("MessageWindow").gameObject;
            _text = text_area.GetComponentInChildren<Text>();
        }

        switch (GameMgr.Scene_Category_Num) 
        {
            case 10: //調合シーン
               
                break;

            case 20: //ショップ

                back_ShopFirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
                back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

                break;

            case 30: //酒場

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
       
    }


    void Update()
    {
        if (pitemlistController.shopsell_final_select_flag == true) //最後、これを売るかどうかを待つフラグ
        {
            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();

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
            // 調合シーンでやりたい処理
            if (GameMgr.CompoundSceneStartON)
            {
                itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                GameMgr.compound_status = 100; //トグルを押して、調合中の状態。All_cancelで、status=4に戻る。status=4でキャンセルすると、最初の調合選択シーンに戻る。

                // オリジナル調合かヒカリに作らせるを選択した場合の処理
                if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
                {
                    yes.SetActive(true);

                    compound_active();
                }

                // トッピング調合を選択した場合の処理
                if (GameMgr.compound_select == 2)
                {
                    yes.SetActive(true);

                    compound_topping_active();
                }

                // 「焼く」を選択した場合の処理
                if (GameMgr.compound_select == 5)
                {
                    yes.SetActive(true);

                    compound_roast_active();
                }

                // 魔法調合の場合の処理
                if (GameMgr.compound_select == 21)
                {
                    yes.SetActive(true);

                    //Debug.Log("GameMgr.Comp_kettei_bunki: " + GameMgr.Comp_kettei_bunki);
                    magiccompound_active();
                }
            }
            else
            {
                if (GameMgr.Scene_Category_Num == 10) // 調合シーン以外でメインシーンでやりたい処理
                {

                    itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                    GameMgr.compound_status = 100; //トグルを押して、調合中の状態。All_cancelで、status=4に戻る。status=4でキャンセルすると、最初の調合選択シーンに戻る。

                    
                    // お菓子を「あげる」を選択した場合の処理
                    if (GameMgr.compound_select == 10)
                    {

                    }

                    // 単にメニューを開いたとき
                    if (GameMgr.compound_select == 99)
                    {
                        Player_ItemList_Open();
                    }

                    // シナリオやサブイベントなどを読み中の処理
                    if (GameMgr.compound_select == 1000)
                    {
                        itempresent_active();
                    }
                }

                else if (GameMgr.Scene_Category_Num == 20) // ショップでやりたい処理
                {
                    itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                    back_ShopFirst_btn.interactable = false;

                    switch (GameMgr.Scene_Select)
                    {

                        case 5: //売るとき

                            itemsell_active(); //アイテムを売る
                            break;

                        case 6: //わたすとき

                            GameMgr.compound_status = 100; //トグルを押して、調合中の状態。
                            itempresent_active();
                            break;
                    }
                }

                else if (GameMgr.Scene_Category_Num == 30) // 酒場でやりたい処理
                {
                    itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

                    back_ShopFirst_btn.interactable = false;

                    switch (GameMgr.Scene_Select)
                    {

                        case 3: //納品時の画面開いた時

                            quest_Judge_CanvasPanel = canvas.transform.Find("Quest_Judge_CanvasPanel").gameObject;
                            NouhinKetteiPanel_obj = quest_Judge_CanvasPanel.transform.Find("NouhinKetteiPanel").gameObject;

                            shopquestlistController_obj = quest_Judge_CanvasPanel.transform.Find("ShopQuestList_ScrollView").gameObject;
                            shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();

                            questjudge_obj = GameObject.FindWithTag("Quest_Judge");
                            questjudge = questjudge_obj.GetComponent<Quest_Judge>();

                            //黒半透明パネルの取得
                            black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

                            nouhin_active(); //納品したいアイテムを、納品個数に達するまで、選択できる。か、一種類のみで、必要個数
                            break;

                        case 6: //わたすとき

                            GameMgr.compound_status = 100; //トグルを押して、調合中の状態。
                            itempresent_active();
                            break;

                    }
                }

                else // その他シーンでやりたい処理
                {
                    // シナリオやサブイベントなどを読み中の処理
                    if (GameMgr.compound_select == 1000)
                    {
                        GameMgr.compound_status = 100; //トグルを押して、調合中の状態。
                        itempresent_active();
                    }
                    else
                    {
                        //アイテム画面を表示した時などで使用
                        Player_ItemList_Open();
                    }

                }
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
            if (count != GameMgr.List_count1)
            {
                selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                if (selectToggle == true) break;
            }
            ++count;
        }

        pitemlistController._listitem[count].GetComponent<Toggle>().interactable = false;

        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        GameMgr.List_count1 = count;

        //リスト中の選択された番号を格納。
        GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
        GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

        Debug.Log("アイテム配列番号: " + GameMgr.temp_itemID1 + " " + database.items[GameMgr.temp_itemID1].itemName);

        card_view.ItemListCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1);

        //あらためて新しく押されたやつ以外の表示をリセットする。
        count = 0;
        while (count < pitemlistController._listitem.Count)
        {
            if (count != GameMgr.List_count1)
            {
                pitemlistController._listitem[count].GetComponent<Toggle>().isOn = false;
                pitemlistController._listitem[count].GetComponent<Toggle>().interactable = true;
            }
            ++count;
        }
        pitemlistController.transform.Find("BlackImg").gameObject.SetActive(true);

        StartCoroutine("itemselect_kakunin_PitemList");
    }

    IEnumerator itemselect_kakunin_PitemList()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された アイテムを飾る、使う場合の処理 だが、現在はNoしか受け付けないようにしている。


                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                itemselect_cancel.All_cancel();

                GameMgr.List_count1 = 9999;
                GameMgr.compound_status = 99; //何も選択していない状態にもどる。

                pitemlistController.transform.Find("BlackImg").gameObject.SetActive(false);
                break;
        }

    }



    /* ### 調合シーンの処理 ### */

    public void compound_active()
    {
        switch (GameMgr.Comp_kettei_bunki)
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

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count1 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;                
                GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                //GameMgr.Final_list_itemID1という変数に、プレイヤーが一個目に選択したアイテムIDのID→database.itemsのリスト番号に変換したものを格納する。
                GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                //押したタイミングで、分岐＝１に。
                GameMgr.Comp_kettei_bunki = 1;

                //もし生地アイテムを一個目に選んだ場合、生地にアイテムを混ぜ込む処理になる。現在は未使用。
                if (database.items[GameMgr.temp_itemID1].itemType_sub == Item.ItemType_sub.Pate)
                {
                    _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";
                }
                else
                {
                    _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";
                }


                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + _itemID1 + " " + database.items[_itemID1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示。トグルタイプとリスト番号を入れると、表示してくれる。
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
                    if (count != GameMgr.List_count1)
                    {
                        selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                        if (selectToggle == true) break;
                    }

                    ++count;
                }

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count2 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_itemID2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                GameMgr.temp_itemID2 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                //押したタイミングで、分岐＝２に。
                GameMgr.Comp_kettei_bunki = 2;

                _text.text = database.items[GameMgr.temp_itemID2].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("2個目　アイテムID:" + _itemID1 + " " + database.items[_itemID1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView02(GameMgr.Final_toggle_Type2, GameMgr.Final_list_itemID2); //選択したアイテム2枚目をカードで表示
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
                    if (count != GameMgr.List_count1)
                    {
                        if (count != GameMgr.List_count2)
                        {
                            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                            if (selectToggle == true) break;
                        }
                    }

                    ++count;
                }

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count3 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_itemID3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                GameMgr.temp_itemID3 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                //押したタイミングで、分岐＝３に。
                GameMgr.Comp_kettei_bunki = 3;

                _text.text = database.items[GameMgr.temp_itemID3].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("3個目　アイテムID:" + _itemID1 + " " + database.items[_itemID1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView03(GameMgr.Final_toggle_Type3, GameMgr.Final_list_itemID3); //選択したアイテム2枚目をカードで表示
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

                GameMgr.Final_kettei_kosu1 = GameMgr.updown_kosu;
                card_view.OKCard_DrawView(GameMgr.Final_kettei_kosu1);

                yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);                

                itemselect_cancel.kettei_on_waiting = false;
                

                if (database.items[GameMgr.temp_itemID1].itemType_sub == Item.ItemType_sub.Pate)
                {
                    _text.text = "一個目: " + database.items[GameMgr.temp_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" 
                        + "　生地にアイテムを混ぜます" + "\n" + "二個目を選択してください。";
                }
                else
                {
                    _text.text = "一個目: " + database.items[GameMgr.temp_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" 
                        + "\n" + "二個目を選択してください。";
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

                GameMgr.Final_kettei_kosu2 = GameMgr.updown_kosu;
                card_view.OKCard_DrawView02(GameMgr.Final_kettei_kosu2);

                yes.SetActive(true);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);               

                itemselect_cancel.kettei_on_waiting = false;
               
                yes_text.text = "作る";
                yes_text.color = new Color(255f / 255f, 255f / 255f, 255f / 255f); //白文字
                yes.GetComponent<Image>().sprite = yes_sprite2;

                _text.text = "一個目: " + database.items[GameMgr.temp_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" 
                    + "二個目: " + database.items[GameMgr.temp_itemID2].itemNameHyouji + " " + GameMgr.Final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";
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

                GameMgr.Final_kettei_kosu3 = GameMgr.updown_kosu;
                card_view.OKCard_DrawView03(GameMgr.Final_kettei_kosu3);

                updown_counter_obj.SetActive(false);

                yes.SetActive(true);

                GameMgr.final_select_flag = true;


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

        switch (GameMgr.Comp_kettei_bunki)
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

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_basecount = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_baseType = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_baseitemID = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                GameMgr.Comp_kettei_bunki = 10;

                _text.text = database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "をベースにします。";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + GameMgr.Final_list_baseitemID + " " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView(GameMgr.Final_toggle_baseType, GameMgr.Final_list_baseitemID); //選択したアイテムをカードで表示

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

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count1 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                GameMgr.Comp_kettei_bunki = 11;

                //トッピングの場合、このタイミングで確率も計算。一個目
                Compo_KakuritsuKeisan_1();

                _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。" + "\n" + "これでいいですか？";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + GameMgr.temp_itemID1 + " " + database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView02(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示
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
                        if (count != GameMgr.List_count1)
                        {
                            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                            if (selectToggle == true) break;
                        }
                    //}

                    ++count;
                }

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count2 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_itemID2 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                GameMgr.temp_itemID2 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                GameMgr.Comp_kettei_bunki = 12;

                //トッピングの場合、このタイミングで確率も計算。二個目
                Compo_KakuritsuKeisan_2();

                _text.text = database.items[GameMgr.temp_itemID2].itemNameHyouji + "が選択されました。" + "\n" + "これでいいですか？";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("2個目　アイテムID:" + GameMgr.temp_itemID2 + " " + database.items[GameMgr.temp_itemID2].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView03(GameMgr.Final_toggle_Type2, GameMgr.Final_list_itemID2); //選択したアイテム2枚目をカードで表示
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
                        if (count != GameMgr.List_count1)
                        {
                            if (count != GameMgr.List_count2)
                            {
                                selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                                if (selectToggle == true) break;
                            }
                        }
                    //}

                    ++count;
                }

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count3 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_itemID3 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                GameMgr.temp_itemID3 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                GameMgr.Comp_kettei_bunki = 13;

                //トッピングの場合、このタイミングで確率も計算。三個目
                Compo_KakuritsuKeisan_3();
                

                _text.text = database.items[GameMgr.temp_itemID3].itemNameHyouji + "が選択されました。" + "\n" + "これでいいですか？";

                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("3個目　アイテムID:" + GameMgr.temp_itemID3 + " " + database.items[GameMgr.temp_itemID3].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView04(GameMgr.Final_toggle_Type3, GameMgr.Final_list_itemID3); //選択したアイテム2枚目をカードで表示
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

                GameMgr.Final_kettei_basekosu = 1; //updown_counter.updown_kosu;

                itemselect_cancel.kettei_on_waiting = false;                

                _text.text = "ベースアイテム: " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + " " + "1個" + "\n" 
                    + "トッピングアイテム一個目を選択してください。";
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

                //pitemlistController.final_kettei_kosu1 = GameMgr.updown_kosu;
                GameMgr.Final_kettei_kosu1 = 1;

                card_view.OKCard_DrawView02(1);
                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);
                                

                if (GameMgr.topping_Set_Count == 1) //トッピングが一度に一個のとき
                {
                    GameMgr.final_select_flag = true; //ここにfinalをいれることで、一個だけしかトッピングできないようにする。
                }
                else 
                {
                    itemselect_cancel.kettei_on_waiting = false; //finalをいれたときは、こっちはオフで大丈夫。
                    _text.text = "ベースアイテム: " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "\n" + "一個目: "
                    + database.items[GameMgr.temp_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" + "二個目を選択するか、決定を押してね。";
                }


                //yes_text.text = "調合する";


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

                GameMgr.Final_kettei_kosu2 = GameMgr.updown_kosu;

                card_view.OKCard_DrawView03(1);
                //yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);
                

                if (GameMgr.topping_Set_Count == 2) //トッピングが一度に２個のとき
                {
                    GameMgr.final_select_flag = true; //ここにfinalをいれることで、二個のトッピングになる。
                }
                else
                {
                    itemselect_cancel.kettei_on_waiting = false; //finalをいれたときは、こっちはオフで大丈夫。
                    _text.text = "ベースアイテム: " + database.items[GameMgr.Final_list_baseitemID].itemNameHyouji + "\n" +
                    "一個目: " + database.items[GameMgr.temp_itemID1].itemNameHyouji + " " + GameMgr.Final_kettei_kosu1 + "個" + "\n" +
                    "二個目: " + database.items[GameMgr.temp_itemID2].itemNameHyouji + " " + GameMgr.Final_kettei_kosu2 + "個" + "\n" + "最後に一つ追加できます。";
                }
                

                //yes_text.text = "調合する";

                
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

                GameMgr.Final_kettei_kosu3 = GameMgr.updown_kosu;

                updown_counter_obj.SetActive(false);

                card_view.OKCard_DrawView04();
                

                if (GameMgr.topping_Set_Count == 3) //トッピングが一度に３個のとき
                {
                    GameMgr.final_select_flag = true; //3個まで
                }
                else
                {
                    GameMgr.final_select_flag = true; //3個まで
                }

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

        //表示中リストの、リスト番号を保存。
        GameMgr.List_count1 = count;

        //リスト中の選択された番号を格納。
        GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
        GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

        GameMgr.Comp_kettei_bunki = 9999; //分岐なし。テキストの更新を避けるため、とりあえず適当な数字を入れて回避。

        _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "を焼きますか？○○時間かかります。";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("1個目　アイテムID:" + GameMgr.temp_itemID1 + " " + database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示
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

                //調合成功の場合、アイテム増減の処理は、「Exp_Controller」で行う。
                exp_Controller.roast_result_ok = true; //調合完了のフラグをたてておく。

                GameMgr.Final_kettei_kosu1 = GameMgr.updown_kosu;

                GameMgr.compound_status = 4;

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

    /* ### 魔法使用時の調合処理 ### */

    public void magiccompound_active()
    {
        switch (GameMgr.Comp_kettei_bunki)
        {
            case 0: //魔法を指定した状態で、どのアイテムにするかを選択する状態

                //一個目のアイテムを選択したときの処理（トグルの処理）

                count = 0;

                while (count < pitemlistController._listitem.Count)
                {
                    selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
                    if (selectToggle == true) break;
                    ++count;
                }

                //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
                GameMgr.List_count1 = count;

                //リスト中の選択された番号を格納。
                GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
                GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
                GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

                //押したタイミングで、分岐。
                GameMgr.Comp_kettei_bunki = 20;

                _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。" + "\n" + "個数を選択してください。";


                //Debug.Log(count + "番が押されたよ");
                //Debug.Log("1個目　アイテムID:" + GameMgr.temp_itemID1 + " " + database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。");
                //Debug.Log("これでいいですか？");

                card_view.SelectCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示。トグルタイプとリスト番号を入れると、表示してくれる。
                updown_counter_obj.SetActive(true);

                SelectPaused();

                StartCoroutine("magicitemselect_kakunin_one");
                break;
        }
    }



    IEnumerator magicitemselect_kakunin_one()
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

                //itemselect_cancel.update_ListSelect_Flag = 1; //一個目を選択したものを選択できないようにするときの番号。
                //itemselect_cancel.update_ListSelect(); //アイテム選択時の、リストの表示処理

                GameMgr.Final_kettei_kosu1 = GameMgr.updown_kosu;
                //card_view.DeleteCard_DrawView(); //決定したら表示してたカードを削除　もしくは、少し演出のアニメ入れてから消す
                card_view.OKCard_DrawView(GameMgr.Final_kettei_kosu1);               

                yes.SetActive(false);
                //no.SetActive(false);
                updown_counter_obj.SetActive(false);

                itemselect_cancel.kettei_on_waiting = false;
                GameMgr.final_select_flag = true; //調合最終確認

                //Debug.Log("一個目選択完了！");
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                itemselect_cancel.All_cancel();

                break;
        }

    }

    // 魔法の調合処理　ここまで //



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

        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        GameMgr.List_count1 = count;

        //リスト中の選択された番号を格納。
        GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
        GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

        _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。　" + 
            GameMgr.ColorYellow + database.items[GameMgr.temp_itemID1].sell_price + " " + GameMgr.MoneyCurrency + "</color>"
            + "\n" + "個数を選択してください";

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + GameMgr.temp_itemID1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示

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

                GameMgr.Final_kettei_kosu1 = GameMgr.updown_kosu; //最終個数を入れる。

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
                no.SetActive(true);
                updown_counter_obj.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                card_view.DeleteCard_DrawView();
                itemselect_cancel.kettei_on_waiting = false;


                break;
        }
    }

    IEnumerator shop_sell_Final_select()
    {

        _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "を　" + GameMgr.Final_kettei_kosu1 + "個 売りますか？" + "\n" +
            "全部で　" + GameMgr.ColorYellow + database.items[GameMgr.temp_itemID1].sell_price * GameMgr.Final_kettei_kosu1 + 
            " " + GameMgr.MoneyCurrency + "</color>" + "で買い取ります。";

        updown_counter.UpdownButton_InteractALLOFF();

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
                no.SetActive(true);
                back_ShopFirst_btn.interactable = true;

                updown_counter.UpdownButton_InteractALLON();
                updown_counter_obj.SetActive(false);

                card_view.DeleteCard_DrawView();
               
                exp_Controller.Shop_SellOK();
                itemselect_cancel.kettei_on_waiting = false;

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
                no.SetActive(true);
                back_ShopFirst_btn.interactable = true;

                updown_counter.UpdownButton_InteractALLON();
                updown_counter_obj.SetActive(false);

                card_view.DeleteCard_DrawView();
                itemselect_cancel.kettei_on_waiting = false;

                break;
        }

    }

    /* ここまで */



    /* ### アイテムを所持品から渡すときの処理 ### */
    /* 色んなイベントで使いまわす */

    public void itempresent_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < pitemlistController._listitem.Count)
        {
            selectToggle = pitemlistController._listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        GameMgr.List_count1 = count;

        //リスト中の選択された番号を格納。
        GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
        GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);

        /*
        _text.text = database.items[GameMgr.temp_itemID1].itemNameHyouji + "が選択されました。　" +
            GameMgr.ColorYellow + database.items[GameMgr.temp_itemID1].sell_price + " " + GameMgr.MoneyCurrency + "</color>"
            + "\n" + "個数を選択してください";
            */

        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + GameMgr.temp_itemID1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示

        SelectPaused();

        yes.SetActive(true);
        no.SetActive(true);

        StartCoroutine("itempresent_Finalkakunin");

    }

    IEnumerator itempresent_Finalkakunin()
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

                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }

                yes.SetActive(false);
                no.SetActive(false);                

                card_view.DeleteCard_DrawView();
                itemselect_cancel.kettei_on_waiting = false;

                GameMgr.event_pitem_use_OK = true;

                //決定したアイテムの番号と個数
                GameMgr.event_kettei_itemID = GameMgr.Final_list_itemID1;
                GameMgr.event_kettei_item_Type = GameMgr.Final_toggle_Type1;
                //GameMgr.event_kettei_item_Kosu = updown_counter.updown_kosu; //最終個数を入れる。
                GameMgr.event_kettei_item_Kosu = 1;

                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                _text.text = "渡すアイテムを選択してね。";

                for (i = 0; i < pitemlistController._listitem.Count; i++)
                {
                    pitemlistController._listitem[i].GetComponent<Toggle>().interactable = true;
                    pitemlistController._listitem[i].GetComponent<Toggle>().isOn = false;
                }

                yes.SetActive(false);
                no.SetActive(true);
                //updown_counter_obj.SetActive(false);

                card_view.DeleteCard_DrawView();
                itemselect_cancel.kettei_on_waiting = false;

                GameMgr.compound_status = 1000;

                break;
        }
    }

    /* ここまで */



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

        //表示中リストの、リスト番号を保存。トグルを、isOn=falseする際に、使用する。
        pitemlistController._listcount.Add(count);

        //リスト中の選択された番号を格納。
        GameMgr.Final_toggle_Type1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_type;
        GameMgr.Final_list_itemID1 = pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggle_originplist_ID;
        GameMgr.temp_itemID1 = database.SearchItemID(pitemlistController._listitem[count].GetComponent<itemSelectToggle>().toggleitem_ID);


        //Debug.Log(count + "番が押されたよ");
        //Debug.Log("アイテムID:" + GameMgr.temp_itemID1 + "が選択されました。");
        //Debug.Log("これでいいですか？");

        card_view.SelectCard_DrawView(GameMgr.Final_toggle_Type1, GameMgr.Final_list_itemID1); //選択したアイテムをカードで表示
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

                pitemlistController._listkosu.Add(GameMgr.updown_kosu);

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

                    shopquestlistController.final_select_flag = true; //shopQuestSelectToggleで待ち中　yes_selectitem_kettei.onclick2でyes, noを待っている

                    black_effect.SetActive(true);
                    yes.SetActive(false);
                    no.SetActive(false);

                    //納品するアイテムを表示する
                    //card_view.DeleteCard_DrawView();
                    card_view.SelectCard_DrawView(pitemlistController._listitem[pitemlistController._listcount[0]].GetComponent<itemSelectToggle>().toggleitem_type,
                        pitemlistController._listitem[pitemlistController._listcount[0]].GetComponent<itemSelectToggle>().toggle_originplist_ID); //選択したアイテムをカードで表示

                                                                                                                                                  
                    //最終確認へ

                }
                else
                {
                    _text.text = "次のお菓子を選んでね。";

                    //リスト更新
                    //shopquestlistController.NouhinList_DrawView();

                    NouhinKetteiPanel_obj.SetActive(false);
                    itemselect_cancel.kettei_on_waiting = false;

                    card_view.DeleteCard_DrawView();
                }               
                
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

    /* ここまで */







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
        if (GameMgr.Scene_Category_Num == 10) // 調合シーンでやりたい処理。
        {
            yes_text.color = new Color(56f / 255f, 56f / 255f, 36f / 255f); //焦げ茶文字
            yes.GetComponent<Image>().sprite = yes_sprite1;
        }

    }


    //
    //調合確率の計算
    //
    void Compo_KakuritsuKeisan_1()
    {
        _success_rate = (100 * database.items[GameMgr.temp_itemID1].Ex_Probability) + PlayerStatus.player_renkin_lv;

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
        _success_rate = (exp_Controller._temp_srate_1 * database.items[GameMgr.temp_itemID2].Ex_Probability * 0.75f) + PlayerStatus.player_renkin_lv;

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
        _success_rate = (exp_Controller._temp_srate_2 * database.items[GameMgr.temp_itemID3].Ex_Probability * 0.5f) + PlayerStatus.player_renkin_lv;

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

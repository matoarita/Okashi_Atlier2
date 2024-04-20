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

public class shopitemSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    Toggle m_Toggle;
    public Text m_Text; //デバッグ用。未使用。

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private Text _coin_cullency; //通貨　GameMgrで決めたものを自動で入力する

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;
    private GameObject blackpanel_A;

    private GameObject shopitemlistController_obj;
    private ShopItemListController shopitemlistController;
    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[4];

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemShopDataBase shop_database;

    private GameObject yes_no_panel;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    public int toggle_shop_ID; //こっちは、ショップデータベース上のIDを保持する。
    public int toggle_shopitem_ID; //リストの要素自体に、アイテムDB上のアイテムIDを保持する。
    public string toggle_shopitem_nameHyouji; //表示用名前
    public int toggle_shopitem_costprice; //金額も保持
    public int toggle_shopitem_type; //リストの要素に、通常アイテムか、イベントアイテム判定用のタイプを保持する。
    public int toggle_shopitem_dongri_type; //どんぐりタイプも保持

    private int i;
    private int _id;

    private int _itemcount; //現在の所持数　店売り＋オリジナル
    private string _item_Namehyouji;
    private int pitemlist_max;
    private int count;
    private bool selectToggle;

    private List<GameObject> category_toggle = new List<GameObject>();

    private int kettei_item1; //このスクリプトは、プレファブのインスタンスに取り付けているので、各プレファブ共通で、変更できる値が必要。そのパラメータは、PlayerItemListControllerで管理する。

    private int count_1;
    private int _cost;
    private int player_money;
    private int emeraldonguriID;

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

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        shopitemlistController_obj = GameObject.FindWithTag("ShopitemList_ScrollView");
        shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();
        back_ShopFirst_obj = canvas.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;
        yes_no_panel.SetActive(false);

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        blackpanel_A = canvas.transform.Find("Black_Panel_A").gameObject;

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();


        //カテゴリータブの取得
        category_toggle.Clear();
        foreach (Transform child in shopitemlistController_obj.transform.Find("CategoryView/Viewport/Content/").transform)
        {
            //Debug.Log(child.name);           
            category_toggle.Add(child.gameObject);
        }        


        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        _coin_cullency = this.transform.Find("Background/Item_price").GetComponent<Text>();
        _coin_cullency.text = GameMgr.MoneyCurrencyEn;

        i = 0;
        count = 0;

    }


    void Update()
    {

        if (shopitemlistController.shop_final_select_flag == true) //最後、これを買うかどうかを待つフラグ
        {
            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
            updown_button = updown_counter_obj.GetComponentsInChildren<Button>();

            shopitemlistController.shop_final_select_flag = false;
            StartCoroutine("shop_buy_Final_select");
        }
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        if (m_Toggle.isOn == true)
        {
            updown_counter_obj = canvas.transform.Find("updown_counter(Clone)").gameObject;
            updown_counter = updown_counter_obj.GetComponent<Updown_counter>();
            updown_button = updown_counter_obj.GetComponentsInChildren<Button>();

            itemselect_cancel.kettei_on_waiting = true; //トグルが押された時点で、トグル内のボタンyes,noを優先する

            back_ShopFirst_btn.interactable = false;
            shop_buy_active();
        }
    }


    /* ### ショップでアイテムを買うときのシーン ### */

    public void shop_buy_active()
    {

        //アイテムを選択したときの処理（トグルの処理）

        count = 0;

        while (count < shopitemlistController._shop_listitem.Count)
        {
            selectToggle = shopitemlistController._shop_listitem[count].GetComponent<Toggle>().isOn;
            if (selectToggle == true) break;
            ++count;
        }

        shopitemlistController.shop_count = count; //カウントしたリスト番号を保持
        shopitemlistController.shop_kettei_ID = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shop_ID; //ショップIDを入れる。
        shopitemlistController.shop_kettei_item1 = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_ID; //アイテムIDを入れる。
        shopitemlistController.shop_itemType = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_type; //判定用アイテムタイプを入れる。
        shopitemlistController.shop_dongriType = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_dongri_type;
        _item_Namehyouji = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_nameHyouji; //表示用ネームを入れる。
        shopitemlistController.shop_itemName_Hyouji = _item_Namehyouji;
        shopitemlistController.shop_costprice = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_costprice;

        if (shopitemlistController.shop_itemType == 1) //レシピを選択したとき
        {
            _id = pitemlist.SearchEventItemID(shopitemlistController.shop_kettei_item1); //IDをもとにevent_itemsの配列番号に変換
            _text.text = _item_Namehyouji + "を何個買いますか？";
            card_view.ShopSelectCard_DrawView(1, _id);
        }
        else if (shopitemlistController.shop_itemType == 5) //エメラルドショップのアイテムを選択したとき
        {
            _id = pitemlist.SearchEmeraldItemID(shopitemlistController.shop_kettei_item1);
            _itemcount = pitemlist.KosuCountEmerald(pitemlist.emeralditemlist[_id].event_itemName);
            _text.text = _item_Namehyouji + "を買いますか？" + "\n" + "個数を選択してください。" + "\n" + "現在の所持数: " + _itemcount;
        }
        else //それ以外の通常のアイテムは個数が表示
        {
            _id = database.SearchItemID(shopitemlistController.shop_kettei_item1); //IDをもとにitemsの配列番号に変換
            _itemcount = pitemlist.KosuCount(database.items[_id].itemName);
            _text.text = _item_Namehyouji + "を買いますか？" + "\n" + "個数を選択してください。" + "\n" + "現在の所持数: " + _itemcount;
            card_view.ShopSelectCard_DrawView(0, _id);
        }

        Debug.Log(count + "番が押されたよ");
        Debug.Log("アイテム:" + _item_Namehyouji + "が選択されました。");

        blackpanel_A.SetActive(true);

        //Debug.Log("これでいいですか？");

        //すごく面倒な処理だけど、一時的にリスト要素への入力受付を停止している。
        for (i = 0; i < shopitemlistController._shop_listitem.Count; i++)
        {
            shopitemlistController._shop_listitem[i].GetComponent<Toggle>().interactable = false;
        }
        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().interactable = false;
        }

        yes_no_panel.SetActive(true);
        updown_counter_obj.SetActive(true);

        StartCoroutine("shop_buy_kosu_select");

    }

    IEnumerator shop_buy_kosu_select()
    {
        

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
        
        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。これでいいですか？の確認。

                //Debug.Log("ok");
                //解除

                shopitemlistController.shop_final_itemkosu_1 = GameMgr.updown_kosu; //最終個数を入れる。

                shopitemlistController.shop_final_select_flag = true; //確認のフラグ

                Debug.Log("選択完了！");
                
                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "何にしますか？";

                //キャンセル時、リストのインタラクティブ解除。その時、プレイヤーの所持金をチェックし、足りないものはOFF表示にする。
                Money_Check();

                yes_no_panel.SetActive(false);
                updown_counter_obj.SetActive(false);

                back_ShopFirst_btn.interactable = true;

                card_view.DeleteCard_DrawView();
                blackpanel_A.SetActive(false);

                itemselect_cancel.kettei_on_waiting = false; //トグルが押された時点で、トグル内のボタンyes,noを優先する


                break;
        }
    }


    IEnumerator shop_buy_Final_select()
    {
        switch (GameMgr.Scene_Category_Num)
        {
            case 50:

                switch(shopitemlistController.shop_dongriType)
                {
                    case 0: //エメラルどんぐり

                        _text.text = shopitemlistController.shop_itemName_Hyouji + "を　" + shopitemlistController.shop_final_itemkosu_1 + "個 買いますか？" + "\n" +
            "エメラルどんぐり　" + GameMgr.ColorYellow + shopitemlistController.shop_costprice * shopitemlistController.shop_final_itemkosu_1 + "個</color>" + "いただくよ。";
                        break;

                    case 1: //サファイアどんぐり

                        _text.text = shopitemlistController.shop_itemName_Hyouji + "を　" + shopitemlistController.shop_final_itemkosu_1 + "個 買いますか？" + "\n" +
            "サファイアどんぐり　" + GameMgr.ColorYellow + shopitemlistController.shop_costprice * shopitemlistController.shop_final_itemkosu_1 + "個</color>" + "いただくよ。";
                        break;
                }
                
                break;

            default:

                _text.text = shopitemlistController.shop_itemName_Hyouji + "を　" + shopitemlistController.shop_final_itemkosu_1 + "個 買いますか？" + "\n" +
            "お金が　" + GameMgr.ColorYellow + shopitemlistController.shop_costprice * shopitemlistController.shop_final_itemkosu_1 + GameMgr.MoneyCurrency + "　</color>" + "かかります。";
                break;
        }

        updown_button[0].interactable = false;
        updown_button[1].interactable = false;
        updown_button[2].interactable = false;
        updown_button[3].interactable = false;

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
        itemselect_cancel.kettei_on_waiting = false; //トグルが押された時点で、トグル内のボタンyes,noを優先する


        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。購入決定。


                //exp_Controller.shop_buy_ok = true; //購入完了のフラグをたてる。

                for (i = 0; i < shopitemlistController._shop_listitem.Count; i++)
                {
                    shopitemlistController._shop_listitem[i].GetComponent<Toggle>().interactable = true;
                    shopitemlistController._shop_listitem[i].GetComponent<Toggle>().isOn = false;
                }
                for (i = 0; i < category_toggle.Count; i++)
                {
                    category_toggle[i].GetComponent<Toggle>().interactable = true;
                }

                card_view.DeleteCard_DrawView();
                blackpanel_A.SetActive(false);

                yes_no_panel.SetActive(false);
                back_ShopFirst_btn.interactable = true;

                updown_button[0].interactable = true;
                updown_button[1].interactable = true;
                updown_button[2].interactable = true;
                updown_button[3].interactable = true;
                updown_counter_obj.SetActive(false);
                
                exp_Controller.Shop_ResultOK();

                break;

            case false:

                //Debug.Log("cancel");

                _text.text = "何にしますか？";

                //キャンセル時、リストのインタラクティブ解除。その時、プレイヤーの所持金をチェックし、足りないものはOFF表示にする。
                Money_Check();

                card_view.DeleteCard_DrawView();
                blackpanel_A.SetActive(false);

                yes_selectitem_kettei.kettei1 = false;
                yes_no_panel.SetActive(false);

                back_ShopFirst_btn.interactable = true;

                updown_button[0].interactable = true;
                updown_button[1].interactable = true;
                updown_button[2].interactable = true;
                updown_button[3].interactable = true;
                updown_counter_obj.SetActive(false);

                break;
        }

    }

    void Money_Check()
    {
        MoneyCheck_Method();          
    }

    void MoneyCheck_Method()
    {
        for (i = 0; i < shopitemlistController._shop_listitem.Count; i++)
        {
            shopitemlistController._shop_listitem[i].GetComponent<Toggle>().isOn = false;

            switch (GameMgr.Scene_Category_Num)
            {
                case 20:

                    player_money = PlayerStatus.player_money;
                    _cost = shop_database.shopitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;

                    break;

                case 40:

                    player_money = PlayerStatus.player_money;
                    _cost = shop_database.shopitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;

                    break;

                case 50:

                    switch (shop_database.shopitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_dongriType)
                    {
                        case 0: //エメラルどんぐり

                            //emeraldonguriID = pitemlist.SearchItemString("emeralDongri");

                            player_money = pitemlist.playeritemlist["emeralDongri"];
                            _cost = shop_database.shopitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;
                            break;

                        case 1: //サファイアどんぐり

                            //emeraldonguriID = pitemlist.SearchItemString("sapphireDongri");

                            player_money = pitemlist.playeritemlist["sapphireDongri"];
                            _cost = shop_database.shopitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;
                            break;
                    }
                            

                    break;
            }

            //お金が足りない場合は、選択できないようにする。
            if (player_money < _cost)
            {
                shopitemlistController._shop_listitem[i].GetComponent<Toggle>().interactable = false;
            }
            else
            {
                shopitemlistController._shop_listitem[i].GetComponent<Toggle>().interactable = true;
            }

            shopitemlistController._shop_listitem[i].GetComponent<ButtonAnimTrigger>().OnEnterAnimScaleDown();
        }

        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().interactable = true;
        }
    }
}

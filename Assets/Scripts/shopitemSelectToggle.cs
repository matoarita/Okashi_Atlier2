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

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;

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

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    public int toggle_shop_ID; //こっちは、ショップデータベース上のIDを保持する。
    public int toggle_shopitem_ID; //リストの要素自体に、アイテムDB上のアイテムIDを保持する。
    public string toggle_shopitem_nameHyouji; //表示用名前
    public int toggle_shopitem_costprice; //金額も保持
    public int toggle_shopitem_type; //リストの要素に、通常アイテムか、イベントアイテム判定用のタイプを保持する。

    private int i;

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

        yes = shopitemlistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = shopitemlistController_obj.transform.Find("No").gameObject;
        no_text = no.GetComponentInChildren<Text>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        

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


        //カテゴリータブの取得
        category_toggle.Clear();
        foreach (Transform child in shopitemlistController_obj.transform.Find("CategoryView/Viewport/Content/").transform)
        {
            //Debug.Log(child.name);           
            category_toggle.Add(child.gameObject);
        }        


        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        i = 0;

        count = 0;

        yes.SetActive(false);
        no.SetActive(false);
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
        _item_Namehyouji = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_nameHyouji; //表示用ネームを入れる。
        shopitemlistController.shop_itemName_Hyouji = _item_Namehyouji;
        shopitemlistController.shop_costprice = shopitemlistController._shop_listitem[count].GetComponent<shopitemSelectToggle>().toggle_shopitem_costprice;

        if (shopitemlistController.shop_itemType == 1) //レシピを選択したとき
        {
            _text.text = _item_Namehyouji + "を何個買いますか？";
        }
        else if (shopitemlistController.shop_itemType == 5) //エメラルドショップのアイテムを選択したとき
        {
            _itemcount = pitemlist.KosuCountEmerald(pitemlist.emeralditemlist[shopitemlistController.shop_kettei_item1].event_itemName);
            _text.text = _item_Namehyouji + "を買いますか？" + "\n" + "個数を選択してください。" + "\n" + "現在の所持数: " + _itemcount;
        }
        else //それ以外の通常のアイテムは個数が表示
        {           
            _itemcount = pitemlist.KosuCount(database.items[shopitemlistController.shop_kettei_item1].itemName);
            _text.text = _item_Namehyouji + "を買いますか？" + "\n" + "個数を選択してください。" + "\n" + "現在の所持数: " + _itemcount;
        }

        Debug.Log(count + "番が押されたよ");
        Debug.Log("アイテム:" + _item_Namehyouji + "が選択されました。");

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

        yes.SetActive(true);
        no.SetActive(true);
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

                shopitemlistController.shop_final_itemkosu_1 = updown_counter.updown_kosu; //最終個数を入れる。

                shopitemlistController.shop_final_select_flag = true; //確認のフラグ

                Debug.Log("選択完了！");
                
                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");

                _text.text = "何にしますか？";

                //キャンセル時、リストのインタラクティブ解除。その時、プレイヤーの所持金をチェックし、足りないものはOFF表示にする。
                Money_Check();

                yes.SetActive(false);
                no.SetActive(false);
                updown_counter_obj.SetActive(false);

                back_ShopFirst_btn.interactable = true;

                break;
        }
    }


    IEnumerator shop_buy_Final_select()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Emerald_Shop":

                _text.text = shopitemlistController.shop_itemName_Hyouji + "を　" + shopitemlistController.shop_final_itemkosu_1 + "個 買いますか？" + "\n" +
            "エメラルどんぐり　" + GameMgr.ColorYellow + shopitemlistController.shop_costprice * shopitemlistController.shop_final_itemkosu_1 + "個</color>" + "いただくよ。";
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

                yes.SetActive(false);
                no.SetActive(false);
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

                yes_selectitem_kettei.kettei1 = false;
                yes.SetActive(false);
                no.SetActive(false);

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

            switch (SceneManager.GetActiveScene().name)
            {
                case "Shop":

                    player_money = PlayerStatus.player_money;
                    _cost = shop_database.shopitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;

                    break;

                case "Farm":

                    player_money = PlayerStatus.player_money;
                    _cost = shop_database.farmitems[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;

                    break;

                case "Emerald_Shop":

                    emeraldonguriID = pitemlist.SearchItemString("emeralDongri");

                    player_money = pitemlist.playeritemlist[emeraldonguriID];
                    _cost = shop_database.emeraldshop_items[shopitemlistController._shop_listitem[i].GetComponent<shopitemSelectToggle>().toggle_shop_ID].shop_costprice;

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

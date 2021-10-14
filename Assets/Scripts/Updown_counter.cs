using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Updown_counter : MonoBehaviour {

    private GameObject canvas;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private QuestSetDataBase quest_database;

    private ItemShopDataBase shop_database;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject shop_Main_obj;
    private Shop_Main shop_Main;

    private GameObject farm_Main_obj;
    private Farm_Main farm_Main;

    private GameObject emeraldshop_Main_obj;
    private EmeraldShop_Main emeraldshop_Main;

    private GameObject text_area;
    private Text _text;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private GameObject shopitemlistController_obj;
    private ShopItemListController shopitemlistController;

    private PlayerItemList pitemlist;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private Text _count_text;
    public int updown_kosu;
    private int listkosu_count;

    private int i, count, _zaiko_max;
    private int _item_max1;
    private int _item_max2;
    private int _item_max3;

    private int itemID_1;
    private string itemname_1;

    private string cmpitem_name1;
    private string cmpitem_name2;
    private string cmpitem_name3;

    private string cmpitem_namehyouji1;
    private string cmpitem_namehyouji2;
    private string cmpitem_namehyouji3;

    private int cmpitem1_type;
    private int cmpitem2_type;
    private int cmpitem3_type;

    private string _a;
    private string _b;
    private string _c;

    private int cmpitem_kosu1;
    private int cmpitem_kosu2;
    private int cmpitem_kosu3;

    private int cmpitem_kosu1_select;
    private int cmpitem_kosu2_select;
    private int cmpitem_kosu3_select;

    private int itemdb_id1;
    private int itemdb_id2;
    private int itemdb_id3;

    private int player_itemkosu1;
    private int player_itemkosu2;
    private int player_itemkosu3;

    private int emeraldonguriID;
    private int kaerucoin;

    private Button[] updown_button = new Button[2];

    private GameObject updown_button_Big;
    private GameObject updown_button_Small;
    private GameObject updown_counter_setpanel;

    private int _itemcount;

    private int _p_or_recipi_flag;

    public bool OpenFlag;

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        updown_button = this.GetComponentsInChildren<Button>();
        updown_button[0].interactable = true;
        updown_button[1].interactable = true;        

        OpenFlag = false;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                if (GameMgr.compound_status == 110) //最後、何セット作るかを確認中
                { }
                else
                {
                    this.transform.localPosition = new Vector3(0, -80, 0);
                }
                break;

            case "Shop":

                ShopUpdownCounter_Pos();
                break;

            case "Farm":

                ShopUpdownCounter_Pos();
                break;

            case "Emerald_Shop":

                ShopUpdownCounter_Pos();
                break;

            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        //初期位置の更新がEnableだとうまくいかないので、強引にフラグを使って、Update内で処理
        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                if (OpenFlag != true)
                {
                    if (shop_Main.shop_scene == 1) //ショップ「買う」の時
                    {
                        ShopUpdownCounter_Pos();
                    }
                    else if (shop_Main.shop_scene == 3) //依頼の納品の時
                    {
                        ShopUpdownCounter_Pos2();
                    }

                    OpenFlag = true;
                }

                break;
        }
    }

    void ShopUpdownCounter_Pos()
    {
        this.transform.localPosition = new Vector3(280, -60, 0);
    }

    void ShopUpdownCounter_Pos2()
    {
        this.transform.localPosition = new Vector3(0, -80, 0);
    }

    void OnEnable()
    {
        //Debug.Log("Reset Updown Counter");

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        updown_button = this.GetComponentsInChildren<Button>();
        updown_button[0].interactable = true;
        updown_button[1].interactable = true;

        updown_button_Big = this.transform.Find("up_big").gameObject;
        updown_button_Big.SetActive(false);
        updown_button_Small = this.transform.Find("up_small").gameObject;
        updown_button_Small.SetActive(false);

        updown_counter_setpanel = this.transform.Find("SetPanel").gameObject;
        updown_counter_setpanel.SetActive(false);

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Shop":

                shop_Main_obj = GameObject.FindWithTag("Shop_Main");
                shop_Main = shop_Main_obj.GetComponent<Shop_Main>();

                //ショップリスト画面の取得
                shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
                shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();

                pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

                yes = pitemlistController_obj.transform.Find("Yes").gameObject;
                no = pitemlistController_obj.transform.Find("No").gameObject;
                yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

                if (shop_Main.shop_scene == 1 || shop_Main.shop_scene == 5)
                {
                    updown_button_Big.SetActive(true);
                    updown_button_Small.SetActive(true);
                }
                else
                {
                    updown_button_Big.SetActive(false);
                    updown_button_Small.SetActive(false);
                }

                break;

            case "Farm":

                farm_Main_obj = GameObject.FindWithTag("Farm_Main");
                farm_Main = farm_Main_obj.GetComponent<Farm_Main>();

                //ショップリスト画面の取得
                shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                updown_button_Big.SetActive(true);
                updown_button_Small.SetActive(true);

                break;

            case "Emerald_Shop":

                emeraldshop_Main_obj = GameObject.FindWithTag("EmeraldShop_Main");
                emeraldshop_Main = emeraldshop_Main_obj.GetComponent<EmeraldShop_Main>();

                //ショップリスト画面の取得
                shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                updown_button_Big.SetActive(true);
                updown_button_Small.SetActive(true);

                break;

            case "Compound":

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
                pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

                recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
                recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

                if (recipilistController_obj.activeSelf == true)
                {
                    yes = canvas.transform.Find("Yes_no_Panel/Yes").gameObject;
                    no = canvas.transform.Find("Yes_no_Panel/No").gameObject;
                    yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

                    _p_or_recipi_flag = 1;
                }
                else if (pitemlistController_obj.activeSelf == true)
                {
                    _p_or_recipi_flag = 0;
                }

                if (GameMgr.compound_status == 110) //最後、何セット作るかを確認中
                {
                    //this.transform.localPosition = new Vector3(115, -106, 0);
                    //this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    updown_counter_setpanel.SetActive(true);
                    this.transform.Find("counter_img1").gameObject.SetActive(false);
                }
                else
                {
                    this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    this.transform.Find("counter_img1").gameObject.SetActive(true);

                    switch (GameMgr.compound_select)
                    {
                        case 1: //レシピ調合の場合

                            this.transform.localPosition = new Vector3(0, -87, 0);
                            break;

                        case 3: //オリジナル調合の場合の、カウンターの位置

                            this.transform.localPosition = new Vector3(0, -87, 0);
                            break;

                        default:

                            this.transform.localPosition = new Vector3(100, -120, 0);
                            break;

                    }
                }

                break;

            default:

                _p_or_recipi_flag = 0;

                break;
        }

        updown_kosu = 1;
        _zaiko_max = 0;

        _count_text = transform.Find("counter_num").GetComponent<Text>();
        _count_text.text = updown_kosu.ToString();

    }



    public void OnClick_up()
    {
        if (SceneManager.GetActiveScene().name == "Compound")
        {

            if (_p_or_recipi_flag == 0) //プレイヤーアイテムリストのときの処理
            {
                if (GameMgr.compound_status == 110) //最後、何セット作るかを確認中
                {
                    if (GameMgr.compound_select == 3) //オリジナル調合のとき
                    {
                        //カウントをとりあえず１足す
                        ++updown_kosu;

                        switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _item_max1 = pitemlist.playeritemlist[pitemlistController.kettei_item1];
                                break;

                            case 1:

                                _item_max1 = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _item_max2 = pitemlist.playeritemlist[pitemlistController.kettei_item2];
                                break;

                            case 1:

                                _item_max2 = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        if (pitemlistController.kettei_item3 != 9999) //３個目も選んでいれば、下の処理を起動
                        {
                            switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                            {
                                case 0:

                                    _item_max3 = pitemlist.playeritemlist[pitemlistController.kettei_item3];
                                    break;

                                case 1:

                                    _item_max3 = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                    break;

                                default:
                                    break;
                            }

                            player_itemkosu3 = pitemlistController.final_kettei_kosu3 * updown_kosu;
                        }

                        player_itemkosu1 = pitemlistController.final_kettei_kosu1 * updown_kosu;
                        player_itemkosu2 = pitemlistController.final_kettei_kosu2 * updown_kosu;

                        /*Debug.Log("アイテム1 所持数: " + _item_max1 + " プレイヤーカウンタ個数1 : " + player_itemkosu1);
                        Debug.Log("アイテム2 所持数: " + _item_max2 + " プレイヤーカウンタ個数2 : " + player_itemkosu2);
                        Debug.Log("アイテム3 所持数: " + _item_max3 + " プレイヤーカウンタ個数3 : " + player_itemkosu3);
                        Debug.Log("３個めのアイテムを選んだかどうか （空の場合9999）: " + pitemlistController.kettei_item3);*/


                        //判定。もしどれかのアイテムの一つでも、個数がmaxより超えたら、そこがセット数の上限
                        if (pitemlistController.kettei_item3 == 9999) //３個目も選んでいれば、下の処理を起動
                        {
                            if (player_itemkosu1 > _item_max1 || player_itemkosu2 > _item_max2)
                            {
                                //Debug.Log("どれか一つのアイテムが、所持数を超えた");
                                updown_kosu--;
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            if (player_itemkosu1 > _item_max1 || player_itemkosu2 > _item_max2 || player_itemkosu3 > _item_max3)
                            {
                                //Debug.Log("どれか一つのアイテムが、所持数を超えた");
                                updown_kosu--;
                            }
                            else
                            {
                            }
                        }

                        //表示を更新は下にある。

                    }
                }
                else
                {
                    if (GameMgr.compound_select == 2 || GameMgr.compound_select == 3)
                    {
                        switch (pitemlistController.kettei1_bunki)
                        {
                            case 1:

                                switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 2:

                                switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item2]; //二個目の決定アイテムの所持数

                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 3:

                                switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item3]; //三個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 10:

                                switch (pitemlistController._listitem[pitemlistController._base_count].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.base_kettei_item]; //ベース決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.base_kettei_item].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 11:

                                switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 12:

                                switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item2]; //二個目の決定アイテムの所持数                           
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 13:

                                switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item3]; //三個目の決定アイテムの所持数                           
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            default:
                                break;
                        }
                    }

                    else if (GameMgr.compound_select == 5)
                    {
                        switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1]; //一個目の決定アイテムの所持数
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            default:
                                break;
                        }
                    }

                    AddMethod1();
                    
                }
            }
            else //レシピリストのときの処理
            {

                _zaiko_max = 99;

                AddMethod1();

                //個数を変えた際に、必要アイテム数と、所持アイテム数を比較するメソッド
                updown_keisan_Method();

            }

            _count_text.text = updown_kosu.ToString();
        }

        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (shop_Main.shop_scene == 1)　//ショップ「買う」のとき
            {
                _zaiko_max = shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

                AddMethod1();

                if (PlayerStatus.player_money < shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * updown_kosu)
                {
                    //お金が足りない
                    _text.text = "お金が足りない。";

                    updown_kosu--;
                }

                _count_text.text = updown_kosu.ToString();
            }

            if (shop_Main.shop_scene == 3)　//ショップ納品のとき
            {
                listkosu_count = 0;

                //現在選択済みの個数をすべてトータル
                for (i = 0; i < pitemlistController._listkosu.Count; i++)
                {
                    listkosu_count += pitemlistController._listkosu[i];
                }

                switch (pitemlistController._toggle_type1)
                {
                    case 0:

                        if (pitemlist.playeritemlist[pitemlistController.kettei_item1] >= 
                            quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default - listkosu_count)
                        {                           
                            _zaiko_max = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default 
                                - listkosu_count; //クエストの必要個数-今まで選択しているアイテムの個数
                        }
                        else
                        {
                            _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1];
                        }
                        break;

                    case 1:

                        if (pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu >= 
                            quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default - listkosu_count)
                        {
                            _zaiko_max = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default 
                                - listkosu_count; //クエストの必要個数-今まで選択しているアイテムの個数
                        }
                        else
                        {
                            _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                        }

                        break;

                    default:
                        break;
                }

                AddMethod1();
            }

            if (shop_Main.shop_scene == 5)　//ショップ「売る」のとき
            {
                switch (pitemlistController._toggle_type1)
                {
                    case 0:

                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.final_kettei_item1];
                        break;

                    case 1:

                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.final_kettei_item1].ItemKosu;

                        break;

                    default:
                        break;
                }


                AddMethod1();

                //ウィンドウの表示も変える。
                SellYosokuText();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Farm")
        {

            _zaiko_max = shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

            AddMethod1();

            if (PlayerStatus.player_money < shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

                updown_kosu--;
            }

            _count_text.text = updown_kosu.ToString();
        }
        else if (SceneManager.GetActiveScene().name == "Emerald_Shop")
        {

            _zaiko_max = shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

            AddMethod1();

            emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
            kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

            if (kaerucoin < shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "エメラルどんぐりが足りない。";

                updown_kosu--;
            }

            _count_text.text = updown_kosu.ToString();
        }
    }

    public void OnClickup_Big()
    {
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (shop_Main.shop_scene == 1)
            {
                _zaiko_max = shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

                AddMethod2();

                if (PlayerStatus.player_money < shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * updown_kosu)
                {
                    //お金が足りない
                    _text.text = "お金が足りない。";

                    updown_kosu = updown_kosu - 10;
                }

                _count_text.text = updown_kosu.ToString();
            }

            if (shop_Main.shop_scene == 5)
            {
                switch (pitemlistController._toggle_type1)
                {
                    case 0:

                        _zaiko_max = pitemlist.playeritemlist[pitemlistController.final_kettei_item1];
                        break;

                    case 1:

                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.final_kettei_item1].ItemKosu;

                        break;

                    default:
                        break;
                }

                AddMethod2();

                //ウィンドウの表示も変える。
                SellYosokuText();
                
            }
        }
        else if (SceneManager.GetActiveScene().name == "Farm")
        {

            _zaiko_max = shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

            AddMethod2();

            if (PlayerStatus.player_money < shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

                updown_kosu = updown_kosu - 10;
            }

            _count_text.text = updown_kosu.ToString();

        }
        else if (SceneManager.GetActiveScene().name == "Emerald_Shop")
        {

            _zaiko_max = shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

            AddMethod2();

            emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
            kaerucoin = pitemlist.playeritemlist[emeraldonguriID];

            if (kaerucoin < shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "エメラルどんぐりが足りない。";

                updown_kosu = updown_kosu - 10;
            }

            _count_text.text = updown_kosu.ToString();
        }
    }



    public void OnClick_down()
    {
        if (SceneManager.GetActiveScene().name == "Compound")
        {
            if (_p_or_recipi_flag == 0) //プレイヤーアイテムリストのときの処理
            {
                DegMethod1();

                if (GameMgr.compound_status == 110) //最後、何セット作るかを確認中
                {
                    player_itemkosu1 = pitemlistController.final_kettei_kosu1 * updown_kosu;
                    player_itemkosu2 = pitemlistController.final_kettei_kosu2 * updown_kosu;

                    if (pitemlistController.kettei_item3 != 9999) //３個目も選んでいれば、下の処理を起動
                    {
                        player_itemkosu3 = pitemlistController.final_kettei_kosu3 * updown_kosu;
                    }
                }
            }
            else //レシピリストのときの処理
            {
                DegMethod1();

                //個数を変えた際に、必要アイテム数と、所持アイテム数を比較するメソッド
                updown_keisan_Method();

            }

        }
        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (shop_Main.shop_scene == 1) //買い物のとき
            {
                DegMethod1();

                //ウィンドウの表示も変える。
                BuyYosokuText();
            }
            else if (shop_Main.shop_scene == 5) //売るとき
            {
                DegMethod1();

                //ウィンドウの表示も変える。
                SellYosokuText();
            }
            else
            {
                DegMethod1();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Emerald_Shop")
        {
            DegMethod1();

            //ウィンドウの表示も変える。
            BuyYosokuText();
        }
        else
        {
            DegMethod1();
        }
    }

    public void OnClickdown_Small()
    {
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (shop_Main.shop_scene == 1) //買い物のとき
            {
                DegMethod2();

                //ウィンドウの表示も変える。
                BuyYosokuText();
            }
            else if (shop_Main.shop_scene == 5) //売るとき
            {
                DegMethod2();                

                //ウィンドウの表示も変える。
                SellYosokuText();
            }
            else
            {
                DegMethod2();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Emerald_Shop")
        {
            DegMethod2();

            //ウィンドウの表示も変える。
            BuyYosokuText();
        }
        else
        {
            DegMethod2();
        }
    }

    void AddMethod1()
    {
        ++updown_kosu;
        if (updown_kosu > _zaiko_max)
        {
            updown_kosu = _zaiko_max;
        }

        _count_text.text = updown_kosu.ToString();
    }

    void AddMethod2()
    {
        updown_kosu = updown_kosu + 10;
        if (updown_kosu > _zaiko_max)
        {
            updown_kosu = _zaiko_max;
        }

        _count_text.text = updown_kosu.ToString();
    }

    void DegMethod1()
    {
        --updown_kosu;
        if (updown_kosu <= 1)
        {
            updown_kosu = 1;
        }

        _count_text.text = updown_kosu.ToString();
    }

    void DegMethod2()
    {
        updown_kosu = updown_kosu - 10;
        if (updown_kosu <= 1)
        {
            updown_kosu = 1;
        }

        _count_text.text = updown_kosu.ToString();
    }

    //レシピリストのときの処理
    public void updown_keisan_Method()
    {
        count = recipilistController._count1;
        itemID_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleCompoitem_ID; //itemID_1という変数に、プレイヤーが選択した調合DBの配列番号を格納する。
        itemname_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_itemNameHyouji;

        recipilistController.final_select_kosu = updown_kosu; //選択個数(セットの回数）

        //必要アイテム・個数の代入
        cmpitem_kosu1 = databaseCompo.compoitems[itemID_1].cmpitem_kosu1;
        cmpitem_kosu2 = databaseCompo.compoitems[itemID_1].cmpitem_kosu2;
        cmpitem_kosu3 = databaseCompo.compoitems[itemID_1].cmpitem_kosu3;

        player_itemkosu1 = 0;
        player_itemkosu2 = 0;
        player_itemkosu3 = 0;

        i = 0;

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_1)
            {
                cmpitem_name1 = database.items[i].itemName; //一個目に選択したアイテム名。店売り・オリジナルでこの名前は共通。
                cmpitem_namehyouji1 = database.items[i].itemNameHyouji; //調合DB一個目の番号を保存。また、nameを日本語表示に。

                if (database.items[i].itemType_sub.ToString() == "Machine") {
                    cmpitem1_type = 1;//機材アイテムなどの、個数が関係ないアイテムかどうかをチェック
                }
                else { cmpitem1_type = 0; }

                itemdb_id1 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }

        i = 0;

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_2)
            {
                cmpitem_name2 = database.items[i].itemName; //二個目に選択したアイテム名。
                cmpitem_namehyouji2 = database.items[i].itemNameHyouji; //調合DB二個目のnameを日本語表示に。

                if (database.items[i].itemType_sub.ToString() == "Machine")
                {
                    cmpitem2_type = 1;//機材アイテムなどの、個数が関係ないアイテムかどうかをチェック
                }
                else { cmpitem2_type = 0; }

                itemdb_id2 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }

        i = 0;

        itemdb_id3 = 9999; //空の可能性もあるので、もし空なら9999にしておく。あれば、iで更新される。

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_3)
            {
                cmpitem_name3 = database.items[i].itemName; //三個目に選択したアイテム名。
                cmpitem_namehyouji3 = database.items[i].itemNameHyouji; //調合DB三個目のnameを日本語表示に。

                if (database.items[i].itemType_sub.ToString() == "Machine")
                {
                    cmpitem3_type = 1;//機材アイテムなどの、個数が関係ないアイテムかどうかをチェック
                }
                else { cmpitem3_type = 0; }

                itemdb_id3 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }

        if(cmpitem1_type == 0)
        {
            cmpitem_kosu1_select = cmpitem_kosu1 * updown_kosu; //必要個数×選択している作成数
        }
        else
        {
            cmpitem_kosu1_select = cmpitem_kosu1; //機材アイテムなどは、個数が反映されない。基本は1
        }

        if (cmpitem2_type == 0)
        {
            cmpitem_kosu2_select = cmpitem_kosu2 * updown_kosu; //必要個数×選択している作成数
        }
        else
        {
            cmpitem_kosu2_select = cmpitem_kosu2; //機材アイテムなどは、個数が反映されない。基本は1
        }

        if (cmpitem3_type == 0)
        {
            cmpitem_kosu3_select = cmpitem_kosu3 * updown_kosu; //必要個数×選択している作成数
        }
        else
        {
            cmpitem_kosu3_select = cmpitem_kosu3; //機材アイテムなどは、個数が反映されない。基本は1
        }


        //
        //*** 材料個数足りてるかの判定 ***//

        //まずは、店売り・オリジナルの両方から、アイテムを探し、トータルの個数を取得。オリジナルは、itemNameが複数ある場合もあるのでforで全てカウント。（クッキーの味違いなど）IDは違う。

        //一個目
        //オリジナルの所持個数を計算
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            if ( cmpitem_name1 == pitemlist.player_originalitemlist[i].itemName )
            {
                player_itemkosu1 += pitemlist.player_originalitemlist[i].ItemKosu;
            }
        }
        //店売りの所持個数を計算
        player_itemkosu1 += pitemlist.playeritemlist[itemdb_id1];

        //二個目
        //オリジナルの所持個数を計算
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            if (cmpitem_name2 == pitemlist.player_originalitemlist[i].itemName)
            {
                player_itemkosu2 += pitemlist.player_originalitemlist[i].ItemKosu;
            }
        }
        //店売りの所持個数を計算
        player_itemkosu2 += pitemlist.playeritemlist[itemdb_id2];

        //三個目
        //オリジナルの所持個数を計算
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            if (cmpitem_name3 == pitemlist.player_originalitemlist[i].itemName)
            {
                player_itemkosu3 += pitemlist.player_originalitemlist[i].ItemKosu;
            }
        }
        //店売りの所持個数を計算
        player_itemkosu3 += pitemlist.playeritemlist[itemdb_id3];




        //テキストの更新　左：現在の所持数　右：必要個数
        _a = cmpitem_namehyouji1 + ": " + GameMgr.ColorYellow + player_itemkosu1 + "</color>" + "／" + cmpitem_kosu1_select;
        _b = cmpitem_namehyouji2 + ": " + GameMgr.ColorYellow + player_itemkosu2 + "</color>" + "／" + cmpitem_kosu2_select;
        _c = cmpitem_namehyouji3 + ": " + GameMgr.ColorYellow + player_itemkosu3 + "</color>" + "／" + cmpitem_kosu3_select;

        //材料個数が足りてるかの判定し、足りてないときは赤字にテキスト更新
        if (cmpitem_kosu1_select > player_itemkosu1)
        {
            _a = GameMgr.ColorRed + cmpitem_namehyouji1 + ": " + player_itemkosu1 + "／" + cmpitem_kosu1_select + "</color>";
        }
        if (cmpitem_kosu2_select > player_itemkosu2)
        {
            _b = GameMgr.ColorRed + cmpitem_namehyouji2 + ": " + player_itemkosu2 + "／" + cmpitem_kosu2_select + "</color>";
        }
        if (cmpitem_kosu3_select > player_itemkosu3)
        {
            _c = GameMgr.ColorRed + cmpitem_namehyouji3 + ": " + player_itemkosu3 + "／" + cmpitem_kosu3_select + "</color>";
        }

        
        //さらにテキスト続き
        if (databaseCompo.compoitems[itemID_1].cmpitemID_3 == "empty") //2個のアイテムが必要な場合
        {

            if (cmpitem_kosu1_select > player_itemkosu1 || cmpitem_kosu2_select > player_itemkosu2)
            {
                _text.text = itemname_1 + "材料が足りない..。" + "\n" + _a + "\n" + _b;
                updown_button[1].interactable = false;
                yes.SetActive(false);
            }
            else
            {
                _text.text = itemname_1 + "が選択されました。何個作る？" + "\n" + _a + "\n" + _b;
                yes.SetActive(true);
                updown_button[1].interactable = true;

            }

        }
        else //3個アイテムが必要な場合
        {
            if (cmpitem_kosu1_select > player_itemkosu1 || cmpitem_kosu2_select > player_itemkosu2 || cmpitem_kosu3_select > player_itemkosu3)
            {
                _text.text = "材料が足りない..。" + "\n" + _a + "\n" + _b + "\n" + _c;
                updown_button[1].interactable = false;
                yes.SetActive(false);
            }
            else
            {
                _text.text = itemname_1 + "が選択されました。何個作る？" + "\n" + _a + "\n" + _b + "\n" + _c;
                yes.SetActive(true);
                updown_button[1].interactable = true;

            }
        }

        //
        //使用するアイテムを最終的に決定。「店売り」→「オリジナル」の順で、自動で決定する。ただし、削除処理はここではなく、Compound_Keisanで行う。
        //

        //レシピから作る場合は、オリジナルを使用したとしても、「店売り」のデフォルトの味パラメータのものが生成される。（ややこしいので）
        //オリジナルの味パラメータをレシピから再現するなら、オリジナルレシピをユーザーが登録できる、みたいな機能が必要。
        //


        //最終的な必要アイテム（パラメータは、現在デフォルトの値のみ）＋最終個数をコントローラー側の変数に代入

        recipilistController.kettei_recipiitem1 = database.items[itemdb_id1].itemID;
        recipilistController.kettei_recipiitem2 = database.items[itemdb_id2].itemID;

        recipilistController.final_kettei_recipikosu1 = cmpitem_kosu1;
        recipilistController.final_kettei_recipikosu2 = cmpitem_kosu2;

        if (databaseCompo.compoitems[itemID_1].cmpitemID_3 == "empty") //2個のアイテムが必要な場合。３個めは空＝9999
        {
            recipilistController.kettei_recipiitem3 = 9999;
            recipilistController.final_kettei_recipikosu3 = 0;
        }
        else //3個アイテムが必要な場合
        {
            recipilistController.kettei_recipiitem3 = database.items[itemdb_id3].itemID;
            recipilistController.final_kettei_recipikosu3 = cmpitem_kosu3;
        }
    }

    void BuyYosokuText()
    {
        _itemcount = pitemlist.KosuCount(database.items[shopitemlistController.shop_kettei_item1].itemName);
        _text.text = shopitemlistController.shop_itemName_Hyouji + "を買いますか？" + "\n" + "個数を選択してください。" + "\n" + "現在の所持数: " + _itemcount;
    }

    void SellYosokuText()
    {
        _text.text = database.items[pitemlistController.final_kettei_item1].itemNameHyouji + "が選択されました。　" +
            GameMgr.ColorYellow + database.items[pitemlistController.final_kettei_item1].sell_price * updown_kosu + " " + GameMgr.MoneyCurrency + "</color>"
            + "\n" + "個数を選択してください";
    }

    
}

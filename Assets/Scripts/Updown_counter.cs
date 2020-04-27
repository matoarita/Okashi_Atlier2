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

    private GameObject text_area;
    private Text _text;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject recipilistController_obj;
    private RecipiListController recipilistController;

    private GameObject shopitemlistcontroller_obj;
    private ShopItemListController shopitemlistcontroller;

    private PlayerItemList pitemlist;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private Text _count_text;
    public int updown_kosu;

    private int i, count, _zaiko_max;

    private int itemID_1;
    private string itemname_1;

    private string cmpitem_1;
    private string cmpitem_2;
    private string cmpitem_3;

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

    private Button[] updown_button = new Button[2];

    private GameObject updown_button_Big;
    private GameObject updown_button_Small;

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

                this.transform.localPosition = new Vector3(0, -80, 0);
                break;

            case "Shop":

                this.transform.localPosition = new Vector3(280, -15, 0);
                break;

            case "Farm":

                this.transform.localPosition = new Vector3(280, -15, 0);
                break;

            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        //初期位置の更新がEnableだとうまくいかないので、強引にフラグを使って、Update内で処理
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (OpenFlag != true)
            {
                if (shop_Main.shop_scene == 1)
                {
                    this.transform.localPosition = new Vector3(280, -15, 0);
                }
                else if (shop_Main.shop_scene == 3)
                {
                    this.transform.localPosition = new Vector3(0, -80, 0);

                    switch (pitemlistController._toggle_type1)
                    {
                        case 0:

                            if (pitemlist.playeritemlist[pitemlistController.kettei_item1] < quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                            {
                                _text.text = "数が足りない..。";
                                yes.SetActive(false);

                                _count_text.text = updown_kosu.ToString();
                            }
                            else
                            {
                                _text.text = database.items[pitemlistController.kettei_item1].itemNameHyouji + "を " + quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default + "個" + "\n" + "渡しますか？";
                                updown_kosu = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default;

                                _count_text.text = updown_kosu.ToString();
                            }
                            break;

                        case 1:

                            if (pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu < quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                            {
                                _text.text = "数が足りない..。";
                                yes.SetActive(false);

                                _count_text.text = updown_kosu.ToString();
                            }
                            else
                            {
                                _text.text = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].itemNameHyouji + "を " + quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default + "個" + "\n" + "渡しますか？";
                                updown_kosu = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default;

                                _count_text.text = updown_kosu.ToString();
                            }

                            break;

                        default:
                            break;
                    }

                    updown_button[0].interactable = false;
                    updown_button[1].interactable = false;
                    /*
                    if (quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default == 1)
                    {
                        //yesをon
                        yes.SetActive(true);
                    }
                    else
                    {
                        yes.SetActive(false);
                    }*/

                }

                OpenFlag = true;
            }
        }
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

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        if (SceneManager.GetActiveScene().name == "Shop")
        {
            shop_Main_obj = GameObject.FindWithTag("Shop_Main");
            shop_Main = shop_Main_obj.GetComponent<Shop_Main>();

            //ショップリスト画面の取得
            shopitemlistcontroller_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
            shopitemlistcontroller = shopitemlistcontroller_obj.GetComponent<ShopItemListController>();

            shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
            shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();

            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            yes = pitemlistController_obj.transform.Find("Yes").gameObject;
            yes_text = yes.GetComponentInChildren<Text>();
            no = pitemlistController_obj.transform.Find("No").gameObject;
            yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

            if (shop_Main.shop_scene == 1)
            {
                updown_button_Big.SetActive(true);
                updown_button_Small.SetActive(true);
            }
            else
            {
                updown_button_Big.SetActive(false);
                updown_button_Small.SetActive(false);
            }

        }
        else if (SceneManager.GetActiveScene().name == "Farm")
        {
            farm_Main_obj = GameObject.FindWithTag("Farm_Main");
            farm_Main = farm_Main_obj.GetComponent<Farm_Main>();

            //ショップリスト画面の取得
            shopitemlistcontroller_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
            shopitemlistcontroller = shopitemlistcontroller_obj.GetComponent<ShopItemListController>();

            updown_button_Big.SetActive(true);
            updown_button_Small.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "Compound")
        {
            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

            if (recipilistController_obj.activeSelf == true)
            {
                yes = recipilistController_obj.transform.Find("Yes").gameObject;
                yes_text = yes.GetComponentInChildren<Text>();
                no = recipilistController_obj.transform.Find("No").gameObject;
                yes_selectitem_kettei = yes.GetComponent<SelectItem_kettei>();

                _p_or_recipi_flag = 1;
            }
            else if (pitemlistController_obj.activeSelf == true)
            {
                _p_or_recipi_flag = 0;
            }

            switch (compound_Main.compound_select)
            {
                case 1: //レシピ調合の場合

                    this.transform.localPosition = new Vector3(0, -70, 0);
                    break;

                case 3: //オリジナル調合の場合の、カウンターの位置

                    this.transform.localPosition = new Vector3(0, -80, 0);
                    break;

                default:

                    this.transform.localPosition = new Vector3(100, -120, 0);
                    break;

            }
        }
        else
        {
            _p_or_recipi_flag = 0;
        }

        updown_kosu = 1;
        _zaiko_max = 0;

        _count_text = transform.GetChild(0).gameObject.GetComponent<Text>();
        _count_text.text = updown_kosu.ToString();

    }

    public void OnClick_up()
    {
        if (SceneManager.GetActiveScene().name == "Compound")
        {
            /*Debug.Log("updown_kosu: " + updown_kosu);
            Debug.Log("分岐: " + pitemlistController.kettei1_bunki);
            Debug.Log("選択したアイテムID: " + pitemlistController.final_kettei_item1);
            Debug.Log("選択したアイテム所持数: " + pitemlist.playeritemlist[pitemlistController.final_kettei_item1]);
            Debug.Log("選択したリスト番号: " + pitemlistController.kettei_item1);*/

            if (_p_or_recipi_flag == 0) //プレイヤーアイテムリストのときの処理
            {
                if (compound_Main.compound_select == 2 || compound_Main.compound_select == 3)
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
                else if (compound_Main.compound_select == 5)
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

                ++updown_kosu;

                if (updown_kosu > _zaiko_max)
                {
                    updown_kosu = _zaiko_max;
                }


            }
            else //レシピリストのときの処理
            {
                //_zaiko_max = pitemlist.playeritemlist[pitemlistController.final_kettei_item1]; //一個目の決定アイテムの所持数

                _zaiko_max = 9;

                ++updown_kosu;

                if (updown_kosu > _zaiko_max)
                {
                    updown_kosu = _zaiko_max;
                }

                //個数を変えた際に、必要アイテム数と、所持アイテム数を比較するメソッド
                updown_keisan_Method();

            }

            _count_text.text = updown_kosu.ToString();
        }

        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (shop_Main.shop_scene == 1)
            {
                _zaiko_max = shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_itemzaiko;

                ++updown_kosu;
                if (updown_kosu > _zaiko_max)
                {
                    updown_kosu = _zaiko_max;
                }

                if (PlayerStatus.player_money < shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_costprice * updown_kosu)
                {
                    //お金が足りない
                    _text.text = "お金が足りない。";

                    updown_kosu--;
                }

                _count_text.text = updown_kosu.ToString();
            }

            if (shop_Main.shop_scene == 3)
            {
                switch (pitemlistController._toggle_type1)
                {
                    case 0:

                        if (pitemlist.playeritemlist[pitemlistController.kettei_item1] >= quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                        {
                            _zaiko_max = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default; //一個目の決定アイテムの所持数
                        }
                        else
                        {

                            _zaiko_max = pitemlist.playeritemlist[pitemlistController.kettei_item1];
                        }
                        break;

                    case 1:

                        if (pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu >= quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                        {
                            _zaiko_max = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default; //一個目の決定アイテムの所持数
                        }
                        else
                        {

                            _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                        }

                        break;

                    default:
                        break;
                }

                ++updown_kosu;
                if (updown_kosu > _zaiko_max)
                {
                    updown_kosu = _zaiko_max;
                }


                if (updown_kosu >= quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                {
                    //yesをon
                    yes.SetActive(true);
                }
                else
                {
                    yes.SetActive(false);

                }
                _count_text.text = updown_kosu.ToString();
            }

        }
        else if (SceneManager.GetActiveScene().name == "Farm")
        {

            _zaiko_max = shop_database.farmitems[shopitemlistcontroller.shop_kettei_ID].shop_itemzaiko;

            ++updown_kosu;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
            }

            if (PlayerStatus.player_money < shop_database.farmitems[shopitemlistcontroller.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

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
                _zaiko_max = shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_itemzaiko;

                updown_kosu = updown_kosu + 10;
                if (updown_kosu > _zaiko_max)
                {
                    updown_kosu = _zaiko_max;
                }

                if (PlayerStatus.player_money < shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_costprice * updown_kosu)
                {
                    //お金が足りない
                    _text.text = "お金が足りない。";

                    updown_kosu = updown_kosu - 10;
                }

                _count_text.text = updown_kosu.ToString();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Farm")
        {

            _zaiko_max = shop_database.farmitems[shopitemlistcontroller.shop_kettei_ID].shop_itemzaiko;

            updown_kosu = updown_kosu + 10;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
            }

            if (PlayerStatus.player_money < shop_database.farmitems[shopitemlistcontroller.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

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
                --updown_kosu;
                if (updown_kosu <= 1)
                {
                    updown_kosu = 1;
                }

            }
            else //レシピリストのときの処理
            {
                --updown_kosu;
                if (updown_kosu <= 1)
                {
                    updown_kosu = 1;
                }

                //個数を変えた際に、必要アイテム数と、所持アイテム数を比較するメソッド
                updown_keisan_Method();

            }

            _count_text.text = updown_kosu.ToString();
        }
        else
        {
            --updown_kosu;
            if (updown_kosu <= 1)
            {
                updown_kosu = 1;
            }

            if (SceneManager.GetActiveScene().name == "Shop")
            {
                if (shop_Main.shop_scene == 3)
                {
                    if (updown_kosu >= quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default)
                    {
                        //yesをon
                        yes.SetActive(true);
                    }
                    else
                    {
                        yes.SetActive(false);

                    }
                }
            }

            _count_text.text = updown_kosu.ToString();
        }
    }

    public void OnClickdown_Small()
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

        recipilistController.final_select_kosu = updown_kosu; //選択個数

        //必要アイテム・個数の代入
        cmpitem_kosu1 = databaseCompo.compoitems[itemID_1].cmpitem_kosu1;
        cmpitem_kosu2 = databaseCompo.compoitems[itemID_1].cmpitem_kosu2;
        cmpitem_kosu3 = databaseCompo.compoitems[itemID_1].cmpitem_kosu3;

        i = 0;

        while (i < database.items.Count)
        {
            if (database.items[i].itemName == databaseCompo.compoitems[itemID_1].cmpitemID_1)
            {
                cmpitem_1 = database.items[i].itemNameHyouji; //調合DB一個目の番号を保存。また、nameを日本語表示に。

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
                cmpitem_2 = database.items[i].itemNameHyouji; //調合DB二個目のnameを日本語表示に。

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
                cmpitem_3 = database.items[i].itemNameHyouji; //調合DB三個目のnameを日本語表示に。

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
        

        _a = cmpitem_1 + ": " + GameMgr.ColorYellow + cmpitem_kosu1_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id1];
        _b = cmpitem_2 + ": " + GameMgr.ColorYellow + cmpitem_kosu2_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id2];
        _c = cmpitem_3 + ": " + GameMgr.ColorYellow + cmpitem_kosu3_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id3];

        //材料個数が足りてるかの判定
        if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1])
        {
            _a = GameMgr.ColorRed + cmpitem_1 + ": " + cmpitem_kosu1_select + "／" + pitemlist.playeritemlist[itemdb_id1] + "</color>";
        }
        if (cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2])
        {
            _b = GameMgr.ColorRed + cmpitem_2 + ": " + cmpitem_kosu2_select + "／" + pitemlist.playeritemlist[itemdb_id2] + "</color>";
        }
        if (cmpitem_kosu3_select > pitemlist.playeritemlist[itemdb_id3])
        {
            _c = GameMgr.ColorRed + cmpitem_3 + ": " + cmpitem_kosu3_select + "／" + pitemlist.playeritemlist[itemdb_id3] + "</color>";
        }

        

        if (databaseCompo.compoitems[itemID_1].cmpitemID_3 == "empty") //2個のアイテムが必要な場合
        {

            if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1] || cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2])
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
            if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1] || cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2] || cmpitem_kosu3_select > pitemlist.playeritemlist[itemdb_id3])
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


        //最終的な必要アイテム＋最終個数をコントローラー側の変数に代入

        recipilistController.kettei_recipiitem1 = database.items[itemdb_id1].itemID;
        recipilistController.kettei_recipiitem2 = database.items[itemdb_id2].itemID;

        recipilistController.final_kettei_recipikosu1 = cmpitem_kosu1_select;
        recipilistController.final_kettei_recipikosu2 = cmpitem_kosu2_select;

        if (databaseCompo.compoitems[itemID_1].cmpitemID_3 == "empty") //2個のアイテムが必要な場合。３個めは空＝9999
        {
            recipilistController.kettei_recipiitem3 = 9999;
            recipilistController.final_kettei_recipikosu3 = 0;
        }
        else //3個アイテムが必要な場合
        {
            recipilistController.kettei_recipiitem3 = database.items[itemdb_id3].itemID;
            recipilistController.final_kettei_recipikosu3 = cmpitem_kosu3_select;
        }
    }
}

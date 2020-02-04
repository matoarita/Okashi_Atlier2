using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Updown_counter : MonoBehaviour {

    private GameObject canvas;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private ItemShopDataBase shop_database;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

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

    private int _p_or_recipi_flag;

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        updown_button = this.GetComponentsInChildren<Button>();
        updown_button[0].interactable = true;
        updown_button[1].interactable = true;
        //this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnEnable()
    {
        //Debug.Log("Reset Updown Counter");

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //windowテキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        updown_button = this.GetComponentsInChildren<Button>();
        updown_button[0].interactable = true;
        updown_button[1].interactable = true;

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        if (SceneManager.GetActiveScene().name == "Shop")
        {
            //ショップリスト画面を開く。初期設定で最初はOFF。
            shopitemlistcontroller_obj = GameObject.FindWithTag("ShopitemList_ScrollView");
            shopitemlistcontroller = shopitemlistcontroller_obj.GetComponent<ShopItemListController>();

            _p_or_recipi_flag = 0;
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
            _zaiko_max = shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_itemzaiko;

            ++updown_kosu;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
            }

            if ( PlayerStatus.player_money < shop_database.shopitems[shopitemlistcontroller.shop_kettei_ID].shop_costprice * updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

                updown_kosu--;
            }

            _count_text.text = updown_kosu.ToString();
        }

        else
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

            ++updown_kosu;
            if (updown_kosu > _zaiko_max)
            {
                updown_kosu = _zaiko_max;
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

            _count_text.text = updown_kosu.ToString();
        }
    }


    //レシピリストのときの処理
    public void updown_keisan_Method()
    {
        count = recipilistController._count1;
        itemID_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleCompoitem_ID; //itemID_1という変数に、プレイヤーが選択した調合DBの配列番号を格納する。
        itemname_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_itemNameHyouji;

        recipilistController.final_select_kosu = updown_kosu; //最終的に作る個数

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
                itemdb_id3 = i; //その時のアイテムDB番号も、保存
                break;
            }
            ++i;
        }


        cmpitem_kosu1_select = cmpitem_kosu1 * updown_kosu; //必要個数×選択している作成数
        cmpitem_kosu2_select = cmpitem_kosu2 * updown_kosu; //必要個数×選択している作成数
        cmpitem_kosu3_select = cmpitem_kosu3 * updown_kosu; //必要個数×選択している作成数

        _a = cmpitem_1 + ": " + "<color=#0000ff>" + cmpitem_kosu1_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id1];
        _b = cmpitem_2 + ": " + "<color=#0000ff>" + cmpitem_kosu2_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id2];
        _c = cmpitem_3 + ": " + "<color=#0000ff>" + cmpitem_kosu3_select + "</color>" + "／" + pitemlist.playeritemlist[itemdb_id3];

        if (cmpitem_kosu1_select > pitemlist.playeritemlist[itemdb_id1])
        {
            _a = "<color=#ff0000>" + cmpitem_1 + ": " + cmpitem_kosu1_select + "／" + pitemlist.playeritemlist[itemdb_id1] + "</color>";
        }
        if (cmpitem_kosu2_select > pitemlist.playeritemlist[itemdb_id2])
        {
            _b = "<color=#ff0000>" + cmpitem_2 + ": " + cmpitem_kosu2_select + "／" + pitemlist.playeritemlist[itemdb_id2] + "</color>";
        }
        if (cmpitem_kosu3_select > pitemlist.playeritemlist[itemdb_id3])
        {
            _c = "<color=#ff0000>" + cmpitem_3 + ": " + cmpitem_kosu3_select + "／" + pitemlist.playeritemlist[itemdb_id3] + "</color>";
        }



        //材料個数が足りてるかの判定

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

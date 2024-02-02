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
    //private int updown_kosu;
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

    // Use this for initialization
    void Start () {

        

    }
	
	// Update is called once per frame
	void Update () {

    }

    void InitSetting()
    {
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //ショップデータベースの取得
        shop_database = ItemShopDataBase.Instance.GetComponent<ItemShopDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //updown_button = this.GetComponentsInChildren<Button>();
        //updown_button[0].interactable = true;
        //updown_button[1].interactable = true;
    }

    void OnEnable()
    {
        //Debug.Log("Reset Updown Counter");

        InitSetting();
        
        updown_button = this.GetComponentsInChildren<Button>();
        foreach (var obj in updown_button)
        {
            obj.interactable = true;
        }

        updown_button_Big = this.transform.Find("up_big").gameObject;
        updown_button_Big.SetActive(false);
        updown_button_Small = this.transform.Find("up_small").gameObject;
        updown_button_Small.SetActive(false);

        updown_counter_setpanel = this.transform.Find("SetPanel").gameObject;
        updown_counter_setpanel.SetActive(false);
       

        //調合中に開かれた場合は、シーンに限らずプレイヤーアイテムとレシピリストを取得
        if (GameMgr.CompoundSceneStartON)
        {
            pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
            pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

            recipilistController_obj = canvas.transform.Find("RecipiList_ScrollView").gameObject;
            recipilistController = recipilistController_obj.GetComponent<RecipiListController>();

            _p_or_recipi_flag = 0;
            //レシピ調合とそれ以外で、YesNoの取得オブジェクトが違う
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
                updown_counter_setpanel.SetActive(true);
                this.transform.Find("counter_img1").gameObject.SetActive(false);
            }
            else
            {
                this.transform.localPosition = new Vector3(0, -80, 0);
                SettingPos();                
            }
        }
        else
        {
            switch (GameMgr.Scene_Category_Num)
            {
                case 20:

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

                    if (GameMgr.Scene_Select == 1 || GameMgr.Scene_Select == 5)
                    {
                        updown_button_Big.SetActive(true);
                        updown_button_Small.SetActive(true);
                    }
                    else
                    {
                        updown_button_Big.SetActive(false);
                        updown_button_Small.SetActive(false);
                    }

                    if (GameMgr.Scene_Select == 1) //ショップ「買う」の時
                    {
                        ShopUpdownCounter_Pos();
                    }
                    else if (GameMgr.Scene_Select == 3) //依頼の納品の時
                    {
                        ShopUpdownCounter_Pos2();
                    }
                    else if (GameMgr.Scene_Select == 5) //売るの時
                    {
                        ShopUpdownCounter_Pos2();
                    }

                    break;

                case 40:

                    //ショップリスト画面の取得
                    shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                    shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                    updown_button_Big.SetActive(true);
                    updown_button_Small.SetActive(true);

                    ShopUpdownCounter_Pos();

                    break;

                case 50:

                    //ショップリスト画面の取得
                    shopitemlistController_obj = canvas.transform.Find("ShopitemList_ScrollView").gameObject;
                    shopitemlistController = shopitemlistController_obj.GetComponent<ShopItemListController>();

                    updown_button_Big.SetActive(true);
                    updown_button_Small.SetActive(true);

                    ShopUpdownCounter_Pos();

                    break;

                default:                    

                    break;
            }
        }

        GameMgr.updown_kosu = 1;
        _zaiko_max = 0;

        _count_text = transform.Find("counter_num").GetComponent<Text>();
        _count_text.text = GameMgr.updown_kosu.ToString();

    }

    void SettingPos()
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

            case 7: //ヒカリに作らせる場合の、カウンターの位置

                this.transform.localPosition = new Vector3(0, -87, 0);

                if (GameMgr.compound_status == 100) //アイテム選択中
                {
                    switch (GameMgr.Comp_kettei_bunki) //itemselectToggle内で、分岐数字を変えてるので、注意。selectToggleでは、0,1,2になっている。
                    {
                        case 1:

                            if (database.items[pitemlistController.final_kettei_item1].itemType.ToString() == "Okashi")
                            {
                                updown_button[0].interactable = false;
                                updown_button[1].interactable = false;
                            }
                            break;

                        case 2:

                            if (database.items[pitemlistController.final_kettei_item2].itemType.ToString() == "Okashi")
                            {
                                updown_button[0].interactable = false;
                                updown_button[1].interactable = false;
                            }
                            break;

                        case 3:

                            if (database.items[pitemlistController.final_kettei_item3].itemType.ToString() == "Okashi")
                            {
                                updown_button[0].interactable = false;
                                updown_button[1].interactable = false;
                            }
                            break;
                    }
                }
                break;

            case 21: //オリジナル調合の場合の、カウンターの位置

                this.transform.localPosition = new Vector3(0, -87, 0);
                break;

            default:

                this.transform.localPosition = new Vector3(100, -120, 0);
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


    public void OnClick_up()
    {
        //調合シーンでの処理
        if (GameMgr.CompoundSceneStartON)
        {
            if (_p_or_recipi_flag == 0) //プレイヤーアイテムリストのときの処理
            {
                if (GameMgr.compound_status == 110) //最後、何セット作るかを確認中
                {
                    if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7) //オリジナル調合のとき
                    {
                        //カウントをとりあえず１足す
                        ++GameMgr.updown_kosu;

                        switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _item_max1 = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName];
                                break;

                            case 1:

                                _item_max1 = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            case 2:

                                _item_max1 = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _item_max2 = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item2].itemName];
                                break;

                            case 1:

                                _item_max2 = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                break;

                            case 2:

                                _item_max2 = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item2].ItemKosu;
                                break;

                            default:
                                break;
                        }

                        if (pitemlistController.kettei_item3 != 9999) //３個目も選んでいれば、下の処理を起動
                        {
                            switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                            {
                                case 0:

                                    _item_max3 = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item3].itemName];
                                    break;

                                case 1:

                                    _item_max3 = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                    break;

                                case 2:

                                    _item_max3 = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item3].ItemKosu;
                                    break;

                                default:
                                    break;
                            }

                            player_itemkosu3 = pitemlistController.final_kettei_kosu3 * GameMgr.updown_kosu;
                        }

                        player_itemkosu1 = pitemlistController.final_kettei_kosu1 * GameMgr.updown_kosu;
                        player_itemkosu2 = pitemlistController.final_kettei_kosu2 * GameMgr.updown_kosu;

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
                                GameMgr.updown_kosu--;
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
                                GameMgr.updown_kosu--;
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
                    if (GameMgr.compound_select == 2 || GameMgr.compound_select == 3 || GameMgr.compound_select == 7 || GameMgr.compound_select == 21)
                    {
                        switch (GameMgr.Comp_kettei_bunki)
                        {
                            case 1:

                                switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName]; //一個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 2:

                                switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item2].itemName]; //二個目の決定アイテムの所持数

                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item2].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 3:

                                switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item3].itemName]; //三個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item3].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 10:

                                switch (pitemlistController._listitem[pitemlistController._base_count].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.base_kettei_item].itemName]; //ベース決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.base_kettei_item].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.base_kettei_item].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 11:

                                switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName]; //一個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 12:

                                switch (pitemlistController._listitem[pitemlistController._count2].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item2].itemName]; //二個目の決定アイテムの所持数                           
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item2].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item2].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 13:

                                switch (pitemlistController._listitem[pitemlistController._count3].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item3].itemName]; //三個目の決定アイテムの所持数                           
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item3].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item3].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 20:

                                switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                                {
                                    case 0:

                                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName]; //一個目の決定アイテムの所持数
                                        break;

                                    case 1:

                                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    case 2:

                                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            default:
                                break;
                        }
                    }

                    else if (GameMgr.compound_select == 5) //焼くの場合　未使用
                    {
                        switch (pitemlistController._listitem[pitemlistController._count1].GetComponent<itemSelectToggle>().toggleitem_type)
                        {
                            case 0:

                                _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName]; //一個目の決定アイテムの所持数
                                break;

                            case 1:

                                _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                                break;

                            case 2:

                                _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
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

            _count_text.text = GameMgr.updown_kosu.ToString();
        }

        else //調合以外の処理
        {
            if (GameMgr.Scene_Category_Num == 20)
            {
                if (GameMgr.Scene_Select == 1) //ショップ「買う」のとき
                {
                    _zaiko_max = shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

                    AddMethod1();

                    if (PlayerStatus.player_money < shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * GameMgr.updown_kosu)
                    {
                        //お金が足りない
                        _text.text = "お金が足りない。";

                        GameMgr.updown_kosu--;
                    }

                    _count_text.text = GameMgr.updown_kosu.ToString();
                }

                if (GameMgr.Scene_Select == 3) //ショップ納品のとき
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

                            if (pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName] >=
                                quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default - listkosu_count)
                            {
                                _zaiko_max = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default
                                    - listkosu_count; //クエストの必要個数-今まで選択しているアイテムの個数
                            }
                            else
                            {
                                _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName];
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

                        case 2:

                            if (pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu >=
                                quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default - listkosu_count)
                            {
                                _zaiko_max = quest_database.questTakeset[shopquestlistController._count].Quest_kosu_default
                                    - listkosu_count; //クエストの必要個数-今まで選択しているアイテムの個数
                            }
                            else
                            {
                                _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                            }

                            break;

                        default:
                            break;
                    }

                    AddMethod1();
                }

                if (GameMgr.Scene_Select == 5) //ショップ「売る」のとき
                {
                    switch (pitemlistController._toggle_type1)
                    {
                        case 0:

                            _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName];
                            break;

                        case 1:

                            _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                            break;

                        case 2:

                            _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                            break;

                        default:
                            break;
                    }


                    AddMethod1();

                    //ウィンドウの表示も変える。
                    SellYosokuText();
                }
            }
            else if (GameMgr.Scene_Category_Num == 40)
            {

                _zaiko_max = shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

                AddMethod1();

                if (PlayerStatus.player_money < shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_costprice * GameMgr.updown_kosu)
                {
                    //お金が足りない
                    _text.text = "お金が足りない。";

                    GameMgr.updown_kosu--;
                }

                _count_text.text = GameMgr.updown_kosu.ToString();
            }
            else if (GameMgr.Scene_Category_Num == 50)
            {

                _zaiko_max = shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

                AddMethod1();

                //emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
                kaerucoin = pitemlist.playeritemlist["emeralDongri"];

                if (kaerucoin < shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_costprice * GameMgr.updown_kosu)
                {
                    //お金が足りない
                    _text.text = "エメラルどんぐりが足りない。";

                    GameMgr.updown_kosu--;
                }

                _count_text.text = GameMgr.updown_kosu.ToString();
            }
        }
    }

    public void OnClickup_Big()
    {
        if (GameMgr.Scene_Category_Num == 20)
        {
            if (GameMgr.Scene_Select == 1)
            {
                _zaiko_max = shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

                AddMethod2();

                if (PlayerStatus.player_money < shop_database.shopitems[shopitemlistController.shop_kettei_ID].shop_costprice * GameMgr.updown_kosu)
                {
                    //お金が足りない
                    _text.text = "お金が足りない。";

                    GameMgr.updown_kosu = GameMgr.updown_kosu - 10;
                }

                _count_text.text = GameMgr.updown_kosu.ToString();
            }

            if (GameMgr.Scene_Select == 5)
            {
                switch (pitemlistController._toggle_type1)
                {
                    case 0:

                        _zaiko_max = pitemlist.playeritemlist[database.items[pitemlistController.kettei_item1].itemName];
                        break;

                    case 1:

                        _zaiko_max = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].ItemKosu;
                        break;

                    case 2:

                        _zaiko_max = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].ItemKosu;
                        break;

                    default:
                        break;
                }

                AddMethod2();

                //ウィンドウの表示も変える。
                SellYosokuText();
                
            }
        }
        else if (GameMgr.Scene_Category_Num == 40)
        {

            _zaiko_max = shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

            AddMethod2();

            if (PlayerStatus.player_money < shop_database.farmitems[shopitemlistController.shop_kettei_ID].shop_costprice * GameMgr.updown_kosu)
            {
                //お金が足りない
                _text.text = "お金が足りない。";

                GameMgr.updown_kosu = GameMgr.updown_kosu - 10;
            }

            _count_text.text = GameMgr.updown_kosu.ToString();

        }
        else if (GameMgr.Scene_Category_Num == 50)
        {

            _zaiko_max = shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_itemzaiko;

            AddMethod2();

            //emeraldonguriID = pitemlist.SearchItemString("emeralDongri");
            kaerucoin = pitemlist.playeritemlist["emeralDongri"];

            if (kaerucoin < shop_database.emeraldshop_items[shopitemlistController.shop_kettei_ID].shop_costprice * GameMgr.updown_kosu)
            {
                //お金が足りない
                _text.text = "エメラルどんぐりが足りない。";

                GameMgr.updown_kosu = GameMgr.updown_kosu - 10;
            }

            _count_text.text = GameMgr.updown_kosu.ToString();
        }
    }



    public void OnClick_down()
    {
        //調合シーンでの処理
        if (GameMgr.CompoundSceneStartON)
        {
            if (_p_or_recipi_flag == 0) //プレイヤーアイテムリストのときの処理
            {
                DegMethod1();

                if (GameMgr.compound_status == 110) //最後、何セット作るかを確認中
                {
                    player_itemkosu1 = pitemlistController.final_kettei_kosu1 * GameMgr.updown_kosu;
                    player_itemkosu2 = pitemlistController.final_kettei_kosu2 * GameMgr.updown_kosu;

                    if (pitemlistController.kettei_item3 != 9999) //３個目も選んでいれば、下の処理を起動
                    {
                        player_itemkosu3 = pitemlistController.final_kettei_kosu3 * GameMgr.updown_kosu;
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
        else
        //調合以外での処理
        {
            if (GameMgr.Scene_Category_Num == 20)
            {
                if (GameMgr.Scene_Select == 1) //買い物のとき
                {
                    DegMethod1();

                    //ウィンドウの表示も変える。
                    BuyYosokuText();
                }
                else if (GameMgr.Scene_Select == 5) //売るとき
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
            else if (GameMgr.Scene_Category_Num == 50)
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
    }

    public void OnClickdown_Small()
    {
        if (GameMgr.Scene_Category_Num == 20)
        {
            if (GameMgr.Scene_Select == 1) //買い物のとき
            {
                DegMethod2();

                //ウィンドウの表示も変える。
                BuyYosokuText();
            }
            else if (GameMgr.Scene_Select == 5) //売るとき
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
        else if (GameMgr.Scene_Category_Num == 50)
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
        ++GameMgr.updown_kosu;
        if (GameMgr.updown_kosu > _zaiko_max)
        {
            GameMgr.updown_kosu = _zaiko_max;
        }

        _count_text.text = GameMgr.updown_kosu.ToString();
    }

    void AddMethod2()
    {
        GameMgr.updown_kosu = GameMgr.updown_kosu + 10;
        if (GameMgr.updown_kosu > _zaiko_max)
        {
            GameMgr.updown_kosu = _zaiko_max;
        }

        _count_text.text = GameMgr.updown_kosu.ToString();
    }

    void DegMethod1()
    {
        --GameMgr.updown_kosu;
        if (GameMgr.updown_kosu <= 1)
        {
            GameMgr.updown_kosu = 1;
        }

        _count_text.text = GameMgr.updown_kosu.ToString();
    }

    void DegMethod2()
    {
        GameMgr.updown_kosu = GameMgr.updown_kosu - 10;
        if (GameMgr.updown_kosu <= 1)
        {
            GameMgr.updown_kosu = 1;
        }

        _count_text.text = GameMgr.updown_kosu.ToString();
    }


    //レシピリストのときの処理
    public void updown_keisan_Method()
    {
        //windowテキストエリアの取得 調合シーンは、専用のウィンドウを指定する
        text_area = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/MessageWindowComp").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        count = recipilistController._count1;
        itemID_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_toggleCompoitem_ID; //itemID_1という変数に、プレイヤーが選択した調合DBの配列番号を格納する。
        itemname_1 = recipilistController._recipi_listitem[count].GetComponent<recipiitemSelectToggle>().recipi_itemNameHyouji;

        recipilistController.final_select_kosu = GameMgr.updown_kosu; //選択個数(セットの回数）

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
            cmpitem_kosu1_select = cmpitem_kosu1 * GameMgr.updown_kosu; //必要個数×選択している作成数
        }
        else
        {
            cmpitem_kosu1_select = cmpitem_kosu1; //機材アイテムなどは、個数が反映されない。基本は1
        }

        if (cmpitem2_type == 0)
        {
            cmpitem_kosu2_select = cmpitem_kosu2 * GameMgr.updown_kosu; //必要個数×選択している作成数
        }
        else
        {
            cmpitem_kosu2_select = cmpitem_kosu2; //機材アイテムなどは、個数が反映されない。基本は1
        }

        if (cmpitem3_type == 0)
        {
            cmpitem_kosu3_select = cmpitem_kosu3 * GameMgr.updown_kosu; //必要個数×選択している作成数
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
        for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
        {
            if (cmpitem_name1 == pitemlist.player_extremepanel_itemlist[i].itemName)
            {
                player_itemkosu1 += pitemlist.player_extremepanel_itemlist[i].ItemKosu;
            }
        }
        //店売りの所持個数を計算
        player_itemkosu1 += pitemlist.playeritemlist[database.items[itemdb_id1].itemName];

        //二個目
        //オリジナルの所持個数を計算
        for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
        {
            if (cmpitem_name2 == pitemlist.player_originalitemlist[i].itemName)
            {
                player_itemkosu2 += pitemlist.player_originalitemlist[i].ItemKosu;
            }
        }
        for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
        {
            if (cmpitem_name2 == pitemlist.player_extremepanel_itemlist[i].itemName)
            {
                player_itemkosu2 += pitemlist.player_extremepanel_itemlist[i].ItemKosu;
            }
        }
        //店売りの所持個数を計算
        player_itemkosu2 += pitemlist.playeritemlist[database.items[itemdb_id2].itemName];

        //三個目
        //オリジナルの所持個数を計算
        if (itemdb_id3 != 9999)
        {
            for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
            {
                if (cmpitem_name3 == pitemlist.player_originalitemlist[i].itemName)
                {
                    player_itemkosu3 += pitemlist.player_originalitemlist[i].ItemKosu;
                }
            }
            for (i = 0; i < pitemlist.player_extremepanel_itemlist.Count; i++)
            {
                if (cmpitem_name3 == pitemlist.player_extremepanel_itemlist[i].itemName)
                {
                    player_itemkosu3 += pitemlist.player_extremepanel_itemlist[i].ItemKosu;
                }
            }
            //店売りの所持個数を計算
            player_itemkosu3 += pitemlist.playeritemlist[database.items[itemdb_id3].itemName];
        }




        //テキストの更新　左：現在の所持数　右：必要個数
        _a = cmpitem_namehyouji1 + ": " + GameMgr.ColorYellow + player_itemkosu1 + "</color>" + "／" + cmpitem_kosu1_select;
        _b = cmpitem_namehyouji2 + ": " + GameMgr.ColorYellow + player_itemkosu2 + "</color>" + "／" + cmpitem_kosu2_select;
        if (itemdb_id3 != 9999)
        {
            _c = cmpitem_namehyouji3 + ": " + GameMgr.ColorYellow + player_itemkosu3 + "</color>" + "／" + cmpitem_kosu3_select;
        }
        else
        {
            _c = "";
        }

        //材料個数が足りてるかの判定し、足りてないときは赤字にテキスト更新
        if (cmpitem_kosu1_select > player_itemkosu1)
        {
            _a = GameMgr.ColorRed + cmpitem_namehyouji1 + ": " + player_itemkosu1 + "／" + cmpitem_kosu1_select + "</color>";
        }
        if (cmpitem_kosu2_select > player_itemkosu2)
        {
            _b = GameMgr.ColorRed + cmpitem_namehyouji2 + ": " + player_itemkosu2 + "／" + cmpitem_kosu2_select + "</color>";
        }
        if (itemdb_id3 != 9999)
        {
            if (cmpitem_kosu3_select > player_itemkosu3)
            {
                _c = GameMgr.ColorRed + cmpitem_namehyouji3 + ": " + player_itemkosu3 + "／" + cmpitem_kosu3_select + "</color>";
            }
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
        switch (pitemlistController._toggle_type1)
        {
            case 0:

                _text.text = database.items[pitemlistController.kettei_item1].itemNameHyouji + "が選択されました。　" +
            GameMgr.ColorYellow + database.items[pitemlistController.kettei_item1].sell_price * GameMgr.updown_kosu + " " + GameMgr.MoneyCurrency + "</color>"
            + "\n" + "個数を選択してください";
                break;

            case 1:

                _text.text = pitemlist.player_originalitemlist[pitemlistController.kettei_item1].itemNameHyouji + "が選択されました。　" +
            GameMgr.ColorYellow + pitemlist.player_originalitemlist[pitemlistController.kettei_item1].sell_price * GameMgr.updown_kosu + " " + GameMgr.MoneyCurrency + "</color>"
            + "\n" + "個数を選択してください";
                break;

            case 2:

                _text.text = pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].itemNameHyouji + "が選択されました。　" +
            GameMgr.ColorYellow + pitemlist.player_extremepanel_itemlist[pitemlistController.kettei_item1].sell_price * GameMgr.updown_kosu + " " + GameMgr.MoneyCurrency + "</color>"
            + "\n" + "個数を選択してください";
                break;

            default:
                break;
        }
        
    }

    public void UpdownButton_InteractALLON()
    {
        foreach (var obj in updown_button)
        {
            obj.interactable = true;
        }
    }

    public void UpdownButton_InteractALLOFF()
    {
        foreach (var obj in updown_button)
        {
            obj.interactable = false;
        }
    }
}

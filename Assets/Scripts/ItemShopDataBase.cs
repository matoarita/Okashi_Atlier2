using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopDataBase : SingletonMonoBehaviour<ItemShopDataBase>
{

    private ItemDataBase database;

    private Entity_shopItemDataBase excel_shopitemdatabase;

    private PlayerItemList pitemlist;

    private int _id;
    private Sprite _icon;
    private string _name;
    private string _name_hyouji;

    private int _itemID;
    private int _cost;
    private int _sell;
    private int _zaiko;
    private int _itemType;
    private int _itemhyouji;
    private bool _itemhyouji_on;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<ItemShop> shopitems = new List<ItemShop>();
    public List<ItemShop> farmitems = new List<ItemShop>();
    public List<ItemShop> emeraldshop_items = new List<ItemShop>();

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        excel_shopitemdatabase = Resources.Load("Excel/Entity_shopItemDataBase") as Entity_shopItemDataBase;


        //ショップのデータの読み込み
        sheet_no = 0;
        sheet_count = 0;

        while (sheet_count < 1)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {

                InitShopDB_Common();

                //ここでリストに追加している
                shopitems.Add(new ItemShop(count, _itemID, _icon, _name, _name_hyouji, _cost, _sell, _zaiko, _itemType, _itemhyouji, _itemhyouji_on));

                ++count;
            }
            ++sheet_count;
        }

        //ファームのデータの読み込み
        sheet_no = 1;
        sheet_count = 0;

        while (sheet_count < 1)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {

                InitShopDB_Common();

                //ここでリストに追加している
                farmitems.Add(new ItemShop(count, _itemID, _icon, _name, _name_hyouji, _cost, _sell, _zaiko, _itemType, _itemhyouji, _itemhyouji_on));

                ++count;
            }
            ++sheet_count;
        }

        //エメラルドショップデータの読み込み
        sheet_no = 2;
        sheet_count = 0;

        while (sheet_count < 1)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {

                InitShopDB_Common();

                //ここでリストに追加している
                emeraldshop_items.Add(new ItemShop(count, _itemID, _icon, _name, _name_hyouji, _cost, _sell, _zaiko, _itemType, _itemhyouji, _itemhyouji_on));

                ++count;
            }
            ++sheet_count;
        }


        /*for (i = 0; i < shopitems.Count; i++)
        {
            Debug.Log("ショップID: " + shopitems[i].shop_ID + " アイテムID: " + shopitems[i].shop_itemID + " アイテム名: " + shopitems[i].shop_itemNameHyouji);
        }*/

    }
	
    void InitShopDB_Common()
    {
        _name = excel_shopitemdatabase.sheets[sheet_no].list[count].name;
        _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;
        _itemType = excel_shopitemdatabase.sheets[sheet_no].list[count].itemType;
        _cost = excel_shopitemdatabase.sheets[sheet_no].list[count].shop_sell_price;
        _sell = excel_shopitemdatabase.sheets[sheet_no].list[count].shop_buy_price;
        _itemhyouji = excel_shopitemdatabase.sheets[sheet_no].list[count].item_hyouji;
        _itemhyouji_on = excel_shopitemdatabase.sheets[sheet_no].list[count].item_hyouji_on;

        //Debug.Log("ショップ_itemType: " + _itemType);


        if (_itemType == 1) //レシピ
        {

            i = 0;
            //Debug.Log("イベントアイテムを検出");

            //Debug.Log("イベントアイテムリストカウント: " + pitemlist.eventitemlist.Count);

            while (i < pitemlist.eventitemlist.Count)
            {
                if (pitemlist.eventitemlist[i].event_itemName == _name)
                {
                    //Debug.Log("ショップアイテム名: " + _name);

                    _itemID = pitemlist.eventitemlist[i].ev_ItemID;
                    //Debug.Log("イベントアイテムID: " + _itemID);
                    _icon = Resources.Load<Sprite>("Sprites/" + pitemlist.eventitemlist[i].event_fileName);
                    _name_hyouji = pitemlist.eventitemlist[i].event_itemNameHyouji;

                    break;
                }

                i++;
            }
        }
        else if (_itemType == 5) //エメラルショップのアイテム
        {
            i = 0;
            //Debug.Log("エメラルドショップアイテムを検出");

            //Debug.Log("エメラルドショップアイテムリストカウント: " + pitemlist.eventitemlist.Count);

            while (i < pitemlist.emeralditemlist.Count)
            {
                if (pitemlist.emeralditemlist[i].event_itemName == _name)
                {
                    //Debug.Log("ショップアイテム名: " + _name);

                    _itemID = pitemlist.emeralditemlist[i].ev_ItemID;
                    //Debug.Log("イベントアイテムID: " + _itemID);
                    _icon = Resources.Load<Sprite>("Sprites/" + pitemlist.emeralditemlist[i].event_fileName);
                    _name_hyouji = pitemlist.emeralditemlist[i].event_itemNameHyouji;

                    break;
                }

                i++;
            }
        }
        else
        {
            i = 0;

            while (i < database.items.Count)
            {

                if (database.items[i].itemName == _name)
                {
                    _itemID = database.items[i].itemID;
                    _icon = database.items[i].itemIcon_sprite;
                    _name_hyouji = database.items[i].itemNameHyouji;

                    break;
                }

                ++i;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void Setup_shopdatabase()
    {
        
    }

    //ショップの在庫をリセットし、初期状態に戻す。
    public void ShopZaiko_Reset()
    {
        //ショップのデータの読み込み
        sheet_no = 0;
        sheet_count = 0;

        while (sheet_count < 1)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {
                _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;

                //ここでリストに追加している
                shopitems[count].shop_itemzaiko = _zaiko;

                ++count;
            }
            ++sheet_count;
        }

        //ファームのデータの読み込み
        sheet_no = 1;
        sheet_count = 0;

        while (sheet_count < 1)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {

                _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;

                //ここでリストに追加している
                farmitems[count].shop_itemzaiko = _zaiko;

                ++count;
            }
            ++sheet_count;
        }

        //エメラルドショップデータの読み込み
        sheet_no = 2;
        sheet_count = 0;

        while (sheet_count < 1)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {

                _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;

                //ここでリストに追加している
                emeraldshop_items[count].shop_itemzaiko = _zaiko;

                ++count;
            }
            ++sheet_count;
        }
    }
}

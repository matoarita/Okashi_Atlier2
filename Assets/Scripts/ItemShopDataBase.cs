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

    private int _shopID;
    private int _itemID;
    private int _cost;
    private int _sell;
    private int _zaiko;
    private int _itemType;
    private int _dongriType;
    private int _itemhyouji;
    private bool _itemhyouji_on;
    private int _read_endflag;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<ItemShop> shopitems = new List<ItemShop>();
    //public List<ItemShop> farmitems = new List<ItemShop>();
    //public List<ItemShop> emeraldshop_items = new List<ItemShop>();

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        excel_shopitemdatabase = Resources.Load("Excel/Entity_shopItemDataBase") as Entity_shopItemDataBase;

        //ショップデータ全て　読み込み
        //Debug.Log("ショップシートカウント: " + excel_shopitemdatabase.sheets.Count);
        sheet_no = 0;
        while (sheet_no < excel_shopitemdatabase.sheets.Count)
        {
            count = 0;
            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {
                InitShopDB_Common();

                //ここでリストに追加している
                shopitems.Add(new ItemShop(_shopID, _itemID, _icon, _name, _name_hyouji, _cost, _sell, _zaiko, _itemType, _dongriType, _itemhyouji, _itemhyouji_on, _read_endflag));

                ++count;
            }
            ++sheet_no;
        }

        //デバッグ用
        /*for (i = 0; i < shopitems.Count; i++)
        {
            Debug.Log("ショップID: " + shopitems[i].shop_ID + " アイテムID: " + shopitems[i].shop_itemID + " アイテム名: " + shopitems[i].shop_itemNameHyouji);
        }*/

    }

    int SearchSheetsNameID(string _name)
    {
        i = 0;
        while (sheet_no < excel_shopitemdatabase.sheets.Count)
        {
            if (excel_shopitemdatabase.sheets[i].name == _name)
            {

                return i;
            }
            ++i;
        }

        return 0; //例外でシート名がなかったときは0がかえる
    }
	
    void InitShopDB_Common()
    {
        _shopID = excel_shopitemdatabase.sheets[sheet_no].list[count].ShopID;
        _name = excel_shopitemdatabase.sheets[sheet_no].list[count].name;
        _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;
        _itemType = excel_shopitemdatabase.sheets[sheet_no].list[count].itemType;
        _dongriType = excel_shopitemdatabase.sheets[sheet_no].list[count].dongriType;
        _cost = excel_shopitemdatabase.sheets[sheet_no].list[count].shop_sell_price;
        _sell = excel_shopitemdatabase.sheets[sheet_no].list[count].shop_buy_price;
        _itemhyouji = excel_shopitemdatabase.sheets[sheet_no].list[count].item_hyouji;
        _itemhyouji_on = excel_shopitemdatabase.sheets[sheet_no].list[count].item_hyouji_on;
        _read_endflag = excel_shopitemdatabase.sheets[sheet_no].list[count].read_endflag;

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
                    //Debug.Log("エメラルドアイテムID: " + _itemID);
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
        i = 0;
        sheet_no = 0;
        while (sheet_no < excel_shopitemdatabase.sheets.Count)
        {
            count = 0;
            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {
                _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;

                //ここでリストに追加している
                shopitems[i].shop_itemzaiko = _zaiko;

                ++count;
                ++i;
            }
            ++sheet_no;
        }
    }

    //アイテム名＋個数で、指定した在庫数に変更する。
    public void ReSetShopItemString(string itemName, int count_kosu)
    {
        i = 0;
        while (i < shopitems.Count)
        {
            if (shopitems[i].shop_itemName == itemName)
            {
                shopitems[i].shop_itemzaiko = count_kosu;
                break;
            }
            i++;
        }
    }

    //ショップID＋個数で、指定した在庫数に変更する。
    public void ReSetShopItemIDZaiko(int _shopID, int count_kosu)
    {
        i = 0;
        while (i < shopitems.Count)
        {
            if (shopitems[i].shop_ID == _shopID)
            {
                shopitems[i].shop_itemzaiko = count_kosu;
                break;
            }
            i++;
        }
    }

    //ショップIDを入れると、DBのリスト番号を返す
    public int SeartchShopID(int _shopID)
    {
        i = 0;
        while (i < shopitems.Count)
        {
            if (shopitems[i].shop_ID == _shopID)
            {
                return i;
            }
            i++;
        }

        return 0;
    }
}

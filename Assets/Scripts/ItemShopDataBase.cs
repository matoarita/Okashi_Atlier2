﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopDataBase : SingletonMonoBehaviour<ItemShopDataBase>
{

    private ItemDataBase database;

    private Entity_shopItemDataBase excel_shopitemdatabase;

    private PlayerItemList pitemlist;

    private int _id;
    private Texture2D _icon;
    private string _name;
    private string _name_hyouji;

    private int _itemID;
    private int _cost;
    private int _sell;
    private int _zaiko;
    private int _itemType;
    private int _itemhyouji;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<ItemShop> shopitems = new List<ItemShop>();

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        excel_shopitemdatabase = Resources.Load("Excel/Entity_shopItemDataBase") as Entity_shopItemDataBase;


        sheet_no = 0;

        while (sheet_no < excel_shopitemdatabase.sheets.Count)
        {
            count = 0;

            while (count < excel_shopitemdatabase.sheets[sheet_no].list.Count)
            {

                _name = excel_shopitemdatabase.sheets[sheet_no].list[count].name;
                _zaiko = excel_shopitemdatabase.sheets[sheet_no].list[count].zaiko;
                _itemType = excel_shopitemdatabase.sheets[sheet_no].list[count].itemType;
                _cost = excel_shopitemdatabase.sheets[sheet_no].list[count].shop_sell_price;
                _sell = excel_shopitemdatabase.sheets[sheet_no].list[count].shop_buy_price;
                _itemhyouji = excel_shopitemdatabase.sheets[sheet_no].list[count].item_hyouji;

                //Debug.Log("ショップ_itemType: " + _itemType);

                if (_itemType != 1)
                {
                    i = 0;

                    while (i < database.items.Count)
                    {

                        if (database.items[i].itemName == _name)
                        {
                            _itemID = database.items[i].itemID;
                            _icon = database.items[i].itemIcon;
                            _name_hyouji = database.items[i].itemNameHyouji;                            

                            break;
                        }

                        ++i;
                    }
                }
                else if (_itemType == 1)
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
                            _icon = Resources.Load<Texture2D>("Sprites/Items/" + "recipibook");
                            _name_hyouji = pitemlist.eventitemlist[i].event_itemNameHyouji;

                            break;
                        }

                        i++;
                    }
                }

                //ここでリストに追加している
                shopitems.Add(new ItemShop(count, _itemID, _icon, _name, _name_hyouji, _cost, _sell, _zaiko, _itemType, _itemhyouji));

                ++count;
            }
            ++sheet_no;
        }


        /*for (i = 0; i < shopitems.Count; i++)
        {
            Debug.Log("ショップID: " + shopitems[i].shop_ID + " アイテムID: " + shopitems[i].shop_itemID + " アイテム名: " + shopitems[i].shop_itemNameHyouji);
        }*/

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup_shopdatabase()
    {
        
    }
}

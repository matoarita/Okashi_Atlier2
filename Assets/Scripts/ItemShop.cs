﻿using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemShop
{
    public int shop_ID;
    public int shop_itemID;
    public string shop_itemName;        //名前、画像ファイル名
    public string shop_itemNameHyouji;  //名前　ゲーム中での表示用。日本語。
    public Sprite shop_itemIcon;     //アイコン
    public int shop_itemType;

    public int shop_costprice; //アイテムの値段。アイテムDBから引っ張ってくる。
    public int shop_sellprice; //アイテムの売却時の値段。アイテムDBから引っ張ってくる。
    public int shop_itemzaiko; //在庫数

    public int shop_item_hyouji;


    //ここでリスト化時に渡す引数をあてがいます   
    public ItemShop(int id, int _itemID, Sprite _icon, string name, string nameHyouji, int cost, int sell, int zaiko, int _itemType, int item_hyouji)
    {
        shop_ID = id;
        shop_itemID = _itemID;

        shop_itemName = name;
        shop_itemNameHyouji = nameHyouji;

        //アイコンはnameとイコールにするのでアイコンがあるパス＋nameで取ってきます    
        shop_itemIcon = _icon;

        shop_itemType = _itemType;

        shop_costprice = cost;
        shop_sellprice = sell;
        shop_itemzaiko = zaiko;

        shop_item_hyouji = item_hyouji;
    }

}
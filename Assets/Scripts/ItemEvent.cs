using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemEvent
{
    public int ev_ItemID;
    public string event_fileName;
    public Sprite itemIcon_sprite;      //アイコン
    public string event_itemName;
    public string event_itemNameHyouji;
    public string event_itemDesc;         //アイテムの説明文
    public int event_cost_price;
    public int event_sell_price;
    public int ev_itemKosu;
    public int ev_itemType;
    public int ev_ReadFlag;
    public int ev_costumeNum;
    public int ev_costumeEquip; //アクセサリー用　どのアクセを装備してるか否か　重複が可能
    public int ev_magicbook; //魔法の本かどうか
    public int ev_ListOn;

    public string ev_memo;
    public int ev_Re_flag_num;
    public int ev_Ev_flag_num;



    //ここでリスト化時に渡す引数をあてがいます   
    public ItemEvent(int id, string file_Name, string ev_item, string ev_itemNameHyouji, string desc, int _cost, int _sell, int kosu, int flag, int itemType, 
        int costumeNum, int costumeEquip, int magic_book, int list_on, string _memo, int reflag_num, int evflag_num)
    {
        ev_ItemID = id;
        event_fileName = file_Name;
        itemIcon_sprite = Resources.Load<Sprite>("Sprites/" + file_Name);
        event_itemName = ev_item;
        event_itemNameHyouji = ev_itemNameHyouji;
        event_itemDesc = desc;

        event_cost_price = _cost;
        event_sell_price = _sell;
        ev_itemKosu = kosu;
        ev_itemType = itemType;
        ev_ReadFlag = flag;
        ev_costumeNum = costumeNum;
        ev_costumeEquip = costumeEquip;
        ev_magicbook = magic_book;
        ev_ListOn = list_on;

        ev_memo = _memo;
        ev_Re_flag_num = reflag_num;
        ev_Ev_flag_num = evflag_num;
    }

}
using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class QuestSet
{
    public int _ID;
    public int Quest_ID;    //DB固有の番号。IDは、リストの順番を入れ替えると、順番が崩れてしまうので、各リストに固有のIDとしてつけている。

    public Sprite questIcon;      //アイコン
    public string Quest_FileName;
    public string Quest_itemName;
    public string Quest_itemSubtype;

    public int Quest_kosu_default;
    public int Quest_kosu_min;
    public int Quest_kosu_max;
    public int Quest_buy_price;

    public int Quest_rich;
    public int Quest_sweat;
    public int Quest_bitter;
    public int Quest_sour;
    public int Quest_crispy;
    public int Quest_fluffy;
    public int Quest_smooth;
    public int Quest_hardness;
    public int Quest_jiggly;
    public int Quest_chewy;

    public string[] Quest_topping = new string[5];

    public string Quest_Title;
    public string Quest_desc;


    //ここでリスト化時に渡す引数をあてがいます   
    public QuestSet(int id, int _questID, string fileName, string _itemname, string _itemsubtype, int _kosu_default, int _kosu_min, int _kosu_max, int _buy_price, int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, string tp01, string tp02, string tp03, string tp04, string tp05, string _title, string _setkansou)
    {
        _ID = id;
        Quest_ID = _questID;

        Quest_FileName = fileName;
        questIcon = Resources.Load<Sprite>("Sprites/Items/" + Quest_FileName);
        Quest_itemName = _itemname;
        Quest_itemSubtype = _itemsubtype;

        Quest_kosu_default = _kosu_default;
        Quest_kosu_min = _kosu_min;
        Quest_kosu_max = _kosu_max;
        Quest_buy_price = _buy_price;

        Quest_rich = _rich;
        Quest_sweat = _sweat;
        Quest_bitter = _bitter;
        Quest_sour = _sour;

        Quest_crispy = _crispy;
        Quest_fluffy = _fluffy;
        Quest_smooth = _smooth;
        Quest_hardness = _hardness;
        Quest_jiggly = _jiggly;
        Quest_chewy = _chewy;

        Quest_topping[0] = tp01;
        Quest_topping[1] = tp02;
        Quest_topping[2] = tp03;
        Quest_topping[3] = tp04;
        Quest_topping[4] = tp05;

        Quest_Title = _title;
        Quest_desc = _setkansou;
    }

}
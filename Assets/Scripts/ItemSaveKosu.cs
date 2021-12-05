using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemSaveKosu
{
    public string itemName;
    public int itemKosu;
    public int read_Flag; //イベントアイテム用の項目
    //ここまで


    //ここでリスト化時に渡す引数をあてがいます   
    public ItemSaveKosu(string _name, int _kosu, int _readflag)
    {
        itemName = _name;
        itemKosu = _kosu;
        read_Flag = _readflag;
    }
}
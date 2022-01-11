using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemSaveFlag
{
    public string itemName;
    public int Param;
    public bool Flag; //イベントアイテム用の項目
    //ここまで


    //ここでリスト化時に渡す引数をあてがいます   
    public ItemSaveFlag(string _name, int _param, bool _flag)
    {
        itemName = _name;
        Param = _param;
        Flag = _flag;
    }
}
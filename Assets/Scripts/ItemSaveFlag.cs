using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemSaveFlag
{
    public string itemName;
    public int Param;
    public int Param2;
    public int Param3;
    public bool Flag; //イベントアイテム用の項目
    //ここまで


    //ここでリスト化時に渡す引数をあてがいます   名前＋パラメータ３つとフラグ一個をセーブできる
    public ItemSaveFlag(string _name, int _param, int _param2, int _param3, bool _flag)
    {
        itemName = _name;
        Param = _param;
        Param2 = _param2;
        Param3 = _param3;
        Flag = _flag;
    }
}
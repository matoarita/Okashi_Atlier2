using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemSaveCompoFlag
{
    public string comp_name;
    public int comp_Flag;
    public int comp_Count;
    //ここまで


    //ここでリスト化時に渡す引数をあてがいます   
    public ItemSaveCompoFlag(string _cmpname, int _cmpflag, int _cmpcount)
    {
        comp_name = _cmpname;
        comp_Flag = _cmpflag;
        comp_Count = _cmpcount;

    }
}
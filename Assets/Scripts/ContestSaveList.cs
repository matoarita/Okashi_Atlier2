using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ContestSaveList
{
    public string contestName;
    public int Month;
    public int Day;
    public int Param1;
    public int Flag; //イベントアイテム用の項目
    public int FightsCount; //出場回数
    public int Victory; //そのコンテストの過去のランキング
    //ここまで


    //ここでリスト化時に渡す引数をあてがいます   
    public ContestSaveList(string _name, int _month, int _day, int _param1, int _flag, int _fcount, int _victory)
    {
        contestName = _name;
        Month = _month;
        Day = _day;
        Param1 = _param1;
        Flag = _flag;
        FightsCount = _fcount;
        Victory = _victory;
    }
}
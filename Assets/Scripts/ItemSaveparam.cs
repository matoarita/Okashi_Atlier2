using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemSaveparam
{
    public int itemID;
    public string itemName;

    //以下パラメータはExcel上には記載なし
    public int Eat_kaisu;
    public int HighScore_flag;
    public int last_total_score;
    public int last_rich_score;
    public int last_sweat_score;
    public int last_bitter_score;
    public int last_sour_score;
    public int last_crispy_score;
    public int last_fluffy_score;
    public int last_smooth_score;
    public int last_hardness_score;
    public int last_jiggly_score;
    public int last_chewy_score;
    public string last_hinttext;
    //ここまで
    

    //ここでリスト化時に渡す引数をあてがいます   
    public ItemSaveparam(int id, string itemname, int _eat_kaisu, int _highscore, int _lasttotal_score, 
        int _last_rich_score, int _last_sweat_score, int _last_bitter_score, int _last_sour_score,
        int _last_crispy_score, int _last_fluffy_score, int _last_smooth_score, int _last_hardness_score,
        int _last_jiggly_score, int _last_chewy_score, string _hinttext)
    {
        itemID = id;
        itemName = itemname;
        
        Eat_kaisu = _eat_kaisu;
        HighScore_flag = _highscore;

        last_total_score = _lasttotal_score;
        last_rich_score = _last_rich_score;
        last_sweat_score = _last_sweat_score;
        last_bitter_score = _last_bitter_score;
        last_sour_score = _last_sour_score;
        last_crispy_score = _last_crispy_score;
        last_fluffy_score = _last_fluffy_score;
        last_smooth_score = _last_smooth_score;
        last_hardness_score = _last_hardness_score;
        last_jiggly_score = _last_jiggly_score;
        last_chewy_score = _last_chewy_score;
        last_hinttext = _hinttext;
    }

}
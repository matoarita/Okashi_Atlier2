using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class GirlLikeCompo
{
    public int ID;
    public int set_ID;    //DB固有の番号。
    
    public int set1;
    public int set2;
    public int set3;

    public string desc;
    public string comment; //ゲーム中では使用しない。メモ用。

    public int set_flag; //これがONだと、腹減りセットが選択されるようになる。

    public int set_score; //ゲーム中、そのセットを何回作ったか。スコア

    public string hint_text; //吹き出しをおしたときに表示されるヒント。


    //ここでリスト化時に渡す引数をあてがいます   
    public GirlLikeCompo(int id, int _set_id, int set1_id, int set2_id, int set3_id, string _desc, string _comment, int _set_flag, int _set_score, string _hint)
    {
        ID = id;
        set_ID = _set_id;

        set1 = set1_id;
        set2 = set2_id;
        set3 = set3_id;

        desc = _desc;
        comment = _comment;

        set_flag = _set_flag;

        set_score = _set_score;

        hint_text = _hint;
    }

}
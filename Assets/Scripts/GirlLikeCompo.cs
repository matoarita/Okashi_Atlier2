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

    public string spquest_name1;
    public string spquest_name2;
    public string spquest_name3;

    public string desc;
    public string comment; //ゲーム中では使用しない。メモ用。

    public int set_flag; //これがONだと、腹減りセットが選択されるようになる。

    public int set_score; //ゲーム中、そのセットを何回作ったか。スコア

    public string hint_text; //吹き出しをおしたときに表示されるヒント。
    public bool clearFlag; //そのクエストをクリアしたかどうかのフラグ。

    public string fileName;
    public Sprite itemIcon_sprite;


    //ここでリスト化時に渡す引数をあてがいます   
    public GirlLikeCompo(int id, int _set_id, int set1_id, int set2_id, int set3_id, string _quest_name1, string _quest_name2, string _quest_name3, string _desc, string _comment, int _set_flag, int _set_score, string _hint, bool clear_flag, string _fileName)
    {
        ID = id;
        set_ID = _set_id;

        set1 = set1_id;
        set2 = set2_id;
        set3 = set3_id;

        spquest_name1 = _quest_name1;
        spquest_name2 = _quest_name2;
        spquest_name3 = _quest_name3;

        desc = _desc;
        comment = _comment;

        set_flag = _set_flag;
        set_score = _set_score;

        hint_text = _hint;
        clearFlag = clear_flag;

        fileName = _fileName;
        itemIcon_sprite = Resources.Load<Sprite>("Sprites/Items/" + fileName);
    }

}
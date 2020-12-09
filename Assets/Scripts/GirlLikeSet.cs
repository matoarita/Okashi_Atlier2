using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class GirlLikeSet
{
    public int girlLike_ID;
    public int girlLike_compNum;    //DB固有の番号。IDは、リストの順番を入れ替えると、順番が崩れてしまうので、各リストに固有のIDとしてつけている。

    public string girlLike_itemName;
    public string girlLike_itemSubtype;

    public int girlLike_set_score; //そのセットをクリアしたときの好感度上昇値

    public int girlLike_rich;
    public int girlLike_sweat;
    public int girlLike_bitter;
    public int girlLike_sour;
    public int girlLike_crispy;
    public int girlLike_fluffy;
    public int girlLike_smooth;
    public int girlLike_hardness;
    public int girlLike_jiggly;
    public int girlLike_chewy;
    
    public string[] girlLike_topping = new string[5];   //特定のトッピングに応じて、加算される。
    public int[] girlLike_topping_score = new int[5];
    public int girlLike_Non_topping_score; //トッピングが何もない、もしくは乗っててほしいトッピングがない時に、加算される値。大抵、おおきくマイナスになる。

    public int girlLike_comment_flag;

    public string set_kansou;


    //ここでリスト化時に渡す引数をあてがいます   
    public GirlLikeSet(int id, int _compnum, string _itemname, string _itemsubtype, int _set_score, int _rich, int _sweat, int _bitter, int _sour, 
        int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, 
        string tp01, string tp02, string tp03, string tp04, string tp05, int tp_score01, int tp_score02, int tp_score03, int tp_score04, int tp_score05, 
        int non_tp_score, string _setkansou, int _comment_flag)
    {
        girlLike_ID = id;
        girlLike_compNum = _compnum;

        girlLike_itemName = _itemname;
        girlLike_itemSubtype = _itemsubtype;

        girlLike_set_score = _set_score;

        girlLike_rich = _rich;
        girlLike_sweat = _sweat;
        girlLike_bitter = _bitter;
        girlLike_sour = _sour;

        girlLike_crispy = _crispy;
        girlLike_fluffy = _fluffy;
        girlLike_smooth = _smooth;
        girlLike_hardness = _hardness;
        girlLike_jiggly = _jiggly;
        girlLike_chewy = _chewy;

        girlLike_topping[0] = tp01;
        girlLike_topping[1] = tp02;
        girlLike_topping[2] = tp03;
        girlLike_topping[3] = tp04;
        girlLike_topping[4] = tp05;

        girlLike_topping_score[0] = tp_score01;
        girlLike_topping_score[1] = tp_score02;
        girlLike_topping_score[2] = tp_score03;
        girlLike_topping_score[3] = tp_score04;
        girlLike_topping_score[4] = tp_score05;
        girlLike_Non_topping_score = non_tp_score;

        set_kansou = _setkansou;

        girlLike_comment_flag = _comment_flag;
    }

}
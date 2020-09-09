using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemAdd
{
    public string _Addname;
    public int _Addhp;
    public int _Addday;
    public int _Addquality;
    public int _Addexp;
    public int _Addrich;
    public int _Addsweat;
    public int _Addbitter;
    public int _Addsour;
    public int _Addcrispy;
    public int _Addfluffy;
    public int _Addsmooth;
    public int _Addhardness;
    public int _Addjiggly;
    public int _Addchewy;
    public int _Addpowdery;
    public int _Addoily;
    public int _Addwatery;
    public float _Addgirl1_like;
    public int _Addcost;
    public int _Addsell;
    
    public string _Add_itemType;
    public string _Add_itemType_sub;

    //トッピングスロット
    public string[] _Addtp = new string[13];

    //トッピングスロット
    public string[] _Addkoyu_toppingtype = new string[3];

    public int _Addkosu; //選んだ個数


    //ここでリスト化時に渡す引数をあてがいます   
    public ItemAdd(string name, int hp, int day, int quality, int _exp, 
        int rich, int sweat, int bitter, int sour, int crispy, int fluffy, int smooth, int hardness, int jiggly, int chewy, int powdery, int oily, int watery, 
        string type, string subtype, float _girl1_like, int cost, int sell, 
        string tp01, string tp02, string tp03, string tp04, string tp05, string tp06, string tp07, string tp08, string tp09, string tp10, string koyu_tp1, int kosu)
    {

        _Addname = name;
        _Addhp = hp;
        _Addday = day;

        _Addquality = quality;
        _Addexp = _exp;

        _Addrich = rich;
        _Addsweat = sweat;
        _Addbitter = bitter;
        _Addsour = sour;

        _Addcrispy = crispy;
        _Addfluffy = fluffy;
        _Addsmooth = smooth;
        _Addhardness = hardness;
        _Addjiggly = jiggly;
        _Addchewy = chewy;

        _Addpowdery = powdery;
        _Addoily = oily;
        _Addwatery = watery;


        _Add_itemType = type;
        _Add_itemType_sub = subtype;

        _Addgirl1_like = _girl1_like;

        _Addcost = cost;
        _Addsell = sell;


        _Addtp[0] = tp01;
        _Addtp[1] = tp02;
        _Addtp[2] = tp03;
        _Addtp[3] = tp04;
        _Addtp[4] = tp05;
        _Addtp[5] = tp06;
        _Addtp[6] = tp07;
        _Addtp[7] = tp08;
        _Addtp[8] = tp09;
        _Addtp[9] = tp10;

        _Addtp[10] = koyu_tp1;
        _Addtp[11] = "Non";
        _Addtp[12] = "Non";

        //現状こっちは未使用。_Addtpに全てまとめている。
        _Addkoyu_toppingtype[0] = koyu_tp1;
        _Addkoyu_toppingtype[1] = "Non";
        _Addkoyu_toppingtype[2] = "Non";
        //

        _Addkosu = kosu;
    }

}
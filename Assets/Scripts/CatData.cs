using UnityEngine;
using System;
using System.Collections;

// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class CatData
{
    public string catID;

    //いらないかも
    public string fileName;         //ねこアイコン画像ファイルネーム名。
    public Sprite catIcon_sprite;
    //

    public int caticon_Num;

    public string catnameHyouji;         //名前、画像ファイル名

    public int catType; //猫の種族　このタイプにより画像が決まる。ので、ここで保存する必要がない。

    public int catCost;
    public int catCostLV; //エサ代のレベル
    public int catTansaku_Speed; //探索スピード
    public int catTansaku_DefaultSpeed; //探索スピード　初期値のこと
    public int catTansaku_Kaisu; //探索回数
    public int cat_GetMateriaTimeCounter; //カウンタ　TimeControllerでへっていき、0になると採取
    public int catExp;
    public int catHP; //ねこの好感度のこと　体力ではない
    public int catLv;

    public string catTansaku_MapName;
    public int catStatus; //ひまか採取中の状態 0=ひまで何もしてない。 100=探索に出かけ中。
    public bool catEsaNoGive; //エサをあげれなかったとき、不機嫌状態になる。セリフが変わる。次にエサをあげると、これはオフになる。

    public string[] getmat_itemname_cat; //ねこがそれまで採ってきた材料のitemIDと個数
    public int[] getmat_kosu_cat;

    private int i;

    public CatData(string _id, string _catname, int _caticon_num, int _cattype, int _catcost, int _catcostlv, int _cattansaku_speed, int _cattansaku_kaisu, int _catexp, int _cathp, int _catlv,
        string _cattansaku_mapname, int _catstatus, bool _catesa_nogive)
    {
        catID = _id;

        caticon_Num = _caticon_num;
        catnameHyouji = _catname;

        catType = _cattype;
        catCost = _catcost;
        catCostLV = _catcostlv;
        catTansaku_Speed = _cattansaku_speed;
        catTansaku_DefaultSpeed = _cattansaku_speed;
        catTansaku_Kaisu = _cattansaku_kaisu;
        cat_GetMateriaTimeCounter = catTansaku_Speed;
        catExp = _catexp;
        catHP = _cathp;
        catLv = _catlv;

        catTansaku_MapName = _cattansaku_mapname;
        catStatus = _catstatus;
        catEsaNoGive = _catesa_nogive;

        getmat_itemname_cat = new string[99];
        getmat_kosu_cat = new int[99];

        for(i=0; i < getmat_itemname_cat.Length; i++)
        {
            getmat_itemname_cat[i] = "Non";
            getmat_kosu_cat[i] = 0;
        }           
    }
}

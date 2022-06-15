using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemCompound
{
    public int cmpitemID;
    public string cmpitem_Name;
    public string cmpitemID_1;
    public string cmpitemID_2;
    public string cmpitemID_3;
    public string cmp_subtype_1;
    public string cmp_subtype_2;
    public string cmp_subtype_3;
    public string cmpitemID_result;
    public int cmpitem_result_kosu;
    public int cmpitem_kosu1;
    public int cmpitem_kosu2;
    public int cmpitem_kosu3;
    public float cmpitem_bestkosu1;
    public float cmpitem_bestkosu2;
    public float cmpitem_bestkosu3;
    public int cmpitem_flag;

    public int cost_Time;

    public int success_Rate;
    public int renkin_Bexp;

    public string KeisanMethod; //値を加算するか、比率で計算するか。Nonの場合、加算。何らかのアイテム名が入ってる場合、それを基準に（例えば小麦粉）他の材料の値を計算する。
    public int comp_count;

    public string release_recipi; //新しくお菓子を作ったとき、ここに入った名前のレシピを解放する
    public int recipi_count; //ゲーム中でカウントするレシピ　これが0だと、レシピは表示されない。
    public int buf_kouka_on;
    public int secretFlag; //隠しレシピか否かのフラグ

    //Excelにはのってない変数
    public int hikari_make_count; //ヒカリがそのレシピを作った回数

    //ここでリスト化時に渡す引数をあてがいます   
    public ItemCompound(int id, string cmpname, string item1, string item2, string item3, string subtype1, string subtype2, string subtype3, 
        string result_item, int _result_kosu, int _kosu1, int _kosu2, int _kosu3, float _bestkosu1, float _bestkosu2, float _bestkosu3, 
        int _flag, int cost_time, int srate, int renkin_bexp, string _keisanm, int _comp_count, string _release_recipi, int _recipi_count, 
        int _buf_kouka_on, int _secretFlag, int _hikari_make_count)
    {
        cmpitemID = id;
        cmpitem_Name = cmpname;
        cmpitemID_1 = item1;
        cmpitemID_2 = item2;
        cmpitemID_3 = item3;
        cmp_subtype_1 = subtype1;
        cmp_subtype_2 = subtype2;
        cmp_subtype_3 = subtype3;

        cmpitemID_result = result_item;
        cmpitem_result_kosu = _result_kosu;

        cmpitem_kosu1 = _kosu1;
        cmpitem_kosu2 = _kosu2;
        cmpitem_kosu3 = _kosu3;
        cmpitem_bestkosu1 = _bestkosu1;
        cmpitem_bestkosu2 = _bestkosu2;
        cmpitem_bestkosu3 = _bestkosu3;
        cmpitem_flag = _flag;

        cost_Time = cost_time;

        success_Rate = srate;
        renkin_Bexp = renkin_bexp;

        KeisanMethod = _keisanm;
        comp_count = _comp_count;

        release_recipi = _release_recipi;
        recipi_count = _recipi_count;
        buf_kouka_on = _buf_kouka_on;
        secretFlag = _secretFlag;

        _hikari_make_count = hikari_make_count;
    }

}
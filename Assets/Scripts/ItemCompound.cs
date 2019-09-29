using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemCompound
{
    public int cmpitemID;
    public string cmpitemID_1;
    public string cmpitemID_2;
    public string cmpitemID_3;
    public string cmpitemID_result;
    public int cmpitem_kosu1;
    public int cmpitem_kosu2;
    public int cmpitem_kosu3;
    public int cmpitem_flag;

    public int cost_Time;

    public int success_Rate;
    public int renkin_Bexp;


    //ここでリスト化時に渡す引数をあてがいます   
    public ItemCompound(int id, string item1, string item2, string item3, string result_item, int _kosu1, int _kosu2, int _kosu3, int _flag, int cost_time, int srate, int renkin_bexp)
    {
        cmpitemID = id;
        cmpitemID_1 = item1;
        cmpitemID_2 = item2;
        cmpitemID_3 = item3;
        cmpitemID_result = result_item;

        cmpitem_kosu1 = _kosu1;
        cmpitem_kosu2 = _kosu2;
        cmpitem_kosu3 = _kosu3;
        cmpitem_flag = _flag;

        cost_Time = cost_time;

        success_Rate = srate;
        renkin_Bexp = renkin_bexp;
    }

}
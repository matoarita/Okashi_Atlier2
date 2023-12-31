﻿using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class SlotName
{
    public int roastID;
    public string slotName;
    public string slot_Hyouki_1;
    public string slot_Hyouki_2;
    public int slot_totalScore;
    public int slot_getgirllove;
    public int slot_Money;
    public int slot_Beauty;


    //ここでリスト化時に渡す引数をあてがいます   
    public SlotName(int id, string slotname, string slot_H1, string slot_H2, int slot_totalscore, int slot_gscore, int slot_money, int slot_beauty)
    {
        roastID = id;

        slotName = slotname;

        if (slotname == "Non")
        {
            slot_Hyouki_1 = "";
            slot_Hyouki_2 = "";
        }
        else
        {
            slot_Hyouki_1 = slot_H1;
            slot_Hyouki_2 = slot_H2;
        }

        slot_totalScore = slot_totalscore;
        slot_getgirllove = slot_gscore;
        slot_Money = slot_money;
        slot_Beauty = slot_beauty;
    }

}
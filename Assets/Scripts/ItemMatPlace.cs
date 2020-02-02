using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemMatPlace
{
    public int matplaceID;
    public string placeName;
    public string placeNameHyouji;
    public int placeCost;
    public int placeFlag;
    public string dropItem1;
    public string dropItem2;
    public string dropItem3;
    public string dropItem4;
    public string dropItem5;
    public string dropRare1;
    public string dropRare2;
    public string dropRare3;
    public float dropProb1;
    public float dropProb2;
    public float dropProb3;
    public float dropProb4;
    public float dropProb5;
    public float dropRareProb1;
    public float dropRareProb2;
    public float dropRareProb3;



    //ここでリスト化時に渡す引数をあてがいます   
    public ItemMatPlace(int id, string place_name, string place_name_Hyouji, int place_cost, int place_flag, string drop_item1, string drop_item2, string drop_item3, string drop_item4, string drop_item5, string drop_rare1, string drop_rare2, string drop_rare3, float drop_prob1, float drop_prob2, float drop_prob3, float drop_prob4, float drop_prob5, float drop_rare_prob1, float drop_rare_prob2, float drop_rare_prob3)
    {
        matplaceID = id;

        placeName = place_name;
        placeNameHyouji = place_name_Hyouji;

        placeCost = place_cost;
        placeFlag = place_flag;

        dropItem1 = drop_item1;
        dropItem2 = drop_item2;
        dropItem3 = drop_item3;
        dropItem4 = drop_item4;
        dropItem5 = drop_item5;
        dropRare1 = drop_rare1;
        dropRare2 = drop_rare2;
        dropRare3 = drop_rare3;

        dropProb1 = drop_prob1;
        dropProb2 = drop_prob2;
        dropProb3 = drop_prob3;
        dropProb4 = drop_prob4;
        dropProb5 = drop_prob5;
        dropRareProb1 = drop_rare_prob1;
        dropRareProb2 = drop_rare_prob2;
        dropRareProb3 = drop_rare_prob3;
    }

}
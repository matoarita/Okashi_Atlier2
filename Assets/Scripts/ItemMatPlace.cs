using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemMatPlace
{
    public int matplaceID;
    public string placeName;
    public string placeNameHyouji;
    public int placeDay;
    public int placeCost;
    public int placeHP;
    public int placeFlag;
    public int placeDefaultFlag;
    public int placeType;   //街=0か、ダンジョンタイプ=1か。
    public string dropItem1;
    public string dropItem2;
    public string dropItem3;
    public string dropItem4;
    public string dropItem5;
    public string dropItem6;
    public string dropItem7;
    public string dropItem8;
    public string dropItem9;
    public string dropItem10;
    public string dropRare1;
    public string dropRare2;
    public string dropRare3;
    public float dropProb1;
    public float dropProb2;
    public float dropProb3;
    public float dropProb4;
    public float dropProb5;
    public float dropProb6;
    public float dropProb7;
    public float dropProb8;
    public float dropProb9;
    public float dropProb10;
    public float dropRareProb1;
    public float dropRareProb2;
    public float dropRareProb3;

    public Sprite center_bg;
    public Sprite back_bg;

    public int read_end; //各マップリストデータの読み終わり地点

    public Sprite mapIcon_sprite;

    //ここでリスト化時に渡す引数をあてがいます   
    public ItemMatPlace(int id, string fileName, string place_name, string place_name_Hyouji, int place_day, int place_cost, int place_hp, int place_flag, int place_default_flag, 
        int place_type,
        string drop_item1, string drop_item2, string drop_item3, string drop_item4, string drop_item5,
        string drop_item6, string drop_item7, string drop_item8, string drop_item9, string drop_item10,
        string drop_rare1, string drop_rare2, string drop_rare3, 
        float drop_prob1, float drop_prob2, float drop_prob3, float drop_prob4, float drop_prob5,
        float drop_prob6, float drop_prob7, float drop_prob8, float drop_prob9, float drop_prob10, 
        float drop_rare_prob1, float drop_rare_prob2, float drop_rare_prob3, string _center_bg, string _back_bg, int _read_end)
    {
        matplaceID = id;

        placeName = place_name;
        placeNameHyouji = place_name_Hyouji;

        placeDay = place_day;
        placeCost = place_cost;
        placeHP = place_hp;
        placeFlag = place_flag;
        placeDefaultFlag = place_default_flag;
        placeType = place_type;

        dropItem1 = drop_item1;
        dropItem2 = drop_item2;
        dropItem3 = drop_item3;
        dropItem4 = drop_item4;
        dropItem5 = drop_item5;
        dropItem6 = drop_item6;
        dropItem7 = drop_item7;
        dropItem8 = drop_item8;
        dropItem9 = drop_item9;
        dropItem10 = drop_item10;
        dropRare1 = drop_rare1;
        dropRare2 = drop_rare2;
        dropRare3 = drop_rare3;

        dropProb1 = drop_prob1;
        dropProb2 = drop_prob2;
        dropProb3 = drop_prob3;
        dropProb4 = drop_prob4;
        dropProb5 = drop_prob5;
        dropProb6 = drop_prob6;
        dropProb7 = drop_prob7;
        dropProb8 = drop_prob8;
        dropProb9 = drop_prob9;
        dropProb10 = drop_prob10;
        dropRareProb1 = drop_rare_prob1;
        dropRareProb2 = drop_rare_prob2;
        dropRareProb3 = drop_rare_prob3;

        mapIcon_sprite = Resources.Load<Sprite>("Sprites/BG_Icon/" + fileName);

        center_bg = Resources.Load<Sprite>("Utage_Scenario/Texture/Bg/MatPlace/" + _center_bg);
        back_bg = Resources.Load<Sprite>("Utage_Scenario/Texture/Bg/MatPlace/" + _back_bg);

        read_end = _read_end;
    }

}
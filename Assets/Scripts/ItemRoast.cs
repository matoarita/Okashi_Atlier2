using UnityEngine;
using System.Collections;


// 調合組み合わせ用データ　List型に対応している。３つの値（選択アイテム１と２、結果用のアイテムID）を持つ

[System.Serializable]//この属性を使ってインスペクター上で表示

public class ItemRoast
{
    public int roastID;
    public string roast_itemName;
    public string roast_itemName_result;
    public int roast_itemID;
    public int roast_item_resultID;



    //ここでリスト化時に渡す引数をあてがいます   
    public ItemRoast(int id, string item1, string item2, int id1, int id2)
    {
        roastID = id;
        roast_itemName = item1;
        roast_itemName_result = item2;
        roast_itemID = id1;
        roast_item_resultID = id2;
    }

}
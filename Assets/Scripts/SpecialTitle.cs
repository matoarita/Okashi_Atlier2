using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class SpecialTitle
{
    public int ID;
    public string titleName;
    public string titleNameHyouji;
    public bool Flag; //イベントアイテム用の項目
    public int Score;
    public Item ItemData; //アイテムのデータ保存用

    public Sprite imgIcon_sprite; //使う場合は、_fileNameに、"Sprites/"以下のフォルダ名/画像データ名を指定。使わない場合は、Non
    //ここまで


    //ここでリスト化時に渡す引数をあてがいます   
    public SpecialTitle(int _id, string _name, string _namehyouji, bool _flag, string _fileName)
    {
        ID = _id;
        titleName = _name;
        titleNameHyouji = _namehyouji;
        Flag = _flag;

        if (_fileName != "Non")
        {
            imgIcon_sprite = Resources.Load<Sprite>("Sprites/" + _fileName);
        }

        Score = 0;

        ItemData = new Item(9999, "Non", "orange", "Non" + "Non" + " " + "Non", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        "Non", "Non", 0, 0, 0, 0, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0,
                        0, 0, 0, 0, 0, 0, "", 0, 1, 0, 0);
    }
}
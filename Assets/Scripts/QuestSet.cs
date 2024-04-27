using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class QuestSet
{
    public int _ID;
    public int Quest_ID;    //DB固有の番号。IDは、リストの順番を入れ替えると、順番が崩れてしまうので、各リストに固有のIDとしてつけている。

    public int QuestType;   //0なら材料採取系。1ならお菓子を納品系。こっちは、プレイヤーが納品するアイテムを、リストから選択する形式
    public int QuestHyouji; //数字を指定すると、ストーリーの進行によって、どのクエストが出るのかを操作できる。
    public int QuestHyoujiHeart; //ハートレベルに応じてクエスト表示を追加
    public int HighType; //そのクエストが高品質クエストかどうか

    public Sprite questIcon;      //アイコン
    public string Quest_FileName;
    public string Quest_itemName;
    public string Quest_itemSubtype;

    public int Quest_kosu_default;
    public int Quest_kosu_min;
    public int Quest_kosu_max;
    public int Quest_buy_price;

    public int Quest_rich;
    public int Quest_sweat;
    public int Quest_bitter;
    public int Quest_sour;
    public int Quest_crispy;
    public int Quest_fluffy;
    public int Quest_smooth;
    public int Quest_hardness;
    public int Quest_jiggly;
    public int Quest_chewy;
    public int Quest_juice;

    public int Quest_beauty;
    public int Quest_tea_flavor;

    public string[] Quest_topping = new string[5];
    public int[] Quest_tp_score = new int[5];

    public int Quest_AfterDay;
    public int Quest_LimitMonth;
    public int Quest_LimitDay;
    public int Quest_AreaType; //どの酒場の依頼か
    public string Quest_ClientName; //依頼者の名前

    public string Quest_Title;
    public string Quest_desc;
    public int read_endflag;


    //ここでリスト化時に渡す引数をあてがいます   
    public QuestSet(int id, int _questID, int _questType, int _questHyouji, int _questHyoujiHeart, int _hightype, 
        string fileName, string _itemname, string _itemsubtype, 
        int _kosu_default, int _kosu_min, int _kosu_max, int _buy_price, 
        int _rich, int _sweat, int _bitter, int _sour, int _crispy, int _fluffy, int _smooth, int _hardness, int _jiggly, int _chewy, int _juice, int _beauty,
        string tp01, string tp02, string tp03, string tp04, string tp05, int tp_score_01, int tp_score_02, int tp_score_03, int tp_score_04, int tp_score_05,
        int _quest_afterday, int _quest_limitmonth, int _quest_limitday, int _quest_areaType, string _quest_clientname, string _title, string _setkansou, int _read_endflag)
    {
        _ID = id;
        Quest_ID = _questID;
        QuestType = _questType;
        QuestHyouji = _questHyouji;
        QuestHyoujiHeart = _questHyoujiHeart;
        HighType = _hightype;

        Quest_FileName = fileName;
        questIcon = Resources.Load<Sprite>("Sprites/Items/" + Quest_FileName);
        Quest_itemName = _itemname;
        Quest_itemSubtype = _itemsubtype;

        Quest_kosu_default = _kosu_default;
        Quest_kosu_min = _kosu_min;
        Quest_kosu_max = _kosu_max;
        Quest_buy_price = _buy_price;

        Quest_rich = _rich;
        Quest_sweat = _sweat;
        Quest_bitter = _bitter;
        Quest_sour = _sour;

        Quest_crispy = _crispy;
        Quest_fluffy = _fluffy;
        Quest_smooth = _smooth;
        Quest_hardness = _hardness;
        Quest_jiggly = _jiggly;
        Quest_chewy = _chewy;

        Quest_juice = _juice;
        Quest_beauty = _beauty;

        Quest_topping[0] = tp01;
        Quest_topping[1] = tp02;
        Quest_topping[2] = tp03;
        Quest_topping[3] = tp04;
        Quest_topping[4] = tp05;

        Quest_tp_score[0] = tp_score_01;
        Quest_tp_score[1] = tp_score_02;
        Quest_tp_score[2] = tp_score_03;
        Quest_tp_score[3] = tp_score_04;
        Quest_tp_score[4] = tp_score_05;

        Quest_AfterDay = _quest_afterday;
        Quest_LimitMonth = _quest_limitmonth;
        Quest_LimitDay = _quest_limitday;
        Quest_AreaType = _quest_areaType;
        Quest_ClientName = _quest_clientname;

        Quest_Title = _title;
        Quest_desc = _setkansou;
        read_endflag = _read_endflag;
    }

    public void ResetSprite(string fileName)
    {
        Quest_FileName = fileName;
        questIcon = Resources.Load<Sprite>("Sprites/Items/" + Quest_FileName);
    }
}
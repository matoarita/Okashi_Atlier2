using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//シングルトン化しているので、ゲーム中ItemDataBaseは一個だけ。Findで探す必要もないので、itemDataBaseクラスを使うときは、その書き方にならうこと。
//できれば、ゲーム中のタイトル画面などで、一回だけ読むのがふさわしい。今は、mainで毎回読み込んでいるので、あとで修正が必要。

public class ItemDataBase : SingletonMonoBehaviour<ItemDataBase>
{

    private Entity_ItemDataBase excel_itemdatabase; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private int _id;
    private string _file_name;
    private string _name;
    private string _name_hyouji;
    private string _desc;
    private int _hp;
    private int _comp_hosei;
    private int _day;

    private int _exp;
    private int _quality;
    private float _ex_probability;

    private int _rich;
    private int _sweat;
    private int _bitter;
    private int _sour;

    private int _crispy;
    private int _fluffy;
    private int _smooth;
    private int _hardness;
    private int _jiggly;
    private int _chewy;

    private int _powdery;
    private int _oily;
    private int _watery;

    private int _beauty;
    private int _juice;

    private string _type;
    private string _subtype;

    private int _base_score;
    private float _girl1_like;
    private int _rare;
    private int _manpuku;
    private int _secretFlag;

    private int _cost;
    private int _sell;

    private string _tp01;
    private string _tp02;
    private string _tp03;
    private string _tp04;
    private string _tp05;
    private string _tp06;
    private string _tp07;
    private string _tp08;
    private string _tp09;
    private string _tp10;

    private string[] _koyutp = new string[5];

    private int _itemhyouji;
    private int _judge_num;
    private int _ex_kaisu = 1;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    //public int trash_ID_1; //失敗アイテムのアイテムID。他クラスから参照されます。

    public List<int> sheet_topendID = new List<int>(); //シートごとに、IDの頭と最後を、順番に入れている。[0][1]は、シート０のIDの頭、と最後、という感じ。

    //エクセルのデータを、一度itemsというリストに入れる。itemsは、クラス「Item」型を生成する。
    //Item型内で、画像データ（texture2d）を保存している。エクセルでは、画像を直接スプライトで保存できないため、こうしている。nameの部分が、画像のパス・ファイル名にあたる。
    public List<Item> items = new List<Item>();
    public List<Item> items_gamedefault = new List<Item>(); //ゲーム最初の設定データ。ゲーム内で再計算されて、上書きされたものが、上のメインとなるitems。

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です。
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        items.Clear();
        items_gamedefault.Clear();

        excel_itemdatabase = Resources.Load("Excel/Entity_ItemDataBase") as Entity_ItemDataBase;

        sheet_topendID.Add(0); //シートの頭のIDは0。

        //trash_ID_1 = 500;
        sheet_no = 0;

        while (sheet_no < excel_itemdatabase.sheets.Count)
        {
            count = 0;

            while (count < excel_itemdatabase.sheets[sheet_no].list.Count)
            {
                // 一旦代入
                _id = excel_itemdatabase.sheets[sheet_no].list[count].ItemID;
                _file_name = excel_itemdatabase.sheets[sheet_no].list[count].file_name;
                _name = excel_itemdatabase.sheets[sheet_no].list[count].name;
                _name_hyouji = excel_itemdatabase.sheets[sheet_no].list[count].nameHyouji;
                _desc = excel_itemdatabase.sheets[sheet_no].list[count].desc;
                _comp_hosei = excel_itemdatabase.sheets[sheet_no].list[count].comp_hosei;
                _hp = excel_itemdatabase.sheets[sheet_no].list[count].hp;
                _day = excel_itemdatabase.sheets[sheet_no].list[count].day;

                _quality = excel_itemdatabase.sheets[sheet_no].list[count].quality;
                _exp = excel_itemdatabase.sheets[sheet_no].list[count].exp;
                _ex_probability = excel_itemdatabase.sheets[sheet_no].list[count].ex_probability;

                _rich = excel_itemdatabase.sheets[sheet_no].list[count].rich;
                _sweat = excel_itemdatabase.sheets[sheet_no].list[count].sweat;
                _bitter = excel_itemdatabase.sheets[sheet_no].list[count].bitter;
                _sour = excel_itemdatabase.sheets[sheet_no].list[count].sour;

                _crispy = excel_itemdatabase.sheets[sheet_no].list[count].crispy;
                _fluffy = excel_itemdatabase.sheets[sheet_no].list[count].fluffy;
                _smooth = excel_itemdatabase.sheets[sheet_no].list[count].smooth;
                _hardness = excel_itemdatabase.sheets[sheet_no].list[count].hardness;
                _jiggly = excel_itemdatabase.sheets[sheet_no].list[count].jiggly;
                _chewy = excel_itemdatabase.sheets[sheet_no].list[count].chewy;

                _powdery = excel_itemdatabase.sheets[sheet_no].list[count].powdery;
                _oily = excel_itemdatabase.sheets[sheet_no].list[count].oily;
                _watery = excel_itemdatabase.sheets[sheet_no].list[count].watery;

                _beauty = excel_itemdatabase.sheets[sheet_no].list[count].beauty;

                _type = excel_itemdatabase.sheets[sheet_no].list[count].type;
                _subtype = excel_itemdatabase.sheets[sheet_no].list[count].subtype;

                _base_score = excel_itemdatabase.sheets[sheet_no].list[count].base_score;
                _girl1_like = excel_itemdatabase.sheets[sheet_no].list[count].girl1_like;

                _cost = excel_itemdatabase.sheets[sheet_no].list[count].cost_price;
                _sell = excel_itemdatabase.sheets[sheet_no].list[count].sell_price;

                _tp01 = excel_itemdatabase.sheets[sheet_no].list[count].topping01;
                _tp02 = excel_itemdatabase.sheets[sheet_no].list[count].topping02;
                _tp03 = excel_itemdatabase.sheets[sheet_no].list[count].topping03;
                _tp04 = excel_itemdatabase.sheets[sheet_no].list[count].topping04;
                _tp05 = excel_itemdatabase.sheets[sheet_no].list[count].topping05;
                _tp06 = excel_itemdatabase.sheets[sheet_no].list[count].topping06;
                _tp07 = excel_itemdatabase.sheets[sheet_no].list[count].topping07;
                _tp08 = excel_itemdatabase.sheets[sheet_no].list[count].topping08;
                _tp09 = excel_itemdatabase.sheets[sheet_no].list[count].topping09;
                _tp10 = excel_itemdatabase.sheets[sheet_no].list[count].topping10;

                _koyutp[0] = excel_itemdatabase.sheets[sheet_no].list[count].koyu_topping1;
                _koyutp[1] = excel_itemdatabase.sheets[sheet_no].list[count].koyu_topping2;
                _koyutp[2] = excel_itemdatabase.sheets[sheet_no].list[count].koyu_topping3;
                _koyutp[3] = excel_itemdatabase.sheets[sheet_no].list[count].koyu_topping4;
                _koyutp[4] = excel_itemdatabase.sheets[sheet_no].list[count].koyu_topping5;

                _itemhyouji = excel_itemdatabase.sheets[sheet_no].list[count].item_hyouji;
                _judge_num = excel_itemdatabase.sheets[sheet_no].list[count].Set_JudgeNum;

                _rare = excel_itemdatabase.sheets[sheet_no].list[count].Rare;
                _manpuku = excel_itemdatabase.sheets[sheet_no].list[count].Manpuku;
                _secretFlag = excel_itemdatabase.sheets[sheet_no].list[count].SecretFlag;

                _juice = _sweat + _bitter + _sour;

                //ここでリストに追加している
                items.Add(new Item(_id, "Non", _file_name, _name, _name_hyouji, _desc, _comp_hosei, _hp, _day, _quality, _exp, _ex_probability,
                    _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _juice, _type, _subtype, _base_score, _girl1_like, 
                    _cost, _sell, _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10,
                    _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4], 0, _ex_kaisu, _itemhyouji, _judge_num, 0, 0, 0, "", 0, _rare, _manpuku, _secretFlag));
                //comp_hoseiでバグらないようにするための、クローン
                items_gamedefault.Add(new Item(_id, "Non", _file_name, _name, _name_hyouji, _desc, _comp_hosei, _hp, _day, _quality, _exp, _ex_probability,
                    _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _powdery, _oily, _watery, _beauty, _juice, _type, _subtype, _base_score, _girl1_like,
                    _cost, _sell, _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09, _tp10,
                    _koyutp[0], _koyutp[1], _koyutp[2], _koyutp[3], _koyutp[4], 0, _ex_kaisu, _itemhyouji, _judge_num, 0, 0, 0, "", 0, _rare, _manpuku, _secretFlag));

                ++count;
            }

            ++sheet_no;


            sheet_topendID.Add(_id); // sheetの終わりのIDを入れる。シート0～から。          

            if (sheet_no < excel_itemdatabase.sheets.Count )
            {
                sheet_count = _id + 1; //一枚前のシートの要素数をカウント　_idのラストは、例えば2が入っているので、+1すれば、要素数になる

                for (i = 0; i < excel_itemdatabase.sheets[sheet_no].list[0].ItemID - sheet_count; i++) //次のシートの0行目のID番号をみる。例えば300とか。
                {
                    items.Add(new Item(_id + i + 1, "Non", "orange", "Non" + (sheet_no - 1).ToString() + " " + (sheet_count + i).ToString(), "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        "Non", "Non", 0, 0, 0, 0, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0,
                        _ex_kaisu, 0, 0, 0, 0, 0, "", 0, 1, 0, 0));

                    //comp_hoseiでバグらないようにするための、クローン
                    items_gamedefault.Add(new Item(_id + i + 1, "Non", "orange", "Non" + (sheet_no - 1).ToString() + " " + (sheet_count + i).ToString(), "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        "Non", "Non", 0, 0, 0, 0, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0,
                        _ex_kaisu, 0, 0, 0, 0, 0, "", 0, 1, 0, 0));
                }

                sheet_topendID.Add(excel_itemdatabase.sheets[sheet_no].list[0].ItemID); // 次sheetの頭のIDを入れる。
            }
        }


        // デバッグ用 //
        /*for (i = 0; i < items.Count; i++)
        {
            Debug.Log(items[i].itemID);
        }*/

        /*for (i = 0; i < sheet_topendID.Count; i++)
        {
            Debug.Log( "  " + sheet_topendID[i]);
        }*/

        //Debug.Log(items[excel_itemdatabase.sheets[1].list[0].ItemID].itemID);

        /*// (int id, string name, string nameHyouji, string desc, int mp, int day, int sweat, int sour, int crispy, int fluffy, KaoriType kaori, int kaori_p, ColorType color1, ColorType color2, ElementType etype, ItemType type, ItemType_sub subtype)
        items.Add(new Item(0, "orange", "オレンジ", "Orange desu", 3, 1, 30, 20, 0, 0, 0, 20, 0, 3, 5));
        items.Add(new Item(1, "grape", "ぶどう", "budou desu", 4, 1, 20, 40, 0, 0, 1, 20, 1, 3, 5));*/

    }

    public int GetItemFirstEat(string _itemname)
    {
        i = 0;
        while(i<items.Count)
        {
            if(items[i].itemName == _itemname)
            {
                return items[i].Eat_kaisu;                
            }
            i++;
        }

        return 0;
    }

    //前回スコアの初期化
    public void ResetLastScore()
    {
        for (i = 0; i < items.Count; i++)
        {
            items[i].ExtremeKaisu = _ex_kaisu;
            items[i].Eat_kaisu = 0;
            items[i].HighScore_flag = 0;

            items[i].last_total_score = 0;
            items[i].last_rich_score = 0;
            items[i].last_sweat_score = 0;
            items[i].last_bitter_score = 0;
            items[i].last_sour_score = 0;
            items[i].last_crispy_score = 0;
            items[i].last_fluffy_score = 0;
            items[i].last_smooth_score = 0;
            items[i].last_hardness_score = 0;
            items[i].last_jiggly_score = 0;
            items[i].last_chewy_score = 0;
            items[i].last_beauty_score = 50;
            items[i].last_hinttext = "";
        }
    }

    //アイテムIDをいれると、そのアイテムの(配列番号)を返すメソッド
    public int SearchItemID(int _itemID)
    {

        i = 0;
        while (i <= items.Count)
        {
            if (items[i].itemID == _itemID)
            {
                return i;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999

    }

    //アイテム名をいれると、そのアイテムの(配列番号)を返すメソッド
    public int SearchItemIDString(string Name)
    {
        if (Name == "Non")
        {
            return 9999;
        }
        else
        {
            i = 0;
            while (i <= items.Count)
            {
                if (items[i].itemName == Name)
                {
                    return i;
                }
                i++;
            }

            return 9999; //見つからなかった場合、9999
        }
    }
   
}
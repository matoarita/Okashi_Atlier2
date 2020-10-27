using UnityEngine;
using System;
using System.Collections;


// アイテムの設定用データ　List型に対応している。　class Item 直下で、変数を宣言。　public Item(...)が、引数をうける関数？

[System.Serializable]//この属性を使ってインスペクター上で表示

public class Item
{
    public string fileName;         //ファイルネーム名。
    public string itemName;         //名前、画像ファイル名
    public string itemNameHyouji;   //名前　ゲーム中での表示用。日本語。
    public int itemID;              //アイテムID
    public string itemDesc;         //アイテムの説明文
    public Texture2D itemIcon;      //アイコン
    public Sprite itemIcon_sprite;      //アイコン
    public int itemComp_Hosei;      //材料が生成される際、お菓子自体のパラメータを加算するかどうか。1だと加算する。現状使ってない。いらないかも。
    public int itemHP;              //消費MP
    public int item_day;            //調合に必要な日数。アイテム同士で加算する。
    public int ItemKosu;
    public int ExtremeKaisu;        //エクストリーム残り回数

    public int Exp;                 //おかし自体の経験値。エクストリームの際、それぞれ合計したものが取得経験値になる。
    public int Quality;             //おかしの品質を表す数値。
    public float Ex_Probability;    //エクストリーム調合時、確率乗算パラメータ

    public int Rich;                //おかしの味の深さ・コクを表す。少ないと、さっぱりたんぱくな味わい、高いとコクがある。
    public int Sweat;               //おかしの味　あまさ
    public int Bitter;              //おかしの味　苦さ
    public int Sour;                //おかしの味　すっぱさ

    public int Crispy;              //おかしの食感　さくさく
    public int Fluffy;              //おかしの食感　ふわふわ
    public int Smooth;              //おかしの食感　しっとり　口溶けの良さ。とろとろ（なめらかさ）
    public int Hardness;            //おかしの食感　歯ごたえ　強いと固くざっくり、どっしりした歯ごたえ。ガチガチ。少ないと、ほろほろに崩れる。軽い。
    public int Jiggly;              //おかしの食感　ぷるぷる
    public int Chewy;               //おかしの食感　もちもち

    public int Powdery;             //粉っぽさ　マイナス要因   粉を入れすぎると、ガッチガチを超えて、粉の塊になりマズくなる。
    public int Oily;                //油っぽさ　マイナス要因   バターを入れすぎると、油っこくなり、気持ち悪くなる。
    public int Watery;              //水っぽさ　マイナス要因   水・ミルクなどを入れすぎると、水っぽくなり、固まらない。

    public ItemType itemType;      //アイテムの種類メインカテゴリー
    public ItemType_sub itemType_sub;      //アイテムの種類サブカテゴリー（クッキー系とかパイ系など）

    public float girl1_itemLike; //そのアイテムに対する女の子１の好み値。固有。

    public int cost_price; //ショップで買うときの値段
    public int sell_price; //ショップに売るときの値段

    public int item_Hyouji;
    public int SetJudge_Num;

    public float total_kyori; //ベスト配合と現在配合した材料の距離を保存。アイテムランクで表示される。

    //以下パラメータはExcel上には記載なし
    public int Eat_kaisu;
    public bool HighScore_flag;
    public int last_total_score;
    public int last_rich_score;
    public int last_sweat_score;
    public int last_bitter_score;
    public int last_sour_score;
    public int last_crispy_score;
    public int last_fluffy_score;
    public int last_smooth_score;
    public int last_hardness_score;
    public int last_jiggly_score;
    public int last_chewy_score;
    public string last_hinttext;
    public string item_SlotName; //スロット名部分のみの名称。色変更用に。
    public string item_FullName; //スロット名も含めた最終の名称。オリジナルアイテムリスト用で使う。
    //ここまで

    //トッピングスロット
    public string[] toppingtype = new string[10];

    //トッピングスロット
    public string[] koyu_toppingtype = new string[5];


    //アイテムタイプ　「お菓子・アクセサリー・くすり・材料」の4種類
    public enum ItemType
    {
        Non,
        Okashi,
        Acce,
        Potion,
        Mat,
        Etc
    }

    //アイテムタイプ　サブカテゴリー
    public enum ItemType_sub
    {
        Non,
        Cookie,
        Cookie_base,        
        Pie,
        Pie_base,
        Chocolate,
        Chocolate_Mat,
        Chocolate_base,
        Cake,
        Cake_base,
        Rusk,
        Creampuff,
        Crepe,
        Donuts,
        Parfe,
        IceCream,
        Bread,
        PanCake,
        Financier,
        Maffin,
        Biscotti,
        Jelly,
        Omochi,
        Appaleil,
        Cream,
        Pate,
        Komugiko,
        Egg,
        Suger,
        Salt,
        Butter,
        Fruits,
        Nuts,
        Fish,
        Source,
        Potion,
        Juice,
        Tea,
        Soup,
        Coffee,
        Machine,
        Donguri,
        Rare,
        Etc
    }

    //ここでリスト化時に渡す引数をあてがいます   
    public Item(int id, string file_name, string name, string nameHyouji, string desc, int _comp_hosei, int hp, int day, int quality, int _exp, float ex_pro, 
        int rich, int sweat, int bitter, int sour, int crispy, int fluffy, int smooth, int hardness, int jiggly, int chewy, int powdery, int oily, int watery, 
        string type, string subtype, float _girl1_like, int cost, int sell, 
        string tp01, string tp02, string tp03, string tp04, string tp05, string tp06, string tp07, string tp08, string tp09, string tp10, 
        string koyu_tp1, string koyu_tp2, string koyu_tp3, string koyu_tp4, string koyu_tp5, int itemkosu, int extreme_kaisu, int _item_hyouji, 
        int _judge_num, int _eat_kaisu, bool _highscore, int _lasttotal_score, string _hinttext, float _total_kyori)
    {
        itemID = id;
        fileName = file_name;

        itemName = name;
        itemNameHyouji = nameHyouji;
        
        //アイコンはnameとイコールにするのでアイコンがあるパス＋nameで取ってきます    
        itemIcon = Resources.Load<Texture2D>("Sprites/Items/" + fileName);
        itemIcon_sprite = Resources.Load<Sprite>("Sprites/Items/" + fileName);
        itemDesc = desc;
        itemHP = hp;
        item_day = day;
        itemComp_Hosei = _comp_hosei;

        Exp = _exp;

        Quality = quality;
        Ex_Probability = ex_pro;

        Rich = rich;
        Sweat = sweat;
        Bitter = bitter;
        Sour = sour;

        Crispy = crispy;
        Fluffy = fluffy;
        Smooth = smooth;
        Hardness = hardness;
        Jiggly = jiggly;
        Chewy = chewy;

        Powdery = powdery;
        Oily = oily;
        Watery = watery;

        
        itemType = (ItemType)Enum.Parse(typeof(ItemType), type);      
        itemType_sub = (ItemType_sub)Enum.Parse(typeof(ItemType_sub), subtype);

        girl1_itemLike = _girl1_like;

        cost_price = cost;
        sell_price = sell;

        toppingtype[0] = tp01;
        toppingtype[1] = tp02;
        toppingtype[2] = tp03;
        toppingtype[3] = tp04;
        toppingtype[4] = tp05;
        toppingtype[5] = tp06;
        toppingtype[6] = tp07;
        toppingtype[7] = tp08;
        toppingtype[8] = tp09;
        toppingtype[9] = tp10;

        koyu_toppingtype[0] = koyu_tp1;
        koyu_toppingtype[1] = koyu_tp2;
        koyu_toppingtype[2] = koyu_tp3;
        koyu_toppingtype[3] = koyu_tp4;
        koyu_toppingtype[4] = koyu_tp5;

        ItemKosu = itemkosu;

        ExtremeKaisu = extreme_kaisu; //エクストリームは3回まで

        item_Hyouji = _item_hyouji;
        SetJudge_Num = _judge_num;

        Eat_kaisu = _eat_kaisu;
        HighScore_flag = _highscore;

        last_total_score = _lasttotal_score;
        last_rich_score = 0;
        last_sweat_score = 0;
        last_bitter_score = 0;
        last_sour_score = 0;
        last_crispy_score = 0;
        last_fluffy_score = 0;
        last_smooth_score = 0;
        last_hardness_score = 0;
        last_jiggly_score = 0;
        last_chewy_score = 0;
        last_hinttext = _hinttext;
        item_SlotName = "";
        item_FullName = item_SlotName + itemNameHyouji; //何もしなければ、アイテム名が入っている。

        total_kyori = _total_kyori;
    }

}
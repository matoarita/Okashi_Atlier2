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
    public int itemMP;              //消費MP
    public int item_day;            //調合に必要な日数。アイテム同士で加算する。
    public int ItemKosu;
    public int ExtremeKaisu;        //エクストリーム残り回数

    public int Quality;             //おかしの品質を表す数値。

    public int Rich;                //おかしの味の深さ・コクを表す。少ないと、さっぱりたんぱくな味わい、高いとコクがある。
    public int Sweat;               //おかしの味　あまさ
    public int Bitter;              //おかしの味　苦さ
    public int Sour;                //おかしの味　すっぱさ

    public int Crispy;              //おかしの食感　さくさく
    public int Fluffy;              //おかしの食感　ふわふわ
    public int Smooth;              //おかしの食感　口溶けの良さ。とろとろ（なめらかさ）
    public int Hardness;            //おかしの食感　ほろほろ　強いとざっくり、どっしりした歯ごたえ
    public int Jiggly;              //おかしの食感　ぷるぷる
    public int Chewy;               //おかしの食感　もちもち

    public int Powdery;             //粉っぽさ　マイナス要因   粉を入れすぎると、ガッチガチを超えて、粉の塊になりマズくなる。
    public int Oily;                //油っぽさ　マイナス要因   バターを入れすぎると、油っこくなり、気持ち悪くなる。
    public int Watery;              //水っぽさ　マイナス要因   水・ミルクなどを入れすぎると、水っぽくなり、固まらない。

    public ItemType itemType;      //アイテムの種類メインカテゴリー
    public ItemType_sub itemType_sub;      //アイテムの種類サブカテゴリー（クッキー系とかパイ系など）

    public int girl1_itemLike; //そのアイテムに対する女の子１の好み値。固有。

    public int cost_price; //ショップで買うときの値段
    public int sell_price; //ショップに売るときの値段

    //トッピングスロット
    public string[] toppingtype = new string[10];

    //トッピングスロット
    public string[] koyu_toppingtype = new string[3];


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
        Chocolate_base,
        Cake,
        Cake_base,
        Jelly,
        Omochi,
        Appaleil,
        Pate,
        Komugiko,
        Egg,
        Suger,
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
        Parfe,
        IceCream
    }

    //ここでリスト化時に渡す引数をあてがいます   
    public Item(int id, string file_name, string name, string nameHyouji, string desc, int mp, int day, int quality, int rich, int sweat, int bitter, int sour, int crispy, int fluffy, int smooth, int hardness, int jiggly, int chewy, int powdery, int oily, int watery, string type, string subtype, int _girl1_like, int cost, int sell, string tp01, string tp02, string tp03, string tp04, string tp05, string tp06, string tp07, string tp08, string tp09, string tp10, string koyu_tp, int itemkosu, int extreme_kaisu)
    {
        itemID = id;
        fileName = file_name;

        itemName = name;
        itemNameHyouji = nameHyouji;
        
        //アイコンはnameとイコールにするのでアイコンがあるパス＋nameで取ってきます    
        itemIcon = Resources.Load<Texture2D>("Sprites/Items/" + fileName);
        itemDesc = desc;
        itemMP = mp;
        item_day = day;

        Quality = quality;

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

        koyu_toppingtype[0] = koyu_tp;
        koyu_toppingtype[1] = "Non";
        koyu_toppingtype[2] = "Non";

        ItemKosu = itemkosu;

        ExtremeKaisu = extreme_kaisu; //エクストリームは3回まで
    }

}
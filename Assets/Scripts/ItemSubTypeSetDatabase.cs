﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSubTypeSetDatabase : SingletonMonoBehaviour<ItemSubTypeSetDatabase>
{

    //ItemのSubTypeを入れると、どのサブに対してどの味判定を設定するかを返すメソッド

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetImageSub(string _subType) //
    {
        switch (_subType)
        {
            case "Non":
                GameMgr.Item_subcategoryText = "";
                break;
            //お菓子
            case "Biscotti":
                GameMgr.Item_subcategoryText = "ビスコッティ";
                Hardness_Text();
                break;
            case "Bread":
                GameMgr.Item_subcategoryText = "パン";
                Crispy_Text();
                break;
            case "Bread_Sliced":
                GameMgr.Item_subcategoryText = "パン";
                Crispy_Text();
                break;
            case "Cookie":
                GameMgr.Item_subcategoryText = "クッキー";
                Crispy_Text();
                break;
            /*case "Cookie_Mat":
                GameMgr.Item_subcategoryText = "クッキー";
                Crispy_Text();
                break;*/
            case "Cookie_Hard":
                GameMgr.Item_subcategoryText = "ノンシュガークッキー";
                Hardness_Text();
                break;
            case "Chocolate":
                GameMgr.Item_subcategoryText = "チョコレート";
                Smooth_Text();
                break;
            case "Chocolate_Mat":
                GameMgr.Item_subcategoryText = "チョコレート";
                Smooth_Text();
                break;
            case "Cake":
                GameMgr.Item_subcategoryText = "ケーキ";
                Fluffy_Text();
                break;
            case "Cake_Mat":
                GameMgr.Item_subcategoryText = "ケーキの素材";
                Fluffy_Text();
                break;
            case "CheeseCake":
                GameMgr.Item_subcategoryText = "チーズケーキ";
                Fluffy_Text();
                break;
            case "Castella":
                GameMgr.Item_subcategoryText = "カステラ";
                Fluffy_Text();
                break;
            case "Cannoli":
                GameMgr.Item_subcategoryText = "カンノーリ";
                Crispy_Text();
                break;
            case "Candy":
                GameMgr.Item_subcategoryText = "キャンディ";
                Hardness_Text();
                break;
            case "Crepe":
                GameMgr.Item_subcategoryText = "クレープ";
                Fluffy_Text();
                break;
            case "Crepe_Mat":
                GameMgr.Item_subcategoryText = "クレープ";
                Fluffy_Text();
                break;
            case "Creampuff":
                GameMgr.Item_subcategoryText = "シュークリーム";
                Fluffy_Text();
                break;
            case "Coffee":
                GameMgr.Item_subcategoryText = "コーヒー";
                Tea_Text();
                break;
            case "Coffee_Mat":
                GameMgr.Item_subcategoryText = "コーヒー";
                Tea_Text();
                break;
            case "Donuts":
                GameMgr.Item_subcategoryText = "ドーナツ";
                Fluffy_Text();
                break;
            case "Financier":
                GameMgr.Item_subcategoryText = "フィナンシェ";
                Fluffy_Text();
                break;
            case "IceCream":
                GameMgr.Item_subcategoryText = "アイスクリーム";
                Smooth_Text();
                break;
            case "Juice":
                GameMgr.Item_subcategoryText = "ジュース";
                Juice_Text();
                break;
            case "Jelly": //ゼリーはハードとなめらかさ
                GameMgr.Item_subcategoryText = "ゼリー";
                Jelly_Text();
                break;
            case "Maffin":
                GameMgr.Item_subcategoryText = "マフィン";
                Fluffy_Text();
                break;
            case "PanCake":
                GameMgr.Item_subcategoryText = "パンケーキ";
                Fluffy_Text();
                break;
            case "Parfe": //パフェとかはテキストは現在一つだが、３つのパラメータで判定している。
                GameMgr.Item_subcategoryText = "パフェ";
                Parfe_Text();
                break;
            case "Pie":
                GameMgr.Item_subcategoryText = "パイ";
                Crispy_Text();
                break;
            case "SumireSuger":
                GameMgr.Item_subcategoryText = "すみれ砂糖菓子";
                Tea_Text();
                break;
            case "Rusk":
                GameMgr.Item_subcategoryText = "ラスク";
                Crispy_Text();
                break;
            case "Tea":
                GameMgr.Item_subcategoryText = "お茶";
                Tea_Text();
                break;
            case "Tea_Mat":
                GameMgr.Item_subcategoryText = "お茶";
                Tea_Text();
                break;
            case "Tea_Potion":
                GameMgr.Item_subcategoryText = "お茶";
                Tea_Text();
                break;

            //材料など
            case "Fruits":
                GameMgr.Item_subcategoryText = "フルーツ";
                Etc_Text_Non();
                break;
            case "Nuts":
                GameMgr.Item_subcategoryText = "ナッツ";
                Etc_Text_Non();
                break;
            case "Source":
                GameMgr.Item_subcategoryText = "お菓子材料";
                Etc_Text();
                break;
            case "Potion":
                GameMgr.Item_subcategoryText = "お菓子材料";
                Etc_Text();
                break;
            case "Appaleil":
                GameMgr.Item_subcategoryText = "生地";
                Etc_Text();
                break;
            case "Pate":
                GameMgr.Item_subcategoryText = "生地";
                Etc_Text();
                break;
            case "Cream":
                GameMgr.Item_subcategoryText = "クリーム";
                Etc_Text();
                break;
            case "Cookie_base":
                GameMgr.Item_subcategoryText = "生地";
                break;
            case "Pie_base":
                GameMgr.Item_subcategoryText = "生地";
                break;
            case "Chocolate_base":
                GameMgr.Item_subcategoryText = "生地";
                break;
            case "Cake_base":
                GameMgr.Item_subcategoryText = "生地";
                break;
            case "Komugiko":
                GameMgr.Item_subcategoryText = "小麦粉";
                break;
            case "Suger":
                GameMgr.Item_subcategoryText = "砂糖";
                break;
            case "Butter":
                GameMgr.Item_subcategoryText = "バター";
                break;
            case "Egg":
                GameMgr.Item_subcategoryText = "たまご";
                break;
            case "Milk":
                GameMgr.Item_subcategoryText = "ミルク";
                Etc_Text_Non();
                break;
            case "Water":
                GameMgr.Item_subcategoryText = "水";
                Etc_Text_Non();
                break;
            case "Machine":
                GameMgr.Item_subcategoryText = "器具";
                break;

            default:
                // 処理３　指定がなかった場合
                GameMgr.Item_subcategoryText = "";
                break;
        }        
    }

    void Crispy_Text()
    {
        GameMgr.Item_ShokukanTypeText = "さくさく感";
        GameMgr.Item_ShokukanTypeNum = 0;
        GameMgr.Item_ShokukanTypeScoreNum = 0;
    }

    void Fluffy_Text()
    {
        GameMgr.Item_ShokukanTypeText = "ふわふわ感";
        GameMgr.Item_ShokukanTypeNum = 1;
        GameMgr.Item_ShokukanTypeScoreNum = 1;
    }

    void Smooth_Text()
    {
        GameMgr.Item_ShokukanTypeText = "なめらか感";
        GameMgr.Item_ShokukanTypeNum = 2;
        GameMgr.Item_ShokukanTypeScoreNum = 2;
    }

    void Hardness_Text()
    {
        GameMgr.Item_ShokukanTypeText = "歯ごたえ";
        GameMgr.Item_ShokukanTypeNum = 3;
        GameMgr.Item_ShokukanTypeScoreNum = 3;
    }

    void Juice_Text()
    {
        GameMgr.Item_ShokukanTypeText = "のどごし";
        GameMgr.Item_ShokukanTypeNum = 4;
        GameMgr.Item_ShokukanTypeScoreNum = 4;
    }

    void Tea_Text()
    {
        GameMgr.Item_ShokukanTypeText = "香り";
        GameMgr.Item_ShokukanTypeNum = 5;
        GameMgr.Item_ShokukanTypeScoreNum = 5;
    }

    void Jelly_Text()
    {
        GameMgr.Item_ShokukanTypeText = "歯ごたえ";
        GameMgr.Item_ShokukanTypeNum = 3;
        GameMgr.Item_ShokukanTypeScoreNum = 10;
    }

    void Parfe_Text()
    {
        GameMgr.Item_ShokukanTypeText = "なめらか感";
        GameMgr.Item_ShokukanTypeNum = 2; //カードの表記は、なめらかを表示するということ　SetImage.csで指定
        GameMgr.Item_ShokukanTypeScoreNum = 11; //判定はパフェ特有の３つの判定をするということ GirlEadJudge.csで指定
    }

    void Etc_Text()
    {
        GameMgr.Item_ShokukanTypeText = "食感";
        GameMgr.Item_ShokukanTypeNum = 90;
    }

    void Etc_Text_Non()
    {
        GameMgr.Item_ShokukanTypeText = "-";
        GameMgr.Item_ShokukanTypeNum = 99;
    }
}

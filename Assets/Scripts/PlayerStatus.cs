using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonMonoBehaviour<PlayerStatus>
{
    //主人公ステータス
    public static int player_money; // 所持金
    public static int player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。
    
    public static int player_renkin_lv; //パティシエレベル
    public static int player_renkin_exp; //パティシエ経験
    public static int player_extreme_kaisu_Max; //仕上げ可能回数
    public static int player_extreme_kaisu; //現在の仕上げ可能回数

    

    public static int player_zairyobox_lv; // 材料カゴのLV
    public static int player_zairyobox; // 材料カゴの大きさ

    public static int player_kamado_lv; //かまどのレベル

    //２からのステータス追加
    public static int player_mp;
    public static int player_maxmp;
    public static int player_default_mp; //ゲーム初期値　セーブ不要
    public static int player_patissier_lv;
    public static int player_patissier_exp; //パティシエLV用だが、LVはハートLVに依存するので、現在未使用。使ってもいい。
    public static int player_patissier_job_pt;
    public static int player_patissier_Rank;
    public static int player_ninki_param; //名声値はこれを使う。名声が上昇すると、パティシエランクが上がる。

    //妹のステータス
    //好感度のexpとlvだけは、girl1_statusに登録。
    public static int girl1_Love_exp;               //女の子の好感度値のこと。ゲーム中に、お菓子をあげることで変動する。
    public static int girl1_Love_lv;                //好感度のレベル。100ごとに１上がる。
    public static int player_girl_findpower;        //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
    public static int player_girl_findpower_def;    //デフォルト初期値。変動しない。
    public static int player_girl_lifepoint;        //妹の体力。探索のときに消費する。大きくなるほど、より遠くまで探索できる、ということ。
    public static int player_girl_maxlifepoint;     //妹の体力のMAX
    public static int player_girl_eatCount;         //妹が食べたお菓子の回数　お菓子種類に限らず総カウント数
    public static int player_girl_eatCount_tabetai; //妹が食べたいお菓子をあげた回数


    //お菓子経験値　全１５種類
    public static int player_girl_appaleil_exp;
    public static int player_girl_cream_exp;
    public static int player_girl_cookie_exp;
    public static int player_girl_chocolate_exp;
    public static int player_girl_crepe_exp;
    public static int player_girl_creampuff_exp;
    public static int player_girl_donuts_exp;
    public static int player_girl_cake_exp;
    public static int player_girl_rusk_exp;
    public static int player_girl_candy_exp;
    public static int player_girl_jelly_exp;
    public static int player_girl_juice_exp;
    public static int player_girl_tea_exp;
    public static int player_girl_icecream_exp;
    public static int player_girl_rareokashi_exp;

    public static int player_girl_appaleil_lv;
    public static int player_girl_cream_lv;
    public static int player_girl_cookie_lv;
    public static int player_girl_chocolate_lv;
    public static int player_girl_crepe_lv;
    public static int player_girl_creampuff_lv;
    public static int player_girl_donuts_lv;
    public static int player_girl_cake_lv;
    public static int player_girl_rusk_lv;
    public static int player_girl_candy_lv;
    public static int player_girl_jelly_lv;
    public static int player_girl_juice_lv;
    public static int player_girl_tea_lv;
    public static int player_girl_icecream_lv;
    public static int player_girl_rareokashi_lv;

    public static Dictionary<string, string> player_girl_okashiparam_NameList = new Dictionary<string, string>();

    public static int player_girl_okashiparam_Count;


    //セーブしない
    public static int player_girl_maxlifepoint_default;     //妹の体力のMAXデフォルト

    //エクストラモード
    public static int player_girl_manpuku;         //妹の満腹度　ハードモードで使用

    /* セーブしてない */
    //女の子の今のご機嫌状態　ハート量に応じて変化するgokigen_statusとは別。現在の機嫌。 1=最悪 2=ごきげんななめ 3=まあまあ 4=良い 5=上機嫌
    public static int player_girl_expression;
    public static int player_girl_express_param; //ご機嫌度合 0~100　で上記の５段階
  
    //**//


    //日付・フラグ関係
    public static int player_day; //現在の日付 総カウント
    public static int player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分　現在は未使用。

    public static int player_cullent_month; //現在の月（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_day; //現在の日（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_hour; //現在の時間　何時か
    public static int player_cullent_minute; //現在の時間　何分か

    public static int player_cullent_Deadmonth; //締め切りの月（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_Deadday; //締め切りの月（上記プレイヤーデイを基に、time_controllerで計算する。）

    //セーブ不要
    public static int player_contest_hour; //コンテストの現在時間　何時か
    public static int player_contest_minute; //コンテストの現在時間　何分か
    public static int player_contest_second; //コンテストの現在時間　何分か
    public static int player_contest_LimitTime; //コンテストの制限時間
    //

    public static bool First_recipi_on; //はじめて調合したフラグ
    public static bool First_extreme_on; //はじめて仕上げをしたフラグ


    // Update is called once per frame
    void Update () {
       
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Setup_PlayerStatus()
    {
        Debug.Log("Before scene loaded: Player_status");

        //プレイヤー初期設定
        player_money = 2000;

        //時間関係
        player_day = 91; //cullent_monthとcullent_dayは、左のplayer_dayをもとに、TimeControllerで計算するので、初期化不要。
        player_time = 0; //現在は未使用。

        player_cullent_hour = 8; //8:00始まり
        player_cullent_minute = 0;

        player_renkin_lv = 1;
        player_renkin_exp = 0;
        player_ninki_param = 10;
        player_kaeru_coin = 0;
        player_zairyobox = 5; //カゴの大きさ
        player_zairyobox_lv = 1;
        player_extreme_kaisu_Max = 1;
        player_extreme_kaisu = player_extreme_kaisu_Max;

        //妹のステータス初期設定
        girl1_Love_exp = 0;
        girl1_Love_lv = 1;
        player_girl_findpower_def = 100;
        player_girl_findpower = player_girl_findpower_def; //探索力
        player_girl_maxlifepoint_default = 10;
        player_girl_maxlifepoint = player_girl_maxlifepoint_default;
        player_girl_lifepoint = player_girl_maxlifepoint;
        player_girl_eatCount = 0;
        player_girl_eatCount_tabetai = 0;

        player_girl_expression = 3;
        player_girl_express_param = 50;
        player_girl_manpuku = 50;

        First_recipi_on = false;
        First_extreme_on = false;

        //お菓子経験値
        player_girl_appaleil_exp = 0;
        player_girl_cream_exp = 0;
        player_girl_cookie_exp = 0;
        player_girl_chocolate_exp = 0;
        player_girl_crepe_exp = 0;
        player_girl_creampuff_exp = 0;
        player_girl_donuts_exp = 0;
        player_girl_cake_exp = 0;
        player_girl_rusk_exp = 0;
        player_girl_candy_exp = 0;
        player_girl_jelly_exp = 0;
        player_girl_juice_exp = 0;
        player_girl_tea_exp = 0;
        player_girl_icecream_exp = 0;
        player_girl_rareokashi_exp = 0;

        player_girl_appaleil_lv = 1;
        player_girl_cream_lv = 1;
        player_girl_cookie_lv = 1;
        player_girl_chocolate_lv = 1;
        player_girl_crepe_lv = 1;
        player_girl_creampuff_lv = 1;
        player_girl_donuts_lv = 1;
        player_girl_cake_lv = 1;
        player_girl_rusk_lv = 1;
        player_girl_candy_lv = 1;
        player_girl_jelly_lv = 1;
        player_girl_juice_lv = 1;
        player_girl_tea_lv = 1;
        player_girl_icecream_lv = 1;
        player_girl_rareokashi_lv = 1;

        player_default_mp = 5;
        player_maxmp = player_default_mp;
        player_mp = player_maxmp;
        player_patissier_lv = 1;
        player_patissier_exp = 0;
        player_patissier_job_pt = 0;
        player_patissier_Rank = 1;

        InitTitleCollectionLibrary();

        player_girl_okashiparam_Count = 15;
        //セーブデータがあれば、次にそこから読み込んで、更新
    }

    //ヒカリお菓子ステータスのネームリスト
    public static void InitTitleCollectionLibrary()
    {
        player_girl_okashiparam_NameList.Clear();
        player_girl_okashiparam_NameList.Add("appaleil", "生地");
        player_girl_okashiparam_NameList.Add("cream", "クリーム");
        player_girl_okashiparam_NameList.Add("cookie", "クッキー");
        player_girl_okashiparam_NameList.Add("chocolate", "チョコレート");
        player_girl_okashiparam_NameList.Add("crepe", "クレープ");
        player_girl_okashiparam_NameList.Add("creampuff", "シュークリーム");
        player_girl_okashiparam_NameList.Add("donuts", "ドーナツ");
        player_girl_okashiparam_NameList.Add("cake", "ケーキ");
        player_girl_okashiparam_NameList.Add("rusk", "ラスク");
        player_girl_okashiparam_NameList.Add("candy", "キャンディ");
        player_girl_okashiparam_NameList.Add("jelly", "ゼリー");
        player_girl_okashiparam_NameList.Add("juice", "ジュース");
        player_girl_okashiparam_NameList.Add("tea", "ティー");
        player_girl_okashiparam_NameList.Add("icecream", "アイスクリーム");
        player_girl_okashiparam_NameList.Add("rareokashi", "レアお菓子");
    }

    //Rankの数値をもとに、ランク表記に変える。
    public static string SetPatissierRank(int _parank)
    {
        switch(_parank )
        {
            case 1:
                return "一つ星";

            case 2:
                return "二つ星";

            case 3:
                return "三つ星";

            case 4:
                return "四つ星";

            case 5:
                return "五つ星";

            default:

                return "ブラック";
        }
        
    }
}

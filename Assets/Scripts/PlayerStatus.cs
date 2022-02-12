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

    public static int player_ninki_param; //人気度。いるかな？とりあえず置き

    public static int player_zairyobox_lv; // 材料カゴのLV
    public static int player_zairyobox; // 材料カゴの大きさ

    public static int player_kamado_lv; //かまどのレベル


    //妹のステータス
    //好感度のexpとlvだけは、girl1_statusに登録。
    public static int girl1_Love_exp;               //女の子の好感度値のこと。ゲーム中に、お菓子をあげることで変動する。
    public static int girl1_Love_lv;                //好感度のレベル。100ごとに１上がる。
    public static int player_girl_findpower;        //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
    public static int player_girl_findpower_def;    //デフォルト初期値。変動しない。
    public static int player_girl_lifepoint;        //妹の体力。探索のときに消費する。大きくなるほど、より遠くまで探索できる、ということ。
    public static int player_girl_maxlifepoint;     //妹の体力のMAX
    public static int player_girl_eatCount;         //妹が食べたお菓子の回数

    /* まだセーブしてない */
    //女の子の今のご機嫌状態　ハート量に応じて変化するgokigen_statusとは別。現在の機嫌。 1=最悪 2=ごきげんななめ 3=まあまあ 4=良い 5=上機嫌
    public static int player_girl_expression;
    public static int player_girl_express_param; //ご機嫌度合 0~100　で上記の５段階

    public static int player_girl_manpuku;         //妹の満腹度　ハードモードで使用
    //**//


    //日付・フラグ関係
    public static int player_day; //現在の日付
    public static int player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分　現在は未使用。

    public static int player_cullent_month; //現在の月（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_day; //現在の日（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_hour; //現在の時間　何時か
    public static int player_cullent_minute; //現在の時間　何分か

    public static int player_cullent_Deadmonth; //締め切りの月（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_Deadday; //締め切りの月（上記プレイヤーデイを基に、time_controllerで計算する。）

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
        player_girl_maxlifepoint = 10;
        player_girl_lifepoint = player_girl_maxlifepoint;
        player_girl_eatCount = 0;

        player_girl_expression = 3;
        player_girl_express_param = 50;
        player_girl_manpuku = 50;

        First_recipi_on = false;
        First_extreme_on = false;

        //セーブデータがあれば、次にそこから読み込んで、更新
    }
}

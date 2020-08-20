using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonMonoBehaviour<PlayerStatus>
{
    //主人公ステータス
    public static int player_money; // 所持金
    public static int player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。
    
    public static int player_renkin_lv; //錬金レベル
    public static int player_renkin_exp; //錬金経験

    public static int player_ninki_param; //人気度。いるかな？とりあえず置き

    public static int player_zairyobox; // 材料カゴの大きさ


    //妹のステータス
    public static int player_girl_findpower; //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。


    //日付・フラグ関係
    public static int player_day; //現在の日付
    public static int player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

    public static int player_cullent_month; //現在の月（上記プレイヤーデイを基に、time_controllerで計算する。）
    public static int player_cullent_day; //現在の日（上記プレイヤーデイを基に、time_controllerで計算する。）

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
        player_day = 91;
        player_time = 0; //8:00始まり
        player_renkin_lv = 1;
        player_renkin_exp = 0;
        player_ninki_param = 10;
        player_kaeru_coin = 0;
        player_zairyobox = 10;

        //妹のステータス初期設定
        player_girl_findpower = 100; //探索力

        First_recipi_on = false;
        First_extreme_on = false;

        //セーブデータがあれば、次にそこから読み込んで、更新
    }
}

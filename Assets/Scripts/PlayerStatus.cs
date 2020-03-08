using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonMonoBehaviour<PlayerStatus>
{

    public static int player_money; // 所持金
    public static int player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。
    public static int player_day; //現在の日付
    public static int player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

    public static int player_renkin_lv; //錬金レベル
    public static int player_renkin_exp; //錬金経験

    public static int player_ninki_param; //人気度。いるかな？とりあえず置き

    public static int player_zairyobox; // 材料カゴの大きさ

    public static bool First_recipi_on;

    public static List<bool> player_travelList = new List<bool>(); //旅行先。行ける場所が増えると、カウントも増える。


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

        First_recipi_on = false;

        //セーブデータがあれば、次にそこから読み込んで、更新
    }
}

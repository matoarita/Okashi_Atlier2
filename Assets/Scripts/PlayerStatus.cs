﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonMonoBehaviour<PlayerStatus>
{

    public static int player_money; // 所持金
    public static int player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。
    public static int player_day; //残り日数　ちょっとまだ考え中.. 一日なのか、○○時間なのか

    public static int player_renkin_lv; //錬金レベル
    public static int player_renkin_exp; //錬金経験

    public static int player_ninki_param; //人気度。いるかな？とりあえず置き

    public static List<bool> player_travelList = new List<bool>(); //旅行先。行ける場所が増えると、カウントも増える。
	
	// Update is called once per frame
	void Update () {
        
        //各ステータスの上限
        if( player_money > 999999 )
        {
            player_money = 999999;
        }

        //お金が0になってしまった場合は、ゲームオーバー？
        if ( player_money <= 0 )
        {
            player_money = 0;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Setup_PlayerStatus()
    {
        Debug.Log("Before scene loaded: Player_status");

        //プレイヤー初期設定
        player_money = 2000;
        player_day = 91;
        player_renkin_lv = 1;
        player_renkin_exp = 0;
        player_ninki_param = 10;
        player_kaeru_coin = 0;

        player_travelList.Add(true); //一つ目、「近くの森」解禁。これは初期設定。
        //player_travelList.Add(true); //二つ目
        //player_travelList.Add(true); //三つ目

        //セーブデータがあれば、次にそこから読み込んで、更新
    }
}

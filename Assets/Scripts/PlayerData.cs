using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//データセーブ用。パラメータを保存するクラス。

[System.Serializable]
public class PlayerData
{
    //主人公ステータス
    //public string name;
    
    public int save_player_money; // 所持金
    public int save_player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。

    public int save_player_renkin_lv; //錬金レベル
    public int save_player_renkin_exp; //錬金経験

    public int save_player_ninki_param; //人気度。いるかな？とりあえず置き
    public int save_player_zairyobox; // 材料カゴの大きさ


    //妹のステータス
    public int save_player_girl_findpower; //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
    public int save_girl_love_exp; //妹の好感度
    public int save_girl_love_lv; //妹の好感度レベル


    //日付・フラグ関係
    public int save_player_day; //現在の日付
    public int save_player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

    public bool save_First_recipi_on; //はじめて調合したフラグ
    public bool save_First_extreme_on; //はじめて仕上げをしたフラグ

    //ステージ番号
    public int save_stage_number;
    
    //イベントフラグ
    public int save_GirlLoveEvent_num;
    public bool[] save_GirlLoveEvent_stage1 = new bool[GameMgr.GirlLoveEvent_stage1.Length];  //各イベントの、現在読み中かどうかのフラグ。
    public bool[] save_GirlLoveEvent_stage2 = new bool[GameMgr.GirlLoveEvent_stage2.Length];
    public bool[] save_GirlLoveEvent_stage3 = new bool[GameMgr.GirlLoveEvent_stage3.Length];

    //お菓子クエストフラグ
    public bool[] save_OkashiQuest_flag_stage1 = new bool[GameMgr.OkashiQuest_flag_stage1.Length]; //各SPイベントのクリアしたかどうかのフラグ。
    public bool[] save_OkashiQuest_flag_stage2 = new bool[GameMgr.OkashiQuest_flag_stage2.Length];
    public bool[] save_OkashiQuest_flag_stage3 = new bool[GameMgr.OkashiQuest_flag_stage3.Length];

    //マップイベントフラグ
    public bool[] save_MapEvent_01;         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
    public bool[] save_MapEvent_02;         //井戸。
    public bool[] save_MapEvent_03;         //ストロベリーガーデン
    public bool[] save_MapEvent_04;         //ひまわりの丘
    public bool[] save_MapEvent_05;

    //広場でのイベント
    public bool[] save_hiroba_event_end = new bool[GameMgr.hiroba_event_end.Length]; //イベントを読み終えたかどうかを保存するフラグ。配列順は適当。

    public override string ToString()
    {
        return $"{ base.ToString() } { JsonUtility.ToJson(this) }";
    }
}
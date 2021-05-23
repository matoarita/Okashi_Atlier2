using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//データセーブ用。パラメータを保存するクラス。

[System.Serializable]
public class PlayerData
{
    //主人公ステータス
    
    public int save_player_money; // 所持金
    public int save_player_kaeru_coin; //かえるコインの所持数。危ないお店などで使える。

    public int save_player_renkin_lv; //パティシエレベル
    public int save_player_renkin_exp; //パティシエ経験
    public int save_player_extreme_kaisu_Max; //仕上げ可能回数
    public int save_player_extreme_kaisu; //現在の仕上げ可能回数


    public int save_player_ninki_param; //人気度。いるかな？とりあえず置き
    public int save_player_zairyobox; // 材料カゴの大きさ
    public int save_player_zairyobox_lv; // 材料カゴのLV


    //妹のステータス
    public int save_player_girl_findpower; //妹のアイテム発見力。高いと、マップの隠し場所を発見できたりする。
    public int save_girl_love_exp; //妹の好感度
    public int save_girl_love_lv; //妹の好感度レベル
    public int save_player_girl_lifepoint; //妹の体力
    public int save_player_girl_maxlifepoint; //妹のMAX体力


    //日付・フラグ関係
    public int save_player_day; //現在の日付
    public int save_player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分

    public bool save_First_recipi_on; //はじめて調合したフラグ
    public bool save_First_extreme_on; //はじめて仕上げをしたフラグ

    public bool save_special_animatFirst; //SPクエスト最初表示したかどうかのフラグ

    //ステージ番号
    public int save_stage_number;

    //シナリオの進み具合
    public int save_scenario_flag;

    //セーブしたかどうかを保存するフラグ
    public bool save_saveOK = false;

    //初期アイテム取得フラグ
    public bool save_gamestart_recipi_get;

    //クエスト以外で、クリアするのに必要なハート量
    public int save_stageclear_love; //そのクエストをクリアするのに、必要なハート数。クエストで食べたいお菓子とは別に、ある程度新しいお菓子をあげても、クリアできる、という仕様
    public int save_stageclear_cullentlove; //クエストをクリアするのに、必要なハートの蓄積量。

    //コスチューム番号
    public int save_costume_num;
    public int[] save_acce_num = new int[GameMgr.Accesory_Num.Length];

    //飾っているアイテムのリスト
    public bool[] save_DecoItems = new bool[GameMgr.DecoItems.Length];

    //コレクションに登録したアイテムのリスト
    public List<bool> save_CollectionItems = new List<bool>();

    //エンディングカウント
    public int save_ending_count;

    //イベントフラグ
    public int save_GirlLoveEvent_num;
    public bool[] save_GirlLoveEvent_stage1 = new bool[GameMgr.GirlLoveEvent_stage1.Length];  //各イベントの、現在読み中かどうかのフラグ。
    public bool[] save_GirlLoveEvent_stage2 = new bool[GameMgr.GirlLoveEvent_stage2.Length];
    public bool[] save_GirlLoveEvent_stage3 = new bool[GameMgr.GirlLoveEvent_stage3.Length];

    //サブイベントフラグ
    public bool[] save_GirlLoveSubEvent_stage1 = new bool[GameMgr.GirlLoveEvent_stage1.Length];

    //ビギナーフラグ
    public bool[] save_Beginner_flag = new bool[GameMgr.Beginner_flag.Length];

    //マップイベントフラグ
    public bool[] save_MapEvent_01;         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
    public bool[] save_MapEvent_02;         //井戸。
    public bool[] save_MapEvent_03;         //ストロベリーガーデン
    public bool[] save_MapEvent_04;         //ひまわりの丘
    public bool[] save_MapEvent_05;         //ラベンダー
    public bool[] save_MapEvent_06;         //バードサンクチュアリ

    //広場でのイベント
    public bool[] save_hiroba_event_end = new bool[GameMgr.hiroba_event_end.Length]; //イベントを読み終えたかどうかを保存するフラグ。配列順は適当。

    //お菓子クエストフラグ
    public bool[] save_OkashiQuest_flag_stage1 = new bool[GameMgr.OkashiQuest_flag_stage1.Length]; //各SPイベントのクリアしたかどうかのフラグ。
    public bool[] save_OkashiQuest_flag_stage2 = new bool[GameMgr.OkashiQuest_flag_stage2.Length];
    public bool[] save_OkashiQuest_flag_stage3 = new bool[GameMgr.OkashiQuest_flag_stage3.Length];

    //現在のクエストのクリアフラグ
    public bool save_QuestClearflag;
    public bool save_QuestClearButton_anim; //クエストクリア演出が発生したか否か

    //さっき食べたお菓子の情報
    public string save_Okashi_lasthint; //さっき食べたお菓子のヒント。
    public string save_Okashi_lastname; //さっき食べたお菓子の名前。
    public string save_Okashi_lastslot; //さっき食べたお菓子のスロット。
    public int save_Okashi_lastID; //さっき食べたお菓子のアイテムID
    public int save_Okashi_totalscore; //さっき食べたお菓子の点数
    public int save_Okashi_lastshokukan_param; //さっき食べたお菓子のパラメータ
    public string save_Okashi_lastshokukan_mes; //さっき食べたお菓子のパラメータ
    public int save_Okashi_lastsweat_param; //さっき食べたお菓子のパラメータ
    public int save_Okashi_lastsour_param; //さっき食べたお菓子のパラメータ
    public int save_Okashi_lastbitter_param; 

    //ステージ１クリア時の好感度を保存
    public int save_stage1_girl1_loveexp;
    public int save_stage2_girl1_loveexp;
    public int save_stage3_girl1_loveexp;

    public int save_stage1_clear_love;
    public int save_stage2_clear_love;
    public int save_stage3_clear_love;

    //ショップのイベントリスト
    public bool[] save_ShopEvent_stage = new bool[GameMgr.ShopEvent_stage.Length];
    public bool[] save_ShopLvEvent_stage = new bool[GameMgr.ShopLVEvent_stage.Length];

    //ショップの在庫
    public List<int> save_shopzaiko = new List<int>();
    public List<int> save_farmzaiko = new List<int>();
    public List<int> save_emeraldshop_zaiko = new List<int>();

    //酒場のイベントリスト
    public bool[] save_BarEvent_stage = new bool[GameMgr.BarEvent_stage.Length];

    //ショップのうわさ話リスト
    public bool[] save_ShopUwasa_stage1 = new bool[GameMgr.ShopUwasa_stage1.Length];

    //コンテストのイベントリスト
    public bool[] save_ContestEvent_stage = new bool[GameMgr.ContestEvent_stage.Length];

    //コンテスト審査員の点数
    public int[] save_contest_Score = new int[GameMgr.contest_Score.Length];
    public int save_contest_TotalScore;

    //牧場のイベントリスト
    public bool[] save_FarmEvent_stage = new bool[GameMgr.FarmEvent_stage.Length];

    //エメラルドアイテムイベントリスト
    public bool[] save_emeraldShopEvent_stage = new bool[GameMgr.emeraldShopEvent_stage.Length];

    //アイテムリスト<デフォルト> 所持数のみのリスト
    public List<int> save_playeritemlist = new List<int>();

    //プレイヤーのイベントアイテムリスト。
    public List<ItemEvent> save_eventitemlist = new List<ItemEvent>();

    //プレイヤーが作成したオリジナルのアイテムリスト。
    public List<Item> save_player_originalitemlist = new List<Item>();

    //プレイヤーのエメラルドアイテムリスト。
    public List<ItemEvent> save_player_emeralditemlist = new List<ItemEvent>();

    //アイテムの前回スコアなどを記録する
    public List<ItemSaveparam> save_itemdatabase = new List<ItemSaveparam>();

    //調合のフラグ＋調合回数を記録する
    public List<ItemCompound> save_itemCompodatabase = new List<ItemCompound>();

    //今うけてるクエストを保存する。
    public List<QuestSet> save_questTakeset = new List<QuestSet>();

    //マップのフラグリスト
    public List<int> save_mapflaglist = new List<int>();

    //エクストリームパネル用のアイテムとタイプ
    public int save_extreme_itemid;
    public int save_extreme_itemtype;

    //お菓子の一度にトッピングできる回数
    public int save_topping_Set_Count;

    //音設定データ
    public float save_masterVolumeparam;
    public float save_BGMVolumeParam;
    public float save_SeVolumeParam;

    public int save_mainBGM_Num;

    public override string ToString()
    {
        return $"{ base.ToString() } { JsonUtility.ToJson(this) }";
    }
}
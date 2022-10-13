using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//データセーブ用。パラメータを保存するクラス。

[System.Serializable]
public class PlayerData
{
    //主人公ステータス
    
    public int save_player_money; // 所持金
    public int save_player_money_system; // 所持金　システム引継ぎ用
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
    public int save_player_girl_maxlifepoint_system; //妹のMAX体力 システム引継ぎ用
    public int save_player_girl_eatCount; //妹が食べたお菓子の回数
    public int save_player_girl_eatCount_tabetai; //妹が食べたいお菓子をあげた回数
    public int save_player_girl_manpuku; //妹の満腹度
    public int save_player_girl_yaruki; //妹のやる気

    //お菓子経験値　全１５種類
    public int save_player_girl_appaleil_exp;
    public int save_player_girl_cream_exp;
    public int save_player_girl_cookie_exp;
    public int save_player_girl_chocolate_exp;
    public int save_player_girl_crepe_exp;
    public int save_player_girl_creampuff_exp;
    public int save_player_girl_donuts_exp;
    public int save_player_girl_cake_exp;
    public int save_player_girl_rusk_exp;
    public int save_player_girl_candy_exp;
    public int save_player_girl_jelly_exp;
    public int save_player_girl_juice_exp;
    public int save_player_girl_tea_exp;
    public int save_player_girl_icecream_exp;
    public int save_player_girl_rareokashi_exp;

    public int save_player_girl_appaleil_lv;
    public int save_player_girl_cream_lv;
    public int save_player_girl_cookie_lv;
    public int save_player_girl_chocolate_lv;
    public int save_player_girl_crepe_lv;
    public int save_player_girl_creampuff_lv;
    public int save_player_girl_donuts_lv;
    public int save_player_girl_cake_lv;
    public int save_player_girl_rusk_lv;
    public int save_player_girl_candy_lv;
    public int save_player_girl_jelly_lv;
    public int save_player_girl_juice_lv;
    public int save_player_girl_tea_lv;
    public int save_player_girl_icecream_lv;
    public int save_player_girl_rareokashi_lv;

    //日付・フラグ関係
    public int save_player_day; //現在の日付
    //public int save_player_time; //現在の時刻　8:00~24:00まで　10分刻み　トータルで96*10分
    public int save_player_cullent_hour; //現在の時間
    public int save_player_cullent_minute; //現在の分

    public bool save_First_recipi_on; //はじめて調合したフラグ
    public bool save_First_extreme_on; //はじめて仕上げをしたフラグ

    public bool save_special_animatFirst; //SPクエスト最初表示したかどうかのフラグ

    //ステージ番号
    public int save_stage_number;
    public int save_stage_quest_num;
    public int save_stage_quest_num_sub;

    //シナリオの進み具合
    public int save_scenario_flag;

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

    //イベントフラグ
    public int save_GirlLoveEvent_num;
    public bool[] save_GirlLoveEvent_stage1 = new bool[GameMgr.GirlLoveEvent_stage1.Length];  //各イベントの、現在読み中かどうかのフラグ。
    public bool[] save_GirlLoveEvent_stage2 = new bool[GameMgr.GirlLoveEvent_stage2.Length];
    public bool[] save_GirlLoveEvent_stage3 = new bool[GameMgr.GirlLoveEvent_stage3.Length];

    //サブイベントフラグ
    public bool[] save_GirlLoveSubEvent_stage1 = new bool[GameMgr.GirlLoveEvent_stage1.Length];
    public bool[] save_GirlLoveSubEvent_stage1_system = new bool[GameMgr.GirlLoveEvent_stage1.Length]; //衣装などのイベントは、周回しても発生しないように、システムにもセーブする

    //好感度ハイスコアイベントの取得フラグ
    public bool[] save_OkashiQuestHighScore_event = new bool[GameMgr.GirlLoveEvent_stage1.Length];

    //ビギナーフラグ
    public bool[] save_Beginner_flag = new bool[GameMgr.Beginner_flag.Length];

    //マップイベントフラグ
    public bool[] save_MapEvent_01;         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
    public bool[] save_MapEvent_02;         //井戸。
    public bool[] save_MapEvent_03;         //ストロベリーガーデン
    public bool[] save_MapEvent_04;         //ひまわりの丘
    public bool[] save_MapEvent_05;         //ラベンダー
    public bool[] save_MapEvent_06;         //バードサンクチュアリ
    public bool[] save_MapEvent_07;         //ベリーファーム
    public bool[] save_MapEvent_08;         //白猫のおはか

    //広場でのイベント
    public bool[] save_hiroba_event_end = new bool[GameMgr.hiroba_event_end.Length]; //イベントを読み終えたかどうかを保存するフラグ。配列順は適当。

    //お菓子クエストフラグ
    public bool[] save_OkashiQuest_flag_stage1 = new bool[GameMgr.OkashiQuest_flag_stage1.Length]; //各SPイベントのクリアしたかどうかのフラグ。
    public bool[] save_OkashiQuest_flag_stage2 = new bool[GameMgr.OkashiQuest_flag_stage2.Length];
    public bool[] save_OkashiQuest_flag_stage3 = new bool[GameMgr.OkashiQuest_flag_stage3.Length];

    //現在のクエストのクリアフラグ
    public bool save_QuestClearflag;
    public bool save_QuestClearButton_anim; //クエストクリア演出が発生したか否か

    //ヒカリのお菓子作り系フラグ
    public int[] save_hikari_kettei_item = new int[GameMgr.hikari_kettei_item.Length];
    public int[] save_hikari_kettei_kosu = new int[GameMgr.hikari_kettei_kosu.Length];
    public int[] save_hikari_kettei_toggleType = new int[GameMgr.hikari_kettei_toggleType.Length];
    public string[] save_hikari_kettei_itemName = new string[GameMgr.hikari_kettei_itemName.Length];
    public bool save_hikari_make_okashiFlag; //ヒカリがお菓子を制作中かどうかのフラグ
    public bool save_hikari_makeokashi_startflag;
    public int save_hikari_make_okashiID;
    public int save_hikari_make_okashi_compID; //CompoDBのID
    public int save_hikari_make_okashiTimeCost; //かかる時間
    public int save_hikari_make_okashiTimeCounter; //制作時間のタイマー
    public float save_hikari_make_success_rate; //成功率
    public int save_hikari_make_doubleItemCreated;
    public float save_hikari_make_okashi_totalkyori;
    public int save_hikari_make_okashiKosu;
    public int save_hikari_make_success_count;
    public int save_hikari_make_failed_count;

    //クリアお菓子の情報
    public int save_SpecialQuestClear_okashiItemID;

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
    public int save_Okashi_quest_bunki_on; //条件分岐しているか否かのフラグ
    public bool save_high_score_flag; //高得点でクリアしたというフラグ。
    public int save_Okashi_lasttotalscore; //前回食べたお菓子の点数。メモ保存用。

    public int save_Okashi_toplast_score; //前回あげた最高得点
    public int save_Okashi_toplast_heart; //前回あげたときの最高ハート取得量

    public int save_Okashi_spquest_eatkaisu; //そのクエスト内で、お菓子を食べた回数をカウント
    public int save_Okashi_spquest_MaxScore; //そのクエスト内で、お菓子の最高得点
    public bool save_Okashi_Extra_SpEvent_Start; //ハート系クエストで、食べたお菓子が一定回数以下のとき、発動するクエスト

    public string save_NowEatOkashiName; //今食べたいお菓子の名前表示
    public int save_NowEatOkashiID; //今食べたいお菓子ID表示

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
    public List<ItemSaveKosu> save_shopzaiko = new List<ItemSaveKosu>();
    public List<ItemSaveKosu> save_farmzaiko = new List<ItemSaveKosu>();
    public List<ItemSaveKosu> save_emeraldshop_zaiko = new List<ItemSaveKosu>();

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

    //アイテムリスト<デフォルト> アイテム名＋所持数のみのリスト
    public List<ItemSaveKosu> save_playeritemlist = new List<ItemSaveKosu>();

    //アイテムリスト　どんぐりと装備品セーブ用
    public List<ItemSaveKosu> save_dongurilist = new List<ItemSaveKosu>();

    //プレイヤーのイベントアイテムリスト。
    public List<ItemSaveKosu> save_eventitemlist = new List<ItemSaveKosu>();

    //プレイヤーのエメラルドアイテムリスト。
    public List<ItemSaveKosu> save_player_emeralditemlist = new List<ItemSaveKosu>();

    //プレイヤーが作成したオリジナルのアイテムリスト。
    public List<Item> save_player_originalitemlist = new List<Item>();

    //プレイヤーが作成したオリジナルの予測アイテムリスト。
    public List<Item> save_player_yosokuitemlist = new List<Item>();

    //お菓子パネルのアイテムリスト。
    public List<Item> save_player_extremepanel_itemlist = new List<Item>();

    //アイテムの前回スコアなどを記録する
    public List<ItemSaveparam> save_itemdatabase = new List<ItemSaveparam>();

    //調合のフラグ＋調合回数を記録する
    public List<ItemSaveCompoFlag> save_itemCompodatabase = new List<ItemSaveCompoFlag>();

    //今うけてるクエストを保存する。
    public List<QuestSet> save_questTakeset = new List<QuestSet>();

    //マップのフラグリスト
    public List<ItemSaveKosu> save_mapflaglist = new List<ItemSaveKosu>();

    //称号リスト
    public List<ItemSaveFlag> save_titlecollectionlist = new List<ItemSaveFlag>();

    //イベントリスト
    public List<ItemSaveFlag> save_event_collection_list = new List<ItemSaveFlag>();

    //コンテストクリアお菓子リスト
    public List<ItemSaveFlag> save_contestclear_collection_list = new List<ItemSaveFlag>();
    public List<Item> save_contestclear_collection_listItemData = new List<Item>();

    //獲得音楽図鑑のフラグ
    public List<ItemSaveFlag> save_bgm_collection_list = new List<ItemSaveFlag>(); //音楽リスト。 

    //エクストリームパネル用のアイテムとタイプ
    public int save_extreme_itemid;
    public int save_extreme_itemtype;

    //お菓子の一度にトッピングできる回数
    public int save_topping_Set_Count;

    //ピクニック系
    public bool save_picnic_End;
    public int save_picnic_count;
    public bool save_picnic_event_ON;

    //外出系
    public bool outgirl_End;
    public int outgirl_count;
    public bool outgirl_event_ON;
    public bool outgirl_Nowprogress;

    public bool save_hiroba_ichigo_first;
    public bool[] save_ichigo_collection_listFlag = new bool[GameMgr.ichigo_collection_listFlag.Length];

    //音設定データ
    public float save_masterVolumeparam;
    public float save_BGMVolumeParam;
    public float save_SeVolumeParam;   

    public int save_mainBGM_Num;
    public int save_userBGM_Num;

    //ゲームスピード
    public int save_GameSpeedParam;

    //システムデータ関係

    //セーブしたかどうかを保存するフラグ
    public bool save_saveOK = false;

    //オートセーブフラグ
    public bool save_Autosave_ON = false;

    //調合シーンでBGM切り替えるフラグ
    public bool save_CompoBGMChange_ON = true;

    //エンディングカウント
    public int save_ending_count;

    //ストーリーモード
    public int save_Story_Mode;

    //ゲームバージョン情報
    public float save_GameVersion;
    public string save_GameSaveDaytime;

    public override string ToString()
    {
        return $"{ base.ToString() } { JsonUtility.ToJson(this) }";
    }
}
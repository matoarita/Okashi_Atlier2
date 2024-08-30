using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : SingletonMonoBehaviour<GameMgr>
{
    //↑シングルトンにすることで、ゲーム中、GameManagerオブジェクトは、必ず一つのみになる。
    //DontDestroyOnLoad（シーン間で移動してもオブジェクトが削除されない）と併用することで、シーンをまたいでも、常にそのゲームオブジェクトは生き残る。

    //
    // ** -- イベント数をセッティング -- ** //
    //
    public static int GirlLoveEvent_stage_num = 100;
    public static int GirlLoveSubEvent_stage_num = 1000;
    public static int Event_num = 30;
    public static int Uwasa_num = 100;
    public static int NpcEvent_stage_num = 3000;
    public static int NpcEvent_people_num = 300;
    public static int OrEvent_num = 1000;

    //** --ここまで-- **//

    // ** -- デフォルトの設定 -- ** //

    // ゲーム開始前に呼び出す。デバッグログをオフにする。
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Debug.unityLogger.logEnabled = true; // ←falseでログを止める
    }

    public static bool DEBUG_MODE = false; //デバッグモード　falseだと、デバッグパネルの表示をデフォルトでオフにする。
    public static bool DEBUG_MagicPlayTime_ON = false; //デバッグ　魔法の演出時間を表示する。
    public static bool DEBUG_TasteSPScore_ON = true; //デバッグ　味のSPスコアなども表示する これがfalseでも、デバッグモードがONになると表示される
    public static bool RESULTPANEL_ON = true; //ED後、リザルトを表示するか否か。 
    public static bool System_REALTIMEMODE_ON = false; //リアルタイムに時間を進める。
    public static bool WEATHER_TIMEMODE_ON = false; //時間によって朝・昼・夜の背景を変更するかどうか。   

    //各システムの使用の有無
    public static bool System_Manpuku_ON = false; //エクストラ　満腹度ONOFF。trueだと、ONにする。
    public static bool System_ExtraResult_ON = false; //エクストラ　道中クエストのリザルト画面とご褒美画面をONにする。
    public static bool System_ExtraStageClearResult_ON = false; //エクストラ　ステージクリア時にリザルト画面とご褒美画面をONにする。
    public static bool System_GameOver_ON = false; //エクストラ　ゲームオーバーのONOFF
    public static bool System_HikariMake_OnichanTimeCost_ON = true; //エクストラ　おにいちゃんがお菓子作ったときの時間を、ヒカリのお菓子作り時間に反映するかどうか
    public static bool System_Contest_RealTimeProgress_ON = true; //コンテスト中に時間をリアルタイムに経過するかどうか　現状の仕様はON
    public static bool System_BarQuest_LimitDayON = true; //酒場クエストの締め切り日を有効にする。falseでオフ。締め切りがなくなる。
    public static bool System_Shiokuri_ON = false; //仕送りの有無
    public static bool System_Yachin_ON = true; //家賃システムの有無
    public static bool System_Contest_StartNow = false; //コンテストすぐ開始するか、〇日後に開始するかの切り替え　Falseで〇日後　〇日後の場合、Excelで日付指定も必要
    public static bool System_SpecialOkashiEnshutu_ON = true; //特別なお菓子作ったときに演出を表示するかどうか。

    public static bool System_DebugItemSet_ON = false; //デバッグ用　コンテストのデータやアイテムや魔法などを最初からセットする　最終的にはオフにすること
    public static bool System_DebugAreaKaikin_ON = false; //デバッグ用　進めないエリアの→などを全て表示する。

    public static float System_default_sceneFadeBGMTime = 0.5f; //デフォルトのBGMのフェード時間

    //シーン移動の際の切り替え時間
    public static float SceneFadeTime = 0.3f;

    //各ハートレベル・スターのブロック(スターは一旦保留）
    public static int System_HeartBlockLv_01 = 5; //秘密の花園
    public static int System_HeartBlockLv_50 = 37; //冬
    public static int System_HeartBlockLv_51 = 28; //秋
    public static int System_HeartBlockLv_52 = 15; //夏
    public static int System_HeartBlockLv_53 = 45;

    public static int System_StarBlockLv_01 = 30;
    public static int System_StarBlockLv_02 = 60;
    public static int System_StarBlockLv_03 = 90;
    public static int System_StarBlockLv_04 = 130;

    public static int System_Yachin_Cost01 = 1000; //家賃の額 月始めバージョン
    public static int System_Yachin_Cost02 = 500; //10日ごとバージョン

    //見た目点数の基準点
    public static int System_Beauty_BasicScore = 30; //見た目得点の基準　これをもとに、倍率をかけて実際の見た目得点になる


    //重要アイテム名
    public static string System_TreasureItem01 = "ブルージェム";

    //温度の最小・最大
    public static int System_tempature_control_tempMin = 150;
    public static int System_tempature_control_tempMax = 230;

    //魔法の演出時間
    public static float System_magic_playtime_default = 2.0f;
    public static float System_magic_playtime_def01 = 3.0f;

    //** --ここまで-- **//


    public static bool scenario_ON;     //全シーンで共通。宴・シナリオを優先するフラグ。これがONのときは、調合シーンなどでも、宴の表示をまず優先する。宴を読み終えたらOFFにする。
    public static int scenario_flag_input;     //デバッグ用。シナリオフラグをインスペクタから入力
    public static int scenario_flag_cullent;   //デバッグ用。現在のシナリオフラグを確認用

    public static bool scenario_read_endflag; //シナリオ（メインなどのイベント用）を読み終えたフラグ

    public static bool event_recipi_flag;   //イベントレシピを見たときに、宴を表示する用のフラグ
    public static int event_recipiID;       //その時のイベント番号
    public static bool event_recipi_endflag; //レシピを読み終えたときのフラグ

    public static bool recipi_read_flag;    //入手したレシピを読むときの、宴を表示する用のフラグ
    public static bool itemuse_recipi_flag;    //レシピリストから選択したときの、宴を表示する用のフラグ
    public static int recipi_read_ID;       //その時のイベント番号
    public static bool recipi_read_endflag; //レシピを読み終えたときのフラグ

    public static int stageclear_love; //そのクエストをクリアするのに、必要なハート数。クエストで食べたいお菓子とは別に、ある程度新しいお菓子をあげても、クリアできる、という仕様


    /* セーブする */

    public static int scenario_flag;    //全シーンで共通。今、どのシナリオまできているか。クエストやステージではなく、ゲーム自体の進行度を表す。
    public static int ending_count;     //エンディングを迎えた回数
    public static bool bestend_on_flag; //ベストED　Aを一度でも迎えたことがあるフラグ
    public static int stage_number;     //ステージ番号　stage1 stage2のこと
    public static int stage_quest_num; //メインのクエスト番号
    public static int stage_quest_num_sub; //クエスト番号
    public static int Story_Mode; //0が本編。1が、フリーモード（強くてニューゲーム）。
    public static string Scene_Name; //その場所の固有名　主にセーブした場所を記録する。

    //コマンドの解禁フラグ
    public static bool System_MagicUse_Flag; //魔法の解禁フラグ
    public static bool System_HikariMakeUse_Flag; //ヒカリがお菓子作る解禁フラグ

    //セーブしたかどうかを保存しておくフラグ
    public static bool saveOK;

    //オートセーブのON/OFF
    public static bool AUTOSAVE_ON = false; //シーンからメインに戻ってきたときや、採取から帰ってきたときにオートセーブするかどうか

    //調合シーンでBGM切り替えるかどうかのフラグ
    public static bool CompoBGMCHANGE_ON = false;    

    //初期アイテム取得のフラグ
    public static bool gamestart_recipi_get;

    //現在着ているコスチュームの番号
    public static int Costume_Num;
    public static int[] Accesory_Num = new int[6]; //アクセ番号 現在アクセ数６個

    //飾っているアイテムのリスト
    //public static bool[] DecoItems = new bool[30];
    public static Dictionary<string, bool> BGAcceItemsName = new Dictionary<string, bool>(); //背景の置物のリスト。

    //コレクションに登録したアイテムのリスト
    public static List<bool> CollectionItems = new List<bool>(); //登録済みか否か。こっちはセーブ必要。
    public static List<string> CollectionItemsName = new List<string>(); //登録済みか否か。こっちはセーブ不要。    

    //現在覚えているレシピの数と達成率。調合成功率アップのパーセント
    public static int game_Cullent_recipi_count;
    public static int game_All_recipi_count;
    public static float game_Recipi_archivement_rate;
    public static int game_Exup_rate;

    //イベントフラグ
    public static int GirlLoveEvent_num;            //女の子の好感度に応じて発生するイベントの、イベント番号   
    public static bool[] GirlLoveEvent_stage1 = new bool[GirlLoveEvent_stage_num];  //各イベントの、現在読み中かどうかのフラグ。
    public static bool[] GirlLoveEvent_stage2 = new bool[GirlLoveEvent_stage_num];
    public static bool[] GirlLoveEvent_stage3 = new bool[GirlLoveEvent_stage_num];   

    //クリア時の好感度
    public static int stage1_clear_girl1_loveexp; //ステージ１クリア時の好感度を保存
    public static int stage2_clear_girl1_loveexp;
    public static int stage3_clear_girl1_loveexp;

    public static int stage1_clear_girl1_lovelv; //クリア時の好感度LV
    public static int stage2_clear_girl1_lovelv;
    public static int stage3_clear_girl1_lovelv;    

    //クッキーをはじめて作ったかどうか、などのゲーム序盤に○○の処理をしたかどうかを判定するイベントフラグ   
    public static bool[] Beginner_flag = new bool[Event_num];
    //0:はじめてクッキーを作った
    //1:ラスクのレシピを読んだ
    //2:コレクションアイテムをはじめて手に入れた
    //3:チュートリアルを読んだ
    //4:体力がはじめて0になった
    //5:お金がはじめて1000を下回った
    //はじめての調合、はじめての仕上げは、PlayerStatusで管理

    //好感度やパティシエレベルで発生するサブイベントのフラグ   
    public static bool[] GirlLoveSubEvent_stage1 = new bool[GirlLoveSubEvent_stage_num];

    //好感度ハイスコアイベントの取得フラグ
    public static bool[] OkashiQuestHighScore_event = new bool[GirlLoveEvent_stage_num];

    //マップイベント
    public static bool[] MapEvent_01 = new bool[Event_num];         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
    public static bool[] MapEvent_02 = new bool[Event_num];         //井戸。
    public static bool[] MapEvent_03 = new bool[Event_num];         //ストロベリーガーデン
    public static bool[] MapEvent_04 = new bool[Event_num];         //ひまわりの丘
    public static bool[] MapEvent_05 = new bool[Event_num];         //ラベンダー畑
    public static bool[] MapEvent_06 = new bool[Event_num];         //バードサンクチュアリ
    public static bool[] MapEvent_07 = new bool[Event_num];         //ベリーファーム 
    public static bool[] MapEvent_08 = new bool[Event_num];         //白猫のおはか  

    //2から
    public static bool[] MapEvent_Or = new bool[OrEvent_num];

    //広場でのイベント
    public static bool[] hiroba_event_end = new bool[99]; //イベントを読み終えたかどうかを保存するフラグ。配列順は適当。

    //2の広場イベントNPCフラグ
    public static bool[] NPCHiroba_HikarieventList = new bool[GirlLoveSubEvent_stage_num]; //２でのヒカリの広場全般イベントリスト。
    //100~ 散歩道

    public static bool[] NPCHiroba_eventList = new bool[NpcEvent_stage_num]; //主に2でのNPCイベントのフラグリスト　配列の番号で各キャラを指定 100~ とか　200~とか
                                                                             //0~ コンテストレセプション 100~白い布

    public static bool[] NPCMagic_eventList = new bool[NpcEvent_stage_num]; //主に2での魔法NPCイベントのフラグリスト

    //NPCの友好度ポイント　各NPCの進行度を数値で表したもの　50からはじまり、ミラボー先生なら、あげたときにクリアしたら+10。そして次の魔法の本に..。という具合。
    public static int[] NPC_FriendPoint = new int[NpcEvent_people_num]; //300人分はいる

    public static int[] Treature_getList = new int[OrEvent_num]; //道端に落ちてるアイテムなどの宝箱リスト
    public static bool[] NPCHiroba_blockReleaseList = new bool[OrEvent_num]; //主に2での広場ブロックを解除するイベントリスト

    //ショップのイベントリスト
    public static bool[] ShopEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool[] ShopLVEvent_stage = new bool[Event_num]; //パティシエレベルなどに応じたイベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool[] BarEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool[] emeraldShopEvent_stage = new bool[Event_num];
    public static bool[] FarmEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //2から
    public static bool[] Or_ShopEvent_stage = new bool[OrEvent_num];

    //酒場のうわさ話リスト
    public static bool[] ShopUwasa_stage1 = new bool[Uwasa_num]; //うわさ話のリスト。シナリオの進行度に合わせて、リストは変わっていく。５個ずつぐらい？


    //コンテストのイベントリスト
    public static bool[] ContestEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //お菓子イベントクリアのフラグ
    public static bool[] OkashiQuest_flag_stage1 = new bool[Event_num]; //各イベントのクリアしたかどうかのフラグ。
    public static bool[] OkashiQuest_flag_stage2 = new bool[Event_num];
    public static bool[] OkashiQuest_flag_stage3 = new bool[Event_num];

    public static bool QuestClearflag; //現在のクエストで60点以上だして、クリアしたかどうかのフラグ。
    public static bool QuestClearButton_anim; //クリア初回のみ、ボタンが登場する演出のフラグ。他シーンを移動しても、大丈夫なようにしている。  

    //クリアお菓子の情報
    public static int SpecialQuestClear_okashiItemID;

    //さっき食べたお菓子情報
    public static string Okashi_lasthint; //さっき食べたお菓子のヒント。
    public static string Okashi_lastname; //さっき食べたお菓子の名前。
    public static string Okashi_lastslot; //さっき食べたお菓子のスロット部分名。
    public static int Okashi_lastID; //さっき食べたお菓子のアイテムID
    public static int Okashi_makeID; //さっき作ったお菓子のアイテムID。セーブ不要。
    public static int Okashi_lastshokukan_param; //さっき食べたお菓子のパラメータ
    public static string Okashi_lastshokukan_mes; //さっき食べたお菓子のパラメータ
    public static int Okashi_lastsweat_param; //さっき食べたお菓子のパラメータ
    public static int Okashi_lastsour_param; //さっき食べたお菓子のパラメータ
    public static int Okashi_lastbitter_param; //さっき食べたお菓子のパラメータ
    public static int Okashi_totalscore; //女の子にあげたときの点数   
    public static int Okashi_last_totalscore; //前回食べたお菓子の点数 
    public static int Okashi_quest_bunki_on; //特定お菓子のときの条件分岐
    public static bool high_score_flag; //高得点でクリアしたというフラグ。セーブされる。
    public static bool high_score_flag2; //150点~でクリアしたというフラグ。セーブされる。  

    public static int Okashi_toplast_score; //前回あげた最高得点
    public static int Okashi_toplast_heart; //前回あげたときの最高ハート取得量

    public static int Okashi_spquest_eatkaisu; //そのクエスト内で、お菓子を食べた回数をカウント
    public static int Okashi_spquest_MaxScore; //そのクエスト内で、最大のお菓子の点数
    public static bool Okashi_Extra_SpEvent_Start; //ハート系クエストで、食べたお菓子が一定回数以下のとき、発動するクエスト
    public static int ExtraClear_QuestItemRank;      //エクストラクエストクリア時のご褒美のランク

    //コンテスト審査員の点数
    public static int[] contest_Score = new int[3];
    public static int contest_TotalScore;
    public static List<int> contest_TotalScoreList = new List<int>();
    public static int contest_PrizeScore; //各ラウンドのトータルスコアの合計値　賞品獲得の計算で使う
    public static int[] contest_Taste_Score = new int[3];
    public static int[] contest_Beauty_Score = new int[3];
    public static int[] contest_Sweat_Score = new int[3];
    public static int[] contest_Bitter_Score = new int[3];
    public static int[] contest_Sour_Score = new int[3];
    public static string[] contest_Sweat_Comment = new string[3];
    public static string[] contest_Bitter_Comment = new string[3];
    public static string[] contest_Sour_Comment = new string[3];
    public static bool contest_Disqualification; //コンテスト失格フラグ

    //お菓子の一度にトッピングできる回数
    public static int topping_Set_Count;

    //ヒカリに作らせるお菓子の材料
    public static int[] hikari_kettei_item = new int[10];
    public static string[] hikari_kettei_originalID = new string[10];
    public static int[] hikari_kettei_kosu = new int[10];
    public static int[] hikari_kettei_toggleType = new int[10];
    public static string[] hikari_kettei_itemName = new string[10]; //ItemDBのItemNameも入れておく。材料表示するときなどに使う。
    public static bool hikari_make_okashiFlag; //ヒカリがお菓子を制作中かどうかのフラグ
    public static int hikari_make_okashiID;
    public static int hikari_make_okashi_compID; //CompoDBのID
    public static int hikari_make_okashiTimeCost; //かかる時間
    public static int hikari_make_okashiTimeCounter; //制作時間のタイマー
    public static float hikari_make_success_rate; //成功率
    public static int hikari_make_doubleItemCreated;
    public static float hikari_make_okashi_totalkyori;
    public static int hikari_make_okashiKosu; //ヒカリが現在制作したお菓子の個数
    public static int hikari_make_success_count; //ヒカリが制作に成功した数
    public static int hikari_make_failed_count; //ヒカリが制作に失敗した数
    public static bool hikari_make_Allfailed; //すべて失敗して材料がなくなってしまった 
    public static bool hikari_zairyo_no_flag; //作る材料が単になくなった場合

    public static int hikari_makeokashi_startcounter; //これはセーブ不要。10秒ほどたったら、元のアイドルモーションにもどすためのタイマー
    public static bool hikari_makeokashi_startflag; //これもセーブ不要。作りをお願いした最初だけ、モーションが変わるフラグ。
    public static float hikari_make_okashiTime_costbuf; //セーブ不要。お菓子作りにかかる時間をお菓子LVによって補正かける。かかる時間okashiTimeCostを保存しているので、こっちはセーブ不要
    public static float hikari_make_okashiTime_successrate_buf; //こっちも、hikari_make_success_rateを保存すれば、保存不要。

    //オプションの設定　マスター音量など
    public static float MasterVolumeParam;
    public static float BGMVolumeParam;
    public static float SeVolumeParam;
    public static float AmbientVolumeParam;
    public static int GameSpeedParam;

    public static bool SleepSkipFlag;
    public static bool PicnicSkipFlag;
    public static bool OutGirlSkipFlag;

    //現在のメインBGMの番号
    public static int mainBGM_Num;
    public static int userBGM_Num; //ユーザーが音楽図鑑で選んだ自分の一曲

    //ピクニックイベントのカウンター
    public static int picnic_count;
    public static bool picnic_event_ON;

    //外出るイベントのカウンター
    public static int outgirl_count;
    public static bool outgirl_event_ON;
    public static bool outgirl_Nowprogress; //ヒカリが採取などで外へ外出中のフラグ

    //いちごイベントのフラグ
    public static bool hiroba_ichigo_first; //一回でもいちごお菓子をわたした。
    public static bool[] ichigo_collection_listFlag; //いちごのお菓子のコレクションフラグ。
    public static List<string> ichigo_collection_list = new List<string>(); //いちごのお菓子の名前リスト。こっちはセーブ不要。

    //獲得称号リストのフラグ
    public static List<SpecialTitle> title_collection_list = new List<SpecialTitle>(); //称号の名前リスト。

    //獲得スチルリストのフラグ
    public static List<SpecialTitle> event_collection_list = new List<SpecialTitle>(); //イベントの名前リスト。

    //獲得コンテストお菓子リストのフラグ
    public static List<SpecialTitle> contestclear_collection_list = new List<SpecialTitle>(); //イベントの名前リスト。 
    
    //獲得音楽図鑑のフラグ
    public static List<SpecialTitle> bgm_collection_list = new List<SpecialTitle>(); //音楽リスト。 

    //現在受けているコンテスト
    public static List<ContestSaveList> contest_accepted_list = new List<ContestSaveList>(); //

    //バージョン情報
    public static float GameVersion = 2.0f;
    public static string GameSaveDaytime = ""; //セーブしたときの日付

    /* セーブ　ここまで */


    //シナリオ関係の発生フラグ
    public static bool Prologue_storyflag; //プロローグの開始

    //広場イベント発生フラグ
    public static bool hiroba_event_flag;   //イベントレシピを見たときに、宴を表示する用のフラグ   
    public static int hiroba_event_placeNum;  //どの場所を選んだか
    public static int hiroba_event_ID; //イベントID

    //通常お菓子を食べた後の感想
    public static int OkashiComment_ID;
    public static bool OkashiComment_flag;
    public static int OkashiComment_itemID; //食べたお菓子のアイテムID

    //スペシャルお菓子を食べる前の会話フラグ
    public static bool sp_okashi_hintflag;

    //スペシャルお菓子を食べた後の感想フラグ
    public static int sp_okashi_ID;         //食べた瞬間に表示する感想
    public static bool sp_okashi_flag;      //食べた瞬間に表示する感想
    public static int okashiafter_ID;       //採点表示のあとに表示する感想
    public static bool okashiafter_flag;    //採点表示のあとに表示する感想
    public static int okashihint_ID;        //SPお菓子以外のものをあげたとき、感想も出す場合はON
    public static bool okashihint_flag;     //SPお菓子以外のものをあげたとき、感想も出す場合はON
    public static int okashinontphint_ID;       //SPお菓子に必要なトッピングがのってなかったときに出す感想
    public static bool okashinontphint_flag;    //SPお菓子に必要なトッピングがのってなかったときに出す感想
    public static int okashiafter_status;    //採点表示　SPお菓子の感想か固有の感想か
    public static int mainquest_ID;         //クエストクリア時のイベント
    public static bool mainClear_flag;      //クエストクリア時のイベント
    public static int ExtraOkashiQuestComment_num;      //エクストラクエストクリア時のイベント 
    public static bool ExtraClear_flag;      //エクストラクエストクリア時のイベント 
    public static string ExtraClear_QuestName;      //エクストラクエストクリア時のイベント名前 
    public static int ExtraClear_QuestNum;          //エクストラクエストクリア時のイベント番号を一時保存     
    public static bool QuestClearButtonMessage_flag;  //クエストクリア時のボタン出現時、一言しゃべる
    public static int QuestClearButtonMessage_EvNum; //クエストクリア時のイベント番号

    //現在のクエストが、クエスト全体の何番目か。デバッグでハートレベル更新の際、使う。
    public static int NextQuestID; //次クエストのgirlLikeCompoSetの_compIDを指定。GirlEatJudgeで使用する。

    //好感度イベント発生フラグ
    public static bool girlloveevent_flag;          //女の子の好感度に応じて発生するイベントのフラグ
    public static bool girlloveevent_endflag;       //宴で読み終了したときのフラグ
    public static bool questclear_After;            //クエストクリアボタンを押したよ、というフラグ。セーブの必要はなし。次のSPクエストへ進行するためのフラグ。

    public static int GirlLoveSubEvent_num;
    public static int girlloveevent_bunki; //メインイベントかサブイベントかを分岐する

    //エメラルどんぐりゲット時の会話
    public static bool emeralDonguri_flag;  //高得点時、エメラルどんぐりをくれるイベント発生のフラグ
    public static int emeralDonguri_status;

    //妹の口をクリックしたときのヒント表示フラグ
    public static int touchhint_ID;
    public static bool touchhint_flag;

    //寝るイベントフラグ
    public static bool sleep_flag;
    public static int sleep_status;

    //お菓子クエストクリアフラグの発生関係
    //public static bool clear_spokashi_flag; //SPお菓子でクリアしたか、好感度あげてクリアしたかどうか。現在未使用。
    public static bool QuestClearAnim_Flag;   //クリアしたときに、ボタンを登場させるか否かのフラグ。そのクエストの最後のときだけ演出をだす時に使う。
    public static bool QuestClearCommentflag; //一回SPクリアしたあと、感想をだしたかどうか。
    public static int Okashi_quest_bunki_num; //条件分岐した場合の、クエスト番号

    //お菓子イベント現在のナンバー
    public static string NowEatOkashiName; //今食べたいお菓子の名前表示
    public static int NowEatOkashiID; //今食べたいお菓子のアイテムID。Itemdatabaseのリスト番号。

    //お菓子の点数基準値
    public static int mazui_score;
    public static int low_score;
    public static int high_score;
    public static int high_score_2;

    //水っぽさなどの基準値
    public static int Watery_Line;

    //お菓子の点数    
    public static int Okashi_dislike_status; //状態。2で、新しいお菓子をあげた場合
    public static int Okashi_OnepointHint_num; //お菓子あげたあと、一言メモを更新する。そのときのヒント番号

    //ショップの話すコマンド
    public static bool shop_event_flag;  //ショップで発生するイベントのフラグ。
    public static int shop_event_num;
    public static bool shop_lvevent_flag;  //ショップで発生するイベントのフラグ。
    public static int shop_lvevent_num;
    public static bool talk_flag;       //ショップの「話す」コマンドをONにしたとき、これがONになり、宴の会話が優先される。NPCなどでも使う。
    public static int talk_number;      //その時の会話番号。
    public static bool uwasa_flag;       //ショップの「うわさ話」コマンドをONにしたとき、これがONになり、宴の会話が優先される。NPCなどでも使う。
    public static int uwasa_number;      //その時のうわさ話番号。
    public static bool shop_hint;
    public static int shop_hint_num;

    //バーのコマンド
    public static bool bar_event_flag;  //バーで発生するイベントのフラグ。
    public static int bar_event_num;
    public static int bar_quest_okashiScore; //酒場のクエストでのお菓子スコア

    //マップイベント発生フラグ
    public static int map_ev_ID;           //その時のイベント番号
    public static bool map_event_flag;      //マップイベントの、宴を表示する用のフラグ

   
    //ステージはじまりの日付
    public static int stage1_start_day;
    public static int stage2_start_day;
    public static int stage3_start_day;

    public static int stage1_limit_day;
    public static int stage2_limit_day;
    public static int stage3_limit_day;

    //エメラルドショップのイベント発生フラグ
    public static bool emeraldshop_event_flag;  //ショップで発生するイベントのフラグ。
    public static int emeraldshop_event_num;

    //コンテストのイベント発生フラグ
    public static bool contest_event_flag;  //イベントのフラグ。
    public static bool contest_or_event_flag;  //オランジーナ系コンテスト　２はこっちを使う
    public static bool contest_or_contestjudge_flag;
    public static bool contest_or_prizeget_flag;
    public static bool contest_or_limittimeover_flag;
    public static int contest_event_num;
    public static bool contest_MainMatchStart; //コンテスト実際の試合開始の合図

    //コンテストに提出したお菓子    
    public static string contest_okashiName;
    public static string contest_okashiNameHyouji;
    public static string contest_okashiSubType;
    public static string contest_okashiSlotName;
    public static int contest_okashiID;
    public static Item contest_okashi_ItemData;

    public static bool special_shogo_flag;
    public static int special_shogo_num;

    //コンテスト感想
    public static string[] contest_judge1_comment = new string[4];
    public static string[] contest_judge2_comment = new string[4];
    public static string[] contest_judge3_comment = new string[4];

    //エンディングのフラッグ
    public static bool ending_on;       //コンテストメインで、エンディングシーンへ移動するためのフラグ
    public static bool ending_on2;      //BadEDの場合。EDムービーなし。
    public static int ending_number;    //エンディング番号    

    //牧場のイベント発生フラグ
    public static bool farm_event_flag;  //ショップで発生するイベントのフラグ。
    public static int farm_event_num;

    //別シーンから家にもどってきたときに、イベント発生するかどうかのフラグ
    public static bool CompoundEvent_flag;
    public static int CompoundEvent_num; //別シーンから、どのイベントを呼び出すかを、指定する。
    public static bool CompoundEvent_storyflag;
    public static int CompoundEvent_storynum; //メインシーンから、宴に移る際に、どのシナリオを読むかを指定する。

    //CGギャラリー再生用のフラグ
    public static bool CGGallery_readflag;
    public static int CGGallery_num; //別シーンから、どのイベントを呼び出すかを、指定する。

    //今自分がいるシーンの属性　調合関係とかショップ関係、バー関係など シーン名そのものが違っても、処理は共通として使用できる。
    public static int Scene_Category_Num;           //Compound=10, Compound_Entrance=11, Shop=20, Bar=30, Farm=40, EmeraldShop=50, Hiroba=60, 
                                                    //Contest=100, Contest_Outside=110, Contest_Recption=120, GetMaterial_Scene=130, Station=140, NPCMagicHouse=150
                                                    //NPC_Catsle=160, 200_omake=200, 999_Gameover=999, 001_Title=1000, 読み専用シーン=5000, 回避用=9999


    //その他、一時的なフラグ
    public static int MapSubEvent_Flag;
    public static bool MenuOpenFlag; //メニューを現在開いているか閉じているか
    public static bool QuestManzokuFace; //60点以上取って、喜び表情に変えるフラグ
    public static bool OsotoIkitaiFlag; //外いこうよ～モード。このときに外へいくと、喜んでハートがあがる。
    public static bool picnic_event_reading_now; //ピクニック読み中
    public static bool picnic_after; //ピクニック後、少し余韻にひたる
    public static int picnic_after_time; //余韻にひたる時間
    public static bool GirlLove_loading; //好感度イベントを読み中
    public static bool Load_eventflag; //ロード直後のフラグ
    public static bool check_GirlLoveEvent_flag;
    public static bool check_GirlLoveSubEvent_flag;
    public static bool check_GirlLoveTimeEvent_flag;
    public static bool check_CompoAfter_flag;
    public static bool check_GetMat_flag;
    public static bool check_OkashiAfter_flag;
    public static bool[] check_SleepEnd_Eventflag = new bool[10];
    public static int ResultComplete_flag;
    public static bool Mute_on;
    public static bool SubEvAfterHeartGet; //Utage_scenarioからも読まれる
    public static int SubEvAfterHeartGet_num;
    public static bool outgirl_returnhome_reading_now; //外出から帰ってきたときの宴読み中
    public static bool outgirl_returnhome_homeru; //リザルトパネルをおして、ほめるか否か
    public static bool girl_returnhome_flag; //兄が家に帰ってきたときに、妹がすでに外出からかえってきているかどうかのフラグ
    public static int girl_returnhome_num;
    public static bool girl_returnhome_endflag;
    public static bool girl_returnhome_endflag2;
    public static bool ReadGirlLoveTimeEvent_reading_now; //外出から帰ってきたときの宴読み中
    public static bool ResultOFF; //リザルトパネルのオンオフ
    public static bool Degheart_on; //ハート下がっている途中は、時間で下がる機能を一時的にオフにするフラグ
    public static bool utage_charaHyouji_flag; //イベントで、宴キャラクタの表示をONにするかOFFにするか
    public static int RandomEatOkashi_counter; //食べたいお菓子が変わるまでのカウンタ
    public static bool specialsubevent_flag1; //お菓子の採点が777のときに、サブイベントを呼び出すときのフラグ
    public static string hikarimakeokashi_itemTypeSub_nameHyouji; //ヒカリのお菓子Expテーブルの各お菓子の名前表記。スクリプト間の値受け渡し用で一時的。
    public static int hikarimakeokashi_nowlv; //ヒカリのお菓子Expテーブルで、現在のお菓子レベル。スクリプト間の値受け渡し用で一時的。
    public static int hikarimakeokashi_finalgetexp; //ヒカリのお菓子経験値　最終獲得値。一時的。
    public static bool hikariokashiExpTable_noTypeflag; //ヒカリのお菓子Expテーブルで、どのお菓子タイプにも合わなかった場合。例外処理。スクリプト間の値受け渡し用で一時的。
    public static int MainQuestClear_flag; //メインクエストをクリアしたのか、道中の通常のSPクエストをクリアしたのか、判定するフラグ
    public static bool final_select_flag; //調合シーンで、調合の最終決定の確認
    public static bool tempature_control_select_flag; //調合シーンで、温度管理画面を開く
    public static bool tempature_control_Offflag; //温度管理画面を閉じる
    public static bool tempature_control_ON; //焼き菓子かどうかをチェックし、焼き菓子ならON　最終チェックからキャンセルで戻るときに使用する
    public static string tempature_control_Param_yakitext; //焼き具合のテキスト
    public static int System_tempature_control_Param_temp; //温度管理画面の温度の値
    public static int System_tempature_control_Param_time; //温度管理画面の時間の値
    public static bool matbgm_change_flag; //採取地へ行く際のBGM切り替えのフラグ
    public static bool compobgm_change_flag; //調合シーンと元シーンとで、BGMの切り替えを行うフラグ
    public static bool extremepanel_Koushin; //なんらかの調合が終わり、エクストリームパネルの表示を更新するフラグ
    public static bool live2d_posmove_flag; //Live2Dキャラの位置を移動したフラグ
    public static bool Reset_SceneStatus; //酒場クエストの遷移状態をリセットするフラグ　主にQuest_Judgeとの連携
    public static int Scene_Status; //各シーンごとの状態
    public static int Scene_Select; //各シーンごとの状態 今なにを選択しているか
    public static bool Scene_LoadedOn_End; //シーンの読み込み完了フラグ
    public static bool girlEat_ON; //女の子　食べ中のフラグ
    public static bool Kaigyo_ON; //メッセージウィンドウの改行ボタンをおした。宴ではなく、材料採取の改行時など。
    public static int Comp_kettei_bunki; //調合の、今何の調合をしている最中かを表すステータス
    public static string UseMagicSkill; //使用する魔法・スキルのネーム
    public static string UseMagicSkill_nameHyouji; //使用する魔法・スキルのネームの表示用
    public static int UseMagicSkill_ID; //使用するスキルのID    
    public static int UseMagicSkillLv; //使用するスキルの使用レベル
    public static int MagicSkillSelectStatus; //今、魔法を使うを選択したか、習得を選択したかを分岐    
    public static bool EventAfter_MoveEnd; //なんらかのイベント終了後、すぐにヒカリを元の位置に戻す
    public static bool Status_zero_readOK; //メインステータスを読み終わったよ～のフラグ　その後に、ヒカリが戻ってくるなどの処理を挟む用
    public static int OkashiMake_PanelSetType; //さっき作ったお菓子が、パネルにセットされるお菓子かどうか。生地などはセットされず、すぐ調合画面を戻す
    public static int Contest_listnum; //今出場してるコンテストのDBのリスト番号
    public static int ContestSelectNum; //どのコンテストに今出場しているか
    public static int ContestRoundNum; //今何回戦か
    public static int ContestRoundNumMax; //その大会のMaxのラウンド数
    public static int Contest_Cate_Ranking; //トーナメント形式かランキング形式か
    public static string Contest_Name; //コンテストの名前
    public static string Contest_NameHyouji; //コンテストの名前日本語表記
    public static string Contest_ProblemSentence; //コンテストの課題の内容
    public static string Contest_ProblemSentence2; //コンテストの課題の内容
    public static string Contest_HallBGName; //コンテストの会場背景の指定　stringで
    public static string Contest_ChubouBGName; //コンテストの会場厨房背景の指定　stringで
    public static string Contest_BGMSelect; //コンテスト制作中のBGMの指定
    public static int Contest_DB_list_Type; //コンテスト番号に応じた、判定番号を指定
    public static int Contest_commentDB_Select; //番号に応じて、コメントのDBを指定
    public static bool Contest_ON; //コンテストの最中のフラグ　調合時にBGMを変わらないようにするなどのフラグ
    public static bool Contest_Clear_Failed; //特殊点が足りないなどの場合、コンテスト不合格のフラグがたつ。trueで不合格。 
    public static int contest_boss_score; //コンテスト　対戦相手のスコア
    public static string contest_boss_name; //コンテスト　対戦相手の名前
    public static int contest_Rank_Count; //コンテスト　ランキングで何位だったか
    public static bool Contest_yusho_flag; //コンテスト優勝したかどうかのフラグ
    public static bool Contest_winner_flag; //コンテストで対戦相手に勝ったかどうかのフラグ
    public static bool Contest_Next_flag; //コンテスト〇回戦を開始する
    public static bool Contest_PrizeGet_flag; //コンテスト賞品を獲得する処理のフラグ
    public static string Contest_PrizeGet_ItemName; //獲得した賞品のアイテム名
    public static int Contest_PrizeGet_Money; //獲得した賞金
    public static bool contest_eventEnd_flag; //コンテストイベント全て終了
    public static bool contest_eventEdenLoser_flag; //エデンコンテストで途中で負けてしまった場合
    public static bool contest_LimitTimeOver_DegScore_flag; //コンテスト制限時間をこえて、減点のフラグ
    public static bool contest_LimitTimeOver_Gameover_flag; //コンテスト制限時間をこえて失格のフラグ
    public static bool contest_LimitTimeOver_After_flag; //コンテスト失格後、なんらかのペナルティやメッセージが発生するフラグ
    public static List<int> contest_BeautyJudgeScore = new List<int>(); //コンテストの見た目担当の人の見た目審査基準　girlBeautyのこと
    public static bool NewAreaRelease_flag; //なんらかのイベント後、新エリアが解禁されるフラグ
    public static List<int> PrizeScoreAreaList = new List<int>(); //コンテストのランキングスコア、もしくは賞品のスコア範囲のリスト
    public static List<string> PrizeItemList = new List<string>(); //コンテストの優勝のアイテムリスト
    public static List<string> PrizeCharacterList = new List<string>(); //コンテストの参加者リスト
    public static List<int> PrizeGetMoneyList = new List<int>(); //コンテストの優勝の賞金リスト
    public static int PrizeGetninkiparam_before; //コンテストの順位で獲得する人気度 補正前
    public static int Contest_PrizeGetninkiparam; //コンテストの順位で獲得する人気度 最終値
    public static int SceneSelectNum; //シーンの移動先を指定する番号　番号をもとに、移動先シーンのStartでその場所名を決定する
    public static bool Getmat_return_home; //採取地から家に帰ってきたフラグ
    public static int Select_place_num; //採取のDBリスト番号
    public static string Select_place_name; //採取地の名前
    public static int Select_place_day; //採取地までにかかる日数
    public static Dictionary<string, int> GetMat_ResultList = new Dictionary<string, int>(); //採取で取得したアイテムのリスト　名前と個数
    public static bool Money_counterAnim_on; //所持金お金動くアニメON
    public static bool Money_counterAnim_StartSetting; //そのとき最初だけ初期設定
    public static bool Money_counterOnly; //アニメはなしで、カウンタを生成する
    public static int Money_counterParam; //そのときの入ったお金
    public static int Money_StartParam; //アニメ前の始まりのお金
    public static int Contest_OrganizeMonth; //コンテストの開催月
    public static int Contest_OrganizeDay; //コンテストの開催日
    public static bool Contest_ReadyToStart; //宴の読みが終わってから、コンテストを開始するフラグ
    public static bool Contest_ReadyToStart2; //出場するではいを押した後、すぐにコンテスト開始するフラグ
    public static bool Contest_afterHomeEventFlag; //コンテスト終了後、家にかえって寝たあとに発生するイベント
    public static bool Contest_afterHomeHeartUpFlag; //コンテスト終了後、寝ておきてから、順位に応じてハートが上がるイベントのフラグ
    public static bool CharacterTouch_ALLOFF; //キャラの触り判定をオフにする。
    public static bool CharacterTouch_ALLON; //キャラの触り判定をオンにする。   
    public static bool BGTouch_ALLOFF; //背景オブジェクトの触り判定をオフにする。
    public static bool BGTouch_ALLON; //背景オブジェクトの触り判定をオンにする。
    public static bool EatAnim_End; //食べるときのエフェクトアニメの終了を検知
    public static bool Utage_Prizepanel_ON; //コンテスト賞品のシーン再生中、賞品リストを表示する。
    public static bool Utage_Prizepanel_WaitHyouji; //プライズパネルが完全に開くまで、宴の続きを再生しないフラグ
    public static bool Utage_Prizepanel_OFF; //賞品リストをオフにする。
    public static bool Utage_SceneEnd_BlackON; //うたげ終了時、シーン移動する際に、ゲーム本編の黒をONにする。でないと、一瞬切り替え表示が見えてしまう。
    public static bool Scene_Black_Off; //シーンによっては、このフラグがたつと、宴途中などで、シーンの黒画面をオフにする
    public static bool Utage_MapMoveBlackON; //宴読み終わり後に、マップ移動のとき、シーンをあらかじめブラックに消すフラグ　こっちは、SceneEnd_BlackONをTrueにするため分岐する用
    public static int Utage_Prizepanel_Type; //コンテストのシーン再生中、賞品リストか順位表を表示する際のタイプ指定
    public static bool Utage_MapMoveON; //シナリオ読み後、シーンを移動するフラグ
    public static bool Ajimi_AfterFlag; //味見直後　テキスト更新用のフラグ
    public static string AjimiAfter_Text; //味見直後　テキスト
    public static string GetMat_BackPlaceName; //採取から戻るときの戻り先の指定
    public static bool Station_TrainGoFlag; //電車にのるフラグ
    public static string Item_subcategoryText; //カード表示するときのサブカテゴリーの日本語表記
    public static string Item_ShokukanTypeText; //そのときの食感の表示
    public static int Item_ShokukanTypeNum; //どの食感を選んでいるかを番号で指定　その後の各スクリプトの処理で分岐して使用する
    public static int Item_ShokukanTypeScoreNum; //どの食感で、どの判定を使うかを指定
    public static List<string> ContestItem_supplied_List = new List<string>(); //コンテストで支給されるアイテム
    public static List<int> ContestItem_supplied_KosuList = new List<int>(); //コンテストで支給されるアイテムの個数
    public static int Cullender_Month; //カレンダーで計算した、入力した日付をもとに月を返す値
    public static int Cullender_Day; //カレンダーで計算した、入力した日付をもとに日を返す値
    public static bool Window_FaceIcon_OnOff; //顔ぐらふぃっくの表示／非表示
    public static string Window_CharaName; //顔ぐらふぃっくの非表示のときの名前
    public static int EatOkashi_DecideFlag; //食べたいお菓子が、ランダムなのかメインクエストで固定するのかを分岐するフラグ
    public static bool SPquestPanelOff; //メインクエストの表示パネルをオフ　実質自由な時間の始まりを意味する
    public static bool OutEntrance_ON; //玄関から外へでるボタンをオンにする
    public static int UwasaNum_Select; //うわさ番号のDBを指定
    public static bool ShopEnter_ButtonON; //外からボタンを押して入店するフラグ　入店時のSEの重複を防ぐ
    public static bool hiroba_treasureget_flag; //道端の宝箱拾ったフラグ
    public static string hiroba_treasureget_Name; //そのお宝の名前
    public static int hiroba_treasureget_Num; //そのお宝の種類番号
    public static int hiroba_treasureget_Kosu; //そのお宝の取得個数
    public static bool Extreme_On; //トッピングや魔法調合から、新規作成扱いにするフラグ
    public static bool System_magic_playON; //魔法ミニゲーム画面を使用　成功率などが、確率でなくミニゲームの結果に変わる
    public static float System_magic_playtime; //魔法の演出時間　魔法によって変わる
    public static float System_magic_playParamUp; //魔法ミニゲームの結果による、食感補正値
    public static bool System_magic_playSucess; //魔法ミニゲームで、成功か失敗か
    public static bool Special_OkashiEnshutsuFlag; //特定のおかしをはじめて作成するときに、特別演出が発生するフラグ
    public static string Special_OkashiEnshutsuName; //演出の指定
    public static string MainQuestTitleName; //メインクエストのタイトル


    //一時フラグ　アイテムDB関連
    public static string ResultItem_nameHyouji; //完成したアイテム名表示用
    public static int Result_Kosu; //完成したアイテムの個数
    public static bool Result_compound_success; //調合が成功したか失敗したか

    public static int temp_itemID1; //一時的にアイテムIDを保存しておくための変数 アイテムDBのリスト配列
    public static int temp_itemID2; //一時的にアイテムIDを保存しておくための変数
    public static int temp_itemID3; //一時的にアイテムIDを保存しておくための変数
    public static int temp_baseitemID;

    public static int Final_list_itemID1; //一時的なDBのリスト配列番号　店売り・オリジナル・エクストリーム全て含む　各スクリプトをまたぐため、ここで設定 　
    public static int Final_list_itemID2; //アイテムIDではなく、リストの配列を指定しているところに注意　またtoggle_Typeと一緒に使う
    public static int Final_list_itemID3;
    public static int Final_list_baseitemID;

    public static int Final_toggle_Type1; //店売りかオリジナルアイテムリストか、エクストリームパネルのリストか　を　指定する
    public static int Final_toggle_Type2;
    public static int Final_toggle_Type3;
    public static int Final_toggle_baseType;

    public static int Final_kettei_kosu1;
    public static int Final_kettei_kosu2;
    public static int Final_kettei_kosu3;
    public static int Final_kettei_basekosu;

    public static int List_count1; //一時的なアイテムリストのリスト番号　IDと違い、開いたリストの配列をそのまま指定　toggle_Typeは使わない
    public static int List_count2;
    public static int List_count3;
    public static int List_basecount;

    public static int Final_setCount; //セット数
    public static int Final_result_itemID1; //一時的なDBのリスト配列番号 生成されるアイテム
    public static int Final_result_compID; //そのときのcompoDBの調合リスト番号


    public static bool CompoundSceneStartON; //調合の処理を開始したというフラグ　あらゆるシーンから、調合シーンができるようにするためのフラグ管理

    public static bool hikari_tabetaiokashi_buf; //食べたいお菓子をあげたときに、一時的にバフがかかる状態
    public static int hikari_tabetaiokashi_buf_time; //効果時間

    private PlayerItemList pitemlist;

    public static int system_i;
    public static int system_count;
    public static int system_temp_int;
    public static int updown_kosu;
    private int ev_id;

    private float timeLeft;
    public static int Game_timeCount; //ゲーム内共通の時間

    //ゲームの現在の状態を表すステータス
    public static int compound_status; //メイン　各シーン共通で使われるので注意。
    public static int compound_select;

    //チュートリアル用の管理フラグ
    public static bool tutorial_ON;         //これがONになったら、ゲーム全体がチュートリアルモードになる。 
    public static bool tutorial_Progress;   //進行したときにフラグをたてる。すると、次のテキストが流れる。
    public static int tutorial_Num;         //チュートリアルの進行度

    //ステージ最初の読み込みフラグ
    public static bool stage1_load_ok;

    //ロード「続きから」を押したフラグ
    public static bool GameLoadOn;

    //キー入力受付開始のフラグ
    public static bool KeyInputOff_flag;

    //メインメッセージを更新するタイミング用のフラグ
    public static bool MesaggeKoushinON;    

    public static bool Scene_back_home; //シーンから、メイン画面にもどるときの、ドア開閉時の音を鳴らす用のフラグ。

    //カメラズームアウトの終わりを検出する
    public static bool camerazoom_endflag;

    //クエストクリアエフェクトの終わりを検出する。
    public static bool qclear_effect_endflag;

    //イベント中、アイテムリストを開き、選択画面がある場合のフラグ
    public static bool event_pitem_use_select;
    public static bool event_pitem_use_OK;
    public static int event_kettei_itemID;
    public static int event_kettei_item_Type;
    public static int event_kettei_item_Kosu;
    public static bool event_pitem_cancel;
    public static int event_judge_status;
    public static int event_okashi_score;
    public static bool NPC_event_ON; //調合画面メインでイベントがおこったフラグ
    public static bool hiroba_event_ON;
    public static bool shop_event_ON;
    public static bool farm_event_ON;
    public static bool bar_event_ON;
    public static bool KoyuJudge_ON;
    public static int KoyuJudge_num;
    public static bool NPC_DislikeFlag;
    public static bool NPC_Dislike_UseON;

    public static Dictionary<int, int> Hikariokashi_Exptable = new Dictionary<int, int>();
    //public static Dictionary<int, int> Hikariokashi_Exptable2 = new Dictionary<int, int>();

    //各NPCお菓子判定番号
    public static int Mose_Okashi_num01;
    public static int Shop_Okashi_num01;
    public static int Shop_Okashi_num02;
    public static int Farm_Okashi_num01;
    public static int Bar_Okashi_num01;

    //女の子の名前
    public static string mainGirl_Name;
    public static string player_Name; //主人公の名前
    public static string player_Name_First;

    //ゲーム共通の固有の色
    public static string ColorYellow;
    public static string ColorGold;
    public static string ColorLemon;
    public static string ColorPink;
    public static string ColorRed;
    public static string ColorRedDeep;
    public static string ColorBlue;
    public static string ColorCyan;
    public static string ColorMizuiro;
    public static string ColorOrange;
    public static string ColorGreen;
    public static string ColorGlay;

    //ゲームの通貨名
    public static string MoneyCurrency;
    public static string MoneyCurrencyEn;

    //ゴールドマスター称号用のルピア閾値
    public static int GoldMasterMoneyLine;

    //時間の概念を使用するかどうかのフラグ
    public static bool TimeUSE_FLAG = true; //使用するならtrue

    //一日の食費
    public static int Foodexpenses;
    public static int Foodexpenses_default;
    public static string MgrTodayFood; //今日の食事　セーブはしない   

    public static bool Haraheri_Msg;

    //一日のはじまり・終わりの時間
    public static int StartDay_hour;
    public static int EndDay_hour;

    //寝る前の現在の月日
    public static int SleepBefore_Month;
    public static int SleepBefore_Day;

    //時間刻む単位
    public static int TimeStep;

    //現在の天気の状態
    public static int BG_cullent_weather;
    public static int BG_before_weather;

    public static int Shopday; //ショップ入ったら更新する日付。その日を記録する。
    public static bool Sale_ON; //セール判定　日付をまたいだら、OFFに。

    //ロードしたセーブデータのバージョン情報
    public static float Load_GameVersion;
    public static string Load_GameSaveDaytime; //セーブしたときの日付を読み込み

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スクリーン設定の固定
        Screen.SetResolution(800, 600, false); //(800, 600, false, 60) 3番目はフルスクリーンのありなし。4番目はFPSの固定

        //FPS指定　こっちが本流っぽい
        Application.targetFrameRate = 60;

        //秒計算。　
        timeLeft = 1.0f;

        //刻む時間の単位　分単位
        TimeStep = 1;

        //エンディングカウント。リセットされない。
        ending_number = 1;
        ending_count = 0;
        bestend_on_flag = false;

        //シーンに割り振られたカテゴリ番号　ロードなどではリセットされない。
        Scene_Category_Num = 0;

        //各イベントフラグ・ゲームパラメーターの初期設定
        ResetGameDefaultStatus();

        //音量設定などの初期値
        MasterVolumeParam = 1.0f;
        BGMVolumeParam = 1.0f;
        SeVolumeParam = 1.0f;
        AmbientVolumeParam = 1.0f;

        //各色の設定
        ColorYellow = "<color=#FDFF80>"; // ゴールドに近いくすんだ黄色 #BA9535  かなり薄い黄色 #FDFF80
        ColorGold = "<color=#BA9535>";
        ColorLemon = "<color=#FDFF80>"; // かなり薄い黄色FDFF80
        ColorPink = "<color=#FF5CA1>";
        ColorRed = "<color=#FF0000>";
        ColorRedDeep = "<color=#FF4D4D>";
        ColorBlue = "<color=#0000FF>";
        ColorCyan = "<color=#44A2FF>";
        ColorMizuiro = "<color=#8DC1FF>"; 
        ColorOrange = "<color=#FF8400>";
        ColorGreen = "<color=#48EE72>";
        ColorGlay = "<color=#909090>";

        //通貨の名前
        MoneyCurrency = "ルピア";
        MoneyCurrencyEn = "Lp";

        //メインの女の子名前
        mainGirl_Name = "ヒカリ";
        player_Name_First = "アキラ";
        player_Name = "アキラ・ノワゼット";

        //ゴールドマスターのライン
        GoldMasterMoneyLine = 100000;

        //日が終わる時間
        StartDay_hour = 8;
        EndDay_hour = 20; //20時
    }
	
	// Update is called once per frame
	void Update () {

        //デバッグ用
        scenario_flag_cullent = scenario_flag;

        //時間のカウント
        timeLeft -= Time.deltaTime;

        //1秒ごとのタイムカウンター
        if (timeLeft <= 0.0)
        {
            timeLeft = 1.0f;
            Game_timeCount++;
        }

    }

    public static void ResetGameDefaultStatus()
    {
        GameLoadOn = false;
        saveOK = false;

        System_MagicUse_Flag = false;
        System_HikariMakeUse_Flag = false;

        stage1_clear_girl1_lovelv = 1;
        stage2_clear_girl1_lovelv = 1;
        stage3_clear_girl1_lovelv = 1;

        compound_status = 0;
        compound_select = 0;

        updown_kosu = 1;

        BG_cullent_weather = 2;
        BG_before_weather = BG_cullent_weather;

        //食費
        Foodexpenses_default = 100;
        Foodexpenses = Foodexpenses_default;

        //ストーリーモード
        Story_Mode = 0; //0=本編　1=エクストラモード　初期値は0でOK
        GameSpeedParam = 3;

        SleepSkipFlag = false;
        PicnicSkipFlag = false;
        OutGirlSkipFlag = false;

        scenario_flag = 0; //シナリオの進み具合を管理するフラグ。GameMgr.scenario_flagでアクセス可能。
        scenario_ON = false;
        scenario_flag_input = 0;
        scenario_flag_cullent = scenario_flag;

        camerazoom_endflag = false;
        qclear_effect_endflag = false;
        Haraheri_Msg = false;

        Costume_Num = 0; //初期コスチューム　メイド服がデフォルト
        for (system_i = 0; system_i < Accesory_Num.Length; system_i++)
        {
            Accesory_Num[system_i] = 0;
        }

        event_recipi_flag = false;
        event_recipi_endflag = false;

        recipi_read_flag = false;
        recipi_read_endflag = false;

        touchhint_flag = false;

        itemuse_recipi_flag = false;
        map_event_flag = false;

        gamestart_recipi_get = false;
        MesaggeKoushinON = false;

        SleepBefore_Month = 4;
        SleepBefore_Day = 1;

        Prologue_storyflag = false;

        shop_event_flag = false;
        shop_lvevent_flag = false;
        shop_event_num = 0;
        talk_flag = false;
        talk_number = 0;
        uwasa_flag = false;
        uwasa_number = 0;
        shop_hint = false;
        shop_hint_num = 0;
        NPC_event_ON = false;
        hiroba_event_ON = false;
        shop_event_ON = false;
        farm_event_ON = false;
        bar_event_ON = false;
        KoyuJudge_ON = false;
        NPC_DislikeFlag = false;
        NPC_Dislike_UseON = false;

        farm_event_flag = false;
        farm_event_num = 0;

        bar_event_flag = false;
        bar_event_num = 0;

        emeraldshop_event_flag = false;
        emeraldshop_event_num = 0;

        picnic_count = 3;
        picnic_event_ON = false;

        outgirl_count = 3;
        outgirl_event_ON = true;
        outgirl_Nowprogress = false;

        hiroba_ichigo_first = false;

        hiroba_event_flag = false;
        //広場イベント読み終えたフラグの初期化
        for (system_i = 0; system_i < hiroba_event_end.Length; system_i++)
        {
            hiroba_event_end[system_i] = false;
        }

        stage_number = 1;
        stage_quest_num = 1;
        stage_quest_num_sub = 1;

        stage1_load_ok = false;

        Scene_back_home = false;


        Game_timeCount = 0; //1秒タイマー

        GirlLoveEvent_num = 0;
        girlloveevent_flag = false;
        girlloveevent_endflag = false;
        questclear_After = false;

        sleep_flag = false;
        sleep_status = 0;
        scenario_read_endflag = false;
        KeyInputOff_flag = false;
        event_pitem_use_select = false;
        event_pitem_use_OK = false;
        event_pitem_cancel = false;

        CompoundEvent_flag = false;
        CompoundEvent_num = 0;
        CompoundEvent_storyflag = false;
        CompoundEvent_storynum = 0;

        CGGallery_readflag = false;

        Okashi_totalscore = 0;
        Okashi_last_totalscore = 0;
        Okashi_dislike_status = 0;
        Okashi_OnepointHint_num = 0;
        Okashi_quest_bunki_on = 0;
        Okashi_toplast_score = 0;
        Okashi_toplast_heart = 0;
        high_score_flag = false;
        high_score_flag2 = false;
        hikari_tabetaiokashi_buf = false;
        hikari_tabetaiokashi_buf_time = 0;

        ExtraClear_QuestItemRank = 1;

        contest_eventEnd_flag = false;
        contest_eventEdenLoser_flag = false;
        contest_LimitTimeOver_DegScore_flag = false;
        contest_LimitTimeOver_Gameover_flag = false;
        contest_LimitTimeOver_After_flag = false;
        NewAreaRelease_flag = false;
        MenuOpenFlag = false;
        QuestManzokuFace = false;
        OsotoIkitaiFlag = false;
        picnic_event_reading_now = false;
        outgirl_returnhome_reading_now = false;
        outgirl_returnhome_homeru = false;
        girl_returnhome_flag = false;
        girl_returnhome_num = 0;
        girl_returnhome_endflag = false;
        girl_returnhome_endflag2 = false;
        ReadGirlLoveTimeEvent_reading_now = false;
        ResultOFF = false;
        Degheart_on = false;
        Load_eventflag = false;
        picnic_after = false;
        picnic_after_time = 0;
        GirlLove_loading = false;
        check_GirlLoveEvent_flag = false;
        check_GirlLoveSubEvent_flag = false;
        check_GirlLoveTimeEvent_flag = false;
        check_CompoAfter_flag = false;
        check_GetMat_flag = false;
        check_OkashiAfter_flag = false;       
        ResultComplete_flag = 0;
        Mute_on = false;
        SubEvAfterHeartGet = false;
        SubEvAfterHeartGet_num = 0;
        utage_charaHyouji_flag = false;
        RandomEatOkashi_counter = 0;
        specialsubevent_flag1 = false;
        hikariokashiExpTable_noTypeflag = false;
        Okashi_Extra_SpEvent_Start = false;
        ExtraClear_QuestName = "";        
        MainQuestClear_flag = 0;
        final_select_flag = false;
        tempature_control_select_flag = false;
        tempature_control_Offflag = false;
        tempature_control_ON = false;
        CompoundSceneStartON = false;
        matbgm_change_flag = false;
        compobgm_change_flag = false;
        extremepanel_Koushin = false;
        live2d_posmove_flag = false;
        Reset_SceneStatus = false;
        Scene_LoadedOn_End = false;
        girlEat_ON = false;
        Scene_Black_Off = false;
        Kaigyo_ON = false;
        Comp_kettei_bunki = 0;
        UseMagicSkill = "";
        UseMagicSkill_nameHyouji = "";
        UseMagicSkill_ID = 0;
        ResultItem_nameHyouji = "";
        Result_Kosu = 0;
        Result_compound_success = false;
        MagicSkillSelectStatus = 0;
        ContestSelectNum = 0;
        Contest_Name = "";
        Contest_NameHyouji = "";
        Contest_ProblemSentence = "";
        Contest_ProblemSentence2 = "";
        Contest_HallBGName = "";
        Contest_ON = false;
        EventAfter_MoveEnd = false;
        Status_zero_readOK = false;
        OkashiMake_PanelSetType = 0;
        Contest_Clear_Failed = false;
        contest_event_flag = false;
        contest_or_event_flag = false;
        contest_MainMatchStart = false;
        contest_or_contestjudge_flag = false;
        contest_or_limittimeover_flag = false;
        contest_or_prizeget_flag = false;
        Contest_Next_flag = false;
        Contest_PrizeGet_flag = false;
        contest_Rank_Count = 1;
        SceneSelectNum = 0;
        Getmat_return_home = false;
        Money_counterAnim_on = false;
        Money_counterAnim_StartSetting = false;
        Money_counterOnly = false;
        Contest_ReadyToStart = false;
        Contest_ReadyToStart2 = false;
        Contest_afterHomeEventFlag = false;
        Contest_afterHomeHeartUpFlag = false;
        CharacterTouch_ALLOFF = false; ; //キャラの触り判定をオフにする。
        CharacterTouch_ALLON = false;
        BGTouch_ALLOFF = false; //背景オブジェクトの触り判定をオフにする。
        BGTouch_ALLON = false;
        EatAnim_End = false;
        Utage_Prizepanel_ON = false;
        Utage_Prizepanel_WaitHyouji = false;
        Utage_Prizepanel_OFF = false;
        Utage_SceneEnd_BlackON = false;
        Utage_MapMoveBlackON = false;
        Utage_MapMoveON = false;
        Utage_Prizepanel_Type = 0;
        Ajimi_AfterFlag = false;
        Station_TrainGoFlag = false;
        Window_FaceIcon_OnOff = false;
        Window_CharaName = "";
        EatOkashi_DecideFlag = 1; //ランダムで食べたいお菓子決まる
        SPquestPanelOff = false;
        OutEntrance_ON = false;
        ShopEnter_ButtonON = false;
        hiroba_treasureget_flag = false;
        Contest_commentDB_Select = 0;
        Extreme_On = false;
        tempature_control_Param_yakitext = "";
        System_magic_playtime = 2.0f;
        System_magic_playSucess = false;
        System_magic_playON = false;
        Special_OkashiEnshutsuFlag = false;
        MainQuestTitleName = "";

        for (system_i = 0; system_i < check_SleepEnd_Eventflag.Length; system_i++)
        {
            check_SleepEnd_Eventflag[system_i] = false;
        }

        //好感度イベントフラグの初期化
        for (system_i = 0; system_i < GirlLoveEvent_stage1.Length; system_i++)
        {
            GirlLoveEvent_stage1[system_i] = false;
            GirlLoveEvent_stage2[system_i] = false;
            GirlLoveEvent_stage3[system_i] = false;
        }

        //広場NPCイベントフラグの初期化
        for (system_i = 0; system_i < NPCHiroba_eventList.Length; system_i++)
        {
            NPCHiroba_eventList[system_i] = false;
            NPCMagic_eventList[system_i] = false;
        }
        for (system_i = 0; system_i < NPCHiroba_HikarieventList.Length; system_i++)
        {
            NPCHiroba_HikarieventList[system_i] = false;
        }

        //NPC友好度の初期化 50はじまり
        for (system_i = 0; system_i < NPC_FriendPoint.Length; system_i++)
        {
            NPC_FriendPoint[system_i] = 50;
        }
        

        //道端の宝箱リストの初期化
        for (system_i = 0; system_i < Treature_getList.Length; system_i++)
        {
            Treature_getList[system_i] = 0;
        }

        //ブロックリリースフラグリストの初期化
        for (system_i = 0; system_i < NPCHiroba_blockReleaseList.Length; system_i++)
        {
            NPCHiroba_blockReleaseList[system_i] = false;
        }
        


        //好感度サブイベントフラグの初期化
        for (system_i = 0; system_i < GirlLoveSubEvent_stage1.Length; system_i++)
        {
            GirlLoveSubEvent_stage1[system_i] = false;
        }

        //お菓子クエストハイスコアゲットフラグの初期化
        for (system_i = 0; system_i < OkashiQuestHighScore_event.Length; system_i++)
        {
            OkashiQuestHighScore_event[system_i] = false;
        }


        //ビギナーフラグの初期化
        for (system_i = 0; system_i < Beginner_flag.Length; system_i++)
        {
            Beginner_flag[system_i] = false;
        }

        //ショップイベントフラグの初期化
        for (system_i = 0; system_i < ShopEvent_stage.Length; system_i++)
        {
            ShopEvent_stage[system_i] = false;
            ShopLVEvent_stage[system_i] = false;
            FarmEvent_stage[system_i] = false;
            BarEvent_stage[system_i] = false;
        }

        //エメラルドショップイベントフラグの初期化
        for (system_i = 0; system_i < emeraldShopEvent_stage.Length; system_i++)
        {
            emeraldShopEvent_stage[system_i] = false;
        }

        for (system_i = 0; system_i < ShopUwasa_stage1.Length; system_i++)
        {
            ShopUwasa_stage1[system_i] = false;
        }

        //コンテストイベントフラグの初期化
        for (system_i = 0; system_i < ContestEvent_stage.Length; system_i++)
        {
            ContestEvent_stage[system_i] = false;
        }
        for (system_i = 0; system_i < contest_Score.Length; system_i++)
        {
            contest_Score[system_i] = 0;
            contest_Taste_Score[system_i] = 0;
            contest_Beauty_Score[system_i] = 0;
            contest_Sweat_Score[system_i] = 0;
            contest_Bitter_Score[system_i] = 0;
            contest_Sour_Score[system_i] = 0;
            contest_Sweat_Comment[system_i] = "";
            contest_Bitter_Comment[system_i] = "";
            contest_Sour_Comment[system_i] = "";
        }
        contest_okashiName = "";
        contest_okashiNameHyouji = "";
        contest_okashiSubType = "";
        contest_okashiSlotName = "";
        contest_TotalScore = 0;
        contest_TotalScoreList.Clear();
        contest_Disqualification = false;
        contest_PrizeScore = 0;
        contest_BeautyJudgeScore.Clear();
        PrizeScoreAreaList.Clear();
        PrizeItemList.Clear();
        PrizeCharacterList.Clear();
        PrizeGetMoneyList.Clear();
        GetMat_ResultList.Clear();
        ContestItem_supplied_List.Clear();
        ContestItem_supplied_KosuList.Clear();
        contest_boss_name = "";
        Contest_PrizeGet_ItemName = "";
        Contest_PrizeGet_Money = 0;
        special_shogo_flag = false;
        contest_accepted_list.Clear();


        //コンテスト感想初期化
        for (system_i = 0; system_i < contest_judge1_comment.Length; system_i++)
        {
            contest_judge1_comment[system_i] = "";
            contest_judge2_comment[system_i] = "";
            contest_judge3_comment[system_i] = "";
        }

        //ヒカリの作るアイテムリスト初期化
        for (system_i = 0; system_i < hikari_kettei_item.Length; system_i++)
        {
            hikari_kettei_item[system_i] = 0;
            hikari_kettei_originalID[system_i] = "";
            hikari_kettei_kosu[system_i] = 0;
            hikari_kettei_toggleType[system_i] = 0;
            hikari_kettei_itemName[system_i] = "";
        }
        hikari_make_okashiFlag = false;
        hikari_make_okashiID = 0;
        hikari_make_okashi_compID = 0;
        hikari_make_okashiTimeCost = 0;
        hikari_make_okashiTimeCounter = 0;
        hikari_make_doubleItemCreated = 0;
        hikari_make_okashi_totalkyori = 0f;
        hikari_make_okashiKosu = 0;
        hikari_make_success_count = 0;
        hikari_make_failed_count = 0;
        hikari_makeokashi_startcounter = 0;
        hikari_makeokashi_startflag = false;
        hikari_make_Allfailed = false;
        hikari_zairyo_no_flag = false;

        //マップイベントの初期化
        for (system_i = 0; system_i < MapEvent_01.Length; system_i++)
        {
            MapEvent_01[system_i] = false;
            MapEvent_02[system_i] = false;
            MapEvent_03[system_i] = false;
            MapEvent_04[system_i] = false;
            MapEvent_05[system_i] = false;
            MapEvent_06[system_i] = false;
            MapEvent_07[system_i] = false;
            MapEvent_08[system_i] = false;
        }

        for (system_i = 0; system_i < MapEvent_Or.Length; system_i++)
        {
            MapEvent_Or[system_i] = false;
            Or_ShopEvent_stage[system_i] = false;
        }
        

        //通常お菓子感想フラグ
        OkashiComment_flag = false;

        //お菓子感想フラグ
        sp_okashi_hintflag = false;
        sp_okashi_flag = false;
        okashiafter_flag = false;
        okashihint_flag = false;
        okashinontphint_flag = false;
        mainClear_flag = false;
        ExtraClear_flag = false;
        emeralDonguri_flag = false;
        QuestClearButtonMessage_flag = false;

        //お菓子フラグの初期化
        for (system_i = 0; system_i < OkashiQuest_flag_stage1.Length; system_i++)
        {
            OkashiQuest_flag_stage1[system_i] = false;
            OkashiQuest_flag_stage2[system_i] = false;
            OkashiQuest_flag_stage3[system_i] = false;
        }

        SpecialQuestClear_okashiItemID = 200; //デフォルトでねこクッキーを入れる。
        QuestClearflag = false;
        QuestClearButton_anim = false;
        QuestClearAnim_Flag = false;
        QuestClearCommentflag = false;
        Okashi_lastname = "";
        Okashi_lastslot = "";
        Okashi_lasthint = "";
        Okashi_lastshokukan_param = 0;
        Okashi_lastsweat_param = 0;
        Okashi_lastsour_param = 0;
        Okashi_lastbitter_param = 0;
        Okashi_spquest_eatkaisu = 0;
        Okashi_spquest_MaxScore = 0;
        NowEatOkashiID = 0;
        NowEatOkashiName = "";

        //コンテストお菓子初期化
        contest_okashi_ItemData = new Item(9999, "Non", "orange", "Non" + "Non" + " " + "Non", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        "Non", "Non", "Non", "Non", 0, 0, 0, 0, "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", "Non", 0,
                        0, 0, 0, 0, 0, 0, "", 0, 1, 0, 0, 0, 0);

        //お菓子のクリア基準値
        mazui_score = 30;
        low_score = 60;
        high_score = 100;
        high_score_2 = 150;

        //水っぽさなどのマイナス効果の基準
        Watery_Line = 50;

        //チュートリアルフラグ
        tutorial_ON = false;
        tutorial_Progress = false;
        tutorial_Num = 0;

        ending_on = false;
        ending_on2 = false;

        //ステージごとの、始まりの日数
        stage1_start_day = 91;
        stage2_start_day = 121;
        stage3_start_day = 151;

        //ステージごとの締め切りの日数
        stage1_limit_day = 98;
        stage2_limit_day = 151;
        stage3_limit_day = 211;

        //コレクションアイテムリストDBと、登録リスト初期化
        InitCollectionItemsLibrary();
        InitBGAcceItemsLibrary();

        //いちご少女の殿堂入りリスト初期化
        InitIchigoOkashiLibrary();

        //称号リストの初期化
        InitTitleCollectionLibrary();

        //イベントコレクションのリスト
        InitEventCollectionLibrary();

        //コンテストクリアお菓子のリスト
        InitContestClearCollectionLibrary();

        //音楽コレクションのリスト
        InitBgmCollectionLibrary();

        //各サブNPCのお菓子判定番号をセット
        InitSubNPCEvent_OkashiJudgeLibrary();

        //ヒカリのお菓子経験値テーブルをセット
        InitHikariOkashi_ExpTable();

        CollectionItems.Clear();
        for (system_i = 0; system_i < CollectionItemsName.Count; system_i++)
        {
            CollectionItems.Add(false);
        }

        //飾りアイテムの初期化
        /*for (system_i = 0; system_i < DecoItems.Length; system_i++)
        {
            DecoItems[system_i] = false;
        }*/

        //一度に仕上げできる回数
        topping_Set_Count = 1;

        //メインBGMの番号
        mainBGM_Num = 0;
        userBGM_Num = 0;

        Shopday = 0;
        Sale_ON = false;
    }
    
    public static void InitCollectionItemsLibrary()
    {
        CollectionItemsName.Clear();
        CollectionItemsName.Add("amabie_statue");
        CollectionItemsName.Add("green_pendant");
        CollectionItemsName.Add("star_pendant");
        CollectionItemsName.Add("aquamarine_pendant");
        CollectionItemsName.Add("white_lily");
        CollectionItemsName.Add("himmeli");
        CollectionItemsName.Add("kuma_nuigurumi");
        CollectionItemsName.Add("copper_coin");
        CollectionItemsName.Add("compass");
        CollectionItemsName.Add("star_bottle");
    }

    public static void InitBGAcceItemsLibrary()
    {
        //DecoItemsの配列数分まで用意
        BGAcceItemsName.Clear();
        BGAcceItemsName.Add("himmeli", false);
        BGAcceItemsName.Add("kuma_nuigurumi", false);
        BGAcceItemsName.Add("saboten_1", false);
        BGAcceItemsName.Add("saboten_2", false);
        BGAcceItemsName.Add("saboten_3", false);
        BGAcceItemsName.Add("dryflowerpot_1", false);
        BGAcceItemsName.Add("dryflowerpot_2", false);
        BGAcceItemsName.Add("dryflowerpot_3", false);
        BGAcceItemsName.Add("aroma_candle1", false);
        BGAcceItemsName.Add("aroma_candle2", false);

        BGAcceItemsName.Add("mini_house", false);
        BGAcceItemsName.Add("aroma_potion1", false);
        BGAcceItemsName.Add("aroma_potion2", false);
        BGAcceItemsName.Add("aroma_potion3", false);
        BGAcceItemsName.Add("magic_crystal1", false);
        BGAcceItemsName.Add("magic_crystal2", false);
        BGAcceItemsName.Add("magic_crystal3", false);

        /*system_temp_int = DecoItems.Length - BGAcceItemsName.Count;

        for (system_i = 0; system_i < system_temp_int; system_i++)
        {
            BGAcceItemsName.Add("Non");
        }*/
    }

    //いちごお菓子コレクションのリスト　ItemNameとそろえる。
    public static void InitIchigoOkashiLibrary()
    {
        ichigo_collection_list.Clear();
        ichigo_collection_list.Add("strawberry_cookie");
        ichigo_collection_list.Add("rusk_strawberry");
        ichigo_collection_list.Add("strawberry_creampuff");
        ichigo_collection_list.Add("pink_charlotte_donuts");
        ichigo_collection_list.Add("izet_color_donuts");
        ichigo_collection_list.Add("strawberry_crepe");
        ichigo_collection_list.Add("strawberryblueberry_crepe");
        ichigo_collection_list.Add("strawberry_ice_cream");
        ichigo_collection_list.Add("strawberry_parfe");        
        ichigo_collection_list.Add("strawberry_juice");

        ichigo_collection_listFlag = new bool[ichigo_collection_list.Count];
        for (system_i = 0; system_i < ichigo_collection_listFlag.Length; system_i++)
        {
            ichigo_collection_listFlag[system_i] = false;
        }
    }

    //称号コレクションのリスト
    public static void InitTitleCollectionLibrary()
    {
        title_collection_list.Clear();
        title_collection_list.Add(new SpecialTitle(000, "title1", "D:パティシエたまご", false, "Icon/badge_icon_01"));
        title_collection_list.Add(new SpecialTitle(001, "title3", "C:パティシエ一人前", false, "Icon/badge_icon_09"));
        title_collection_list.Add(new SpecialTitle(002, "title4", "B:上級パティシエ", false, "Icon/badge_icon_11"));
        title_collection_list.Add(new SpecialTitle(003, "title5", "A:虹のパティシエ", false, "Icon/badge_icon_10"));
        title_collection_list.Add(new SpecialTitle(004, "title6", "S:グランド・シェフ", false, "Icon/badge_icon_12"));      
        title_collection_list.Add(new SpecialTitle(005, "title100", "深紅-スカーレット-", false, "Icon/badge_icon_02")); //バッチアイコンのスプライトが入っている。
        title_collection_list.Add(new SpecialTitle(006, "title101", "白羽-ホワイトプリム-", false, "Icon/badge_icon_03"));
        title_collection_list.Add(new SpecialTitle(007, "title102", "蒼碧-ブルーヴェール-", false, "Icon/badge_icon_04"));
        title_collection_list.Add(new SpecialTitle(008, "title103", "緑癒-ハイルング-", false, "Icon/badge_icon_05"));
        title_collection_list.Add(new SpecialTitle(009, "title104", "ししゃもマニア", false, "Icon/badge_icon_06"));
        title_collection_list.Add(new SpecialTitle(010, "title105", "ゴールドマスター", false, "Icon/badge_icon_07"));
        title_collection_list.Add(new SpecialTitle(011, "title7", "SS:愛のパティシエ", false, "Icon/badge_icon_08"));

    }

    //称号のnameを入れると、表示用名前を返すメソッド
    public static string SearchTitleCollectionNameString(string _name)
    {
        for (system_i = 0; system_i < title_collection_list.Count; system_i++)
        {
            if(title_collection_list[system_i].titleName == _name)
            {
                return title_collection_list[system_i].titleNameHyouji;
            }
        }

        return ""; //一致しない場合は空
    }

    //称号のnameを入れると、フラグを置き換えるメソッド
    public static void SetTitleCollectionFlag(string _name, bool _flag)
    {
        for (system_i = 0; system_i < title_collection_list.Count; system_i++)
        {
            if (title_collection_list[system_i].titleName == _name)
            {
                title_collection_list[system_i].Flag = _flag;
            }
        }
    }

    //称号のnameを入れると、フラグをゲットするメソッド
    public static bool GetTitleCollectionFlag(string _name)
    {
        for (system_i = 0; system_i < title_collection_list.Count; system_i++)
        {
            if (title_collection_list[system_i].titleName == _name)
            {
                return title_collection_list[system_i].Flag;
            }
        }

        return false;
    }

    //イベントコレクションのリスト
    public static void InitEventCollectionLibrary()
    {
        event_collection_list.Clear();       
               
        event_collection_list.Add(new SpecialTitle(003, "event4", "うまいぞ！にいちゃんのクッキー", false, "EventCG_Icon/cg_gallery_icon_2"));
        event_collection_list.Add(new SpecialTitle(004, "event5", "ラスクとありんこ", false, "EventCG_Icon/cg_gallery_icon_1"));
        event_collection_list.Add(new SpecialTitle(005, "event6", "やもりのねがいごと", false, "EventCG_Icon/cg_gallery_icon_10"));
        event_collection_list.Add(new SpecialTitle(006, "event7", "四つ葉の花かんむり", false, "EventCG_Icon/cg_gallery_icon_4"));
        event_collection_list.Add(new SpecialTitle(007, "event8", "おにいちゃんにキス！", false, "EventCG_Icon/cg_gallery_icon_3"));
        event_collection_list.Add(new SpecialTitle(008, "event9", "ねこちゃんのお墓", false, "EventCG_Icon/cg_gallery_icon_5"));
        event_collection_list.Add(new SpecialTitle(009, "event10", "ままに会いたい", false, "EventCG_Icon/cg_gallery_icon_6"));
        event_collection_list.Add(new SpecialTitle(000, "event1", "きらきらぽんぽん", false, "EventCG_Icon/cg_gallery_icon_7"));
        event_collection_list.Add(new SpecialTitle(001, "event2", "おやすみ", false, "EventCG_Icon/cg_gallery_icon_8"));        
        event_collection_list.Add(new SpecialTitle(002, "event3", "誕生日のクッキー", false, "EventCG_Icon/cg_gallery_icon_9"));

        //デバッグ用
        /*for (system_i = 0; system_i < event_collection_list.Count; system_i++)
        {
            event_collection_list[system_i].Flag = true;
        }*/
    }

    //イベントのnameを入れると、表示用名前を返すメソッド
    public static string SearchEventCollectionNameString(string _name)
    {
        for (system_i = 0; system_i < event_collection_list.Count; system_i++)
        {
            if (event_collection_list[system_i].titleName == _name)
            {
                return event_collection_list[system_i].titleNameHyouji;
            }
        }

        return ""; //一致しない場合は空
    }

    //イベントのnameを入れると、フラグを返すメソッド
    public static bool SearchEventCollectionFlag(string _name)
    {
        for (system_i = 0; system_i < event_collection_list.Count; system_i++)
        {
            if (event_collection_list[system_i].titleName == _name)
            {
                return event_collection_list[system_i].Flag;
            }
        }

        return false; //一致しない場合は空
    }

    //イベントのnameを入れると、フラグを置き換えるメソッド
    public static void SetEventCollectionFlag(string _name, bool _flag)
    {
        for (system_i = 0; system_i < event_collection_list.Count; system_i++)
        {
            if (event_collection_list[system_i].titleName == _name)
            {
                event_collection_list[system_i].Flag = _flag;
            }
        }
    }

    //イベントの現在解放済みをカウントするメソッド
    public static int GetEventCollectionListCount()
    {
        system_count = 0;
        for (system_i = 0; system_i < event_collection_list.Count; system_i++)
        {
            if (event_collection_list[system_i].Flag)
            {
                system_count++;
            }
          
        }
        return system_count;
    }

    //コンテストクリアお菓子コレクションのリスト
    public static void InitContestClearCollectionLibrary()
    {
        contestclear_collection_list.Clear();

        contestclear_collection_list.Add(new SpecialTitle(000, "contestclear1", "クッキーで優勝！", false, "Items/neko_cookie"));
        contestclear_collection_list.Add(new SpecialTitle(001, "contestclear2", "カリカリ！ラスクで優勝！", false, "Items/rusk"));
        contestclear_collection_list.Add(new SpecialTitle(002, "contestclear3", "ふんわり☆クレープで優勝！", false, "Items/crepe"));
        contestclear_collection_list.Add(new SpecialTitle(003, "contestclear4", "さくふわ☆シュークリームで優勝！", false, "Items/creampuff"));
        contestclear_collection_list.Add(new SpecialTitle(004, "contestclear5", "さっくり☆ドーナツで優勝！", false, "Items/donuts_pinkcharlotte"));
        contestclear_collection_list.Add(new SpecialTitle(015, "contestclear16", "マフィンの王様！マフィンで優勝！", false, "Items/maffin_jewery"));
        contestclear_collection_list.Add(new SpecialTitle(016, "contestclear17", "しっとり☆フィナンシェで優勝！", false, "Items/financier"));
        contestclear_collection_list.Add(new SpecialTitle(017, "contestclear18", "ビスコッティで優勝！", false, "Items/biscouti"));
        contestclear_collection_list.Add(new SpecialTitle(018, "contestclear19", "パンケーキマイスタ～！パンケーキで優勝！", false, "Items/pan_cake_maple"));
        contestclear_collection_list.Add(new SpecialTitle(019, "contestclear20", "ほっこり！カステラで優勝！", false, "Items/castella"));
        //contestclear_collection_list.Add(new SpecialTitle(020, "contestclear21", "パンで優勝！", false, "Items/bugget"));
        contestclear_collection_list.Add(new SpecialTitle(005, "contestclear6", "優雅なお茶会マスター！お茶で優勝！", false, "Items/rich_tea"));
        contestclear_collection_list.Add(new SpecialTitle(006, "contestclear7", "健康！ビタミンNo.１！ジュースで優勝！", false, "Items/orange_juice"));
        contestclear_collection_list.Add(new SpecialTitle(007, "contestclear8", "イギリス紳士な朝食を！コーヒーで優勝！", false, "Items/coffee"));
        contestclear_collection_list.Add(new SpecialTitle(008, "contestclear9", "シンプル！素材の味で優勝！", false, "Items/crepe_maple"));
        contestclear_collection_list.Add(new SpecialTitle(009, "contestclear10", "きみも、ジェリーボーイ！ゼリー優勝！", false, "Items/slimejelly"));
        contestclear_collection_list.Add(new SpecialTitle(010, "contestclear11", "乙女のプリンセストータで優勝！", false, "Items/princess_tota"));
        contestclear_collection_list.Add(new SpecialTitle(011, "contestclear12", "オサ～レ☆ティラミスで優勝！", false, "Items/tiramisu"));
        contestclear_collection_list.Add(new SpecialTitle(021, "contestclear22", "夢のレーヴドゥヴィオレッタで優勝！", false, "Items/violatte_suger"));
        contestclear_collection_list.Add(new SpecialTitle(012, "contestclear13", "カンノーリでハードボイルドな優勝！", false, "Items/cannoli"));
        contestclear_collection_list.Add(new SpecialTitle(013, "contestclear14", "アイスクリーム！ユースクリ～ム！で優勝！", false, "Items/icecream"));
        contestclear_collection_list.Add(new SpecialTitle(014, "contestclear15", "憧れのパフェマスタ～！パフェで優勝！", false, "Items/parfe_vanilla"));
        contestclear_collection_list.Add(new SpecialTitle(022, "contestclear23", "キラキラ宝石職人！キャンディで優勝！", false, "Items/jewery_candy"));

        //デバッグ用
        /*for (system_i = 0; system_i < contestclear_collection_list.Count; system_i++)
        {
            contestclear_collection_list[system_i].Flag = true;
        }*/
    }

    //コンテストクリアのnameを入れると、表示用名前を返すメソッド
    public static string SearchContestClearCollectionNameString(string _name)
    {
        for (system_i = 0; system_i < contestclear_collection_list.Count; system_i++)
        {
            if (contestclear_collection_list[system_i].titleName == _name)
            {
                return contestclear_collection_list[system_i].titleNameHyouji;
            }
        }

        return ""; //一致しない場合は空
    }

    //コンテストクリアのnameを入れると、フラグを置き換えるメソッド
    public static void SetContestClearCollectionFlag(string _name, bool _flag)
    {
        for (system_i = 0; system_i < contestclear_collection_list.Count; system_i++)
        {
            if (contestclear_collection_list[system_i].titleName == _name)
            {
                contestclear_collection_list[system_i].Flag = _flag;
            }
        }
    }

    //コンテストクリアのnameを入れると、点数を置き換えるメソッド
    public static void SetContestClearCollectionScore(string _name, int _score)
    {
        for (system_i = 0; system_i < contestclear_collection_list.Count; system_i++)
        {
            if (contestclear_collection_list[system_i].titleName == _name)
            {
                contestclear_collection_list[system_i].Score = _score;
            }
        }
    }

    //コンテストクリアのnameを入れると、そのときの最高得点を取得するメソッド
    public static int GetContestClearCollectionScore(string _name)
    {
        for (system_i = 0; system_i < contestclear_collection_list.Count; system_i++)
        {
            if (contestclear_collection_list[system_i].titleName == _name)
            {
                return contestclear_collection_list[system_i].Score;
            }
        }

        return 0; //_nameなければとりあえず0点
    }

    //コンテストクリアのnameを入れると、アイテムデータを置き換えるメソッド
    public static void SetContestClearCollectionItemData(string _name, Item _itemdata)
    {
        for (system_i = 0; system_i < contestclear_collection_list.Count; system_i++)
        {
            if (contestclear_collection_list[system_i].titleName == _name)
            {
                contestclear_collection_list[system_i].ItemData = _itemdata;
            }
        }
    }

    //音楽コレクションのリスト 音楽リストは、頭から順番にフラグを設定しているので、順番が大事。OptionPanelのBGMリストと順番をそろえる。
    public static void InitBgmCollectionLibrary()
    {
        bgm_collection_list.Clear();

        bgm_collection_list.Add(new SpecialTitle(001, "bgm1", "デフォルト", true, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(002, "bgm2", "太陽のワルツ", true, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(003, "bgm3", "フィオーレ・ファティーナ", true, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(004, "bgm4", "エプロンとワンピース", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(005, "bgm5", "悠久の午後", false, "Items/neko_cookie"));      
        bgm_collection_list.Add(new SpecialTitle(007, "bgm7", "ヴィヴィのアフタヌーンティー", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(008, "bgm8", "白猫街道まっしぐら", false, "Items/neko_cookie"));       
        bgm_collection_list.Add(new SpecialTitle(010, "bgm10", "ちっちゃなパティシエのお菓子作り", true, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(011, "bgm11", "アムルーズ・エマ", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(012, "bgm12", "近くの森", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(014, "bgm14", "ベリーファーム", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(009, "bgm9", "陽だまりの午後", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(013, "bgm13", "いちごのおさんぽ道", false, "Items/neko_cookie"));      
        bgm_collection_list.Add(new SpecialTitle(015, "bgm15", "ひまわりの想い出", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(016, "bgm16", "井戸～Ido～", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(017, "bgm17", "バードサンクチュアリ", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(018, "bgm18", "白猫のお墓<ジムノペディ～第1番～>", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(019, "bgm19", "大広場のカンタービレ", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(023, "bgm23", "プリンのお菓子店", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(024, "bgm24", "モタリケ・ファ～ム", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(025, "bgm25", "クエスト日和", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(006, "bgm6", "ずんたかぽんぽん・マーチ！", true, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(030, "bgm30", "パティシエール・レッスン", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(022, "bgm22", "不思議な3分間クッキング", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(020, "bgm20", "ウェルカム・トゥー・ヒカリのアトリエ", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(021, "bgm21", "風と共に", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(026, "bgm26", "ピクニックだよ！にいちゃん！", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(027, "bgm27", "小さな海の冒険", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(028, "bgm28", "天空の庭", false, "Items/neko_cookie"));
        bgm_collection_list.Add(new SpecialTitle(029, "bgm29", "あったか帰り道", false, "Items/neko_cookie"));       
    }

    //音楽リストのnameを入れると、フラグを置き換えるメソッド
    public static void SetBGMCollectionFlag(string _name, bool _flag)
    {
        for (system_i = 0; system_i < bgm_collection_list.Count; system_i++)
        {
            if (bgm_collection_list[system_i].titleName == _name)
            {
                bgm_collection_list[system_i].Flag = _flag;
            }
        }
    }

    //背景アイテムのnameを入れると、フラグを置き換えるメソッド
    public static void SetBGAcceFlag(string _name, bool _flag)
    {
        if (BGAcceItemsName.ContainsKey(_name))
        {
            BGAcceItemsName[_name] = _flag;
        }
        //Keyが無かった場合は、無視
        else { }
    }

    //各サブイベントのNPCのお菓子判定番号
    public static void InitSubNPCEvent_OkashiJudgeLibrary()
    {
        Mose_Okashi_num01 = 100000; //モーセ
        Shop_Okashi_num01 = 100010; //プリンさん　エクストラ　クエストNo11 お茶会用
        Shop_Okashi_num02 = 100011; //プリンさん　エクストラ　クエストNo11 お茶会用
        Farm_Okashi_num01 = 100020; //モタリケさん　エクストラ
        Bar_Okashi_num01 = 100030; //フィオナさん　エクストラ
    }

    //ヒカリのお菓子経験値テーブル
    public static void InitHikariOkashi_ExpTable()
    {
        Hikariokashi_Exptable.Clear();

        Hikariokashi_Exptable.Add(1, 10);
        Hikariokashi_Exptable.Add(2, 30);
        Hikariokashi_Exptable.Add(3, 70);
        Hikariokashi_Exptable.Add(4, 150);
        Hikariokashi_Exptable.Add(5, 200);
        Hikariokashi_Exptable.Add(6, 300);
        Hikariokashi_Exptable.Add(7, 400);
        Hikariokashi_Exptable.Add(8, 500);
        Hikariokashi_Exptable.Add(9, 9999);

        //少し難しめのお菓子は、レベルも上がりにくくなる。
        /*Hikariokashi_Exptable2.Clear();

        Hikariokashi_Exptable2.Add(1, 30);
        Hikariokashi_Exptable2.Add(2, 70);
        Hikariokashi_Exptable2.Add(3, 150);
        Hikariokashi_Exptable2.Add(4, 270);
        Hikariokashi_Exptable2.Add(5, 450);
        Hikariokashi_Exptable2.Add(6, 800);
        Hikariokashi_Exptable2.Add(7, 1200);
        Hikariokashi_Exptable2.Add(8, 1500);
        Hikariokashi_Exptable2.Add(9, 9999);*/
    }
}

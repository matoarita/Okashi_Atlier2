using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : SingletonMonoBehaviour<GameMgr>
{
    //↑シングルトンにすることで、ゲーム中、GameManagerオブジェクトは、必ず一つのみになる。
    //DontDestroyOnLoad（シーン間で移動してもオブジェクトが削除されない）と併用することで、シーンをまたいでも、常にそのゲームオブジェクトは生き残る。

    //
    //イベント数をセッティング
    //
    public static int GirlLoveEvent_stage_num = 100;
    public static int GirlLoveSubEvent_stage_num = 200;
    public static int Event_num = 30;

    //** --ここまで-- **//

    // ゲーム開始前に呼び出す。デバッグログをオフにする。
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Debug.unityLogger.logEnabled = true; // ←falseでログを止める
    }


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
    public static int stageclear_cullentlove; //クエストをクリアするのに、必要なハートの蓄積量。

    public static bool DEBUG_MODE = true; //デバッグモード　falseだと、デバッグパネルの表示をオフにする。



    /* セーブする */

    public static int scenario_flag;    //全シーンで共通。今、どのシナリオまできているか。クエストやステージではなく、ゲーム自体の進行度を表す。
    public static int ending_count;     //エンディングを迎えた回数
    public static int stage_number;     //ステージ番号　stage1 stage2のこと
    public static int stage_quest_num; //メインのクエスト番号
    public static int stage_quest_num_sub; //クエスト番号

    //セーブしたかどうかを保存しておくフラグ
    public static bool saveOK;

    //初期アイテム取得のフラグ
    public static bool gamestart_recipi_get;

    //現在着ているコスチュームの番号
    public static int Costume_Num;
    public static int[] Accesory_Num = new int[6]; //アクセ番号 現在アクセ数６個

    //飾っているアイテムのリスト
    public static bool[] DecoItems = new bool[10];

    //コレクションに登録したアイテムのリスト
    public static List<bool> CollectionItems = new List<bool>(); //登録済みか否か。こっちはセーブ必要。
    public static List<string> CollectionItemsName = new List<string>(); //登録済みか否か。こっちはセーブ不要。
    public static List<string> BGAcceItemsName = new List<string>(); //背景の置物のリスト。こっちはセーブ不要。

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
    public static int stage1_girl1_loveexp; //ステージ１クリア時の好感度を保存
    public static int stage2_girl1_loveexp;
    public static int stage3_girl1_loveexp;

    public static int stage1_clear_love;
    public static int stage2_clear_love;
    public static int stage3_clear_love;    

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

    //広場でのイベント
    public static bool[] hiroba_event_end = new bool[99]; //イベントを読み終えたかどうかを保存するフラグ。配列順は適当。

    //ショップのイベントリスト
    public static bool[] ShopEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool[] ShopLVEvent_stage = new bool[Event_num]; //パティシエレベルなどに応じたイベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //酒場のイベントリスト
    public static bool[] BarEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //酒場のうわさ話リスト
    public static bool[] ShopUwasa_stage1 = new bool[Event_num]; //うわさ話のリスト。シナリオの進行度に合わせて、リストは変わっていく。５個ずつぐらい？

    //エメラルドショップのイベントリスト
    public static bool[] emeraldShopEvent_stage = new bool[Event_num];

    //牧場のイベントリスト
    public static bool[] FarmEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //コンテストのイベントリスト
    public static bool[] ContestEvent_stage = new bool[Event_num]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //エクストリームパネルのアイテム保存
    public static int sys_extreme_itemID;
    public static int sys_extreme_itemType;

    //お菓子イベントクリアのフラグ
    public static bool[] OkashiQuest_flag_stage1 = new bool[Event_num]; //各イベントのクリアしたかどうかのフラグ。
    public static bool[] OkashiQuest_flag_stage2 = new bool[Event_num];
    public static bool[] OkashiQuest_flag_stage3 = new bool[Event_num];

    public static bool QuestClearflag; //現在のクエストで60点以上だして、クリアしたかどうかのフラグ。
    public static bool QuestClearButton_anim; //クリア初回のみ、ボタンが登場する演出のフラグ。他シーンを移動しても、大丈夫なようにしている。  

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
    public static int Okashi_quest_bunki_on; //特定お菓子のときの条件分岐
    public static bool high_score_flag; //高得点でクリアしたというフラグ。セーブされる。

    public static int Okashi_last_score; //前回あげた最高得点
    public static int Okashi_last_heart; //前回あげたときの最高ハート取得量

    //コンテスト審査員の点数
    public static int[] contest_Score = new int[3];
    public static int contest_TotalScore;
    public static int[] contest_Taste_Score = new int[3];
    public static int[] contest_Beauty_Score = new int[3];

    //お菓子の一度にトッピングできる回数
    public static int topping_Set_Count;

    //オプションの設定　マスター音量など
    public static float MasterVolumeParam;
    public static float BGMVolumeParam;
    public static float SeVolumeParam;

    //現在のメインBGMの番号
    public static int mainBGM_Num;

    //ピクニックイベントのカウンター
    public static bool picnic_End;
    public static int picnic_count;
    public static bool picnic_event_ON;

    //いちごイベントのフラグ
    public static bool hiroba_ichigo_first; //一回でもいちごお菓子をわたした。
    public static bool[] ichigo_collection_listFlag; //いちごのお菓子のコレクションフラグ。
    public static List<string> ichigo_collection_list = new List<string>(); //いちごのお菓子の名前リスト。こっちはセーブ不要。

    //女の子の今のご機嫌状態　gokigen_status（ハートに応じた絶対的な感情）とは別で、すぐに変化するもの 1=最悪 2=ごきげんななめ 3=まあまあ 4=良い 5=上機嫌
    public static int girl_expression;
    public static int girl_express_param; //ご機嫌度合 0~100　で上記の５段階

    /* セーブ　ここまで */



    //広場イベント発生フラグ
    public static bool hiroba_event_flag;   //イベントレシピを見たときに、宴を表示する用のフラグ   
    public static int hiroba_event_placeNum;  //どの場所を選んだか
    public static int hiroba_event_ID; //イベントID

    //通常お菓子を食べた後の感想
    public static int OkashiComment_ID;
    public static bool OkashiComment_flag;

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
    public static int OkashiQuest_Num; //セーブはしなくてもGirlLoveEvent_numからロード時に自動で設定し直すので大丈夫。
    public static string NowEatOkashi; //今食べたいお菓子　girlCompoDBのテキストが入っている。

    //お菓子の点数基準値
    public static int mazui_score;
    public static int low_score;
    public static int high_score;
    public static int high_score_2;    

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
    public static bool contest_event_flag;  //ショップで発生するイベントのフラグ。
    public static int contest_event_num;

    //コンテストに提出したお菓子
    public static string contest_okashiName;
    public static string contest_okashiNameHyouji;
    public static string contest_okashiSlotName;
    public static int contest_okashiID;

    //コンテスト感想
    public static string[] contest_judge1_comment = new string[4];
    public static string[] contest_judge2_comment = new string[4];
    public static string[] contest_judge3_comment = new string[4];

    //エンディングのフラッグ
    public static bool ending_on;       //コンテストメインで、エンディングシーンへ移動するためのフラグ
    public static int ending_number;    //エンディング番号    

    //牧場のイベント発生フラグ
    public static bool farm_event_flag;  //ショップで発生するイベントのフラグ。
    public static int farm_event_num;

    //別シーンから家にもどってきたときに、イベント発生するかどうかのフラグ
    public static bool CompoundEvent_flag;
    public static int CompoundEvent_num; //別シーンから、どのイベントを呼び出すかを、指定する。
    public static bool CompoundEvent_storyflag;
    public static int CompoundEvent_storynum; //メインシーンから、宴に移る際に、どのシナリオを読むかを指定する。

    //その他、一時的なフラグ
    public static int MapSubEvent_Flag;
    public static bool MenuOpenFlag; //メニューを現在開いているか閉じているか
    public static bool QuestManzokuFace; //60点以上取って、喜び表情に変えるフラグ
    public static bool OsotoIkitaiFlag; //外いこうよ～モード。このときに外へいくと、喜んでハートがあがる。
    public static bool picnic_event_reading_now; //ピクニック読み中
    public static bool picnic_after; //ピクニック後、少し余韻にひたる
    public static int picnic_after_time; //余韻にひたる時間

    private PlayerItemList pitemlist;

    public static int system_i;
    private int ev_id;

    private float timeLeft;
    public static int Game_timeCount; //ゲーム内共通の時間

    //ゲームの現在の状態を表すステータス
    public static int compound_status;
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
    public static bool hiroba_event_ON;

    //ゲーム共通の固有の色
    public static string ColorYellow;
    public static string ColorGold;
    public static string ColorLemon;
    public static string ColorPink;
    public static string ColorRed;
    public static string ColorRedDeep;
    public static string ColorBlue;
    public static string ColorCyan;
    public static string ColorOrange;
    public static string ColorGreen;

    //ゲームの通貨名
    public static string MoneyCurrency;
    public static string MoneyCurrencyEn;

    //時間の概念を使用するかどうかのフラグ
    public static bool TimeUSE_FLAG = true; //使用するならtrue

    //一日の食費
    public static int Foodexpenses;
    public static int Foodexpenses_default;
    public static string MgrTodayFood; //今日の食事　セーブはしない   


    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //秒計算。　
        timeLeft = 1.0f;

        //エンディングカウント。リセットされない。
        ending_number = 1;
        ending_count = 0;

        //各イベントフラグ・ゲームパラメーターの初期設定
        ResetGameDefaultStatus();

        //音量設定などの初期値
        MasterVolumeParam = 1.0f;
        BGMVolumeParam = 1.0f;
        SeVolumeParam = 1.0f;

        //各色の設定
        ColorYellow = "<color=#FDFF80>"; // ゴールドに近いくすんだ黄色 #BA9535  かなり薄い黄色 #FDFF80
        ColorGold = "<color=#BA9535>";
        ColorLemon = "<color=#FDFF80>"; // かなり薄い黄色FDFF80
        ColorPink = "<color=#FF5CA1>";
        ColorRed = "<color=#FF0000>";
        ColorRedDeep = "<color=#FF4D4D>";
        ColorBlue = "<color=#0000FF>";
        ColorCyan = "<color=#44A2FF>";
        ColorOrange = "<color=#FF8400>";
        ColorGreen = "<color=48EE72FF>";

        //通貨の名前
        MoneyCurrency = "ルピア";
        MoneyCurrencyEn = "Lp";
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

        compound_status = 0;
        compound_select = 0;

        girl_expression = 3;
        girl_express_param = 50;

        //食費
        Foodexpenses_default = 100;
        Foodexpenses = Foodexpenses_default;

        scenario_flag = 0; //シナリオの進み具合を管理するフラグ。GameMgr.scenario_flagでアクセス可能。
        scenario_ON = false;
        scenario_flag_input = 0;
        scenario_flag_cullent = scenario_flag;

        camerazoom_endflag = false;
        qclear_effect_endflag = false;

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

        shop_event_flag = false;
        shop_lvevent_flag = false;
        shop_event_num = 0;
        talk_flag = false;
        talk_number = 0;
        uwasa_flag = false;
        uwasa_number = 0;
        shop_hint = false;
        shop_hint_num = 0;
        hiroba_event_ON = false;

        farm_event_flag = false;
        farm_event_num = 0;

        bar_event_flag = false;
        bar_event_num = 0;

        emeraldshop_event_flag = false;
        emeraldshop_event_num = 0;

        picnic_End = false;
        picnic_count = 3;
        picnic_event_ON = true;

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

        Okashi_totalscore = 0;
        Okashi_dislike_status = 0;
        Okashi_OnepointHint_num = 0;
        Okashi_quest_bunki_on = 0;
        Okashi_last_score = 0;
        Okashi_last_heart = 0;
        high_score_flag = false;

        sys_extreme_itemID = 9999;
        sys_extreme_itemType = 0;

        stageclear_cullentlove = 0;

        MenuOpenFlag = false;
        QuestManzokuFace = false;
        OsotoIkitaiFlag = false;
        picnic_event_reading_now = false;
        picnic_after = false;
        picnic_after_time = 0;

        //好感度イベントフラグの初期化
        for (system_i = 0; system_i < GirlLoveEvent_stage1.Length; system_i++)
        {
            GirlLoveEvent_stage1[system_i] = false;
            GirlLoveEvent_stage2[system_i] = false;
            GirlLoveEvent_stage3[system_i] = false;            
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
        }
        contest_okashiName = "";
        contest_okashiNameHyouji = "";
        contest_TotalScore = 0;

        //コンテスト感想初期化
        for (system_i = 0; system_i < contest_judge1_comment.Length; system_i++)
        {
            contest_judge1_comment[system_i] = "";
            contest_judge2_comment[system_i] = "";
            contest_judge3_comment[system_i] = "";
        }

        //マップイベントの初期化
        for (system_i = 0; system_i < MapEvent_01.Length; system_i++)
        {
            MapEvent_01[system_i] = false;
            MapEvent_02[system_i] = false;
            MapEvent_03[system_i] = false;
            MapEvent_04[system_i] = false;
            MapEvent_05[system_i] = false;
            MapEvent_06[system_i] = false;
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
        emeralDonguri_flag = false;
        QuestClearButtonMessage_flag = false;

        //お菓子フラグの初期化
        for (system_i = 0; system_i < OkashiQuest_flag_stage1.Length; system_i++)
        {
            OkashiQuest_flag_stage1[system_i] = false;
            OkashiQuest_flag_stage2[system_i] = false;
            OkashiQuest_flag_stage3[system_i] = false;
        }
        OkashiQuest_Num = 0;
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

        //お菓子のクリア基準値
        mazui_score = 30;
        low_score = 60;
        high_score = 100;
        high_score_2 = 150;

        //チュートリアルフラグ
        tutorial_ON = false;
        tutorial_Progress = false;
        tutorial_Num = 0;

        ending_on = false;

        //ステージごとの、クリア好感度の数値設定。現在は未使用
        stage1_clear_love = 100;
        stage2_clear_love = 200;
        stage3_clear_love = 450;

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

        CollectionItems.Clear();
        for (system_i = 0; system_i < CollectionItemsName.Count; system_i++)
        {
            CollectionItems.Add(false);
        }

        //飾りアイテムの初期化
        for (system_i = 0; system_i < DecoItems.Length; system_i++)
        {
            DecoItems[system_i] = false;
        }

        //一度に仕上げできる回数
        topping_Set_Count = 1;

        //メインBGMの番号
        mainBGM_Num = 0;
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
        BGAcceItemsName.Clear();
        BGAcceItemsName.Add("himmeli");
        BGAcceItemsName.Add("kuma_nuigurumi");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
        BGAcceItemsName.Add("Non");
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
        ichigo_collection_list.Add("strawberry_parfe");
        ichigo_collection_list.Add("strawberry_ice_cream");
        ichigo_collection_list.Add("strawberry_juice");

        ichigo_collection_listFlag = new bool[ichigo_collection_list.Count];
        for (system_i = 0; system_i < ichigo_collection_listFlag.Length; system_i++)
        {
            ichigo_collection_listFlag[system_i] = false;
        }
    }
}

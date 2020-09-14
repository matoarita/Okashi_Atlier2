using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : SingletonMonoBehaviour<GameMgr>
{
    //↑シングルトンにすることで、ゲーム中、GameManagerオブジェクトは、必ず一つのみになる。
    //DontDestroyOnLoad（シーン間で移動してもオブジェクトが削除されない）と併用することで、シーンをまたいでも、常にそのゲームオブジェクトは生き残る。

    public static int scenario_flag;    //全シーンで共通。今、どのシナリオまできているか。
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

    //イベントフラグ
    public static int GirlLoveEvent_num;            //女の子の好感度に応じて発生するイベントの、イベント番号
    public static bool girlloveevent_flag;          //女の子の好感度に応じて発生するイベントのフラグ
    public static bool girlloveevent_endflag;       //宴で読み終了したときのフラグ

    public static bool[] GirlLoveEvent_stage1 = new bool[30];  //各イベントの、現在読み中かどうかのフラグ。
    public static bool[] GirlLoveEvent_stage2 = new bool[30];
    public static bool[] GirlLoveEvent_stage3 = new bool[30];

    //好感度やパティシエレベルで発生するサブイベントのフラグ
    public static int GirlLoveSubEvent_num;
    public static int girlloveevent_bunki; //メインイベントかサブイベントかを分岐する
    public static bool[] GirlLoveSubEvent_stage1 = new bool[30];

    //マップイベント
    public static int　map_ev_ID;           //その時のイベント番号
    public static bool map_event_flag;      //マップイベントの、宴を表示する用のフラグ

    public static bool[] MapEvent_01 = new bool[20];         //各エリアのマップイベント。一度読んだイベントは、発生しない。近くの森。
    public static bool[] MapEvent_02 = new bool[20];         //井戸。
    public static bool[] MapEvent_03 = new bool[20];         //ストロベリーガーデン
    public static bool[] MapEvent_04 = new bool[20];         //ひまわりの丘
    public static bool[] MapEvent_05 = new bool[20];         //ラベンダー畑

    //広場でのイベント
    public static bool hiroba_event_flag;   //イベントレシピを見たときに、宴を表示する用のフラグ
    public static bool[] hiroba_event_end = new bool[99]; //イベントを読み終えたかどうかを保存するフラグ。配列順は適当。
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
    public static bool emeralDonguri_flag;  //高得点時、エメラルどんぐりをくれるイベント発生のフラグ

    //妹の口をクリックしたときのヒント表示フラグ
    public static int touchhint_ID;
    public static bool touchhint_flag;

    //寝るイベントフラグ
    public static bool sleep_flag;
    public static int sleep_status;

    //お菓子イベントクリアのフラグ
    public static bool[] OkashiQuest_flag_stage1 = new bool[30]; //各イベントのクリアしたかどうかのフラグ。
    public static bool[] OkashiQuest_flag_stage2 = new bool[30];
    public static bool[] OkashiQuest_flag_stage3 = new bool[30];

    public static bool QuestClearflag; //現在のクエストで60点以上だして、クリアしたかどうかのフラグ。
    public static bool clear_spokashi_flag; //SPお菓子でクリアしたか、好感度あげてクリアしたかどうか。

    //お菓子イベント現在のナンバー
    public static int OkashiQuest_Num;

    //お菓子の点数基準値
    public static int low_score;
    public static int high_score;

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

    public static int stage_number;     //ステージ番号

    public static int stage1_girl1_loveexp; //ステージ１クリア時の好感度を保存
    public static int stage2_girl1_loveexp;
    public static int stage3_girl1_loveexp;

    public static int stage1_clear_love;
    public static int stage2_clear_love;
    public static int stage3_clear_love;

    public static int stage1_start_day;
    public static int stage2_start_day;
    public static int stage3_start_day;

    public static int stage1_limit_day;
    public static int stage2_limit_day;
    public static int stage3_limit_day;

    //ショップのイベントリスト
    public static bool[] ShopEvent_stage = new bool[30]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool[] ShopLVEvent_stage = new bool[30]; //パティシエレベルなどに応じたイベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。

    //ショップのうわさ話リスト
    public static bool[] ShopUwasa_stage1 = new bool[30]; //うわさ話のリスト。シナリオの進行度に合わせて、リストは変わっていく。５個ずつぐらい？

    //コンテストのイベントリスト
    public static bool[] ContestEvent_stage = new bool[30]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool contest_event_flag;  //ショップで発生するイベントのフラグ。
    public static int contest_event_num;

    //コンテストに提出したお菓子
    public static string contest_okashiName;
    public static string contest_okashiSlotName;

    //コンテスト審査員の点数
    public static int[] contest_Score = new int[3];
    public static int contest_TotalScore;

    //エンディングのフラッグ
    public static bool ending_on;

    //牧場のイベントリスト
    public static bool[] FarmEvent_stage = new bool[30]; //各イベント読んだかどうかのフラグ。一度読めばONになり、それ以降発生しない。
    public static bool farm_event_flag;  //ショップで発生するイベントのフラグ。
    public static int farm_event_num;

    //別シーンから家にもどってきたときに、イベント発生するかどうかのフラグ
    public static bool CompoundEvent_flag;
    public static int CompoundEvent_num; //別シーンから、どのイベントを呼び出すかを、指定する。
    public static bool CompoundEvent_storyflag;
    public static int CompoundEvent_storynum; //メインシーンから、宴に移る際に、どのシナリオを読むかを指定する。

    private PlayerItemList pitemlist;

    public static int system_i;
    private int ev_id;

    private float timeLeft;
    public static int Game_timeCount; //ゲーム内共通の時間

    //チュートリアル用の管理フラグ
    public static bool tutorial_ON;         //これがONになったら、ゲーム全体がチュートリアルモードになる。 
    public static bool tutorial_Progress;   //進行したときにフラグをたてる。すると、次のテキストが流れる。
    public static int tutorial_Num;         //チュートリアルの進行度

    //ステージ最初の読み込みフラグ
    public static bool stage1_load_ok;

    //初期アイテム取得のフラグ
    public static bool gamestart_recipi_get;

    //エクストリームパネルのアイテム保存
    public static int sys_extreme_itemID;
    public static int sys_extreme_itemType;

    //ロード「続きから」を押したフラグ
    public static bool GameLoadOn;

    //キー入力受付開始のフラグ
    public static bool KeyInputOff_flag;

    //オプションの設定　マスター音量など
    public static float MasterVolumeParam;

    //ゲーム共通の固有の色
    public static string ColorYellow;
    public static string ColorGold;
    public static string ColorLemon;
    public static string ColorPink;
    public static string ColorRed;
    public static string ColorBlue;
    public static string ColorCyan;
    public static string ColorOrange;
    public static string ColorGreen;

    public static bool Scene_back_home; //シーンから、メイン画面にもどるときの、ドア開閉時の音を鳴らす用のフラグ。


    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //秒計算。　
        timeLeft = 1.0f;

        //各イベントフラグ・ゲームパラメーターの初期設定
        ResetGameDefaultStatus();

        //音量設定などの初期値
        MasterVolumeParam = 1.0f;

        //各色の設定
        ColorYellow = "<color=#FDFF80>"; // ゴールドに近いくすんだ黄色 #BA9535  かなり薄い黄色 #FDFF80
        ColorGold = "<color=#BA9535>";
        ColorLemon = "<color=#FDFF80>"; // かなり薄い黄色FDFF80
        ColorPink = "<color=#FF5CA1>";
        ColorRed = "<color=#FF0000>";
        ColorBlue = "<color=#0000FF>";
        ColorCyan = "<color=#44A2FF>";
        ColorOrange = "<color=#FF8400>";
        ColorGreen = "<color=48EE72FF>";
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

        scenario_flag = 0; //シナリオの進み具合を管理するフラグ。GameMgr.scenario_flagでアクセス可能。
        scenario_ON = false;
        scenario_flag_input = 0;
        scenario_flag_cullent = scenario_flag;

        event_recipi_flag = false;
        event_recipi_endflag = false;

        recipi_read_flag = false;
        recipi_read_endflag = false;

        touchhint_flag = false;

        itemuse_recipi_flag = false;
        map_event_flag = false;

        gamestart_recipi_get = false;

        shop_event_flag = false;
        shop_lvevent_flag = false;
        shop_event_num = 0;
        talk_flag = false;
        talk_number = 0;
        uwasa_flag = false;
        uwasa_number = 0;
        shop_hint = false;
        shop_hint_num = 0;

        farm_event_flag = false;
        farm_event_num = 0;

        hiroba_event_flag = false;
        //広場イベント読み終えたフラグの初期化
        for (system_i = 0; system_i < hiroba_event_end.Length; system_i++)
        {
            hiroba_event_end[system_i] = false;
        }

        stage_number = 1;

        stage1_load_ok = false;

        Scene_back_home = false;

        
        Game_timeCount = 0; //1秒タイマー

        GirlLoveEvent_num = 0;
        girlloveevent_flag = false;
        girlloveevent_endflag = false;

        sleep_flag = false;
        sleep_status = 0;
        scenario_read_endflag = false;
        KeyInputOff_flag = false;

        CompoundEvent_flag = false;
        CompoundEvent_num = 0;
        CompoundEvent_storyflag = false;
        CompoundEvent_storynum = 0;

        sys_extreme_itemID = 9999;
        sys_extreme_itemType = 0;

        //好感度イベントフラグの初期化
        for (system_i = 0; system_i < GirlLoveEvent_stage1.Length; system_i++)
        {
            GirlLoveEvent_stage1[system_i] = false;
            GirlLoveEvent_stage2[system_i] = false;
            GirlLoveEvent_stage3[system_i] = false;

            GirlLoveSubEvent_stage1[system_i] = false;
        }

        //ショップイベントフラグの初期化
        for (system_i = 0; system_i < GirlLoveEvent_stage1.Length; system_i++)
        {
            ShopEvent_stage[system_i] = false;
            ShopLVEvent_stage[system_i] = false;
            FarmEvent_stage[system_i] = false;
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
        }
        contest_okashiName = "";
        contest_TotalScore = 0;

        //マップイベントの初期化
        for (system_i = 0; system_i < MapEvent_01.Length; system_i++)
        {
            MapEvent_01[system_i] = false;
            MapEvent_02[system_i] = false;
            MapEvent_03[system_i] = false;
            MapEvent_04[system_i] = false;
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

        //お菓子フラグの初期化
        for (system_i = 0; system_i < OkashiQuest_flag_stage1.Length; system_i++)
        {
            OkashiQuest_flag_stage1[system_i] = false;
            OkashiQuest_flag_stage2[system_i] = false;
            OkashiQuest_flag_stage3[system_i] = false;
        }
        OkashiQuest_Num = 0;
        QuestClearflag = false;
        clear_spokashi_flag = false;

        //お菓子のクリア基準値
        low_score = 60;
        high_score = 85;

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
        
    }
    
}

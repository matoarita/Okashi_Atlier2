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
    public int scenario_flag_input;     //デバッグ用。シナリオフラグをインスペクタから入力
    public int scenario_flag_cullent;   //デバッグ用。現在のシナリオフラグを確認用

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

    public static bool GirlLoveEvent_01;            //各イベントの、読んだかどうかのチェック用フラグ。一度読んだイベントは、発生しない。

    //マップイベント
    public static int　map_ev_ID;           //その時のイベント番号
    public static bool map_event_flag;      //マップイベントの、宴を表示する用のフラグ

    public static bool MapEvent_01;         //マップイベント。一度読んだイベントは、発生しない。近くの森へはじめてきた。
    public static bool MapEvent_02;         //井戸へはじめてきた。
    public static bool MapEvent_03;         //森へきたとき。草笛のイベント

    //通常お菓子を食べた後の感想
    public static int OkashiComment_ID;
    public static bool OkashiComment_flag;

    //スペシャルお菓子を食べる前の会話フラグ
    public static bool sp_okashi_hintflag;

    //スペシャルお菓子を食べた後の感想フラグ
    public static int sp_okashi_ID;
    public static bool sp_okashi_flag;
    public static int mainquest_ID;
    public static bool mainClear_flag;

    //寝るイベントフラグ
    public static bool sleep_flag;

    //お菓子イベントクリアのフラグ
    public static bool[] OkashiQuest_flag = new bool[100];
    //00 = オリジナルクッキークリア
    //01 = ラスククリア

    //ショップの話すコマンド
    public static bool talk_flag;       //ショップの「話す」コマンドをONにしたとき、これがONになり、宴の会話が優先される。NPCなどでも使う。
    public static int talk_number;      //その時の会話番号。
    public static bool shop_hint;

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

    private PlayerItemList pitemlist;

    private int i, j;
    private int ev_id;

    private float timeLeft;
    public static int Game_timeCount; //ゲーム内共通の時間

    //チュートリアル用の管理フラグ
    public static bool tutorial_ON;         //これがONになったら、ゲーム全体がチュートリアルモードになる。 
    public static bool tutorial_Progress;   //進行したときにフラグをたてる。すると、次のテキストが流れる。
    public static int tutorial_Num;         //チュートリアルの進行度

    //ステージ最初の読み込みフラグ
    public static bool stage1_load_ok;

    //イベントフラグ管理用
    [SerializeField]
    private bool gamestart_recipi_get;

    //ゲーム共通の固有の色
    public static string ColorYellow;
    public static string ColorLemon;
    public static string ColorPink;
    public static string ColorRed;
    public static string ColorBlue;
    public static string ColorCyan;
    public static string ColorOrange;


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        scenario_flag = 0; //シナリオの進み具合を管理するフラグ。GameMgr.scenario_flagでアクセス可能。
        scenario_ON = false;
        scenario_flag_input = 0;
        scenario_flag_cullent = scenario_flag;

        event_recipi_flag = false;
        event_recipi_endflag = false;

        recipi_read_flag = false;
        recipi_read_endflag = false;

        itemuse_recipi_flag = false;
        map_event_flag = false;

        gamestart_recipi_get = false;

        talk_flag = false;
        talk_number = 0;
        shop_hint = false;

        stage_number = 1;

        stage1_load_ok = false;

        //秒計算。　
        timeLeft = 1.0f;
        Game_timeCount = 0; //1秒タイマー

        GirlLoveEvent_num = 0;       
        girlloveevent_flag = false;
        girlloveevent_endflag = false;

        sleep_flag = false;
        scenario_read_endflag = false;

        //好感度イベントフラグの初期化
        GirlLoveEvent_01 = false;

        //マップイベントの初期化
        MapEvent_01 = false;

        //通常お菓子感想フラグ
        OkashiComment_flag = false;

        //お菓子感想フラグ
        sp_okashi_hintflag = false;
        sp_okashi_flag = false;

        //お菓子フラグの初期化
        for (i = 0; i < OkashiQuest_flag.Length; i++) {
            OkashiQuest_flag[i] = false;
        }

        //チュートリアルフラグ
        tutorial_ON = false;
        tutorial_Progress = false;
        tutorial_Num = 0;

        //ステージごとの、クリア好感度の数値設定
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
        
        //各色の設定
        ColorYellow = "<color=#BA9535>"; // ゴールドに近いくすんだ黄色
        ColorLemon = "<color=#FDFF80>"; // かなり薄い黄色FDFF80
        ColorPink = "<color=#FF5CA1>";
        ColorRed = "<color=#FF0000>";
        ColorBlue = "<color=#0000FF>";
        ColorCyan = "<color=#44A2FF>";
        ColorOrange = "<color=#FF8400>";
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


        //ゲーム中にイベントで入手したアイテムの管理

        //ゲームの一番最初に絶対手に入れるレシピ
        if (gamestart_recipi_get != true)
        {
            if (scenario_flag == 110)
            {
                ev_id = Find_eventitemdatabase("najya_start_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //ナジャの基本のレシピを追加


                ev_id = Find_eventitemdatabase("ev01_neko_cookie_recipi");
                pitemlist.add_eventPlayerItem(ev_id, 1); //クッキーのレシピを追加

                gamestart_recipi_get = true; //ゲットしたよフラグをONに。
            }
        }
    }

    //アイテム名を入力すると、該当するeventitem_IDを返す処理
    public int Find_eventitemdatabase(string compo_itemname)
    {
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (compo_itemname == pitemlist.eventitemlist[j].event_itemName)
            {
                return j;
            }
            j++;
        }

        return 9999; //該当するIDがない場合
    }


}

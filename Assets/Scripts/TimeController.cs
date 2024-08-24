using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TimeController : SingletonMonoBehaviour<TimeController>
{

    //時間の概念を使用するかどうかは、GameMgr.csに記述  使用しないときは、TimePanelオブジェクトもオフにする
    //TimeControllerは、時間を計算するだけ。描画は、スクリプトを分けている。TimePanelで取得して、描画してる。

    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_main;

    private GirlEat_Judge girleat_judge;

    private HikariOkashiExpTable hikariOkashiExpTable;

    private Compound_Keisan compound_keisan;

    private Girl1_status girl1_status;

    private PlayerItemList pitemlist;

    private ItemDataBase database; 

    private List<int> calender = new List<int>();
    private int _cullent_time;
    private int _cullent_hour;
    private int _cullent_minute;
    private int _stage_limit_day;
    private int month, day;
    private int hour, minute;
    private int cullent_hour_clock;

    private int limit_month, limit_day;

    public int timeIttei; //表示用にpublicにしてるだけ。
    public int timeIttei2;
    public int timeIttei3;
    public int timeIttei4;
    public int timeIttei5;
    public int timeIttei6;
    public int timeIttei7;
    public int timeIttei8;
    public bool timeDegHeart_flag; //表示用にpublicにしてるだけ。

    private int i, count;
    private int dice;

    private int _getexp;

    private float timeLeft;
    private float timeLeft2;
    private bool count_switch;

    private bool itemkosu_check;

    private float timespeed_range;

    private int total_day;

    public bool TimeCheck_flag; //調合メインメソッドのトップ画面で起動開始
    public bool TimeReturnHomeSleep_Status; //兄が帰ってきたあと、少しセリフ変わる。 

    private GameObject DebugTimecountUp_button;
    private GameObject DebugTimecountDown_button;

    private int heart_countup_time;
    private int heart_up_auto_param;
    private int manpuku_deg_param;

    private int Start_count;
    private bool Zairyo_nothing;

    // Use this for initialization
    void Start()
    {
        InitParam();

        timeIttei = 0;
        timeIttei3 = 0;
        timeIttei4 = 0;
        timeIttei5 = 0;
        timeIttei6 = 0;
        timeIttei7 = 0;
        timeIttei8 = 0;

        timeDegHeart_flag = false;
        TimeCheck_flag = false;
        TimeReturnHomeSleep_Status = false;
    }

    void InitParam()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //合成計算オブジェクトの取得
        compound_keisan = Compound_Keisan.Instance.GetComponent<Compound_Keisan>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //ヒカリお菓子EXPデータベースの取得
        hikariOkashiExpTable = HikariOkashiExpTable.Instance.GetComponent<HikariOkashiExpTable>();        

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子
        girleat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        //カレンダー初期化
        calender.Clear();

        calender.Add(31); //１月
        calender.Add(28); //２月
        calender.Add(31); //３月
        calender.Add(30); //４月
        calender.Add(31); //５月
        calender.Add(30); //６月
        calender.Add(31); //７月
        calender.Add(31); //８月
        calender.Add(30); //９月
        calender.Add(31); //１０月
        calender.Add(30); //１１月
        calender.Add(31); //１２月       
      
        timespeed_range = 1.0f;       
    }

    private void OnEnable()
    {
        //InitParam();
    }

    // Update is called once per frame
    void Update()
    {
        //セットアップ
        if (GameMgr.Scene_LoadedOn_End) //シーン読み込み完了してから動き出す
        {
            if (canvas == null)
            {
                InitParam();               
               
            }

            switch (GameMgr.Scene_Category_Num)
            {
                case 10: //調合シーン

                    if (compound_Main_obj == null)
                    {
                        //Debug.Log("このタイミングでcompound_Main TimeController");
                        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                        compound_main = compound_Main_obj.GetComponent<Compound_Main>();

                    }
                    break;

            }

            TimeKoushin(0); //時間の更新　初期
        }

        

        switch (GameMgr.Scene_Category_Num)
        {
            case 10: //調合シーンでは時間を計算する

                //時間のカウント
                timeLeft -= Time.deltaTime;
                timeLeft2 += Time.deltaTime;

                //1秒ごとのタイムカウンター
                if (timeLeft <= 0.0)
                {
                    GameSpeedRange(); //ゲームスピードパラメータの変更。

                    timeLeft = 1.0f; //現実の1秒の時間。
                    count_switch = !count_switch;

                    //試験的に導入。秒ごとにリアルタイムに時間がすすみ、ハートが減っていく。
                    if (!GameMgr.scenario_ON)
                    {
                        if (GameMgr.compound_status == 110) //採集中やステータス画面など開いてるときは減らない
                        {
                            timeIttei++;
                            if (PlayerStatus.player_girl_expression == 1) //まずい状態直後、ハートが自動で下がる状態に。
                            {
                                //試験的に導入。秒ごとにリアルタイムに時間がすすみ、ハートが減っていく。
                                RealTimeHeartControll();
                            }
                        }
                    }                   
                }

                //フリーモードのときは、時間がリアルタイムで経過　timeLeft2が更新されると、ゲーム時間が5分進む。
                //RealTime_Method(); //アトリエ２では、off

                break;

            case 100: //コンテスト中は、リアルタイムに1sずつ減っていく。

                if (GameMgr.System_Contest_RealTimeProgress_ON)
                {
                    if (GameMgr.contest_MainMatchStart)
                    {
                        //時間のカウント
                        timeLeft -= Time.deltaTime;

                        //1秒ごとのタイムカウンター
                        if (timeLeft <= 0.0)
                        {
                            GameSpeedRange(); //ゲームスピードパラメータの変更。

                            timeLeft = 1.0f; //現実の1秒の時間。
                            count_switch = !count_switch;

                            PlayerStatus.player_contest_second++;
                            if (PlayerStatus.player_contest_second >= 60) //1分たった。
                            {
                                PlayerStatus.player_contest_second = 0;
                                SetMinuteToHourContest(1);
                            }

                        }
                    }
                }
                break;
        }
    }

    void RealTime_Method()
    {
        if (GameMgr.Story_Mode == 1)
        {
            if (!GameMgr.scenario_ON)
            {
                if (GameMgr.compound_status == 110 && GameMgr.compound_select == 0)  //採集中やステータス画面など開いてるときは減らない
                {
                    if (!GameMgr.ReadGirlLoveTimeEvent_reading_now) //特定の時間イベント読み中の間もoffに。
                    {

                        if (timeLeft2 >= 10f * timespeed_range) //timeLeft2は、現実の1秒で1.0ずつ増加する。10カウント＝10秒。そこからtimespeed_rangeで更新時間を早めている。
                        {

                            timeLeft2 = 0.0f;
                            SetMinuteToHour(5); //5分
                            TimeKoushin(0);

                            compound_main.Weather_Change(5.0f);


                            //サブ時間イベントをチェック
                            /*if (GameMgr.ResultOFF) //リザルト画面表示中は、時間イベントは発生しない
                            {

                            }
                            else
                            {
                                GameMgr.check_GirlLoveTimeEvent_flag = false;
                            }*/

                            //ヒカリがお菓子を作ってる場合、ここでお菓子制作時間を計算
                            if (!GameMgr.outgirl_Nowprogress)
                            {
                                heart_up_auto_param = 1; //自動でハート上がる量　デフォルト

                                if (pitemlist.KosuCount("aroma_potion3") >= 1)
                                {
                                    heart_up_auto_param += 2;
                                }
                                if (pitemlist.KosuCount("aroma_potion2") >= 1)
                                {
                                    heart_up_auto_param += 1;
                                }

                                if (GameMgr.hikari_make_okashiFlag)
                                {
                                    GameMgr.hikari_make_okashiTimeCounter -= 5 * GameMgr.TimeStep;
                                    if (GameMgr.hikari_make_okashiTimeCounter <= 0) //カウンタが0になると、制作完了　トータルの制作時間を再度入れなおす                                                                                                                             
                                    {
                                        GameMgr.hikari_make_okashiTimeCounter = GameMgr.hikari_make_okashiTimeCost;

                                        //お菓子制作。材料チェックと成功率を計算する。
                                        HikariMakeOkashiJudge();

                                    }

                                    //お菓子を作ってる間、ハート上がる。
                                    timeIttei4++;
                                    if (PlayerStatus.player_girl_manpuku >= 10)
                                    {
                                        heart_countup_time = 6; //デフォルト

                                        if (pitemlist.KosuCount("memory_feather3") >= 1)
                                        {
                                            heart_countup_time = 2;
                                        }
                                        else if (pitemlist.KosuCount("memory_feather2") >= 1)
                                        {
                                            heart_countup_time = 3;
                                        }
                                        else if (pitemlist.KosuCount("memory_feather1") >= 1)
                                        {
                                            heart_countup_time = 4;
                                        }


                                        if (PlayerStatus.player_girl_expression >= 4) //機嫌は5段階
                                        {
                                            if (timeIttei4 >= heart_countup_time) //30分ごとに。
                                            {
                                                timeIttei4 = 0;

                                                girleat_judge.UpDegHeart(heart_up_auto_param, false);
                                            }
                                        }
                                        else if (PlayerStatus.player_girl_expression == 3)
                                        {
                                            if (timeIttei4 >= heart_countup_time * 2) //60分ごとに。
                                            {
                                                timeIttei4 = 0;

                                                girleat_judge.UpDegHeart(heart_up_auto_param, false);
                                            }
                                        }
                                    }
                                }
                                else //ヒカリがお菓子を作ってないときは、アイテムのみハート上がる。
                                {
                                    timeIttei4++;
                                    if (PlayerStatus.player_girl_manpuku >= 10)
                                    {
                                        heart_countup_time = 12; //デフォルト

                                        if (pitemlist.KosuCount("memory_feather3") >= 1)
                                        {
                                            heart_countup_time = 4;
                                        }
                                        else if (pitemlist.KosuCount("memory_feather2") >= 1)
                                        {
                                            heart_countup_time = 6;
                                        }
                                        else if (pitemlist.KosuCount("memory_feather1") >= 1)
                                        {
                                            heart_countup_time = 8;
                                        }

                                        if (PlayerStatus.player_girl_expression >= 4) //機嫌は5段階
                                        {
                                            if (timeIttei4 >= heart_countup_time) //60分ごとに。
                                            {
                                                timeIttei4 = 0;

                                                girleat_judge.UpDegHeart(heart_up_auto_param, false);
                                            }
                                        }
                                    }
                                }

                                //にんじん等のアイテムで、お菓子を作っていなくても常にハートが上がる
                                timeIttei5++;
                                timeIttei6++;
                                timeIttei7++;
                                timeIttei8++;
                                if (PlayerStatus.player_girl_manpuku >= 10)
                                {
                                    if (PlayerStatus.player_girl_expression >= 3) //機嫌は5段階
                                    {
                                        if (pitemlist.KosuCount("pink_ninjin") >= 1)
                                        {
                                            if (timeIttei5 >= 24) //120分ごとに。
                                            {
                                                timeIttei5 = 0;

                                                girleat_judge.UpDegHeart(1, false);
                                            }
                                        }

                                        if (GameMgr.BGAcceItemsName["saboten_1"])
                                        {
                                            if (timeIttei6 >= 24) //120分ごとに。
                                            {
                                                timeIttei6 = 0;

                                                girleat_judge.UpDegHeart(1, false);
                                            }
                                        }
                                        else if (GameMgr.BGAcceItemsName["saboten_2"])
                                        {
                                            if (timeIttei6 >= 12) //60分ごとに。
                                            {
                                                timeIttei6 = 0;

                                                girleat_judge.UpDegHeart(1, false);
                                            }
                                        }
                                        else if (GameMgr.BGAcceItemsName["saboten_3"])
                                        {
                                            if (timeIttei6 >= 6) //30分ごとに。
                                            {
                                                timeIttei6 = 0;

                                                girleat_judge.UpDegHeart(1, false);
                                            }
                                        }

                                        if (pitemlist.KosuCount("angel_statue1") >= 1)
                                        {
                                            if (timeIttei7 >= 4) //20分ごとに。
                                            {
                                                timeIttei7 = 0;

                                                girleat_judge.UpDegHeart(1, false);
                                            }
                                        }

                                        if (pitemlist.KosuCount("angel_statue2") >= 1)
                                        {
                                            if (timeIttei8 >= 4) //20分ごとに。
                                            {
                                                timeIttei8 = 0;

                                                girleat_judge.UpDegHeart(2, false);
                                            }
                                        }
                                    }
                                }
                            }

                            //5分を基準に腹もへる。
                            if (!GameMgr.outgirl_Nowprogress)
                            {
                                timeIttei3++;

                                manpuku_deg_param = 3; //満腹が減る時間間隔　デフォルト 15分 効果は重複する。

                                //アイテムによって満腹度は減りにくくなる。
                                if (pitemlist.KosuCount("hikari_manpuku_deg2") >= 1)
                                {
                                    manpuku_deg_param = manpuku_deg_param * 3;
                                }
                                if (pitemlist.KosuCount("hikari_manpuku_deg1") >= 1)
                                {
                                    manpuku_deg_param = manpuku_deg_param * 2;
                                }

                                //さらに、食べたいお菓子あげて一定時間満腹減少状態になってるとき。重複する。
                                if (pitemlist.KosuCount("hikari_manpuku_deg3") >= 1)
                                {
                                    if (GameMgr.hikari_tabetaiokashi_buf)
                                    {
                                        manpuku_deg_param = manpuku_deg_param * 2;
                                    }
                                }

                                //ねこバッジを持ってる数だけ、さらにお腹が減りにくくなる。
                                if (pitemlist.KosuCount("neko_badge3") >= 1)
                                {
                                    manpuku_deg_param = manpuku_deg_param + (1 * pitemlist.KosuCount("neko_badge3"));
                                }


                                if (timeIttei3 >= manpuku_deg_param) //1=5分なので、2だと10分で腹減り-1
                                {
                                    timeIttei3 = 0;

                                    //満腹度が減る。
                                    girl1_status.ManpukuBarKoushin(-1);

                                    //満腹度が0になると、ハートも減り始める。
                                    if (GameMgr.System_Manpuku_ON)
                                    {
                                        if (PlayerStatus.player_girl_manpuku <= 0)
                                        {
                                            girleat_judge.UpDegHeart(-1, false);
                                            girl1_status.GirlExpressionKoushin(-3);

                                            /*if (PlayerStatus.girl1_Love_lv >= 40 && PlayerStatus.girl1_Love_lv < 80)
                                            {
                                                girleat_judge.DegHeart(-1 * (int)(PlayerStatus.girl1_Love_lv * 0.05f), false);
                                            }
                                            else if (PlayerStatus.girl1_Love_lv >= 80 && PlayerStatus.girl1_Love_lv < 90)
                                            {
                                                girleat_judge.DegHeart(-1 * (int)(PlayerStatus.girl1_Love_lv * 0.075f), false);
                                            }
                                            else if (PlayerStatus.girl1_Love_lv >= 90)
                                            {
                                                girleat_judge.DegHeart(-1 * (int)(PlayerStatus.girl1_Love_lv * 0.1f), false);
                                            }
                                            else
                                            {
                                                girleat_judge.DegHeart(-1, false);
                                            }*/

                                            girl1_status.MotionChange(23);
                                        }
                                    }
                                    else { }

                                    //機嫌も少しずつ収まっていく。
                                    if (PlayerStatus.player_girl_express_param >= 50)
                                    {
                                        girl1_status.GirlExpressionKoushin(-1);
                                    }
                                }
                            }

                            //食べたいお菓子をあげた直後、しばらくハートが上がる特殊状態になる。
                            if (GameMgr.hikari_tabetaiokashi_buf)
                            {
                                GameMgr.hikari_tabetaiokashi_buf_time--;

                                if (GameMgr.hikari_tabetaiokashi_buf_time <= 0)
                                {
                                    GameMgr.hikari_tabetaiokashi_buf = false; //バフ終了
                                }
                            }
                        }

                    }
                }
            }
        }
    }

    public void HikariMakeOkashiJudge()
    {
        //まず残り個数チェック
        //材料がなくなってたら、ここで終了。これは、にいちゃんが途中で材料を使った場合のチェックになる。
        HikariKosuCheckMethod();
        
        if (itemkosu_check) //trueなら個数がたりないので、終了
        {
            //終了
            GameMgr.hikari_make_okashiFlag = false;
            GameMgr.hikari_makeokashi_startflag = false;

            //このとき、成功が0だった場合は、全て失敗してるので、失敗しちゃった～の顔に。
            if (GameMgr.hikari_make_success_count == 0)
            {
                GameMgr.hikari_make_Allfailed = true;
            }
            else
            {
                //材料なくなった～の表情に
                GameMgr.hikari_zairyo_no_flag = true;
            }
        } else
        {
            //制作チェック

            //サイコロをふる
            dice = Random.Range(1, 100); //1~100までのサイコロをふる。

            Debug.Log("ヒカリ成功確率: " + GameMgr.hikari_make_success_rate + " " + "ダイスの目: " + dice);

            if (dice <= (int)GameMgr.hikari_make_success_rate) //出た目が、成功率より下なら成功
            {
                GameMgr.hikari_make_success_count++;

                //お菓子を一個完成。リザルトの個数のみカウンタを追加。+材料のみ減らす。
                GameMgr.hikari_make_okashiKosu++;
                _getexp = 2;
                hikariOkashiExpTable.hikariOkashi_ExpTableMethod(database.items[GameMgr.hikari_make_okashiID].itemType_sub.ToString(), _getexp, 1, 0);

                //成功すると、機嫌が少しよくなる。
                if (!GameMgr.Contest_ON)
                {
                    girl1_status.GirlExpressionKoushin(10);
                }
            }
            else //失敗
            {
                GameMgr.hikari_make_failed_count++;

                //生成されず。材料だけ消費。
                _getexp = 5;
                hikariOkashiExpTable.hikariOkashi_ExpTableMethod(database.items[GameMgr.hikari_make_okashiID].itemType_sub.ToString(), _getexp, 1, 0);

                //コンテスト中は、ハート系は動かない
                if (!GameMgr.Contest_ON)
                {
                    //ハートも下がる。
                    girleat_judge.UpDegHeart(-5, false);

                    //失敗すると、機嫌は下がる。-20で1段階下がる。
                    girl1_status.GirlExpressionKoushin(-10);
                }
            }

            compound_keisan.Delete_playerItemList(2);

            //
            //作ったあとのタイミングで、改めて個数をチェックする。なくなってれば、終了
            HikariKosuCheckMethod();

            if (itemkosu_check) //trueなら個数がたりないので、終了
            {
                //終了
                GameMgr.hikari_make_okashiFlag = false;
                GameMgr.hikari_makeokashi_startflag = false;

                //このとき、成功が0だった場合は、全て失敗してるので、失敗しちゃった～の顔に。
                if (GameMgr.hikari_make_success_count == 0)
                {
                    GameMgr.hikari_make_Allfailed = true;
                }
                else
                {
                    //材料なくなった～の表情に
                    GameMgr.hikari_zairyo_no_flag = true;
                }
            }
        }
        
    }

    void HikariKosuCheckMethod()
    {
        itemkosu_check = false;
        for (i = 0; i < 3; i++)
        {
            if (i == 2 && GameMgr.hikari_kettei_item[2] == 9999) //3個目が空のときは9999入ってて、無視
            {

            }
            else
            {
                /*Debug.Log("オリジナルアイテムリスト総数: " + pitemlist.player_originalitemlist.Count);
                Debug.Log("GameMgr.hikari_kettei_originalID[i]: " + GameMgr.hikari_kettei_originalID[i]);
                Debug.Log("GameMgr.hikari_kettei_toggleType[i]: " + GameMgr.hikari_kettei_toggleType[i]);
                Debug.Log("pitemlist.ReturnOriginalKoyuIDtoItemID(GameMgr.hikari_kettei_originalID[i]): " + 
                    pitemlist.ReturnOriginalKoyuIDtoItemID(GameMgr.hikari_kettei_originalID[i]));*/

                if (GameMgr.hikari_kettei_toggleType[i] == 0) //店売りアイテム
                {
                    if (database.items[GameMgr.hikari_kettei_item[i]].itemType_sub.ToString() == "Machine")
                    {

                    }
                    else
                    {
                        if (pitemlist.playeritemlist[database.items[GameMgr.hikari_kettei_item[i]].itemName] < GameMgr.hikari_kettei_kosu[i])
                        {
                            //個数足りない　終了
                            itemkosu_check = true;
                        }
                    }
                }
                else if (GameMgr.hikari_kettei_toggleType[i] == 1) //オリジナルアイテム
                {
                    if (pitemlist.ReturnOriginalKoyuIDtoItemID(GameMgr.hikari_kettei_originalID[i]) == 9999)
                    {
                        //例外　もしなかった場合
                        itemkosu_check = true;
                    }
                    else
                    {
                        if (pitemlist.player_originalitemlist[pitemlist.ReturnOriginalKoyuIDtoItemID(GameMgr.hikari_kettei_originalID[i])].ItemKosu < GameMgr.hikari_kettei_kosu[i])
                        {
                            //終了
                            itemkosu_check = true;
                        }
                    }
                }
                else if (GameMgr.hikari_kettei_toggleType[i] == 2) //エクストリームアイテム
                {
                    if (pitemlist.ReturnOriginalKoyuIDtoItemID(GameMgr.hikari_kettei_originalID[i]) == 9999)
                    {
                        //例外　もしなかった場合
                        itemkosu_check = true;
                    }
                    else
                    {
                        if (pitemlist.player_extremepanel_itemlist[pitemlist.ReturnOriginalKoyuIDtoItemID(GameMgr.hikari_kettei_originalID[i])].ItemKosu < GameMgr.hikari_kettei_kosu[i])
                        {
                            //終了
                            itemkosu_check = true;
                        }
                    }
                }
            }
        }
    }

    //天気状態の更新
    void Weather_Judge_Method()
    {
        //Debug.Log("天気チェック");
        if (PlayerStatus.player_cullent_hour >= 0 && PlayerStatus.player_cullent_hour < 8)
        {
            GameMgr.BG_cullent_weather = 1;

        }
        else if (PlayerStatus.player_cullent_hour >= 8 && PlayerStatus.player_cullent_hour < 11)
        {
            GameMgr.BG_cullent_weather = 2;

        }
        else if (PlayerStatus.player_cullent_hour >= 11 && PlayerStatus.player_cullent_hour < 13)
        {
            GameMgr.BG_cullent_weather = 3;

        }
        else if (PlayerStatus.player_cullent_hour >= 13 && PlayerStatus.player_cullent_hour < 16)
        {
            GameMgr.BG_cullent_weather = 4;

        }
        else if (PlayerStatus.player_cullent_hour >= 16 && PlayerStatus.player_cullent_hour < 19)
        {
            GameMgr.BG_cullent_weather = 5;

        }
        else if (PlayerStatus.player_cullent_hour >= 19)
        {
            GameMgr.BG_cullent_weather = 6;
        }
    }

    //現在の月日・現在時刻を計算する。また、イベントチェックも行う。
    public void TimeKoushin(int _mstatus)
    {
        
        if (GameMgr.TimeUSE_FLAG) //TRUEのときは使用。オフにするときは、TimePanelのゲームオブジェクトもオフにする。
        {

            /* 時刻の計算 */

            //現在時刻を計算 この時点で、25時 ○○分とかの可能性もあり。そのときに、player_dayへの変換もする。
            TimeKeisan();            

            //
            /* 月日の計算 */
            //

            //InitParam();

            //プレイヤーデイを基に、カレンダーの日付に変換。
            if (PlayerStatus.player_day > 365)
            {
                PlayerStatus.player_day = 1;
            }

                      

            //カレンダー変換機能
            if (_mstatus == 0)
            {
                CullenderKeisan(PlayerStatus.player_day);              

                //現在の月と日を更新しておく。
                PlayerStatus.player_cullent_month = month;
                PlayerStatus.player_cullent_day = day;
            }
            else if (_mstatus == 1)
            {
                //入力された日付から、逆算してPlayerStatus.player_dayを計算する。
                CullenderKeisanInverse(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day);
            }

            //** 月日の計算　ここまで **//


            //時間帯に合わせて天気情報も更新
            Weather_Judge_Method();


            //時刻と月日の計算後にイベントチェック

            //20時を超えた場合、寝るなどのイベント発生チェック
            if (PlayerStatus.player_cullent_hour >= GameMgr.EndDay_hour) //20時をこえた
            {
                DayEndEvent();
            }
            else if (PlayerStatus.player_cullent_hour >= 0 && PlayerStatus.player_cullent_hour < GameMgr.StartDay_hour) //深夜1時～朝8時未満
            {
                DayEndEvent();
            }
            else 
            {}
        }
    }

    //日数をいれると、その日が何月何日か出してくれる
    public void CullenderKeisan(int _cullent_day)
    {
        month = 0;
        day = 0;

        count = 0;
        while (count < calender.Count)
        {
            if (_cullent_day > calender[count]) { _cullent_day -= calender[count]; }
            else //その月の日付
            {
                month = count + 1; //月　0始まりなので、足す１
                day = _cullent_day; //日

                GameMgr.Cullender_Month = month;
                GameMgr.Cullender_Day = day;
                break;
            }
            ++count;
        }
    }

    //何月何日かをいれると、それを日数に逆算する ContestListControllerなどで使用
    public int CullenderKeisanInverse(int _month, int _day)
    {
        total_day = 0;

        if(_month <= 0)
        {
            _month = 1;
        }

        count = 0;
        while (count < _month-1) //まず月をもとに、前月までの総日数をだす
        {
            total_day += calender[count];
            ++count;
        }

        //そのあと、残りの日数を加算
        total_day += _day;

        return total_day;
    }

    void DayEndEvent()
    {
        //一日が経った。
        if (TimeCheck_flag) //Compound_MainでTimeCheck_flagをtrueにしている。
        {
            TimeCheck_flag = false;

            //寝るイベントが発生
            if (TimeReturnHomeSleep_Status) //兄が帰ってきたあとのセリフ
            {
                TimeReturnHomeSleep_Status = false;
                GameMgr.sleep_status = 2;
            }
            else
            {
                GameMgr.sleep_status = 0;
            }
            compound_main.OnSleepReceive();
        }
    }

    void TimeKeisan()
    {
        //Debug.Log("時間の更新");

        count = 0;

        //まず、分を時間と分に変換。
        if (PlayerStatus.player_cullent_minute >= 0)
        {
            _cullent_minute = PlayerStatus.player_cullent_minute;

            //60分で1時間に変換            
            while (_cullent_minute > 0) //
            {
                if (_cullent_minute >= 60)
                {
                    _cullent_minute -= 60;
                    count++;
                }
                else //その月の日付
                {
                    break;
                }
            }
        }
        else if(PlayerStatus.player_cullent_minute < 0) //SetMinuteでもしマイナスの分になっていた場合は、ここで正常な時間と分に戻す。SetMinuteの調整で、-60分とかにはならない。
        {
            _cullent_minute = PlayerStatus.player_cullent_minute;

            count--;
            _cullent_minute = 60 - Mathf.Abs(_cullent_minute);
        }

        PlayerStatus.player_cullent_hour += count;
        PlayerStatus.player_cullent_minute = _cullent_minute;

        //次にcullent_hourを、日と時間に変換  ○○時(49時とか78時などの可能性もある）を日数に変換し、日数加算と24時間以内の時間に直す。

        count = 0;
        //24時間で1日がたつ
        if (PlayerStatus.player_cullent_hour >= 0) //
        {
            _cullent_hour = PlayerStatus.player_cullent_hour;
            
            while (_cullent_hour > 0) //
            {
                if (_cullent_hour >= 24)
                {
                    _cullent_hour -= 24;
                    count++;
                }
                else //その月の日付
                {
                    break;
                }
            }
        }
        if (PlayerStatus.player_cullent_hour < 0) //-1時とかになった場合の計算
        {
            _cullent_hour = PlayerStatus.player_cullent_hour;

            while (_cullent_hour < 0) //
            {
                if (_cullent_hour <= -24)
                {
                    _cullent_hour += 24;
                    count--;
                }
                else //その月の日付
                {
                    count--;
                    _cullent_hour = 24 - Mathf.Abs(_cullent_hour);
                    break;
                }
            }
        }

        PlayerStatus.player_day += count;
        PlayerStatus.player_cullent_hour = _cullent_hour; //残った時　多分、深夜1時とかになるはず。15時とかならそのまま15時

        //Debug.Log("現在時刻: " + PlayerStatus.player_cullent_hour + ": " + PlayerStatus.player_cullent_minute);
       
    }

    //他のスクリプトから寝るを選択
    /*void OnSleepMethod()
    {
        GameMgr.sleep_status = 1;
        compound_main.OnSleepReceive();
    }*/

    public void DeadLine_Setting()
    {
        _stage_limit_day = GameMgr.stage1_limit_day;

        //締め切り日も計算
        count = 0;
        while (count < calender.Count)
        {
            if (_stage_limit_day > calender[count]) { _stage_limit_day -= calender[count]; }
            else //その月の日付
            {
                limit_month = count + 1; //月　0始まりなので、足す１
                limit_day = _stage_limit_day; //日
                break;
            }
            ++count;
        }

        PlayerStatus.player_cullent_Deadmonth = limit_month;
        PlayerStatus.player_cullent_Deadday = limit_day;
    }

    public void ResetTimeFlag() //compound_main, touch_controllerなどから読む。compound_status=0のときにリセットする
    {
        timeDegHeart_flag = false;
        timeIttei = 0;
    }

    void RealTimeHeartControll()
    {
        if (GameMgr.Degheart_on)
        { }
        else
        {
            switch (timeDegHeart_flag)
            {
                case false:

                    if (timeIttei >= 5) //放置して5秒たつと、下がり始めのフラグがたつ。その後、何秒かごとに減っていく。
                    {
                        timeIttei = 0;
                        timeDegHeart_flag = true;

                        girleat_judge.UpDegHeart(-1, false); //false なら音なし

                    }
                    break;

                case true:

                    if (timeIttei >= 2)
                    {
                        timeIttei = 0;

                        girleat_judge.UpDegHeart(-1, false);

                    }
                    break;
            }
        }
    }


    //入力された分単位の時間を、時間と分にわけて、現在の時間に加算する。マイナスの場合、引き算する。
    public void SetMinuteToHour(int _m)
    {
        if (_m >= 0)
        {
            minute = 0;
            hour = 0;
            //現在1分刻みで計算

            while (_m > 0) //
            {
                if (_m >= 60)
                {
                    _m -= 60;
                    hour++;
                }
                else //12より下なら、分のみで、時間は計算いらない。
                {
                    minute = _m * GameMgr.TimeStep; //残り時間 * 5分
                    _m = 0;
                    break;
                }
            }            
        }
        else
        {
            minute = 0;
            hour = 0;

            while (_m < 0) //
            {
                if (_m <= -60)
                {
                    _m += 60;
                    hour--;
                }
                else //
                {
                    minute = _m * GameMgr.TimeStep; //巻き戻し時間をだす。マイナスになるかも。
                    _m = 0;
                    break;
                }
            }
        }

        //加算前に、現在の月日をセッティング　月をまたいだかチェックで使う
        GameMgr.SleepBefore_Month = PlayerStatus.player_cullent_month;
        GameMgr.SleepBefore_Day = PlayerStatus.player_cullent_day;

        //入力された分を、時間と分に直し加算する。
        PlayerStatus.player_cullent_hour += hour;
        PlayerStatus.player_cullent_minute += minute;
    }

    //入力された分単位の時間を、時間と分にわけて、現在の時間に加算し、予測時間をだす。実際の加算はしない。Returnは時間のみ。
    public int YosokuMinuteToHour(int _m)
    {
        _cullent_hour = PlayerStatus.player_cullent_hour;
        _cullent_minute = PlayerStatus.player_cullent_minute;

        minute = 0;
        hour = 0;
        //現在1分刻みで計算
        

        while (_m > 0) //
        {
            if (_m >= 60) //5分刻みなら12ごとに時間を+1
            {
                _m -= 60;
                hour++;
            }
            else //12より下なら、分のみで、時間は計算いらない。
            {
                minute = _m * GameMgr.TimeStep; //残り時間 * 5分
                _m = 0;
                break;
            }
        }

        //入力された分を、時間と分に直し加算する。
        _cullent_hour += hour;
        _cullent_minute += minute;


        //さらに、60分をこえてるかどうかをちぇっくし、時間に変換

        //60分で1時間に変換   
        count = 0;
        while (_cullent_minute > 0) //
        {
            if (_cullent_minute >= 60)
            {
                _cullent_minute -= 60;
                count++;
            }
            else //その月の日付
            {
                break;
            }
        }

        _cullent_hour += count;

        return _cullent_hour;
    }


    //入力された分単位の時間を、時間と分にわけて、コンテストの時間に加算する。マイナスの場合、引き算する。
    public void SetMinuteToHourContest(int _m)
    {
        //制限時間から引き算
        PlayerStatus.player_contest_LimitTime -= _m;

        //もし制限時間を超えた場合、30分以内なら減点はされるが、提出は可能。30分をこえると失格になり、
        //イベント発生のち、コンテスト終了
        if(PlayerStatus.player_contest_LimitTime < 0)
        {
            if(Mathf.Abs(PlayerStatus.player_contest_LimitTime) < 30)
            {
                //提出は可能　だが減点　時間とボタンが赤になる。
                GameMgr.contest_LimitTimeOver_DegScore_flag = true;
            }
            else
            {
                //３０分を超えた場合　失格
                GameMgr.contest_LimitTimeOver_Gameover_flag = true;
            }
        }

        //時間と分になおす
        if (_m >= 0)
        {
            minute = 0;
            hour = 0;
            //現在1分刻みで計算

            while (_m > 0) //
            {
                if (_m >= 60)
                {
                    _m -= 60;
                    hour++;
                }
                else //12より下なら、分のみで、時間は計算いらない。
                {
                    minute = _m * GameMgr.TimeStep; //残り時間 * 5分
                    _m = 0;
                    break;
                }
            }
        }
        else
        {
            minute = 0;
            hour = 0;

            while (_m < 0) //
            {
                if (_m <= -60)
                {
                    _m += 60;
                    hour--;
                }
                else //
                {
                    minute = _m * GameMgr.TimeStep; //巻き戻し時間をだす。マイナスになるかも。
                    _m = 0;
                    break;
                }
            }           
        }

        //入力された分を、時間と分に直し加算する。
        PlayerStatus.player_contest_hour += hour;
        PlayerStatus.player_contest_minute += minute;
    }

    public void OnDebugTimeCountUpButton()
    {
        SetMinuteToHour(30); //+30分
        TimeKoushin(0);
    }

    public void OnDebugTimeCountDownButton()
    {
        SetMinuteToHour(-30); //-30分
        TimeKoushin(0);
    }

    //時間をいれると、その経過時間をチェックし、お菓子を作ってないかを判定 Exp_Controllerから読み出し。
    public void HikarimakeTimeCheck(int _costTime)
    {
        if (GameMgr.System_HikariMake_OnichanTimeCost_ON)
        {
            //ヒカリがお菓子を作ってる場合、ここでお菓子制作時間を計算
            if (!GameMgr.outgirl_Nowprogress)
            {
                if (GameMgr.hikari_make_okashiFlag)
                {
                    //にいちゃんの制作時間より、まだヒカリの制作時間のほうが長い場合
                    if(GameMgr.hikari_make_okashiTimeCounter > _costTime)
                    {
                        GameMgr.hikari_make_okashiTimeCounter -= _costTime; //ヒカリの制作時間を減らす
                    }
                    else //にいちゃんの制作時間中に、ヒカリの制作が終わった場合　数回繰り返す可能性がある
                    {
                        Zairyo_nothing = false;
                        Start_count = _costTime;
                        while (Start_count >= GameMgr.hikari_make_okashiTimeCounter)
                        {
                            Debug.Log("ヒカリのお菓子制作　チェック");

                            Start_count -= GameMgr.hikari_make_okashiTimeCounter;

                            //お菓子制作。個数チェックと成功率を計算する。
                            HikariMakeOkashiJudge();

                            if (!GameMgr.hikari_make_okashiFlag)
                            {
                                //終了 途中で材料なくなった
                                Zairyo_nothing = true;
                                break;
                            }

                            GameMgr.hikari_make_okashiTimeCounter = GameMgr.hikari_make_okashiTimeCost;
                        }

                        if (!Zairyo_nothing)
                        {
                            GameMgr.hikari_make_okashiTimeCounter -= Start_count; //最後に余った制作時間分を、カウンタから減らす
                        }
                    }
                    
                }
            }
        }
    }

    //外部から。指定した日付と時間に瞬時に更新する。月と日、時間と分で指定できる。
    public void SetCullentDayTime(int _month, int _day, int _hour, int _minute)
    {
        PlayerStatus.player_cullent_month = _month;
        PlayerStatus.player_cullent_day = _day;
        PlayerStatus.player_cullent_hour = _hour;
        PlayerStatus.player_cullent_minute = _minute;

        //日付更新
        TimeKoushin(1);

        //天気も変更
        //Weather_ChangeNow(1.0f);
    }

    //コンテストで開催日時を計算するのに使用　入れた日数を、現在の日数に足して、何月何日に変換して戻す Contest_DetailedPanel.csから読み出し
    public void AfterTimeLimit_Keisan(int _afterday)
    {
        CullenderKeisan(PlayerStatus.player_day + _afterday);
        GameMgr.Contest_OrganizeMonth = month;
        GameMgr.Contest_OrganizeDay = day;
    }


    void GameSpeedRange()
    {
        switch(GameMgr.GameSpeedParam)
        {
            case 1:

                timespeed_range = 0.15f;
                break;

            case 2:

                timespeed_range = 0.5f;
                break;

            case 3:

                timespeed_range = 1.0f;
                break;

            case 4:

                timespeed_range = 2.0f;
                break;

            case 5:

                timespeed_range = 3.0f;
                break;

            default:

                timespeed_range = 1.0f;
                break;
        }
    }
}

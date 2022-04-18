using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TimeController : MonoBehaviour
{

    //時間の概念を使用するかどうかは、GameMgr.csに記述  使用しないときは、TimePanelオブジェクトもオフにする

    private GameObject canvas;
    private Compound_Main compound_main;

    private GirlEat_Judge girleat_judge;
    private MoneyStatus_Controller moneyStatus_Controller;

    private Compound_Keisan compound_keisan;

    private Girl1_status girl1_status;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private GameObject _month_obj1;
    private GameObject _monthday_obj1;
    private GameObject _month_obj2;
    private Text _month_text1;
    private Text _day_text1;
    private Text _day_text2;

    private GameObject _time_obj1_hour;
    private GameObject _time_obj1_count;
    private GameObject _time_obj1_minute;
    private GameObject _time_obj2_hour;
    private GameObject _time_obj2_count;
    private GameObject _time_obj2_minute;
    private GameObject _time_obj1_limit;
    private GameObject _time_obj2_limit;

    private Text _time_hour1;
    private Text _time_count1;
    private Text _time_minute1;
    private Text _time_hour2;
    private Text _time_count2;
    private Text _time_minute2;
    private Text _time_limit1;
    private Text _time_limit2;

    private List<int> calender = new List<int>();
    private int _cullent_day;
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
    public bool timeDegHeart_flag; //表示用にpublicにしてるだけ。

    private int i, count;

    private float timeLeft;
    private bool count_switch;

    private bool itemkosu_check;

    private float timespeed_range;

    private bool money_counter;

    public bool TimeCheck_flag; //調合メインメソッドのトップ画面で起動開始
    public bool TimeReturnHomeSleep_Status; //兄が帰ってきたあと、少しセリフ変わる。 

    private GameObject DebugTimecountUp_button;
    private GameObject DebugTimecountDown_button;

    private GameObject clock_hari1;
    private GameObject clock_hari2;

    private Transform clock_hari1Transform;
    private Transform clock_hari2Transform;

    private Vector3 localAngle1;
    private Vector3 localAngle2;

    // Use this for initialization
    void Start()
    {
        InitParam();

        timeIttei = 0;
        timeIttei2 = 0;
        timeIttei3 = 0;
        timeIttei4 = 0;
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

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                compound_main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();
                girleat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

                //お金パネル
                moneyStatus_Controller = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();

                //女の子データの取得
                girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子
                break;

        }

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

        _month_obj1 = this.transform.Find("TimeHyouji_1/Image/Month").gameObject;
        _month_text1 = _month_obj1.GetComponent<Text>();

        _monthday_obj1 = this.transform.Find("TimeHyouji_1/Image/Month_Day").gameObject;
        _day_text1 = _monthday_obj1.GetComponent<Text>();

        _month_obj2 = this.transform.Find("TimeHyouji_2/Month").gameObject;
        _day_text2 = _month_obj2.GetComponent<Text>();

        _time_obj1_hour = this.transform.Find("TimeHyouji_1/Image/HourPanel/Hour").gameObject;
        _time_hour1 = _time_obj1_hour.GetComponent<Text>();

        _time_obj1_count = this.transform.Find("TimeHyouji_1/Image/HourPanel/TimeCount").gameObject;
        _time_count1 = _time_obj1_count.GetComponent<Text>();

        _time_obj1_minute = this.transform.Find("TimeHyouji_1/Image/HourPanel/Minute").gameObject;
        _time_minute1 = _time_obj1_minute.GetComponent<Text>();

        _time_obj2_hour = this.transform.Find("TimeHyouji_2/Hour").gameObject;
        _time_hour2 = _time_obj2_hour.GetComponent<Text>();

        _time_obj2_count = this.transform.Find("TimeHyouji_2/TimeCount").gameObject;
        _time_count2 = _time_obj2_count.GetComponent<Text>();

        _time_obj2_minute = this.transform.Find("TimeHyouji_2/Minute").gameObject;
        _time_minute2 = _time_obj2_minute.GetComponent<Text>();

        _time_obj1_limit = this.transform.Find("TimeHyouji_1/NokoriTimePanel/NokoriTimeParam").gameObject;
        _time_limit1 = _time_obj1_limit.GetComponent<Text>();

        _time_obj2_limit = this.transform.Find("TimeHyouji_2/NokoriTimeParam").gameObject;
        _time_limit2 = _time_obj2_limit.GetComponent<Text>();

        DebugTimecountUp_button = this.transform.Find("TimeHyouji_2/TimeCountUpButton").gameObject;
        DebugTimecountDown_button = this.transform.Find("TimeHyouji_2/TimeCountDownButton").gameObject;

        clock_hari1 = this.transform.Find("TimeHyouji_1/Image/HourPanel/ClockBase/Clock_hari1").gameObject;
        clock_hari2 = this.transform.Find("TimeHyouji_1/Image/HourPanel/ClockBase/Clock_hari2").gameObject;

        // transformを取得
        clock_hari1Transform = clock_hari1.transform;
        clock_hari2Transform = clock_hari2.transform;

        // ローカル座標を基準に、回転を取得
        localAngle1 = clock_hari1Transform.localEulerAngles;
        localAngle2 = clock_hari2Transform.localEulerAngles;

        timeLeft = 1.0f;
        count_switch = true;

        timespeed_range = 1.0f;

        money_counter = false;

        if (GameMgr.Story_Mode == 0)
        {
            this.transform.Find("TimeHyouji_1").GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            this.transform.Find("TimeHyouji_1").GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    private void OnEnable()
    {
        InitParam();
    }

    // Update is called once per frame
    void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //時間のカウント
                timeLeft -= Time.deltaTime;

                //1秒ごとのタイムカウンター
                if (timeLeft <= 0.0)
                {
                    timeLeft = 1.0f;
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
                                RealTimeControll();
                            }
                        }
                    }

                    //フリーモードのときは、時間がリアルタイムで経過　30カウントで5分とか
                    if (GameMgr.Story_Mode == 1)
                    {
                        GameSpeedRange(); //ゲームスピードパラメータの変更

                        if (!GameMgr.scenario_ON)
                        {
                            if (GameMgr.compound_status == 110 && GameMgr.compound_select == 0)  //採集中やステータス画面など開いてるときは減らない
                            {
                                if (!GameMgr.ReadGirlLoveTimeEvent_reading_now) //特定の時間イベント読み中の間もoffに。
                                {
                                    timeIttei2++;
                                    if (timeIttei2 >= (int)(10 * timespeed_range))
                                    {
                                        timeIttei2 = 0;
                                        SetMinuteToHour(1); //1=5分単位
                                        TimeKoushin();

                                        Weather_Change(5.0f);

                                        //サブ時間イベントをチェック
                                        if (GameMgr.ResultOFF) //リザルト画面表示中は、時間イベントは発生しない
                                        {

                                        }
                                        else
                                        {
                                            GameMgr.check_GirlLoveTimeEvent_flag = false;
                                        }

                                        //ヒカリがお菓子を作ってる場合、ここでお菓子制作時間を計算
                                        if (!GameMgr.outgirl_Nowprogress)
                                        {
                                            if (GameMgr.hikari_make_okashiFlag)
                                            {
                                                GameMgr.hikari_make_okashiTimeCounter += 5;
                                                if (GameMgr.hikari_make_okashiTimeCounter >= GameMgr.hikari_make_okashiTimeCost) //costtime=1が5分　ヒカリが作ると2倍時間かかる
                                                {
                                                    GameMgr.hikari_make_okashiTimeCounter = 0;

                                                    //お菓子を一個完成。リザルトの個数のみカウンタを追加。+材料のみ減らす。
                                                    GameMgr.hikari_make_okashiKosu++;

                                                    //削除前に残り個数チェック
                                                    //材料がなくなってたら、ここで終了。
                                                    itemkosu_check = false;
                                                    for (i = 0; i < 3; i++)
                                                    {
                                                        if (i == 2 && GameMgr.hikari_kettei_item[2] == 9999) //3個目が空のときは9999入ってて、無視
                                                        {

                                                        }
                                                        else
                                                        {
                                                            if (GameMgr.hikari_kettei_toggleType[i] == 0)
                                                            {
                                                                if (database.items[GameMgr.hikari_kettei_item[i]].itemType_sub.ToString() == "Machine")
                                                                {

                                                                }
                                                                else
                                                                {
                                                                    if (pitemlist.playeritemlist[database.items[GameMgr.hikari_kettei_item[i]].itemName] - GameMgr.hikari_kettei_kosu[i] < GameMgr.hikari_kettei_kosu[i])
                                                                    {
                                                                        //終了
                                                                        itemkosu_check = true;
                                                                    }
                                                                }
                                                            }
                                                            else if (GameMgr.hikari_kettei_toggleType[i] == 1)
                                                            {
                                                                if (pitemlist.player_originalitemlist[GameMgr.hikari_kettei_item[i]].ItemKosu - GameMgr.hikari_kettei_kosu[i] < GameMgr.hikari_kettei_kosu[i])
                                                                {
                                                                    //終了
                                                                    itemkosu_check = true;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    compound_keisan.Delete_playerItemList(2);

                                                    if (itemkosu_check)
                                                    {
                                                        //終了
                                                        GameMgr.hikari_make_okashiFlag = false;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    if (!GameMgr.outgirl_Nowprogress)
                                    {
                                        timeIttei3++;
                                        if (timeIttei3 >= (int)(20 * timespeed_range))
                                        {
                                            timeIttei3 = 0;

                                            //満腹度が減る。
                                            compound_main.ManpukuBarKoushin(-1);

                                            //満腹度が0になると、ハートも減り始める。
                                            if (PlayerStatus.player_girl_manpuku <= 0)
                                            {
                                                //girleat_judge.DegHeart(-1 * (int)(PlayerStatus.girl1_Love_lv * 0.2f), false);
                                                girleat_judge.DegHeart(-1, false);
                                                girl1_status.MotionChange(23);
                                            }
                                        }

                                        //時間でゆるやかにハートも減る。
                                        /*timeIttei4++;
                                        if (timeIttei4 >= (int)(30*timespeed_range))
                                        {
                                            timeIttei4 = 0;

                                            //満腹度が0になると、ハートも減り始める。
                                            if (PlayerStatus.player_girl_manpuku <= 70)
                                            {
                                                girleat_judge.DegHeart(-2, false);
                                            }

                                        }*/
                                    }
                                }
                            }
                        }
                    }
                }

                if (count_switch)
                {
                    //表示
                    _time_count1.text = ":";
                    _time_count2.text = ":";

                }
                else
                {
                    //表示
                    _time_count1.text = " ";
                    _time_count2.text = " ";
                }

                if (GameMgr.DEBUG_MODE)
                {
                    DebugTimecountUp_button.SetActive(true);
                    DebugTimecountDown_button.SetActive(true);
                }
                else
                {
                    DebugTimecountUp_button.SetActive(false);
                    DebugTimecountDown_button.SetActive(false);
                }
                break;
        }
    }

    //時間に応じて、天気（背景）を変更する。BG_RealtimeChange()内の数字は、切り替わりの時間。調合後（ExpController）や採取から帰ってきたとき（Compound_Main）からも読まれる。
    //下の関数とほぼ同じだが、こっちはcompound_main.BG_RealtimeChange内のtweenのDoFadeを、重複して発生しないようにしている。基本はこっちを使用でOK。
    public void Weather_Change(float _changetime)
    {
        //フリーモードのときのみ　変更
        if (GameMgr.Story_Mode == 1)
        {
            Weather_Judge_Method();            

            //Debug.Log("GameMgr.BG_cullent_weather: " + GameMgr.BG_cullent_weather);
            //Debug.Log("GameMgr.BG_before_weather: " + GameMgr.BG_before_weather);
            if (GameMgr.BG_cullent_weather != GameMgr.BG_before_weather)
            {
                GameMgr.BG_before_weather = GameMgr.BG_cullent_weather;

                //天気アニメ変更をトリガー
                compound_main.BG_RealtimeChange(_changetime); //背景更新
                Debug.Log("天気を変更　秒数: " + _changetime);
            }
        }
    }

    //現在時刻に合わせて、即背景を変更。前時間と現在時間の比較計算をしない。ロード直後はこっちを使用。（うまくbeforeとcullentの値の切り替えが出来なかったため。）
    public void Weather_ChangeNow(float _changetime)
    {
        //フリーモードのときのみ　変更
        if (GameMgr.Story_Mode == 1)
        {
            Weather_Judge_Method();

            //Debug.Log("GameMgr.BG_cullent_weather: " + GameMgr.BG_cullent_weather);
            //Debug.Log("GameMgr.BG_before_weather: " + GameMgr.BG_before_weather);
            GameMgr.BG_before_weather = GameMgr.BG_cullent_weather;

            //天気アニメ変更をトリガー
            compound_main.BG_RealtimeChange(_changetime); //背景更新
            Debug.Log("天気を変更　秒数: " + _changetime);
        }
    }

    void Weather_Judge_Method()
    {
        Debug.Log("天気チェック");
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
    public void TimeKoushin()
    {
        
        if (GameMgr.TimeUSE_FLAG) //TRUEのときは使用。オフにするときは、TimePanelのゲームオブジェクトもオフにする。
        {

            /* 時刻の計算 */

            //現在時刻を計算 この時点で、25時 ○○分とかの可能性もあり。そのときに、player_dayへの変換もする。
            TimeKeisan();

            //表示
            _time_hour1.text = PlayerStatus.player_cullent_hour.ToString("00");
            _time_minute1.text = PlayerStatus.player_cullent_minute.ToString("00");
            _time_hour2.text = PlayerStatus.player_cullent_hour.ToString("00");
            _time_minute2.text = PlayerStatus.player_cullent_minute.ToString("00");

            //時計版表示           
            localAngle1.z = -1 * PlayerStatus.player_cullent_minute * 6; // ローカル座標を基準に、z軸を軸にした回転
            clock_hari1Transform.localEulerAngles = localAngle1; // 回転角度を設定

            if(PlayerStatus.player_cullent_hour >= 12)
            {
                cullent_hour_clock = PlayerStatus.player_cullent_hour - 12;
            }
            else
            {
                cullent_hour_clock = PlayerStatus.player_cullent_hour;
            }
            localAngle2.z = -1 * 30 * cullent_hour_clock; // ローカル座標を基準に、z軸を軸にした回転
            localAngle2.z = localAngle2.z  + (- 2.5f * (PlayerStatus.player_cullent_minute / 5));
            clock_hari2Transform.localEulerAngles = localAngle2; // 回転角度を設定

            //** 時刻の計算　ここまで **//


            //
            /* 月日の計算 */
            //

            InitParam();

            //プレイヤーデイを基に、カレンダーの日付に変換。
            if (PlayerStatus.player_day > 365)
            {
                PlayerStatus.player_day = 1;
            }

            _cullent_day = PlayerStatus.player_day;
            month = 0;
            day = 0;

            /*現在は未使用 */

            switch (GameMgr.stage_number)
            {
                case 1:

                    _stage_limit_day = GameMgr.stage1_limit_day;
                    break;

                case 2:

                    _stage_limit_day = GameMgr.stage2_limit_day;
                    break;

                case 3:

                    _stage_limit_day = GameMgr.stage3_limit_day;
                    break;

            }

            //残り日数
            _time_limit1.text = (_stage_limit_day - _cullent_day).ToString() + "日";
            _time_limit2.text = (_stage_limit_day - _cullent_day).ToString() + "日";

            //** ここの間は未使用 **//

            count = 0;
            while (count < calender.Count)
            {
                if (_cullent_day > calender[count]) { _cullent_day -= calender[count]; }
                else //その月の日付
                {
                    month = count + 1; //月　0始まりなので、足す１
                    day = _cullent_day; //日
                    break;
                }
                ++count;
            }

            //現在の月と日を更新しておく。
            PlayerStatus.player_cullent_month = month;
            PlayerStatus.player_cullent_day = day;

            //表示
            _month_text1.text = month.ToString();
            _day_text1.text = day.ToString();
            _day_text2.text = month.ToString() + "/" + day.ToString();

            //** 月日の計算　ここまで **//


            //時刻と月日の計算後にイベントチェック

            //20時を超えた場合、寝るなどのイベント発生チェック
            if (PlayerStatus.player_cullent_hour >= GameMgr.EndDay_hour) //20時をこえた
            {
                DayEndEvent();               
            }
            else if(PlayerStatus.player_cullent_hour >= 0 && PlayerStatus.player_cullent_hour < GameMgr.StartDay_hour) //深夜1時～朝8時未満
            {
                DayEndEvent();
            }
        }
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
    public void OnSleepMethod()
    {
        GameMgr.sleep_status = 1;
        compound_main.OnSleepReceive();
    }

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

    void RealTimeControll()
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

                        girleat_judge.DegHeart(-1, false); //false なら音なし

                    }
                    break;

                case true:

                    if (timeIttei >= 2)
                    {
                        timeIttei = 0;

                        girleat_judge.DegHeart(-1, false);

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
            //現在5分刻みで計算
            //10分刻みなので、6ごとに時間を+1 5分刻みなら12ごとに時間を+1

            while (_m > 0) //
            {
                if (_m >= 12)
                {
                    _m -= 12;
                    hour++;
                }
                else //12より下なら、分のみで、時間は計算いらない。
                {
                    minute = _m * 5; //残り時間 * 10分
                    _m = 0;
                    break;
                }
            }

            //入力された分を、時間と分に直し加算する。
            PlayerStatus.player_cullent_hour += hour;
            PlayerStatus.player_cullent_minute += minute;
        }
        else
        {
            minute = 0;
            hour = 0;

            while (_m < 0) //
            {
                if (_m <= -12)
                {
                    _m += 12;
                    hour--;
                }
                else //
                {
                    minute = _m * 5; //巻き戻し時間をだす。マイナスになるかも。
                    _m = 0;
                    break;
                }
            }

            //入力された分を、時間と分に直し加算する。
            PlayerStatus.player_cullent_hour += hour;
            PlayerStatus.player_cullent_minute += minute;
        }
    }

    //入力された分単位の時間を、時間と分にわけて、現在の時間に加算し、予測時間をだす。実際の加算はしない。Returnは時間のみ。
    public int YosokuMinuteToHour(int _m)
    {
        _cullent_hour = PlayerStatus.player_cullent_hour;
        _cullent_minute = PlayerStatus.player_cullent_minute;

        minute = 0;
        hour = 0;
        //現在5分刻みで計算
        //10分刻みなので、6ごとに時間を+1 5分刻みなら12ごとに時間を+1

        while (_m > 0) //
        {
            if (_m >= 12)
            {
                _m -= 12;
                hour++;
            }
            else //12より下なら、分のみで、時間は計算いらない。
            {
                minute = _m * 5; //残り時間 * 10分
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

    public void OnDebugTimeCountUpButton()
    {
        SetMinuteToHour(6); //+30分
        TimeKoushin();
    }

    public void OnDebugTimeCountDownButton()
    {
        SetMinuteToHour(-6); //-30分
        TimeKoushin();
    }

    void GameSpeedRange()
    {
        switch(GameMgr.GameSpeedParam)
        {
            case 1:

                timespeed_range = 0.25f;
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

                timespeed_range = 4.0f;
                break;

            default:

                timespeed_range = 1.0f;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    private GameObject _month_obj1;
    private GameObject _month_obj2;
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
    private int _stage_limit_day;
    private int month, day;
    private int hour, minute;


    private int max_time;
    private int count;

    private float timeLeft;
    private bool count_switch;

    public bool TimeCheck_flag; //調合メインメソッドのトップ画面で起動開始

    // Use this for initialization
    void Start()
    {
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
        _day_text1 = _month_obj1.GetComponent<Text>();

        _month_obj2 = this.transform.Find("TimeHyouji_2/Image/Month").gameObject;
        _day_text2 = _month_obj2.GetComponent<Text>();

        _time_obj1_hour = this.transform.Find("TimeHyouji_1/Image/Hour").gameObject;
        _time_hour1 = _time_obj1_hour.GetComponent<Text>();

        _time_obj1_count = this.transform.Find("TimeHyouji_1/Image/TimeCount").gameObject;
        _time_count1 = _time_obj1_count.GetComponent<Text>();

        _time_obj1_minute = this.transform.Find("TimeHyouji_1/Image/Minute").gameObject;
        _time_minute1 = _time_obj1_minute.GetComponent<Text>();

        _time_obj2_hour = this.transform.Find("TimeHyouji_2/Image/Hour").gameObject;
        _time_hour2 = _time_obj2_hour.GetComponent<Text>();

        _time_obj2_count = this.transform.Find("TimeHyouji_2/Image/TimeCount").gameObject;
        _time_count2 = _time_obj2_count.GetComponent<Text>();

        _time_obj2_minute = this.transform.Find("TimeHyouji_2/Image/Minute").gameObject;
        _time_minute2 = _time_obj2_minute.GetComponent<Text>();

        _time_obj1_limit = this.transform.Find("TimeHyouji_1/NokoriTimeParam").gameObject;
        _time_limit1 = _time_obj1_limit.GetComponent<Text>();

        _time_obj2_limit = this.transform.Find("TimeHyouji_2/Image/NokoriTimeParam").gameObject;
        _time_limit2 = _time_obj2_limit.GetComponent<Text>();

        _cullent_day = PlayerStatus.player_day;

        max_time = 12; //1時間単位

        timeLeft = 1.0f;
        count_switch = true;

        TimeCheck_flag = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void TimeKoushin()
    {
        //プレイヤーデイを基に、カレンダーの日付に変換。
        if (PlayerStatus.player_day > 365)
        {
            PlayerStatus.player_day = 1;
        }

        _cullent_day = PlayerStatus.player_day;
        month = 0;
        day = 0;

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

        //表示
        _day_text1.text = month.ToString() + "/" + day.ToString();
        _day_text2.text = month.ToString() + "/" + day.ToString();

        //時間を計算
        TimeKeisan();


        //時間のカウント
        timeLeft -= Time.deltaTime;

        //1秒ごとのタイムカウンター
        if (timeLeft <= 0.0)
        {
            timeLeft = 1.0f;
            count_switch = !count_switch;
        }

        if (count_switch)
        {
            //表示
            _time_hour1.text = hour.ToString("00");
            _time_count1.text = ":";
            _time_minute1.text = minute.ToString("00");
            _time_hour2.text = hour.ToString("00");
            _time_count2.text = ":";
            _time_minute2.text = minute.ToString("00");
        }
        else
        {
            //表示
            _time_hour1.text = hour.ToString("00");
            _time_count1.text = " ";
            _time_minute1.text = minute.ToString("00");
            _time_hour2.text = hour.ToString("00");
            _time_count2.text = " ";
            _time_minute2.text = minute.ToString("00");
        }
    }

    void TimeKeisan()
    {
        //時間を計算
        _cullent_time = PlayerStatus.player_time;
        hour = 8; //8時始まり
        minute = 0;

        //10分刻みなので、6ごとに時間を+1
        count = 0;
        while (count < max_time)
        {
            if (_cullent_time >= 6)
            {
                hour++;
                _cullent_time -= 6;

                if(hour > 24 )
                {
                    PlayerStatus.player_day++;
                    hour = 0; //0時にリセット
                }
            }
            else //その月の日付
            {
                minute = _cullent_time * 10; //残り時間 * 10分
                break;
            }
            ++count;
        }

        if (count >= max_time)
        {
            //一日が経った。
            if (TimeCheck_flag)
            {

                //寝るイベントが発生
                GameMgr.scenario_ON = true;
                StartCoroutine("SleepDayEnd");

                //もし、材料採取などしていたら、別でイベントを発生し、家に戻す。か、無視。


                /*
                PlayerStatus.player_time = _cullent_time;
                PlayerStatus.player_day++;

                if (_cullent_time > 0)
                {
                    //さらに日数計算。
                    TimeKeisan();
                }
                */
            }
        }
    }

    IEnumerator SleepDayEnd()
    {
        GameMgr.sleep_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                         //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;
        GameMgr.scenario_ON = false;

        //リセット。
        PlayerStatus.player_day++;
        PlayerStatus.player_time = 0;
    }
}

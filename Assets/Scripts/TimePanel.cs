﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TimePanel : MonoBehaviour {

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

    private Text _time_hour1;
    private Text _time_count1;
    private Text _time_minute1;
    private Text _time_hour2;
    private Text _time_count2;
    private Text _time_minute2;

    private GameObject DebugTimecountUp_button;
    private GameObject DebugTimecountDown_button;

    private Text Contest_nokori_hour;
    private Text Contest_nokori_minute;
    private int _hour, _minute, _nokoritime;

    private GameObject clock_hari1;
    private GameObject clock_hari2;

    private Transform clock_hari1Transform;
    private Transform clock_hari2Transform;

    private Vector3 localAngle1;
    private Vector3 localAngle2;

    private float timeLeft;
    private bool count_switch;

    private int cullent_hour_clock;

    // Use this for initialization
    void Start () {

        _month_obj1 = this.transform.Find("TimeHyouji_1/Image/calender_bg/Month").gameObject;
        _month_text1 = _month_obj1.GetComponent<Text>();

        _monthday_obj1 = this.transform.Find("TimeHyouji_1/Image/calender_bg/Month_Day").gameObject;
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

        Contest_nokori_hour = this.transform.Find("ContestTimePanel/NokoriTimeHour").GetComponent<Text>();
        Contest_nokori_minute = this.transform.Find("ContestTimePanel/NokoriTimeMinutes").GetComponent<Text>();

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

        /*if (GameMgr.Story_Mode == 0)
        {
            this.transform.Find("TimeHyouji_1").GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            this.transform.Find("TimeHyouji_1").GetComponent<CanvasGroup>().alpha = 1;
        }*/
    }
	
	// Update is called once per frame
	void Update () {

        switch(GameMgr.Scene_Category_Num)
        {
            case 100: //コンテストでは、参照する時間の引数を変える。

                this.transform.Find("TimeHyouji_1/Image/calender_bg").gameObject.SetActive(false);
                this.transform.Find("ContestTimePanel").gameObject.SetActive(true);
                ContestTimeDraw();
                break;

            default:
                TimeDraw();
                break;
        }
        

    }

    void TimeDraw()
    {
        //表示
        _time_hour1.text = PlayerStatus.player_cullent_hour.ToString("00");
        _time_minute1.text = PlayerStatus.player_cullent_minute.ToString("00");
        _time_hour2.text = PlayerStatus.player_cullent_hour.ToString("00");
        _time_minute2.text = PlayerStatus.player_cullent_minute.ToString("00");

        //時計版表示           
        localAngle1.z = -1 * PlayerStatus.player_cullent_minute * 6; // ローカル座標を基準に、z軸を軸にした回転
        clock_hari1Transform.localEulerAngles = localAngle1; // 回転角度を設定

        if (PlayerStatus.player_cullent_hour >= 12)
        {
            cullent_hour_clock = PlayerStatus.player_cullent_hour - 12;
        }
        else
        {
            cullent_hour_clock = PlayerStatus.player_cullent_hour;
        }
        localAngle2.z = -1 * 30 * cullent_hour_clock; // ローカル座標を基準に、z軸を軸にした回転
        localAngle2.z = localAngle2.z + (-2.5f * (PlayerStatus.player_cullent_minute / 5));
        clock_hari2Transform.localEulerAngles = localAngle2; // 回転角度を設定

        //表示 月日
        _month_text1.text = PlayerStatus.player_cullent_month.ToString();
        _day_text1.text = PlayerStatus.player_cullent_day.ToString();
        _day_text2.text = PlayerStatus.player_cullent_month.ToString() + "/" + PlayerStatus.player_cullent_day.ToString();


        //表示用時間のカウント 秒がドットで進んでいく表示を更新するだけ。実際の時間には影響なし。
        timeLeft -= Time.deltaTime;

        //1秒ごとのタイムカウンター
        if (timeLeft <= 0.0)
        {
            timeLeft = 1.0f; //現実の1秒の時間。
            count_switch = !count_switch;
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
        //ここまで


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
    }

    void ContestTimeDraw()
    {
        //表示
        _time_hour1.text = PlayerStatus.player_contest_hour.ToString("00");
        _time_minute1.text = PlayerStatus.player_contest_minute.ToString("00");
        _time_hour2.text = PlayerStatus.player_contest_hour.ToString("00");
        _time_minute2.text = PlayerStatus.player_contest_minute.ToString("00");

        //時計版表示           
        localAngle1.z = -1 * PlayerStatus.player_contest_minute * 6; // ローカル座標を基準に、z軸を軸にした回転
        clock_hari1Transform.localEulerAngles = localAngle1; // 回転角度を設定

        if (PlayerStatus.player_contest_hour >= 12)
        {
            cullent_hour_clock = PlayerStatus.player_contest_hour - 12;
        }
        else
        {
            cullent_hour_clock = PlayerStatus.player_contest_hour;
        }
        localAngle2.z = -1 * 30 * cullent_hour_clock; // ローカル座標を基準に、z軸を軸にした回転
        localAngle2.z = localAngle2.z + (-2.5f * (PlayerStatus.player_contest_minute / 5));
        clock_hari2Transform.localEulerAngles = localAngle2; // 回転角度を設定

        //表示 月日
        //_month_text1.text = PlayerStatus.player_cullent_month.ToString();
        //_day_text1.text = PlayerStatus.player_cullent_day.ToString();
        //_day_text2.text = PlayerStatus.player_cullent_month.ToString() + "/" + PlayerStatus.player_cullent_day.ToString();


        //表示用時間のカウント 秒がドットで進んでいく表示を更新するだけ。実際の時間には影響なし。
        timeLeft -= Time.deltaTime;

        //1秒ごとのタイムカウンター
        if (timeLeft <= 0.0)
        {
            timeLeft = 1.0f; //現実の1秒の時間。
            count_switch = !count_switch;
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
        //ここまで


        //残り制限時間を表示
        _hour = 0;
        _minute = 0;
        _nokoritime = PlayerStatus.player_contest_LimitTime;
        if (_nokoritime >= 0)
        {
            while (_nokoritime >= 60)
            {
                _nokoritime = _nokoritime - 60;
                _hour++;
            }
        }
        else //残り時間が制限時間をこえてマイナスになった場合
        {
            while (_nokoritime <= -60)
            {
                _nokoritime = _nokoritime + 60;
                _hour--;
            }
        }
        _minute = _nokoritime * GameMgr.TimeStep; //1分刻み

        Contest_nokori_hour.text = _hour.ToString();
        Contest_nokori_minute.text = _minute.ToString();

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
    }
}

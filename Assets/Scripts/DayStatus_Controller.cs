using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayStatus_Controller : MonoBehaviour
{

    private GameObject _day_param;

    private Text _day_text;

    private List<int> calender = new List<int>();
    private int _cullent_day;
    private int month, day;
    private int count;

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

        _day_param = transform.GetChild(1).gameObject;
        _day_text = _day_param.GetComponent<Text>();

        _cullent_day = PlayerStatus.player_day;

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーデイを基に、カレンダーの日付に変換。
        if (PlayerStatus.player_day > 360)
        {
            PlayerStatus.player_day = 1;
        }

        _cullent_day = PlayerStatus.player_day;
        count = 0;
        month = 0;
        day = 0;

        while (count < calender.Count)
        {
            if ( _cullent_day > calender[count]) { _cullent_day -= calender[count]; }
            else //その月の日付
            {
                month = count + 1; //月　0始まりなので、足す１
                day = _cullent_day; //日
                break;
            }
            ++count;
        }

        _day_text.text = month.ToString() + "月 " + day.ToString() + "日";
    }

}

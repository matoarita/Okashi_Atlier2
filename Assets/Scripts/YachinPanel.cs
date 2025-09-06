using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YachinPanel : MonoBehaviour {

    private TimeController time_controller;

    private GameObject costtext_obj;
    private GameObject costtext_obj2;
    private GameObject otetsuki_obj;

    private Text costtext;
    private Text costtext2;
    private Text nokoriday_text;

    private int i, count;
    private int month_max;
    private int yachin_day, nokori_day;
    private int cullent_yachin;

    private bool month_matagu; //月をまたぐ場合　当月残り日数＋次の月の家賃デイを追加

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitSetting()
    {
        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        costtext_obj = this.transform.Find("Panel/cost_text").gameObject;
        costtext_obj2 = this.transform.Find("Panel/cost_text2").gameObject;
        otetsuki_obj = this.transform.Find("Panel/OtetsukiImg").gameObject;

        costtext = this.transform.Find("Panel/cost_text").GetComponent<Text>();
        costtext2 = this.transform.Find("Panel/cost_text2").GetComponent<Text>();
        nokoriday_text = this.transform.Find("Panel/day_text").GetComponent<Text>();

        if(GameMgr.System_Yachin_ON)
        {
            if (GameMgr.yachinSPRoomON_Flag) //家賃がない家の場合、家賃なくなる
            {
                this.transform.Find("Panel").gameObject.SetActive(true);
            } else
            {
                this.transform.Find("Panel").gameObject.SetActive(false);
            }       
        }
        else
        {
            this.transform.Find("Panel").gameObject.SetActive(false);
        }
    }

    public void YachinHyouji()
    {
        InitSetting();

        //Debug.Log("GameMgr.yachin_otetsuki_count: " + GameMgr.yachin_otetsuki_count);

        if (GameMgr.yachin_otetsuki_count >= 1)
        {
            costtext_obj.SetActive(false);
            costtext_obj2.SetActive(true);
            costtext2.text = GameMgr.Yachin_Cost_cullent.ToString();

            otetsuki_obj.SetActive(true);
        }
        else
        {
            costtext_obj.SetActive(true);
            costtext_obj2.SetActive(false);
            costtext.text = GameMgr.Yachin_Cost_cullent.ToString();

            otetsuki_obj.SetActive(false);
        }

        //現在の日付を確認し、家賃まで何日か計算する
        yachinday_keisan();
        nokoriday_text.text = nokori_day.ToString();
    }

    void yachinday_keisan()
    {
        
        //まず、その月に家賃の〇日が何個あるかチェック　10日ごとなら、28日の場合、10日と20日の２つ
        count = 0;
        month_max = GameMgr.System_calender[PlayerStatus.player_cullent_month - 1];
        while (month_max >= GameMgr.System_Yachin_Day)
        {
            month_max -= GameMgr.System_Yachin_Day;
            count++;
        }

        //今日の日付が、さっきの家賃各日に比べてどこにいるかをチェック。どこの間にいるかを確認してから、次の家賃日を引き算して、残り日数をだす。
        month_matagu = false;
        i = 1;
        while (count >= i)
        {
            if (PlayerStatus.player_cullent_day < GameMgr.System_Yachin_Day * i) //一番下の家賃日より下
            {
                yachin_day = GameMgr.System_Yachin_Day * i; //家賃日の決定
                break;
            }
            else if (PlayerStatus.player_cullent_day > GameMgr.System_Yachin_Day * i) //一番下の家賃日よりは上
            {
                if (count == i) //家賃日最大よりも日数が上だった場合　31日とか。
                {
                    month_matagu = true; //月をまたぎ、次月の最初の家賃日までの日数になる。
                    yachin_day = GameMgr.System_calender[PlayerStatus.player_cullent_month - 1] - PlayerStatus.player_cullent_day;
                    break;
                }
                else
                {
                    i++; //次のカウントへ
                }
            }
            else //家賃日当日
            {
                if (count == i) //家賃日最大と今日の日付が一緒
                {
                    month_matagu = true; //月をまたぎ、次月の最初の家賃日までの日数になる。
                    yachin_day = GameMgr.System_calender[PlayerStatus.player_cullent_month - 1] - PlayerStatus.player_cullent_day;
                }
                else
                {
                    i++;
                    yachin_day = GameMgr.System_Yachin_Day * i; //家賃日の決定　次の日になる。
                }
                break;
            }
        }

        //残り日数を計算
        if (!month_matagu)
        {
            nokori_day = yachin_day - PlayerStatus.player_cullent_day;
        }
        else
        {
            //月をまたいだ場合
            nokori_day = yachin_day + GameMgr.System_Yachin_Day;
        }
    }

    public void Setting_CullentYachin()
    {
        if (GameMgr.OrCompound_RoomNum == 0)
        {
            cullent_yachin = GameMgr.System_Yachin_Cost02;
        }else
        {
            cullent_yachin = GameMgr.System_Yachin_Cost_SPRoom;
        }

        //現在の家賃額を設定
        switch (GameMgr.yachin_otetsuki_count)
        {
            case 0:

                GameMgr.Yachin_Cost_cullent = cullent_yachin; //払う額の設定
                break;

            case 1:
                GameMgr.Yachin_Cost_cullent = cullent_yachin * 2; //払う額の設定
                break;

            default:

                GameMgr.Yachin_Cost_cullent = cullent_yachin; //払う額の設定
                break;
        }
    }
}

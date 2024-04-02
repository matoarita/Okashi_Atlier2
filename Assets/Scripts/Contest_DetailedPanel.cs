using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contest_DetailedPanel : MonoBehaviour {

    private ContestStartListDataBase conteststartList_database;

    private GameObject contest_detailed_datapanel;
    private TimeController time_controller;

    private Text contest_title;
    private Text contest_cost;
    private Text contest_day;
    private Text contest_rankingtype;
    private Text contest_bringtype;
    private Text contest_lv;
    private Text contest_condition;
    private Text contest_theme;
    private Image contest_icon;

    private int _list;
    private int _Rank;

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

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        contest_detailed_datapanel = this.transform.Find("ContestDetailed_datapanel").gameObject;
        contest_title = contest_detailed_datapanel.transform.Find("background/ContestTitle").GetComponent<Text>();
        contest_cost = contest_detailed_datapanel.transform.Find("background/ContestCost").GetComponent<Text>();
        contest_day = contest_detailed_datapanel.transform.Find("background/ContestDay").GetComponent<Text>();
        contest_rankingtype = contest_detailed_datapanel.transform.Find("background/ContestRankingType").GetComponent<Text>();
        contest_bringtype = contest_detailed_datapanel.transform.Find("background/ContestBringType").GetComponent<Text>();
        contest_lv = contest_detailed_datapanel.transform.Find("background/ContestLv").GetComponent<Text>();
        contest_condition = contest_detailed_datapanel.transform.Find("background/ContestCondition").GetComponent<Text>();
        contest_theme = contest_detailed_datapanel.transform.Find("background/ContestThemeComment").GetComponent<Text>();
        contest_icon = contest_detailed_datapanel.transform.Find("background/ContestImgIcon").GetComponent<Image>();
    }

    //
    public void OnContestSettingData(int _id)
    {
        InitSetting();

        _list = conteststartList_database.SearchContestID(_id);

        contest_title.text = conteststartList_database.conteststart_lists[_list].ContestNameHyouji;

        if(conteststartList_database.conteststart_lists[_list].Contest_Cost <= 0)
        {
            contest_cost.text = "無料";
        }
        else
        {
            contest_cost.text = conteststartList_database.conteststart_lists[_list].Contest_Cost.ToString() + GameMgr.MoneyCurrency;
        }

        //月のところが0だった場合、今の日付から〇日後を開催日として計算
        if(conteststartList_database.conteststart_lists[_list].Contest_PMonth == 0)
        {
            time_controller.AfterTimeLimit_Keisan(conteststartList_database.conteststart_lists[_list].Contest_Pday);
            contest_day.text = GameMgr.Contest_OrganizeMonth + "月 " + GameMgr.Contest_OrganizeDay + "日";
        }
        else
        {
            contest_day.text = conteststartList_database.conteststart_lists[_list].Contest_PMonth + "月 " + 
                conteststartList_database.conteststart_lists[_list].Contest_Pday + "日";
        }
        
        

        if(conteststartList_database.conteststart_lists[_list].Contest_RankingType == 0)
        {
            contest_rankingtype.text = "トーナメント";
        }
        else
        {
            contest_rankingtype.text = "ランキング";
        }
        
        if(conteststartList_database.conteststart_lists[_list].Contest_BringType == 0)
        {
            contest_bringtype.text = "〇";
        }
        else
        {
            contest_bringtype.text = "×";
        }

        switch(conteststartList_database.conteststart_lists[_list].Contest_Lv)
        {
            case 1:

                contest_lv.text = "D";
                break;

            case 2:

                contest_lv.text = "C";
                break;

            case 3:

                contest_lv.text = "B";
                break;

            case 4:

                contest_lv.text = "A";
                break;

            case 5:

                contest_lv.text = "S";
                break;
        }

        _Rank = conteststartList_database.conteststart_lists[_list].Contest_PatissierRank;
        if (_Rank <= 1)
        {
            contest_condition.text = "誰でも参加可";
        }
        else if (_Rank > 1 && _Rank <= 3)
        {
            contest_condition.text = "パティシエランク" + "\n" + "シルバー以上";
        }
        else if (_Rank > 3)
        {
            contest_condition.text = "パティシエランク" + "\n" + "ゴールド以上";
        }

        contest_theme.text = conteststartList_database.conteststart_lists[_list].Contest_themeComment;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContestStartListDataBase : SingletonMonoBehaviour<ContestStartListDataBase>
{
    private Entity_ContestStartListDataBase excel_conteststartlist_itemdatabase;

    private ContestPrizeScoreDataBase contestPrizeScore_dataBase;

    private PlayerItemList pitemlist;

    private int _id;
    private int _placenum;
    private string FileName;
    private string Name;
    private string Name_Hyouji;
    private string EnshutuName_Hyouji;
    private string Contest_themeComment;
    private int _pmonth;
    private int _pday;
    private int _endmonth;
    private int _endday;
    private int _cost;
    private int _flag;
    private int _patissierRank;
    private int _lv;
    private int _bringType;
    private int _bringmax;
    private int _rankingType;
    private int _accepted;
    private int _getpt;
    private int _contestVictory;
    private int _contestFightsCount;
    private string ContestBGName;
    private string ContestBG_ChubouName;
    private string ContestBGMSelect;
    private int ContestBGMSelectHall;
    private string Comment_out;
    private int _read_endflag;

    private string contest_name_origin;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    private int fights_count;
    private int total_count;

    public List<ContestStartList> conteststart_lists = new List<ContestStartList>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        ResetDefaultMapExcel();

    }

    public void ResetDefaultMapExcel()
    {
        conteststart_lists.Clear();

        excel_conteststartlist_itemdatabase = Resources.Load("Excel/Entity_ContestStartListDataBase") as Entity_ContestStartListDataBase;


        sheet_no = 0;

        while (sheet_no < excel_conteststartlist_itemdatabase.sheets.Count)
        {
            count = 0;

            while (count < excel_conteststartlist_itemdatabase.sheets[sheet_no].list.Count)
            {
                // 一旦代入
                _id = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestID;
                _placenum = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_placeNum;
                FileName = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].file_name;
                Name = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Name;
                Name_Hyouji = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Name_Hyouji;
                EnshutuName_Hyouji = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_EnshutuName;
                Contest_themeComment = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].theme_comment;
                _pmonth = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Pmonth;
                _pday = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Pday;
                _endmonth = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Endmonth;
                _endday = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Endday;
                _cost = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_cost;
                _flag = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_flag;
                _patissierRank = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_PatissierRank;
                _lv = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Lv;
                _bringType = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_BringType;
                _bringmax = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_BringMax;
                _rankingType = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_RankingType;
                _accepted = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Accepted;
                _getpt = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].GetPatissierPoint;
                _contestVictory = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestVictory;
                _contestFightsCount = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestFightsCount;
                ContestBGName = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestBGName;
                ContestBG_ChubouName = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestBGChubouName;
                ContestBGMSelect = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestBGMSelect;
                ContestBGMSelectHall = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].ContestBGMSelectHall;
                Comment_out = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].comment_out;
                _read_endflag = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].read_endflag;


                //ここでリストに追加している
                conteststart_lists.Add(new ContestStartList(_id, _placenum, FileName, Name, Name_Hyouji, EnshutuName_Hyouji, Contest_themeComment,
                    _pmonth, _pday, _endmonth, _endday, _cost, _flag, _patissierRank, _lv, _bringType, _bringmax,
                    _rankingType, _accepted, _getpt, _contestVictory, _contestFightsCount, ContestBGName, ContestBG_ChubouName, ContestBGMSelect, ContestBGMSelectHall,
                    Comment_out, _read_endflag));

                ++count;
            }
            ++sheet_no;
        }

        //デバッグ用
        /*for(i=0; i< conteststart_lists.Count; i++)
        {
            Debug.Log("conteststart_lists[i]: " + conteststart_lists[i].ContestID + " " + conteststart_lists[i].ContestName);
        }*/
    }

    

    //
    //コンテスト設定
    //
    public void ContestSetting()
    {
        contestPrizeScore_dataBase = ContestPrizeScoreDataBase.Instance.GetComponent<ContestPrizeScoreDataBase>();

        contest_name_origin = conteststart_lists[SearchContestPlaceNum(GameMgr.ContestSelectNum)].ContestName;
        GameMgr.Contest_Name = contest_name_origin;
        GameMgr.Contest_NameHyouji = conteststart_lists[SearchContestPlaceNum(GameMgr.ContestSelectNum)].ContestNameHyouji;
        GameMgr.Contest_EnshutuNameHyouji = conteststart_lists[SearchContestPlaceNum(GameMgr.ContestSelectNum)].ContestEnshutuName;

        if (GameMgr.Contest_Cate_Ranking == 0) //コンテストがトーナメント形式=0
        {
            Debug.Log("トーナメント形式");            

            //コンテストごとに、判定を変える　また、判定はGirlEat_Judgeでも特殊点を判定
            switch (GameMgr.ContestSelectNum)
            {
                case 1000: //オレンジーナコンテストA1

                    GameMgr.ContestRoundNumMax = 3; //そのコンテストの最大のラウンド数

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_1";
                            ContestData_001();
                            break;

                        case 2: //二回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_2";
                            ContestData_002();
                            break;

                        case 3: //決勝戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_3";
                            ContestData_003();
                            break;
                    }

                    break;

                case 2000: //オレンジーナコンテストB1

                    GameMgr.ContestRoundNumMax = 3; //そのコンテストの最大のラウンド数

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_1";
                            ContestData_020();
                            break;

                        case 2: //二回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_2";
                            ContestData_021();
                            break;

                        case 3: //決勝戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_3";
                            ContestData_022();
                            break;
                    }

                    break;

                case 3000: //オレンジーナコンテストC1

                    GameMgr.ContestRoundNumMax = 3; //そのコンテストの最大のラウンド数

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_1";
                            ContestData_040();
                            break;

                        case 2: //二回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_2";
                            ContestData_041();
                            break;

                        case 3: //決勝戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_3";
                            ContestData_042();
                            break;
                    }

                    break;

                case 4000: //オレンジーナコンテストD1　 クープデュモンド

                    GameMgr.ContestRoundNumMax = 3; //そのコンテストの最大のラウンド数

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_1";
                            ContestData_060();
                            break;

                        case 2: //二回戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_2";
                            ContestData_061();
                            break;

                        case 3: //決勝戦

                            GameMgr.Contest_Name_Round = GameMgr.Contest_Name + "_3";
                            ContestData_062();
                            break;
                    }

                    break;
            }

            contestPrizeScore_dataBase.OnPrizeListSet(GameMgr.ContestSelectNum);
        }
        else //ランキング形式=1
        {
            Debug.Log("ランキング形式（一回戦のみ）");
            GameMgr.ContestRoundNumMax = 1; //そのコンテストの最大のラウンド数 １の場合、ランキング形式（複数参加者がランキングで競う）で一回戦のみ
            switch (GameMgr.ContestSelectNum)
            {

                case 10000: //オレンジーナコンテスト　弱小　ランキング形式

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_100();
                            break;
                    }
                    break;

                case 10100: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_101();
                            break;
                    }
                    break;

                case 10200: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_102();
                            break;
                    }
                    break;

                case 10300: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_103();
                            break;
                    }
                    break;

                case 10400: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_104();
                            break;
                    }
                    break;

                case 10500: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_105();
                            break;
                    }
                    break;

                case 10600: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_106();
                            break;
                    }
                    break;

                case 10700: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_107();
                            break;
                    }
                    break;

                case 10800: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_108();
                            break;
                    }
                    break;

                case 10900: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_109();
                            break;
                    }
                    break;

                case 20000: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_200();
                            break;
                    }
                    break;

                case 20100: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_201();
                            break;
                    }
                    break;

                case 20200: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_202();
                            break;
                    }
                    break;

                case 20300: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_203();
                            break;
                    }
                    break;

                case 20400: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_204();
                            break;
                    }
                    break;

                case 20500: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_205();
                            break;
                    }
                    break;

                case 20600: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_206();
                            break;
                    }
                    break;

                case 20700: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_207();
                            break;
                    }
                    break;

                case 20800: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_208();
                            break;
                    }
                    break;

                case 20900: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_209();
                            break;
                    }
                    break;

                case 30000: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_300();
                            break;
                    }
                    break;

                case 30100: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_301();
                            break;
                    }
                    break;

                case 30200: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_302();
                            break;
                    }
                    break;

                case 30300: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_303();
                            break;
                    }
                    break;

                case 30400: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_304();
                            break;
                    }
                    break;

                case 30500: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_305();
                            break;
                    }
                    break;

                case 30600: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_306();
                            break;
                    }
                    break;

                case 30700: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_307();
                            break;
                    }
                    break;

                case 30800: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_308();
                            break;
                    }
                    break;

                case 30900: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_309();
                            break;
                    }
                    break;

                case 40000: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_400();
                            break;
                    }
                    break;

                case 40100: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_401();
                            break;
                    }
                    break;

                case 40200: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_402();
                            break;
                    }
                    break;

                case 40300: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_403();
                            break;
                    }
                    break;

                case 40400: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_404();
                            break;
                    }
                    break;

                case 40500: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_405();
                            break;
                    }
                    break;

                case 40600: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_406();
                            break;
                    }
                    break;

                case 40700: //

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin;
                            ContestRankingData_407();
                            break;
                    }
                    break;
            }

            contestPrizeScore_dataBase.OnPrizeListRankingSet(GameMgr.ContestSelectNum);
            GameMgr.contest_boss_score = GameMgr.PrizeScoreAreaList[GameMgr.PrizeScoreAreaList.Count - 1]; //ランキング形式はここでボススコアにも点いれる

            /*if (GameMgr.ContestRoundNum == 1) //最初のときだけ設定
            {
                contestPrizeScore_dataBase.OnPrizeListRankingSet(GameMgr.ContestSelectNum);
                GameMgr.contest_boss_score = GameMgr.PrizeScoreAreaList[GameMgr.PrizeScoreAreaList.Count - 1]; //ランキング形式はここでボススコアにも点いれる
            }*/
        }
        //コンテストごとに、判定を変える　また、判定はGirlEat_Judgeでも特殊点を判定        
        Debug.Log("コンテスト名前と番号とラウンド数: " + GameMgr.Contest_Name + " " + GameMgr.ContestSelectNum + " " + GameMgr.ContestRoundNum + "回戦");
        Debug.Log("GameMgr.Contest_DB_list_Type: " + GameMgr.Contest_DB_list_Type);

    }

    //トーナメント形式データ トーナメントの選手の名前はこっちのcsで決める

    void ContestData_001()
    {
        //ランダムでもし課題を選ぶ場合は、ここでランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 20000; //compNum=20000~を指定     
        GameMgr.Contest_commentDB_Select = 20000;
        GameMgr.Contest_ProblemSentence = "テーマ：焼き菓子";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。焼き菓子を作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位

        GameMgr.contest_boss_score = 98; //一回戦相手の点数
        GameMgr.contest_boss_name = "エリカ";
    }

    void ContestData_002()
    {
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 21000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 21000;
        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類は問わず。おいしいお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 137; //
        GameMgr.contest_boss_name = "シスター・リーシュ";
    }

    void ContestData_003()
    {
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 22000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 22000;
        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。腕によりをかけた至高の一品を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 183; //
        GameMgr.contest_boss_name = "ハーマイオニー";
    }

    void ContestData_020()
    {
        //ランダムでもし課題を選ぶ場合は、ここでランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 30000; //compNum=20000~を指定  
        GameMgr.Contest_commentDB_Select = 20000;
        GameMgr.Contest_ProblemSentence = "テーマ：焼き菓子";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。焼き菓子を作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位

        GameMgr.contest_boss_score = 162; //一回戦相手の点数
        GameMgr.contest_boss_name = "おそうじアリス";
    }

    void ContestData_021()
    {
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 31000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 21000;
        GameMgr.Contest_ProblemSentence = "テーマ：光魔法を使ったお菓子";
        GameMgr.Contest_ProblemSentence2 = "光魔法で仕上げたかわいいお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 191; //
        GameMgr.contest_boss_name = "ウリユ";
    }

    void ContestData_022()
    {
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 32000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 22000;
        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。腕によりをかけた至高の一品を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 250; //
        GameMgr.contest_boss_name = "イセヤ";
    }

    void ContestData_040()
    {
        //ランダムでもし課題を選ぶ場合は、ここでランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 40000; //compNum=20000~を指定    
        GameMgr.Contest_commentDB_Select = 20000;
        GameMgr.Contest_ProblemSentence = "テーマ：チョコレート";
        GameMgr.Contest_ProblemSentence2 = "材料・種類は問わず。おいしいチョコレートを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 178; //一回戦相手の点数
        GameMgr.contest_boss_name = "フォルトーネ";
    }

    void ContestData_041()
    {
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 41000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 21000;
        GameMgr.Contest_ProblemSentence = "テーマ：風をテーマにしたチョコレート";
        GameMgr.Contest_ProblemSentence2 = "地球の美しい青を表現するチョコレートを作ってください。" + "\n" + "制限時間: 8時間";
        
        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 312; //
        GameMgr.contest_boss_name = "クルル";
    }

    void ContestData_042()
    {
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 42000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 22000;
        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類は問わず。腕によりをかけた至高の一品を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 500; //
        GameMgr.contest_boss_name = "ベル";
    }

    void ContestData_060()
    {
        //ランダムでもし課題を選ぶ場合は、ここでランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 50000; //compNum=20000~を指定   
        GameMgr.Contest_commentDB_Select = 20000;
        GameMgr.Contest_ProblemSentence = "テーマ：チョコレートケーキ限定";
        GameMgr.Contest_ProblemSentence2 = "おいしい伝統的なチョコレートケーキを作ってください。" + "\n" + "制限時間: 8時間";
        

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 163; //一回戦相手の点数
        GameMgr.contest_boss_name = "シャルロット";
    }

    void ContestData_061()
    {
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 51000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 21000;
        GameMgr.Contest_ProblemSentence = "テーマ：「風」をテーマにした至高のチョコレート";
        GameMgr.Contest_ProblemSentence2 = "風・緑の美しさをイメージするチョコレートを作ってください。" + "\n" + "制限時間: 8時間";


        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 228; //
        GameMgr.contest_boss_name = "クラリス";
    }

    void ContestData_062()
    {
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 52000; //compNum=20000~を指定
        GameMgr.Contest_commentDB_Select = 22000;
        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類は問わず。一番自信のあるお菓子を作ってください。" + "\n" + "制限時間: 8時間";


        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 333; //
        GameMgr.contest_boss_name = "シュバルツヴェルダー";
    }

    //



    //ランキング形式データ　選手の名前は、ContestPrizeScoreDBで決める

    void ContestRankingData_100() //クッキーノービスカップ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題で使用する（コンテストに固有の判定入れなくてよい）
        GameMgr.Contest_DB_list_Type = 100000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：おいしいクッキー";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず、おいしいクッキーを作ってください。" + "\n" + "制限時間: 4時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 240; //制限時間　1分単位       

        //支給品があれば、追加する
        GameMgr.ContestItem_supplied_List.Clear();
        GameMgr.ContestItem_supplied_KosuList.Clear();
        GameMgr.ContestItem_supplied_List.Add("komugiko_supplied");
        GameMgr.ContestItem_supplied_KosuList.Add(5);
        GameMgr.ContestItem_supplied_List.Add("butter_supplied");
        GameMgr.ContestItem_supplied_KosuList.Add(5);
        GameMgr.ContestItem_supplied_List.Add("suger_supplied");
        GameMgr.ContestItem_supplied_KosuList.Add(5);
    }

    void ContestRankingData_101() //オランジーナ・パティスリーアワード　ここで少し大き目のコンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題で使用する（コンテストに固有の判定入れなくてよい）
        GameMgr.Contest_DB_list_Type = 101000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：ケーキorクリームブリュレ";
        GameMgr.Contest_ProblemSentence2 = "ケーキ、もしくはクリームブリュレで、おいしいお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_102() //ベオルブ系のディナー
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題で使用する（コンテストに固有の判定入れなくてよい）
        GameMgr.Contest_DB_list_Type = 102000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類は問わず。華やかなお菓子を作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_103() //ラスク・ブロカント
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 103000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：ラスク";
        GameMgr.Contest_ProblemSentence2 = "アンティーク市場で手軽に食べられるラスクを作ること。" + "\n" + "制限時間: 5時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 300; //制限時間　1分単位      

        //支給品があれば、追加する
        GameMgr.ContestItem_supplied_List.Clear();
        GameMgr.ContestItem_supplied_KosuList.Clear();
        GameMgr.ContestItem_supplied_List.Add("bugget_supplied"); //サプライドリスト（_supplied_List）に追加したアイテムは、全て後で削除される。特に設定は必要なし。
        GameMgr.ContestItem_supplied_KosuList.Add(2);
    }

    void ContestRankingData_104() //ルミエールエピファニア
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 104000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：光のお菓子限定";
        GameMgr.Contest_ProblemSentence2 = "光をイメージしたお菓子を作ること" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_105() //ルミエールカンデラ SP9キラキラ感も採点の対象になる
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 105000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：光のお菓子限定";
        GameMgr.Contest_ProblemSentence2 = "光の魔法で仕上げたお菓子を作ること" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_106() //ガレットデロワ　オペラ・ザッハトルテ・ファンタジアン基準
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 106000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：おいしいケーキ";
        GameMgr.Contest_ProblemSentence2 = "春の大精霊様にふさわしい豪華なケーキを作ること" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_107() //ディオ・ショコラ・チャンピオンシップ　遥かなる蒼・チョコ全般なら全部OK
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 107000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：チョコレート";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。おいしいチョコレートを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_108() //フィナンシェバターズカップ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 108000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：フィナンシェ";
        GameMgr.Contest_ProblemSentence2 = "おいしいフィナンシェを作ってください。" + "\n" + "制限時間: 4時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 240; //制限時間　1分単位          
    }

    void ContestRankingData_109() //春の大祭典　自由課題　中級者向け
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 108000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。おいしいおかしを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_200() //ひんやりお菓子コンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 120000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：ひんやりしたお菓子";
        GameMgr.Contest_ProblemSentence2 = "アイスやゼリーなど、冷たいお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_201() //フライング・ソーダコンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 121000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：ソーダ";
        GameMgr.Contest_ProblemSentence2 = "暑さを吹き飛ばすシュワシュワ爽快なソーダを作ってください！！" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_202() //ボンボヤージュ・カップ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 122000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "オランジーナビーチに合うオシャレなおかしを作ってください！" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_203() //おみやげおかしコンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 123000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：おみやげお菓子＜クッキー・ラスクを除く＞";
        GameMgr.Contest_ProblemSentence2 = "おみやげに合う子供が喜ぶスイーツを希望じゃ！" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_204() //スカーレットマイスター
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 124000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：いちごのお菓子";
        GameMgr.Contest_ProblemSentence2 = "いちご好きがうなる、おいしいいちごのお菓子を作ってください。" + "\n" + "制限時間: 4時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 240; //制限時間　1分単位          
    }

    void ContestRankingData_205() //遥かなる蒼賞
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 125000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：海をテーマにしたチョコレート";
        GameMgr.Contest_ProblemSentence2 = "海の美しさを表現したチョコレートを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_206() //マジックパティスリー・アワード
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 126000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：魔法お菓子限定";
        GameMgr.Contest_ProblemSentence2 = "魔法で作る、ちょっと変わった見た目のお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_207() //プラム洋菓子技術コンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 127000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。クオリティの高いおかしを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_208() //チョコレート初級コンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 127000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：チョコレート";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。おいしいチョコレートを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_209() //パティシエの森
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 127000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。自由にお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_300() //クレープ・ドゥ・シャノワール
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 140000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：おいしいクレープ";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。黒猫たちが喜ぶクレープを作ってください。" + "\n" + "制限時間: 4時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 240; //制限時間　1分単位          
    }

    void ContestRankingData_301() //アデュルティ・ガトー
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 141000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：大人っぽいお菓子";
        GameMgr.Contest_ProblemSentence2 = "大人に似合う、アダルトで渋いお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_302() //メルヘンランド♪カップ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 142000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：メルヘンでかわいいお菓子";
        GameMgr.Contest_ProblemSentence2 = "メルヘンでかわいい見た目のお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_303() //キラキラ・ボンボンズ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 143000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：子供向けのお菓子";
        GameMgr.Contest_ProblemSentence2 = "子供が喜びそうな、子供向けのお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_304() //英国ティータイムコンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 144000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：ティー";
        GameMgr.Contest_ProblemSentence2 = "香り高いお茶を作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_305() //ピエスモンテ 彫刻おかし限定 特定のおかしでないと通らない＋食感の判定は下方調整
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 145000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：芸術的なおかし限定";
        GameMgr.Contest_ProblemSentence2 = "場を彩る芸術性の高いお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_306() //コンチェルティーノ・イン・ブルー
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 146000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：青をテーマにしたお菓子";
        GameMgr.Contest_ProblemSentence2 = "青色をイメージした美しいお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_307() //ビジョウ・パティスリー・カップ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 147000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：キラキラお菓子限定";
        GameMgr.Contest_ProblemSentence2 = "キラキラをモチーフにしたお菓子を作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_308() //デザインお菓子コンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 147000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：デザインお菓子";
        GameMgr.Contest_ProblemSentence2 = "見た目を重視した派手なお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_309() //秋のお菓子コンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 147000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "材料・種類問わず。自由にお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_400() //クワイットスノウ　自由課題　雪国に合うお菓子あと少し点数高い
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 1; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 160000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：自由課題";
        GameMgr.Contest_ProblemSentence2 = "雪国に合う自由なお菓子を作ってください。" + "\n" + "制限時間: 4時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 240; //制限時間　1分単位          
    }

    void ContestRankingData_401() //アムール・チョコレイト・コンテスト
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 161000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：愛をテーマにしたチョコレート";
        GameMgr.Contest_ProblemSentence2 = "愛にあふれたチョコレートを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_402() //ネオユニバース・カップ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 162000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：宇宙をテーマにしたお菓子";
        GameMgr.Contest_ProblemSentence2 = "宇宙のようなきらびやかなお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_403() //フェド・フルラージュ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 163000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：お花のクッキー・ケーキ・チョコレート";
        GameMgr.Contest_ProblemSentence2 = "お花をモチーフにしたお菓子を作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    void ContestRankingData_404() //ルミエール・ドゥ・ソレイユ　あたたかいお菓子限定　アフォガートやアップルパイ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 164000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：温かいお菓子限定＜焼き菓子は除く＞";
        GameMgr.Contest_ProblemSentence2 = "寒さを飛ばす温かいお菓子を作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_405() //ミルフイユ・ドゥ・パリ
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 165000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：おいしいミルフイユ";
        GameMgr.Contest_ProblemSentence2 = "パリパリサクサクおいしいミルフイユを作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_406() //チーズケーキ・パティスリーアワード
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 166000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：チーズケーキ";
        GameMgr.Contest_ProblemSentence2 = "おいしいチーズケーキを作ってください。" + "\n" + "制限時間: 6時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 360; //制限時間　1分単位          
    }

    void ContestRankingData_407() //夢見るチョコレート選手権　愛＋メルヘンが高いチョコレートのお菓子
    {
        //ランダムでもし課題を選ぶ場合は、ContestDataをランダムで指定してよい
        GameMgr.Contest_JudgeType = 0; //1のときは、女の子の好み判定を使用する　自由課題など
        GameMgr.Contest_DB_list_Type = 167000; //compNum=100000~を指定
        GameMgr.Contest_commentDB_Select = 100000;

        GameMgr.Contest_ProblemSentence = "テーマ：愛とメルヘンがテーマのチョコレート";
        GameMgr.Contest_ProblemSentence2 = "夢見る女の子のための、かわいいチョコレートを作ってください。" + "\n" + "制限時間: 8時間";

        //コンテスト時間指定
        Contest_SetStartTime();
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位          
    }

    //

    //コンテスト名をいれると、そのコンテストを解禁する
    public void contestHyoujiKaikin(string _name)
    {
        for (i = 0; i < conteststart_lists.Count; i++)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                conteststart_lists[i].Contest_Flag = 1;
            }
        }
    }

    //コンテストIDをいれると、そのコンテストのリストIDを返すメソッド
    public int SearchContestID(int ID)
    {
        i = 0;
        while (i < conteststart_lists.Count)
        {
            if (conteststart_lists[i].ContestID == ID)
            {
                return i;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999
    }

    //コンテストplace_numをいれると、そのコンテストのリストIDを返すメソッド
    public int SearchContestPlaceNum(int ID)
    {
        i = 0;
        while (i < conteststart_lists.Count)
        {
            if (conteststart_lists[i].Contest_placeNumID == ID)
            {
                return i;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999
    }

    //コンテスト名をいれると、そのコンテストのリストIDを返すメソッド
    public int SearchContestString(string Name)
    {
        if (Name == "Non")
        {
            return 9999;
        }
        else
        {
            i = 0;
            while (i < conteststart_lists.Count)
            {
                if (conteststart_lists[i].ContestName == Name)
                {
                    return i;
                }
                i++;
            }

            return 9999; //見つからなかった場合、9999
        }
    }

    //コンテスト名をいれると、そのコンテストの受付フラグをONにする
    public void SetContestAcceptedString(string _name)
    {
        i = 0;
        while (i < conteststart_lists.Count)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                conteststart_lists[i].Contest_Accepted = 1;
                break;
            }
            i++;
        }
    }

    //コンテスト名とランキング順位を入れると、そのコンテストのこれまでの成績を更新する。より上位のものに置き換える
    public void SetContestVictroyString(string _name, int _rank)
    {
        i = 0;
        while (i < conteststart_lists.Count)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                if (conteststart_lists[i].ContestVictory != 0)
                {
                    if (conteststart_lists[i].ContestVictory > _rank)
                    {
                        conteststart_lists[i].ContestVictory = _rank;
                        break;
                    }
                    else
                    {
                        break; //前回より順位が低かったので、更新せずbreak
                    }
                }
                else
                {
                    conteststart_lists[i].ContestVictory = _rank;
                    break;
                }
            }
            i++;
        }

        //デバッグ用
        /*for(i=0; i< conteststart_lists.Count; i++)
        {
            Debug.Log("コンテスト順位: " + conteststart_lists[i].ContestNameHyouji + ": " + conteststart_lists[i].ContestVictory);
        }*/
    }

    //名前をいれると、そのコンテストの出場回数をかえす
    public int ContestName_FightCount(string _name)
    {
        i = 0;
        while (i < conteststart_lists.Count)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                return conteststart_lists[i].ContestFightsCount;
            }
            i++;
        }

        return 0; //該当なかった場合も0
    }

    //これまでのコンテストの総出場回数を返す
    public int ContestAllFightsCount()
    {
        i = 0;
        fights_count = 0;

        while (i < conteststart_lists.Count)
        {
            fights_count += conteststart_lists[i].ContestFightsCount;
            i++;
        }

        return fights_count;
    }

    //エデンコンの初出場かそうでないかをチェックする
    public void EdenFirstVictoryCheck(string _contestName)
    {
        GameMgr.EdenPrizeChange = false;
        GameMgr.EdenFirstVictory = false;

        switch (_contestName)
        {
            case "Or_Contest_001": //春コン

                //ハーマイオニーはいくら負けても、大会優勝時にエデンゲット
                GameMgr.EdenPrizeChange = false;
                GameMgr.EdenFirstVictory = true;
                //FirstVictoryCheck(_contestName);
                break;

            case "Or_Contest_002": //夏コン

                FirstVictoryCheck(_contestName);
                break;

            case "Or_Contest_003": //秋コン

                FirstVictoryCheck(_contestName);
                break;
        }
    }

    void FirstVictoryCheck(string _contestName)
    {
        GameMgr.EdenPrizeChange = true;

        if (ContestName_FightCount(_contestName) <= 1) //初出場
        {
            if (GameMgr.contest_Rank_Count == 1) //優勝した場合　そのままエデンレシピもらう　かつ　コンテスト終了後の敵キャラの会話がスキップ
            {
                GameMgr.EdenFirstVictory = true;
            }
        }
        else //二回目以降
        {
            //もらえるアイテムがレシピでない　かつ　コンテスト終了後の敵キャラの会話でエデンもらえる
            GameMgr.EdenFirstVictory = false;
        }
    }

    //ランクを入れると、それに合わせたグレードに表記を変換する
    public string RankToGradeText(int _rank)
    {
        switch (_rank)
        {
            case 1:

                return "★"; //優しい　一番簡単・ありふれたの意味
                            //return "Gentle";　//優しい　一番簡単・ありふれたの意味

            case 2:

                return "★★";
            //return "IPA-1"; //国際パティシエ協会の略

            case 3:

                return "★★★";
            //return "G3";

            case 4:

                return "★★★★";
            //return "G2";

            case 5:

                return "★★★★★";
                //return "G1";
        }

        return "-"; //例外処理
    }

    //支給品アイテム　あれば追加する処理　コンテスト後、削除される。
    public void AddContest_SurppliedItem()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        if (GameMgr.ContestItem_supplied_List.Count > 0)
        {
            for(i=0; i< GameMgr.ContestItem_supplied_List.Count; i++)
            {
                Debug.Log("支給品アイテムを追加: " + GameMgr.ContestItem_supplied_List[i] + " " + GameMgr.ContestItem_supplied_KosuList[i]);
                pitemlist.addPlayerItemString(GameMgr.ContestItem_supplied_List[i], GameMgr.ContestItem_supplied_KosuList[i]);
            }
        }
    }

    //ロード時に、コンテストデータを読み込み、フラグを上書きする
    public void SetContestFightsData(string _name, int _fightscount, int _victory)
    {      
        for(i = 0; i < conteststart_lists.Count; i++)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                conteststart_lists[i].ContestFightsCount = _fightscount;
                conteststart_lists[i].ContestVictory = _victory;
            }
        }
    }

    //優勝記録と戦った回数の初期化
    public void ResetDefaultFightsData()
    {
        for (i = 0; i < conteststart_lists.Count; i++)
        {
                conteststart_lists[i].ContestFightsCount = 0;
                conteststart_lists[i].ContestVictory = 0;
        }
    }

    //順位を何回とったかのカウントを取得する　順位をいれると、その順位の取得数合計を返す
    public int ReturnVictoryCount(int _rank)
    {
        total_count = 0;
        for (i = 0; i < conteststart_lists.Count; i++)
        {
            if(conteststart_lists[i].ContestVictory == _rank)
            {
                total_count++;
            }
        }

        return total_count;
    }

    //名前を入れると、そのコンテストの最高順位を返す
    public int SearchContestVictory(string _name)
    {
        i = 0;
        while (i < conteststart_lists.Count)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                return conteststart_lists[i].ContestVictory;
            }
            i++;
        }

        return 9999; //見つからなかった場合、9999
    }

    void Contest_SetStartTime()
    {
        PlayerStatus.player_contest_hour = 10; //コンテストの開始時間
        PlayerStatus.player_contest_minute = 0; //開始分
    }
}
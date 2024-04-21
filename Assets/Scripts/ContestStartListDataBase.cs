using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContestStartListDataBase : SingletonMonoBehaviour<ContestStartListDataBase>
{
    private Entity_ContestStartListDataBase excel_conteststartlist_itemdatabase;

    private int _id;
    private int _placenum;
    private string FileName;
    private string Name;
    private string Name_Hyouji;
    private string Contest_themeComment;
    private int _pmonth;
    private int _pday;
    private int _cost;
    private int _flag;
    private int _patissierRank;
    private int _lv;
    private int _bringType;
    private int _bringmax;
    private int _rankingType;
    private int _accepted;
    private int _getpt;
    private string Comment_out;
    private int _read_endflag;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

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
                Contest_themeComment = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].theme_comment;
                _pmonth = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Pmonth;
                _pday = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Pday;
                _cost = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_cost;
                _flag = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_flag;
                _patissierRank = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_PatissierRank;
                _lv = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Lv;
                _bringType = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_BringType;
                _bringmax = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_BringMax;
                _rankingType = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_RankingType;
                _accepted = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].Contest_Accepted;
                _getpt = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].GetPatissierPoint;
                Comment_out = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].comment_out;
                _read_endflag = excel_conteststartlist_itemdatabase.sheets[sheet_no].list[count].read_endflag;


                //ここでリストに追加している
                conteststart_lists.Add(new ContestStartList(_id, _placenum, FileName, Name, Name_Hyouji, Contest_themeComment,
                    _pmonth, _pday, _cost, _flag, _patissierRank, _lv, _bringType, _bringmax,
                    _rankingType, _accepted, _getpt, Comment_out, _read_endflag));

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
        while (i <= conteststart_lists.Count)
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
        while (i <= conteststart_lists.Count)
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
            while (i <= conteststart_lists.Count)
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
        while (i <= conteststart_lists.Count)
        {
            if (conteststart_lists[i].ContestName == _name)
            {
                conteststart_lists[i].Contest_Accepted = 1;
                break;
            }
            i++;
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
}
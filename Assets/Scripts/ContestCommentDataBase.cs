using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//シングルトン化しているので、ゲーム中ItemDataBaseは一個だけ。Findで探す必要もないので、itemDataBaseクラスを使うときは、その書き方にならうこと。
//できれば、ゲーム中のタイトル画面などで、一回だけ読むのがふさわしい。今は、mainで毎回読み込んでいるので、あとで修正が必要。

public class ContestCommentDataBase : SingletonMonoBehaviour<ContestCommentDataBase>
{
    private Entity_ContestCommentDataBase excel_contestcomment_database;

    private int _id;
    private int _commentid;
    private string itemName;
    private int _setid;
    private string comment_1;
    private string comment_2;
    private string comment_3;
    private string comment_4;
    private int search_flag;

    private int i, j;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<ContestComment> contestcomment_lists = new List<ContestComment>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        contestcomment_lists.Clear();

        excel_contestcomment_database = Resources.Load("Excel/Entity_ContestCommentDataBase") as Entity_ContestCommentDataBase;


        sheet_no = 0;

        while (sheet_no < excel_contestcomment_database.sheets.Count)
        {
            count = 0;
            while (count < excel_contestcomment_database.sheets[sheet_no].list.Count)
            {
                //代入
                SetExcelDB();

                ++count;
            }

            ++sheet_no;
        }
    }

    void SetExcelDB()
    {
        // 一旦代入
        _id = excel_contestcomment_database.sheets[sheet_no].list[count].ID;
        _commentid = excel_contestcomment_database.sheets[sheet_no].list[count].commentID;
        itemName = excel_contestcomment_database.sheets[sheet_no].list[count].item_Name;
        _setid = excel_contestcomment_database.sheets[sheet_no].list[count].setID;
        comment_1 = excel_contestcomment_database.sheets[sheet_no].list[count].comment1;
        comment_2 = excel_contestcomment_database.sheets[sheet_no].list[count].comment2;
        comment_3 = excel_contestcomment_database.sheets[sheet_no].list[count].comment3;
        comment_4 = excel_contestcomment_database.sheets[sheet_no].list[count].comment4;
        search_flag = excel_contestcomment_database.sheets[sheet_no].list[count].search_endflag;

        //ここでリストに追加している
        contestcomment_lists.Add(new ContestComment(_id, _commentid, itemName, _setid, comment_1, comment_2, comment_3, comment_4, search_flag));
    }
}
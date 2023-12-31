﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlLikeCompoDataBase : SingletonMonoBehaviour<GirlLikeCompoDataBase>
{
    private Entity_GirlLikeSetCompoDataBase excel_girlLikecompo_database; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private int _id;
    private int _setid;
    private int _nextid;

    private int _set1;
    private int _set2;
    private int _set3;

    private string _spquest_name1;
    private string _spquest_name2;
    private string _spquest_name3;
    private string _desc;
    private string _comment;

    private int _set_flag;
    private int _set_score;

    private string _hint_text;
    private bool _clear_flag;

    private string _fileName;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<int> sheet_topendID = new List<int>(); //シートごとに、IDの頭と最後を、順番に入れている。[0][1]は、シート０のIDの頭、と最後、という感じ。

    public List<GirlLikeCompo> girllike_composet = new List<GirlLikeCompo>();

    //ランダム選択用のセット
    public List<GirlLikeCompo> girllike_compoRandomset = new List<GirlLikeCompo>();

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です。
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        girllike_composet.Clear();

        excel_girlLikecompo_database = Resources.Load("Excel/Entity_GirlLikeSetCompoDataBase") as Entity_GirlLikeSetCompoDataBase;

        sheet_topendID.Add(0); //シートの頭のIDは0。

        sheet_no = 0;

        while (sheet_no < excel_girlLikecompo_database.sheets.Count)
        {
            count = 0;

            while (count < excel_girlLikecompo_database.sheets[sheet_no].list.Count)
            {
                // 一旦代入
                _id = excel_girlLikecompo_database.sheets[sheet_no].list[count].ID;
                _setid = excel_girlLikecompo_database.sheets[sheet_no].list[count].set_compID;
                _nextid = excel_girlLikecompo_database.sheets[sheet_no].list[count].next_questID;

                _set1 = excel_girlLikecompo_database.sheets[sheet_no].list[count].set1;
                _set2 = excel_girlLikecompo_database.sheets[sheet_no].list[count].set2;
                _set3 = excel_girlLikecompo_database.sheets[sheet_no].list[count].set3;

                _spquest_name1 = excel_girlLikecompo_database.sheets[sheet_no].list[count].spquest_name1;
                _spquest_name2 = excel_girlLikecompo_database.sheets[sheet_no].list[count].spquest_name2;
                _spquest_name3 = excel_girlLikecompo_database.sheets[sheet_no].list[count].spquest_name3;
                _desc = excel_girlLikecompo_database.sheets[sheet_no].list[count].desc;
                _comment = excel_girlLikecompo_database.sheets[sheet_no].list[count].comment;

                _set_flag = excel_girlLikecompo_database.sheets[sheet_no].list[count].set_flag;
                _set_score = excel_girlLikecompo_database.sheets[sheet_no].list[count].set_score;

                _hint_text = excel_girlLikecompo_database.sheets[sheet_no].list[count].hint_text;
                _clear_flag = excel_girlLikecompo_database.sheets[sheet_no].list[count].clear;

                _fileName = excel_girlLikecompo_database.sheets[sheet_no].list[count].file_name;

                //ここでリストに追加している
                girllike_composet.Add(new GirlLikeCompo(_id, _setid, _nextid, _set1, _set2, _set3, _spquest_name1, _spquest_name2, _spquest_name3, _desc, _comment, _set_flag, _set_score, _hint_text, _clear_flag, _fileName));

                //Debug.Log("GirlLike_tp01: " + girllikeset[count].girlLike_topping[0]);

                ++count;


            }

            sheet_topendID.Add(_id); // sheetの終わりのIDを入れる。シート0～から。

            sheet_no++;
        }
    }

    public void StageSet()
    {
        //flagが1のもののみのセットを作る。+食べたことがないものだけ。
        girllike_compoRandomset.Clear();
        count = 0;

        while (count < girllike_composet.Count)
        {
            if (girllike_composet[count].set_flag == 1)
            {
                // 一旦代入
                _id = girllike_composet[count].ID;
                _setid = girllike_composet[count].set_ID;
                _nextid = girllike_composet[count].next_ID;

                _set1 = girllike_composet[count].set1;
                _set2 = girllike_composet[count].set2;
                _set3 = girllike_composet[count].set3;

                _spquest_name1 = girllike_composet[count].spquest_name1;
                _spquest_name2 = girllike_composet[count].spquest_name2;
                _spquest_name3 = girllike_composet[count].spquest_name3;

                _desc = girllike_composet[count].desc;
                _comment = girllike_composet[count].comment;

                _set_flag = girllike_composet[count].set_flag;

                _set_score = girllike_composet[count].set_score;

                _hint_text = girllike_composet[count].hint_text;
                _clear_flag = girllike_composet[count].clearFlag;

                _fileName = girllike_composet[count].fileName;

                //ここでリストに追加している
                girllike_compoRandomset.Add(new GirlLikeCompo(_id, _setid, _nextid, _set1, _set2, _set3, _spquest_name1, _spquest_name2, _spquest_name3, _desc, _comment, _set_flag, _set_score, _hint_text, _clear_flag, _fileName));

            }

            ++count;
        }
    }

    //指定したコンプIDのフラグをONかOFFにする。
    public void SetGirlSetFlag(int _compID, int _flag)
    {
        for (i = 0; i < girllike_composet.Count; i++)
        {
            if (girllike_composet[i].set_ID == _compID)
            {
                girllike_composet[i].set_flag = _flag;
            }
        }
    }
}

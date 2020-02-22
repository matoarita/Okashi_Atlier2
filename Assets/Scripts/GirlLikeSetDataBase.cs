﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlLikeSetDataBase : SingletonMonoBehaviour<GirlLikeSetDataBase>
{
    private Entity_GirlLikeSetDataBase excel_girlLikeset_database; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private int _id;
    private int _compnum;

    private string _itemname;
    private string _itemsubtype;

    private int _rich;
    private int _sweat;
    private int _bitter;
    private int _sour;

    private int _crispy;
    private int _fluffy;
    private int _smooth;
    private int _hardness;
    private int _jiggly;
    private int _chewy;

    private string _tp01;
    private string _tp02;
    private string _tp03;
    private string _tp04;
    private string _tp05;

    private string _setkansou;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<int> sheet_topendID = new List<int>(); //シートごとに、IDの頭と最後を、順番に入れている。[0][1]は、シート０のIDの頭、と最後、という感じ。

    public List<GirlLikeSet> girllikeset = new List<GirlLikeSet>();

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です。
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        girllikeset.Clear();

        excel_girlLikeset_database = Resources.Load("Excel/Entity_GirlLikeSetDataBase") as Entity_GirlLikeSetDataBase;

        sheet_topendID.Add(0); //シートの頭のIDは0。

        sheet_no = 0;

        while (sheet_no < excel_girlLikeset_database.sheets.Count)
        {
            count = 0;

            while (count < excel_girlLikeset_database.sheets[sheet_no].list.Count)
            {
                // 一旦代入
                _id = excel_girlLikeset_database.sheets[sheet_no].list[count].setID;
                _compnum = excel_girlLikeset_database.sheets[sheet_no].list[count].compNum;
                _itemname = excel_girlLikeset_database.sheets[sheet_no].list[count].girllike_itemname;
                _itemsubtype = excel_girlLikeset_database.sheets[sheet_no].list[count].girllike_itemsubtype;

                _rich = excel_girlLikeset_database.sheets[sheet_no].list[count].rich;
                _sweat = excel_girlLikeset_database.sheets[sheet_no].list[count].sweat;
                _bitter = excel_girlLikeset_database.sheets[sheet_no].list[count].bitter;
                _sour = excel_girlLikeset_database.sheets[sheet_no].list[count].sour;

                _crispy = excel_girlLikeset_database.sheets[sheet_no].list[count].crispy;
                _fluffy = excel_girlLikeset_database.sheets[sheet_no].list[count].fluffy;
                _smooth = excel_girlLikeset_database.sheets[sheet_no].list[count].smooth;
                _hardness = excel_girlLikeset_database.sheets[sheet_no].list[count].hardness;
                _jiggly = excel_girlLikeset_database.sheets[sheet_no].list[count].jiggly;
                _chewy = excel_girlLikeset_database.sheets[sheet_no].list[count].chewy;

                _tp01 = excel_girlLikeset_database.sheets[sheet_no].list[count].topping01;
                _tp02 = excel_girlLikeset_database.sheets[sheet_no].list[count].topping02;
                _tp03 = excel_girlLikeset_database.sheets[sheet_no].list[count].topping03;
                _tp04 = excel_girlLikeset_database.sheets[sheet_no].list[count].topping04;
                _tp05 = excel_girlLikeset_database.sheets[sheet_no].list[count].topping05;

                _setkansou = excel_girlLikeset_database.sheets[sheet_no].list[count].desc;

                //ここでリストに追加している
                girllikeset.Add(new GirlLikeSet(_id, _compnum, _itemname, _itemsubtype,
                    _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy,
                    _tp01, _tp02, _tp03, _tp04, _tp05, _setkansou));

                //Debug.Log("GirlLike_tp01: " + girllikeset[count].girlLike_topping[0]);

                ++count;

                
            }

            sheet_topendID.Add(_id); // sheetの終わりのIDを入れる。シート0～から。

            sheet_no++;
        }
    }
}
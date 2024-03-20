using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestSetDataBase : SingletonMonoBehaviour<ContestSetDataBase>
{
    private Entity_ContestSetDataBase excel_contestset_database; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    //コンテストの判定用　中身はGirlLikeSetとGirlLikeSetDatabaseと項目は共通　分けてもいいが、判定時girl1_statusの判定メソッド中身に注意。書き足しなどが必要。

    private int _id;
    private int _compnum;

    private string _itemname;
    private string _itemsubtype;

    private int _set_score;

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
    private int _juice;

    private int _beauty;

    private int _sp1_wind; //テーマに応じた特別な採点　「風らしさ」など。コンテストごとに参照する得点が変わる。

    private string _tp01;
    private string _tp02;
    private string _tp03;
    private string _tp04;
    private string _tp05;
    private string _tp06;
    private string _tp07;
    private string _tp08;
    private string _tp09;

    private int _tp_score01;
    private int _tp_score02;
    private int _tp_score03;
    private int _tp_score04;
    private int _tp_score05;
    private int _tp_score06;
    private int _tp_score07;
    private int _tp_score08;
    private int _tp_score09;
    private int _non_tp_score;

    private string _setkansou;

    private int _comment_flag;
    private int _search_endflag;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    //public List<int> sheet_topendID = new List<int>(); //シートごとに、compNumのIDの頭と最後を、順番に入れている。[0][1]は、シート０のIDの頭、と最後、という感じ。

    public List<GirlLikeSet> contest_set = new List<GirlLikeSet>();

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です。
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        contest_set.Clear();

        excel_contestset_database = Resources.Load("Excel/Entity_ContestSetDataBase") as Entity_ContestSetDataBase;

        sheet_no = 0;

        while (sheet_no < excel_contestset_database.sheets.Count)
        {

            count = 0;
            //sheet_topendID.Add(excel_contestset_database.sheets[sheet_no].list[count].compNum);
            //シート一枚目から順に入れる
            while (count < excel_contestset_database.sheets[sheet_no].list.Count)
            {
                // 一旦代入
                SetExcelDB();

                ++count;


            }
            //sheet_topendID.Add(excel_contestset_database.sheets[sheet_no].list[count-1].compNum);

            ++sheet_no;
        }
       
    }

    void SetExcelDB()
    {
        _id = excel_contestset_database.sheets[sheet_no].list[count].setID;
        _compnum = excel_contestset_database.sheets[sheet_no].list[count].compNum;
        _itemname = excel_contestset_database.sheets[sheet_no].list[count].girllike_itemname;
        _itemsubtype = excel_contestset_database.sheets[sheet_no].list[count].girllike_itemsubtype;

        _set_score = excel_contestset_database.sheets[sheet_no].list[count].set_score;

        _rich = excel_contestset_database.sheets[sheet_no].list[count].rich;
        _sweat = excel_contestset_database.sheets[sheet_no].list[count].sweat;
        _bitter = excel_contestset_database.sheets[sheet_no].list[count].bitter;
        _sour = excel_contestset_database.sheets[sheet_no].list[count].sour;

        _crispy = excel_contestset_database.sheets[sheet_no].list[count].crispy;
        _fluffy = excel_contestset_database.sheets[sheet_no].list[count].fluffy;
        _smooth = excel_contestset_database.sheets[sheet_no].list[count].smooth;
        _hardness = excel_contestset_database.sheets[sheet_no].list[count].hardness;
        _jiggly = excel_contestset_database.sheets[sheet_no].list[count].jiggly;
        _chewy = excel_contestset_database.sheets[sheet_no].list[count].chewy;
        _juice = excel_contestset_database.sheets[sheet_no].list[count].juice;

        _beauty = excel_contestset_database.sheets[sheet_no].list[count].beauty;
        _sp1_wind = excel_contestset_database.sheets[sheet_no].list[count].Sp1_Wind;

        _tp01 = excel_contestset_database.sheets[sheet_no].list[count].topping01;
        _tp02 = excel_contestset_database.sheets[sheet_no].list[count].topping02;
        _tp03 = excel_contestset_database.sheets[sheet_no].list[count].topping03;
        _tp04 = excel_contestset_database.sheets[sheet_no].list[count].topping04;
        _tp05 = excel_contestset_database.sheets[sheet_no].list[count].topping05;
        _tp06 = excel_contestset_database.sheets[sheet_no].list[count].topping06;
        _tp07 = excel_contestset_database.sheets[sheet_no].list[count].topping07;
        _tp08 = excel_contestset_database.sheets[sheet_no].list[count].topping08;
        _tp09 = excel_contestset_database.sheets[sheet_no].list[count].topping09;
        _non_tp_score = excel_contestset_database.sheets[sheet_no].list[count].Non_tpscore;

        _tp_score01 = excel_contestset_database.sheets[sheet_no].list[count].tp_score01;
        _tp_score02 = excel_contestset_database.sheets[sheet_no].list[count].tp_score02;
        _tp_score03 = excel_contestset_database.sheets[sheet_no].list[count].tp_score03;
        _tp_score04 = excel_contestset_database.sheets[sheet_no].list[count].tp_score04;
        _tp_score05 = excel_contestset_database.sheets[sheet_no].list[count].tp_score05;
        _tp_score06 = excel_contestset_database.sheets[sheet_no].list[count].tp_score06;
        _tp_score07 = excel_contestset_database.sheets[sheet_no].list[count].tp_score07;
        _tp_score08 = excel_contestset_database.sheets[sheet_no].list[count].tp_score08;
        _tp_score09 = excel_contestset_database.sheets[sheet_no].list[count].tp_score09;

        _setkansou = excel_contestset_database.sheets[sheet_no].list[count].desc;

        _comment_flag = excel_contestset_database.sheets[sheet_no].list[count].commet_flag;
        _search_endflag = excel_contestset_database.sheets[sheet_no].list[count].search_endflag;

        //ここでリストに追加している
        contest_set.Add(new GirlLikeSet(_id, _compnum, _itemname, _itemsubtype, _set_score,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _juice, _beauty,
            _sp1_wind,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp06, _tp07, _tp08, _tp09,
            _tp_score01, _tp_score02, _tp_score03, _tp_score04, _tp_score05, _tp_score06, _tp_score07, _tp_score08, _tp_score09,
            _non_tp_score, _setkansou, _comment_flag, _search_endflag));

        //Debug.Log("GirlLike_tp01: " + girllikeset[count].girlLike_topping[0]);
        //Debug.Log("id: " + _compnum + " _sp_score1: " + _sp_score1);
    }
}

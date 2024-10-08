using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSetDataBase : SingletonMonoBehaviour<QuestSetDataBase>
{
    private Entity_QuestSetDataBase excel_questset_database; //sample_excelクラス。エクセル読み込み時、XMLインポートで生成されたときのクラスを読む。リスト型と同じように扱える。

    private TimeController time_controller;

    private int _id;
    private int _questID;
    private int _questType;
    private int _questHyouji;
    private int _questHyoujiHeart;
    private int _hightype;

    private string _filename;
    private string _itemname;
    private string _itemname2;
    private string _itemname3;
    private string _itemsubtype;

    private int _kosu_default;
    private int _kosu_min;
    private int _kosu_max;
    private int _buy_price;

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

    private string _tp01;
    private string _tp02;
    private string _tp03;
    private string _tp04;
    private string _tp05;

    private int _tp_score01;
    private int _tp_score02;
    private int _tp_score03;
    private int _tp_score04;
    private int _tp_score05;

    public int _quest_AfterDay;
    public int _quest_LimitMonth;
    public int _quest_LimitDay;
    public int _quest_AreaType; //
    public string _quest_ClientName; //

    private string _title;
    private string _desc;
    private int _read_endflag;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    private int _day;

    //public List<int> sheet_topendID = new List<int>(); //シートごとに、IDの頭と最後を、順番に入れている。[0][1]は、シート０のIDの頭、と最後、という感じ。

    public List<QuestSet> questset = new List<QuestSet>();

    //依頼するときに、ランダムで３つほど選んで表示する用。パラメータもいくつかはランダムでセットされる。
    public List<QuestSet> questRandomset = new List<QuestSet>();

    //受注したクエストのリスト。クエストランダムセットから選んだセットを、コピーして新しく登録している。
    public List<QuestSet> questTakeset = new List<QuestSet>();

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です。
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        questset.Clear();

        excel_questset_database = Resources.Load("Excel/Entity_QuestSetDataBase") as Entity_QuestSetDataBase;

        //sheet_topendID.Add(0); //シートの頭のIDは0。

        sheet_no = 0;
        while (sheet_no < excel_questset_database.sheets.Count)
        {
            count = 0;
            while (count < excel_questset_database.sheets[sheet_no].list.Count)
            {
                SetParam();

                //ここでリストに追加している
                questset.Add(new QuestSet(_id, _questID, _questType, _questHyouji, _questHyoujiHeart, _hightype, _filename, _itemname, _itemname2, _itemname3, 
                    _itemsubtype, _kosu_default, _kosu_min, _kosu_max, _buy_price,
                    _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _juice, _beauty,
                    _tp01, _tp02, _tp03, _tp04, _tp05, _tp_score01, _tp_score02, _tp_score03, _tp_score04, _tp_score05,
                    _quest_AfterDay, _quest_LimitMonth, _quest_LimitDay, _quest_AreaType, _quest_ClientName, _title, _desc, _read_endflag));              

                ++count;
            }

            sheet_no++;
 
        }

        //デバッグ用
        /*for (i = 0; i < questset.Count; i++)
        {
            Debug.Log("QuestID: " + questset[i].Quest_ID);
        }*/
    }

    void SetParam()
    {
        // 一旦代入
        _id = excel_questset_database.sheets[sheet_no].list[count].ID;
        _questID = excel_questset_database.sheets[sheet_no].list[count].QuestID;
        _questType = excel_questset_database.sheets[sheet_no].list[count].QuestType;
        _questHyouji = excel_questset_database.sheets[sheet_no].list[count].QuestHyouji;
        _questHyoujiHeart = excel_questset_database.sheets[sheet_no].list[count].QuestHyoujiHeart;
        _hightype = excel_questset_database.sheets[sheet_no].list[count].HighType;

        _filename = excel_questset_database.sheets[sheet_no].list[count].file_name;
        _itemname = excel_questset_database.sheets[sheet_no].list[count].quest_itemName;
        _itemname2 = excel_questset_database.sheets[sheet_no].list[count].quest_itemName2;
        _itemname3 = excel_questset_database.sheets[sheet_no].list[count].quest_itemName3;
        _itemsubtype = excel_questset_database.sheets[sheet_no].list[count].quest_itemsubtype;

        _kosu_default = excel_questset_database.sheets[sheet_no].list[count].kosu_default;
        _kosu_min = excel_questset_database.sheets[sheet_no].list[count].kosu_min;
        _kosu_max = excel_questset_database.sheets[sheet_no].list[count].kosu_max;
        _buy_price = excel_questset_database.sheets[sheet_no].list[count].buy_price;

        _rich = excel_questset_database.sheets[sheet_no].list[count].rich;
        _sweat = excel_questset_database.sheets[sheet_no].list[count].sweat;
        _bitter = excel_questset_database.sheets[sheet_no].list[count].bitter;
        _sour = excel_questset_database.sheets[sheet_no].list[count].sour;

        _crispy = excel_questset_database.sheets[sheet_no].list[count].crispy;
        _fluffy = excel_questset_database.sheets[sheet_no].list[count].fluffy;
        _smooth = excel_questset_database.sheets[sheet_no].list[count].smooth;
        _hardness = excel_questset_database.sheets[sheet_no].list[count].hardness;
        _jiggly = excel_questset_database.sheets[sheet_no].list[count].jiggly;
        _chewy = excel_questset_database.sheets[sheet_no].list[count].chewy;

        _juice = excel_questset_database.sheets[sheet_no].list[count].juice;
        _beauty = excel_questset_database.sheets[sheet_no].list[count].beauty;

        _tp01 = excel_questset_database.sheets[sheet_no].list[count].topping01;
        _tp02 = excel_questset_database.sheets[sheet_no].list[count].topping02;
        _tp03 = excel_questset_database.sheets[sheet_no].list[count].topping03;
        _tp04 = excel_questset_database.sheets[sheet_no].list[count].topping04;
        _tp05 = excel_questset_database.sheets[sheet_no].list[count].topping05;

        _tp_score01 = excel_questset_database.sheets[sheet_no].list[count].tp_score1;
        _tp_score02 = excel_questset_database.sheets[sheet_no].list[count].tp_score2;
        _tp_score03 = excel_questset_database.sheets[sheet_no].list[count].tp_score3;
        _tp_score04 = excel_questset_database.sheets[sheet_no].list[count].tp_score4;
        _tp_score05 = excel_questset_database.sheets[sheet_no].list[count].tp_score5;

        _quest_AfterDay = excel_questset_database.sheets[sheet_no].list[count].quest_afterday;
        _quest_LimitMonth = excel_questset_database.sheets[sheet_no].list[count].limit_month;
        _quest_LimitDay = excel_questset_database.sheets[sheet_no].list[count].limit_day;
        _quest_AreaType = excel_questset_database.sheets[sheet_no].list[count].area_Type;
        _quest_ClientName = excel_questset_database.sheets[sheet_no].list[count].ClientName;

        _title = excel_questset_database.sheets[sheet_no].list[count].quest_Title;
        _desc = excel_questset_database.sheets[sheet_no].list[count].desc;
        _read_endflag = excel_questset_database.sheets[sheet_no].list[count].read_endflag;
    }

    public void RandomNewSetInit(int count)
    {
        // 一旦代入
        _id = questset[count]._ID;
        _questID = questset[count].Quest_ID;
        _questType = questset[count].QuestType;
        _questHyouji = questset[count].QuestHyouji;
        _questHyoujiHeart = questset[count].QuestHyoujiHeart;
        _hightype = questset[count].HighType;

        _filename = questset[count].Quest_FileName;
        _itemname = questset[count].Quest_itemName;
        _itemname2 = questset[count].Quest_itemName2;
        _itemname3 = questset[count].Quest_itemName3;
        _itemsubtype = questset[count].Quest_itemSubtype;

        _kosu_min = questset[count].Quest_kosu_min;
        _kosu_max = questset[count].Quest_kosu_max;

        _kosu_default = Random.Range(_kosu_min, _kosu_max);
        _buy_price = questset[count].Quest_buy_price + Random.Range(0, 20);

        _rich = questset[count].Quest_rich;
        _sweat = questset[count].Quest_sweat;
        _bitter = questset[count].Quest_bitter;
        _sour = questset[count].Quest_sour;

        _crispy = questset[count].Quest_crispy;
        _fluffy = questset[count].Quest_fluffy;
        _smooth = questset[count].Quest_smooth;
        _hardness = questset[count].Quest_hardness;
        _jiggly = questset[count].Quest_jiggly;
        _chewy = questset[count].Quest_chewy;

        _juice = questset[count].Quest_juice;
        _beauty = questset[count].Quest_beauty;

        _tp01 = questset[count].Quest_topping[0];
        _tp02 = questset[count].Quest_topping[1];
        _tp03 = questset[count].Quest_topping[2];
        _tp04 = questset[count].Quest_topping[3];
        _tp05 = questset[count].Quest_topping[4];

        _tp_score01 = questset[count].Quest_tp_score[0];
        _tp_score02 = questset[count].Quest_tp_score[1];
        _tp_score03 = questset[count].Quest_tp_score[2];
        _tp_score04 = questset[count].Quest_tp_score[3];
        _tp_score05 = questset[count].Quest_tp_score[4];

        //日付に少しランダムで幅をつける
        if(questset[count].Quest_AfterDay <= 10)
        {
            _quest_AfterDay = questset[count].Quest_AfterDay + Random.Range(0, 5);
        }
        else
        {
            _quest_AfterDay = questset[count].Quest_AfterDay + Random.Range(0, 10);
        }      
        if(_quest_AfterDay < 0) { _quest_AfterDay = 1; }

        _quest_LimitMonth = questset[count].Quest_LimitMonth;
        _quest_LimitDay = questset[count].Quest_LimitDay;
        _quest_AreaType = questset[count].Quest_AreaType;
        _quest_ClientName = questset[count].Quest_ClientName;

        _title = questset[count].Quest_Title;
        _desc = questset[count].Quest_desc;
        _read_endflag = questset[count].read_endflag;

        //ここでリストに追加している
        questRandomset.Add(new QuestSet(_id, _questID, _questType, _questHyouji, _questHyoujiHeart, _hightype, _filename, _itemname, _itemname2, _itemname3, 
            _itemsubtype, _kosu_default, _kosu_min, _kosu_max, _buy_price,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _juice, _beauty,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp_score01, _tp_score02, _tp_score03, _tp_score04, _tp_score05,
            _quest_AfterDay, _quest_LimitMonth, _quest_LimitDay, _quest_AreaType, _quest_ClientName, _title, _desc, _read_endflag));
    }


    public void QuestTakeSetInit(int count)
    {
        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        // 一旦代入
        _id = questRandomset[count]._ID;
        _questID = questRandomset[count].Quest_ID;
        _questType = questRandomset[count].QuestType;
        _questHyouji = questRandomset[count].QuestHyouji;
        _questHyoujiHeart = questRandomset[count].QuestHyoujiHeart;
        _hightype = questRandomset[count].HighType;

        _filename = questRandomset[count].Quest_FileName;
        _itemname = questRandomset[count].Quest_itemName;
        _itemname2 = questRandomset[count].Quest_itemName2;
        _itemname3 = questRandomset[count].Quest_itemName3;
        _itemsubtype = questRandomset[count].Quest_itemSubtype;

        _kosu_min = questRandomset[count].Quest_kosu_min;
        _kosu_max = questRandomset[count].Quest_kosu_max;

        _kosu_default = questRandomset[count].Quest_kosu_default;
        _buy_price = questRandomset[count].Quest_buy_price;

        _rich = questRandomset[count].Quest_rich;
        _sweat = questRandomset[count].Quest_sweat;
        _bitter = questRandomset[count].Quest_bitter;
        _sour = questRandomset[count].Quest_sour;

        _crispy = questRandomset[count].Quest_crispy;
        _fluffy = questRandomset[count].Quest_fluffy;
        _smooth = questRandomset[count].Quest_smooth;
        _hardness = questRandomset[count].Quest_hardness;
        _jiggly = questRandomset[count].Quest_jiggly;
        _chewy = questRandomset[count].Quest_chewy;

        _juice = questRandomset[count].Quest_juice;
        _beauty = questRandomset[count].Quest_beauty;

        _tp01 = questRandomset[count].Quest_topping[0];
        _tp02 = questRandomset[count].Quest_topping[1];
        _tp03 = questRandomset[count].Quest_topping[2];
        _tp04 = questRandomset[count].Quest_topping[3];
        _tp05 = questRandomset[count].Quest_topping[4];

        _tp_score01 = questRandomset[count].Quest_tp_score[0];
        _tp_score02 = questRandomset[count].Quest_tp_score[1];
        _tp_score03 = questRandomset[count].Quest_tp_score[2];
        _tp_score04 = questRandomset[count].Quest_tp_score[3];
        _tp_score05 = questRandomset[count].Quest_tp_score[4];

        //クエスト決定したタイミングで、AfterDayの日付と現在の日付をもとに、締め切り日を設定する。
        _quest_AfterDay = questRandomset[count].Quest_AfterDay;
        _day = PlayerStatus.player_day + _quest_AfterDay;

        time_controller.CullenderKeisan(_day); //GameMgr.Cullender_MonthとDayで値が返る
        _quest_LimitMonth = GameMgr.Cullender_Month;
        _quest_LimitDay = GameMgr.Cullender_Day;

        _quest_AreaType = questRandomset[count].Quest_AreaType;
        _quest_ClientName = questRandomset[count].Quest_ClientName;

        _title = questRandomset[count].Quest_Title;
        _desc = questRandomset[count].Quest_desc;
        _read_endflag = questRandomset[count].read_endflag;

        //ここでリストに追加している
        questTakeset.Add(new QuestSet(_id, _questID, _questType, _questHyouji, _questHyoujiHeart, _hightype, _filename, _itemname, _itemname2, _itemname3, 
            _itemsubtype, _kosu_default, _kosu_min, _kosu_max, _buy_price,
            _rich, _sweat, _bitter, _sour, _crispy, _fluffy, _smooth, _hardness, _jiggly, _chewy, _juice, _beauty,
            _tp01, _tp02, _tp03, _tp04, _tp05, _tp_score01, _tp_score02, _tp_score03, _tp_score04, _tp_score05,
            _quest_AfterDay, _quest_LimitMonth, _quest_LimitDay, _quest_AreaType, _quest_ClientName, _title, _desc, _read_endflag));
    }

    public void ResetQuestTakeSet()
    {
        questTakeset.Clear();
    }

    public void ResetSpriteTex(int _count)
    {
        _filename = questTakeset[_count].Quest_FileName;
        questTakeset[_count].ResetSprite(_filename);
    }
}

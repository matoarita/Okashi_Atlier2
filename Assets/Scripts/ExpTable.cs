using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//
//経験値テーブル。ついでに、レベルアップのチェックもここで行っている。
//



public class ExpTable : SingletonMonoBehaviour<ExpTable>
{

    //ハートレベルのテーブル
    public List<int> stage1_hlvTable = new List<int>();
    private List<int> skill_patissier_List = new List<int>();
    private int _hlv_last, _sum;

    public Dictionary<int, int> exp_table;

    private int i, count;

    private int now_level, before_level, before_start_lv;
    private int _temp_lv;
    private int _dev;

    private GameObject canvas;
    private GameObject text_area;
    private Text _text;


    private List<string> _temp_skill = new List<string>();

    //SEを鳴らす
    private SoundController sc;

    private GirlEat_Judge girlEat_judge;

    IEnumerator routine;

    private int _mstatus;


    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this.gameObject);

        InitSetup();

        //好感度レベルのテーブル初期化
        Init_Stage1_LVTable();
        Init_SkillTable();

        skill_patissier_List.Clear();
    }

    private void InitSetup()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        girlEat_judge = GirlEat_Judge.Instance.GetComponent<GirlEat_Judge>();
    }

    // Update is called once per frame
    void Update () {
	
        if(canvas == null)
        {
            InitSetup();
        }
	}

    //ハートレベルに応じてスキルを覚えるパターン Girl_Eat_Judgeかデバッグパネルから読む。
    public void SkillCheckHeartLV(int _nowlevel, int _status)
    {
        //_status = 0 実際に仕上げ回数を増やす　1は、パネルの表示のみ
        _mstatus = _status;
        //レベルがあがるごとに、アイテム発見力があがる。
        /*PlayerStatus.player_girl_findpower = 100 + ((girl1_Love_lv-1) * 10);

        //上限処理
        if(PlayerStatus.player_girl_findpower >= 999)
        {
            PlayerStatus.player_girl_findpower = 999;
        }*/

        if (_mstatus == 0)
        {
            ShiageUpCheck(_nowlevel);
        }
        else if (_mstatus == 1)
        {
            switch (_nowlevel)
            {
                case 2:
          
                    break;

                case 3:

                    ShiageUpPanelHyouji();
                    break;

                case 5:

                    //_temp_skill.Add("一度に　2個　トッピングできるようになった！");
                    /*GameMgr.topping_Set_Count = 2;

                    if (_mstatus == 1) //GirlEatJudgeから読んだ場合、パネルを生成する
                    {
                        girlEat_judge.LvUpPanel2();
                    }*/
                    break;

                case 6:

                    break;

                case 9:

                    ShiageUpPanelHyouji();
                    break;

                case 18:

                    ShiageUpPanelHyouji();
                    break;
            }
        }
            
    }

    void ShiageUpCheck(int _lv)
    {

        //こっちが、実際に仕上げ回数を更新する。ハート上がったタイミングで更新のやり方だと、
        //上がり途中で別シーンとかに移動する可能性があり、その場合仕上げ回数が増えないことになる。ので、それの回避。

        //仕上げできる回数が上がる。
        if (_lv < 3)
        {
            PlayerStatus.player_extreme_kaisu_Max = 1;
        }
        else if (_lv >= 3 && _lv < 9)
        {
            PlayerStatus.player_extreme_kaisu_Max = 2;
        }
        else if (_lv >= 9 && _lv < 18)
        {
            PlayerStatus.player_extreme_kaisu_Max = 3;
        }
        else if (_lv >= 18)
        {
            PlayerStatus.player_extreme_kaisu_Max = 4;
        }

    }

    void ShiageUpPanelHyouji()
    {
        girlEat_judge.LvUpPanel1(1);
    }


    //パティシエレベルに応じてスキルを覚えるパターン Girl_Eat_Judgeかデバッグパネルから読む。
    public void SkillCheckPatissierLV(int _nowlevel, int _status)
    {
        _mstatus = _status;

        if (PlayerStatus.girl1_Love_lv > PlayerStatus.player_patissier_lv)
        {
            _dev = PlayerStatus.girl1_Love_lv - PlayerStatus.player_patissier_lv;
            PlayerStatus.player_patissier_lv += _dev; //ハートLVが、現在パティシエレベルより上回ると、パティシエレベルも同時に上がる。また下がることはない。
            PlayerStatus.player_patissier_job_pt += _dev;
        }

    }

    


    //ハートレベルアップテーブル(パティシエレベルと現在共通）
    void Init_Stage1_LVTable()
    {
        stage1_hlvTable.Clear();
        stage1_hlvTable.Add(15); //LV2。LV1で、次のレベルが上がるまでの好感度値
        stage1_hlvTable.Add(60);　//LV3 LV1の分は含めない。
        stage1_hlvTable.Add(120); //LV4
        stage1_hlvTable.Add(200); //LV5
        stage1_hlvTable.Add(300); //LV6
        stage1_hlvTable.Add(410); //LV7
        stage1_hlvTable.Add(530); //LV8
        stage1_hlvTable.Add(650); //LV9
        stage1_hlvTable.Add(780); //LV10
        stage1_hlvTable.Add(920); //LV11
        stage1_hlvTable.Add(1050); //LV12
        stage1_hlvTable.Add(1200); //LV13
        stage1_hlvTable.Add(1350); //LV14
        stage1_hlvTable.Add(1500); //LV15

        _hlv_last = stage1_hlvTable.Count;
        //LV16以上～99まで　ハートレベル*100ごとに上がるように設定
        for (i = 1; i < (99 - _hlv_last); i++)
        {
            stage1_hlvTable.Add((_hlv_last + i) * 100);
        }
        stage1_hlvTable[stage1_hlvTable.Count - 1] = 9999; //最後だけ9999

        //デバッグ用
        /*for (i = 0; i < stage1_lvTable.Count; i++)
        {
            Debug.Log("stage1_levelTable: " + "次のLv" + (i+2) + " " + stage1_lvTable[i]);
        }
        Debug.Log("stage1_lvTable.Count: " + stage1_lvTable.Count);*/
    }

    void Init_SkillTable()
    {
        i = 1;
        while(i<99) //LV３ごとにジョブが1上がる
        {
            if(i % 3 == 0)
            {
                skill_patissier_List.Add(0);
            }
            i++;
        }
    }

    //更新後のHeartExpをいれると、現在のHLVに再計算する
    public void HeartLVKoushin()
    {
        i = 0;
        PlayerStatus.girl1_Love_lv = 1;
        while (PlayerStatus.girl1_Love_exp >= stage1_hlvTable[i])
        {
            //_girllove_param -= stage_levelTable[i];
            PlayerStatus.girl1_Love_lv++;
            i++;
        }

        //
    }

    //レベルをいれると、それまでに必要な経験値の合計を返すメソッド レベルは１始まり
    public int SumLvTable(int _count)
    {
        _sum = 0;

        for (i = 0; i < _count - 1; i++)
        {
            _sum += stage1_hlvTable[i];
        }

        return _sum;
    }
}

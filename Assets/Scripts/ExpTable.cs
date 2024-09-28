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
    private List<int> stage1_joblvTable = new List<int>();
    private int _hlv_last, _joblv_last, _sum;
    private int _last_htable;

    public Dictionary<int, int> exp_table;

    private int i, count;

    private int now_level, before_lv;
    private int _lv;
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
        Init_Stage1_heartLVTable();
        Init_JobTable();
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
            SkillLVCheck(_nowlevel);
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

                    
                    break;

                case 6:

                    break;

                case 13: //ヒカリのおかし作り解禁

                    break;

                case 15:

                    ShiageUpPanelHyouji();
                    break;

                case 20: //二種類～同時トッピングできるようになる。

                    girlEat_judge.LvUpPanel3();
                    break;

                case 25: //複数個同時にのせられるようになる。パネル表記はなしで、ハートイベントなどで知らせる。

                    break;

                case 30:

                    ShiageUpPanelHyouji();
                    break;
            }
        }

        SkillCheckPatissierLV();
    }

    void SkillLVCheck(int _lv)
    {

        //こっちが、実際に仕上げ回数を更新する。ハート上がったタイミングで更新のやり方だと、
        //上がり途中で別シーンとかに移動する可能性があり、その場合仕上げ回数が増えないことになる。ので、それの回避。

        //仕上げできる回数が上がる。
        if (_lv < 3)
        {
            PlayerStatus.player_extreme_kaisu_Max = 1;
        }
        else if (_lv >= 3 && _lv < 15)
        {
            PlayerStatus.player_extreme_kaisu_Max = 2;
        }
        else if (_lv >= 15 && _lv < 30)
        {
            PlayerStatus.player_extreme_kaisu_Max = 3;
        }
        else if (_lv >= 30)
        {
            PlayerStatus.player_extreme_kaisu_Max = 4;
        }
        

        //ヒカリお菓子作り覚える
        if (_lv < GameMgr.System_HeartLVevent_01)
        {
            GameMgr.System_HikariMakeUse_Flag = false; //
        }
        else if (_lv >= GameMgr.System_HeartLVevent_01)
        {
            GameMgr.System_HikariMakeUse_Flag = true;
        }

        //二種類～同時トッピング
        if (_lv < 20)
        {
            GameMgr.topping_Set_Count = 1; //デフォルト
        }
        else if(_lv >= 20)
        {
            //_temp_skill.Add("一度に　2個　トッピングできるようになった！");
            GameMgr.topping_Set_Count = 2;
        }

        //複数個まとめて数のせる
        if (_lv < 25)
        {
            GameMgr.System_Topping_Multiple_Flag = false;
        }
        else if (_lv >= 25)
        {
            GameMgr.System_Topping_Multiple_Flag = true;
        }
    }

    void ShiageUpPanelHyouji()
    {
        girlEat_judge.LvUpPanel2(1);
    }


    //ジョブレベルのチェック　ジョブがあがったらジョブポイントがたまる
    public void SkillCheckPatissierLV()
    {
        //ハートレベルに連動してレベル上がるパターン
        if (PlayerStatus.girl1_Love_maxlv > PlayerStatus.player_patissier_lv)
        {
            _dev = PlayerStatus.girl1_Love_maxlv - PlayerStatus.player_patissier_lv;           
            PlayerStatus.player_patissier_job_pt += _dev;
            PlayerStatus.player_patissier_lv = PlayerStatus.girl1_Love_maxlv; //ハートLVが、現在パティシエレベルより上回ると、パティシエレベルも同時に上がる。また下がることはない。
        } else //例外処理
        {
            PlayerStatus.player_patissier_lv = PlayerStatus.girl1_Love_maxlv;
        }

        //ジョブ経験値に合わせてレベル上がるパターン
        /*
        before_lv = PlayerStatus.player_patissier_lv;
        JobLVKoushin();

        if(PlayerStatus.player_patissier_lv > before_lv)
        {
            _dev = PlayerStatus.player_patissier_lv - before_lv;
            PlayerStatus.player_patissier_job_pt += _dev;            
        }*/
    }

    


    //ハートレベルアップテーブル(パティシエレベルと現在共通）
    void Init_Stage1_heartLVTable()
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

        //LV16以上～99まで　ハートレベル*100ごとに上がるように設定
        _hlv_last = stage1_hlvTable.Count; //上にいれたとこまでの最後　この場合14が入る     
        _last_htable = stage1_hlvTable[stage1_hlvTable.Count - 1]; //最後にいれた数字　1500が入っている
        for (i = 1; i < (30 - _hlv_last); i++)
        {
            stage1_hlvTable.Add(_last_htable + i * 100); //14+i=15～から入っていく
        }

        //LV30以上～99まで　ハートレベル*100ごとに上がるように設定
        _hlv_last = stage1_hlvTable.Count; //上にいれたとこまでの最後
        _last_htable = stage1_hlvTable[stage1_hlvTable.Count - 1]; //最後にいれた数字　更新
        for (i = 1; i < (99 - _hlv_last); i++)
        {
            stage1_hlvTable.Add(_last_htable + i * 150); //30+i=31～から入っていく
        }

        //LV99ラストにいくための経験値
        stage1_hlvTable[stage1_hlvTable.Count - 1] = 15000; //最後の数字 LV98→LV99までが、ここで設定した値になる。

        //デバッグ用
        /*for (i = 0; i < stage1_hlvTable.Count; i++)
        {
            Debug.Log("stage1_hlvTable: " + "次のLv" + (i+2) + " " + stage1_hlvTable[i]);
        }
        Debug.Log("stage1_hlvTable.Count: " + stage1_hlvTable.Count);*/
    }

    //ジョブのレベルアップテーブル
    void Init_JobTable()
    {
        stage1_joblvTable.Clear();
        stage1_joblvTable.Add(5); //LV2。LV1で、次のレベルが上がるまでの好感度値
        stage1_joblvTable.Add(10);　//LV3 LV1の分は含めない。
        stage1_joblvTable.Add(20); //LV4
        stage1_joblvTable.Add(30); //LV5
        stage1_joblvTable.Add(40); //LV6
        stage1_joblvTable.Add(55); //LV7
        stage1_joblvTable.Add(75); //LV8
        stage1_joblvTable.Add(95); //LV9
        stage1_joblvTable.Add(125); //LV10
        stage1_joblvTable.Add(155); //LV11
        stage1_joblvTable.Add(185); //LV12
        stage1_joblvTable.Add(215); //LV13
        stage1_joblvTable.Add(250); //LV14
        stage1_joblvTable.Add(300); //LV15

        _joblv_last = stage1_joblvTable.Count;
        //LV16以上～50まで　100ごとに上がるように設定
        for (i = 1; i < (50 - _joblv_last); i++)
        {
            stage1_joblvTable.Add((_joblv_last + i) * 100);
        }
        stage1_joblvTable[stage1_joblvTable.Count - 1] = 9999; //最後だけ9999

        //デバッグ用
        /*for (i = 0; i < stage1_joblvTable.Count; i++)
        {
            Debug.Log("stage1_joblvTable: " + "次のLv" + (i+2) + " " + stage1_joblvTable[i]);
        }
        Debug.Log("stage1_joblvTable.Count: " + stage1_joblvTable.Count);*/
    }

    //更新後のrenkinExpをいれると、現在のジョブLVに再計算する
    public void JobLVKoushin()
    {
        i = 0;
        PlayerStatus.player_patissier_lv = 1;
        while (PlayerStatus.player_renkin_exp >= stage1_joblvTable[i])
        {
            //_girllove_param -= stage_levelTable[i];
            PlayerStatus.player_patissier_lv++;
            i++;
        }

        //
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
        if (PlayerStatus.girl1_Love_maxlv <= PlayerStatus.girl1_Love_lv) //maxlvの上限更新
        {
            PlayerStatus.girl1_Love_maxlv = PlayerStatus.girl1_Love_lv;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//
//経験値テーブル。ついでに、レベルアップのチェックもここで行っている。
//



public class ExpTable : SingletonMonoBehaviour<ExpTable>
{
    private ItemDataBase database;
    private ItemSubTypeSetDatabase itemsubtypeset_database;
    private MagicSkillListDataBase magicskill_database;
    private CatDataBase catDataBase;

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
    private string _namehyouji;
    private int _starlv;

    private GameObject canvas;
    private GameObject text_area;
    private Text _text;
    private string _subtype;
    private int random, random2;

    private List<string> _temp_skill = new List<string>();

    private Dictionary<int, int> CatExpTable = new Dictionary<int, int>();

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

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //アイテムサブタイプの表記を分けるデータベース
        itemsubtypeset_database = ItemSubTypeSetDatabase.Instance.GetComponent<ItemSubTypeSetDatabase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //ねこ経験値テーブル設定
        InitCatExpTable_library();
    }

    // Update is called once per frame
    void Update () {
	
        if(canvas == null)
        {
            InitSetup();
        }
	}

    //ハートレベルに応じてスキルを覚えるパターン Girl_Eat_Judgeかデバッグパネルから読む。
    public void SkillCheckHeartLV(int _maxlevel, int _status)
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
            SkillLVCheck(_maxlevel);
        }
        else if (_mstatus == 1)
        {
            switch (_maxlevel)
            {
                case 2:
          
                    break;

                case 3:

                    ShiageUpPanelHyouji();
                    break;

                case 5:
                    
                    break;

                case 6:

                    //MagicLearnPanelHyouji("Cookie_SecondBake");  
                    break;

                case 7:

                    //MagicLearnPanelHyouji("Heart_of_Icecream"); //下の欄の「魔法をおぼえる」のほうも更新すること
                    MagicLearnPanelHyouji("Freezing_Spell");
                    break;

                case 8:
                    
                    break;

                case 9: //ヒカリのおかし作り解禁

                    break;

                case 10:

                    //MagicLearnPanelHyouji("Bake_Beans");
                    //MagicLearnPanelHyouji("Chocolate_Tempering");
                    ShiageUpPanelHyouji();
                    break;

                case 11:

                    //MagicLearnPanelHyouji("SugerPot");
                    break;

                case 12:

                    //MagicLearnPanelHyouji("Buttelfy_illumination");
                    break;

                case 13:

                    //MagicLearnPanelHyouji("Bubble_Mist");
                    break;

                case 14:

                    //MagicLearnPanelHyouji("Wind_Crown");
                    //MagicLearnPanelHyouji("Wind_Pen");
                    break;

                case 15:

                    //ShiageUpPanelHyouji();
                    break;

                case 17:

                    MagicUpPanelHyouji(1);
                    break;

                case 18:

                    //MagicLearnPanelHyouji("Star_Blessing");
                    break;

                case 19:

                    //MagicLearnPanelHyouji("Latte_Art");
                    break;

                case 20:

                    MagicLearnPanelHyouji("Warming_Handmade"); //下の欄の「魔法をおぼえる」のほうも更新すること
                    //girlEat_judge.LvUpPanel3(); //二種類～同時トッピングできるようになる。
                    ShiageUpPanelHyouji();
                    break;

                case 21:

                    //MagicLearnPanelHyouji("Magic_Soda");
                    break;

                case 25: //おかしの個数が一個増える　神スキル

                    OkashiKosuAddPanelHyouji();
                    break;

                case 26:

                    break;

                case 28:

                    //MagicLearnPanelHyouji("Warming_Handmade");
                    break;

                case 30:
                    
                    break;

                case 31:

                    //MagicLearnPanelHyouji("Statue_of_Bear");
                    break;

                case 35:

                    //MagicLearnPanelHyouji("Moonlight_Banana");
                    break;

                case 50:

                    MagicLearnPanelHyouji("Rainbow_Rain");
                    ShiageUpPanelHyouji();
                    break;

                case 75:

                    ShiageUpPanelHyouji();
                    break;

                case 90:

                    ShiageUpPanelHyouji();
                    break;
            }

            if (_maxlevel > 3 && GameMgr.System_MagicUse_Flag) //レベルが4以上から、LV2ごとにMPが+1
            {
                if (_maxlevel % 2 == 0)
                {
                    MagicUpPanelHyouji(1);
                }
            }
        }        

        //スキルのチェック
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
        else if (_lv >= 3 && _lv < 10)
        {
            PlayerStatus.player_extreme_kaisu_Max = 2;
        }
        else if (_lv >= 10 && _lv < 20)
        {
            PlayerStatus.player_extreme_kaisu_Max = 3;
        }
        else if (_lv >= 20 && _lv < 50)
        {
            PlayerStatus.player_extreme_kaisu_Max = 4;
        }
        else if (_lv >= 50 && _lv < 75)
        {
            PlayerStatus.player_extreme_kaisu_Max = 5;
        }
        else if (_lv >= 75 && _lv < 90)
        {
            PlayerStatus.player_extreme_kaisu_Max = 6;
        }
        else if (_lv >= 90)
        {
            PlayerStatus.player_extreme_kaisu_Max = 7;
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
        /*if (_lv < 20)
        {
            GameMgr.topping_Set_Count = 1; //デフォルト
        }
        else if(_lv >= 20)
        {
            //_temp_skill.Add("一度に　2個　トッピングできるようになった！");
            GameMgr.topping_Set_Count = 2;
        }*/

        //複数個まとめて数のせる
        /*
        if (_lv < 25)
        {
            GameMgr.System_Topping_Multiple_Flag = false;
        }
        else if (_lv >= 25)
        {
            GameMgr.System_Topping_Multiple_Flag = true;
            GameMgr.System_Topping_Multiple_Max = 2;

            if (_lv >= 35)
            {
                GameMgr.System_Topping_Multiple_Max = 3;
            }
        }*/

        //おかし個数+1
        if (_lv < 25)
        {
            PlayerStatus.player_okashi_kosuup_max = 0;
        }
        else if (_lv >= 25)
        {
            PlayerStatus.player_okashi_kosuup_max = 1;
        }

        //ねこくるようになる
        if (_lv < 15)
        {
            GameMgr.System_CatGetMat_Flag = false;
        }
        else if (_lv >= 15)
        {
            GameMgr.System_CatGetMat_Flag = true;
        }

        //魔法をおぼえる
        if (GameMgr.System_MagicUse_Flag)
        {
            if (_lv >= 7)
            {
                //Magic_Learn("Heart_of_Icecream");
                Magic_Learn("Freezing_Spell");
            }

            if (_lv >= 20)
            {
                Magic_Learn("Warming_Handmade");
            }

            if (_lv >= 50)
            {
                Magic_Learn("Rainbow_Rain");
            }
            /*if (_lv >= 35)
            {
                Magic_Learn("Moonlight_Banana");
            }*/
        }
    }

    //スターによって解放されるパラメータがある場合　スターパネルでも記述
    public void StarLVCheck()
    {
        _starlv = PlayerStatus.player_ninki_param;
        if (_starlv >= 15)
        {
            GameMgr.topping_Set_Count = 2;
        }
    }

    void Magic_Learn(string _magicname)
    {
        if (magicskill_database.skillName_SearchLearnLevel(_magicname) < 1)
        {
            magicskill_database.skillHyoujiKaikin(_magicname);
            magicskill_database.skillLearnLv_Name(_magicname, 1);
        }
    }

    void ShiageUpPanelHyouji()
    {
        girlEat_judge.LvUpPanel2(1);
    }

    void OkashiKosuAddPanelHyouji()
    {
        girlEat_judge.LvUpPanel7(1);
    }

    void MagicLearnPanelHyouji(string _magicname)
    {
        _namehyouji = magicskill_database.magicskill_lists[magicskill_database.SearchSkillString(_magicname)].skillNameHyouji;        
        girlEat_judge.LvUpPanel6(_namehyouji);
    }

    void MagicUpPanelHyouji(int _mp)
    {
        PlayerStatus.player_maxmp += _mp;
        girlEat_judge.LvUpPanel4(_mp);
    }

    //ハートLVアップ時にステータス上がる
    public void StatusUp()
    {
        if (GameMgr.System_HeartLV_StatusUp)
        {
            _subtype = database.items[GameMgr.Okashi_lastID].itemType_sub.ToString();
            itemsubtypeset_database.SetImageSub(_subtype); //さっき食べたおかしのサブタイプをみる

            //①直前に食べたおかしの種類によって、上がるパラメータが決まる
            random = Random.Range(1, 10);
            switch (GameMgr.Item_ShokukanTypeNum)
            {
                case 0: //さくさく

                    girlEat_judge.LvUpPanel5("さくさく", random);
                    break;

                case 1: //ふわふわ

                    girlEat_judge.LvUpPanel5("ふわふわ", random);
                    break;

                case 2: //なめらか

                    girlEat_judge.LvUpPanel5("なめらか", random);
                    break;

                case 3: //歯ごたえ

                    girlEat_judge.LvUpPanel5("歯ごたえ", random);
                    break;

                case 4: //のどごし

                    girlEat_judge.LvUpPanel5("のどごし", random);
                    break;

                case 5: //香り

                    girlEat_judge.LvUpPanel5("香り", random);
                    break;
            }

            //②ランダムで、おかし成功率か時間短縮が上がる。
            //魔法のおかしだと、魔法の効果、成功率が上がる。

            //おかしの成功率系判定
            random = Random.Range(0, 10);
            if (random <= 7) //70%ぐらい？
            {
                random2 = Random.Range(1, 3);
                if (database.items[GameMgr.Okashi_lastID].Magic == 0)
                {
                    PlayerStatus.player_okashi_kakuritsuup += random2;
                    girlEat_judge.LvUpPanel5(GameMgr.System_PStatusName1, random2);
                }
                else
                {
                    //魔法のおかしの場合
                    PlayerStatus.player_okashi_magic_kakuritsuup += random2;
                    girlEat_judge.LvUpPanel5(GameMgr.System_PStatusName3, random2);
                }
            }
        }
    }


    //ジョブレベルのチェック　ジョブがあがったらジョブポイントがたまる Exp_Controllerからもよみだし
    public void SkillCheckPatissierLV()
    {
        if (!GameMgr.System_JobLVUP_ON)
        {
            if (PlayerStatus.player_patissier_lv < GameMgr.System_patissier_maxlv) //パティシエLV上限よりも下の場合のみ
            {
                //ハートレベルに連動してレベル上がるパターン
                if (PlayerStatus.girl1_Love_maxlv > PlayerStatus.player_patissier_lv)
                {
                    if (PlayerStatus.girl1_Love_maxlv >= GameMgr.System_patissier_maxlv) //マックスレベルと同じか超えそうになった場合
                    {
                        _dev = GameMgr.System_patissier_maxlv - PlayerStatus.player_patissier_lv;
                        PlayerStatus.player_patissier_job_pt += _dev;
                        PlayerStatus.player_patissier_lv = GameMgr.System_patissier_maxlv;
                    }
                    else
                    {
                        _dev = PlayerStatus.girl1_Love_maxlv - PlayerStatus.player_patissier_lv;
                        PlayerStatus.player_patissier_job_pt += _dev;
                        PlayerStatus.player_patissier_lv = PlayerStatus.girl1_Love_maxlv; //ハートLVが、現在パティシエレベルより上回ると、パティシエレベルも同時に上がる。また下がることはない。

                    }
                }
                else //例外処理
                {
                    PlayerStatus.player_patissier_lv = PlayerStatus.girl1_Love_maxlv;
                }
            }
        }
        else
        {
            //ジョブ経験値に合わせてレベル上がるパターン
            
            before_lv = PlayerStatus.player_patissier_lv;
            JobLVKoushin();

            if(PlayerStatus.player_patissier_lv > before_lv)
            {
                _dev = PlayerStatus.player_patissier_lv - before_lv;
                PlayerStatus.player_patissier_job_pt += _dev;            
            }
        }

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
            stage1_hlvTable.Add(_last_htable + i * 100); //30+i=31～から入っていく 
        }

        //LV99ラストにいくための経験値
        stage1_hlvTable[stage1_hlvTable.Count - 1] = 9999; //最後の数字 LV98→LV99までが、ここで設定した値になる。

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
        stage1_joblvTable.Add(10); //LV2。LV1で、次のレベルが上がるまでの好感度値
        stage1_joblvTable.Add(30);　//LV3 LV1の分は含めない。
        stage1_joblvTable.Add(60); //LV4
        stage1_joblvTable.Add(90); //LV5
        stage1_joblvTable.Add(130); //LV6
        stage1_joblvTable.Add(175); //LV7
        stage1_joblvTable.Add(215); //LV8
        stage1_joblvTable.Add(255); //LV9
        stage1_joblvTable.Add(325); //LV10
        stage1_joblvTable.Add(400); //LV11
        stage1_joblvTable.Add(480); //LV12
        stage1_joblvTable.Add(560); //LV13
        stage1_joblvTable.Add(660); //LV14
        stage1_joblvTable.Add(800); //LV15

        _joblv_last = stage1_joblvTable.Count;
        //LV16以上～50まで　100ごとに上がるように設定
        for (i = 1; i < (50 - _joblv_last); i++)
        {
            stage1_joblvTable.Add(stage1_joblvTable[stage1_joblvTable.Count-1] + 200 + (i*10));
        }
        stage1_joblvTable[stage1_joblvTable.Count - 1] = 15000; //最後だけ15000

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
        now_level = 1;
        while (i < stage1_joblvTable.Count)
        {
            if(PlayerStatus.player_renkin_exp >= stage1_joblvTable[i])
            {
                now_level++;
                i++;
            }
            else
            {
                break;
            }            
        }

        PlayerStatus.player_patissier_lv = now_level;

        //Debug.Log("現在のパティシエLVと経験値: " + PlayerStatus.player_patissier_lv + " " + PlayerStatus.player_renkin_exp);
    }

    //更新後のHeartExpをいれると、現在のHLVに再計算する　Girleat_judgeから読み出し
    public void HeartLVKoushin()
    {
        now_level = PlayerStatus.girl1_Love_lv; //MaxLVではなく、一時的にMaxより下がってる可能性があるので、それを考慮してこんな入れ方に。

        //**再計算**//
        i = 0;
        PlayerStatus.girl1_Love_lv = 1;
        while (PlayerStatus.girl1_Love_exp >= stage1_hlvTable[i])
        {
            //_girllove_param -= stage_levelTable[i];
            PlayerStatus.girl1_Love_lv++;
            i++;
        }
        //**  **//

        if (now_level < PlayerStatus.girl1_Love_lv)
        {
            //レベルアップ時のパネルも表示
            girlEat_judge.LvUpPanel1();
        }

        //スキルチェックは、MaxLVを更新したときだけ
        if (PlayerStatus.girl1_Love_maxlv < PlayerStatus.girl1_Love_lv) //maxlvの上限更新
        {
            PlayerStatus.girl1_Love_maxlv = PlayerStatus.girl1_Love_lv;

            //ステータスもチェック
            
            //覚えるスキルなどがないかチェック。あった場合、それもパネルに表示
            SkillCheckHeartLV(PlayerStatus.girl1_Love_maxlv, 1); //2番目が1だと、パネルの表示
            SkillCheckHeartLV(PlayerStatus.girl1_Love_maxlv, 0); //2番目が0で、実際のスキルの更新

            //ステータスもランダムであがる。
            StatusUp(); //

            //好感度によって発生するサブイベントがないかチェック
            GameMgr.check_GirlLoveSubEvent_flag = false;

            //お菓子以外で、条件を満たしていないかクエストクリアチェック
            girlEat_judge.ExtraSPQuestClearCheck();
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

    //ねこのレベルアップ処理
    public void CatLvUp_Check(int _catid)
    {
        if (catDataBase.catdata_list[_catid].catLv >= 20) //LV20が上限
        { }
        else
        {
            if (catDataBase.catdata_list[_catid].catExp >= CatExpTable[catDataBase.catdata_list[_catid].catLv]) //LVUP簡易 200つまり20回探索したらLV1上がる
            {

                catDataBase.catdata_list[_catid].catExp = 0;

                catDataBase.catdata_list[_catid].catLv++;

                catDataBase.catdata_list[_catid].catTansaku_DefaultSpeed -= 20; //20分早くなる
                if (catDataBase.catdata_list[_catid].catTansaku_DefaultSpeed <= 20) //下限20
                {
                    catDataBase.catdata_list[_catid].catTansaku_DefaultSpeed = 20;
                }

                if (catDataBase.catdata_list[_catid].catLv % 3 == 0) //LV3ごと
                {
                    catDataBase.catdata_list[_catid].catTansaku_Kaisu++;

                    if (catDataBase.catdata_list[_catid].catTansaku_Kaisu >= 9) //9回探索が上限
                    {
                        catDataBase.catdata_list[_catid].catTansaku_Kaisu = 9;
                    }
                }

                GameMgr.CatStartPanel_HyoujiKoushinFlag = true;
            }
        }
    }

    void InitCatExpTable_library()
    {
        CatExpTable.Clear();
        CatExpTable.Add(1, 50);
        CatExpTable.Add(2, 70);
        CatExpTable.Add(3, 100);
        CatExpTable.Add(4, 150);
        CatExpTable.Add(5, 250);
        CatExpTable.Add(6, 300);
        CatExpTable.Add(7, 400);
        CatExpTable.Add(8, 500);
        CatExpTable.Add(9, 600);
        CatExpTable.Add(10, 750);
        CatExpTable.Add(11, 850);
        CatExpTable.Add(12, 950);
        CatExpTable.Add(13, 1000);
        CatExpTable.Add(14, 1100);
        CatExpTable.Add(15, 1200);
        CatExpTable.Add(16, 1300);
        CatExpTable.Add(17, 1500);
        CatExpTable.Add(18, 1700);
        CatExpTable.Add(19, 2000);
        CatExpTable.Add(20, 9999);
    }
}

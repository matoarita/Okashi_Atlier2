using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//
//経験値テーブル。ついでに、レベルアップのチェックもここで行っている。
//



public class ExpTable : SingletonMonoBehaviour<ExpTable>
{
    
    //経験値テーブル
    [SerializeField]
    public int[] exp_table_setting = new int[30];

    public Dictionary<int, int> exp_table;

    private int i, count;

    private int now_level, before_level, before_start_lv;
    private int _temp_lv;

    private GameObject canvas;
    private GameObject text_area;
    private Text _text;

    private List<string> _temp_skill = new List<string>();

    //SEを鳴らす
    private SoundController sc;

    private GirlEat_Judge girlEat_judge;

    IEnumerator routine;

    public bool check_on;
    private bool check_Lclick;

    private int _mstatus;


    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this.gameObject);

        SetInit_ExpTable();
        InitSetup();
    }

    private void InitSetup()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        girlEat_judge = GirlEat_Judge.Instance.GetComponent<GirlEat_Judge>();

        check_on = false;
        check_Lclick = false;
    }

    // Update is called once per frame
    void Update () {
		
        if ( check_on == true )
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                check_Lclick = true;
            }
        }

        if(canvas == null)
        {
            InitSetup();
        }
	}

    
    public void Check_LevelUp()
    {
        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        if(!check_on)
        {
            before_start_lv = PlayerStatus.player_renkin_lv; //現在のレベル（レベルアップ前）
        }

        check_on = true; //チェック中

        for (i = 1; i < exp_table_setting.Length - 1; i++)
        {
            //Console.WriteLine("[{0}:{1}]", table.Key, table.Value);
            if (PlayerStatus.player_renkin_exp >= exp_table[i] && PlayerStatus.player_renkin_exp < exp_table[i+1]) //ex: 0~15なら、LV1。15~45ならLV2、といった具合。
            {
                now_level = i;
            }
        }

        before_level = PlayerStatus.player_renkin_lv; //現在のレベル（レベルアップ更新中）

        //現在のレベルより、ナウレベルのほうが高い場合は、レベルアップ
        if (now_level > before_level)
        {

            //音を鳴らす。音終わりに次の処理。
            sc.PlaySe(13);

            routine = WaitTime();
            StartCoroutine("WaitTime");
            StartCoroutine("Skip_LevelUp");


            /*音終わりに次の処理をする書き方。メモ。
            //音を鳴らす
            audioSource.Play();

            StartCoroutine(Checking(() => {

                Debug.Log("レベルが上がった！！");                 

                //１ずつあげて、その時に覚えるスキルなどがあれば、それをチェックする。

                PlayerStatus.player_renkin_lv++;

                //○○を覚えた！など

                //最後にテキスト表示
                _text.text = "レベルが上がった！" + "\n" + "錬金レベルが" + PlayerStatus.player_renkin_lv + "になった！";
            }));
            */


        }
        else //全て上がりきったら処理を抜ける。それまで、他の操作は出来なくする。
        {
            check_Lclick = false;
            check_on = false;
            StopCoroutine("Skip_LevelUp");
        }
    }

    /*
    public delegate void functionType();
    private IEnumerator Checking(functionType callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying)
            {
                callback();
                break;
            }
        }
    }*/

    IEnumerator WaitTime()
    {
        //3秒待つ
        yield return new WaitForSeconds(3);

        //左クリックが押されたら、強制終了
        if (check_Lclick == true)
        {
            check_Lclick = false;
            check_on = false;

            routine = null;
            yield break;
        }


        //レベルが上がった以降の処理を書く。
        Debug.Log("レベルが上がった！！");
        
        //１ずつあげて、その時に覚えるスキルなどがあれば、それをチェックする。
        PlayerStatus.player_renkin_lv++;

        //○○を覚えた！など
        _temp_skill.Clear();
        SkillCheck(PlayerStatus.player_renkin_lv);

        //最後にテキスト表示
        TextHyouji();

        Check_LevelUp(); //もう一回繰り返し
    }

    IEnumerator Skip_LevelUp()
    {
        while (routine != null)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        PlayerStatus.player_renkin_lv = now_level;

        //○○を覚えた！など
        //前のレベルから現在レベルまでの間に、スキルがないかチェックする。
        _temp_skill.Clear();
        _temp_lv = before_start_lv;
        for (count=0; count <  (now_level-before_start_lv); count++ )
        {          
            SkillCheck(_temp_lv+count+1);
        }

        //最後にテキスト表示
        TextHyouji();
    }

    void TextHyouji()
    {
        if (_temp_skill.Count > 0)
        {
            _text.text = "レベルが上がった！" + "\n" + "パティシエレベルが " + GameMgr.ColorYellow + PlayerStatus.player_renkin_lv + "</color>" + " になった！" + "\n" + _temp_skill[0];
        }
        else
        {
            _text.text = "レベルが上がった！" + "\n" + "パティシエレベルが " + GameMgr.ColorYellow + PlayerStatus.player_renkin_lv + "</color>" + " になった！";
        }
    }

    public void SkillCheck(int _nowlevel)
    {
        _mstatus = 0;

        switch (_nowlevel)
        {
            case 2:

                _temp_skill.Add("仕上げ出来る回数が 1 上がった！");
                PlayerStatus.player_extreme_kaisu_Max++;
                PlayerStatus.player_extreme_kaisu++;
                break;

            case 5:

                _temp_skill.Add("一度に　2個　トッピングできるようになった！");
                GameMgr.topping_Set_Count = 2;
                break;

            case 6:

                _temp_skill.Add("仕上げ出来る回数が 1 上がった！");
                PlayerStatus.player_extreme_kaisu_Max++;
                PlayerStatus.player_extreme_kaisu++;
                break;
            

            case 10:

                _temp_skill.Add("仕上げ出来る回数が 1 上がった！");
                PlayerStatus.player_extreme_kaisu_Max++;
                PlayerStatus.player_extreme_kaisu++;
                break;

            case 15:

                _temp_skill.Add("仕上げ出来る回数が 1 上がった！");
                PlayerStatus.player_extreme_kaisu_Max++;
                PlayerStatus.player_extreme_kaisu++;
                break;

            case 30:

                _temp_skill.Add("仕上げ出来る回数が 1 上がった！");
                PlayerStatus.player_extreme_kaisu_Max++;
                PlayerStatus.player_extreme_kaisu++;
                break;

            case 50:

                _temp_skill.Add("仕上げ出来る回数が 1 上がった！");
                PlayerStatus.player_extreme_kaisu_Max++;
                PlayerStatus.player_extreme_kaisu++;
                break;
        }
    }

    //ハートレベルに応じてスキルを覚えるパターン Girl_Eat_Judgeかデバッグパネルから読む。
    public void SkillCheckHeartLV(int _nowlevel, int _status)
    {
        _mstatus = _status;
        //レベルがあがるごとに、アイテム発見力があがる。
        /*PlayerStatus.player_girl_findpower = 100 + ((girl1_Love_lv-1) * 10);

        //上限処理
        if(PlayerStatus.player_girl_findpower >= 999)
        {
            PlayerStatus.player_girl_findpower = 999;
        }*/

        switch (_nowlevel)
        {
            case 2:

                //ShiageUp();              
                break;

            case 3:

                ShiageUp();
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

                ShiageUp();
                break;

            case 9:

                ShiageUp();
                break;

            case 12:

                ShiageUp();
                break;

            case 15:

                ShiageUp();
                break;

            case 18:

                ShiageUp();
                break;
        }
    }

    void ShiageUp()
    {
        //仕上げできる回数が１上がる。
        PlayerStatus.player_extreme_kaisu_Max++;

        if (_mstatus == 1) //GirlEatJudgeから読んだ場合、パネルを生成する
        {
            girlEat_judge.LvUpPanel1(1);
        }
    }

    public void SetInit_ExpTable() //現在は未使用。
    {
        exp_table = new Dictionary<int, int>();

        exp_table.Add(1, 0);
        exp_table.Add(2, 15);
        exp_table.Add(3, 45);
        exp_table.Add(4, 90);
        exp_table.Add(5, 150);
        exp_table.Add(6, 220);
        exp_table.Add(7, 300);
        exp_table.Add(8, 380);
        exp_table.Add(9, 490);
        exp_table.Add(10, 600);

        exp_table.Add(11, 780);
        exp_table.Add(12, 950);
        exp_table.Add(13, 1080);
        exp_table.Add(14, 1310);
        exp_table.Add(15, 1560);
        exp_table.Add(16, 1800);
        exp_table.Add(17, 2100);
        exp_table.Add(18, 2400);
        exp_table.Add(19, 2700);
        exp_table.Add(20, 3000);

        exp_table.Add(21, 3300);
        exp_table.Add(22, 3600);
        exp_table.Add(23, 3900);
        exp_table.Add(24, 4500);
        exp_table.Add(25, 5100);
        exp_table.Add(26, 5700);
        exp_table.Add(27, 6300);
        exp_table.Add(28, 6900);
        exp_table.Add(29, 7800);
        exp_table.Add(30, 9999);
        //Debug.Log( i+1 + " " + exp_table[i+1]);

    }
}

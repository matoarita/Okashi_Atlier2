using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Contest_Judge : MonoBehaviour {

    private GameObject canvas;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private SoundController sc;
    private BGM sceneBGM;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GirlEat_Judge girlEat_judge;

    private GameObject ScoreHyoujiPanel;
    private GameObject MainQuestOKPanel;
    public bool ScoreHyouji_ON;
    private Text MainQuestText;
    private string _mainquest_name;
    private int _set_MainQuestID;
    private Text Hint_Text;
    private string temp_hint_text;
    private Text Result_Text;
    private string _result_text;
    private string _sweat_kansou, _bitter_kansou, _sour_kansou;
    private string _shokukan_kansou;
    private string _temp_spkansou, _special_kansou;
    private bool Mazui_flag;

    private Text Okashi_Score;
    private List<GameObject> Manzoku_star = new List<GameObject>();
    private Text Manzoku_Score;
    private Text Delicious_Text;

    private Exp_Controller exp_Controller;
    private Touch_Controller touch_controller;

    private PlayerItemList pitemlist;

    private Special_Quest special_quest;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    public bool subQuestClear_check;
    private bool HighScore_flag;
    public bool Gameover_flag;
    private bool kansou_on; //採点表示の際、事前に「うんめー」などのお菓子の感想を表示するか否か。specialクエストの場合は、表示する。

    //女の子の反映用ハートエフェクト
    private GameObject GirlHeartEffect_obj;
    private Particle_Heart_Character GirlHeartEffect;
    private int girllove_param;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;
    private Dictionary<int, int> girlLikeCompoScore = new Dictionary<int, int>();

    private string _commentrandom;
    private List<string> _commentDict = new List<string>();

    private Girl1_status girl1_status;
    private GameObject character;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count, sum;
    private int random;

    private int kettei_item1; //女の子にあげるアイテムの、アイテムリスト番号。
    private int _toggle_type1; //店売りか、オリジナルのアイテムなのかの判定用


    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    AudioSource audioSource;

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    //判定中の女の子のアニメ用
    private bool judge_anim_on;
    private bool judge_end;
    private int judge_anim_status;

    //時間
    private float timeOut;

    //アイテムのパラメータ
    private int _baseID;
    private string _basename;
    private string _basenameHyouji;
    private int _basequality;
    private int _basesweat;
    private int _basebitter;
    private int _basesour;
    private int _baserich;
    private int _basecrispy;
    private int _basefluffy;
    private int _basesmooth;
    private int _basehardness;
    private int _basejiggly;
    private int _basechewy;
    private int _basepowdery;
    private int _baseoily;
    private int _basewatery;
    private float _basegirl1_like;
    private int _baseSetjudge_num;
    private string[] _basetp;
    private string[] _koyutp;

    private string _baseitemtype;
    private string _baseitemtype_sub;

    //private string _basename;
    //private int _basemp;
    //private int _baseday;
    private int _basecost;
    //private int _basesell;

    //女の子の計算用パラメータ

    private int _girlquality;
    private int[] _girlsweat;
    private int[] _girlbitter;
    private int[] _girlsour;
    private int[] _girlrich;
    private int[] _girlcrispy;
    private int[] _girlfluffy;
    private int[] _girlsmooth;
    private int[] _girlhardness;
    private int[] _girljiggly;
    private int[] _girlchewy;

    private int _girlpowdery;
    private int _girloily;
    private int _girlwatery;

    private int _set_compID;

    private string[] _girl_subtype;
    private string[] _girl_likeokashi;

    private int[] _girl_set_score;
    private int[] _girl_comment_flag;


    //女の子の採点用パラメータ

    private int quality_result;
    private int rich_result;
    private int sweat_result;
    private int bitter_result;
    private int sour_result;

    private int crispy_result;
    private int fluffy_result;
    private int smooth_result;
    private int hardness_result;
    private int jiggly_result;
    private int chewy_result;

    private int powdery_result;
    private int oily_result;
    private int watery_result;


    //採点用パラメータを元に、さらに計算し、最終的に出されるスコア。
    //女の子が感想をいうときにも、使う。スコアは、girl1_statusにも共有し、宴と処理を絡める。
    private int sweat_level;
    private int bitter_level;
    private int sour_level;

    public int quality_score;
    public int rich_score;
    public int sweat_score;
    public int bitter_score;
    public int sour_score;

    public int crispy_score;
    public int fluffy_score;
    public int smooth_score;
    public int hardness_score;
    public int jiggly_score;
    public int chewy_score;
    private int shokukan_score;

    public int subtype1_score;
    public int subtype2_score;

    public int topping_score;
    public int topping_flag_point;
    public bool topping_flag;
    public bool topping_all_non;

    public int[] total_score;
    private float _temp_score;

    private bool[] dislike_flag;
    private int dislike_status;

    public int girllike_point;

    private string shokukan_mes;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの所持数
    List<int> itemslotScore = new List<int>();

    private int rnd, rnd2;
    private int set_id;

    //女の子の好み組み合わせセットのデータ
    private int _compID;
    private int set1_ID;
    private int set2_ID;
    private int set3_ID;
    private int Set_Count;

    private List<int> set_ID = new List<int>();

    //エフェクト
    private GameObject Score_effect_Prefab1;
    private GameObject Score_effect_Prefab2;
    private List<GameObject> _listScoreEffect = new List<GameObject>();


    private GameObject stageclear_toggle;
    private GameObject stageclear_Button;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _windowtext = text_area.GetComponentInChildren<Text>();

        //女の子、お菓子の判定処理オブジェクトの取得
        girlEat_judge = this.GetComponent<GirlEat_Judge>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //要素数の初期化
        _girlsweat = new int[girl1_status.youso_count];
        _girlbitter = new int[girl1_status.youso_count];
        _girlsour = new int[girl1_status.youso_count];
        _girlrich = new int[girl1_status.youso_count];
        _girlcrispy = new int[girl1_status.youso_count];
        _girlfluffy = new int[girl1_status.youso_count];
        _girlsmooth = new int[girl1_status.youso_count];
        _girlhardness = new int[girl1_status.youso_count];
        _girljiggly = new int[girl1_status.youso_count];
        _girlchewy = new int[girl1_status.youso_count];

        _girl_subtype = new string[girl1_status.youso_count];
        _girl_likeokashi = new string[girl1_status.youso_count];

        _girl_set_score = new int[girl1_status.youso_count];
        _girl_comment_flag = new int[girl1_status.youso_count];

        total_score = new int[girl1_status.youso_count];

        dislike_flag = new bool[girl1_status.youso_count];

        _basetp = new string[database.items[0].toppingtype.Length];
        _koyutp = new string[database.items[0].koyu_toppingtype.Length];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //選んだアイテムを審査委員が判定するメソッド
    public void Contest_Judge_method(int value1, int value2, int judge_num, int judge_type)
    {
        //一度、決定したアイテムのリスト番号と、タイプを取得
        kettei_item1 = value1;
        _toggle_type1 = value2;

        //アイテムパラメータの取得

        switch (_toggle_type1)
        {
            case 0:

                _baseID = database.items[kettei_item1].itemID;
                _basename = database.items[kettei_item1].itemName;
                _basenameHyouji = database.items[kettei_item1].itemNameHyouji;
                _basequality = database.items[kettei_item1].Quality;
                _basesweat = database.items[kettei_item1].Sweat;
                _basebitter = database.items[kettei_item1].Bitter;
                _basesour = database.items[kettei_item1].Sour;
                _baserich = database.items[kettei_item1].Rich;
                _basecrispy = database.items[kettei_item1].Crispy;
                _basefluffy = database.items[kettei_item1].Fluffy;
                _basesmooth = database.items[kettei_item1].Smooth;
                _basehardness = database.items[kettei_item1].Hardness;
                _basejiggly = database.items[kettei_item1].Jiggly;
                _basechewy = database.items[kettei_item1].Chewy;
                _basepowdery = database.items[kettei_item1].Powdery;
                _baseoily = database.items[kettei_item1].Oily;
                _basewatery = database.items[kettei_item1].Watery;
                _basegirl1_like = database.items[kettei_item1].girl1_itemLike;
                _baseitemtype = database.items[kettei_item1].itemType.ToString();
                _baseitemtype_sub = database.items[kettei_item1].itemType_sub.ToString();
                _basecost = database.items[kettei_item1].cost_price;
                _baseSetjudge_num = database.items[kettei_item1].SetJudge_Num;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = database.items[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = database.items[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            case 1:

                _baseID = pitemlist.player_originalitemlist[kettei_item1].itemID;
                _basename = pitemlist.player_originalitemlist[kettei_item1].itemName;
                _basenameHyouji = pitemlist.player_originalitemlist[kettei_item1].itemNameHyouji;
                _basequality = pitemlist.player_originalitemlist[kettei_item1].Quality;
                _basesweat = pitemlist.player_originalitemlist[kettei_item1].Sweat;
                _basebitter = pitemlist.player_originalitemlist[kettei_item1].Bitter;
                _basesour = pitemlist.player_originalitemlist[kettei_item1].Sour;
                _baserich = pitemlist.player_originalitemlist[kettei_item1].Rich;
                _basecrispy = pitemlist.player_originalitemlist[kettei_item1].Crispy;
                _basefluffy = pitemlist.player_originalitemlist[kettei_item1].Fluffy;
                _basesmooth = pitemlist.player_originalitemlist[kettei_item1].Smooth;
                _basehardness = pitemlist.player_originalitemlist[kettei_item1].Hardness;
                _basejiggly = pitemlist.player_originalitemlist[kettei_item1].Jiggly;
                _basechewy = pitemlist.player_originalitemlist[kettei_item1].Chewy;
                _basepowdery = pitemlist.player_originalitemlist[kettei_item1].Powdery;
                _baseoily = pitemlist.player_originalitemlist[kettei_item1].Oily;
                _basewatery = pitemlist.player_originalitemlist[kettei_item1].Watery;
                _basegirl1_like = pitemlist.player_originalitemlist[kettei_item1].girl1_itemLike;
                _baseitemtype = pitemlist.player_originalitemlist[kettei_item1].itemType.ToString();
                _baseitemtype_sub = pitemlist.player_originalitemlist[kettei_item1].itemType_sub.ToString();
                _basecost = pitemlist.player_originalitemlist[kettei_item1].cost_price;
                _baseSetjudge_num = pitemlist.player_originalitemlist[kettei_item1].SetJudge_Num;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_originalitemlist[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            default:
                break;
        }



        //** 判定用に、コンテストの好み値(GirlLikeSet)をセッティング

        switch (judge_type)
        {

            case 0:

                //girlLikeCompo組み合わせセットの_compIDを元に選ぶ。
                /*i = 0;
                while (i < girlLikeCompo_database.girllike_composet.Count)
                {
                    //OkashiQuest_ID = specialquestのクエスト番号（コンテストイベント時は1500）が入っているはず。
                    if (girlLikeCompo_database.girllike_composet[i].set_ID == judge_num)
                    {
                        _compID = i;
                        break;
                    }
                    i++;
                }*/

                set1_ID = judge_num; //審査員１の好み
                set2_ID = judge_num + 1; //審査員２の好み
                set3_ID = judge_num + 2; //審査員３の好み

                set_ID.Clear();

                //set_idにリストの番号をセット
                if (set1_ID != 9999)
                {
                    set_ID.Add(set1_ID);
                }
                if (set2_ID != 9999)
                {
                    set_ID.Add(set2_ID);
                }
                if (set3_ID != 9999)
                {
                    set_ID.Add(set3_ID);
                }

                //さきほどのset_IDをもとに、好みの値を決定する。
                for (count = 0; count < set_ID.Count; count++)
                {
                    //compNum, セットする配列番号　の順　セットの番号は現状３つまで設定可 ３番めの番号は、女の子かコンテストかのタイプ判定　1=コンテスト
                    girl1_status.InitializeStageGirlHungrySet(set_ID[count], count);
                    //Debug.Log("set_ID: " + count + " : " + set_ID[count]);
                }

                Set_Count = set_ID.Count;

                break;

            case 1:

                girl1_status.InitializeStageGirlHungrySet(judge_num, 0);
                Set_Count = 1;
                break;
        }


        //上記は、girls1_status上でセッティングしただけなので、こっちのスクリプトに読み込む
        SetGirlTasteInit();

        //**



        //一回まず各スコアを初期化。
        for (i = 0; i < itemslotScore.Count; i++)
        {
            itemslotScore[i] = 0;
        }

        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslotScore[count]++;
                }
                count++;
            }
        }

        //固有トッピングスロットも見る。一致する効果があれば、所持数+1
        for (i = 0; i < _koyutp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_koyutp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log("_koyutp: " + _koyutp[i]);
                    itemslotScore[count]++;
                }
                count++;
            }
        }

        //お菓子の味判定処理
        judge_result_contest(); //判定し、トータルのスコアが算出される。

        switch (dislike_status)
        {
            case 0:

                switch (judge_type)
                {
                    case 0:

                        _windowtext.text = "審査員１　点数：" + total_score[0] + "点" + "\n" +
                            "審査員２　点数：" + total_score[1] + "点" + "\n" +
                            "審査員３　点数：" + total_score[2] + "点";

                        //点数を200点を上限にし、100点に正規化する処理
                        for (i = 0; i < GameMgr.contest_Score.Length; i++)
                        {
                            _temp_score = SujiMap(total_score[i], 0, 200, 0, 100);
                            total_score[i] = (int)_temp_score;
                        }

                        sum = 0;
                        for (i=0; i< GameMgr.contest_Score.Length; i++)
                        {
                            GameMgr.contest_Score[i] = total_score[i];
                            sum += total_score[i];
                        }

                        GameMgr.contest_TotalScore = sum / GameMgr.contest_Score.Length;
                        break;

                    case 1:

                        _windowtext.text = "決勝戦" + "\n" +
                            "審査員　総合得点：" + total_score[0] + "点";

                        GameMgr.contest_TotalScore = total_score[0];
                        break;
                }
                break;

            case 1: //求められる課題のお菓子と違った場合

                _windowtext.text = "課題のお菓子ではないので、失格！";
                break;

            case 2: //油っこいなどのマイナスをこえてしまった場合

                break;


        }
        
        //先に算出しておいて、あとで、審査員一人一人のコメント＋点数を演出して出す。宴へ戻る。
    }


    void judge_result_contest()
    {
        count = 0;

        while (count < Set_Count) //セットの組み合わせ=審査員の数だけ判定。まずかった場合は、単純にスコアが下がる補正がかかるようにフラグをたてる。
        {
            //パラメータ初期化し、判定処理
            dislike_flag[count] = true;
            dislike_status = 0;
            set_id = count;

            //
            //判定処理　パターンCのみ
            //
            if (_basepowdery > 50)
            {
                dislike_flag[count] = false;
                dislike_status = 2;
            }
            if (_baseoily > 50)
            {
                dislike_flag[count] = false;
                dislike_status = 2;
            }
            if (_basewatery > 50)
            {
                dislike_flag[count] = false;
                dislike_status = 2;
            }


            //
            //判定処理　パターンA
            //                    
            /*
            //④特定のお菓子の判定。④が一致していない場合は、③は計算するまでもなく不正解となる。
            if (_girl_likeokashi[count] == "Non") //特に指定なし
            {
                //③お菓子の種別の計算
                if (_girl_subtype[count] == "Non") //特に指定なし
                {

                }
                else if (_girl_subtype[count] == _baseitemtype_sub) //お菓子の種別が一致している。
                {

                }
                else
                {
                    dislike_flag[count] = false;
                    dislike_status = 1;
                }
            }
            else if (_girl_likeokashi[count] == _basename) //お菓子の名前が一致している。
            {
                //サブは計算せず、特定のお菓子自体が正解なら、正解

            }
            else
            {
                dislike_flag[count] = false;
                dislike_status = 1;
            }*/

            //次に味の判定処理。判定後、採点の数値がかえってくる。

            total_score[count] = girlEat_judge.Judge_Score_Return(kettei_item1, _toggle_type1, 1, count); //点数の判定。3番目の0~1の数字は、女の子のお菓子の判定か、コンテストでの判定かのタイプ分け
            //Debug.Log("審査員　点数: " + total_score[count]);

            count++;

        }
    }

    IEnumerator Girl_Judge_anim_co()
    {

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        if (!GameMgr.tutorial_ON)
        {
            //お菓子を食べた後のちょっとした感想をだす。
            if (dislike_status == 1 || dislike_status == 2 || dislike_status == 6)
            {
                //StartCoroutine("Girl_Comment");
            }
            else if (dislike_status == 3 || dislike_status == 4)//まずいとき
            {
                //StartCoroutine("Girl_Comment");
            }
            else if (dislike_status == 5)
            {
                //Girl_reaction();
            }
        }
        else
        {
            //Girl_reaction();
        }
    }

    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslotScore.Add(0);
        }

    }

    void SetGirlTasteInit()
    {
        for (i = 0; i < girl1_status.youso_count; i++)
        {
            //女の子の計算パラメータを代入
            _girlquality = girl1_status.girl1_Quality;
            _girlsweat[i] = girl1_status.girl1_Sweat[i];
            _girlbitter[i] = girl1_status.girl1_Bitter[i];
            _girlsour[i] = girl1_status.girl1_Sour[i];
            _girlrich[i] = girl1_status.girl1_Rich[i];
            _girlcrispy[i] = girl1_status.girl1_Crispy[i];
            _girlfluffy[i] = girl1_status.girl1_Fluffy[i];
            _girlsmooth[i] = girl1_status.girl1_Smooth[i];
            _girlhardness[i] = girl1_status.girl1_Hardness[i];
            _girljiggly[i] = girl1_status.girl1_Jiggly[i];
            _girlchewy[i] = girl1_status.girl1_Chewy[i];

            _girl_subtype[i] = girl1_status.girl1_likeSubtype[i];
            _girl_likeokashi[i] = girl1_status.girl1_likeOkashi[i];

            _girl_set_score[i] = girl1_status.girl1_like_set_score[i];
            _girl_comment_flag[i] = girl1_status.girllike_comment_flag[i];
        }

        //一回だけ代入すればよい。
        _girlpowdery = girl1_status.girl1_Powdery;
        _girloily = girl1_status.girl1_Oily;
        _girlwatery = girl1_status.girl1_Watery;

        //girllikeCompoのOkashiQuest_IDのこと。コンテストでは使ってないかも。
        _set_compID = girl1_status.Set_compID;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}

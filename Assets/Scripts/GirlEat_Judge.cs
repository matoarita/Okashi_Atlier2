//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlEat_Judge : MonoBehaviour {

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

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

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

    private bool subQuestClear_check;
    private bool HighScore_flag;
    private bool Gameover_flag;
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

    private GameObject hukidashiitem;   
    private Text _hukidashitext;

    private GameObject eat_hukidashiPrefab;
    private GameObject eat_hukidashiitem;
    private Text eat_hukidashitext;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count;
    private int random;

    private int kettei_item1; //女の子にあげるアイテムの、アイテムリスト番号。
    private int _toggle_type1; //店売りか、オリジナルのアイテムなのかの判定用

    private GameObject _slider_obj;
    private Slider _slider; //好感度バーを取得
    private int _exp;
    private int slot_girlscore, slot_money;
    private int Getlove_exp;
    private int GetMoney;
    private bool loveanim_on;
    private Text girl_lv;

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

    private SpriteRenderer s;

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
    private int _basegirl1_like;
    private int _baseSetjudge_num;
    private string[] _basetp = new string[10];
    private string[] _koyutp = new string[3];

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
    public int topping_flag;

    public int total_score;

    private bool dislike_flag;
    private int dislike_status;
    private int true_set;

    public int girllike_point;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの点数
    List<int> itemslotScore = new List<int>();

    private GameObject effect_Prefab;
    private List<GameObject> _listEffect = new List<GameObject>();

    private GameObject heart_Prefab;
    public List<GameObject> _listHeart = new List<GameObject>();
    public int heart_count; //画面上に存在するハートの個数

    private GameObject hearthit_Prefab;
    private List<GameObject> _listHeartHit = new List<GameObject>();

    private GameObject hearthit2_Prefab;
    private List<GameObject> _listHeartHit2 = new List<GameObject>();

    private Vector3 heartPos;

    private int rnd, rnd2;
    private int set_id;

    //エフェクト
    private GameObject Score_effect_Prefab1;
    private GameObject Score_effect_Prefab2;
    private List<GameObject> _listScoreEffect = new List<GameObject>();

    //好感度レベルテーブルの取得
    private List<int> stage_levelTable = new List<int>();

    // Use this for initialization
    void Start() {

        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();

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

        //エクストリームパネルの取得
        Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");
        extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();

        //タッチ判定オブジェクトの取得
        touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //キャラクタ取得
        character = GameObject.FindWithTag("Character");

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("Canvas").transform.Find("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //女の子の反映用ハートエフェクト取得
        GirlHeartEffect_obj = GameObject.FindWithTag("Particle_Heart_Character");
        GirlHeartEffect = GirlHeartEffect_obj.GetComponent<Particle_Heart_Character>();

        //女の子のレベル取得
        girl_lv = GameObject.FindWithTag("Girl_love_exp_bar").transform.Find("LV_param").GetComponent<Text>();
        girl_lv.text = girl1_status.girl1_Love_lv.ToString();

        //エフェクトプレファブの取得
        effect_Prefab = (GameObject)Resources.Load("Prefabs/Particle_Heart");
        Score_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_ResultFeather");
        Score_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_Compo5");

        //ハートプレファブの取得
        heart_Prefab = (GameObject)Resources.Load("Prefabs/HeartUpObj");
        hearthit_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect");
        hearthit2_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect2");

        //Prefab内の、コンテンツ要素を取得
        eat_hukidashiPrefab = (GameObject)Resources.Load("Prefabs/Eat_hukidashi");

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindowMain").gameObject;
        _windowtext = text_area.GetComponentInChildren<Text>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //お菓子採点結果表示用パネルの取得
        ScoreHyoujiPanel = canvas.transform.Find("ScoreHyoujiPanel/Result_Panel").gameObject;
        Okashi_Score = ScoreHyoujiPanel.transform.Find("Image/Okashi_Score").GetComponent<Text>();
        MainQuestOKPanel = canvas.transform.Find("ScoreHyoujiPanel/MainQuestOKPanel").gameObject;
        MainQuestText = MainQuestOKPanel.transform.Find("Image/QuestClearText").GetComponent<Text>();
        Hint_Text = ScoreHyoujiPanel.transform.Find("Image/Hint_Text").GetComponent<Text>();
        Result_Text = ScoreHyoujiPanel.transform.Find("Image/Result_Text").GetComponent<Text>();
        ScoreHyoujiPanel.SetActive(false);
        ScoreHyouji_ON = false;
        MainQuestOKPanel.SetActive(false);

        i = 0;
        foreach (Transform child in ScoreHyoujiPanel.transform.Find("Image/Manzoku_Score_star/Viewport/Content").transform)
        {
            //Debug.Log(child.name);           
            Manzoku_star.Add(child.gameObject);
            Manzoku_star[i].SetActive(false);
            i++;
        }
        Manzoku_Score = ScoreHyoujiPanel.transform.Find("Image/Manzoku_Score").GetComponent<Text>();
        Delicious_Text = ScoreHyoujiPanel.transform.Find("DeliciousPanel/Text").GetComponent<Text>();
        ScoreHyoujiPanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        kettei_item1 = 0;
        _toggle_type1 = 0;

        dislike_flag = true;

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //好感度バーの取得
        _slider_obj = GameObject.FindWithTag("Girl_love_exp_bar").gameObject;
        _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();

        _exp = 0;
        Getlove_exp = 0;
        GetMoney = 0;
        loveanim_on = false;

        stage_levelTable.Clear();

        //バーの最大値の設定。テーブルの初期設定はGirl1_statusで行っている。ここではそれをもとにコピーしてるだけ。
        switch (GameMgr.stage_number)
        {
            case 1:

                for (i = 0; i < girl1_status.stage1_lvTable.Count; i++) {
                    stage_levelTable.Add(girl1_status.stage1_lvTable[i]);
                }

                break;

            case 2:

                for (i = 0; i < girl1_status.stage1_lvTable.Count; i++)
                {
                    stage_levelTable.Add(girl1_status.stage1_lvTable[i]);
                }
                break;

            case 3:

                for (i = 0; i < girl1_status.stage1_lvTable.Count; i++)
                {
                    stage_levelTable.Add(girl1_status.stage1_lvTable[i]);
                }
                break;
        }

        //スライダのMAXを設定。現在の好感度レベルで変わる。
        Love_Slider_Setting();        

        //スライダ表示を更新
        i = 0;
        girllove_param = girl1_status.girl1_Love_exp;
        while (i < girl1_status.girl1_Love_lv - 1 )
        {
            girllove_param -= stage_levelTable[i];
            i++;
        }
        _slider.value = girllove_param;

        //レベルを表示
        girl_lv.text = girl1_status.girl1_Love_lv.ToString();

        //アニメーション用時間
        timeOut = 5.0f;

        judge_anim_on = false;
        judge_end = false;
        judge_anim_status = 0;

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

        //サブクエストチェック用フラグ
        subQuestClear_check = false;
        HighScore_flag = false;
        Gameover_flag = false;
        kansou_on = false;

        //テキストのセッティング
        CommentTextInit();
    }
	
	// Update is called once per frame
	void Update () {

        //好感度バーアニメーションの処理
        if (loveanim_on == true)
        {

            if (Getlove_exp < 0)//減る場合は、こっちの処理。レベルが下がることはない。増えるアニメ処理は、別メソッドで行う。
            {
                //好感度が0の場合、0が下限。
                if (girl1_status.girl1_Love_exp <= 0)
                {
                    Getlove_exp = 0;
                    _exp = 0;
                    loveanim_on = false;

                    compound_Main.check_GirlLoveEvent_flag = false;
                }
                else
                {
                    //１ずつ減少
                    --_exp;
                    
                    //スライダにも反映
                    _slider.value -= 1;

                    if(_slider.value <= 0) //スライダが0になった場合、そこが下限。girl1_Love_expは、それ以上減少しない。
                    {

                    }
                    else
                    {
                        --girl1_status.girl1_Love_exp;
                    }

                    if (_exp <= Getlove_exp)
                    {
                        Getlove_exp = 0;
                        _exp = 0;
                        loveanim_on = false;

                        compound_Main.check_GirlLoveEvent_flag = false;
                    }
                }
            }
            else
            {
                //0の場合で、アニメーションが作動した場合は、特になにもしない。
                Getlove_exp = 0;
                _exp = 0;
                loveanim_on = false;
            }
        }

        if (judge_anim_on == true)
        {
            switch(judge_anim_status)
            {
                case 0: //初期化 状態１
                 
                    girl1_status.GirlEat_Judge_on = false;

                    Extremepanel_obj.SetActive(false);
                    MoneyStatus_Panel_obj.SetActive(false);
                    text_area.SetActive(false);

                    timeOut = 1.0f;
                    judge_anim_status = 1;
                    s.sprite = girl1_status.Girl1_img_eat_start;

                    //現在の吹き出しを削除
                    girl1_status.DeleteHukidashiOnly();

                    //食べ中の表示用吹き出しを生成
                    eat_hukidashiitem = Instantiate(eat_hukidashiPrefab);
                    eat_hukidashitext = eat_hukidashiitem.GetComponentInChildren<Text>();

                    eat_hukidashitext.text = ".";

                    //カメラ寄る。
                    trans++; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    break;

                case 1: // 状態2

                    if( timeOut <= 0.0)
                    {
                        timeOut = 1.0f;
                        judge_anim_status = 2;

                        eat_hukidashitext.text = ". .";
                        
                    }
                    break;

                case 2:

                    if (timeOut <= 0.0)
                    {
                        timeOut = 2.0f;
                        judge_anim_status = 3;

                    }
                    break;

                case 3: //アニメ終了。判定する

                    Extremepanel_obj.SetActive(true);
                    MoneyStatus_Panel_obj.SetActive(true);
                    text_area.SetActive(true);

                    //食べ中吹き出しの削除
                    if (eat_hukidashiitem != null)
                    {
                        Destroy(eat_hukidashiitem);
                    }

                    judge_anim_on = false;
                    judge_end = true;
                    judge_anim_status = 0;

                    //カメラ寄る。
                    trans--; //transが0以下のときに、ズームアウトするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    break;

                default:
                    break;
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }
    }

    //選んだアイテムと、女の子の好みを比較するメソッド

    public void Girleat_Judge_method(int value1, int value2)
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

        //もし通常の場合は、あげたお菓子によって、その好み値をセッティングする。girlLikeSetの番号を指定して、判定用に使う。
        if(girl1_status.OkashiNew_Status == 1)
        {
            girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
        }

        SetGirlTasteInit();        

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
                if ( _basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
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

        //確認用
        /*count = 0;
        foreach (string key in itemslotInfo)
        {
            Debug.Log(key + ": " + itemslotScore[count].ToString());
            count++;
        }*/


        switch (girl1_status.timeGirl_hungry_status)
        {
            case 0: //お腹が特に減ってない状態。

                Debug.Log("今はあまりお腹が減ってないらしい");

                _windowtext.text = "今はあまりお腹が減ってないらしい";

                compound_Main.compound_status = 0;
                //お菓子の判定処理を終了
                compound_Main.girlEat_ON = false;

                break;

            case 1: //お腹が空いた状態。吹き出しがでる。


                //女の子が食べたいものを満たしているか、比較する
                //OKだったら、正解し、さらに好感度の上昇値を計算する。

                //判定アニメ起動
                judge_anim_on = true;
                judge_end = false;
                StartCoroutine("Girl_Judge_anim_co");

                break;

            case 2:

                _windowtext.text = "お菓子をあげたばかりだ。";

                compound_Main.compound_status = 0;
                //お菓子の判定処理を終了
                compound_Main.girlEat_ON = false;

                break;

            default:
                break;
        }
        


        //宴用にgirl1_statusにも、点数を共有
        girl1_status.girl_final_kettei_item = kettei_item1;

        girl1_status.quality_score_final = quality_score;
        girl1_status.sweat_score_final = sweat_score;
        girl1_status.bitter_score_final = bitter_score;
        girl1_status.sour_score_final = sour_score;

        girl1_status.crispy_score_final = crispy_score;
        girl1_status.fluffy_score_final = fluffy_score;
        girl1_status.smooth_score_final = smooth_score;
        girl1_status.hardness_score_final = hardness_score;
        girl1_status.jiggly_score_final = jiggly_score;
        girl1_status.chewy_score_final = chewy_score;

        girl1_status.subtype1_score_final = subtype1_score;
        girl1_status.subtype2_score_final = subtype2_score;

        girl1_status.total_score_final = total_score;


        // => Compound_Mainに戻る


        //window_param_result_obj.SetActive(true);

        //デバッグ用　計算結果の表示
        /*window_result_text.text = "###  好みの比較　結果　###"
            + "\n" + "\n" + _basenameHyouji + " のあまさ: " + _basesweat + "\n" + " 女の子の好みの甘さ: " + _girlsweat + "\n" + "あまさの差: " + sweat_result + " 点数: " + sweat_score
            + "\n" + "\n" + _basenameHyouji + " の苦さ: " + _basebitter + "\n" + " 女の子の好みの苦さ: " + _girlbitter + "\n" + "にがさの差: " + bitter_result + " 点数: " + bitter_score
            + "\n" + "\n" + _basenameHyouji + " の酸味: " + _basesour + "\n" + " 女の子の好みの酸味: " + _girlsour + "\n" + "酸味の差: " + sour_result + " 点数: " + sour_score
            + "\n" + "\n" + "さくさく度: " + _basecrispy + "\n" + "さくさく閾値: " + _girlcrispy + "\n" + "差: " + crispy_result + " 点数: " + crispy_score
            + "\n" + "\n" + "ふわふわ度: " + _basefluffy + "\n" + "ふわふわ閾値: " + _girlfluffy + "\n" + "差: " + fluffy_result + " 点数: " + fluffy_score
            + "\n" + "\n" + "なめらか度: " + _basesmooth + "\n" + "なめらか閾値: " + _girlsmooth + "\n" + "差: " + smooth_result + " 点数: " + smooth_score
            + "\n" + "\n" + "歯ごたえ度: " + _basehardness + "\n" + "歯ごたえ閾値: " + _girlhardness + "\n" + "差: " + hardness_result + " 点数: " + hardness_score
            + "\n" + "\n" + "ぷるぷる度: " + "-"
            + "\n" + "\n" + "噛み応え度: " + "-"
            + "\n" + "\n" + girl1_status.girl1_Subtype1 + "が好き " + "点数: " + subtype1_score
            + "\n" + "\n" + girl1_status.girl1_Subtype2 + "が好き " + "点数: " + subtype2_score
            + "\n" + "\n" + "総合得点: " + total_score;*/
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
        }

        //一回だけ代入すればよい。
        _girlpowdery = girl1_status.girl1_Powdery;
        _girloily = girl1_status.girl1_Oily;
        _girlwatery = girl1_status.girl1_Watery;

        _set_compID = girl1_status.Set_compID;
    }

    IEnumerator Girl_Judge_anim_co()
    {
        judge_result(); //先に判定し、マズイなどがあったら、アニメにも反映する。

        judge_score(); //点数の判定。

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        if (GameMgr.tutorial_ON != true)
        {
            //お菓子を食べた後のちょっとした感想をだす。
            if (dislike_status == 1 || dislike_status == 2 || dislike_status == 6)
            {                
                StartCoroutine("Girl_Comment");
                //Girl_reaction();
            }
            else //まずいときは、吹き出しでまずい感想だすだけ
            {
                StartCoroutine("Girl_Comment");
                //Girl_reaction();
            }
        }
        else
        {
            Girl_reaction();
        }
    }

    //
    //お菓子食べた直後の、おいしい・まずいといった感想。トッピングに対する感想もここで。
    //
    IEnumerator Girl_Comment()
    {
        //Debug.Log("Girl_Comment");

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();
        //character.GetComponent<FadeCharacter>().FadeImageOff();

        if (Mazui_flag) //味がまずかった場合（total_scoreがマイナスだったとき）
        {
            GameMgr.OkashiComment_ID = 9999; //
        }
        else
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + topping_flag; //クエストID+topping_flag(1~5)で指定する。ねこクッキーで、アイザン入りなら、1000+1で、1001。
        }
        GameMgr.OkashiComment_flag = true;
       
        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;

        //character.GetComponent<FadeCharacter>().FadeImageOn();
        canvas.SetActive(true);

        Girl_reaction();
    }

    void Dislike_Okashi_Judge()
    {
        //粉っぽさなどのマイナス判定。一番強い。ここであまりに粉っぽさなどが強い場合は、問答無用で嫌われる。

        if (_basepowdery > 50)
        {
            dislike_flag = false;
            dislike_status = 3;
        }
        if (_baseoily > 50)
        {
            dislike_flag = false;
            dislike_status = 3;
        }
        if (_basewatery > 50)
        {
            dislike_flag = false;
            dislike_status = 3;
        }

        if (_basegirl1_like <= 0) //女の子の好みが-のものも、嫌われる。お菓子それ自体が嫌い、ということ。
        {
            dislike_flag = false;
            dislike_status = 4;
        }

        for (i = 0; i < itemslotScore.Count; i++) //トッピングスロットを計算。嫌いなトッピングが使われていると、嫌われる。
        {
            //0はNonなので、無視
            if (i != 0)
            {
                //あげたアイテムに、女の子の嫌いな材料が使われていた！
                if (itemslotInfo[i] == "Shishamo" && itemslotScore[i] > 0)
                {
                    dislike_flag = false;
                    dislike_status = 4;
                }
            }
        }
    }

    void judge_result()
    {

        //通常
        if(girl1_status.OkashiNew_Status == 1)
        {
            dislike_flag = true;
            dislike_status = 1; //1=デフォルトで良い。 2=新しいお菓子だった。　3=まずい。　4=嫌い。 5=今はこれの気分じゃない。 6=普通に食べて判定。ただしスペシャルを無視。
            set_id = 0;

            //
            //判定処理　パターンCのみ
            //
            Dislike_Okashi_Judge();

        }
        //スペシャルお菓子の場合
        else if ( girl1_status.OkashiNew_Status == 0) {

            count = 0;

            //Debug.Log("girl1_status.Set_Count: " + girl1_status.Set_Count);
            while (count < girl1_status.Set_Count) //セットの組み合わせの数だけ判定。最大３。どれか一つのセットが条件をクリアしていれば、正解。
            {
                //パラメータ初期化し、判定処理
                dislike_flag = true;
                dislike_status = 1;
                set_id = count;

                //
                //判定処理　パターンCのみ
                //
                Dislike_Okashi_Judge();              

                if (dislike_status == 3 || dislike_status == 4)
                {
                    //パターンCのマイナスフラグがたってしまったので、以下の判定処理を無視
                    break;
                }
                else
                {
                    //
                    //判定処理　パターンA
                    //                    

                    /*
                    //②味の比較。
                    if (_baserich >= _girlrich[count])
                    {
                        //break;
                    }
                    else { dislike_flag = false; }

                    if (_basesweat >= _girlsweat[count])
                    {
                        //break;
                    }
                    else { dislike_flag = false; }

                    if (_basebitter >= _girlbitter[count])
                    {
                        //break;
                    }
                    else { dislike_flag = false; }

                    if (_basesour >= _girlsour[count])
                    {
                        //break;
                    }
                    else { dislike_flag = false; }*/


                    //④特定のお菓子の判定。④が一致していない場合は、③は計算するまでもなく不正解となる。
                    if (_girl_likeokashi[count] == "Non") //特に指定なし
                    {
                        //③お菓子の種別の計算
                        if (_girl_subtype[count] == "Non") //特に指定なし
                        {
                            //break;
                        }
                        else if (_girl_subtype[count] == _baseitemtype_sub) //お菓子の種別が一致している。
                        {
                            //break;
                        }
                        else
                        {
                            dislike_flag = false;
                        }
                    }
                    else if (_girl_likeokashi[count] == _basename) //お菓子の名前が一致している。
                    {
                        //サブは計算せず、特定のお菓子自体が正解なら、正解
                        //break;
                    }
                    else
                    {
                        dislike_flag = false;
                    }

                    //Debug.Log("_girl_likeokashi[count]: " + _girl_likeokashi[count]);
                    Debug.Log("あげたお菓子: " + _basename);

                    //判定 嫌いなものがなければbreak。falseだった場合、次のセットを見る。
                    if (dislike_flag) {
                        true_set = count;
                        break;
                    }

                    count++;
                }


                //この時点で、嫌いなもの（吹き出しと違うもの）であれば、dislike_flagがたっている。

                //
                //判定処理　パターンB
                //

                //吹き出しにあっているかいないかの判定。
                if (dislike_flag == true) //吹き出しに合っている場合
                {

                }
                else if (dislike_flag == false) //吹き出しに合っていない場合
                {

                    dislike_status = 5; //スペシャルクエストだった場合は、これじゃないという。
                    /*
                    if (database.items[_baseID].First_eat == 0) //新しい食べ物の場合
                    {
                        dislike_flag = true;
                        dislike_status = 2;

                        //判定処理が通常のものにかわる。
                        girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順
                        SetGirlTasteInit();
                    }
                    else
                    {
                        dislike_flag = true;
                        dislike_status = 6;

                        //判定処理が通常のものにかわる。
                        girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順
                        SetGirlTasteInit();
                    }*/
                }
            }
        }
    }

    void judge_score()
    {
        //初期化。

        if (dislike_flag == true) //正解の場合のみ、味を採点する。好感度とお金に反映される。
        {

            //お菓子の判定処理

            //クッキーの場合はさくさく感など。大きいパラメータをまず見る。次に甘さ・苦さ・酸味が、女の子の好みに近いかどうか。

            total_score = 0;
            crispy_score = 0;
            fluffy_score = 0;
            smooth_score = 0;
            hardness_score = 0;
            topping_score = 0;
            topping_flag = 0;

            //未使用。
            quality_score = 0;
            rich_score = 0;
            jiggly_score = 0;
            chewy_score = 0;


            //味パラメータの計算。味は、GirlLikeSetの値で、理想値としている。
            //理想の値に近いほど高得点。超えすぎてもいけない。

            //rich_result = _baserich - _girlrich[set_id];
            sweat_result = _basesweat - _girlsweat[set_id];
            bitter_result = _basebitter - _girlbitter[set_id];
            sour_result = _basesour - _girlsour[set_id];


            //あまみ・にがみ・さんみに対して、それぞれの評価。差の値により、7段階で評価する。
            //元のセットの値が0のときは、計算せずスコアに加点しない。

            if (_girlsweat[set_id] == 0)
            {
                Debug.Log("甘み: 判定なし");
                sweat_level = 0;
                sweat_score = 0;
            }
            else
            {
                //甘味
                if (Mathf.Abs(sweat_result) == 0)
                {
                    Debug.Log("甘み: Perfect!!");
                    sweat_level = 7;
                    sweat_score = (int)(_basesweat * 2.0f);
                }
                else if (Mathf.Abs(sweat_result) < 5)
                {
                    Debug.Log("甘み: Great!!");
                    sweat_level = 6;
                    sweat_score = (int)(_basesweat * 1.5f);
                }
                else if (Mathf.Abs(sweat_result) < 15)
                {
                    Debug.Log("甘み: Well!");
                    sweat_level = 5;
                    sweat_score = (int)(_basesweat * 0.75f);
                }
                else if (Mathf.Abs(sweat_result) < 30)
                {
                    Debug.Log("甘み: Good!");
                    sweat_level = 4;
                    sweat_score = 5;
                }
                else if (Mathf.Abs(sweat_result) < 50)
                {
                    Debug.Log("甘み: Normal");
                    sweat_level = 3;
                    sweat_score = 2;
                }
                else if (Mathf.Abs(sweat_result) < 80)
                {
                    Debug.Log("甘み: poor");
                    sweat_level = 2;
                    sweat_score = -35;
                }
                else if (Mathf.Abs(sweat_result) <= 100)
                {
                    Debug.Log("甘み: death..");
                    sweat_level = 1;
                    sweat_score = -80;
                }
                else
                {
                    Debug.Log("100を超える場合はなし");
                }
            }
            Debug.Log("甘み点: " + sweat_score);

            if (_girlbitter[set_id] == 0)
            {
                Debug.Log("苦み: 判定なし");
                bitter_level = 0;
                bitter_score = 0;
            }
            else
            {
                //苦味
                if (Mathf.Abs(bitter_result) == 0)
                {
                    Debug.Log("苦味: Perfect!!");
                    bitter_level = 7;
                    bitter_score = (int)(_basebitter * 3.0f);
                }
                else if (Mathf.Abs(bitter_result) < 5)
                {
                    Debug.Log("苦味: Great!!");
                    bitter_level = 6;
                    bitter_score = (int)(_basebitter * 2.0f);
                }
                else if (Mathf.Abs(bitter_result) < 15)
                {
                    Debug.Log("苦味: Well!");
                    bitter_level = 5;
                    bitter_score = (int)(_basebitter * 0.75f);
                }
                else if (Mathf.Abs(bitter_result) < 30)
                {
                    Debug.Log("苦味: Good!");
                    bitter_level = 4;
                    bitter_score = 5;
                }
                else if (Mathf.Abs(bitter_result) < 50)
                {
                    Debug.Log("苦味: Normal");
                    bitter_level = 3;
                    bitter_score = 2;
                }
                else if (Mathf.Abs(bitter_result) < 80)
                {
                    Debug.Log("苦味: poor");
                    bitter_level = 2;
                    bitter_score = -35;
                }
                else if (Mathf.Abs(bitter_result) <= 100)
                {
                    Debug.Log("苦味: death..");
                    bitter_level = 1;
                    bitter_score = -80;
                }
                else
                {
                    Debug.Log("100を超える場合はなし");
                }
            }
            Debug.Log("苦味点: " + bitter_score);

            if (_girlsour[set_id] == 0)
            {
                Debug.Log("酸味: 判定なし");
                sour_level = 0;
                sour_score = 0;
            }
            else
            {
                //酸味
                if (Mathf.Abs(sour_result) == 0)
                {
                    Debug.Log("酸味: Perfect!!");
                    sour_level = 7;
                    sour_score = (int)(_basesour * 3.0f);
                }
                else if (Mathf.Abs(sour_result) < 5)
                {
                    Debug.Log("酸味: Great!!");
                    sour_level = 6;
                    sour_score = (int)(_basesour * 1.2f);
                }
                else if (Mathf.Abs(sour_result) < 15)
                {
                    Debug.Log("酸味: Well!");
                    sour_level = 5;
                    sour_score = (int)(_basesour * 0.75f);
                }
                else if (Mathf.Abs(sour_result) < 30)
                {
                    Debug.Log("酸味: Good!");
                    sour_level = 4;
                    sour_score = 5;
                }
                else if (Mathf.Abs(sour_result) < 50)
                {
                    Debug.Log("酸味: Normal");
                    sour_level = 3;
                    sour_score = 2;
                }
                else if (Mathf.Abs(sour_result) < 80)
                {
                    Debug.Log("酸味: poor");
                    sour_level = 2;
                    sour_score = -35;
                }
                else if (Mathf.Abs(sour_result) <= 100)
                {
                    Debug.Log("酸味: death..");
                    sour_level = 1;
                    sour_score = -80;
                }
                else
                {
                    Debug.Log("100を超える場合はなし");
                }
            }
            Debug.Log("酸味点: " + sour_score);


            //食感パラメータは、大きければ大きいほど、そのまま得点に。
            //サブジャンルごとに、比較の対象が限定される。例えば、クッキーなら、さくさく度だけを見る。
            //またジャンルごとに、どのスコアの比重が大きくなるか、補正がかかる。アイスなら甘味が大事、とか。
            switch (_baseitemtype_sub)
            {
                case "Cookie":

                    crispy_score = _basecrispy; //わかりやすく、サクサク度の数値がそのまま点数に。
                    shokukan_score = crispy_score;
                    Debug.Log("サクサク度の点: " + crispy_score);

                    break;

                case "Cake":

                    fluffy_score = _basefluffy;
                    shokukan_score = fluffy_score;
                    Debug.Log("ふわふわ度の点: " + fluffy_score);

                    break;

                case "Chocolate":

                    smooth_score = _basesmooth;
                    shokukan_score = smooth_score;
                    Debug.Log("くちどけの点: " + smooth_score);

                    break;

                case "Chocolate_Mat":

                    smooth_score = _basesmooth;
                    shokukan_score = smooth_score;
                    Debug.Log("くちどけの点: " + smooth_score);

                    break;

                case "Bread":

                    crispy_score = _basecrispy;
                    shokukan_score = crispy_score;
                    Debug.Log("サクサク度の点: " + crispy_score);

                    break;

                case "IceCream":

                    smooth_score = _basesmooth;
                    shokukan_score = smooth_score;
                    Debug.Log("くちどけの点: " + smooth_score);

                    sweat_score *= 2;

                    break;

                default:

                    crispy_score = _basecrispy; //わかりやすく、サクサク度の数値がそのまま点数に。
                    shokukan_score = crispy_score;
                    Debug.Log("サクサク度の点: " + crispy_score);
                    break;
            }

            //トッピングの値も計算する。①基本的に上がるやつ　+　②クエスト固有でさらに上がるやつ　の2点

            //①  スロットネームDBのtotal_scoreを加算する。
            for (i = 0; i < itemslotScore.Count; i++)
            {

                //0はNonなので、無視
                if (i != 0)
                {
                    //トッピングされているものに応じて、得点
                    if (itemslotScore[i] > 0)
                    {
                        topping_score += slotnamedatabase.slotname_lists[i].slot_totalScore;
                    }

                }
            }

            //②　ガールライクセットDBに設定した、各tp_scoreを加算する。（スロットが一致しているものだけに限る）
            switch (true_set)
            {

                case 0:

                    for (i = 0; i < itemslotScore.Count; i++)
                    {

                        //0はNonなので、無視　かつ、女の子のスコアが1以上
                        if (i != 0 && girl1_status.girl1_hungryScoreSet1[i] > 0)
                        {
                            //女の子のスコア(所持数)より、生成したアイテムのスロットの所持数が大きい場合は、そのトッピングが好みとマッチしている。正解
                            if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet1[i])
                            {
                                topping_score += girl1_status.girl1_hungryToppingScoreSet1[i];

                                //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。
                                topping_flag = girl1_status.girl1_hungryToppingNumberSet1[i];
                                //Debug.Log("topping_flag: " + topping_flag);
                            }
                            else
                            {
                            }
                        }

                        //Debug.Log("girl1_status.girl1_hungryScoreSet1: " + girl1_status.girl1_hungryScoreSet1[i]);
                    }
                    break;

                case 1:

                    for (i = 0; i < itemslotScore.Count; i++)
                    {
                        //①トッピングスロットの計算

                        //0はNonなので、無視
                        if (i != 0 && girl1_status.girl1_hungryScoreSet2[i] > 0)
                        {
                            //女の子のスコア(所持数)より、生成したアイテムのスロットの所持数が大きい場合は、そのトッピングが好みとマッチしている。正解
                            if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet2[i])
                            {
                                topping_score += girl1_status.girl1_hungryToppingScoreSet2[i];

                                //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。
                                topping_flag = girl1_status.girl1_hungryToppingNumberSet2[i];
                            }
                            else
                            {
                            }
                        }
                    }
                    break;

                case 2:

                    for (i = 0; i < itemslotScore.Count; i++)
                    {
                        //①トッピングスロットの計算

                        //0はNonなので、無視
                        if (i != 0 && girl1_status.girl1_hungryScoreSet3[i] > 0)
                        {
                            //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                            if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet3[i])
                            {
                                topping_score += girl1_status.girl1_hungryToppingScoreSet3[i];

                                //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。
                                topping_flag = girl1_status.girl1_hungryToppingNumberSet3[i];
                            }
                            else
                            {

                            }
                        }
                    }
                    break;

                default:

                    break;
            }


            //以上、全ての点数を合計。
            total_score = quality_score + sweat_score + bitter_score + sour_score
                + crispy_score + fluffy_score
                + smooth_score + hardness_score + jiggly_score + chewy_score
                + topping_score;

            Debug.Log("###  ###");

            Debug.Log("総合点: " + total_score);

            Debug.Log("###  ###");

            if (total_score < 0) //total_scoreが0以下でも、マズイ。
            {
                //girl1_status.girl_Mazui_flag = true;
                Mazui_flag = true;
            }
            else
            {
                Mazui_flag = false;
            }

            //得点に応じて、好感度・お金に補正がかかる。→ LoveScoreCal()で計算

            //作ったお菓子の点数が、前回より高い場合は、最高得点を更新。
            if (total_score > database.items[_baseID].last_total_score)
            {
                database.items[_baseID].last_total_score = total_score;
            }

        }
        else
        { }
    }

    void Girl_reaction()
    {
        if (dislike_flag == true) //正解の場合
        {
            switch (dislike_status)
            {
                //新しいお菓子をあげた場合の処理
                case 2:

                    if (girl1_status.hukidashiitem != null)
                    {
                        hukidashiitem = GameObject.FindWithTag("Hukidashi");
                        _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                        _hukidashitext.text = "む！今まで食べたことがないお菓子だ！！";
                    }
                    
                    //3秒ほど表示したら、お菓子の感想を言ったり、なんか褒めてくれたりする。
                    StartCoroutine("WaitCommentNewOkashiDesc");

                    break;

                default:

                    //共通
                    if (hukidashiitem == null)
                    {
                        girl1_status.hukidasiInit();
                    }

                    hukidashiitem = GameObject.FindWithTag("Hukidashi");
                    _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                    _hukidashitext.text = "お兄ちゃん！ありがとー！！";

                    //そのお菓子セットをどれだけ食べたか。回数を増やす。
                    if (girl1_status.OkashiNew_Status == 0)　//スペシャルクエストの場合
                    {
                        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
                        {
                            if (_set_compID == girlLikeCompo_database.girllike_composet[i].set_ID)
                            {
                                ++girlLikeCompo_database.girllike_composet[i].set_score;
                            }
                        }
                    }
                    else if (girl1_status.OkashiNew_Status == 1) //通常の場合
                    {
                        if (GameMgr.tutorial_ON == true) //チュートリアルモードなど回避用
                        { }
                        else
                        {
                            for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
                            {
                                if (girlLikeCompo_database.girllike_compoRandomset[_set_compID].set_ID == girlLikeCompo_database.girllike_composet[i].set_ID)
                                {
                                    ++girlLikeCompo_database.girllike_composet[i].set_score;
                                }
                            }
                        }
                    }
                    else { }

                    //スペシャルクエストだった場合は、まずいフラグをオフ
                    if (girl1_status.OkashiNew_Status == 0)
                    {
                        girl1_status.girl_Mazui_flag = false;
                    }

                    //3秒ほど表示したら、お菓子の感想を言ったり、なんか褒めてくれたりする。
                    StartCoroutine("WaitCommentDesc");
                    break;
            }
        
            
            //お菓子をたべたフラグをON + 食べた回数もカウント
            database.items[_baseID].First_eat += 1;

            //好感度とお金を計算
            LoveScoreCal();

            //お金の取得
            //moneyStatus_Controller.GetMoney(GetMoney);

            //アイテムの削除
            delete_Item();

            
            switch (dislike_status)
            {

                case 2: //新しいお菓子をあげた場合の処理

                    OkashiSaitenhyouji(); //採点パネル表示してからリザルト
                    //Okashi_Result();

                    break;

                default:

                    //スペシャルクエストお菓子をあげた場合の処理
                    if (girl1_status.OkashiNew_Status == 0)　//スペシャルクエストの場合
                    {
                        //感想を言う。その後、好感度とお金の計算
                        //kansou_on = false;
                        //StartCoroutine("Sp_Okashi_Comment");
                        OkashiSaitenhyouji(); //採点パネル表示してからリザルト
                    }
                    else //通常
                    {
                        OkashiSaitenhyouji(); //採点パネル表示してからリザルト
                        //Okashi_Result();
                    }
                    break;
            }
        }
        else //失敗の場合
        {
            if (girl1_status.hukidashiitem != null)
            {
                hukidashiitem = GameObject.FindWithTag("Hukidashi");
                _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();

            }
            else
            {
                girl1_status.hukidasiInit();

                hukidashiitem = GameObject.FindWithTag("Hukidashi");
                _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
            }

            switch (dislike_status)
            {

                case 3: //粉っぽいなど、マイナスの値が超えた。

                    Mazui_flag = true;

                    //スペシャルクエストだった場合は、まずいフラグがたつ。
                    if (girl1_status.OkashiNew_Status == 0)
                    {
                        girl1_status.girl_Mazui_flag = true;
                    }

                    hukidashiitem.GetComponent<TextController>().SetText("げろげろ..。ま、まずいよ..。兄ちゃん。");

                    //まずいときは、スコアは0点。
                    total_score = 0;

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_verysad;

                    //好感度取得+アニメーションをON
                    Getlove_exp = -10;
                    //DegHeart(Getlove_exp);

                    //アイテムの削除
                    delete_Item();

                    OkashiSaitenhyouji(); //採点パネル表示してからリザルト

                    //音を鳴らす
                    audioSource.PlayOneShot(sound2);

                    //テキストウィンドウの更新
                    exp_Controller.GirlDisLikeText(Getlove_exp);

                    break;

                case 4: //嫌いな材料が使われていた

                    Mazui_flag = true;

                    //スペシャルクエストだった場合は、まずいフラグがたつ。
                    if (girl1_status.OkashiNew_Status == 0)
                    {
                        girl1_status.girl_Mazui_flag = true;
                    }

                    hukidashiitem.GetComponent<TextController>().SetText("ぐええ..。コレ嫌いー！..。");

                    //まずいときは、スコアは0点。
                    total_score = 0;

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_verysad_close;

                    //好感度取得+アニメーションをON
                    Getlove_exp = -10;
                    //DegHeart(Getlove_exp);

                    //アイテムの削除
                    delete_Item();

                    OkashiSaitenhyouji(); //採点パネル表示してからリザルト

                    //音を鳴らす
                    audioSource.PlayOneShot(sound2);

                    //テキストウィンドウの更新
                    exp_Controller.GirlDisLikeText(Getlove_exp);

                    break;

                case 5: //吹き出しでない場合

                    hukidashiitem.GetComponent<TextController>().SetText("今はこれの気分じゃない！");

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_verysad_close;

                    //好感度は変わらず、お菓子も減りはしない。

                    //音を鳴らす
                    audioSource.PlayOneShot(sound2);

                    //テキストウィンドウの更新
                    exp_Controller.GirlNotEatText();

                    break;

                default:

                    /*
                    Mazui_flag = true;

                    //スペシャルクエストだった場合は、まずいフラグがたつ。
                    if (girl1_status.OkashiNew_Status == 0)
                    {
                        girl1_status.girl_Mazui_flag = true;
                    }

                    hukidashiitem.GetComponent<TextController>().SetText("コレ嫌いー！");

                    //まずいときは、スコアは0点。
                    total_score = 0;

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_gokigen;

                    //好感度取得
                    Getlove_exp = -10;
                    DegHeart(Getlove_exp);

                    //アイテムの削除
                    delete_Item();

                    //アニメーションをON
                    loveanim_on = true;

                    //音を鳴らす
                    audioSource.PlayOneShot(sound2);

                    //テキストウィンドウの更新
                    exp_Controller.GirlDisLikeText(Getlove_exp);
                    */
                    break;
            }
                      

            //お菓子をあげたあとの状態に移行する。残り時間を、短く設定。
            girl1_status.timeGirl_hungry_status = 2;
            girl1_status.timeOut = 5.0f;

            //お菓子判定終了
            compound_Main.girlEat_ON = false;

            //リセット＋フラグチェック
            //減る場合は、update内でちぇっく
        }
        
        compound_Main.compound_status = 0;

        girl1_status.GirlEat_Judge_on = true; //またカウントが進み始める

        //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
        if (GameMgr.tutorial_ON == true)
        {

            StartCoroutine("WaitForSeconds");  //1秒まって次へ              

        }
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1.0f);

        if (GameMgr.tutorial_Num == 105)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 110;
        }
        if (GameMgr.tutorial_Num == 285)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 290;
        }
    }


    //
    //お菓子をあげたあとの吹き出しコメント・感想メソッド
    //
    IEnumerator WaitCommentDesc()
    {
        yield return new WaitForSeconds(3.0f);

        if (hukidashiitem != null)
        {
            //ランダムで、吹き出しの内容を決定
            CommentTextInit();
            random = Random.Range(0, _commentDict.Count);
            _commentrandom = _commentDict[random];

            hukidashiitem.GetComponent<TextController>().SetText(_commentrandom);
        }

        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
    }

    IEnumerator WaitCommentNewOkashiDesc()
    {
        yield return new WaitForSeconds(3.0f);

        if (hukidashiitem != null)
        {
            hukidashiitem.GetComponent<TextController>().SetText(database.items[_baseID].itemNameHyouji + "うまいぞ！");
        }

        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
    }
    //
    //スペシャルお菓子 食べたときの感想。アニメーション終了後で発生する。
    //
    /*IEnumerator Sp_Okashi_Comment() 
    {
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();

        while (main_cam.transform.position.z != -10)
        {
            yield return null;
        }

        if (kansou_on)
        {
            //宴を呼び出す。
            character.GetComponent<FadeCharacter>().FadeImageOff();

            GameMgr.sp_okashi_ID = _set_compID; //GirlLikeCompoSetの_set_compIDが入っている。
            GameMgr.sp_okashi_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                           //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);
            while (!GameMgr.recipi_read_endflag)
            {
                yield return null;
            }

            GameMgr.recipi_read_endflag = false;

            character.GetComponent<FadeCharacter>().FadeImageOn();
            canvas.SetActive(true);

            OkashiSaitenhyouji();
        } else
        {
            canvas.SetActive(true);
            OkashiSaitenhyouji();
        }
        
    }*/

    void OkashiSaitenhyouji()
    {
        //お菓子の採点結果を表示する。　シャキーーン！！　満足度　ドンドン　わーーーぱちぱちって感じ
        ScoreHyoujiPanel.SetActive(true);
        ScoreHyouji_ON = true;
        Okashi_Score.text = total_score.ToString();

        //☆の初期化
        for (i = 0; i < 4; i++)
        {
            Manzoku_star[i].SetActive(false);
        }

        if (total_score > 0 && total_score < 30)
        {
            Delicious_Text.text = "Morte..";
            //Manzoku_Score.text = "★";
            for (i = 0; i < 1; i++)
            {                
                Manzoku_star[i].SetActive(true);
            }

            SetHintText(0); //通常得点時
            Hint_Text.text = temp_hint_text;
            //HighScore_flag = false;
        }
        else if (total_score >= 30 && total_score < 60)
        {
            Delicious_Text.text = "Bene!";
            //Manzoku_Score.text = "★★";
            for (i = 0; i < 2; i++)
            {               
                Manzoku_star[i].SetActive(true);
            }

            SetHintText(0); //通常得点時
            Hint_Text.text = temp_hint_text;
            //HighScore_flag = false;
        }
        else if (total_score >= 60 && total_score < 80)
        {
            Delicious_Text.text = "Di molto Bene!";
            //Manzoku_Score.text = "★★★";
            for (i = 0; i < 3; i++)
            {                
                Manzoku_star[i].SetActive(true);
            }

            SetHintText(0); //通常得点時
            Hint_Text.text = temp_hint_text;
            //HighScore_flag = false;
        }
        else if (total_score >= 80 && total_score < 95)
        {
            Delicious_Text.text = "Benissimo!!";
            //Manzoku_Score.text = "★★★★";
            for (i = 0; i < 4; i++)
            {                
                Manzoku_star[i].SetActive(true);
            }

            SetHintText(1); //高得点時
            Hint_Text.text = temp_hint_text;
            //HighScore_flag = true; //高得点をとれた！その場合、サブクエストが発生したり、特別なイベントが発生することもある。
            database.items[_baseID].HighScore_flag = true;
        }
        else if (total_score >= 95)
        {
            Delicious_Text.text = "Vittoria!!";
            //Manzoku_Score.text = "★★★★★";
            for (i = 0; i < 5; i++)
            {                
                Manzoku_star[i].SetActive(true);
            }

            SetHintText(1); //高得点時
            Hint_Text.text = temp_hint_text;
            //HighScore_flag = true;
            database.items[_baseID].HighScore_flag = true;
        }
        else if (total_score <= 0) //0以下。つまりまずかった
        {
            total_score = 0;
            Delicious_Text.text = "Death..";

            SetHintText(2); //マズイとき時
            Hint_Text.text = temp_hint_text;
            //HighScore_flag = false;
        }

        if (Mazui_flag)
        {
            sc.PlaySe(6); //ダウン音
        }
        else
        {
            _listScoreEffect.Clear();

            //エフェクト生成＋アニメ開始
            _listScoreEffect.Add(Instantiate(Score_effect_Prefab1));
            //_listScoreEffect.Add(Instantiate(Score_effect_Prefab2));

            //音鳴らす。
            sc.PlaySe(41); //ファンファーレ
            sc.PlaySe(42);
            sc.PlaySe(43);
        }
        
        Okashi_Result();
    }

    
    void Okashi_Result()
    {
        if (Mazui_flag)
        {
            DegHeart(Getlove_exp);

            //テキストウィンドウの更新
            exp_Controller.GirlDisLikeText(Getlove_exp);
        }
        else
        {
            //アニメーションをON。好感度パラメータの反映もここ。
            loveGetPlusAnimeON();

            //エフェクト生成＋アニメ開始
            _listEffect.Add(Instantiate(effect_Prefab));
            StartCoroutine("Love_effect");

            //好感度パラメータに応じて、実際にキャラクタからハートがでてくる量を更新
            GirlHeartEffect.LoveRateChange();

            //音を鳴らす
            audioSource.PlayOneShot(sound1);

            //テキストウィンドウの更新
            exp_Controller.GirlLikeText(Getlove_exp, GetMoney, total_score);

            //リセット
            Getlove_exp = 0;
        }               

        

        //お菓子をあげたあとの状態に移行する。
        girl1_status.timeGirl_hungry_status = 2;
        girl1_status.timeOut = 5.0f;

        //キャラクタ表情変更
        girl1_status.face_girl_Yorokobi();
        StartCoroutine("DefaultFaceChange");//2秒ぐらいしたら、表情だけすぐに戻す。

        //フラグチェック        
        compound_Main.check_GirlLoveEvent_flag = false;
       
        //その時点での点数を保持。（マズイときは、失敗フラグ＝0点）
        special_quest.special_score_record[special_quest.spquest_set_num, special_quest.special_kaisu] = total_score;

        //クエスト挑戦回数を増やす。
        special_quest.special_kaisu++;

        //もしクエスト回数が３回まできたら、判定する。通常クリアなら、次のクエストへ進行する
        if(special_quest.special_kaisu >= special_quest.special_kaisu_max)
        {
            //クエストクリア時のイベントやフラグ管理
            SelectNewOkashiSet();
        }

        //お菓子の判定処理を終了
        compound_Main.girlEat_ON = false;
    }

    IEnumerator DefaultFaceChange()
    {
        yield return new WaitForSeconds(2.0f);

        girl1_status.DefaultFace();
    }

    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for( i = 0; i < slotnamedatabase.slotname_lists.Count; i++ )
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslotScore.Add(0);
        }
            
    }

    void InitializeGirlLikeCompoScore()
    {
        girlLikeCompoScore.Clear();

        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            girlLikeCompoScore.Add(girlLikeCompo_database.girllike_composet[i].set_ID, girlLikeCompo_database.girllike_composet[i].set_score);
        }
    }

    void delete_Item()
    {
        switch (_toggle_type1)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                pitemlist.deletePlayerItem(kettei_item1, 1);
                break;

            case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                pitemlist.deleteOriginalItem(kettei_item1, 1);
                break;

            default:
                break;
        }

        //エクストリームアイテムのほうも、空にする。
        extreme_panel.deleteExtreme_Item();

    }

    //
    //取得するお金と好感度の計算処理のメソッド
    //
    void LoveScoreCal()
    {
        
        slot_girlscore = 0;
        slot_money = 0;

        //スロットの計算。該当するスロット効果があれば、それを得点にする。スロットの好感度への影響は、トータルスコアのほうで計算するので、こっちでは未使用にした。
        for ( i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            while (count < slotnamedatabase.slotname_lists.Count)
            {
                if(_basetp[i] == slotnamedatabase.slotname_lists[count].slotName)
                {                   
                    slot_girlscore += slotnamedatabase.slotname_lists[count].slot_girlScore; //未使用
                    slot_money += slotnamedatabase.slotname_lists[count].slot_Money;
                    break;
                }
                count++;
            }
        }

        //前に計算したトータルスコアを元に計算。

        switch (dislike_status)
        {

            case 2: //吹き出しと違うけど、新しいお菓子をあげた場合の処理。今は使ってない。

                //好感度取得

                Getlove_exp = (int)(_basegirl1_like * 5.0f);

                Debug.Log("取得好感度: " + Getlove_exp);

                //お金の取得

                GetMoney = _basecost + total_score + slot_money;

                Debug.Log("取得お金: " + GetMoney);
                break;


            default: //通常

                if (Mazui_flag)
                {
                    Getlove_exp = -10;
                    Debug.Log("取得好感度: " + Getlove_exp);

                    GetMoney = 0;
                    Debug.Log("取得お金: " + GetMoney);
                }
                else
                {
                    //好感度取得
                    if (total_score >= 0 && total_score < 10) //問答無用で１
                    {
                        Getlove_exp = 1;
                    }
                    else if (total_score >= 10 && total_score < 45) //ベース×5
                    {
                        Getlove_exp = (int)(_basegirl1_like * 5.0f);
                    }
                    else if (total_score >= 45 && total_score < 60) //ベース×5
                    {
                        Getlove_exp = (int)(_basegirl1_like * 7.0f);
                    }
                    else if (total_score >= 60 && total_score < 80) //ベース×10
                    {
                        Getlove_exp = (int)(_basegirl1_like * 10.0f);
                    }
                    else if (total_score >= 80 && total_score < 100) //ベース×15
                    {
                        Getlove_exp = (int)(_basegirl1_like * 15.0f);
                    }
                    else if (total_score >= 100) //100点を超えた場合、ベース×25
                    {
                        Getlove_exp = (int)(_basegirl1_like * 25.0f);
                    }

                    //トッピングの値も加算する。
                    for (i = 0; i < itemslotScore.Count; i++)
                    {
                        //0はNonなので、無視
                        if (i != 0)
                        {
                            if (itemslotScore[i] > 0)
                            {
                                Getlove_exp += slotnamedatabase.slotname_lists[i].slot_girlScore;
                            }
                        }
                    }

                    //そのお菓子を食べた回数で割り算。
                    /*if (database.items[_baseID].First_eat == 0)
                    {
                        database.items[_baseID].First_eat = 1; //0で割り算を回避。
                    }
                    Getlove_exp /= database.items[_baseID].First_eat;
                    if (Getlove_exp <= 1) { Getlove_exp = 1; }*/
                    Debug.Log("取得好感度: " + Getlove_exp);

                    //お金の取得
                    if (total_score >= 0 && total_score < 15)
                    {
                        GetMoney = (int)(_basecost * 0.5f) + (int)(total_score * 0.3f) + slot_money;
                    }
                    else if (total_score >= 15 && total_score < 45)
                    {
                        GetMoney = (int)(_basecost * 1.0f) + (int)(total_score * 0.3f) + slot_money;
                    }
                    else if (total_score >= 45 && total_score < 60)
                    {
                        GetMoney = (int)(_basecost * 1.5f) + (int)(total_score * 0.3f) + slot_money;
                    }
                    else if (total_score >= 60 && total_score < 80)
                    {
                        GetMoney = (int)(_basecost * 2.0f) + (int)(total_score * 0.3f) + slot_money;
                    }
                    else if (total_score >= 80 && total_score < 100)
                    {
                        GetMoney = (int)(_basecost * 5.0f) + (int)(total_score * 0.3f) + slot_money;
                    }
                    else if (total_score >= 100) //100点を超えた場合、2倍程度増加
                    {
                        GetMoney = (_basecost + slot_money) + (int)(total_score * 0.6f) * 10;
                    }
                    Debug.Log("取得お金: " + GetMoney);
                }
                break;
        }
    }



    //
    //好感度の値反映処理のメソッド
    //

    IEnumerator Love_effect()
    {
        //10秒待つ
        yield return new WaitForSeconds(10);

        //初期化しておく
        for (i = 0; i < _listEffect.Count; i++)
        {
            Destroy(_listEffect[i]);
        }
        _listEffect.Clear();
    }

    void loveGetPlusAnimeON()
    {
        _listHeart.Clear();
        heart_count = Getlove_exp;
        //Debug.Log("heart_count: " + heart_count);

        //ハートのインスタンスを、獲得好感度分だけ生成する。
        for (i = 0; i < heart_count; i++)
        {
            _listHeart.Add(Instantiate(heart_Prefab, _slider_obj.transform));
            _listHeart[i].GetComponent<HeartUpObj>()._id = i;
        }

        //好感度　取得分増加
        girl1_status.girl1_Love_exp += Getlove_exp;        
    }

    //ハートがゲージに衝突した時に、このメソッドが呼び出される。
    public void GetHeartValue()
    {

        //スライダにも反映
        _slider.value++;

        //現在のスライダ上限に好感度が達したら、次のレベルへ。
        if(_slider.value >= _slider.maxValue)
        {
            girl1_status.girl1_Love_lv++;
            _slider.value = 0;

            //Maxバリューを再設定
            Love_Slider_Setting();

            girl_lv.text = girl1_status.girl1_Love_lv.ToString();
        }
        
        //エフェクト
        _listHeartHit.Add(Instantiate(hearthit_Prefab, _slider_obj.transform.Find("Panel").gameObject.transform));
        _listHeartHit2.Add(Instantiate(hearthit2_Prefab, _slider_obj.transform.Find("Panel").gameObject.transform));

    }

    //好感度が下がるときの処理。外部からアクセス用。ゲージにも反映される。
    public void DegHeart(int _param)
    {
        //好感度取得
        Getlove_exp = _param;

        //アニメーションをON
        loveanim_on = true;
    }

    void Love_Slider_Setting()
    {
        if (girl1_status.girl1_Love_lv <= 5)
        {
            _slider.maxValue = stage_levelTable[girl1_status.girl1_Love_lv - 1]; //レベルは１始まりなので、配列番号になおすため、-1してる
        }
        else //5以上は、現状、同じ数値
        {
            _slider.maxValue = stage_levelTable[stage_levelTable.Count - 1];
        }
    }



    //
    //次の食べたいお菓子を決めるメソッド。
    //
    void SelectNewOkashiSet()
    {

        //3回のお菓子終了・クエストクリアかどうかを判定        

        //判定
        HighScore_flag = false;
        Gameover_flag = true;

        for (i=0; i < special_quest.special_score_record.GetLength(1); i++)
        {
            if(special_quest.special_score_record[special_quest.spquest_set_num, i] < 85 || 
                special_quest.special_score_record[special_quest.spquest_set_num, i] >= 60 ) //60点~85点でノーマル合格
            {
                Gameover_flag = false;
            }
            if (special_quest.special_score_record[special_quest.spquest_set_num, i] >= 85) //85点以上でハイスコア合格
            {
                HighScore_flag = true;
                Gameover_flag = false;
            }
            else //60未満だとゲームオーバーフラグ
            {
                Debug.Log("ゲームオーバーフラグ ON");                
                //ゲーム終了　ぐええ
            }
        }

        //初期化
        girl1_status.OkashiNew_Status = 1; //クエストクリアで、1に戻す。0にすると、次のクエストが開始する。（スペシャル吹き出し登場する）
        subQuestClear_check = true;
        special_quest.special_kaisu = 0;       
        girl1_status.special_animatFirst = false;

        //その他、通常の状態で、何らかの条件を満たした場合。現在未使用。

        //点数をまず初期化
        InitializeGirlLikeCompoScore();

    }





    //
    //スコア表示パネルを押したらこのメソッドが呼び出し
    //
    public void ResultPanel_On()
    {
        ScoreHyoujiPanel.SetActive(false);

        //それじゃあ、兄ちゃん。クッキーの採点をするね！といった画面と演出

        if (subQuestClear_check)
        {
            if (!Gameover_flag)
            {
                switch (girl1_status.OkashiQuest_ID)
                {
                    case 1000: //オリジナルクッキー

                        if (GameMgr.OkashiQuest_flag[0] != true) //まだクエストクリアフラグがたってない
                        {
                            GameMgr.OkashiQuest_flag[0] = true;

                            if (!HighScore_flag) //通常クリア
                            {
                                ClearFlagOn();

                                _mainquest_name = "出来たて　オリジナルクッキー！";

                                MainQuestText.text = _mainquest_name;
                                _set_MainQuestID = 1000;
                                StartCoroutine("SubQuestClearEvent");
                            }
                            else //さらに高得点だったら、特別なイベントや報酬などが発生
                            {
                                ClearFlagOn();

                                _mainquest_name = "うまいぞ！　兄ちゃんのクッキー！";

                                MainQuestText.text = _mainquest_name;
                                _set_MainQuestID = 1001;
                                StartCoroutine("SubQuestClearEvent");
                            }
                        }

                        break;

                    case 1010: //ラスク

                        if (GameMgr.OkashiQuest_flag[1] != true) //まだクエストクリアフラグがたってない
                        {
                            GameMgr.OkashiQuest_flag[1] = true;

                            if (!HighScore_flag) //通常クリア
                            {
                                ClearFlagOn();

                                _mainquest_name = "カリカリラスクマン";

                                MainQuestText.text = _mainquest_name;
                                _set_MainQuestID = 1010;
                                StartCoroutine("SubQuestClearEvent");
                            }
                            else //さらに高得点だったら、特別なイベントや報酬などが発生
                            {
                                ClearFlagOn();
                                
                                _mainquest_name = "ラスクとありんこ";

                                MainQuestText.text = _mainquest_name;
                                _set_MainQuestID = 1011;
                                StartCoroutine("SubQuestClearEvent");
                            }
                        }

                        break;

                    default:
                        break;
                }
            }
            else
            {
                //ゲームオーバー画面を表示
                Debug.Log("ゲームオーバー画面表示");
                ResultOFF();
            }
        }
        else
        {
            //クエストまだクリアでなければ、お菓子の感想を表示する。
            //_set_MainQuestID = girl1_status.OkashiQuest_ID;
            //StartCoroutine("OkashiAfter_Comment");

            ResultOFF();
        }      
    }

    //そのクエストをクリアしたフラグをONにする。
    void ClearFlagOn()
    {
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                girlLikeCompo_database.girllike_composet[i].clearFlag = true; //クリアした
            }
        }
    }

    IEnumerator OkashiAfter_Comment()
    {
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();

        //宴を呼び出す。
        character.GetComponent<FadeCharacter>().FadeImageOff();

        GameMgr.okashiafter_ID = _set_MainQuestID; //GirlLikeCompoSetの_set_compIDが入っている。
        GameMgr.okashiafter_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                       //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);
        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        character.GetComponent<FadeCharacter>().FadeImageOn();
        canvas.SetActive(true);

        ResultOFF();

    }

    IEnumerator SubQuestClearEvent()
    {
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();
        sceneBGM.MuteBGM();

        character.GetComponent<FadeCharacter>().FadeImageOff();

        GameMgr.scenario_ON = true;
        GameMgr.mainquest_ID = _set_MainQuestID; //GirlLikeCompoSetの_set_compIDが入っている。
        GameMgr.mainClear_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        sceneBGM.MuteOFFBGM();

        character.GetComponent<FadeCharacter>().FadeImageOn();
        canvas.SetActive(true);

        //表示の音を鳴らす。
        sc.PlaySe(25);

        MainQuestOKPanel.SetActive(true);
    }

    public void ResultOFF()
    {
        MainQuestOKPanel.SetActive(false);

        subQuestClear_check = false;

        girl1_status.GirlEat_Judge_on = true;
        girl1_status.hukidasiOn();
        touch_controller.Touch_OnAllON();

        sc.PlaySe(18);

        //初期化
        for (i = 0; i < _listScoreEffect.Count; i++)
        {
            Destroy(_listScoreEffect[i]);
        }
        _listScoreEffect.Clear();

        ScoreHyouji_ON = false;
        GameMgr.scenario_ON = false;
        compound_Main.check_GirlLoveEvent_flag = false;
    }

    void SetHintText(int _status)
    {
        
        //ヒントを表示するか否か。
        switch (GameMgr.stage_number)
        {
            case 1:

                SweatHintHyouji();
                break;

            default:

                SweatHintHyouji();
                BitterHintHyouji();
                SourHintHyouji();
                ShokukanHintHyouji();
                break;
        }

        //トータルスコアが低いときは、そのクスエトをクリアに必要な固有のヒントをくれる。（クッキーのときは、「もっとかわいくして！」とか。妹が好みのものを伝えてくる。）
        if (total_score < 60)
        {
            for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
            {
                if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
                {
                    _temp_spkansou = girlLikeCompo_database.girllike_composet[i].hint_text;
                }
            }
            _special_kansou = "\n" + _temp_spkansou;
        }
        else if (total_score >= 60)
        {
            _special_kansou = "\n" + "";
        }

        //感想の表示　_statusが0なら、通常得点、1なら、高得点時（80点以上）の感想。2は、まずすぎたとき。
        switch (_status)
        {

            case 0:

                temp_hint_text = _sweat_kansou + _bitter_kansou + _sour_kansou + _shokukan_kansou + _special_kansou;                
                break;

            case 1:

                temp_hint_text = "兄ちゃん！" + "\n" + "この" + _basenameHyouji + "うますぎィ！" + "\n" + "最高！！";
                break;

            case 2:

                temp_hint_text = "マズすぎるぅ..。";
                break;

            default:
                break;
        }

        database.items[_baseID].last_hinttext = temp_hint_text;

        _result_text = "好感度が " + Getlove_exp  + " アップ！　"; //"お金を " + GetMoney + "G ゲットした！"
        Result_Text.text = _result_text;
    }

    void SweatHintHyouji()
    {
        //甘さがどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (sweat_level == 7)
        {
            _sweat_kansou = "とても甘くて良い！";
        }
        else if (sweat_level == 6)
        {
            _sweat_kansou = "甘さ、結構いい感じ！";
        }
        else if (sweat_level == 4 || sweat_level == 5)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = "もうちょっと甘さがほしい";
            }
            else
            {
                _sweat_kansou = "もうちょっと甘さが足りない";
            }
        }
        else if (sweat_level >= 1 && sweat_level <= 3)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = "甘さが足りない";
            }
            else
            {
                _sweat_kansou = "甘すぎ";
            }
        }
        else
        {
            _sweat_kansou = "";
        }
    }

    void BitterHintHyouji()
    {
        //苦さがどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (bitter_level == 7)
        {
            _bitter_kansou = "\n" + "大人な苦さ！";
        }
        else if (bitter_level == 6)
        {
            _bitter_kansou = "\n" + "苦み、ほどよくいい感じ";
        }
        else if (bitter_level == 4 || bitter_level == 5)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = "\n" + "もうちょっと苦さがほしい";
            }
            else
            {
                _bitter_kansou = "\n" + "ちょっと苦いかも";
            }

        }
        else if (bitter_level >= 1 && bitter_level <= 3)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = _bitter_kansou = "\n" + "苦さが足りない";
            }
            else
            {
                _bitter_kansou = "\n" + "苦すぎ..。";
            }

        }
        else
        {
            _bitter_kansou = "";
        }
    }

    void SourHintHyouji()
    {
        //酸味がどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (sour_level == 7)
        {
            _sour_kansou = "\n" + "絶妙なすっぱさ！";
        }
        else if (sour_level == 6)
        {
            _sour_kansou = "\n" + "いいすっぱみ！";
        }
        else if (sour_level == 4 || sour_level == 5)
        {
            if (sour_result < 0)
            {
                _sour_kansou = "\n" + "もうちょっと酸っぱさほしい";
            }
            else
            {
                _sour_kansou = "\n" + "ちょっと酸っぱすぎるかも";
            }

        }
        else if (sour_level >= 1 && sour_level <= 3)
        {
            if (sour_result < 0)
            {
                _sour_kansou = _sour_kansou = "\n" + "酸っぱさがほしい";
            }
            else
            {
                _sour_kansou = "\n" + "酸っぺぇ..。";
            }

        }
        else
        {
            _sour_kansou = "";
        }
    }

    void ShokukanHintHyouji()
    {
        //食感に関するヒント
        if (shokukan_score >= 0 && shokukan_score < 40) //
        {
            _shokukan_kansou = "\n" + "さくさく感がもっとほしい";
        }
        else if (shokukan_score >= 40 && shokukan_score < 60) //
        {
            _shokukan_kansou = "\n" + "さくさく感がほしい";
        }
        else if (shokukan_score >= 60 && shokukan_score < 80) //
        {
            _shokukan_kansou = "";
        }
        else if (shokukan_score >= 80) //
        {
            _shokukan_kansou = "";
        }
    }


    void CommentTextInit()
    {
        _commentDict.Clear();

        switch (girl1_status.GirlGokigenStatus)
        {
            case 0:

                _commentDict.Add("..。");
                _commentDict.Add("さくさく・・。");
                break;

            case 1:

                _commentDict.Add(".. ..。");
                _commentDict.Add("..おいしい。");
                break;

            case 2:

                _commentDict.Add("兄ちゃん、おじょうず..。");
                _commentDict.Add("あま～。兄ちゃん、これおいしい。");
                _commentDict.Add("さくさく・・。うまうま・・。");
                break;

            case 3:

                _commentDict.Add("兄ちゃん、このおかしうめぇ。");
                _commentDict.Add("うみゃ～♪");
                _commentDict.Add("さくさく。うまうま。");
                break;

            case 4:

                _commentDict.Add("腕をあげたねぇ、お兄ちゃん。");
                _commentDict.Add("うまいな！");
                _commentDict.Add("さくさく。うまうま。かりかり。");
                break;

            case 5:

                _commentDict.Add("お兄ちゃん！これ最高！");
                _commentDict.Add("安定のおあじ..");
                _commentDict.Add("こころがポカポカ。");
                _commentDict.Add("あたたか～い");
                break;
        }
        
    }
}

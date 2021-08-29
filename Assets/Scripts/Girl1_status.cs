using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using DG.Tweening;

public class Girl1_status : SingletonMonoBehaviour<Girl1_status>
{
    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private Touch_Controller touch_controller; //タッチのONOFFのみのスクリプト
    private Touch_Controll touch_controll; //タッチした際のメソッドを記述

    private SoundController sc;

    private Text questname;
    private GameObject questtitle_panel;

    private Sequence sequence_girlmove;
    private Sequence sequence_girlmove2;

    public float timeOut;  //girleat_judgeから読んでいる
    public float timeOut2; //その他は、デバッグ用に外側からすぐ見れるようにpublicにしてる。
    public float timeOut3;
    public float timeOutMoveX;
    public float timeOutHeartDeg;
    private float Default_hungry_cooltime;
    public int timeGirl_hungry_status; //今、お腹が空いているか、空いてないかの状態
    public int touchGirl_status; //今、どこを触っているかの番号

    public bool GirlEat_Judge_on;
    public int GirlGokigenStatus; //女の子の現在のご機嫌の状態。6段階ほどあり、好感度が上がるにつれて、だんだん見た目が元気になっていく。
    public int GirlOishiso_Status; //食べたあとの、「おいしそ～」の状態。この状態では、アイドルモーションが少し変化する。

    private GameObject text_area;

    private GameObject hukidashiPrefab;
    private GameObject canvas;

    public GameObject hukidashiitem;
    private bool hukidashion;
    private Text _text;

    private int _temp_status;

    private GameObject MoneyStatus_Panel_obj;
    private GameObject Extremepanel_obj;

    private List<string> _touchhead_comment_lib = new List<string>();
    private string _touchhead_comment;
    private List<string> _touchface_comment_lib = new List<string>();
    private string _touchface_comment;
    private List<string> _touchchest_comment_lib = new List<string>();
    private string _touchchest_comment;
    private List<string> _touchhand_comment_lib = new List<string>();
    private string _touchhand_comment;
    private List<string> _touchtwintail_comment_lib = new List<string>();
    private string _touchtwintail_comment;

    private string MazuiHintComment;
    private int MazuiStatus;
    private int touchhint_num;

    public bool WaitHint_on;
    public float timeOutHint;
    private string _hint1;
    private string _hintrandom;
    private List<string> _hintrandomDict = new List<string>();

    private BGM sceneBGM;
    private Map_Ambience map_ambience;

    //女の子の好み値。この値と、選択したアイテムの数値を比較し、近いほど得点があがる。
    public int girl1_Quality;

    public int[] girl1_Rich;
    public int[] girl1_Sweat;
    public int[] girl1_Sour;
    public int[] girl1_Bitter;

    public int[] girl1_Crispy;
    public int[] girl1_Fluffy;
    public int[] girl1_Smooth;
    public int[] girl1_Hardness;
    public int[] girl1_Chewy;
    public int[] girl1_Jiggly;
    public int[] girl1_Juice;

    public int[] girl1_Beauty;

    //マイナスとなる要素。これは、お菓子の種類は関係なく、この数値を超えると、嫌がられる。
    public int girl1_Powdery;
    public int girl1_Oily;
    public int girl1_Watery;

    public string[] girl1_likeSubtype;
    private string girl1_Subtype1_hyouji;

    public string[] girl1_likeOkashi;

    private string[] girllike_desc;
    private string _desc;
    public int[] girllike_comment_flag;
    public int[] girllike_judgeNum;

    public int[] girl1_like_set_score;
    public int[] girl1_NonToppingScoreSet;

    public int youso_count; //GirlEat_judgeでも、パラメータ初期化の際使う。
    public int Set_Count;   

    public bool girl_comment_flag; //女の子が感想をいうときに、宴をON/OFFにするフラグ
    public bool girl_comment_endflag; //感想を全て言い終えたフラグ

    public bool girl_Mazui_flag; //一度目にまずいお菓子を食べたときのフラグ。このフラグがたつと、女の子の口を押したときに次のヒントを教えてもらえる。

    private bool special_animstart_flag;
    private bool special_animstart_endflag;
    private int special_animstart_status;
    private float special_timeOut;
    public bool special_animatFirst;

    private int i, j, count;
    private int index;
    private int setID;
    private int _compID;
    private bool isRunning;
    private bool isRunning2;

    public bool facemotion_start; //Live2DAnimationTriggerからも読んでいる
    public bool facemotion_init; //一度だけの更新用
    public bool tween_start;
    private float facemotion_duration;
    private float facemotion_length;
    private float facemotion_time;   
    private Tween weightTween;
    private float facemotion_weight;
    private float Idle_duration;
    public bool IdleChangeTemp;

    private float rnd;
    private int random;
    private bool IdleMotionStart;

    public bool heartUP_facechange;

    //ランダムで変化する、女の子が今食べたいお菓子のテーブル
    public List<string> girl1_hungryInfo = new List<string>();

    public List<int> girl1_hungryScoreSet1 = new List<int>();           //①食べたいトッピングスロットのリスト　所持数。
    public List<int> girl1_hungryScoreSet2 = new List<int>();
    public List<int> girl1_hungryScoreSet3 = new List<int>();
    public List<int> girl1_hungryToppingScoreSet1 = new List<int>();    //②そのトッピングがのっているときの、点数。採点に反映される。
    public List<int> girl1_hungryToppingScoreSet2 = new List<int>();
    public List<int> girl1_hungryToppingScoreSet3 = new List<int>();
    public List<int> girl1_hungryToppingNumberSet1 = new List<int>();    //③トッピングスロットの番号。左から1~5で指定。
    public List<int> girl1_hungryToppingNumberSet2 = new List<int>();
    public List<int> girl1_hungryToppingNumberSet3 = new List<int>();

                                                                        //下３つは、上記セットを計算する用の、テンプデータ
    public List<int> girl1_hungrySet = new List<int>();                 //①食べたいトッピングスロットのリスト　所持数。
    public List<int> girl1_hungrytoppingSet = new List<int>();          //②そのトッピングがのっているときの、点数。採点に反映される。
    public List<int> girl1_hungrytoppingNumberSet = new List<int>();    //③トッピングスロットの番号。左から1~5で指定。


    //女の子の好み組み合わせセットのデータ
    public int Set_compID;
    private int set1_ID;
    private int set2_ID;
    private int set3_ID;

    private List<int> set_ID = new List<int>();

    //女の子イラストデータ
    public Sprite Girl1_img_normal;
    public Sprite Girl1_img_gokigen;
    public Sprite Girl1_img_eat_start;
    public Sprite Girl1_img_smile;
    public Sprite Girl1_img_verysad;
    public Sprite Girl1_img_verysad_close;
    public Sprite Girl1_img_hirameki;
    public Sprite Girl1_img_tereru;
    public Sprite Girl1_img_angry;
    public Sprite Girl1_img_iya;

    //女の子タッチのカウント
    public bool Girl1_touchhair_start;
    public int Girl1_touchhair_status; //髪の毛をなでてあげることで、一時的に機嫌がよくなる。
    private int Girl1_touchhair_count; //一定時間内に触った回数
    public bool Girl1_touch_end; //タッチ終了時に送る信号

    public bool Girl1_touchtwintail_start;
    private int Girl1_touchtwintail_count;
    private bool Girl1_touchtwintail_flag; //全ての会話を表示したら、しばらく触れなくなる

    public bool Girl1_touchchest_start;
    public bool Girl1_touchhand_start;
    public bool Girl1_touchribbon_start;

    public bool touchanim_start; //タッチしはじめたら、その他のモーションなどを一時的に止める。

    //歩きスタート
    public bool Walk_Start;

    //特定のお菓子か、ランダムから選ぶかのフラグ
    public int OkashiNew_Status;
    public int OkashiQuest_ID; //特定のお菓子、のお菓子セットのID
    public string OkashiQuest_Name; //そのときのお菓子のクエストネーム
    public string OkashiQuest_Number; //そのときのお菓子のクエスト番号　文字列で直接表示
    public int OkashiQuest_AllCount; //そのステージ内のクエスト総数
    public int OkashiQuest_Count; //そのステージ内のクエストの番号
    public int Special_ignore_count; //スペシャルを無視した場合、カウント。3回あたりをこえると、スペシャルは無視される。
    public bool HukidashiFlag; //イベント中などでは、吹き出しを一時的にださない。主に、Live2DAnimationTrigger用に使用。

    //エフェクト関係
    private GameObject Emo_effect_Prefab1;
    private GameObject Emo_effect_Prefab2;
    private GameObject Emo_effect_Prefab3;
    private List<GameObject> _listEffect = new List<GameObject>();
    private GameObject character;

    //Live2Dモデルの取得
    private GameObject _model_obj;
    private CubismModel _model;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;
    private int trans_facemotion;
    private GameObject character_root;
    private GameObject character_move;

    //ハートレベルのテーブル
    public List<int> stage1_lvTable = new List<int>();

    private int _sum;
    private int _noweat_count;

    private Text girl_param;
    private Slider _slider; //好感度バーを取得

    public bool gireleat_start_flag; //食べ始めアニメ開始のスイッチ

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        
        girl_comment_flag = false;
        girl_comment_endflag = false;
        HukidashiFlag = true;
        gireleat_start_flag = false;
        _desc = "";

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();
       
        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();        

        //女の子の顔を触った時のヒントライブラリー初期化
        Init_touchFaceComment();
        Init_touchTwintailComment();

        //好感度レベルのテーブル初期化
        Init_Stage1_LVTable();         

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //カメラの取得
                main_cam = Camera.main;
                maincam_animator = main_cam.GetComponent<Animator>();
                trans = maincam_animator.GetInteger("trans");

                //Live2Dモデルの取得
                _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
                _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                live2d_animator = _model.GetComponent<Animator>();
                /*trans_expression = live2d_animator.GetInteger("trans_expression");*/
                character_root = GameObject.FindWithTag("CharacterRoot").gameObject;
                character_move = character_root.transform.Find("CharacterMove").gameObject;
                character = GameObject.FindWithTag("Character");

                //初期表情の設定
                CheckGokigen();
                DefaultFace();

                //エクストリームパネルの取得
                Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");

                //お金の増減用パネルの取得
                MoneyStatus_Panel_obj = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").gameObject;

                //タッチ判定オブジェクトの取得
                touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();
                touch_controll = character.GetComponent<Touch_Controll>();

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
                map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                girl_param = canvas.transform.Find("MainUIPanel/Girl_love_exp_bar").transform.Find("Girllove_param").GetComponent<Text>();
                _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();

                //メイン画面に表示する、現在のクエスト
                questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();
                questtitle_panel = canvas.transform.Find("QuestTitlePanel").gameObject;
                questtitle_panel.SetActive(false);

                break;
        }
       
        // *** パラメータ初期設定 ***

        youso_count = 3; //配列3のサイズ
        Set_Count = 1;   //デフォルトで１。

        //女の子の好み。初期化。甘さ・苦さ・酸味は近いものほど高得点。
        girl1_Rich = new int[youso_count];
        girl1_Sweat = new int[youso_count];
        girl1_Sour = new int[youso_count];
        girl1_Bitter = new int[youso_count];

        girl1_Crispy = new int[youso_count];
        girl1_Fluffy = new int[youso_count];
        girl1_Smooth = new int[youso_count];
        girl1_Hardness = new int[youso_count];
        girl1_Chewy = new int[youso_count];
        girl1_Jiggly = new int[youso_count];
        girl1_Juice = new int[youso_count];

        girl1_Beauty = new int[youso_count];

        girl1_like_set_score = new int[youso_count];
        girl1_NonToppingScoreSet = new int[youso_count];

        girl1_likeSubtype = new string[youso_count];
        girl1_likeOkashi = new string[youso_count];
        girllike_desc = new string[youso_count];
        girllike_comment_flag = new int[youso_count];

        girllike_judgeNum = new int[youso_count];
        
        //デフォルトステータスを設定
        ResetDefaultStatus();

        // *** ここまで *** 

        //エフェクトプレファブの取得
        Emo_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Emo_Hirameki_Anim");
        Emo_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Emo_Kirari_Anim");
        Emo_effect_Prefab3 = (GameObject)Resources.Load("Prefabs/Emo_Angry_Anim");
       
    }

    //ゲームのはじめなどで一度だけリセットされる項目。
    public void ResetDefaultStatus()
    {
        //この時間ごとに、女の子は、お菓子を欲しがり始める。
        Default_hungry_cooltime = 0.5f;
        timeOut = Default_hungry_cooltime;
        timeOut2 = 10.0f;
        timeOutMoveX = 7.0f;
        timeOutHeartDeg = 5.0f;
        timeGirl_hungry_status = 1;

        GirlGokigenStatus = 0;
        GirlOishiso_Status = 0;
        OkashiNew_Status = 1;
        Special_ignore_count = 0;
        special_animatFirst = false;

        tween_start = false;
        facemotion_start = false;
        facemotion_time = 0.3f;
        facemotion_init = false;
        facemotion_weight = 0f;
        IdleChangeTemp = false;
        IdleMotionStart = false;
        heartUP_facechange = false;

        GirlEat_Judge_on = true;
        WaitHint_on = false;
        timeOutHint = 5.0f;

        special_animstart_flag = false;
        special_animstart_endflag = false;
        special_animstart_status = 0;
        special_timeOut = 3.0f;
        

        MazuiStatus = 0;

        touchGirl_status = 0;
        Girl1_touch_end = false;

        Girl1_touchhair_start = false;      
        Girl1_touchhair_count = 0;
        Girl1_touchhair_status = 0;

        Girl1_touchtwintail_start = false;
        Girl1_touchtwintail_count = 0;
        Girl1_touchtwintail_flag = false;

        Girl1_touchchest_start = false;
        Girl1_touchhand_start = false;
        Girl1_touchribbon_start = false;

        touchanim_start = false;

        Walk_Start = true; //歩きフラグをON

        girl_Mazui_flag = false;


        //ステージごとに、女の子が食べたいお菓子のセットを初期化
        InitializeStageGirlHungrySet(0, 0); //とりあえず0で初期化

    }

    // Update is called once per frame
    void Update () {

        //シーン移動の際、破壊されてしまうオブジェクトは、毎回初期化
        if( canvas == null )
        {
            canvas = GameObject.FindWithTag("Canvas");
            facemotion_start = false;
            IdleChangeTemp = false;

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    //カメラの取得
                    main_cam = Camera.main;
                    maincam_animator = main_cam.GetComponent<Animator>();
                    trans = maincam_animator.GetInteger("trans");

                    compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                    compound_Main = compound_Main_obj.GetComponent<Compound_Main>();                    

                    //テキストエリアの取得
                    text_area = canvas.transform.Find("MessageWindow").gameObject;

                    //エクストリームパネルの取得
                    Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");

                    //お金の増減用パネルの取得
                    MoneyStatus_Panel_obj = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").gameObject;

                    //BGMの取得
                    sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
                    map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();

                    //Live2Dモデルの取得
                    _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
                    _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                    live2d_animator = _model.GetComponent<Animator>();
                    character_root = GameObject.FindWithTag("CharacterRoot").gameObject;
                    character_move = character_root.transform.Find("CharacterMove").gameObject;
                    character = GameObject.FindWithTag("Character");

                    //タッチ判定オブジェクトの取得
                    touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();
                    touch_controll = character.GetComponent<Touch_Controll>();

                    //メイン画面に表示する、現在のクエスト
                    questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();
                    questtitle_panel = canvas.transform.Find("QuestTitlePanel").gameObject;
                    questtitle_panel.SetActive(false);

                    //初期表情の設定
                    CheckGokigen();
                    DefaultFace();

                    //タイマーをリセット
                    timeOut = Default_hungry_cooltime;
                    timeOut2 = 10.0f;
                    GirlEat_Judge_on = true;

                    HukidashiFlag = true;
                    gireleat_start_flag = false;

                    GirlOishiso_Status = 0; //シーン移動でも、おいしそ～状態はリセット
                    _noweat_count = 0;
                    hukidashion = false;

                    break;

                case "Shop":

                    //カメラの取得
                    main_cam = Camera.main;
                    maincam_animator = main_cam.GetComponent<Animator>();
                    trans = maincam_animator.GetInteger("trans");

                    GirlEat_Judge_on = false;

                    break;
            }
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":
                //女の子の今のご機嫌チェック
                CheckGokigen();

                //trueだと腹減りカウントが進む。
                if (GirlEat_Judge_on == true)
                {
                    timeOut -= Time.deltaTime;
                    timeOut2 -= Time.deltaTime;
                    timeOutHeartDeg -= Time.deltaTime;
                    timeOutMoveX -= Time.deltaTime;

                }

                if (WaitHint_on) //吹き出しを表示中
                {
                    timeOutHint -= Time.deltaTime;

                    if (timeOutHint <= 0.0f)
                    {
                        //吹き出しが残っていたら、削除。
                        if (hukidashiitem != null)
                        {
                            DeleteHukidashi();
                        }

                        WaitHint_on = false;
                        GirlEat_Judge_on = true;
                        Girl1_touchtwintail_count = 0;

                        if (GirlOishiso_Status != 0) //成功　もしくはしっぱい
                        {
                            GirlOishiso_Status = 0; //またおいしそ～状態から戻る。
                            DefaultFace();
                        }
                    }
                }
                break;
        }

        if (hukidashiPrefab == null)
        {
            //Prefab内の、コンテンツ要素を取得       
            hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");
        }


        if (GameMgr.scenario_ON == true) //宴シナリオを読み中は、腹減りカウントしない。
        {

        }
        else { 

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    if (compound_Main.compound_status == 110) //トップ画面のときだけ発動
                    {
                        //一定時間たつと、女の子はお腹がへって、お菓子を欲しがる。
                        if (timeOut <= 0.0)
                        {
                            switch (timeGirl_hungry_status)
                            {
                                //timeGirl_hungry_status = 0: 満腹
                                //timeGirl_hungry_status = 1: 腹減った
                                //timeGirl_hungry_status = 2: あげた直後

                                case 0: //満腹状態のとき

                                    timeGirl_hungry_status = 1; //お腹が空いた状態に切り替え。吹き出しがでる。

                                    rnd = Random.Range(30.0f, 60.0f);
                                    timeOut = Default_hungry_cooltime + rnd;
                                    Girl_Hungry();

                                    //キャラクタ表情変更
                                    DefFaceChange();

                                    break;

                                case 1: //腹減ったのとき

                                    _model.GetComponent<CubismEyeBlinkController>().enabled = true;
                                    timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                    rnd = Random.Range(1.0f, 2.0f);
                                    timeOut = Default_hungry_cooltime + rnd;
                                    DeleteHukidashiOnly();

                                    //キャラクタ表情変更
                                    DefFaceChange();
                                    break;

                                case 2: //お菓子をあげたあとの状態。

                                    timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                    timeOut = Default_hungry_cooltime;
                                    DeleteHukidashiOnly();

                                    //キャラクタ表情変更
                                    DefFaceChange();
                                    
                                    break;

                                default:

                                    timeOut = Default_hungry_cooltime;
                                    break;
                            }

                        }

                        //一定時間たつとヒントを出すか、アイドルモーションを再生
                        if (timeOut2 <= 0.0)
                        {
                            timeOut2 = 5.0f;
                            timeGirl_hungry_status = 1; //お腹が空いた状態に切り替え。吹き出しがでる。

                            Girl_Hungry();                          

                            random = Random.Range(0, 100);
                            if (random < 50)
                            {
                                Girl1_Hint(10.0f);
                                IdleMotionStart = false;
                            }
                            else
                            {
                                //Girl1_Hint(20.0f);
                                IdleMotionStart = true;
                            }

                            
                        }

                        //タッチを終えたら、カウントスタートし、数秒後に元の状態にリセット
                        if (Girl1_touch_end)
                        {
                            WaitHint_on = false;
                            timeOut3 -= Time.deltaTime;

                            //一定時間がたち、元の状態に戻る。
                            if (timeOut3 <= 0.0f)
                            {
                                GirlEat_Judge_on = true;
                                
                                _model.GetComponent<CubismEyeBlinkController>().enabled = true;                               
                                live2d_animator.SetInteger("trans_facemotion", 0);
                                Girl1_touch_end = false;

                                //表情をリセット
                                switch (compound_Main.compound_status)
                                {
                                    case 4: //調合中のシーン
                                        face_girl_Normal();
                                        break;

                                    default:
                                        DefaultFace();
                                        break;
                                }

                                //フェードで終了する。
                                _model.GetComponent<GazeController>().enabled = false;
                                //facemotion_start = true;
                                //facemotion_init = false;

                                //吹き出し・ハングリーステータスをリセット
                                ResetHukidashi();
                            }
                        }
                       
                        //自動で歩く
                        /*if (Walk_Start)
                        {
                            if (timeOutMoveX <= 0.0f)
                            {
                                rnd = Random.Range(3.0f, 10.0f);
                                timeOutMoveX = 2.0f + rnd;

                                IdleMoveX();
                            }
                        }*/

                    }
                    break;

                default:
                    break;
            }
        }
        
        if(special_animstart_flag) //スペシャル吹き出し出す最初に、アニメ
        {
            switch (special_animstart_status)
            {
                case 0: //キュピーン！！１～２秒程度のアクション。まず初期化

                    _listEffect.Clear();

                    AddMotionAnimReset();
                    ResetCharacterPosition();

                    special_timeOut = 1.0f;

                    canvas.SetActive(false);

                    //カメラ寄る。
                    trans = 1; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //エフェクト生成＋アニメ開始
                    _listEffect.Add(Instantiate(Emo_effect_Prefab1, character.transform));
                    //_listEffect.Add(Instantiate(Emo_effect_Prefab2, character.transform));

                    //音ならす
                    sc.PlaySe(39);

                    //キャラクタ表情変更
                    face_girl_Hirameki();                    

                    special_animstart_status = 1;
                    break;

                case 1: //処理待ち

                    if (special_timeOut <= 0.0f)
                    {
                        special_animstart_status = 2;
                    }
                    break;

                case 2:

                    canvas.SetActive(true);

                    _listEffect.Clear();

                    //カメラ引く。
                    trans = 0; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //キャラクタ表情変更。お菓子食べる前の表情。
                    DefaultFace();                    

                    special_animstart_flag = false;
                    special_animstart_endflag = true;
                    break;
            }

            special_timeOut -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                if (facemotion_start)
                {
                    //カットで切り替えると違和感があるため、自然にフェードで切り替えるようにする。
                    facemotion_duration = live2d_animator.GetCurrentAnimatorStateInfo(2).normalizedTime; //ステートインフォの中の数字は、Animatorのレイヤー番号
                    facemotion_length = live2d_animator.GetCurrentAnimatorStateInfo(2).length;
                    //Debug.Log("ステート長さ" + facemotion_duration); 


                    if (!facemotion_init)
                    {

                        if (facemotion_length != 1) //Resetモーションで出る、再生秒数1を捨てる。アニメのほうでは、1以上で設定する。
                        {
                            Debug.Log("ステート全体長さ" + facemotion_length);

                            facemotion_time = SujiMap(facemotion_length, 3f, 20f, 0.7f, 0.95f); //facemotion_lengthの値が、３～２０秒を、0.75~0.9に変換する 
                            facemotion_init = true;
                            Debug.Log("何秒からフェードアウト" + facemotion_time);
                        }
                    }


                    if (facemotion_duration >= facemotion_time) //再生終了前から徐々にウェイトを戻し、ふぇーどでアニメを戻す
                    {
                        //facemotion_weight = 1.0f;

                        if (!tween_start)
                        {
                            tween_start = true;
                            weightTween = DOTween.To(
                                () => facemotion_weight,          // 何を対象にするのか
                                num =>
                                {
                                    facemotion_weight = num;
                                    live2d_animator.SetLayerWeight(2, facemotion_weight);
                                },   // 値の更新
                                0f,                  // 最終的な値
                                1.0f      // アニメーション時間
                            ).OnComplete(() =>
                            {
                                AddMotionAnimReset();
                                
                    });
                        }
                    }

                }
                else
                {

                    if (!IdleChangeTemp)
                    {

                        //アイドルモーションをランダムで決定。10秒ほど放置していると、勝手に動く。好感度が高くなると、表現も豊かに。

                        if (IdleMotionStart)
                        {
                            IdleMotionStart = false;

                            IdleChange();
                        }
                    }
                    else
                    {
                        /*
                        //ベースレイヤーのアイドルのタイムが0のときに、モーション切り替えをスタートする。
                        //Idle_duration = live2d_animator.GetCurrentAnimatorStateInfo(0).normalizedTime; //ステートインフォの中の数字は、Animatorのレイヤー番号
                        //Idle_duration = Idle_duration - Mathf.Floor(Idle_duration);
                        //Debug.Log("Idle_duration: " + Idle_duration);

                        //if (Idle_duration <= 0.01)
                        //{
                            IdleChangeTemp = false;

                            //全身モーション再生スタートの合図をだす。
                            facemotion_start = true;

                            //再生開始
                            //一度再生をリセットして、ヘッドを0に戻す。
                            live2d_animator.Play("None_facemotion");
                            live2d_animator.Update(2);

                            live2d_animator.SetInteger("trans_facemotion", trans_facemotion); //trans_facemotionは、表情も含めた体全体の動き
                            StartCoroutine(ChangeFaceMotion(9999)); //trans_facemotionの連続防止用

                            //AddMotionLayerのウェイトを徐々にフェードアウト
                            if (!tween_start)
                            {
                                tween_start = true;
                                facemotion_weight = 0f;
                                weightTween = DOTween.To(
                                                            () => facemotion_weight,          // 何を対象にするのか
                                                            num =>
                                                            {
                                                                facemotion_weight = num;
                                                                live2d_animator.SetLayerWeight(2, facemotion_weight);
                                                            },   // 値の更新
                                                            1f,                  // 最終的な値
                                                            0.5f      // アニメーション時間
                                                        ).OnComplete(() =>
                                                        {
                                                            tween_start = false;
                                                        });
                            }*/

                        //}
                    }
                }

                break;
        }
    }

    //Facemotionを強制的にOFF　GirlEatJudgeなどからも読まれる。
    public void AddMotionAnimReset()
    {
        weightTween.Kill();
        tween_start = false;
        facemotion_init = false;
        facemotion_start = false;
        IdleChangeTemp = false;

        live2d_animator.Play("None_facemotion"); //一度アニメーションをリセット
        //live2d_animator.Update(2);
        live2d_animator.SetLayerWeight(2, 0); //強制的にAddMotionLayerを0にする。
       
        live2d_animator.SetInteger("trans_facemotion", 0); //trans_facemotionは、表情も含めた体全体の動き
        _model.GetComponent<CubismEyeBlinkController>().enabled = true;
        //_model.GetComponent<GazeController>().enabled = false;
    }

    void DefFaceChange()
    {
        if (GameMgr.QuestClearflag) //そのお菓子をクリアしたあとの表情
        {
            AfterOkashiDefaultFace();
        }
        else
        {
            DefaultFace();
        }
    }

    public void CheckGokigen()
    {
        //女の子の今のご機嫌
        if (PlayerStatus.girl1_Love_lv >= 1 && PlayerStatus.girl1_Love_lv < 2) // HLv 1
        {
            //テンションが低すぎて暗い
            GirlGokigenStatus = 0;
           
        }
        else if (PlayerStatus.girl1_Love_lv >= 2 && PlayerStatus.girl1_Love_lv < 4) //2~3
        {
            //ご機嫌ななめ
            GirlGokigenStatus = 1;

        }
        else if (PlayerStatus.girl1_Love_lv >= 4 && PlayerStatus.girl1_Love_lv < 6) //4~5
        {
            //少し機嫌が悪い
            GirlGokigenStatus = 2;
           
        }
        else if (PlayerStatus.girl1_Love_lv >= 6 && PlayerStatus.girl1_Love_lv < 8) //6~7
        {
            //ちょっと元気でてきた
            GirlGokigenStatus = 3;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 8 && PlayerStatus.girl1_Love_lv < 10) //8~9
        {
            //だいぶ元気でてきた
            GirlGokigenStatus = 4;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 10 && PlayerStatus.girl1_Love_lv < 13) //10~12
        {
            //元気
            GirlGokigenStatus = 5;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 13) //13~
        {
            //最高に上機嫌
            GirlGokigenStatus = 6;
            
        }
    }

    //お菓子を食べる前の、デフォルトの状態。
    public void DefaultFace()
    {
        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
        live2d_animator = _model.GetComponent<Animator>();
        trans_expression = live2d_animator.GetInteger("trans_expression");

        switch (GirlGokigenStatus)
        {
            case 0:
                face_girl_Bad();
                break;

            case 1:
                face_girl_Bad();
                break;

            case 2:
                face_girl_Little_Fine();
                break;

            case 3:
                face_girl_Fine();
                break;

            case 4:
                face_girl_Normal();
                break;

            case 5:
                face_girl_Joukigen();
                break;

            case 6:
                face_girl_Joukigen();
                break;

            default:
                face_girl_Joukigen();
                break;
        }
    }

    //お菓子に満足したあとの表情。基本的に喜んでいる。好感度によって、少し差がある。
    public void AfterOkashiDefaultFace()
    {
        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
        live2d_animator = _model.GetComponent<Animator>();
        trans_expression = live2d_animator.GetInteger("trans_expression");

        switch (GirlGokigenStatus)
        {
            case 0:
                face_girl_Fine();
                break;

            case 1:
                face_girl_Fine();
                break;

            case 2:
                face_girl_Fine();
                break;

            case 3:
                face_girl_Fine();
                break;

            case 4:
                face_girl_Normal();
                break;

            case 5:
                face_girl_Joukigen();
                break;

            case 6:
                face_girl_Joukigen();
                break;

            default:
                face_girl_Joukigen();
                break;
        }
    }


    //女の子が食べたいものの決定。ランダムでもいいし、ストーリーによっては、一つのイベントの感じで、同じものを合格するまで出し続けてもいい。
    public void Girl_Hungry()
    { 
        //デフォルトで１に設定。セット組み合わせの処理にいったときに、２や３に変わる。
        Set_Count = 1;

        //テーブルの決定
        if (GameMgr.tutorial_ON == true)
        {

        }
        else
        {
            //Debug.Log("通常腹減りステータスON");

            switch (OkashiNew_Status)
            {
                case 0:

                    //前の残りの吹き出しアイテムを削除。
                    if (hukidashiitem != null)
                    {
                        Destroy(hukidashiitem);
                    }

                    //
                    //①特定の課題お菓子。
                    //                   

                    //今選んだやつの、girllikeComposetのIDも保存しておく。（こっちは直接選んでいる。）
                    Set_compID = OkashiQuest_ID;
                    //OkashiQuest_ID = compIDを指定すると、女の子が食べたいお菓子＜組み合わせ＞がセットされる。

                    SetQuestHukidashiText(OkashiQuest_ID, false);

                    if (special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート
                    {
                        //一度、一度ドアップになり、電球がキラン！　→　そのあと、クエストの吹き出し。最初の一回だけ。
                        StartCoroutine("Special_StartAnim");
                    }                   

                    break;

                case 1: //チュートリアル用の回避

                    //前の残りの吹き出しアイテムを削除。
                    if (hukidashiitem != null)
                    {
                        Destroy(hukidashiitem);
                    }
                   

                    break;

                case 2: //現在未使用だが、一応残し

                    //
                    //②通常ステージ、ランダムセット。
                    //

                    //その他、通常のステージ攻略時は、セット組み合わせからランダムに選ぶ。
                    //例えば、セット1・4の組み合わせだと、1でも4でもどっちでも正解。カリっとしたお菓子を食べたい～、のような感じ。    


                    //まず、表示フラグが1のもののみのセットを作る。
                    girlLikeCompo_database.StageSet();

                    //そこからランダムで選択。compIDを指定しているわけではないので、注意！
                    random = Random.Range(0, girlLikeCompo_database.girllike_compoRandomset.Count);

                    //今選んだやつの、randomsetのIDも保存しておく。
                    Set_compID = random;

                    //ランダムセットから、女の子が食べたいお菓子＜組み合わせ＞がセットされる。
                    SetQuestRandomSet(random, true);
                    break;


                default:
                    break;
            }
        }            

    }

    IEnumerator Special_StartAnim()
    {
        if (special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート
        {
            special_animstart_flag = true;
            special_animstart_endflag = false;
            GirlEat_Judge_on = false;
            special_animstart_status = 0;
            touch_controller.Touch_OnAllOFF();
        }
        else
        {
            special_animstart_endflag = true;
        }

        while (!special_animstart_endflag)
        {
            yield return null;
        }


        //会話イベントがまだの場合、会話を表示
        if (!special_animatFirst)
        {
            canvas.SetActive(false);
            sceneBGM.MuteBGM();
            map_ambience.Mute();
            touch_controller.Touch_OnAllOFF();

            while (!GameMgr.camerazoom_endflag)
            {
                yield return null;
            }
            GameMgr.camerazoom_endflag = false;

            //最初にお菓子にまつわるヒントやお話。宴へとぶ。SpOkashiBeforeコメント。
            GameMgr.scenario_ON = true;
            GameMgr.sp_okashi_ID = Set_compID; //GirlLikeCompoSetの_set_compIDが入っている。
            GameMgr.sp_okashi_hintflag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                               //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);
            while (!GameMgr.recipi_read_endflag)
            {
                yield return null;
            }

            GameMgr.scenario_ON = false;
            GameMgr.recipi_read_endflag = false;
            //touch_controller.Touch_OnAllON();
            canvas.SetActive(true);
            sceneBGM.MuteOFFBGM();
            map_ambience.MuteOFF();
            sceneBGM.PlayMain();
        }    

        //現在のクエストネーム更新。Special_Quest.csで、OkashiQuest_Nameは更新している。パネル表示後にネーム更新されるように、ここで描画更新している。
        questname.text = OkashiQuest_Name;

        questtitle_panel.SetActive(true);

        special_animatFirst = true;
        GirlEat_Judge_on = true;
    }

    //チュートリアルで使用
    public void SetOneQuest(int _ID)
    {
        InitializeStageGirlHungrySet(_ID, 0);　//comp_Numの値を直接指定

        Set_Count = 1;
        OkashiNew_Status = 1; //チュートリアルなど。直接指定できるときの状態
        Set_compID = _ID;

        //テキストの設定。直接しているか、セット組み合わせエクセルにかかれたキャプションのどちらかが入る。
        _desc = girllike_desc[0];
    }


    //クエストごとの固有吹き出しテキストの設定
    void SetQuestHukidashiText(int _ID, bool _rndset)
    {
        if (_rndset == true)
        {
            //テキストの設定。セット組み合わせのときは、セット組み合わせ用のメッセージになる。
            _desc = "にいちゃん！　" + girlLikeCompo_database.girllike_compoRandomset[_ID].desc + "が食べたい！";
            GameMgr.NowEatOkashi = girlLikeCompo_database.girllike_compoRandomset[_ID].desc;
        }
        else
        {
            //直接組み合わせセットの_compIDを元に選ぶ。
            i = 0;
            while (i < girlLikeCompo_database.girllike_composet.Count)
            {
                if (girlLikeCompo_database.girllike_composet[i].set_ID == _ID)
                {
                    _compID = i;
                    break;
                }
                i++;
            }

            //テキストの設定。セット組み合わせのときは、セット組み合わせ用のメッセージになる。
            if (GameMgr.GirlLoveEvent_num == 50) //コンテストのときは、この処理をなくしておく。
            {
                _desc = "にいちゃん！　コンテストは、好きなお菓子を持っていくんだよ～！";
                GameMgr.NowEatOkashi = "コンテスト";
            }
            else
            {
                _desc = "にいちゃん！　" + girlLikeCompo_database.girllike_composet[_compID].desc + "が食べたい！";
                GameMgr.NowEatOkashi = girlLikeCompo_database.girllike_composet[_compID].desc;
            }
        }
    }
    
    public void SetQuestRandomSet(int _ID, bool _rndset)
    {
        if (_rndset == true)
        {
            //ランダムセットから一つを選ぶ。
            set1_ID = girlLikeCompo_database.girllike_compoRandomset[_ID].set1;
            set2_ID = girlLikeCompo_database.girllike_compoRandomset[_ID].set2;
            set3_ID = girlLikeCompo_database.girllike_compoRandomset[_ID].set3;

            //テキストの設定。セット組み合わせのときは、セット組み合わせ用のメッセージになる。
            if (GameMgr.GirlLoveEvent_num == 50) //コンテストのときは、この処理をなくしておく。
            {
                _desc = "にいちゃん！　コンテストは、好きなお菓子を持っていくんだよ～！";
                GameMgr.NowEatOkashi = "コンテスト";
            }
            else
            {
                _desc = "にいちゃん！　" + girlLikeCompo_database.girllike_composet[_ID].desc + "が食べたい！";
                GameMgr.NowEatOkashi = girlLikeCompo_database.girllike_composet[_ID].desc;
            }
        }
        else
        {
            //直接組み合わせセットの_compIDを元に選ぶ。
            i = 0;
            while(i < girlLikeCompo_database.girllike_composet.Count)
            {
                if (girlLikeCompo_database.girllike_composet[i].set_ID == _ID)
                {
                    _compID = i;
                    break;
                }
                i++;
            }

            set1_ID = girlLikeCompo_database.girllike_composet[_compID].set1;
            set2_ID = girlLikeCompo_database.girllike_composet[_compID].set2;
            set3_ID = girlLikeCompo_database.girllike_composet[_compID].set3;

            //テキストの設定。セット組み合わせのときは、セット組み合わせ用のメッセージになる。
            if (GameMgr.GirlLoveEvent_num == 50) //コンテストのときは、この処理をなくしておく。
            {
                _desc = "にいちゃん！　コンテストは、好きなお菓子を持っていくんだよ～！";
                GameMgr.NowEatOkashi = "コンテスト";
            }
            else
            {
                _desc = "にいちゃん！　" + girlLikeCompo_database.girllike_composet[_compID].desc + "が食べたい！";
                GameMgr.NowEatOkashi = girlLikeCompo_database.girllike_composet[_compID].desc;
            }
        }

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

        Set_Count = set_ID.Count;

        //Debug.Log("Set_Count: " + Set_Count);

        //さきほどのset_IDをもとに、好みの値を決定する。
        for (count = 0; count < Set_Count; count++)
        {
            InitializeStageGirlHungrySet(set_ID[count], count);

        }

        
    }

    /*public void Girl_Full() //compound_mainから読み込んでいる。
    {
        if (hukidashiitem != null)
        {
            DeleteHukidashi();
        }

        timeOut = 5.0f;
    }*/

    public void Girl_hukidashi_Off()
    {
        //前の残りの吹き出しアイテムを一時的にオフ
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(false);
        }
    }

    public void Girl_hukidashi_On()
    {
        //前の残りの吹き出しアイテムを一時的にオン
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(true);
        }
    }

    public void ResetHukidashi()
    {
        //一度吹き出しを削除し、クエスト吹き出しなどを表示する。
        timeOut = Default_hungry_cooltime;
        timeOut2 = 10.0f;
        timeGirl_hungry_status = 0;

        DeleteHukidashi();
    }

    //お菓子出来たて直後の吹き出しリセット設定。Live2DAnimationTrigger.csから読み出し
    public void ResetHukidashiYodare()
    {
        //一度吹き出しを削除し、クエスト吹き出しなどを表示する。
        timeOut = 30.0f;
        timeOut2 = 5.0f;
        timeGirl_hungry_status = 1;

    }

    //デフォルト・共通の腹減り初期化設定
    public void Girl1_Status_Init()
    {
        //timeOut = 5.0f;

        timeGirl_hungry_status = 0;
    }

    //好感度イベントの腹減り初期化設定
    public void Girl1_Status_Init2()
    {
        timeOut = 0.5f;

        timeGirl_hungry_status = 0;
    }

    //
    // らんだむで表示される女の子のセリフ。ヒントか、好感度によって変わる反応
    //
    public void Girl1_Hint(float _temptimehint)
    {
        if (hukidashiitem == null)
        {
            hukidasiInit(_temptimehint);
        }

        //まだ一度も調合していない
        if (PlayerStatus.First_recipi_on != true)
        {
            _hint1 = "..まずは左の「おかしパネル」から、お菓子を作ろうね。おにいちゃん。";            
            hukidashiitem.GetComponent<TextController>().SetText(_hint1);
        }
        else
        {
            //ランダムで吹き出しの内容を出す。 or 今食べたいものをしゃべる。
            NowEatText();
        }

        //Idleにリセット
        trans_motion = 1000;
        live2d_animator.SetInteger("trans_motion", trans_motion);
        _model.GetComponent<CubismEyeBlinkController>().enabled = true;
        
    }

    void NowEatText()
    {
        Random.InitState(GameMgr.Game_timeCount); //シード値をバラバラに変える。ゲーム内タイマーで変える。

        //ランダムで、吹き出しの内容を決定。口を触った時のコメントと一緒。
        Init_touchFaceComment();

        random = Random.Range(0, _touchface_comment_lib.Count);
        _hintrandom = _touchface_comment_lib[random];

        //ヒントをだすか、今食べたいもののどちらかを表示する。3連続で食べたいものが表示されていないなら、4つめは次は必ず食べたいものを表示する。
        if (_noweat_count >= 3)
        {
            _noweat_count = 0;
            hukidashiitem.GetComponent<TextController>().SetTextColorPink(_desc);
        }
        else { 

            random = Random.Range(0, 100);
            if (random < 50)
            {
                _noweat_count++;
                hukidashiitem.GetComponent<TextController>().SetText(_hintrandom);
            }
            else
            {
                if (GameMgr.QuestClearflag)
                {
                    hukidashiitem.GetComponent<TextController>().SetText("兄ちゃん！お菓子おいしかった！ありがと～♪");

                    //表情喜びに。2秒ほどしてすぐ戻す。
                    face_girl_Yorokobi();

                    StartCoroutine("FaceModosu");
                }
                else
                {
                    _noweat_count = 0;
                    hukidashiitem.GetComponent<TextController>().SetTextColorPink(_desc);
                }
            }
        }
    }

    IEnumerator FaceModosu()
    {
        yield return new WaitForSeconds(2.0f);

        AfterOkashiDefaultFace();
    }

    IEnumerator WaitHintDesc()
    {
        if (isRunning) //重複を防ぐ。
        {
            yield break;
        }
        isRunning = true;

        GirlEat_Judge_on = false;

        yield return new WaitForSeconds(5.0f);

        //吹き出しが残っていたら、内容を変える。
        if (hukidashiitem != null)
        {
            _text.text = _desc;
        }

        GirlEat_Judge_on = true;
        isRunning = false;
    }

    public void hukidashiReturnHome()
    {
        hukidasiInit(10.0f);

        hukidashiitem.GetComponent<TextController>().SetText("おいしそ～♪");


    }

    public void hukidashiOkashiFailedReturnHome()
    {
        hukidasiInit(10.0f);

        hukidashiitem.GetComponent<TextController>().SetText("失敗しちゃった・・。");


    }

    //吹き出しの生成メソッド
    public void hukidasiInit(float _timehint)
    {
        //Live2Dモデルの取得
        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;

        if (hukidashiitem != null)
        {
            DeleteHukidashi();
        }

        hukidashiitem = Instantiate(hukidashiPrefab, _model_obj.transform);
        hukidashion = true; //今吹き出しがゲームに表示されている状態

        //hukidashiitem.transform.Find("hukidashi_Image_special").gameObject.SetActive(true);
        //hukidashiitem.transform.Find("hukidashi_Image").gameObject.SetActive(false);

        //音を鳴らす
        sc.PlaySe(7);

        _text = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();

        //15秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = _timehint;
        GirlEat_Judge_on = false;
    }

    //吹き出しを一時オフ
    public void hukidasiOff()
    {
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(false);
            hukidashion = false;
        }
    }

    //吹き出しを一時オン
    public void hukidasiOn()
    {
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(true);
            hukidashion = true;
        }
    }

    void DeleteHukidashi()
    {
        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);
            hukidashion = false; //吹き出しが削除されて、見えない状態

            //音を鳴らす
            sc.PlaySe(8);
        }     

    }

    public void DeleteHukidashiOnly() //こっちは消すだけ。音無し。
    {
        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);
            hukidashion = false;
        }
    }
    


    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコア(所持数)と、追加得点用のスコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            girl1_hungryInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            girl1_hungryScoreSet1.Add(0);
            girl1_hungryScoreSet2.Add(0);
            girl1_hungryScoreSet3.Add(0);
            girl1_hungryToppingScoreSet1.Add(0);
            girl1_hungryToppingScoreSet2.Add(0);
            girl1_hungryToppingScoreSet3.Add(0);
            girl1_hungryToppingNumberSet1.Add(0);
            girl1_hungryToppingNumberSet2.Add(0);
            girl1_hungryToppingNumberSet3.Add(0);
        }
    }

    public void InitializeStageGirlHungrySet(int _id, int _set_num)
    {
        //IDをセット。「compNum」の値で指定する。

        //compNumの値で指定しているので、IDに変換する。
        j = 0;
        while (j < girlLikeSet_database.girllikeset.Count)
        {
            if (_id == girlLikeSet_database.girllikeset[j].girlLike_compNum)
            {
                //Debug.Log("girlLikeSet_database.girllikeset[j].girlLike_compNum: " + girlLikeSet_database.girllikeset[j].girlLike_compNum);
                //Debug.Log("j :" + j);
                setID = j;
                break;
            }
            j++;
        }



        //初期化
        girl1_hungrySet.Clear();
        girl1_hungrytoppingSet.Clear();
        girl1_hungrytoppingNumberSet.Clear();


        //ステージごとに、女の子が欲しがるアイテムのセット

        //セット例
        //①スロット：　オレンジ・ナッツ・ぶどう

        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {

            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[0])
            {

                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[0] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[0]);
                    girl1_hungrytoppingNumberSet.Add(1);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[1])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[1] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[1]);
                    girl1_hungrytoppingNumberSet.Add(2);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[2])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[2] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[2]);
                    girl1_hungrytoppingNumberSet.Add(3);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[3])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[3] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[3]);
                    girl1_hungrytoppingNumberSet.Add(4);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[4])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[4] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[4]);
                    girl1_hungrytoppingNumberSet.Add(5);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[5])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[5] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[5]);
                    girl1_hungrytoppingNumberSet.Add(6);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[6])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[6] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[6]);
                    girl1_hungrytoppingNumberSet.Add(7);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[7])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[7] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[7]);
                    girl1_hungrytoppingNumberSet.Add(8);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[8])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[8] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(girlLikeSet_database.girllikeset[setID].girlLike_topping_score[8]);
                    girl1_hungrytoppingNumberSet.Add(9);
                }
            }
        }


        //以下、パラメータのセッティング

        //①女の子の食べたいトッピング

        switch (_set_num)
        {
            case 0:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet1.Count; i++)
                {
                    girl1_hungryScoreSet1[i] = 0;
                    girl1_hungryToppingScoreSet1[i] = 0;
                    girl1_hungryToppingNumberSet1[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet1[girl1_hungrySet[i]]++;
                    girl1_hungryToppingScoreSet1[girl1_hungrySet[i]] = girl1_hungrytoppingSet[i];
                    girl1_hungryToppingNumberSet1[girl1_hungrySet[i]] = girl1_hungrytoppingNumberSet[i];
                }
                break;

            case 1:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet2.Count; i++)
                {
                    girl1_hungryScoreSet2[i] = 0;
                    girl1_hungryToppingScoreSet2[i] = 0;
                    girl1_hungryToppingNumberSet2[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet2[girl1_hungrySet[i]]++;
                    girl1_hungryToppingScoreSet2[girl1_hungrySet[i]] = girl1_hungrytoppingSet[i];
                    girl1_hungryToppingNumberSet2[girl1_hungrySet[i]] = girl1_hungrytoppingNumberSet[i];
                }
                break;

            case 2:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet3.Count; i++)
                {
                    girl1_hungryScoreSet3[i] = 0;
                    girl1_hungryToppingScoreSet3[i] = 0;
                    girl1_hungryToppingNumberSet3[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet3[girl1_hungrySet[i]]++;
                    girl1_hungryToppingScoreSet3[girl1_hungrySet[i]] = girl1_hungrytoppingSet[i];
                    girl1_hungryToppingNumberSet3[girl1_hungrySet[i]] = girl1_hungrytoppingNumberSet[i];
                }
                break;

            default:
                break;
        }

        //欲しいトッピングがなかったときに、マイナスに働くパラメータ
        girl1_NonToppingScoreSet[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_Non_topping_score;

        //②味のパラメータ。これに足りてないと、「甘さが足りない」といったコメントをもらえる。
        girl1_Rich[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_rich;
        girl1_Sweat[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sweat;
        girl1_Sour[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sour;
        girl1_Bitter[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_bitter;

        girl1_Crispy[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_crispy;
        girl1_Fluffy[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_fluffy;
        girl1_Smooth[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_smooth;
        girl1_Hardness[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_hardness;
        girl1_Chewy[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_chewy;
        girl1_Jiggly[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_jiggly;
        girl1_Juice[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_juice;

        girl1_Beauty[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_beauty;

        //③お菓子の種類：　空＝お菓子はなんでもよい　か　クッキー
        girl1_likeSubtype[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_itemSubtype;

        //④特定のお菓子が食べたいかを決定。関係性は、④＞③。
        //④が決まった場合、③は無視し、①と②だけ計算する。④が空=Nonの場合、③を計算。④も③も空の場合、お菓子の種類は関係なくなる。
        girl1_likeOkashi[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_itemName;

        //セットごとの固有の味の採点値をセット
        girl1_like_set_score[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_set_score;

        //コメントをセット
        girllike_desc[_set_num] = girlLikeSet_database.girllikeset[setID].set_kansou;

        //お菓子食べた後の感想用フラグのセット
        girllike_comment_flag[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_comment_flag;

        //compNumの番号も保存。どの判定用お菓子セットを選んだかがわかる。
        girllike_judgeNum[_set_num] = _id;
    }



    //
    //タッチ関係
    //

    //頭　一回タッチ
    public void Touchhair_Start()
    {
        Girl1_touchhair_status = 0;
        Girl1_touchhair_count = 0;
        Girl1_touchhair_start = true;
        Girl1_touch_end = false;
        touchanim_start = true;
        GirlEat_Judge_on = false;

        //一回タッチするだけだと、「いてっ」って感じの反応
        touch_startreset();

        //タップモーション　ランダムで決定
        Random_TapMotion();
        facemotion_start = true;
        facemotion_init = false;
      
    }

    //頭　ドラッグで触り続けた場合
    public void TouchSisterHair()
    {
        switch (Girl1_touchhair_status)
        {

            case 0: //初期化

                Girl1_touchhair_start = true;
                Girl1_touch_end = false;
                GirlEat_Judge_on = false;

                //5秒以内に髪の毛を何度か触ると、ちょっと照れる。
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 1;

                live2d_animator.SetInteger("trans_nade", 0); //リセット

                //Init_touchHeadComment();
                //_touchhead_comment = _touchhead_comment_lib[0];
                //hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更
                //face_girl_Surprise();

                break;

            case 1: //髪の毛触る回数カウント中

                Girl1_touch_end = false;

                Girl1_touchhair_count++;               

                if (Girl1_touchhair_count >= 3) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 2;
                }
                break;

            case 2:

                touch_startreset();
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 3;              

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[1];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情・モーション変更
                HairTouch_Motion();

                _model_obj.GetComponent<GazeController>().enabled = true;

                break;

            case 3: //髪の毛触る回数カウント中＜2段階目＞

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 3) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 4;
                }
                break;

            case 4:

                touch_startreset();
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 5;

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[2];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //表情変化２
                HairTouch_Motion2();

                break;

            case 5:

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 6;
                }

                break;

            case 6:

                touch_startreset();
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 7;

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[3];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //表情変化２
                HairTouch_Motion3();

                break;

            case 7:

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 8;
                }

                break;

            case 8:

                touch_startreset();
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 9;

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[4];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                break;

            case 9:

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 30) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 10;
                }

                break;

            case 10:

                touch_startreset();
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 11;

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[5];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更　ちょっと嫌そう？真顔に。
                facemotion_start = true; //Facemotionは、このタイミングでオフに。
                face_girl_Iya();

                break;

            case 11:

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 12;
                }

                break;

            case 12:

                touch_startreset();
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 13;

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[6];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //音鳴らす
                sc.PlaySe(45);

                //キャラクタ表情変更  怒る
                face_girl_Angry();

                //エモ
                _listEffect.Add(Instantiate(Emo_effect_Prefab3, character.transform));
                break;

            case 13:

                Girl1_touchhair_count++;

                break;

            default:
                break;
        }             
        
    }

    //髪なでなで時のモーションセット1
    void HairTouch_Motion()
    {
        weightTween.Kill(); //フェードアウト中なら中断する
        tween_start = false;
        facemotion_start = false;

        switch (GirlGokigenStatus)
        {
            case 0: 

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("Nade_motion_01", 2, 0.0f);
                break;

            case 1:

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("Nade_motion_01", 2, 0.0f);
                break;

            case 2: 

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("Nade_motion_01", 2, 0.0f);
                break;

            case 3: 

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_01_2", 2, 0.0f);
                break;

            case 4: 

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_01_2", 2, 0.0f);
                break;

            case 5: 

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_01_2", 2, 0.0f);
                break;

            case 6: 

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_01_2", 2, 0.0f);
                break;

            default:

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_01_2", 2, 0.0f);
                break;
        }
        facemotion_weight = 1.0f;
    }

    //髪なでなで時のモーションセット2
    void HairTouch_Motion2()
    {

        switch (GirlGokigenStatus)
        {
            case 0: 

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            case 1: 

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            case 2: 

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            case 3: 

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            case 4: 

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            case 5: 

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            case 6:

                live2d_animator.SetInteger("trans_nade", 10);
                break;

            default:

                live2d_animator.SetInteger("trans_nade", 10);
                break;
        }
    }

    //髪なでなで時のモーションセット3
    void HairTouch_Motion3()
    {

        switch (GirlGokigenStatus)
        {
            case 0: 

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            case 1: 

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            case 2: 

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            case 3: 

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            case 4: 

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            case 5:

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            case 6:

                live2d_animator.SetInteger("trans_nade", 11);
                break;

            default:

                live2d_animator.SetInteger("trans_nade", 11);
                break;
        }
    }


    //ツインテール　一回さわった
    public void Touchtwintail_Start()
    {
        Girl1_touchtwintail_start = true;
        touchanim_start = true;
        facemotion_start = true;

        //タップモーション
        live2d_animator.SetLayerWeight(2, 1);
        live2d_animator.Play("tapmotion_01", 2, 0.0f); //tapmotion_01は、頭なでなで・ツインテール共通のモーション
        facemotion_weight = 1.0f;

        trans_facemotion = 9999; //その他のモーションに遷移しないように回避
        live2d_animator.SetInteger("trans_facemotion", trans_facemotion); //trans_facemotionは、表情も含めた体全体の動き       
        

        _model_obj.GetComponent<GazeController>().enabled = true;
    }

    //ツインテール　ドラッグで触り続けた場合
    public void TouchSisterTwinTail()
    {
        touch_startreset();

        //コメント順番に表示
        Init_touchTwintailComment();

        if (Girl1_touchtwintail_count >= _touchtwintail_comment_lib.Count)
        {
            Girl1_touchtwintail_flag = true; //ツインテールに関する全てのコメントを表示した
            Girl1_touchtwintail_count = 0;
            StartCoroutine("WaitTwintailSeconds");
        }

        if (!Girl1_touchtwintail_flag)
        {
            _touchtwintail_comment = _touchtwintail_comment_lib[Girl1_touchtwintail_count];
            hukidashiitem.GetComponent<TextController>().SetText(_touchtwintail_comment);
        }
        else
        {
            hukidashiitem.GetComponent<TextController>().SetText("..。");
        }
        Girl1_touchtwintail_count++;
    }

    IEnumerator WaitTwintailSeconds()
    {
        yield return new WaitForSeconds(10.0f);

        Girl1_touchtwintail_flag = false;
    }




    //口のあたりをクリックすると、ヒントを表示する。
    public void TouchSisterFace()
    {        
        if (hukidashion)
        {
            DeleteHukidashiOnly(); //必ず吹き出しを一度削除する
        }
        else
        {
            touch_startreset();
            //ランダムで吹き出しの内容を出す。 or 今食べたいものをしゃべる。
            NowEatText();
        }
        

        //15秒ほど表示したら、また食べたいお菓子を表示か削除
        timeOutHint = 10.0f;

    }


    //リボン
    public void TouchRibbon_Start()
    {
        Girl1_touchchest_start = true;
        touchanim_start = true;
        facemotion_start = true;

        //タップモーション
        live2d_animator.SetLayerWeight(2, 1);
        live2d_animator.Play("tapmotion_03_1", 2, 0.0f);
        facemotion_weight = 1.0f;

        trans_facemotion = 9999; //その他のモーションに遷移しないように回避
        live2d_animator.SetInteger("trans_facemotion", trans_facemotion); //trans_facemotionは、表情も含めた体全体の動き

    }

    public void TouchSisterRibbon()
    {
        touch_startreset();

        //コメントランダム
        //random = Random.Range(0, _touchface_comment_lib.Count);
        //_touchface_comment = _touchface_comment_lib[random];

        hukidashiitem.GetComponent<TextController>().SetText("お母さんが誕生日にくれたリボンだよ～。うひひ。");

        //タップモーション
        live2d_animator.SetLayerWeight(2, 1);
        live2d_animator.Play("tapmotion_03_1", 2, 0.0f);
        facemotion_weight = 1.0f;

        trans_facemotion = 9999; //その他のモーションに遷移しないように回避
        live2d_animator.SetInteger("trans_facemotion", trans_facemotion); //trans_facemotionは、表情も含めた体全体の動き

    }

    //手
    public void TouchHand_Start()
    {
        Girl1_touchchest_start = true;
        touchanim_start = true;
        facemotion_start = true;

        //タップモーション
        live2d_animator.SetLayerWeight(2, 1);
        live2d_animator.Play("tapmotion_03_1", 2, 0.0f);
        facemotion_weight = 1.0f;

        trans_facemotion = 9999; //その他のモーションに遷移しないように回避
        live2d_animator.SetInteger("trans_facemotion", trans_facemotion); //trans_facemotionは、表情も含めた体全体の動き
    }

    public void TouchSisterHand()
    {
        touch_startreset();

        //吹き出し内容の決定
        Init_touchHandComment();

        random = Random.Range(0, _touchhand_comment_lib.Count);
        _touchhand_comment = _touchhand_comment_lib[random];

        hukidashiitem.GetComponent<TextController>().SetText(_touchhand_comment);
    }

    //胸
    public void TouchChest_Start()
    {
        Girl1_touchchest_start = true;
        touchanim_start = true;
        facemotion_start = true;

        //タップモーション　最初触った一回だけ発動        
        live2d_animator.SetLayerWeight(2, 1);
        live2d_animator.Play("tapmotion_02", 2, 0.0f);
        facemotion_weight = 1.0f;

        trans_facemotion = 9999; //その他のモーションに遷移しないように回避
        live2d_animator.SetInteger("trans_facemotion", trans_facemotion); //trans_facemotionは、表情も含めた体全体の動き
        
    }

    public void TouchSisterChest()
    {
        touch_startreset();

        //吹き出し内容の決定
        Init_touchChestComment();

        random = Random.Range(0, _touchchest_comment_lib.Count);
        _touchchest_comment = _touchchest_comment_lib[random];

        hukidashiitem.GetComponent<TextController>().SetText(_touchchest_comment);

    }

    //花
    public void TouchFlower()
    {
        touch_startreset();

        hukidashiitem.GetComponent<TextController>().SetText("お兄ちゃん。それは花だよ。しおれてたら、お水をあげてね。");
    }


    //タップモーション　ランダムで決定
    void Random_TapMotion()
    {
        _model_obj.GetComponent<GazeController>().enabled = false;

        random = Random.Range(0, 3);

        switch(random)
        {
            case 0:

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_03_1", 2, 0.0f);
                hukidashiitem.GetComponent<TextController>().SetText("うわっ！");
                break;

            case 1:

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_03_2", 2, 0.0f);
                hukidashiitem.GetComponent<TextController>().SetText("あいたっ！");
                break;

            case 2:

                live2d_animator.SetLayerWeight(2, 1);
                live2d_animator.Play("tapmotion_03_3", 2, 0.0f);
                hukidashiitem.GetComponent<TextController>().SetText("いてぃっ！");
                break;
        }
        facemotion_weight = 1.0f;
    }

    void touch_startreset() //触り始め共通でリセットする項目。
    {
        if (hukidashiitem == null)
        {
            hukidasiInit(999.0f);
        }

        weightTween.Kill(); //フェードアウト中なら中断する
        tween_start = false;
    }   

    //ランダムで左右に動く
    void IdleMoveX()
    {
        rnd = Random.Range(2.0f, -2.0f);

        MoveXMethod(rnd);
        
    }

    void MoveXMethod(float _move)
    {
        sequence_girlmove = DOTween.Sequence();

        sequence_girlmove.Append(character_move.transform.DOMoveX(_move, 3.0f)
        .SetEase(Ease.InOutSine));

        sequence_girlmove2 = DOTween.Sequence().SetLoops(3);
        sequence_girlmove2.Append(character_move.transform.DOMoveY(0.1f, 0.5f))
            .SetRelative();
        sequence_girlmove2.Append(character_move.transform.DOMoveY(-0.1f, 0.5f))
            .SetRelative();
    }

    //歩くをストップ
    public void DoTSequence_Kill()
    {
        sequence_girlmove.Complete();
        sequence_girlmove2.Complete();
        timeOutMoveX = 7.0f;
    }

    //移動した位置を元に戻す。
    public void ResetCharacterPosition()
    {
        DoTSequence_Kill();
        character_move.transform.DOMoveX(0, 0.0f);
    }

    //ランダムで仕草　ここではどのモーションを再生するか決定するだけ。
    public void IdleChange()
    {
        _model.GetComponent<CubismEyeBlinkController>().enabled = false;
        _model.GetComponent<GazeController>().enabled = false;

        /*
        if (GirlOishiso_Status == 0) //デフォルトの状態
        { }
        else if (GirlOishiso_Status == 1) //お菓子出来たてのあと、おいしそ～状態
        {
            //_temp_status = 1; //デバッグ用
            
            random = Random.Range(0, 100); //抽選 2分の1で、おいしそ～状態特有のアイドルモーションに変化
            if (random >= 50)
            {
                _temp_status = 0;
            }
            else
            {
                _temp_status = 1;
            }
        }*/

        if (_temp_status == 0) //デフォルトの状態
        {
            
            switch (GirlGokigenStatus)
            {               
                case 0:

                    random = Random.Range(0, 100); //抽選
                    if (random >= 80)
                    {
                        trans_facemotion = 0; //0は、何も動かない状態。
                    }
                    else
                    {
                        trans_facemotion = 1; //ママのいない悲しみモーション
                        
                        IdleMotionHukidashiSetting(trans_facemotion); //吹き出しも一緒に生成
                        IdleChangeTemp = true;
                    }

                    break;

                case 1:

                    random = Random.Range(0, 100); //抽選
                    if (random >= 80)
                    {
                        trans_facemotion = 0; //0は、何も動かない状態。
                    }
                    else
                    {
                        trans_facemotion = 1; //ママのいない悲しみモーション

                        IdleMotionHukidashiSetting(trans_facemotion);
                        IdleChangeTemp = true;
                    }
                    break;

                case 2:

                    random = Random.Range(0, 4); //0~3
                    trans_facemotion = random + 10; //10はじまりが、ちょっと動くモーション

                    IdleMotionHukidashiSetting(trans_facemotion);
                    IdleChangeTemp = true;
                    break;

                case 3:

                    random = Random.Range(0, 4); //0~3
                    trans_facemotion = random + 1; //10はじまりが、ちょっと動くモーション

                    IdleMotionHukidashiSetting(trans_facemotion);
                    IdleChangeTemp = true;
                    break;

                case 4:

                    random = Random.Range(0, 4); //0~3
                    trans_facemotion = random + 1; //10はじまりが、ちょっと動くモーション

                    IdleMotionHukidashiSetting(trans_facemotion);
                    IdleChangeTemp = true;
                    break;

                case 5:

                    random = Random.Range(0, 4); //0~3
                    trans_facemotion = random + 1; //10はじまりが、ちょっと動くモーション

                    IdleMotionHukidashiSetting(trans_facemotion);
                    IdleChangeTemp = true;
                    break;

                case 6:

                    random = Random.Range(0, 4); //0~3
                    trans_facemotion = random + 1; //10はじまりが、ちょっと動くモーション

                    IdleMotionHukidashiSetting(trans_facemotion);
                    IdleChangeTemp = true;
                    break;

                default:

                    random = Random.Range(0, 4); //0~3
                    trans_facemotion = random + 1; //10はじまりが、ちょっと動くモーション

                    IdleMotionHukidashiSetting(trans_facemotion);
                    IdleChangeTemp = true;

                    break;
            }

        }
        /*else if (_temp_status == 1) //お菓子出来たてのあと、おいしそ～状態 50%の確率で、出来たてのモーションを再生
        {
            trans_facemotion = 500; //くんくんモーション
            IdleMotionHukidashiSetting(trans_facemotion);
            IdleChangeTemp = true;
        }*/
    }   

    void IdleMotionHukidashiSetting(int _motion_num)
    {
        if (hukidashiitem == null)
        {
            hukidasiInit(10.0f);
        }

        switch (_motion_num)
        {

            case 1: //悲しみモーションのときのセリフ

                
                _touchface_comment_lib.Clear();
                _touchface_comment_lib.Add("..ママ。");
                _touchface_comment_lib.Add("ぐすん..。");
                _touchface_comment_lib.Add("（..ママ。）");
                _touchface_comment_lib.Add("..ママ。会いたいなぁ..。");
                _touchface_comment_lib.Add("..にいちゃん。お菓子..。食べたい。");
                _touchface_comment_lib.Add("..。にいちゃん。..。なんでもない。");

                break;

            default:

                _touchface_comment_lib.Clear();
                _touchface_comment_lib.Add("..辛抱たまらん！");
                break;
        }

        random = Random.Range(0, _touchface_comment_lib.Count);
        _hintrandom = _touchface_comment_lib[random];
        hukidashiitem.GetComponent<TextController>().SetText(_hintrandom);
    }

    IEnumerator ChangeFaceMotion(int _num) //番号を指定すると、0.1秒後に、facemotionの値だけを自動で切り替え。主に、モーションの重複の回避用に9999を入れなおしている。
    {
        yield return new WaitForSeconds(0.1f);

        trans_facemotion = _num;
        live2d_animator.SetInteger("trans_facemotion", trans_facemotion);
    }

    void Init_touchHeadComment()
    {
        //髪の毛触るときは、上から順番に表示されていく。回数に注意。
        _touchhead_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0:

                _touchhead_comment_lib.Add("..?");
                _touchhead_comment_lib.Add("..");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add("（少し嬉しいようだ..。）");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("..ガウゥ！！！！");
                break;

            case 1:

                _touchhead_comment_lib.Add("..!!");
                _touchhead_comment_lib.Add("..");
                _touchhead_comment_lib.Add("にいちゃん、おててに粉ついてるよ～..。");
                _touchhead_comment_lib.Add("..♪");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("..ウガーー!!");
                break;

            case 2:

                _touchhead_comment_lib.Add("ん、どうしたの？兄。");
                _touchhead_comment_lib.Add("えへへ..。");
                _touchhead_comment_lib.Add("気持ちいい。さわさわ..。");
                _touchhead_comment_lib.Add("あ～～～..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("キィーーーーー！！！！");
                break;

            case 3:

                _touchhead_comment_lib.Add("あ、おにいちゃんのおてて！");
                _touchhead_comment_lib.Add("へへ..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add("あったか～い..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("ガァーーー！！！！");
                break;

            case 4:

                _touchhead_comment_lib.Add("ん、なでなでして。おにいちゃん");
                _touchhead_comment_lib.Add("んん..。");
                _touchhead_comment_lib.Add("あったかあったか..。");
                _touchhead_comment_lib.Add("～～..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("ギニャーーーー！！！！");
                break;

            case 5:

                _touchhead_comment_lib.Add("おにいちゃん..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add("うひひ..。");
                _touchhead_comment_lib.Add("クッキーのにおい..。うまそ..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("グガーーー！！！！");
                break;

            case 6:

                _touchhead_comment_lib.Add("おにいちゃん..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add("うひひ..。");
                _touchhead_comment_lib.Add("クッキーのにおい..。うまそ..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("グガーーー！！！！");
                break;
        }

    }

    void Init_touchFaceComment()
    {
        _touchface_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0:

                _touchface_comment_lib.Add("..");               
                _touchface_comment_lib.Add("..おかし、食べたいな。おにいちゃん..。");
                _touchface_comment_lib.Add("..まずはクッキーを作ってみようね。");                
                _touchface_comment_lib.Add("にいちゃん..。");
                _touchface_comment_lib.Add("..おなか、すいた。");
                break;

            case 1:

                _touchface_comment_lib.Add(".. ..。");               
                _touchface_comment_lib.Add("お腹へった。");
                _touchface_comment_lib.Add("..兄ちゃんのおかし、食べたい。");
                _touchface_comment_lib.Add("兄ちゃんのおかし作り、てつだう。");
                break;

            case 2:

                _touchface_comment_lib.Add("ちょっと元気。");
                _touchface_comment_lib.Add("材料の数は、兄ちゃんの好みに変えられるんだよ～。");
                _touchface_comment_lib.Add("腹へた～..。");
                _touchface_comment_lib.Add("うまうま・・。");
                _touchface_comment_lib.Add("にいちゃんのおかし、おいしい。もぐもぐ..。");
                break;

            case 3:

                _touchface_comment_lib.Add("エメラルド色のどんぐり、欲しい？兄ちゃん。");
                _touchface_comment_lib.Add("うきうき！");
                _touchface_comment_lib.Add("味見..。味見..。");
                _touchface_comment_lib.Add("ねぇねぇ兄ちゃん。材料を採りにいこうよ～。");
                _touchface_comment_lib.Add("いい朝だねぇ～。お兄ちゃん～。");
                _touchface_comment_lib.Add("いっぱい手伝うね！お兄ちゃん。");
                break;

            case 4:

                _touchface_comment_lib.Add("るんるん♪");
                _touchface_comment_lib.Add("材料の比率は、兄ちゃんの好みに変えられるんだよ～。");
                _touchface_comment_lib.Add("兄ちゃん、もうコンテストとか余裕？");
                _touchface_comment_lib.Add("あ～～。今日はあたたかいね、兄ちゃん！");
                _touchface_comment_lib.Add("♪");
                _touchface_comment_lib.Add("エメラルどんぐり、拾いにいこうよ～。お兄ちゃん。");
                break;

            case 5:

                _touchface_comment_lib.Add("キラキラ♪");
                _touchface_comment_lib.Add("兄ちゃん！大好き！！");
                _touchface_comment_lib.Add("兄ちゃんのお菓子、こころがぽかぽかするんじゃ～");
                _touchface_comment_lib.Add("兄ちゃんのおてて、あたたか～い");
                _touchface_comment_lib.Add("お兄ちゃん。あたたかい～。");
                _touchface_comment_lib.Add("どこかへ出かけたいなぁ～");
                break;

            case 6:

                _touchface_comment_lib.Add("キラキラ♪");
                _touchface_comment_lib.Add("兄ちゃん！大好き！！");
                _touchface_comment_lib.Add("兄ちゃんのお菓子、こころがぽかぽかするんじゃ～");
                _touchface_comment_lib.Add("兄ちゃんのおてて、あたたか～い");
                _touchface_comment_lib.Add("お兄ちゃん。あたたかい～。");
                _touchface_comment_lib.Add("どこかへ出かけたいなぁ～");
                break;
        }

        //パティシエレベルに応じて、ヒントをだす。
        if(PlayerStatus.player_renkin_lv < 3) //LV 1~2
        {
            _touchface_comment_lib.Add("さくさく感の出し方は、ショップのおねえちゃんが知ってたかも？");
            _touchface_comment_lib.Add("さわられると、ビックリしちゃうよ～・・。おにいちゃん。");
            _touchface_comment_lib.Add("にいちゃん、フルーツは外でしか採れないよ～。");
            _touchface_comment_lib.Add("にいちゃん、たいりょくが０になったら、材料集めはムリぃ～・・。");
        }
        else if (PlayerStatus.player_renkin_lv >= 3 && PlayerStatus.player_renkin_lv < 5) //LV 3~4 大体クレープきたぐらい
        {
            _touchface_comment_lib.Add("にいちゃん。こまったときは、ショップのおねえちゃんにきこう。");
            _touchface_comment_lib.Add("今までにたべたクッキーの枚数をおぼえてる？");
            _touchface_comment_lib.Add("にいちゃん。今日のご飯は、ビールと枝豆の炊き込みご飯だよ♪");
            _touchface_comment_lib.Add("にいちゃん。伝説のお菓子のレシピが・・。どこかにあるらしいよ。");
            _touchface_comment_lib.Add("にいちゃん。新しいおかしをいっぱい作れば、パティシエの腕前が上がるよ！");
        }
    }

    void Init_touchTwintailComment()
    {
        //touchTwintailは、上から順番に表示される。
        _touchtwintail_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0:

                _touchtwintail_comment_lib.Add("うわ");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..さらさら。");
                _touchtwintail_comment_lib.Add("..。");
                break;

            case 1:

                _touchtwintail_comment_lib.Add("..!");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..気持ちいい。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（ちょっと元気になってきたかな？）");
                break;

            case 2:

                _touchtwintail_comment_lib.Add("わっ");
                _touchtwintail_comment_lib.Add("お母さんゆずりで、さらさらなんだよ～。");
                _touchtwintail_comment_lib.Add("お母さん、元気かなぁ～..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 3:

                _touchtwintail_comment_lib.Add("へへ。兄ちゃんに髪触られた♪");
                _touchtwintail_comment_lib.Add("さらさら。気持ちいい。");
                _touchtwintail_comment_lib.Add("うひひ。");
                _touchtwintail_comment_lib.Add("あ～..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 4:

                _touchtwintail_comment_lib.Add("わ～い♪");
                _touchtwintail_comment_lib.Add("髪の毛さらさら。気持ちいい。");
                _touchtwintail_comment_lib.Add("しゃらら～ん");
                _touchtwintail_comment_lib.Add("あ～..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 5:

                _touchtwintail_comment_lib.Add("うわ～い♪");
                _touchtwintail_comment_lib.Add("髪の毛さらさら。気持ちいい。");
                _touchtwintail_comment_lib.Add("しゃらら～ん");
                _touchtwintail_comment_lib.Add("あ～..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 6:

                _touchtwintail_comment_lib.Add("うわ～い♪");
                _touchtwintail_comment_lib.Add("髪の毛さらさら。気持ちいい。");
                _touchtwintail_comment_lib.Add("しゃらら～ん");
                _touchtwintail_comment_lib.Add("あ～..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;
        }
        
    }

    void Init_touchChestComment()
    {
        //胸さわるとき
        _touchchest_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0: //不快な感じ

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("..。");
                _touchchest_comment_lib.Add("..やめて。");

                break;

            case 1: //不快な感じ

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("..。");
                _touchchest_comment_lib.Add("..やめて。");

                break;

            case 2: //不快だけど、そこまで嫌がらない。

                _touchchest_comment_lib.Add("..？");
                _touchchest_comment_lib.Add("..。");
                _touchchest_comment_lib.Add("変なとこ触っちゃだめ！");

                break;

            case 3:

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("..はずかしいよ～。");
                _touchchest_comment_lib.Add("胸をさわるのはやめて、にいちゃん！");
                _touchchest_comment_lib.Add("えっち！にいちゃんのばか！");

                break;

            case 4:

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("兄ちゃんのえっちー！");
                _touchchest_comment_lib.Add("胸をさわるんじゃない！にいちゃん！");
                _touchchest_comment_lib.Add("..あまり胸を触ると、おこるよ！");

                break;

            case 5:

                _touchchest_comment_lib.Add("！！");
                _touchchest_comment_lib.Add("えっち！ばか！！");
                _touchchest_comment_lib.Add("どこさわってんの？にいちゃんのばか！！");
                _touchchest_comment_lib.Add("く、くすぐったい..。");
                _touchchest_comment_lib.Add("ぎゃー！！");
                _touchchest_comment_lib.Add("胸を触るなといっているよ！！にいちゃん！！");

                break;

            case 6:

                _touchchest_comment_lib.Add("！！");
                _touchchest_comment_lib.Add("ばか..！！");
                _touchchest_comment_lib.Add("にいちゃんのおてて、あったか～い♪");
                _touchchest_comment_lib.Add("そんなとこさわられると、くすぐったいよ。にいちゃん～！");
                _touchchest_comment_lib.Add("ぐひぃ～・・。");
                _touchchest_comment_lib.Add("胸を触るのを、やめなさい！あにぃ～。");
                _touchchest_comment_lib.Add("あひぃ～。");
                _touchchest_comment_lib.Add("（ちょっときもちいい。）");
                _touchchest_comment_lib.Add("胸をさわるのは、やめろ！ばか兄！！");
                _touchchest_comment_lib.Add("は、はずかしいよ～。にいちゃん！");
                _touchchest_comment_lib.Add("ぎぃや～～。（くすぐったいよ..。にいちゃん～。）");

                break;
        }

    }

    void Init_touchHandComment()
    {
        _touchhand_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0: //反応があまりない

                _touchhand_comment_lib.Add("..。");
                _touchhand_comment_lib.Add("..。");

                break;

            case 1:

                _touchhand_comment_lib.Add("..おてて。");
                _touchhand_comment_lib.Add("..。");
                break;

            case 2:

                _touchhand_comment_lib.Add("..！");
                _touchhand_comment_lib.Add("..うわ。");
                _touchhand_comment_lib.Add("おてて？");
                break;

            case 3:

                _touchhand_comment_lib.Add("..むむ。");
                _touchhand_comment_lib.Add("..うわ。");
                _touchhand_comment_lib.Add("おてて？");
                _touchhand_comment_lib.Add("あったかい～。");
                break;

            case 4:

                _touchhand_comment_lib.Add("むむ..。");
                _touchhand_comment_lib.Add("おてて、もみもみ。");
                _touchhand_comment_lib.Add("お菓子のにおいする～。");
                break;

            case 5:

                _touchhand_comment_lib.Add("あて！");
                _touchhand_comment_lib.Add("にいちゃん、おててあったか～い。");
                _touchhand_comment_lib.Add("お菓子のにおい。くんくん..。");
                _touchhand_comment_lib.Add("気持ちいい～。");
                break;

            case 6:

                _touchhand_comment_lib.Add("おわ！");
                _touchhand_comment_lib.Add("さわさわ・・。");
                _touchhand_comment_lib.Add("にいちゃん、おててあったか～い。");
                _touchhand_comment_lib.Add("お菓子のにおい。くんくん..。");
                _touchhand_comment_lib.Add("気持ちいい～。");
                _touchhand_comment_lib.Add("にいちゃんのおてて、マッサージ..。もみもみ。");
                _touchhand_comment_lib.Add("くすぐったいよ～");
                break;

            default:

                _touchhand_comment_lib.Add("おわ！");
                _touchhand_comment_lib.Add("さわさわ・・。");
                _touchhand_comment_lib.Add("にいちゃん、おててあったか～い。");
                _touchhand_comment_lib.Add("お菓子のにおい。くんくん..。");
                _touchhand_comment_lib.Add("気持ちいい～。");
                _touchhand_comment_lib.Add("にいちゃんのおてて、マッサージ..。もみもみ。");
                _touchhand_comment_lib.Add("くすぐったいよ～");
                _touchhand_comment_lib.Add("はずかしいよ～、にいちゃん。");
                _touchhand_comment_lib.Add("えへへ。ちょっとパパのにおいする。");
                _touchhand_comment_lib.Add("がぶぅ！！（噛みつかれた！）");
                break;
        }

    }


    //表情を変更するメソッド
    public void face_girl_Normal()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 1; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Joukigen()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 2; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Bad()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 3; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Little_Fine()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 4; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Fine()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 5; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }    

    public void face_girl_Tereru()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 6; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Yorokobi()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 7; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Angry()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 8; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Hirameki()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 9; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Mazui()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 10; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Mazui2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 11; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Iya()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 12; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Surprise()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 13; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Cry()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 14; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Yodare()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 15; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Cry2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 16; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Mazui3()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 17; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Surprise2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 18; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Surprise3()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 19; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Angry2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 20; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_KomariGyagu()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 21; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Mogumogu()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 22; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Mogumogu2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 23; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Mogumogu3()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 24; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Yorokobi2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 25; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_KomariGyagu2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 26; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    //表情パラメータを一旦リセット
    public void face_girl_Reset()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 999; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }


    //ハートレベルアップテーブル
    void Init_Stage1_LVTable()
    {
        stage1_lvTable.Clear();
        stage1_lvTable.Add(20); //LV2。LV1で、次のレベルが上がるまでの好感度値
        stage1_lvTable.Add(70);　//LV3 LV1の分は含めない。
        stage1_lvTable.Add(130); //LV4
        stage1_lvTable.Add(200); //LV5
        stage1_lvTable.Add(350); //LV6
        stage1_lvTable.Add(550); //LV7
        stage1_lvTable.Add(800); //LV8
        stage1_lvTable.Add(1070); //LV9
        stage1_lvTable.Add(1370); //LV10
        stage1_lvTable.Add(1700); //LV11
        stage1_lvTable.Add(2000); //LV12
        stage1_lvTable.Add(2400); //LV13
        stage1_lvTable.Add(2850); //LV14
        stage1_lvTable.Add(3350); //LV15

        //LV16以上～99まで　200ごとに上がるように設定
        for (i=1; i < ( 99 - stage1_lvTable.Count); i++)
        {
            stage1_lvTable.Add(stage1_lvTable[stage1_lvTable.Count-1] + 200);
        }
    }

    //レベルをいれると、それまでに必要な経験値の合計を返すメソッド レベルは１始まり
    public int SumLvTable(int _count)
    {
        _sum = 0;

        for(i=0; i < _count-1; i++)
        {
            _sum += stage1_lvTable[i];
        }

        return _sum;
    }

    public void LvUpStatus() //好感度レベルがあがったときに、ステータス上昇などの処理.GirlEatJudgeから読み出し。
    {
        //レベルがあがるごとに、アイテム発見力があがる。
        /*PlayerStatus.player_girl_findpower = 100 + ((girl1_Love_lv-1) * 10);

        //上限処理
        if(PlayerStatus.player_girl_findpower >= 999)
        {
            PlayerStatus.player_girl_findpower = 999;
        }*/
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}

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

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    //コンテストの判定セット
    private ContestSetDataBase contestSet_database;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private SoundController sc;

    private Special_Quest special_quest;

    private Text questname;
    private GameObject questtitle_panel;

    private Sequence sequence_girlmove;
    private Sequence sequence_girlmove2;

    public float timeOut;  //girleat_judgeから読んでいる
    public float timeOut2; //その他は、デバッグ用に外側からすぐ見れるようにpublicにしてる。
    public float timeOut3;
    public float timeOutHint;
    private float timeOutSec; //1秒ずつ減るカウンタ
    private float Default_hungry_cooltime;
    private float Default_hukidashi_hyoujitime;
    private float Default_hukidashi_nexttime;
    public int timeGirl_hungry_status; //今、お腹が空いているか、空いてないかの状態
    public int touchGirl_status; //今、どこを触っているかの番号
    public int QuestManzoku_counter; //お菓子たべて満足～から何秒で元に戻るかの時間 girleat_judgeから読み出し

    public bool GirlEat_Judge_on;
    public int GirlGokigenStatus; //女の子の現在のご機嫌の状態。6段階ほどあり、好感度が上がるにつれて、だんだん見た目が元気になっていく。
    public int GirlOishiso_Status; //食べたあとの、「おいしそ～」の状態。この状態では、アイドルモーションが少し変化する。

    private GameObject hukidashiPrefab;
    private GameObject canvas;

    public GameObject hukidashiitem;
    private bool hukidashion;
    private Text _text;

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
    public int[] girl1_Tea_Flavor;

    public int[] girl1_SP1_Wind;
    public int[] girl1_SP_Score2;
    public int[] girl1_SP_Score3;
    public int[] girl1_SP_Score4;
    public int[] girl1_SP_Score5;
    public int[] girl1_SP_Score6;
    public int[] girl1_SP_Score7;
    public int[] girl1_SP_Score8;
    public int[] girl1_SP_Score9;
    public int[] girl1_SP_Score10;

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
   
    public bool tween_start;
    private float facemotion_duration;
    private float facemotion_length;
    private float facemotion_time;   
    private Tween weightTween;
    private float facemotion_weight;
    private float Idle_duration;
    private bool facemotion_start;
    private bool make_Idlemotion_start;

    private float rnd;
    private int random;

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

    private List<int> girlRandomEat_List = new List<int>(); //エクストラモード時、レシピから食べたいお菓子ひとつを決めるときのIDリスト


    //女の子の好み組み合わせセットのデータ
    private int Set_compID;
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
    public bool CubismLookFlag; //マウス押したときに、目線が追従するかどうかのON/OFF

    public bool Girl1_touchtwintail_start;
    private int Girl1_touchtwintail_count;
    private bool Girl1_touchtwintail_flag; //全ての会話を表示したら、しばらく触れなくなる

    public bool Girl1_touchchest_start;
    public bool Girl1_touchhand_start;
    public bool Girl1_touchribbon_start;

    //public bool touchanim_start; //タッチしはじめたら、その他のモーションなどを一時的に止める。

    //特定のお菓子か、ランダムから選ぶかのフラグ
    public int OkashiQuest_ID; //特定のお菓子、のお菓子セットのID
    //public string OkashiQuest_Name; //そのときのお菓子のクエストネーム
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
    private int trans_makemotion;
    private GameObject character_root;
    private GameObject character_move;

    //ハートレベルのテーブル
    //public List<int> stage1_lvTable = new List<int>();

    private int _sum;
    private int _noweat_count;
    private int _hlv_last;
    private int hukidashi_number;
    private int _id;

    private Text girl_param;
    private Slider _slider; //好感度バーを取得

    public bool gireleat_start_flag; //食べ始めアニメ開始のスイッチ

    public int motion_layer_num = 1; //モーションのレイヤー番号　Live2DAnimationTriggerからも読む。

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
        //canvas = GameObject.FindWithTag("Canvas");
        hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        //コンテストの判定セットの取得
        contestSet_database = ContestSetDataBase.Instance.GetComponent<ContestSetDataBase>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スペシャルクエストcsの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();
        
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
     
        //好感度レベルのテーブル初期化
        //Init_Stage1_LVTable();                
       
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
        girl1_Tea_Flavor = new int[youso_count];

        girl1_SP1_Wind = new int[youso_count];
        girl1_SP_Score2 = new int[youso_count];
        girl1_SP_Score3 = new int[youso_count];
        girl1_SP_Score4 = new int[youso_count];
        girl1_SP_Score5 = new int[youso_count];
        girl1_SP_Score6 = new int[youso_count];
        girl1_SP_Score7 = new int[youso_count];
        girl1_SP_Score8 = new int[youso_count];
        girl1_SP_Score9 = new int[youso_count];
        girl1_SP_Score10 = new int[youso_count];

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
        Default_hukidashi_hyoujitime = 20.0f;
        Default_hukidashi_nexttime = 5.0f;

        timeOut = Default_hungry_cooltime;
        timeOut2 = 10.0f;
        timeGirl_hungry_status = 1;
        QuestManzoku_counter = 10;

        GirlGokigenStatus = 0;
        GirlOishiso_Status = 0;
        Special_ignore_count = 0;
        special_animatFirst = false;

        tween_start = false;
        facemotion_time = 0.3f;
        facemotion_weight = 0f;
        facemotion_start = false;
        make_Idlemotion_start = false;

        GirlEat_Judge_on = true;
        WaitHint_on = false;
        timeOutHint = 5.0f;
        _noweat_count = 0;

        special_animstart_flag = false;
        special_animstart_endflag = false;
        special_animstart_status = 0;
        special_timeOut = 3.0f;
        

        MazuiStatus = 0;

        touchGirl_status = 0;
        Girl1_touch_end = false;
        CubismLookFlag = false;

        Girl1_touchhair_start = false;      
        Girl1_touchhair_count = 0;
        Girl1_touchhair_status = 0;

        Girl1_touchtwintail_start = false;
        Girl1_touchtwintail_count = 0;
        Girl1_touchtwintail_flag = false;

        Girl1_touchchest_start = false;
        Girl1_touchhand_start = false;
        Girl1_touchribbon_start = false;

        //touchanim_start = false;

        girl_Mazui_flag = false;        


        //ステージごとに、女の子が食べたいお菓子のセットを初期化
        InitializeStageGirlHungrySet(0, 0, 0); //とりあえず0で初期化

    }

    // Update is called once per frame
    void Update () {

        if (GameMgr.Scene_LoadedOn_End) //シーン読み込み完了してから動き出す
        {
            //シーン関係のオブジェクト読み込みをUpdateのタイミングでする。
            if (canvas == null)
            {
                canvas = GameObject.FindWithTag("Canvas");

                switch (GameMgr.Scene_Category_Num)
                {
                    case 10: //メイン調合
                        
                        //カメラの取得
                        main_cam = Camera.main;
                        maincam_animator = main_cam.GetComponent<Animator>();
                        trans = maincam_animator.GetInteger("trans");

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

                        //メイン画面に表示する、現在のクエスト
                        questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();
                        questtitle_panel = canvas.transform.Find("QuestTitlePanel").gameObject;
                        questtitle_panel.SetActive(false);

                        //初期表情の設定
                        CheckGokigen();
                        DefFaceChange();

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

                    case 100: //コンテスト

                        //Live2Dモデルの取得
                        _model_obj = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
                        _model = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove/Hikari_Live2D_3").FindCubismModel();
                        character = GameObject.FindWithTag("Character");
                        live2d_animator = _model_obj.GetComponent<Animator>();

                        GirlEat_Judge_on = false;
                        break;

                    case 1000: //タイトル画面

                        //Live2Dモデルの取得
                        _model_obj = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
                        _model = GameObject.FindWithTag("CharacterRoot").transform.Find("CharacterMove/Hikari_Live2D_3").FindCubismModel();
                        character = GameObject.FindWithTag("Character");
                        live2d_animator = _model_obj.GetComponent<Animator>();

                        GirlEat_Judge_on = false;

                        break;
                }

                //エフェクトプレファブの取得
                Emo_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Emo_Hirameki_Anim");
                Emo_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Emo_Kirari_Anim");
                Emo_effect_Prefab3 = (GameObject)Resources.Load("Prefabs/Emo_Angry_Anim");
            }

            switch (GameMgr.Scene_Category_Num)
            {
                case 10:

                    if (special_animatFirst != true) //ピコンでるまでは触れない
                    {
                        GameMgr.CharacterTouch_ALLOFF = true;
                    }

                    //女の子の今のご機嫌チェック
                    CheckGokigen();

                    //必ず1秒ずつ減るカウンタ
                    timeOutSec -= Time.deltaTime;

                    if (GameMgr.outgirl_Nowprogress)
                    { }
                    else
                    {
                        //trueだと腹減りカウントが進む。
                        if (GirlEat_Judge_on)
                        {
                            timeOut -= Time.deltaTime; //腹減りのカウンタ
                            timeOut2 -= Time.deltaTime; //ランダムでヒントや食べたいお菓子を決定するカウンタ

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
                                    DefFaceChange();
                                }

                                //アイドルモーションの更新
                                IdleMotionReset();

                                _model.GetComponent<CubismEyeBlinkController>().enabled = true;
                            }
                        }
                    }
                    break;
            }
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

            //タッチを終えたら、カウントスタートし、数秒後に元の状態にリセット
            if (Girl1_touch_end) //Touch_Controllからtrueにしている。なので、女の子がいるシーンでないと、この中の処理は走らない。
            {
                WaitHint_on = false;
                timeOut3 -= Time.deltaTime;

                //一定時間がたち、元の状態に戻る。
                if (timeOut3 <= 0.0f)
                {
                    GirlEat_Judge_on = true;

                    _model.GetComponent<CubismEyeBlinkController>().enabled = true;
                    Girl1_touch_end = false;
                    CubismLookFlag = false;

                    //表情をリセット
                    switch (GameMgr.compound_status)
                    {
                        case 4: //調合中のシーン
                            face_girl_Normal();
                            break;

                        default:
                            DefFaceChange();
                            break;
                    }

                    //吹き出し・ハングリーステータスをリセット
                    ResetHukidashi();
                }
            }

            switch (GameMgr.Scene_Category_Num)
            {
                case 10:

                    if (GameMgr.compound_status == 110) //トップ画面のときだけ発動
                    {
                        //一定時間たつと、女の子はお腹がへって、お菓子を欲しがる。
                        if (timeOut <= 0.0f)
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
                                    Girl_EatDecide();

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

                        //一定時間たつとヒントを出すか、アイドルモーションを再生。同時に食べたいものを指定する。
                        if (!GameMgr.tutorial_ON) //チュートリアル中は、ランダムモーションは発生しない
                        {
                            if (timeOut2 <= 0.0f)
                            {
                                rnd = Random.Range(0.0f, 5.0f);
                                timeOut2 = Default_hukidashi_nexttime + rnd;
                                timeGirl_hungry_status = 1; //お腹が空いた状態に切り替え。吹き出しがでる。

                                Girl_EatDecide();

                                Girl1_Hint(Default_hukidashi_hyoujitime); //ランダムセリフ＋モーションを決定する
                            }
                        }

                        

                        //常に1秒をカウントするカウンタ
                        if (timeOutSec <= 0.0f)
                        {
                            timeOutSec = 1.0f;

                            //ピクニック後、余韻のカウンタ
                            if (GameMgr.picnic_after)
                            {
                                GameMgr.picnic_after_time--;

                                if (GameMgr.picnic_after_time <= 0)
                                {
                                    GameMgr.picnic_after = false;
                                }
                            }

                            //お菓子たべたあと満足状態　5～10秒ほどしたら、戻る。
                            if (GameMgr.QuestManzokuFace)
                            {
                                QuestManzoku_counter--; //GirlEat_Judgeでリセットしてる

                                if (QuestManzoku_counter <= 0)
                                {
                                    GameMgr.QuestManzokuFace = false;

                                    DefFaceChange();
                                    //DeleteHukidashi();
                                }
                            }
                        }

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
                    DefFaceChange();                    

                    special_animstart_flag = false;
                    special_animstart_endflag = true;
                    break;
            }

            special_timeOut -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {

        if(facemotion_start)
        {
            facemotion_start = false;
            //モーションが繰り返されるのを防止
            trans_motion = 9999;
            live2d_animator.SetInteger("trans_motion", trans_motion);
            
        }

        if(make_Idlemotion_start)
        {
            make_Idlemotion_start = false;
            trans_makemotion = 9999;
            live2d_animator.SetInteger("trans_makemotion", trans_makemotion);
        }
    }

    //Facemotionを強制的にOFF　Compound_Main,GirlEatJudgeなどからも読まれる。
    public void AddMotionAnimReset()
    {
        weightTween.Kill();
        tween_start = false;
        _model.GetComponent<CubismEyeBlinkController>().enabled = true;
    }

    //普段の素の状態での表情変化 GameMgr.girl_expressionがまあまあのときは、ハートレベルに応じて、素の状態が変化する。Compound_Main・GirlEat_Judgeからも読む。
    public void DefFaceChange()
    {
        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
        live2d_animator = _model.GetComponent<Animator>();
        trans_expression = live2d_animator.GetInteger("trans_expression");

        Debug.Log("DefFaceChange() 表情リセット");        

        switch (PlayerStatus.player_girl_expression)
        {
            case 1: //まずいもの食べた直後などの最悪の表情

                face_girl_Angry2();
                break;


            default:

                if (GameMgr.picnic_after) //ピクニック後の余韻の表情
                {
                    face_girl_Metoji();
                }
                else
                {
                    if (GameMgr.QuestManzokuFace) //そのお菓子をクリアしたあとの表情
                    {
                        AfterOkashiDefaultFace();
                    }
                    else
                    {
                        DefaultFace();
                    }
                }
                break;
        }
        

    }

    public void CheckGokigen()　//Updateで常にチェック　Title_Main.csからも読まれる
    {
        //女の子の今のご機嫌　ハートレベルに応じた絶対的なもの
        if (PlayerStatus.girl1_Love_lv >= 1 && PlayerStatus.girl1_Love_lv < 2) // HLv
        {
            //テンションが低すぎて暗い
            GirlGokigenStatus = 0; //1と一緒
           
        }
        else if (PlayerStatus.girl1_Love_lv >= 2 && PlayerStatus.girl1_Love_lv < 3) //
        {
            //少し機嫌が悪い
            GirlGokigenStatus = 2;
           
        }
        else if (PlayerStatus.girl1_Love_lv >= 3 && PlayerStatus.girl1_Love_lv < 4) //
        {
            //少し機嫌が悪い
            GirlGokigenStatus = 3;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 4 && PlayerStatus.girl1_Love_lv < 20) //
        {
            //ちょっと元気でてきた
            GirlGokigenStatus = 4;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 20 && PlayerStatus.girl1_Love_lv < 35) //
        {
            //元気
            GirlGokigenStatus = 5;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 35 && PlayerStatus.girl1_Love_lv < 40) //
        {
            //最高に上機嫌
            GirlGokigenStatus = 6;
            
        }
        else if (PlayerStatus.girl1_Love_lv >= 40 && PlayerStatus.girl1_Love_lv < 45) //
        {
            //最高に上機嫌
            GirlGokigenStatus = 7;

        }
        else if (PlayerStatus.girl1_Love_lv >= 45 && PlayerStatus.girl1_Love_lv < 50)
        {
            //最高に上機嫌
            GirlGokigenStatus = 8;

        }
        else if (PlayerStatus.girl1_Love_lv >= 50 && PlayerStatus.girl1_Love_lv < 60)
        {
            //最高に上機嫌
            GirlGokigenStatus = 9;

        }
        else if (PlayerStatus.girl1_Love_lv >= 60 && PlayerStatus.girl1_Love_lv < 70)
        {
            //最高に上機嫌
            GirlGokigenStatus = 10;

        }
        else if (PlayerStatus.girl1_Love_lv >= 70 && PlayerStatus.girl1_Love_lv < 80)
        {
            //最高に上機嫌
            GirlGokigenStatus = 11;

        }
        else if (PlayerStatus.girl1_Love_lv >= 80 && PlayerStatus.girl1_Love_lv < 90)
        {
            //あたたかい安心
            GirlGokigenStatus = 12;

        }
        else if (PlayerStatus.girl1_Love_lv >= 90)
        {
            //あたたかい安心
            GirlGokigenStatus = 13;

        }
    }

    //お菓子を食べる前の、デフォルトの状態。Debug_Panel・Title_Main.csからも読み出し。
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
                face_girl_Bad();
                break;

            case 3:
                face_girl_Little_Fine();
                break;

            case 4:
                face_girl_Normal();
                break;

            case 5:
                face_girl_Normal();
                break;

            case 6:
                face_girl_Joukigen();
                break;

            case 7:
                face_girl_Tereru4();
                break;

            case 8:
                face_girl_Tereru4();
                break;

            case 9:
                face_girl_Joukigen();
                //face_girl_Joukigen2();
                break;

            default:
                face_girl_Tereru4();
                break;
        }
    }

    //お菓子に満足したあとの表情。基本的に喜んでいる。好感度によって、少し差があるにしてもいいかも？
    public void AfterOkashiDefaultFace()
    {
        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
        live2d_animator = _model.GetComponent<Animator>();
        trans_expression = live2d_animator.GetInteger("trans_expression");

        face_girl_Fine();
    }


    //女の子が食べたいものの決定。Save_Controllerからも呼び出し。
    public void Girl_EatDecide()
    {
        //デフォルトで１に設定。セット組み合わせの処理にいったときに、２や３に変わる。
        Set_Count = 1;


        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);
        }

        //テーブルの決定
        if (GameMgr.tutorial_ON)
        {  }
        else
        {
            switch (GameMgr.EatOkashi_DecideFlag) //0だとメインクエストで食べたいお菓子固定。
            {
                case 0: //特定の課題お菓子。メインシナリオ。

                    //食べたいものはSpcial_Questで決定。ここでは吹き出しだけ設定してる。OkashiQuest_IDをSpecial_Quest.csから選択している。
                    //OkashiQuest_ID = compIDを指定すると、女の子が食べたいお菓子＜組み合わせ＞がセットされる。
                    //Set_compID = OkashiQuest_ID;                
                    SetQuestHukidashiText(OkashiQuest_ID, 0);

                    if (special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート
                    {
                        //一度ドアップになり、電球がキラン！　→　そのあと、クエストの吹き出し。最初の一回だけ。
                        StartCoroutine("Special_StartAnim");
                    }

                    break;

                case 1: //ランダムで食べる

                    //ランダムで食べたいお菓子決める
                    RandomEatOkashiDecide();

                    if (special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート
                    {                        

                        //Debug.Log("エクストラモード　電球ピコ");
                        //一度ドアップになり、電球がキラン！　→　そのあと、クエストの吹き出し。最初の一回だけ。
                        StartCoroutine("Special_StartAnim");
                    }
                    break;

                case 100: //チュートリアル用の回避

                    break;
              
                default:
                    break;
            }
        }
    }

    void RandomEatOkashiDecide()
    {
        GameMgr.RandomEatOkashi_counter++;

        if (GameMgr.RandomEatOkashi_counter >= 3)
        {
            GameMgr.RandomEatOkashi_counter = 0;

            RandomOkashiDecideMethod(); //新しく食べたいお菓子を設定しなおす
        }
        else
        {
            if(GameMgr.NowEatOkashiID == 0) //まだ設定される前にこっちが呼ばれると、小麦粉に多分なっているので、ねこクッキーをデフォにしとく
            {
                GameMgr.NowEatOkashiID = database.SearchItemIDString("neko_cookie");
            }
            SetQuestHukidashiText(GameMgr.NowEatOkashiID, 1);
        }
    }

    //girlEatJudgeからも読まれる。直接この処理に入れば、すぐに食べたいお菓子を変更できる。
    public void RandomOkashiDecideMethod()
    {
        //ランダムでおぼえたレシピから一つ、食べたいお菓子がきまる。
        girlRandomEat_List.Clear();
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            if (databaseCompo.compoitems[i].cmpitem_flag == 1 && databaseCompo.compoitems[i].recipi_count == 1)
            {
                if (database.items[database.SearchItemIDString(databaseCompo.compoitems[i].cmpitemID_result)].itemType.ToString() == "Okashi")
                {
                    _id = database.SearchItemIDString(databaseCompo.compoitems[i].cmpitemID_result);
                    if (database.items[_id].itemName == "shishamo_cookie" || database.items[_id].itemName == "shishamo_crepe" ||
                       database.items[_id].itemName == "murasaki_mushroom_cookie" ||
                       database.items[_id].itemType_subB == "a_Cake_Mat" || database.items[_id].itemType_subB == "a_CookieSource")
                    { }
                    else
                    {
                        girlRandomEat_List.Add(database.SearchItemIDString(databaseCompo.compoitems[i].cmpitemID_result));
                        //Debug.Log("databaseCompo.compoitems[i].cmpitemID_result: " + databaseCompo.compoitems[i].cmpitemID_result);
                    }

                }
            }
        }

        random = Random.Range(0, girlRandomEat_List.Count);
        if (girlRandomEat_List.Count > 0)
        {
            SetQuestHukidashiText(girlRandomEat_List[random], 1);
        }
        else //手帳レシピ０の場合、ねこクッキー
        {
            SetQuestHukidashiText(database.SearchItemIDString("neko_cookie"), 1);
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
            GameMgr.CharacterTouch_ALLOFF = true;

            GameMgr.compound_select = 1000; //シナリオイベント読み中の状態
            GameMgr.compound_status = 1000;
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
            GameMgr.CharacterTouch_ALLOFF = true;

            while (!GameMgr.camerazoom_endflag)
            {
                yield return null;
            }
            GameMgr.camerazoom_endflag = false;

            //最初にお菓子にまつわるヒントやお話。宴へとぶ。SpOkashiBeforeコメント。
            
            if (GameMgr.Story_Mode == 0)
            {
                GameMgr.sp_okashi_ID = OkashiQuest_ID; //GirlLikeCompoSetの_set_compIDが入っている。
            }
            else //エクストラで呼び出すシナリオ。処理的には、同じ。
            {
                //GameMgr.sp_okashi_ID = 10000;
                GameMgr.sp_okashi_ID = OkashiQuest_ID; //GirlLikeCompoSetの_set_compIDが入っている。
            }
            //Debug.Log("OkashiQuest_ID: " + OkashiQuest_ID);

            GameMgr.scenario_ON = true;
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
        //questname.text = OkashiQuest_Name;

        questtitle_panel.SetActive(true);

        special_animatFirst = true;
        GirlEat_Judge_on = true;
    }

    //チュートリアルで使用
    public void SetOneQuest(int _ID)
    {
        InitializeStageGirlHungrySet(_ID, 0, 0);　//comp_Numの値を直接指定

        Set_Count = 1;
        //Set_compID = _ID;

        //テキストの設定。直接しているか、セット組み合わせエクセルにかかれたキャプションのどちらかが入る。
        _desc = girllike_desc[0];
    }


    //クエストごとの食べたいお菓子吹き出しテキストの設定
    void SetQuestHukidashiText(int _ID, int _status)
    {
        switch (_status)
        {
            case 0: //メイン

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

                if (_ID == 1120) //幻の青色紅茶
                {
                    _desc = "にいちゃん！　" + girlLikeCompo_database.girllike_composet[_compID].desc + "がのみたい！";
                }
                else
                {
                    _desc = "にいちゃん！　" + girlLikeCompo_database.girllike_composet[_compID].desc + "が食べたい！";
                }
                GameMgr.NowEatOkashiName = girlLikeCompo_database.girllike_composet[_compID].desc;

                break;

            case 1: //ランダム

                if (database.items[_ID].itemType_sub.ToString() == "Coffee" || database.items[_ID].itemType_sub.ToString() == "Coffee_Mat" ||
                   database.items[_ID].itemType_sub.ToString() == "Juice" || database.items[_ID].itemType_sub.ToString() == "Tea" ||
                   database.items[_ID].itemType_sub.ToString() == "Tea_Mat" || database.items[_ID].itemType_sub.ToString() == "Tea_Potion")
                {
                    _desc = "にいちゃん！　" + database.items[_ID].itemNameHyouji + "がのみたい！";
                }
                else
                {
                    _desc = "にいちゃん！　" + database.items[_ID].itemNameHyouji + "が食べたい！";
                }

                GameMgr.NowEatOkashiName = database.items[_ID].itemNameHyouji;
                GameMgr.NowEatOkashiID = _ID;

                Debug.Log("にいちゃん！　" + database.items[_ID].itemNameHyouji + "が食べたい！（or のみたい！）");
                break;
        }

        //クエストネームパネルのほうも表示更新する
        special_quest.RedrawQuestName();
    }

    //girl_eatJudgeから設定
    public void SetQuestRandomSet(int _ID)
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

        set1_ID = girlLikeCompo_database.girllike_composet[_compID].set1;
        set2_ID = girlLikeCompo_database.girllike_composet[_compID].set2;
        set3_ID = girlLikeCompo_database.girllike_composet[_compID].set3;

       
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
            InitializeStageGirlHungrySet(set_ID[count], count, 0);

        }
    }


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

    public void ResetHukidashiNoSound()
    {
        //一度吹き出しを削除し、クエスト吹き出しなどを表示する。
        timeOut = Default_hungry_cooltime;
        timeOut2 = 10.0f;
        timeGirl_hungry_status = 0;

        DeleteHukidashiOnly();
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

        switch (GameMgr.Scene_Category_Num)
        {
            case 10:

                //まだ一度も調合していない
                if (PlayerStatus.First_recipi_on != true)
                {
                    IdleMotionHukidashiSetting(400);
                }
                else
                {

                    //ヒントをだすか、今食べたいもののどちらかを表示する。3連続で食べたいものが表示されていないなら、4つめは次は必ず食べたいものを表示する。
                    if (_noweat_count >= 3)
                    {
                        _noweat_count = 0;

                        if (GameMgr.EatOkashi_DecideFlag == 1) //ランダムで食べるフラグがたってるときにランダムで選ぶ special_questで設定してる
                        {
                            RandomEatOkashiDecide();
                        }                       

                        if (hukidashiitem == null)
                        {
                            hukidasiInit(_temptimehint);
                        }
                        FaceMotionPlay(1014);                       
                        hukidashiitem.GetComponent<TextController>().SetTextColorPink(_desc);
                    }
                    else
                    {
                        if (PlayerStatus.player_girl_expression == 1) //まずいのあとは、怒ってとりとめのない会話がなくなる。
                        {
                            IdleMotionHukidashiSetting(410);
                        }
                        else
                        {
                            if (GameMgr.picnic_after)
                            {
                                IdleMotionHukidashiSetting(420);
                            }
                            else
                            {
                                if (GameMgr.QuestManzokuFace)
                                {
                                    IdleMotionHukidashiSetting(430);

                                    //表情喜びに。5秒ほどしてすぐ戻す。
                                    face_girl_Yorokobi();
                                }
                                else
                                {
                                    //ランダムでヒント内容を出す。 or 今食べたいものをしゃべる。口をタッチしたときと一緒のコメント。
                                    random = Random.Range(0, 100);
                                    if (random >= 0 && random < 50)
                                    {
                                        _noweat_count++;

                                        if (GameMgr.hikari_makeokashi_startflag) //もしヒカリお菓子作り中なら、25%ぐらいで、またグルグルのアイドルに戻る
                                        {
                                            if (random >= 0 && random < 25)
                                            {
                                                random = Random.Range(0, 2);
                                                switch (random)
                                                {
                                                    case 0:

                                                        FaceMotionPlay(1022);
                                                        break;

                                                    case 1:

                                                        FaceMotionPlay(1025);
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                IdleChange(); //ランダムモーション＋ヒントを決定
                                            }
                                        }
                                        else
                                        {
                                            IdleChange(); //ランダムモーション＋ヒントを決定
                                        }
                                    }
                                    else
                                    {
                                        _noweat_count++;
                                        IdleChange(); //ランダムモーション＋ヒントを決定
                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case 100: //コンテスト中

                random = Random.Range(0, 100);
                if (random >= 0 && random < 50)
                {
                    random = Random.Range(0, 2);
                    switch (random)
                    {
                        case 0:

                            FaceMotionPlay(1022);
                            break;

                        case 1:

                            FaceMotionPlay(1025);
                            break;
                    }
                }
                else
                {
                    IdleMotionHukidashiSetting(1000); //吹き出しも一緒に生成
                }
                break;

            case 1000: //タイトルのときのセリフ

                random = Random.Range(0, 100);
                if (random < 20)
                {                   
                    IdleMotionHukidashiSetting(440);
                }
                else
                {
                    IdleChange();
                }
                break;
        }

    }

    //アニメーションをアイドル状態に戻す。Compound_Mainからも読まれる。
    public void IdleMotionReset()
    {
        if (GameMgr.ResultComplete_flag != 0) //厨房から帰ってくるときアニメ再生中は、こっちは動かさないようにする。
        {  }
        else
        {
            //Idleにリセット
            if (!GameMgr.hikari_makeokashi_startflag)
            {
                live2d_animator.Play("Idle", motion_layer_num, 0.0f);
                //make_Idlemotion_start = true;
                //trans_makemotion = 10; //Idleにリセット
                //live2d_animator.SetInteger("trans_makemotion", trans_makemotion);
            }
            else
            {
                //ヒカリがお菓子作り中の場合のIdle　この場合のみ、trans_makemotionで指定しているので、trans_motionとの間違いに気を付ける。
                if (PlayerStatus.girl1_Love_lv < 80)
                {
                    switch(PlayerStatus.player_girl_expression)
                    {
                        case 1: //怒り

                            trans_makemotion = 500;
                            break;

                        case 2: //不機嫌
                            trans_makemotion = 400;
                            break;

                        default:

                            //live2d_animator.Play("Idle_hikariMake", motion_layer_num, 0.0f);
                            trans_makemotion = 100;
                            break;
                    }

                    make_Idlemotion_start = true;
                    live2d_animator.SetInteger("trans_makemotion", trans_makemotion);
                }
                else
                {
                    switch (PlayerStatus.player_girl_expression)
                    {
                        case 1: //怒り

                            trans_makemotion = 500;
                            break;

                        case 2: //不機嫌
                            trans_makemotion = 400;
                            break;

                        default:

                            random = Random.Range(0, 10);
                            if (random >= 0 && random < 4)
                            {
                                //live2d_animator.Play("Idle_hikariMake", motion_layer_num, 0.0f);                              
                                trans_makemotion = 100;
                                
                            }
                            else if (random >= 4 && random < 7)
                            {
                                //live2d_animator.Play("Idle_hikariMake2", motion_layer_num, 0.0f); //ヤムチャの歌をうたいながら
                                trans_makemotion = 200;
                            }
                            else if (random >= 7 && random < 10)
                            {
                                //live2d_animator.Play("Idle_hikariMake3", motion_layer_num, 0.0f); //棒目で上機嫌
                                trans_makemotion = 300;
                            }
                            break;
                    }

                    make_Idlemotion_start = true;
                    live2d_animator.SetInteger("trans_makemotion", trans_makemotion);
                }
            }

            _model.GetComponent<CubismEyeBlinkController>().enabled = true;
        }
    }


    public void hukidashiReturnHome()
    {
        if (GameMgr.outgirl_Nowprogress)
        {
            GirlOishiso_Status = 0;
        }
        else
        {
            hukidasiInit(Default_hukidashi_hyoujitime);

            if (PlayerStatus.player_girl_expression >= 3)
            {
                hukidashiitem.GetComponent<TextController>().SetText("おいしそ～♪");
            }
            else
            {
                hukidashiitem.GetComponent<TextController>().SetText("..ごくり。");
            }
        }
    }

    public void hukidashiOkashiFailedReturnHome()
    {
        hukidasiInit(Default_hukidashi_hyoujitime);

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
        _text = hukidashiitem.transform.Find("hukidashi_Pos/hukidashi_Text").GetComponent<Text>();

        //音を鳴らす
        sc.PlaySe(7);
        

        //15秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = _timehint;
        GirlEat_Judge_on = false;
    }

    //吹きだし生成　こっちは、メッセージも外から入れる。
    public void hukidashiMessage(string _msg)
    {
        hukidasiInit(Default_hukidashi_hyoujitime);
        _text.text = _msg;
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
    
    public void GirlEatJudgecounter_OFF() //外部compound_mainなどから、一時的に、腹減りカウンタをストップする
    {
        //一時的に腹減りを止める。
        GirlEat_Judge_on = false;
        WaitHint_on = false;
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

    public void InitializeStageGirlHungrySet(int _id, int _set_num, int _mstatus)
    {
        //IDをセット。「compNum」の値で指定する。

        //compNumの値で指定しているので、IDに変換する。
        j = 0;
        while (j < girlLikeSet_database.girllikeset.Count)
        {
            if (_id == girlLikeSet_database.girllikeset[j].girlLike_compNum)
            {
                Debug.Log("girlLikeSet_database.girllikeset[j].girlLike_compNum: " + girlLikeSet_database.girllikeset[j].girlLike_compNum);
                Debug.Log("j :" + j);
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

        if (_mstatus == 0) //通常
        {
            girl1_Beauty[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_beauty;
        }
        else //コンテストで女の子好み使用する場合　3人目の審査員はbeautyの判定をしない 2人目の審査員は少し判定値高くなる
        {
            if (_set_num == 2)
            {
                girl1_Beauty[_set_num] = 0;
            }
            else if (_set_num == 1)
            {
                girl1_Beauty[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_beauty + 20;
            }
            else
            {
                girl1_Beauty[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_beauty;
            }
        }
        girl1_Tea_Flavor[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_tea_flavor;

        girl1_SP1_Wind[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp1_Wind;
        girl1_SP_Score2[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score2;
        girl1_SP_Score3[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score3;
        girl1_SP_Score4[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score4;
        girl1_SP_Score5[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score5;
        girl1_SP_Score6[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score6;
        girl1_SP_Score7[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score7;
        girl1_SP_Score8[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score8;
        girl1_SP_Score9[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score9;
        girl1_SP_Score10[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sp_Score10;

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
    //コンテスト判定用
    //
    public void InitializeStageContestJudgeSet(int _id, int _set_num)
    {
        //IDをセット。「compNum」の値で指定する。

        //compNumの値で指定しているので、IDに変換する。
        j = 0;
        while (j < contestSet_database.contest_set.Count)
        {
            if (_id == contestSet_database.contest_set[j].girlLike_compNum)
            {
                //Debug.Log("contestSet_database.contest_set[j].girlLike_compNum: " + contestSet_database.contest_set[j].girlLike_compNum);
                //Debug.Log("リストの配列番号 j :" + j);
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

            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[0])
            {

                if (contestSet_database.contest_set[setID].girlLike_topping[0] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[0]);
                    girl1_hungrytoppingNumberSet.Add(1);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[1])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[1] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[1]);
                    girl1_hungrytoppingNumberSet.Add(2);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[2])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[2] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[2]);
                    girl1_hungrytoppingNumberSet.Add(3);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[3])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[3] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[3]);
                    girl1_hungrytoppingNumberSet.Add(4);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[4])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[4] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[4]);
                    girl1_hungrytoppingNumberSet.Add(5);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[5])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[5] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[5]);
                    girl1_hungrytoppingNumberSet.Add(6);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[6])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[6] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[6]);
                    girl1_hungrytoppingNumberSet.Add(7);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[7])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[7] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[7]);
                    girl1_hungrytoppingNumberSet.Add(8);
                }
            }
            if (slotnamedatabase.slotname_lists[i].slotName == contestSet_database.contest_set[setID].girlLike_topping[8])
            {
                if (contestSet_database.contest_set[setID].girlLike_topping[8] != "Non")
                {
                    girl1_hungrySet.Add(i);
                    girl1_hungrytoppingSet.Add(contestSet_database.contest_set[setID].girlLike_topping_score[8]);
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
        girl1_NonToppingScoreSet[_set_num] = contestSet_database.contest_set[setID].girlLike_Non_topping_score;

        //②味のパラメータ。これに足りてないと、「甘さが足りない」といったコメントをもらえる。
        girl1_Rich[_set_num] = contestSet_database.contest_set[setID].girlLike_rich;
        girl1_Sweat[_set_num] = contestSet_database.contest_set[setID].girlLike_sweat;
        girl1_Sour[_set_num] = contestSet_database.contest_set[setID].girlLike_sour;
        girl1_Bitter[_set_num] = contestSet_database.contest_set[setID].girlLike_bitter;

        girl1_Crispy[_set_num] = contestSet_database.contest_set[setID].girlLike_crispy;
        girl1_Fluffy[_set_num] = contestSet_database.contest_set[setID].girlLike_fluffy;
        girl1_Smooth[_set_num] = contestSet_database.contest_set[setID].girlLike_smooth;
        girl1_Hardness[_set_num] = contestSet_database.contest_set[setID].girlLike_hardness;
        girl1_Chewy[_set_num] = contestSet_database.contest_set[setID].girlLike_chewy;
        girl1_Jiggly[_set_num] = contestSet_database.contest_set[setID].girlLike_jiggly;
        girl1_Juice[_set_num] = contestSet_database.contest_set[setID].girlLike_juice;

        girl1_Beauty[_set_num] = contestSet_database.contest_set[setID].girlLike_beauty;
        girl1_Tea_Flavor[_set_num] = contestSet_database.contest_set[setID].girlLike_tea_flavor;

        girl1_SP1_Wind[_set_num] = contestSet_database.contest_set[setID].girlLike_sp1_Wind;
        girl1_SP_Score2[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score2;
        girl1_SP_Score3[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score3;
        girl1_SP_Score4[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score4;
        girl1_SP_Score5[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score5;
        girl1_SP_Score6[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score6;
        girl1_SP_Score7[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score7;
        girl1_SP_Score8[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score8;
        girl1_SP_Score9[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score9;
        girl1_SP_Score10[_set_num] = contestSet_database.contest_set[setID].girlLike_sp_Score10;

        //③お菓子の種類：　空＝お菓子はなんでもよい　か　クッキー
        girl1_likeSubtype[_set_num] = contestSet_database.contest_set[setID].girlLike_itemSubtype;

        //④特定のお菓子が食べたいかを決定。関係性は、④＞③。
        //④が決まった場合、③は無視し、①と②だけ計算する。④が空=Nonの場合、③を計算。④も③も空の場合、お菓子の種類は関係なくなる。
        girl1_likeOkashi[_set_num] = contestSet_database.contest_set[setID].girlLike_itemName;

        //セットごとの固有の味の採点値をセット
        girl1_like_set_score[_set_num] = contestSet_database.contest_set[setID].girlLike_set_score;

        //コメントをセット
        girllike_desc[_set_num] = contestSet_database.contest_set[setID].set_kansou;

        //お菓子食べた後の感想用フラグのセット
        girllike_comment_flag[_set_num] = contestSet_database.contest_set[setID].girlLike_comment_flag;

        //compNumの番号も保存。どの判定用お菓子セットを選んだかがわかる。
        girllike_judgeNum[_set_num] = _id;

        //Debug.Log("girl1_Bitter: " + "[" + _set_num + "]" + girl1_Bitter[_set_num]);
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
        CubismLookFlag = true; //目線追従する。
        //touchanim_start = true;
        GirlEat_Judge_on = false;

        //一回タッチするだけだと、「いてっ」って感じの反応
        touch_startreset();

        //タップモーション　ランダムで決定
        Random_TapMotion();
      
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

                //表情変化３
                if (GirlGokigenStatus >= 4)
                {
                    HairTouch_Motion3();
                }

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
                live2d_animator.SetInteger("trans_nade", 20);

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
                live2d_animator.SetInteger("trans_nade", 21);

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

        switch (GirlGokigenStatus)
        {
            case 0:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            case 1:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            case 2:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            case 3:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            case 4:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            case 5:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            case 6:

                live2d_animator.SetInteger("trans_nade", 5);
                break;

            default:

                live2d_animator.SetInteger("trans_nade", 5);
                break;
        }
        
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
        touch_startreset();

        Girl1_touchtwintail_count = 0;
        Girl1_touchtwintail_start = true;
        CubismLookFlag = true; //目線追従する。

        //タップモーション
        live2d_animator.Play("tapmotion_01", motion_layer_num, 0.0f); //tapmotion_01は、頭なでなで・ツインテール共通のモーション タップ系は、.Playですぐに再生で問題ない
        live2d_animator.SetInteger("trans_tap", 0);
    }

    //ツインテール　ドラッグで触り続けた場合
    public void TouchSisterTwinTail()
    {
        touch_startreset();
        Init_touchTwintailComment();

        //コメント順番に表示
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

        if(Girl1_touchtwintail_count >= 4)
        {
            if (GirlGokigenStatus >= 5 && GirlGokigenStatus < 6)
            {
                //鼻歌に遷移
                live2d_animator.SetInteger("trans_tap", 10);
            }
            else if (GirlGokigenStatus >= 6)
            {
                //鼻歌に遷移
                live2d_animator.SetInteger("trans_tap", 11);
            }

        }
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
            //ランダムで吹き出しの内容を出し、モーション。
            Girl1_Hint(Default_hukidashi_hyoujitime);
        }

    }


    //リボン
    public void TouchRibbon_Start()
    {
        touch_startreset();

        Girl1_touchchest_start = true;
        CubismLookFlag = true; //目線追従する。

        //タップモーション
        live2d_animator.Play("tapmotion_03_1", motion_layer_num, 0.0f);

    }

    public void TouchSisterRibbon()
    {        
        //コメントランダム
        //random = Random.Range(0, _touchface_comment_lib.Count);
        //_touchface_comment = _touchface_comment_lib[random];

        hukidashiitem.GetComponent<TextController>().SetText("お母さんが誕生日にくれたリボンだよ～。うひひ。");

        //タップモーション
        live2d_animator.Play("tapmotion_03_1", motion_layer_num, 0.0f);

    }

    //手
    public void TouchHand_Start()
    {
        touch_startreset();

        Girl1_touchchest_start = true;
        CubismLookFlag = true; //目線追従する。

        //タップモーション
        live2d_animator.Play("tapmotion_03_1", motion_layer_num, 0.0f);
    }

    public void TouchSisterHand()
    {

        //吹き出し内容の決定
        Init_touchHandComment();

        random = Random.Range(0, _touchhand_comment_lib.Count);
        _touchhand_comment = _touchhand_comment_lib[random];

        hukidashiitem.GetComponent<TextController>().SetText(_touchhand_comment);
    }

    //胸
    public void TouchChest_Start()
    {
        touch_startreset();

        Girl1_touchchest_start = true;
        CubismLookFlag = true; //目線追従する。

        //タップモーション　最初触った一回だけ発動        
        live2d_animator.Play("tapmotion_02", motion_layer_num, 0.0f);
        
    }

    public void TouchSisterChest()
    {       
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

        random = Random.Range(0, 3);

        switch(random)
        {
            case 0:

                live2d_animator.Play("tapmotion_03_1", motion_layer_num, 0.0f);
                hukidashiitem.GetComponent<TextController>().SetText("うわっ！");
                break;

            case 1:

                live2d_animator.Play("tapmotion_03_2", motion_layer_num, 0.0f);
                hukidashiitem.GetComponent<TextController>().SetText("あいたっ！");
                break;

            case 2:

                live2d_animator.Play("tapmotion_03_3", motion_layer_num, 0.0f);
                hukidashiitem.GetComponent<TextController>().SetText("いてぃっ！");
                break;
        }
    }

    void touch_startreset() //触り始め共通でリセットする項目。
    {
        
        if (hukidashiitem == null)
        {
            hukidasiInit(Default_hukidashi_hyoujitime);
        }
        /*
        weightTween.Kill(); //フェードアウト中なら中断する
        tween_start = false;*/
    }   

    //ランダムで左右に動く 現在未使用
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

    //移動した位置を元に戻す。
    public void ResetCharacterPosition()
    {
        character_move.transform.DOMoveX(0, 0.0f);
    }

    //FaceMotionの数字を入れると、それを再生。かつ再生フラグもたてる。.Playを使うよりも、アニメの遷移をなめらかにする処理。Debug_Panelからも読み込み。
    public void FaceMotionPlay(int _trans_motion)
    {
        _model.GetComponent<CubismEyeBlinkController>().enabled = false;

        trans_motion = _trans_motion;
        live2d_animator.SetInteger("trans_motion", trans_motion);
        facemotion_start = true;
        timeOut2 = 35.0f; //次のヒント発生タイミングを、毎回、モーション再生ごとにリセット
    }

    //ランダムで仕草　ランダムモーションor口をタップしたときの共通　どのモーションを再生するか＋セリフを決定　モーションがなくても、セリフだけは表示される。
    void IdleChange()
    {      
        //Debug.Log("ランダムモーション　再生");
        hukidashi_number = 0;

        if (GirlGokigenStatus >= 0 && GirlGokigenStatus < 4)
        {
            switch (GirlGokigenStatus)
            {
                case 0: //沈んでいる.. 0と1は一緒なので、0を設定する。

                    random = Random.Range(0, 4); //0~2     
                    hukidashi_number = 3;
                    break;

                case 1:

                    random = Random.Range(0, 4); //0~2
                    hukidashi_number = 3;
                    break;

                case 2: //少し機嫌がよくなってきた？けど、まだ暗い。

                    random = Random.Range(0, 4); //0~2
                    hukidashi_number = 3;
                    break;

                case 3: //少し機嫌がよくなってきた？けど、まだ暗い。～LV5

                    random = Random.Range(0, 4); //0~2
                    hukidashi_number = 23;
                    break;
            }

            switch (random) //モーション4種類＋セリフがそれらにつく
            {
                case 0:

                    //モーション1種類
                    FaceMotionPlay(1003);
                    IdleMotionHukidashiSetting(1); //吹き出しも一緒に生成
                    break;

                case 1:

                    IdleMotionHukidashiSetting(100); //吹き出しも一緒に生成
                    break;

                case 2:

                    //おなかへった..
                    Debug.Log("おなかへった");

                    //はらへり
                    if (GameMgr.Story_Mode == 0)
                    {
                        //おなかへった..                    
                        IdleMotionHukidashiSetting(hukidashi_number);
                    }
                    else
                    {
                        //満腹度30以下で発生
                        if (PlayerStatus.player_girl_manpuku <= 30)
                        {
                            //おなかへった..                    
                            IdleMotionHukidashiSetting(hukidashi_number);
                        }
                        else
                        {
                            Debug.Log("ヒント");
                            IdleMotionHukidashiSetting(100);
                        }
                    }
                    
                    break;

                case 3:

                    IdleMotionHukidashiSetting(100); //吹き出しも一緒に生成
                    break;
            }
        }
        else if (GirlGokigenStatus >= 4)
        {
            switch (GirlGokigenStatus)
            {
                case 4:

                    random = Random.Range(0, 9); //0~7 右の数字は含まない
                    hukidashi_number = 20;
                    break;

                case 5:

                    random = Random.Range(0, 10); 
                    hukidashi_number = 30;
                    break;

                case 6:

                    random = Random.Range(0, 11); 
                    hukidashi_number = 40;                    
                    break;

                case 7:

                    random = Random.Range(0, 12);
                    hukidashi_number = 40;
                    break;

                case 8: //25~50

                    random = Random.Range(0, 12);
                    hukidashi_number = 40;
                    break;

                case 9: //50~ だんだん好きって言い始める

                    random = Random.Range(0, 13);
                    hukidashi_number = 50;
                    break;

                case 10: //60~ 

                    random = Random.Range(0, 14);
                    hukidashi_number = 50;
                    break;

                case 11: //70~

                    random = Random.Range(0, 15);
                    hukidashi_number = 60;
                    break;

                case 12: //80~

                    random = Random.Range(0, 16);
                    hukidashi_number = 60;
                    break;

                case 13: //90~

                    random = Random.Range(0, 17);
                    hukidashi_number = 60;
                    break;

                default: //それ以上

                    random = Random.Range(0, 17); 
                    hukidashi_number = 60;
                    break;
            }

            switch (random) //モーション決定＋セリフがそれらにつく
            {
                case 0:

                    //Gokigenに合わせた固有のセリフ
                    Debug.Log("0 Gokigenに合わせた固有のセリフ");
                    IdleMotionHukidashiSetting(hukidashi_number);
                    break;

                case 1:

                    //ヒントだす
                    Debug.Log("ヒント");
                    IdleMotionHukidashiSetting(100);
                    break;               

                case 2:

                    //るんるんモーション                                      
                    IdleMotionHukidashiSetting(32);
                    break;                

                case 3:

                    if (GameMgr.Story_Mode != 0)
                    {
                        //朝～夜　時間に合わせたセリフモーション                  
                        IdleMotionHukidashiSetting(200);
                    }
                    else
                    {
                        //るんるんモーション                                      
                        IdleMotionHukidashiSetting(32);
                    }
                    
                    break;

                case 4:

                    //コスチュームセリフ
                    if (GameMgr.Costume_Num != 0)
                    {
                        //コスチュームモーション               
                        IdleMotionHukidashiSetting(300);
                    }
                    else
                    {
                        //Gokigenに合わせた固有のセリフ
                        Debug.Log("0 Gokigenに合わせた固有のセリフ");
                        IdleMotionHukidashiSetting(hukidashi_number);
                    }

                    break;

                case 5:

                    //左右にふりふり                   
                    IdleMotionHukidashiSetting(21);
                    break;               

                case 6:

                    //はらへり
                    if (GameMgr.Story_Mode == 0)
                    {
                        //おなかへった..                    
                        IdleMotionHukidashiSetting(23);
                    }
                    else
                    {
                        //満腹度30以下で発生
                        if(PlayerStatus.player_girl_manpuku <= 30)
                        {
                            //おなかへった..                    
                            IdleMotionHukidashiSetting(23);
                        }
                        else
                        {
                            Debug.Log("ヒント");
                            IdleMotionHukidashiSetting(100);
                        }
                    }
                    break;

                case 7:

                    //♪モーション                    
                    IdleMotionHukidashiSetting(35);
                    //FaceMotionPlay(1015);
                    break;

                case 8:

                    //ボウルをガシャガシャ                    
                    IdleMotionHukidashiSetting(34);
                    break;

                case 9:

                    //クッキーのつまみぐい                    
                    IdleMotionHukidashiSetting(33);
                    break;

                case 10:

                    //きらきらほわわ                    
                    IdleMotionHukidashiSetting(31);
                    break;

                case 11:

                    //吹き出しはなしで、モーションのみ                
                    random = Random.Range(0, 2); //

                    switch (random)
                    {
                        case 0:
                            Debug.Log("61 生地ガシャモーション");
                            FaceMotionPlay(1012); //生地をガシャガシャしはじめる
                            break;

                        case 1:
                            Debug.Log("2002  なでられたときのへにゃっとモーション");
                            FaceMotionPlay(2002); //
                            break;

                        default:
                            Debug.Log("61 生地ガシャモーション");
                            FaceMotionPlay(1012); //生地をガシャガシャしはじめる
                            break;
                    }
                    break;

                case 12:

                    IdleMotionHukidashiSetting(70); //雑談
                    break;

                case 13:

                    IdleMotionHukidashiSetting(75); //雑談
                    break;

                case 14:

                    IdleMotionHukidashiSetting(80); //雑談
                    break;

                case 15:

                    IdleMotionHukidashiSetting(85); //雑談
                    break;

                case 16:

                    IdleMotionHukidashiSetting(87); //雑談
                    break;

                default:

                    break;
            }
        }        
    }   

    void IdleMotionHukidashiSetting(int _motion_num)
    {
        if (hukidashiitem == null)
        {
            hukidasiInit(Default_hukidashi_hyoujitime);
        }

        _touchface_comment_lib.Clear();
        GameMgr.OsotoIkitaiFlag = false;

        switch (_motion_num)
        {
            case 0: //悲しみモーションのときのセリフ

                _touchface_comment_lib.Add("..まま。");
                _touchface_comment_lib.Add("ぐすん..。");
                _touchface_comment_lib.Add("..まま..。");
                _touchface_comment_lib.Add("..。にいちゃん。..。なんでもない。");

                break;

            case 1: //悲しみモーションのときのセリフ

                _touchface_comment_lib.Add("..まま。");
                _touchface_comment_lib.Add("ぐすん..。");
                _touchface_comment_lib.Add("..まま..。");
                _touchface_comment_lib.Add("..。にいちゃん。..。なんでもない。");

                break;

            case 2:

                _touchface_comment_lib.Add(".. ..。");
                _touchface_comment_lib.Add("..にいちゃんのおかし、食べたい。");
                _touchface_comment_lib.Add("にいちゃんのおかし作り、てつだう。");
                break;

            case 3:

                FaceMotionPlay(1014);
                _touchface_comment_lib.Add("..にいちゃん。おかし..。食べたい。");
                _touchface_comment_lib.Add("..おかし、食べたいなぁ。おにいちゃん..。");
                _touchface_comment_lib.Add("お腹へった..。");
                break;

            case 10:

                _touchface_comment_lib.Add("ちょっと元気。");
                _touchface_comment_lib.Add("うまうま・・。");

                break;

            case 20:

                if (!GameMgr.hikari_make_okashiFlag)
                {
                    FaceMotionPlay(1006);
                    _touchface_comment_lib.Add("ちょっと元気でてきた。");
                    _touchface_comment_lib.Add("えへへ♪　にいちゃん、お菓子また作って～♪");
                    _touchface_comment_lib.Add("うひひ。エメラルどんぐり、また集めなきゃ。");
                }
                else
                {
                    FaceMotionPlay(1022);
                    _touchface_comment_lib.Add("えへへ♪　にいちゃんのお菓子、作りちゅう～♪");
                    _touchface_comment_lib.Add("ぐるぐる.. 生地まぜ、たのし～♪");
                }

                break;

            case 21:

                if (GameMgr.BG_cullent_weather < 2)
                {
                    FaceMotionPlay(1024);
                    _touchface_comment_lib.Add("にいちゃ..。zZZ..。");
                }
                else if (GameMgr.BG_cullent_weather >= 2 && GameMgr.BG_cullent_weather < 6)
                {
                    //材料とりにいきたいモード。このときに外へいくと、ハートがあがる。
                    GameMgr.OsotoIkitaiFlag = true;

                    Debug.Log("2 左右にふりふり");
                    FaceMotionPlay(1005);
                    _touchface_comment_lib.Add("ねぇねぇにいちゃん。材料を採りにいこうよ～。");
                    _touchface_comment_lib.Add("どこかへ出かけたいなぁ～");
                }
                else if (GameMgr.BG_cullent_weather >= 6)
                {
                    FaceMotionPlay(1006);
                    _touchface_comment_lib.Add("ねぇねぇにいちゃん。明日も、お外へ材料とりにいこ～ね！");
                }
                break;

            case 22:

                _touchface_comment_lib.Add("うきうき！");
                _touchface_comment_lib.Add("いっぱい手伝うね！おにいちゃん。");
                break;

            case 23:

                Debug.Log("23 おなかへった");
                FaceMotionPlay(1014);
                _touchface_comment_lib.Add("お腹へった..。");
                break;

            case 30:

                if (!GameMgr.hikari_make_okashiFlag)
                {
                    FaceMotionPlay(1018); //こっちをむいて口パク
                    _touchface_comment_lib.Add("今日はあたたかいね～、にいちゃん！");
                    _touchface_comment_lib.Add("エメラルド色のどんぐり、欲しい？にいちゃん。");
                    _touchface_comment_lib.Add("にいちゃん。あのね.. 鳥さんがお庭にきてたから、パンあげたら食べたよ！");
                }
                else
                {
                    FaceMotionPlay(1022);
                    _touchface_comment_lib.Add("えへへ♪　にいちゃんのお菓子、作りちゅう～♪");
                    _touchface_comment_lib.Add("ぐるこん♪　ぐ～るこん♪");
                }

                break;

            case 31:

                Debug.Log("31 きらきらほわわ");
                FaceMotionPlay(1001);
                _touchface_comment_lib.Add("早く、にいちゃんのお菓子食べたいな～。");
                break;

            case 32: //るんるんモーション

                Debug.Log("32 るんるん");
                //材料とりにいきたいモード。このときに外へいくと、ハートがあがる。
                GameMgr.OsotoIkitaiFlag = true;

                FaceMotionPlay(1006);
                _touchface_comment_lib.Add("るんるん♪");
                _touchface_comment_lib.Add("エメラルどんぐり、拾いにいこうよ～。おにいちゃん。");
                break;

            case 33: //クッキーつまみぐい

                Debug.Log("33 つまみぐい");
                FaceMotionPlay(1009);
                _touchface_comment_lib.Add("こっそり.. 味見～♪");
                _touchface_comment_lib.Add("にいちゃんのおかし、おいしい。もぐもぐ..。");
                _touchface_comment_lib.Add("にいちゃんのおかし、もぐもぐ..。");
                break;

            case 34: //ボウルをガシャガシャ

                Debug.Log("34 ボウルをガシャガシャ");
                FaceMotionPlay(1013);
                _touchface_comment_lib.Add("にいちゃんのクッキー、おいしくなぁれ♪");
                break;

            case 35: //エモのみ　♪

                Debug.Log("35 ♪モーション");
                FaceMotionPlay(1015);
                _touchface_comment_lib.Add("♪");
                break;

            case 40:

                if (!GameMgr.hikari_make_okashiFlag)
                {
                    random = Random.Range(0, 2); //0~4

                    switch (random)
                    {
                        case 0:
                            FaceMotionPlay(2002);
                            _touchface_comment_lib.Add("にいちゃんのお菓子作り、てつだう～！！");
                            _touchface_comment_lib.Add("にいちゃんのおてて、あたたか～い！");
                            break;

                        case 1:

                            FaceMotionPlay(1005);
                            _touchface_comment_lib.Add("にいちゃん、コンテストは、もう余裕？");
                            break;
                    }
                }
                else
                {
                    random = Random.Range(0, 2);
                    switch (random)
                    {
                        case 0:

                            FaceMotionPlay(1022);
                            break;

                        case 1:

                            FaceMotionPlay(1025);
                            break;
                    }
                    _touchface_comment_lib.Add("えへへ♪　にいちゃんのお菓子、おいしくなぁれ～♪");
                    _touchface_comment_lib.Add("ぐるこん♪　ぐ～るこん♪");
                }
                break;

            case 50:


                FaceMotionPlay(1019);
                _touchface_comment_lib.Add("にいちゃん！大好き！！");

                break;

            case 60:

                if (!GameMgr.hikari_make_okashiFlag)
                {
                    random = Random.Range(0, 2);
                    switch (random)
                    {
                        case 0:

                            FaceMotionPlay(1019);
                            _touchface_comment_lib.Add("にいちゃん！大好き！！");
                            _touchface_comment_lib.Add("おにいちゃんのお菓子..、だいすき～！");
                            break;

                        case 1:

                            FaceMotionPlay(2002);
                            _touchface_comment_lib.Add("にいちゃんのお菓子、こころがぽかぽかするんじゃ～");
                            _touchface_comment_lib.Add("にいちゃんのおてて、あたたか～い！");
                            break;
                    }
                }
                else
                {
                    random = Random.Range(0, 2);
                    switch (random)
                    {
                        case 0:

                            FaceMotionPlay(1022);
                            break;

                        case 1:

                            FaceMotionPlay(1025);
                            break;
                    }

                    _touchface_comment_lib.Add("えへへ♪　にいちゃんのお菓子、作りちゅう～♪");
                    _touchface_comment_lib.Add("おいしくなれ～♪　おいしくなれ～♪");
                }
                break;

            case 70:

                random = Random.Range(0, 1); //0~4
                zatudan(random);               

                break;

            case 75:

                random = Random.Range(0, 2); //0~4
                zatudan(random);

                break;

            case 80:

                random = Random.Range(0, 3); //0~4
                zatudan(random);

                break;

            case 85:

                random = Random.Range(0, 4); //0~4
                zatudan(random);

                break;

            case 87:

                random = Random.Range(0, 5); //0~4
                zatudan(random);

                break;


            case 100:

                //レベルが低い時のヒント
                if (GirlGokigenStatus < 3) //LV 1~2
                {
                    FaceMotionPlay(1014);
                    _touchface_comment_lib.Add("まま・・。いつ帰ってくるのかなぁ？");
                    _touchface_comment_lib.Add("まま、会いたいな～。");
                    _touchface_comment_lib.Add("ぱぱに会いたいな～。");
                    _touchface_comment_lib.Add("にいちゃん。まま、いつ帰ってくるかな？");
                }
                if (GirlGokigenStatus >= 3 && GirlGokigenStatus < 4) //LV 1~2
                {
                    random = Random.Range(0, 2); //0~4

                    switch (random)
                    {
                        case 0:

                            FaceMotionPlay(1018); //こっちをむいて口パク
                            _touchface_comment_lib.Add("さくさく感の出し方は、ショップのおねえちゃんが知ってたかも？");
                            _touchface_comment_lib.Add("さわられると、ビックリしちゃうよ～・・。おにいちゃん。");
                            _touchface_comment_lib.Add("にいちゃん、フルーツは外でしか採れないよ～。");
                            break;

                        case 1:

                            FaceMotionPlay(1014);
                            _touchface_comment_lib.Add("にいちゃん、たいりょくが０になったら、材料集めはムリぃ～・・。");
                            _touchface_comment_lib.Add("にいちゃん。こまったときは、ショップのおねえちゃんにきこう。");
                            break;
                    }
                }
                //ごきげんに応じて、ヒントをだす。
                else if (GirlGokigenStatus >= 4) //
                {
                    random = Random.Range(0, 5); //0~4

                    switch (random)
                    {
                        case 0:

                            FaceMotionPlay(2000);
                            _touchface_comment_lib.Add("にいちゃん。今日のご飯は、ビールと枝豆の炊き込みご飯だよ♪");
                            break;

                        case 1:

                            FaceMotionPlay(2000);
                            _touchface_comment_lib.Add("にいちゃん。今日のお夕飯は、じゃがバターとシチューだよ～♪");
                            break;

                        case 2:

                            FaceMotionPlay(1017);
                            _touchface_comment_lib.Add("今までにたべたクッキーの枚数をおぼえてる？");
                            break;

                        default:

                            FaceMotionPlay(1018);
                            _touchface_comment_lib.Add("にいちゃん。伝説のお菓子のレシピが・・。どこかにあるらしいよ。");
                            _touchface_comment_lib.Add("にいちゃん。同じ素材でも上位素材があるよ。採取地で、ごくまれに採れるらしいよ！");
                            break;
                    }

                }
                break;

            case 200: //天気に関するモーション

                //もし、現在のBGMが特定のものが使われてる場合、それに関連したセリフも発生するかも。
                if(GameMgr.bgm_collection_list[GameMgr.userBGM_Num].titleName == "bgm18")
                {
                    random = Random.Range(0, 5); //0~4
                }
                else
                {
                    random = 0;
                }


                if (random < 3) //0~2のとき　60%の確率で、天気の話
                {

                    switch (GameMgr.BG_cullent_weather)
                    {

                        case 1:

                            //FaceMotionPlay(1014);
                            _touchface_comment_lib.Add("にいちゃん。ふわぁ～・・。もう寝ようよ～。");
                            break;

                        case 2:

                            FaceMotionPlay(1006); //るんるん
                            _touchface_comment_lib.Add("にいちゃん！　朝ごはんのぱん、焼けたよ～！");
                            _touchface_comment_lib.Add("じつにいい朝だ・・。にいちゃん、ぱん食べよ～♪");
                            _touchface_comment_lib.Add("にいちゃん！　とりさんが庭にきてるよ～！");
                            _touchface_comment_lib.Add("いい朝だねぇ～。にいちゃん～。");
                            _touchface_comment_lib.Add("にいちゃん～！太陽がまぶし～よ～！");
                            break;

                        case 3:

                            random = Random.Range(0, 2); //0~4

                            switch (random)
                            {
                                case 0:

                                    FaceMotionPlay(1018);
                                    _touchface_comment_lib.Add("あったかくなってきたね～♪　にいちゃん。");
                                    _touchface_comment_lib.Add("おひる、何食べる～？にいちゃん。");
                                    break;

                                case 1:

                                    //材料とりにいきたいモード。このときに外へいくと、ハートがあがる。
                                    GameMgr.OsotoIkitaiFlag = true;

                                    FaceMotionPlay(1006); //るんるん
                                    _touchface_comment_lib.Add("お日様がキラキラ..。");
                                    _touchface_comment_lib.Add("あったかおひる。お外いきたいな～。");
                                    break;
                            }

                            break;

                        case 4:

                            random = Random.Range(0, 2); //0~4

                            switch (random)
                            {
                                case 0:
                                    FaceMotionPlay(1024);
                                    _touchface_comment_lib.Add("ご飯たべた後はすぐねむくなっちゃう・・むにゃ。");
                                    _touchface_comment_lib.Add("..zZ。");
                                    break;

                                case 1:

                                    FaceMotionPlay(2001);
                                    _touchface_comment_lib.Add("午後のティーはおいしいよ。にいちゃん♪。");
                                    break;
                            }
                            break;

                        case 5:

                            random = Random.Range(0, 2); //0~4

                            switch (random)
                            {
                                case 0:
                                    FaceMotionPlay(1023);
                                    _touchface_comment_lib.Add("夕陽がしずんでくよ。にいちゃん。");
                                    _touchface_comment_lib.Add("オレンジの夕陽.. きれい～。にいちゃん。");
                                    _touchface_comment_lib.Add("にいちゃん～。もう夕方だね～。");
                                    break;

                                case 1:

                                    _touchface_comment_lib.Add("ばんごはん、何する～？にいちゃん。");
                                    break;
                            }
                            break;

                        case 6:

                            FaceMotionPlay(1024);
                            _touchface_comment_lib.Add("にいちゃん。しごと終わりのミルクは、最高だ！　ごくごく・・♪");
                            _touchface_comment_lib.Add("もう遅い時間～。ふわぁ～・・。");
                            _touchface_comment_lib.Add("にいちゃん。そろそろ寝ようよ～。");
                            _touchface_comment_lib.Add("今日はよく頑張ったね！にいちゃん～ .. ..");
                            break;

                        default:

                            FaceMotionPlay(1024);
                            _touchface_comment_lib.Add("..zZZZ。");
                            break;
                    }
                }
                else //特定の音楽のときにでるセリフ
                {

                    switch (GameMgr.bgm_collection_list[GameMgr.userBGM_Num].titleName)
                    {
                        case "bgm9":

                            FaceMotionPlay(1024);
                            _touchface_comment_lib.Add("にいちゃん。いまかかってる音楽、ここちいいねぇ～..。ぽかぽか。");
                            break;

                        case "bgm18":

                            FaceMotionPlay(1024);
                            _touchface_comment_lib.Add("..zZZZ。この音楽、ねむくなってきた..。");
                            break;

                        default:

                            FaceMotionPlay(1018);
                            sc.PlaySe(40);
                            _touchface_comment_lib.Add("にいちゃん。鳥さんが、ないてるよ～♪");
                            break;
                    }

                }             

                break;

            case 300: //コスチュームセリフ

                switch (GameMgr.Costume_Num)
                {
                    case 0: //デフォルト　なし

                        break;

                    case 1: //黒エプロン

                        FaceMotionPlay(1006); //るんるんモーション
                        _touchface_comment_lib.Add("へへ♪　黒エプロンもかわいい～♪");
                        _touchface_comment_lib.Add("このかっこうでお外でてみたいな～♪");
                        _touchface_comment_lib.Add("メイドさんのかっこう。おそうじがんばる～！");
                        break;

                    case 2: //スク水

                        FaceMotionPlay(1006); //るんるんモーション
                        _touchface_comment_lib.Add("にいちゃん～。スク水♪　スク水♪");
                        _touchface_comment_lib.Add("えへへ♪　このかっこう、にあう～？");
                        _touchface_comment_lib.Add("にいちゃん。25メートル泳げそう。");

                        break;

                    case 3: //白い服

                        FaceMotionPlay(1006); //るんるんモーション
                        _touchface_comment_lib.Add("にいちゃん～。真っ白服どう～？　似合う～？　いえ～ぃ♪");
                        _touchface_comment_lib.Add("スカートひらひら。かわいい～♪");
                        _touchface_comment_lib.Add("へへ♪　にいちゃん。この服、にあう～？");
                        break;

                    case 4: //赤い服

                        FaceMotionPlay(1006); //るんるんモーション
                        _touchface_comment_lib.Add("うわぁ～☆　ハートリボンお気に入り♪");
                        _touchface_comment_lib.Add("赤くてオシャレな服～♪　お外でかけたいな～♪");
                        _touchface_comment_lib.Add("にいちゃん！　ちょっと大人っぽい服だよ！　似合う～？");
                        break;
                }

                break;

            case 400:

                FaceMotionPlay(1007);
                _touchface_comment_lib.Add("..まずは左の「おかしパネル」から、お菓子を作ろうね。おにいちゃん。");
                break;

            case 410: //まずいものを食べて機嫌が悪い時のセリフ

                _touchface_comment_lib.Add("..。");
                break;

            case 420:

                _touchface_comment_lib.Add("にいちゃん。ピクニック楽しかった～♪");
                break;

            case 430:

                FaceMotionPlay(1006);
                _touchface_comment_lib.Add("兄ちゃん！お菓子おいしかった！ありがと～♪");
                break;

            case 440: //タイトル画面　おにいちゃんおかえりなさいなど

                FaceMotionPlay(1006);
                _touchface_comment_lib.Add("..おにいちゃん！　おかえりなさい～☆");
                break;

            case 1000: //コンテスト中

                _touchface_comment_lib.Add("..いっぱい作る～！！");
                _touchface_comment_lib.Add("ぐ～るぐ～る☆");
                _touchface_comment_lib.Add("..あわわ。粉入れすぎちゃった..。");
                break;

            default:

                _touchface_comment_lib.Add("..にいちゃん。");
                break;
        }

        random = Random.Range(0, _touchface_comment_lib.Count);
        _hintrandom = _touchface_comment_lib[random];
        hukidashiitem.GetComponent<TextController>().SetText(_hintrandom);
    }

    void zatudan(int _random)
    {
        switch (_random)
        {
            //50~
            case 0: //雑談をする

                FaceMotionPlay(1018); //こっちをむいて口パク
                _touchface_comment_lib.Add("インドは、カレーの本場なんだよ。ちょっと甘くておいしいらしいよ..！");
                _touchface_comment_lib.Add("にいちゃん。おしごとってなぁに？　お金をかせぐこと？");
                _touchface_comment_lib.Add("にいちゃん。てんごくって、どこにあるの～？　おそらの上？");
                _touchface_comment_lib.Add("にいちゃん。なんで、ゆめって、楽しいのに、消えちゃうのかなぁ？");
                _touchface_comment_lib.Add("にいちゃん。この間、ゆめの中で、じゃがバターいっぱい食べた♪");
                _touchface_comment_lib.Add("にいちゃん。お空のくも、ふわふわしてておいしそう..。");
                break;

            //60~ （case 0 も含む）
            case 1: //雑談をする2 癒し

                FaceMotionPlay(2001); //はなうた
                _touchface_comment_lib.Add("しっぱいは、せいこうのはは、なんだよ～！にいちゃん！");
                _touchface_comment_lib.Add("にいちゃんといつまでも一緒♪　でも、キラキラ宝石は欲しい..。にいちゃん！");
                _touchface_comment_lib.Add("にいちゃん。つかれたら、ヒカリが膝枕でよしよししてあげるね。よしよし..。");                
                _touchface_comment_lib.Add("おいもとバターって、相性ぴったり♪　まるで恋人みたいだね。");
                _touchface_comment_lib.Add("プリンのおねえちゃん。なにしてるかなぁ～？");
                _touchface_comment_lib.Add("このあいだ、はっぱがキラキラしててきれいだったよ～。にいちゃん！");
                _touchface_comment_lib.Add("にいちゃん！　このまえ、おさかな焦がしちゃった～..。");

                _touchface_comment_lib.Add("にいちゃん！　だいすき～♪");
                _touchface_comment_lib.Add("にいちゃんのために、お菓子作る～♪");
                break;

            //70~から
            case 2:

                FaceMotionPlay(2001); //はなうた
                _touchface_comment_lib.Add("にいちゃん！今日はのんびり日和～♪　ごろごろ..。");
                _touchface_comment_lib.Add("あのね！　このあいだ、いしの隙間でやもりさん見つけたよ～！");
                _touchface_comment_lib.Add("うしさんのおちち。モ～モ～♪");
                _touchface_comment_lib.Add("おうごんおじゃが..　いっぱい食べたいな～♪");

                _touchface_comment_lib.Add("にいちゃんのために、ハートクッキー作ろっかな！");
                _touchface_comment_lib.Add("焼きたてフィナンシェ.. サックサクでおいしいんだよ～♪");
                break;

            //80~から
            case 3:

                FaceMotionPlay(2001); //はなうた
                _touchface_comment_lib.Add("とれとれ、とれたてか～にの身を～、ぱんにつ～めて～♪");
                _touchface_comment_lib.Add("にいちゃん～！かにぱんまんのご本、いっしょに見よ～よ♪");
                _touchface_comment_lib.Add("きらきらぽんぽ～♪　きょうもいっぱい！にいちゃん！");
                _touchface_comment_lib.Add("にいちゃん。フォカッチャ大使は、じつはいい人なんだよ..！");

                _touchface_comment_lib.Add("にいちゃん、ずっとヒカリのそばにいてね♪");
                _touchface_comment_lib.Add("にいちゃん。ままとぱぱと一緒に、ピクニックいきたいなぁ～♪");
                break;

            //90~から
            case 4:

                FaceMotionPlay(2001); //はなうた
                _touchface_comment_lib.Add("ほぐほぐ、ほぐしたか～にの身を～、ぱんにつ～めて～♪");

                _touchface_comment_lib.Add("にいちゃんと一緒に、ずっとお菓子作ってたいなぁ～♪");
                break;

            default:

                FaceMotionPlay(1019);
                _touchface_comment_lib.Add("にいちゃん！大好き！！");
                _touchface_comment_lib.Add("おにいちゃんのお菓子..、だいすき～！");
                break;
        }
    }

    //compound_Mainなどから読む 即座にそのモーションと吹き出しを再生
    public void MotionChange(int _motion_num)
    {                           
        IdleMotionHukidashiSetting(_motion_num);
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
                _touchhead_comment_lib.Add("にいちゃん、おててに粉ついてるよ～..。");
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

                _touchhead_comment_lib.Add("ん、どうしたの？あに。");
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

            default:

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

    void Init_touchTwintailComment()
    {
        //touchTwintailは、上から順番に表示される。
        _touchtwintail_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0:

                _touchtwintail_comment_lib.Add("うわっ");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                break;

            case 1:

                _touchtwintail_comment_lib.Add("..!");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                break;

            case 2:

                _touchtwintail_comment_lib.Add("わっ");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 3:

                _touchtwintail_comment_lib.Add("いたっ");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 4:

                _touchtwintail_comment_lib.Add("わっ");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            case 5:

                _touchtwintail_comment_lib.Add("うわ♪");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（ぱぱのにおい..。）");
                break;

            case 6:

                _touchtwintail_comment_lib.Add("うわ♪");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（ままのクッキーのにおい..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;

            default:

                _touchtwintail_comment_lib.Add("うわ♪");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..♪");
                _touchtwintail_comment_lib.Add("（ままのクッキーのにおい..。）");
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
                _touchchest_comment_lib.Add("..なんでそんなとこさわるの？。");

                break;

            case 1: //不快な感じ

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("..。");
                _touchchest_comment_lib.Add("..なんでそんなとこさわるの？。");

                break;

            case 2: //不快だけど、そこまで嫌がらない。

                _touchchest_comment_lib.Add("..？");
                _touchchest_comment_lib.Add("..。");
                _touchchest_comment_lib.Add("へんなとこ、さわっちゃだめ！");

                break;

            case 3:

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("..は、はずかしいよ～..。");
                _touchchest_comment_lib.Add("なんでそんなとこさわるの..？　おにいちゃん。");
                _touchchest_comment_lib.Add("えっち！にいちゃんのばか！");

                break;

            case 4:

                _touchchest_comment_lib.Add("..！");
                _touchchest_comment_lib.Add("..は、はずかしいよ～..。");
                _touchchest_comment_lib.Add("なんでそんなとこさわるの..？　おにいちゃん。");
                _touchchest_comment_lib.Add("..わ！！");

                break;

            case 5:

                _touchchest_comment_lib.Add("！！");
                _touchchest_comment_lib.Add("..は、はずかしい～..。");
                _touchchest_comment_lib.Add("..ひ、ひどいよ～。にいちゃんのばか！！");
                _touchchest_comment_lib.Add("くすぐったいよ～..。");
                _touchchest_comment_lib.Add("..はひー！！");
                _touchchest_comment_lib.Add("..そこは触っちゃだめ！！にいちゃん！！");

                break;

            case 6:

                _touchchest_comment_lib.Add("！！");
                _touchchest_comment_lib.Add("ばか..！！");
                _touchchest_comment_lib.Add("..ひ、ひどいよ～。にいちゃんのばか！！");
                _touchchest_comment_lib.Add("..は、はずかしい～..。");
                _touchchest_comment_lib.Add("ぐひぃ～・・。");
                _touchchest_comment_lib.Add("..あにぃ～。");
                _touchchest_comment_lib.Add("..はひー！！");
                _touchchest_comment_lib.Add("くすぐったいよ～..。");
                _touchchest_comment_lib.Add("ぎぃや～～。（くすぐったいよ..。にいちゃん～。）");

                break;

            default:

                _touchchest_comment_lib.Add("！！");
                _touchchest_comment_lib.Add("ばか..！！");
                _touchchest_comment_lib.Add("..ひ、ひどいよ～。にいちゃんのばか！！");
                _touchchest_comment_lib.Add("..は、はずかしい～..。");
                _touchchest_comment_lib.Add("ぐひぃ～・・。");
                _touchchest_comment_lib.Add("..やめなさい！あにぃ～。");
                _touchchest_comment_lib.Add("..はひー！！");
                _touchchest_comment_lib.Add("くすぐったいよ～..。");
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

    public void face_girl_Joukigen2()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 32; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Metoji()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 35; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Tereru4()
    {
        face_girl_Reset();

        //intパラメーターの値を設定する.  
        trans_expression = 36; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    //表情パラメータを一旦リセット。だが、Live2D_animatorの書き方的には、多分機能してない。
    public void face_girl_Reset()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 999; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }


    //機嫌状態の処理
    public void GirlExpressionKoushin(int _param)
    {
        if (_param >= 0)
        {
            PlayerStatus.player_girl_express_param += _param;
        }
        else
        {
            PlayerStatus.player_girl_express_param += _param;
        }

        if (PlayerStatus.player_girl_express_param <= 0)
        {
            PlayerStatus.player_girl_express_param = 0;
        }
        else if (PlayerStatus.player_girl_express_param >= 100)
        {
            PlayerStatus.player_girl_express_param = 100;
        }

        //機嫌決定
        if (PlayerStatus.player_girl_express_param < 20)
        {
            PlayerStatus.player_girl_expression = 1;
        }
        else if (PlayerStatus.player_girl_express_param >= 20 && PlayerStatus.player_girl_express_param < 40)
        {
            PlayerStatus.player_girl_expression = 2;
        }
        else if (PlayerStatus.player_girl_express_param >= 40 && PlayerStatus.player_girl_express_param < 60)
        {
            PlayerStatus.player_girl_expression = 3;
        }
        else if (PlayerStatus.player_girl_express_param >= 60 && PlayerStatus.player_girl_express_param < 80)
        {
            PlayerStatus.player_girl_expression = 4;
        }
        else if (PlayerStatus.player_girl_express_param >= 80)
        {
            PlayerStatus.player_girl_expression = 5;
        }

    }

    //満腹ゲージの処理
    public void ManpukuBarKoushin(int _param)
    {
        if (GameMgr.System_Manpuku_ON)
        {
            if (_param >= 0)
            {
                PlayerStatus.player_girl_manpuku += _param;
                if (PlayerStatus.player_girl_manpuku > 0)
                {
                    GameMgr.Haraheri_Msg = false;
                }
            }
            else
            {
                PlayerStatus.player_girl_manpuku += _param;
            }

            if (PlayerStatus.player_girl_manpuku <= 0)
            {
                PlayerStatus.player_girl_manpuku = 0;

                if (!GameMgr.Haraheri_Msg)
                {
                    GameMgr.Haraheri_Msg = true;
                    //音鳴らす
                    sc.PlaySe(45);

                    MotionChange(23);
                }
            }
            if (PlayerStatus.player_girl_manpuku >= 100)
            {
                PlayerStatus.player_girl_manpuku = 100;
            }
        }
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}

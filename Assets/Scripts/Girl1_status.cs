using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

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

    private Touch_Controller touch_controller;

    private SpriteRenderer s;

    private SoundController sc;

    private Text questname;
    private GameObject questtitle_panel;
    private Text questpanel_text;

    public float timeOut;
    public float timeOut2;
    public float timeOut3;
    private float Default_hungry_cooltime;
    public int timeGirl_hungry_status; //今、お腹が空いているか、空いてないかの状態

    public bool GirlEat_Judge_on;
    public int GirlGokigenStatus; //女の子の現在のご機嫌の状態。6段階ほどあり、好感度が上がるにつれて、だんだん見た目が元気になっていく。

    private GameObject text_area;

    private GameObject hukidashiPrefab;
    private GameObject canvas;

    public GameObject hukidashiitem;
    private Text _text;

    private GameObject MoneyStatus_Panel_obj;

    private GameObject Extremepanel_obj;

    public int touch_status; //今どこを触っているかの状態。TimeOutが入り組んで、ぐちゃぐちゃにならないように分ける。

    private List<string> _touchhead_comment_lib = new List<string>();
    private string _touchhead_comment;
    private List<string> _touchface_comment_lib = new List<string>();
    private string _touchface_comment;

    private string MazuiHintComment;
    private int MazuiStatus;

    private List<string> _touchtwintail_comment_lib = new List<string>();
    private string _touchtwintail_comment;

    public bool WaitHint_on;
    public float timeOutHint;
    private string _hint1;
    private string _hintrandom;
    private List<string> _hintrandomDict = new List<string>();

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    private BGM sceneBGM;

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

    public int[] girl1_like_set_score;
    public int[] girl1_NonToppingScoreSet;

    public int youso_count; //GirlEat_judgeでも、パラメータ初期化の際使う。
    public int Set_Count;

    public int girl1_Love_exp; //女の子の好感度値のこと。ゲーム中に、お菓子をあげることで変動する。
    public int girl1_Love_lv; //好感度のレベル。100ごとに１上がる。

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


    //女の子の好み組み合わせセットのデータ
    public int Set_compID;
    private int glike_compID;
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

    private int Girl1_touchtwintail_count;
    private bool Girl1_touchtwintail_flag; //全ての会話を表示したら、しばらく触れなくなる

    //特定のお菓子か、ランダムから選ぶかのフラグ
    public int OkashiNew_Status;
    public int OkashiQuest_ID; //特定のお菓子、のお菓子セットのID
    public string OkashiQuest_Name; //そのときのお菓子のクエストネーム
    public int Special_ignore_count; //スペシャルを無視した場合、カウント。3回あたりをこえると、スペシャルは無視される。

    //エフェクト関係
    private GameObject Emo_effect_Prefab1;
    private GameObject Emo_effect_Prefab2;
    private GameObject Emo_effect_Prefab3;
    private List<GameObject> _listEffect = new List<GameObject>();
    private GameObject character;

    //Live2Dモデルの取得
    private CubismModel _model;
    private Animator live2d_animator;
    private int trans_expression;

    //ハートレベルのテーブル
    public List<int> stage1_lvTable = new List<int>();

    private int _sum;
    private int _temp_lvTablecount;

    // Use this for initialization
    void Start()
    {

        DontDestroyOnLoad(this);

        
        girl_comment_flag = false;
        girl_comment_endflag = false;
        _desc = "";

        audioSource = GetComponent<AudioSource>();

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");
        character = GameObject.FindWithTag("Character");

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

        //この時間ごとに、女の子は、お菓子を欲しがり始める。
        Default_hungry_cooltime = 0.5f;
        timeOut = Default_hungry_cooltime;
        timeOut2 = 10.0f;
        timeGirl_hungry_status = 1;

        GirlGokigenStatus = 0;
        girl1_Love_exp = 0;
        girl1_Love_lv = 1;
        OkashiNew_Status = 1;
        Special_ignore_count = 0;

        GirlEat_Judge_on = true;
        WaitHint_on = false;
        timeOutHint = 5.0f;

        special_animstart_flag = false;
        special_animstart_endflag = false;
        special_animstart_status = 0;
        special_timeOut = 3.0f;
        special_animatFirst = false;

        MazuiStatus = 0;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //カメラの取得
                main_cam = Camera.main;
                maincam_animator = main_cam.GetComponent<Animator>();
                trans = maincam_animator.GetInteger("trans");

                //Live2Dモデルの取得
                _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                live2d_animator = _model.GetComponent<Animator>();
                trans_expression = live2d_animator.GetInteger("trans_expression");

                //初期表情の設定
                CheckGokigen();
                DefaultFace();

                //エクストリームパネルの取得
                Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");

                //お金の増減用パネルの取得
                MoneyStatus_Panel_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;

                //タッチ判定オブジェクトの取得
                touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();

                //メイン画面に表示する、現在のクエスト
                questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();
                questtitle_panel = canvas.transform.Find("QuestTitlePanel").gameObject;
                questpanel_text = questtitle_panel.transform.Find("QuestPanel/QuestName").GetComponent<Text>();
                questtitle_panel.SetActive(false);

                break;
        }

        //女の子のイラストデータ
        /*Girl1_img_normal = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_normal");
        Girl1_img_gokigen = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_gokigen");
        Girl1_img_smile = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_yorokobi");
        Girl1_img_eat_start = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_eat_start");
        Girl1_img_verysad = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_verysad");
        Girl1_img_verysad_close = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_verysad_close");
        Girl1_img_hirameki = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_hirameki");
        Girl1_img_tereru = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_tereru");
        Girl1_img_angry = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_angry");
        Girl1_img_iya = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_iya");*/

        Girl1_touchhair_start = false;
        Girl1_touchhair_count = 0;
        Girl1_touchhair_status = 0;

        Girl1_touchtwintail_count = 0;
        Girl1_touchtwintail_flag = false;

        girl_Mazui_flag = false;

        touch_status = 0;

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

        girl1_like_set_score = new int[youso_count];
        girl1_NonToppingScoreSet = new int[youso_count];

        girl1_likeSubtype = new string[youso_count];
        girl1_likeOkashi = new string[youso_count];
        girllike_desc = new string[youso_count];
        girllike_comment_flag = new int[youso_count];

        //ステージごとに、女の子が食べたいお菓子のセットを初期化
        InitializeStageGirlHungrySet(0, 0); //とりあえず0で初期化

        // *** ここまで *** 

        //エフェクトプレファブの取得
        Emo_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Emo_Hirameki_Anim");
        Emo_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Emo_Kirari_Anim");
        Emo_effect_Prefab3 = (GameObject)Resources.Load("Prefabs/Emo_Angry_Anim");

        //好感度ステータスで変わる吹き出しテキストをセッティング        
        RandomGenkiInit();
    }

    // Update is called once per frame
    void Update () {

        //シーン移動の際、破壊されてしまうオブジェクトは、毎回初期化
        if( canvas == null )
        {
            canvas = GameObject.FindWithTag("Canvas");
           
            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    //カメラの取得
                    main_cam = Camera.main;
                    maincam_animator = main_cam.GetComponent<Animator>();
                    trans = maincam_animator.GetInteger("trans");

                    //Live2Dモデルの取得
                    _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                    live2d_animator = _model.GetComponent<Animator>();
                    trans_expression = live2d_animator.GetInteger("trans_expression");

                    compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                    compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                    character = GameObject.FindWithTag("Character");

                    //テキストエリアの取得
                    text_area = canvas.transform.Find("MessageWindow").gameObject;

                    //エクストリームパネルの取得
                    Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");

                    //お金の増減用パネルの取得
                    MoneyStatus_Panel_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;

                    //s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();

                    //タッチ判定オブジェクトの取得
                    touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

                    //BGMの取得
                    sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

                    //Live2Dモデルの取得
                    _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();

                    //メイン画面に表示する、現在のクエスト
                    questname = canvas.transform.Find("MessageWindowMain/SpQuestNamePanel/QuestNameText").GetComponent<Text>();
                    questtitle_panel = canvas.transform.Find("QuestTitlePanel").gameObject;
                    questpanel_text = questtitle_panel.transform.Find("QuestPanel/QuestName").GetComponent<Text>();
                    questtitle_panel.SetActive(false);

                    //初期表情の設定
                    CheckGokigen();
                    DefaultFace();

                    //タイマーをリセット
                    timeOut = Default_hungry_cooltime;
                    timeOut2 = 10.0f;
                    GirlEat_Judge_on = true;

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
                break;
        }

        if (hukidashiPrefab == null)
        {
            //Prefab内の、コンテンツ要素を取得       
            hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");
        }

        //trueだと腹減りカウントが進む。
        if (GirlEat_Judge_on == true)
        {
            timeOut -= Time.deltaTime;
            timeOut2 -= Time.deltaTime;
        }

        if(WaitHint_on) //吹き出しヒントを表示中
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
            }
        }

        switch (touch_status)
        {

            case 0: //何も触っていない。

                break;

            case 1: //髪の毛

                //髪の毛触り始めたらカウントスタート
                if (Girl1_touchhair_start)
                {
                    WaitHint_on = false;
                    timeOut3 -= Time.deltaTime;

                    if (timeOut3 <= 0.0f)
                    {
                        Girl1_touchhair_status = 0;
                        Girl1_touchhair_count = 0;
                        Girl1_touchhair_start = false;
                        GirlEat_Judge_on = true;

                        //吹き出し・ハングリーステータスをリセット
                        ResetHukidashi();
                    }
                }
                break;

            case 2: //口を触る。

                Girl1_touchhair_start = false;
                break;

            case 3: //リボンを触る。

                Girl1_touchhair_start = false;
                break;

            case 4: //ツインテールを触る。

                Girl1_touchhair_start = false;
                break;

            case 5: //胸を触る。

                Girl1_touchhair_start = false;
                break;

            case 6: //お花を触る。

                Girl1_touchhair_start = false;
                break;

            default:
                break;
                
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
                                case 0:

                                    timeGirl_hungry_status = 1; //お腹が空いた状態に切り替え。吹き出しがでる。

                                    rnd = Random.Range(30.0f, 60.0f);
                                    timeOut = Default_hungry_cooltime + rnd;
                                    Girl_Hungry();

                                    //キャラクタ表情変更
                                    DefaultFace();
                                    
                                    break;

                                case 1:

                                    timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                    rnd = Random.Range(1.0f, 5.0f);
                                    timeOut = Default_hungry_cooltime + rnd;
                                    Girl_Full();

                                    //キャラクタ表情変更
                                    DefaultFace();
                                    break;

                                case 2:

                                    //お菓子をあげたあとの状態。

                                    timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                    timeOut = Default_hungry_cooltime;
                                    Girl_Full();

                                    //キャラクタ表情変更
                                    DefaultFace();
                                    break;

                                default:

                                    timeOut = Default_hungry_cooltime;
                                    break;
                            }

                        }

                        //一定時間たつとヒントを出す。
                        if (timeOut2 <= 0.0)
                        {
                            timeOut2 = 5.0f;
                            timeGirl_hungry_status = 1; //お腹が空いた状態に切り替え。吹き出しがでる。

                            Girl_Hungry();
                            Girl1_Hint();
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

                    //キャラクタ表情変更
                    DefaultFace();                    

                    special_animstart_flag = false;
                    special_animstart_endflag = true;
                    break;
            }

            special_timeOut -= Time.deltaTime;
        }
    }

    public void CheckGokigen()
    {
        //女の子の今のご機嫌
        if (girl1_Love_exp >= 0 && girl1_Love_exp < SumLvTable(1))
        {
            GirlGokigenStatus = 0;
           
        }
        else if (girl1_Love_exp >= SumLvTable(1) && girl1_Love_exp < SumLvTable(2))
        {
            GirlGokigenStatus = 1;
           
        }
        else if (girl1_Love_exp >= SumLvTable(2) && girl1_Love_exp < SumLvTable(3))
        {
            GirlGokigenStatus = 2;
            
        }
        else if (girl1_Love_exp >= SumLvTable(3) && girl1_Love_exp < SumLvTable(4))
        {
            GirlGokigenStatus = 3;
            
        }
        else if (girl1_Love_exp >= SumLvTable(4) && girl1_Love_exp < SumLvTable(5))
        {
            GirlGokigenStatus = 4;
            
        }
        else if (girl1_Love_exp >= SumLvTable(5))
        {
            GirlGokigenStatus = 5;
            
        }
    }

    public void DefaultFace()
    {
        switch (GirlGokigenStatus)
        {
            case 0:
                face_girl_Bad();
                break;

            case 1:
                face_girl_Little_Fine();
                break;

            case 2:
                face_girl_Fine();
                break;

            case 3:
                face_girl_Normal();
                break;

            case 4:
                face_girl_Joukigen();
                break;

            case 5:
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

                    //番号を入れると、女の子の好みデータベースから、値を取得し、セット。OkashiQuest_IDは、外部から指定。
                    glike_compID = OkashiQuest_ID;

                    //今選んだやつの、girllikeComposetのIDも保存しておく。（こっちは直接選んでいる。）
                    Set_compID = glike_compID;

                    //OkashiQuest_ID = compIDを指定すると、女の子が食べたいお菓子＜組み合わせ＞がセットされる。
                    SetQuestRandomSet(glike_compID, false);

                    if (special_animatFirst != true) //最初の一回だけ、吹き出しアニメスタート
                    {
                        //一度、一度ドアップになり、電球がキラン！　→　そのあと、クエストの吹き出し。最初の一回だけ。
                        StartCoroutine("Special_StartAnim");
                    }                   

                    break;

                case 1:

                    //前の残りの吹き出しアイテムを削除。
                    if (hukidashiitem != null)
                    {
                        Destroy(hukidashiitem);
                    }

                    //
                    //②通常ステージ、ランダムセット。
                    //

                    //その他、通常のステージ攻略時は、セット組み合わせからランダムに選ぶ。
                    //例えば、セット1・4の組み合わせだと、1でも4でもどっちでも正解。カリっとしたお菓子を食べたい～、のような感じ。    

                    
                    //まず、表示フラグが1のもののみのセットを作る。
                    girlLikeCompo_database.StageSet();

                    //そこからランダムで選択。compIDを指定しているわけではないので、注意！
                    random = Random.Range(0, girlLikeCompo_database.girllike_compoRandomset.Count);

                    glike_compID = random;

                    //今選んだやつの、randomsetのIDも保存しておく。
                    Set_compID = glike_compID;

                    //ランダムセットから、女の子が食べたいお菓子＜組み合わせ＞がセットされる。
                    SetQuestRandomSet(glike_compID, true);                   


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

            //最初にお菓子にまつわるヒントやお話。宴へとぶ
            GameMgr.scenario_ON = true;
            GameMgr.sp_okashi_ID = Set_compID; //GirlLikeCompoSetの_set_compIDが入っている。
            GameMgr.sp_okashi_hintflag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                               //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);
            while (!GameMgr.recipi_read_endflag)
            {
                yield return null;
            }

            //シナリオも進む。
            switch(Set_compID)
            {
                case 1010: //ラスク

                    GameMgr.scenario_flag = 150;
                    break;

                default:

                    break;
            }
           
            GameMgr.scenario_ON = false;
            GameMgr.recipi_read_endflag = false;
            touch_controller.Touch_OnAllON();
            canvas.SetActive(true);
            sceneBGM.MuteOFFBGM();
        }       

        //表示用吹き出しを生成                   
        hukidasiInit();

        //吹き出しのテキスト決定
        _text = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
        _text.text = _desc;

        //現在のクエストネーム更新
        questname.text = OkashiQuest_Name;

        //クエストタイトルパネルを表示
        questpanel_text.text = OkashiQuest_Name;
        questtitle_panel.SetActive(true);

        special_animatFirst = true;
        GirlEat_Judge_on = true;
    }

    //チュートリアルで使用
    public void SetOneQuest(int _ID)
    {
        InitializeStageGirlHungrySet(_ID, 0);

        Set_Count = 1;
        OkashiNew_Status = 1; //回避用
        Set_compID = _ID;

        //テキストの設定。直接しているか、セット組み合わせエクセルにかかれたキャプションのどちらかが入る。
        _desc = girllike_desc[0];
    }

    
    void SetQuestRandomSet(int _ID, bool _rndset)
    {
        if (_rndset == true)
        {
            //ランダムセットから一つを選ぶ。
            set1_ID = girlLikeCompo_database.girllike_compoRandomset[_ID].set1;
            set2_ID = girlLikeCompo_database.girllike_compoRandomset[_ID].set2;
            set3_ID = girlLikeCompo_database.girllike_compoRandomset[_ID].set3;

            //テキストの設定。セット組み合わせのときは、セット組み合わせ用のメッセージになる。
            _desc = girlLikeCompo_database.girllike_compoRandomset[_ID].desc;
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
            _desc = girlLikeCompo_database.girllike_composet[_compID].desc;
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

    public void Girl_Full()
    {
        if (hukidashiitem != null)
        {
            DeleteHukidashi();
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

    //デフォルト・共通の腹減り初期化設定
    public void Girl1_Status_Init()
    {
        timeOut = 5.0f;

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
    public void Girl1_Hint()
    {
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }

        //まだ一度も調合していない
        if (PlayerStatus.First_recipi_on != true)
        {
            _hint1 = "左のパネルを押して、お菓子を作ろうね。お兄ちゃん。";            
            hukidashiitem.GetComponent<TextController>().SetText(_hint1);
        }
        else
        {
            //ランダムで、吹き出しの内容を決定
            RandomGenkiInit();
            random = Random.Range(0, _hintrandomDict.Count);
            _hintrandom = _hintrandomDict[random];

            //ヒントをだすか、今食べたいもののどちらかを表示する。
            random = Random.Range(0, 100);
            if(random < 50)
            {
                hukidashiitem.GetComponent<TextController>().SetText(_hintrandom);
            }
            else
            {
                hukidashiitem.GetComponent<TextController>().SetText(_desc);
            }
            
        }

        //15秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 15.0f;
        GirlEat_Judge_on = false;
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


    public void hukidasiInit()
    {
        hukidashiitem = Instantiate(hukidashiPrefab);

        if (OkashiNew_Status == 0)
        {
            hukidashiitem.transform.Find("hukidashi_Image_special").gameObject.SetActive(true);
            hukidashiitem.transform.Find("hukidashi_Image").gameObject.SetActive(false);
        }
        else
        {
            hukidashiitem.transform.Find("hukidashi_Image_special").gameObject.SetActive(false);
            hukidashiitem.transform.Find("hukidashi_Image").gameObject.SetActive(true);
        }


        //音を鳴らす
        audioSource.PlayOneShot(sound1);

        _text = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
    }

    //吹き出しを一時オフ
    public void hukidasiOff()
    {
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(false);
        }
    }

    //吹き出しを一時オン
    public void hukidasiOn()
    {
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(true);
        }
    }

    void DeleteHukidashi()
    {
        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);

            //音を鳴らす
            audioSource.PlayOneShot(sound2);
        }

        //まず全ての値を0に初期化
        for (i = 0; i < girl1_hungryScoreSet1.Count; i++)
        {
            girl1_hungryScoreSet1[i] = 0;
            girl1_hungryScoreSet2[i] = 0;
            girl1_hungryScoreSet3[i] = 0;
            girl1_hungryToppingScoreSet1[i] = 0;
            girl1_hungryToppingScoreSet2[i] = 0;
            girl1_hungryToppingScoreSet3[i] = 0;
            girl1_hungryToppingNumberSet1[i] = 0;
            girl1_hungryToppingNumberSet2[i] = 0;
            girl1_hungryToppingNumberSet3[i] = 0;
        }        

    }

    public void DeleteHukidashiOnly() //こっちは消すだけ
    {
        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);
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

        //②味のパラメータ。現状は未実装。これに足りてないと、「甘さが足りない」といったコメントをもらえる。
        girl1_Rich[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_rich;
        girl1_Sweat[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sweat;
        girl1_Sour[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sour;
        girl1_Bitter[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_bitter;

        //③お菓子の種類：　空＝お菓子はなんでもよい　か　クッキー
        girl1_likeSubtype[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_itemSubtype;

        //④特定のお菓子が食べたいかを決定。関係性は、④＞③。
        //④が決まった場合、③は無視し、①と②だけ計算する。④が空=Nonの場合、③を計算。④も③も空の場合、お菓子の種類は関係なくなる。
        girl1_likeOkashi[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_itemName;

        //セットごとの固有の好感度をセット
        girl1_like_set_score[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_set_score;

        //コメントをセット
        girllike_desc[_set_num] = girlLikeSet_database.girllikeset[setID].set_kansou;

        //お菓子食べた後の感想用フラグのセット
        girllike_comment_flag[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_comment_flag;

        //外部から直接指定されたとき用に、_descの中身も更新。
        //_desc = girllike_desc[0];

    }

    public void TouchSisterHair()
    {
        switch (Girl1_touchhair_status)
        {

            case 0: //初期化

                timeOut3 = 5.0f; //5秒以内に髪の毛を何度か触ると、ちょっと照れる。
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 1;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[0];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更
                face_girl_Surprise();

                break;

            case 1: //髪の毛触る回数カウント中

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 3) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 2;
                }
                break;

            case 2:

                timeOut3 = 5.0f;
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 3;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[1];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更
                face_girl_Tereru();

                break;

            case 3: //髪の毛触る回数カウント中＜2段階目＞

                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 3) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 4;
                }
                break;

            case 4:

                timeOut3 = 5.0f;
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 5;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[2];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更
                //s.sprite = Girl1_img_tereru;

                break;

            case 5:

                timeOut3 = 3.0f;
                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 6;
                }

                break;

            case 6:

                timeOut3 = 5.0f;
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 7;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[3];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更
                //s.sprite = Girl1_img_tereru;
                break;

            case 7:

                timeOut3 = 3.0f;
                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 8;
                }

                break;

            case 8:

                timeOut3 = 5.0f;
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 9;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[4];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更
                //s.sprite = Girl1_img_tereru;
                break;

            case 9:

                timeOut3 = 3.0f;
                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 10;
                }

                break;

            case 10:

                timeOut3 = 5.0f;
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 11;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                Init_touchHeadComment();
                _touchhead_comment = _touchhead_comment_lib[5];
                hukidashiitem.GetComponent<TextController>().SetText(_touchhead_comment);

                //キャラクタ表情変更　ちょっと嫌そう？真顔に。
                face_girl_Iya();

                break;

            case 11:

                timeOut3 = 3.0f;
                Girl1_touchhair_count++;

                if (Girl1_touchhair_count >= 7) //〇回以上触ると、ステータスが1段階上がる。
                {
                    Girl1_touchhair_status = 12;
                }

                break;

            case 12:

                timeOut3 = 5.0f;
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 13;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

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

                timeOut3 = 3.0f;
                Girl1_touchhair_count++;

                break;

            default:
                break;
        }             
        
    }

    public void Touchhair_Start()
    {
        Girl1_touchhair_status = 0;
        Girl1_touchhair_count = 0;
        Girl1_touchhair_start = true;
        GirlEat_Judge_on = false;
        timeOut3 = 7.0f;
    }

    //口のあたりをクリックすると、ヒントを表示する。
    public void TouchSisterFace()
    {
        /*
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }
        
        if (girl_Mazui_flag) //まずいフラグがたっていた場合、その時のクエストのヒントを教えてくれる。
        {
            Init_MazuiHintComment();
            _touchface_comment = MazuiHintComment;
        }
        else
        {
            //コメントランダム
            Init_touchFaceComment();
            random = Random.Range(0, _touchface_comment_lib.Count);
            _touchface_comment = _touchface_comment_lib[random];
        }*/

        //hukidashiitem.GetComponent<TextController>().SetText(_touchface_comment);

        //5個の中からランダムで選ぶ。宴のヒントの数と合わせているので、数には注意。
        random = Random.Range(0, 5);

        StartCoroutine("TouchFaceHintHyouji");

    }

    IEnumerator TouchFaceHintHyouji()
    {
        WaitHint_on = false;
        GirlEat_Judge_on = false;
        hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();

        GameMgr.touchhint_ID = random; //GirlLikeCompoSetの_set_compIDが入っている。
        GameMgr.touchhint_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        //カメラ寄る。
        trans = 1; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;

        hukidasiOn();
        canvas.SetActive(true);
        touch_controller.Touch_OnAllON();

        //5秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 5.0f;
        timeOut2 = 5.0f;
        GirlEat_Judge_on = false;

        //カメラ元に戻す
        trans = 0; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);
    }

    public void TouchSisterRibbon()
    {
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }

        //コメントランダム
        //random = Random.Range(0, _touchface_comment_lib.Count);
        //_touchface_comment = _touchface_comment_lib[random];

        hukidashiitem.GetComponent<TextController>().SetText("お母さんが誕生日にくれたリボンだよ～。うひひ。");

        //5秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 5.0f;
        GirlEat_Judge_on = false;
    }

    public void TouchSisterChest()
    {
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }

        hukidashiitem.GetComponent<TextController>().SetText("兄ちゃんのえっちー！");

        //5秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 5.0f;
        GirlEat_Judge_on = false;
    }

    public void TouchFlower()
    {
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }

        hukidashiitem.GetComponent<TextController>().SetText("お兄ちゃん。それは花だよ。しおれてたら、お水をあげてね。");

        //5秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 5.0f;
        GirlEat_Judge_on = false;
    }

    public void TouchSisterTwinTail()
    {
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }

        //コメント順番に表示
        Init_touchTwintailComment();
        //random = Random.Range(0, _touchtwintail_comment_lib.Count);  

        if (Girl1_touchtwintail_count >= _touchtwintail_comment_lib.Count)
        {
            Girl1_touchtwintail_flag = true;
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

        //5秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 5.0f;
        GirlEat_Judge_on = false;
    }

    IEnumerator WaitTwintailSeconds()
    {
        yield return new WaitForSeconds(10.0f);

        Girl1_touchtwintail_flag = false;
    }


    void Init_touchHeadComment()
    {
        //髪の毛触るときは、上から順番に表示されていく。回数に注意。
        _touchhead_comment_lib.Clear();

        switch (GirlGokigenStatus)
        {
            case 0:

                _touchhead_comment_lib.Add("兄ちゃん、頭なでなで?");
                _touchhead_comment_lib.Add("..");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add("（頭をなでられると嬉しいようだ..。）");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("..ガウゥ！！！！");
                break;

            case 1:

                _touchhead_comment_lib.Add("なでなで..!!");
                _touchhead_comment_lib.Add("..");
                _touchhead_comment_lib.Add("頭なでなで..。");
                _touchhead_comment_lib.Add("..♪");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("..ウガーー!!");
                break;

            case 2:

                _touchhead_comment_lib.Add("ん、どうした？兄。");
                _touchhead_comment_lib.Add("えへへ..。");
                _touchhead_comment_lib.Add("気持ちいい。さわさわ..。");
                _touchhead_comment_lib.Add("あ～～～..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("キィーーーーー！！！！");
                break;

            case 3:

                _touchhead_comment_lib.Add("ん、なでなでしてくれるの？");
                _touchhead_comment_lib.Add("えへへ..。頭なでなで。");
                _touchhead_comment_lib.Add("気持ちいい..。");
                _touchhead_comment_lib.Add("あ～～～..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("ガァーーー！！！！");
                break;

            case 4:

                _touchhead_comment_lib.Add("ん、なでなでして。兄ちゃん");
                _touchhead_comment_lib.Add("えへへ..。");
                _touchhead_comment_lib.Add("兄ちゃんの手、あったかい..。");
                _touchhead_comment_lib.Add("ほわ～～～..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("ギニャーーーー！！！！");
                break;

            case 5:

                _touchhead_comment_lib.Add("ん、いつものなでなでタイム..。");
                _touchhead_comment_lib.Add("えへへ..。");
                _touchhead_comment_lib.Add("兄ちゃんの手、あったかくて気持ちいい。");
                _touchhead_comment_lib.Add("お菓子のにおい..。");
                _touchhead_comment_lib.Add("..。");
                _touchhead_comment_lib.Add(".. ..。");
                _touchhead_comment_lib.Add("さわりすぎ！！！！");
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
                _touchface_comment_lib.Add("..ママ。");
                _touchface_comment_lib.Add("ぐすん..。");
                _touchface_comment_lib.Add("..おかし、食べたい。");
                _touchface_comment_lib.Add("うぇぇ..。");
                _touchface_comment_lib.Add("兄ちゃん..。");
                break;

            case 1:

                _touchface_comment_lib.Add(".. ..。");
                _touchface_comment_lib.Add("（..ママ。）");
                _touchface_comment_lib.Add("お腹へった。");
                _touchface_comment_lib.Add("..兄ちゃんのおかし、食べたい。");
                _touchface_comment_lib.Add("兄ちゃんのお菓子作り、てつだう。");
                break;

            case 2:

                _touchface_comment_lib.Add("ちょっと元気。");
                _touchface_comment_lib.Add("材料の比率は、兄ちゃんの好みに変えられるんだよ～。");
                _touchface_comment_lib.Add("..。");
                _touchface_comment_lib.Add("おいしい。兄ちゃんのお菓子。");
                break;

            case 3:

                _touchface_comment_lib.Add("エメラルド色のどんぐり、欲しい？兄ちゃん。");
                _touchface_comment_lib.Add("うきうき。いっぱい手伝うよ～！");
                _touchface_comment_lib.Add("味見..。味見..。");
                _touchface_comment_lib.Add("ねぇねぇ兄ちゃん。材料を採りにいこうよ～。");
                break;

            case 4:

                _touchface_comment_lib.Add("るんるん♪");
                _touchface_comment_lib.Add("材料の比率は、兄ちゃんの好みに変えられるんだよ～。");
                _touchface_comment_lib.Add("兄ちゃん、もうコンテストとか余裕？");
                _touchface_comment_lib.Add("あ～～。今日はあたたかいね、兄ちゃん！");
                break;

            case 5:

                _touchface_comment_lib.Add("キラキラ♪");
                _touchface_comment_lib.Add("兄ちゃん！大好き！！");
                _touchface_comment_lib.Add("兄ちゃんのお菓子、こころがぽかぽかするんだ～");
                _touchface_comment_lib.Add("兄ちゃんのおてて、あたたか～い");
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

                _touchtwintail_comment_lib.Add("..");
                _touchtwintail_comment_lib.Add("（髪の毛さらさら）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..さらさら。");
                _touchtwintail_comment_lib.Add("..。");
                break;

            case 1:

                _touchtwintail_comment_lib.Add("..!");
                _touchtwintail_comment_lib.Add("（髪の毛さらさら）");
                _touchtwintail_comment_lib.Add("..気持ちいい。");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（ちょっと元気になってきたかな？）");
                break;

            case 2:

                _touchtwintail_comment_lib.Add("髪の毛が気になるの？");
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

                _touchtwintail_comment_lib.Add("わ～い♪");
                _touchtwintail_comment_lib.Add("髪の毛さらさら。気持ちいい。");
                _touchtwintail_comment_lib.Add("しゃらら～ん");
                _touchtwintail_comment_lib.Add("あ～..♪");
                _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
                _touchtwintail_comment_lib.Add("..。");
                _touchtwintail_comment_lib.Add("（さらさら..。）");
                break;
        }
        
    }

    void RandomGenkiInit()
    {
        _hintrandomDict.Clear();

        switch (GirlGokigenStatus)
        {
            case 0:

                _hintrandomDict.Add("..。");
                _hintrandomDict.Add("..ママ。");
                _hintrandomDict.Add("..ぐすん。");
                _hintrandomDict.Add("..おなか、すいた。");
                break;

            case 1:

                _hintrandomDict.Add("..。");
                _hintrandomDict.Add("..ママ。");
                _hintrandomDict.Add("..ママ。会いたいなぁ..。");
                break;

            case 2:

                _hintrandomDict.Add("（ちょっと元気）");
                _hintrandomDict.Add("兄ちゃんのおかし.. たべたいなぁ～。");
                _hintrandomDict.Add("おいしい。兄ちゃんのお菓子。");
                break;

            case 3:

                _hintrandomDict.Add("いい朝だねぇ～。お兄ちゃん～。");
                _hintrandomDict.Add("いっぱい手伝うね！お兄ちゃん。");
                break;

            case 4:

                _hintrandomDict.Add("♪");
                _hintrandomDict.Add("エメラルどんぐり、拾いにいこうよ～。お兄ちゃん。");
                break;

            case 5:

                _hintrandomDict.Add("お兄ちゃん。あたたかい～。");
                _hintrandomDict.Add("どこかへ出かけたいなぁ～");
                break;
        }       
    }

    void Init_MazuiHintComment()
    {
        switch(OkashiQuest_ID)
        {
            case 1010:

                if (GameMgr.scenario_flag == 160)
                {
                    switch (MazuiStatus)
                    {
                        case 0:

                            MazuiHintComment = "兄ちゃん..。ちょっとパンが粉っぽい気がする。";
                            break;

                        case 1:

                            MazuiHintComment = "でもカリカリ..。";
                            break;
                    }

                    MazuiStatus++;
                    if (MazuiStatus >= 2)
                    {
                        MazuiStatus = 0;
                    }
                }

                if (GameMgr.scenario_flag == 170)
                {
                    MazuiHintComment = "兄ちゃん。井戸へ行こう！";
                }

                break;

            default:

                break;
        }
    }

    //表情を変更するメソッド
    public void face_girl_Normal()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 1; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Joukigen()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 2; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Bad()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 3; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Little_Fine()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 4; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Fine()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 5; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }    

    public void face_girl_Tereru()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 6; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);
    }

    public void face_girl_Yorokobi()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 7; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

        //s.sprite = Girl1_img_smile;
    }

    public void face_girl_Angry()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 8; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

        //s.sprite = Girl1_img_angry;
    }

    public void face_girl_Hirameki()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 9; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

        //s.sprite = Girl1_img_hirameki;
    }

    public void face_girl_Mazui()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 10; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Mazui2()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 11; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Iya()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 12; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }

    public void face_girl_Surprise()
    {
        //intパラメーターの値を設定する.  
        trans_expression = 13; //各表情に遷移。
        live2d_animator.SetInteger("trans_expression", trans_expression);

    }


    void Init_Stage1_LVTable()
    {
        stage1_lvTable.Clear();
        stage1_lvTable.Add(20); //LV1で、次のレベルが上がるまでの好感度値
        stage1_lvTable.Add(25);　//LV2 LV1の分も含めている。トータルだと75。の意味。
        stage1_lvTable.Add(50); //LV3　LV1~LV2の分も含めている。
        stage1_lvTable.Add(100); //LV4
        stage1_lvTable.Add(150); //LV5以上

        _temp_lvTablecount = stage1_lvTable.Count;

        //LV6以上～99まで　200ごとに上がるように設定
        for (i=0; i < ( 99 - _temp_lvTablecount); i++)
        {
            stage1_lvTable.Add(200);
        }
    }

    public int SumLvTable(int _count)
    {
        _sum = 0;

        for(i=0; i < _count; i++)
        {
            _sum += stage1_lvTable[i];
        }

        return _sum;
    }
}

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

    public float timeOut;
    public float timeOut2;
    public float timeOut3;
    public int timeGirl_hungry_status; //今、お腹が空いているか、空いてないかの状態

    public bool GirlEat_Judge_on;

    private GameObject text_area;

    private GameObject hukidashiPrefab;
    private GameObject canvas;

    public GameObject hukidashiitem;
    private Text _text;

    private GameObject MoneyStatus_Panel_obj;

    private GameObject Extremepanel_obj;

    public int touch_status; //今どこを触っているかの状態。TimeOutが入り組んで、ぐちゃぐちゃにならないように分ける。

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

    public int[] girl1_like_set_score;

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


    //採点結果　宴と共有する用のパラメータ。採点は、GirlEat_Judgeで行っている。
    public int girl_final_kettei_item;
    public int itemLike_score_final;

    public int quality_score_final;

    public int rich_score_final;
    public int sweat_score_final;
    public int bitter_score_final;
    public int sour_score_final;

    public int crispy_score_final;
    public int fluffy_score_final;
    public int smooth_score_final;
    public int hardness_score_final;
    public int jiggly_score_final;
    public int chewy_score_final;

    public int subtype1_score_final;
    public int subtype2_score_final;

    public int total_score_final;

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
    public List<int> girl1_hungryScoreSet1 = new List<int>();
    public List<int> girl1_hungryScoreSet2 = new List<int>();
    public List<int> girl1_hungryScoreSet3 = new List<int>();

    public List<int> girl1_hungrySet = new List<int>();  //①食べたいトッピングスロットのリスト


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
    public int Special_ignore_count; //スペシャルを無視した場合、カウント。3回あたりをこえると、スペシャルは無視される。

    //エフェクト関係
    private GameObject Emo_effect_Prefab1;
    private GameObject Emo_effect_Prefab2;
    private GameObject Emo_effect_Prefab3;
    private List<GameObject> _listEffect = new List<GameObject>();
    private GameObject character;

    //Live2Dモデルの取得
    private CubismModel _model;

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

        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;

        

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //女の子の顔を触った時のヒントライブラリー初期化
        Init_touchFaceComment();
        Init_touchTwintailComment();

        //この時間ごとに、女の子は、お菓子を欲しがり始める。
        timeOut = 1.0f;
        timeOut2 = 10.0f;
        timeGirl_hungry_status = 1;

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

                //エクストリームパネルの取得
                Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");

                //お金の増減用パネルの取得
                MoneyStatus_Panel_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;

                //タッチ判定オブジェクトの取得
                touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();
                break;
        }

        //女の子のイラストデータ
        Girl1_img_normal = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_normal");
        Girl1_img_gokigen = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_gokigen");
        Girl1_img_smile = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_yorokobi");
        Girl1_img_eat_start = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_eat_start");
        Girl1_img_verysad = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_verysad");
        Girl1_img_verysad_close = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_verysad_close");
        Girl1_img_hirameki = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_hirameki");
        Girl1_img_tereru = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_tereru");
        Girl1_img_angry = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_angry");
        Girl1_img_iya = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_iya");

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

        girl1_likeSubtype = new string[youso_count];
        girl1_likeOkashi = new string[youso_count];
        girllike_desc = new string[youso_count];

        //ステージごとに、女の子が食べたいお菓子のセットを初期化
        InitializeStageGirlHungrySet(0, 0); //とりあえず0で初期化

        // *** ここまで *** 

        //エフェクトプレファブの取得
        Emo_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Emo_Hirameki_Anim");
        Emo_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Emo_Kirari_Anim");
        Emo_effect_Prefab3 = (GameObject)Resources.Load("Prefabs/Emo_Angry_Anim");

        //ヒントテキストをセッティング
        _hint1 = "まずは、左のパネルでお菓子を作ろうね！お兄ちゃん。";
        RandomHintInit();
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

                    compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                    compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                    character = GameObject.FindWithTag("Character");

                    //テキストエリアの取得
                    text_area = canvas.transform.Find("MessageWindow").gameObject;

                    //エクストリームパネルの取得
                    Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");

                    //お金の増減用パネルの取得
                    MoneyStatus_Panel_obj = canvas.transform.Find("MoneyStatus_panel").gameObject;

                    s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();

                    //タッチ判定オブジェクトの取得
                    touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

                    //Live2Dモデルの取得
                    _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                    break;

                case "Shop":

                    //カメラの取得
                    main_cam = Camera.main;
                    maincam_animator = main_cam.GetComponent<Animator>();
                    trans = maincam_animator.GetInteger("trans");
                    
                    break;
            }
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

        if(WaitHint_on) //感想や触ったコメント表示したあと、_descの内容に戻す。
        {
            timeOutHint -= Time.deltaTime;

            if (timeOutHint <= 0.0f)
            {
                //吹き出しが残っていたら、削除。
                if (hukidashiitem != null)
                {
                    if (_desc == "")
                    {
                        DeleteHukidashi();
                    }
                    else
                    {
                        DeleteHukidashi();
                        //_text.text = _desc;
                    }
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
                                    timeOut = 5.0f + rnd;
                                    Girl_Hungry();

                                    //キャラクタ表情変更
                                    s.sprite = Girl1_img_gokigen;
                                    break;

                                case 1:

                                    timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                    rnd = Random.Range(1.0f, 5.0f);
                                    timeOut = 2.0f + rnd;
                                    Girl_Full();

                                    //キャラクタ表情変更
                                    s.sprite = Girl1_img_gokigen;
                                    break;

                                case 2:

                                    //お菓子をあげたあとの状態。

                                    timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                    timeOut = 5.0f;
                                    Girl_Full();

                                    //キャラクタ表情変更
                                    //s.sprite = Girl1_img_gokigen;
                                    break;

                                default:

                                    timeOut = 5.0f;
                                    break;
                            }

                        }

                        //一定時間たつとヒントを出す。
                        if (timeOut2 <= 0.0)
                        {
                            rnd = Random.Range(10.0f, 20.0f);
                            timeOut2 = 20.0f + rnd;

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

                    Extremepanel_obj.SetActive(false);
                    MoneyStatus_Panel_obj.SetActive(false);
                    text_area.SetActive(false);

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
                    s.sprite = Girl1_img_hirameki;

                    special_animstart_status = 1;
                    break;

                case 1: //処理待ち

                    if (special_timeOut <= 0.0f)
                    {
                        special_animstart_status = 2;
                    }
                    break;

                case 2:

                    Extremepanel_obj.SetActive(true);
                    MoneyStatus_Panel_obj.SetActive(true);
                    text_area.SetActive(true);

                    _listEffect.Clear();

                    //カメラ引く。
                    trans = 0; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //キャラクタ表情変更
                    s.sprite = Girl1_img_smile;

                    special_animstart_flag = false;
                    special_animstart_endflag = true;
                    break;
            }

            special_timeOut -= Time.deltaTime;
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
                    //イベントやチュートリアル、ある好感度をこえたときの条件によって、こちらの特定のアイテムを常に出すようにする。
                    //

                    //番号を入れると、女の子の好みデータベースから、値を取得し、セット。OkashiQuest_IDは、外部から指定。
                    glike_compID = OkashiQuest_ID;

                    //今選んだやつの、girllikeComposetのIDも保存しておく。（こっちは直接選んでいる。）
                    Set_compID = glike_compID;

                    //OkashiQuest_ID = compIDを指定すると、女の子が食べたいお菓子＜組み合わせ＞がセットされる。
                    SetQuestRandomSet(glike_compID, false);

                    //一度、一度ドアップになり、電球がキラン！　→　そのあと、クエストの吹き出し。最初の一回だけ。
                    StartCoroutine("Special_StartAnim");
                    

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

                    //表示用吹き出しを生成                    
                    //hukidasiInit();  
                    

                    //吹き出しのテキスト決定
                    //hukidashiitem.GetComponent<TextController>().SetText(_desc);
                    //_text = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                    //_text.text = _desc;

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
                case 1010:

                    GameMgr.scenario_flag = 150;
                    break;

                default:

                    break;
            }

            GameMgr.scenario_ON = false;
            GameMgr.recipi_read_endflag = false;
            touch_controller.Touch_OnAllON();
            canvas.SetActive(true);
        }       

        //表示用吹き出しを生成                   
        hukidasiInit();

        //吹き出しのテキスト決定
        //hukidashiitem.GetComponent<TextController>().SetText(_desc);
        _text = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
        _text.text = _desc;

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
        timeOut = 5.0f;
        timeOut2 = 10.0f;
        timeGirl_hungry_status = 0;

        DeleteHukidashi();
    }

    
    public void Girl1_Status_Init()
    {
        timeOut = 5.0f;

        timeGirl_hungry_status = 0;
    }

    public void Girl1_Status_Init2()
    {
        timeOut = 1.0f;

        timeGirl_hungry_status = 0;
    }

    //
    // らんだむで表示される女の子のセリフ。ヒントか、とりとめもないこと
    //
    public void Girl1_Hint()
    {
        if (hukidashiitem == null)
        {
            hukidasiInit();
        }

        //好感度25以下で、かつまだ一度も調合していない
        if (girl1_Love_exp <= 25 && PlayerStatus.First_recipi_on != true)
        {                      
            hukidashiitem.GetComponent<TextController>().SetText(_hint1);
        }
        else
        {
            //ランダムで、吹き出しの内容を決定
            random = Random.Range(0, _hintrandomDict.Count);
            _hintrandom = _hintrandomDict[random];

            hukidashiitem.GetComponent<TextController>().SetText(_hintrandom);
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
            hukidashiitem.transform.Find("Image_special").gameObject.SetActive(true);
            hukidashiitem.transform.Find("Image").gameObject.SetActive(false);
        }
        else
        {
            hukidashiitem.transform.Find("Image_special").gameObject.SetActive(false);
            hukidashiitem.transform.Find("Image").gameObject.SetActive(true);
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
        }

        //まず全ての値を0に初期化
        for (i = 0; i < girl1_hungryScoreSet1.Count; i++)
        {
            girl1_hungryScoreSet1[i] = 0;
            girl1_hungryScoreSet2[i] = 0;
            girl1_hungryScoreSet3[i] = 0;
        }

        //音を鳴らす
        audioSource.PlayOneShot(sound2);

        //キャラクタ表情変更
        //s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();
        //s.sprite = Girl1_img_gokigen;
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

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            girl1_hungryInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            girl1_hungryScoreSet1.Add(0);
            girl1_hungryScoreSet2.Add(0);
            girl1_hungryScoreSet3.Add(0);
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
                
                setID = j;
                break;
            }
            j++;
        }

        //初期化
        girl1_hungrySet.Clear();


        //ステージごとに、女の子が欲しがるアイテムのセット

        //セット例
        //①スロット：　オレンジ・ナッツ・ぶどう

        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {

            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[0])
            {

                if(girlLikeSet_database.girllikeset[setID].girlLike_topping[0] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[1])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[1] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[2])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[2] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[3])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[3] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[4])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[4] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }
            }
        }

        
        //以下、パラメータのセッティング

        //①女の子の食べたいトッピング

        switch(_set_num)
        {
            case 0:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet1.Count; i++)
                {
                    girl1_hungryScoreSet1[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet1[girl1_hungrySet[i]]++;
                }
                break;

            case 1:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet2.Count; i++)
                {
                    girl1_hungryScoreSet2[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet2[girl1_hungrySet[i]]++;
                }
                break;

            case 2:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet3.Count; i++)
                {
                    girl1_hungryScoreSet3[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet3[girl1_hungrySet[i]]++;
                }
                break;

            default:
                break;
        }
        

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

        //外部から直接指定されたとき用に、_descの中身も更新。
        //_desc = girllike_desc[0];

        //Debug.Log("_desc: " + _desc);
    }

    public void TouchSisterHair()
    {
        switch(Girl1_touchhair_status)
        {

            case 0: //初期化

                timeOut3 = 5.0f; //5秒以内に髪の毛を何度か触ると、ちょっと照れる。
                Girl1_touchhair_count = 0;
                Girl1_touchhair_status = 1;

                if (hukidashiitem == null)
                {
                    hukidasiInit();
                }

                hukidashiitem.GetComponent<TextController>().SetText("ん、どうした？兄。");
               
                break;

            case 1: //髪の毛触る回数カウント中

                Girl1_touchhair_count++;

                if(Girl1_touchhair_count >= 3) //〇回以上触ると、ステータスが1段階上がる。
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

                hukidashiitem.GetComponent<TextController>().SetText("えへへ..。");

                //キャラクタ表情変更
                s.sprite = Girl1_img_tereru;

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

                hukidashiitem.GetComponent<TextController>().SetText("気持ちいい。さわさわ..。");

                //キャラクタ表情変更
                s.sprite = Girl1_img_tereru;

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

                hukidashiitem.GetComponent<TextController>().SetText("あ～～～..。");

                //キャラクタ表情変更
                s.sprite = Girl1_img_tereru;
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

                hukidashiitem.GetComponent<TextController>().SetText("..。");

                //キャラクタ表情変更
                s.sprite = Girl1_img_tereru;
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

                hukidashiitem.GetComponent<TextController>().SetText(".. ..。");

                //キャラクタ表情変更
                s.sprite = Girl1_img_iya;
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

                hukidashiitem.GetComponent<TextController>().SetText("キシャーーーーー！！！！");

                //音鳴らす
                sc.PlaySe(45);

                //キャラクタ表情変更
                s.sprite = Girl1_img_angry;

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

    public void TouchSisterFace()
    {
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
            random = Random.Range(0, _touchface_comment_lib.Count);
            _touchface_comment = _touchface_comment_lib[random];
        }

        hukidashiitem.GetComponent<TextController>().SetText(_touchface_comment);

        //5秒ほど表示したら、また食べたいお菓子を表示か削除
        WaitHint_on = true;
        timeOutHint = 5.0f;
        GirlEat_Judge_on = false;
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

        hukidashiitem.GetComponent<TextController>().SetText("兄ちゃんが誕生日にくれたリボンだよ～。うひひ。");

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

        //コメントランダム
        //random = Random.Range(0, _touchface_comment_lib.Count);
        //_touchface_comment = _touchface_comment_lib[random];

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

        //コメントランダム
        //random = Random.Range(0, _touchface_comment_lib.Count);
        //_touchface_comment = _touchface_comment_lib[random];

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
        //random = Random.Range(0, _touchtwintail_comment_lib.Count);                  
        if(Girl1_touchtwintail_count >= _touchtwintail_comment_lib.Count)
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

    void Init_touchFaceComment()
    {
        _touchface_comment_lib.Add("森へ行くと、エメラルド色のどんぐりが採れるんだよ！お兄ちゃん。");
        _touchface_comment_lib.Add("材料の比率は、兄ちゃんの好みに変えられるんだよ～。");
        _touchface_comment_lib.Add("味見..。味見..。");
        _touchface_comment_lib.Add("ねぇねぇ兄ちゃん。まずは材料を採りにいこうよ～。");
    }

    void Init_touchTwintailComment()
    {
        _touchtwintail_comment_lib.Add("髪の毛が気になるの？");
        _touchtwintail_comment_lib.Add("お母さんゆずりで、さらさらなんだよ～。");
        _touchtwintail_comment_lib.Add("お母さん、元気かなぁ～..。");
        _touchtwintail_comment_lib.Add("..。");
        _touchtwintail_comment_lib.Add("（気持ちいいようだ..。）");
        _touchtwintail_comment_lib.Add("..。");
        _touchtwintail_comment_lib.Add("（さらさら..。）");
    }

    void RandomHintInit()
    {
        _hintrandomDict.Add("いい朝だねぇ～。お兄ちゃん～。");
        _hintrandomDict.Add("エメラルどんぐり、拾いにいこうよ～。お兄ちゃん。");
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
}

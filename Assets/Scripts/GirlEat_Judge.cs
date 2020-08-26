//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

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

    private KaeruCoin_Controller kaerucoin_Controller;

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

    private GameObject Girlloveexp_bar;

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
    private bool non_spquest_flag;

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

    private GameObject hukidashiitem;   
    private Text _hukidashitext;

    private GameObject eat_hukidashiPrefab;
    private GameObject eat_hukidashiitem;
    private Text eat_hukidashitext;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count;
    private int random;
    private int countNum;

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

    private GameObject EatAnimPanel;
    private Image EatAnimPanel_itemImage;
    private Texture2D texture2d;
    private GameObject EatStartEffect;

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

    public int total_score;

    private bool dislike_flag;
    private int dislike_status;

    public int girllike_point;

    private bool emerarudonguri_get;
    private bool last_score_kousin;
    private string shokukan_mes;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの所持数
    List<int> itemslotScore = new List<int>();

    private GameObject effect_Prefab;
    private GameObject Emo_effect_manzoku;
    private GameObject Emo_effect_daimanzoku;
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

    //Live2Dモデルの取得
    private CubismModel _model;
    private Animator live2d_animator;
    private CubismRenderController _renderController;

    private GameObject stageclear_toggle;
    private GameObject stageclear_Button;
    private bool stageclear_button_on;

    private int contest_type;

    // Use this for initialization
    void Start() {

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

        //好感度ゲージの取得
        Girlloveexp_bar = GameObject.FindWithTag("Girl_love_exp_bar");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();


        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                //エクストリームパネルの取得
                Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");
                extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();

                //タッチ判定オブジェクトの取得
                touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

                //Live2Dモデルの取得
                _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                live2d_animator = _model.GetComponent<Animator>();
                _renderController = _model.GetComponent<CubismRenderController>();

                //キャラクタ取得
                character = GameObject.FindWithTag("Character");

                //お金の増減用パネルの取得
                MoneyStatus_Panel_obj = GameObject.FindWithTag("Canvas").transform.Find("MoneyStatus_panel").gameObject;
                moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

                //エメラルドングリパネルの取得
                kaerucoin_Controller = canvas.transform.Find("KaeruCoin_Panel").GetComponent<KaeruCoin_Controller>();

                //女の子の反映用ハートエフェクト取得
                GirlHeartEffect_obj = GameObject.FindWithTag("Particle_Heart_Character");
                GirlHeartEffect = GirlHeartEffect_obj.GetComponent<Particle_Heart_Character>();

                //女の子のレベル取得
                girl_lv = GameObject.FindWithTag("Girl_love_exp_bar").transform.Find("LV_param").GetComponent<Text>();
                girl_lv.text = girl1_status.girl1_Love_lv.ToString();

                //エフェクトプレファブの取得
                effect_Prefab = (GameObject)Resources.Load("Prefabs/Particle_Heart");
                Emo_effect_manzoku = (GameObject)Resources.Load("Prefabs/Emo_HeartAnimL");
                Emo_effect_daimanzoku = (GameObject)Resources.Load("Prefabs/Emo_HeartAnimL");
                Score_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_ResultFeather");
                Score_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_Compo5");                

                //ハートプレファブの取得
                heart_Prefab = (GameObject)Resources.Load("Prefabs/HeartUpObj");
                hearthit_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect");
                hearthit2_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect2");

                //Prefab内の、コンテンツ要素を取得
                eat_hukidashiPrefab = (GameObject)Resources.Load("Prefabs/Eat_hukidashi");

                //食べ始めアニメオブジェクトの取得
                EatAnimPanel = canvas.transform.Find("EatAnimPanel").gameObject;
                EatAnimPanel_itemImage = EatAnimPanel.transform.Find("ItemImage").GetComponent<Image>();
                EatAnimPanel.SetActive(false);

                EatStartEffect = GameObject.FindWithTag("EatAnim_Effect").transform.Find("Comp").gameObject;

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

                //クエストクリアボタンの取得
                stageclear_toggle = canvas.transform.Find("CompoundSelect_ScrollView").transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;
                stageclear_Button = canvas.transform.Find("StageClear_Button").gameObject;
                stageclear_button_on = false;

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

                        for (i = 0; i < girl1_status.stage1_lvTable.Count; i++)
                        {
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
                while (i < girl1_status.girl1_Love_lv - 1)
                {
                    girllove_param -= stage_levelTable[i];
                    i++;
                }
                _slider.value = girllove_param;

                //レベルを表示
                girl_lv.text = girl1_status.girl1_Love_lv.ToString();

                //windowテキストエリアの取得
                text_area = canvas.transform.Find("MessageWindowMain").gameObject;
                _windowtext = text_area.GetComponentInChildren<Text>();

                break;

            case "Contest":

                //テキストエリアの取得　コンテストの場合
                text_area = canvas.transform.Find("MessageWindow").gameObject;
                _windowtext = text_area.GetComponentInChildren<Text>();
                break;
                
        }

        
               
        audioSource = GetComponent<AudioSource>();

        kettei_item1 = 0;
        _toggle_type1 = 0;

        dislike_flag = true;
        emerarudonguri_get = false;

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();
      
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
        _girl_comment_flag = new int[girl1_status.youso_count];

        //サブクエストチェック用フラグ
        subQuestClear_check = false;
        HighScore_flag = false;
        Gameover_flag = false;
        kansou_on = false;

        //テキストのセッティング
        CommentTextInit();

        _basetp = new string[database.items[0].toppingtype.Length];
        _koyutp = new string[database.items[0].koyu_toppingtype.Length];
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
                    Girlloveexp_bar.SetActive(false);
                    if (stageclear_Button.activeInHierarchy)
                    {
                        stageclear_button_on = true;
                        stageclear_Button.SetActive(false);
                    }

                    timeOut = 4.0f;
                    judge_anim_status = 1;

                    //現在の吹き出しを削除
                    girl1_status.DeleteHukidashiOnly();

                    //食べ始めのアニメーションをスタート
                    EatAnimPanel.SetActive(true);
                    texture2d = database.items[_baseID].itemIcon;
                    EatAnimPanel_itemImage.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);
                    


                    //カメラ寄る。
                    trans = 2; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    break;

                case 1: // 状態2

                    if( timeOut <= 0.0)
                    {
                        timeOut = 1.0f;
                        judge_anim_status = 3;

                        //eat_hukidashitext.text = ". .";
                        
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
                    Girlloveexp_bar.SetActive(true);                    

                    if (stageclear_button_on)
                    {
                        stageclear_Button.SetActive(true);
                    }

                    //食べ中吹き出しの削除
                    if (eat_hukidashiitem != null)
                    {
                        Destroy(eat_hukidashiitem);
                    }

                    judge_anim_on = false;
                    judge_end = true;
                    judge_anim_status = 0;

                    //カメラ寄る。
                    trans = 0; //transが0以下のときに、ズームアウトするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    EatAnimPanel.SetActive(false);
                    EatStartEffect.SetActive(false);

                    break;

                default:
                    break;
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }
    }

    //選んだアイテムと、女の子の好みを比較するメソッド

    public void Girleat_Judge_method(int value1, int value2, int _Type)
    {

        //一度、決定したアイテムのリスト番号と、タイプを取得
        kettei_item1 = value1;
        _toggle_type1 = value2;

        contest_type = _Type;

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



        //** 判定用に、女の子の好み値(GirlLikeSet)をセッティング

        if (contest_type == 0) //コンテストではこのセッティングは使用しない
        {
            //通常の場合は、あげたお菓子によって、その好み値をセッティングする。girlLikeSetのcompNum番号を指定して、判定用に使う。
            if (girl1_status.OkashiNew_Status == 1)
            {
                girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
            }

            SetGirlTasteInit();
        }
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
                if ( _basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
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

        if (contest_type == 0) //コンテストでは使用しない
        {
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
        }
        

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
            _girl_comment_flag[i] = girl1_status.girllike_comment_flag[i];
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

        judge_score(0, set_id); //点数の判定。中の数字は、女の子のお菓子の判定か、コンテストでの判定かのタイプ分け

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        if (!GameMgr.tutorial_ON)
        {
            //お菓子を食べた後のちょっとした感想をだす。
            if (dislike_status == 1 || dislike_status == 2 || dislike_status == 6)
            {                
                StartCoroutine("Girl_Comment");
            }
            else if (dislike_status == 3 || dislike_status == 4)//まずいとき
            {
                StartCoroutine("Girl_Comment");
            }
            else if (dislike_status == 5)
            {
                Girl_reaction();
            }
        }
        else
        {
            Girl_reaction();
        }
    }    

    void Dislike_Okashi_Judge()
    {
        Mazui_flag = false; //初期化

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

        i = 0;
        while (i < itemslotScore.Count) //トッピングスロットを計算。嫌いなトッピングが使われていると、嫌われる。
        {
            //0はNonなので、無視
            if (i != 0)
            {
                //あげたアイテムに、女の子の嫌いな材料が使われていた！
                if (slotnamedatabase.slotname_lists[i].slot_totalScore < 0 && itemslotScore[i] > 0)
                {
                    dislike_flag = false;
                    dislike_status = 4;
                    break;
                }
            }
            i++;
        }

        if(!dislike_flag)
        {
            //粉っぽさなどのマイナス判定。一番強い。ここであまりに粉っぽさなどが強い場合は、問答無用で嫌われる。
            Mazui_flag = true;

            //スペシャルクエストだった場合は、まずいフラグがたつ。
            /*if (girl1_status.OkashiNew_Status == 0)
            {
                girl1_status.girl_Mazui_flag = true;
            }*/
        }
    }

    void judge_result()
    {
        non_spquest_flag = false;

        if (GameMgr.GirlLoveEvent_num == 5 && contest_type == 0) //コンテストのときに「あげる」をおすと、こちらの処理
        {
            non_spquest_flag = true;

            //お菓子の判定値をセッティング
            girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
            SetGirlTasteInit();

            dislike_flag = true;
            dislike_status = 1; //1=デフォルトで良い。 2=新しいお菓子だった。　3=まずい。　4=嫌い。 5=今はこれの気分じゃない。
            set_id = 0;

            //
            //判定処理　パターンCのみ
            //
            Dislike_Okashi_Judge();
        }
        else
        {
            //通常
            if (girl1_status.OkashiNew_Status == 1)
            {
                dislike_flag = true;
                dislike_status = 1; //1=デフォルトで良い。 2=新しいお菓子だった。　3=まずい。　4=嫌い。 5=今はこれの気分じゃない。
                set_id = 0;

                //
                //判定処理　パターンCのみ
                //
                Dislike_Okashi_Judge();

            }
            //スペシャルお菓子の場合
            else if (girl1_status.OkashiNew_Status == 0)
            {

                count = 0;
                dislike_status = 1;

                //Debug.Log("girl1_status.Set_Count: " + girl1_status.Set_Count);
                while (count < girl1_status.Set_Count) //セットの組み合わせの数だけ判定。最大３。どれか一つのセットが条件をクリアしていれば、正解。
                {
                    //パラメータ初期化し、判定処理
                    dislike_flag = true;                    
                    set_id = count;


                    //
                    //判定処理　パターンA
                    //                    

                    //④特定のお菓子の判定。④が一致していない場合は、③は計算するまでもなく不正解となる。
                    if (_girl_likeokashi[count] == "Non") //特に指定なし
                    {
                        //③お菓子の種別の計算
                        if (_girl_subtype[count] == "Non") //特に指定なし
                        {
                            dislike_flag = true;
                        }
                        else if (_girl_subtype[count] == _baseitemtype_sub) //お菓子の種別が一致している。
                        {
                            dislike_flag = true;
                        }
                        else
                        {
                            dislike_flag = false;
                        }
                    }
                    else if (_girl_likeokashi[count] == _basename) //お菓子の名前が一致している。
                    {
                        //サブは計算せず、特定のお菓子自体が正解なら、正解。ピンポイントで正解。
                        dislike_flag = true;
                    }
                    else
                    {
                        dislike_flag = false;
                    }

                    Debug.Log("あげたお菓子: " + _basename);

                    //判定 嫌いなものがなければbreak。falseだった場合、次のセットを見る。
                    if (dislike_flag)
                    {
                        break;
                    }

                    count++;
                }


                //この時点で、吹き出しと違うものであれば、dislike_flagがfalse。

                //
                //判定処理　パターンB
                //

                //吹き出しにあっているかいないかの判定。
                if (dislike_flag == false) //吹き出しに合っていない場合
                {

                    //dislike_status = 5; //スペシャルクエストだった場合は、これじゃないという。

                    //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。

                    non_spquest_flag = true;

                    if (database.items[_baseID].Eat_kaisu == 0) //新しい食べ物の場合
                    {
                        //お菓子の判定値をセッティング
                        girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
                        SetGirlTasteInit();

                        dislike_flag = true;
                        dislike_status = 2;

                        //
                        //判定処理　パターンCのみ
                        //
                        Dislike_Okashi_Judge();
                    }
                    else
                    {
                        //お菓子の判定値をセッティング
                        girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
                        SetGirlTasteInit();

                        dislike_flag = true;
                        dislike_status = 1; //1=デフォルトで良い。 2=新しいお菓子だった。　3=まずい。　4=嫌い。 5=今はこれの気分じゃない。
                        set_id = 0;

                        //
                        //判定処理　パターンCのみ
                        //
                        Dislike_Okashi_Judge();

                    }
                }
                else //吹き出しに合っていた場合に、味を判定する。
                {
                    //
                    //判定処理　パターンCのみ
                    //
                    Dislike_Okashi_Judge();
                }

            }
        }
    }

    public void judge_score(int _setType, int _setCountNum)
    {
        //初期化。

        //お菓子の判定処理

        //クッキーの場合はさくさく感など。大きいパラメータをまず見る。次に甘さ・苦さ・酸味が、女の子の好みに近いかどうか。
        countNum = _setCountNum;
        total_score = 0;
        crispy_score = 0;
        fluffy_score = 0;
        smooth_score = 0;
        hardness_score = 0;
        topping_score = 0;
        topping_flag_point = 0;
        topping_flag = false;
        topping_all_non = true; //判定のトッピングスロットが全てNon
        last_score_kousin = false;

        //未使用。

        rich_score = 0;
        jiggly_score = 0;
        chewy_score = 0;


        //基本得点
        quality_score = _girl_set_score[countNum];

        //味パラメータの計算。味は、GirlLikeSetの値で、理想値としている。
        //理想の値に近いほど高得点。超えすぎてもいけない。

        //rich_result = _baserich - _girlrich[countNum];
        sweat_result = _basesweat - _girlsweat[countNum];
        bitter_result = _basebitter - _girlbitter[countNum];
        sour_result = _basesour - _girlsour[countNum];


        //あまみ・にがみ・さんみに対して、それぞれの評価。差の値により、7段階で評価する。
        //元のセットの値が0のときは、計算せずスコアに加点しない。

        if (_girlsweat[countNum] == 0)
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
            else if (Mathf.Abs(sweat_result) > 100)
            {
                Debug.Log("甘み: death..");
                sweat_level = 1;
                sweat_score = -80;
            }
        }
        Debug.Log("甘み点: " + sweat_score);

        if (_girlbitter[countNum] == 0)
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
            else if (Mathf.Abs(bitter_result) > 100)
            {
                Debug.Log("苦味: death..");
                bitter_level = 1;
                bitter_score = -80;
            }
        }
        Debug.Log("苦味点: " + bitter_score);

        if (_girlsour[countNum] == 0)
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
            else if (Mathf.Abs(sour_result) > 100)
            {
                Debug.Log("酸味: death..");
                sour_level = 1;
                sour_score = -80;
            }
        }
        Debug.Log("酸味点: " + sour_score);


        //食感パラメータは、大きければ大きいほど、そのまま得点に。
        //ただし、女の子の好み値を超えてないと加点されない。
        //サブジャンルごとに、比較の対象が限定される。例えば、クッキーなら、さくさく度だけを見る。
        //またジャンルごとに、どのスコアの比重が大きくなるか、補正がかかる。アイスなら甘味が大事、とか。
        switch (_baseitemtype_sub)
        {
            case "Cookie":

                if (_basecrispy >= _girlcrispy[countNum])
                {
                    crispy_score = _basecrispy;
                    //crispy_score = _basecrispy - _girlcrispy[countNum]; //お菓子のサクサク度-好み値が点数に。
                }
                else
                {
                    crispy_score = 0;
                }
                shokukan_score = crispy_score;
                shokukan_mes = "さくさく感";
                Debug.Log("サクサク度の点: " + crispy_score);

                break;

            case "Rusk":

                if (_basecrispy >= _girlcrispy[countNum])
                {
                    crispy_score = _basecrispy;
                    //crispy_score = _basecrispy - _girlcrispy[countNum];
                }
                else
                {
                    crispy_score = 0;
                }
                shokukan_score = crispy_score;
                shokukan_mes = "さくさく感";
                Debug.Log("サクサク度の点: " + crispy_score);

                break;

            case "Cake":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum]; 
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Crepe":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Creampuff":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Donuts":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "PanCake":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Financier":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Maffin":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Bread":

                if (_basefluffy >= _girlfluffy[countNum])
                {
                    fluffy_score = _basefluffy;
                    //fluffy_score = _basefluffy - _girlfluffy[countNum];
                }
                else
                {
                    fluffy_score = 0;
                }
                shokukan_score = fluffy_score;
                shokukan_mes = "ふわふわ感";
                Debug.Log("ふわふわ度の点: " + fluffy_score);

                break;

            case "Chocolate":

                if (_basesmooth >= _girlsmooth[countNum])
                {
                    smooth_score = _basesmooth;
                    //smooth_score = _basesmooth - _girlsmooth[countNum];
                }
                else
                {
                    smooth_score = 0;
                }
                shokukan_score = smooth_score;
                shokukan_mes = "くちどけ感";
                Debug.Log("くちどけの点: " + smooth_score);

                break;

            case "Chocolate_Mat":

                if (_basesmooth >= _girlsmooth[countNum])
                {
                    smooth_score = _basesmooth;
                    //smooth_score = _basesmooth - _girlsmooth[countNum];
                }
                else
                {
                    smooth_score = 0;
                }
                shokukan_score = smooth_score;
                shokukan_mes = "くちどけ感";
                Debug.Log("くちどけの点: " + smooth_score);

                break;          

            case "Biscotti":

                if (_basehardness >= _girlhardness[countNum])
                {
                    hardness_score = _basehardness;
                    //hardness_score = _basehardness - _girlhardness[countNum];
                }
                else
                {
                    hardness_score = 0;
                }
                shokukan_score = hardness_score;
                shokukan_mes = "歯ごたえ";
                Debug.Log("歯ごたえの点: " + crispy_score);

                break;
           
            case "IceCream":

                if (_basesmooth >= _girlsmooth[countNum])
                {
                    smooth_score = _basesmooth;
                    //smooth_score = _basesmooth - _girlsmooth[countNum];
                }
                else
                {
                    smooth_score = 0;
                }
                shokukan_score = smooth_score;
                shokukan_mes = "くちどけ感";
                Debug.Log("くちどけの点: " + smooth_score);

                sweat_score *= 2;

                break;

            default:

                if (_basecrispy >= _girlcrispy[countNum])
                {
                    crispy_score = _basecrispy;
                    //crispy_score = _basecrispy - _girlcrispy[countNum];
                }
                else
                {
                    crispy_score = 0;
                }
                shokukan_score = crispy_score;
                Debug.Log("サクサク度の点: " + crispy_score);
                break;
        }

        //トッピングの値も計算する。①基本的に上がるやつ　+　②クエスト固有でさらに上がるやつ　の2点

        //①  スロットがついていれば、絶対に加算される。スロットネームDBのtotal_scoreを加算する。
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

        //②　ガールライクセットDBに設定があるトッピングのみ、さらに追加で各tp_scoreを加算する。（スロットが一致しているものだけに限る）
        switch (countNum)
        {

            case 0:

                for (i = 0; i < itemslotScore.Count; i++)
                {

                    //0はNonなので、無視　かつ、女の子のスコアが1以上
                    if (i != 0 && girl1_status.girl1_hungryScoreSet1[i] > 0)
                    {
                        topping_all_non = false;

                        //女の子のスコア(所持数)より、生成したアイテムのスロットの所持数が大きい場合は、そのトッピングが好みとマッチしている。正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet1[i])
                        {
                            topping_score += girl1_status.girl1_hungryToppingScoreSet1[i];

                            //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。今のとこ、一番後ろのTPに反応する。
                            topping_flag_point = girl1_status.girl1_hungryToppingNumberSet1[i];
                            topping_flag = true; //好みが一致するトッピングが、一つでもあった。
                        }
                        else
                        {

                        }
                    }
                }

                break;

            case 1:

                for (i = 0; i < itemslotScore.Count; i++)
                {
                    //①トッピングスロットの計算

                    //0はNonなので、無視
                    if (i != 0 && girl1_status.girl1_hungryScoreSet2[i] > 0)
                    {
                        topping_all_non = false;

                        //女の子のスコア(所持数)より、生成したアイテムのスロットの所持数が大きい場合は、そのトッピングが好みとマッチしている。正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet2[i])
                        {
                            topping_score += girl1_status.girl1_hungryToppingScoreSet2[i];

                            //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。
                            topping_flag_point = girl1_status.girl1_hungryToppingNumberSet2[i];
                            topping_flag = true; //好みが一致するトッピングが、一つでもあった。
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
                        topping_all_non = false;

                        //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet3[i])
                        {
                            topping_score += girl1_status.girl1_hungryToppingScoreSet3[i];

                            //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。
                            topping_flag_point = girl1_status.girl1_hungryToppingNumberSet3[i];
                            topping_flag = true; //好みが一致するトッピングが、一つでもあった。
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

        //女の子の食べたいトッピングがあるにも関わらず、そのトッピングがのっていなかった。
        if (!topping_all_non && !topping_flag)
        {
            topping_score += girl1_status.girl1_NonToppingScoreSet[countNum]; //点数がマイナスに働く。
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
            Mazui_flag = true;
        }
        else
        {
        }

        //得点に応じて、好感度・お金に補正がかかる。→ LoveScoreCal()で計算

        switch (_setType)
        {
            case 0:

                //作ったお菓子の点数が、前回より高い場合は、最高得点を更新。
                if (total_score > database.items[_baseID].last_total_score)
                {
                    //最高得点の他、その時の食感・甘さ系のパラメータ・スロット名も含めたアイテム名を更新
                    database.items[_baseID].last_total_score = total_score;

                    database.items[_baseID].last_rich_score = _baserich;
                    database.items[_baseID].last_sweat_score = _basesweat;
                    database.items[_baseID].last_bitter_score = _basebitter;
                    database.items[_baseID].last_sour_score = _basesour;
                    database.items[_baseID].last_crispy_score = _basecrispy;
                    database.items[_baseID].last_fluffy_score = _basefluffy;
                    database.items[_baseID].last_smooth_score = _basesmooth;
                    database.items[_baseID].last_hardness_score = _basehardness;
                    database.items[_baseID].last_jiggly_score = _basejiggly;
                    database.items[_baseID].last_chewy_score = _basechewy;

                    last_score_kousin = true;

                    //85点以上で、さらに高得点を一度もとったことがなければ、えめらるどんぐり一個もらえる
                    if (total_score >= GameMgr.high_score && !database.items[_baseID].HighScore_flag)
                    {
                            emerarudonguri_get = true;                        
                    }
                }
                
                break;

            case 1:

                break;
        }

    }

    public int Judge_Score_Return(int value1, int value2, int SetType, int _Setcount)
    {
        SetGirlTasteInit();

        //コンテスト用に、渡すアイテムのパラメータ設定
        Girleat_Judge_method(value1, value2, SetType);

        judge_score(SetType, _Setcount); //SetTypeは、女の子かコンテスト用かの判定。_Setcountは、GirlLikeCompoの1,2,3番目のどれを判定に使うかの数値

        return total_score;
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

                    //3秒ほど表示したら、お菓子の感想を言ったり、なんか褒めてくれたりする。
                    StartCoroutine("WaitCommentDesc");
                    break;
            }
        
            
            //お菓子をたべたフラグをON + 食べた回数もカウント
            database.items[_baseID].Eat_kaisu += 1;

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

                    OkashiSaitenhyouji(); //採点パネル表示してからリザルト
                    //Okashi_Result();

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

                    hukidashiitem.GetComponent<TextController>().SetText("げろげろ..。ま、まずいよ..。兄ちゃん。");

                    //まずいときは、スコアは0点。
                    total_score = 0;

                    //キャラクタ表情変更
                    //s.sprite = girl1_status.Girl1_img_verysad;

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

                    hukidashiitem.GetComponent<TextController>().SetText("ぐええ..。コレ嫌いー！..。");

                    //まずいときは、スコアは0点。
                    total_score = 0;

                    //キャラクタ表情変更
                    //s.sprite = girl1_status.Girl1_img_verysad_close;

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
                    //s.sprite = girl1_status.Girl1_img_verysad_close;

                    //好感度は変わらず、お菓子も減りはしない。

                    //音を鳴らす
                    audioSource.PlayOneShot(sound2);

                    //テキストウィンドウの更新
                    exp_Controller.GirlNotEatText();

                    //お菓子をあげたあとの状態に移行する。残り時間を、短く設定。
                    girl1_status.timeGirl_hungry_status = 2;
                    girl1_status.timeOut = 5.0f;

                    girl1_status.GirlEat_Judge_on = true; //またカウントが進み始める

                    //お菓子判定終了
                    compound_Main.girlEat_ON = false;

                    break;

                default:

                    break;
            }

            //リセット＋フラグチェック
            //減る場合は、update内でちぇっく
        }
        
        compound_Main.compound_status = 0;        

        //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
        /*if (GameMgr.tutorial_ON)
        {

            StartCoroutine("WaitForSeconds");  //1秒まって次へ              

        }*/
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1.0f);

        /*
        if (GameMgr.tutorial_Num == 105)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 110;
        }
        if (GameMgr.tutorial_Num == 285)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 290;
        }*/
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



    //お菓子の採点結果を表示する。　シャキーーン！！　満足度　ドンドン　わーーーぱちぱちって感じ
    void OkashiSaitenhyouji()
    {       
        ScoreHyoujiPanel.SetActive(true);
        ScoreHyouji_ON = true;       
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;


        //お菓子の点数を表示
        if (total_score < GameMgr.low_score) //
        {
            Okashi_Score.color = new Color(129f / 255f, 87f / 255f, 60f / 255f); //茶色　青文字(105f / 255f, 168f / 255f, 255f / 255f)      
        } else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
        {
            Okashi_Score.color = new Color(255f / 255f, 105f / 255f, 170f / 255f); //ピンク
        } else
        {
            Okashi_Score.color = new Color(255f / 255f, 105f / 255f, 170f / 255f); //ピンク　黄色(255f / 255f, 252f / 255f, 158f / 255f)
        }
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
        else if (total_score >= 30 && total_score < GameMgr.low_score)
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
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
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
        else if (total_score >= GameMgr.high_score && total_score < 95)
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
            if (!database.items[_baseID].HighScore_flag)
            {
                database.items[_baseID].HighScore_flag = true;
            }
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
            if (!database.items[_baseID].HighScore_flag)
            {
                database.items[_baseID].HighScore_flag = true;
            }
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
            //sc.PlaySe(41); //ファンファーレ
            sc.PlaySe(42); //キラリ～ン 42か47
            sc.PlaySe(43); //拍手
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
              

        //キャラクタ表情変更
        girl1_status.face_girl_Yorokobi();
        StartCoroutine("DefaultFaceChange");//2秒ぐらいしたら、表情だけすぐに戻す。

        //そのクエストでの最高得点を保持。（マズイときは、失敗フラグ＝0点）
        if (special_quest.special_score_record[special_quest.spquest_set_num, 0] <= total_score)
        {
            special_quest.special_score_record[special_quest.spquest_set_num, 0] = total_score;
        }

        if (!GameMgr.tutorial_ON)
        {
            //フラグチェック        
            compound_Main.check_GirlLoveEvent_flag = false;

            if (!non_spquest_flag) //メインのSPお菓子クエストの場合。クエスト以外のお菓子を揚げた場合、クエストクリアボタンなどの処理を除外する。
            {
                //クエスト挑戦回数を増やす。
                special_quest.special_kaisu++;

                //60点以上だったら、そのクエストをクリアできる、スキップボタンが表示
                if (total_score >= GameMgr.low_score)
                {
                    GameMgr.QuestClearflag = true;

                    stageclear_Button.SetActive(true);

                    _windowtext.text = "満足しているようだ。";
                }
                else
                {
                    _windowtext.text = "";
                }

                //もしクエスト回数が３回まできたら、判定する。通常クリアなら、次のクエストへ進行する
                //if (special_quest.special_kaisu >= special_quest.special_kaisu_max)
                //{
                //クエストクリア時のイベントやフラグ管理
                //SelectNewOkashiSet();
                //}
            }
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

        //前に計算したトータルスコアを元に計算。

        switch (dislike_status)
        {
            
            default: //通常

                if (Mazui_flag)
                {
                    Getlove_exp = -10;
                    Debug.Log("取得好感度: " + Getlove_exp);

                }
                else
                {
                    if (last_score_kousin) //前回の最高得点より高い点数の場合のみ、好感度があがる。
                    {
                        //①好感度取得
                        if (total_score < 40) //60点以下のときは、好感度ほぼあがらず。
                        {
                            Getlove_exp = 0;
                        }
                        else if (total_score >= 40 && total_score < GameMgr.low_score) //60点以下のときは、好感度ほぼあがらず。
                        {
                            Getlove_exp = (int)(_basegirl1_like * 0.5f);
                        }
                        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score) //ベース×5
                        {
                            Getlove_exp = (int)(_basegirl1_like * 5.0f);
                        }
                        else if (total_score >= GameMgr.high_score && total_score < 100) //ベース×15
                        {
                            Getlove_exp = (int)(_basegirl1_like * 15.0f);
                        }
                        else if (total_score >= 100) //100点を超えた場合、ベース×25
                        {
                            Getlove_exp = (int)(_basegirl1_like * 25.0f);
                        }

                        //②トッピングの値で、好感度を加算する。ただし、60点以上でないと、加算されない。固有スロットもみている。
                        if (total_score >= GameMgr.low_score)
                        {
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
                        }

                        //③そのお菓子を食べた回数で割り算。同じお菓子を何度あげても、だんだん好感度は上がらなくなってくる。
                        /*if (database.items[_baseID].Eat_kaisu == 0)
                        {
                            database.items[_baseID].Eat_kaisu = 1; //0で割り算を回避。
                        }
                        Getlove_exp /= database.items[_baseID].Eat_kaisu;
                        if (Getlove_exp <= 1) { Getlove_exp = 1; }*/


                        Debug.Log("取得好感度: " + Getlove_exp);

                        //お金の取得
                        /*
                        if (total_score >= 0 && total_score < 15)
                        {
                            GetMoney = (int)(_basecost * 0.5f) + (int)(total_score * 0.3f) + slot_money;
                        }
                        else if (total_score >= 15 && total_score < 45)
                        {
                            GetMoney = (int)(_basecost * 1.0f) + (int)(total_score * 0.3f) + slot_money;
                        }
                        else if (total_score >= 45 && total_score < GameMgr.low_score)
                        {
                            GetMoney = (int)(_basecost * 1.5f) + (int)(total_score * 0.3f) + slot_money;
                        }
                        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
                        {
                            GetMoney = (int)(_basecost * 2.0f) + (int)(total_score * 0.3f) + slot_money;
                        }
                        else if (total_score >= GameMgr.high_score && total_score < 100)
                        {
                            GetMoney = (int)(_basecost * 5.0f) + (int)(total_score * 0.3f) + slot_money;
                        }
                        else if (total_score >= 100) //100点を超えた場合、2倍程度増加
                        {
                            GetMoney = (_basecost + slot_money) + (int)(total_score * 0.6f) * 10;
                        }*/
                        //Debug.Log("取得お金: " + GetMoney);
                    }
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
            girl1_status.LvUpStatus();
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

        //クエストクリアかどうかを判定  回数制限はとりあえず無し。

        //判定
        HighScore_flag = false;
        Gameover_flag = false;

        if (total_score >= GameMgr.high_score) //85点以上で、ハイスコア判定
        {
            HighScore_flag = true;
        } 
        /*
        for (i=0; i < special_quest.special_score_record.GetLength(1); i++)
        {
            if(special_quest.special_score_record[special_quest.spquest_set_num, i] < 85 && 
                special_quest.special_score_record[special_quest.spquest_set_num, i] >= 60 ) //60点~85点でノーマル合格
            {
                Gameover_flag = false;
            }
            else if (special_quest.special_score_record[special_quest.spquest_set_num, i] >= 85) //85点以上でハイスコア合格
            {
                HighScore_flag = true;
                Gameover_flag = false;
            }
            else //60未満だとゲームオーバーフラグ
            {
                Debug.Log("ゲームオーバーフラグ ON");                
                //ゲーム終了　ぐええ
            }
        }*/

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
    //クエストクリアトグルをおして、スキップした場合に処理されるメソッド。
    //
    public void QuestClearMethod()
    {
        SelectNewOkashiSet();
        ResultPanel_On();
    }



    //
    //スコア表示パネルを押したらこのメソッドが呼び出し
    //
    public void ResultPanel_On()
    {
        ScoreHyoujiPanel.SetActive(false);
        sc.PlaySe(2);

        //それじゃあ、兄ちゃん。クッキーの採点をするね！といった画面と演出

        if (subQuestClear_check)
        {
            if (!Gameover_flag)
            {
                //クリアフラッグをたてる。
                switch (girl1_status.OkashiQuest_ID)
                {
                    case 1000: //オリジナルクッキー

                        GameMgr.OkashiQuest_flag_stage1[0] = true;

                        break;

                    case 1100: //ラスク

                        GameMgr.OkashiQuest_flag_stage1[1] = true;

                        break;

                    case 1200: //クレープ

                        GameMgr.OkashiQuest_flag_stage1[2] = true;

                        break;

                    case 1300: //シュークリーム

                        GameMgr.OkashiQuest_flag_stage1[3] = true;

                        break;

                    case 1400: //ドーナツ

                        GameMgr.OkashiQuest_flag_stage1[4] = true;

                        break;

                    default:
                        break;
                }

                ClearFlagOn();
                MainQuestText.text = _mainquest_name;

                if (!HighScore_flag) //通常クリア
                {
                    _set_MainQuestID = girl1_status.OkashiQuest_ID;
                }
                else
                {
                    _set_MainQuestID = girl1_status.OkashiQuest_ID + 1;
                }

                StartCoroutine("SubQuestClearEvent");
            }
            else
            {
                //ゲームオーバー画面を表示。現在、ゲームオーバーを使用していない。
                Debug.Log("ゲームオーバー画面表示");
                ResultOFF();

                FadeManager.Instance.LoadScene("999_Gameover", 0.3f);
            }
        }
        else
        {
            if (!GameMgr.tutorial_ON)
            {
                if (!Mazui_flag) //まずいがなければ、通常の感想
                {
                    //クエストまだクリアでなければ、お菓子の感想を表示する。
                    StartCoroutine("OkashiAfter_Comment");                   
                }
                else
                {
                    ResultOFF();
                }
            }
            else
            {
                GameMgr.tutorial_Progress = true; //チュートリアル時、パネルを押したよ～のフラグ
                ResultOFF();
            }
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

                if (!HighScore_flag) //通常クリア
                {
                    _mainquest_name = girlLikeCompo_database.girllike_composet[i].spquest_name1;
                }
                else //さらに高得点だったら、特別なイベントや報酬などが発生
                {
                    _mainquest_name = girlLikeCompo_database.girllike_composet[i].spquest_name2;
                }
            }
        }
    }

    public void ResultOFF()
    {
        MainQuestOKPanel.SetActive(false);

        subQuestClear_check = false;

        //お菓子をあげたあとの状態に移行する。
        girl1_status.timeGirl_hungry_status = 2;
        girl1_status.timeOut = 5.0f;
        girl1_status.GirlEat_Judge_on = true;//またカウントが進み始める
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

    //
    //通常お菓子食べた直後の、おいしい・まずいといった感想。トッピングに対する感想もここで。
    //
    IEnumerator Girl_Comment()
    {

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();

        //カメラが元の位置にもどってから、キャラ表示を切り替え
        while (main_cam.transform.position.z != -10)
        {
            yield return null;
        }

        if (Mazui_flag) //味がまずかった場合（total_scoreがマイナスだったとき）
        {
            GameMgr.OkashiComment_ID = 9999; //
        }
        else
        {
            if (GameMgr.GirlLoveEvent_num == 5 && contest_type == 0) //コンテストのときに「あげる」をおすと、こちらの処理
            {
                GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID;
            }
            else
            { 
                //SPのお菓子でないものをあげた場合のコメント
                if(non_spquest_flag)
                {
                    GameMgr.OkashiComment_ID = 999;
                }
                //宴を呼び出す。GirlLikeSetのフラグで、スロットに関する感想か、total_scoreに関する感想のどちらかを表示する。
                if (_girl_comment_flag[set_id] == 0)
                {
                    NormalCommentEatBunki();
                }
                else if (_girl_comment_flag[set_id] == 1)
                {
                    if (!topping_flag) //トッピングに一致するものがなかったとき。
                    {
                        NormalCommentEatBunki();
                    }
                    else
                    {
                        GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + topping_flag_point; //クエストID+topping_flag(1~5)で指定する。ねこクッキーで、アイザン入りなら、1000+1で、1001。
                    }
                }
            }
        }
        GameMgr.OkashiComment_flag = true;

        while (!GameMgr.scenario_read_endflag)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = false;

        canvas.SetActive(true);

        Girl_reaction();
    }   

    //
    //採点表示後にお菓子の感想を表示する
    //
    IEnumerator OkashiAfter_Comment()
    {
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();

        if (GameMgr.GirlLoveEvent_num == 5 && contest_type == 0) //コンテストのときに「あげる」をおすと、こちらの処理
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID;

            //お菓子ごとにさらに感想だしてもよいかも。
        }
        else
        {
            if (!non_spquest_flag) //メインクエストの場合の感想
            {
                //宴を呼び出す。GirlLikeSetのフラグで、トータルスコアに関する感想、スロットに反応する場合スロットの感想、のどちらかを表示する。
                if (_girl_comment_flag[set_id] == 0)
                {
                    NormalCommentAfterBunki();
                }
                else if (_girl_comment_flag[set_id] == 1)
                {
                    if (!topping_flag) //トッピングに一致するものがなかったとき。
                    {
                        NormalCommentAfterBunki();
                    }
                    else
                    {
                        GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + topping_flag_point; //スロットの感想 1000番台~
                    }
                }
            }
            else //クエスト以外のお菓子をあげた場合、お菓子ごとの感想などを表示する？
            {
                GameMgr.okashiafter_ID = 1500;
            }
        }
        GameMgr.okashiafter_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                       //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        //満足度にあわせて音を鳴らす。
        if (total_score < GameMgr.low_score)　//60点以下
        {
            //sc.PlaySe(60);
        }
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score) //60点以上～85点以下
        {
            sc.PlaySe(60);

            //エフェクト生成＋アニメ開始
            _listEffect.Add(Instantiate(Emo_effect_manzoku, character.transform));
        }
        else //85点以上
        {
            sc.PlaySe(60);

            //エフェクト生成＋アニメ開始
            _listEffect.Add(Instantiate(Emo_effect_daimanzoku, character.transform));

            if (emerarudonguri_get)
            {
                emerarudonguri_get = false;
                StartCoroutine("EmeralDonguriEvent");
            }
        }

        GameMgr.recipi_read_endflag = false;

        canvas.SetActive(true);

        ResultOFF();
    }

    void NormalCommentEatBunki()
    {
        if (total_score < GameMgr.low_score)
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + 50; //スロットの感想 1050番台~
        }
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + 51; //スロットの感想 1050番台~
        }
        else
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + 52; //スロットの感想 1050番台~
        }
    }


    void NormalCommentAfterBunki()
    {
        if (total_score < GameMgr.low_score)
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + 50; //スロットの感想 1050番台~
        }
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + 51; //スロットの感想 1050番台~
        }
        else
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + 52; //スロットの感想 1050番台~
        }
    }


    //
    //85点以上だった場合、妹がエメラルどんぐりをくれる。
    //
    IEnumerator EmeralDonguriEvent()
    {
        yield return new WaitForSeconds(1.0f);

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();

        GameMgr.scenario_ON = true;
        GameMgr.emeralDonguri_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        //エメラルどんぐり一個もらえる。
        pitemlist.addPlayerItemString("emeralDongri", 1);
        //PlayerStatus.player_kaeru_coin++;
        //kaerucoin_Controller.ReDrawParam();

        GameMgr.scenario_ON = false;
        GameMgr.recipi_read_endflag = false;

        canvas.SetActive(true);

    }

    //
    //クエストクリア時の感想を表示する。
    //
    IEnumerator SubQuestClearEvent()
    {
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();
        sceneBGM.MuteBGM();

        GameMgr.KeyInputOff_flag = false;
        GameMgr.scenario_ON = true;
        GameMgr.mainquest_ID = _set_MainQuestID; //GirlLikeCompoSetの_set_compIDが入っている。
        GameMgr.mainClear_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        sceneBGM.MuteOFFBGM();

        canvas.SetActive(true);
        stageclear_Button.SetActive(false);

        //表示の音を鳴らす。
        sc.PlaySe(47);　//前は、25

        MainQuestOKPanel.SetActive(true);
    }    

    void SetHintText(int _status)
    {
        
        //ヒントを表示する。０のものは、判定なしなので、表示もしない。

        SweatHintHyouji();

        if (bitter_level != 0)
        {
            BitterHintHyouji();
        }
        if (sour_level != 0)
        {
            SourHintHyouji();
        }

        ShokukanHintHyouji();


        //トータルスコアが低いときは、そのクスエトをクリアに必要な固有のヒントをくれる。（クッキーのときは、「もっとかわいくして！」とか。妹が好みのものを伝えてくる。）
        if (!non_spquest_flag)
        {
            if (total_score < GameMgr.low_score)
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
            else if (total_score >= GameMgr.low_score)
            {
                _special_kansou = "\n" + "";
            }
        }
        else //クエスト以外のお菓子をあげたときの感想・ヒント
        {
            if (dislike_status == 2)
            {
                _special_kansou = "\n" + "今まで食べたことないお菓子だ！";
            }
            else
            {
                _special_kansou = "\n" + "";
            }
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

                temp_hint_text = "マズすぎるぅ..。" + "\n" + _sweat_kansou + _bitter_kansou + _sour_kansou;
                break;

            default:
                break;
        }

        database.items[_baseID].last_hinttext = temp_hint_text;

        if (Getlove_exp > 0)
        {
            _result_text = "好感度が " + Getlove_exp + " アップ！　"; //"お金を " + GetMoney + "G ゲットした！"
        }
        else if (Getlove_exp == 0)
        {
            _result_text = "好感度は変わらなかった。";
        }
        else
        {
            _result_text = "好感度が " + Mathf.Abs(Getlove_exp) + " 下がった..。　"; //"お金を " + GetMoney + "G ゲットした！"
        }

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
            _shokukan_kansou = "\n" + shokukan_mes + "がもっとほしい";
        }
        else if (shokukan_score >= 40 && shokukan_score < GameMgr.low_score) //
        {
            _shokukan_kansou = "\n" + shokukan_mes + "がほしい";
        }
        else if (shokukan_score >= GameMgr.low_score && shokukan_score < GameMgr.high_score) //
        {
            _shokukan_kansou = "";
        }
        else if (shokukan_score >= GameMgr.high_score) //
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

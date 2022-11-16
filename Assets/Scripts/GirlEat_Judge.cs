//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using DG.Tweening;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class GirlEat_Judge : SingletonMonoBehaviour<GirlEat_Judge> {

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
    private GameObject hinttaste_toggle;

    private GameObject MainUIPanel_obj;

    private GameObject Girlloveexp_bar;
    private Buf_Power_Keisan bufpower_keisan;

    private Debug_Panel debug_panel;
    private Text debug_taste_resultText;

    private GameObject BlackPanel_event;
    private GameObject ScoreHyoujiPanel;
    private GameObject MainQuestOKPanel;
    private GameObject ExtraQuestOKPanel;
    private GameObject ExtraQuest_TreasurePanel;
    private GameObject QuestClearEffectPanel;
    private Text MainQuestText;
    private string _mainquest_name;
    private int _set_MainQuestID;
    private Text Hint_Text;
    private string temp_hint_text;
    private Text Result_Text;
    private string _result_text;
    public string _sweat_kansou, _bitter_kansou, _sour_kansou; //UtageScenarioからも読み出し
    public string _contest_sweat_kansou, _contest_bitter_kansou, _contest_sour_kansou; //Contestからも読み出し
    public string _shopgirl_sweat_kansou, _shopgirl_bitter_kansou, _shopgirl_sour_kansou, _shopgirl_shokukan_kansou; //UtageScenarioからも読み出し
    public string _shokukan_kansou;
    private string _temp_spkansou, _special_kansou;
    private bool Mazui_flag;
    private bool non_spquest_flag;
    private bool sp_quest_clear;
    private int _tempgetlove;

    private Text Okashi_Score;
    private Text Manzoku_Score;
    private Text Delicious_Text;

    private Exp_Controller exp_Controller;
    private Touch_Controller touch_controller;

    private PlayerItemList pitemlist;

    private Special_Quest special_quest;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemMatPlaceDataBase matplace_database;

    public bool subQuestClear_check;
    private bool HighScore_flag;
    private bool kansou_on; //採点表示の際、事前に「うんめー」などのお菓子の感想を表示するか否か。specialクエストの場合は、表示する。

    //女の子の反映用ハートエフェクト
    private Particle_Heart_Character GirlHeartEffect;
    private int girllove_param;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private ExpTable exp_table;

    private string _commentrandom;
    private List<string> _commentDict = new List<string>();

    private Girl1_status girl1_status;
    private GameObject character;
    private GameObject character_root;

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
    private Vector3 _temppos;
    private Slider _slider; //好感度バーを取得
    private Tween coinTween;
    private int currentDispCoin;
    private int preDispCoin;
    private float countTime;
    private int slot_girlscore, slot_money;
    public int Getlove_exp;
    private int GetMoney;
    private Text girl_lv;
    private Text girl_param;
    private Color origin_color;

    private int _tempGirllove;
    private int _tempResultGirllove;
    private float _lovecounter;
    public int star_Count;
    private int emeraldonguri_status;

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    //判定中の女の子のアニメ用
    private bool judge_anim_on;
    private bool judge_end;
    private int judge_anim_status;
    private bool heart_animON; //ハートが上がったり下がったりするアニメのフラグ  ロードすると強制解除。
    private bool loveplusanimON; //実際のハートが発生して、衝突したときにハートが上昇する時のフラグ

    private SpriteRenderer s;

    private GameObject EatAnimPanel;
    private Image EatAnimPanel_itemImage;
    private Sprite texture2d;
    private GameObject EatStartEffect;

    //時間
    private float timeOut;
    private float timeOutHeart;

    //アイテムのパラメータ
    private int _baseID;
    private string _basename;
    private string _basenameHyouji;
    private string _basenameFull;
    private string _basenameSlot;
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
    private int _basejuice;
    private int _basepowdery;
    private int _baseoily;
    private int _basewatery;
    private int _basebeauty;
    private int _basescore;
    private float _basegirl1_like;
    private int _baseSetjudge_num;
    private string[] _basetp;
    private string[] _koyutp;

    private int _basegirl1_manpuku;

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
    private int[] _girljuice;
    private int[] _girlbeauty;

    private int _girlpowdery;
    private int _girloily;
    private int _girlwatery;

    private int _set_compID;

    private string[] _girl_subtype;
    private string[] _girl_likeokashi;

    private int[] _girl_set_score;
    private int[] _girl_comment_flag;
    private int[] _girl_judgenum;


    //女の子の採点用パラメータ
    //private int taste_result;

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
    private int taste_level;
    private int taste_score;
    private string taste_type;

    private int sweat_level;
    private int bitter_level;
    private int sour_level;

    public int set_score;
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
    public int juice_score;
    public int shokukan_score;
    public int beauty_score;

    public int subtype1_score;
    public int subtype2_score;

    private int _temp_kyori;
    private float _temp_deg;
    private float _temp_ratio;
    public int topping_score;
    public int topping_flag_point;
    public bool topping_flag;
    public bool topping_all_non;
    private bool tpcheck;
    private bool no_hint;
    private string tpcheck_slot;
    private bool tpcheck_utageON;
    private int tpcheck_utagebunki;

    public int total_score;

    private bool dislike_flag;
    private int dislike_status;
    private int dislike_num; //あぶらっこいか、水っぽいか、粉っぽいを判別
    private int dislike_score;

    public int girllike_point;

    private bool emerarudonguri_get;
    private bool last_score_kousin;
    private string shokukan_mes;
    private int shokukan_baseparam;

    private bool emerarudonguri_end;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの所持数
    List<int> itemslotScore = new List<int>();

    private GameObject effect_Prefab;
    private GameObject questclear_effect_Prefab;
    private GameObject Emo_effect_manzoku;
    private GameObject Emo_effect_daimanzoku;
    private List<GameObject> _listEffect = new List<GameObject>();
    private List<GameObject> _questClearEffect = new List<GameObject>();


    private GameObject heart_Prefab;
    private GameObject heart_Prefab2;
    public List<GameObject> _listHeart = new List<GameObject>();
    public int heart_count; //画面上に存在するハートの個数

    private GameObject hearthit_Prefab;
    private List<GameObject> _listHeartHit = new List<GameObject>();

    private GameObject hearthit2_Prefab;
    private List<GameObject> _listHeartHit2 = new List<GameObject>();

    private GameObject Magic_effect_Prefab1;
    private List<GameObject> _listHeartAtkeffect = new List<GameObject>();


    private Vector3 heartPos;

    private int rnd, rnd2;
    private int set_id;
    private int _slotid;

    //エフェクト
    private GameObject Score_effect_Prefab1;
    private GameObject Score_effect_Prefab2;
    private List<GameObject> _listScoreEffect = new List<GameObject>();

    private PlayableDirector playableDirector;

    //好感度レベルテーブルの取得
    private List<int> stage_levelTable = new List<int>();
    //レベルアップ用
    private GameObject zeropoint;
    private GameObject HeartLvUpPanel_obj;
    private GameObject lvuppanel_Prefab;
    private List<GameObject> _listlvup_obj = new List<GameObject>();


    //Live2Dモデルの取得
    private CubismModel _model;
    private Animator live2d_animator;
    private CubismRenderController _renderController;
    private int trans_motion;
    private int trans_expression;

    private GameObject stageclear_panel;
    private GameObject stageclear_toggle;
    private GameObject stageclear_Button;

    private int contest_type;

    private GameObject Fadeout_Black_obj;

    private int _temp_count;
    private float _buf_moneyup;
    private float _up_deg;
    private bool hlv_check;

    // Use this for initialization
    void Start() {

        SceneInitSetting();

    }

    void SceneInitSetting()
    {
        Debug.Log("GirlEatJudge初期化");

        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

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

        //レベルアップチェック用オブジェクトの取得
        exp_table = ExpTable.Instance.GetComponent<ExpTable>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //カメラの取得
                main_cam = Camera.main;
                maincam_animator = main_cam.GetComponent<Animator>();
                trans = maincam_animator.GetInteger("trans");

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                //BGMの取得
                sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

                //エクストリームパネルの取得
                Extremepanel_obj = canvas.transform.Find("MainUIPanel/ExtremePanel").gameObject;
                extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();

                hinttaste_toggle = canvas.transform.Find("MainUIPanel/Comp/HintTaste_Toggle").gameObject;

                MainUIPanel_obj = canvas.transform.Find("MainUIPanel").gameObject;

                BlackPanel_event = canvas.transform.Find("Black_Panel_Event").gameObject;

                //タッチ判定オブジェクトの取得
                touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

                //Live2Dモデルの取得
                _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                live2d_animator = _model.GetComponent<Animator>();
                _renderController = _model.GetComponent<CubismRenderController>();

                //キャラクタ取得
                character = GameObject.FindWithTag("Character");
                character_root = GameObject.FindWithTag("CharacterRoot");

                //お金の増減用パネルの取得
                MoneyStatus_Panel_obj = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").gameObject;
                moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

                //女の子の反映用ハートエフェクト取得
                GirlHeartEffect = character_root.transform.Find("CharacterMove/Particle_Heart_Character").GetComponent<Particle_Heart_Character>();

                //女の子のレベル表示取得
                girl_lv = canvas.transform.Find("MainUIPanel/Girl_love_exp_bar").transform.Find("LV_param").GetComponent<Text>();
                girl_param = canvas.transform.Find("MainUIPanel/Girl_love_exp_bar").transform.Find("Girllove_param").GetComponent<Text>();
                //origin_color = girl_param.color;
                origin_color = new Color(255f / 255f, 255f / 255f, 255f / 255f);

                //クエストクリアボタンの取得
                stageclear_panel = canvas.transform.Find("MainUIPanel/StageClearButton_Panel").gameObject;
                stageclear_toggle = canvas.transform.Find("MainUIPanel/Comp/CompoundSelect_ScrollView").transform.Find("Viewport/Content_compound/StageClear_Toggle").gameObject;
                stageclear_Button = canvas.transform.Find("MainUIPanel/StageClearButton_Panel/StageClear_Button").gameObject;

                //エフェクトプレファブの取得
                effect_Prefab = (GameObject)Resources.Load("Prefabs/Particle_Heart");
                questclear_effect_Prefab = (GameObject)Resources.Load("Prefabs/ParticleQClearEffect");
                Emo_effect_manzoku = (GameObject)Resources.Load("Prefabs/Emo_HeartAnimL");
                Emo_effect_daimanzoku = (GameObject)Resources.Load("Prefabs/Emo_HeartAnimL");
                Score_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_ResultFeather");
                Score_effect_Prefab2 = (GameObject)Resources.Load("Prefabs/Particle_Compo5");
                playableDirector = stageclear_panel.GetComponent<PlayableDirector>();
                playableDirector.enabled = false;

                //ハートプレファブの取得
                heart_Prefab = (GameObject)Resources.Load("Prefabs/HeartUpObj");
                heart_Prefab2 = (GameObject)Resources.Load("Prefabs/HeartUpObj2");
                hearthit_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect");
                hearthit2_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect2");
                //エフェクトプレファブの取得
                Magic_effect_Prefab1 = (GameObject)Resources.Load("Prefabs/Particle_KiraExplode_Heart");

                //Prefab内の、コンテンツ要素を取得
                eat_hukidashiPrefab = (GameObject)Resources.Load("Prefabs/Eat_hukidashi");

                //フェードアウトブラックのオブジェクトを取得
                Fadeout_Black_obj = GameObject.FindWithTag("FadeOutBlack");

                //食べ始めアニメオブジェクトの取得
                EatAnimPanel = canvas.transform.Find("EatAnimPanel").gameObject;
                EatAnimPanel_itemImage = EatAnimPanel.transform.Find("ItemImage").GetComponent<Image>();
                EatAnimPanel.SetActive(false);

                EatStartEffect = GameObject.FindWithTag("EatAnim_Effect").transform.Find("Comp").gameObject;

                //好感度レベルアップ時の演出パネル取得
                HeartLvUpPanel_obj = canvas.transform.Find("HeartLvUpPanel").gameObject;
                foreach (Transform child in HeartLvUpPanel_obj.transform.Find("Viewport/Content").transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
                {
                    Destroy(child.gameObject);
                }
                lvuppanel_Prefab = (GameObject)Resources.Load("Prefabs/GirlLoveLevelUpPanel");
                _listlvup_obj.Clear();

                //お菓子採点結果表示用パネルの取得
                ScoreHyoujiPanel = canvas.transform.Find("ScoreHyoujiPanel/Result_Panel").gameObject;
                Okashi_Score = ScoreHyoujiPanel.transform.Find("Image/ScoreBackIcon/Okashi_Score").GetComponent<Text>();
                MainQuestOKPanel = canvas.transform.Find("ScoreHyoujiPanel/MainQuestOKPanel").gameObject;
                MainQuestText = MainQuestOKPanel.transform.Find("Panel1/QuestPanel/Image/QuestClearText").GetComponent<Text>();
                ExtraQuestOKPanel = canvas.transform.Find("ScoreHyoujiPanel/ExtraQuestOKPanel").gameObject;
                ExtraQuest_TreasurePanel = canvas.transform.Find("ScoreHyoujiPanel/ExtraQuestTreasurePanel").gameObject;
                Hint_Text = ScoreHyoujiPanel.transform.Find("Image/Hint_Text").GetComponent<Text>();
                Result_Text = ScoreHyoujiPanel.transform.Find("GetLovePanelBG/Result_GetLoveText/Result_Text").GetComponent<Text>();
                ScoreHyoujiPanel.SetActive(false);
                MainQuestOKPanel.SetActive(false);
                ExtraQuestOKPanel.SetActive(false);
                ExtraQuest_TreasurePanel.SetActive(false);

                QuestClearEffectPanel = canvas.transform.Find("QuestClearEffectPanel").gameObject;
                QuestClearEffectPanel.SetActive(false);

                Manzoku_Score = ScoreHyoujiPanel.transform.Find("Image/Manzoku_Score").GetComponent<Text>();
                Delicious_Text = ScoreHyoujiPanel.transform.Find("Image/DeliciousPanel/Text").GetComponent<Text>();
                ScoreHyoujiPanel.SetActive(false);

                //好感度ゲージの取得
                Girlloveexp_bar = MainUIPanel_obj.transform.Find("Girl_love_exp_bar").gameObject;

                //好感度バーの取得
                _slider_obj = MainUIPanel_obj.transform.Find("Girl_love_exp_bar").gameObject;
                _slider = _slider_obj.GetComponent<Slider>();

                zeropoint = canvas.transform.Find("ZeroPoint").gameObject;

                Getlove_exp = 0;
                GetMoney = 0;


                stage_levelTable.Clear();

                //GirlExpバーの最大値の設定。テーブルの初期設定はGirl1_statusで行っている。ここではそれをもとにコピーしてるだけ。
                for (i = 0; i < girl1_status.stage1_lvTable.Count; i++)
                {
                    stage_levelTable.Add(girl1_status.stage1_lvTable[i]);
                }

                //スライダ表示を更新

                //スライダのMAXを設定。現在の好感度レベルで変わる。
                Love_Slider_Setting();
                girllove_param = PlayerStatus.girl1_Love_exp;
                _slider.value = girllove_param;

                //レベルを表示
                girl_lv.text = PlayerStatus.girl1_Love_lv.ToString();
                girl_param.text = PlayerStatus.girl1_Love_exp.ToString();

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

        kettei_item1 = 0;
        _toggle_type1 = 0;

        dislike_flag = true;
        emerarudonguri_get = false;
        emerarudonguri_end = false;

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //アニメーション用時間
        timeOut = 5.0f;
        timeOutHeart = 0.05f;

        judge_anim_on = false;
        judge_end = false;
        judge_anim_status = 0;
        heart_animON = false;
        loveplusanimON = false;

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
        _girljuice = new int[girl1_status.youso_count];

        _girlbeauty = new int[girl1_status.youso_count];

        _girl_subtype = new string[girl1_status.youso_count];
        _girl_likeokashi = new string[girl1_status.youso_count];

        _girl_set_score = new int[girl1_status.youso_count];
        _girl_comment_flag = new int[girl1_status.youso_count];
        _girl_judgenum = new int[girl1_status.youso_count];

        //サブクエストチェック用フラグ
        subQuestClear_check = false;
        HighScore_flag = false;
        kansou_on = false;
        sp_quest_clear = false;

        //テキストのセッティング
        CommentTextInit();

        _basetp = new string[database.items[0].toppingtype.Length];
        _koyutp = new string[database.items[0].koyu_toppingtype.Length];
    }

    //ロード時に必ずリセットされる項目
    public void ResetLoadStatus()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                heart_animON = false;
                loveplusanimON = false;
                girl_param.color = origin_color;
                break;
        }
    }

    // Update is called once per frame
    void Update() {

        //シーン移動の際、破壊されてしまうオブジェクトは、毎回初期化
        if (canvas == null)
        {
            SceneInitSetting();
        }

        if (judge_anim_on == true)
        {
            switch (judge_anim_status)
            {
                case 0: //初期化 状態１

                    girl1_status.gireleat_start_flag = true;
                    girl1_status.GirlEat_Judge_on = false;

                    text_area.SetActive(false);
                    MainUIPanel_obj.SetActive(false);

                    timeOut = 4.0f;
                    judge_anim_status = 1;

                    //現在の吹き出しを削除
                    girl1_status.DeleteHukidashiOnly();

                    //食べ始めのアニメーションをスタート
                    EatAnimPanel.SetActive(true);
                    texture2d = database.items[database.SearchItemIDString(_basename)].itemIcon_sprite;
                    EatAnimPanel_itemImage.sprite = texture2d;

                    //カメラ寄る。
                    trans = 2; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //Live2D「うわぁ～～」のアニメーション
                    girl1_status.AddMotionAnimReset(); //現在再生していたAddMotionアニメを、強制的にオフにする。
                    trans_motion = 2;
                    live2d_animator.SetInteger("trans_motion", trans_motion);

                    trans_expression = 110; //各表情に遷移。
                    live2d_animator.SetInteger("trans_expression", trans_expression);

                    _model.GetComponent<CubismAutoEyeBlinkInput>().enabled = false;
                    _model.GetComponent<CubismEyeBlinkController>().enabled = false;

                    touch_controller.Touch_OnAllOFF();

                    break;

                case 1: // 状態2

                    if (timeOut <= 0.0)
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

                    girl1_status.gireleat_start_flag = false;

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

                    //Live2Dモーションをもとに戻す
                    trans_motion = 0;
                    live2d_animator.SetInteger("trans_motion", trans_motion);

                    trans_expression = 2; //ごきげん表情に一度戻す。
                    live2d_animator.SetInteger("trans_expression", trans_expression);

                    _model.GetComponent<CubismAutoEyeBlinkInput>().enabled = true;
                    _model.GetComponent<CubismEyeBlinkController>().enabled = true;

                    EatAnimPanel.SetActive(false);
                    EatStartEffect.SetActive(false);

                    break;

                default:
                    break;
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }

        //ハートゲージのアニメーション中
        if (heart_animON)
        {
            if (loveplusanimON)
            { } //ハート実際に発生したときは、ハートが衝突したときにスライドの表示更新の処理を優先するので、以下の処理を使わなくする。
            else
            {
                if (_tempResultGirllove >= _tempGirllove) //アニメ前より、現在の計算用ハートが+
                {
                    if (timeOutHeart <= 0)
                    {
                        timeOutHeart = 0.05f;

                        _lovecounter++;
                        Slider_Koushin((int)(_lovecounter));

                        girl_param.color = new Color(255f / 255f, 255f / 255f, 255f / 255f); //青文字(105f / 255f, 168f / 255f, 255f / 255f)
                        girl_param.text = _lovecounter.ToString();
                        //sc.PlaySe(37); //トゥルルルルという文字送り音

                        if (_lovecounter >= _tempResultGirllove) //増える処理の終了
                        {
                            EndDegHeart();
                        }
                    }
                }
            }

            //スライダの更新 減る処理の場合のみ、Updateで行う。
            if (_tempResultGirllove < _tempGirllove) //アニメ前より、現在の計算用ハートが-
            {
                if (timeOutHeart <= 0)
                {
                    timeOutHeart = 0.05f;

                    _lovecounter--;
                    Slider_Koushin((int)(_lovecounter));

                    girl_param.color = new Color(105f / 255f, 168f / 255f, 255f / 255f); //青文字(105f / 255f, 168f / 255f, 255f / 255f)
                    girl_param.text = _lovecounter.ToString();
                    sc.PlaySe(37); //トゥルルルルという文字送り音

                    if (_lovecounter <= _tempResultGirllove) //減る処理の終了
                    {
                        EndDegHeart();
                    }
                }               
            }

            timeOutHeart -= Time.deltaTime;
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
                _basenameFull = database.items[kettei_item1].item_FullName;
                _basenameSlot = database.items[kettei_item1].item_SlotName;
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
                _basebeauty = database.items[kettei_item1].Beauty;
                _basescore = database.items[kettei_item1].Base_Score;
                _basegirl1_like = database.items[kettei_item1].girl1_itemLike;
                _baseitemtype = database.items[kettei_item1].itemType.ToString();
                _baseitemtype_sub = database.items[kettei_item1].itemType_sub.ToString();
                _basecost = database.items[kettei_item1].cost_price;
                _baseSetjudge_num = database.items[kettei_item1].SetJudge_Num;
                _basejuice = database.items[kettei_item1].Juice;
                _basegirl1_manpuku = database.items[kettei_item1].Manpuku;

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
                _basenameFull = pitemlist.player_originalitemlist[kettei_item1].item_FullName;
                _basenameSlot = pitemlist.player_originalitemlist[kettei_item1].item_SlotName;
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
                _basebeauty = pitemlist.player_originalitemlist[kettei_item1].Beauty;
                _basescore = pitemlist.player_originalitemlist[kettei_item1].Base_Score;
                _basegirl1_like = pitemlist.player_originalitemlist[kettei_item1].girl1_itemLike;
                _baseitemtype = pitemlist.player_originalitemlist[kettei_item1].itemType.ToString();
                _baseitemtype_sub = pitemlist.player_originalitemlist[kettei_item1].itemType_sub.ToString();
                _basecost = pitemlist.player_originalitemlist[kettei_item1].cost_price;
                _baseSetjudge_num = pitemlist.player_originalitemlist[kettei_item1].SetJudge_Num;
                _basejuice = pitemlist.player_originalitemlist[kettei_item1].Juice;
                _basegirl1_manpuku = pitemlist.player_originalitemlist[kettei_item1].Manpuku;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_originalitemlist[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            case 2:

                _baseID = pitemlist.player_extremepanel_itemlist[kettei_item1].itemID;
                _basename = pitemlist.player_extremepanel_itemlist[kettei_item1].itemName;
                _basenameHyouji = pitemlist.player_extremepanel_itemlist[kettei_item1].itemNameHyouji;
                _basenameFull = pitemlist.player_extremepanel_itemlist[kettei_item1].item_FullName;
                _basenameSlot = pitemlist.player_extremepanel_itemlist[kettei_item1].item_SlotName;
                _basequality = pitemlist.player_extremepanel_itemlist[kettei_item1].Quality;
                _basesweat = pitemlist.player_extremepanel_itemlist[kettei_item1].Sweat;
                _basebitter = pitemlist.player_extremepanel_itemlist[kettei_item1].Bitter;
                _basesour = pitemlist.player_extremepanel_itemlist[kettei_item1].Sour;
                _baserich = pitemlist.player_extremepanel_itemlist[kettei_item1].Rich;
                _basecrispy = pitemlist.player_extremepanel_itemlist[kettei_item1].Crispy;
                _basefluffy = pitemlist.player_extremepanel_itemlist[kettei_item1].Fluffy;
                _basesmooth = pitemlist.player_extremepanel_itemlist[kettei_item1].Smooth;
                _basehardness = pitemlist.player_extremepanel_itemlist[kettei_item1].Hardness;
                _basejiggly = pitemlist.player_extremepanel_itemlist[kettei_item1].Jiggly;
                _basechewy = pitemlist.player_extremepanel_itemlist[kettei_item1].Chewy;
                _basepowdery = pitemlist.player_extremepanel_itemlist[kettei_item1].Powdery;
                _baseoily = pitemlist.player_extremepanel_itemlist[kettei_item1].Oily;
                _basewatery = pitemlist.player_extremepanel_itemlist[kettei_item1].Watery;
                _basebeauty = pitemlist.player_extremepanel_itemlist[kettei_item1].Beauty;
                _basescore = pitemlist.player_extremepanel_itemlist[kettei_item1].Base_Score;
                _basegirl1_like = pitemlist.player_extremepanel_itemlist[kettei_item1].girl1_itemLike;
                _baseitemtype = pitemlist.player_extremepanel_itemlist[kettei_item1].itemType.ToString();
                _baseitemtype_sub = pitemlist.player_extremepanel_itemlist[kettei_item1].itemType_sub.ToString();
                _basecost = pitemlist.player_extremepanel_itemlist[kettei_item1].cost_price;
                _baseSetjudge_num = pitemlist.player_extremepanel_itemlist[kettei_item1].SetJudge_Num;
                _basejuice = pitemlist.player_extremepanel_itemlist[kettei_item1].Juice;
                _basegirl1_manpuku = pitemlist.player_extremepanel_itemlist[kettei_item1].Manpuku;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_extremepanel_itemlist[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_extremepanel_itemlist[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            default:
                break;
        }



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
                if (_basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    itemslotScore[count]++;
                }
                count++;
            }
        }

        //固有トッピングスロットも見る。一致する効果があれば、所持数+1。現在は未使用。
        /*for (i = 0; i < _koyutp.Length; i++)
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
        }*/

        //確認用
        /*count = 0;
        foreach (string key in itemslotInfo)
        {
            Debug.Log(key + ": " + itemslotScore[count].ToString());
            count++;
        }*/

        if (contest_type == 0) //コンテストや、その他イベントシーンでは使用しない compoundMainオンリー。
        {
            switch (girl1_status.timeGirl_hungry_status)
            {
                case 0: //お腹が特に減ってない状態。はなくなった

                    //Debug.Log("今はあまりお腹が減ってないらしい");

                    //判定アニメ起動
                    judge_anim_on = true;
                    judge_end = false;
                    StartCoroutine("Girl_Judge_anim_co");

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

                    GameMgr.compound_status = 0;
                    //お菓子の判定処理を終了
                    compound_Main.girlEat_ON = false;

                    break;

                default:
                    break;
            }
        }


        // => Compound_Mainに戻る

        //デバッグパネルの取得
        /*debug_panel = canvas.transform.Find("Debug_Panel(Clone)").GetComponent<Debug_Panel>();
        debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();
        debug_taste_resultText.text = "";*/
        //DebugTextLog();

    }

    void DebugTextLog()
    {
        //デバッグ用　計算結果の表示

        debug_taste_resultText.text +=
            "\n" + "\n" + "###  好みの比較　結果　###"
            + "\n" + "\n" + "コンポ判定の番号(0~2）: " + countNum
            + "\n" + "\n" + "判定用お菓子セットの番号: " + _girl_judgenum[countNum]
            + "\n" + "\n" + "アイテム名: " + _basenameHyouji
            + "\n" + "\n" + "あまさ: " + _basesweat
            + "\n" + " 女の子の好みの甘さ: " + _girlsweat[countNum]
            + "\n" + "あまさの差: " + sweat_result
            + "\n" + " 点数: " + sweat_score
            + "\n" + "\n" + "苦さ: " + _basebitter
            + "\n" + " 女の子の好みの苦さ: " + _girlbitter[countNum]
            + "\n" + "にがさの差: " + bitter_result
            + "\n" + " 点数: " + bitter_score
            + "\n" + "\n" + "酸味: " + _basesour
            + "\n" + " 女の子の好みの酸味: " + _girlsour[countNum]
            + "\n" + "酸味の差: " + sour_result
            + "\n" + " 点数: " + sour_score
            + "\n" + "\n" + "さくさく度: " + _basecrispy + "\n" + "さくさく閾値: " + _girlcrispy[countNum] + "\n" + " 点数: " + crispy_score
            + "\n" + "\n" + "ふわふわ度: " + _basefluffy + "\n" + "ふわふわ閾値: " + _girlfluffy[countNum] + "\n" + " 点数: " + fluffy_score
            + "\n" + "\n" + "なめらか度: " + _basesmooth + "\n" + "なめらか閾値: " + _girlsmooth[countNum] + "\n" + " 点数: " + smooth_score
            + "\n" + "\n" + "歯ごたえ度: " + _basehardness + "\n" + "歯ごたえ閾値: " + _girlhardness[countNum] + "\n" + " 点数: " + hardness_score
            + "\n" + "\n" + "飲みごたえ度: " + _basejuice + "\n" + "飲みごたえ閾値: " + _girljuice[countNum] + "\n" + " 点数: " + juice_score
            + "\n" + "\n" + "ぷるぷる度: " + "-"
            + "\n" + "\n" + "噛み応え度: " + "-"
            + "\n" + "\n" + "判定セットごとの基本得点: " + set_score
            + "\n" + "\n" + "トッピングスコア: " + topping_score
            + "\n" + "\n" + "お菓子の見た目: " + _basebeauty + "\n" + "見た目閾値: " + _girlbeauty[countNum] + "\n" + "見た目スコア: " + beauty_score
            + "\n" + "\n" + "総合得点: " + total_score;
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
            _girljuice[i] = girl1_status.girl1_Juice[i];

            _girlbeauty[i] = girl1_status.girl1_Beauty[i];

            _girl_subtype[i] = girl1_status.girl1_likeSubtype[i];
            _girl_likeokashi[i] = girl1_status.girl1_likeOkashi[i];

            _girl_set_score[i] = girl1_status.girl1_like_set_score[i];
            _girl_comment_flag[i] = girl1_status.girllike_comment_flag[i];

            _girl_judgenum[i] = girl1_status.girllike_judgeNum[i];
        }

        //一回だけ代入すればよい。
        _girlpowdery = girl1_status.girl1_Powdery;
        _girloily = girl1_status.girl1_Oily;
        _girlwatery = girl1_status.girl1_Watery;

        //_set_compID = girl1_status.Set_compID;
    }

    IEnumerator Girl_Judge_anim_co()
    {
        judge_result(); //先に判定し、マズイなどがあったら、アニメにも反映する。

        judge_score(0, set_id); //点数の判定。中の数字は、女の子のお菓子の判定か、コンテストでの判定かのタイプ分け

        Touch_WindowInteractOFF();

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        canvas.SetActive(false);

        while (!GameMgr.camerazoom_endflag)
        {
            yield return null;
        }
        GameMgr.camerazoom_endflag = false;

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
                canvas.SetActive(true);
                Girl_reaction();
            }

            if (!GameMgr.Beginner_flag[0]) //はじめてクッキーをあげた場合に、ON
            {
                GameMgr.Beginner_flag[0] = true;
            }
        }
        else
        {
            Debug.Log("チュートリアル中");
            canvas.SetActive(true);
            Girl_reaction();
        }
    }



    //
    //お菓子判定処理
    //
    void judge_result()
    {
        non_spquest_flag = false;

        if (GameMgr.GirlLoveEvent_num == 50 && contest_type == 0) //コンテストのときに「あげる」をおすと、こちらの処理
        {
            SettingGirlHungrySet();
        }
        else
        {
            switch (girl1_status.OkashiNew_Status)
            {
                case 0: //メイン

                    JudgeGirlCompo();
                    break;

                case 1: //主にチュートリアル

                    SettingGirlHungrySet();
                    break;

                case 2: //エクストラ

                    switch (GameMgr.GirlLoveEvent_num)
                    {
                        case 0: //ハートあげる系

                            //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。  
                            SettingGirlHungrySet();
                            break;

                        case 1: //ハートあげる系

                            //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。  
                            SettingGirlHungrySet();
                            break;

                        case 2: //ハートあげる系

                            //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。  
                            SettingGirlHungrySet();
                            break;

                        case 3: //ハートあげる系

                            //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。  
                            SettingGirlHungrySet();
                            break;

                        case 20: //ハートあげる系

                            //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。  
                            SettingGirlHungrySet();
                            break;

                        default: //特殊なものがない限りは、デフォルト。60点以上でクリア

                            JudgeGirlCompo();
                            break;
                    }


                    break;
            }
        }

        GameMgr.Okashi_dislike_status = dislike_status;
    }

    void SettingGirlHungrySet()
    {
        non_spquest_flag = true; //そもそも吹き出しに合わない設定

        //まず、値のセッティング
        girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //
        SetGirlTasteInit();

        dislike_status = 1; //1=デフォルトで良い。 2=新しいお菓子だった。　3=まずい。　4=嫌い。 5=今はこれの気分じゃない。
        dislike_flag = true;
        set_id = 0;

        if (database.items[_baseID].Eat_kaisu == 0) //新しい食べ物の場合
        {
            dislike_status = 2;
        }
    }

    void JudgeGirlCompo()
    {
        //まず、値のセッティング
        girl1_status.SetQuestRandomSet(girl1_status.OkashiQuest_ID); //girlLikeCompoから3つのセットを指定
        SetGirlTasteInit();

        //判定処理
        DislikeJudge();


        //この時点で、吹き出しと違うものであれば、dislike_flagがfalse。
        Debug.Log("dislike_flag: " + dislike_flag);

        //
        //判定処理　パターンB
        //

        //吹き出しにあっているかいないかの判定。
        if (dislike_flag == false) //吹き出しに合っていない場合
        {
            non_spquest_flag = true;

            //クエストとは無関係に、お菓子を判定する。お菓子ごとの設定された判定に従って、お菓子の判定。

            //お菓子の判定値をセッティング
            girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
            SetGirlTasteInit();

            dislike_flag = true;
            set_id = 0;

            if (database.items[_baseID].Eat_kaisu == 0) //新しい食べ物の場合
            {
                dislike_status = 2;
            }
            else
            {
                dislike_status = 1; //1=デフォルトで良い。 2=新しいお菓子だった。　3=まずい。　4=嫌い。 5=今はこれの気分じゃない。
            }
        }
        else //吹き出しに合っていた場合に、味を判定する。
        {

        }
    }

    void DislikeJudge()
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
                    dislike_flag = false;
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
    }

    public void judge_score(int _setType, int _setCountNum)
    {
        //初期化。

        //お菓子の判定処理

        //クッキーの場合はさくさく感など。大きいパラメータをまず見る。次に甘さ・苦さ・酸味が、女の子の好みに近いかどうか。
        Mazui_flag = false; //初期化
        countNum = _setCountNum;
        total_score = 0;

        set_score = 0;
        shokukan_score = 0;
        crispy_score = 0;
        fluffy_score = 0;
        smooth_score = 0;
        hardness_score = 0;
        juice_score = 0;
        beauty_score = 0;
        topping_score = 0;
        topping_flag_point = 0;
        topping_flag = false;
        topping_all_non = false; //判定のトッピングスロットが全てNon
        //last_score_kousin = false;

        //未使用。
        rich_score = 0;
        jiggly_score = 0;
        chewy_score = 0;


        //セットごとの基本得点。難易度が高いお菓子ほど、はじめから得点が上がりやすいように補正がかかる。
        set_score = _girl_set_score[countNum];

        //味パラメータの計算。味は、GirlLikeSetの値で、理想値としている。
        //理想の値に近いほど高得点。超えすぎてもいけない。

        //rich_result = _baserich - _girlrich[countNum];
        sweat_result = _basesweat - _girlsweat[countNum];
        bitter_result = _basebitter - _girlbitter[countNum];
        sour_result = _basesour - _girlsour[countNum];


        //あまみ・にがみ・さんみに対して、それぞれの評価。差の値により、7段階で評価する。
        //元のセットの値が0のときは、計算せずスコアに加点しない。
        Debug.Log("### ###");
        Debug.Log("判定番号: " + _girl_judgenum[countNum]);

        //甘味
        sweat_score = TasteKeisanBase(_girlsweat[countNum], sweat_result, "甘味: "); //クエストの値, お菓子の値-クエストの値, デバッグ表示用。返り値は、点数。
        sweat_level = TasteLevel_Keisan(_girlsweat[countNum], sweat_score);
        Debug.Log("甘み点: " + sweat_score);

        //苦み
        bitter_score = TasteKeisanBase(_girlbitter[countNum], bitter_result, "苦み: ");
        bitter_level = TasteLevel_Keisan(_girlbitter[countNum], bitter_score);
        Debug.Log("苦味点: " + bitter_score);

        //酸味
        sour_score = TasteKeisanBase(_girlsour[countNum], sour_result, "酸味: ");
        sour_level = TasteLevel_Keisan(_girlsour[countNum], sour_score);
        Debug.Log("酸味点: " + sour_score);

        //コンテスト用（その他NPCイベントの感想用）にここでヒントを更新
        SweatHintHyouji();
        BitterHintHyouji();
        SourHintHyouji();


        //食感パラメータは、大きければ大きいほど、そのまま得点に。
        //ただし、女の子の好み値を超えてないと加点されない。
        //サブジャンルごとに、比較の対象が限定される。例えば、クッキーなら、さくさく度だけを見る。
        //またジャンルごとに、どのスコアの比重が大きくなるか、補正がかかる。アイスなら甘味が大事、とか。
        ShokukanScore_keisan(_baseitemtype_sub);
        ShokukanHintHyouji();


        //トッピングの値も計算する。①基本的に上がるやつ　+　②クエスト固有でさらに上がるやつ　の2点

        //①  スロットがついていれば、絶対に加算される。スロットネームDBのtotal_scoreを加算する。
        for (i = 0; i < itemslotScore.Count; i++)
        {

            //0はNonなので、無視
            if (i != 0)
            {
                //トッピングされているものに応じて、得点+見た目点数。２つは、意味は違うが、実質の計算上一緒なので、topping_scoreは基本0でもいいかも。
                if (itemslotScore[i] > 0)
                {
                    topping_score += slotnamedatabase.slotname_lists[i].slot_totalScore * itemslotScore[i]; //味に対するボーナス得点
                    _basebeauty += slotnamedatabase.slotname_lists[i].slot_Beauty * itemslotScore[i]; //見た目に対するボーナス得点
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
                        topping_all_non = true; //女の子が食べたいトッピングがある場合true

                        //女の子のスコア(所持数)より、生成したアイテムのスロットの所持数が大きい場合は、そのトッピングが好みとマッチしている。正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet1[i])
                        {
                            topping_score += girl1_status.girl1_hungryToppingScoreSet1[i] * itemslotScore[i];

                            //該当したスロットの、フラグもたてる。複数のフラグがたつ場合は、何か処理をしたい。けど、とりあえず未実装。一個だけ対応。今のとこ、一番後ろのTPに反応する。
                            topping_flag_point = girl1_status.girl1_hungryToppingNumberSet1[i]; //スロット左から何番目がヒットしたか。
                            topping_flag = true; //好みが一致するトッピングが、一つでもあった。
                        }
                        else //逆に、必要なトッピングがあるのに、そのトッピングがのってなかった場合、
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
                        topping_all_non = true;

                        //女の子のスコア(所持数)より、生成したアイテムのスロットの所持数が大きい場合は、そのトッピングが好みとマッチしている。正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet2[i])
                        {
                            topping_score += girl1_status.girl1_hungryToppingScoreSet2[i] * itemslotScore[i];

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
                        topping_all_non = true;

                        //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet3[i])
                        {
                            topping_score += girl1_status.girl1_hungryToppingScoreSet3[i] * itemslotScore[i];

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



        //女の子の食べたいトッピングがあるにも関わらず、そのトッピングが一つものっていなかった。
        if (topping_all_non && !topping_flag)
        {
            topping_score += girl1_status.girl1_NonToppingScoreSet[countNum]; //点数がマイナスに働く。
        }
        Debug.Log("トッピングスコア: " + topping_score);

        //見た目点数の計算
        if (_girlbeauty[countNum] > 0)
        {
            beauty_score = _basebeauty - _girlbeauty[countNum];
        }
        else
        {
            beauty_score = 0;
        }

        //最終の点数合計
        if (GameMgr.Story_Mode == 0)
        {
            //以上、全ての点数を合計。
            TotalScoreKeisan();
        }
        else
        {
            switch (GameMgr.GirlLoveEvent_num)
            {

                case 13: //カミナリのようにすっぱいクレープ 酸味で点数があがる

                    if (_baseitemtype_sub == "Crepe")
                    {
                        total_score = (int)(_basesour * 1.2f) + (int)(shokukan_score * 0.2f);
                    }
                    else
                    {
                        //以上、全ての点数を合計。
                        TotalScoreKeisan();
                    }
                    break;

                default:

                    //以上、全ての点数を合計。
                    TotalScoreKeisan();
                    break;
            }
        }

            

        //水っぽい・油っぽいなどの減点点数処理
        Debuf_Param_Judge();

        switch (dislike_status)
        {
            case 3: //水っぽいなどがでたら、total_scoreをマイナス。

                total_score = 0;
                //total_score -= dislike_score;
                break;

            case 4: //嫌いな材料が使われても、total_score=0に。

                total_score = 0;
                break;
        }

        //フリーモード時　さらに計算。コンテスト時は除外。
        if (GameMgr.Story_Mode == 1)
        {
            if (_setType == 0)
            {
                if (GameMgr.NowEatOkashiID == _baseID) //食べたいお菓子をあげた場合。得点も少し上がる。
                {
                    total_score = (int)(total_score * 1.3f);
                }
            }
        }

        //まずいの判定
        if (total_score < GameMgr.mazui_score) //total_scoreが30より下だと、マズイ。
        {
            Mazui_flag = true;

        }
        else
        {
        }

        GameMgr.Okashi_totalscore = total_score;

        Debug.Log("###  ###");
        Debug.Log("総合点: " + total_score);
        Debug.Log("###  ###");

        //得点に応じて、好感度・お金に補正がかかる。→ LoveScoreCal()で計算



        //過去最高点の更新
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
                    database.items[_baseID].last_juice_score = _basejuice;
                    database.items[_baseID].last_beauty_score = _basebeauty;

                    //last_score_kousin = true;

                    //85点以上で、さらに高得点を一度もとったことがなければ、えめらるどんぐり一個もらえる
                    if (database.items[_baseID].HighScore_flag == 0)
                    {
                        if (total_score >= GameMgr.high_score)
                        {
                            emerarudonguri_get = true;
                        }
                    }
                    else if (database.items[_baseID].HighScore_flag == 1)
                    {
                        if (total_score >= GameMgr.high_score_2)
                        {
                            emerarudonguri_get = true;
                        }
                    }
                }

                //ゲーム中の最高スコアを更新した場合。
                if (total_score > GameMgr.Okashi_toplast_score)
                {
                    GameMgr.Okashi_toplast_score = total_score;
                }

                //食べたお菓子のスコアを保存する。
                GameMgr.Okashi_last_totalscore = total_score;

                //エクストラモード
                if (GameMgr.Story_Mode == 1)
                {
                    if (total_score >= 777) //ゲーム中に777点以上をだすと、レコードがもらえる。未実装。
                    {
                        //GameMgr.specialsubevent_flag1 = true;
                    }

                    //そのクエスト内で、最高得点を更新した場合。
                    if (total_score > GameMgr.Okashi_spquest_MaxScore)
                    {
                        GameMgr.Okashi_spquest_MaxScore = total_score;
                    }

                }

                break;

            case 1:

                break;
        }


        if (_setType == 1) //コンテストのときは、ここでデバッグテキスト更新
        {
            //デバッグパネルの取得
            debug_panel = canvas.transform.Find("Debug_Panel(Clone)").GetComponent<Debug_Panel>();
            debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();
            DebugTextLog();
        }
        else
        {
            //デバッグパネルの取得
            debug_panel = canvas.transform.Find("Debug_Panel(Clone)").GetComponent<Debug_Panel>();
            debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();
            debug_taste_resultText.text = "";
            DebugTextLog();
        }
    }

    void TotalScoreKeisan()
    {
        //以上、全ての点数を合計。
        total_score = set_score + sweat_score + bitter_score + sour_score
        + shokukan_score + topping_score + beauty_score;
    }

    void Debuf_Param_Judge()
    {
        dislike_score = 0;

        if (_basepowdery > GameMgr.Watery_Line) //50より上
        {
            //dislike_flag = false;
            dislike_score += _basepowdery * 2;
            dislike_status = 3;
            dislike_num = 0;
        }
        if (_baseoily > GameMgr.Watery_Line)
        {
            //dislike_flag = false;
            dislike_score += _baseoily * 2;
            dislike_status = 3;
            dislike_num = 1;
        }
        //ジュース・ティー・コーヒー類は、水系の値の判定をなくす。もともと、みずなので。
        if (_baseitemtype_sub == "Juice" || _baseitemtype_sub == "Tea" || _baseitemtype_sub == "Tea_Potion" || _baseitemtype_sub == "Coffee_Mat"
            || _baseitemtype_sub == "Coffee")
        { }
        else {
            if (_basewatery > GameMgr.Watery_Line)
            {
                //dislike_flag = false;
                dislike_score += _basewatery * 2;
                dislike_status = 3;
                dislike_num = 2;
            }
        }

        if (_basegirl1_like <= 0) //女の子の好みが-のものも、嫌われる。お菓子それ自体が嫌い、ということ。
        {
            dislike_flag = false;
            dislike_status = 4;
        }

    }

    //酒場クエスト(Quest_Judge.cs)などからも読み込み
    public int TasteKeisanBase(int _girltaste, int _taste_result, string _taste_Type)
    {
        if (_girltaste == 0) //クエスト判定の値が0なら、そもそも判定しない。
        {
            Debug.Log(_taste_Type + "判定なし");
            taste_score = 0;
        }
        else
        {
            TasteScore_keisan(_taste_result, _taste_Type);     //差をいれると、点数計算       
        }

        return taste_score;
    }

    void TasteScore_keisan(int _taste_result, string _taste_type)
    {
        if (Mathf.Abs(_taste_result) == 0)
        {
            Debug.Log(_taste_type + "Perfect!!");　//完璧な具合
            taste_score = 100;
        }
        else if (Mathf.Abs(_taste_result) < 5) //+-1~4　絶妙な塩梅
        {
            Debug.Log(_taste_type + "Great!!");
            taste_score = 80;
        }
        else if (Mathf.Abs(_taste_result) < 10) //+-5~9  ほどよく良い塩梅
        {
            Debug.Log(_taste_type + "Well done!");
            taste_score = 40;
        }
        else if (Mathf.Abs(_taste_result) < 15) //+-10~14　いい感じ
        {
            Debug.Log(_taste_type + "Well!");
            taste_score = 20;
        }
        else if (Mathf.Abs(_taste_result) < 20) //+-15~19　いい感じ
        {
            Debug.Log(_taste_type + "A little Well!");
            taste_score = 20;
        }
        else if (Mathf.Abs(_taste_result) < 30) //+-20~29  ちょっと足りないかな
        {
            Debug.Log(_taste_type + "Good!");
            taste_score = 20;
        }
        else if (Mathf.Abs(_taste_result) < 50) //+-30~49　全然足りない
        {
            Debug.Log(_taste_type + "Normal");
            taste_score = 10;
        }
        else if (Mathf.Abs(_taste_result) < 80) //+-50~79
        {
            Debug.Log(_taste_type + "poor");
            taste_score = 0;
        }
        else if (Mathf.Abs(_taste_result) <= 100) //+-80~99
        {
            Debug.Log(_taste_type + "death..");
            taste_score = -80;
        }
        else if (Mathf.Abs(_taste_result) > 100) //+-100
        {
            Debug.Log(_taste_type + "death..");
            taste_score = -80;
        }
    }

    public int TasteLevel_Keisan(int _girltaste, int _tast_score)
    {
        if (_girltaste == 0)
        {
            return 0; //クエスト判定の値が0なら、そもそも判定しない。
        }
        else
        {
            if (_tast_score >= 100)
            {
                taste_level = 8;
            }
            else if ((_tast_score < 100) && _tast_score >= 45)
            {
                taste_level = 7;
            }
            else if ((_tast_score < 45) && _tast_score >= 35)
            {
                taste_level = 6;
            }
            else if ((_tast_score < 35) && _tast_score >= 23)
            {
                taste_level = 5;
            }
            else if ((_tast_score < 23) && _tast_score >= 15)
            {
                taste_level = 5;
            }
            else if ((_tast_score < 15) && _tast_score >= 10)
            {
                taste_level = 4;
            }
            else if ((_tast_score < 10) && _tast_score >= 0)
            {
                taste_level = 3;
            }
            else if ((_tast_score < 0) && _tast_score >= -35)
            {
                taste_level = 2;
            }
            else if (_tast_score < -35)
            {
                taste_level = 1;
            }

            return taste_level;
        }
    }

    void ShokukanScore_keisan(string _temp_baseitemtype_sub)
    {
        switch (_temp_baseitemtype_sub)
        {
            case "Biscotti":
                Hardness_Score();
                break;
            case "Bread":
                Crispy_Score();
                break;
            case "Bread_Sliced":
                Crispy_Score();
                break;
            case "Cookie":
                Crispy_Score();
                break;
            case "Cookie_Mat":
                Crispy_Score();
                break;
            case "Cookie_Hard":
                Hardness_Score();
                break;
            case "Chocolate":
                Smooth_Score();
                break;
            case "Chocolate_Mat":
                Smooth_Score();
                break;
            case "Cake":
                Fluffy_Score();
                break;
            case "Cake_Mat":
                Fluffy_Score();
                break;
            case "Castella":
                Fluffy_Score();
                break;
            case "Cannoli":
                Crispy_Score();
                break;
            case "Candy":
                Hardness_Score();
                break;
            case "Crepe":
                Fluffy_Score();
                break;
            case "Crepe_Mat":
                Fluffy_Score();
                break;
            case "Creampuff":
                Fluffy_Score();
                break;
            case "Coffee":
                Tea_Score();
                break;
            case "Coffee_Mat":
                Tea_Score();
                break;
            case "Donuts":
                Fluffy_Score();
                break;
            case "Financier":
                Fluffy_Score();
                break;
            case "IceCream":
                Smooth_Score();
                break;
            case "Juice":
                Juice_Score();
                break;
            case "Jelly":
                Hardness_Score();
                Smooth_Score();
                break;
            case "Maffin":
                Fluffy_Score();
                break;
            case "PanCake":
                Fluffy_Score();
                break;
            case "Parfe":
                Crispy_Score();
                Fluffy_Score();
                Smooth_Score();
                break;
            case "Pie":
                Crispy_Score();
                break;
            case "Rusk":
                Crispy_Score();
                break;
            case "SumireSuger":
                Tea_Score();
                break;
            case "Tea":
                Tea_Score();
                break;
            case "Tea_Mat":
                Tea_Score();
                break;
            case "Tea_Potion":
                Tea_Score();
                break;

            default:
                Crispy_Score();
                break;
        }
    }


    void Crispy_Score()
    {
        if (_girlcrispy[countNum] > 0)
        {
            _temp_kyori = _basecrispy - _girlcrispy[countNum];
            _temp_deg = 1.0f * _basecrispy / _girlcrispy[countNum];


            if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
            {
                _temp_ratio = 1.0f;
                Debug.Log("_temp_deg: " + _temp_deg);

                crispy_score = (int)(_basescore * _temp_ratio * _temp_deg);
            }
            else
            {
                _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                Debug.Log("_temp_ratio: " + _temp_ratio);

                crispy_score = (int)(_basescore * _temp_ratio);
            }
        }
        else
        {
            crispy_score = 0;
        }

        //crispy_score = _basecrispy;
        shokukan_baseparam = _basecrispy;
        shokukan_score += crispy_score;
        shokukan_mes = "さくさく感";
        Debug.Log("サクサク度ベース: " + _basecrispy + " 判定値: " + _girlcrispy[countNum] + " サクサク度の点: " + crispy_score);
    }

    void Fluffy_Score()
    {
        if (_girlfluffy[countNum] > 0)
        {
            _temp_kyori = _basefluffy - _girlfluffy[countNum];
            _temp_deg = 1.0f * _basefluffy / _girlfluffy[countNum];

            if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
            {
                _temp_ratio = 1.0f;
                Debug.Log("_temp_deg: " + _temp_deg);

                fluffy_score = (int)(_basescore * _temp_ratio * _temp_deg);
            }
            else
            {
                _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                Debug.Log("_temp_ratio: " + _temp_ratio);

                fluffy_score = (int)(_basescore * _temp_ratio);
            }
        }
        else
        {
            fluffy_score = 0;
        }

        //fluffy_score = _basefluffy;
        shokukan_baseparam = _basefluffy;
        shokukan_score += fluffy_score;
        shokukan_mes = "ふわふわ感";
        Debug.Log("ふわふわ度ベース: " + _basefluffy + " 判定値: " + _girlfluffy[countNum] + " ふわふわ度の点: " + fluffy_score);
    }

    void Smooth_Score()
    {
        if (_girlsmooth[countNum] > 0)
        {
            _temp_kyori = _basesmooth - _girlsmooth[countNum];
            _temp_deg = 1.0f * _basesmooth / _girlsmooth[countNum];

            if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
            {
                _temp_ratio = 1.0f;
                Debug.Log("_temp_deg: " + _temp_deg);

                smooth_score = (int)(_basescore * _temp_ratio * _temp_deg);
            }
            else
            {
                _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                Debug.Log("_temp_ratio: " + _temp_ratio);

                smooth_score = (int)(_basescore * _temp_ratio);
            }
        }
        else
        {
            smooth_score = 0;
        }

        //smooth_score = _basesmooth;
        shokukan_baseparam = _basesmooth;
        shokukan_score += smooth_score;
        shokukan_mes = "なめらか感";
        Debug.Log("なめらかベース: " + _basesmooth + " 判定値: " + _girlsmooth[countNum] + " なめらかの点: " + smooth_score);
    }

    void Hardness_Score()
    {
        if (_girlhardness[countNum] > 0)
        {
            _temp_kyori = _basehardness - _girlhardness[countNum];
            _temp_deg = 1.0f * _basehardness / _girlhardness[countNum];

            if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
            {
                _temp_ratio = 1.0f;
                Debug.Log("_temp_deg: " + _temp_deg);

                hardness_score = (int)(_basescore * _temp_ratio * _temp_deg);
            }
            else
            {
                _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                Debug.Log("_temp_ratio: " + _temp_ratio);

                hardness_score = (int)(_basescore * _temp_ratio);
            }
        }
        else
        {
            hardness_score = 0;
        }

        //hardness_score = _basehardness;
        shokukan_baseparam = _basehardness;
        shokukan_score += hardness_score;
        shokukan_mes = "歯ごたえ";
        Debug.Log("歯ごたえベース: " + _basehardness + " 判定値: " + _girlhardness[countNum] + " 歯ごたえの点: " + hardness_score);
    }

    void Juice_Score()
    {
        if (_girljuice[countNum] > 0)
        {
            _temp_kyori = _basejuice - _girljuice[countNum];
            _temp_deg = 1.0f * _basejuice / _girljuice[countNum];

            if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
            {
                _temp_ratio = 1.0f;
                Debug.Log("_temp_deg: " + _temp_deg);

                juice_score = (int)(_basescore * _temp_ratio * _temp_deg);
            }
            else
            {
                _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                Debug.Log("_temp_ratio: " + _temp_ratio);

                juice_score = (int)(_basescore * _temp_ratio);
            }
        }
        else
        {
            juice_score = 0;
        }


        shokukan_baseparam = _basejuice;
        shokukan_score += juice_score;
        shokukan_mes = "のどごし";
        Debug.Log("のどごしベース: " + _basejuice + " 判定値: " + _girljuice[countNum] + " のどごしの点: " + juice_score);
    }

    void Tea_Score()
    {
        if (_girlcrispy[countNum] > 0)
        {
            _temp_kyori = _basecrispy - _girlcrispy[countNum];
            _temp_deg = 1.0f * _basecrispy / _girlcrispy[countNum];

            if (_temp_kyori >= 0) //好みよりも、お菓子の食感の値が、大きい。
            {
                _temp_ratio = 1.0f;
                Debug.Log("_temp_deg: " + _temp_deg);

                crispy_score = (int)(_basescore * _temp_ratio * _temp_deg);
            }
            else
            {
                _temp_ratio = SujiMap(Mathf.Abs(_temp_kyori), 0, 50, 1.0f, 0.1f);
                Debug.Log("_temp_ratio: " + _temp_ratio);

                crispy_score = (int)(_basescore * _temp_ratio);
            }
        }
        else
        {
            crispy_score = 0;
        }

        shokukan_baseparam = _basecrispy;
        shokukan_score += crispy_score;
        shokukan_mes = "香り";
        Debug.Log("香りベース: " + _basecrispy + " 判定値: " + _girlcrispy[countNum] + " 香り（サクサク度）の点: " + crispy_score);
    }

    void Girl_reaction()
    {
        if (dislike_flag == true) //正解の場合
        {

            //お菓子をたべたフラグをON + 食べた回数もカウント
            database.items[_baseID].Eat_kaisu += 1;
            Debug.Log("食べた回数: " + database.items[_baseID].Eat_kaisu);

            //エクストラモード時、そのクエストで食べた回数をカウント。
            GameMgr.Okashi_spquest_eatkaisu++;

            //好感度とお金を計算
            LoveScoreCal();

            //アイテムの削除
            delete_Item();

            OkashiSaitenhyouji(); //採点パネル表示してからリザルト

        }
        else //失敗の場合
        {

            switch (dislike_status)
            {

                case 3: //粉っぽいなど、マイナスの値が超えた。

                    girl1_status.hukidashiMessage("げろげろ..。ま、まずいよ..。兄ちゃん。");

                    //まずいときは、スコアは0点。
                    //total_score = 0;

                    //キャラクタ表情変更
                    //s.sprite = girl1_status.Girl1_img_verysad;

                    //好感度取得+アニメーションをON
                    Getlove_exp = -10;

                    //アイテムの削除
                    delete_Item();

                    OkashiSaitenhyouji(); //採点パネル表示してからリザルト

                    //音を鳴らす
                    sc.PlaySe(6);

                    //テキストウィンドウの更新
                    exp_Controller.GirlDisLikeText(Getlove_exp);

                    break;

                case 4: //嫌いな材料が使われていた

                    girl1_status.hukidashiMessage("ぐええ..。コレ嫌いー！..。");

                    //まずいときは、スコアは0点。
                    //total_score = 0;

                    //キャラクタ表情変更
                    //s.sprite = girl1_status.Girl1_img_verysad_close;

                    //好感度取得+アニメーションをON
                    Getlove_exp = -10;

                    //アイテムの削除
                    delete_Item();

                    OkashiSaitenhyouji(); //採点パネル表示してからリザルト

                    //音を鳴らす
                    sc.PlaySe(6);

                    //テキストウィンドウの更新
                    exp_Controller.GirlDisLikeText(Getlove_exp);

                    break;

                case 5: //吹き出しでない場合

                    girl1_status.hukidashiMessage("今はこれの気分じゃない！");

                    //キャラクタ表情変更
                    //s.sprite = girl1_status.Girl1_img_verysad_close;

                    //好感度は変わらず、お菓子も減りはしない。

                    //音を鳴らす
                    sc.PlaySe(6);

                    //テキストウィンドウの更新
                    exp_Controller.GirlNotEatText();

                    //お菓子をあげたあとの状態に移行する。残り時間を、短く設定。
                    girl1_status.timeGirl_hungry_status = 2;
                    girl1_status.timeOut = 5.0f;

                    girl1_status.GirlEat_Judge_on = true; //またカウントが進み始める

                    //お菓子の判定処理を終了
                    EndCompJudge();

                    break;

                default:

                    break;
            }

            //リセット＋フラグチェック
            //減る場合は、update内でちぇっく
        }

    }


    //
    //お菓子をあげたあとの吹き出しコメント・感想メソッド
    //
    IEnumerator WaitCommentDesc()
    {
        yield return new WaitForSeconds(3.0f);

        //ランダムで、吹き出しの内容を決定
        CommentTextInit();
        random = Random.Range(0, _commentDict.Count);
        _commentrandom = _commentDict[random];

        girl1_status.hukidashiMessage(_commentrandom);

        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
    }

    IEnumerator WaitCommentNewOkashiDesc()
    {
        yield return new WaitForSeconds(3.0f);

        girl1_status.hukidashiMessage(database.items[_baseID].itemNameHyouji + "うまいぞ！");

        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
    }



    //お菓子の採点結果を表示する。　シャキーーン！！　満足度　ドンドン　わーーーぱちぱちって感じ
    void OkashiSaitenhyouji()
    {
        //まずはリセット
        GameMgr.high_score_flag = false;

        //☆
        if (total_score > 0 && total_score < GameMgr.mazui_score)
        {
            //Delicious_Text.text = "Morte..";
            //Manzoku_Score.text = "★";

            star_Count = 1;
            SetHintText(0); //通常得点時
            Hint_Text.text = temp_hint_text;
        }
        else if (total_score >= GameMgr.mazui_score && total_score < GameMgr.low_score)
        {
            //Delicious_Text.text = "Bene!";
            //Manzoku_Score.text = "★★";

            star_Count = 2;
            SetHintText(0); //通常得点時
            Hint_Text.text = temp_hint_text;
        }
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score) //60~100
        {
            //Delicious_Text.text = "Di molto Bene!";
            //Manzoku_Score.text = "★★★";

            star_Count = 3;
            SetHintText(0); //通常得点時
            Hint_Text.text = temp_hint_text;
        }
        else if (total_score >= GameMgr.high_score && total_score < GameMgr.high_score_2) //100~150
        {
            //Delicious_Text.text = "Benissimo!!";
            //Manzoku_Score.text = "★★★★";

            star_Count = 4;
            SetHintText(1); //高得点時
            Hint_Text.text = temp_hint_text;
            GameMgr.high_score_flag = true; //ハイスコアでクリアしたので、高得点ゲットのフラグがたつ。
            if (database.items[_baseID].HighScore_flag < 1)
            {
                database.items[_baseID].HighScore_flag = 1;
                emeraldonguri_status = 0;
            }
        }
        else if (total_score >= GameMgr.high_score_2) //150点以上
        {
            //Delicious_Text.text = "Vittoria!!";
            //Manzoku_Score.text = "★★★★★";

            star_Count = 5;
            SetHintText(1); //高得点時
            Hint_Text.text = temp_hint_text;
            GameMgr.high_score_flag = true; //ハイスコアでクリアしたので、高得点ゲットのフラグがたつ。
            if (database.items[_baseID].HighScore_flag < 2)
            {
                database.items[_baseID].HighScore_flag = 2;
                emeraldonguri_status = 1;
            }
        }
        else if (total_score <= 0) //0以下。つまりまずかった
        {
            total_score = 0;
            GameMgr.Okashi_totalscore = total_score;
            //Delicious_Text.text = "Death..";

            star_Count = 0;

            if (dislike_status == 3)
            {
                SetHintText(3); //粉っぽいなどのマイナスの時
            }
            else
            {
                SetHintText(2); //マズイとき時
            }

            Hint_Text.text = temp_hint_text;
        }

        //二回目以降、まだ高得点でてないときは、お菓子後の特別演出を出すようにする。
        if (GameMgr.high_score_flag)
        {
            if (!GameMgr.OkashiQuestHighScore_event[GameMgr.GirlLoveEvent_num])
            {
                GameMgr.QuestClearCommentflag = false;
            }
        }

        if (Mazui_flag)
        {
            sc.PlaySe(6); //ダウン音
            compound_Main.GirlExpressionKoushin(-75); //まずいと、不機嫌になる。
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

        ScoreHyoujiPanel.SetActive(true);
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;

        Okashi_Result();
    }


    void Okashi_Result()
    {

        //キャラクタ表情変更
        if (!Mazui_flag)
        {
            girl1_status.face_girl_Yorokobi();
            StartCoroutine("OkashiAfterFaceChange");//2秒ぐらいしたら、表情だけすぐに戻す。
        }

        //そのクエストでの最高得点を保持。（マズイときは、失敗フラグ＝0点）
        if (special_quest.special_score_record[special_quest.spquest_set_num] <= total_score)
        {
            special_quest.special_score_record[special_quest.spquest_set_num] = total_score;
        }

        if (!GameMgr.tutorial_ON)
        {
            //クリア分岐1  点数関係のクリア条件
            QuestClearJoukenCheck();

            //クエストのときに、特定の条件を満たすと、次クエストが分岐する。
            SPQuest_BunkiCheck();
        }
    }

    void DefaultQuestJouken()
    {
        if (non_spquest_flag == false) //メインのSPお菓子クエストをクリアした。感想をだすだけ。ハートがたまらないと、次のSPクエストにはいけない。
        {
            //60点以上だった。
            if (total_score >= GameMgr.low_score)
            {
                Debug.Log("６０点以上がでた。満足。");

                if (GameMgr.Story_Mode == 0)
                {
                    if (topping_all_non && topping_flag) //食べたいトッピングがあり、どれか一つでもトッピングがのっていた。
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";
                    }
                    else if (topping_all_non && !topping_flag) //食べたいトッピングがあるが、該当するトッピングはのっていなかった。現状、それでもクリア可能。
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";
                    }
                    else if (!topping_all_non) //そもそも食べたいトッピングない場合
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";
                    }
                }
                else
                { }

                //それ以外で、食べたいトッピングの登録があるけど、それに該当するトッピングがない場合は、クリアはできない仕様。

            }
            else //そもそも60点以下
            {
                sp_quest_clear = false;
                _windowtext.text = "";
            }
        }
    }



    IEnumerator OkashiAfterFaceChange()
    {
        yield return new WaitForSeconds(2.0f);

        girl1_status.DefFaceChange();
    }

    void InitializeItemSlotDicts()
    {
        itemslotInfo.Clear();
        itemslotScore.Clear();

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslotScore.Add(0);
        }

    }

    void delete_Item()
    {
        switch (_toggle_type1)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                pitemlist.deletePlayerItem(database.items[kettei_item1].itemName, 1);
                break;

            case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                pitemlist.deleteOriginalItem(kettei_item1, 1);
                break;

            case 2: //お菓子パネルからアイテム削除

                pitemlist.deleteExtremePanelItem(kettei_item1, 1);

                //個数がまだ残ってた場合、一個消費し、そのままオリジナルアイテムリストへ移動。お菓子パネルのお菓子は削除する。
                if (pitemlist.player_extremepanel_itemlist.Count > 0)
                {
                    if (pitemlist.player_extremepanel_itemlist[0].ItemKosu > 0)
                    {
                        pitemlist.ExtremeToCopyOriginalItem(pitemlist.player_extremepanel_itemlist[0].ItemKosu);
                    }
                }

                //エクストリームアイテムのほうも、空にする。
                extreme_panel.deleteExtreme_Item();
                break;

            default:
                break;
        }
    }




    //
    //取得するお金と好感度の計算処理のメソッド
    //
    void LoveScoreCal()
    {

        slot_girlscore = 0;
        slot_money = 0;

        Getlove_exp = 0;
        GetMoney = 0;

        //前に計算したトータルスコアを元に計算。


        if (Mazui_flag)
        {
            Getlove_exp = (int)(-1.0f * PlayerStatus.girl1_Love_lv * (SujiMap(total_score, 0, GameMgr.mazui_score, GameMgr.mazui_score, 0) * 0.1f));
            Debug.Log("取得好感度: " + Getlove_exp);

        }
        else
        {
            //①トッピングの値で、好感度を加算する。ただし、60点以上でないと、加算されない。固有スロットもみている。
            if (total_score >= GameMgr.low_score)
            {
                for (i = 0; i < itemslotScore.Count; i++)
                {
                    //0はNonなので、無視
                    if (i != 0)
                    {
                        if (itemslotScore[i] > 0)
                        {
                            Getlove_exp += slotnamedatabase.slotname_lists[i].slot_getgirllove;
                            GetMoney += slotnamedatabase.slotname_lists[i].slot_Money;
                        }
                    }
                }
            }

            //②好感度取得  

            //点数計算。トータルスコア*0.1が基準の好感度。そこに女の子の好みの補正値(_basegirl1_like)と、スコアごとの補正をかける。
            //_basegirl1_likeは、女の子の好みで補正値。ねこクッキーで１が基準。オレンジクッキーだと２とか。
            if (total_score < GameMgr.mazui_score) //40点以下のときは、まずいになるので、実質30より下の処理は通らない。
            {

            }
            else if (total_score >= GameMgr.mazui_score && total_score < GameMgr.low_score) //60点以下のときは、好感度ほぼあがらず。
            {
                Getlove_exp += (int)((total_score * 0.1f) * (_basegirl1_like * 0.1f));
                GetMoney += (int)(_basecost * 0.5f);
                compound_Main.GirlExpressionKoushin(5);

                if (Getlove_exp <= 0) //ただし、1は必ず上がる。
                {
                    Getlove_exp = 1;
                }
            }
            else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score) //ベース×2
            {
                Getlove_exp += (int)((total_score * 0.1f) * (_basegirl1_like * 0.7f));
                GetMoney += (int)(_basecost * 1.0f);
                compound_Main.GirlExpressionKoushin(20);
            }
            else if (total_score >= GameMgr.high_score && total_score < GameMgr.high_score_2) //ベース×3
            {
                Getlove_exp += (int)((total_score * 0.1f) * (_basegirl1_like * 1.0f));
                GetMoney += (int)(_basecost * 1.5f);
                compound_Main.GirlExpressionKoushin(30);
            }
            else if (total_score >= GameMgr.high_score_2 && total_score < 220) //150点~220場合
            {
                Getlove_exp += (int)((total_score * 0.15f) * (_basegirl1_like * 1.15f));
                GetMoney += (int)(_basecost * 2.0f);
                GetMoney *= (int)(total_score * 0.01f);
                compound_Main.GirlExpressionKoushin(40);
            }
            else if (total_score >= 220 && total_score < 300) //220~300点を超えた場合、ベース×5
            {
                Getlove_exp += (int)((total_score * 0.17f) * (_basegirl1_like * 1.3f));
                GetMoney += (int)(_basecost * 2.2f);
                GetMoney *= (int)(total_score * 0.01f);
                compound_Main.GirlExpressionKoushin(50);
            }
            else if (total_score >= 300 && total_score < 500) //300~500点を超えた場合、ベース×5
            {
                Getlove_exp += (int)((total_score * 0.2f) * (_basegirl1_like * 1.5f));
                GetMoney += (int)(_basecost * 2.5f);
                GetMoney *= (int)(total_score * 0.01f);
                compound_Main.GirlExpressionKoushin(70);
            }
            else if (total_score >= 500) //300~500点を超えた場合、ベース×5
            {
                Getlove_exp += (int)((total_score * 0.4f) * (_basegirl1_like * 1.75f));
                GetMoney += (int)(_basecost * 2.0f);
                GetMoney *= (int)(total_score * 0.01f);
                compound_Main.GirlExpressionKoushin(100);
            }

            //そのお菓子を食べた回数でお金取得を割り算。同じお菓子を何度あげても、だんだんお金は上がらなくなってくる。
            /*if (database.items[_baseID].Eat_kaisu == 0)
            {
                database.items[_baseID].Eat_kaisu = 1; //0で割り算を回避。
            }
            GetMoney /= database.items[_baseID].Eat_kaisu;*/

            //③補正
            //そのお菓子を食べた回数で割り算。同じお菓子を何度あげても、だんだん好感度は上がらなくなってくる。
            Getlove_exp = Getlove_exp * 2; //2倍
            if (database.items[_baseID].Eat_kaisu == 0)
            {
                database.items[_baseID].Eat_kaisu = 1; //0で割り算を回避。
            }

            if (database.items[_baseID].Eat_kaisu >= 0 && database.items[_baseID].Eat_kaisu < 2)
            {
            }
            else if (database.items[_baseID].Eat_kaisu >= 2 && database.items[_baseID].Eat_kaisu < 5)
            {
                Getlove_exp = (int)(Getlove_exp * 0.75f);
            }
            else if (database.items[_baseID].Eat_kaisu >= 5 && database.items[_baseID].Eat_kaisu < 7) {
                Getlove_exp = (int)(Getlove_exp * 0.5f);
            }
            else if (database.items[_baseID].Eat_kaisu >= 7 && database.items[_baseID].Eat_kaisu < 15)
            {
                Getlove_exp = (int)(Getlove_exp * 0.25f);
            }
            else if (database.items[_baseID].Eat_kaisu >= 15 && database.items[_baseID].Eat_kaisu < 20)
            {
                Getlove_exp = (int)(Getlove_exp * 0.1f);
            }
            else if (database.items[_baseID].Eat_kaisu >= 20)
            {
                Getlove_exp = (int)(Getlove_exp * 0.05f);
            }


            if (Getlove_exp <= 1) { Getlove_exp = 1; }

            //再計算後、60点以上とれていたら、1は必ず上がる。マイナスにはならない。
            if (total_score >= GameMgr.low_score) //60点
            {
                if (Getlove_exp <= 0) //1は必ず上がる。
                {
                    Getlove_exp = 1;
                }
            }

            //エクストラモード時　さらに計算
            if (GameMgr.Story_Mode == 1)
            {
                Getlove_exp = (int)(Getlove_exp * 0.3f); //ハートが上がりにくく補正
                GameMgr.RandomEatOkashi_counter++;

                if (GameMgr.NowEatOkashiID == _baseID) //食べたいお菓子をあげた場合。ハート〇倍。
                {
                    Debug.Log("エクストラ　食べたいお菓子をあげた　ハート*1.2倍");

                    GameMgr.hikari_tabetaiokashi_buf = true; //一時的に特殊状態
                    GameMgr.hikari_tabetaiokashi_buf_time = 72; //効果時間デフォルト 1=5分

                    Getlove_exp = (int)(Getlove_exp * 1.2f);
                    PlayerStatus.player_girl_eatCount_tabetai++; //食べたいお菓子をあげた回数カウント

                    //体力も上がる。
                    PlayerStatus.player_girl_maxlifepoint += 2;

                    //機嫌もよくなる。
                    compound_Main.GirlExpressionKoushin(20);

                    //次で食べたいお菓子が強制的に変わる。
                    GameMgr.RandomEatOkashi_counter = 0;
                    girl1_status.RandomOkashiDecideMethod();
                }
            }

            //ハートレベルが上がるにつれて、ハート量獲得が減少する補正。
            if (PlayerStatus.girl1_Love_lv >= 70 && PlayerStatus.girl1_Love_lv < 80)
            {
                Getlove_exp = (int)(Getlove_exp * 0.75f);
            }
            else if (PlayerStatus.girl1_Love_lv >= 80 && PlayerStatus.girl1_Love_lv < 85)
            {
                Getlove_exp = (int)(Getlove_exp * 0.5f);
            }
            else if (PlayerStatus.girl1_Love_lv >= 90 && PlayerStatus.girl1_Love_lv < 95)
            {
                Getlove_exp = (int)(Getlove_exp * 0.25f);
            }
            else if (PlayerStatus.girl1_Love_lv >= 95 && PlayerStatus.girl1_Love_lv < 99)
            {
                Getlove_exp = (int)(Getlove_exp * 0.1f);
            }
            else if (PlayerStatus.girl1_Love_lv >= 99)
            {
                Getlove_exp = (int)(Getlove_exp * 1.0f);
            }

            //ハート　装備品による補正
            if(pitemlist.KosuCount("aroma_potion1") >= 1)
            {
                Getlove_exp = (int)(Getlove_exp * 1.2f);
            }

            //お金関係　装備品による補正
            _buf_moneyup = bufpower_keisan.Buf_CompFatherMoneyUp_Keisan();
            GetMoney = (int)(GetMoney * _buf_moneyup / 2);

            if (GameMgr.Story_Mode == 1)
            {
                GetMoney = (int)(GetMoney * 0.4f); //エクストラの最終的な調整　元のままだと、少し入りすぎた感があるため。
            }

            Debug.Log("最終の取得好感度: " + Getlove_exp);
            Debug.Log("取得お金: " + GetMoney);

            //先に、取得好感度をステージクリア用のハート入れに取得
            if (!GameMgr.tutorial_ON)
            {
                GameMgr.stageclear_cullentlove += Getlove_exp;
            }
        }

        //フリーモード時　満腹度の計算
        if (GameMgr.Story_Mode == 1)
        {
            compound_Main.ManpukuBarKoushin((int)(_basegirl1_manpuku + (total_score * 0.1f))); //満腹度も計算
            Debug.Log("満腹上昇値Base: " + _basegirl1_manpuku);
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

    //好感度が上がる・下がるときの処理。外部からアクセス用でもある。ゲージにも反映される。
    public void UpDegHeart(int _param, bool _sound)
    {
        //TimeControllerでのハート減る　重複防止用に、フラグをたてる。
        GameMgr.Degheart_on = true;

        //好感度取得
        Getlove_exp = _param;

        if (!heart_animON) //アニメ前
        {
            _tempGirllove = PlayerStatus.girl1_Love_exp;//あがる前の好感度を一時保存
            _tempResultGirllove = PlayerStatus.girl1_Love_exp + Getlove_exp; //あがった後の好感度を一時保存
            _lovecounter = _tempGirllove; //スライダ表示用のカウンター
            heart_animON = true;
        }
        else //アニメ中 計算用の_tempの値を変動させる。
        {
            _tempResultGirllove += Getlove_exp;
        }

        //好感度が0の場合、0が下限。
        if (_tempResultGirllove <= 0)
        {
            _tempResultGirllove = 0;
        }

        //好感度が9999の場合、9999が上限。
        if (_tempResultGirllove >= 9999)
        {
            _tempResultGirllove = 9999;
        }
    }

    void EndDegHeart()
    {
        //実際の好感度に値を反映
        PlayerStatus.girl1_Love_exp = _tempResultGirllove;
        GameMgr.stageclear_cullentlove += Getlove_exp;
        GameMgr.Degheart_on = false;
        heart_animON = false;

        //0以下になったら、下限は0
        if (PlayerStatus.girl1_Love_exp <= 0)
        {
            PlayerStatus.girl1_Love_exp = 0;
        }
        if (PlayerStatus.girl1_Love_exp >= 9999)
        {
            PlayerStatus.girl1_Love_exp = 9999;
        }
        if (GameMgr.stageclear_cullentlove <= 0)
        {
            GameMgr.stageclear_cullentlove = 0;
        }
        girl_param.text = PlayerStatus.girl1_Love_exp.ToString();
        girl_param.color = origin_color;

        Slider_Koushin(_tempResultGirllove); //念のため、スライダを再度更新
    }

    public void loveGetPlusAnimeON(int _Getlove_param, bool _mstatus) //ハート増える処理。通常は、お菓子食べたあとに発生。サブイベント終了後などからも、呼ばれる可能性あり。
    {
        _listHeart.Clear();
        _listHeartAtkeffect.Clear();
        _up_deg = 1f;

        //音を鳴らす
        sc.PlaySe(5);

        //ハート個数決定
        heart_count = _Getlove_param;

        //処理落ちするため、数が多くなりすぎないように調整。
        if (heart_count >= 300)
        {
            heart_count = 300;
            _up_deg = _Getlove_param / 300f;
        }

        //ハートのインスタンスを、獲得好感度分だけ生成する。
        for (i = 0; i < heart_count; i++)
        {
            if (GameMgr.Story_Mode == 0)
            {
                _listHeart.Add(Instantiate(heart_Prefab, _slider_obj.transform));
            }
            else
            {
                _listHeart.Add(Instantiate(heart_Prefab2, _slider_obj.transform));
            }
            _listHeart[i].GetComponent<HeartUpObj>()._id = i;
        }

        if (!heart_animON) //アニメ前
        {
            _tempGirllove = PlayerStatus.girl1_Love_exp;//あがる前の好感度を一時保存
            _tempResultGirllove = PlayerStatus.girl1_Love_exp + _Getlove_param; //あがった後の好感度を一時保存
            _lovecounter = _tempGirllove; //スライダ表示用のカウンター
            heart_animON = true;
            loveplusanimON = true;
        }
        else //アニメ中 計算用の_tempの値で計算する。アニメ終わりに、この値を実際のハートの値に反映させる。
        {
            _tempResultGirllove += Getlove_exp;
        }

        //好感度上限
        if (_tempResultGirllove >= girl1_status.stage1_lvTable[girl1_status.stage1_lvTable.Count - 1]) //カンスト
        {
            _tempResultGirllove = girl1_status.stage1_lvTable[girl1_status.stage1_lvTable.Count - 1];
        }


        //**
        //クリア判定
        //ステージクリアに必要なハート量がたまったかどうか。たまっていれば、SPクエストをクリアしたことになり、次のSPクエストが始まる。
        //仮にレベルがあがるか先に予測し、上がる場合は、sp_quest_clearのフラグをたてる。
        //☆ハートレベルが上がったらクリアする
        //**

        if (GameMgr.Story_Mode == 0)
        {
            if (_slider.value + _Getlove_param >= _slider.maxValue)
            {
                if (GameMgr.GirlLoveEvent_num == 50) //コンテストのときは、判定処理をなくしておく。
                {
                }
                else
                {
                    Debug.Log("ハートレベル上がったので、クエストクリア");
                    sp_quest_clear = true;
                }
            }
            else
            { }
        }


        //**　画面上に生成したハートがなくなるまでのコルーチン
        StartCoroutine("HeartKoushin");

        if (_mstatus)
        {
            GetLoveEnd();
        }
        else { }
    }

    //スライダバリューを正確に更新。現在の好感度数値をいれればOK。下限もレベルテーブルの値が入っている。5000~5100など。
    void Slider_Koushin(int cullent_value)
    {
        _slider.value = cullent_value;

        if (_slider.minValue > cullent_value)
        {
            if (cullent_value <= 0) //0より下回る場合は、何もしない。
            {

            }
            else
            {
                //ゲージの最小を下回った場合は、レベルが下がる。
                PlayerStatus.girl1_Love_lv--;
                Love_Slider_Setting();
            }
        }
    }

    //スライダマックスバリューを更新
    public void Love_Slider_Setting()
    {
        if (PlayerStatus.girl1_Love_lv <= 1)
        {
            _slider.minValue = 0;
        }
        else
        {
            _slider.minValue = stage_levelTable[PlayerStatus.girl1_Love_lv - 2];
        }

        if (PlayerStatus.girl1_Love_lv >= 99)
        {
            _slider.maxValue = 99999; //Lv99でカンストしたときは、Lv100のMaxがないので、適当な数字に。
        }
        else
        {
            _slider.maxValue = stage_levelTable[PlayerStatus.girl1_Love_lv - 1]; //レベルは１始まりなので、配列番号になおすため、-1してる
        }

        girl_lv.text = PlayerStatus.girl1_Love_lv.ToString();

    }

    //発生したハートが全てなくなったら、実際の好感度の変動と、表示も更新。 ハートが上がるアニメ中に、ハートが下がる可能性もある。（TimeControllerからの処理で）
    IEnumerator HeartKoushin()
    {
        while (heart_count > 0)
        {
            yield return null;
        }

        //ハートゲージ更新のアニメは終了
        heart_animON = false;
        loveplusanimON = false;

        //好感度　取得分増加
        if (PlayerStatus.girl1_Love_lv >= 99)
        {
            PlayerStatus.girl1_Love_exp = girl1_status.stage1_lvTable[girl1_status.stage1_lvTable.Count - 1];
        }
        else
        {
            PlayerStatus.girl1_Love_exp = _tempResultGirllove;
        }

        //テキストも更新
        girl_param.text = PlayerStatus.girl1_Love_exp.ToString();

        //念のため、スライダを再度更新
        Slider_Koushin(PlayerStatus.girl1_Love_exp);

        //リセット
        Getlove_exp = 0;

        //好感度によって発生するサブイベントがないかチェック
        GameMgr.check_GirlLoveSubEvent_flag = false;

        //エクストラモード　ハート判定
        HeartUpQuestBunkiCheck();
    }

    //ハートがゲージに衝突した時に、このメソッドが呼び出される。
    public void GetHeartValue()
    {
        if (PlayerStatus.girl1_Love_lv >= 99) //カンスト
        {
            girl_param.text = _tempResultGirllove.ToString();
        }
        else
        {
            
            _lovecounter += _up_deg; //love_counter自体は、floatで正確な上がり幅を計算し、表示用のカウンタでは、intにむりやりなおす。レベルチェックもlove_counterを基準に計算。

            //スライダにも反映
            _slider.value = (int)_lovecounter;
            girl_param.text = ((int)_lovecounter).ToString();

            //現在のスライダ上限に好感度が達したら、次のレベルへ。
            hlv_check = false;
            while (!hlv_check)
            {
                if (_lovecounter >= _slider.maxValue)
                {
                    PlayerStatus.girl1_Love_lv++;

                    //Maxバリューを再設定
                    Love_Slider_Setting();

                    //分かりやすくするように、レベルアップ時のパネルも表示
                    _listlvup_obj.Add(Instantiate(lvuppanel_Prefab, HeartLvUpPanel_obj.transform.Find("Viewport/Content").transform));

                    //覚えるスキルなどがないかチェック。あった場合、それもパネルに表示
                    exp_table.SkillCheckHeartLV(PlayerStatus.girl1_Love_lv, 1); //2番目が1だと、GirlEatJudgeから読むフラグ
                }
                else
                {
                    hlv_check = true;
                }
            }
        }

        //エフェクト
        _listHeartHit.Add(Instantiate(hearthit_Prefab, _slider_obj.transform.Find("Panel").gameObject.transform));
        _listHeartHit2.Add(Instantiate(hearthit2_Prefab, _slider_obj.transform.Find("Panel").gameObject.transform));

    }

    void GetLoveEnd()
    {

        //一時的に触れなくする。
        if (emerarudonguri_get)
        {
            Touch_WindowInteractOFF();
        }
        else
        {
            if (sp_quest_clear && !GameMgr.QuestClearButton_anim) //クエストクリアした際は、一連の演出をみせる。
            {
                Touch_WindowInteractOFF();

            }
            else
            {
                GameMgr.compound_status = 0;
            }

        }

        //エメラルどんぐり入るかチェック
        if (emerarudonguri_get)
        {
            emerarudonguri_get = false;

            emerarudonguri_end = false;
            StartCoroutine("EmeralDonguriEvent");
        }
        else
        {
            emerarudonguri_end = true;
        }

        //sp_quest_clearがたっていた場合、そのクエストはクリア。
        if (sp_quest_clear)
        {
            if (!GameMgr.QuestClearButton_anim)
            {
                Debug.Log("クエストクリア");
                StartCoroutine("QuestClearStart");
            }
            else
            {
                sp_quest_clear = false;

                Debug.Log("クエストクリア　クリアボタンはすでに登場　お菓子判定終了");
                StartCoroutine("EmeraldCheck_EndCompo"); //エメラルどんぐりイベントチェックしてから、お菓子判定終了

            }
        }
        else
        {
            //Debug.Log("クエスト未クリア　お菓子判定終了");
            StartCoroutine("EmeraldCheck_EndCompo"); //エメラルどんぐりイベントチェックしてから、お菓子判定終了
        }
    }

    void Touch_WindowInteractOFF()
    {
        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        //girl1_status.hukidasiOff();
        touch_controller.Touch_OnAllOFF();
        compound_Main.OffCompoundSelect();
        compound_Main.OnCompoundSelectObj();
        Extremepanel_obj.SetActive(false);
        hinttaste_toggle.SetActive(false);
    }

    //外部からクエストクリアしたかどうかをチェックする　compound_mainなどから読む。
    public void ExtraSPQuestClearCheck()
    {
        if (!GameMgr.QuestClearButton_anim)
        {
            HeartUpQuestBunkiCheck();
        }
    }

    IEnumerator EmeraldCheck_EndCompo()
    {
        //エメラルどんぐりイベントが発生した場合は、どんぐりが終了するまで待つ。emerarudonguri_end GameMgr.QuestClearflag
        while (!emerarudonguri_end)
        {
            yield return null;
        }

        emerarudonguri_end = false;

        //お菓子の判定処理を終了
        EndCompJudge();
        canvas.SetActive(true);
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

        GameMgr.emeralDonguri_status = emeraldonguri_status; //0=85点以上　1=100点以上
        GameMgr.emeralDonguri_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        switch (emeraldonguri_status)
        {
            case 0:

                //エメラルどんぐり一個もらえる。
                pitemlist.addPlayerItemString("emeralDongri", 1);
                //ついでに妹の体力が上がる。
                PlayerStatus.player_girl_maxlifepoint++;
                break;

            case 1:

                //エメラルどんぐり一個もらえる。
                pitemlist.addPlayerItemString("emeralDongri", 1);
                //サファイアどんぐり一個もらえる。
                pitemlist.addPlayerItemString("sapphireDongri", 1);
                //ついでに妹の体力が上がる。
                PlayerStatus.player_girl_maxlifepoint += 3;
                break;

            default:

                //エメラルどんぐり一個もらえる。
                pitemlist.addPlayerItemString("emeralDongri", 1);
                //ついでに妹の体力が上がる。
                PlayerStatus.player_girl_maxlifepoint++;
                break;

        }


        //はじめてエメラルどんぐりをゲットしたら、怪しげな館登場
        matplace_database.matPlaceKaikin("Emerald_Shop"); //怪しげな館解禁

        canvas.SetActive(true);
        emerarudonguri_end = true;
    }



    //ExpTableから読み出し
    public void LvUpPanel1(int _kaisu) //仕上げ回数あがった
    {
        _listlvup_obj.Add(Instantiate(lvuppanel_Prefab, HeartLvUpPanel_obj.transform.Find("Viewport/Content").transform));
        _listlvup_obj[_listlvup_obj.Count - 1].GetComponent<GirlLoveLevelUpPanel>().SelectPanel_2(_kaisu);
    }

    public void LvUpPanel2() //同時に2個仕上げできるようになった
    {
        _listlvup_obj.Add(Instantiate(lvuppanel_Prefab, HeartLvUpPanel_obj.transform.Find("Viewport/Content").transform));
        _listlvup_obj[_listlvup_obj.Count - 1].GetComponent<GirlLoveLevelUpPanel>().SelectPanel_3();
    }



    //
    //通常お菓子食べた直後の、おいしい・まずいといった感想。トッピングに対する感想もここで。
    //
    IEnumerator Girl_Comment()
    {

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
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
            if (GameMgr.GirlLoveEvent_num == 50 && contest_type == 0) //コンテストのときに「あげる」をおすと、こちらの処理
            {
                GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID;
            }
            else
            {
                //SPのお菓子でないものをあげた場合のコメント
                if (non_spquest_flag)
                {
                    GameMgr.OkashiComment_ID = 999;
                }
                else
                {
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
                            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + topping_flag_point; //クエストID+topping_flag(1~9)で指定する。ねこクッキーで、アイザン入りなら、1000+1で、1001。
                        }
                    }
                }
            }
        }

        girl1_status.hukidasiOff();

        GameMgr.OkashiComment_flag = true;
        GameMgr.OkashiComment_itemID = _baseID;
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

        //
        //①アフター感想
        //
        if (Mazui_flag)
        {
            GameMgr.okashiafter_ID = 999;
        }
        else
        {
            if (GameMgr.GirlLoveEvent_num == 50 && contest_type == 0) //コンテストのときに「あげる」をおすと、こちらの処理
            {
                OkashiAfterKansou_Setting();
            }
            else
            {
                if (!non_spquest_flag) //メインクエストの場合の感想
                {
                    GameMgr.okashiafter_status = 0;

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
                    OkashiAfterKansou_Setting();
                }
            }
        }

        Debug.Log("お菓子アフター感想スタート");
        GameMgr.okashiafter_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」
                                         //Debug.Log("レシピ: " + pitemlist.eventitemlist[recipi_num].event_itemNameHyouji);

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        //
        //②クエストクリアメッセージいれる
        //
        if (GameMgr.Story_Mode == 0)
        {
            if (total_score < GameMgr.low_score) //60点未満だと、クリアメッセージはでない。
            {

            }
            else
            {
                if (non_spquest_flag == false) //SPクエストのお菓子でないとこのメッセージはでない。
                {
                    if (!GameMgr.QuestClearCommentflag) //コメント一度だしたら、そのクエストで二回目以降はでない。
                    {
                        //高得点のときは、ここで特別スチルがでる。
                        if (total_score >= GameMgr.high_score)
                        {
                            if (!GameMgr.OkashiQuestHighScore_event[GameMgr.GirlLoveEvent_num])
                            {

                                switch (GameMgr.GirlLoveEvent_num)
                                {
                                    //特定のお菓子のみ、特殊イベント発生
                                    case 0: //クッキー

                                        break;

                                    case 2: //かわいいクッキー

                                        pitemlist.add_EmeraldPlayerItem(pitemlist.Find_emeralditemdatabase("Meid_Black_Costume"), 1);
                                        break;

                                    case 20: //クレープ

                                        pitemlist.addPlayerItem("yotuba_crown", 1);
                                        //イベントCG解禁
                                        GameMgr.SetEventCollectionFlag("event7", true);
                                        break;

                                    case 30: //シュークリーム

                                        //イベントCG解禁
                                        GameMgr.SetEventCollectionFlag("event8", true); //キスイベント
                                        break;

                                    case 40: //ドーナツ　白衣装ゲット

                                        pitemlist.add_EmeraldPlayerItem(pitemlist.Find_emeralditemdatabase("PinkGoth_Costume"), 1);
                                        break;
                                }

                                GameMgr.OkashiQuestHighScore_event[GameMgr.GirlLoveEvent_num] = true;

                                GameMgr.QuestClearButtonMessage_EvNum = girl1_status.OkashiQuest_ID + 10000;
                            }
                            else
                            {
                                GameMgr.QuestClearButtonMessage_EvNum = girl1_status.OkashiQuest_ID; //すでに高得点イベントが発生したら、通常感想。
                            }

                        }
                        else
                        {
                            GameMgr.QuestClearButtonMessage_EvNum = girl1_status.OkashiQuest_ID; //高得点でない場合、通常の感想。
                        }

                        sceneBGM.MuteBGM();
                        GameMgr.QuestClearButtonMessage_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」      

                        while (!GameMgr.recipi_read_endflag)
                        {
                            yield return null;
                        }

                        GameMgr.recipi_read_endflag = false;

                        sceneBGM.MuteOFFBGM();
                        canvas.SetActive(true);
                        ResetResult();
                        touch_controller.Touch_OnAllON();
                        GameMgr.QuestClearCommentflag = true;
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }
        else { } //エクストラモードはメッセージがでない。

        //
        //③ハート実際の獲得する演出
        //

        //満足度にあわせて音を鳴らす。
        if (total_score < GameMgr.low_score)　//60点以下
        {

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

        }

        if (Getlove_exp < 0) //ハートが下がる
        {
            UpDegHeart(Getlove_exp, true); //trueだと音あり

            //テキストウィンドウの更新
            exp_Controller.GirlDisLikeText(Getlove_exp);

            //女の子の表情をまずい表情にする。
            girl1_status.DefFaceChange();

            //お菓子の判定処理を終了
            EndCompJudge();

        }
        else //ハートが上昇
        {

            //アニメーションをON。好感度パラメータの反映もここ。
            loveGetPlusAnimeON(Getlove_exp, true);

            //エフェクト生成＋アニメ開始
            _listEffect.Add(Instantiate(effect_Prefab));
            StartCoroutine("Love_effect");

            //好感度パラメータに応じて、実際にキャラクタからハートがでてくる量を更新
            GirlHeartEffect.LoveRateChange();

            //表情も一瞬喜びに。
            StartCoroutine("GirlHeartUpYorokobiFace");
            GameMgr.QuestManzokuFace = true; //おいしかった～の表情
            girl1_status.QuestManzoku_counter = 10;

            //テキストウィンドウの更新
            exp_Controller.GirlLikeText(Getlove_exp, GetMoney, total_score);

            if (GetMoney > 0)
            {
                moneyStatus_Controller.GetMoney(GetMoney);
            }

            //吹き出しをだす。
            switch (dislike_status)
            {
                case 1: //デフォルトおいしい

                    if (total_score >= GameMgr.low_score)
                    {
                        //Debug.Log("吹き出しON");
                        //ランダムで、吹き出しの内容を決定
                        CommentTextInit();
                        random = Random.Range(0, _commentDict.Count);
                        _commentrandom = _commentDict[random];

                        girl1_status.hukidashiMessage(_commentrandom);

                        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
                    }
                    else //まずいときは吹き出しでない
                    {

                    }

                    break;

                //新しいお菓子をあげた場合の処理
                case 2:

                    girl1_status.hukidashiMessage(database.items[_baseID].itemNameHyouji + "うまいぞ！");

                    girl1_status.timeOut += 3.0f; //少し表示時間をのばす
                    break;

            }
        }


        canvas.SetActive(true);

        ResultOFF();
    }

    void OkashiAfterKansou_Setting()
    {
        //_baseSetjudge_num  ItemDBに登録された、お菓子ごとの固有の判定ナンバー
        GameMgr.okashiafter_ID = _baseSetjudge_num;
        GameMgr.okashiafter_status = 1;

        //特殊条件でセリフが変わる。
        if (_basename == "strange_tea" && total_score >= GameMgr.low_score) //へんな草ティーがおいしく作れた
        {
            GameMgr.okashiafter_ID = 5000;
        }
    }


    IEnumerator GirlHeartUpYorokobiFace()
    {

        girl1_status.face_girl_Yorokobi();

        yield return new WaitForSeconds(3.0f);

        girl1_status.DefFaceChange();
    }

    void NormalCommentEatBunki()
    {
        if (total_score < GameMgr.low_score)
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + 50; //スロットの感想 元のID+50番台~
        }
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + 51; //スロットの感想 元のID+50番台~
        }
        else
        {
            GameMgr.OkashiComment_ID = girl1_status.OkashiQuest_ID + 52; //スロットの感想 元のID+50番台~
        }
    }


    void NormalCommentAfterBunki()
    {
        if (total_score < GameMgr.low_score)
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + 50; //スロットの感想 元のID+50番台~
        }
        else if (total_score >= GameMgr.low_score && total_score < GameMgr.high_score)
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + 51; //スロットの感想 元のID+50番台~
        }
        else
        {
            GameMgr.okashiafter_ID = girl1_status.OkashiQuest_ID + 52; //スロットの感想 元のID+50番台~
        }
    }





    //
    //クエストボタン登場の演出
    //
    IEnumerator QuestClearStart()
    {
        //触れなくなる
        compound_Main.WindowOn(); //ウィンドウは表示
        Touch_WindowInteractOFF(); //その後触れなくする。

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        //girl1_status.hukidasiOff();
        touch_controller.Touch_OnAllOFF();

        //クエスト初クリア時に、最後に食べたお菓子をクリアお菓子として保存する。ItemIDとして保存。
        GameMgr.SpecialQuestClear_okashiItemID = GameMgr.Okashi_lastID;

        //エメラルどんぐりイベントが発生した場合は、どんぐりが終了するまで待つ。emerarudonguri_end GameMgr.QuestClearflag
        while (!emerarudonguri_end)
        {
            yield return null;
        }

        emerarudonguri_end = false;

        //全てのハートがなくなるまで待つ。
        while (heart_count > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        //演出処理　このタイミングで、「クエストクリア～！」みたいな演出？一時的に触れなくする。
        compound_Main.girlEat_ON = true;
        Touch_WindowInteractOFF();
        QuestClearEffectPanel.SetActive(true);
        sceneBGM.MuteBGM();

        while (!GameMgr.qclear_effect_endflag)
        {
            yield return null;
        }
        GameMgr.qclear_effect_endflag = false;
        //ここまで　演出処理

        canvas.SetActive(false);

        //表情も喜びの表情に。
        girl1_status.face_girl_Yorokobi();

        sp_quest_clear = false;

        if (GameMgr.QuestClearAnim_Flag) //ステージクリアの場合、クリアボタンも登場する演出
        {
            //ボタンが登場する演出
            StartCoroutine("ClearButtonAnim");
        }
        else //SPクエストクリアで、ボタン登場演出がない場合。次へ。
        {
            //お菓子の判定処理を終了
            //compound_Main.girlEat_ON = false;

            sceneBGM.MuteOFFBGM(); //ここのタイミングでBGMをオンに。

            GameMgr.QuestClearflag = true;
            QuestClearMethod(); //次のSPクエストを開始

            ResetResult();
        }

    }




    IEnumerator ClearButtonAnim()
    {
        //レベルアップパネルは一時オフ
        GameMgr.QuestClearButton_anim = true;
        for (i = 0; i < _listlvup_obj.Count; i++)
        {
            _listlvup_obj[i].SetActive(false);
        }

        canvas.SetActive(true);
        stageclear_panel.SetActive(true);
        stageclear_panel.transform.Find("ClearButton_Effect/SPClearTojoSe").GetComponent<PlayerRankTojoSE>().OnTriggerFlag(0);
        playableDirector.enabled = true;
        playableDirector.Play();

        sc.PlaySe(88); //キラキラ音　タイムラインだと音割れするのでスクリプトから

        yield return new WaitForSeconds(4.0f);

        playableDirector.enabled = false;
        playableDirector.time = 0;
        stageclear_Button.GetComponent<Toggle>().interactable = true;
        stageclear_panel.transform.Find("ClearButton_Effect").gameObject.SetActive(false);

        GameMgr.QuestClearflag = true;

        //お菓子の判定処理を終了
        EndCompJudge();
        ResetResult();

        sceneBGM.MuteOFFBGM(); //ここのタイミングでBGMをオンに。

        //表情をお菓子食べたあとの喜びの表情。
        girl1_status.face_girl_Fine();

        //まだレベルアップパネルステータス開いてたらONにする。
        for (i = 0; i < _listlvup_obj.Count; i++)
        {
            if (_listlvup_obj[i].GetComponent<GirlLoveLevelUpPanel>().OnPanelflag)
            {
                _listlvup_obj[i].SetActive(true);
            }
        }
    }



    //
    //クエストクリアトグルをおした場合に処理されるメソッド。このスクリプト自身と、CompoundMainから読まれる。
    //
    public void QuestClearMethod()
    {
        ClearFlagOn();

        //エクストラモードの場合は、クリア後にこのタイミングで感想～リザルト画面をいれる。
        //一度黒におとし、感想のシナリオ再生
        if (GameMgr.Story_Mode == 1)
        {
            StartCoroutine("ExtraQuestClearEvent");
        }

        //
        else
        {
            SelectNewOkashiSet();
        }
    }

    //
    //クエストクリア時の感想を表示する。
    //
    IEnumerator ExtraQuestClearEvent()
    {
        Debug.Log("エクストラクリア後の感想　シナリオ再生");

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();
        sceneBGM.MuteBGM();

        girl1_status.ResetCharacterPosition();

        GameMgr.KeyInputOff_flag = false;

        //yield return new WaitForSeconds(0.5f);

        GameMgr.scenario_ON = true;
        GameMgr.Extraquest_ID = girl1_status.OkashiQuest_ID; //
        GameMgr.ExtraClear_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;

        GameMgr.ExtraClear_QuestName = girl1_status.OkashiQuest_Name; //このタイミングで一度現クエストのクエストネームを保存
        GameMgr.ExtraClear_QuestNum = GameMgr.GirlLoveEvent_num;
        SelectNewOkashiSet();
    }

    //
    //次の食べたいお菓子を決めるメソッド。
    //
    void SelectNewOkashiSet()
    {
        //初期化 
        girl1_status.special_animatFirst = false;

        //次のお菓子クエストがあるかどうかをチェック。特定の条件を満たしていれば、ルートが分岐する。
        if (GameMgr.Okashi_quest_bunki_on == 0)
        {
            QuestBunki1();
        }
        else //1~の数字のときは、分岐開始
        {
            QuestBunki2();
        }
    }

    //
    //次クエスト指定
    //
    void QuestBunki1()
    {
        //次のクエストを指定。
        GameMgr.MesaggeKoushinON = false;

        if (GameMgr.NextQuestID % 100 != 0) //次クエが100番台以外
        {
            if (GameMgr.Story_Mode == 0)
            {
                special_quest.SetSpecialOkashiDict(GameMgr.NextQuestID, 0);

                EndSpQuest();
            }
            else
            {
                special_quest.SetSpecialOkashiDict(GameMgr.NextQuestID, 2);

                EndExtraQuest();
            }

        }

        else //次クエが100で割り切れる。次のSpお菓子へ
        {
            subQuestClear_check = true;
            GameMgr.QuestClearAnim_Flag = false; //次のメインクエストへ行くまえに、また演出はOFFに。
            ResultPanel_On();

        }

    }

    void QuestBunki2()
    {
        GameMgr.MesaggeKoushinON = false;

        if (GameMgr.Story_Mode == 0)
        {
            switch (GameMgr.GirlLoveEvent_num)
            {
                case 10: //ラスク　条件分岐

                    GameMgr.GirlLoveEvent_num = 13; //分岐の場合、直接次クエスト(specialquest.cs)をここで指定する。
                    break;

                    /*case 20:

                        GameMgr.GirlLoveEvent_num = 22;
                        break;*/
            }

            special_quest.SetSpecialOkashi(GameMgr.GirlLoveEvent_num, 0);

            EndSpQuest();
        }
        else
        {
            special_quest.SetSpecialOkashi(GameMgr.GirlLoveEvent_num, 2);

            EndExtraQuest();
        }
    }




    //
    //クエストの分岐するかどうかをチェック
    //
    void SPQuest_BunkiCheck()
    {
        if (GameMgr.Story_Mode == 0)
        {
            switch (GameMgr.GirlLoveEvent_num)
            {
                case 10: //ラスク　条件分岐

                    if (total_score >= GameMgr.low_score) //６０点以上で、ラスクレモンかラスクオレンジをあげる
                    {
                        if (_basename == "rusk_lemon" || _basename == "rusk_orange")
                        {
                            //すっぱいラスクを食べたので、次のクエストは別のおかしに。
                            GameMgr.Okashi_quest_bunki_on = 1;
                        }
                    }
                    break;

                    /*case 20: //クレープ　条件分岐

                        if (total_score >= 150) //クレープが150点以上
                        {
                                //次のクエストは別のおかしに。
                                GameMgr.Okashi_quest_bunki_on = 1;                    
                        }
                        break;*/
            }
        }
    }




    //
    //クエストクリア条件関係 こっちは食べたとき用。
    //
    void QuestClearJoukenCheck()
    {
        if (GameMgr.Story_Mode == 0)
        {
            switch (GameMgr.GirlLoveEvent_num)
            {
                default: //特殊なものがない限りは、デフォルト。60点以上でクリア

                    DefaultQuestJouken();

                    break;
            }
        }
        else
        {
            switch (GameMgr.GirlLoveEvent_num)
            {
                case 4: //230点以上のスーパークレープ　条件分岐

                    if (total_score >= 230)
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";

                        if (total_score >= 230 && total_score < 300)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 2;
                        }
                        else if (total_score >= 300 && total_score < 350)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 3;
                        }
                        else if (total_score >= 350)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 4;
                        }
                    }

                    break;

                case 10: //茶色いクッキー

                    if (_basename == "cocoa_cookie" && total_score >= GameMgr.low_score)
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";                       
                    }

                    //エクストラ獲得アイテムのランクも決定
                    ExtraItemGetRank();
                    break;

                case 13: //カミナリのようにすっぱいクレープ 酸味が100以上か、絶妙にすっぱいときのクレープ　すっぱすぎてもクリアできる

                    if (_baseitemtype_sub == "Crepe" && _basesour >= 85)
                    {

                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";

                        if (total_score >= 100 && total_score < 120)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 2;
                        }
                        else if (total_score >= 120 && total_score < 150)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 3;
                        }
                        else if (total_score >= 150)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 4;
                        }
                    }
                    break;

                case 14: //300点以上のいちごのクレープ

                    if (total_score >= 300)
                    {
                        if (_basename == "strawberry_crepe" || _basename == "strawberryblueberry_crepe" || _basename == "berry_crepe" || _basename == "doubleberry_crepe")
                        {
                            sp_quest_clear = true;
                            _windowtext.text = "満足しているようだ。";                           
                        }

                        if (total_score >= 300 && total_score < 400)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 2;
                        }
                        else if (total_score >= 400 && total_score < 500)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 3;
                        }
                        else if (total_score >= 500)
                        {
                            GameMgr.ExtraClear_QuestItemRank = 4;
                        }
                    }
                    break;

                case 21: //ムーディーな大人のおかし　カンノーリ　ティラミス　コーヒー　カフェオレシュー　ココアクッキー　ビスコッティ

                    if (total_score >= GameMgr.low_score)
                    {
                        if (_basename == "cannoli" || _basename == "tiramisu" || _basename == "sea_losanonos" || _basename == "cream_coffee"
                            || _basename == "cafeaulait_creampuff" || _basename == "cocoa_cookie" || _basename == "biscotti")
                        {
                            sp_quest_clear = true;
                            _windowtext.text = "満足しているようだ。";                          
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();

                    }
                    break;

                case 23: //200をこえる夢のようなパンケーキ

                    if (_baseitemtype_sub == "PanCake" && total_score >= 200 && _basebeauty >= 120)
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";
                    }

                    if (total_score >= 200 && total_score < 300)
                    {
                        GameMgr.ExtraClear_QuestItemRank = 2;
                    }
                    else if (total_score >= 300 && total_score < 400)
                    {
                        GameMgr.ExtraClear_QuestItemRank = 3;
                    }
                    else if (total_score >= 400)
                    {
                        GameMgr.ExtraClear_QuestItemRank = 4;
                    }
                    break;

                /*case 24: //300点超えのプリンセストータ

                    if (_basename == "princess_tota" && total_score >= 300)
                    {
                        sp_quest_clear = true;
                        _windowtext.text = "満足しているようだ。";
                        
                    }

                    if (total_score >= 300 && total_score < 350)
                    {
                        GameMgr.ExtraClear_QuestItemRank = 2;
                    }
                    else if (total_score >= 350 && total_score < 400)
                    {
                        GameMgr.ExtraClear_QuestItemRank = 3;
                    }
                    else if (total_score >= 400)
                    {
                        GameMgr.ExtraClear_QuestItemRank = 4;
                    }
                    break;*/

                default: //Extraモードでは、食べたいお菓子がランダムで変わるので、こちらは使用しない。

                    DefaultQuestJouken();

                    break;
            }
        }
    }

    //
    //クエストクリア条件　エクストラモード時ハート系　食べなくてもハートが上がったタイミングでクエストクリア判定する
    //
    void HeartUpQuestBunkiCheck()
    {
        if (GameMgr.Story_Mode != 0)
        {
            switch (GameMgr.GirlLoveEvent_num)
            {
                case 0:

                    if (PlayerStatus.girl1_Love_exp >= 100)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント Girl1_statusで、分岐
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 1:

                    if (PlayerStatus.girl1_Love_exp >= 300)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 2:

                    if (PlayerStatus.girl1_Love_exp >= 650)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 3:

                    if (PlayerStatus.girl1_Love_exp >= 1000)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 11:

                    if (GameMgr.ShopEvent_stage[10])
                    {
                        Debug.Log("＜エクストラ＞プリンさんにお菓子渡した。クエストクリア");
                        sp_quest_clear = true;

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 12:

                    /*
                    if (databaseCompo.Hikarimake_Totalcount() >= 3)
                    {
                        Debug.Log("＜エクストラ＞ヒカリがお菓子を3種類覚えたので、クエストクリア");
                        sp_quest_clear = true;

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }*/
                    if (PlayerStatus.girl1_Love_exp >= 2000)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }

                    break;

                case 20:

                    if (PlayerStatus.girl1_Love_exp >= 3000)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 22:

                    /*if (databaseCompo.Hikarimake_Totalcount() >= 10)
                    {
                        Debug.Log("＜エクストラ＞ヒカリがお菓子を10種類覚えたので、クエストクリア");
                        sp_quest_clear = true;

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }*/
                    if (PlayerStatus.girl1_Love_exp >= 4000)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }
                    break;

                case 24:

                    if (PlayerStatus.girl1_Love_exp >= 5000)
                    {
                        Debug.Log("＜エクストラ＞ハートが一定超えたので、クエストクリア");
                        sp_quest_clear = true;

                        if (GameMgr.Okashi_spquest_eatkaisu <= 3)
                        {
                            //3回以内だと、特別イベント
                            GameMgr.Okashi_Extra_SpEvent_Start = false;
                        }

                        //エクストラ獲得アイテムのランクも決定
                        ExtraItemGetRank();
                    }

                    break;
            }

            
            if (sp_quest_clear)
            {
                

                GetLoveEnd();
            }
        }
    }

    void ExtraItemGetRank()
    {
        if (GameMgr.Okashi_spquest_MaxScore < 100) //ランク１は使わないが、例外で、ハートのときなどに、お菓子をあげる前にクリアする可能性はあり。その場合は、１も通る。
        {
            GameMgr.ExtraClear_QuestItemRank = 1;
        }
        else if (GameMgr.Okashi_spquest_MaxScore >= 100 && GameMgr.Okashi_spquest_MaxScore < 200)
        {
            GameMgr.ExtraClear_QuestItemRank = 2;
        }
        else if (GameMgr.Okashi_spquest_MaxScore >= 200 && GameMgr.Okashi_spquest_MaxScore < 300)
        {
            GameMgr.ExtraClear_QuestItemRank = 3;
        }
        else if (GameMgr.Okashi_spquest_MaxScore >= 300)
        {
            GameMgr.ExtraClear_QuestItemRank = 4;
        }
        /*else if (GameMgr.Okashi_spquest_MaxScore >= 300)
        {
            GameMgr.ExtraClear_QuestItemRank = 5;
        }*/
    }


    void EndSpQuest()
    {       
        ResultOFF();

        //パラメータリセット
        EndFlagResetParam();       

        GameMgr.GirlLoveEvent_stage1[GameMgr.GirlLoveEvent_num] = true; //現在進行中のイベントをONにしておく。

        GameMgr.Okashi_quest_bunki_on = 0;

        //お菓子の判定処理を終了
        EndCompJudge();

        //ResultOFF()の後ろに設定しないと、hungry_statusがこんがらがる。
        girl1_status.timeGirl_hungry_status = 0;
        girl1_status.timeOut = 1.0f; //次クエストをすぐ開始
    }

    void EndExtraQuest()
    {

        //表示の音を鳴らす。
        sceneBGM.OnMainClearResultBGM();

        //エクストラクエストクリア時のリザルト画面表示
        canvas.SetActive(true);
        ExtraQuestOKPanel.SetActive(true);

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

    void ClearQuestName()
    {
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == _temp_count)
            {
                if (!GameMgr.high_score_flag) //通常クリア
                {
                    _mainquest_name = girlLikeCompo_database.girllike_composet[i].spquest_name2;
                }
                else //さらに高得点だったら、特別なイベントや報酬などが発生
                {
                    _mainquest_name = girlLikeCompo_database.girllike_composet[i].spquest_name3;
                }
            }
        }
    }

    //
    //スコア表示パネルを押したらこのメソッドが呼び出し
    //
    public void ResultPanel_On()
    {
        ScoreHyoujiPanel.SetActive(false);
        sc.PlaySe(2);

        if (subQuestClear_check)
        {
            //クリアフラッグをたてる。
            _temp_count = 0;
            while (_temp_count <= girl1_status.OkashiQuest_ID)
            {
                _temp_count += 100;
                if (_temp_count > girl1_status.OkashiQuest_ID)
                {
                    _temp_count -= 100;
                    break;
                }
            }

            switch (_temp_count)
            {
                case 1000: //クッキー系完了

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

                //エクストラ関係
                case 10000: //

                    GameMgr.OkashiQuest_flag_stage1[0] = true;

                    break;

                case 10100: //

                    GameMgr.OkashiQuest_flag_stage1[1] = true;

                    break;

                case 10200: //

                    GameMgr.OkashiQuest_flag_stage1[2] = true;

                    //3と4を飛ばしてコンテストへ
                    GameMgr.OkashiQuest_flag_stage1[3] = true;
                    GameMgr.OkashiQuest_flag_stage1[4] = true;
                    GameMgr.GirlLoveEvent_stage1[30] = true;
                    GameMgr.GirlLoveEvent_stage1[40] = true;

                    break;

                default:
                    break;
            }

            ClearQuestName();
            MainQuestText.text = _mainquest_name;

            if (GameMgr.Story_Mode == 0)
            {
                if (!GameMgr.high_score_flag) //通常クリア
                {
                    _set_MainQuestID = _temp_count;
                }
                else //ハイスコアクリア　直前で食べたお菓子で85~だせばフラグがたつ。
                {
                    _set_MainQuestID = _temp_count + 1;
                }
            }
            else
            {
                _set_MainQuestID = _temp_count;
            }

            StartCoroutine("MainQuestClearEvent");

        }
        else
        {
            if (!GameMgr.tutorial_ON)
            {
                //クエストまだクリアでなければ、お菓子の感想を表示する。
                StartCoroutine("OkashiAfter_Comment");
            }
            else //チュートリアル中の特殊処理
            {
                GameMgr.tutorial_Progress = true; //チュートリアル時、パネルを押したよ～のフラグ
                ResultOFF();

                //お菓子の判定処理を終了
                compound_Main.girlEat_ON = false;
                GameMgr.compound_status = 0;
            }
        }
    }

    //
    //クエストクリア時の感想を表示する。
    //
    IEnumerator MainQuestClearEvent()
    {

        girl1_status.GirlEat_Judge_on = false;
        girl1_status.WaitHint_on = false;
        girl1_status.hukidasiOff();
        canvas.SetActive(false);
        touch_controller.Touch_OnAllOFF();
        sceneBGM.MuteBGM();

        girl1_status.ResetCharacterPosition();

        GameMgr.KeyInputOff_flag = false;
        GameMgr.scenario_ON = true;
        GameMgr.mainquest_ID = _set_MainQuestID; //
        GameMgr.mainClear_flag = true; //->宴の処理へ移行する。「Utage_scenario.cs」

        //イベントスチル解禁
        switch (GameMgr.mainquest_ID)
        {
            case 1001: //うまいぞ！にいちゃんのクッキー

                //イベントCG解禁
                GameMgr.SetEventCollectionFlag("event4", true);
                break;

            case 1101: //ラスクとありんこ

                //イベントCG解禁
                GameMgr.SetEventCollectionFlag("event5", true);
                break;

            case 1401: //やもりのねがいごと

                //イベントCG解禁
                GameMgr.SetEventCollectionFlag("event6", true);
                break;

        }

        while (!GameMgr.recipi_read_endflag)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = false;
        //sceneBGM.MuteOFFBGM();

        canvas.SetActive(true);
        stageclear_Button.SetActive(false);

        GameMgr.QuestClearButton_anim = false;

        //表示の音を鳴らす。
        sceneBGM.OnMainClearResultBGM();
        //sc.PlaySe(19);　//前は、25, 47

        MainQuestOKPanel.SetActive(true);
    }

    public void ResultOFF() //Debug_Panelからも読み出し。フェードアウトなどの処理がない。
    {
        MainQuestOKPanel.SetActive(false);
        subQuestClear_check = false;

        ResetResult();
    }

    //メインクエストOKパネルを閉じたあと
    public void PanelResultOFF()
    {
        BlackPanel_event.SetActive(true);
        MainQuestOKPanel.SetActive(false);
        ExtraQuestOKPanel.SetActive(false);
        ExtraQuest_TreasurePanel.SetActive(false);
        sc.PlaySe(18); //閉じる音
        sceneBGM.OnMainClearResultBGMOFF();

        //パラメータリセット
        EndFlagResetParam();

        //黒で一度フェードアウト
        sceneBGM.MuteBGM();
        Fadeout_Black_obj.GetComponent<FadeOutBlack>().FadeIn();
        //Debug.Log("## 一度ブラックアウト　演出 ##");
        StartCoroutine("Black_FadeOut");
    }

    void EndFlagResetParam()
    {
        subQuestClear_check = false;
        GameMgr.QuestManzokuFace = false;
        GameMgr.QuestClearflag = false; //ボタンをおすとまたフラグをオフに。
        GameMgr.QuestClearButton_anim = false;
        GameMgr.QuestClearCommentflag = false;
        GameMgr.stageclear_cullentlove = 0;
        GameMgr.Okashi_spquest_eatkaisu = 0;
        GameMgr.Okashi_spquest_MaxScore = 0;
    }

    IEnumerator Black_FadeOut()
    {
        yield return new WaitForSeconds(1.0f);

        BlackPanel_event.SetActive(false);
        canvas.SetActive(false);      
        Fadeout_Black_obj.GetComponent<FadeOutBlack>().FadeOut();

        yield return new WaitForSeconds(1.0f);

        GameMgr.check_GirlLoveEvent_flag = false; //好感度によって発生するイベントがないかチェックする 
        GameMgr.questclear_After = true;
        ResetResult();
    }

    void ResetResult()
    {
        //お菓子をあげたあとの状態に移行する。
        girl1_status.timeGirl_hungry_status = 2;
        girl1_status.timeOut = 1.0f;
        girl1_status.GirlEat_Judge_on = true;//またカウントが進み始める
        
        //初期化
        for (i = 0; i < _listScoreEffect.Count; i++)
        {
            Destroy(_listScoreEffect[i]);
        }
        _listScoreEffect.Clear();
      
        GameMgr.scenario_ON = false;

    }

    void EndCompJudge() //女の子の判定終了のフラグ系
    {
        //お菓子の判定処理を終了
        compound_Main.girlEat_ON = false;
        GameMgr.compound_status = 0;

        GameMgr.check_GirlLoveSubEvent_flag = false; //サブイベントが発生するかをチェック
        GameMgr.check_OkashiAfter_flag = true; //食べた直後～、というフラグ
    }

    //食べたあとのヒント関係の処理
    void SetHintText(int _hintstatus)
    {      
        //ヒントを表示する。０のものは、判定なしなので、表示もしない。
        SweatHintHyouji();
        BitterHintHyouji();
        SourHintHyouji();       
        ShokukanHintHyouji();

        //クエストクリアの条件を満たしていない場合、
        //そのクエストクリアに必要な固有のヒントをくれる。（クッキーのときは、「もっとかわいくして！」とか。妹が好みのものを伝えてくる。）
        _temp_spkansou = "";
        _special_kansou = "";
        tpcheck_utageON = false;
        tpcheck = false;
        no_hint = true; //デフォルトでは、ヒントを出さない。
        tpcheck_utagebunki = 0;

        //トッピングがのってないときのヒント Excelにデフォルトのヒントがある。このスクリプトで直接入力してもOK。ヒント必要ない場合は、後述のスクリプトで空になる。
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == girl1_status.OkashiQuest_ID)
            {
                _temp_spkansou = girlLikeCompo_database.girllike_composet[i].hint_text;
            }
        }
        _special_kansou = _temp_spkansou;

        if (!non_spquest_flag)
        {
            //条件判定
            switch (girl1_status.OkashiQuest_ID)
            {
                case 1020: //クッキー１　クッキーまでは合っているが、かわいいトッピングがのってないとき

                    if (total_score < GameMgr.low_score) //60点以下だった
                    {
                        if (topping_all_non && !topping_flag) //好みのトッピングはある(toppingu_all_non=true)が、一つものってなかった場合(topping_flag=false)だった
                        {
                            no_hint = false;
                            tpcheck_utageON = true;
                        }
                        else
                        {
                            //ヒントはなし
                        }
                    }
                    break;

                case 1110: //ラスク２　すっぱいトッピングがのってないとき

                    if (topping_all_non && !topping_flag) //好みのトッピングはあるが、一つものってなかった場合
                    {
                        no_hint = false;
                        tpcheck_utageON = true;
                    }
                    else
                    {
                        //ヒントはなし
                    }
                    break;

                case 1200: //クレープ１　ホイップクリームがのってなかった時　tp_check=false

                    if (_baseitemtype_sub == "Crepe_Mat")
                    {
                        no_hint = false;
                        tpcheck_utageON = true;
                    }

                    //特定のトッピングスロットをみる
                    /*tpcheck_slot = "WhipeedCream";
                    ToppingCheck();

                    if (tpcheck) //ホイップがちゃんとのっていた。
                    {
                        //ヒントはなし
                    }
                    else
                    {
                        no_hint = false;
                        tpcheck_utageON = true;
                    }*/

                    break;

                case 1230: //豪華なクレープ

                    //30点以下の場合、ヒントがでる。
                    //特定のトッピングスロットをみる
                    tpcheck_slot = "Blackcurrant";
                    ToppingCheck();
                    if (tpcheck) //カシスは外れ
                    {
                        no_hint = false;
                        _special_kansou = "この黒い粒、すっぱすぎるよ・・。にいちゃん。";
                    }
                    else
                    {
                        if (beauty_score <= -30)
                        {
                            no_hint = false;
                            _special_kansou = GameMgr.ColorRedDeep + "ぜんぜん豪華じゃない。" + "</color>";
                        }
                        else if (beauty_score > -30 && beauty_score <= -10)
                        {

                            no_hint = false;
                            _special_kansou = "もう少し、トッピングがほしいかも？";

                        }
                        else if (beauty_score >= -10 && beauty_score <= 20)
                        {

                            no_hint = false;
                            _special_kansou = "よい豪華さ！";

                        }
                        else
                        {

                            no_hint = false;
                            _special_kansou = "神の華やかさ！";

                        }
                    }

                    break;

                case 1300: //シュークリーム１　ホイップクリームがのってなかった時　tp_check=false

                    //マフィンと間違えたとき
                    if (_baseitemtype_sub == "Maffin")
                    {
                        no_hint = false;
                        tpcheck_utageON = true;
                        tpcheck_utagebunki = 1;
                    }
                    else
                    {
                        //トッピングスロットをみる
                        if (_basename == "puff")
                        {
                            tpcheck = false; //ホイップがない
                        }
                        else
                        {
                            tpcheck = true; //trueにすればチェックOK
                        }
                        /*tpcheck_slot = "WhipeedCream";
                        ToppingCheck();

                        tpcheck_slot = "WhipeedCreamStrawberry";
                        ToppingCheck();*/

                        if (tpcheck) //なにかしらのホイップがのっていた
                        {

                        }
                        else
                        {
                            no_hint = false;
                            tpcheck_utageON = true;
                            tpcheck_utagebunki = 0;
                        }
                    }

                    break;

                case 1400: //ドーナツ１　ストロベリーホイップクリームがのってなかった時　tp_check=false

                    //トッピングスロットをみる
                    tpcheck_slot = "WhipeedCreamStrawberry";
                    ToppingCheck();

                    tpcheck_slot = "WhipeedCreamRaspberry";
                    ToppingCheck();

                    tpcheck_slot = "WhipeedCreamCherry";
                    ToppingCheck();

                    if (!tpcheck) //ストロベリークリームはのっていなかった。
                    {
                        tpcheck_slot = "Strawberry"; //次にすとろべりーを調べる
                        ToppingCheck();

                        if (tpcheck) //いちごはのってた場合。惜しい。
                        {
                            no_hint = false;
                            tpcheck_utageON = true;
                            tpcheck_utagebunki = 1;
                            _special_kansou = "にいちゃん。クリームも、ピンク色だったかも？";
                        }
                        else //ストロベリークリームも、いちごものってなかった
                        {
                            no_hint = false;
                            tpcheck_utageON = true;
                        }
                    }

                    break;

                case 10130: //めちゃすっぱいクレープ１　すっぱさが足りてないとき

                    if (_basesour <= 85)
                    {
                        no_hint = false;
                        tpcheck_utageON = true;
                        tpcheck_utagebunki = 0;
                    }

                    break;

                default:

                    break;
            }

        }
        else //クエスト以外のお菓子をあげたときの感想・ヒント。そもそもお菓子を間違えている場合。
        {

            //条件判定
            switch (girl1_status.OkashiQuest_ID)
            {
                case 1300: //シュークリーム１

                    //マフィンと間違えたとき
                    if (_baseitemtype_sub == "Maffin")
                    {
                        no_hint = false;
                        tpcheck_utageON = true;
                        tpcheck_utagebunki = 1;
                    }
                    break;
            }

        }

        //
        if (!no_hint) //falseのときは、ヒントだす。
        {
            if (tpcheck_utageON) //宴でもヒント表示するか否か。宴で表示する場合、他の宴イベントのあとに表示される。
            {
                GameMgr.okashinontphint_flag = true;
                GameMgr.okashinontphint_ID = girl1_status.OkashiQuest_ID + tpcheck_utagebunki;
            }
        }
        else
        {
            _special_kansou = "";
        }

        //クエスト以外で新しいお菓子をあげたときの、感想（共通）
        if (!non_spquest_flag)
        { }
        else
        {
            if (dislike_status == 2)
            {
                _special_kansou = "今まで食べたことないお菓子だ！";
            }
            else
            {
                _special_kansou = "";
            }
        }

        _special_kansou = "\n" + _special_kansou;

        //感想の表示　_statusが0なら、通常得点、1なら、高得点時（85点以上）の感想。2は、まずすぎたとき。
        switch (_hintstatus)
        {

            case 0:

                temp_hint_text = _shokukan_kansou + _sweat_kansou + _bitter_kansou + _sour_kansou + _special_kansou;                
                break;

            case 1:

                temp_hint_text = _shokukan_kansou + _sweat_kansou + _bitter_kansou + _sour_kansou + "\n" + "この" + _basenameHyouji + "うますぎィ！" + "最高！！";
                break;

            case 2:

                temp_hint_text = "マズすぎるぅ..。" + "\n" + _shokukan_kansou + _sweat_kansou + _bitter_kansou + _sour_kansou + _special_kansou;
                break;

            case 3: //粉っぽいなどのとき

                switch(dislike_num)
                {
                    case 0: //粉っぽい

                        temp_hint_text = "粉っぽいよ..。にいちゃん。" + "\n" + _shokukan_kansou + _sweat_kansou + _bitter_kansou + _sour_kansou + _special_kansou;
                        break;

                    case 1: //油っぽい

                        temp_hint_text = "あぶらっこいよ..。にいちゃん。" + "\n" + _shokukan_kansou + _sweat_kansou + _bitter_kansou + _sour_kansou + _special_kansou;
                        break;

                    case 2: //水っぽい　粉＜油＜水の順

                        temp_hint_text = "水っぽいよ..。にいちゃん。" + "\n" + _shokukan_kansou + _sweat_kansou + _bitter_kansou + _sour_kansou + _special_kansou;
                        break;
                }
                break;

            default:
                break;
        }

        //temp_hint_text = "◆妹からのヒント◆" + "\n" + temp_hint_text;

        //if (last_score_kousin)
        //{
            database.items[_baseID].last_hinttext = temp_hint_text;
            GameMgr.Okashi_lasthint = temp_hint_text;
            GameMgr.Okashi_lastname = _basenameHyouji;
            GameMgr.Okashi_lastslot = _basenameSlot;
            GameMgr.Okashi_lastID = _baseID;
            GameMgr.Okashi_lastshokukan_param = shokukan_baseparam;
            GameMgr.Okashi_lastshokukan_mes = shokukan_mes;
            GameMgr.Okashi_lastsweat_param = _basesweat;
            GameMgr.Okashi_lastsour_param = _basesour;
            GameMgr.Okashi_lastbitter_param = _basebitter;
        //}
    }

    void ToppingCheck()
    {
        for (i = 0; i < _basetp.Length; i++)
        {
            if (_basetp[i] == tpcheck_slot) //キーと一致するアイテムスロットがあれば、点数を+1
            {
                tpcheck = true;
            }
        }

        //固有トッピングスロットも見る。
        for (i = 0; i < _koyutp.Length; i++)
        {
            if (_koyutp[i] == tpcheck_slot) //キーと一致するアイテムスロットがあれば、点数を+1
            {
                tpcheck = true;
            }
        }
    }

    void SweatHintHyouji()
    {
        //甘さがどの程度好みにあっていたかを、感想でいう。８はピッタリパーフェクト。
        if (sweat_level == 8)
        {
            _sweat_kansou = GameMgr.ColorGold + "甘さ S: 神の甘さ！ パーフェクト！！" + "</color>";
            _contest_sweat_kansou = "甘さ S: ほほう..これは。すばらしい甘みです。パーフェクトですね！高得点をつけましょう。";
            _shopgirl_sweat_kansou = "甘さ S: すばらしい甘味だったわ！このお茶にはピッタリね♪。";
        }
        else if (sweat_level == 7)
        {
            _sweat_kansou = GameMgr.ColorPink + "甘さ A+: 絶妙な甘さ！" + "</color>";
            _contest_sweat_kansou = "甘さ A+: 絶妙で、神レベルの甘みで良いです。これは高得点をつけざるを得ない。";
            _shopgirl_sweat_kansou = "甘さ A+: 絶妙な甘みね。とてもお茶に合ってると思うわ。";
        }
        else if (sweat_level == 6)
        {
            _sweat_kansou = "甘さ A: 甘さ、かなり近い！";
            _contest_sweat_kansou = "甘さ A: この甘さは、バランスがとれてて良い具合ですね。";
            _shopgirl_sweat_kansou = "甘さ A: よい甘味だったわ。バランスがとれてて、お茶にぴったりよ♪";
        }
        else if (sweat_level == 5)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = "甘さ B: 甘さがちょっと足りない";
                _contest_sweat_kansou = "甘さ B: 甘さが、すこし足りないようで、ちょっと物足りなかったですね。";
                _shopgirl_sweat_kansou = "甘さ B: 甘さがもう少しあると、いい感じかも！";
            }
            else
            {
                _sweat_kansou = "甘さ B: 少し甘いかも？";
                _contest_sweat_kansou = "甘さ B: 少し甘さが強かったようで、後味が少々くどいです。";
                _shopgirl_sweat_kansou = "甘さ B: 少し甘味が強かった..かな？";
            }
        }
        else if (sweat_level == 4)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = "甘さ C: 甘さがちょっと足りない";
                _contest_sweat_kansou = "甘さ C: 甘さが、すこし足りないようで、ちょっと物足りなかったですね。";
                _shopgirl_sweat_kansou = "甘さ C: もう少し甘味がほしいかな～。";
            }
            else
            {
                _sweat_kansou = "甘さ C: 少し甘いかも？";
                _contest_sweat_kansou = "甘さ C: 少し甘さが強かったようで、後味が少々くどいです。";
                _shopgirl_sweat_kansou = "甘さ C: 少し甘すぎたみたい。";
            }
        }
        else if (sweat_level == 3)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = GameMgr.ColorRedDeep + "甘さ D: 甘さがもっと欲しい" + "</color>";
                _contest_sweat_kansou = "甘さ D: 甘さがもっとほしいですね。これではお菓子として物足りないです。";
                _shopgirl_sweat_kansou = "甘さ D: もっと甘さがでないかしら。";
            }
            else
            {
                _sweat_kansou = GameMgr.ColorRedDeep + "甘さ D: 甘さがちょっと強すぎ" + "</color>";
                _contest_sweat_kansou = "甘さ D: 甘さが少し、強すぎたでしょうか。";
                _shopgirl_sweat_kansou = "甘さ D: ちょっと甘さが強すぎたみたいね。";
            }
        }
        else if (sweat_level >= 1 && sweat_level <= 2)
        {
            if (sweat_result < 0)
            {
                _sweat_kansou = GameMgr.ColorRedDeep + "甘さ F: 甘さが全然足りない" + "</color>";
                _contest_sweat_kansou = "甘さ F: 甘さが全然足りていないですね..。もう少し甘さを足しましょう。";
                _shopgirl_sweat_kansou = "甘さ F: 甘さがまったくないのが、ちょっと..。";
            }
            else
            {
                _sweat_kansou = GameMgr.ColorRedDeep + "甘さ F: 甘すぎ" + "</color>";
                _contest_sweat_kansou = "甘さ F: 甘すぎですね..。砂糖を入れすぎのようです。";
                _shopgirl_sweat_kansou = "甘さ F: これは甘すぎみたい。";
            }
        }
        else
        {
            _sweat_kansou = "";
            _contest_sweat_kansou = "※甘さについて、感想なし";
            _shopgirl_sweat_kansou = "※甘さについて、感想なし";
        }

        if (sweat_level != 0)
        {
            _sweat_kansou = "\n" + _sweat_kansou;
        }
        else
        {
            _sweat_kansou = "";
        }
    }

    void BitterHintHyouji()
    {
        //苦さがどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (bitter_level == 8)
        {
            _bitter_kansou = GameMgr.ColorGold + "苦さ S: 神の苦さ！ パーフェクト！！" + "</color>";
            _contest_bitter_kansou = "苦さ S: この苦み.. パーフェクトです。まさしく神の苦み・・！　";
            _shopgirl_bitter_kansou = "苦さ S: この苦み.. すばらしい神の苦みね！　ピッタリだわ♪";
        }
        else if (bitter_level == 7)
        {
            _bitter_kansou = GameMgr.ColorPink + "苦さ A+: 絶妙な苦さ！" + "</color>";
            _contest_bitter_kansou = "苦さ A+: 絶妙な苦さですね！　まさしく神レベル。これは高得点をつけざるを得ません。";
            _shopgirl_bitter_kansou = "苦さ A+: 絶妙な苦みで.. よい味わいだわ！";
        }
        else if (bitter_level == 6)
        {
            _bitter_kansou = "苦さ A: 苦さ、かなり近い！";
            _contest_bitter_kansou = "苦さ A: 苦さほどよく、かなり良い具合です。もう一味、苦さが近くなれば..パーフェクトでしょう。";
            _shopgirl_bitter_kansou = "苦さ A: 苦みがかなりほどよくて、いい味わいだわ。もう少し苦みに工夫すれば..パーフェクトね。";
        }
        else if (bitter_level == 5)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = "苦さ B: 苦さがちょっと足りない";
                _contest_bitter_kansou = "苦さ B: 苦さが少し足りない感じですね。";
                _shopgirl_bitter_kansou = "苦さ B: あと少し、苦みがほしいかも。";
            }
            else
            {
                _bitter_kansou = "苦さ B: 少し苦いかも？";
                _contest_bitter_kansou = "苦さ B: うぅん..。少し苦みが強かったですね。";
                _shopgirl_bitter_kansou = "苦さ B: 少し苦さが強かったかも。でも、いい味わいよ♪";
            }
        }
        else if (bitter_level == 4)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = "苦さ C: 苦さがちょっと足りない";
                _contest_bitter_kansou = "苦さ C: 苦さが少し足りない感じですね。";
                _shopgirl_bitter_kansou = "苦さ C: 苦さが、もう少し欲しいわね。";
            }
            else
            {
                _bitter_kansou = "苦さ C: 少し苦いかも？";
                _contest_bitter_kansou = "苦さ C: うぅん..。少し苦みが強かったですね。";
                _shopgirl_bitter_kansou = "苦さ C: 少し苦みが強かったみたい。";
            }

        }
        else if (bitter_level == 3)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = GameMgr.ColorRedDeep + "苦さ D:苦さがもっと欲しい" + "</color>";
                _contest_bitter_kansou = "苦さ D: 苦さがもっと欲しいですね。少し大人なアクセントが効いてない感じです。";
                _shopgirl_bitter_kansou = "苦さ D: 苦さが、もっと欲しいわね。";
            }
            else
            {
                _bitter_kansou = GameMgr.ColorRedDeep + "苦さ D: 苦みが少し強すぎかも。" + "</color>";
                _contest_bitter_kansou = "苦さ D: 苦みが少し強すぎのようですね。これでは、お菓子本来の味が消えてしまいます。";
                _shopgirl_bitter_kansou = "苦さ D: 苦さが、強すぎみたいね。";
            }

        }
        else if (bitter_level >= 1 && bitter_level <= 2)
        {
            if (bitter_result < 0)
            {
                _bitter_kansou = GameMgr.ColorRedDeep + "苦さ F: 苦さが全然足りない" + "</color>";
                _contest_bitter_kansou = "苦さ F: 苦さが全然足りていないようです。もっと苦みは欲しいところですね。";
                _shopgirl_bitter_kansou = "苦さ F: 苦さが、全然足りないかも..。もっと苦みがでないかしら。";
            }
            else
            {
                _bitter_kansou = GameMgr.ColorRedDeep + "苦さ F: 苦すぎ..。" + "</color>";
                _contest_bitter_kansou = "苦さ F: これは苦すぎですね..。..ぐふ。";
                _shopgirl_bitter_kansou = "苦さ F: これは苦すぎね..。";
            }

        }
        else
        {
            _bitter_kansou = "";
            _contest_bitter_kansou = "※苦さについて、感想なし";
            _shopgirl_bitter_kansou = "※苦さについて、感想なし";
        }

        if (bitter_level != 0)
        {
            _bitter_kansou = "\n" + _bitter_kansou;
        }
        else
        {
            _bitter_kansou = "";
        }
    }

    void SourHintHyouji()
    {
        
        //酸味がどの程度好みにあっていたかを、感想でいう。７はピッタリパーフェクト。
        if (sour_level == 8)
        {
            _sour_kansou = GameMgr.ColorGold + "酸味 S: 神のすっぱさ！ パーフェクト！！" + "</color>";
            _contest_sour_kansou = "酸味 S: ううん！この強烈に舌に残りつつも後味はさらりと消えて..。神の酸っぱさですね！！";
            _shopgirl_sour_kansou = "酸味 S: ううん！酸味がいい具合に効いて..　風味がベストマッチだわ！神の酸っぱさね！";
        }
        else if (sour_level == 7)
        {
            _sour_kansou = GameMgr.ColorPink + "酸味 A+: 絶妙なすっぱさ！" + "</color>";
            _contest_sour_kansou = "酸味 A+: 絶妙なすっぱさで..　まさしく神レベル。これは高得点をつけざるを得ません。";
            _shopgirl_sour_kansou = "酸味 A+: あぁん！　絶妙な酸味.. 体の疲れがとれてきたわ！";
        }
        else if (sour_level == 6)
        {
            _sour_kansou = "酸味 A: すっぱさ、かなり近い！";
            _contest_sour_kansou = "酸味 A: すっぱさ、かなり良い塩梅です。一味効いてますね。";
            _shopgirl_sour_kansou = "酸味 A: あら！ほどよい酸味で、香りとよく調和してるわね。すばらしいわ！";
        }
        else if (sour_level == 5)
        {
            if (sour_result < 0)
            {
                _sour_kansou = "酸味 B: すっぱさちょっと足りない";
                _contest_sour_kansou = "酸味 B: すっぱさが、もう少し入れてもよいかもですね。";
                _shopgirl_sour_kansou = "酸味 B: 酸味があと少し効いたら.. 最高ね！";
            }
            else
            {
                _sour_kansou = "酸味 B: 少しすっぱいかも？";
                _contest_sour_kansou = "酸味 B: 少しすっぱさが強かったですね。他の味にまで影響がでそうです。";
                _shopgirl_sour_kansou = "酸味 B: ほんの少し、酸味が強かったかしら。でもいいお味ね。";
            }
        }
        else if (sour_level == 4)
        {
            if (sour_result < 0)
            {
                _sour_kansou = "酸味 C: すっぱさちょっと足りない";
                _contest_sour_kansou = "酸味 C: すっぱさが、もう少し入れてもよいかもですね。";
                _shopgirl_sour_kansou = "酸味 C: もう少し酸味が欲しいかも。";
            }
            else
            {
                _sour_kansou = "酸味 C: 少しすっぱいかも？";
                _contest_sour_kansou = "酸味 C: 少しすっぱさが強かったですね。他の味にまで影響がでそうです。。";
                _shopgirl_sour_kansou = "酸味 C: 少しすっぱさが強かったかしら。";
            }

        }
        else if (sour_level == 3)
        {
            if (sour_result < 0)
            {
                _sour_kansou = GameMgr.ColorRedDeep + "酸味 D: すっぱさがもっと欲しい" + "</color>";
                _contest_sour_kansou = "酸味 D: すっばさがもっと欲しいところです。少々平凡な味わいです。";
                _shopgirl_sour_kansou = "酸味 D: そうね。もっとすっぱさが欲しいとこね！";
            }
            else
            {
                _sour_kansou = GameMgr.ColorRedDeep + "酸味 D: 少しすっぱ過ぎる？" + "</color>";
                _contest_sour_kansou = "酸味 D: これは、かなりすっぱいですね。う～ん..。";
                _shopgirl_sour_kansou = "酸味 D: これは、かなりすっぱいわね..。";
            }

        }
        else if (sour_level >= 1 && sour_level <= 2)
        {
            if (sour_result < 0)
            {
                _sour_kansou = GameMgr.ColorRedDeep + "酸味 F: 全然すっぱさがない" + "</color>";
                _contest_sour_kansou = "酸味 F: すっぱさが全然ないですね..。これでは、味の印象が残らず、くどすぎてしまいます。";
                _shopgirl_sour_kansou = "酸味 F: 酸味がぜんぜんないわね。もっとすっぱさをだせるかしら。";
            }
            else
            {
                _sour_kansou = GameMgr.ColorRedDeep + "酸味 F: すっぺぇ..。" + "</color>";
                _contest_sour_kansou = "酸味 F: すっぱすぎですね..。材料を見直してみましょう。";
                _shopgirl_sour_kansou = "酸味 F: すっぱすぎね..。";
            }

        }
        else
        {
            _sour_kansou = "";
            _contest_sour_kansou = "※酸味について、感想なし"; //コンテスト用
            _shopgirl_sour_kansou = "※酸味について、感想なし";
        }

        

        if (sour_level != 0)
        {
            _sour_kansou = "\n" + _sour_kansou;
        }
        else
        {
            _sour_kansou = "";
        }
    }

    void ShokukanHintHyouji()
    {
        //食感に関するヒント
        if (shokukan_score < 20) //
        {
            _shokukan_kansou = GameMgr.ColorRedDeep + "食感 F: " + shokukan_mes + "が全然足りない..。" + "</color>";
            _shopgirl_shokukan_kansou = shokukan_mes + "が全然足りてないわね..。";
        }
        else if (shokukan_score >= 20 && shokukan_score < 40) //
        {
            _shokukan_kansou = GameMgr.ColorRedDeep + "食感 C: " + shokukan_mes + "がもっとほしい" + "</color>";
            _shopgirl_shokukan_kansou = shokukan_mes + "がもっとほしいかも。";
        }
        else if (shokukan_score >= 40 && shokukan_score < GameMgr.low_score) //
        {
            _shokukan_kansou = "食感 B: " + "まあまあの" + shokukan_mes;
            _shopgirl_shokukan_kansou = "ほどよい" + shokukan_mes + "ね。";
        }
        else if (shokukan_score >= GameMgr.low_score && shokukan_score < GameMgr.high_score) //
        {
            _shokukan_kansou = "食感 A: " + "良い" + shokukan_mes;
            _shopgirl_shokukan_kansou = "よい" + shokukan_mes + "ね。癒される味わいだわ。";
        }
        else if (shokukan_score >= GameMgr.high_score && shokukan_score < GameMgr.high_score_2) //
        {
            _shokukan_kansou = GameMgr.ColorPink + "食感 A+: " + "絶妙な" + shokukan_mes + "</color>";
            _shopgirl_shokukan_kansou = "絶妙な" + shokukan_mes + "ね。良い味わい深さになってるわ。";
        }
        else if (shokukan_score >= GameMgr.high_score_2) //
        {
            _shokukan_kansou = GameMgr.ColorGold + "食感 S: " + "神の" + shokukan_mes + "！！" + "</color>";
            _shopgirl_shokukan_kansou = "神の" + shokukan_mes + "ね！　最高の味わいだと思うわ！";
        }
    }


    void CommentTextInit()
    {
        _commentDict.Clear();

        //_commentDict.Add("おいしい～♪");

        switch (girl1_status.GirlGokigenStatus)
        {
            case 0:

                _commentDict.Add("おいしい～♪");
                break;

            case 1:

                _commentDict.Add("..。");
                _commentDict.Add("さくさく・・。");
                _commentDict.Add("..おいしい。");
                break;

            case 2:

                _commentDict.Add(".. ..。");
                _commentDict.Add("..おいしい。");
                _commentDict.Add("..むむ。いいお味。");
                break;

            case 3:

                _commentDict.Add("兄ちゃん、おじょうず..。");
                _commentDict.Add("あま～。兄ちゃん、これおいしい。");
                _commentDict.Add("さくさく・・。うまうま・・。");
                break;

            case 4:

                _commentDict.Add("兄ちゃん、このおかしうめぇ。");
                _commentDict.Add("うみゃ～♪");
                _commentDict.Add("さくさく。うまうま。");
                break;

            case 5:

                _commentDict.Add("腕をあげたねぇ、お兄ちゃん。");
                _commentDict.Add("うまいな！");
                _commentDict.Add("さくさく。うまうま。かりかり。");
                break;

            case 6:

                _commentDict.Add("お兄ちゃん！これ最高！");
                _commentDict.Add("安定のおあじ..");
                _commentDict.Add("こころがポカポカ。");
                _commentDict.Add("あたたか～い");
                break;

            default:

                _commentDict.Add("お兄ちゃん！これ最高！");
                _commentDict.Add("安定のおあじ..");
                _commentDict.Add("こころがポカポカ。");
                _commentDict.Add("あたたか～い");
                _commentDict.Add("兄ちゃん、おじょうず..。");
                _commentDict.Add("兄ちゃん、このおかしうめぇ。");
                _commentDict.Add("うみゃ～♪");
                _commentDict.Add("さくさく。うまうま。");
                _commentDict.Add("腕をあげたねぇ、お兄ちゃん。");
                break;
        }

    }


    //** **//
    //コンテスト判定に使いまわし
    //** **//
    public int Judge_Score_Return(int value1, int value2, int SetType, int _Setcount)
    {      
        SceneInitSetting();
            
        //コンテスト用に、渡すアイテムのパラメータ設定
        Girleat_Judge_method(value1, value2, 1); //決定したアイテムのID(value1)と、タイプ(value2)を取得。SetTypeは、コンテストか否か。
        SetGirlTasteInit();      

        judge_score(1, _Setcount); //SetTypeは、0=女の子か1=コンテスト用かの判定。_Setcountは、GirlLikeCompoの1,2,3番目のどれを判定に使うかの数値

        //他に、食感スコアと見た目スコアも、反映させている。 returnは使ってないけど。
        return total_score;
    }

    //** **//
    //イベント（ピクニックなど）に使いまわし
    //** **//
    public int Judge_Score_ReturnEvent(int value1, int value2, int SetType, bool KoyuJudgeON, int Koyunum, bool DislikeFlag)
    {
        //デバッグパネルの取得
        debug_panel = canvas.transform.Find("Debug_Panel(Clone)").GetComponent<Debug_Panel>();
        debug_taste_resultText = canvas.transform.Find("Debug_Panel(Clone)/Hyouji/OkashiTaste_Scroll View/Viewport/Content/Text").GetComponent<Text>();
        debug_taste_resultText.text = "";

        //SceneInitSetting();

        //コンテスト用に、渡すアイテムのパラメータ設定
        Girleat_Judge_method(value1, value2, SetType); //決定したアイテムのID(value1)と、タイプ(value2)を取得。SetTypeは、コンテストか否か。

        //お菓子の判定値をセッティング
        if (!KoyuJudgeON)
        {
            girl1_status.InitializeStageGirlHungrySet(_baseSetjudge_num, 0); //compNum, セットする配列番号　の順　
        }
        else
        {
            girl1_status.InitializeStageGirlHungrySet(Koyunum, 0); //外部から、判定のセット番号を直接指定もできる。その場合、KoyuJudgeON=True
        }
        SetGirlTasteInit();
        girl1_status.Set_Count = 1;

        //そもそもお菓子が食べたいお菓子の種類だったかどうかを判定　ピクニックでは必要なし
        if (!DislikeFlag) //ピクニックなど、必要ないものではFalse
        {
            dislike_flag = true;
            dislike_status = 2;
        }
        else //サブNPCのイベントなど必要な場合はこっちもチェック
        {
            DislikeJudge();

            if (dislike_flag == false) //好みでない場合の処理　好みと一致していないとfalseがでる。その場合、たいてい、クエストは失敗におわる。
            {
                GameMgr.NPC_DislikeFlag = false;
            }
            else
            {
                GameMgr.NPC_DislikeFlag = true;
            }
        }
      

        //判定
        judge_score(SetType, 0); //SetTypeは、0=女の子か1=コンテスト用かの判定。

        //他に、食感スコアと見た目スコアも、反映させている。 returnは使ってないけど。
        return total_score;
    }

    //エフェクトをすぐに全て削除
    public void EffectClear()
    {
        for(i=0; i< _listHeartHit.Count; i++)
        {
            Destroy(_listHeartHit[i]);
        }
        _listHeartHit.Clear();

        for (i = 0; i < _listHeartHit2.Count; i++)
        {
            Destroy(_listHeartHit2[i]);
        }
        _listHeartHit2.Clear();


    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}

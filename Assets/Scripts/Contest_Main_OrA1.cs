using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using DG.Tweening;

public class Contest_Main_OrA1 : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private SoundController sc;
    private Girl1_status girl1_status;

    private GameObject placename_panel;
    private GameObject scene_black_effect;
    private GameObject contest_startbutton_panel;
    private GameObject canvas;
    private GameObject timelimitover_panel;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private ContestStartListDataBase conteststartList_database;

    private PlayerDefaultStartItemGet playerDefaultStart_ItemGet;

    private CombinationMain Combinationmain; //テスト用
    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<string> _itemSubtypeB_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。

    private ItemDataBase database;

    private MagicSkillListDataBase magicskill_database;

    private Debug_Panel_Init debug_panel_init;
    private Exp_Controller exp_Controller;

    private GameObject contest_judge_obj;
    private Contest_Judge contest_judge;

    private int contest_status;
    private int contest_scene;
    private PlayerItemList pitemlist;

    private GameObject text_area;
    private Text _text;

    private GameObject contest_select;
    private GameObject conteston_toggle_compo;
    private GameObject conteston_toggle_giveup;

    private int kettei_itemID;
    private int kettei_itemType;

    private BGM sceneBGM;

    private string itemName;
    private string item_subType;
    private string contest_name_origin;
    private int compNum;

    private int i, count;
    private bool judge_flag;
    private int judge_Type, DB_list_Type;
    private int inputcount;
    private bool StartRead; //シーンに入って最初の一回だけ起動する
    private bool contest_eventStart_flag; //シーン最初にシナリオ開始する

    //Live2Dモデルの取得    
    private GameObject _model_root_obj;
    private GameObject _model_move;
    private GameObject _model_obj;
    private CubismRenderController cubism_rendercontroller;
    private Animator live2d_animator;
    private bool character_ON;

    //賞品リスト
    private List<string> PrizeItemList = new List<string>();
    private Dictionary<int, string> PrizeRankList = new Dictionary<int, string>();

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 100;

        //GameMgr.ContestSelectNum = 10000; //コンテストの会場番号　//大会の場合、1回戦　2回戦　決勝戦とかをGameMgr.ContestRoundNumで決める。

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //ゲーム最初に所持するアイテムを決定するスクリプト
        playerDefaultStart_ItemGet = PlayerDefaultStartItemGet.Instance.GetComponent<PlayerDefaultStartItemGet>();

        //調合用メソッドの取得　テスト用
        Combinationmain = CombinationMain.Instance.GetComponent<CombinationMain>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;        
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f); //黒い画面からスタート

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;
        placename_panel.SetActive(false);

        //コンテスト開始ボタンパネル
        contest_startbutton_panel = canvas.transform.Find("ContestStartButtonPanel").gameObject;

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //お菓子の判定処理オブジェクトの取得
        contest_judge_obj = GameObject.FindWithTag("Contest_Judge");
        contest_judge = contest_judge_obj.GetComponent<Contest_Judge>();

        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //Live2Dモデルの取得
        character_ON = false;
        for (i = 0; i < SceneManager.sceneCount; i++)
        {
            //読み込まれているシーンを取得し、その名前をログに表示
            string sceneName = SceneManager.GetSceneAt(i).name;
            //Debug.Log(sceneName);

            GameObject[] rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();

            foreach (var obj in rootObjects)
            {
                //Debug.LogFormat("RootObject = {0}", obj.name);
                if (obj.name == "CharacterRoot")
                {
                    //Debug.Log("character_On: ヒカリちゃん　シーン内に存在する");
                    character_ON = true;

                    //Live2Dモデルの取得
                    _model_root_obj = GameObject.FindWithTag("CharacterRoot").gameObject;
                    _model_move = _model_root_obj.transform.Find("CharacterMove").gameObject;
                    _model_obj = _model_root_obj.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
                    cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();
                    live2d_animator = _model_obj.GetComponent<Animator>();
                    live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。
                }
                else
                {

                }
            }
        }
        

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "";

        contest_select = canvas.transform.Find("Contest_Select").gameObject;
        conteston_toggle_compo = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_Compo").gameObject;
        conteston_toggle_giveup = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_GiveUp").gameObject;

        timelimitover_panel = canvas.transform.Find("TimeOverPanel").gameObject;
        timelimitover_panel.SetActive(false);

        contest_status = 0;
        StartRead = false;
        contest_eventStart_flag = false;

        PrizeRankDict();

        //デバッグ用　最初に所持するアイテム
        Debug_StartItem();

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }
	
	// Update is called once per frame
	void Update () {

        //コンテスト会場きたときのイベント
        if (!contest_eventStart_flag)
        {
            contest_eventStart_flag = true;
            GameMgr.Contest_ON = true;

            //さらにどのコンテストに現在出場しているかを指定
            GameMgr.ContestRoundNum = 1; //一回戦
            GameMgr.contest_TotalScoreList.Clear();
            ContestDataSetting();            

            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();            

            GameMgr.contest_event_num = GameMgr.ContestSelectNum;
            GameMgr.contest_or_event_flag = true;
        }

        //二回戦以降、始まる場合の処理　Utage_scenarioの採点後にフラグをたてている。
        if(GameMgr.Contest_Next_flag)
        {
            GameMgr.Contest_Next_flag = false;

            GameMgr.ContestRoundNum++;
            ContestDataSetting();

            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_or_event_flag = true;
        }

        //決勝戦終了後、賞品獲得
        if(GameMgr.Contest_PrizeGet_flag)
        {
            GameMgr.Contest_PrizeGet_flag = false;

            PrizeGet(); //アイテム獲得

            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_or_prizeget_flag = true;
        }

        //コンテスト終了　会場外へでる。
        if (GameMgr.contest_eventEnd_flag)
        {
            GameMgr.contest_eventEnd_flag = false;
            GameMgr.Contest_ON = false;

            FadeManager.Instance.LoadScene("Or_Outside_the_Contest", 0.3f);
        }

        //制限時間を少し超えた場合、注意のパネルがでる
        if(GameMgr.contest_LimitTimeOver_DegScore_flag)
        {
            timelimitover_panel.SetActive(true);
        }

        //
        //
        //


        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            text_area.SetActive(false);
            //placename_panel.SetActive(false);
            contest_select.SetActive(false);           
            _model_move.SetActive(false);
            contest_startbutton_panel.SetActive(false);
        }
        else
        {
            switch (contest_status)
            {
                case 0:

                    text_area.SetActive(true);
                    //placename_panel.SetActive(true);
                    contest_select.SetActive(true);
                    contest_startbutton_panel.SetActive(true);
                    sceneBGM.MuteOFFBGM();
                    _model_move.SetActive(true);
                    live2d_animator.SetLayerWeight(3, 0.0f); //宴用表情はオフにしておく。

                    if (!StartRead) //シーン最初だけ読み込む
                    {
                        StartRead = true;
                        sceneBGM.PlaySub();
                        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f);
                    }                   

                    GameMgr.compound_select = 0; //何もしていない状態
                    GameMgr.compound_status = 0;

                    contest_status = 100;
                    contest_scene = 0;

                    GameMgr.Status_zero_readOK = true;

                    //制限時間　30分を超えた場合、失格フラグ
                    if (GameMgr.contest_LimitTimeOver_Gameover_flag)
                    {
                        GameMgr.contest_LimitTimeOver_Gameover_flag = false;

                        timelimitover_panel.SetActive(true);
                        LimitTimeOver();
                    }

                    break;

                case 100: //退避

                    break;

                case 500: //調合用

                    //調合終了まち
                    if (GameMgr.CompoundSceneStartON == false)
                    {
                        GameMgr.compound_select = 0; //何もしていない状態
                        GameMgr.compound_status = 0;

                        contest_status = 0;
                        contest_scene = 0;
                    }
                    break;

                default:

                    break;
            }
        }
    }

    //各コンテストのデータの初期設定
    void ContestDataSetting()
    {
        contest_name_origin = conteststartList_database.conteststart_lists[GameMgr.Contest_listnum].ContestName;

        if (GameMgr.Contest_Cate_Ranking == 0) //コンテストがトーナメント形式=0
        {
            Debug.Log("トーナメント形式");

            //コンテストごとに、判定を変える　また、判定はGirlEat_Judgeでも特殊点を判定
            switch (GameMgr.ContestSelectNum)
            {
                case 1000: //オレンジーナコンテストA1 クープデュモンド

                    GameMgr.ContestRoundNumMax = 3; //そのコンテストの最大のラウンド数

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin + "_1";
                            ContestData_001();
                            break;

                        case 2: //二回戦

                            GameMgr.Contest_Name = contest_name_origin + "_2";
                            ContestData_002();
                            break;

                        case 3: //決勝戦

                            GameMgr.Contest_Name = contest_name_origin + "_3";
                            ContestData_003();
                            break;
                    }

                    if (GameMgr.ContestRoundNum == 1) //最初のときだけ設定
                    {
                        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。
                        PrizeItemList.Clear();
                        PrizeItemList.Add("nuts");
                        PrizeItemList.Add("ice_box");
                        PrizeItemList.Add("neko_badge2");
                        PrizeItemList.Add("whisk_magic");
                        PrizeItemList.Add("gold_oven");

                        GameMgr.PrizeScoreAreaList.Clear();
                        GameMgr.PrizeScoreAreaList.Add(60);
                        GameMgr.PrizeScoreAreaList.Add(120);
                        GameMgr.PrizeScoreAreaList.Add(180);
                        GameMgr.PrizeScoreAreaList.Add(240);
                    }

                    break;
            }
        }
        else //ランキング形式=1
        {
            Debug.Log("ランキング形式（一回戦のみ）");

            switch (GameMgr.ContestSelectNum)
            {

                case 10000: //オレンジーナコンテスト　弱小　ランキング形式

                    GameMgr.ContestRoundNumMax = 1; //そのコンテストの最大のラウンド数 １の場合、ランキング形式（複数参加者がランキングで競う）で一回戦のみ

                    switch (GameMgr.ContestRoundNum)
                    {
                        case 1: //一回戦

                            GameMgr.Contest_Name = contest_name_origin + "_1";
                            ContestData_100();
                            break;

                    }

                    if (GameMgr.ContestRoundNum == 1) //最初のときだけ設定
                    {
                        //賞品リスト　アイテム名のリストと点数の範囲　スコアに応じて変わる。ラウンドごとの点数の合計。
                        PrizeItemList.Clear();
                        PrizeItemList.Add("nuts");
                        PrizeItemList.Add("ice_box");
                        PrizeItemList.Add("neko_badge2");
                        PrizeItemList.Add("whisk_magic");
                        PrizeItemList.Add("gold_oven");

                        //相手の点数リスト
                        GameMgr.PrizeScoreAreaList.Clear();
                        GameMgr.PrizeScoreAreaList.Add(30);
                        GameMgr.PrizeScoreAreaList.Add(56);
                        GameMgr.PrizeScoreAreaList.Add(83);
                        GameMgr.PrizeScoreAreaList.Add(GameMgr.contest_boss_score);
                    }

                    break;
            }
        }
        //コンテストごとに、判定を変える　また、判定はGirlEat_Judgeでも特殊点を判定
        
        Debug.Log("コンテスト名前と番号とラウンド数: " + GameMgr.Contest_Name + " " + GameMgr.ContestSelectNum + " " + GameMgr.ContestRoundNum + "回戦");

        _text.text = GameMgr.Contest_ProblemSentence;
        contest_status = 0;
        StartRead = false; //BGMをリセット
    }

    void ContestData_001()
    {
        //ランダムでもし課題を選ぶ場合は、ここでランダムで指定してよい
        GameMgr.Contest_DB_list_Type = 20000; //compNum=20000~を指定        
        GameMgr.Contest_ProblemSentence = "至高のチョコレート（Aランク）" + "\n" + "テーマ：「風」をテーマにした美しいチョコレート";

        //コンテスト時間指定
        PlayerStatus.player_contest_hour = 8; //コンテストの開始時間
        PlayerStatus.player_contest_minute = 0; //開始分
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 80; //一回戦相手の点数
    }

    void ContestData_002()
    {
        GameMgr.Contest_DB_list_Type = 21000; //compNum=20000~を指定
        GameMgr.Contest_ProblemSentence = "自由課題" + "\n" + "テーマ：「海」をテーマにした自由なお菓子";

        //コンテスト時間指定
        PlayerStatus.player_contest_hour = 8; //コンテストの開始時間
        PlayerStatus.player_contest_minute = 0; //開始分
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 90; //
    }

    void ContestData_003()
    {
        GameMgr.Contest_DB_list_Type = 22000; //compNum=20000~を指定
        GameMgr.Contest_ProblemSentence = "至高のケーキ" + "\n" + "テーマ：「愛」をテーマにした至高のケーキ";

        //コンテスト時間指定
        PlayerStatus.player_contest_hour = 8; //コンテストの開始時間
        PlayerStatus.player_contest_minute = 0; //開始分
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位

        GameMgr.contest_boss_score = 97; //
    }

    void ContestData_100()
    {
        //ランダムでもし課題を選ぶ場合は、ここでランダムで指定してよい
        GameMgr.Contest_DB_list_Type = 100000; //compNum=100000~を指定

        GameMgr.Contest_ProblemSentence = "テーマ：おいしいクッキー";

        //コンテスト時間指定
        PlayerStatus.player_contest_hour = 8; //コンテストの開始時間
        PlayerStatus.player_contest_minute = 0; //開始分
        PlayerStatus.player_contest_LimitTime = 480; //制限時間　1分単位   

        GameMgr.contest_boss_score = 92;
    }

    void PrizeGet()
    {
        //5段階ぐらいで分ける？
        i = 0;
        while (i <= GameMgr.PrizeScoreAreaList.Count)
        {
            if (i == 0)
            {
                if (GameMgr.contest_PrizeScore < GameMgr.PrizeScoreAreaList[i])
                {                   
                    pitemlist.addPlayerItemString(PrizeItemList[i], 1);
                    GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(PrizeItemList[i])].itemNameHyouji;
                    Debug.Log("ランク: " + PrizeRankList[i]);
                    break;
                }
            }
            else
            {
                if(i != GameMgr.PrizeScoreAreaList.Count)
                {
                    if (GameMgr.contest_PrizeScore >= GameMgr.PrizeScoreAreaList[i - 1] && GameMgr.contest_PrizeScore < GameMgr.PrizeScoreAreaList[i])
                    {                      
                        pitemlist.addPlayerItemString(PrizeItemList[i], 1);
                        GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(PrizeItemList[i])].itemNameHyouji;
                        Debug.Log("ランク: " + PrizeRankList[i]);
                        break;
                    }
                }
                else //リストの一番最後
                {
                    if (GameMgr.contest_PrizeScore >= GameMgr.PrizeScoreAreaList[i - 1])
                    {
                        pitemlist.addPlayerItemString(PrizeItemList[i], 1);
                        GameMgr.Contest_PrizeGet_ItemName = database.items[database.SearchItemIDString(PrizeItemList[i])].itemNameHyouji;
                        Debug.Log("ランク: " + PrizeRankList[i]);
                        break;
                    }
                }
                
            }
            i++;
        }
    }

    void PrizeRankDict()
    {
        PrizeRankList.Clear();
        PrizeRankList.Add(0, "D");
        PrizeRankList.Add(1, "C");
        PrizeRankList.Add(2, "B");
        PrizeRankList.Add(3, "A");
        PrizeRankList.Add(4, "S");
    }
    

    public void OnCheck_Compound() //調合シーンに入る
    {
        if (conteston_toggle_compo.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_compo.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            contest_status = 500; //
            contest_scene = 500;
            text_area.SetActive(false);

            GameMgr.compound_status = 6;
            GameMgr.CompoundSceneStartON = true; //調合シーンに入っています、というフラグ開始。処理をCompoundMainControllerオブジェに移す。
        }
    }

    public void OnCheck_GiveUp() //諦める
    {
        if (conteston_toggle_giveup.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_giveup.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。
            GameMgr.Contest_ON = false;

            FadeManager.Instance.LoadScene("Or_Outside_the_Contest", 0.3f);
        }
    }

    //審査員におかしを提出する
    public void OnContestJudge_Start()
    {
        sceneBGM.MuteBGM();
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

        GameMgr.contest_event_num = GameMgr.ContestSelectNum;

        StartCoroutine("WaitForJudge");
    }

    IEnumerator WaitForJudge()
    {
        yield return new WaitForSeconds(1f); //1秒待つ

        //お菓子を採点する
        contest_judge.Contest_Judge_Start();

        //パネルのお菓子を削除
        pitemlist.deleteExtremePanelItem(0, 1);

        GameMgr.contest_LimitTimeOver_DegScore_flag = false; //採点後にオフにする。
        GameMgr.scenario_ON = true;
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.contest_or_contestjudge_flag = true;
    }

    //制限時間をこえたので失格
    void LimitTimeOver()
    {
        sceneBGM.MuteBGM();
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

        GameMgr.contest_event_num = GameMgr.ContestSelectNum;

        StartCoroutine("WaitForLimitTimeOver");
    }

    IEnumerator WaitForLimitTimeOver()
    {
        yield return new WaitForSeconds(1f); //1秒待つ

        GameMgr.scenario_ON = true;
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.contest_or_limittimeover_flag = true;
    }

    void Debug_StartItem()
    {
        //pitemlist.addPlayerItemString("komugiko", 10);

        //デバッグ用　全てのアイテムを追加する
        //playerDefaultStart_ItemGet.AddAllItem_NoAcce();
        pitemlist.addPlayerItemString("cacao_beans", 10);
        pitemlist.addPlayerItemString("cacao_nibs", 10);
        pitemlist.addPlayerItemString("coffee_powder", 10);

        pitemlist.addPlayerItemString("nuts_pistachio", 10);
        pitemlist.addPlayerItemString("appaleil_pistachio", 10);

        pitemlist.addPlayerItemString("suger", 10);
        pitemlist.addPlayerItemString("emerald_suger", 10);

        pitemlist.addPlayerItemString("beans_crusher", 1);
        pitemlist.addPlayerItemString("appaleil_chocolate", 10);
        pitemlist.addPlayerItemString("appaleil_chocolate_twister_lv1", 10);

        pitemlist.addPlayerItemString("mint", 10);

        magicskill_database.skillLearnLv_Name("Cookie_Study", 10);
        magicskill_database.skillLearnLv_Name("Freezing_Spell", 10);
        magicskill_database.skillLearnLv_Name("Luminous_Suger", 10);

        magicskill_database.skillLearnLv_Name("Bake_Beans", 10);
        magicskill_database.skillLearnLv_Name("Freezing_Spell", 10);
        magicskill_database.skillLearnLv_Name("Removing_Shells", 10);
        magicskill_database.skillLearnLv_Name("Chocolate_Tempering", 10);
        magicskill_database.skillLearnLv_Name("Wind_Twister", 10);

    }

    //デバッグ用にすぐに計算するボタン
    public void OnDebugContest_Judge_Now()
    {
        contest_judge.Contest_Judge_Start();
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMgr.Scene_LoadedOn_End = true;
    }

    //シーンがアンロードされたタイミングで呼び出しされる
    void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        
        GameMgr.Scene_LoadedOn_End = false;
    }
}

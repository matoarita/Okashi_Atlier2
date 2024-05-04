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

    private TimeController time_controller;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private GameObject card_view_obj;
    private CardView card_view;

    private ItemDataBase database;

    private GameObject black_panel_A;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private ContestStartListDataBase conteststartList_database;
    private ContestPrizeScoreDataBase contestPrizeScore_dataBase;

    private PlayerDefaultStartItemGet playerDefaultStart_ItemGet;

    private CombinationMain Combinationmain; //テスト用
    private List<string> _itemIDtemp_result = new List<string>(); //調合リスト。アイテムネームに変換し、格納しておくためのリスト。itemNameと一致する。
    private List<string> _itemSubtype_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<string> _itemSubtypeB_temp_result = new List<string>(); //調合DBのサブタイプの組み合わせリスト。
    private List<int> _itemKosutemp_result = new List<int>(); //調合の個数組み合わせ。

    private MagicSkillListDataBase magicskill_database;

    private Debug_Panel_Init debug_panel_init;
    private Exp_Controller exp_Controller;

    private GameObject contest_judge_obj;
    private Contest_Judge contest_judge;

    private PlayerItemList pitemlist;

    private GameObject contestPrizePanel;

    private GameObject text_area;
    private Text _text;

    private GameObject yes_no_panel; //通常時のYes, noボタン
    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes_no_submit_panel;
    private GameObject yes_no_giveup_panel;

    private GameObject contest_select;
    private GameObject conteston_toggle_01;
    private GameObject conteston_toggle_giveup;

    private GameObject mainUI_panel;

    private int kettei_itemID;
    private int kettei_itemType;

    private BGM sceneBGM;

    private string itemName;
    private string item_subType;
    private string contest_name_origin;
    private int compNum;

    private int i, count;
    private int _rank;
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

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 100;

        GameMgr.Scene_Name = "Or_Contest";
        
        /* デバッグ用 */
        GameMgr.ContestSelectNum = 10000; //コンテストの会場番号　現在デバッグ用　//大会の場合、1回戦　2回戦　決勝戦とかをGameMgr.ContestRoundNumで決める。
        GameMgr.Contest_Cate_Ranking = 1;
        GameMgr.Story_Mode = 1;
        GameMgr.GirlLoveEvent_num = 10;
        /* */

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
        contestPrizeScore_dataBase = ContestPrizeScoreDataBase.Instance.GetComponent<ContestPrizeScoreDataBase>();

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

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //女の子、お菓子の判定処理オブジェクトの取得
        GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
        girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //シーン全てをブラックに消すパネル
        scene_black_effect = canvas.transform.Find("Scene_Black").gameObject;        
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f); //黒い画面からスタート

        //場所名前パネル
        placename_panel = canvas.transform.Find("PlaceNamePanel").gameObject;
        placename_panel.SetActive(false);

        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        yes_no_submit_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel1").gameObject;
        yes_no_giveup_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel2").gameObject;

        contestPrizePanel = canvas.transform.Find("ContestPrizePanel").gameObject;
        contestPrizePanel.SetActive(false);


        //黒半透明パネルの取得
        black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;
        black_panel_A.SetActive(false);

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //コンテスト開始ボタンパネル
        contest_startbutton_panel = canvas.transform.Find("MainUIPanel/ContestStartButtonPanel").gameObject;

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

        mainUI_panel = canvas.transform.Find("MainUIPanel").gameObject;
        contest_select = canvas.transform.Find("MainUIPanel/Contest_Select").gameObject;
        conteston_toggle_01 = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_01").gameObject;
        conteston_toggle_giveup = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_GiveUp").gameObject;

        timelimitover_panel = canvas.transform.Find("MainUIPanel/TimeOverPanel").gameObject;
        timelimitover_panel.SetActive(false);

        GameMgr.Scene_Status = 0;
        StartRead = false;
        contest_eventStart_flag = false;

        //デバッグ用　最初に所持するアイテム
        if (GameMgr.System_DebugItemSet_ON)
        {
            Debug_StartItem();
        }

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
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;
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
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;
        }

        //決勝戦終了後、賞品獲得
        if(GameMgr.Contest_PrizeGet_flag)
        {
            GameMgr.Contest_PrizeGet_flag = false;

            contestPrizePanel.SetActive(false); //ランキング戦で一回表示してる可能性があるので、一度オフ
            contestPrizeScore_dataBase.PrizeGet(); //アイテム獲得

            //支給されたアイテムはここで削除
            pitemlist.DeleteContestSurppliedItem();

            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_or_prizeget_flag = true;
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;
        }

        //コンテスト終了　会場外へでる。
        if (GameMgr.contest_eventEnd_flag)
        {
            GameMgr.contest_eventEnd_flag = false;
            GameMgr.Contest_ON = false;
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;

            //FadeManager.Instance.LoadScene("Or_Outside_the_Contest", 0.3f);
            //家に帰って寝る
            time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day, 20, 0); //20時終了
            FadeManager.Instance.LoadScene("Or_Compound", 0.3f);
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
            //contest_select.SetActive(false);
            yes_no_panel.SetActive(false);
            _model_move.SetActive(false);
            //contest_startbutton_panel.SetActive(false);
            mainUI_panel.SetActive(false);

            if (GameMgr.Utage_Prizepanel_ON)
            {
                GameMgr.Utage_Prizepanel_ON = false;
                contestPrizePanel.SetActive(true);
                contestPrizePanel.GetComponent<GraphicRaycaster>().enabled = false; //宴が触れるように。
                scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
            }

            if (GameMgr.Utage_SceneEnd_BlackON)
            {
                GameMgr.Utage_SceneEnd_BlackON = false;
                scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 0.0f);
            }
            
        }
        else
        {
            switch (GameMgr.Scene_Status)
            {
                case 0:

                    text_area.SetActive(true);
                    //contest_select.SetActive(true);
                    //contest_startbutton_panel.SetActive(true);

                    yes_no_panel.SetActive(false);
                    yes_no_giveup_panel.SetActive(false);
                    yes_no_submit_panel.SetActive(false);
                    mainUI_panel.SetActive(true);
                    sceneBGM.MuteOFFBGM();

                    _model_move.SetActive(true);
                    live2d_animator.SetLayerWeight(3, 0.0f); //宴用表情はオフにしておく。
                    GameMgr.CharacterTouch_ALLON = true; //タッチもオンにする。

                    if (!StartRead) //シーン最初だけ読み込む
                    {
                        StartRead = true;
                        sceneBGM.PlaySub();
                        sceneBGM.NowFadeVolumeONBGM();
                        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f);
                    }                   

                    GameMgr.compound_select = 0; //何もしていない状態
                    GameMgr.compound_status = 0;

                    GameMgr.Scene_Status = 100;
                    GameMgr.Scene_Select = 0;

                    GameMgr.Status_zero_readOK = true;
                    GameMgr.contest_MainMatchStart = true; //本戦開始の合図　TimeControllerで時間が進み始める

                    //制限時間　30分を超えた場合、失格フラグ
                    if (GameMgr.contest_LimitTimeOver_Gameover_flag)
                    {
                        GameMgr.contest_LimitTimeOver_Gameover_flag = false;

                        timelimitover_panel.SetActive(true);
                        LimitTimeOver();
                    }

                    text_default();

                    break;

                case 10: //「あげる」を選択

                    GameMgr.Scene_Status = 13; //あげるシーンに入っています、というフラグ
                    GameMgr.Scene_Select = 10; //あげるを選択

                    yes_no_panel.SetActive(true);
                    yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);
                    black_panel_A.SetActive(true);

                    //腹減りカウント一時停止
                    girl1_status.GirlEatJudgecounter_OFF();

                    text_area.SetActive(true);
                    //WindowOff();

                    card_view.PresentGirl(2, 0);
                    StartCoroutine("Girl_present_Final_select");


                    break;

                case 11: //お菓子をあげたあとの処理。女の子が、お菓子を判定

                    GameMgr.Scene_Status = 12;
                    text_area.SetActive(false);
                    GameMgr.girlEat_ON = true; //お菓子判定中フラグ

                    //お菓子の判定処理を起動。引数は、決定したアイテムのアイテムIDと、店売りかオリジナルで制作したアイテムかの、判定用ナンバー 0or1or10 1=コンテストのとき 10=コンテスト味見
                    girlEat_judge.Girleat_Judge_method(0, 2, 10);
                    GameMgr.extremepanel_Koushin = true; //食べたので、パネルのお菓子は消える。

                    break;

                case 12: //お菓子を判定中

                    break;

                case 13: //あげるかあげないかを選択中

                    break;

                case 100: //退避

                    break;

                case 500: //調合用

                    //調合終了まち
                    if (GameMgr.CompoundSceneStartON == false)
                    {
                        GameMgr.compound_select = 0; //何もしていない状態
                        GameMgr.compound_status = 0;

                        GameMgr.Scene_Status = 0;
                        GameMgr.Scene_Select = 0;
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
        //DBで初期設定を行っている
        conteststartList_database.ContestSetting();

        //コンテスト支給のものがあれば、このタイミングで追加
        conteststartList_database.AddContest_SurppliedItem();

        _text.text = GameMgr.Contest_ProblemSentence;
        GameMgr.Scene_Status = 0;
        StartRead = false; //BGMをリセット

        //Debug_Scorekeisan();//デバッグ用
    }

    void text_default()
    {
        if(GameMgr.Ajimi_AfterFlag)
        {
            GameMgr.Ajimi_AfterFlag = false;
            _text.text = GameMgr.AjimiAfter_Text;
        }
        else
        {
            _text.text = GameMgr.Contest_ProblemSentence;
        }
        
    }

    

    
    /*
    public void OnCheck_Compound() //調合シーンに入る
    {
        if (conteston_toggle_01.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_01.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            GameMgr.Scene_Status = 500; //
            GameMgr.Scene_Select = 500;
            text_area.SetActive(false);

            GameMgr.compound_status = 6;
            GameMgr.CompoundSceneStartON = true; //調合シーンに入っています、というフラグ開始。処理をCompoundMainControllerオブジェに移す。
        }
    }*/

    public void OnCheck_GirlEat() //ヒカリに味見させる
    {
        if (conteston_toggle_01.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_01.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            Debug.Log("味見～～～～");

            if (pitemlist.player_extremepanel_itemlist.Count > 0)
            {
                _text.text = "今、作ったお菓子を味見してもらう？" + "\n" + "※15分経過する"; // + "\n" + "あと " + GameMgr.ColorLemon + nokori_kaisu + "</color>" + "回　あげられるよ。"
                GameMgr.Scene_Status = 10;

            }
            else //まだ作ってないときは
            {
                _text.text = "まだお菓子を作っていない。";
            }
        }
    }

    IEnumerator Girl_present_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false;

        black_panel_A.SetActive(false);
        card_view.DeleteCard_DrawView();
        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                //女の子にアイテムをあげる処理
                GameMgr.Scene_Status = 11; //status=11で処理。

                yes_no_panel.SetActive(false);

                //時間の消費
                time_controller.SetMinuteToHourContest(15);

                //お菓子をあげた回数をカウント
                /*PlayerStatus.player_girl_eatCount++;

                if (PlayerStatus.player_girl_eatCount >= 999)
                {
                    PlayerStatus.player_girl_eatCount = 999; //999でカンスト
                }*/

                break;

            case false:

                //Debug.Log("cancel");

                //_textmain.text = "";
                GameMgr.Scene_Status = 0;
                yes_no_panel.SetActive(false);

                break;

        }
    }




    public void OnCheck_GiveUp() //諦める
    {
        if (conteston_toggle_giveup.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_giveup.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            yes_no_giveup_panel.SetActive(true);
            black_panel_A.SetActive(true);

            //腹減りカウント一時停止
            girl1_status.GirlEatJudgecounter_OFF();

            text_area.SetActive(true);
            _text.text = "にいちゃん！　コンテストあきらめる？";
            StartCoroutine("GiveUp_Final_select");
            
        }
    }

    IEnumerator GiveUp_Final_select()
    {

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false;

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                yes_no_giveup_panel.SetActive(false);

                GiveUpContest();

                break;

            case false:

                //Debug.Log("cancel");

                //_textmain.text = "";
                GameMgr.Scene_Status = 0;
                yes_no_giveup_panel.SetActive(false);

                break;

        }
    }

    //コンテストギブアップ　暗くなる演出
    void GiveUpContest()
    {
        sceneBGM.MuteBGM();
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

        //支給されたアイテムはここで削除
        pitemlist.DeleteContestSurppliedItem();

        StartCoroutine("WaitForGiveUpContest");
    }

    IEnumerator WaitForGiveUpContest()
    {
        yield return new WaitForSeconds(2.0f); //1秒待つ

        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.Contest_ON = false;
        FadeManager.Instance.LoadScene("Or_Outside_the_Contest", 0.3f);
    }




    //審査員におかしを提出する
    public void OnContestJudge_Start()
    {
        yes_no_submit_panel.SetActive(true);
        black_panel_A.SetActive(true);

        //腹減りカウント一時停止
        girl1_status.GirlEatJudgecounter_OFF();

        text_area.SetActive(true);

        if(pitemlist.player_extremepanel_itemlist.Count > 0)
        {
            _text.text = "にいちゃん！　このお菓子で提出する？";
        }
        else //まだできていない
        {
            _text.text = "にいちゃん！" + "\n" + "お菓子がまだできてないよ～・・。";
            yes_no_submit_panel.transform.Find("Yes_Clear").GetComponent<Button>().interactable = false;
            yes_no_submit_panel.transform.Find("Yes_Clear").GetComponent<Sound_Trigger>().enabled = false;
        }
        
        StartCoroutine("Submit_Final_select");       
    }

    //提出する最終確認
    IEnumerator Submit_Final_select()
    {
        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
        yes_selectitem_kettei.onclick = false;

        black_panel_A.SetActive(false);

        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                yes_no_submit_panel.SetActive(false);

                sceneBGM.MuteBGM();
                scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
                scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

                GameMgr.contest_event_num = GameMgr.ContestSelectNum;

                StartCoroutine("WaitForJudge");

                break;

            case false:

                //Debug.Log("cancel");

                //_textmain.text = "";
                GameMgr.Scene_Status = 0;
                yes_no_submit_panel.SetActive(false);
                yes_no_submit_panel.transform.Find("Yes_Clear").GetComponent<Button>().interactable = true;
                yes_no_submit_panel.transform.Find("Yes_Clear").GetComponent<Sound_Trigger>().enabled = true;

                break;

        }
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
        pitemlist.addPlayerItemString("komugiko", 10);
        pitemlist.addPlayerItemString("butter", 10);
        pitemlist.addPlayerItemString("suger", 10);

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

    //デバッグ用　コンテストの総合得点いれると、今何位か分かる＋パラメータのセッティング
    void Debug_Scorekeisan()
    {
        GameMgr.contest_TotalScore = 99;


        i = 0;
        while (i <= GameMgr.PrizeScoreAreaList.Count)
        {
            if (i == 0)
            {
                if (GameMgr.contest_TotalScore < GameMgr.PrizeScoreAreaList[i])
                {
                    _rank = GameMgr.PrizeScoreAreaList.Count + 1 - i;
                    Debug.Log("順位: " + _rank + "位");
                    break;
                }
            }
            else
            {
                if (i != GameMgr.PrizeScoreAreaList.Count)
                {
                    if (GameMgr.contest_TotalScore >= GameMgr.PrizeScoreAreaList[i - 1] && GameMgr.contest_TotalScore < GameMgr.PrizeScoreAreaList[i])
                    {
                        _rank = GameMgr.PrizeScoreAreaList.Count + 1 - i;
                        Debug.Log("順位: " + _rank + "位");
                        break;
                    }
                }
                else //リストの一番最後
                {
                    if (GameMgr.contest_TotalScore >= GameMgr.PrizeScoreAreaList[i - 1])
                    {
                        _rank = GameMgr.PrizeScoreAreaList.Count + 1 - i;
                        Debug.Log("順位: " + "優勝");
                        break;
                    }
                }

            }
            i++;
        }

        GameMgr.contest_Rank_Count = _rank;
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

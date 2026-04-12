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
    private GameObject contestFirstEnshutuPanel;

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
    private GameObject yes_no_namekakunin_panel;

    private GameObject contest_select;
    private GameObject conteston_toggle_01;
    private GameObject conteston_toggle_giveup;
    private GameObject hinttaste_toggle;
    private GameObject conteston_toggle_nameok;
    private GameObject okashihint_panel;

    private GameObject namesetting_panel;
    private Text nameplate_text;
    private InputField inputField_okashiname;
    private Image okashi_img;
    private string default_itemName;
    private string kettei_itemName;

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
    private int _id;
    private bool judge_flag;
    private int judge_Type, DB_list_Type;
    private int inputcount;
    private bool StartRead; //シーンに入って最初の一回だけ起動する
    private bool contest_eventStart_flag; //シーン最初にシナリオ開始する
    private bool Contest_PrizeGetScene; //コンテスト　賞品獲得画面に入ってるフラグ　このシーンのみで使う

    private GameObject BGImagePanel;
    private GameObject BG_contest_chuubou;
    private GameObject BG_contest_hall;
    private GameObject BG_contest_prizeget;

    private List<GameObject> BG_contest_hall_List = new List<GameObject>();
    private List<GameObject> BG_contest_chuubou_List = new List<GameObject>();

    private bool contestBG_OK;
    private bool contestBG_chuubouOK;

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
       
        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの取得
        canvas = GameObject.FindWithTag("Canvas");
        
        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();
        contestPrizeScore_dataBase = ContestPrizeScoreDataBase.Instance.GetComponent<ContestPrizeScoreDataBase>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

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
        yes_no_namekakunin_panel = canvas.transform.Find("StageClear_Yes_no_Panel/Panel4").gameObject;

        namesetting_panel = canvas.transform.Find("NameSettingPanel").gameObject;
        namesetting_panel.SetActive(false);
        nameplate_text = namesetting_panel.transform.Find("NamePlate/Text").GetComponent<Text>();
        nameplate_text.text = "";
        inputField_okashiname = namesetting_panel.transform.Find("NameInput").GetComponent<InputField>();
        inputField_okashiname.text = "";
        okashi_img = namesetting_panel.transform.Find("ItemPanel/ItemImg").GetComponent<Image>();

        conteston_toggle_nameok = namesetting_panel.transform.Find("Yesno_Select/Viewport/Content/ContestOn_Toggle_NameOK").gameObject;

        contestPrizePanel = canvas.transform.Find("ContestPrizePanel").gameObject;
        contestPrizePanel.GetComponent<CanvasGroup>().alpha = 0;
        contestPrizePanel.SetActive(false);
        
        Contest_PrizeGetScene = false;

        //黒半透明パネルの取得
        black_panel_A = canvas.transform.Find("Black_Panel_A").gameObject;
        black_panel_A.SetActive(false);

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //コンテスト開始ボタンパネル
        contest_startbutton_panel = canvas.transform.Find("MainUIPanel/ContestStartButtonPanel").gameObject;

        //コンテスト演出パネル
        contestFirstEnshutuPanel = canvas.transform.Find("ContestFirstEnshutuPanel").gameObject;
        contestFirstEnshutuPanel.SetActive(false);

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

        //背景取得
        BGImagePanel = GameObject.FindWithTag("BG");
        BG_contest_chuubou = BGImagePanel.transform.Find("ContestBG_chuubou").gameObject;
        BG_contest_hall = BGImagePanel.transform.Find("ContestBG_Hall").gameObject;
        BG_contest_prizeget = BGImagePanel.transform.Find("ContestBG_PrizeGet").gameObject;
        BG_contest_chuubou.SetActive(false);
        BG_contest_hall.SetActive(true);        
        BG_contest_prizeget.SetActive(false);

        BG_contest_hall_List.Clear();
        foreach (Transform obj in BG_contest_hall.transform)
        {
            BG_contest_hall_List.Add(obj.gameObject);
        }
        BG_contest_chuubou_List.Clear();
        foreach (Transform obj in BG_contest_chuubou.transform)
        {
            BG_contest_chuubou_List.Add(obj.gameObject);
        }
        

        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        _text.text = "";

        mainUI_panel = canvas.transform.Find("MainUIPanel").gameObject;
        contest_select = canvas.transform.Find("MainUIPanel/Contest_Select").gameObject;
        conteston_toggle_01 = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_01").gameObject;
        conteston_toggle_giveup = contest_select.transform.Find("Viewport/Content/ContestOn_Toggle_GiveUp").gameObject;
        hinttaste_toggle = canvas.transform.Find("MainUIPanel/HintTaste_Toggle").gameObject;

        //お菓子ヒントパネルの取得
        okashihint_panel = canvas.transform.Find("TasteHintPanel").gameObject;

        timelimitover_panel = canvas.transform.Find("MainUIPanel/TimeOverPanel").gameObject;
        timelimitover_panel.SetActive(false);

        GameMgr.Scene_Status = 0;
        StartRead = false;
        contest_eventStart_flag = false;
        

        //ウィンドウキャラ名設定
        //GameMgr.Window_CharaName = GameMgr.mainGirl_Name;
        GameMgr.Window_CharaName = "";


        //コンテスト再開用データがあり、メイン画面から「再開」してここに来た場合は、データをここで入れる
        //いくつかのデータは後ろでリセットされるので、Updataのほうでも更新
        if(GameMgr.ContestRestart_MainStart)
        {
            //もしエクストリームパネルにすでにお菓子があった場合は、オリジナルリストへ移動しておく。
            pitemlist.MoveExtremeToOriginalItem();

            //さらに、ヒカリが制作中の場合、制作を一度リセット
            pitemlist.HikariMakeReset();

            _id = conteststartList_database.SearchContestString(GameMgr.ContestRestart_contestname); //コンテストの会場番号　コンテスト名いれたらOK

            GameMgr.ContestSelectNum = conteststartList_database.conteststart_lists[_id].Contest_placeNumID;
            GameMgr.Contest_Cate_Ranking = conteststartList_database.conteststart_lists[_id].Contest_RankingType;
            GameMgr.Contest_BringType = conteststartList_database.conteststart_lists[_id].Contest_BringType;
            GameMgr.Contest_HallBGName = conteststartList_database.conteststart_lists[_id].ContestBGName;
            GameMgr.Contest_ChubouBGName = conteststartList_database.conteststart_lists[_id].ContestBGChubouName;
            GameMgr.Contest_BGMSelect = conteststartList_database.conteststart_lists[_id].ContestBGMSelect;
            GameMgr.Contest_BGMSelectHall = conteststartList_database.conteststart_lists[_id].ContestBGMSelectHall;
            GameMgr.Contest_FightsCount = conteststartList_database.conteststart_lists[_id].ContestFightsCount;

            //出場回数+1
            conteststartList_database.conteststart_lists[_id].ContestFightsCount++;

            //素材持ち込み不可の場合、一時的に預かりリストへ持ち物を預ける
            if (GameMgr.Contest_BringType == 1) //素材のみ持ち込みOK
            {
                pitemlist.Keep_PitemList(1);
            }
            else if (GameMgr.Contest_BringType == 2) //素材持ち込み×
            {
                pitemlist.Keep_PitemList(2);
            }

            //参加費用は再戦の場合、かからない仕様
        }

        //デバッグ用
        //GameMgr.System_DebugItemSet_ON = true;
        if (GameMgr.System_DebugItemSet_ON)
        {
            _id = conteststartList_database.SearchContestString("Or_Contest_001"); //コンテストの会場番号　コンテスト名いれたらOK

            GameMgr.ContestSelectNum = conteststartList_database.conteststart_lists[_id].Contest_placeNumID;
            GameMgr.Contest_Cate_Ranking = conteststartList_database.conteststart_lists[_id].Contest_RankingType;
            GameMgr.Contest_BringType = conteststartList_database.conteststart_lists[_id].Contest_BringType;
            GameMgr.Contest_HallBGName = conteststartList_database.conteststart_lists[_id].ContestBGName;
            GameMgr.Contest_ChubouBGName = conteststartList_database.conteststart_lists[_id].ContestBGChubouName;
            GameMgr.Contest_BGMSelect = conteststartList_database.conteststart_lists[_id].ContestBGMSelect;
            GameMgr.Contest_BGMSelectHall = conteststartList_database.conteststart_lists[_id].ContestBGMSelectHall;
            GameMgr.Contest_NameHyouji = conteststartList_database.conteststart_lists[_id].ContestNameHyouji;
            GameMgr.Contest_EnshutuNameHyouji = conteststartList_database.conteststart_lists[_id].ContestEnshutuName;

            //GameMgr.Story_Mode = 1;
            GameMgr.GirlLoveEvent_num = 10;
            GameMgr.System_MagicUse_Flag = true;
            GameMgr.System_HikariMakeUse_Flag = true;

            Debug_StartItem();
        }

        //背景設定
        ContestHall_Select(GameMgr.Contest_HallBGName, GameMgr.Contest_ChubouBGName); //会場背景と厨房背景を設定

        //シーン読み込み完了時のメソッド
        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }
	
	// Update is called once per frame
	void Update () {

        //各コンテストのデータ初期化とイベントはじまる
        if (!contest_eventStart_flag)
        {
            contest_eventStart_flag = true;
            GameMgr.Contest_ON = true;
           
            //コンテスト開始時のみリセット
            GameMgr.contest_TotalScoreList.Clear();
            GameMgr.contest_okashiNameList.Clear(); //提出したお菓子を各回ごとに記録したもの　リセット

            if (GameMgr.Contest_Cate_Ranking == 0)
            {
                if (GameMgr.ContestRestart_MainStart) //コンテスト再開する場合　〇回戦をここで指定
                {
                    GameMgr.ContestRoundNum = GameMgr.ContestRestart_contestRoundNum;
                }
                else
                {
                    //さらに何回戦かを初期設定
                    if (!GameMgr.System_ContestEdenFinalStart_ON)
                    {
                        GameMgr.ContestRoundNum = 1; //〇回戦　一回戦からスタート
                    }
                    else
                    {
                        GameMgr.ContestRoundNum = 3; //〇回戦　決勝戦スタート
                    }
                }
            }
            else
            {
                GameMgr.ContestRoundNum = 1;
            }

            //コンテスト開始前に、データなどの設定や初期化まとめ
            Contest_CommonReset();
            

            //コンテスト再開した場合、ここでもデータをセット。
            if (GameMgr.ContestRestart_MainStart)
            {
                GameMgr.ContestRestart_MainStart = false;

                for (i = 0; i < GameMgr.ContestRestart_contest_okashiNameList.Length; i++)
                {
                    if(GameMgr.ContestRestart_contest_okashiNameList[i] == "Non" || GameMgr.ContestRestart_contest_okashiNameList[i] == "")
                    { }
                    else {
                        GameMgr.contest_okashiNameList.Add(GameMgr.ContestRestart_contest_okashiNameList[i]);
                    }
                }               
            }

            //コンテスト開始時　データ読み込み後に、再開用データをリセット
            ContestRestart_DataReset();

            //scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをフェードイン
        }

        //二回戦以降、始まる場合の処理　Utage_scenarioの採点後にフラグをたてている。
        if(GameMgr.Contest_Next_flag)
        {
            GameMgr.Contest_Next_flag = false;
            pitemlist.deleteAllExtremePanelItem(); //先ほど提出したお菓子を削除
            GameMgr.extremepanel_Koushin = true; //パネルの表示更新

            GameMgr.ContestRoundNum++;

            //コンテスト開始前に、データなどの設定や初期化まとめ
            Contest_CommonReset();           
                        
            //

            //scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをフェードイン
        }

        //決勝戦終了後、賞品獲得
        if(GameMgr.Contest_PrizeGet_flag)
        {
            GameMgr.Contest_PrizeGet_flag = false;

            //エデンコンの場合、初出場かそうでないかをチェック
            conteststartList_database.EdenFirstVictoryCheck(GameMgr.Contest_Name);

            //エデンコンの場合、コンテストで優勝したというフラグをたてる　ハートルートはこれの有無で封鎖する
            conteststartList_database.EdenContestVictorySet(GameMgr.Contest_Name);

            Contest_PrizeGetScene = true;
            contestPrizePanel.SetActive(false); //ランキング戦で一回表示してる可能性があるので、一度オフ
            contestPrizeScore_dataBase.PrizeGet(); //アイテム獲得

            //そのコンテストの順位を更新する。1位と2位は、名前の横に王冠がでる。
            conteststartList_database.SetContestVictroyString(GameMgr.Contest_Name, GameMgr.contest_Rank_Count);       

            //優勝した場合、そのコンテスト優勝時のアイテムデータを記録
            if(GameMgr.contest_Rank_Count == 1)
            {
                GameMgr.Contest_tempSubmitItemData.ContestVictory_Score = GameMgr.contest_TotalScore;
                conteststartList_database.SetVictoryItemData(GameMgr.Contest_Name, GameMgr.Contest_tempSubmitItemData);
            }

            GameMgr.scenario_ON = true;

            sceneBGM.MuteBGM();

            GameMgr.contest_or_prizeget_flag = true;
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;

            scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをフェードイン
        }        

        //コンテスト終了　会場外へでる。時間過ぎて失格もここを通る。
        if (GameMgr.contest_eventEnd_flag)
        {
            GameMgr.contest_eventEnd_flag = false;
            GameMgr.Contest_ON = false;
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;

            //支給されたアイテムはここで削除
            PlayerItem_Delete_Return();

            //通常コンテストで一位以外のとき、再戦できるように保存
            if(GameMgr.contest_Rank_Count != 1)
            {
                ContestRestartDataSave(1);
            }

            //家に帰って寝る
            time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day, 20, 0); //20時終了
            GameMgr.Contest_afterHomeEventFlag = true;           
            GameMgr.Contest_afterHomeHeartUpFlag = true; //コンテスト終了後にハートが上がるフラグ     

            Contest_FlagRelease();           

            FadeManager.Instance.LoadScene("Or_Compound", 0.3f);
        }

        //コンテスト終了　トーナメント形式コンテストで負けた
        if (GameMgr.contest_eventEdenLoser_flag)
        {
            GameMgr.contest_eventEdenLoser_flag = false;
            GameMgr.Contest_ON = false;
            GameMgr.contest_MainMatchStart = false;
            PlayerStatus.player_contest_second = 0;

            //支給されたアイテムはここで削除
            PlayerItem_Delete_Return();

            //負けたとき、途中再開できるように保存
            ContestRestartDataSave(1);

            if (!GameMgr.System_ContestGameOver_ON)
            {              
                //家に帰って寝る
                time_controller.SetCullentDayTime(PlayerStatus.player_cullent_month, PlayerStatus.player_cullent_day, 20, 0); //20時終了
                GameMgr.Contest_afterHomeEventFlag = true;
                GameMgr.Contest_afterHomeHeartUpFlag = true; //コンテスト終了後にハートが上がるフラグ

                FadeManager.Instance.LoadScene("Or_Compound", 0.3f);
                //FadeManager.Instance.LoadScene("Or_Outside_the_Contest", 0.3f);
            }
            else
            {
                GameMgr.SceneSelectNum = 0;
                FadeManager.Instance.LoadScene("999_Gameover", 0.3f);
            }
        }

        //制限時間を少し超えた場合、注意のパネルがでる
        if (GameMgr.contest_LimitTimeOver_DegScore_flag)
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

            //会場の背景を表示
            if(Contest_PrizeGetScene)
            {
                BG_contest_chuubou.SetActive(false);
                BG_contest_hall.SetActive(false);
                BG_contest_prizeget.SetActive(true);
            }
            else
            {
                BG_contest_chuubou.SetActive(false);
                BG_contest_hall.SetActive(true);
                //ContestHall_Select();
                BG_contest_prizeget.SetActive(false);
            }
            

            if (GameMgr.Utage_Prizepanel_ON)
            {
                GameMgr.Utage_Prizepanel_ON = false;
                GameMgr.Utage_Prizepanel_WaitHyouji = false;

                StartCoroutine("WaitForRankingPanelOn");                
            }

            if (GameMgr.Utage_Prizepanel_OFF)
            {
                GameMgr.Utage_Prizepanel_OFF = false;
                GameMgr.Utage_Prizepanel_WaitHyouji = false;

                contestPrizePanel.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(OffPrizePanelactive);
            }

            if (GameMgr.Utage_SceneStart_BlackOFF)
            {
                GameMgr.Utage_SceneStart_BlackOFF = false;
                scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをオフ
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

                    //厨房の背景を表示
                    BG_contest_chuubou.SetActive(true);
                    BG_contest_hall.SetActive(false);

                    text_area.SetActive(true);
                    contest_select.SetActive(true);
                    hinttaste_toggle.SetActive(true);
                    //contest_startbutton_panel.SetActive(true);

                    yes_no_panel.SetActive(false);
                    yes_no_giveup_panel.SetActive(false);
                    yes_no_submit_panel.SetActive(false);
                    yes_no_namekakunin_panel.SetActive(false);
                    namesetting_panel.SetActive(false);
                    mainUI_panel.SetActive(true);
                    sceneBGM.MuteOFFBGM();

                    _model_move.SetActive(true);
                    live2d_animator.SetLayerWeight(3, 0.0f); //宴用表情はオフにしておく。
                    if (GameMgr.CompoAfter_BackGirl) //戻り中の間はタッチはできない girl1_status内でもUpdateでオフにしている。効力強い。
                    {
                        GameMgr.CharacterTouch_ALLOFF = true; //
                    }
                    else
                    {
                        GameMgr.CharacterTouch_ALLON = true; //タッチもオンにする。
                    }
                    
                    girl1_status.IdleMotionReset(0); //コンテスト用アイドルモーションにリセット 0は即時切り替え

                    GameMgr.compound_select = 0; //何もしていない状態
                    GameMgr.compound_status = 0;

                    GameMgr.Scene_Status = 100;
                    GameMgr.Scene_Select = 0;

                    //エクストリームパネル表示更新
                    GameMgr.extremepanel_Koushin = true;

                    if (!StartRead) //コンテスト開始時最初だけ読み込む
                    {
                        sceneBGM.MuteBGM();

                        Debug.Log("ContestMainOrA1 StartRead ON");
                        StartRead = true;                     
                        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f);

                        contestFirstEnshutuPanel.SetActive(true);
                        contestFirstEnshutuPanel.GetComponent<ContestFirstEnshutuPanel>().SetContestName();
                        contestFirstEnshutuPanel.GetComponent<ContestFirstEnshutuPanel>().SetOnEnshutuStart();

                        mainUI_panel.GetComponent<CanvasGroup>().DOFade(0, 0.0f);
                        mainUI_panel.GetComponent<CanvasGroup>().interactable = false;
                        text_area.GetComponent<CanvasGroup>().DOFade(0, 0.0f);

                        girl1_status.SetMotion_ContestBefore();

                        GameMgr.Scene_Status = 1000;
                        GameMgr.Scene_Select = 0;


                        GameMgr.ContestStartEnshutu_Flag = true;
                        StartCoroutine("StartEnshutu");
                    }                                                        

                    //制限時間　60分を超えた場合、失格フラグ
                    if (GameMgr.contest_LimitTimeOver_Gameover_flag)
                    {
                        GameMgr.contest_LimitTimeOver_Gameover_flag = false;

                        timelimitover_panel.SetActive(true);
                        LimitTimeOver();
                    }

                    GameMgr.Status_zero_readOK = true;

                    text_default();

                    break;

                case 10: //「あげる」を選択

                    GameMgr.Scene_Status = 13; //あげるシーンに入っています、というフラグ
                    GameMgr.Scene_Select = 10; //あげるを選択

                    yes_no_panel.SetActive(true);
                    yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);
                    black_panel_A.SetActive(true);
                    contest_select.SetActive(false);
                    hinttaste_toggle.SetActive(false);

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

                case 20: //名前を決め中

                    break;

                case 100: //退避

                    break;

                case 250: //お菓子ヒントボタンおした

                    GameMgr.compound_status = 251;
                    GameMgr.compound_select = 250;

                    //腹減りカウント一時停止
                    girl1_status.GirlEatJudgecounter_OFF();

                    //extreme_panel.LifeAnimeOnFalse(); //HP減少一時停止

                    okashihint_panel.SetActive(true); //お菓子ヒントパネルを表示。

                    mainUI_panel.SetActive(false);

                    break;

                case 251: //お菓子ヒント画面選択中

                    break;

                case 500: //調合用

                    contest_select.SetActive(false);
                    hinttaste_toggle.SetActive(false);

                    //調合終了まち
                    if (GameMgr.CompoundSceneStartON == false)
                    {
                        GameMgr.compound_select = 0; //何もしていない状態
                        GameMgr.compound_status = 0;

                        GameMgr.Scene_Status = 0;
                        GameMgr.Scene_Select = 0;
                    }
                    break;


                case 1000: //最初のコンテスト演出中

                    GameMgr.CharacterTouch_ALLOFF = true; //
                    UITouch_ALLOFF();
                    break;

                default:

                    break;
            }
        }
    }

    void Contest_FlagRelease()
    {
        //初回コンテストで、クッキー優勝した場合フラグがたつ
        if (GameMgr.Contest_Name == "Or_Contest_010" &&
            GameMgr.contest_Rank_Count == 1 && !GameMgr.NPCMagic_eventList[0])
        {
            GameMgr.Contest_Cookie_VictoryHoleinOne = true;
        }

        //夏コンと秋コンで優勝フラグもとっておく。フラグの取得順によって、お話が変わる場合あり。
        if (GameMgr.Contest_Name == "Or_Contest_002" && GameMgr.contest_Rank_Count == 1)
        {
            GameMgr.contest_summer_edenVitory = true;
        }
        if (GameMgr.Contest_Name == "Or_Contest_003" && GameMgr.contest_Rank_Count == 1)
        {
            GameMgr.contest_autumn_edenVitory = true;
        }
    }

    void Contest_CommonReset()
    {
        StartSetReset();
        ContestDataSetting(); //重要　コンテストのデータセッティング
       
        sceneBGM.MuteBGM();

        //会場ホールのBGMは宴で鳴らしてるので、Utage_Scenarioの「SubRoutine」で選択する。
        //コンテスト中BGMは、スクリプトのBGM.cs
        GameMgr.contest_event_num = GameMgr.ContestSelectNum;

        if (GameMgr.ContestRestart_MainStart) //再戦のときは、いきなりスタートする
        {
            if(GameMgr.ContestThemeSelectUse) //ただし、途中で課題セレクトがある試合は、そこまでスキップして選択できる
            {
                GameMgr.scenario_ON = true;
                GameMgr.contest_or_event_flag = true;
                GameMgr.ContestRestart_ThemeSelectJump = true;
            }
            else
            {
                GameMgr.ContestRestart_ThemeSelectJump = false;
            }
        }
        else
        {
            GameMgr.scenario_ON = true;
            GameMgr.contest_or_event_flag = true;
        }

        GameMgr.contest_MainMatchStart = false;
        PlayerStatus.player_contest_second = 0;

        //名前欄を空白に
        nameplate_text.text = "";
        inputField_okashiname.text = "";

        //MPは全回復
        PlayerStatus.player_mp = PlayerStatus.player_maxmp;

        //プレイヤーステータスのリセット
        PlayerStatus.ResetPlayerMagicStatus();

        //もし、決勝戦のみで背景などを変える場合は、ここで直接指定する
        ContestFinal_BGChange();
    }

    void StartSetReset()
    {
        GameMgr.contest_LimitTimeOver_DegScore_flag = false;
        timelimitover_panel.SetActive(false);

        GameMgr.contest_LimitTimeOver_After_flag = false;
        GameMgr.contest_Disqualification = false;
        GameMgr.contest_Disqualification2 = false;

        GameMgr.ContestThemeSelectUse = false;
        GameMgr.ContestThemeSelectNum = 0;
        GameMgr.ContestThemeCount = 0;
        GameMgr.ContestRestart_ThemeSelectJump = false;

    }

    void ContestRestart_DataReset()
    {
        //コンテスト再開フラグ関係も開始時に一度リセット
        GameMgr.ContestRestart_contestname = "";
        GameMgr.ContestRestart_contestnameHyouji = "";
        GameMgr.ContestRestart_contestRankType = 0;
        GameMgr.ContestRestart_contestRoundNum = 1;       
        GameMgr.ContestRestart_contestRoundNumMax = 1;
        for (i = 0; i < GameMgr.ContestRestart_contest_okashiNameList.Length; i++)
        {
            GameMgr.ContestRestart_contest_okashiNameList[i] = "Non";
        }
        GameMgr.ContestRestart_Giveup_flag = false; //再開用のフラグ 
    }

    //特定のコンテスト決勝戦でもし背景を変える場合、ここで指定する
    void ContestFinal_BGChange()
    {
        if (GameMgr.ContestRoundNum == GameMgr.ContestRoundNumMax)
        {
            if (GameMgr.Contest_Name == "Or_Contest_001" || GameMgr.Contest_Name == "Or_Contest_002" || GameMgr.Contest_Name == "Or_Contest_003"
                || GameMgr.Contest_Name == "Or_Contest_004")
            {
                GameMgr.Contest_BGMSelect = GameMgr.Contest_BGMSelectFinal; //sound38
                ContestHall_Select(GameMgr.Contest_HallBGNameFinal, GameMgr.Contest_ChubouBGNameFinal);
            }
        }
    }

    IEnumerator StartEnshutu()
    {
        while (GameMgr.ContestStartEnshutu_Flag == true)
        {
            yield return null; // オンクリックがfalseになるまでは、とりあえず待機
        }

        Debug.Log("コンテスト最初演出終了　コンテストスタート！");

        //演出が終了　BGMなど始まる
        sceneBGM.PlayContestStartBGM();
        sceneBGM.NowFadeVolumeONBGM();
        sceneBGM.MuteOFFBGM();

        girl1_status.IdleMotionReset(1); //コンテスト用アイドルモーションにリセット
        girl1_status.GirlEat_Judge_on = true;

        
        GameMgr.contest_MainMatchStart = true; //本戦開始の合図　TimeControllerで時間が進み始める

        GameMgr.Scene_Status = 100;
        GameMgr.Scene_Select = 0;

        GameMgr.CharacterTouch_ALLON = true; //タッチもオンにする。

        mainUI_panel.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        mainUI_panel.GetComponent<CanvasGroup>().interactable = true;
        text_area.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

        UITouch_ALLON();
    }

    void UITouch_ALLOFF()
    {
        mainUI_panel.transform.Find("HintTaste_Toggle").GetComponent<Toggle>().interactable = false;
        mainUI_panel.transform.Find("HintTaste_Toggle").GetComponent<Sound_Trigger>().se_sound_ON = false;
        mainUI_panel.transform.Find("ExtremePanel/Comp/ExtremeButton").GetComponent<Button>().interactable = false;
        mainUI_panel.transform.Find("ExtremePanel/Comp/ExtremeButton").GetComponent<Sound_Trigger>().se_sound_ON = false;        
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_01").GetComponent<Toggle>().interactable = false;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_01").GetComponent<Sound_Trigger>().se_sound_ON = false;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_GiveUp").GetComponent<Toggle>().interactable = false;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_GiveUp").GetComponent<Sound_Trigger>().se_sound_ON = false;
        mainUI_panel.transform.Find("ContestStartButtonPanel/ContestStartButton/TestStartButton").GetComponent<Button>().interactable = false;
        mainUI_panel.transform.Find("ContestStartButtonPanel/ContestStartButton/TestStartButton").GetComponent<Sound_Trigger>().se_sound_ON = false;
    }

    void UITouch_ALLON()
    {
        mainUI_panel.transform.Find("HintTaste_Toggle").GetComponent<Toggle>().interactable = true;
        mainUI_panel.transform.Find("HintTaste_Toggle").GetComponent<Sound_Trigger>().se_sound_ON = true;
        mainUI_panel.transform.Find("ExtremePanel/Comp/ExtremeButton").GetComponent<Button>().interactable = true;
        mainUI_panel.transform.Find("ExtremePanel/Comp/ExtremeButton").GetComponent<Sound_Trigger>().se_sound_ON = true;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_01").GetComponent<Toggle>().interactable = true;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_01").GetComponent<Sound_Trigger>().se_sound_ON = true;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_GiveUp").GetComponent<Toggle>().interactable = true;
        mainUI_panel.transform.Find("Contest_Select/Viewport/Content/ContestOn_Toggle_GiveUp").GetComponent<Sound_Trigger>().se_sound_ON = true;
        mainUI_panel.transform.Find("ContestStartButtonPanel/ContestStartButton/TestStartButton").GetComponent<Button>().interactable = true;
        mainUI_panel.transform.Find("ContestStartButtonPanel/ContestStartButton/TestStartButton").GetComponent<Sound_Trigger>().se_sound_ON = true;
    }

    //コンテストごとに、会場風景が変わる。
    void ContestHall_Select(string _hallname, string _chuubouname)
    {
        for(i=0; i< BG_contest_hall_List.Count; i++)
        {
            BG_contest_hall_List[i].SetActive(false);
        }
        for (i = 0; i < BG_contest_chuubou_List.Count; i++)
        {
            BG_contest_chuubou_List[i].SetActive(false);
        }

        contestBG_OK = false;
        i = 0;
        while (i < BG_contest_hall_List.Count)
        {
            if (BG_contest_hall_List[i].name == _hallname)
            {
                BG_contest_hall_List[i].SetActive(true);
                contestBG_OK = true;
                break;
            }
            i++;
        }

        contestBG_chuubouOK = false;
        i = 0;
        while (i < BG_contest_chuubou_List.Count)
        {
            if (BG_contest_chuubou_List[i].name == _chuubouname)
            {
                BG_contest_chuubou_List[i].SetActive(true);
                contestBG_chuubouOK = true;
                break;
            }
            i++;
        }

        if (!contestBG_OK) //例外処理　もし一致する背景がなかった場合　真っ黒になってしまうので、デフォルトを指定
        {
            BG_contest_hall_List[0].SetActive(true);
        }
        if (!contestBG_chuubouOK) //例外処理　もし一致する背景がなかった場合　真っ黒になってしまうので、デフォルトを指定
        {
            BG_contest_chuubou_List[0].SetActive(true);
        }
    }

    void OnPrizePanelactive()
    {
        contestPrizePanel.GetComponent<GraphicRaycaster>().enabled = false; //宴が触れるように。
                                                                            //scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        GameMgr.Utage_Prizepanel_WaitHyouji = true;
    }

    void OffPrizePanelactive()
    {
        contestPrizePanel.GetComponent<GraphicRaycaster>().enabled = true; //
        contestPrizePanel.SetActive(false);
        GameMgr.Utage_Prizepanel_WaitHyouji = true;
    }

    IEnumerator WaitForRankingPanelOn()
    {
        yield return new WaitForSeconds(0.5f); //少し待つ

        contestPrizePanel.GetComponent<CanvasGroup>().alpha = 0;
        contestPrizePanel.SetActive(true);
        
        //contestPrizePanel.GetComponent<CanvasGroup>().DOFade(0, 0.0f);
        contestPrizePanel.GetComponent<CanvasGroup>().DOFade(1, 1.0f).OnComplete(OnPrizePanelactive);
    }

    //各コンテストのデータの初期設定
    void ContestDataSetting()
    {
        //DBで初期設定を行っている
        conteststartList_database.ContestSetting();

        //コンテスト場所に応じて背景を設定

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
            _text.text = GameMgr.Contest_ProblemSentence + "\n" + GameMgr.Contest_ProblemSentence2;
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
                time_controller.SetMinuteToHourContest(15, 0, false);

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

    public void OnTasteHint_Toggle() //お菓子ヒントボタンを押した
    {
        if (hinttaste_toggle.GetComponent<Toggle>().isOn == true)
        {
            hinttaste_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            GameMgr.Scene_Status = 250;

        }
    }


    public void OnCheck_GiveUp() //諦める
    {
        if (conteston_toggle_giveup.GetComponent<Toggle>().isOn == true)
        {
            conteston_toggle_giveup.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            yes_no_giveup_panel.SetActive(true);
            black_panel_A.SetActive(true);
            contest_select.SetActive(false);
            hinttaste_toggle.SetActive(false);

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
        PlayerItem_Delete_Return();

        //MPは全回復
        PlayerStatus.player_mp = PlayerStatus.player_maxmp;

        //あきらめた場合、出場してたコンテストのデータを保存し、フラグたてる。これはセーブされ、次回再開できる。
        ContestRestartDataSave(0);       

        StartCoroutine("WaitForGiveUpContest");
    }

    void ContestRestartDataSave(int _bunki)
    {
        GameMgr.ContestRestart_contestname = GameMgr.Contest_Name;
        GameMgr.ContestRestart_contestnameHyouji = GameMgr.Contest_NameHyouji;
        GameMgr.ContestRestart_contestRankType = GameMgr.Contest_Cate_Ranking;
        GameMgr.ContestRestart_contestRoundNum = GameMgr.ContestRoundNum;
        GameMgr.ContestRestart_contestRoundNumMax = GameMgr.ContestRoundNumMax;

        if (GameMgr.Contest_Cate_Ranking == 0) //トーナメント形式のみの処理
        {
            if (_bunki == 1) //負けた場合は、その回の提出したお菓子自体は、保存しない
            {
                GameMgr.contest_okashiNameList.RemoveAt(GameMgr.contest_okashiNameList.Count - 1);
            }

            if (GameMgr.contest_okashiNameList.Count > 0)
            {
                i = 0;
                foreach (string _name in GameMgr.contest_okashiNameList)
                {
                    Debug.Log("GameMgr.contest_okashiNameList: " + _name);
                    GameMgr.ContestRestart_contest_okashiNameList[i] = _name;
                    i++;
                }
            }
        }

        GameMgr.ContestRestart_Giveup_flag = true; //再開用のフラグ 
        GameMgr.ContestRestart_Giveup_flagNum = _bunki;
    }

    IEnumerator WaitForGiveUpContest()
    {
        yield return new WaitForSeconds(2.0f); //1秒待つ

        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.Contest_ON = false;

        FadeManager.Instance.LoadScene("Or_Compound", 0.3f);
        //FadeManager.Instance.LoadScene("Or_Outside_the_Contest", 0.3f);
    }

    //支給アイテム削除や一時預かりアイテムを返す処理
    void PlayerItem_Delete_Return()
    {
        //支給されたアイテムはここで削除
        pitemlist.DeleteContestSurppliedItem();

        //素材持ち込み不可などの場合、アイテムを返してくれる
        if (GameMgr.Contest_BringType != 0) //
        {
            pitemlist.Contest_ReturnKeepItem();
        }
    }


    //審査員におかしを提出する
    public void OnContestJudge_Start()
    {
        yes_no_submit_panel.SetActive(true);
        black_panel_A.SetActive(true);
        contest_select.SetActive(false);
        hinttaste_toggle.SetActive(false);

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
            yes_no_submit_panel.transform.Find("Yes_TeiShutu").GetComponent<Button>().interactable = false;
            yes_no_submit_panel.transform.Find("Yes_TeiShutu").GetComponent<Sound_Trigger>().enabled = false;
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

        
        switch (yes_selectitem_kettei.kettei1)
        {
            case true:

                yes_no_submit_panel.SetActive(false);
                yes_no_namekakunin_panel.SetActive(true); //名前確認にとぶ

                if (girl1_status.GirlGokigenStatus < 6)
                {
                    _text.text = "にいちゃん！　おかしに名前をつける？";
                }
                else if (girl1_status.GirlGokigenStatus >= 6 && girl1_status.GirlGokigenStatus < 9)
                {
                    _text.text = "にいちゃん！　せっかくだから、おかしに名前つけたいな！";
                }
                else if (girl1_status.GirlGokigenStatus >= 9 && girl1_status.GirlGokigenStatus < 12)
                {
                    _text.text = "にいちゃんが作ったおかし.." + "\n" + "名前つけてあげたいな！";
                }
                else if (girl1_status.GirlGokigenStatus >= 12)
                {
                    _text.text = "にいちゃん..。 このおかしに、名前をつけてあげて！";
                }

                default_itemName = pitemlist.player_extremepanel_itemlist[0].item_FullName;
                conteston_toggle_nameok.GetComponent<Button>().interactable = false;

                StartCoroutine("NameSet_select");
                break;

            case false:

                black_panel_A.SetActive(false);
                //Debug.Log("cancel");

                //_textmain.text = "";
                GameMgr.Scene_Status = 0;
                yes_no_submit_panel.SetActive(false);
                yes_no_submit_panel.transform.Find("Yes_TeiShutu").GetComponent<Button>().interactable = true;
                yes_no_submit_panel.transform.Find("Yes_TeiShutu").GetComponent<Sound_Trigger>().enabled = true;

                break;

        }
    }

    //名前を決定するか確認中　ただし、「そのまま」をおすか「キャンセル」か入力されるまでここで待つ　「名前を決める」をおすと、さらに中に進み、「決定」をおされるまで待つ
    IEnumerator NameSet_select()
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

                yes_no_namekakunin_panel.SetActive(false);
                namesetting_panel.SetActive(false);

                sceneBGM.MuteBGM();
                scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
                scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

                GameMgr.contest_event_num = GameMgr.ContestSelectNum;
                GameMgr.Contest_tempSubmitItemData = pitemlist.player_extremepanel_itemlist[0];

                Debug.Log("作品名（デフォルト）: " + GameMgr.Contest_tempSubmitItemData.itemNameHyouji);
                Debug.Log("作品名（ユーザー入力）: " + GameMgr.Contest_tempSubmitItemData.user_customname);

                StartCoroutine("WaitForJudge");

                break;

            case false:

                //Debug.Log("cancel");

                //_textmain.text = "";
                GameMgr.Scene_Status = 0;
                namesetting_panel.SetActive(false);
                yes_no_namekakunin_panel.SetActive(false);
                yes_no_submit_panel.SetActive(false);
                yes_no_submit_panel.transform.Find("Yes_TeiShutu").GetComponent<Button>().interactable = true;
                yes_no_submit_panel.transform.Find("Yes_TeiShutu").GetComponent<Sound_Trigger>().enabled = true;

                break;

        }
    }

    IEnumerator WaitForJudge()
    {
        yield return new WaitForSeconds(2.0f); //2秒待つ

        //お菓子を採点する
        contest_judge.Contest_Judge_Start(0);

        //パネルのお菓子を削除
        pitemlist.deleteExtremePanelItem(0, 1);

        GameMgr.contest_LimitTimeOver_DegScore_flag = false; //採点後にオフにする。
        GameMgr.scenario_ON = true;
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.contest_or_contestjudge_flag = true;

        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをフェードイン
    }



    //「名前を決める」ボタンをおした
    public void OnNameSettingButton()
    {
        GameMgr.Scene_Status = 20;

        namesetting_panel.SetActive(true);
        yes_no_namekakunin_panel.SetActive(false);

        okashi_img.sprite = pitemlist.player_extremepanel_itemlist[0].itemIcon_sprite;
    }

    //名前を入力しEnterをおした　もしくはOKボタン　名前の確定
    public void OnSubmitNameInput()
    {
        nameplate_text.text = inputField_okashiname.text;
        kettei_itemName = inputField_okashiname.text;

        conteston_toggle_nameok.GetComponent<Button>().interactable = true;
    }

    //名前を入力し終えて、これで提出をおした
    public void OnOK_NameSetting()
    {
        //nameplate_text.text = inputField_okashiname.text;
        kettei_itemName = inputField_okashiname.text;

        //提出ネームの決定
        pitemlist.player_extremepanel_itemlist[0].user_customname = kettei_itemName;        

        //Yesをおしたのと一緒
        yes_selectitem_kettei.onclick = true;
        yes_selectitem_kettei.kettei1 = true;
    }

    //名前入力画面でやめるをおした
    public void OnCancel_NameSetting()
    {
        namesetting_panel.SetActive(false);
        yes_no_namekakunin_panel.SetActive(true);
    }

    //名前入力画面でデフォルト名に戻すをおした
    public void OnDefaultReset_NameSetting()
    {
        nameplate_text.text = default_itemName;
        kettei_itemName = "";
        pitemlist.player_extremepanel_itemlist[0].user_customname = "";
        inputField_okashiname.text = "";

        conteston_toggle_nameok.GetComponent<Button>().interactable = true; //カスタムネーム内は空だけど、ここからも通常通り提出できる
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
        yield return new WaitForSeconds(2.0f); //1秒待つ

        GameMgr.scenario_ON = true;
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;
        GameMgr.contest_or_limittimeover_flag = true;

        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをフェードイン
    }

   
  
    //デバッグ用
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
        contest_judge.Contest_Judge_Start(9999);
    }

    //デバッグ用　プライズ画面へすぐ飛ぶ　一位で優勝したことにする
    public void OnDebugContest_GetPrize_AfterSkipButton()
    {
        GameMgr.Contest_ON = false;

        Debug.Log("コンテスト　本戦終了！！");
        GameMgr.contest_Rank_Count = 1;

        //GameMgr.contest_TotalScoreList.Clear();
        GameMgr.contest_okashiNameList.Clear(); //提出したお菓子を各回ごとに記録したもの　リセット

        if (GameMgr.Contest_Cate_Ranking == 0)
        {
            GameMgr.ContestRoundNum = 3; //〇回戦　決勝戦スタートということにする
        }
        else
        {
            GameMgr.ContestRoundNum = 1;
        }

        StartSetReset();

        //DBで初期設定を行っている
        conteststartList_database.ContestSetting();

        sceneBGM.MuteBGM();
        scene_black_effect.GetComponent<CanvasGroup>().DOFade(1, 1.0f);
        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = true;

        GameMgr.contest_event_num = GameMgr.ContestSelectNum;
        GameMgr.Contest_tempSubmitItemData = database.items[0];

        StartCoroutine("WaitForDebugPrizeSkip");
    }

    IEnumerator WaitForDebugPrizeSkip()
    {
        yield return new WaitForSeconds(2.0f); //2秒待つ

        GameMgr.Contest_PrizeGet_flag = true;

        //お菓子を採点する
        //contest_judge.Contest_Judge_Start(0);

        //パネルのお菓子を削除
        //pitemlist.deleteExtremePanelItem(0, 1);

        scene_black_effect.GetComponent<GraphicRaycaster>().enabled = false;

        scene_black_effect.GetComponent<CanvasGroup>().DOFade(0, 1.0f); //ブラックをフェードイン
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

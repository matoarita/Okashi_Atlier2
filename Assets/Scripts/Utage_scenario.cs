﻿using UnityEngine;
using System.Collections;
using Utage;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Utage_scenario : MonoBehaviour
{

    AdvEngine Engine { get { return engine ?? (engine = FindObjectOfType<AdvEngine>()); } }
    public AdvEngine engine;
    public string scenarioLabel; //シナリオラベルを定義しておく必要がある

    private bool scenario_loading;

    private int event_ID;
    private int recipi_read_ID;
    private int itemuse_recipi_ID;
    private int map_ev_ID;
    private int Okashicomment_ID;
    private int sp_Okashi_ID;
    private int mainClear_ID;
    private int touchhint_ID;

    private int GirlLoveEvent_num;

    private int story_num;
    private int shop_talk_number;
    private int shop_uwasa_number;
    private int shop_hint_number;
    private int hiroba_num;
    private int hiroba_endflag_num;
    private int contest_num;

    private int re_flag;
    private int ev_flag;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject canvas;

    private GameObject playeritemlist_onoff;
    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private ItemMatPlaceDataBase matplace_database;
    private ContestCommentDataBase databaseContestComment;
    private Contest_Main contest_main;
    private Girl1_status girl1_status; //女の子１のステータスを取得。    
    private MoneyStatus_Controller moneyStatus_Controller;
    private EmeraldShop_Main emeraldshop_main;
    private Exp_Controller exp_Controller;

    private GameObject GirlEat_judge_obj;
    private GirlEat_Judge girlEat_judge;

    private int i, j;
    private string recipi_Name;
    private int CommentID;
    private int judge_num; //審査員の番号
    private bool SpecialItemFlag;
    private int total_score;
    private bool eventend_flag;

    private bool tutorial_flag;
    private int catgrave_flag;

    private bool FadeAnim_flag;
    private int FadeAnim_status;

    private bool yusho_flag; //優勝したかどうか。

    //Live2Dモデルの取得
    private CubismModel _model;
    private CubismRenderController _renderController;
    private Animator live2d_animator;

    //シーン中のキャラクタ画像を取得（NPCなど）
    private GameObject character;

    //BGMの取得
    private BGM sceneBGM;
    private Map_Ambience map_ambience;

    //宴のマスター音量の取得
    private GameObject utagesoundmanager_obj;
    private SoundManager utagesoundmanager;


    // Use this for initialization
    void Start()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //コンテスト感想データベースの取得
        databaseContestComment = ContestCommentDataBase.Instance.GetComponent<ContestCommentDataBase>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        utagesoundmanager_obj = GameObject.FindWithTag("UtageManageres").gameObject;

        scenario_loading = false; //「Utage」シーンを最初に読み込むときに、falseに初期化。宴のシナリオを読み中はtrue。コルーチンのリセットを回避する。       
    }

    void Update()
    {
        if(utagesoundmanager == null)
        {
            utagesoundmanager_obj = GameObject.FindWithTag("UtageManageres").gameObject;
            utagesoundmanager = utagesoundmanager_obj.transform.Find("SoundManager").GetComponent<SoundManager>();
        }

        utagesoundmanager.MasterVolume = GameMgr.MasterVolumeParam;

        //フェードアニメ用
        if (FadeAnim_flag)
        {
            //1のときはOFF
            if (FadeAnim_status == 1)
            {
                _renderController.Opacity -= 0.5f;

                if (_renderController.Opacity <= 0.0f)
                {
                    _renderController.Opacity = 0.0f;
                    FadeAnim_flag = false;
                }
            }
        }

        if (!scenario_loading) // scenario_loading=false のときは、中の処理を実行する。
        {
            //シナリオに関する処理
            if (SceneManager.GetActiveScene().name == "010_Prologue")
            { // hogehogeシーンでのみやりたい処理

                switch (GameMgr.scenario_flag)
                {
                    case 0:
                       
                        scenarioLabel = "Prologue";
                        story_num = GameMgr.scenario_flag;
                        StartCoroutine(Scenario_Start());
                        break;

                    default:
                        break;
                }

            }

            //1話はプロローグからの流れで自動で始まる。

            if (SceneManager.GetActiveScene().name == "020_Stage2")
            {

                switch (GameMgr.scenario_flag)
                {

                    case 2000: //2話はじまり
                       
                        scenarioLabel = "Chapter_2";
                        story_num = GameMgr.scenario_flag;
                        StartCoroutine(Scenario_Start());
                        break;

                    case 2900: //2話調合パート終了

                        break;

                    default:
                        break;
                }
                    
            }

            if (SceneManager.GetActiveScene().name == "030_Stage3")
            {

                switch (GameMgr.scenario_flag)
                {

                    case 3000: //1話はじまり

                        scenarioLabel = "Chapter_3";
                        story_num = GameMgr.scenario_flag;
                        StartCoroutine(Scenario_Start());
                        break;

                    case 3900: //1話調合パート終了

                        break;

                    default:
                        break;
                }

            }

            //調合シーンでのテキスト処理
            if (SceneManager.GetActiveScene().name == "Compound")
            {
                if (!_model)
                {
                    //Live2Dモデルの取得
                    _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();
                    _renderController = _model.GetComponent<CubismRenderController>();
                    live2d_animator = _model.GetComponent<Animator>();
                }

                if (!sceneBGM)
                {
                    //BGMの取得
                    sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
                    map_ambience = GameObject.FindWithTag("Map_Ambience").gameObject.GetComponent<Map_Ambience>();
                }

                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();


                switch (GameMgr.scenario_flag)
                {

                    case 110: //調合パート開始時にアトリエへ初めて入る。一番最初に工房へ来た時のセリフ。また、何を作ればよいかを指示してくれる。

                        if (!GameMgr.Beginner_flag[3]) //チュートリアルを読んだかどうか
                        {
                            GameMgr.Beginner_flag[3] = true;
                            scenarioLabel = "Tutorial";
                            StartCoroutine(Tutorial_Start());
                        }
                        else
                        {
                            GameMgr.scenario_ON = false;
                            GameMgr.tutorial_ON = false;
                            GameMgr.scenario_flag = 120;
                        }
                        break;

                    default:
                        break;
                }
                

                if(GameMgr.CompoundEvent_storyflag)
                {
                    GameMgr.CompoundEvent_storyflag = false;
                    
                    story_num = GameMgr.CompoundEvent_storynum;
                    StartCoroutine(backHome());
                }
                
                if (GameMgr.girlloveevent_flag)
                {
                    GameMgr.girlloveevent_flag = false;                    

                    //好感度イベントを表示
                    StartCoroutine(Girllove_event_Hyouji());
                }

                if (GameMgr.recipi_read_flag)
                {
                    GameMgr.recipi_read_flag = false;
                    recipi_read_ID = GameMgr.recipi_read_ID;

                    //レシピを手に入れて読むときの表示
                    StartCoroutine(Recipi_read_Hyouji());
                }

                if (GameMgr.itemuse_recipi_flag == true)
                {
                    GameMgr.itemuse_recipi_flag = false;
                    itemuse_recipi_ID = GameMgr.recipi_read_ID;

                    //イベントレシピを表示
                    StartCoroutine(ItemUse_Recipi_Hyouji());
                }

                if (GameMgr.map_event_flag == true)
                {
                    GameMgr.map_event_flag = false;
                    map_ev_ID = GameMgr.map_ev_ID;

                    //マップイベントのテキストを表示
                    StartCoroutine(MapEvent_Hyouji());
                }

                if (GameMgr.OkashiComment_flag == true)
                {
                    GameMgr.OkashiComment_flag = false;
                    Okashicomment_ID = GameMgr.OkashiComment_ID;

                    //お菓子食べた直後（採点表示パネル前）の通常感想テキストを表示
                    StartCoroutine(OkashiComment_Hyouji());
                }

                if (GameMgr.sp_okashi_hintflag == true)
                {
                    GameMgr.sp_okashi_hintflag = false;
                    sp_Okashi_ID = GameMgr.sp_okashi_ID;

                    //SPお菓子食べる前のふきだし
                    StartCoroutine(SpOkashiComment_HintHyouji());
                }

                if (GameMgr.sp_okashi_flag == true)
                {
                    GameMgr.sp_okashi_flag = false;
                    sp_Okashi_ID = GameMgr.sp_okashi_ID;

                    //SPお菓子食べたあとの感想テキストを表示
                    StartCoroutine(SpOkashiComment_Hyouji());
                }

                if (GameMgr.okashiafter_flag == true)
                {
                    GameMgr.okashiafter_flag = false;
                    sp_Okashi_ID = GameMgr.okashiafter_ID;

                    //お菓子食べたあとの感想（採点表示パネル後）のテキストを表示
                    StartCoroutine(OkashiAfterComment_Hyouji());
                }

                if (GameMgr.emeralDonguri_flag == true)
                {
                    GameMgr.emeralDonguri_flag = false;

                    //お菓子食べたあとの感想（採点表示パネル後）のテキストを表示
                    StartCoroutine(EmeralDonguri_Hyouji());
                }

                if (GameMgr.QuestClearButtonMessage_flag == true)
                {
                    GameMgr.QuestClearButtonMessage_flag = false;

                    //お菓子食べたあとの感想（採点表示パネル後）のテキストを表示
                    StartCoroutine(QuestClearButton_Hyouji());
                }
                

                if (GameMgr.mainClear_flag == true)
                {
                    GameMgr.mainClear_flag = false;
                    mainClear_ID = GameMgr.mainquest_ID;

                    //SPお菓子食べたあとの感想テキストを表示
                    StartCoroutine(MainQuestClear_Hyouji());
                }

                if (GameMgr.sleep_flag == true)
                {
                    GameMgr.sleep_flag = false;
                    scenarioLabel = "Sleep";

                    //寝るイベントを表示
                    StartCoroutine(Sleep());
                }

                if (GameMgr.touchhint_flag == true)
                {
                    GameMgr.touchhint_flag = false;
                    touchhint_ID = GameMgr.touchhint_ID;

                    //ヒントを表示
                    StartCoroutine(TouchHint_Hyouji());
                }
            }               

            //ショップ・牧場シーンでのイベント処理
            if (SceneManager.GetActiveScene().name == "Shop" || SceneManager.GetActiveScene().name == "Farm" || SceneManager.GetActiveScene().name == "Bar")
            {
                character = GameObject.FindWithTag("Character");
               
                if(GameMgr.shop_event_flag)
                {
                    GameMgr.shop_event_flag = false;
                    story_num = GameMgr.shop_event_num;
                    CharacterSpriteSetOFF();

                    scenarioLabel = "Shop_Event";
                    StartCoroutine(Scenario_Start());

                }

                if (GameMgr.shop_lvevent_flag)
                {
                    GameMgr.shop_lvevent_flag = false;
                    story_num = GameMgr.shop_lvevent_num;
                    CharacterSpriteSetOFF();

                    scenarioLabel = "Shop_LvEvent";
                    StartCoroutine(Scenario_Start());

                }

                if ( GameMgr.talk_flag == true )
                {
                    GameMgr.talk_flag = false;
                    shop_talk_number = GameMgr.talk_number;
                    StartCoroutine(Shop_Talk());

                }

                if (GameMgr.uwasa_flag == true)
                {
                    GameMgr.uwasa_flag = false;
                    shop_uwasa_number = GameMgr.uwasa_number;
                    StartCoroutine(Shop_Uwasa());

                }

                if ((GameMgr.shop_hint == true))
                {
                    GameMgr.shop_hint = false;
                    shop_hint_number = GameMgr.shop_hint_num;
                    StartCoroutine(Shop_Hint());
                }

                if (GameMgr.farm_event_flag)
                {
                    GameMgr.farm_event_flag = false;
                    story_num = GameMgr.farm_event_num;
                    CharacterSpriteSetOFF();

                    scenarioLabel = "Farm_Event";
                    StartCoroutine(Scenario_Start());

                }
            }

            //エメラルショップシーンでのイベント処理
            if (SceneManager.GetActiveScene().name == "Emerald_Shop")
            {
                character = GameObject.FindWithTag("Character");

                if (GameMgr.emeraldshop_event_flag)
                {
                    GameMgr.emeraldshop_event_flag = false;
                    story_num = GameMgr.emeraldshop_event_num;
                    CharacterSpriteSetOFF();

                    scenarioLabel = "emeraldShop_Event";
                    StartCoroutine(Emerald_Shop());

                }
            }

            //広場シーンでのイベント処理
            if (SceneManager.GetActiveScene().name == "Hiroba2")
            {
                //character = GameObject.FindWithTag("Character");               

                if (!sceneBGM)
                {
                    //BGMの取得
                    sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();
                }

                if (GameMgr.hiroba_event_flag)
                {
                    GameMgr.hiroba_event_flag = false;
                    hiroba_num = GameMgr.hiroba_event_ID;
                    //CharacterSpriteSetOFF();                    
                    
                    StartCoroutine(Hiroba_Event());

                }
                
            }

            //コンテストシーンでのイベント処理
            if (SceneManager.GetActiveScene().name == "Contest")
            {
                contest_main = GameObject.FindWithTag("contest_Main").GetComponent<Contest_Main>();

                if (GameMgr.contest_event_flag)
                {
                    GameMgr.contest_event_flag = false;
                    contest_num = GameMgr.contest_event_num;
                    //CharacterSpriteSetOFF();                    

                    StartCoroutine(Contest_Event());

                }
            }
        }
    }

    //
    // イベント・シナリオの読み処理。フラグ管理も行う。
    //
    IEnumerator Scenario_Start()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち
        
        engine.Param.TrySetParameter("Story_num", story_num);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario( scenarioLabel );

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }


        switch (GameMgr.scenario_flag)
        {
            case 0:

                GameMgr.scenario_flag = 100; //プロローグ終了。一話＝100。
                break;

            case 2000:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 2009;
                break;

            case 3000:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 3009;
                break;

            default:

                GameMgr.scenario_ON = false;
                break;
        }

        if (SceneManager.GetActiveScene().name == "Shop" || SceneManager.GetActiveScene().name == "Farm" || SceneManager.GetActiveScene().name == "Bar")
        {
            CharacterSpriteFadeON();

            GameMgr.scenario_ON = false;
        }

        scenario_loading = false;

        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ
    }

    //
    //テキスト表示するだけの処理（シナリオフラグの判定なども行わない）
    //
    IEnumerator Text_Read()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        scenario_loading = false;

        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ
    }

    //
    //寝る時の処理（シナリオフラグの判定なども行わない）
    //
    IEnumerator Sleep()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("Sleep_num", GameMgr.sleep_status);
        engine.Param.TrySetParameter("FoodExpenses", GameMgr.Foodexpenses);
        engine.Param.TrySetParameter("TodayFood", GameMgr.MgrTodayFood);

        //環境音は先にとめる。
        map_ambience.Mute();

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
       

        //音を止めて、宿屋のジングル
        sceneBGM.MuteBGM();

        //続きから再度読み込み
        engine.ResumeScenario();

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        //BGMを再開
        sceneBGM.MuteOFFBGM();
        map_ambience.MuteOFF();

        scenario_loading = false;

        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ
    }

    //
    // チュートリアルの処理
    //
    IEnumerator Tutorial_Start()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        
        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        tutorial_flag = (bool)engine.Param.GetParameter("Tutorial_Flag");

        if(tutorial_flag) //チュートリアルを見る場合
        {
            StartCoroutine(Tutorial_Start_Content());
        }
        else //見ない場合
        {
            switch (GameMgr.scenario_flag)
            {

                case 110:

                    GameMgr.scenario_ON = false;
                    GameMgr.scenario_flag = 120;
                    break;

                default:
                    break;
            }

            scenario_loading = false;
        }

        
    }

    //
    // チュートリアル開始した場合の中身の処理
    //
    IEnumerator Tutorial_Start_Content()
    {

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち        

        //「宴」のシナリオを呼び出す
        scenarioLabel = "Tutorial_Content";
        Engine.JumpScenario(scenarioLabel);

        //音切り替え
        sceneBGM.OnTutorialBGM();

        //キャラクタ切り替え
        CharacterLive2DImageOFF();

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        tutorial_flag = (bool)engine.Param.GetParameter("Tutorial_Flag");

        if (tutorial_flag) //作り方チュートリアルを見る場合
        {
            StartCoroutine(Tutorial_Make_Content());
        }
        else //見ない場合
        {
            switch (GameMgr.scenario_flag)
            {

                case 110:

                    GameMgr.scenario_ON = false;
                    GameMgr.tutorial_ON = false;
                    GameMgr.scenario_flag = 120;
                    break;

                default:
                    break;
            }

            //BGMを再開
            sceneBGM.OnMainBGM();

            //キャラクタイメージON
            CharacterLive2DImageON();

            scenario_loading = false;
        }
       
    }

    //
    // チュートリアル開始した場合の中身の処理
    //
    IEnumerator Tutorial_Make_Content()
    {

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //はいを押した時の処理。エクストリームパネルを表示する。コンテントも表示してもいいかもだけど、触れないようにしておく。
        GameMgr.tutorial_ON = true;
        GameMgr.tutorial_Num = 0;
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        scenarioLabel = "Tutorial_Content2";
        Engine.JumpScenario(scenarioLabel);

        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        //ゲームの再開処理を書く
        GameMgr.tutorial_Num = 10;
        CharacterLive2DImageON();

        while (!GameMgr.tutorial_Progress) //エクストリームパネルを押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        GameMgr.tutorial_Num = 15;

        //続きから再度読み込み
        engine.ResumeScenario();

        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        //ゲームの再開処理を書く
        GameMgr.tutorial_Num = 16;

        while (!GameMgr.tutorial_Progress) //オリジナル調合を押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();

        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        GameMgr.tutorial_Num = 30;

        while (!GameMgr.tutorial_Progress) //右のレシピメモ開き押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();



        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 50;

        while (!GameMgr.tutorial_Progress) //調合完了待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;


        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 70;

        while (!GameMgr.tutorial_Progress) //レシピ閃いた！ボタンの押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        CharacterLive2DImageOFF();

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 90;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 100;
        CharacterLive2DImageON();

        while (!GameMgr.tutorial_Progress) //採点表示パネルをオフにするボタンの押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        GameMgr.tutorial_Num = 110;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 120;
        CharacterLive2DImageOFF();

        while (!GameMgr.tutorial_Progress) //「オレンジネコクッキー」吹き出し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 140;
        CharacterLive2DImageON();

        while (!GameMgr.tutorial_Progress) //2度目エクストリームパネルの押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 160;

        while (!GameMgr.tutorial_Progress) //レシピから作るボタンの押し、調合完了待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 180;

        while (!GameMgr.tutorial_Progress) //元の画面に戻るのを待つ
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;
        CharacterLive2DImageOFF();

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 200;
        CharacterLive2DImageON();

        while (!GameMgr.tutorial_Progress) //パネルを触れるように。パネルを押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 220;

        while (!GameMgr.tutorial_Progress) //エクストリーム調合をポチ
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 240;

        while (!GameMgr.tutorial_Progress) //エクストリーム調合の、完了待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 260;

        while (!GameMgr.tutorial_Progress) //カード押して、元画面に戻る待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        //続きから再度読み込み
        engine.ResumeScenario();


        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }
        GameMgr.tutorial_Num = 280;

        while (!GameMgr.tutorial_Progress) //再度、採点表示パネルをオフにするボタンの押し待ち
        {
            yield return null;
        }
        GameMgr.tutorial_Progress = false;

        GameMgr.tutorial_Num = 290;

        CharacterLive2DImageOFF();

        //続きから再度読み込み
        engine.ResumeScenario();


        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        CharacterLive2DImageON();

        switch (GameMgr.scenario_flag)
        {

            case 110:

                GameMgr.scenario_ON = false;
                GameMgr.scenario_flag = 120;
                GameMgr.tutorial_ON = false;
                girl1_status.Girl1_Status_Init();
                girl1_status.OkashiNew_Status = 1;
                break;

            default:
                break;
        }

        //BGMを再開
        sceneBGM.OnMainBGM();

        scenario_loading = false;
    }

    //
    // 現状使用しない（シナリオ内でアイテムを獲得するスクリプト）
    //
    /*
    IEnumerator Chapter1_Start()
    {
        GameMgr.scenario_flag = 201; //アップデートを繰り返さないようにする。

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Chapter1_1_old";
   

        engine.Param.TrySetParameter("Kaeru_Coin", PlayerStatus.player_kaeru_coin);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);       

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        PlayerStatus.player_kaeru_coin = (int)engine.Param.GetParameter("Kaeru_Coin");

        pitemlist.add_eventPlayerItem(0, 1); //オレンジクッキーのレシピを追加

        GameMgr.scenario_flag = 209; //110 最初の調合パートの開始 109で一度シーンを読み込み開始し、読み終えると110になる。

    }*/

    //
    //家に帰った直後の会話イベント
    //
    IEnumerator backHome()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Chapter1_Story";

        engine.Param.TrySetParameter("Story_num", story_num);

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        scenario_loading = false;

        GameMgr.scenario_ON = false;
        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ
    }

    //
    // 女の子の感想・コメント
    //
    IEnumerator Girl_Comment()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Girl_Comment1"; //まずは、食べたときの前セリフ

        scenario_loading = true;

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        //一呼吸おき、反応する処理。キャラクタの表情が変わったり、エフェクトがキラキラとか。
        //ゲーム側の処理を挟む。

        //感想を述べる処理へ。
        engine.ResumeScenario();

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        girl1_status.girl_comment_endflag = true; //感想を言い終えたフラグ。

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }


    //
    // イベントレシピ表示
    //
    IEnumerator Recipi_read_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Recipi_read"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定

        //event_IDから、レシピの名前を検索
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (recipi_read_ID == pitemlist.eventitemlist[j].ev_ItemID)
            {
                recipi_Name = pitemlist.eventitemlist[j].event_itemName;
                re_flag = pitemlist.eventitemlist[j].ev_Re_flag_num;
                break;
            }
            j++;
        }

        engine.Param.TrySetParameter("Re_flag", re_flag);
      

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        if (re_flag != 0) //最初のレシピだけは表示しない。
        {
            scenarioLabel = "Recipi_read_after"; //アイテム発見力があがるメッセージ
                                                 //「宴」のシナリオを呼び出す
            Engine.JumpScenario(scenarioLabel);

            //「宴」のシナリオ終了待ち
            while (!Engine.IsEndScenario)
            {
                yield return null;
            }
        }


        //オレンジクッキーのレシピをはじめて読みおえた
        if (GameMgr.scenario_flag == 115)
        {
            if (event_ID == 0)
            {
                GameMgr.scenario_flag = 120; //レシピを読み終えた
            }
        }

        GameMgr.recipi_read_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // レシピリストから使用したときの、イベントレシピ表示
    //
    IEnumerator ItemUse_Recipi_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Event_Recipi"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定

        //event_IDから、レシピの名前を検索
        j = 0;
        while (j < pitemlist.eventitemlist.Count)
        {
            if (itemuse_recipi_ID == pitemlist.eventitemlist[j].ev_ItemID)
            {
                recipi_Name = pitemlist.eventitemlist[j].event_itemName;
                ev_flag = pitemlist.eventitemlist[j].ev_Ev_flag_num;
                break;
            }
            j++;
        }

        engine.Param.TrySetParameter("Ev_flag", ev_flag);        


        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        if(ev_flag == 40 && !GameMgr.Beginner_flag[1]) //ラスクのレシピを初めて読んだ
        {
            GameMgr.Beginner_flag[1] = true;
        }

        GameMgr.recipi_read_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // 好感度イベント
    //
    IEnumerator Girllove_event_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        if (GameMgr.girlloveevent_bunki == 0)
        {
            GirlLoveEvent_num = GameMgr.GirlLoveEvent_num;
            scenarioLabel = "GirlLove_Event"; //イベントレシピタグのシナリオを再生。
        }
        else if (GameMgr.girlloveevent_bunki == 1)
        {
            GirlLoveEvent_num = GameMgr.GirlLoveSubEvent_num;
            scenarioLabel = "GirlLove_EventSub"; //イベントレシピタグのシナリオを再生。
        }

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("Girllove_event_num", GirlLoveEvent_num);       

        //今食べたいお菓子を設定
        engine.Param.TrySetParameter("NowSPQuest", GameMgr.NowEatOkashi);

        //コンテスト時は、締め切り日も設定
        if (GameMgr.GirlLoveEvent_num == 50)
        {
            engine.Param.TrySetParameter("Limit_Month", PlayerStatus.player_cullent_Deadmonth);
            engine.Param.TrySetParameter("Limit_Day", PlayerStatus.player_cullent_Deadday);
        }

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        Debug.Log("GirlLoveEvent_num: " + GirlLoveEvent_num);
        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        if (GameMgr.event_pitem_use_select) //アイテムを使用するイベントの場合
        {
            GameMgr.event_pitem_use_select = false;

            //キャンバスの読み込み
            canvas = GameObject.FindWithTag("Canvas");

            //アイテムリストオブジェクト取得
            playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;

            //女の子、お菓子の判定処理オブジェクトの取得
            GirlEat_judge_obj = GameObject.FindWithTag("GirlEat_Judge");
            girlEat_judge = GirlEat_judge_obj.GetComponent<GirlEat_Judge>();

            //
            //「宴」のポーズ終了待ち アイテム選択画面に入る。
            while (!engine.IsPausingScenario)
            {
                yield return null;
            }

            //強制終了フラグがたってないかを検出。ピクニックいく、いかないの判定などで使用。たっていたら、終了する。
            eventend_flag = (bool)engine.Param.GetParameter("EventEnd_Flag");

            if(eventend_flag)
            {
                //アイテムを開いたりする処理は無視して、endにいく。

                //ここで、宴のパラメータ設定。リセットしておく。
                engine.Param.TrySetParameter("EventEnd_Flag", false);

                //いかないを選択したので、ハート獲得演出はキャンセル
                compound_Main.SubEvAfterHeartGet = false;

                //続きから再度読み込み
                engine.ResumeScenario();
            }
            else
            {
                playeritemlist_onoff.SetActive(true); //プレイヤーアイテム画面を表示。

                
                while (!GameMgr.event_pitem_use_OK && !GameMgr.event_pitem_cancel) //アイテム選択待ち
                {
                    yield return null;
                }

                if(GameMgr.event_pitem_use_OK) //アイテム渡す場合の処理
                {
                    GameMgr.event_pitem_use_OK = false;


                    //ピクニックイベントの場合、アイテムの判定処理がここで入る。
                    total_score = girlEat_judge.Judge_Score_ReturnEvent(GameMgr.event_kettei_itemID, GameMgr.event_kettei_item_Type, 1); //３番目はコンテストタイプ　1ならコンテストやイベントなど
                    Debug.Log("点数: （通常の固有お菓子判定と一緒のはず）" + total_score);

                    //選択したアイテム
                    if (GameMgr.event_kettei_item_Type == 0) //通常
                    {
                        Debug.Log("選択したアイテム: " + database.items[GameMgr.event_kettei_itemID].itemNameHyouji + " 個数: " + GameMgr.event_kettei_item_Kosu);
                        GameMgr.contest_okashiSlotName = "";
                        GameMgr.contest_okashiNameHyouji = database.items[GameMgr.event_kettei_itemID].itemNameHyouji;

                        //削除
                        pitemlist.deletePlayerItem(GameMgr.event_kettei_itemID, GameMgr.event_kettei_item_Kosu);
                    }
                    else //自分が制作したオリジナルアイテム
                    {
                        Debug.Log("選択したアイテム: " + pitemlist.player_originalitemlist[GameMgr.event_kettei_itemID].itemNameHyouji + " 個数: " + GameMgr.event_kettei_item_Kosu);
                        GameMgr.contest_okashiSlotName = pitemlist.player_originalitemlist[GameMgr.event_kettei_itemID].item_SlotName;
                        GameMgr.contest_okashiNameHyouji = pitemlist.player_originalitemlist[GameMgr.event_kettei_itemID].itemNameHyouji;

                        if ((pitemlist.player_originalitemlist.Count - 1) == GameMgr.event_kettei_itemID) //エクストリームパネルに設定されているお菓子を選んだ
                        {
                            exp_Controller._temp_extreme_id = 9999;
                        }

                        //削除
                        pitemlist.deleteOriginalItem(GameMgr.event_kettei_itemID, GameMgr.event_kettei_item_Kosu);
                    }

                    //提出したお菓子の名前をセット
                    engine.Param.TrySetParameter("contest_OkashiName", GameMgr.contest_okashiNameHyouji);
                    engine.Param.TrySetParameter("contest_OkashiSlotName", GameMgr.contest_okashiSlotName);

                    playeritemlist_onoff.SetActive(false);

                    //続きから再度読み込み
                    engine.ResumeScenario();
                }

                if (GameMgr.event_pitem_cancel)//やっぱりやめた場合の処理
                {
                    GameMgr.event_pitem_cancel = false;

                    playeritemlist_onoff.SetActive(false);

                    //ここで、宴のパラメータ設定。リセットしておく。
                    engine.Param.TrySetParameter("EventEnd_Flag", true);

                    //続きから再度読み込み
                    engine.ResumeScenario();

                    //
                    //「宴」のポーズ終了待ち
                    while (!engine.IsPausingScenario)
                    {
                        yield return null;
                    }

                    //いかないを選択したので、ハート獲得演出はキャンセル
                    compound_Main.SubEvAfterHeartGet = false;
                    //ここで、宴のパラメータ設定。リセットしておく。
                    engine.Param.TrySetParameter("EventEnd_Flag", false);

                    //続きから再度読み込み
                    engine.ResumeScenario();
                }
            }
            
        }

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        GameMgr.girlloveevent_endflag = true; //レシピを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // マップイベント表示
    //
    IEnumerator MapEvent_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "MapEvent"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("MapEv_num", map_ev_ID);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        if (map_ev_ID == 11)
        {
            catgrave_flag = (int)engine.Param.GetParameter("CatGrave_Flag");

            if (catgrave_flag == 0)
            {
                GameMgr.MapSubEvent_Flag = 0;
            }
            else
            {
                GameMgr.MapSubEvent_Flag = 10;
            }
        }
        

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }



    //
    // 通常お菓子感想表示
    //
    IEnumerator OkashiComment_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "OkashiEatComment"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("OkashiComment_num", Okashicomment_ID);

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        scenario_loading = false;

        GameMgr.scenario_read_endflag = true; //レシピを読み終えたフラグ

    }


    //
    // SPお菓子吹き出し、初回のみ表示
    //
    IEnumerator SpOkashiComment_HintHyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "SpOkashiBefore"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("SpOkashiBefore_num", sp_Okashi_ID);

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        if(sp_Okashi_ID == 1010) //雨が止むので、グラフィックを差し替える。
        {
            while (!engine.IsPausingScenario)
            {
                yield return null;
            }

            //背景の変更
            compound_Main.Change_BGimage();

            //元のシナリオにもどる。
            engine.ResumeScenario();
        }

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //背景の変更
        compound_Main.Change_BGimage();

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // SPお菓子 食べた瞬間の感想表示
    //
    IEnumerator SpOkashiComment_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "SpOkashi"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("SpOkashi_num", sp_Okashi_ID);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // お菓子 食べたあと　採点表示のあとの感想表示
    //
    IEnumerator OkashiAfterComment_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        if (GameMgr.okashiafter_status == 0)
        {
            scenarioLabel = "SpOkashiAfter"; //イベントレシピタグのシナリオを再生。
        }
        else if (GameMgr.okashiafter_status == 1)
        {
            scenarioLabel = "KoyuOkashiAfter"; //イベントレシピタグのシナリオを再生。
        }

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("SpOkashiAfter_num", sp_Okashi_ID);

        //Debug.Log("GameMgr.Okashi_totalscore: " + GameMgr.Okashi_totalscore);

        //デフォルト感想用に、お菓子の点数で感想の番号を変える。
        if(GameMgr.Okashi_totalscore <= 30) //まずい
        {
            engine.Param.TrySetParameter("OkashiScore_num", 0);
        }
        else if (GameMgr.Okashi_totalscore > 30 && GameMgr.Okashi_totalscore <= GameMgr.low_score) //30~60
        {
            engine.Param.TrySetParameter("OkashiScore_num", 1);
        }
        else if (GameMgr.Okashi_totalscore > GameMgr.low_score && GameMgr.Okashi_totalscore <= GameMgr.high_score) //60~85
        {
            engine.Param.TrySetParameter("OkashiScore_num", 2);
        }
        else if (GameMgr.Okashi_totalscore > GameMgr.high_score)
        {
            engine.Param.TrySetParameter("OkashiScore_num", 3);
        }

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //食べたいお菓子が違ってた場合も、ヒントをだす。シュークリームのときに、マフィンを送った場合。
        if (GameMgr.okashihint_flag)
        {
            scenarioLabel = "SpOkashiHint"; //イベントレシピタグのシナリオを再生。

            //ここで、宴のパラメータ設定
            engine.Param.TrySetParameter("SpOkashiHint_num", GameMgr.okashihint_ID);

            //「宴」のシナリオを呼び出す
            Engine.JumpScenario(scenarioLabel);

            //「宴」のシナリオ終了待ち
            while (!Engine.IsEndScenario)
            {
                yield return null;
            }

            GameMgr.okashihint_flag = false;
        }

        //食べたいお菓子に、必要なトッピングのってなかったときの処理。
        if (GameMgr.okashinontphint_flag)
        {
            GameMgr.okashinontphint_flag = false;

            scenarioLabel = "SpOkashiHintNonTopping"; //イベントレシピタグのシナリオを再生。

            //ここで、宴のパラメータ設定
            engine.Param.TrySetParameter("NonToppingHint_num", GameMgr.okashinontphint_ID);

            //「宴」のシナリオを呼び出す
            Engine.JumpScenario(scenarioLabel);

            //「宴」のシナリオ終了待ち
            while (!Engine.IsEndScenario)
            {
                yield return null;
            }
           
        }

        if ( !PlayerStatus.First_extreme_on ) //仕上げを一度もやったことがなかったら、ヒントだす。
        {
            scenarioLabel = "SpOkashiHint"; //イベントレシピタグのシナリオを再生。

            //ここで、宴のパラメータ設定
            engine.Param.TrySetParameter("SpOkashiHint_num", 0);

            //「宴」のシナリオを呼び出す
            Engine.JumpScenario(scenarioLabel);

            //「宴」のシナリオ終了待ち
            while (!Engine.IsEndScenario)
            {
                yield return null;
            }
        }       

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // エメラルどんぐりゲット時の表示
    //
    IEnumerator EmeralDonguri_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenario_loading = true;

        scenarioLabel = "EmeralDonguri"; //イベントレシピタグのシナリオを再生。

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("EmeralDongri_num", GameMgr.emeralDonguri_status);      

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }        

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // SPクエストクリア時の感想
    //
    IEnumerator QuestClearButton_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenario_loading = true;

        scenarioLabel = "QuestClearMessage"; //イベントレシピタグのシナリオを再生。

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("QuestClearMessage_num", GameMgr.OkashiQuest_Num);     

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }

    //
    // メインクエストクリア表示
    //
    IEnumerator MainQuestClear_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "MainQuestClear"; //イベントレシピタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("MainClear_num", mainClear_ID);

        //ゲーム上のキャラクタOFF
        CharacterLive2DImageOFF();

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        //ゲーム上のキャラクタON
        CharacterLive2DImageON();

        GameMgr.recipi_read_endflag = true; //読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }


    //
    // ヒント表示
    //
    IEnumerator TouchHint_Hyouji()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenario_loading = true;

        scenarioLabel = "TouchFaceHint";

        //ここで、宴のパラメータ設定
        engine.Param.TrySetParameter("TouchHint_num", touchhint_ID);


        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

    }


    //
    // ショップの「話す」コマンド
    //
    IEnumerator Shop_Talk()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Shop_Talk"; //ショップ話すタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴で呼び出したいイベント番号を設定する。
        engine.Param.TrySetParameter("Shop_Talk_Num", shop_talk_number);

        if(matplace_database.matplace_lists[matplace_database.SearchMapString("Farm")].placeFlag == 1)
        {
            engine.Param.TrySetParameter("Farm_Flag", true);
        }

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        if( (bool)engine.Param.GetParameter("Farm_Flag") )
        {
            matplace_database.matPlaceKaikin("Farm"); //モタリケ牧場解禁
        }               

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

        
        GameMgr.scenario_ON = false;

    }

    //
    // ショップの「うわさ話」コマンド
    //
    IEnumerator Shop_Uwasa()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Shop_Uwasa"; //ショップ話すタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴で呼び出したいイベント番号を設定する。
        engine.Param.TrySetParameter("Shop_Uwasa_Num", shop_uwasa_number);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }

        if ((bool)engine.Param.GetParameter("UwasaOn_Flag"))
        {
            if (shop_uwasa_number != 9999)
            {
                //キャンバスの読み込み
                canvas = GameObject.FindWithTag("Canvas");
                moneyStatus_Controller = canvas.transform.Find("MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();

                GameMgr.ShopUwasa_stage1[shop_uwasa_number] = true; //そのうわさを読んだフラグをON
                engine.Param.TrySetParameter("MoneyCheck_Flag", true);

                //お金の消費
                /*if (PlayerStatus.player_money >= 0)
                {
                    //PlayerStatus.player_money -= 50;
                    GameMgr.ShopUwasa_stage1[shop_uwasa_number] = true;
                    //moneyStatus_Controller.UseMoney(100); //うわさ話をきくをONにしたので、-100G

                    //ここで、宴で呼び出したいイベント番号を設定する。
                    engine.Param.TrySetParameter("MoneyCheck_Flag", true);
                }
                else //お金がたりないので、きけない。
                {
                    //ここで、宴で呼び出したいイベント番号を設定する。
                    engine.Param.TrySetParameter("MoneyCheck_Flag", false);
                }*/
            }
            else //きける話がひとつもない場合は、無視。現在は、うわさ話無しは、削除。
            {

            }
        }

        //宴でのフラグ関係を設定
        if (matplace_database.matplace_lists[matplace_database.SearchMapString("Lavender_field")].placeFlag == 1) {
            engine.Param.TrySetParameter("UwasaFlag01Check", true);
        }

        if (databaseCompo.compoitems[databaseCompo.SearchCompoIDString("bugget")].cmpitem_flag == 1)
        {
            engine.Param.TrySetParameter("UwasaFlag02Check", true);
        }
            

        //続きから再度読み込み
        engine.ResumeScenario();

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        if ((bool)engine.Param.GetParameter("UwasaOn_Flag"))
        {
            if (shop_uwasa_number != 9999)
            {

                //うわさ話をきき、フラグがたつ場合の処理
                switch (shop_uwasa_number)
                {
                    case 0:

                        //いける場所を追加
                        matplace_database.matPlaceKaikin("Lavender_field"); //アメジストの湖畔解禁
                        break;

                    case 1:

                        //レシピを追加
                        databaseCompo.CompoON_compoitemdatabase("bugget"); //パンのレシピ解禁
                        break;
                }
            }
            else
            {

            }
        }

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

        GameMgr.scenario_ON = false;

    }

    IEnumerator Shop_Hint()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Shop_Hint"; //ショップ話すタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴で呼び出したいイベント番号を設定する。
        engine.Param.TrySetParameter("Shop_Hint_Num", shop_hint_number);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

        GameMgr.scenario_ON = false;

    }

    IEnumerator Emerald_Shop()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        engine.Param.TrySetParameter("Story_num", story_num);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        switch(story_num)
        {
            case 0:

                //
                //「宴」のポーズ終了待ち
                while (!engine.IsPausingScenario)
                {
                    yield return null;
                }

                emeraldshop_main = GameObject.FindWithTag("EmeraldShop_Main").gameObject.GetComponent<EmeraldShop_Main>();
                emeraldshop_main.BlackOff();

                //続きから再度読み込み
                engine.ResumeScenario();
                break;
        }

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        CharacterSpriteFadeON();


        scenario_loading = false;

        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ
    }

    //
    // 広場のイベント処理
    //
    IEnumerator Hiroba_Event()
    {
        scenario_loading = true;

        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        //場所ごとにラベルを変えている
        switch (GameMgr.hiroba_event_placeNum)
        {
            case 0: //いちご少女

                scenarioLabel = "Hiroba_ichigo";
                break;

            case 1: //噴水

                scenarioLabel = "Hiroba_hunsui";
                break;

            case 2: //村長の家

                scenarioLabel = "Hiroba_sonchou";
                break;

            case 3: //パン工房

                scenarioLabel = "Hiroba_pan";
                break;

            case 4: //お花屋さん

                scenarioLabel = "Hiroba_flower";
                break;

            case 5: //図書館

                scenarioLabel = "Hiroba_library";
                break;

            case 6: //井戸端奥さん

                scenarioLabel = "Hiroba_okusan";
                break;

            default:
                break;
        }

        engine.Param.TrySetParameter("Hiroba_num", hiroba_num);
        engine.Param.TrySetParameter("Hiroba_endflag_Num", 0); //0で初期化

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        hiroba_endflag_num = (int)engine.Param.GetParameter("Hiroba_endflag_Num");
        switch(hiroba_endflag_num)
        {
            case 5041:

                GameMgr.hiroba_event_ID = 5041;
                break;

            case 5042:

                GameMgr.hiroba_event_ID = 5042;
                break;

            default:

                break;
        }

        scenario_loading = false;

        GameMgr.scenario_read_endflag = true; //シナリオを読み終えたフラグ
    }


    //
    // コンテストイベント
    //
    IEnumerator Contest_Event()
    {
        while (Engine.IsWaitBootLoading) yield return null; //宴の起動・初期化待ち

        scenarioLabel = "Contest_Event"; //ショップ話すタグのシナリオを再生。

        scenario_loading = true;

        //ここで、宴で呼び出したいイベント番号を設定する。
        engine.Param.TrySetParameter("Contest_num", contest_num);

        //「宴」のシナリオを呼び出す
        Engine.JumpScenario(scenarioLabel);

        //お菓子を判定する。採点結果により、審査員の反応も少し変わる。
        contest_main.Contest_Judge();

        //提出したお菓子の名前をセット
        engine.Param.TrySetParameter("contest_OkashiName", GameMgr.contest_okashiNameHyouji);
        engine.Param.TrySetParameter("contest_OkashiSlotName", GameMgr.contest_okashiSlotName);

        //採点をセット
        engine.Param.TrySetParameter("contest_score1", GameMgr.contest_Score[0]); //タカノ
        engine.Param.TrySetParameter("contest_score2", GameMgr.contest_Score[1]); //アントワネット
        engine.Param.TrySetParameter("contest_score3", GameMgr.contest_Score[2]); //村長
        engine.Param.TrySetParameter("contest_total_score", GameMgr.contest_TotalScore);

        //
        //「宴」のポーズ終了待ち
        while (!engine.IsPausingScenario)
        {
            yield return null;
        }


        //採点によって、感想が変わる。
        if (GameMgr.contest_TotalScore > GameMgr.high_score) //85~
        {
            engine.Param.TrySetParameter("contest_comment_num", 0);
        }
        else if (GameMgr.contest_TotalScore > GameMgr.low_score && GameMgr.contest_TotalScore <= GameMgr.high_score) //60~85
        {
            engine.Param.TrySetParameter("contest_comment_num", 1);
        }       
        else if (GameMgr.contest_TotalScore > 30 && GameMgr.contest_TotalScore <= GameMgr.low_score) //30~60
        {
            engine.Param.TrySetParameter("contest_comment_num", 2);
        }
        else if (GameMgr.contest_TotalScore <= 30)
        {
            engine.Param.TrySetParameter("contest_comment_num", 3);
        }


        //感想データベースから該当の感想を検索
        KansouSelect();       


        //優勝かどうかの判定
        if (GameMgr.contest_TotalScore > 90) //アマクサよりも高得点なら、優勝
        {
            yusho_flag = true;
            engine.Param.TrySetParameter("contest_ranking_num", 1);
        }
        else //そうでないなら、アマクサには負ける。
        {
            yusho_flag = false;
            engine.Param.TrySetParameter("contest_ranking_num", 0);
        }

        //コンテストの点によって、EDが分岐する。ベストEDとバッドEDのみ、好感度も影響する。
        if (PlayerStatus.girl1_Love_exp < 200) //LV3以下 badED ED:D
        {
            engine.Param.TrySetParameter("ED_num", 1);
            GameMgr.ending_number = 1;
        }
        else {

            if (GameMgr.contest_TotalScore < 60) // LV4 ノーマルED ED:C
            {
                engine.Param.TrySetParameter("ED_num", 2);
                GameMgr.ending_number = 2;
            }
            else if (GameMgr.contest_TotalScore >= 60 && yusho_flag == false) // LV5~ 
            {
                engine.Param.TrySetParameter("ED_num", 2);
                GameMgr.ending_number = 3;
            }
            else if (GameMgr.contest_TotalScore >= 60 && yusho_flag == true) 
            {
                if (PlayerStatus.girl1_Love_exp > 500)
                {
                    engine.Param.TrySetParameter("ED_num", 4); // LV5~ ベスト+優勝ED ED:A　ヒカリパティシエED
                    GameMgr.ending_number = 4;
                }
                else
                {
                    engine.Param.TrySetParameter("ED_num", 3); //ベストED ED:B ケーキED
                    GameMgr.ending_number = 3;
                }
            }

        }
        

        //続きから再度読み込み
        engine.ResumeScenario();

        //「宴」のシナリオ終了待ち
        while (!Engine.IsEndScenario)
        {
            yield return null;
        }

        scenario_loading = false; //シナリオを読み終わったので、falseにし、updateを読み始める。

        GameMgr.ending_on = true; //エンディングをONにする。

        GameMgr.scenario_ON = false;

    }

    void KansouSelect()
    {
        judge_num = 0;
        SpecialItemFlag = false;

        i = 0;
        while (i < databaseContestComment.contestcomment_lists.Count)
        {
            if (databaseContestComment.contestcomment_lists[i].ItemName == GameMgr.contest_okashiName)
            {
                CommentID = i;
                SpecialItemFlag = true;
                Debug.Log("審査員　特定のお菓子に反応: " + GameMgr.contest_okashiName);
                break;
            }
            i++;
        }

        if (!SpecialItemFlag)
        {
            i = 0;
            while (i < databaseContestComment.contestcomment_lists.Count)
            {
                if (databaseContestComment.contestcomment_lists[i].CommentID == 0)
                {
                    CommentID = i;
                    break;
                }
                i++;
            }
        }

        //審査員１の感想をセット
        if (GameMgr.contest_Score[judge_num] > GameMgr.high_score) //85~
        {
            engine.Param.TrySetParameter("contest_judge1_comment1", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_1);
            engine.Param.TrySetParameter("contest_judge1_comment2", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_2);
            engine.Param.TrySetParameter("contest_judge1_comment3", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_3);
            engine.Param.TrySetParameter("contest_judge1_comment4", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_4);
        }
        else if (GameMgr.contest_Score[judge_num] > GameMgr.low_score && GameMgr.contest_Score[0] <= GameMgr.high_score) //60~85
        {
            engine.Param.TrySetParameter("contest_judge1_comment1", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_1);
            engine.Param.TrySetParameter("contest_judge1_comment2", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_2);
            engine.Param.TrySetParameter("contest_judge1_comment3", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_3);
            engine.Param.TrySetParameter("contest_judge1_comment4", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_4);
        }
        else if (GameMgr.contest_Score[judge_num] > 30 && GameMgr.contest_Score[0] <= GameMgr.low_score) //30~60
        {
            engine.Param.TrySetParameter("contest_judge1_comment1", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_1);
            engine.Param.TrySetParameter("contest_judge1_comment2", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_2);
            engine.Param.TrySetParameter("contest_judge1_comment3", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_3);
            engine.Param.TrySetParameter("contest_judge1_comment4", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_4);
        }
        else if (GameMgr.contest_Score[judge_num] <= 30)
        {
            engine.Param.TrySetParameter("contest_judge1_comment1", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_1);
            engine.Param.TrySetParameter("contest_judge1_comment2", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_2);
            engine.Param.TrySetParameter("contest_judge1_comment3", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_3);
            engine.Param.TrySetParameter("contest_judge1_comment4", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_4);
        }

        //審査員２の感想をセット。アントワネットは、見た目を重視する。豪華であればあるほど、得点が高い。
        judge_num++;
        CommentID += 4; //４はじまり
        if (GameMgr.contest_Beauty_Score[judge_num] > 50) //アントワとの差　50以上良い
        {
            engine.Param.TrySetParameter("contest_judge2_comment1", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_1);
            engine.Param.TrySetParameter("contest_judge2_comment2", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_2);
            engine.Param.TrySetParameter("contest_judge2_comment3", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_3);
            engine.Param.TrySetParameter("contest_judge2_comment4", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_4);
        }
        else if (GameMgr.contest_Beauty_Score[judge_num] > 0 && GameMgr.contest_Beauty_Score[judge_num] <= 50) //アントワとの差 50以内
        {
            engine.Param.TrySetParameter("contest_judge2_comment1", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_1);
            engine.Param.TrySetParameter("contest_judge2_comment2", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_2);
            engine.Param.TrySetParameter("contest_judge2_comment3", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_3);
            engine.Param.TrySetParameter("contest_judge2_comment4", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_4);
        }
        else if (GameMgr.contest_Beauty_Score[judge_num] > -40 && GameMgr.contest_Beauty_Score[judge_num] <= 0) //アントワとの差
        {
            engine.Param.TrySetParameter("contest_judge2_comment1", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_1);
            engine.Param.TrySetParameter("contest_judge2_comment2", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_2);
            engine.Param.TrySetParameter("contest_judge2_comment3", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_3);
            engine.Param.TrySetParameter("contest_judge2_comment4", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_4);
        }
        else if (GameMgr.contest_Beauty_Score[judge_num] <= -40) //アントワとの差 -40より下
        {
            engine.Param.TrySetParameter("contest_judge2_comment1", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_1);
            engine.Param.TrySetParameter("contest_judge2_comment2", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_2);
            engine.Param.TrySetParameter("contest_judge2_comment3", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_3);
            engine.Param.TrySetParameter("contest_judge2_comment4", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_4);
        }

        //審査員３の感想をセット。じいさんは、食感の値に対して、感想を述べる。
        judge_num++;
        CommentID += 4; //８はじまり
        if (GameMgr.contest_Taste_Score[judge_num] > GameMgr.high_score) //85~
        {
            engine.Param.TrySetParameter("contest_judge3_comment1", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_1);
            engine.Param.TrySetParameter("contest_judge3_comment2", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_2);
            engine.Param.TrySetParameter("contest_judge3_comment3", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_3);
            engine.Param.TrySetParameter("contest_judge3_comment4", databaseContestComment.contestcomment_lists[CommentID + 0].Comment_4);
        }
        else if (GameMgr.contest_Taste_Score[judge_num] > GameMgr.low_score && GameMgr.contest_Taste_Score[judge_num] <= GameMgr.high_score) //60~85
        {
            engine.Param.TrySetParameter("contest_judge3_comment1", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_1);
            engine.Param.TrySetParameter("contest_judge3_comment2", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_2);
            engine.Param.TrySetParameter("contest_judge3_comment3", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_3);
            engine.Param.TrySetParameter("contest_judge3_comment4", databaseContestComment.contestcomment_lists[CommentID + 1].Comment_4);
        }
        else if (GameMgr.contest_Taste_Score[judge_num] > 30 && GameMgr.contest_Taste_Score[judge_num] <= GameMgr.low_score) //30~60
        {
            engine.Param.TrySetParameter("contest_judge3_comment1", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_1);
            engine.Param.TrySetParameter("contest_judge3_comment2", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_2);
            engine.Param.TrySetParameter("contest_judge3_comment3", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_3);
            engine.Param.TrySetParameter("contest_judge3_comment4", databaseContestComment.contestcomment_lists[CommentID + 2].Comment_4);
        }
        else if (GameMgr.contest_Taste_Score[judge_num] <= 30)
        {
            engine.Param.TrySetParameter("contest_judge3_comment1", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_1);
            engine.Param.TrySetParameter("contest_judge3_comment2", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_2);
            engine.Param.TrySetParameter("contest_judge3_comment3", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_3);
            engine.Param.TrySetParameter("contest_judge3_comment4", databaseContestComment.contestcomment_lists[CommentID + 3].Comment_4);
        }
    }


    //ゲームメイン中のLive2DキャラクタをONにする。
    void CharacterLive2DImageON()
    {        
        _renderController.Opacity = 1.0f;

        //宴用の表情モードはオフに。
        live2d_animator.SetLayerWeight(3, 0.0f);
    }

    //ゲームメイン中のLive2DキャラクタをOFFにする。
    void CharacterLive2DImageOFF()
    {        
        _renderController.Opacity = 1.0f;
        FadeAnim_flag = true;
        FadeAnim_status = 1;

        //宴用の表情モードに切り替える。
        live2d_animator.SetLayerWeight(3, 1.0f);
    }

    //キャラクタスプライト画像をONにする
    void CharacterSpriteFadeON()
    {
        character.GetComponent<FadeCharacter>().FadeImageOn();
    }

    void CharacterSpriteFadeOFF()
    {
        character.GetComponent<FadeCharacter>().FadeImageOff();
    }

    void CharacterSpriteSetON()
    {
        character.GetComponent<FadeCharacter>().SetOn();
    }

    void CharacterSpriteSetOFF()
    {
        character.GetComponent<FadeCharacter>().SetOff();
    }
}
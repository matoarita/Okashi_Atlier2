using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Debug_Panel : MonoBehaviour {

    private GameObject canvas;

    private SoundController sc;

    private ItemMatPlaceDataBase matplace_database;
    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject mainlist_controller2_obj;
    private MainListController2 mainlist_controller2;

    private GameObject StoryNumber;
    private Text StoryNumber_text;

    private GameObject EventNumber;
    private Text EventNumber_text;

    private GameObject StageNumber;
    private Text StageNumber_text;

    private Text DebugInputOn;

    private InputField input_scenario;
    private InputField input_event;
    private InputField input_girllove;
    private string input_text;
    private string input_text2;
    private string input_text3;
    private int scenario_num;
    private int event_num;
    private int girllove_param;
    private Text girl_lv;
    public bool Debug_INPUT_ON; //デバッグ外部からの入力受け付けるかどうか。PSコントローラーでやるときはOFFにしたほうがよい。バグがでるため。

    private Toggle Mazui_toggle;
    private Toggle Mazui_toggle_input;

    private Text Counter;
    private int i;

    private Girl1_status girl1_status;
    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;
    private Slider _slider; //好感度バーを取得

    private GameObject GirlHeartEffect_obj;
    private Particle_Heart_Character GirlHeartEffect;

    //好感度レベルテーブルの取得
    private List<int> stage_levelTable = new List<int>();

    // Use this for initialization
    void Start () {

        StoryNumber = this.transform.Find("StoryNumber").gameObject;
        StoryNumber_text = StoryNumber.GetComponent<Text>();

        EventNumber = this.transform.Find("EventNumber").gameObject;
        EventNumber_text = EventNumber.GetComponent<Text>();

        StageNumber = this.transform.Find("StageNumber").gameObject;
        StageNumber_text = StageNumber.GetComponent<Text>();

        input_scenario = this.transform.Find("InputField").gameObject.GetComponent<InputField>();
        input_event = this.transform.Find("InputField_EventNum").gameObject.GetComponent<InputField>();
        input_girllove = this.transform.Find("InputField_GirlLove").gameObject.GetComponent<InputField>();


        Mazui_toggle = this.transform.Find("MazuiToggle").gameObject.GetComponent<Toggle>();
        Mazui_toggle_input = this.transform.Find("MazuiToggleInput").gameObject.GetComponent<Toggle>();

        Debug_INPUT_ON = false;
        DebugInputOn = this.transform.Find("DebugInputOnText").GetComponent<Text>();        

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

    }

    // Update is called once per frame
    void Update()
    {
        if(StoryNumber == null)
        {
            StoryNumber = this.transform.Find("StoryNumber").gameObject;
            StoryNumber_text = StoryNumber.GetComponent<Text>();

            EventNumber = this.transform.Find("EventNumber").gameObject;
            EventNumber_text = EventNumber.GetComponent<Text>();

            StageNumber = this.transform.Find("StageNumber").gameObject;
            StageNumber_text = StageNumber.GetComponent<Text>();

            input_scenario = this.transform.Find("InputField").gameObject.GetComponent<InputField>();
            input_event = this.transform.Find("InputField_EventNum").gameObject.GetComponent<InputField>();
            input_girllove = this.transform.Find("InputField_GirlLove").gameObject.GetComponent<InputField>();


            Mazui_toggle = this.transform.Find("MazuiToggle").gameObject.GetComponent<Toggle>();
            Mazui_toggle_input = this.transform.Find("MazuiToggleInput").gameObject.GetComponent<Toggle>();

            Debug_INPUT_ON = false;
            DebugInputOn = this.transform.Find("DebugInputOnText").GetComponent<Text>();

            //女の子データの取得
            girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

            //スペシャルお菓子クエストの取得
            special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();
        }

        if( GameMgr.tutorial_ON == true )
        {
            StoryNumber_text.text = "TutorialNumber: " + GameMgr.tutorial_Num;
        } else
        {
            StoryNumber_text.text = "StNum: " + GameMgr.scenario_flag;
        }

        if(Debug_INPUT_ON)
        {
            DebugInputOn.text = "Input:ON";
        }
        else
        {
            DebugInputOn.text = "Input:OFF";
        }

        EventNumber_text.text = "Event: " + GameMgr.OkashiQuest_Num;

        StageNumber_text.text = "Stage: " + GameMgr.stage_number;
        Mazui_toggle.isOn = girl1_status.girl_Mazui_flag;

        //ここに処理。時間カウント。デバッグ用。
        Counter = this.transform.Find("TimeCount").gameObject.GetComponentInChildren<Text>(); //デバッグ用
        Counter.text = "PlayTime: " + GameMgr.Game_timeCount + " s";

    }

    public void InputScenarioNum()
    {
        if (Debug_INPUT_ON)
        {
            input_text = input_scenario.text;
            Int32.TryParse(input_text, out scenario_num);
            GameMgr.scenario_flag = scenario_num;
        }
    }

    //イベント番号を指定し、そこからスタート。２を入れると、クレープから始まり、それまでのイベントはクリアしたことになる。
    public void InputEventNum()
    {
        if (Debug_INPUT_ON)
        {
            input_text3 = input_event.text;
            Int32.TryParse(input_text3, out event_num);           

            switch (GameMgr.stage_number)
            {
                case 1:

                    //初期化
                    for (i = 0; i < GameMgr.OkashiQuest_flag_stage1.Length; i++)
                    {
                        GameMgr.OkashiQuest_flag_stage1[i] = false;
                    }

                    for (i = 0; i < GameMgr.GirlLoveEvent_stage1.Length; i++)
                    {
                        GameMgr.GirlLoveEvent_stage1[i] = false;
                    }

                    //クエストクリアフラグをたてる
                    for (i = 0; i < event_num; i++)
                    {
                        GameMgr.OkashiQuest_flag_stage1[i] = true;

                        //点数は、60点でクリアしたことにする。
                        special_quest.special_score_record[i, 0] = 60;
                    }
                    break;

                default:
                    break;
            }
            //**              
            
            girl1_status.OkashiNew_Status = 1; //クエストクリアで、1に戻す。0にすると、次のクエストが開始する。（スペシャル吹き出し登場する）
            special_quest.special_kaisu = 0;
            girl1_status.special_animatFirst = false;

            if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
            {
                //現在のクエストを再度設定。前クエストの終わりから、スタート。
                if (event_num != 0)
                {
                    special_quest.SetSpecialOkashi(event_num - 1);
                    GameMgr.GirlLoveEvent_stage1[event_num - 1] = true;
                    Debug.Log("event_num: " + event_num);
                }

                //女の子判定データの取得
                girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();
                                                                               
                //** 初期化               
                girlEat_judge.subQuestClear_check = true;
                girlEat_judge.Gameover_flag = false;                

                if (event_num != 0)
                {
                    girlEat_judge.ResultPanel_On();
                }
                else
                {
                    girlEat_judge.subQuestClear_check = false;
                    girlEat_judge.ResultOFF();
                }
                
            }
            else //その他シーン。すぐに指定した番号のイベントに切り替える。
            {
                //現在のクエストを再度設定
                if (event_num != 0)
                {
                    special_quest.SetSpecialOkashi(event_num);
                    GameMgr.GirlLoveEvent_num = event_num;
                    GameMgr.GirlLoveEvent_stage1[event_num] = true;
                    Debug.Log("event_num: " + event_num);
                }
            }
        }
    }

    public void InputGirlLoveParam()
    {
        if (Debug_INPUT_ON)
        {
            input_text2 = input_girllove.text;
            Int32.TryParse(input_text2, out girllove_param);
            girl1_status.girl1_Love_exp = girllove_param;

            if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
            {
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                //女の子の反映用ハートエフェクト取得
                GirlHeartEffect_obj = GameObject.FindWithTag("Particle_Heart_Character");
                GirlHeartEffect = GirlHeartEffect_obj.GetComponent<Particle_Heart_Character>();

                //好感度バーの取得
                _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();

                //女の子のレベル取得
                girl_lv = GameObject.FindWithTag("Girl_love_exp_bar").transform.Find("LV_param").GetComponent<Text>();
                girl1_status.girl1_Love_lv = 1;

                stage_levelTable.Clear();

                //好感度レベルテーブルを取得
                switch (GameMgr.stage_number)
                {
                    case 1:

                        for (i = 0; i < girl1_status.stage1_lvTable.Count; i++)
                        {
                            stage_levelTable.Add(girl1_status.stage1_lvTable[i]);
                            //Debug.Log("stage1_levelTable: " + stage_levelTable[i]);
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

                i = 0;
                while (girllove_param >= stage_levelTable[i])
                {
                    girllove_param -= stage_levelTable[i];
                    girl1_status.girl1_Love_lv++;
                    i++;
                }
                _slider.value = girllove_param;

                //スライダマックスバリューも更新
                _slider.maxValue = stage_levelTable[girl1_status.girl1_Love_lv - 1]; //レベルは１始まりなので、配列番号になおすため、-1してる


                //レベル表示も更新
                girl_lv.text = girl1_status.girl1_Love_lv.ToString();

                //表情も即時変更
                girl1_status.CheckGokigen();
                girl1_status.DefaultFace();

                //好感度パラメータに応じて、実際にキャラクタからハートがでてくる量を更新
                GirlHeartEffect.LoveRateChange();

                compound_Main.check_GirlLoveEvent_flag = false;
            }
        }
    }

    public void InputMazuiFlag()
    {
        if (Debug_INPUT_ON)
        {
            if (Mazui_toggle_input.isOn) //まずいフラグをONにする。
            {
                girl1_status.girl_Mazui_flag = true;

            }
            else
            {
                girl1_status.girl_Mazui_flag = false;
            }
            Mazui_toggle.isOn = Mazui_toggle_input.isOn;
        }
    }

    public void InputMainFlagOn()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        sc.PlaySe(30);

        //ストーリーの最初から、現在までの最低限必要なフラグをONにする。シナリオチェックの際、やりやすいようにフラグのON/OFFの数を調節してOK
        for (i = 0; i <= GameMgr.GirlLoveEvent_num; i++)
        {
            switch (i)
            {
                case 2:

                    matplace_database.matPlaceKaikin("Lavender_field"); //ラベンダー畑
                    break;

                case 4:

                    GameMgr.hiroba_event_end[0] = true; //アマクサ会話終了　広場「お花屋さん」「図書館」「道端奥さん」ON
                    GameMgr.hiroba_event_end[1] = true; //パン工房ON
                    GameMgr.hiroba_event_end[2] = true; //ストロベリーガーデンON

                    //
                    GameMgr.hiroba_event_end[3] = true; //お花屋さんと会話した
                    GameMgr.hiroba_event_end[4] = true; //図書館　ドーナツのことを聞かずに帰った
                    GameMgr.hiroba_event_end[5] = true; //図書館　図書館　ドーナツのことを聞いた
                    GameMgr.hiroba_event_end[6] = true; //パン工房でベニエと会う。油を探すことになった。
                    GameMgr.hiroba_event_end[7] = true; //お花屋さんから油の話をきいた。「ひまわり畑」ON

                    if (SceneManager.GetActiveScene().name == "Hiroba2") // 
                    {
                        //キャンバスの読み込み
                        canvas = GameObject.FindWithTag("Canvas");
                        mainlist_controller2_obj = canvas.transform.Find("MainList_ScrollView").gameObject;
                        mainlist_controller2 = mainlist_controller2_obj.GetComponent<MainListController2>();

                        mainlist_controller2.ToggleFlagCheck();
                        mainlist_controller2.MenuWindowExpand();
                    }
                    break;

                case 5:

                    GameMgr.hiroba_event_end[8] = true; //ドーナツイベントの終了

                    matplace_database.matPlaceKaikin("Hiroba"); //
                    matplace_database.matPlaceKaikin("StrawberryGarden"); //ストロベリーガーデン解禁　いちごがとれるようになる。
                    matplace_database.matPlaceKaikin("HimawariHill"); //ひまわり畑解禁　ひまわりの種がとれるようになる。

                    break;

                default:
                    break;
            }
        }
    }
}

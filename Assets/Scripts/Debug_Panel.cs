using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Debug_Panel : MonoBehaviour {

    private GameObject canvas;

    private SoundController sc;

    private PlayerItemList pitemlist;

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

    private KaeruCoin_Controller kaeruCoin_Controller;
    private MoneyStatus_Controller moneyStatus_Controller;

    private Text DebugInputOn;

    private InputField input_money;
    private InputField input_edonguri;
    private InputField input_event;
    private InputField input_girllove;
    private string input_text;
    private string input_text2;
    private string input_text3;
    private int money_num;
    private int edonguri_num;
    private int event_num;
    private int _temp_event_num;
    private int girllove_param;
    private Text girl_lv;
    private Text girl_param;
    public bool Debug_INPUT_ON; //デバッグ外部からの入力受け付けるかどうか。PSコントローラーでやるときはOFFにしたほうがよい。バグがでるため。

    private Text Counter;
    private int i, count;

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

        StoryNumber = this.transform.Find("Hyouji/StoryNumber").gameObject;
        StoryNumber_text = StoryNumber.GetComponent<Text>();

        EventNumber = this.transform.Find("Hyouji/EventNumber").gameObject;
        EventNumber_text = EventNumber.GetComponent<Text>();

        StageNumber = this.transform.Find("Hyouji/StageNumber").gameObject;
        StageNumber_text = StageNumber.GetComponent<Text>();

        input_money = this.transform.Find("Hyouji/InputField_Money").gameObject.GetComponent<InputField>();
        input_edonguri = this.transform.Find("Hyouji/InputField_EDonguri").gameObject.GetComponent<InputField>();
        input_event = this.transform.Find("Hyouji/InputField_EventNum").gameObject.GetComponent<InputField>();
        input_girllove = this.transform.Find("Hyouji/InputField_GirlLove").gameObject.GetComponent<InputField>();

        Debug_INPUT_ON = false;
        DebugInputOn = this.transform.Find("Hyouji/DebugInputOnText").GetComponent<Text>();        

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

    }

    // Update is called once per frame
    void Update()
    {
        if(StoryNumber == null)
        {
            StoryNumber = this.transform.Find("Hyouji/StoryNumber").gameObject;
            StoryNumber_text = StoryNumber.GetComponent<Text>();

            EventNumber = this.transform.Find("Hyouji/EventNumber").gameObject;
            EventNumber_text = EventNumber.GetComponent<Text>();

            StageNumber = this.transform.Find("Hyouji/StageNumber").gameObject;
            StageNumber_text = StageNumber.GetComponent<Text>();

            input_money = this.transform.Find("Hyouji/InputField_Money").gameObject.GetComponent<InputField>();
            input_edonguri = this.transform.Find("Hyouji/InputField_EDonguri").gameObject.GetComponent<InputField>();
            input_event = this.transform.Find("Hyouji/InputField_EventNum").gameObject.GetComponent<InputField>();
            input_girllove = this.transform.Find("Hyouji/InputField_GirlLove").gameObject.GetComponent<InputField>();

            Debug_INPUT_ON = false;
            DebugInputOn = this.transform.Find("Hyouji/DebugInputOnText").GetComponent<Text>();

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
            StoryNumber_text.text = "ScN: " + GameMgr.scenario_flag;
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

        //ここに処理。時間カウント。デバッグ用。
        Counter = this.transform.Find("Hyouji/TimeCount").gameObject.GetComponentInChildren<Text>(); //デバッグ用
        Counter.text = "PlayTime: " + GameMgr.Game_timeCount + " s";

    }

    public void InputMoneyNum()
    {
        if (Debug_INPUT_ON)
        {
            input_text = input_money.text;
            Int32.TryParse(input_text, out money_num);

            canvas = GameObject.FindWithTag("Canvas");

            PlayerStatus.player_money = money_num;
            if (canvas.transform.Find("MoneyStatus_panel").gameObject)
            {
                moneyStatus_Controller = canvas.transform.Find("MoneyStatus_panel").GetComponent<MoneyStatus_Controller>();
                moneyStatus_Controller.money_Draw();
            }
        }
    }

    public void InputEDonguriNum()
    {
        if (Debug_INPUT_ON)
        {
            input_text = input_edonguri.text;
            Int32.TryParse(input_text, out edonguri_num);

            canvas = GameObject.FindWithTag("Canvas");

            pitemlist.ReSetPlayerItemString("emeralDongri", edonguri_num);
            if ( canvas.transform.Find("KaeruCoin_Panel").gameObject )
            {
                kaeruCoin_Controller = canvas.transform.Find("KaeruCoin_Panel").GetComponent<KaeruCoin_Controller>();
                kaeruCoin_Controller.ReDrawParam();
            }            
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
                    count = 0;
                    for (i = 0; i < event_num; i++)
                    {
                        if(i == 0)
                        {
                            GameMgr.OkashiQuest_flag_stage1[count] = true;
                        }

                        if (i != 0 && i % 10 == 0)
                        {
                            GameMgr.OkashiQuest_flag_stage1[count] = true;
                            count++;
                        }
                    }
                        
                    /*for (i = 0; i < event_num; i++)
                    {                                    
                        GameMgr.GirlLoveEvent_stage1[i] = true;
                        //Debug.Log("GameMgr.GirlLoveEvent_stage1[i]: " + GameMgr.GirlLoveEvent_stage1[i]);

                        //点数は、60点でクリアしたことにする。
                        special_quest.special_score_record[i, 0] = 60;
                    }*/
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
                    if (event_num % 10 == 0)
                    {
                        special_quest.SetSpecialOkashi(event_num - 10, 0);
                        GameMgr.GirlLoveEvent_stage1[event_num - 10] = true;
                        Debug.Log("event_num: " + event_num);
                    }
                    else
                    {
                        count = 0;
                        for (i = 0; i < event_num; i++)
                        {
                            if (i != 0 && i % 10 == 0) //0は除外し、10桁をカウントするごとに+1
                            {
                                count++;
                            }
                        }
                        if (count == 0)
                        {
                            _temp_event_num = 0;
                        }
                        else
                        {
                            _temp_event_num = count * 10;
                        }
                        special_quest.SetSpecialOkashi(_temp_event_num - 10, 0);
                        GameMgr.GirlLoveEvent_stage1[_temp_event_num - 10] = true;
                        Debug.Log("event_num: " + _temp_event_num);
                    }
                }

                //女の子判定データの取得
                girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();
                                                                               
                //** 初期化               
                girlEat_judge.subQuestClear_check = true; //強制的に、そのクエストをクリアしたことにする。
                girlEat_judge.Gameover_flag = false;
                girlEat_judge.clear_spokashi_status = 1; //SPお菓子でクリアしたことにする。

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
                    special_quest.SetSpecialOkashi(event_num, 0);
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

            GirlLove_Koushin(girllove_param);

            compound_Main.check_GirlLoveEvent_flag = false;
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
                case 20:

                    matplace_database.matPlaceKaikin("Lavender_field"); //ラベンダー畑
                    break;

                case 40:

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

                case 50:

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

    public void GirlLove_Koushin(int _girllove_param)
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        girl1_status.girl1_Love_exp = 0;

        if (SceneManager.GetActiveScene().name == "Compound") // 調合シーンでやりたい処理。それ以外のシーンでは、この中身の処理は無視。
        {
            canvas = GameObject.FindWithTag("Canvas");

            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            //女の子の反映用ハートエフェクト取得
            GirlHeartEffect_obj = GameObject.FindWithTag("Particle_Heart_Character");
            GirlHeartEffect = GirlHeartEffect_obj.GetComponent<Particle_Heart_Character>();

            //好感度バーの取得
            _slider = canvas.transform.Find("Girl_love_exp_bar").GetComponent<Slider>();

            //女の子のレベル取得
            girl_lv = canvas.transform.Find("Girl_love_exp_bar").transform.Find("LV_param").GetComponent<Text>();
            girl_param = canvas.transform.Find("Girl_love_exp_bar").transform.Find("Girllove_param").GetComponent<Text>();

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

            girl1_status.girl1_Love_exp = _girllove_param;

            i = 0;
            girl1_status.girl1_Love_lv = 1;
            while (_girllove_param >= stage_levelTable[i])
            {
                _girllove_param -= stage_levelTable[i];
                girl1_status.girl1_Love_lv++;
                i++;
            }

            //スライダマックスバリューも更新
            _slider.maxValue = stage_levelTable[girl1_status.girl1_Love_lv - 1]; //レベルは１始まりなので、配列番号になおすため、-1してる

            _slider.value = _girllove_param;
            girl1_status.LvUpStatus();
           

            //レベル表示も更新
            girl_lv.text = girl1_status.girl1_Love_lv.ToString();
            girl_param.text = girl1_status.girl1_Love_exp.ToString();


            //表情も即時変更
            girl1_status.CheckGokigen();
            girl1_status.DefaultFace();

            //好感度パラメータに応じて、実際にキャラクタからハートがでてくる量を更新
            GirlHeartEffect.LoveRateChange();            
        }
    }

}

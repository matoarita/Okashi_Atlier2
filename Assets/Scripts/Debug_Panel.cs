using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Debug_Panel : MonoBehaviour {

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject StoryNumber;
    private Text StoryNumber_text;

    private GameObject StageNumber;
    private Text StageNumber_text;

    private InputField input_scenario;
    private InputField input_girllove;
    private string input_text;
    private string input_text2;
    private int scenario_num;
    private int girllove_param;
    private Text girl_lv;

    private Toggle Mazui_toggle;
    private Toggle Mazui_toggle_input;

    private Text Counter;
    private int i;

    private Girl1_status girl1_status;
    private Slider _slider; //好感度バーを取得

    private GameObject GirlHeartEffect_obj;
    private Particle_Heart_Character GirlHeartEffect;

    //好感度レベルテーブルの取得
    private List<int> stage_levelTable = new List<int>();

    // Use this for initialization
    void Start () {

        StoryNumber = this.transform.Find("StoryNumber").gameObject;
        StoryNumber_text = StoryNumber.GetComponent<Text>();

        StageNumber = this.transform.Find("StageNumber").gameObject;
        StageNumber_text = StageNumber.GetComponent<Text>();

        input_scenario = this.transform.Find("InputField").gameObject.GetComponent<InputField>();
        input_girllove = this.transform.Find("InputField_GirlLove").gameObject.GetComponent<InputField>();

        Mazui_toggle = this.transform.Find("MazuiToggle").gameObject.GetComponent<Toggle>();
        Mazui_toggle_input = this.transform.Find("MazuiToggleInput").gameObject.GetComponent<Toggle>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        

    }

    // Update is called once per frame
    void Update()
    {
        if( GameMgr.tutorial_ON == true )
        {
            StoryNumber_text.text = "TutorialNumber: " + GameMgr.tutorial_Num;
        } else
        {
            StoryNumber_text.text = "StoryNumber: " + GameMgr.scenario_flag;
        }
        
        StageNumber_text.text = "Stage: " + GameMgr.stage_number;
        Mazui_toggle.isOn = girl1_status.girl_Mazui_flag;

        //ここに処理。時間カウント。デバッグ用。
        Counter = this.transform.Find("TimeCount").gameObject.GetComponentInChildren<Text>(); //デバッグ用
        Counter.text = "PlayTime: " + GameMgr.Game_timeCount + " s";

    }

    public void InputScenarioNum()
    {
        input_text = input_scenario.text;
        Int32.TryParse(input_text, out scenario_num);
        GameMgr.scenario_flag = scenario_num;
    }

    public void InputGirlLoveParam()
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
            if (girl1_status.girl1_Love_lv <= 5)
            {
                _slider.maxValue = stage_levelTable[girl1_status.girl1_Love_lv - 1]; //レベルは１始まりなので、配列番号になおすため、-1してる
            }
            else //5以上は、現状、同じ数値
            {
                _slider.maxValue = stage_levelTable[stage_levelTable.Count - 1];
            }

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

    public void InputMazuiFlag()
    {
        if(Mazui_toggle_input.isOn) //まずいフラグをONにする。
        {
            girl1_status.girl_Mazui_flag = true;
            
        } else
        {
            girl1_status.girl_Mazui_flag = false;
        }
        Mazui_toggle.isOn = Mazui_toggle_input.isOn;
    }
}

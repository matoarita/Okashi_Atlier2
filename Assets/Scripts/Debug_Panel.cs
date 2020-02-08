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

    private Text Counter;

    private Girl1_status girl1_status;
    private Slider _slider; //好感度バーを取得

    // Use this for initialization
    void Start () {

        StoryNumber = this.transform.Find("StoryNumber").gameObject;
        StoryNumber_text = StoryNumber.GetComponent<Text>();

        StageNumber = this.transform.Find("StageNumber").gameObject;
        StageNumber_text = StageNumber.GetComponent<Text>();

        input_scenario = this.transform.Find("InputField").gameObject.GetComponent<InputField>();
        input_girllove = this.transform.Find("InputField_GirlLove").gameObject.GetComponent<InputField>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

       
    }

    // Update is called once per frame
    void Update()
    {

        StoryNumber_text.text = "StoryNumber: " + GameMgr.scenario_flag;
        StageNumber_text.text = "Stage: " + GameMgr.stage_number;

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

            //好感度バーの取得
            _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();
            _slider.value = girllove_param;

            compound_Main.check_GirlLoveEvent_flag = false;
        }
    }
}

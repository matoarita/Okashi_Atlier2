using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Panel : MonoBehaviour {

    private GameObject StoryNumber;
    private Text StoryNumber_text;

    private InputField input_scenario;
    private string input_text;
    private int scenario_num;

    private Text Counter;

    // Use this for initialization
    void Start () {

        StoryNumber = this.transform.Find("StoryNumber").gameObject;
        StoryNumber_text = StoryNumber.GetComponent<Text>();

        input_scenario = this.transform.Find("InputField").gameObject.GetComponent<InputField>();

    }

    // Update is called once per frame
    void Update()
    {

        StoryNumber_text.text = "StoryNumber: " + GameMgr.scenario_flag;

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
}

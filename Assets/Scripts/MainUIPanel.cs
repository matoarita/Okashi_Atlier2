using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

public class MainUIPanel : MonoBehaviour {

    private GameObject canvas;
    private GameObject UIOpenButton_obj;
    private GameObject GetMatStatusButton_obj;
    private GameObject TimePanel_obj;

    private SoundController sc;

    private Compound_Main compound_Main;

    private int total_obj_count;

    private Touch_Controller touch_controller;
    private CubismModel _model;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //Live2Dモデル取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();

        UIOpenButton_obj = canvas.transform.Find("MainUIOpenButton").gameObject;
        TimePanel_obj = this.transform.Find("TimePanel").gameObject;
        GetMatStatusButton_obj = this.transform.Find("GetMatStatusPanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //タッチ判定オブジェクトの取得
        touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        total_obj_count = 0;
        foreach (Transform child in this.transform)
        {
            total_obj_count++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOpenButton()
    {

        foreach (Transform child in this.transform) 
        {
            child.gameObject.SetActive(true);
            //child.GetComponent<CanvasGroup>().alpha = 1;
        }
        UIOpenButton_obj.SetActive(false);
        

        if (GameMgr.TimeUSE_FLAG == false)
        {
            TimePanel_obj.SetActive(false);
        }

        //材料採取系ボタンもオフにする。
        GetMatStatusButton_obj.SetActive(false);

        compound_Main.CheckButtonFlag();
        compound_Main.QuestClearCheck();

    }

    public void OnCloseButton()
    {
        foreach (Transform child in this.transform) 
        {
            child.gameObject.SetActive(false);
            //child.GetComponent<CanvasGroup>().alpha = 0;
        }
        UIOpenButton_obj.SetActive(true);
    }

    void OsawariON()
    {
        _model.GetComponent<GazeController>().enabled = true;
        touch_controller.Touch_OnAllON();
    }

    void OsawariOFF()
    {
        _model.GetComponent<GazeController>().enabled = false;
        touch_controller.Touch_OnAllOFF();
    }
}

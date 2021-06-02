using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

public class MainUIPanel : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject canvas;
    private GameObject UIOpenButton_obj;
    private GameObject GetMatStatusButton_obj;
    private GameObject TimePanel_obj;

    private SoundController sc;

    private Compound_Main compound_Main;

    private int total_obj_count;

    private Touch_Controller touch_controller;
    private CubismModel _model;

    private GameObject text_area_Main;
    private Text _textmain;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        //Live2Dモデル取得
        _model = GameObject.FindWithTag("CharacterLive2D").FindCubismModel();

        UIOpenButton_obj = canvas.transform.Find("MainUIOpenButton").gameObject;
        TimePanel_obj = this.transform.Find("Comp/TimePanel").gameObject;
        GetMatStatusButton_obj = this.transform.Find("Comp/GetMatStatusPanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //タッチ判定オブジェクトの取得
        touch_controller = GameObject.FindWithTag("Touch_Controller").GetComponent<Touch_Controller>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.GetComponentInChildren<Text>();

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

        this.transform.Find("Comp/").gameObject.SetActive(true);

        UIOpenButton_obj.SetActive(false);
        text_area_Main.SetActive(true);


        if (GameMgr.TimeUSE_FLAG == false)
        {
            TimePanel_obj.SetActive(false);
        }

        //材料採取系ボタンもオフにする。
        GetMatStatusButton_obj.SetActive(false);

        compound_Main.CheckButtonFlag();
        compound_Main.QuestClearCheck();

        //カメラ左へ。キャラが左へいく。
        trans = 10; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);
    }

    public void OnCloseButton()
    {
        this.transform.Find("Comp/").gameObject.SetActive(false);

        UIOpenButton_obj.SetActive(true);
        text_area_Main.SetActive(false);

        //カメラ正面に戻る。
        trans = 11; //transが1を超えたときに、ズームするように設定されている。

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);
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

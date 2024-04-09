using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using UnityEngine.SceneManagement;


public class Touch_Controll_Item : MonoBehaviour
{
    private GameObject canvas;

    private SoundController sc;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Girl1_status girl1_status;
    private GirlEat_Judge girleat_judge;

    private TimeController time_controller;

    private bool ALL_touch_flag;

    private float timeOut;

    private bool isHimmeli;
    private Animator himmeli_animator;

    private bool touch_interval_flag;
    private float time_inter_default;

    private int i, _rnd;

    private List<GameObject> touch_obj = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();     

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        girleat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();


        ALL_touch_flag = true;

        timeOut = 3.0f;
        time_inter_default = 1.2f;

        isHimmeli = false;

        touch_interval_flag = false;

        //Touchエリアの取得
        touch_obj.Add(this.transform.Find("FlowerPot/TouchFlower").gameObject);

    }

    private void Update()
    {
        if(touch_interval_flag)
        {
            timeOut -= Time.deltaTime;

            if (timeOut <= 0.0f)
            {
                touch_interval_flag = false;
            }
        }

        //背景おぶじぇくとの触り判定をオフにする。　どこのスクリプトからでも呼べる。
        if (GameMgr.BGTouch_ALLOFF)
        {
            GameMgr.CharacterTouch_ALLOFF = false;
            Touch_OnAllOFF();
        }

        if (GameMgr.BGTouch_ALLON)
        {
            GameMgr.CharacterTouch_ALLON = false;
            Touch_OnAllON();
        }
    }

    //
    //その他系
    //
    public void OnTouchBell()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Bell");

            sc.PlaySe(16);
        }

    }

    public void OnTouchFlower()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Flower");

            girl1_status.TouchFlower();
        }

    }

    public void OnTouchWindow()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Window");

            //音を鳴らす。被り無し
            sc.PlaySe(40);
        }

    }

    //
    //飾りもの類
    //
    public void OnTouchHimmeli()
    {
        if (ALL_touch_flag)
        {
            Debug.Log("Touch_Himmeli");

            himmeli_animator = this.transform.Find("himmeliObj/himmeli_live2d").GetComponent<Animator>();
            himmeli_animator.Play("himmeli_Touch", 0, 0); //第２引数は、レイヤーの番号、第３が再生時間で、0を指定している。
                                                          //isHimmeli = true;

            //時間の項目リセット
            TimeReset();
        }
    }

    void TimeReset()
    {
        //時間の項目リセット
        time_controller.ResetTimeFlag();
        
    }

    //全てのオブジェクトのタッチをオフにする。
    void Touch_OnAllOFF()
    {
        i = 0;
        while (i < touch_obj.Count)
        {
            touch_obj[i].SetActive(false);
            i++;
        }
    }

    //全てのオブジェクトのタッチをオフにする。
    void Touch_OnAllON()
    {
        i = 0;
        while (i < touch_obj.Count)
        {
            touch_obj[i].SetActive(true);
            i++;
        }
    }
}
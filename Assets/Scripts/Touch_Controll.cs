﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using UnityEngine.SceneManagement;


public class Touch_Controll : MonoBehaviour
{
    private GameObject canvas;

    private SoundController sc;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private Girl1_status girl1_status;
    private GirlEat_Judge girleat_judge;

    private TimeController time_controller;

    private bool ALL_touch_flag;

    private int draghair_count;
    private int dragtwintail_count;

    //Live2Dモデルの取得    
    private GameObject _model_root_obj;
    private GameObject _model;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    private float timeOut;
    private bool mouseLclick_off;

    private bool isHimmeli;
    private Animator himmeli_animator;

    private bool touch_interval_flag;
    private float time_inter_default;

    private int _rnd;
    private bool nohearteffect;

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

        nohearteffect = false;

        //時間管理オブジェクトの取得
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                time_controller = canvas.transform.Find("MainUIPanel/Comp/TimePanel").GetComponent<TimeController>();
                break;

            case "001_Title":

                nohearteffect = true;
                break;

            case "999_Gameover":

                nohearteffect = true;
                break;
        }

        //Live2Dモデルの取得
        _model_root_obj = GameObject.FindWithTag("CharacterRoot").gameObject;
        _model = _model_root_obj.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
        live2d_animator = _model.GetComponent<Animator>();

        ALL_touch_flag = true;

        draghair_count = 0;
        dragtwintail_count = 0;
        timeOut = 3.0f;
        time_inter_default = 1.2f;

        mouseLclick_off = false;
        isHimmeli = false;

        touch_interval_flag = false;        

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
    }
 
    //
    /* タップ＋ドラッグにも対応してるモーション */
    //

    //頭を触る。一回ならびっくりモーション。ドラッグすると、さわさわ触る反応。
    public void OnTouchHair()
    {        

        if (ALL_touch_flag)
        {
            sc.PlaySe(11); //触ったときの音
            //Debug.Log("Touch_Hair");

            if (!touch_interval_flag)
            {
                //Debug.Log("Touch_Hair");            
                
                if (!girl1_status.Girl1_touchhair_start)
                {
                    girl1_status.Touchhair_Start();
                    girl1_status.TouchSisterHair();
                    girl1_status.touchGirl_status = 1;
                }
                else
                {
                    girl1_status.TouchSisterHair();
                }

                //頭以外のタッチステータスをリセット
                girl1_status.Girl1_touchtwintail_start = false;
                girl1_status.Girl1_touchchest_start = false;

                //一回触ったら連続で触れないように、少し時間をおく。
                touch_interval_flag = true;
                timeOut = time_inter_default;

                //時間の項目リセット
                TimeReset();
            }
        }
    }

    public void DragTouchHair()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("Drag_Hair: " + draghair_count);

            //if (girl1_status.Girl1_touchhair_start)
            //{
                draghair_count++;

                if (draghair_count >= 30)
                {
                    draghair_count = 0;
                    girl1_status.TouchSisterHair();
                }

            //時間の項目リセット
            TimeReset();
            //}
        }
    }

    public void EndDragTouchHair()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("EndDrag_Hair");           

            if (!nohearteffect) //メインシーンのみ
            {
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Compound":

                        //調合シーンメインオブジェクトの取得
                        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                        if (girl1_status.Girl1_touchhair_status >= 12) //触りすぎると、少し好感度が下がる。
                        {
                            girleat_judge.UpDegHeart(-1, true); //マイナスのときのみ、こちらで処理。ゲージにも反映される。
                            compound_Main.GirlExpressionKoushin(-5);
                        }

                        if (girl1_status.Girl1_touchhair_status >= 5 && girl1_status.Girl1_touchhair_status <= 9)
                        {
                            _rnd = Random.Range(0, 3);
                            girleat_judge.loveGetPlusAnimeON(1 + _rnd, false); //1~3　ちょっとハートあがる。
                            compound_Main.GirlExpressionKoushin(20);
                            girl1_status.DefFaceChange();
                        }
                        break;
                }
                
            }

            draghair_count = 0;
            girl1_status.Girl1_touchhair_start = false;
            EndTouchMethod();
            
        }
    }

    public void EndTouchHair()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("EndTouch_Hair");
            draghair_count = 0;
            girl1_status.Girl1_touchhair_start = false;
            EndTouchMethod();
        }
    }

    void EndTouchMethod()
    {        
        girl1_status.Girl1_touch_end = true;
        girl1_status.timeOut3 = 5.0f;        
        //girl1_status.touchanim_start = false;
    }



    //ツインテールを触る。一回ならびっくりモーション。ドラッグすると、さわさわ触る反応。
    public void OnTouchTwinTail()
    {
        if (ALL_touch_flag)
        {
            sc.PlaySe(11); //触ったときの音

            if (!touch_interval_flag)
            {
                //Debug.Log("Touch_LongHair");
                

                if (!girl1_status.Girl1_touchtwintail_start)
                {
                    girl1_status.Touchtwintail_Start();
                    girl1_status.TouchSisterTwinTail();
                    girl1_status.touchGirl_status = 3;
                }
                else
                {
                    girl1_status.TouchSisterTwinTail();
                }

                //ツインテール以外のタッチステータスをリセット
                girl1_status.Girl1_touchhair_start = false;
                girl1_status.Girl1_touchchest_start = false;
                girl1_status.Girl1_touch_end = false;

                //一回触ったら連続で触れないように、少し時間をおく。
                touch_interval_flag = true;
                timeOut = time_inter_default;

                //時間の項目リセット
                TimeReset();
            }
        }

    }

    public void DragTouchTwintail()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("Drag_Twintail: " + dragtwintail_count);

            if (girl1_status.Girl1_touchtwintail_start)
            {
                dragtwintail_count++;

                if (dragtwintail_count >= 150)
                {
                    dragtwintail_count = 0;
                    girl1_status.TouchSisterTwinTail(); ;
                }

                //時間の項目リセット
                TimeReset();
            }
        }
    }

    public void EndDragTouchTwintail()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("EndDrag_Hair");           

            dragtwintail_count = 0;
            EndTouchMethod();
            girl1_status.Girl1_touchtwintail_start = false;

        }
    }

    public void EndTouchTwintail()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("EndTouch_Hair");  
            dragtwintail_count = 0;
            EndTouchMethod();
            girl1_status.Girl1_touchtwintail_start = false;
        }
    }




    //
    /* タップのみに反応するモーション（ドラッグ未対応） */
    //

    //リボンを触る
    public void OnTouchRibbon()
    {
        if (ALL_touch_flag)
        {
            sc.PlaySe(11); //触ったときの音

            if (!touch_interval_flag)
            {
                //Debug.Log("Touch_Ribbon");

                if (!girl1_status.Girl1_touchribbon_start)
                {
                    girl1_status.TouchRibbon_Start();
                    girl1_status.TouchSisterRibbon();
                    girl1_status.touchGirl_status = 2;
                }
                else
                {
                    girl1_status.TouchSisterRibbon();
                }

                //リボン以外のタッチステータスをリセット
                girl1_status.Girl1_touchhair_start = false;
                girl1_status.Girl1_touchtwintail_start = false;
                girl1_status.Girl1_touchchest_start = false;

                //一回触ったら連続で触れないように、少し時間をおく。
                touch_interval_flag = true;
                timeOut = time_inter_default;

                //時間の項目リセット
                TimeReset();
            }
        }
    }

    //口を触る
    public void OnTouchFace()
    {
        if (ALL_touch_flag)
        {
            //sc.PlaySe(11); //触ったときの音

            if (!touch_interval_flag)
            {
                //Debug.Log("Touch_Face");
                
                girl1_status.TouchSisterFace();
                girl1_status.touchGirl_status = 0;

                //口以外のタッチステータスをリセット
                girl1_status.Girl1_touchhair_start = false;
                girl1_status.Girl1_touchtwintail_start = false;
                girl1_status.Girl1_touchchest_start = false;

                //一回触ったら連続で触れないように、少し時間をおく。
                //touch_interval_flag = true;
                //timeOut = time_inter_default;

                //時間の項目リセット
                TimeReset();
            }
        }

    }

    //手を触る
    public void OnTouchHand()
    {
        if (ALL_touch_flag)
        {
            sc.PlaySe(11); //触ったときの音

            if (!touch_interval_flag)
            {
                //Debug.Log("Touch_Hand");

                if (!girl1_status.Girl1_touchhand_start)
                {
                    girl1_status.TouchHand_Start();
                    girl1_status.TouchSisterHand();
                    girl1_status.touchGirl_status = 4;
                }
                else
                {
                    girl1_status.TouchSisterHand();
                }

                //手以外のタッチステータスをリセット
                girl1_status.Girl1_touchhair_start = false;
                girl1_status.Girl1_touchtwintail_start = false;
                girl1_status.Girl1_touchchest_start = false;
                girl1_status.Girl1_touch_end = false;

                //一回触ったら連続で触れないように、少し時間をおく。
                touch_interval_flag = true;
                timeOut = time_inter_default;

                //時間の項目リセット
                TimeReset();
            }
        }
    }

    public void EndTouchHand()
    {
        if (ALL_touch_flag)
        {
            //EndTouchMethod();
        }
    }

    //胸を触る
    public void OnTouchChest()
    {
        if (ALL_touch_flag)
        {
            sc.PlaySe(11); //触ったときの音

            if (!touch_interval_flag)
            {
                //Debug.Log("Touch_Chest");
                
                if (!girl1_status.Girl1_touchchest_start)
                {
                    girl1_status.TouchChest_Start();
                    girl1_status.TouchSisterChest();
                    girl1_status.touchGirl_status = 5;
                }
                else
                {
                    girl1_status.TouchSisterChest();
                }

                //胸以外のタッチステータスをリセット
                girl1_status.Girl1_touchhair_start = false;
                girl1_status.Girl1_touchtwintail_start = false;
                girl1_status.Girl1_touch_end = false;

                //一回触ったら連続で触れないように、少し時間をおく。
                touch_interval_flag = true;
                timeOut = time_inter_default;

                //時間の項目リセット
                TimeReset();
            }
        }
    }

    public void EndToucheChest()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Chest End");
            girl1_status.Girl1_touchchest_start = false;
            //EndTouchMethod();
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

            himmeli_animator = this.transform.Find("himmeli_live2d").GetComponent<Animator>();
            himmeli_animator.Play("himmeli_Touch", 0, 0); //第２引数は、レイヤーの番号、第３が再生時間で、0を指定している。
                                                          //isHimmeli = true;

            //時間の項目リセット
            TimeReset();
        }
    }

    void TimeReset()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //時間の項目リセット
                time_controller.ResetTimeFlag();
                break;
        }
        
    }


    //タッチを一時的にオフ。全て。
    void TouchOff()
    {
        ALL_touch_flag = false;
    }

    //タッチをオン。全て。
    void TouchOn()
    {
        ALL_touch_flag = true;
    }

}
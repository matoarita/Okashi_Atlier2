using UnityEngine;
using System.Collections;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;


public class Touch_Controll : MonoBehaviour
{

    public Vector3 force = new Vector3(0, 10, 0);
    public ForceMode forceMode = ForceMode.VelocityChange;

    private SoundController sc;

    private Girl1_status girl1_status;
    private GirlEat_Judge girleat_judge;

    private bool ALL_touch_flag;

    private int draghair_count;

    private GameObject _model;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    private float timeOut;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
        audioSource = GetComponent<AudioSource>();        

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        girleat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        //Live2Dモデルの取得
        _model = GameObject.FindWithTag("CharacterLive2D").gameObject;
        live2d_animator = _model.GetComponent<Animator>();

        ALL_touch_flag = true;

        draghair_count = 0;
        timeOut = 2.0f;
    }

    private void Update()
    {

    }

    //口を触る
    public void OnTouchFace()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Face");

            girl1_status.TouchSisterFace();
            girl1_status.touchGirl_status = 0;

            //口以外のタッチステータスをリセット
            girl1_status.Girl1_touchhair_start = false;
            girl1_status.Girl1_touchtwintail_start = false;
            //sc.PlaySe(2);
        }

    }

    //頭を触る
    public void OnTouchHair()
    {
        if (ALL_touch_flag)
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
            //sc.PlaySe(0);
        }
    }

    public void DragTouchHair()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("Drag_Hair: " + draghair_count);

            if (girl1_status.Girl1_touchhair_start)
            {
                draghair_count++;

                if (draghair_count >= 30)
                {
                    draghair_count = 0;
                    girl1_status.TouchSisterHair();
                }
            }
        }
    }

    public void EndDragTouchHair()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("EndDrag_Hair");           

            if (girl1_status.Girl1_touchhair_status >= 12) //触りすぎると、少し好感度が下がる。
            {
                girleat_judge.DegHeart(-2); //マイナスのときのみ、こちらで処理。ゲージにも反映される。
            }

            draghair_count = 0;
            
        }
    }

    //リボンを触る
    public void OnTouchRibbon()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Ribbon");

            girl1_status.TouchSisterRibbon();
            girl1_status.touchGirl_status = 2;

            //リボン以外のタッチステータスをリセット
            girl1_status.Girl1_touchhair_start = false;
            girl1_status.Girl1_touchtwintail_start = false;
            //sc.PlaySe(0);
        }

    }

    //ツインテールを触る
    public void OnTouchTwinTail()
    {

        if (ALL_touch_flag)
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
            //sc.PlaySe(0);
        }

    }

    //手を触る
    public void OnTouchHand()
    {
        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Hand");

            girl1_status.TouchSisterHand();
            girl1_status.touchGirl_status = 4;

            //手以外のタッチステータスをリセット
            girl1_status.Girl1_touchhair_start = false;
            girl1_status.Girl1_touchtwintail_start = false;
            //sc.PlaySe(0);
        }
    }

    //胸を触る
    public void OnTouchChest()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Chest");

            girl1_status.TouchSisterChest();
            girl1_status.touchGirl_status = 5;

            //胸以外のタッチステータスをリセット
            girl1_status.Girl1_touchhair_start = false;
            girl1_status.Girl1_touchtwintail_start = false;
            //sc.PlaySe(0);
        }

    }

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
            //sc.PlaySe(2);
        }

    }

    public void OnTouchWindow()
    {

        if (ALL_touch_flag)
        {
            //Debug.Log("Touch_Window");

            //音を鳴らす。被り無し
            //audioSource.clip = sound1;
            //audioSource.PlayOneShot(sound1);
            sc.PlaySe(40);
        }

    }

    //タッチを一時的にオフ。全て。
    public void TouchOff()
    {
        ALL_touch_flag = false;
    }

    //タッチをオン。全て。
    public void TouchOn()
    {
        ALL_touch_flag = true;
    }

   
}
using UnityEngine;
using System.Collections;


public class Touch_Controll : MonoBehaviour
{

    public Vector3 force = new Vector3(0, 10, 0);
    public ForceMode forceMode = ForceMode.VelocityChange;

    private SoundController sc;

    private Girl1_status girl1_status;
    private GirlEat_Judge girleat_judge;

    private bool touch_flag;

    private int draghair_count;

    private GameObject _model;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    private bool isRunning;
    private bool touchhair_start;

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

        touch_flag = true;
        isRunning = false;
        touchhair_start = false;

        draghair_count = 0;
        timeOut = 2.0f;
    }

    private void Update()
    {
        //頭をなでなでしている途中の処理
        if (touchhair_start)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                timeOut = 2.0f;
            }

            if (isRunning)
            {
                timeOut -= Time.deltaTime;
                if(timeOut <= 0)
                {
                    ResetGazeControl();
                }
            }           
        }
    }

    public void OnTouchFace()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Face");

            girl1_status.TouchSisterFace();
            girl1_status.touch_status = 2;
            //sc.PlaySe(2);
        }
    }
   

    public void OnTouchHair()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Hair");            
            touchhair_start = true;
            if (isRunning)
            {
                timeOut = 2.0f;
            }

            if (!girl1_status.Girl1_touchhair_start)
            {               
                girl1_status.Touchhair_Start();
                girl1_status.TouchSisterHair();
                girl1_status.touch_status = 1;
            }
            else
            {
                girl1_status.TouchSisterHair();
            }
            //sc.PlaySe(0);
        }
    }

    public void DragTouchHair()
    {
        if (touch_flag)
        {
            //Debug.Log("Drag_Hair: " + draghair_count);
            touchhair_start = true;
            if (isRunning)
            {
                timeOut = 2.0f;
            }

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
        if (touch_flag)
        {
            //Debug.Log("EndDrag_Hair");           

            if (girl1_status.Girl1_touchhair_status >= 12) //触りすぎると、少し好感度が下がる。
            {
                girleat_judge.DegHeart(-2); //マイナスのときのみ、こちらで処理。ゲージにも反映される。
            }
            //girl1_status.Girl1_touchhair_start = false;
            draghair_count = 0;
                        
            if (isRunning)
            {
                timeOut = 2.0f;
            }
            else
            {
                isRunning = true;

                //Debug.Log("ResetGaze ON");

                timeOut = 2.0f;
            }
        }
    }

    public void OnTouchRibbon()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Ribbon");

            girl1_status.TouchSisterRibbon();
            girl1_status.touch_status = 3;
            //sc.PlaySe(0);
        }
    }

    public void OnTouchTwinTail()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_LongHair");

            girl1_status.TouchSisterTwinTail();
            girl1_status.touch_status = 4;
            //sc.PlaySe(0);
        }
    }

    public void OnTouchChest()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Chest");

            girl1_status.TouchSisterChest();
            girl1_status.touch_status = 5;
            //sc.PlaySe(0);
        }
    }

    public void OnTouchBell()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Bell");

            sc.PlaySe(16);
        }
    }

    public void OnTouchFlower()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Flower");

            girl1_status.TouchFlower();
            girl1_status.touch_status = 6;
            //sc.PlaySe(2);
        }
    }

    public void OnTouchWindow()
    {
        if (touch_flag)
        {
            //Debug.Log("Touch_Window");

            //音を鳴らす。被り無し
            //audioSource.clip = sound1;
            //audioSource.PlayOneShot(sound1);
            sc.PlaySe(40);
        }
    }

    //タッチを一時的にオフ
    public void TouchOff()
    {
        touch_flag = false;
    }

    //タッチをオン
    public void TouchOn()
    {
        touch_flag = true;
    }

    void ResetGazeControl()
    {

        _model.GetComponent<GazeController>().enabled = false;

        trans_motion = 30;
        live2d_animator.SetInteger("trans_motion", trans_motion);

        isRunning = false;
        touchhair_start = false;
    }
}
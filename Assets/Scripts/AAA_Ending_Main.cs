using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AAA_Ending_Main : MonoBehaviour {

    private GameObject canvas;

    private Debug_Panel_Init debug_panel_init;

    private Girl1_status girl1_status;

    private GameObject BGM;
    private AudioSource bgm_source;

    private GameObject movieimage_obj;

    private float timeOut;
    private int TotalcountSec;
    private float alpha;

    private bool ed_end_flag;
    private bool OnSkip;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Ending_lodingOK");

        GameMgr.scenario_flag = 1000;
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive);

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //ムービーの取得
        movieimage_obj = canvas.transform.Find("MovieImage").gameObject;

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        BGM = GameObject.FindWithTag("BGM");
        bgm_source = BGM.GetComponent<AudioSource>();

        timeOut = 1.0f;
        TotalcountSec = 1;

        GameMgr.stage_number = 100;

        ed_end_flag = false;

        alpha = 1.0f;
        OnSkip = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(timeOut <= 0.0)
        {
            timeOut = 1.0f;
            Debug.Log("再生時間カウント: " + TotalcountSec);
            //Debug.Log("bgm_source.time: " + (int)bgm_source.time);
            TotalcountSec++;

            /*if (Mathf.Abs((int)bgm_source.time - TotalcountSec) >= 1)
            {
                TotalcountSec = (int)bgm_source.time;
            }*/

        }

        timeOut -= Time.deltaTime;

        if (TotalcountSec >= 160)
        {
            if (!ed_end_flag)
            {
                ed_end_flag = true;
                StartCoroutine("WaitNextResult");
            }
            
        }

        if(OnSkip)
        {
            alpha -= 0.01f;
            movieimage_obj.GetComponent<RawImage>().color = new Color(1, 1, 1, alpha);
            if (alpha <= 0)
            {
                OnSkip = false;
            }
        }
    }

    IEnumerator WaitNextResult()
    {
        yield return new WaitForSeconds(2.0f);

        if (GameMgr.RESULTPANEL_ON)
        {
            FadeManager.Instance.LoadScene("110_TotalResult", 0.3f); //プレイヤーの腕前ランク総評
        }
        else
        {
            FadeManager.Instance.LoadScene("120_AutoSave", 0.3f);
        }
    }

    public void OnSkipButton()
    {
        OnSkip = true;       
        BGM.GetComponent<BGM>().FadeOutBGM();
        StartCoroutine("WaitNextResult");
    }

}

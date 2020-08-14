using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AAA_Ending_Main : MonoBehaviour {

    private Debug_Panel_Init debug_panel_init;

    private Girl1_status girl1_status;

    private GameObject BGM;
    private AudioSource bgm_source;

    private float timeOut;
    private int TotalcountSec;

    private bool ed_end_flag;

    // Use this for initialization
    void Start()
    {

        Debug.Log("Ending_lodingOK");

        GameMgr.scenario_flag = 1000;
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive);

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        BGM = GameObject.FindWithTag("BGM");
        bgm_source = BGM.GetComponent<AudioSource>();

        timeOut = 1.0f;
        TotalcountSec = 1;

        girl1_status.girl1_Love_exp = 0;
        GameMgr.stage_number = 100;

        ed_end_flag = false;
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
    }

    IEnumerator WaitNextResult()
    {
        yield return new WaitForSeconds(2.0f);

        FadeManager.Instance.LoadScene("110_TotalResult", 0.3f);
    }
}

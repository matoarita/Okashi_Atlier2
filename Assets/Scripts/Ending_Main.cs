using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_Main : MonoBehaviour {

    private Debug_Panel_Init debug_panel_init;

    private Girl1_status girl1_status;

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

        girl1_status.girl1_Love_exp = 0;
        GameMgr.stage_number = 100;
    }

    // Update is called once per frame
    void Update()
    {

        /*if (GameMgr.scenario_flag == 309)
        {
            GameMgr.scenario_flag = 310; //シーン読み込み処理中。このスクリプトで、アップデートを更新しないようにしている。

            FadeManager.Instance.LoadScene("Compound", 0.3f);
        }*/
    }
}

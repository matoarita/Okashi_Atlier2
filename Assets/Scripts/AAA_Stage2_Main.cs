using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AAA_Stage2_Main : MonoBehaviour
{
    private Debug_Panel_Init debug_panel_init;

    private Girl1_status girl1_status;

    // Use this for initialization
    void Start()
    {
        
        Debug.Log("Stage2_lodingOK");

        GameMgr.scenario_flag = 2000;
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive);

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        girl1_status.girl1_Love_exp = 0;
        girl1_status.girl1_Love_lv = 1;
        PlayerStatus.player_day = GameMgr.stage2_start_day;
        GameMgr.stage_number = 2;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameMgr.scenario_flag == 2009) //1話の最初の調合パートに入るので、調合パートの玄関となるシーンへ遷移する
        {
            GameMgr.scenario_flag = 2010; //シーン読み込み処理中。このスクリプトで、アップデートを更新しないようにしている。

            FadeManager.Instance.LoadScene("Compound", 0.3f);
        }
    }
}

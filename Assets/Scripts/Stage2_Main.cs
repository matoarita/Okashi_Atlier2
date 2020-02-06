using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2_Main : MonoBehaviour
{
    private Debug_Panel_Init debug_panel_init;

    // Use this for initialization
    void Start()
    {
        
        Debug.Log("Stage2_lodingOK");

        GameMgr.scenario_flag = 200;
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive);
        

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        GameMgr.stage_number = 2;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameMgr.scenario_flag == 209) //1話の最初の調合パートに入るので、調合パートの玄関となるシーンへ遷移する
        {
            GameMgr.scenario_flag = 210; //シーン読み込み処理中。このスクリプトで、アップデートを更新しないようにしている。

            FadeManager.Instance.LoadScene("Hiroba", 0.3f);
            //SceneManager.LoadScene("Main");
        }
    }
}

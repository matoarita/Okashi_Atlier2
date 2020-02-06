using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prologue_Main : MonoBehaviour {

    private Debug_Panel_Init debug_panel_init;

    // Use this for initialization
    void Start () {

        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストウィンドウを読み込み。シーンにシーンを加算する形。

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        GameMgr.stage_number = 1;

        
    }
	
	// Update is called once per frame
	void Update () {

        if ( GameMgr.scenario_flag == 100 )
        {
            //GameMgr.scenario_flag = 101; //シーン読み込み処理中。このスクリプトで、アップデートを更新しないようにしている。(!FadeManager.Instance.isFading)使うときは、アップデートを更新してないと、読み込まれない。

            GameMgr.scenario_flag = 110;
            FadeManager.Instance.LoadScene("Compound", 0.3f);
        }

    }
}

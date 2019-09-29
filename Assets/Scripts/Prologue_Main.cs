using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prologue_Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストウィンドウを読み込み。シーンにシーンを加算する形。
    }
	
	// Update is called once per frame
	void Update () {

        if ( GameMgr.scenario_flag == 100 )
        {
            GameMgr.scenario_flag = 101; //シーン読み込み処理中。このスクリプトで、アップデートを更新しないようにしている。(!FadeManager.Instance.isFading)使うときは、アップデートを更新してないと、読み込まれない。

            FadeManager.Instance.LoadScene("001_Chapter1", 0.3f);
        }

    }
}

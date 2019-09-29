using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter1_Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //if (!FadeManager.Instance.isFading) {
            Debug.Log("chapter1_lodingOK");
            GameMgr.scenario_flag = 100;
            SceneManager.LoadScene("Utage", LoadSceneMode.Additive);
        //}
    }
	
	// Update is called once per frame
	void Update () {

        if (GameMgr.scenario_flag == 109) //1話の最初の調合パートに入るので、調合パートの玄関となるシーンへ遷移する
        {
            GameMgr.scenario_flag = 110; //シーン読み込み処理中。このスクリプトで、アップデートを更新しないようにしている。

            FadeManager.Instance.LoadScene("Hiroba", 0.3f);
            //SceneManager.LoadScene("Main");
        }
    }
}

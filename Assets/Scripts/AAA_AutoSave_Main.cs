using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AAA_AutoSave_Main : MonoBehaviour {

    private SaveController save_controller;

    // Use this for initialization
    void Start () {

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //エンディング回数を+1       
        GameMgr.ending_count++;
        Debug.Log("エンディング回数: " + GameMgr.ending_count);

        //システムデータのセーブ
        save_controller.SystemsaveCheck();

        StartCoroutine("WaitResetTitle");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitResetTitle()
    {
        yield return new WaitForSeconds(3.0f);

        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
}

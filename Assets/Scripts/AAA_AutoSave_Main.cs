using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AAA_AutoSave_Main : MonoBehaviour {

    private SaveController save_controller;

    private GameObject Msg_window;
    private Text _text;

    // Use this for initialization
    void Start () {

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        Msg_window = GameObject.FindWithTag("Message_Window");
        _text = Msg_window.transform.Find("Text").GetComponent<Text>();
        Msg_window.SetActive(false);

        if (GameMgr.Story_Mode == 1)
        {
            //音楽も解禁
            GameMgr.SetBGMCollectionFlag("bgm21", true);
        }

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
        yield return new WaitForSeconds(1.8f);

        //3秒ほどたってから、「セーブ中」が表示
        Msg_window.SetActive(true);
        _text.text = "システムセーブ中";

        yield return new WaitForSeconds(1.0f);

        _text.text = "システムセーブ中 .";

        yield return new WaitForSeconds(1.0f);

        _text.text = "システムセーブ中 . . ";

        yield return new WaitForSeconds(1.0f);

        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemSelectPanel : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;

    // Use this for initialization
    void Start () {

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //セーブ
    public void OnSaveButton()
    {
        save_controller.OnSaveMethod();
    }

    //ロード
    public void OnLoadButton()
    {
        save_controller.OnLoadMethod();
    }

    //オプション
    public void OnOptionButton()
    {

    }

    //タイトル
    public void OnTitleButton()
    {
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
}

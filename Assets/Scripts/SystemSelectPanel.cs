using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemSelectPanel : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;

    private GameObject text_area_Main;
    private Text _textmain;

    private GameObject canvas;

    private Compound_Main compound_Main;

    private GameObject option_panel;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //調合メイン取得
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        //オプションパネルの取得
        option_panel = canvas.transform.Find("OptionPanel").gameObject;

        //windowテキストエリアの取得
        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //セーブ
    public void OnSaveButton()
    {
        save_controller.OnSaveMethod();
        _textmain.text = "セーブしました。";
        compound_Main.compound_status = 0;
        this.transform.parent.gameObject.SetActive(false);
    }

    //ロード
    public void OnLoadButton()
    {
        save_controller.OnLoadMethod();
        _textmain.text = "ロードしました。";
        compound_Main.compound_status = 0;
        this.transform.parent.gameObject.SetActive(false);
    }

    //オプション
    public void OnOptionButton()
    {
        option_panel.SetActive(true);
        compound_Main.compound_select = 205;
    }

    //タイトル
    public void OnTitleButton()
    {
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
}

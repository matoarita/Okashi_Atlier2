using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_Main : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;

    private Debug_Panel_Init debug_panel_init;

    // Use this for initialization
    void Start () {

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartButton()
    {
        save_controller.ResetAllParam();
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("010_Prologue", 0.3f);
    }

    public void OnLoadButton()
    {
        save_controller.ResetAllParam();
        GameMgr.GameLoadOn = true;
        save_controller.OnLoadMethod();
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("Compound", 0.3f);
    }

    public void OnGalleryButton()
    {

    }

    public void OnOptionButton()
    {

    }
}

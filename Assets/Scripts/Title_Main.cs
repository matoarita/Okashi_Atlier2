﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title_Main : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;

    private Debug_Panel_Init debug_panel_init;

    private GameObject option_panel_obj;

    private GameObject canvas;

    private GameObject galleryButton_obj;

    // Use this for initialization
    void Start () {

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        option_panel_obj = canvas.transform.Find("OptionPanel").gameObject;

        galleryButton_obj = canvas.transform.Find("TitleMenu/Viewport/Content/GalleryButton").gameObject;

        if (GameMgr.ending_count >= 1)
        {
            galleryButton_obj.SetActive(true);
        }
        else
        {
            galleryButton_obj.SetActive(false);
        }
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
        option_panel_obj.SetActive(true);
    }

    public void OnGameEndButton()
    {
        Quit();
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }
}

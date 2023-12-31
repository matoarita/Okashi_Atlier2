﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackTown : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickToTown()
    {
        _text.text = "また来てね～";

        //StartCoroutine(CoUnload());
        BackScene();
    }

    public void OnClickToTown_notext()
    {

        //StartCoroutine(CoUnload());
        BackScene();
    }

    public void OnClickToHiroba2()
    {
        //_text.text = "また来てね～";

        //StartCoroutine(CoUnload());
        //広場シーン読み込み
        FadeManager.Instance.LoadScene("Hiroba2", 0.3f);
    }

    IEnumerator CoUnload()
    {
        //SceneAをアンロード
        var op = SceneManager.UnloadSceneAsync("Utage");
        yield return op;

        //必要に応じて不使用アセットをアンロードしてメモリを解放する
        //けっこう重い処理なので、別に管理するのも手
        yield return Resources.UnloadUnusedAssets();

        //アンロード後の処理を書く
        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Compound", 0.3f);
        GameMgr.Scene_back_home = true;      
    }

    void BackScene()
    {
        GameMgr.Scene_back_home = true;

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Compound", 0.3f);        
    }
}

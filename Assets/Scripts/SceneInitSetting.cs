using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//シーンの最初にプレファブから生成するオブジェクト　全シーン共通で置いておく

public class SceneInitSetting : SingletonMonoBehaviour<SceneInitSetting>
{
    private GameObject canvas;

    private GameObject updown_counter_obj;
    private GameObject updown_counter_Prefab;

    private bool myscene_loaded;

    // Use this for initialization
    void Start() {

        // 別シーン読み込み時にOnSceneLoadedの処理を起動する
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update() {

        //別シーンから、再度読み込まれたときに、すでにお菓子を作成済みだった場合は、初期化する。
        if (myscene_loaded == true)
        {
            if (updown_counter_obj == null) //重複生成防止
            {
                Init();
            }
            myscene_loaded = false;
        }
    }

    void Init() {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //シーン最初にカウンターも生成する。
        updown_counter_Prefab = (GameObject)Resources.Load("Prefabs/updown_counter");
        updown_counter_obj = Instantiate(updown_counter_Prefab, canvas.transform);
    }

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
            //Debug.Log(scene.name + " scene loaded");
            myscene_loaded = true;
    }
}

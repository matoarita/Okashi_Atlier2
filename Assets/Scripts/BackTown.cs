using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackTown : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    private SoundController sc;
    private TimeController time_controller;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

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
        GameMgr.Scene_back_home = true;

        _text.text = "また来てね～";

        //日数の経過。場所ごとに、移動までの日数が変わる。
        time_controller.SetMinuteToHour(GameMgr.System_BackHome_Time);
        time_controller.TimeKoushin(0, false);

        //StartCoroutine(CoUnload());
        BackScene();
    }

    public void OnClickToTown_notext()
    {
        GameMgr.Scene_back_home = true;

        //日数の経過。場所ごとに、移動までの日数が変わる。
        time_controller.SetMinuteToHour(GameMgr.System_BackHome_Time);
        time_controller.TimeKoushin(0, false);

        //StartCoroutine(CoUnload());
        BackScene();
    }

    public void OnClickToTown2()
    {
        //玄関音
        sc.EnterSound_01();
        //sc.EnterSound_03();

        GameMgr.Scene_back_home = true;

        //日数の経過。場所ごとに、移動までの日数が変わる。
        time_controller.SetMinuteToHour(GameMgr.System_BackHome_Time);
        time_controller.TimeKoushin(0, false);

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Or_Compound", GameMgr.SceneFadeTime);
    }


    public void OnClickToHiroba2()
    {
        //_text.text = "また来てね～";

        //StartCoroutine(CoUnload());
        //広場シーン読み込み
        FadeManager.Instance.LoadScene("Hiroba2", GameMgr.SceneFadeTime);
    }    

    void BackScene()
    {
        GameMgr.Scene_back_home = true;

        //日数の経過。場所ごとに、移動までの日数が変わる。
        time_controller.SetMinuteToHour(GameMgr.System_BackHome_Time);
        time_controller.TimeKoushin(0, false);

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Compound", GameMgr.SceneFadeTime);        
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
        FadeManager.Instance.LoadScene("Compound", GameMgr.SceneFadeTime);
        GameMgr.Scene_back_home = true;
    }
}

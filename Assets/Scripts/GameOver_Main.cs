using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Main : MonoBehaviour {

    private GameObject canvas;
    private Text gameover_text;

    private BGM sceneBGM;

    private SaveController save_controller;

    private bool StartRead;

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 999;

        GameMgr.Scene_Name = "999_GameOver";

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        gameover_text = canvas.transform.Find("HyoujiPanel/GameOverText").GetComponent<Text>();
        gameover_text.text = "にいちゃ～ん..。" + "\n" + "エデンとれなかった..。" + "\n" + "もうママに、" + "\n" + "会えないのかなぁ～・・？";

        StartRead = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!StartRead) //シーン最初だけ読み込む
        {
            StartRead = true;
            sceneBGM.PlaySub();
            sceneBGM.NowFadeVolumeONBGM();
            sceneBGM.MuteOFFBGM();
        }
    }

    public void TitleBackButton()
    {
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }

    public void LoadButton()
    {
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        save_controller.OnLoadMethod();
    }
}

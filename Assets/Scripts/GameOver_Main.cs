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
    private SoundController sc;

    private bool StartRead;

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        GameMgr.Scene_Category_Num = 999;

        GameMgr.Scene_Name = "999_GameOver";

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        gameover_text = canvas.transform.Find("HyoujiPanel/GameOverText").GetComponent<Text>();

        switch (GameMgr.SceneSelectNum)
        {
            case 0: //エデンコンテストで負けた

                gameover_text.text = "にいちゃ～ん..。" + "\n" + "エデンとれなかった..。" + "\n" + "もうママに、" + "\n" + "会えないのかなぁ～・・？";
                break;

            case 10: //家賃が払えなかった

                gameover_text.text = "にいちゃ～ん..。" + "\n" + "サーカス楽しいけど..。" + "\n" + "はやくママに、" + "\n" + "会いたいなぁ～・・。";
                break;

            default:

                gameover_text.text = "にいちゃ～ん..。" + "\n" + "エデンとれなかった..。" + "\n" + "もうママに、" + "\n" + "会えないのかなぁ～・・？";
                break;
        }               

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
        //sc.PlaySe(28);
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }

    public void LoadButton()
    {
        sc.PlaySe(28);
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        save_controller.OnLoadMethod(GameMgr.System_save_nowslot);
    }
}

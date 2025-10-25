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

        //コンテストでゲームオーバーになるので、コンテストのデータを一時保存する
        GameMgr.GmO_contest_TotalScore = GameMgr.contest_TotalScore;
        GameMgr.GmO_contest_okashiName = GameMgr.contest_okashiName;
        GameMgr.GmO_contest_okashiNameHyouji = GameMgr.contest_okashiNameHyouji;
        GameMgr.GmO_contest_okashiSlotName = GameMgr.contest_okashiSlotName;
        GameMgr.GmO_contest_okashiID = GameMgr.contest_okashiID;
        GameMgr.GmO_contest_lasthint_text = GameMgr.contest_lasthint_text; //
        GameMgr.GmO_contest_last_Disqualification = GameMgr.contest_last_Disqualification; //課題のおかしでないため失格した場合　保存用
        GameMgr.GmO_contest_shokukan_param = GameMgr.contest_shokukan_param; //
        GameMgr.GmO_contest_shokukan_mes = GameMgr.contest_shokukan_mes; //
        GameMgr.GmO_contest_sweat_param = GameMgr.contest_sweat_param; //
        GameMgr.GmO_contest_sour_param = GameMgr.contest_sour_param; //
        GameMgr.GmO_contest_bitter_param = GameMgr.contest_bitter_param;
        GameMgr.GameOverLoadFlag = false;
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
        GameMgr.GameOverLoadFlag = true;

        sc.PlaySe(28);
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        save_controller.OnLoadMethod(GameMgr.System_save_nowslot);
    }
}

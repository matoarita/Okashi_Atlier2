using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveloadListSelectToggle : MonoBehaviour {

    public int _toggleID; //各トグルの番号

    private GameObject canvas;

    private SaveController save_controller;
    private SoundController sc;
    private BGM sceneBGM;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト   

    private GameObject system_panel;
    private GameObject titleback_panel;
    private Text titleback_text;
    private Text yes_text;

    private GameObject saveload_panel;

    private GameObject text_area_Main;
    private Text _textmain;
    private bool _textmain_loadflag;

    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        system_panel = canvas.transform.Find("SystemPanel").gameObject;
        titleback_panel = canvas.transform.Find("SystemPanel/SaveLoadPanel/SaveBackKakunin").gameObject;
        titleback_panel.SetActive(false);
        titleback_text = titleback_panel.transform.Find("MessageWindow/Text").GetComponent<Text>();
        yes_text = titleback_panel.transform.Find("Yes_Clear/Text").GetComponent<Text>();

        _textmain_loadflag = false;
        switch (GameMgr.Scene_Category_Num)
        {
            case 10: //調合メインシーン

                //windowテキストエリアの取得
                text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
                _textmain = text_area_Main.GetComponentInChildren<Text>();
                _textmain_loadflag = true;
                break;

            case 1000: //タイトルシーン　メインウィンドウないので読まない

                break;

            default:
                
                break;
        }
        

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        saveload_panel = canvas.transform.Find("SystemPanel/SaveLoadPanel").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //新規セーブを確認する
    public void NoDataButton()
    {
        Debug.Log("セーブスロット: " + _toggleID + "番　をおした");

        switch (GameMgr.SaveLoadPanel_mode)
        {
            case 0:
                titleback_panel.SetActive(true);
                titleback_text.text = (_toggleID + 1).ToString() + "番　に" + "セーブする？";
                yes_text.text = "セーブする";

                StartCoroutine("Save_kakunin");
                break;

            case 1:

                titleback_text.text = "セーブデータがないよ～。";
                break;
        }
    }

    //すでにセーブがあるところを押した　セーブのときは、上書きするか。ロードのときは、ロードするかどうか確認
    public void SaveONButton()
    {
        Debug.Log("セーブスロット: " + _toggleID + "番　をおした");

        switch (GameMgr.SaveLoadPanel_mode)
        {
            case 0:

                titleback_panel.SetActive(true);
                titleback_text.text = (_toggleID + 1).ToString() + "番　の" + "セーブを上書きする？";
                yes_text.text = "上書きする";

                StartCoroutine("Save_kakunin");
                break;

            case 1:

                titleback_panel.SetActive(true);
                titleback_text.text = (_toggleID + 1).ToString() + "番　を" + "ロードする？";
                yes_text.text = "ロードする";

                StartCoroutine("Load_kakunin");
                break;
        }
        
    }

    IEnumerator Save_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。これでいいですか？の確認。

                //Debug.Log("ok");
                //解除
                save_controller.OnSaveMethod(_toggleID);
                _textmain.text = (_toggleID+1).ToString() + "番 に" + "セーブしました。";
                //GameMgr.compound_status = 0;
                titleback_panel.SetActive(false);

                saveload_panel.GetComponent<SaveLoadPanel>().ReDraw();

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);
                //no_button.SetActive(true);

                break;
        }
    }

    IEnumerator Load_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された。これでいいですか？の確認。

                //Debug.Log("ok");
                //解除

                //音量フェードアウト
                //sceneBGM.FadeOutBGM();
                sceneBGM.NowFadeVolumeOFFBGM(); //

                save_controller.OnLoadMethod(_toggleID); //Loadメソッド内でシーン移動している

                if (_textmain_loadflag)
                {
                    if (GameMgr.saveOK)
                    {
                        _textmain.text = "ロードしました。";
                    }
                    else
                    {
                        _textmain.text = "セーブデータがありません。";
                    }
                }

                //GameMgr.compound_status = 0;
                titleback_panel.SetActive(false);
                //system_panel.SetActive(false);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);
                //no_button.SetActive(true);

                break;
        }
    }
}

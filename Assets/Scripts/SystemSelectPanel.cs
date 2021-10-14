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

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject canvas;

    private Compound_Main compound_Main;

    private GameObject option_panel;
    private GameObject titleback_panel;
    private Text titleback_text;
    private Text yes_text;

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

        titleback_panel = canvas.transform.Find("SystemPanel/TitleBackKakunin").gameObject;
        titleback_panel.SetActive(false);
        titleback_text = titleback_panel.transform.Find("MessageWindow/Text").GetComponent<Text>();
        yes_text = titleback_panel.transform.Find("Yes_Clear/Text").GetComponent<Text>();

        //windowテキストエリアの取得
        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.GetComponentInChildren<Text>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //セーブ
    public void OnSaveButton()
    {
        titleback_panel.SetActive(true);
        titleback_text.text = "セーブするの？";
        yes_text.text = "セーブする";

        StartCoroutine("Save_kakunin");       
    }

    //ロード
    public void OnLoadButton()
    {
        titleback_panel.SetActive(true);
        titleback_text.text = "ロードするの？";
        yes_text.text = "ロードする";

        StartCoroutine("Load_kakunin");
    }

    //オプション
    public void OnOptionButton()
    {
        option_panel.SetActive(true);
        GameMgr.compound_select = 205;
    }

    //タイトル
    public void OnTitleButton()
    {
        titleback_panel.SetActive(true);
        titleback_text.text = "タイトルに戻る？" + "\n" + "（セーブしてないデータは失われます。）";
        yes_text.text = "タイトルに戻る";

        StartCoroutine("Title_kakunin");
        
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
                save_controller.OnSaveMethod();
                _textmain.text = "セーブしました。";
                GameMgr.compound_status = 0;
                titleback_panel.SetActive(false);
                this.transform.parent.gameObject.SetActive(false);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);

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

                save_controller.OnLoadMethod();                

                if (GameMgr.saveOK)
                {
                    _textmain.text = "ロードしました。";
                }
                else
                {
                    _textmain.text = "セーブデータがありません。";
                }

                GameMgr.compound_status = 0;
                titleback_panel.SetActive(false);
                this.transform.parent.gameObject.SetActive(false);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);

                break;
        }
    }

    IEnumerator Title_kakunin()
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
                FadeManager.Instance.LoadScene("001_Title", 0.3f);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);

                break;
        }
    }
}

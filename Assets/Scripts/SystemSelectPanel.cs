using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemSelectPanel : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;
    private BGM sceneBGM;

    private GameObject text_area_Main;
    private Text _textmain;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject canvas;

    private Compound_Main compound_Main;

    private GameObject saveload_panel;
    private GameObject option_panel;
    private GameObject extraoption_panel;
    private GameObject titleback_panel;
    private Text titleback_text;
    private Text yes_text;
    private GameObject loadButton_obj;

    private GameObject no_button;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Setting_init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //調合メイン取得
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        //オプションパネルの取得
        option_panel = canvas.transform.Find("OptionPanel").gameObject;

        //オプションパネルの取得
        extraoption_panel = canvas.transform.Find("OptionPanel/ExtraOptionList").gameObject;

        titleback_panel = canvas.transform.Find("SystemPanel/SystemSelectPanel/TitleBackKakunin").gameObject;
        titleback_panel.SetActive(false);
        titleback_text = titleback_panel.transform.Find("MessageWindow/Text").GetComponent<Text>();
        yes_text = titleback_panel.transform.Find("Yes_Clear/Text").GetComponent<Text>();

        //windowテキストエリアの取得
        text_area_Main = canvas.transform.Find("MessageWindowMain").gameObject;
        _textmain = text_area_Main.GetComponentInChildren<Text>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        no_button = canvas.transform.Find("SystemPanel/SystemSelectPanel/No").gameObject;
        no_button.SetActive(true);

        //セーブロードパネルの取得
        saveload_panel = canvas.transform.Find("SystemPanel/SaveLoadPanel").gameObject;
        saveload_panel.SetActive(false);

        loadButton_obj = this.transform.Find("Scroll View/Viewport/Content/LoadButton").gameObject;

        if (GameMgr.saveOK)
        {
            //ロードボタンを表示
            loadButton_obj.GetComponent<Button>().interactable = true;
            //loadButton_obj.SetActive(true);
        }
        else
        {
            loadButton_obj.GetComponent<Button>().interactable = false;
        }

        CheckButtonStatus();
    }

    private void OnEnable()
    {
        Setting_init();
        
    }

    void CheckButtonStatus()
    {
        if (GameMgr.Story_Mode == 0)
        {
            this.transform.Find("Scroll View/Viewport/Content/ExtraOptionButton").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("Scroll View/Viewport/Content/ExtraOptionButton").gameObject.SetActive(true);
        }
    }

    //セーブ用画面を開く
    public void OnSaveButton()
    {
        GameMgr.SaveLoadPanel_mode = 0;
        saveload_panel.SetActive(true);

        /*titleback_panel.SetActive(true);
        titleback_text.text = "セーブするの？";
        yes_text.text = "セーブする";
        no_button.SetActive(false);

        StartCoroutine("Save_kakunin");*/
    }

    //ロード用画面を開く
    public void OnLoadButton()
    {
        GameMgr.SaveLoadPanel_mode = 1;
        saveload_panel.SetActive(true);

        /*titleback_panel.SetActive(true);
        titleback_text.text = "ロードするの？";
        yes_text.text = "ロードする";
        no_button.SetActive(false);

        StartCoroutine("Load_kakunin");*/
    }

    //オプション
    public void OnOptionButton()
    {
        option_panel.SetActive(true);
        extraoption_panel.SetActive(false);
        GameMgr.compound_select = 205;
    }

    //エクストラオプション
    public void OnExtraOptionButton()
    {
        option_panel.SetActive(true);
        extraoption_panel.SetActive(true);
        GameMgr.compound_select = 205;
    }

    //タイトル
    public void OnTitleButton()
    {
        titleback_panel.SetActive(true);
        titleback_text.text = "タイトルに戻る？" + "\n" + "（セーブしてないデータは失われます。）";
        yes_text.text = "タイトルに戻る";
        no_button.SetActive(false);

        StartCoroutine("Title_kakunin");
        
    }

    /*
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

                //ロードボタンを表示
                loadButton_obj.GetComponent<Button>().interactable = true;

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);
                no_button.SetActive(true);

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

                save_controller.OnLoadMethod(); //Loadメソッド内でシーン移動している

                if (GameMgr.saveOK)
                {
                    _textmain.text = "ロードしました。";
                }
                else
                {
                    _textmain.text = "セーブデータがありません。";
                }
                
                //GameMgr.compound_status = 0;
                titleback_panel.SetActive(false);
                this.transform.parent.gameObject.SetActive(false);

                break;

            case false: //キャンセルが押された

                //Debug.Log("cancel");
                titleback_panel.SetActive(false);
                no_button.SetActive(true);

                break;
        }
    }*/

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
                no_button.SetActive(true);

                break;
        }
    }
}

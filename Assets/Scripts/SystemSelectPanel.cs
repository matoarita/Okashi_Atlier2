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

    private GameObject system_panel;
    private GameObject saveload_panel;
    private GameObject option_panel;
    private GameObject extraoption_panel;
    private GameObject titleback_panel;
    private Text titleback_text;
    private Text yes_text;
    private GameObject loadButton_obj;
    private GameObject quicksaveButton_obj;
    private Text qsave_slot_text;

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

        system_panel = canvas.transform.Find("SystemPanel").gameObject;

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
        quicksaveButton_obj = this.transform.Find("Scroll View/Viewport/Content/QuickSaveButton").gameObject;
        qsave_slot_text = quicksaveButton_obj.transform.Find("Slotnumtext").GetComponent<Text>();

        if (GameMgr.saveOK)
        {
            //ロードボタンを表示
            loadButton_obj.GetComponent<Button>().interactable = true;
            quicksaveButton_obj.GetComponent<Button>().interactable = true;
            //loadButton_obj.SetActive(true);

            qsave_slot_text.text = (GameMgr.System_save_nowslot + 1).ToString() + "番";
        }
        else
        {
            loadButton_obj.GetComponent<Button>().interactable = false;
            quicksaveButton_obj.GetComponent<Button>().interactable = false;
            qsave_slot_text.text = "";
        }

        CheckButtonStatus();
    }

    private void OnEnable()
    {
        Setting_init();
        
    }

    void CheckButtonStatus()
    {
        /*if (GameMgr.Story_Mode == 0)
        {
            this.transform.Find("Scroll View/Viewport/Content/ExtraOptionButton").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("Scroll View/Viewport/Content/ExtraOptionButton").gameObject.SetActive(true);
        }*/
    }

    //セーブ用画面を開く
    public void OnSaveButton()
    {
        GameMgr.SaveLoadPanel_mode = 0;
        saveload_panel.SetActive(true);
    }

    //クイックセーブする
    public void OnQuickSaveButton()
    {
        save_controller.OnSaveMethod(GameMgr.System_save_nowslot);
        _textmain.text = (GameMgr.System_save_nowslot + 1).ToString() + "番 に" + "セーブしました。";

        GameMgr.compound_status = 0;
        system_panel.SetActive(false);
    }

    //ロード用画面を開く
    public void OnLoadButton()
    {
        GameMgr.SaveLoadPanel_mode = 1;
        saveload_panel.SetActive(true);
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

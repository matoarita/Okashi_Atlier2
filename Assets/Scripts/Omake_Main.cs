using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Omake_Main : MonoBehaviour {

    private SaveController save_controller;
    private SoundController sc;

    private Debug_Panel_Init debug_panel_init;

    private Girl1_status girl1_status;

    private GameObject cg_gallerypanel_obj;
    private GameObject sp_titlepanel_obj;
    private GameObject contestclearpanel_obj;
    private GameObject OmakeEnterPanel_obj;
    private GameObject RecipiListPanel_obj;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject recipilist_onoff;
    private RecipiListController recipilistController;

    private GameObject canvas;

    private bool isLoading;
    Coroutine _waitSeconds;


    // Use this for initialization
    void Start () {

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");

        save_controller = SaveController.Instance.GetComponent<SaveController>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子   

        cg_gallerypanel_obj = canvas.transform.Find("CGGalleryPanel").gameObject;
        sp_titlepanel_obj = canvas.transform.Find("SpecialTitleListPanel").gameObject;
        contestclearpanel_obj = canvas.transform.Find("ContestClearListPanel").gameObject;
        OmakeEnterPanel_obj = canvas.transform.Find("OmakeEnterPanel").gameObject;
        RecipiListPanel_obj = canvas.transform.Find("RecipiListPanel").gameObject;

        //プレイヤー所持アイテムリストパネルの取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();

        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();
        playeritemlist_onoff.SetActive(false);

        //レシピリストパネルの取得
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().RecipiList_ScrollView_Init();
        recipilist_onoff = GameObject.FindWithTag("RecipiList_ScrollView");
        recipilistController = recipilist_onoff.GetComponent<RecipiListController>();

        //位置・大きさを初期設定
        recipilist_onoff.transform.localScale = new Vector3(0.85f, 0.85f, 1.0f);
        recipilist_onoff.transform.localPosition = new Vector3(0, 40, 0);
        recipilist_onoff.SetActive(false);

        isLoading = false;

        //システムロード
        save_controller.SystemloadCheck();

      
    }
	
	// Update is called once per frame
	void Update () {

    }
 

    public void OnCGGalleryButton()
    {
        cg_gallerypanel_obj.GetComponent<CGGalleryPanel>().PageReset();
        cg_gallerypanel_obj.SetActive(true);        
    }

    public void OnSpecialTitleButton()
    {
        sp_titlepanel_obj.SetActive(true);
    }

    public void OnContestClearListButton()
    {
        contestclearpanel_obj.SetActive(true);
    }

    public void OnRecipiListButton()
    {
        GameMgr.compound_select = 1;
        RecipiListPanel_obj.SetActive(true);
        recipilist_onoff.SetActive(true);
    }

    public void OnBackButton()
    {
        FadeManager.Instance.fadeColor = new Color(0.0f, 0.0f, 0.0f);
        FadeManager.Instance.LoadScene("001_Title", 0.3f);
    }
   
    //CGギャラリー閲覧中
    public void ReadCGGallery()
    {
        _waitSeconds = StartCoroutine(WaitSeconds()); //2秒後に宴入力可能になる。
        StartCoroutine("CGGallery_EndWait");
    }

    IEnumerator CGGallery_EndWait()
    {
        //「宴」のシナリオ終了待ち
        while (GameMgr.scenario_ON)
        {
            yield return null;
        }

        if(isLoading)
        {
            isLoading = false;
            StopCoroutine(_waitSeconds);
        }

        cg_gallerypanel_obj.SetActive(true);
        OmakeEnterPanel_obj.SetActive(true);

        cg_gallerypanel_obj.GetComponent<CGGalleryPanel>().OnInteractPanel(); //入力をON
    }

    IEnumerator WaitSeconds()
    {
        isLoading = true;
        yield return new WaitForSeconds(2.0f);

        isLoading = false;
        cg_gallerypanel_obj.SetActive(false);
        OmakeEnterPanel_obj.SetActive(false);
    }
}
